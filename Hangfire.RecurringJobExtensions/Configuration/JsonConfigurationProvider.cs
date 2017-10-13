using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions.Configuration
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：JsonConfigurationProvider.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：Json配置读取类
    /// 创建标识：cml 2017/10/13 14:39:29
    /// </summary>
    public class JsonConfigurationProvider : FileConfigurationProvider
    {
        /// <summary>
        /// Initializes a new <see cref="JsonConfigurationProvider"/>.
        /// </summary>
        /// <param name="builder">The builder for <see cref="IRecurringJobBuilder"/>.</param>
        /// <param name="configFile">The source settings file.</param>
        /// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
        public JsonConfigurationProvider(IRecurringJobBuilder builder, string configFile, bool reloadOnChange = true)
            : base(builder, configFile, reloadOnChange) { }

        /// <summary>
        /// Loads the <see cref="RecurringJobInfo"/> for this source.
        /// </summary>
        /// <returns>The list of <see cref="RecurringJobInfo"/> for this provider.</returns>
        public override IEnumerable<RecurringJobInfo> Load()
        {
            var jsonContent = ReadFromFile();

            if (string.IsNullOrWhiteSpace(jsonContent)) throw new Exception("Json file content is empty.");

            var jsonOptions = JobHelper.FromJson<List<RecurringJobJsonOptions>>(jsonContent);

            foreach (var o in jsonOptions)
                yield return Convert(o);
        }

        private RecurringJobInfo Convert(RecurringJobJsonOptions option)
        {
            ValidateJsonOptions(option);

            return new RecurringJobInfo
            {
                RecurringJobId = option.JobName,
#if NET45
				Method = option.JobType.GetMethod(nameof(IRecurringJob.Execute)),
#else
                Method = option.JobType.GetTypeInfo().GetDeclaredMethod(nameof(IRecurringJob.Execute)),
#endif
                Cron = option.Cron,
                Queue = option.Queue ?? EnqueuedState.DefaultQueue,
                TimeZone = option.TimeZone ?? TimeZoneInfo.Utc,
                ExtendedData = option.ExtendedData,
                Enable = option.Enable ?? true
            };
        }

        private void ValidateJsonOptions(RecurringJobJsonOptions option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));

            if (string.IsNullOrWhiteSpace(option.JobName))
            {
                throw new Exception($"The json token 'job-name' is null, empty, or consists only of white-space.");
            }

            if (!option.JobType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRecurringJob)))
            {
                throw new Exception($"job-type: {option.JobType} must impl the interface {typeof(IRecurringJob)}.");
            }

        }
    }
}