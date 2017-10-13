using Hangfire;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HangfireConsoleDemo
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：LongRunningJob.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：LongRunningJob
    /// 创建标识：cml 2017/10/13 17:10:24
    /// </summary>
    [AutomaticRetry(Attempts = 0)]
    [DisableConcurrentExecution(90)]
    public class LongRunningJob : IRecurringJob
    {
        public void Execute(PerformContext context)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} LongRunningJob Running ...");

            var runningTimes = context.GetJobData<int>("RunningTimes");

            Console.WriteLine($"get job data parameter-> RunningTimes: {runningTimes}");

            var progressBar = context.WriteProgressBar();

            foreach (var i in Enumerable.Range(1, runningTimes).ToList().WithProgress(progressBar))
            {
                Thread.Sleep(1000);
            }
        }
    }
}