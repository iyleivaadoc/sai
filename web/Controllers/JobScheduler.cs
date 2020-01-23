using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace web.Controllers
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobNotificacionDirectores>().Build();
            IJobDetail job2 = JobBuilder.Create<JobNotificacionEtapas>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithCronSchedule("0 42 10 ? * *")
            .Build();

            ITrigger trigger2 = TriggerBuilder.Create()
            .WithIdentity("trigger2", "group1")
            .StartNow()
            .WithCronSchedule("0 56 10 ? * *")
            .Build();

            //    .WithSimpleSchedule(x => x
            //.WithIntervalInMinutes(10)
            //.RepeatForever())

            scheduler.ScheduleJob(job, trigger);
            scheduler.ScheduleJob(job2, trigger2);
        }
    }
}