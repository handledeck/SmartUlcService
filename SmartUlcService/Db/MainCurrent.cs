using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_ctrlcurrent")]
  public class MainCurrent
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    public int id { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public DateTime current_time { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public string body { get; set; }

    [ServiceStack.DataAnnotations.Required]
    [ForeignKey(typeof(MainInfo))]
    public int ctrl_id { get; set; }

  }
}
