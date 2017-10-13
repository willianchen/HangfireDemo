using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions.Configuration
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：IConfigurationProvider.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：IConfigurationProvider
    /// 创建标识：cml 2017/10/13 14:42:05
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
		/// Loads configuration values from the source represented by this <see cref="IConfigurationProvider"/>.
		/// </summary>
		/// <returns>The list of <see cref="RecurringJobInfo"/>.</returns>
		IEnumerable<RecurringJobInfo> Load();
    }
}
