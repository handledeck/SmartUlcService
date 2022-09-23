using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_ctrlinfo")]
  public class MainInfo
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    [ServiceStack.DataAnnotations.PrimaryKey]
    public int id { get; set; }
    public string ip_address { get; set; }
    public string phone_num { get; set; }
    public int arm_id { get; set; }
    public int unit_type_id { get; set; }
    public string meter_type { get; set; }
    public string meter_serial { get; set; }
  }



}
