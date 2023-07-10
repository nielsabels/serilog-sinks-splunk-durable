# Note

This package is based upon [Serilog.Sinks.Splunk.Durable](https://nuget.org/packages/serilog.Sinks.Splunk.Durable). It includes the work from [this Pull Request](https://github.com/alirezavafi/serilog-sinks-splunk-durable/pull/1/files).

# Serilog.Sinks.Splunk.Durable.Customized [![NuGet](https://img.shields.io/nuget/v/Serilog.Sinks.Splunk.Durable.svg)](https://nuget.org/packages/serilog.Sinks.Splunk.Durable) 

A Resilient and durable Serilog sink that writes events to the Splunk (based on Seq sink https://datalust.co/seq). Supports .NET 4.5+, .NET Core, and platforms compatible with the [.NET Platform Standard](https://github.com/dotnet/corefx/blob/master/Documentation/architecture/net-platform-standard.md) 1.1 including Windows 8 & UWP, Windows Phone and Xamarin.

### Getting started

Install the _Serilog.Sinks.Splunk.Durable_ package from Visual Studio's _NuGet_ console:

```powershell
PM> Install-Package Serilog.Sinks.Splunk.Durable
```

Point the logger to Splunk Event Collector:

```csharp
Log.Logger = new LoggerConfiguration()
                     .WriteTo
                     .SplunkEventCollector(splunkHost: "http://localhost:8088",
                                           eventCollectorToken: "1e3fa824-6c86-4e14-a1de-ce87d1bb5dd8",
                                           bufferFileFullName: "Test.log")    
                     .CreateLogger();
```

And use the Serilog logging methods to associate named properties with log events:

```csharp
Log.Error("Failed to log on user {ContactId}", contactId);
```

### JSON `appsettings.json` configuration

To use the Splunk.Plus sink with _Microsoft.Extensions.Configuration_, for example with ASP.NET Core or .NET Core, use the [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) package. First install that package if you have not already done so:

```powershell
Install-Package Serilog.Settings.Configuration
```

Instead of configuring the sink directly in code, call `ReadFrom.Configuration()`:

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
```

In your `appsettings.json` file, under the `Serilog` node, :

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Splunk.Durable" ],
    "WriteTo": [
       {
        "Name": "SplunkEventCollector",
        "Args": {
          "splunkHost": "http://localhost:8088",
          "eventCollectorToken": "1e3fa824-6c86-4e14-a1de-ce87d1bb5dd8",
          "bufferFileFullName": "log-buffer.txt"
        }
      }
    ]
  }
}
```

See the XML `<appSettings>` example above for a discussion of available `Args` options.
