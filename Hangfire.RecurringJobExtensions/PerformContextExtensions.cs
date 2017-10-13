﻿using Hangfire.Common;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：PerformContextExtensions.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：PerformContextExtensions
    /// 创建标识：cml 2017/10/13 17:12:33
    /// </summary>
    public static class PerformContextExtensions
    {
        /// <summary>
        /// Gets job data from <see cref="PerformContext"/> if json configuration exists token 'job-data'.
        /// </summary>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="name">The dictionary key from the property <see cref="RecurringJobInfo.ExtendedData"/></param>
        /// <returns>The value from the property <see cref="RecurringJobInfo.ExtendedData"/></returns>
        public static object GetJobData(this PerformContext context, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var jobDataKey = $"recurringjob-info-{context.BackgroundJob.Job.ToString()}";

            if (!context.Items.ContainsKey(jobDataKey)) return null;

            var jobData = context.Items[jobDataKey] as IDictionary<string, object>;

            if (jobData == null || jobData.Count == 0) return null;

            if (!jobData.ContainsKey(name)) return null;

            return jobData[name];
        }
        /// <summary>
        /// Gets job data from <see cref="PerformContext"/> if json configuration exists token 'job-data'.
        /// </summary>
        /// <typeparam name="T">The specified type to json value.</typeparam>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="name">The dictionary key from the property <see cref="RecurringJobInfo.ExtendedData"/></param>
        /// <returns>The value from the property <see cref="RecurringJobInfo.ExtendedData"/></returns>
        public static T GetJobData<T>(this PerformContext context, string name)
        {
            var o = GetJobData(context, name);

            var json = JobHelper.ToJson(o);

            return JobHelper.FromJson<T>(json);
        }
        /// <summary>
        /// Persists job data to <see cref="PerformContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="name">The dictionary key from the property <see cref="RecurringJobInfo.ExtendedData"/></param>
        /// <param name="value">The persisting value.</param>
        public static void SetJobData(this PerformContext context, string name, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var jobDataKey = $"recurringjob-info-{context.BackgroundJob.Job.ToString()}";

            if (!context.Items.ContainsKey(jobDataKey))
                throw new KeyNotFoundException($"The job data key: {jobDataKey} is not found.");

            var jobData = context.Items[jobDataKey] as IDictionary<string, object>;

            if (jobData == null) jobData = new Dictionary<string, object>();

            jobData[name] = value;
        }
    }
}