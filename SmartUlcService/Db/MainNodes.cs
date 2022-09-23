using System;
using System.Collections.Generic;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_nodes")]
  public class MainNodes
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    [ServiceStack.DataAnnotations.PrimaryKey]
    [ServiceStack.DataAnnotations.Required]
    public int id { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public string name { get; set; }

    public string code { get; set; }

    [ServiceStack.DataAnnotations.Required]
    public int node_kind_id { get; set; }
 
    public int parent_id { get; set; }
    public int active { get; set; }
    public int light { get; set; }
    public string comments { get; set; }
  }
}
