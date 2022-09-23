using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UlcWin.DB
{
  [ServiceStack.DataAnnotations.Alias("main_ctrlinfo")]
  public class OrmDbInfo
  {
    //[ServiceStack.DataAnnotations.AutoIncrement]
    [ServiceStack.DataAnnotations.PrimaryKey]
    public int id { get; set; }
    public string ip_address { get; set; }
    public string phone_num { get; set; }
    public int arm_id { get; set; }
    public int unit_type_id { get; set; }
    public string meters { get; set; }
   
  }
}
