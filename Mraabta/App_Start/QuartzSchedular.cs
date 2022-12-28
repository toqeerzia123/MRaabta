using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MRaabta.App_Start
{
    public class QuartzSchedular
    {
        public static void Start()
        {
            var minutes = int.Parse(ConfigurationManager.AppSettings["minutes"]);
            var runemail = int.Parse(ConfigurationManager.AppSettings["runemail"]);
            var runsms = int.Parse(ConfigurationManager.AppSettings["runsms"]);

            var path = HttpContext.Current.Server.MapPath("~/Quartz.txt");
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            IJobDetail job1 = JobBuilder.Create<EmailJob>().WithIdentity("Job1").UsingJobData("path", path).Build();
            IJobDetail job2 = JobBuilder.Create<SMSJob>().WithIdentity("Job2").UsingJobData("path", path).Build();

            ITrigger trigger1 = TriggerBuilder.Create()
                .WithIdentity("IDGJob1")
                .WithPriority(2)
                .StartAt(DateTime.Now.AddMinutes(10))
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(minutes).RepeatForever())
                .Build();

            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("IDGJob2")
                .WithPriority(1)
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(15).RepeatForever())
                .Build();

            if (runemail == 1)
                scheduler.ScheduleJob(job1, trigger1);

            if (runsms == 1)
                scheduler.ScheduleJob(job2, trigger2);
        }
    }
}