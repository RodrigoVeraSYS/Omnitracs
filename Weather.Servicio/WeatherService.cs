using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Weather.Servicio
{
    public partial class WeatherService : ServiceBase
    {
        IScheduler sched;
        public WeatherService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.WriteEntry("Iniciando el servicio de alertas");
            try
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                sched = schedFact.GetScheduler();
               


                IJobDetail job = JobBuilder.Create<WeatherJob>()
                   .WithIdentity("WeatherJob", "Weathergroup")
                   .StoreDurably()
                   .Build();

             
                sched.AddJob(job, true);

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("Weatherrigger", "WeatherGroup")
                    .StartNow()
                    .UsingJobData("Tipo", "Intervalo")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["MinuteInterval"].ToString()))
                        .RepeatForever())
                          .ForJob(job)
                    .Build();

                ITrigger triggerDiario = TriggerBuilder.Create()
                    .WithIdentity("WeatherDiarioTrigger", "WeatherDiarioGroup")
                    .StartNow()
                    .UsingJobData("Tipo", "Diario")
                      .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(5,0))
                      .ForJob(job)
                    .Build();

                sched.ScheduleJob(triggerDiario);
                sched.ScheduleJob(trigger);

                sched.Start();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }


        public void TestStartupAndStop()
        {
            Logger.WriteEntry("Starting Service Weather");
            try
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                sched = schedFact.GetScheduler();
               
                IJobDetail job = JobBuilder.Create<WeatherJob>()
                    .WithIdentity("WeatherJob", "Weathergroup")
                    .StoreDurably()
                    .Build();

                sched.AddJob(job,true);

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("Weatherrigger", "WeatherGroup")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["MinuteInterval"].ToString()))
                        .RepeatForever())
                          .ForJob(job)
                    .Build();

     
                sched.ScheduleJob( trigger);

                sched.Start();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected override void OnStop()
        {
            Logger.WriteEntry("Stoping Service Weather");
            if (sched != null)
            {
                sched.Shutdown();
            }
        }
    }
}
