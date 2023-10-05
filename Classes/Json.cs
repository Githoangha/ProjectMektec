using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace LineGolden_PLasma
{
    #region class OEE
    public class OEE
    {
        public string DeviceID { get; set; }
        public string StateID { get; set; }
        public string Value { get; set; }
        public string Timestamp { get; set; }

        public void SetParam(string deviceID, string stateID, string value, string time)
        {
            try
            {
                this.DeviceID = deviceID;
                this.StateID = stateID;
                this.Value = value;
                this.Timestamp = DateTime.Parse(time).ToString("yyyy-MM-ddTHH:mm:sszzz");
            }
            catch(Exception ex)
            {
                Lib.SaveToLog(ex.ToString());
            }
            
        }

    }

    #endregion

    #region class MESbarcode
    public class MESBarcode
    {
        public string DeviceID { get; set; }
        public string StateID { get; set; }
        public Key Key { get; set; }
        public ValueBarcode Value { get; set; }
        public string Timestamp { get; set; }
        public void SetParam(string deviceID, string stateID, Key key, ValueBarcode value, string time)
        {
            this.DeviceID = deviceID;
            this.StateID = stateID;
            this.Key = key;
            this.Value = value;
            this.Timestamp = time;
        }
    }

    public class ValueBarcode
    {
        public string IN_CodeJig { get; set; }
        public string IN_CodePcs { get; set; }

        public void SetParam(string codeJig,string codePcs)
        {
            this.IN_CodeJig = codeJig;
            this.IN_CodePcs = codePcs;
        }
    }

    #endregion


    #region class MESplc
    public class MESplc
    {
        public string DeviceID { get; set; }
        public string StateID { get; set; }
        public Key Key { get; set; }
        public ValuePlc Value { get; set; }
        public string Timestamp { get; set; }

        public void SetParam(string deviceID,string stateID,Key key,ValuePlc value,string time)
        {
            this.DeviceID = deviceID;
            this.StateID = stateID;
            this.Key = key;
            this.Value = value;
            this.Timestamp = time;
        }
    }

    public class Key
    {
        public string LineID { get; set; }
        public string RouteID { get; set; }
        public string LotID { get; set; }

        public void SetParam(string lineID,string routeID,string LotID)
        {
            this.LineID = lineID;
            this.RouteID = routeID;
            this.LotID = LotID;
        }

    }

    public class ValuePlc
    {
        public string IN_SPEED_RUNNING { get; set; }
        public string IN_COLUMN_NUM_OF_TRAY_IN { get; set; }
        public string IN_BLOCK_NUM_TRAY_IN { get; set; }
        public string IN_DISTANCE_BLOCK_TRAY_IN { get; set; }
        public string IN_DISTANCE_TOOL_TRAY_IN { get; set; }
        public string IN_DISTANCE_BETWEEN_TRAY_IN { get; set; }
        public string IN_DISTANCE_BETWEEN_TRAY_OUT { get; set; }
        public string IN_ROW_NUM_OF_TRAY_OUT { get; set; }
        public string IN_COLUMN_NUM_OF_TRAY_OUT { get; set; }
        public string IN_DISTANCE_ROW_TRAY_OUT { get; set; }
        public string IN_DISTANCE_COLUMN_TRAY_OUT { get; set; }
        public string IN_DELAY_TIME_VACCUM_TOOL_ROBOT { get; set; }
        public string IN_DELAY_TOOL_UP { get; set; }
        public string IN_DELAY_TIME_AIR_BLOW_TOOL_ROBOT { get; set; }
        public string IN_DELAY_TOOL_DOWN { get; set; }

    }


    #endregion


    #region class Value Default của Json 
    public class JsonValueDE
    {
        public static string stateProcess = "PROCESSING";
        public static string stateWait = "WAIT";
        public static string statePower = "POWER";
        public static string stateError = "Error";
        public static string stateCT = "CT";
        public static string stateAdd = "Add";
        public static string ON = "1";
        public static string OFF = "0";
    }

    #endregion

    #region class Hàm Json 
    public class JsonFunc
    {
        #region Function Upload data OEE to server
        public static void OEEsubmit(string ip, string port, string mod, OEE item, out string msg)
        {
            try
            {
                if (GlobVar.UploadServer)
                {
                    string url = $"http://{ip}:{port}/DataUpload/{mod}";
                    msg = HttpPost(url, item);
                }
                else
                {
                    msg = JsonConvert.SerializeObject(item).Replace("\n", "");
                    string[] temp = msg.Split(',');
                    msg = string.Join(",\r", temp);
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
        }

        /// <summary>
        /// hàm OEE Submit rút gọn
        /// </summary>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        public static void OEEsubmit(OEE item, out string msg)
        {
            try
            {
                if (GlobVar.UploadServer)
                {
                    string url = $"http://{GlobVar.IPServer}:{GlobVar.PortServer}/DataUpload/{GlobVar.MODOEE}";
                    msg = HttpPost(url, item);
                }
                else
                {
                    msg = JsonConvert.SerializeObject(item).Replace("\n", "");
                    string[] temp = msg.Split(',');
                    msg = string.Join(",\r", temp);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        #endregion

        #region Function Upload MES_PLC to server
        public static void MES_PLCsubmit(string ip, string port, string mod, MESplc item, out string msg)
        {
            try
            {
                if (GlobVar.UploadServer)
                {
                    string url = $"http://{ip}:{port}/DataUpload/{mod}";
                    msg = HttpPost(url, item);
                }
                else
                {
                    msg = JsonConvert.SerializeObject(item).Replace("\n", "");
                    string[] temp = msg.Split(',');
                    msg = string.Join(",\r", temp);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        /// <summary>
        /// Hàm MES Submit rút gọn
        /// </summary>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        public static void MES_PLCsubmit(MESplc item, out string msg)
        {
            try
            {
                if (GlobVar.UploadServer)
                {
                    string url = $"http://{GlobVar.IPServer}:{GlobVar.PortServer}/DataUpload/{GlobVar.MODMES}";
                    msg = HttpPost(url, item);
                }
                else
                {
                    msg = JsonConvert.SerializeObject(item).Replace("\n", "");
                    string[] temp = msg.Split(',');
                    msg = string.Join(",\r", temp);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        #endregion

        #region Upload Mes_Barcode to server

        public static void MES_Barcodesubmit(string ip, string port, string mod, MESBarcode item, out string msg)
        {
            try
            {
                if (GlobVar.UploadServer)
                {
                    string url = $"http://{ip}:{port}/DataUpload/{mod}";
                    msg = HttpPost(url, item);
                }
                else
                {
                    msg = JsonConvert.SerializeObject(item).Replace("\n", "");
                    string[] temp = msg.Split(',');
                    msg = string.Join(",\r", temp);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        /// <summary>
        /// Hàm MES Submit rút gọn
        /// </summary>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        public static void MES_Barcodesubmit(MESBarcode item, out string msg)
        {
            try
            {
                if (GlobVar.UploadServer)
                {
                    string url = $"http://{GlobVar.IPServer}:{GlobVar.PortServer}/DataUpload/{GlobVar.MODMES}";
                    msg = HttpPost(url, item);
                }
                else
                {
                    msg = JsonConvert.SerializeObject(item).Replace("\n", "");
                    string[] temp = msg.Split(',');
                    msg = string.Join(",\r", temp);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        #endregion

        public static string HttpPost(string url, Object ticket)
        {

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(ticket.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, ticket);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            string param = Encoding.UTF8.GetString(dataBytes);//为满足格式要求，外层追加中括号
            byte[] bs = Encoding.ASCII.GetBytes(param);
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/json");
            byte[] responseData = wc.UploadData(url, "post", bs);
            wc.Dispose();
            return Encoding.UTF8.GetString(responseData);

        }

        #endregion
    }

}
