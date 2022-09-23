using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_logs")]
  class MainAdminLog
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

    [ServiceStack.DataAnnotations.Required]
    public string message { get; set; }
    
  }

}
