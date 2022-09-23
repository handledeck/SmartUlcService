using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{

  public enum EnLogEvent {
    CHANGE_NET_STATE = 14,
    ChangeIMEI =100
  }

  [ServiceStack.DataAnnotations.Alias("main_logs")]
  public class MainLogs
  {
      [ServiceStack.DataAnnotations.AutoIncrement]
      [ServiceStack.DataAnnotations.PrimaryKey]
      public int id { get; set; }
      
      [ServiceStack.DataAnnotations.Required]
      public DateTime current_time { get; set; }

      [ServiceStack.DataAnnotations.Required]
      public int id_user { get; set; }

      [ServiceStack.DataAnnotations.Required]
      public string usr_name { get; set; }

      public string message { get; set; }
    public int log_event { get; set; }
    public string host_from { get; set; }

  }
}
