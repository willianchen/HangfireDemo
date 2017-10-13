using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Topshelf;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;

namespace HangfireConsoleDemo
{
    class Program
    {
        private const string Endpoint = "http://127.0.0.1:12345";
        static void Main(string[] args)
        {
            //LogProvider.SetCurrentLogProvider(new ColouredConsoleLogProvider());
            HostFactory.Run(x =>
            {
                x.Service<Application>(s =>
                {
                    s.ConstructUsing(name => new Application());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Hangfire Windows Service Sample");
                x.SetDisplayName("Hangfire Windows Service Sample");
                x.SetServiceName("hangfire-sample");
            });
        }


        private class Application
        {
            private IDisposable _host;

            public void Start()
            {
                _host = WebApp.Start<Startup>(Endpoint);

                Console.WriteLine();
                Console.WriteLine("Hangfire Server started.");
                Console.WriteLine("Dashboard is available at {0}/hangfire", Endpoint);
                Console.WriteLine();
            }

            public void Stop()
            {
                _host.Dispose();
            }
        }
    }
}
