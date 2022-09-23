using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUlcService.NLogs
{
  public class UlcSrvLog
  {
    public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static void InitUlcSrvLog()
    {
      var config = new NLog.Config.LoggingConfiguration();
      var logfile = new NLog.Targets.FileTarget("logfile")
      {
        FileName = "${basedir}/logs/now.log",
        ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
        ArchiveFileName = "${basedir}/logs/${date:format=yyyy-MM-dd HH-mm-ss}.log",
        MaxArchiveFiles = 3,
        MaxArchiveDays=5
      };
      var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
      config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
      config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
      NLog.LogManager.Configuration = config;
      Logger.Factory.Configuration = config;
      
    }
  }
}
