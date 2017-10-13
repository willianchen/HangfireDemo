using Hangfire.Common;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireConsoleDemo
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：MyJob2.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：MyJob2
    /// 创建标识：cml 2017/10/13 17:08:30
    /// </summary>
    public class MyJob2 : IRecurringJob
    {
        class SimpleObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public void Execute(PerformContext context)
        {

            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob2 Running ...");

            var intVal = context.GetJobData<int>("IntVal");

            var stringVal = context.GetJobData<string>("StringVal");

            var booleanVal = context.GetJobData<bool>("BooleanVal");

            var simpleObject = context.GetJobData<SimpleObject>("SimpleObject");

            Console.WriteLine($"IntVal:{intVal},StringVal:{stringVal},BooleanVal:{booleanVal},simpleObject:{JobHelper.ToJson(simpleObject)}");

            context.SetJobData("IntVal", ++intVal);

            Console.WriteLine($"IntVal changed to {intVal}");
        }
    }
}