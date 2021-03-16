using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SampleUsingAppSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                                        .AddJsonFile("logsettings.json", false)
                                        .Build();
            var levelSwitch = new LoggingLevelSwitch();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();
            var file = File.CreateText($"internal-{DateTime.Now.Ticks}.log");
            SelfLog.Enable(TextWriter.Synchronized(file));


            Log.Information("Sample starting up");
            foreach (var i in Enumerable.Range(0, 1000))
            {
                Log.Information("Running loop {Counter}, switch is at {Level}", i, levelSwitch.MinimumLevel);

                Thread.Sleep(1000);
                Log.Debug("Loop iteration done");
            }
            Log.CloseAndFlush();
        }
    }
}
