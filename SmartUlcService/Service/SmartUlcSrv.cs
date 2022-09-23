using Quartz;
using SmartUlcService.NLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SmartUlcService
{
  public partial class SmartUlcSrv : ServiceBase,IJob
  {
    public SmartUlcSrv()
    {
      InitializeComponent();
    }

    public void Execute(IJobExecutionContext context)
    {
      UlcSrvLog.Logger.Info("work;");
    }

    protected override void OnStart(string[] args)
    {
    }

    protected override void OnStop()
    {
    }
  }
}
