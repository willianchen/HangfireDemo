﻿using Hangfire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HangfireDemo.Controllers
{
    public class MessageController : ApiController
    {
        /// <summary>
        /// 这个是用来发送消息的静态方法
        /// </summary>
        /// <param name="message"></param>
        public static void Send(string message)
        {
            EventLog.WriteEntry("EventSystem", string.Format("这是由Hangfire后台任务发送的消息:{0},时间为:{1}", message, DateTime.Now));
        }

        public IHttpActionResult Post(string content)
        {
            //这里可以做一些业务判断或操作

            //然后需要推送的时候，调用下面的方法即可
            BackgroundJob.Enqueue(() => Send(content));

            //最后返回（这里是立即返回，不会阻塞）
            return Ok();
        }

        public IHttpActionResult StartRecurringJob(string con)
        {
            //这里可以做一些业务判断或操作

            RecurringJob.AddOrUpdate(() => Send(con),
                                            Cron.Minutely);

            //最后返回（这里是立即返回，不会阻塞）
            return Ok();
        }

    }
}
