using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Console;

[assembly: OwinStartup(typeof(HangfireConsoleDemo.Startup))]

namespace HangfireConsoleDemo
{
    public class Startup
    {
        //private readonly IConfigurationRoot _config;

        //public Startup(IHostingEnvironment env)
        //{
        //    // Set up configuration providers.
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        //    if (env.IsDevelopment())
        //    {
        //        // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
        //        //builder.AddUserSecrets();
        //    }

        //    builder.AddEnvironmentVariables();

        //    _config = builder.Build();
        //}

        public void Configuration(IAppBuilder app)
        {
            // app.UseHangfireDashboard();
            //  app.UseErrorPage();
            // app.UseWelcomePage("/");
            GlobalConfiguration.Configuration.UseRedisStorage("127.0.0.1:6379");
            GlobalConfiguration.Configuration.UseRecurringJob("recurringjob.json");
            //GlobalConfiguration.Configuration.UseRecurringJob(typeof(RecurringJobService));
            GlobalConfiguration.Configuration.UseDefaultActivator();
            GlobalConfiguration.Configuration.UseConsole();
            //GlobalConfiguration.Configuration.use

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] { "default", "apis", "jobs" }
            });


            //   app.UseHangfireServer();

            RecurringJob.AddOrUpdate(
                () => Console.WriteLine("{0} Recurring job completed successfully!", DateTime.Now.ToString()),
                Cron.Minutely);

            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
