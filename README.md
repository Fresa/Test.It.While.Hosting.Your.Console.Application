# Test.It.While.Hosting.Your.Console.Application
Testing framework that hosts your Console Application during test execution

## Why?
This library helps you simplify writing integration tests for your Console Application. It will bootstrap, start and host your Console Application in memory while giving you handles to interact with the application making it possible to mock integration points to isolate the application during testing. This way you are no longer dependent on 3rd party installations when you run your application, and you no longer need to do tricky cleanup sessions before and after your tests, everything is done in memory and erased as soon as the test finishes.

## What's an Integration Test?
Good question! When it comes do different testing approches and terms, they tend to differ among us developers. In this context an integration test is a test that let you probe your 3rd party integration points while the application is running.

**Example:**
You have a Console Application that fetches data from a web api, aggregates the data and stores it in a database.

We can identify atleast two integration points:
1. The web api.
2. The databse.

During a continues integration process, your build server probably runs tests. If it were to run an integration test, it would need to host the application and have control over a web server where the web api exists and over a database instance. These would be hard to control in an orderly fashion, specially during parallel testing. 

In comes TTest.It.While.Hosting.Your.Console.Application to save the day!

## Download
https://www.nuget.org/packages/Test.It.While.Hosting.Your.Console.Application

## Getting Started
tl;dr:
Runnable example available here: https://github.com/Fresa/Test.It.While.Hosting.Your.Console.Application/blob/master/tests/Test.It.While.Hosting.Your.Console.Application.Tests/When_testing_a_console_application.cs

### Setting Up Your First Test
It all begins by you creating a test class which will inherit `ConsoleApplicationSpecification`. This specification defines how to configure, build, start and host your Console Application and at the same time create a communication channel between your test and the hosted application and control the whole test process. All without forcing you into a certain test framework, ofcourse.

The `ConsoleApplicationSpecification` requires an implementation of the `IConsoleApplicationHostStarter` as generic parameter. You may roll your own host starter, or you can use the `DefaultConsoleApplicationHostStarter` which will work in most cases. If you go for the latter it will need an application builder as generic parameter, i.e. you need to implement `IWindowsServiceBuilder`. `IWindowsServiceBuilder` will tell the hosting framework how to start your application. 

You need to specify a test configurer in your implementation of the `IWindowsServiceBuilder`. The test configurer will let you reconfigure your application during startup so you can overwrite the 3rd party integration client registration for example. You will need an implementation of the `IServiceContainer` interface to achive this. I highly recommend using an IOC/DI container (my favorite is SimpleInjector, https://simpleinjector.org/), but you are free to roll your own service container, see [`SimpleServiceContainer`](https://github.com/Fresa/Test.It.While.Hosting.Your.Console.Application/blob/master/tests/Test.It.While.Hosting.Your.Console.Application.Tests/SimpleServiceContainer.cs).

Now, in your test, you will have a `Client` property available where you can control the console application. It can among other things input data to your console application and listen on data being outputed.

### BDD
`WindowsServiceSpecification` uses a BDD style for arranging your test. You can read more about BDD here: https://en.wikipedia.org/wiki/Behavior-driven_development

#### Given
`ConsoleApplicationSpecification` exposes a method called `Given` which is overridable. This is where you set up your test. It exposes the `IServiceContainer` of your application, where you can override behaviour like the client implementation to your 3rd party applications (see Best Practices).

#### When
`ConsoleApplicationSpecification` also exposes a method called `When`. This is where you execute some method exposed by any of your integration points for example.

#### Then
This is your test method. This is where you assert what ever you expect to happen.

### Best Practices
When reconfiguring your application during startup, please be advised that doing to much reconfiguring will heavy alter your application behaviour and you might no longer test any relevant functionality of the Console Application. Keep the reconfiguration to a minimum to be sure to test as much as possible of the soon to be live functionality. Try to reconfigure your 3rd party dependecy systems as close to the network level as possible to assure you keep the reconfiguration to a minimum.

## Release Notes
**1.1.0** Changed the definition and name of the IConsoleApplicationConfiguration to IConsoleApplicationHostStarter
