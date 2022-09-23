using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InterUlc.Db
{
  [ServiceStack.DataAnnotations.Alias("main_stat")]
  public class UlcSmalllStatistic
  {
    [ServiceStack.DataAnnotations.AutoIncrement]
    [System.ComponentModel.Browsable(false)]

    public int id { get; set; }
    [DisplayName("Дата и время")]
    public DateTime current_time
    {
      get;
      set;
    }
    [DisplayName("Все котроллеры")]
    public long all { get; set; }
    [DisplayName("РВП")]
    public long allrvp { get; set; }
    [DisplayName("ULC")]
    public long allulc { get; set; }
    [DisplayName("Нет связи(все)")]
    public long neterrorall { get; set; }
    [DisplayName("Нет связи RS-485")]
    public long all_rs_errorrs { get; set; }
    [DisplayName("Плохая связь(все)")]
    public long allerrorgsm { get; set; }
    [DisplayName("Нет связи(ULC)")]
    public long neterrorulc { get; set; }
    [DisplayName("Нет связи RS-485(ULC)")]
    public long ulc_rs_errorrs { get; set; }
    [DisplayName("Плохая связь(ULC)")]
    public long ulcerrorgsm { get; set; }
    [DisplayName("Нет связи(RVP)")]
    public long neterrorrvp { get; set; }
    [DisplayName("Нет связи RS-485(RVP)")]
    public long rvp_rs_errorrs { get; set; }
    [DisplayName("Плохая связь(RVP)")]
    public long rvperrorgsm { get; set; }

  }
}

