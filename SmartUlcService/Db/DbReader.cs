using InterUlc.CurCfg;
using InterUlc.Logs;
using InterUlc.UlcCfg;
using Npgsql;
using NpgsqlTypes;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace InterUlc.Db
{
  public delegate void ItemRunComplite(Task tsk);
  public class DbReqestNotTrue {

    public int ID { get; set; }
    public string IPAddres { get; set; }
    public string Message { get; set; }

    public int TypeDevice { get; set; }
    public List<Log> Logs { get; set; }

    public ItemRunComplite EvtItemRunComplite { get; set; }
    public Task OwnerTask { get; set; }

    public List<byte[]> Binary { get; set; }

  }

  public class Node
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Node> Nodes;
    public string IP { get; set; }
    public string Phone { get; set; }
    public CurrentCfg Cfg { get; set; }
    public List<Log> Logs { get; set; }
    public string Message  { get; set; }
    public int Type { get; set; }
    public override string ToString()
    {
      return string.Format("id:{0} name:{1} count:{2}", Id, Name, Nodes.Count);
    }
  }

  public class DbReader
  {
    string dBIpAddress { get; set; }
    string DbUserName { get; set; }
    string DbPassword { get; set; }
    public List<Node> Nodes
    {
      get
      {
        return __nodes;
      }
    }
    public string __connection = string.Empty;
    List<Node> __nodes = null;
    NpgsqlConnection __dbConnection = null;

    public DbReader(string dBIpAddress, string DbUserName, string DbPassword)
    {
      this.dBIpAddress = dBIpAddress;
      this.DbUserName = DbUserName;
      this.DbPassword = DbPassword;
      __nodes = new List<Node>();
      this.__connection = string.Format("Host={0};Username={1};Password={2};Database=ctrl_mon_dev",
        this.dBIpAddress, this.DbUserName, this.DbPassword);
      this.__dbConnection = new NpgsqlConnection(this.__connection);
    }


   public  void ReadStatistic(string connString)
    {
      try
      {
        var dbFactory = new OrmLiteConnectionFactory(connString, PostgreSqlDialect.Provider);
        //string.Format(/*"Host={0};Username={1};Password={2};Database=ctrl_mon_dev",
        //   "10.32.18.38", "postgres", "pgp@ssdb"),*/connString, PostgreSqlDialect.Provider);

        using (var db = dbFactory.Open())
        {
          using (var cmd = db.OpenCommand())
          {

            var dat=cmd.ExecLongScalar("select count(*) from main_ctrlinfo");
            dat = cmd.ExecLongScalar("select count(*) from main_ctrlinfo mi where mi.unit_type_id=1");
            dat=cmd.ExecLongScalar("select count(*) from main_ctrlinfo mi where mi.unit_type_id=0");
            //не работает ком порт
            dat = cmd.ExecLongScalar(string.Format("select * from main_ctrldata where device_type =1 and((cdin >> 7) = 0) and \"current_time\" >'{0}'",
              DateTime.UtcNow.ToString("yyyy-MM-dd")));
            
            List<MainInfo> lst = db.Select<MainInfo>("select count(*) from main_ctrlinfo mc where mc.unit_type_id =0");

          }
        }
      }
      catch (Exception e)
      {
        int x = 0;
      }
    }

    public void InsertMsgConfig(string message,int ctrlID)
    {
      try
      {
        this.__dbConnection.Open();
        var sql = "INSERT INTO main_ctrlcurrent(\"current_time\", body, ctrl_id) " +
            "VALUES(@time, @body, @ctrl_id)";
        var cmd = new NpgsqlCommand(sql, this.__dbConnection);
        cmd.Parameters.AddWithValue("time", DateTime.Now);
        cmd.Parameters.AddWithValue("body", message);
        cmd.Parameters.AddWithValue("ctrl_id", ctrlID);
        cmd.ExecuteNonQuery();
        this.__dbConnection.Close();
      }
      catch
      {
      }

      finally
      {
        if (this.__dbConnection.State == System.Data.ConnectionState.Open)
        {
          this.__dbConnection.Close();
        }
      }
    }


    void InsertDataTable(DbReqestNotTrue item, System.Data.IDbConnection conn) {
      try
      {
        UlcCfg.UlcCfg ulcCfg = new UlcCfg.UlcCfg();
        bool res = ulcCfg.GetExtarctRvpConfig(item.Message);
        if (res)
        {
          ulcCfg.ctrl_id = item.ID;
          ulcCfg.current_time = DateTime.UtcNow;
          ulcCfg.DeviceType = item.TypeDevice;
          conn.Insert<UlcCfg.UlcCfg>(ulcCfg);
        }
      }
      catch (Exception e) {
        int x = 0;
      }
    }

    public bool CheckForRecordInDb(int ctrId, out int idCurrent, out int idData)
    {
      idCurrent = -1;
      idData = -1;
      try
      {
        var dbFactory = new ServiceStack.OrmLite.OrmLiteConnectionFactory(
          __connection, PostgreSqlDialect.Provider);
        using (var db = dbFactory.Open())
        {
          string sql = string.Format("select * from main_ctrlcurrent mc where mc.ctrl_id ={0} and mc.\"current_time\" >'{1}'",
            ctrId, DateTime.Now.ToString("yyyy-MM-dd"));
          List<MainCurrent> lstCurrnet = db.Select<MainCurrent>(sql);
          if (lstCurrnet.Count > 0)
          {
            idCurrent = lstCurrnet[0].id;
          }
          sql = string.Format("select * from main_ctrldata md where md.ctrl_id ={0} and md.\"current_time\" >'{1}'",
          ctrId, DateTime.Now.ToString("yyyy-MM-dd"));
          List<MainInfo> lstData = db.Select<MainInfo>(sql);
          if (lstCurrnet.Count > 0)
          {
            idData = lstData[0].id;
          }

          return true;
        }
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static JsonSerializerOptions GetSerializeOption()
    {
      JsonSerializerOptions options = new JsonSerializerOptions
      {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.BasicLatin,
         UnicodeRanges.Cyrillic),
        WriteIndented = true
      };
      return options;
    }

    

    public string GetDbObjectPath(int id, IDbConnection connection) {
      string msg = string.Empty;
      string sql = string.Format("select mn.id, mn.\"name\" as tp ,mn2.\"name\" as res,mn3.\"name\" as fes from main_nodes mn "+ 
      "right join main_nodes mn2 on mn.parent_id = mn2.id "+
      "right join main_nodes mn3 on mn2.parent_id = mn3.id "+
      "where mn.id = {0}",id);
      List<DbLogs> lst= connection.Select<DbLogs>(sql);
      if (lst.Count > 0) {
        //msg = System.Text.Json.JsonSerializer.Serialize(lst[0], typeof(DbLogs), GetSerializeOption());
        msg = string.Format("{0}/{1}/{2}",lst[0].Fes,lst[0].Res,lst[0].Tp);
      }
      return msg;
    }

    public OrmDbConfig GetLastRecordById(IDbConnection connection,int id) {
      string sql = String.Format("SELECT * FROM main_ctrldata mc " +
                  "WHERE ID = (SELECT MAX(ID) FROM main_ctrldata mc1 where mc1.ctrl_id = {0})",id);
      List<OrmDbConfig> lstMax = connection.Select<OrmDbConfig>(sql);
      if (lstMax.Count > 0)
      {
        return lstMax[0];
      }
      else {
        return null;
      }

    }

    string GetShortImei(string imei)
    {
      return imei.Substring(imei.Length - 7, imei.Length - 8);
    }

    public void CheckDeviceIMEI(IDbConnection connection, UlcCfg.UlcCfg ulcCfg)
    {
      string sql = string.Format("SELECT * FROM main_ctrldata mc " +
            "WHERE id = (" +
            "SELECT max(id) FROM main_ctrldata mc2 where mc2.ctrl_id = {0})", ulcCfg.ctrl_id);
      List<UlcCfg.UlcCfg> lstMax = connection.Select<UlcCfg.UlcCfg>(sql);
      if (lstMax.Count > 0)
      {
        string msg = GetDbObjectPath(lstMax[0].ctrl_id, connection);

        if (lstMax[0].IMEI != ulcCfg.IMEI)
        {
          
          int en = (int)EnLogEvent.ChangeIMEI;
          OrmDbConfig ormDbConfig= GetLastRecordById(connection, ulcCfg.ctrl_id);
          MainLogs mainLogs = new MainLogs()
          {
            current_time = DateTime.Now,
            id_user = 0,
            usr_name = "служба опроса",
            message = string.Format("{0}(dt:{1} imei:{2}=>{3} rs:{4})", msg, lstMax[0].current_time.ToString("dd.MM.yyyy HH:mm"),
           GetShortImei(lstMax[0].IMEI.ToString()), GetShortImei(ulcCfg.IMEI.ToString()), (lstMax[0].CDIN >> 7).ToString()),
            log_event = en,
            host_from = Dns.GetHostEntry(Dns.GetHostName()).HostName
          };
          connection.Insert<MainLogs>(mainLogs);
        }
        else if ((DateTime.Now - lstMax[0].current_time).TotalDays > 2)
        {
          int en = (int)EnLogEvent.CHANGE_NET_STATE;
          MainLogs mainLogs = new MainLogs()
          {
            current_time = DateTime.Now,
            id_user = 0,
            usr_name = "служба опроса",
            message = string.Format("dt:{0}-{1}", ulcCfg.current_time.ToString("dd.MM.yyyy HH:mm"),msg),
            log_event = en,
            host_from = Dns.GetHostEntry(Dns.GetHostName()).HostName
          };
          connection.Insert<MainLogs>(mainLogs);
        }
      }
    }

    public void InsertCfgMsg(List<DbReqestNotTrue> lstDbReqestNotTrue)
    {
      int idCurrent = -1;
      int idData = -1;
      try
      {
        var dbFactory = new OrmLiteConnectionFactory(__connection, PostgreSqlDialect.Provider);
        using (var db = dbFactory.Open())
        {
          
          int i = 0;
          foreach (var item in lstDbReqestNotTrue)
          {
            try
            {
              if (!string.IsNullOrEmpty(item.Message))
              {
                UlcCfg.UlcCfg ulcCfg = new UlcCfg.UlcCfg();
                bool res = ulcCfg.GetExtarctRvpConfig(item.Message);
                if (res)
                {
                  try
                  {
                    CheckForRecordInDb(item.ID, out idCurrent, out idData);
                    if (idCurrent > -1)
                    {
                      int xx = db.Update<MainCurrent>(new MainCurrent[] {new MainCurrent()
                      { id= idCurrent,
                          ctrl_id= item.ID, 
                          body= item.Message,
                          current_time=DateTime.Now 
                        }
                      });
                      ulcCfg.id = idData;
                      ulcCfg.ctrl_id = item.ID;
                      ulcCfg.current_time = DateTime.Now;
                      ulcCfg.DeviceType = item.TypeDevice;
                      if (ulcCfg.DeviceType == 1)
                      {
                        CheckDeviceIMEI(db, ulcCfg);
                      }
                      xx = db.Update<UlcCfg.UlcCfg>(new UlcCfg.UlcCfg[] { ulcCfg });
                    }
                    else
                    {
                      db.Insert<MainCurrent>(new MainCurrent[] { new MainCurrent()
                      { ctrl_id=item.ID, body= item.Message, current_time=DateTime.Now } });
                      ulcCfg.ctrl_id = item.ID;
                      ulcCfg.current_time = DateTime.Now;
                      ulcCfg.DeviceType = item.TypeDevice;
                      if (ulcCfg.DeviceType == 1) {
                        CheckDeviceIMEI(db, ulcCfg);
                      }
                      db.Insert<UlcCfg.UlcCfg>(new UlcCfg.UlcCfg[] { ulcCfg });
                    }
                  }
                  catch(Exception exc)
                  {
                    int x = 0;
                  }
                }
              }
              else
              {
                Console.WriteLine("error parcer:{0}", ++i);
              }
            }
            catch (Exception e)
            {
              int x = 0;
            }
          }
        }
      }
      catch (Exception exp)
      {
        int x = 0;
      }

      finally
      {
        if (this.__dbConnection.State == System.Data.ConnectionState.Open)
        {
          this.__dbConnection.Close();
        }
      }
    }

    public void InsertCfgMsg(Node node)
    {
      try
      {
        this.__dbConnection.Open();
        var sql = "INSERT INTO main_ctrlcurrent(\"current_time\", body, ctrl_id) " +
            "VALUES(@time, @body, @ctrl_id)";
        var cmd = new NpgsqlCommand(sql, this.__dbConnection);
        cmd.Parameters.AddWithValue("time", DateTime.Now);
        cmd.Parameters.AddWithValue("body", node.Message);
        cmd.Parameters.AddWithValue("ctrl_id", node.Id);
        cmd.ExecuteNonQuery();
        this.__dbConnection.Close();
      }
      catch{
      }

      finally
      {
        if (this.__dbConnection.State == System.Data.ConnectionState.Open)
        {
          this.__dbConnection.Close();
        }
      }
    }

    public List<DbReqestNotTrue> ReadNotTrueItems(DateTime dt) {
      this.__dbConnection.Open();
      var sql = string.Format("select * FROM main_ctrlinfo mc left join main_ctrlcurrent ci on ci.ctrl_id = mc.id and ci.\"current_time\" > '{0}' " +
      "where ci.body isnull or ci.body = '' order by mc.unit_type_id",dt.ToString("yyyy-MM-dd"));
      var cmd = new NpgsqlCommand(sql, this.__dbConnection);
      List<DbReqestNotTrue> lstNotTrue = new List<DbReqestNotTrue>();
      try
      {
        int inc = 0;
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {

          int id = (int)dr[0];
          string ip = (string)dr[1];
          int typedev = (int)dr[4];
          //if (typedev == 1) {
          //  System.Diagnostics.Debug.WriteLine(++inc);
          //}
          lstNotTrue.Add(new DbReqestNotTrue() { ID = id, IPAddres = ip, TypeDevice=typedev });
          //if (lstNotTrue.Count > 20)
           // break;
        }
        return lstNotTrue;
      }
      catch
      {
        return null;

      }
      finally {
        this.__dbConnection.Close();
      } 
      //"INSERT INTO main_ctrlcurrent(\"current_time\", body, ctrl_id) " +
      //"VALUES(@time, @body, @ctrl_id)";

      
    }

    internal void InsertLogMsg(Node node)
    {
      int xx = 0; ;
      try
      {
        if (node.Logs != null)
        {
          //var con = new NpgsqlConnection(this.__connection);
          this.__dbConnection.Open();
          foreach (var item in node.Logs)
          {
            string evt = Log.ConvertToString(item);
            var sql = "Select * from main_ctrlevent where event_time=@etime and event_type=@etype and event_level=@elevel and ctrl_id=@ctrlid";
            var cmd = new NpgsqlCommand(sql, this.__dbConnection);
            cmd.Parameters.AddWithValue("etime", item.dt);
            cmd.Parameters.AddWithValue("etype", item.Log_type);
            cmd.Parameters.AddWithValue("elevel", (int)item.Log_level);
            cmd.Parameters.AddWithValue("ctrlid", node.Id);
            //cmd.Parameters.AddWithValue("evalue", item.Log_Data);
            var dr_fes = cmd.ExecuteReader();
            bool res = dr_fes.Read();
            dr_fes.Close();
            //this.__dbConnection.Close();
            if (!res)
            {

              sql = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id, event_msg) " +
                "VALUES(@event_time, @event_type, @event_level, @ctrl_id, @event_msg)";
              cmd = new NpgsqlCommand(sql, this.__dbConnection);
              cmd.Parameters.AddWithValue("event_time", item.dt);
              cmd.Parameters.AddWithValue("event_type", item.Log_type);
              //cmd.Parameters.AddWithValue("event_value", item.Log_Data);
              cmd.Parameters.AddWithValue("event_level", (int)item.Log_level);
              cmd.Parameters.AddWithValue("ctrl_id", node.Id);
              cmd.Parameters.AddWithValue("event_msg", evt);
              cmd.ExecuteNonQuery();
              xx++;
            }

          }
          this.__dbConnection.Close();

        }

      }
      catch (Exception exp)
      {
        int x = 0;
      }
      finally
      {
        if (this.__dbConnection.State == System.Data.ConnectionState.Open)
        {
          this.__dbConnection.Close();
        }
      }

    }


    public void WriteBinaryData(List<DbReqestNotTrue> lstItems) {

      try
      {
        this.__dbConnection.Open();
        
        List<Log> listToUpdate = new List<Log>();
        NpgsqlCommand cmd_sel = null;
        string sql_sel = "SELECT \"event_time\" FROM main_ctrlevent mc where mc.ctrl_id=@id ORDER BY mc.\"event_time\" DESC LIMIT 1";
        //"Select * from main_ctrlevent where event_time=@etime and event_type=@etype and event_level=@elevel and ctrl_id=@ctrlid";
        string sql_ins = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id, event_msg) VALUES";
        
        int ii = 0;
        foreach (var item in lstItems)
        {
          if (item.TypeDevice == 0)
            continue;
          else
          {
            if (item.Logs == null)
              continue;
          }

          try
          {
            cmd_sel = new NpgsqlCommand(sql_sel, this.__dbConnection);
            cmd_sel.Parameters.AddWithValue("@id", item.ID);
            //sql = string.Format("SELECT \"event_time\" FROM main_ctrlevent mc where mc.ctrl_id = {0} " +
            //"ORDER BY mc.\"event_time\" DESC LIMIT 1", item.ID);

            //cmd.CommandText = sql;// = new NpgsqlCommand(sql, this.__dbConnection);
            var dr = cmd_sel.ExecuteReader();
            if (dr.HasRows)
            {
              dr.Read();
              dr.Close();
              DateTime dtevt = (DateTime)dr[0];

              foreach (var lItem in item.Logs)
              {
                if (lItem.dt > dtevt)
                {
                  listToUpdate.Add(lItem);
                }
              }
            }

            //Console.WriteLine("Обраб {0}", ii++);

            if (listToUpdate.Count > 0)
            {
              //sql = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id, event_msg) VALUES";
              //StringBuilder sb = new StringBuilder();
              //sb.Append(sql);
              //foreach (var itUpdate in listToUpdate)
              //{
              //  string val = string.Format("('{0}', {1}, {2}, {3}, '{4}'),",
              //      itUpdate.dt, itUpdate.Log_type, (int)itUpdate.Log_level, item.ID, itUpdate.EventMessage);
              //  sb.Append(val);
              //}
              //string vlue = sb.ToString();
              //if (!string.IsNullOrEmpty(vlue))
              //{
              //  cmd.CommandText = sql;
              //  vlue = vlue.TrimEnd(',');
              //  //cmd = new NpgsqlCommand(vlue, this.__dbConnection);
              //  cmd.ExecuteNonQuery();
              //}
            }
            else
            {

              //sql = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id, event_msg) VALUES";
              //StringBuilder sb = new StringBuilder();
              //sb.Append(sql);
              //cmd.Parameters.Add(new NpgsqlParameter("aaa", NpgsqlTypes.NpgsqlDbType.Array))
              //foreach (var lItem in item.Logs)
              //{
              //  string val = string.Format("('{0}', {1}, {2}, {3}, '{4}'),",
              //    lItem.dt, lItem.Log_type, (int)lItem.Log_level, item.ID, lItem.EventMessage);
              //  sb.Append(val);
              //}
              //string vlue = sb.ToString();
              //if (!string.IsNullOrEmpty(vlue))
              //{
              //  vlue = vlue.TrimEnd(',');
              //  cmd.CommandText = vlue;
                
              //  //cmd = new NpgsqlCommand(vlue, this.__dbConnection);
              //  cmd.ExecuteNonQuery();
              //}
            }

          }
          catch (Exception exp)
          {
            int x = 0;
          }
        }
      }
      catch (Exception exp)
      {

      }
      finally
      {
        this.__dbConnection.Close();
      }
    }


    public bool CleanDbEvent() {

      try
      {
        string sql = string.Format("delete from main_ctrlevent mc where mc.event_time < '{0}'",
          DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
        var connString = new NpgsqlConnectionStringBuilder(this.__connection)
        { CommandTimeout = 0 };
        using (var conn = new NpgsqlConnection(connString.ConnectionString))
        {
          conn.Open();
          using (var cmd = new NpgsqlCommand(sql, conn))
            cmd.ExecuteNonQuery();
        }
        return true;
      }
      catch (Exception exp)
      {

        return false;
      }
      finally {
        this.__dbConnection.Close();
      }
    }



    public void SeptStatictics()
    {
      DateTime dt = DateTime.Now;
      UlcSmalllStatistic ulcSmalllStatistic = GetStatistic(dt);
      ulcSmalllStatistic.current_time = DateTime.Now;
      try
      {
       

        var dbFactory = new ServiceStack.OrmLite.OrmLiteConnectionFactory(
          __connection, PostgreSqlDialect.Provider);
        using (var db = dbFactory.Open())
        {
         List<UlcSmalllStatistic> lst = db.Select<UlcSmalllStatistic>(o => o.current_time > DateTime.Now.Date);
           long id = db.Insert<UlcSmalllStatistic>(ulcSmalllStatistic, selectIdentity: true);
        }
      }
      catch (Exception ex)
      {
        int x = 0;
      }
    }




    public List<RsNotTrueData> GetNotTruerS485()
    {

      try
      {
        var dbFactory = new ServiceStack.OrmLite.OrmLiteConnectionFactory(
          __connection, PostgreSqlDialect.Provider);
        using (var db = dbFactory.Open())
        {
          string sql = string.Format("select mn.id, mc.id as id_data, mn.ip_address, mn.meters from main_ctrlinfo mn , main_ctrldata mc where mn.id = mc.ctrl_id and mn.unit_type_id = 1 and(mc.cdin >> 7) = 0 and mc.\"current_time\" > '{0}'",
            DateTime.Now.ToString("yyyy-MM-dd"));
          return db.Select<RsNotTrueData>(sql);

        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public UlcSmalllStatistic GetStatistic(DateTime dt)
    {

      var consql = new NpgsqlConnection(this.__connection);
      UlcSmalllStatistic ulcStatistic = null;
      try
      {
        consql.Open();
        ulcStatistic = new UlcSmalllStatistic();

        //Выбор всех объектов
        var sql = "select count(*) from main_nodes mn where mn.node_kind_id =3 and active =1";
        var cmd = new NpgsqlCommand(sql, consql);
        ulcStatistic.all = (long)cmd.ExecuteScalar();

        //Всего РВП
        sql = "select count(*) from main_nodes mn2, main_ctrlinfo mc where mn2.node_kind_id =3 and active =1 and mn2.id =mc.id and mc.unit_type_id =0";
        cmd.CommandText = sql;
        ulcStatistic.allrvp = (long)cmd.ExecuteScalar();
        //Всего ULC
        sql = "select count(*) from main_nodes mn2, main_ctrlinfo mc where mn2.node_kind_id =3 and active =1 and mn2.id =mc.id and mc.unit_type_id =1";
        cmd.CommandText = sql;
        ulcStatistic.allulc = (long)cmd.ExecuteScalar();
        //Всего не на связи
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id =mc.ctrl_id and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        long nn = (long)cmd.ExecuteScalar();
        ulcStatistic.neterrorall = ulcStatistic.all - nn;
        //Всего нет связи по RS
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id = mc.ctrl_id and mn.unit_type_id =1 and (mc.cdin >>7)=0 and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        ulcStatistic.all_rs_errorrs = (long)cmd.ExecuteScalar();
        //Всего слабый сигнал GSM
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id = mc.ctrl_id and  ((-113 + (mc.signal) * 2))<-100 and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        ulcStatistic.allerrorgsm = (long)cmd.ExecuteScalar();


        // Всего контроллеров ULC не на связи
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id =mc.ctrl_id and mn.unit_type_id =1 and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        nn = (long)cmd.ExecuteScalar();
        ulcStatistic.neterrorulc = ulcStatistic.allulc - nn;
        // всего ulc ошибка RS 
        ulcStatistic.ulc_rs_errorrs = ulcStatistic.all_rs_errorrs;
        //всего ulc GSM  
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id = mc.ctrl_id and mn.unit_type_id =1 and  ((-113 + (mc.signal) * 2))<-100 and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        ulcStatistic.ulcerrorgsm = (long)cmd.ExecuteScalar();


        // Всего контроллеров RVP не на связи
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id =mc.ctrl_id and mn.unit_type_id =0 and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        nn = (long)cmd.ExecuteScalar();
        ulcStatistic.neterrorrvp = ulcStatistic.allrvp - nn;
        //всего рвп неисправность rs
        ulcStatistic.rvp_rs_errorrs = 0;
        //всего rvp GSM
        sql = string.Format("select count(*) from main_ctrlinfo mn ,main_ctrldata mc where mn.id = mc.ctrl_id and mn.unit_type_id =0 and  ((-113 + (mc.signal) * 2))<-100 and mc.\"current_time\" >'{0}'", dt.ToString("yyyy-MM-dd"));
        cmd.CommandText = sql;
        ulcStatistic.rvperrorgsm = (long)cmd.ExecuteScalar();


        consql.Close();
      }
      catch (Exception exp)
      {
        return null;
      }
      finally
      {

        consql.Close();
      }
      return ulcStatistic;

    }

    public void WriteEventMessage(List<DbReqestNotTrue> lstItems)
    {
      try
      {
        this.__dbConnection.Open();
        List<Log> listToUpdate = null;
        NpgsqlCommand cmd_sel = null;
        NpgsqlCommand cmd_ins = null;
        string sql_sel = "SELECT \"event_time\" FROM main_ctrlevent mc where mc.ctrl_id=@id ORDER BY mc.\"event_time\" DESC LIMIT 1";
        //"Select * from main_ctrlevent where event_time=@etime and event_type=@etype and event_level=@elevel and ctrl_id=@ctrlid";
        string sql_ins = "INSERT INTO main_ctrlevent(\"event_time\", event_type, event_level, ctrl_id,event_msg) " +
          "VALUES(@event_time, @event_type, @event_level, @ctrl_id,@event_msg)";
        cmd_ins = new NpgsqlCommand(sql_ins, this.__dbConnection);
        cmd_ins.Parameters.Add(new NpgsqlParameter("@event_time", NpgsqlTypes.NpgsqlDbType.TimestampTz));
        //cmd_ins.Parameters[0].Value = lstColumns.__event_time.ToArray();
        cmd_ins.Parameters.Add(new NpgsqlParameter("@event_type", NpgsqlDbType.Integer));
        //cmd_ins.Parameters[1].Value = lstColumns.__event_type.ToArray();
        cmd_ins.Parameters.Add(new NpgsqlParameter("@event_level", NpgsqlDbType.Integer));
        //cmd_ins.Parameters[2].Value = lstColumns.__event_level.ToArray();
        cmd_ins.Parameters.Add(new NpgsqlParameter("@ctrl_id", NpgsqlDbType.Integer));
        //cmd_ins.Parameters[3].Value = lstColumns.__ctrl_id.ToArray();
        cmd_ins.Parameters.Add(new NpgsqlParameter("@event_msg", NpgsqlDbType.Varchar));
        cmd_ins.Prepare();
        //string sql = string.Empty;
        //NpgsqlCommand cmd = new NpgsqlCommand(sql, this.__dbConnection);
        //LstColumns lstColumns = new LstColumns();
        int ii = 0;
        foreach (var item in lstItems)
        {
          
          if (item.TypeDevice == 0)
            continue;
          else {
            if (item.Logs == null)
              continue;
          }
          try
          {
            listToUpdate = new List<Log>();
            DateTime dts = DateTime.Now.AddDays(-30);
            //sql = string.Format("SELECT \"event_time\" FROM main_ctrlevent mc where mc.ctrl_id = {0} " +
            //"ORDER BY mc.\"event_time\" DESC LIMIT 1", item.ID);

            //cmd.CommandText = sql;// = new NpgsqlCommand(sql, this.__dbConnection);
            cmd_sel = new NpgsqlCommand(sql_sel, this.__dbConnection);
            cmd_sel.Parameters.AddWithValue("@id", item.ID);
            var dr = cmd_sel.ExecuteReader();
            if (dr.HasRows)
            {
              dr.Read();
              
              DateTime dtevt = (DateTime)dr[0];
              //DateTime dts = DateTime.Now.AddDays(-30);
              foreach (var lItem in item.Logs)
              {
                if (lItem.dt>dts)
                {
                  if (lItem.dt > dtevt)
                    
                    //lstColumns.AddRecord(lItem.dt, lItem.Log_type, (int)lItem.Log_level, item.ID, lItem.EventMessage);
                  listToUpdate.Add(lItem);
                  //{ 
                    
                  //}
                }
              }
            }
            
            dr.Close();
            //Console.WriteLine("Обраб {0}", ii++);
            
            //cmd_ins.Parameters[4].Value = lstColumns.__event_msg.ToArray();
            
            //cmd_ins.CommandText = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id) VALUES (@event_time, @event_type, @event_level, @ctrl_id, @event_msg, @event_msg)";

            if (listToUpdate.Count > 0)
            {
              NpgsqlTransaction tran = this.__dbConnection.BeginTransaction();
              try
              {
                foreach (var itLog in listToUpdate)
                {
                  cmd_ins.Parameters[0].Value = itLog.dt;// lstColumns.__event_msg.ToArray();
                  cmd_ins.Parameters[1].Value = itLog.Log_type;
                  cmd_ins.Parameters[2].Value = (int)itLog.Log_level;
                  cmd_ins.Parameters[3].Value = item.ID;
                  cmd_ins.Parameters[4].Value = itLog.EventMessage;
                  cmd_ins.ExecuteNonQuery();
                }

                tran.Commit();
              }
              catch (Exception e)
              {
                tran.Rollback();
                throw;
              }

              //sql = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id) VALUES";
              //StringBuilder sb = new StringBuilder();
              //sb.Append(sql);
              //foreach (var itUpdate in listToUpdate)
              //{
              //  string val = string.Format("('{0}', {1}, {2}, {3}),",
              //      itUpdate.dt, itUpdate.Log_type, (int)itUpdate.Log_level, item.ID/*, itUpdate.EventMessage*/);
              //  sb.Append(val);
              //}
              //string vlue = sb.ToString();
              //if (!string.IsNullOrEmpty(vlue))
              //{

              //  vlue = vlue.TrimEnd(',');
              //  cmd.CommandText = vlue;
              //  //cmd = new NpgsqlCommand(vlue, this.__dbConnection);
              //  cmd.ExecuteNonQuery();
              //}
            }
            else
            {
              //lstColumns.AddRange(item, item.ID);
              
              NpgsqlTransaction tran = this.__dbConnection.BeginTransaction();
              try
              {
                int iCom = 0;
                foreach (var itLog in item.Logs)
                {
                  if (itLog.dt>dts)
                  {
                    cmd_ins.Parameters[0].Value = itLog.dt;// lstColumns.__event_msg.ToArray();
                    cmd_ins.Parameters[1].Value = itLog.Log_type;
                    cmd_ins.Parameters[2].Value = (int)itLog.Log_level;
                    cmd_ins.Parameters[3].Value = item.ID;
                    cmd_ins.Parameters[4].Value = itLog.EventMessage;
                    cmd_ins.ExecuteNonQuery();
                    iCom++;
                  }
                }
                //if(iCom>0)
                tran.Commit();
              }
              catch (Exception e)
              {
                tran.Rollback();
                throw;
              }


              //cmd_ins.Parameters.Add("@event_time",NpgsqlTypes.NpgsqlDbType.Array)
             // dr = cmd_ins.ExecuteReader();
              //sql = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id) VALUES";
              //StringBuilder sb = new StringBuilder();
              //sb.Append(sql);

              //foreach (var lItem in item.Logs)
              //{
              //  string val = string.Format("('{0}', {1}, {2}, {3}),",
              //    lItem.dt, lItem.Log_type, (int)lItem.Log_level, item.ID/*, lItem.EventMessage*/);
              //  sb.Append(val);
              //}
              //string vlue = sb.ToString();
              //if (!string.IsNullOrEmpty(vlue))
              //{
              //  vlue = vlue.TrimEnd(',');
              //  cmd.CommandText = vlue;
              //  //cmd = new NpgsqlCommand(vlue, this.__dbConnection);
              //  cmd.ExecuteNonQuery();
              //}
            }

          }
          catch (Exception exp)
          {
            int x = 0;
          }
        }
      }
      catch (Exception exp)
      {
        
      }
      finally
      {
        this.__dbConnection.Close();
      }
    }


    internal void InsertLogMsg(List<DbReqestNotTrue> lstItems)
    {
      foreach (var node in lstItems)
      {
        try
        {
          if (node.Logs != null)
          {
            //var con = new NpgsqlConnection(this.__connection);
            this.__dbConnection.Open();
            foreach (var item in node.Logs)
            {
              string evt = Log.ConvertToString(item);
              var sql = "Select * from main_ctrlevent where event_time=@etime and event_type=@etype and event_level=@elevel and ctrl_id=@ctrlid";
              var cmd = new NpgsqlCommand(sql, this.__dbConnection);
              cmd.Parameters.AddWithValue("etime", item.dt);
              cmd.Parameters.AddWithValue("etype", item.Log_type);
              cmd.Parameters.AddWithValue("elevel", (int)item.Log_level);
              cmd.Parameters.AddWithValue("ctrlid", node.ID);
              //cmd.Parameters.AddWithValue("evalue", item.Log_Data);
              var dr_fes = cmd.ExecuteReader();
              bool res = dr_fes.Read();
              dr_fes.Close();
              //this.__dbConnection.Close();
              if (!res)
              {
                sql = "INSERT INTO main_ctrlevent(event_time, event_type, event_level, ctrl_id, event_msg) " +
                  "VALUES(@event_time, @event_type, @event_level, @ctrl_id, @event_msg)";
                cmd = new NpgsqlCommand(sql, this.__dbConnection);
                cmd.Parameters.AddWithValue("event_time", item.dt);
                cmd.Parameters.AddWithValue("event_type", item.Log_type);
                //cmd.Parameters.AddWithValue("event_value", item.Log_Data);
                cmd.Parameters.AddWithValue("event_level", (int)item.Log_level);
                cmd.Parameters.AddWithValue("ctrl_id", node.ID);
                cmd.Parameters.AddWithValue("event_msg", evt);
                cmd.ExecuteNonQuery();
              }
            }
            this.__dbConnection.Close();
          }
        }
        catch (Exception exp)
        {
          int x = 0;
        }
        finally
        {
          if (this.__dbConnection.State == System.Data.ConnectionState.Open)
          {
            this.__dbConnection.Close();
          }
        }
      }
    }

    public bool getDTREC(Node node)
    {
      try
      {
        this.__dbConnection.Open();
        var sql = string.Format("select \"current_time\",body from public.main_ctrlcurrent where " +
                  "\"current_time\" = (SELECT MAX(\"current_time\") FROM public.main_ctrlcurrent) and ctrl_id = {0}", node.Id);
        var cmd = new NpgsqlCommand(sql, this.__dbConnection);
        //cmd.Parameters.AddWithValue("ids", rec);
        var dr_fes = cmd.ExecuteReader();
        if (!dr_fes.Read())
          return false;
        DateTime dtn = DateTime.Now;
        DateTime dtl = new DateTime(dtn.Year, dtn.Month, dtn.Day, 0, 0, 0);
        DateTime curt = (DateTime)dr_fes[0];
        string msg= (string)dr_fes[1];
        dr_fes.Close();
        this.__dbConnection.Close();
        if (dtl == curt)
        {
          node.Message = msg;
          return true;
        }
        else
          return false;
      }

      catch (Exception exp)
      {
        return false;
      }
      finally {
        if (this.__dbConnection.State == System.Data.ConnectionState.Open)
        {
          this.__dbConnection.Close();
        }
      }
    } 

    public void ReadDataBase()
    {
      //ipaddress=10.32.18.38
      //username=postgres
      //password=pgp@ssdb
      //var cs = string.Format("Host={0};Username={1};Password={2};Database=ctrl_mon_dev",this.dBIpAddress,this.DbUserName,this.DbPassword);
      NpgsqlConnection con_fes=null;
      NpgsqlConnection con_res=null;
      NpgsqlConnection con_tp=null;
      try
      {
        con_fes = new NpgsqlConnection(this.__connection);
        con_fes.Open();
        var sql_fes = "SELECT * FROM main_nodes where(parent_id is NULL)";
        var cmd_fes = new NpgsqlCommand(sql_fes, con_fes);
        var dr_fes = cmd_fes.ExecuteReader();

        while (dr_fes.Read())
        {
          List<Node> lst_res = new List<Node>();
          con_res = new NpgsqlConnection(this.__connection);
          con_res.Open();
          var sql_res = string.Format("SELECT * FROM main_nodes where(parent_id={0})", (int)dr_fes[0]);
          var cmd_res = new NpgsqlCommand(sql_res, con_res);
          var dr_res = cmd_res.ExecuteReader();
          while (dr_res.Read())
          {
            List<Node> lst_tp = new List<Node>();
            con_tp = new NpgsqlConnection(this.__connection);
            con_tp.Open();
            var sql_tp = string.Format("SELECT * FROM main_nodes where(parent_id={0})", (int)dr_res[0]);
            var cmd_tp = new NpgsqlCommand(sql_tp, con_tp);
            var dr_tp = cmd_tp.ExecuteReader();
            while (dr_tp.Read())
            {
              var sql_ip = string.Format("SELECT * FROM main_ctrlinfo where(id={0})", (int)dr_tp[0]);
              var con_ip = new NpgsqlConnection(this.__connection);
              var cmd_ip = new NpgsqlCommand(sql_ip, con_ip);
              con_ip.Open();
              var dr_ip = cmd_ip.ExecuteReader();
              while (dr_ip.Read())
              {
                lst_tp.Add(new Node() { Id = (int)dr_tp[0], Name = (string)dr_tp[1], IP = (string)dr_ip[1], Phone = (string)dr_ip[2], Type = (int)dr_ip[4], Nodes = null });
              }
              dr_ip.Close();
              con_ip.Close();
            }
            dr_tp.Close();
            con_tp.Close();
            lst_res.Add(new Node() { Id = (int)dr_res[0], Name = (string)dr_res[1], Nodes = lst_tp });
          }
          dr_res.Close();
          con_res.Close();
          __nodes.Add(new Node() { Id = (int)dr_fes[0], Name = (string)dr_fes[1], Nodes = lst_res });
        }
        dr_fes.Close();
        con_fes.Close();
      }
      catch(Exception exp)
      {
        //Console.WriteLine(exp.Message);
      }
      finally {
        if (con_fes != null)
        {
          if (con_fes.State == System.Data.ConnectionState.Open)
          {
            con_fes.Close();
          }
        }
        if (con_res != null)
        {
          if (con_res.State == System.Data.ConnectionState.Open)
          {
            con_res.Close();
          }
        }
        if (con_tp != null)
        {
          if (con_tp.State == System.Data.ConnectionState.Open)
          {
            con_tp.Close();
          }
        }
      }
     
    }
  }
}
