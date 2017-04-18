# Dapplo.Log.LogFile

A file logger is also available, in @Dapplo.Log.LogFile, it supports:
- Async writing to the file, it will only delay your application with a small overhead of accessing an internal queue. 
- Rolling filenames
- If rolling, the file can be moved to a directory and have a different filename.
- If rolling, the file can be gzipped

Available in the package Dapplo.Log.LogFile, which can be installed via NuGet:
```
Install-Package Dapplo.Log.LogFile
```

