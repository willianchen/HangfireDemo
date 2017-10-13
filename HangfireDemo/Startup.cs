using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.MemoryStorage;

[assembly: OwinStartup(typeof(HangfireDemo.Startup))]

namespace HangfireDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888

            //指定Hangfire使用内存存储后台任务信息
            // GlobalConfiguration.Configuration.UseMemoryStorage();
            GlobalConfiguration.Configuration.UseRedisStorage("127.0.0.1:6379");
            //启用HangfireServer这个中间件（它会自动释放）
            //app.UseHangfireServer();
            //启用Hangfire的仪表盘（可以看到任务的状态，进度等信息）
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate(
              () => Console.WriteLine("{0} Recurring job completed successfully!", DateTime.Now.ToString()),
              Cron.Minutely);
        }
    }
}
