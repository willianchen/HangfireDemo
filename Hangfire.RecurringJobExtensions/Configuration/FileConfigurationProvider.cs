﻿using Hangfire.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions.Configuration
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：FileConfigurationProvider.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：文件配置帮助类
    /// 创建标识：cml 2017/10/13 14:40:26
    /// </summary>
    public abstract class FileConfigurationProvider : IConfigurationProvider, IDisposable
    {
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;

        private static readonly ILog _logger = LogProvider.For<FileConfigurationProvider>();
        private readonly FileSystemWatcher _fileWatcher;
        private readonly object _fileWatcherLock = new object();
        private readonly IRecurringJobBuilder _builder;

        public virtual void Dispose()
        {
            _fileWatcher?.Dispose();
        }

        /// <summary>
		/// <see cref="RecurringJob"/> configuraion file
		/// </summary>
		public virtual string ConfigFile { get; set; }

        public abstract IEnumerable<RecurringJobInfo> Load();

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.InfoException($"File {ConfigFile} occurred errors.", e.GetException());
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            lock (_fileWatcherLock)
            {
                _logger.Info($"File {e.Name} changed, try to reload configuration again...");

                var recurringJobInfos = Load().ToList();

                if (recurringJobInfos == null || recurringJobInfos.Count == 0) return;

                _builder.Build(() => recurringJobInfos);

                var serverFilter = FindServerFilter();

                if (serverFilter == null) return;

                foreach (var recurringJobInfo in recurringJobInfos)
                {
                    if (!recurringJobInfo.Enable
                        || recurringJobInfo.ExtendedData == null
                        || recurringJobInfo.ExtendedData.Count == 0)
                    {
                        RecurringJobInfo job = null;
                        serverFilter.RecurringJobInfos.TryRemove(recurringJobInfo.ToString(), out job);
                        continue;
                    }
                    serverFilter.RecurringJobInfos.AddOrUpdate(
                         recurringJobInfo.ToString(),
                         recurringJobInfo,
                         (key, oldValue) => (recurringJobInfo != oldValue) ? recurringJobInfo : oldValue);
                }
            }
        }

        private ExtendedDataJobFilter FindServerFilter()
        {
            var jobFilter = GlobalJobFilters.Filters.FirstOrDefault(x => x.Instance.GetType() == typeof(ExtendedDataJobFilter));

            return jobFilter.Instance as ExtendedDataJobFilter;
        }

        /// <summary>
		/// Initializes a new <see cref="IConfigurationProvider"/>
		/// </summary>
		/// <param name="builder">The builder for <see cref="IRecurringJobBuilder"/>.</param>
		/// <param name="configFile">The source settings file.</param>
		/// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
		public FileConfigurationProvider(IRecurringJobBuilder builder, string configFile, bool reloadOnChange = true)
        {
            _builder = builder;

            ConfigFile = configFile;

            if (!File.Exists(configFile))
                throw new FileNotFoundException($"The json file {configFile} does not exist.");

            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(configFile), Path.GetFileName(configFile));

            _fileWatcher.EnableRaisingEvents = reloadOnChange;
            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Error += OnError;
        }

        /// <summary>
		/// Reads from config file.
		/// </summary>
		/// <returns>The string content reading from file.</returns>
		protected virtual string ReadFromFile()
        {
            if (!File.Exists(ConfigFile))
                throw new FileNotFoundException($"The json file {ConfigFile} does not exist.");

            var content = string.Empty;

            for (int i = 0; i < NumberOfRetries; ++i)
            {
                try
                {
                    // Do stuff with file  
                    using (var file = new FileStream(ConfigFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                    using (StreamReader reader = new StreamReader(file))
                        content = reader.ReadToEnd();

                    break; // When done we can break loop
                }
                catch (Exception ex) when (
                ex is IOException ||
                ex is SecurityException ||
                ex is UnauthorizedAccessException)
                {
                    // Swallow the exception.
                    _logger.DebugException($"read file {ConfigFile} error.", ex);

                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw;

                    Thread.Sleep(DelayOnRetry);
                }
            }

            return content;
        }
    }
}
