using InterUlc.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Logs
{
  public class LstColumns
  {
    public List<DateTime> __event_time;
    public List<int> __event_type;
    public List<int> __event_level;
    public List<int> __ctrl_id;
    public List<string> __event_msg;
    public int Count { get; set; }
    public LstColumns()
    {
      __event_time=new List<DateTime>();
      __event_type = new List<int>();
      __event_level=new List<int>();
      __ctrl_id=new List<int>();
      __event_msg= new List<string>(); 
    }

    public void AddRecord(DateTime evTime,int event_type, int event_level,int ctrl_id, string event_msg)
    {
      this.__event_time.Add(evTime);
      this.__event_type.Add(event_type);
      this.__event_level.Add(event_level);
      this.__ctrl_id.Add(ctrl_id);
      this.__event_msg.Add(event_msg);
      this.Count++;
    }

    public void AddRange(DbReqestNotTrue dbReqestNotTrue,int ctrlId) {
      foreach (var item in dbReqestNotTrue.Logs)
      {
        this.AddRecord(item.dt, item.Log_type, (int)item.Log_level, ctrlId, item.EventMessage);
        //this.Count++;
      }
    }
  }
}
