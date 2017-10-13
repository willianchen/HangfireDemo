using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：RecurringJobBuilder.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：RecurringJobBuilder
    /// 创建标识：cml 2017/10/13 15:24:04
    /// </summary>
    public class RecurringJobBuilder : IRecurringJobBuilder
    {
        private IRecurringJobRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringJobBuilder"/>	with <see cref="IRecurringJobRegistry"/>.
        /// </summary>
        /// <param name="registry"><see cref="IRecurringJobRegistry"/> interface.</param>
        public RecurringJobBuilder(IRecurringJobRegistry registry)
        {
            _registry = registry;
        }

        public void Build(Func<IEnumerable<RecurringJobInfo>> recurringJobInfoProvider)
        {
            if (recurringJobInfoProvider == null) throw new ArgumentNullException(nameof(recurringJobInfoProvider));

            foreach (RecurringJobInfo recurringJobInfo in recurringJobInfoProvider())
            {
                if (string.IsNullOrWhiteSpace(recurringJobInfo.RecurringJobId))
                {
                    throw new Exception($"The property of {nameof(recurringJobInfo.RecurringJobId)} is null, empty, or consists only of white-space.");
                }
                if (!recurringJobInfo.Enable)
                {
                    RecurringJob.RemoveIfExists(recurringJobInfo.RecurringJobId);
                    continue;
                }
                _registry.Register(recurringJobInfo);
            }
        }

        public void Build(Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            foreach (var type in typesProvider())
            {
                foreach (var method in type.GetTypeInfo().DeclaredMethods)
                {
                    if (!method.IsDefined(typeof(RecurringJobAttribute), false)) continue;

                    var attribute = method.GetCustomAttribute<RecurringJobAttribute>(false);

                    if (attribute == null) continue;

                    if (string.IsNullOrWhiteSpace(attribute.RecurringJobId))
                    {
                        attribute.RecurringJobId = method.GetRecurringJobId();
                    }

                    if (!attribute.Enabled)
                    {
                        RecurringJob.RemoveIfExists(attribute.RecurringJobId);
                        continue;
                    }
                    _registry.Register(
                        attribute.RecurringJobId,
                        method,
                        attribute.Cron,
                        string.IsNullOrEmpty(attribute.TimeZone) ? TimeZoneInfo.Utc : TimeZoneInfo.FindSystemTimeZoneById(attribute.TimeZone),
                        attribute.Queue ?? EnqueuedState.DefaultQueue);
                }
            }
        }
    }
}
