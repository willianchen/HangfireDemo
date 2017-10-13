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
    /// 类名：IRecurringJob.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：IRecurringJob
    /// 创建标识：cml 2017/10/13 14:44:08
    /// </summary>
    public interface IRecurringJob
    {
        /// <summary>
		/// Execute the <see cref="RecurringJob"/>.
		/// </summary>
		/// <param name="context">The context to <see cref="PerformContext"/>.</param>
		void Execute(PerformContext context);
    }
}
