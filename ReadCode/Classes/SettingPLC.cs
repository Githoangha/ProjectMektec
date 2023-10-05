using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadCode
{
    public class SettingPLC
    {
        public static string IP_PLC { get; set; }
        public static string Port_PLC { get; set; }
        /// <summary>
        /// Trigger Đèn
        /// </summary>
        public static string TrigggerLamp { get; set; }
        /// <summary>
        /// Trigger nút bấm
        /// </summary>
        public static string TriggerButton { get; set; }
        /// <summary>
        /// Trigger PC ->PLC đã sẵn sàng
        /// </summary>
        public static string TriggerPCReady { get; set; }

        /// <summary>
        /// Trigger tín hiệu Sensor
        /// </summary>
        public static string TriggerSensor { get; set; }

        /// <summary>
        /// Trigger đọc code Error
        /// </summary>
        public static string TriggerError { get; set; }
        /// <summary>
        /// Trigger đọc code OK
        /// </summary>
        public static string TriggerOK { get; set; }
        /// <summary>
        /// Trigger PC ->PLC đã có Data
        /// </summary>
        public static string TriggerHaveData { get; set; }

        /// <summary>
        /// Trigger PLC gửi xác nhận đã nhận được TriggerHaveData
        /// </summary>
        public static string TriggerHaveDataOK { get; set; }

        /// <summary>
        /// Khai báo đường dẫn của máy FVI (Server của các máy còn lại)
        /// </summary>
        //public static string str_ConnectDBConffig_FVI_Server { get; set; }
    }
}
