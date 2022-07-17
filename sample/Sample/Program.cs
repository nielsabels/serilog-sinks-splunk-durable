using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;

namespace Sample
{
    public class Program
    {
        public static void Main()
        {
            var levelSwitch = new LoggingLevelSwitch();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Console()
                .WriteTo.SplunkEventCollector(splunkHost: "http://localhost:8088",
                                              eventCollectorToken: "1e3fa824-6c86-4e14-a1de-ce87d1bb5dd8",
                                              bufferFileFullName: "Test.log",
                                              messageHandler: new HttpClientHandler()
                                              {
                                                  //for capturing data that sent to splunk using fiddler proxy
                                                  Proxy = new WebProxy("127.0.0.1", 8888),
                                              })
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
