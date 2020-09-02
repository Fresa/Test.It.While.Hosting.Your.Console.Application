using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Console.Application.Consoles
{
    internal class DefaultHostController : IHostController
    {
        private readonly ConcurrentQueue<string> _readableLines = new ConcurrentQueue<string>();
        private readonly SemaphoreSlim _readLineWaiter = new SemaphoreSlim(0, 1);

        private readonly ConcurrentQueue<string> _output = new ConcurrentQueue<string>();
        private readonly object _outputLock = new object();

        public void WriteLine(string message)
        {
            lock (_outputLock)
            {
                OutputReceivedPrivate?.Invoke(this, message);
                _output.Enqueue(message);
            }
        }

        private event EventHandler<string> OutputReceivedPrivate;
        public event EventHandler<string> OutputReceived
        {
            add
            {
                lock (_outputLock)
                {
                    foreach (var message in _output)
                    {
                        value.Invoke(this, message);
                    }
                    OutputReceivedPrivate += value;
                }
            }
            remove
            {
                lock (_outputLock)
                {
                    OutputReceivedPrivate -= value;
                }
            }
        }

        public async ValueTask<string> ReadAsync(
            TimeSpan timeout = default,
            CancellationToken cancellationToken = default)
        {
            if (timeout == default)
            {
                timeout = new TimeSpan(-1);
            }

            if (await _readLineWaiter.WaitAsync(timeout, cancellationToken)
                                     .ConfigureAwait(false) == false)
            {
                throw new TimeoutException($"Waited for input for {timeout} seconds.");
            }

            if (_readableLines.TryDequeue(out var readLine))
            {
                return readLine;
            }

            throw new InvalidOperationException("Mutex out of sync");
        }

        public void Input(string line)
        {
            _readableLines.Enqueue(line);
            _readLineWaiter.Release();
        }

        private event EventHandler<int> DisconnectedPrivate;
        public event EventHandler<int> Disconnected
        {
            add
            {
                lock (_disconnectLock)
                {
                    if (_disconnected)
                    {
                        value.Invoke(this, _exitCode);
                    }
                    DisconnectedPrivate += value;
                }
            }
            remove
            {
                lock (_disconnectLock)
                {
                    DisconnectedPrivate -= value;
                }
            }
        }

        private readonly object _disconnectLock = new object();
        private bool _disconnected;
        private int _exitCode;

        public void Disconnect(int exitCode)
        {
            lock (_disconnectLock)
            {
                _disconnected = true;
                _exitCode = exitCode;
            }
            DisconnectedPrivate?.Invoke(this, exitCode);
        }

        #region ExceptionRaised
        private readonly List<(Exception, CancellationToken)> _exceptionsRaised =
            new List<(Exception, CancellationToken)>();
        private readonly object _exceptionLock = new object();

        private event HandleExceptionAsync OnExceptionPrivate = (exception, token) =>
            Task.CompletedTask;
        public event HandleExceptionAsync OnUnhandledExceptionAsync
        {
            add
            {
                lock (_exceptionLock)
                {
                    foreach (var (exception, cancellationToken) in _exceptionsRaised)
                    {
                        value.Invoke(exception, cancellationToken);
                    }
                    OnExceptionPrivate += value;
                }
            }
            remove
            {
                lock (_exceptionLock)
                {
                    OnExceptionPrivate -= value;
                }
            }
        }

        public async Task RaiseExceptionAsync(Exception exception,
            CancellationToken cancellationToken)
        {
            lock (_exceptionLock)
            {
                _exceptionsRaised.Add((exception, cancellationToken));
            }

            await OnExceptionPrivate.Invoke(exception, cancellationToken);
        }
        #endregion
    }

    public delegate Task HandleExceptionAsync(Exception exception,
        CancellationToken cancellationToken);
}