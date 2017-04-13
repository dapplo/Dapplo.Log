# Dapplo.Log

This contains a simple logging facade in dapplo.log, as well as some simple loggers, a file logger and also some (currently example) adapters for other log frameworks.
The facade allows a framework/library to have log statements without forcing the project that uses this to use the same huge logger.
Without a logger and if used correctly the performance penalty is extremely small, as soon as you have issues you can set a logger and get some information.

The project is build modular, currently the facade which is the least you will need, is about 17KB.
Adding a file logger adds another 21KB, which totals to <40KB. Just as a comparison, log4net is about 300KB. (although unfair, it can do a lot more)

- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/5s97m6ha9niupt1y?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-log)
- Coverage Status: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.Log/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.Log?branch=master)
- NuGet package Dapplo.Log: [![NuGet package](https://badge.fury.io/nu/dapplo.log.svg)](https://badge.fury.io/nu/dapplo.log)
- NuGet package Dapplo.Log.Loggers: [![NuGet package](https://badge.fury.io/nu/dapplo.log.loggers.svg)](https://badge.fury.io/nu/dapplo.log.loggers)
- NuGet package Dapplo.Log.LogFile: [![NuGet package](https://badge.fury.io/nu/dapplo.log.logfile.svg)](https://badge.fury.io/nu/dapplo.log.logfile)
- NuGet package Dapplo.Log.XUnit: [![NuGet package](https://badge.fury.io/nu/dapplo.log.xunit.svg)](https://badge.fury.io/nu/dapplo.log.xunit)

This package supports most platforms / profiles, if something is missing let me know.