using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_user")]
  public class MainUsers
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    [ServiceStack.DataAnnotations.PrimaryKey]
    public int id { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public string usr { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public string pwd { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public string items { get; set; }

    public string comment { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public short level { get; set; }

  }
}
