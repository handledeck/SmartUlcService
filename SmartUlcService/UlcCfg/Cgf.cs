using InterUlc.Db;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static InterUlc.Logs.EnumLogs;

namespace InterUlc.UlcCfg
{
 


   [ServiceStack.DataAnnotations.Alias("main_ctrldata")]
  public class UlcCfg
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    public int id { get; set; }
    [ServiceStack.DataAnnotations.Required]
    public DateTime current_time
    {
      get;
      set;
    }
    [ServiceStack.DataAnnotations.Required]
    [ForeignKey(typeof(MainInfo))]
    public int ctrl_id { get; set; }
    public string APN { get; set; }
    public string USER { get; set; }
    public string PASS { get; set; }
    public long? DT { get; set; }
    public ushort? DEBOUNCE { get; set; }
    public byte? DEBUG { get; set; }
    public byte? EST { get; set; }
    public string IP { get; set; }
    public int? TCP { get; set; }
    public byte? TSEND { get; set; }
    public int? AIN { get; set; }
    public int? DIN { get; set; }
    public int? DOUT { get; set; }
    public int? DOOR { get; set; }
    public string LATIT { get; set; }
    public string LONGIT { get; set; }
    public byte? TZ { get; set; }
    public int? CDIN { get; set; }
    public int? CDOUT { get; set; }
    public string CAIN { get; set; }
    public long? SRISE { get; set; }
    public long? SSET { get; set; }
    public byte? SIM { get; set; }
    public byte? GSM { get; set; }
    public byte? GPRS { get; set; }
    public int? SIGNAL { get; set; }
    public int? DBZ { get; set; }
    public string IPOWN { get; set; }
    public string SCHED { get; set; }
    public byte? RAS { get; set; }
    public string VER { get; set; }
    public string SERIAL { get; set; }
    public byte? NUM { get; set; }
    public string SVERS { get; set; }
    public string TECHN { get; set; }
    public long? FMW { get; set; }
    public string TMSET { get; set; }
    public string IPP { get; set; }
    public long? PREP { get; set; }
    public long? IMEI { get; set; }
    public LOG_LVL? LOGSLVL { get; set; }
    public int? BRG { get; set; }
    public string CORV { get; set; }
    public long? TRAFC { get; set; }
    public int? DeviceType { get; set; }

    


    public static void CreateCurrentDataTable(string connString) {
      try
      {
        var dbFactory = new OrmLiteConnectionFactory(connString, PostgreSqlDialect.Provider);
        //string.Format(/*"Host={0};Username={1};Password={2};Database=ctrl_mon_dev",
        //   "10.32.18.38", "postgres", "pgp@ssdb"),*/connString, PostgreSqlDialect.Provider);

        using (var db = dbFactory.Open())
        {
          if (db.CreateTableIfNotExists<UlcCfg>())
          {
            //var cmd = db.OpenCommand();
            //cmd.CommandText = "ALTER TABLE public.main_ctrldata ADD CONSTRAINT main_ctrldata_fk FOREIGN KEY (ctrl_id) REFERENCES main_ctrlinfo(id) ON DELETE CASCADE";
            //cmd.ExecuteNonQuery();
          }
          if (db.CreateTableIfNotExists<MainAdminLog>())
          {
            //var cmd = db.OpenCommand();
            //cmd.CommandText = "ALTER TABLE public.main_ctrldata ADD CONSTRAINT main_ctrldata_fk FOREIGN KEY (ctrl_id) REFERENCES main_ctrlinfo(id) ON DELETE CASCADE";
            //cmd.ExecuteNonQuery();
          }
          //List<MainNodes> ss=db.Select<MainNodes>();
          //db.Insert<UlcCfg>(new UlcCfg() { current_time = DateTime.UtcNow, ctrl_id = 44, LOGSLVL = LOG_LVL.logFATAL });
          //List< UlcCfg> lst= db.Select<UlcCfg>(curtime=>curtime.current_time<DateTime.Now);
        }
      }
      catch (Exception e) {
        int x = 0;
      }
    }

    public bool GetExtarctRvpConfig(string indata)
    {
      bool readed = false;

      try
      {
        if (indata.StartsWith("CONFIG"))
        {
          indata = indata.Substring(7);
        }
        string[] cdata = indata.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (cdata.Length > 0)
        {
          foreach (var item in cdata)
          {
            string[] pdata = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (pdata.Length > 1)
            {


              if (pdata[0] == "APN")
                this.APN = pdata[1];
              else if (pdata[0] == "USER")
                this.USER = pdata[1];
              else if (pdata[0] == "PASS")
                this.PASS = pdata[1];
              else if (pdata[0] == "DT")
                this.DT = uint.Parse(pdata[1]);
              else if (pdata[0] == "DEBOUNCE")
                this.DEBOUNCE = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DEBUG")
                this.DEBUG = byte.Parse(pdata[1]);
              else if (pdata[0] == "EST")
                this.EST = byte.Parse(pdata[1]);
              else if (pdata[0] == "IP")
                this.IP = pdata[1];
              else if (pdata[0] == "TCP")
                this.TCP = int.Parse(pdata[1]);
              else if (pdata[0] == "TSEND")
                this.TSEND = byte.Parse(pdata[1]);
              else if (pdata[0] == "AIN")
                this.AIN = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DIN")
                this.DIN = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DOUT")
                this.DOUT = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DOOR")
                this.DOOR = ushort.Parse(pdata[1]);
              else if (pdata[0] == "LATIT")
                this.LATIT = pdata[1];
              else if (pdata[0] == "LONGIT")
                this.LONGIT = pdata[1];
              else if (pdata[0] == "TZ")
                this.TZ = byte.Parse(pdata[1]);
              else if (pdata[0] == "CDIN")
                this.CDIN = ushort.Parse(pdata[1]);
              else if (pdata[0] == "CDOUT")
                this.CDOUT = ushort.Parse(pdata[1]);
              else if (pdata[0] == "CAIN")
                this.CAIN = pdata[1];
              else if (pdata[0] == "SRISE")
                this.SRISE = int.Parse(pdata[1]);
              else if (pdata[0] == "SSET")
                this.SSET = int.Parse(pdata[1]);
              else if (pdata[0] == "SIM")
                this.SIM = byte.Parse(pdata[1]);
              else if (pdata[0] == "GSM")
                this.GSM = byte.Parse(pdata[1]);
              else if (pdata[0] == "GPRS")
                this.GPRS = byte.Parse(pdata[1]);
              else if (pdata[0] == "SIGNAL")
                this.SIGNAL = short.Parse(pdata[1]);
              else if (pdata[0] == "DBZ")
                this.DBZ = short.Parse(pdata[1]);
              else if (pdata[0] == "IPOWN")
                this.IPOWN = pdata[1];
              else if (pdata[0] == "SCHED")
                this.SCHED = pdata[1];
              else if (pdata[0] == "RAS")
                this.RAS = byte.Parse(pdata[1]);
              else if (pdata[0] == "VER")
                this.VER = pdata[1];
              else if (pdata[0] == "SERIAL")
                this.SERIAL = pdata[1];
              else if (pdata[0] == "NUM")
                this.NUM = byte.Parse(pdata[1]);
              else if (pdata[0] == "SVERS")
                this.SVERS = pdata[1];//.TrimEnd(new char[] { '\r', '\n' });
              else if (pdata[0] == "TECHN")
                this.TECHN = pdata[1];
              else if (pdata[0] == "FMW")
                this.FMW = uint.Parse(pdata[1]);
              else if (pdata[0] == "TMSET") {
                if (pdata.Length > 2)
                {
                  this.TMSET = (pdata[1] + ":" + pdata[2]).Trim(new char[] { '\r','\n'});
                }
                else {
                  this.TMSET = string.Empty;
                }
              }
                
              else if (pdata[0] == "IPP")
                this.IPP = pdata[1];
              else if (pdata[0] == "PERP")
                this.PREP = int.Parse(pdata[1]);
              else if (pdata[0] == "IMEI")
                this.IMEI = long.Parse(pdata[1]);
              else if (pdata[0] == "LOGSLVL")
                this.LOGSLVL = (LOG_LVL)byte.Parse(pdata[1]);
              else if (pdata[0] == "BRG")
                this.BRG = byte.Parse(pdata[1]);
              else if (pdata[0] == "CORV")
                this.CORV = pdata[1];
              else if (pdata[0] == "TRAFC")
                this.TRAFC = long.Parse(pdata[1]);
            }
          }
          readed = true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception exp)
      {
        return false;
      }
      return readed;
    }
  }


  public class UlcEvent
  {

    public DateTime Date { get; set; }
    public LOG_LVL EventLevel { get; set; }
    public string Msg { get; set; }
  }
}

