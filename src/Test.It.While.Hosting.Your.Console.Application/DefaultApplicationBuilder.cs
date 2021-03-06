﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Test.It.ApplicationBuilders;

namespace Test.It.While.Hosting.Your.Console.Application
{
    internal class DefaultApplicationBuilder : IApplicationBuilder
    {
        private readonly ConcurrentQueue<IMiddleware> _middlewares = new ConcurrentQueue<IMiddleware>();

        public Func<IDictionary<string, object>, CancellationToken, Task> Build()
        {
            if (_middlewares.Any() == false)
            {
                throw new InvalidOperationException("There are no middlewares defined in the pipeline.");
            }

            return Builder;
        }

        private async Task Builder(IDictionary<string, object> environment, CancellationToken cancellationToken)
        {
            if (_middlewares.TryDequeue(out var nextMiddleware) == false)
            {
                return;
            }

            nextMiddleware.Initialize(Builder);

            await nextMiddleware.Invoke(environment, cancellationToken);
        }

        public void Use(IMiddleware middleware)
        {
            _middlewares.Enqueue(middleware);
        }
    }
}