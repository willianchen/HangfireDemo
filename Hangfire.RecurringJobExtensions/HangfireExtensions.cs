﻿using Hangfire.RecurringJobExtensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：HangfireExtensions.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：HangfireExtensions
    /// 创建标识：cml 2017/10/13 16:12:51
    /// </summary>
    public static class HangfireExtensions
    {
        /// <summary>
        /// Build <see cref="RecurringJob"/> automatically within specified interface or class.
        /// </summary>
        /// <param name="configuration"><see cref="IGlobalConfiguration"/></param>
        /// <param name="types">Specified interface or class</param>
        /// <returns><see cref="IGlobalConfiguration"/></returns>
        public static IGlobalConfiguration UseRecurringJob(this IGlobalConfiguration configuration, params Type[] types)
        {
            return UseRecurringJob(configuration, () => types);
        }

        /// <summary>
        /// Build <see cref="RecurringJob"/> automatically within specified interface or class.
        /// </summary>
        /// <param name="configuration"><see cref="IGlobalConfiguration"/></param>
        /// <param name="typesProvider">The provider to get specified interfaces or class.</param>
        /// <returns><see cref="IGlobalConfiguration"/></returns>
        public static IGlobalConfiguration UseRecurringJob(this IGlobalConfiguration configuration, Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            IRecurringJobBuilder builder = new RecurringJobBuilder(new RecurringJobRegistry());

            builder.Build(typesProvider);

            return configuration;
        }
        /// <summary>
        /// Build <see cref="RecurringJob"/> automatically by using a JSON configuration
        /// </summary>
        /// <param name="configuration"><see cref="IGlobalConfiguration"/>.</param>
        /// <param name="jsonFile">Json file for <see cref="RecurringJob"/> configuration.</param>
        /// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
        /// <returns><see cref="IGlobalConfiguration"/></returns>
        public static IGlobalConfiguration UseRecurringJob(this IGlobalConfiguration configuration, string jsonFile, bool reloadOnChange = true)
        {
            if (jsonFile == null) throw new ArgumentNullException(nameof(jsonFile));

            var configFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                //AppContext.BaseDirectory,
                jsonFile);


            if (!File.Exists(configFile)) throw new FileNotFoundException($"The json file {configFile} does not exist.");

            IRecurringJobBuilder builder = new RecurringJobBuilder(new RecurringJobRegistry());

            IConfigurationProvider provider = new JsonConfigurationProvider(builder, configFile, reloadOnChange);

            var jobInfos = provider.Load().ToList();

            builder.Build(() => jobInfos);

            GlobalConfiguration.Configuration.UseFilter(new ExtendedDataJobFilter(jobInfos));

            return configuration;
        }

        /// <summary>
        /// Build <see cref="RecurringJob"/> automatically with <seealso cref="IConfigurationProvider"/>.
        /// </summary>
        /// <param name="configuration"><see cref="IGlobalConfiguration"/>.</param>
        /// <param name="provider"><see cref="IConfigurationProvider"/></param>
        /// <returns><see cref="IGlobalConfiguration"/>.</returns>
        public static IGlobalConfiguration UseRecurringJob(this IGlobalConfiguration configuration, IConfigurationProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            IRecurringJobBuilder builder = new RecurringJobBuilder(new RecurringJobRegistry());

            var jobInfos = provider.Load().ToList();

            builder.Build(() => jobInfos);

            GlobalConfiguration.Configuration.UseFilter(new ExtendedDataJobFilter(jobInfos));

            return configuration;
        }
    }
}
