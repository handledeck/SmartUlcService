using IniParser.Parser;
using NLog;
using Quartz;
using Quartz.Impl;
using SmartUlcService.ini;
using SmartUlcService.NLogs;
using SmartUlcService.ScheduleJob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartUlcService
{
  static class Program
  {
    
    // private static Logger logger = LogManager.GetCurrentClassLogger();
    static void Main()
    {
      UlcSrvLog.InitUlcSrvLog();
      ConfigIni configIni = new ConfigIni();
      UlcScheduleJob ulcScheduleJob = new UlcScheduleJob(configIni.Scheduler);
      ulcScheduleJob.Start();
    }

  }

 

}
