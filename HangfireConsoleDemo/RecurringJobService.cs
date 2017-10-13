using System;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using Hangfire;

namespace HangfireConsoleDemo
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：RecurringJobService.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：RecurringJobService
    /// 创建标识：cml 2017/10/13 16:48:17
    /// </summary>
    public class RecurringJobService
    {
        [RecurringJob("*/1 * * * *")]
        [Queue("jobs")]
        public void TestJob1(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} TestJob1 Running ...");
        }
        [RecurringJob("*/2 * * * *", RecurringJobId = "TestJob2")]
        [Queue("jobs")]
        public void TestJob2(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} TestJob2 Running ...");
        }
        [RecurringJob("*/2 * * * *", "China Standard Time", "jobs")]
        public void TestJob3(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} TestJob3 Running ...");
        }
        [RecurringJob("*/5 * * * *", "jobs")]
        public void InstanceTestJob(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} InstanceTestJob Running ...");
        }

        [RecurringJob("*/6 * * * *", "UTC", "jobs")]
        public static void StaticTestJob(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} StaticTestJob Running ...");
        }
    }
}
