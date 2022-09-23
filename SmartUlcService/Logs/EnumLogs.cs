using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace InterUlc.Logs
{
  public class EnumLogs
  {
    public enum LOG_LVL:byte
    {
      [Description("Отладка")]
      logDEBUG = 0,
      [Description("Инфо")]
      logINFO=1,
      [Description("Предупреждение")]
      logWARNING=2,
      [Description("Ошибка")]
      logERROR=3,
      [Description("Фатальная ошибка")]
      logFATAL=4,
      [Description("Отключено")]
      logNONE=5
    }

    internal enum LOG_TYPE
    {
      [Description("Регистрация")]
      Registration = 1, /**/
      [Description("Перезагрузка")]
      DeviceRestart = 2,
      [Description("Изменение конфигурации")]
      ConfigSettings = 3,
      [Description("Уровень сигнала:")]
      SignalStrength = 4,
      [Description("Управление контроллером")]
      ControlDevice = 5,
      [Description("Обновление прошивки")]
      DeviceUpdate = 6,
      [Description("Состояние связи")]
      PdpContext = 7,
      [Description("Событие по порту отладки")]
      SocketWorkDBG = 8,
      [Description("Событие по сквозному порту ")]
      SocketWorkTH = 9,
      [Description("Событие по порту МЭК")]
      SocketWorkIEC = 10,
      [Description("Событие по порту обновления")]
      SocketWorkUPD = 11,
      [Description("Причина закрытия МЭК")]
      IecClose = 12,
      [Description("RS485")]
      UartControl = 13,
      [Description("События по сокету патчинга")]
      SocketWorkPatch = 14,
      [Description("Загрузка патча")]
      PatchUpload = 15,
      [Description("Статус CEER")]
      ExtErrorReport = 16,
      [Description("Статус регистрации сети")]
      RegStatEvent = 17
    }

    internal enum RegistrationCode
    {
      [Description("Таймаут")]
      Timeout = 1,
      [Description("Ошибка PDP")]
      PdpFailure = 2,
      [Description("Загрузка системы")]
      StartBoot = 3,
      [Description("Успешный выход в работу")]
      StartSuccess = 4,
      [Description("Успешная перерегистрация сети")]
      ReRegistrSuccess = 5
    }

    internal enum RegStatEventCode
    {
      [Description("Нет регистрации, нет поиска оператора")]
      NotRegNotSearch = 0,
      [Description("Зарегистрирован, домашняя сеть")]
      RegisteredHomeNetwork = 1,
      [Description("Нет регистрации, поиск нового оператора")]
      NotRegSearchNew = 2,
      [Description("Регистрация отклонена")]
      RegDenied = 3,
      [Description("Неизвестный статус")]
      Unknown = 4,
      [Description("Зарегистрирован, роуминг")]
      RegRoaming = 5
    }


    internal enum RestartCode
    {
      [Description("Из конфигуратора")]
      ByConfig = 1,
      [Description("По звонку")]
      ByRing = 2,
      [Description("По порту 10555")]
      By10555 = 3,
      [Description("Отсутствие пинга")]
      ByPing = 4,
      [Description("По расписанию")]
      BySchedule = 5,
      [Description("Потеряно GSM соединение")]
      ByLostCon = 6,
      [Description("Ошибка запуска таски парсера команд")]
      errTaskCmdParse = 7,
      [Description("Ошибка запуска таски ат парсера")]
      errTaskAtParse = 8,
      [Description("Ошибка запуска таски логирования")]
      errTaskLogging = 9,
      [Description("Ошибка запуска таски учета трафика")]
      errTaskTraffic = 10,
      [Description("Ошибка считывания конфигурации при запуске")]
      errWrongConfig = 11,
      [Description("ошибка запуска таски менеджера событий")]
      errTaskCtrl = 12,
      [Description("Ошибка запуска таски планировщика освещения")]
      errTaskLightSched = 13,
      [Description("Ошибка создания таймера для GSM контроля")]
      errStartTimerGSM = 14,
      [Description("Ошибка создания таймера для перезагрузки системы")]
      errStartTimerReboot = 15,
      [Description("Ошибка запуска таски конфигурации")]
      errTaskDebug = 16,
      [Description("Ошибка запуска таски обновления прошивки")]
      errTaskUpdate = 17,
      [Description("Ошибка запуска таски сквозного канала")]
      errTaskTh = 18,
      [Description("Ошибка запуска таски контроля входов")]
      errTaskInput = 19,
      [Description("Ошибка запуска таски опроса модбас")]
      errTaskModbus = 20,
      [Description("Ошибка запуска таски мэк104")]
      errTaskIec = 21,
      [Description("Ошибка запуска таски пингования")]
      errTaskPing = 22,
      [Description("Ошибка запуска таски загрузки патча")]
      errTaskFileUp = 23
    }

    internal enum ConfigCode
    {
      [Description("Сброс до заводских настроек")]
      SetDefault = 1,
      [Description("Изменение пароля")]
      PassChange = 2,
      [Description("Сохранение основной конфигурации")]
      ConfigSave = 3,
      [Description("Сохранение конфигурации Modbus")]
      MbConfigSave = 4,
    }

    internal enum DeviceControlCode
    {
      [Description("Включение выхода из конфигуратора")]
      OutOnFromConfig = 1,
      [Description("Выключение выхода из конфигуратора")]
      OutOffFromConfig = 2,
      [Description("Изменение выхода через МЭК")]
      OutChangeByIec = 3,
      [Description("Изменение активации расписания через МЭК")]
      SchedChangeByIec = 4,
      [Description("Потеря питания")]
      PowerLoss = 5,
      [Description("Настройка расписания перезагрузки через консоль")]
      ChangeSchedReboot = 6,
      [Description("Восстановление подачи питания")]
      PowerRecovery = 7,
      [Description("Переключение яркости")]
      BrightToggle = 8,
      [Description("Отключение перезагрузки по расписанию")]
      DisactSchedReboot = 9,
      [Description("Включение выхода по расписанию")]
      OutOnBySched = 10,
      [Description("Выключение выхода по расписанию")]
      OutOffBySched = 11,
      [Description("Изменение модбас вывода через мэк104")]
      ChangeModbusDO = 12,
      [Description("Успешная проверка пинга")]
      PingOk = 13,
      [Description("Создание таски парсера команд - успех")]
      taskOkCmdParse = 14,
      [Description("Создание таски парсера ат команд - успех")]
      taskOkAtParse = 15,
      [Description("Создание таски логирования - успех")]
      taskOkLogging = 16,
      [Description("Создание таски учета трафика - успех")]
      taskOkTrafic = 17,
      [Description("Создание таски менеджера событий - успех")]
      taskOkCtrl = 18,
      [Description("Создание таски планировщика освещения - успех")]
      taskOkLightSched = 19,
      [Description("Создание таймера контроля GSM - успех")]
      startOkTimerGSM = 20,
      [Description("Создание таски конфигурации - успех")]
      startOkDebug = 21,
      [Description("Создание таски загрузки обновления - успех")]
      startOkUpdate = 22,
      [Description("Создание таски сквозного канала - успех")]
      startOkTh = 23,
      [Description("Создание таски контроля входов - успех")]
      startOkInputs = 24,
      [Description("Создание таски опроса модбас - успех")]
      startOkModbus = 25,
      [Description("Создание таски мэк104 - успех")]
      startOkIec = 26,
      [Description("Создание таски пингования - успех")]
      startOkPing = 27,
      [Description("Создание таски загрузки патча - успех")]
      startOkFileUp = 28,
      [Description("Левый пинг")]
      LeftPing = 29,
      [Description("рассинхрон времени:")]
      TimeDiff = 30
    }



    internal enum UpdateCode
    {
      [Description("Начало обновления")]
      Start = 1,
      [Description("Обновление успешно")]
      Success = 2,
      [Description("Ошибка при обновлении")]
      Error = 3,
    }

    internal enum PdpCode
    {
      [Description("Активация сети")]
      Active = 1,
      [Description("Отмена активации")]
      Break = 2,
      [Description("Отключение сети")]
      Deactive = 3,
      [Description("Ошибка активации")]
      ActivationFail = 4,
      [Description("Активация по IP6")]
      Ipv6Active = 5,
    }

    #region code of socket error
    //кодировка ошибок сокета в телит EU866
    //#define M2M_SOCKET_BSD_SOCKNOERROR            0
    //#define M2M_SOCKET_BSD_EUNDEFINED             1
    //#define M2M_SOCKET_BSD_EACCES                 2
    //#define M2M_SOCKET_BSD_EADDRINUSE             3
    //#define M2M_SOCKET_BSD_EADDRNOTAVAIL          4
    //#define M2M_SOCKET_BSD_EAFNOSUPPORT           5
    //#define M2M_SOCKET_BSD_EALREADY               6
    //#define M2M_SOCKET_BSD_EBADF                  7
    //#define M2M_SOCKET_BSD_ECONNABORTED           8
    //#define M2M_SOCKET_BSD_ECONNREFUSED           9
    //#define M2M_SOCKET_BSD_ECONNRESET            10
    //#define M2M_SOCKET_BSD_EDESTADDRREQ          11
    //#define M2M_SOCKET_BSD_EFAULT                12
    //#define M2M_SOCKET_BSD_EHOSTDOWN             13
    //#define M2M_SOCKET_BSD_EHOSTUNREACH          14
    //#define M2M_SOCKET_BSD_EINPROGRESS           15
    //#define M2M_SOCKET_BSD_EINTR                 16
    //#define M2M_SOCKET_BSD_EINVAL                17
    //#define M2M_SOCKET_BSD_EISCONN               18
    //#define M2M_SOCKET_BSD_EMFILE                19
    //#define M2M_SOCKET_BSD_EMSGSIZE              20
    //#define M2M_SOCKET_BSD_ENETDOWN              21
    //#define M2M_SOCKET_BSD_ENETRESET             22
    //#define M2M_SOCKET_BSD_ENETUNREACH           23
    //#define M2M_SOCKET_BSD_ENOBUFS               24
    //#define M2M_SOCKET_BSD_ENOPROTOOPT           25
    //#define M2M_SOCKET_BSD_ENOTCONN              26
    //#define M2M_SOCKET_BSD_ENOTSOCK              27
    //#define M2M_SOCKET_BSD_EOPNOTSUPP            28
    //#define M2M_SOCKET_BSD_EPFNOSUPPORT          29
    //#define M2M_SOCKET_BSD_EPROTONOSUPPORT       30
    //#define M2M_SOCKET_BSD_EPROTOTYPE            31
    //#define M2M_SOCKET_BSD_ESHUTDOWN             32
    //#define M2M_SOCKET_BSD_ESOCKTNOSUPPORT       33
    //#define M2M_SOCKET_BSD_ETIMEDOUT             34
    //#define M2M_SOCKET_BSD_EWOULDBLOCK           35

    internal enum LogCode
    {

      eLC_NONE = 0,
      //Registration-----------------
      //eLC_R_RegTimeout = 1,
      //eLC_R_PdpFailure = 2,
      //ConfigSettings---------------
      //eLC_CS_SetDefault = 1,
      //eLC_CS_PassChange = 2,
      //eLC_CS_ConfigSave = 3,
      //ELC_CS_MbConfigSave = 4,
      //DeviceRestart----------------
      //eLC_DR_ByConfig = 1,
      //eLC_DR_ByRing = 2,
      //eLC_DR_By10555 = 3,
      //eLC_DR_ByPing = 4,
      //eLC_DR_BySchedule = 5,
      //eLC_DR_ByLostCon = 6,
      //PassChange-------------------
      eLC_PC_Success = 1,
      //HandControlOuts--------------
      //eLC_HC_OutOnFromConfig = 1,
      //eLC_HC_OutOffFromConfig = 2,
      //eLC_HC_OutChangeByIec = 3,
      //eLC_HC_SchedChangeByIec = 4,
      //eLC_HC_PowerLoss = 5,
      //SocketWork-------------------
      //eLC_SW_ConnectSuccess = 1,
      //eLC_SW_DisconSuccess = 2,
      //eLC_SW_DisconByError = 3,
      //eLC_SW_DisconByTimeout = 4,
      //eLC_SW_BindError = 5,
      //eLC_SW_SocCreateFailure = 6,
      //eLC_SW_SocListenFailure = 7,
      //eLC_SW_SocConnectError = 8,
      //IecClose----------------------
      eLC_IC_ByTimerT1 = 1,
      eLC_IC_ByTimerT3 = 2,
      eLC_IC_ByErrorPack = 3,
      //DeviceUpdate------------------
      //eLC_DU_Start = 1,
      //eLC_DU_Success = 2,
      //eLC_DU_Error = 3,
    }
    #endregion
    internal enum SocketWorkCode
    {
      [Description("Клиент подключен")]
      ConnectSuccess = 1,
      [Description("Клиент отключен")]
      DisconSuccess = 2,
      [Description("Отключено с ошибкой")]
      DisconByError = 3,
      [Description("Отключен по таймауту при простое")]
      DisconByTimeout = 4,
      [Description("Ошибка связывания серверного сокета")]
      BindError = 5,
      [Description("Ошибка при создании серверного сокета")]
      SocCreateFailure = 6,
      [Description("Ошибка прослушивания сокета")]
      SocListenFailure = 7,
      [Description("Ошибка при подключении клиента")]
      SocConnectError = 8,
    }

    internal enum IecCloseCode
    {
      [Description("По отработке таймера Т1")]
      eLC_IC_ByTimerT1 = 1,
      [Description("По отработке таймера Т3")]
      eLC_IC_ByTimerT3 = 2,
      [Description("По нарушению формата протокола")]
      eLC_IC_ByErrorPack = 3,
    }

    internal enum UartControlCode
    {
      [Description("Таймаут по ожиданию ответа")]
      eLC_UC_Timeout = 1,
      [Description("ошибка CRC пакета Modbus")]
      eLC_UC_MbCrcError = 2,
      [Description("Переинициализация RS485")]
      eLC_UC_Reinit = 3,
      [Description("Некорректный запрос Modbus")]
      eLC_UC_MbReqError = 4,
      [Description("Ошибка инициализации RS485")]
      eLC_UC_InitError = 5,
    }



    private static object GetCustomAtribute(Type srcField, string nameField, Type findAttr)
    {
      object result = null;
      foreach (FieldInfo fi in srcField.GetFields())
      {
        if (fi.Name.Equals(nameField))
        {
          foreach (Attribute atr in fi.GetCustomAttributes(findAttr, false))
          {
            return atr;
          }
          break;
        }
      }
      return result;
    }
    public static string GetDescription(Enum e)
    {
      string desc = ((DescriptionAttribute)(GetCustomAtribute(e.GetType(), e.ToString(), typeof(DescriptionAttribute)))).Description;
      return desc;
    }
  }
}

