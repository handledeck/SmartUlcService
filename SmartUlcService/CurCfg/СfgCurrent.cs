using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace InterUlc.CurCfg
{
  public class CurrentCfg
  {

    public string APN { get; set; }
    public string USER { get; set; }
    public string PASS { get; set; }
    public uint DT { get; set; }
    public ushort DEBOUNCE { get; set; }
    public byte DEBUG { get; set; }
    public byte EST { get; set; }
    public string IP { get; set; }
    public int TCP { get; set; }
    public byte TSEND { get; set; }
    public ushort AIN { get; set; }
    public ushort DIN { get; set; }
    public ushort DOUT { get; set; }
    public ushort DOOR { get; set; }
    public string LATIT { get; set; }
    public string LONGIT { get; set; }
    public byte TZ { get; set; }
    public ushort CDIN { get; set; }
    public ushort CDOUT { get; set; }
    public string CAIN { get; set; }
    public float SRISE { get; set; }
    public float SSET { get; set; }
    public byte SIM { get; set; }
    public byte GSM { get; set; }
    public byte GPRS { get; set; }
    public byte SIGNAL { get; set; }
    public byte DBZ { get; set; }
    public string IPOWN { get; set; }
    public string SCHED { get; set; }
    public byte RAS { get; set; }
    public string VER { get; set; }
    public string SERIAL { get; set; }
    public byte NUM { get; set; }
    public string SVERS { get; set; }
    public string TECHN { get; set; }
    public uint FMW { get; set; }
    public string TMSET { get; set; }
    public string IPP { get; set; }
    public int PREP { get; set; }
    public int IMEI { get; set; }
    public byte LOGSLVL { get; set; }

    //internal bool GetConfigIP(TcpClient client, out string message)
    //{
    //  bool isMsg = false;
    //  message = string.Empty;
    //  try
    //  {
    //    byte[] buffer = new byte[1024];
    //    NetworkStream stream = client.GetStream();
    //    stream.ReadTimeout = 20000;
    //    byte[] bRng = System.Text.ASCIIEncoding.ASCII.GetBytes("CONFIG?\r");
    //    for (int i = 0; i < 3; i++)
    //    {
    //      try
    //      {
    //        stream.Write(bRng, 0, bRng.Length);
    //        Thread.Sleep(10);

    //        int size = stream.Read(buffer, 0, buffer.Length);
    //        message = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, size);
    //        if (!string.IsNullOrEmpty(message))
    //        {
    //          if (message.StartsWith("CONFIG") && message[message.Length - 1] == '\n')
    //          {
    //            isMsg = true;
    //            break;
    //          }
    //        }
    //      }
    //      catch { }
    //    }
    //    return isMsg;
    //  }
    //  catch
    //  {
    //    return false;
    //  }
    //}

    public bool GetConfigIP(TcpClient client, string ip, out string message)
    {
      bool isMsg = false;
      message = string.Empty;


      byte[] buffer = new byte[1024];


      byte[] bRng = System.Text.ASCIIEncoding.ASCII.GetBytes("CONFIG?\r");

      for (int i = 0; i < 2; i++)
      {
        try
        {
          if (!client.Connected)
          {
            client = Program.GetConnection(ip, 10251);
            if (client == null)
            {
              Thread.Sleep(100);
              continue;
            }
          }
          NetworkStream stream = client.GetStream();
          stream.ReadTimeout = 15000;
          stream.Write(bRng, 0, bRng.Length);
          //Thread.Sleep(10);

          int size = stream.Read(buffer, 0, buffer.Length);

          message = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, size);
          if (!string.IsNullOrEmpty(message))
          {
            if (message.StartsWith("CONFIG") && message[message.Length - 1] == '\n')
            {
              isMsg = true;
              break;
            }
            else {
              client.Close();
              Thread.Sleep(100);
              continue;
            }
          }
          else
          {
            client.Close();
            Thread.Sleep(100);
          }

        }
        catch
        {
          client.Close();
          Thread.Sleep(100);
          //return false;

        }
      }

      return isMsg;
    }
   


    bool GetExtarctConfig(string indata)
    {
      bool readed = false;

      try
      {
        if (indata.StartsWith("CONFIG"))
        {
          indata = indata.Substring(7);
        }
        string[] cdata = indata.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (cdata.Length > 0)
        {
          foreach (var item in cdata)
          {
            string[] pdata = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (pdata.Length > 1)
            {
              if (pdata[0] == "APN")
                this.APN = pdata[1];
              else if (pdata[0] == "USER")
                this.USER = pdata[1];
              else if (pdata[0] == "DT")
                this.DT = uint.Parse(pdata[1]);
              else if (pdata[0] == "DEBOUNCE")
                this.DEBOUNCE = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DEBUG")
                this.DEBUG = byte.Parse(pdata[1]);
              else if (pdata[0] == "EST")
                this.EST = byte.Parse(pdata[1]);
              else if (pdata[0] == "IP")
                this.IP = pdata[1];
              else if (pdata[0] == "TCP")
                this.TCP = int.Parse(pdata[1]);
              else if (pdata[0] == "TSEND")
                this.TSEND = byte.Parse(pdata[1]);
              else if (pdata[0] == "AIN")
                this.AIN = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DIN")
                this.DIN = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DOUT")
                this.DOUT = ushort.Parse(pdata[1]);
              else if (pdata[0] == "DOOR")
                this.DOOR = ushort.Parse(pdata[1]);
              else if (pdata[0] == "LATIT")
                this.LATIT = pdata[1];
              else if (pdata[0] == "LONGIT")
                this.LONGIT = pdata[1];
              else if (pdata[0] == "TZ")
                this.TZ = byte.Parse(pdata[1]);
              else if (pdata[0] == "CDIN")
                this.CDIN = ushort.Parse(pdata[1]);
              else if (pdata[0] == "CDOUT")
                this.CDOUT = ushort.Parse(pdata[1]);
              else if (pdata[0] == "CAIN")
                this.CAIN = pdata[1];
              else if (pdata[0] == "SRISE")
                this.SRISE = float.Parse(pdata[1]);
              else if (pdata[0] == "SSET")
                this.SSET = float.Parse(pdata[1]);
              else if (pdata[0] == "SIGNAL")
                this.SIGNAL = byte.Parse(pdata[1]);
              else if (pdata[0] == "IPOWN")
                this.IPOWN = pdata[1];
              else if (pdata[0] == "SCHED")
                this.SCHED = pdata[1];
              else if (pdata[0] == "RAS")
                this.RAS = byte.Parse(pdata[1]);
              else if (pdata[0] == "SERIAL")
                this.SERIAL = pdata[1];
              else if (pdata[0] == "SVERS")
                this.SVERS = pdata[1].TrimEnd(new char[] { '\r', '\n' });

            }
          }
          readed = true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception exp)
      {
        return false;
      }
      return readed;
    }
  }
}
