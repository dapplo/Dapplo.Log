# Dapplo.Log

> As soon as the .NET Platform Standard is usable (more stable, less hacky) I will change the project to support this.

This contains both a simple Facade as some simple loggers and log adaptors.
The Facade allows a framework/library to log without forcing the project that uses this to use the same huge logger.

- Documentation can be found [here](http://www.dapplo.net/blocks/Dapplo.Log.Facade) (soon)
- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/5s97m6ha9niupt1y?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-log)
- Coverage Status: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.Log/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.Log?branch=master)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/dapplo.log.facade.svg)](https://badge.fury.io/nu/dapplo.facade.log)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/dapplo.log.loggers.svg)](https://badge.fury.io/nu/dapplo.log.loggers)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/dapplo.log.logfile.svg)](https://badge.fury.io/nu/dapplo.log.logfile)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/dapplo.log.xunit.svg)](https://badge.fury.io/nu/dapplo.log.xunit)

This package supports most platforms / profiles, if something is missing let me know.

Ever written a framework? Well... I did, and one of the biggest problems I had was deciding on a logger framework.
Most of the time, I didn't decide on a logger as this would force the user to use or at least include the one I used.
This project will help you out, and make it possible for your user to decide where the entries go.

To you this, you add the nuget package.
Add the using statement to your class:
```
using Dapplo.Log.Facade;
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

So, where is the file logger? There currently is none, I wanted to keep it as simple as possible. I will add a simple file logger later.

One additional thing might need to be mentioned: there is a Format property in ILogger which can be used to set a function which accepts a string and object[], returns a string), normally string.Format is used but it can be changed to use your own formatting.
An example with using FormatWith from Dapplo.Utils:
```
LogSettings.DefaultLogger = new TraceLogger { Level = LogLevel.Info, Format = StringExtensions.FormatWith };
Log.Info().WriteLine("{Name} is {Age} years old", new { Name = "Jan", Age = 10 });
```
Would give an entry in your debug console with something like: Date time - Info - class:method(linenr) - Jan is 10 years old


I have included examples of "wrappers" in the test project, these are not available in a NuGet Package.
- a [NLogLogger](https://github.com/dapplo/Dapplo.LogFacade/blob/master/Dapplo.LogFacade.Tests/Logger/NLogLogger.cs) for loggin Dapplo.LogFacade to NLog
- a [SeriLogLogger](https://github.com/dapplo/Dapplo.LogFacade/blob/master/Dapplo.LogFacade.Tests/Logger/SeriLogLogger.cs) for loggin Dapplo.LogFacade to SeriLog
- a special [StringWriterLogger](https://github.com/dapplo/Dapplo.LogFacade/blob/master/Dapplo.LogFacade.Tests/Logger/StringWriterLogger.cs) for loggin Dapplo.LogFacade to a StringWriter, this is Thread/Task aware and takes care of separating the StringWriters for your tests. (e.g. works with xUnit tests)

That is all for now...