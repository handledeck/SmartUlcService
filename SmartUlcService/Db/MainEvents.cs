using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_ctrlevent")]
  public class MainEvents
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    public int id { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public DateTime current_time { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public int event_type { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public int event_level { get; set; }

    public int event_value { get; set; }
    
    [ServiceStack.DataAnnotations.Required]
    [ForeignKey(typeof(MainInfo))]
    public int ctrl_id { get; set; }

    public string event_msg { get; set; }


  }
}
