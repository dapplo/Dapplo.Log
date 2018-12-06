# Dapplo.Log

This contains a simple logging facade in dapplo.log, as well as some simple loggers, a file logger and also some (currently example) adapters for other log frameworks.
The facade allows a framework/library to have log statements without forcing the project that uses this to use the same huge logger.
Without a logger and if used correctly the performance penalty is extremely small, as soon as you have issues you can set a logger and get some information.

The project is build modular, currently the facade which is the least you will need, is about 17KB.
Adding a file logger adds another 21KB, which totals to <40KB. Just as a comparison, log4net is about 300KB. (although unfair, it can do a lot more)

- Documentation can be found [here](http://www.dapplo.net/documentation/Dapplo.Log)
- Current build status Azure DevOps: [![Build Status](https://dev.azure.com/Dapplo/Dapplo.Log/_apis/build/status/dapplo.Dapplo.Log)](https://dev.azure.com/Dapplo/Dapplo.Log/_build/latest?definitionId=1)
- Coverage Status Azure DevOps: [![Coverage Status](https://img.shields.io/azure-devops/coverage/Dapplo/Dapplo.Log/1.svg)](https://dapplo.visualstudio.com/Dapplo.Log/_build?definitionId=1)
- Current build status AppVeyor: [![Build status](https://ci.appveyor.com/api/projects/status/5s97m6ha9niupt1y?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-log)
- Coverage Status coveralls: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.Log/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.Log?branch=master)
- NuGet package Dapplo.Log: [![NuGet package](https://badge.fury.io/nu/dapplo.log.svg)](https://badge.fury.io/nu/dapplo.log)
- NuGet package Dapplo.Log.Loggers: [![NuGet package](https://badge.fury.io/nu/dapplo.log.loggers.svg)](https://badge.fury.io/nu/dapplo.log.loggers)
- NuGet package Dapplo.Log.LogFile: [![NuGet package](https://badge.fury.io/nu/dapplo.log.logfile.svg)](https://badge.fury.io/nu/dapplo.log.logfile)
- NuGet package Dapplo.Log.XUnit: [![NuGet package](https://badge.fury.io/nu/dapplo.log.xunit.svg)](https://badge.fury.io/nu/dapplo.log.xunit)

This package supports most platforms / profiles, if something is missing let me know.

Ever written a framework? Well... I did, and one of the biggest problems I had was deciding on a logger framework.
Most of the time, I didn't decide on a logger as this would force the user to use or at least include the one I used.
This project will help you out, and make it possible for your user to decide where the entries go.

To you this, you add the nuget package.
Add the using statement to your class:
```
using Dapplo.Log;
```

Include a static one-liner in your class:
```
private static readonly LogSource Log = new LogSource();
```

And wherever you want to write a log statement, you call:
```
Log.Info().WriteLine(message, params);
Log.Error().WriteLine(exception, message, params);
```

Where the Log is the static LogSource variable in your class, the method after this defines the log level and the WriteLine only specifies the information to log. There is a reason why this is defined like this:
- The LogSource captures the calling class (by using StackTrace or CallerFilePath for PCL) once, so you know where the log came from.
- The log level method is an extension method which defines the level, captures the calling method and line of the code. Available levels are:
  - None
  - Verbose
  - Debug
  - Info
  - Warn
  - Error
  - Fatal
- The Write / WriteLine extension takes care of the arguments.
- If no logger is available, or the level is higher, nothing happens. If there is a logger with the right leven, all information is passed to the logger.

To see the output from the code in your project, you will need to instanciate a logger before using it.
There are a couple of loggers available, here some examples:
* TraceLogger, which uses the System.Diagnostics.Trace to write the formatted information to.
* ConsoleLogger, which uses the Console to write the formatted information to.
* XUnitLogger, this is specific for XUnit and makes it possible to see the output to every Fact

If you need a specific log level, use either this before creating your TraceLogger:
```
LogSettings.DefaultLevel = LogLevel.Warn;
```
Or you can set the level on the Logger itself.

You enable the logging like this:
```
LogSettings.RegisterDefaultLogger<TraceLogger>(LogLevel)
```

A file logger is also available, it supports:
- Async writing to the file, it will only delay your application with a small overhead of accessing an internal queue. 
- Rolling filenames
- If rolling, the file can be moved to a directory and have a different filename.
- If rolling, the file can be gzipped
Available in the package Dapplo.Log.LogFile

I have included examples of "wrappers" in the test project, these are not available in a NuGet Package.
- a [NLogLogger](https://github.com/dapplo/Dapplo.Log/blob/master/src/Dapplo.Log.Tests/Logger/NLogLogger.cs) for loggin with Dapplo.Log to NLog
- a [Log4NetLogger](https://github.com/dapplo/Dapplo.Log/blob/master/src/Dapplo.Log.Tests/Logger/Log4NetLogger.cs) for loggin with Dapplo.Log to Log4Net
- a [Log4NetDapploAppender](https://github.com/dapplo/Dapplo.Log/blob/master/src/Dapplo.Log.Tests/Logger/Log4NetDapploAppender.cs) for loggin with Log4Net to Dapplo.Log
- a [SeriLogLogger](https://github.com/dapplo/Dapplo.Log/blob/master/src/Dapplo.Log.Tests/Logger/SeriLogLogger.cs) for loggin with Dapplo.Log to SeriLog

That is all for now...