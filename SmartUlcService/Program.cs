using IniParser.Parser;
using NLog;
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
    /// <summary>
    /// Главная точка входа для приложения.
    /// </summary>
    static readonly NLog.Logger __logger = NLog.LogManager.GetCurrentClassLogger();
    static string __ini_file_srv = "UlcSrvSettings.ini";
    static string __cfg_file_log ="ulcSrvLogs.txt";
    static string __service_path;

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    // create a static logger field
    // private static Logger logger = LogManager.GetCurrentClassLogger();
    static void Main()
    {
      var config = new NLog.Config.LoggingConfiguration();
      //DateTime dt = DateTime.Now.ToString();
      // Targets where to log to: File and Console
     
      var logfile = new NLog.Targets.FileTarget("logfile")
      {
        FileName = __cfg_file_log,
        ArchiveEvery = NLog.Targets.FileArchivePeriod.Minute,
        ArchiveFileName = "${basedir}/logs/${date:format=yyyy-MM-dd HH-mm-ss}.txt",
         MaxArchiveFiles=3
      };
      var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

      // Rules for mapping loggers to targets            
      config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
      config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

      // Apply config           
      NLog.LogManager.Configuration = config;
      while (true)
      {
        Logger.Info("Hello world");
        Thread.Sleep(800);
      }
      
      //var config =new NLog.Config.LoggingConfiguration();
      //var logFile = new NLog.Targets.FileTarget(__cfg_file_log) { ArchiveEvery= NLog.Targets.FileArchivePeriod.Day};
      //config.AddRuleForAllLevels(logFile);
      //NLog.LogManager.Configuration = config;
      //__service_path = AssemblyDirectory + "\\" + __ini_file_srv;
      //Logger.Info("11111");
      //ReadIniFile();

      //ServiceBase[] ServicesToRun;
      //ServicesToRun = new ServiceBase[]
      //{
      //          new SmUlcSrv()
      //};
      //ServiceBase.Run(ServicesToRun);
    }


    static void WriteIniSrv(string pathFPath)
    {
      StreamWriter s = new StreamWriter(pathFPath, false);
      IniParser.Model.SectionData db = new IniParser.Model.SectionData("DB");
      IniParser.Model.IniData iniDb = new IniParser.Model.IniData();
      db.Comments.Add("test to connection");
      db.Keys.AddKey("ip", "127.0.0.1");
      db.Keys.AddKey("port", "5432");
      iniDb.Sections.Add(db);
      IniParser.Model.SectionData dbUser = new IniParser.Model.SectionData("DBUser");
      IniParser.Model.IniData iniUser = new IniParser.Model.IniData();
      dbUser.Comments.Add("section for user");
      dbUser.Keys.AddKey("user", "postgres");
      dbUser.Keys.AddKey("password", "root");
      //dbUser.Keys.AddKey("port", "5432");

      iniDb.Sections.Add(dbUser);
      IniParser.FileIniDataParser fileIniDataParser = new IniParser.FileIniDataParser();
      fileIniDataParser.WriteData(s, iniDb);
      s.Flush();
      s.Close();
    }

    static void ReadIniFile()
    {
      try
      {
        bool fExt = File.Exists(__service_path);
        
        if (!fExt)
        {
          __logger.Error("aaaaaa!!");
          WriteIniSrv(__service_path);
        }
        else
        {
          StreamReader s = new StreamReader(__service_path, false);
          IniDataParser p = new IniDataParser();
          IniParser.FileIniDataParser fileIniDataParser = new IniParser.FileIniDataParser();
          var iData = fileIniDataParser.ReadData(s);
          var ip = iData["DB"].GetKeyData("ip").Value;
          var port= iData["DB"].GetKeyData("port").Value;
          var user = iData["DBUser"].GetKeyData("user").Value;
          var password = iData["DBUser"].GetKeyData("password").Value;
        }
      }
      catch (Exception exc)
      {

        throw;
      }
    }

    public static string AssemblyDirectory
    {
      get
      {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
      }
    }
  }
}
