using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SmartUlcService
{
  static class Program
  {
    /// <summary>
    /// Главная точка входа для приложения.
    /// </summary>
    
    static string __file = "UlcSrvSettings.ini";
    static string __service_path;
    static void Main()
    {
      __service_path = AssemblyDirectory + "\\" + __file;
      //__workingDirectory = Environment.CurrentDirectory;
      // __service_path = string.Format("{0}\\{1}", __workingDirectory, __file);
      ReadIniFile();

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
      dbUser.Keys.AddKey("password", "");
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
