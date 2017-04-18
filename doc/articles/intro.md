# Dapplo.Log

Ever written a framework? Well... I did, and one of the biggest problems I had was deciding on a logger framework.
Most of the time, I didn't decide on a logger as this would force the user to use or at least include the one I used.
This project will help you out, and make it possible for your user to decide where the entries go.

To you this, you add the nuget package:
```
Install-Package Dapplo.Log
```

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

Where the Log is the static LogSource variable in your class, the method after this defines the log level and the WriteLine only specifies the information to log.

There is a reason why this is defined like this:
- The LogSource captures the calling class once, so you know where the log came from.
- The log level method is an extension method which defines the level, captures the calling method name and line in your code. Available levels are:
  - None
  - Verbose
  - Debug
  - Info
  - Warn
  - Error
  - Fatal
- The Write / WriteLine extension takes care of the arguments.
- If no logger is available, or the level is higher, nothing happens. If there is a logger with the right level, all information is passed to the logger.

To see the output from the code in your project, you will need to instanciate a logger before using it.

There are a couple of loggers available, in Dapplo.Log.Loggers, here some examples:
* TraceLogger, which uses the System.Diagnostics.Trace to write the formatted information to.
* ConsoleLogger, which uses the Console to write the formatted information to.

If you need a specific log level, use either this before creating your TraceLogger:
```
LogSettings.DefaultLevel = LogLevels.Warn;
```
Or you can set the level on the Logger itself.

You enable the logging like this:
```
LogSettings.RegisterDefaultLogger<TraceLogger>(LogLevels.Warn)
```
