using System.Linq;
using System.Threading;
using Serilog;
using Serilog.Core;

namespace Sample
{
    public class Program
    {
        public static void Main()
        {
            // By sharing between the Seq sink and logger itself,
            // Seq API keys can be used to control the level of the whole logging pipeline.
            var levelSwitch = new LoggingLevelSwitch();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Console()
                .WriteTo.SplunkEventCollector(splunkHost: "http://localhost:5341",
                                              eventCollectorToken: "b9f1fc7d-7aea-4aec-83ec-1be2f2b03c19",
                                              bufferFileFullName: "Test.log")
                .CreateLogger();

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
