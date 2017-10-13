using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using System;

namespace HangfireConsoleDemo
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：MyJob1.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：MyJob1
    /// 创建标识：cml 2017/10/13 17:04:44
    /// </summary>
    public class MyJob1 : IRecurringJob
    {
        public void Execute(PerformContext context)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob1 Running ...");
        }
    }
}