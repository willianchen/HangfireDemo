﻿using Hangfire.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：ExtendedDataJobFilter.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：ExtendedDataJobFilter
    /// 创建标识：cml 2017/10/13 15:54:05
    /// </summary>
    public class ExtendedDataJobFilter : IServerFilter
    {
        /// <summary>
        /// The dictionary of <see cref="RecurringJobInfo"/>.
        /// </summary>
        public ConcurrentDictionary<string, RecurringJobInfo> RecurringJobInfos { get; } = new ConcurrentDictionary<string, RecurringJobInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedDataJobFilter"/>
        /// </summary>
        /// <param name="recurringJobInfos">The list of <see cref="RecurringJobInfo"/>.</param>
        public ExtendedDataJobFilter(IList<RecurringJobInfo> recurringJobInfos)
        {
            if (recurringJobInfos == null) throw new ArgumentNullException(nameof(recurringJobInfos));

            //initialize data if exists extendeddata.
            foreach (var jobInfo in recurringJobInfos.Where(x => x.Enable && x.ExtendedData != null && x.ExtendedData.Count > 0))
                RecurringJobInfos.TryAdd(jobInfo.ToString(), jobInfo);
        }

        /// <summary>
        /// Called before the performance of the job.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnPerforming(PerformingContext filterContext)
        {
            if (RecurringJobInfos == null
                || RecurringJobInfos.Count == 0
                || !RecurringJobInfos.ContainsKey(filterContext.BackgroundJob.Job.ToString())) return;

            var jobInfo = RecurringJobInfos[filterContext.BackgroundJob.Job.ToString()];

            if (jobInfo == null) return;

            var jobDataKey = $"recurringjob-info-{jobInfo.ToString()}";

            if (jobInfo.ExtendedData == null) jobInfo.ExtendedData = new Dictionary<string, object>();

            filterContext.Items[jobDataKey] = jobInfo.ExtendedData;
        }
        /// <summary>
        /// Called after the performance of the job.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnPerformed(PerformedContext filterContext)
        {
            //do nothing?
        }

    }
}