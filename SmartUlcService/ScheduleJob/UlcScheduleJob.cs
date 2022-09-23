using Quartz;
using Quartz.Impl;
using SmartUlcService.NLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUlcService.ScheduleJob
{
  public class UlcScheduleJob
  {
    ISchedulerFactory __schedulerFactory = null;
    IScheduler __scheduler = null;

   
    public UlcScheduleJob(string schedule)
    {
     __schedulerFactory = new StdSchedulerFactory();
      __scheduler = __schedulerFactory.GetScheduler();
      IJobDetail jobDetail = JobBuilder.Create<SmartUlcSrv>()
      .WithIdentity("TestJob")
      .Build();
      ITrigger trigger = TriggerBuilder.Create()
          .ForJob(jobDetail)
          .WithCronSchedule(schedule)//"* * * * * ?"
          .WithIdentity("TestTrigger")
          .StartNow()
          .Build();
      __scheduler.ScheduleJob(jobDetail, trigger);
    }
    public void Start()
    {
      __scheduler.Start();
    }


    //internal class SatellitePaymentGenerationJob : IJob
    //{
    //  public void Execute(IJobExecutionContext context)
    //  {
    //    UlcSrvLog.Logger.Info("hello");
    //    //Console.WriteLine("test");
    //  }
    //}
  }

 
}


