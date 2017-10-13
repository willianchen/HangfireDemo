using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：IRecurringJobBuilder.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：IRecurringJobBuilder
    /// 创建标识：cml 2017/10/13 15:04:16
    /// </summary>
    public interface IRecurringJobBuilder
    {
        /// Create <see cref="RecurringJob"/> with the provider for specified interface or class.
		/// </summary>
		/// <param name="typesProvider">Specified interface or class</param>
		void Build(Func<IEnumerable<Type>> typesProvider);
        /// <summary>
        /// Create <see cref="RecurringJob"/> with the provider for specified list <see cref="RecurringJobInfo"/>.
        /// </summary>
        /// <param name="recurringJobInfoProvider">The provider to get <see cref="RecurringJobInfo"/> list/></param>
        void Build(Func<IEnumerable<RecurringJobInfo>> recurringJobInfoProvider);
    }
}
