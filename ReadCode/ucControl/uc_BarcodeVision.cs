using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using AYNETTEK.UHFReader;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace ReadCode
{
    public partial class uc_BarcodeVision : UserControl
    {

        public int _ProgramID { get; set; }

        private int _MachineIndex = 999;
        public string IpCam { get; set; }
        public int PortCam { get; set; }
        public bool _IsConected { get; set; }
        public bool IsComplete { get; set; }
        public bool IsExecuteFunctionReadTag { get; set; }

        public Thread Thread_ReadBarcode, _AliveTCP;


        private Socket BarcodeReader;


        //Array 2 TagJig
        public string[] arrTag;

        //String values Barcode send
        public string values = "";


        public uc_BarcodeVision()
        {
            InitializeComponent();
            //_ProgramID = PrgID;
            this.Dock = DockStyle.Fill;
            lb_Status.BackColor = Color.Red;
            lb_Status.Text = "Disconnect";
        }
        /// <summary>
        /// Thread Check Alive Tcp/IP
        /// </summary>
        public void Main_Thread_AliveTCP()
        {
            // NB - 19032023
            _AliveTCP = new Thread(new ThreadStart(ReconnectBarcodeVision));
            _AliveTCP.IsBackground = true;
            _AliveTCP.Start();
        }

        /// <summary>
        /// lấy dữ liệu theo program_Id để kết nối với barcode
        /// </summary>
        public void ConnectBarcodeVision()
        {
            try
            {
                //Set values connect is false
                _IsConected = false;
                //Tải dữ liệu setting Barcode theo Program
                DataTable dt = Support_SQL.GetTableData($"Select * from BarcodeSetting where ID_Program = {_ProgramID} limit 1");
                if (dt.Rows.Count <= 0)
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Program is currently not config setting Barcode:Vision").ShowDialog();
                    return;
                }
                DataRow row = dt.Rows[0];
                IpCam = row["Ip_Barcode"].ToString().Trim();
                PortCam = Support_SQL.ToInt(row["Port_Barcode"]);

                // Kết nối tới Barcode lấy thông tin từ Database
                Connect(IpCam, PortCam);
            }
            catch (Exception)
            {
                _IsConected = false;
            }
            btn_SettingBarcode.Enabled = !_IsConected;
        }
        /// <summary>
        ///  Support Connect 
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        private void Connect(string Ip, int Port)
        {
            try
            {
                if (BarcodeReader != null)
                {
                    BarcodeReader.Close();
                    BarcodeReader.Dispose();
                    BarcodeReader = null;
                }
                BarcodeReader = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress _Ip = IPAddress.Parse(Ip);
                IPEndPoint _endPoint = new IPEndPoint(_Ip, Port);
                BarcodeReader.Connect(_endPoint);
                if (!BarcodeReader.Connected)
                {
                    StatusLable(lb_Status, "Disconnect", Color.Red);
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Connection Barcode failed!").ShowDialog();
                    return;
                }
                _IsConected = true;
                // NB - 19032023
                Main_Thread_AliveTCP();
                // đổi màu trạng thái
                StatusLable(lb_Status, "Connected", Color.Lime);
            }
            catch (Exception ex)
            {
                StatusLable(lb_Status, "Disconnect", Color.Red);
                _IsConected = false;
                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Connection Barcode failed!").ShowDialog();
                return;
            }
        }

        public void DisconnectBarcodeVision()
        {
            try
            {
                if (BarcodeReader != null || BarcodeReader.Connected)
                {
                    _AliveTCP?.Abort();
                    Thread_ReadBarcode?.Abort();
                    BarcodeReader.Close();
                    BarcodeReader.Dispose();
                    BarcodeReader = null;
                    _IsConected = false;
                    StatusLable(lb_Status, "Disconnect", Color.Red);
                    btn_SettingBarcode.Enabled = !_IsConected;
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Disable nút bấm setting
        /// </summary>
        public void DisableSettingButton()
        {
            btn_SettingBarcode.Enabled = false;
        }

        /// <summary>
        /// Enable nút bấm setting
        /// </summary>
        public void EnableSettingButton()
        {
            btn_SettingBarcode.Enabled = true;
        }
        private void bnt_SettingBarcode_Vision_Click(object sender, EventArgs e)
        {
            frm_SettingCamBarcode frm = new frm_SettingCamBarcode();
            frm.ProgramID = _ProgramID;
            frm.MachineIndex = _MachineIndex;
            frm.ShowDialog();
        }
        /// <summary>
        /// Read 1 CodeJig
        /// </summary>
        public void RunReadTag()
        {
            string tempString = string.Empty;
            if (BarcodeReader != null && BarcodeReader.Connected)
            {
                try
                {
                    values = string.Empty;
                    byte[] buffer = new byte[1024];
                    BarcodeReader.Receive(buffer);
                    tempString = Encoding.ASCII.GetString(buffer).Replace("\0", "").Replace("\u0002", "").Replace("\u0003", "").Replace("\r", "").Replace("\n", "");
                    if (!string.IsNullOrEmpty(tempString))
                    {
                        values = tempString.Trim();
                        return;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        string List_result = "";
        /// <summary>
        /// Read 2 CodeJig
        /// </summary>
        public void RunRead_List_Tag()
        {
            string tempData = "";
            string tempString = "";
            List_result = "";
            while (_IsConected)
            {
                Thread.Sleep(100);
                if (BarcodeReader != null && BarcodeReader.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        BarcodeReader.Receive(buffer);
                        tempString = Encoding.ASCII.GetString(buffer).Replace("\0", "").Replace("\u0002", "").Replace("\u0003", "").Replace("\r", "").Replace("\n", "");
                        if (!string.IsNullOrEmpty(tempString))
                        {
                            List_result = tempString;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }

        /// <summary>
        /// Return Result Code when Read 1 CodeJig
        /// </summary>
        /// <returns></returns>
        public string ResultBarcode()
        {
            return values;
        }
        /// <summary>
        /// Return Result Code when Read 2 CodeJig
        /// </summary>
        /// <returns></returns>
        public List<string> ResultListBarcode()
        {
            List<string> List_CodeJig = new List<string>();
            List_CodeJig = List_result.Split('@').ToList();
            for (int i = 0; i < List_CodeJig.Count; i++)
            {
                if (List_CodeJig[i].Trim() == "")
                {
                    List_CodeJig.RemoveAt(i);
                }
            }
            return List_CodeJig;
        }

        /// <summary>
        /// If connect successful  return true else  fales
        /// </summary>
        /// <param name="s"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        private bool Alive(Socket s, string Ip, int Port)
        {
            try
            {
                IPAddress _Ip = IPAddress.Parse(Ip);
                IPEndPoint _endPoint = new IPEndPoint(_Ip, Port);
                s.Connect(_endPoint);
                if (s == null || !s.Connected)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void StatusLable(Label l, string s, Color c)
        {
            if (l.InvokeRequired)
            {
                l.Invoke(new Action(() =>
                {
                    l.BackColor = c;
                    l.Text = s;
                }));
            }
            else
            {
                l.BackColor = c;
                l.Text = s;
            }
        }
        /// <summary>
        /// tạo 1 kết nối mới  check Alive TCP
        /// </summary>
        public void AliveTCP()
        {
            while (true)
            {
                Thread.Sleep(3000);
                Socket alive_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); ;

                if (!Alive(alive_Socket, IpCam, PortCam))
                {
                    StatusLable(lb_Status, "Disconnect", Color.Red);
                    _IsConected = false;

                }
                else
                {
                    return;
                }
                alive_Socket.Close();
                alive_Socket.Dispose();
                Connect(IpCam, PortCam);
            }
        }

        // NB - 19032023
        bool IsConnect_Faild;
        private void ReconnectBarcodeVision()
        {
            while (c_varGolbal.IsRun)
            {
                try
                {
                    Thread.Sleep(100);
                    Ping ping = new Ping();
                    byte[] buffer = Encoding.ASCII.GetBytes("samplestring");
                    int timeout = 120;
                    PingReply pingresult = ping.Send(IpCam, timeout, buffer);
                    if (pingresult.Status.ToString().ToLower() == "success")
                    {
                        if (IsConnect_Faild)
                        {
                            Thread.Sleep(500);
                            Connect(IpCam, PortCam);
                            if (BarcodeReader.Connected)
                                IsConnect_Faild = false;
                        }
                    }
                    else
                    {
                        if (!IsConnect_Faild)
                        {
                            IsConnect_Faild = true;
                            //Kiểm tra Barcode Vision đang có kết nối thì sẽ ngắt kết nối
                            DisconnectBarcodeVision();
                            StatusLable(lb_Status, "Disconnect", Color.Red);
                            Support_SQL.saveToLog($"Don't connect Barcode Vision, time {DateTime.Now}");
                        }
                    }
                    if (ping != null)
                    {
                        ping.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Support_SQL.saveToLog(ex.ToString() + "\r" + ex.Message);
                }
            }
        }

        /// <summary>
        /// trigger barcode
        /// </summary>
        /// <param name="bytesend"></param>
        public void SendSignal(byte[] bytesend)
        {
            if (BarcodeReader == null) return;
            if (BarcodeReader.Connected && BarcodeReader != null) BarcodeReader.Send(bytesend);
        }

    }
}
