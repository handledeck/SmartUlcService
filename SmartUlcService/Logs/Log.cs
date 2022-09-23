using System;
using System.Collections.Generic;
using System.Text;
using static InterUlc.Logs.EnumLogs;

namespace InterUlc.Logs
{
  public class Log
  {
    public DateTime dt { get; set; }
    public LOG_LVL Log_level { get; set; }
    public byte Log_type { get; set; }
    public ushort Log_Data { get; set; }
    public string EventMessage { get; set; }
    public List<byte[]> Binary { get; set; }

    public static string ParceLevel(LOG_LVL lvl)
    {
      //LOG_LVL val = (LOG_LVL)(lvl);
      return GetDescription((LOG_LVL)lvl);
      //return lvl.ToString();
    }

    private static string TypeToString(LOG_TYPE type)
    {
      return GetDescription((LOG_TYPE)type);
      //LOG_TYPE val = (LOG_TYPE)(type);
      //return val.ToString();
    }

    public static string ConvertToString(Log logEntry)
    {
      string result = string.Empty;
      //result += logEntry.dt.ToString("G") + ":";
      //result += $" [{ParceLevel((LOG_LVL)logEntry.Log_level)}]";
      result += $"{TypeToString((LOG_TYPE)logEntry.Log_type)}";
      result += $" ({CodeToString((LOG_TYPE)logEntry.Log_type, logEntry.Log_Data)})";

      return result;
    }

    private static string CodeToString(LOG_TYPE type, ushort code)
    {
      string result = string.Empty;
      try
      {
        switch (type)
        {
          case LOG_TYPE.Registration:
            {
              //RegistrationCode val = (RegistrationCode)code;
              result = GetDescription((RegistrationCode)code);//val.ToString();
            }
            break;
          case LOG_TYPE.DeviceRestart:
            {
              //RestartCode val = new ComboboxItem<RestartCode>((RestartCode)code);
              //result = val.ToString();
              int ud = (int)code >> 8;
              result = GetDescription((RestartCode)(code & 0xff)) + (ud != 0 ? " (отсроченное)" : "");//val.ToString();
            }
            break;
          case LOG_TYPE.ConfigSettings:
            {
              result = GetDescription((ConfigCode)code);//val.ToString();
                                                        //  ComboboxItem<ConfigCode> val = new ComboboxItem<ConfigCode>((ConfigCode)code);
                                                        //result = val.ToString();
            }
            break;
          case LOG_TYPE.SignalStrength:
            {
              byte min, max;
              min = (byte)(code & 0xff);
              max = (byte)((code >> 8) & 0xff);
              result = $"минимум - {min} и максимум - {max}";
            }
            break;
          case LOG_TYPE.ControlDevice:
            {
              byte c = (byte)(code & 0xff);
              //ComboboxItem<DeviceControlCode> val = new ComboboxItem<DeviceControlCode>((DeviceControlCode)c);
              result = GetDescription((DeviceControlCode)c);//val.ToString();
              if (c == 30)
              {
                result = result.ToString() + (byte)((code >> 8) & 0xff);
              }
              else
                result = result.ToString();
              //result = GetDescription((DeviceControlCode)code);//val.ToString();
              //ComboboxItem<DeviceControlCode> val = new ComboboxItem<DeviceControlCode>((DeviceControlCode)code);
              //result = val.ToString();
              //result = GetDescription((DeviceControlCode)code);//val.ToString();
              //ComboboxItem<DeviceControlCode> val = new ComboboxItem<DeviceControlCode>((DeviceControlCode)code);
              //result = val.ToString();
            }
            break;
          case LOG_TYPE.DeviceUpdate:
            {
              result = GetDescription((UpdateCode)code);//val.ToString();
                                                        //ComboboxItem<UpdateCode> val = new ComboboxItem<UpdateCode>((UpdateCode)code);
                                                        //result = val.ToString();
            }
            break;
          case LOG_TYPE.PdpContext:
            {
              result = GetDescription((PdpCode)code);//val.ToString();
                                                     //ComboboxItem<PdpCode> val = new ComboboxItem<PdpCode>((PdpCode)code);
                                                     //result = val.ToString();
            }
            break;
          case LOG_TYPE.SocketWorkDBG:
          case LOG_TYPE.SocketWorkTH:
          case LOG_TYPE.SocketWorkIEC:
          case LOG_TYPE.SocketWorkUPD:
            {
              if (code == 0) code = 2; //иногда почему-то при закрытии порта отладки выскакивает 0
              byte error = (byte)(code >> 8);

              //ComboboxItem<SocketWorkCode> val = new ComboboxItem<SocketWorkCode>((SocketWorkCode)(code & 0xff));
              result = GetDescription((SocketWorkCode)(code & 0xff)) + ((error > 0) ? $" [{error}]" : "");
              //result = val.ToString() + ((error > 0)?$" [{error}]":"");
            }
            break;
          case LOG_TYPE.IecClose:
            {
              result = GetDescription((IecCloseCode)code);//val.ToString();
                                                          //ComboboxItem<IecCloseCode> val = new ComboboxItem<IecCloseCode>((IecCloseCode)code);
                                                          //result = val.ToString();
            }
            break;
          case LOG_TYPE.ExtErrorReport:
            result = code.ToString();
            break;
          case LOG_TYPE.RegStatEvent:
            {
              result = GetDescription((RegStatEventCode)code);
              break;
            }
          case LOG_TYPE.PatchUpload:
            {
              result = GetDescription((UpdateCode)code);
              //ComboboxItem<UpdateCode> val = new ComboboxItem<UpdateCode>((UpdateCode)(code & 0xff));
              //result = val.ToString() + ((code >> 8) & 0xff);
              break;
            }
          default:
            {
              result = $"Неизвестная группа инструкций!";
            }
            break;
        }
      }
      catch  {
        result = "Ошибка парсера сообщения";
      }
      return result;
    }
  }
}
