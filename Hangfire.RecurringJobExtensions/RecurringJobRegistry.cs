using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：RecurringJobRegistry.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：RecurringJobRegistry
    /// 创建标识：cml 2017/10/13 15:08:06
    /// </summary>
    public class RecurringJobRegistry : IRecurringJobRegistry
    {
        public void Register(RecurringJobInfo recurringJobInfo)
        {
            if (recurringJobInfo == null) throw new ArgumentNullException(nameof(recurringJobInfo));

            Register(recurringJobInfo.RecurringJobId, recurringJobInfo.Method, recurringJobInfo.Cron, recurringJobInfo.TimeZone, recurringJobInfo.Queue);
        }

        public void Register(MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            Register(method.GetRecurringJobId(), method, cron, timeZone, queue);
        }

        public void Register(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (recurringJobId == null) throw new ArgumentNullException(nameof(recurringJobId));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            var parameters = method.GetParameters();

            Expression[] args = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            var x = Expression.Parameter(method.DeclaringType, "x");

            var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

            var addOrUpdate = Expression.Call(
                typeof(RecurringJob),
                nameof(RecurringJob.AddOrUpdate),
                new Type[] { method.DeclaringType },
                new Expression[]
                {
                    Expression.Constant(recurringJobId),
                    Expression.Lambda(methodCall, x),
                    Expression.Constant(cron),
                    Expression.Constant(timeZone),
                    Expression.Constant(queue)
                });

            Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
        }
    }
}
