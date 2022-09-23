using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterUlc.Db
{
  public class RsNotTrueData
  {
    public int id { get; set; }
    public int id_data { get; set; }
    public string ip_address { get; set; }
    public string meters { get; set; }
    public ItemRunComplite EvtItemRunComplite { get; set; }
    public Task OwnerTask { get; set; }
    public override string ToString()
    {
      return this.ip_address;
    }
  }
}
