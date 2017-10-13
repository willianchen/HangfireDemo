using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Server;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：RecurringJobInfo.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：RecurringJobInfo
    /// 创建标识：cml 2017/10/13 14:45:30
    /// </summary>

    /// <summary>
    /// It is used to build <see cref="RecurringJob"/> 
    /// with <see cref="IRecurringJobBuilder.Build(Func{System.Collections.Generic.IEnumerable{RecurringJobInfo}})"/>.
    /// </summary>
    public class RecurringJobInfo
    {
        /// <summary>
		/// The identifier of the RecurringJob
		/// </summary>
		public string RecurringJobId { get; set; }
        /// <summary>
        /// Cron expressions
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// TimeZoneInfo
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }
        /// <summary>
        /// Queue name
        /// </summary>
        public string Queue { get; set; }
        /// <summary>
        /// Method to execute while <see cref="RecurringJob"/> running.
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// The <see cref="RecurringJob"/> data persisted in <see cref="PerformContext"/> with server filter <seealso cref="ExtendedDataJobFilter"/>.  
        /// </summary>
        public IDictionary<string, object> ExtendedData { get; set; }

        /// <summary>
        /// Whether the <see cref="RecurringJob"/> can be added/updated,
        /// default value is true, if false it will be deleted automatically.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Returns a string that represents {<see cref="Method"/>.DeclaringType.Name}.{<see cref="Method"/>.Name} 
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Method.GetRecurringJobId();
        }
    }
}
