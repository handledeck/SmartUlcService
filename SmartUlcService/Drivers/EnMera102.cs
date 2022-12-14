using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UlcWin.Drivers
{
  public class EnMera102
  {
    public enum EnumFunEnMera
    {
      ReadConfig = 0x0101,
      ReadDateTime = 0x0120,
      ReadCurrent = 0x0502,
      WriteDateTime = 0x0121,
      ReadTariffSum = 0x0131,
      ReadTariffSumOfDay = 0x012F,
      ReadPower = 0x0132,
      ReadMiddle3Power = 0x012E,
      ConfUART = 0x0109,
      ReadSerialNumber = 0x011A
    }

    public static Exception Read(byte[] buf, int lenghtRead, TcpClient client, out byte[] bufRead)
    {
      bufRead = new byte[lenghtRead];
      Exception exp = null;
     
      bool readed = true;
      NetworkStream requestInfo = client.GetStream();
      requestInfo.ReadTimeout = 10000;
     
     
        try
        {
          requestInfo.Write(buf, 0, buf.Length);
          //requestInfo.Bridge.DeviceChannel(deviceInfo).Read(bufRead, 0, lenghtRead);
          List<byte> x = new List<byte>();
          byte bLast = 0x0;
          while (readed)
          {
            int bl = (byte)requestInfo.ReadByte();
        
          
          byte b = Convert.ToByte(bl);
          if (b == 0xC0)
          {
            x.Add(b);
            while (readed)
            {
              b = (byte)requestInfo.ReadByte();
              if (b == 0xC0)
              {
                x.Add(b);
                bufRead = x.ToArray();
                readed = false;
                break;
              }
              else
              {
                if (bLast == 0xDB /*&& b == 0xDB*/)
                {
                  /*Если последовательность 0xDB, 0xDC - заменяем на 0xC0; При 0xDB, 0xDD - просто пропускаем ибо 0xDB и так принято*/
                  if (b == 0xDC)
                  {
                    x.RemoveAt(x.Count - 1);//Удалим последнее вхождение 0xDB
                    x.Add(0xC0);
                  }
                }
                else
                {
                  x.Add(b);
                }
                bLast = b;
              }
            }
          }
          else
            throw new Exception();
          }
          exp = null;
        }
        catch (Exception e)
        {
          exp = e;
          readed = false;
        return e;
          //continue;
        }
        //finally {
        //  if (client.Connected)
        //    client.Close();
        //}

      
      return exp;
    }

    public static byte getsrv(bool direct, byte classAccess, byte messageLength)
    {
      byte b = 0;
      if (direct)
      {
        b = (byte)1 << 7;
      }
      b = (byte)(b ^ (byte)classAccess << 4);
      b = (byte)(b ^ messageLength);
      return b;
    }

    public static byte[] packbuf(EnumFunEnMera function, byte[] message, byte lenght, ushort Addr_Prib)
    {
      ushort Addr_Loc = 0;
      byte[] bres = null;
      MemoryStream str = new MemoryStream();
      str.WriteByte(0xC0);
      str.WriteByte(0x48);
      str.Write(BitConverter.GetBytes(Addr_Prib), 0, 2);
      str.Write(BitConverter.GetBytes(Addr_Loc), 0, 2);
      str.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
      byte srv = getsrv(true, 5, lenght);
      str.WriteByte(srv);

      byte[] b_fun = BitConverter.GetBytes((short)function);
      str.WriteByte(b_fun[1]);
      str.WriteByte(b_fun[0]);
      if (lenght > 0)
      {
        str.Write(message, 0, lenght);
      }
      str.WriteByte(0x0);
      str.WriteByte(0xC0);
      bres = str.ToArray();

      byte cr = EnmeraCrc.crc(bres, 1, bres.Length - 2);
      bres[bres.Length - 2] = cr;

      //Проверка пакета на необходимость замен с ESC-вставками
      var mahByteArray = new List<byte>();

      mahByteArray.Add(0xC0);
      for (int i = 1; i < bres.Length - 1; i++)
      {
        if (bres[i] == 0xC0)
        {
          mahByteArray.Add(0xDB);
          mahByteArray.Add(0xDC);
        }
        else if (bres[i] == 0xDB)
        {
          mahByteArray.Add(0xDB);
          mahByteArray.Add(0xDD);
        }
        else
          mahByteArray.Add(bres[i]);
      }
      mahByteArray.Add(0xC0);

      return mahByteArray.ToArray();
      //return bres;
    }

  }

  public class EnmeraCrc
  {
    static byte[] crc8tab = new byte[]{
        0x00, 0xb5, 0xdf, 0x6a, 0x0b, 0xbe, 0xd4, 0x61, 0x16, 0xa3, 0xc9, 0x7c, 0x1d, 0xa8, 0xc2, 0x77, 0x2c, 0x99, 0xf3, 0x46, 0x27, 0x92, 0xf8, 0x4d, 0x3a, 0x8f, 0xe5, 0x50, 0x31, 0x84, 0xee, 0x5b, 0x58, 0xed, 0x87, 0x32, 0x53, 0xe6, 0x8c, 0x39, 0x4e, 0xfb, 0x91, 0x24, 0x45, 0xf0, 0x9a, 0x2f, 0x74, 0xc1, 0xab, 0x1e, 0x7f, 0xca, 0xa0, 0x15, 0x62, 0xd7, 0xbd, 0x08, 0x69, 0xdc, 0xb6, 0x03, 0xb0, 0x05, 0x6f, 0xda, 0xbb, 0x0e, 0x64, 0xd1, 0xa6, 0x13, 0x79, 0xcc, 0xad, 0x18, 0x72, 0xc7, 0x9c, 0x29, 0x43, 0xf6, 0x97, 0x22, 0x48, 0xfd, 0x8a, 0x3f, 0x55, 0xe0, 0x81, 0x34, 0x5e, 0xeb, 0xe8, 0x5d, 0x37, 0x82, 0xe3, 0x56, 0x3c, 0x89, 0xfe, 0x4b, 0x21, 0x94, 0xf5, 0x40, 0x2a, 0x9f, 0xc4, 0x71, 0x1b, 0xae, 0xcf, 0x7a, 0x10, 0xa5, 0xd2, 0x67, 0x0d, 0xb8, 0xd9, 0x6c, 0x06, 0xb3, 0xd5, 0x60, 0x0a, 0xbf, 0xde, 0x6b, 0x01, 0xb4, 0xc3, 0x76, 0x1c, 0xa9, 0xc8, 0x7d, 0x17, 0xa2, 0xf9, 0x4c, 0x26, 0x93, 0xf2, 0x47, 0x2d, 0x98, 0xef, 0x5a, 0x30, 0x85, 0xe4, 0x51, 0x3b, 0x8e, 0x8d, 0x38, 0x52, 0xe7, 0x86, 0x33, 0x59, 0xec, 0x9b, 0x2e, 0x44, 0xf1, 0x90, 0x25, 0x4f, 0xfa, 0xa1, 0x14, 0x7e, 0xcb, 0xaa, 0x1f, 0x75, 0xc0, 0xb7, 0x02, 0x68, 0xdd, 0xbc, 0x09, 0x63, 0xd6, 0x65, 0xd0, 0xba, 0x0f, 0x6e, 0xdb, 0xb1, 0x04, 0x73, 0xc6, 0xac, 0x19, 0x78, 0xcd, 0xa7, 0x12, 0x49, 0xfc, 0x96, 0x23, 0x42, 0xf7, 0x9d, 0x28, 0x5f, 0xea, 0x80, 0x35, 0x54, 0xe1, 0x8b, 0x3e, 0x3d, 0x88, 0xe2, 0x57, 0x36, 0x83, 0xe9, 0x5c, 0x2b, 0x9e, 0xf4, 0x41, 0x20, 0x95, 0xff, 0x4a, 0x11, 0xa4, 0xce, 0x7b, 0x1a, 0xaf, 0xc5, 0x70, 0x07, 0xb2, 0xd8, 0x6d, 0x0c, 0xb9, 0xd3, 0x66 };

    public static byte crc(byte[] buff, int offset, int len)
    {
      byte crc8 = 0;
      for (int i = offset; i < len; i++)
      {
        crc8 = crc8tab[crc8 ^ buff[i]];
      }
      return crc8;
    }
  }
}
