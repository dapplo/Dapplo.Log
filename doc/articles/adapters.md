# Dapplo.Log and other log frameworks

This framework would not be complete if there would not adapters to other frameworks.

Currently there are only examples of "wrappers" in the test project, these are not available in a NuGet Package. This is mainly due to the quick changing frameworks, any adapter package would be obsolete by the time I created it.

Here are the examples:
- a [NLogLogger](https://github.com/dapplo/Dapplo.Log/blob/master/Dapplo.Log.Tests/Logger/NLogLogger.cs) for loggin with Dapplo.Log to NLog
- a [Log4NetLogger](https://github.com/dapplo/Dapplo.Log/blob/master/Dapplo.Log.Tests/Logger/Log4NetLogger.cs) for loggin with Dapplo.Log to Log4Net
- a [SeriLogLogger](https://github.com/dapplo/Dapplo.Log/blob/master/Dapplo.Log.Tests/Logger/SeriLogLogger.cs) for loggin with Dapplo.Log to SeriLog
- a special [StringWriterLogger](https://github.com/dapplo/Dapplo.Log/blob/master/Dapplo.Log.Tests/Logger/StringWriterLogger.cs) for loggin with Dapplo.Log to a StringWriter, this is Thread/Task aware and takes care of separating the StringWriters for your tests. (e.g. works with xUnit tests)
