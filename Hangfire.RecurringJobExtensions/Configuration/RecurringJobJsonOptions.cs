using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Server;

namespace Hangfire.RecurringJobExtensions.Configuration
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：RecurringJobJsonOptions.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：RecurringJobJsonOptions
    /// 创建标识：cml 2017/10/13 15:45:31
    /// </summary>
    public class RecurringJobJsonOptions
    {
        // <summary>
        /// The job name represents for <see cref="RecurringJobInfo.RecurringJobId"/>
        /// </summary>
        [JsonProperty("job-name")]
        public string JobName { get; set; }
        /// <summary>
        /// The job type while impl the interface <see cref="IRecurringJob"/>.
        /// </summary>
        [JsonProperty("job-type")]

        public Type JobType { get; set; }

        /// <summary>
        /// Cron expressions
        /// </summary>
        [JsonProperty("cron-expression")]

        public string Cron { get; set; }

        /// <summary>  
        /// The value of <see cref="TimeZoneInfo"/> can be created by <seealso cref="TimeZoneInfo.FindSystemTimeZoneById(string)"/>
        /// </summary>
        [JsonProperty("timezone")]
        [JsonConverter(typeof(TimeZoneInfoConverter))]
        public TimeZoneInfo TimeZone { get; set; }
        /// <summary>
        /// Whether the property <see cref="TimeZone"/> can be serialized or not.
        /// </summary>
        /// <returns>true if value not null, otherwise false.</returns>
        public bool ShouldSerializeTimeZone() => TimeZone != null;
        /// <summary>
        /// Hangfire queue name
        /// </summary>
        [JsonProperty("queue")]
        public string Queue { get; set; }
        /// <summary>
        /// Whether the property <see cref="Queue"/> can be serialized or not.
        /// </summary>
        /// <returns>true if value not null or empty, otherwise false.</returns>
        public bool ShouldSerializeQueue() => !string.IsNullOrEmpty(Queue);
        /// <summary>
        /// The <see cref="RecurringJob"/> data persisted in <see cref="PerformContext"/> with server filter <seealso cref="ExtendedDataJobFilter"/>.  
        /// </summary>
        [JsonProperty("job-data")]
        public IDictionary<string, object> ExtendedData { get; set; }
        /// <summary>
        /// Whether the property <see cref="ExtendedData"/> can be serialized or not.
        /// </summary>
        /// <returns>true if value not null or count is zero, otherwise false.</returns>
        public bool ShouldSerializeExtendedData() => ExtendedData != null && ExtendedData.Count > 0;

        /// <summary>
        /// Whether the <see cref="RecurringJob"/> can be added/updated,
        /// default value is true, if false it will be deleted automatically.
        /// </summary>
        [JsonProperty("enable")]
        public bool? Enable { get; set; }

        /// <summary>
        /// Whether the property <see cref="Enable"/> can be serialized or not.
        /// </summary>
        /// <returns>true if value is not null, otherwise false.</returns>
        public bool ShouldSerializeEnable() => Enable.HasValue;
    }
}
