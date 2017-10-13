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
    /// 类名：IRecurringJobRegistry.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：IRecurringJobRegistry
    /// 创建标识：cml 2017/10/13 15:05:17
    /// </summary>
    public  interface IRecurringJobRegistry
    {
        /// <summary>
		/// Register RecurringJob via <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="method">the specified method</param>
		/// <param name="cron">Cron expressions</param>
		/// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
		/// <param name="queue">Queue name</param>
		void Register(MethodInfo method, string cron, TimeZoneInfo timeZone, string queue);
        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="recurringJobId">The identifier of the RecurringJob</param>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        void Register(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue);
        /// <summary>
        /// Register RecurringJob via <see cref="RecurringJobInfo"/>.
        /// </summary>
        /// <param name="recurringJobInfo"><see cref="RecurringJob"/> info.</param>
        void Register(RecurringJobInfo recurringJobInfo);
    }
}
