using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using HalconDotNet;
using System.Threading;
using System.IO;
using System.Diagnostics;
using MC_Protocol_NTT;
using AYNETTEK.UHFReader;
using System.Xml;
using System.Runtime.InteropServices;
using log4net;
using WaitWnd;
using DatabaseInterface_MMCV;
using System.Net.NetworkInformation;
using Timer = System.Windows.Forms.Timer;

namespace ReadCode
{
    public partial class frm_Main : Form
    {
        // Cho phép di chuyển Form Main
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        // --------------------------------------------------------------------------------------------------------------------//
        // list process auto
        List<dataVision> lst_dataVision = new List<dataVision>();
        Dictionary<string, List<dataVision>> Dictionary_InfoBarcode = new Dictionary<string, List<dataVision>>();
        DataTable _dtProgram;
        // interface
        List<uc_Vision> lst_CameraReader = new List<uc_Vision>();
        uc_BarcodeVision Barcode_Vision = new uc_BarcodeVision();

        #region variable Excel
        // Excel
        SupportExcel Excel = new SupportExcel();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion
        //Class 
        WorkerThreadAwaitVC3000 Thread_VC3000 = new WorkerThreadAwaitVC3000();

        //PLC
        MC_Protocol PLC_Fx5 = new MC_Protocol();

        private Thread Main_VisonReader;
        private int ID_PrgCurrent = -1;
        private string status_Machine = "STOP";
        private string code_TagJigCurent;
        private string code_TagJigNonePCS;
        private string code_TagJigHavePCS;
        /// Log Debug Program
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frm_Main()
        {
            InitializeComponent();
            dgv_VisionReadcode.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E1F3FF");
        }

        /// <summary>
        /// Form Main Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Main_Load(object sender, EventArgs e)
        {
            // NB - 19032023

            c_varGolbal.IP_PLC = "192.168.125.40";
            c_varGolbal.Port_PLC = 2000;

            //Thread_Time
            Support_SQL.ViewTimeLabel(lbViewTime);
            c_varGolbal.NoReadString = "NoRead";
            c_varGolbal.CodeType = "Data Matrix ECC 200";

            #region load file SettingMachine.xml
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("SettingMachine.xml");
                XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/Setting");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    c_varGolbal.IP_PLC = xmlNode.SelectSingleNode("IP_PLC").InnerText.Trim();
                    c_varGolbal.Port_PLC = Support_SQL.ToInt(xmlNode.SelectSingleNode("Port_PLC").InnerText.Trim());
                    c_varGolbal.CodeType = xmlNode.SelectSingleNode("Code_Read_Type").InnerText;
                    c_varGolbal.NoReadString = xmlNode.SelectSingleNode("Str_NoRead").InnerText;
                    c_varGolbal.NameMachine = xmlNode.SelectSingleNode("Name_Machine").InnerText;
                    //c_varGolbal.RouteID = xmlNode.SelectSingleNode("RouteID").InnerText;
                    //txt_MPN.Text = xmlNode.SelectSingleNode("MPN").InnerText;
                    // NB - 03042023
                    c_varGolbal.str_MachineVersion = xmlNode.SelectSingleNode("Version").InnerText;
                }
            }
            catch (Exception Ex)
            {
                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Error Process Loading Data Setting! \r {Ex}").ShowDialog();
            }
            // --------------------------------------------------------------------------------------------------------------------//
            #endregion

            #region load file Config.xml
            try
            {
                XmlDocument xmlCF = new XmlDocument();
                xmlCF.Load("Config.xml");
                XmlNodeList xmlListCF = xmlCF.DocumentElement.SelectNodes("/Config");
                foreach (XmlNode xmlNode in xmlListCF)
                {
                    c_varGolbal.IP_SMT = xmlNode.SelectSingleNode("IP_SERVER_SMT").InnerText;//HA_ADD
                    txt_LineID.Text = xmlNode.SelectSingleNode("LineId").InnerText;
                    c_varGolbal.LineID = xmlNode.SelectSingleNode("LineId").InnerText;
                    c_varGolbal.DeviceID = xmlNode.SelectSingleNode("DeviceId").InnerText;
                    c_varGolbal.RouteID = xmlNode.SelectSingleNode("RouteId").InnerText;

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Error Process Loading Infor IP_SERVER_SMT or Line ID or Device ID \r " + Ex.ToString(), "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // --------------------------------------------------------------------------------------------------------------------//
            #endregion

            // NB - 19032023
            lblMachineName_version.Text = c_varGolbal.str_MachineVersion;
            txt_Machine.Text = c_varGolbal.NameMachine;
            loadProgram(-1);
            btnRunOrStop.BackColor = Color.Green;
            QuyenSuDung();
        }

        /// <summary>
        /// Form Main Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisconnectAllDevice();
        }
        /// <summary>
        /// Button Run Or Stop Machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunOrStop_Click(object sender, EventArgs e)
        {
            if (status_Machine == "STOP")
            {
                //Reset isTest
                isTest = false;
                // KIỂM TRA CÁC ĐIỀU KIỆN ĐỂ START CHƯƠNG TRÌNH
                if (cboProgram.SelectedValue == null)
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, "Please select the program !").ShowDialog();
                    return;
                }
                if (txt_StaffID.Text == "Missing" || txt_StaffID.Text == "" || txt_StaffID.Text == null)
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, "Please input the StaffID !").ShowDialog();
                    return;
                }
                if (string.IsNullOrEmpty(txtLot_ID.Text) || txtLot_ID.Text == "Missing")
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, "Please input the LotID !").ShowDialog();
                    return;
                }

                #region KIỂM TRA KẾT NỐI SERVER NHÀ MÁY
                try
                {
                    DAL MMCV_DBGetMPN = new DAL();
                    c_varGolbal.MPN = MMCV_DBGetMPN.GetMPN(txtLot_ID.Text);
                    txtMPN.Text = c_varGolbal.MPN;
                }
                catch (Exception Ex)
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Error Get MPN from Sever \r {Ex}").ShowDialog();
                    return;
                }
                if (string.IsNullOrEmpty(c_varGolbal.MPN))
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, "Error Get MPN from Sever !").ShowDialog();
                    return;
                }
                //// --------------------------------------------------------------------------------------------------------------------//
                #endregion

                #region Kiểm tra số lượng Roi Setting

                DataTable dtCam1 = Support_SQL.GetTableData(string.Format(@"select * from RegionPosition where ID_Program ={0} AND CamIndex={1} ORDER by Position", ID_PrgCurrent, 1));
                DataTable dtCam2 = Support_SQL.GetTableData(string.Format(@"select * from RegionPosition where ID_Program ={0} AND CamIndex={1} ORDER by Position", ID_PrgCurrent, 2));
                int cam1 = dtCam1.Rows.Count;
                int cam2 = dtCam2.Rows.Count;
                if (c_varGolbal.QtyPcs > (cam1 + cam2))
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, $"Error Thiếu data Pcs so với số lượng đã cài đặt! \r\n Pcs Setting:{c_varGolbal.QtyPcs}\r\nCam1:{cam1}\r\nCam2:{cam2}").ShowDialog();
                    return;
                }
                else if (c_varGolbal.QtyPcs < (cam1 + cam2))
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, $"Error Thừa data Pcs so với số lượng đã cài đặt! \r\n Pcs Setting:{c_varGolbal.QtyPcs}\r\nCam1:{cam1}\r\nCam2:{cam2}").ShowDialog();
                    return;
                }
                #endregion

                // KẾT NỐI CAMERA
                switch (lb_ReadCodePCS.Text)
                {
                    case "Enable":
                        //log.Debug("Connect to camera in Readcode machine");
                        lbStatus_BOT.Text = "Connect to camera in Readcode machine";
                        for (int i = 0; i < lst_CameraReader.Count; i++)
                        {
                            if (!lst_CameraReader[i]._IsConected)
                            {
                                lst_CameraReader[i].ConnectCamera();
                            }
                            else
                            {
                                lst_CameraReader[i].DisconnectCamera();
                                Thread.Sleep(100);
                                lst_CameraReader[i].ConnectCamera();
                            }
                            // if a device not conneced -> Disconnect all device 
                            if (!lst_CameraReader[i]._IsConected)
                            {
                                DisconnectAllDevice();
                                return;
                            }
                        }
                        break;
                    case "Disable":
                        break;
                    default:
                        break;
                }

                //KẾT NỐI BARCODE Ở MÁY ĐỌC CODE
                //log.Debug("Connect to Barcode in Readcode machine");
                lbStatus_BOT.Text = "Connect to Barcode in Readcode machine";
                if (!Barcode_Vision._IsConected)
                {
                    Barcode_Vision.ConnectBarcodeVision();
                }
                else
                {
                    Barcode_Vision.DisconnectBarcodeVision();
                    Thread.Sleep(100);
                    Barcode_Vision.ConnectBarcodeVision();
                }
                if (!Barcode_Vision._IsConected)
                {
                    DisconnectAllDevice();
                    return;
                }

                lbStatus_BOT.Text = "Connect to PLC";
                if (PLC_Fx5 != null)
                {
                    PLC_Fx5.Disconnect();
                    PLC_Fx5 = null;
                }
                try
                {
                    PLC_Fx5 = new MC_Protocol();
                    PLC_Fx5.IP_Adress = c_varGolbal.IP_PLC;
                    PLC_Fx5.Port = c_varGolbal.Port_PLC;
                    PLC_Fx5.Connect();
                    // NB - 19032023
                    if (PLC_Fx5.Connected)
                    {
                        if (lblStatusConnectPLC.InvokeRequired)
                        {
                            lblStatusConnectPLC.Invoke(new Action(() =>
                            {
                                lblStatusConnectPLC.BackColor = Color.Lime;
                                lblStatusConnectPLC.Text = "Connected";
                            }));
                        }
                        else
                        {
                            lblStatusConnectPLC.BackColor = Color.Lime;
                            lblStatusConnectPLC.Text = "Connected";
                        }
                    }
                    else
                    {
                        if (lblStatusConnectPLC.InvokeRequired)
                        {
                            lblStatusConnectPLC.Invoke(new Action(() =>
                            {
                                lblStatusConnectPLC.BackColor = Color.Red;
                                lblStatusConnectPLC.Text = "Disconnect";
                            }));
                        }
                        else
                        {
                            lblStatusConnectPLC.BackColor = Color.Red;
                            lblStatusConnectPLC.Text = "Disconnect";
                        }
                        DisconnectAllDevice();
                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Connect To PLC Fail").ShowDialog();
                        return;
                    }
                }
                catch (Exception Ex)
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Error Connect To PLC \r {Ex}").ShowDialog();
                }

                // Thay đổi các trạng thái khi kết nối thành công
                c_varGolbal.LotID = txtLot_ID.Text.Trim();
                c_varGolbal.IsRun = true;
                if (btnRunOrStop.InvokeRequired)
                {
                    btnRunOrStop.Invoke(new Action(() =>
                    {
                        btnRunOrStop.Text = "STOP";
                        status_Machine = "RUN";
                        btnRunOrStop.BackColor = Color.Red;
                    }));
                }
                else
                {
                    btnRunOrStop.Text = "STOP";
                    btnRunOrStop.BackColor = Color.Red;
                    status_Machine = "RUN";
                }
                displayStatusMachine("System Running");
                cboProgram.Enabled = btnSetting.Enabled = txt_Machine.Enabled = txt_LineID.Enabled = txt_StaffID.Enabled = btnChangeUser.Enabled = btnExit.Enabled = false;
                c_varGolbal.StaffID = txt_StaffID.Text;
                txtLot_ID.Enabled = false;

                //KHỞI TẠO CHẠY CÁC LUỒNG ĐIỀU KHIỂN CHÍNH TRONG CHƯƠNG TRÌNH
                try
                {
                    if (PLC_Fx5.Connected)
                        PLC_Fx5.SetSingleBit("M1111", true);
                    Main_VisonReader = new Thread(new ThreadStart(MainVisonBarcode));
                    Main_VisonReader.IsBackground = true;
                    Main_VisonReader.Start();
                    Thread_Run_Auto();
                    Thread_Reconnect_PLC();
                }
                catch (Exception)
                {
                    DisconnectAllDevice();
                    return;
                }

            }
            else if (status_Machine == "RUN")
            {
                // NB - 27032023
                c_varGolbal.IsRun = false;
                _run_Auto?.Abort();
                Thread.Sleep(100);
                _reconnect_PLC?.Abort();
                Thread.Sleep(100);
                Main_VisonReader?.Abort();
                Thread.Sleep(200);
                DisconnectAllDevice();
                QuyenSuDung();
                displayStatusMachine("System Stop");

                if (lblStatusConnectPLC.InvokeRequired)
                {
                    lblStatusConnectPLC.Invoke(new Action(() =>
                    {
                        lblStatusConnectPLC.BackColor = Color.Red;
                        lblStatusConnectPLC.Text = "Disconnect";
                    }));
                }
                else
                {
                    lblStatusConnectPLC.BackColor = Color.Red;
                    lblStatusConnectPLC.Text = "Disconnect";
                }
                this.Invoke(new MethodInvoker(delegate
                {
                    ShowResultProcessReadCode("WAIT");
                }));
                cboProgram.Enabled = txt_StaffID.Enabled = btnChangeUser.Enabled = btnExit.Enabled = txtLot_ID.Enabled = true;
            }
        }
        void QuyenSuDung()
        {
            if (btnRunOrStop.InvokeRequired)
            {
                btnRunOrStop.Invoke(new Action(() =>
                {
                    btnRunOrStop.Text = "RUN";
                    btnRunOrStop.BackColor = Color.Green;
                    status_Machine = "STOP";
                }));
            }
            else
            {
                btnRunOrStop.Text = "RUN";
                btnRunOrStop.BackColor = Color.Green;
                status_Machine = "STOP";
            }
            displayStatusMachine("System Stop");
            cboProgram.Enabled = txt_StaffID.Enabled = true;

            if (c_varGolbal.IsAdmin)
            {
                try
                {
                    for (int i = 0; i < lst_CameraReader.Count; i++)
                    {
                        lst_CameraReader[i].EnableSettingButton();
                    }
                    if (btnChangeUser.InvokeRequired)
                    {
                        btnChangeUser.Invoke(new Action(() => btnChangeUser.Text = "ADMIN"));
                    }
                    else
                        btnChangeUser.Text = "ADMIN";
                    Barcode_Vision.EnableSettingButton();
                    btnSetting.Enabled = btnSetting.Enabled = btnTest.Visible = btnRead.Visible = true;
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
            else
            {
                try
                {
                    for (int i = 0; i < lst_CameraReader.Count; i++)
                    {
                        lst_CameraReader[i].DisableSettingButton();
                    }
                    if (btnChangeUser.InvokeRequired)
                    {
                        btnChangeUser.Invoke(new Action(() => btnChangeUser.Text = "USER"));
                    }
                    else
                        btnChangeUser.Text = "USER";
                    btnSetting.Enabled = btnTest.Visible = btnRead.Visible = false;
                    Barcode_Vision.DisableSettingButton();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        #region Vision Read Barcode
        private int Step_ReadCode = 0;
        public static int oldPosition = 0;

        private void ShowError(Exception ex = null)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                ShowResultProcessReadCode("NG");
            }));
            Step_ReadCode = 0;
            c_varGolbal.List_DataCode.Clear();
            if (ex != null)
                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Error Process Read Code PCS Vision \r {ex}").ShowDialog();
        }

        bool _flag_OK_NG;
        bool _flag_Mising;
        List<string> List_CodeJig = new List<string>();
        List<string> List_Code_Read_List_Tag = new List<string>();
        private bool usingFVI;
        private bool skipFVI;
        bool Bit_Sensor_Default;
        bool Bit_Trigger_Default;
        bool checkNoLock = false;
        bool checkQualifyjig = false;
        IEnumerable<IGrouping<string, dataVision>> List_DoubleCode;
        List<string> ListCheckData56 = new List<string>();
        int idUsingFVI = 0;
        /// các biến kiểm tra nếu không chụp trong 10s
        bool BeginSnap = false;
        bool CompleteResult = false;
        Timer TimerShowKQ = new Timer();
        CompleteFinishLot FinishLot = new CompleteFinishLot();
        private void MainVisonBarcode()
        {
            while (c_varGolbal.IsRun) // Vòng quét xử lý vision
            {
                Thread.Sleep(50);
                switch (Step_ReadCode)
                {
                    case 10:
                        {
                            List_CodeJig.Clear();
                            idUsingFVI = 0;
                            //PC Send To PLC:Bật đèn
                            //PLC_Fx5.SetSingleBit("M20", true);
                            //Đọc CodeJig bằng barcode của bên Jig không có sản phẩm
                            //log.Debug("Read Barcode Tag");
                            BeginSnap = true;
                            oldPosition = 0;
                            try
                            {
                                //Trigger Barcode
                                if (Barcode_Vision._IsConected)
                                    Barcode_Vision.SendSignal(ConstSendByte.TON);
                                else
                                {
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);
                                    //PC Send To PLC: Error
                                    PLC_Fx5.SetSingleBit("M30", true);
                                    //log.Debug($"Result:_____NG______BarCode NoRead ");
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"DON'T CONNECT BARCODE \r\n").ShowDialog();
                                    //frm_show_MainVisonBarcode.ShowDialog();
                                    break;
                                }
                                bool Completed = ExecuteWithTimeLimit(TimeSpan.FromSeconds(5), () =>
                                {
                                    Barcode_Vision.RunReadTag();
                                    //code_TagJigCurent = Barcode_Vision.ResultBarcode();
                                });
                                if (Completed)
                                {
                                    code_TagJigCurent = Barcode_Vision.ResultBarcode();
                                    if (code_TagJigCurent != null && code_TagJigCurent.Trim() != "" && code_TagJigCurent.ToUpper() != c_varGolbal.NoReadString.ToUpper())
                                    {
                                        code_TagJigNonePCS = code_TagJigCurent.Trim();
                                        this.Invoke(new Action(() =>
                                        {

                                            // Trường họp 3: đọc code 2 jig và pcs => update lại máy FVI pcs
                                            if (cbTypeModel.SelectedIndex == 3)
                                            {
                                                Step_ReadCode = 20;
                                            }
                                            // trường hơp 2: đọc 2 code jig => quay lại về FVI để lấy code PCS cập nhập vào máy Befor Plasma
                                            else if (cbTypeModel.SelectedIndex == 2)
                                            {
                                                Step_ReadCode = 18;
                                            }
                                            else
                                            {
                                                this.Invoke(new MethodInvoker(delegate
                                                {
                                                    ShowResultProcessReadCode("NG");
                                                }));
                                                Step_ReadCode = 0;
                                                c_varGolbal._isProduct = false;
                                                //PC Send To PLC:Tắt đèn
                                                PLC_Fx5.SetSingleBit("M20", false);
                                                //PC Send To PLC: Error
                                                PLC_Fx5.SetSingleBit("M30", true);
                                                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Model Không Sử Dụng Máy Before Plasma \r\n").ShowDialog();
                                                //frm_show_MainVisonBarcode.ShowDialog();
                                            }
                                        }));


                                    }
                                    else
                                    {
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            ShowResultProcessReadCode("NG");
                                        }));

                                        for (int i = 0; i < lst_CameraReader.Count; i++)
                                        {
                                            lst_CameraReader[i].Invoke(new MethodInvoker(delegate
                                            {
                                                lst_CameraReader[i].Clear_HSmart_Window();
                                            }));
                                        }
                                        Step_ReadCode = 0;
                                        c_varGolbal._isProduct = false;
                                        //PC Send To PLC:Tắt đèn
                                        PLC_Fx5.SetSingleBit("M20", false);
                                        //PC Send To PLC: Error
                                        PLC_Fx5.SetSingleBit("M30", true);
                                        //log.Debug($"Result:_____NG______BarCode NoRead ");
                                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"BarCode NoRead \r\n").ShowDialog();
                                        //frm_show_MainVisonBarcode.ShowDialog();
                                    }
                                }
                                else
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("NG");
                                    }));
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);
                                    //PC Send To PLC: Error
                                    PLC_Fx5.SetSingleBit("M30", true);
                                    log.Debug($"Result:_____NG______Can't Read BarCode TagJig");
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Can't Read BarCode TagJig \r\n").ShowDialog();
                                    //frm_show_MainVisonBarcode.ShowDialog();
                                }


                            }
                            catch (Exception Ex)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    ShowResultProcessReadCode("NG");
                                }));
                                Step_ReadCode = 0;
                                c_varGolbal._isProduct = false;
                                //PC Send To PLC:Tắt đèn
                                PLC_Fx5.SetSingleBit("M20", false);
                                //PC Send To PLC: Error
                                PLC_Fx5.SetSingleBit("M30", true);
                                //log.Debug($"Result:_____NG______Read BarCode TagJig  is fail {Ex}");
                                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Read BarCode TagJig  is fail \r\n {Ex}").ShowDialog();
                                //frm_show_MainVisonBarcode.ShowDialog();
                            }
                        }
                        break;


                    #region Đọc 2 code Jig bằng Barcode(Không dùng)
                    case 15:
                        {
                            //Đọc 2 codeJig bằng Barcode

                            if (!Thread_VC3000.LightOn())
                            {
                                continue;
                            }
                            List_Code_Read_List_Tag.Clear();
                            try
                            {
                                //Trigger Barcode
                                Barcode_Vision.SendSignal(ConstSendByte.TON);

                                bool Completed = ExecuteWithTimeLimit(TimeSpan.FromSeconds(5), () => { Barcode_Vision.RunReadTag(); });
                                if (Completed)
                                {
                                    List_Code_Read_List_Tag = Barcode_Vision.ResultListBarcode();
                                    if (List_Code_Read_List_Tag.Count > 0)
                                    {
                                        code_TagJigNonePCS = List_Code_Read_List_Tag[0];
                                        code_TagJigHavePCS = List_Code_Read_List_Tag[1];
                                        Step_ReadCode = 20;
                                    }
                                    else
                                    {
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            ShowResultProcessReadCode("NG");
                                        }));
                                        Step_ReadCode = 0;
                                        Thread_VC3000.LightOff();
                                        log.Debug($"Result:_____NG______BarCode NoRead");
                                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"BarCode NoRead \r\n").ShowDialog();
                                    }
                                }
                                else
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("NG");
                                    }));
                                    Step_ReadCode = 0;
                                    Thread_VC3000.LightOff();
                                    log.Debug($"Result:_____NG______Can't Read BarCode TagJig");
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Can't Read BarCode TagJig \r\n").ShowDialog();
                                }
                            }
                            catch (Exception Ex)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    ShowResultProcessReadCode("NG");
                                }));
                                Step_ReadCode = 0;
                                Thread_VC3000.LightOff();
                                log.Debug($"Result:_____NG______Read BarCode TagJig  is fail {Ex}");
                                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Read BarCode TagJig  is fail \r\n {Ex}").ShowDialog();
                            }
                        }
                        break;
                    #endregion

                    case 18:
                        #region CHỈ ĐỌC 2 CODE JIG
                        {
                            lst_dataVision.Clear();

                            try
                            {
                                foreach (var item in lst_CameraReader)
                                {
                                    item.SnapImage();
                                    if (item.Run_ReadCodeJig().Count > 0 && item.Run_ReadCodeJig() != null)
                                    {
                                        List_CodeJig = item.Run_ReadCodeJig();
                                    }
                                }
                                bool _Flag_Barcode = false;
                                foreach (var item in List_CodeJig)
                                {
                                    if (item.Trim() != "" && item.ToUpper() != "NOREAD")
                                    {
                                        _Flag_Barcode = true;
                                        code_TagJigHavePCS = item.Trim();
                                    }
                                }

                                if (_Flag_Barcode)
                                {
                                    //Tat den
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);

                                    // Quay về FVI lấy pcs
                                    string sql_Find_codePCS_By_TagJig = $"select * from ReadCode where Code_TagJigHavePCS='{code_TagJigNonePCS}' AND StatusFVI = false ORDER BY ID Desc  Limit 1";

                                    DataTable dtFVI = Support_SQL_PVI_Server.GetTableDataReadCode(sql_Find_codePCS_By_TagJig);
                                    if (dtFVI.Rows.Count > 0)
                                    {
                                        idUsingFVI = Support_SQL_PVI_Server.ToInt(dtFVI.Rows[0]["ID"]);
                                    }
                                    else
                                    {
                                        //PC Send To PLC:Tắt đèn
                                        PLC_Fx5.SetSingleBit("M20", false);
                                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Xin vui lòng kiểm tra Jig chưa qua công đoạn FVI \r\n").ShowDialog();
                                        //frm_show_MainVisonBarcode.ShowDialog();
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            ShowResultProcessReadCode("NG");
                                        }));
                                        _flag_OK_NG = false;
                                        c_varGolbal._isProduct = false;
                                        Step_ReadCode = 0;
                                        continue;
                                    }
                                    if (Support_SQL.ToInt(dtFVI.Rows[0]["NoLock"]) == 2 || Support_SQL.ToInt(dtFVI.Rows[0]["QualifyJig"]) == 2)
                                    {
                                        //PC Send To PLC:Tắt đèn
                                        PLC_Fx5.SetSingleBit("M20", false);

                                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, $"Xin vui lòng kiểm tra Jig\r\nJig {code_TagJigNonePCS} đã Lock hoặc NoQualify ở công đoạn FVI ").ShowDialog();
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            ShowResultProcessReadCode("NG");
                                        }));
                                        _flag_OK_NG = false;
                                        c_varGolbal._isProduct = false;
                                        Step_ReadCode = 0;
                                        continue;
                                    }
                                    if (usingFVI)
                                    {
                                        checkNoLock = SupportDB_MMCV.CheckHaveLock(c_varGolbal.LotID, c_varGolbal.LineID, c_varGolbal.DeviceID);
                                        checkQualifyjig = SupportDB_MMCV.CheckQualifyJig(c_varGolbal.LotID, c_varGolbal.LineID, c_varGolbal.DeviceID, code_TagJigHavePCS, code_TagJigNonePCS, c_varGolbal.StaffID);
                                    }
                                    if (dtFVI.Rows.Count > 0)
                                    {
                                        // Save data pcs và 2 jig về máy Befor Plasma 
                                        dataVision infoVision = new dataVision();
                                        infoVision.TagJigNonePCS = code_TagJigNonePCS;
                                        infoVision.TagJigHavePCS = code_TagJigHavePCS;
                                        infoVision.NoLock = (checkNoLock == true) ? 1 : 2;
                                        infoVision.QualifyJig = (checkQualifyjig == true) ? 1 : 2;
                                        lst_dataVision.Add(infoVision);

                                        string codePCS = Lib.ToString(dtFVI.Rows[0]["CodePcs"]);
                                        //codePCS = string.Join(",", c_varGolbal.List_DataCode).Replace(c_varGolbal.Missing+",","");
                                        // kiểm tra đã tồn tại hay chưa, nếu đã tồn tại thì ko lưu dữ liệu
                                        int id_Backup = Support_SQL.ToInt(Support_SQL.ExecuteScalar($"SELECT ID FROM Readcode WHERE Code_TagJigNonePCS = '{code_TagJigNonePCS}' " +
                                            $"AND Code_TagJigHavePCS = '{code_TagJigHavePCS}' AND CodePCS = '{codePCS}' AND StatusUpload = false", c_varGolbal.str_ConnectDB_Backup));

                                        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (id_Backup == 0)
                                        {
                                            if (usingFVI)
                                            {
                                                Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, lst_dataVision[0].NoLock, lst_dataVision[0].QualifyJig, c_varGolbal.LotID, c_varGolbal.MPN, c_varGolbal.str_ConnectDB_Backup);
                                            }
                                            else
                                            {
                                                Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, c_varGolbal.LotID, c_varGolbal.MPN, c_varGolbal.str_ConnectDB_Backup);
                                            }
                                        }
                                        string Select = "";
                                        if (c_varGolbal.TakeJigHavePcs)
                                        {
                                            Select = $"SELECT * FROM Readcode WHERE Code_TagJigHavePCS = '{code_TagJigHavePCS}' AND StatusUpload = false ORDER by ID DESC";
                                        }
                                        else
                                        {
                                            Select = $"SELECT * FROM Readcode WHERE Code_TagJigNonePCS = '{code_TagJigNonePCS}' AND StatusUpload = false ORDER by ID DESC";
                                        }
                                        DataTable table = Support_SQL.GetTableDataReadCode(Select);
                                        //int id = Support_SQL.ToInt(Support_SQL.ExecuteScalar($"SELECT ID FROM Readcode WHERE CodePCS = '{codePCS}' AND StatusUpload = false"));
                                        if (table.Rows.Count >= 0)
                                        {
                                            for (int i = 0; i < table.Rows.Count; i++)
                                            {
                                                Support_SQL.ExecuteScalar($"DELETE FROM ReadCode WHERE ID={table.Rows[i]["ID"]}");
                                            }
                                        }
                                        if (usingFVI)
                                        {
                                            Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, lst_dataVision[0].NoLock, lst_dataVision[0].QualifyJig, c_varGolbal.LotID, c_varGolbal.MPN);
                                        }

                                        if (checkNoLock && checkQualifyjig)
                                        {
                                            //Show data
                                            this.Invoke(new MethodInvoker(delegate
                                            {
                                                dgv_ShowInfoBarCode(codePCS);
                                            }));
                                            this.Invoke(new MethodInvoker(delegate
                                            {
                                                ShowResultProcessReadCode("OK");
                                            }));
                                            if (Support_SQL_PVI_Server.SaveStateFviByID(idUsingFVI, 1))//Update trạng thái hoàn thành chụp code ở máy trước plasma
                                            {

                                            }
                                            else
                                            {
                                                Lib.ShowError("Update trạng thái State Fvi False\r\nHãy chụp lại");
                                            }
                                        }
                                        else
                                        {
                                            this.Invoke(new MethodInvoker(delegate
                                            {
                                                dgv_ShowInfoBarCode(codePCS);
                                            }));
                                            this.Invoke(new MethodInvoker(delegate
                                            {
                                                ShowResultProcessReadCode("NG");
                                            }));
                                        }



                                    }

                                }
                                else
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("NG");
                                    }));
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);
                                    c_varGolbal.List_DataCode.Clear();
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $" Read BarCode is fail! \r\n ").ShowDialog();
                                    //frm_show_MainVisonBarcode.ShowDialog();
                                }
                            }
                            catch (Exception Ex)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    ShowResultProcessReadCode("NG");
                                }));
                                Step_ReadCode = 0;
                                c_varGolbal._isProduct = false;
                                //PC Send To PLC:Tắt đèn
                                PLC_Fx5.SetSingleBit("M20", false);
                                c_varGolbal.List_DataCode.Clear();
                                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $" Read BarCode is fail! \r\n {Ex}").ShowDialog();
                                //frm_show_MainVisonBarcode.ShowDialog();
                            }
                            //PC Send To PLC:Tắt đèn
                            PLC_Fx5.SetSingleBit("M20", false);
                        }
                        #endregion
                        break;
                    case 20: // TRƯỜNG HỢP 3
                        {
                            c_varGolbal.List_DataCode.Clear();
                            _flag_OK_NG = true;
                            _flag_Mising = true;
                            List<Result> lstResult = new List<Result>();
                            // Chụp ảnh và giải mã Code sản phẩm va barcode của bên Jig có sản phẩm
                            log.Debug("Snap image and decode");
                            try
                            {
                                lst_dataVision.Clear();
                                code_TagJigHavePCS = "";
                                //real
                                PLC_Fx5.SetSingleBit("M20", true);
                                List<HImage> listImage = new List<HImage>();

                                for (int h = 0; h < lst_CameraReader.Count; h++)
                                    listImage.Add(lst_CameraReader[h].SnapImageEx());
                                Thread.Sleep(100);
                                PLC_Fx5.SetSingleBit("M20", false);
                                for (int h = 0; h < lst_CameraReader.Count; h++)
                                {
                                    lstResult.Add(lst_CameraReader[h].Run_Readcode(listImage[h]));
                                    if (lst_CameraReader[h].Run_ReadCodeJig().Count > 0 && lst_CameraReader[h].Run_ReadCodeJig() != null)
                                    {
                                        List_CodeJig = lst_CameraReader[h].Run_ReadCodeJig();
                                    }
                                    listImage[h]?.Dispose();
                                    listImage[h] = null;
                                }

                                uc_Vision.nameJig = "";
                                // NB: 18022023
                                // Kiểm tra Code Jib đã có bất kỳ công đoạn FVI chưa?
                                // -> sử dụng class Support_SQL_PVI_Server.cs

                                DataTable dtFVI = new DataTable();
                                this.Invoke(new Action(() => usingFVI = Support_SQL.ToBoolean(Support_SQL.ExecuteScalar($"SELECT UsingFVI FROM ProgramMain WHERE ProgramName = '{cboProgram.Text.Trim()}'"))));
                                for (int i = 0; i < List_CodeJig.Count; i++)
                                {
                                    if (List_CodeJig[i].Trim() != "")
                                    {
                                        code_TagJigHavePCS = List_CodeJig[i].Trim();
                                        if (usingFVI && !skipFVI)
                                        {
                                            dtFVI = Support_SQL_PVI_Server.GetTableData($"SELECT * From Readcode WHERE Code_TagJigHavePCS = '{code_TagJigNonePCS}' AND StatusFVI = false AND StatusType_FVI_Upload=2 ORDER by ID DESC LIMIT 1");
                                            if (dtFVI.Rows.Count > 0)
                                            {
                                                idUsingFVI = Support_SQL_PVI_Server.ToInt(dtFVI.Rows[0]["ID"]);
                                            }
                                        }
                                    }
                                }
                                oldPosition = 0;
                                //HuyNV 02-03-2023
                                //Check code_TagJigHavePCS nếu NoRead => báo NG màu đỏ Cho chụp lại
                                if (String.IsNullOrEmpty(code_TagJigHavePCS) || code_TagJigHavePCS.ToUpper() == "NOREAD")
                                {
                                    //Cảnh báo
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("NG");
                                    }));
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);
                                    //PC Send To PLC: Error
                                    PLC_Fx5.SetSingleBit("M30", true);
                                    continue;
                                }
                                if (lstResult.Count <= 0)
                                {
                                    //21-02-2023
                                    //HuyNV
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);
                                    //PC Send To PLC: Error
                                    PLC_Fx5.SetSingleBit("M30", true);
                                    continue;
                                }
                                if (idUsingFVI == 0 && usingFVI && !skipFVI)
                                {
                                    //PC Send To PLC:Tắt đèn
                                    PLC_Fx5.SetSingleBit("M20", false);
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Xin vui lòng kiểm tra Jig qua công đoạn FVI \r\n").ShowDialog();
                                    //frm_show_MainVisonBarcode.ShowDialog();
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("NG");
                                    }));
                                    _flag_OK_NG = false;
                                    c_varGolbal._isProduct = false;
                                    Step_ReadCode = 0;
                                    continue;
                                }
                                else if (dtFVI.Rows.Count > 0 && usingFVI && !skipFVI)
                                {
                                    if (Support_SQL.ToInt(dtFVI.Rows[0]["NoLock"]) == 2 || Support_SQL.ToInt(dtFVI.Rows[0]["QualifyJig"]) == 2)
                                    {
                                        //PC Send To PLC:Tắt đèn
                                        PLC_Fx5.SetSingleBit("M20", false);

                                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Warning, $"Xin vui lòng kiểm tra Jig\r\nJig {code_TagJigNonePCS} đã Lock hoặc NoQualify ở công đoạn FVI ").ShowDialog();
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            ShowResultProcessReadCode("NG");
                                        }));
                                        _flag_OK_NG = false;
                                        c_varGolbal._isProduct = false;
                                        Step_ReadCode = 0;
                                        continue;
                                    }
                                }
                                if (usingFVI)
                                {
                                    checkNoLock = SupportDB_MMCV.CheckHaveLock(c_varGolbal.LotID, c_varGolbal.LineID, c_varGolbal.DeviceID);
                                    checkQualifyjig = SupportDB_MMCV.CheckQualifyJig(c_varGolbal.LotID, c_varGolbal.LineID, c_varGolbal.DeviceID, code_TagJigHavePCS, code_TagJigNonePCS, c_varGolbal.StaffID);
                                }
                                for (int i = 0; i < lstResult.Count; i++)
                                {
                                    foreach (Code item in lstResult[i].ListData)
                                    {
                                        dataVision infoVision = new dataVision();
                                        infoVision.ID = Support_SQL.ToInt(item.IDRegion);
                                        infoVision.CamIndex = item.CamIndex.ToString();
                                        infoVision.PcsIndex = item.IDRegion.ToString();
                                        infoVision.PcsBarcode = item.Content;
                                        infoVision.TagJigNonePCS = code_TagJigNonePCS;
                                        infoVision.TagJigHavePCS = code_TagJigHavePCS;
                                        infoVision.statusICT = item.statusICT; //HA_ADD
                                        c_varGolbal.List_DataCode.Add(item.Content);
                                        if (c_varGolbal.Finish_Lot)
                                        {
                                            infoVision.statusFinishLot = true;
                                        }
                                        else
                                        {
                                            infoVision.statusFinishLot = false;
                                        }
                                        if (usingFVI)
                                        {
                                            infoVision.NoLock = (checkNoLock == true) ? 1 : 2;
                                            infoVision.QualifyJig = (checkQualifyjig == true) ? 1 : 2;
                                        }
                                        lst_dataVision.Add(infoVision);
                                        if (infoVision.PcsBarcode.ToUpper() == "NOREAD" || (infoVision.statusICT == false && !infoVision.PcsBarcode.Contains(c_varGolbal.Missing))) //HA_ADD
                                        {
                                            _flag_OK_NG = false;
                                        }
                                        if (infoVision.PcsBarcode == c_varGolbal.Missing)
                                        {
                                            _flag_Mising = false;
                                        }
                                    }
                                }
                                ListCheckData56 = CheckHaveDataPlasma(lst_dataVision);
                                List_DoubleCode = lst_dataVision.FindAll(x => !x.PcsBarcode.Contains(c_varGolbal.NoReadString) && !x.PcsBarcode.Contains(c_varGolbal.Missing))
                                                    .GroupBy(x => x.PcsBarcode).Where(g => g.Count() >= 2);
                                if (List_DoubleCode.Count() > 0)
                                {
                                    foreach (var item in List_DoubleCode)
                                    {
                                        var lst_Item = item.ToList();
                                        foreach (var kq in lst_Item)
                                        {
                                            int index = lst_dataVision.FindIndex(x => x.ID == kq.ID);
                                            var list_Another = lst_Item.FindAll(x => x.ID != kq.ID).Select(x => x.ID);
                                            string str_add = string.Join("=", list_Another);
                                            lst_dataVision[index].PcsIndex += "=" + str_add;
                                        }
                                    }
                                }

                                Step_ReadCode = 40; // Next Step
                            }
                            catch (Exception Ex)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    ShowResultProcessReadCode("NG");
                                }));
                                Step_ReadCode = 0;
                                c_varGolbal._isProduct = false;
                                //PC Send To PLC:Tắt đèn
                                PLC_Fx5.SetSingleBit("M20", false);
                                //PC Send To PLC: Error
                                PLC_Fx5.SetSingleBit("M30", true);
                                c_varGolbal.List_DataCode.Clear();
                                //log.Debug($"Result:_____NG______Read Code PCS is fail! {Ex}");
                                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $" Read Code PCS is fail! \r\n {Ex.ToString()}").ShowDialog();
                                //frm_show_MainVisonBarcode.ShowDialog();
                            }
                            //Tăt đèn
                            PLC_Fx5.SetSingleBit("M20", false);
                        }
                        break;

                    case 40: // TRƯỜNG HỢP 3
                        {
                            // Show data grid view (dgv_VisionReadcode)
                            //log.Debug("Save data to DB");
                            this.Invoke(new MethodInvoker(delegate
                            {
                                dgv_ShowInfoVision();
                            }));
                            //Save data to DB

                            try
                            {
                                if (ListCheckData56.Count > 0)
                                {
                                    string strJoin = string.Join("\r\n", ListCheckData56);
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Đã có những Pcs đã upload data plasma:\r\n {strJoin}").ShowDialog();
                                    //PC Send To PLC: Error
                                    PLC_Fx5.SetSingleBit("M30", true);
                                    //log.Debug($"Result:_____NG______");
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("NG");
                                    }));
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                    continue;
                                }
                                if (List_DoubleCode.Count() > 0)
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        ShowResultProcessReadCode("DOUBLE CODE");
                                    }));
                                    //PC Send To PLC: Error
                                    PLC_Fx5.SetSingleBit("M30", true);
                                    //log.Debug($"Result:_____NG______");
                                    Step_ReadCode = 0;
                                    c_varGolbal._isProduct = false;
                                }
                                else
                                {
                                    if ((_flag_OK_NG && c_varGolbal.Finish_Lot) || (_flag_Mising && _flag_OK_NG))
                                    {
                                        string codePCS = "";
                                        List<string> lst_temp = c_varGolbal.List_DataCode.FindAll(x => !x.Contains(c_varGolbal.Missing) && !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                                        if (lst_temp.Count > 0)
                                        {
                                            #region Insert Data local và File Excel
                                            codePCS = string.Join(",", lst_temp);
                                            //codePCS = string.Join(",", c_varGolbal.List_DataCode).Replace(c_varGolbal.Missing+",","");
                                            // kiểm tra đã tồn tại hay chưa, nếu đã tồn tại thì ko lưu dữ liệu
                                            int id_Backup = Support_SQL.ToInt(Support_SQL.ExecuteScalar($"SELECT ID FROM Readcode WHERE Code_TagJigNonePCS = '{code_TagJigNonePCS}' " +
                                                $"AND Code_TagJigHavePCS = '{code_TagJigHavePCS}' AND CodePCS = '{codePCS}' AND StatusUpload = false", c_varGolbal.str_ConnectDB_Backup));

                                            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            if (id_Backup == 0)
                                            {
                                                if (usingFVI)
                                                {
                                                    Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, lst_dataVision[0].NoLock, lst_dataVision[0].QualifyJig, c_varGolbal.LotID, c_varGolbal.MPN, c_varGolbal.str_ConnectDB_Backup);
                                                }
                                                else
                                                {
                                                    Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, c_varGolbal.LotID, c_varGolbal.MPN, c_varGolbal.str_ConnectDB_Backup);
                                                }
                                            }
                                            string Select = "";
                                            if (c_varGolbal.TakeJigHavePcs)
                                            {
                                                Select = $"SELECT * FROM Readcode WHERE Code_TagJigHavePCS = '{code_TagJigHavePCS}' AND StatusUpload = false ORDER by ID DESC";
                                            }
                                            else
                                            {
                                                Select = $"SELECT * FROM Readcode WHERE Code_TagJigNonePCS = '{code_TagJigNonePCS}' AND StatusUpload = false ORDER by ID DESC";
                                            }
                                            DataTable table = Support_SQL.GetTableDataReadCode(Select);
                                            //int id = Support_SQL.ToInt(Support_SQL.ExecuteScalar($"SELECT ID FROM Readcode WHERE CodePCS = '{codePCS}' AND StatusUpload = false"));
                                            if (table.Rows.Count >= 0)
                                            {
                                                string strIDDelete = "";
                                                List<int> lstIDDelete = new List<int>();
                                                for (int i = 0; i < table.Rows.Count; i++)
                                                {
                                                    lstIDDelete.Add(Lib.ToInt(table.Rows[i]["ID"]));
                                                }
                                                strIDDelete = string.Join(",", lstIDDelete);
                                                Support_SQL.ExecuteScalar($"DELETE FROM ReadCode WHERE ID IN ({strIDDelete})");
                                            }
                                            //Save to DBReadCode
                                            if (usingFVI)
                                            {
                                                Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, lst_dataVision[0].NoLock, lst_dataVision[0].QualifyJig, c_varGolbal.LotID, c_varGolbal.MPN);
                                            }
                                            else
                                            {
                                                Support_SQL.SaveDataToBufferReadcode(ID_PrgCurrent, code_TagJigNonePCS, code_TagJigHavePCS, codePCS, date, c_varGolbal.LotID, c_varGolbal.MPN);
                                            }

                                            List<dataReadcode> lst_DataExcel = new List<dataReadcode>();
                                            dataReadcode element = new dataReadcode();
                                            element.TagJig1 = code_TagJigHavePCS;
                                            element.TagJig2 = code_TagJigNonePCS;
                                            element.PcsBarcode = codePCS;
                                            element.DateTimeIn = date;
                                            element.FinishLot = (c_varGolbal.Finish_Lot == true) ? GlobVar.Yes : GlobVar.No;
                                            element.Status = "OK";
                                            lst_DataExcel.Add(element);
                                            Upload_DB_Excel(lst_DataExcel);

                                            //HuyNV 01-03-2023 nếu chỉ có máy Plasma thôi thì k cần upload dữ liệu tới may FVI
                                            //20-02-2023
                                            //Upadte to DBFVI
                                            if (usingFVI && !skipFVI)
                                            {
                                                try
                                                {
                                                    ExecuteWithTimeLimit(TimeSpan.FromSeconds(5), () => { Support_SQL.UpdatePcsToDB_FVI_SERVER(code_TagJigNonePCS, codePCS); });
                                                }
                                                catch (Exception ex)
                                                {
                                                    this.Invoke(new MethodInvoker(delegate
                                                    {
                                                        ShowResultProcessReadCode("NG");
                                                    }));
                                                    //PC Send To PLC: OK
                                                    //PLC_Fx5.SetSingleBit("M1002", true);

                                                    Step_ReadCode = 0;
                                                    c_varGolbal._isProduct = false;
                                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Update Data FVI is fail \r\n {ex}").ShowDialog();
                                                    //frm_show_MainVisonBarcode.ShowDialog();
                                                    continue;
                                                }
                                            }
                                            if (usingFVI)
                                            {
                                                if (!checkNoLock || !checkQualifyjig)
                                                {
                                                    this.Invoke(new MethodInvoker(delegate
                                                    {
                                                        ShowResultProcessReadCode("NG");
                                                    }));
                                                }
                                                else
                                                {
                                                    this.Invoke(new MethodInvoker(delegate
                                                    {
                                                        ShowResultProcessReadCode("OK");
                                                    }));
                                                }
                                            }
                                            else
                                            {
                                                this.Invoke(new MethodInvoker(delegate
                                                {
                                                    ShowResultProcessReadCode("OK");
                                                }));
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            this.Invoke(new MethodInvoker(delegate
                                            {
                                                ShowResultProcessReadCode("NG");
                                            }));
                                            //PC Send To PLC: Error
                                            PLC_Fx5.SetSingleBit("M30", true);
                                        }

                                        //PC Send To PLC: OK
                                        //PLC_Fx5.SetSingleBit("M1002", true);
                                        //log.Debug($"Result:_____OK______");
                                        Step_ReadCode = 0;
                                        c_varGolbal._isProduct = false;

                                    }
                                    else
                                    {

                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            ShowResultProcessReadCode("NG");
                                        }));
                                        //PC Send To PLC: Error
                                        PLC_Fx5.SetSingleBit("M30", true);
                                        //log.Debug($"Result:_____NG______");
                                        Step_ReadCode = 0;
                                        c_varGolbal._isProduct = false;
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    ShowResultProcessReadCode("NG");
                                }));
                                Step_ReadCode = 0;
                                //PC Send To PLC: Error
                                PLC_Fx5.SetSingleBit("M30", true);
                                c_varGolbal._isProduct = false;
                                //log.Debug($"Result:_____NG______Save Data is fail  {Ex}");
                                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Save Data is fail \r\n {Ex}").ShowDialog();
                                //frm_show_MainVisonBarcode.ShowDialog();
                            }
                            while (!continueStep40) { }
                            //// NB - 28032023 -> Reset chkFinishLot
                            this.Invoke(new MethodInvoker(delegate
                            {
                                c_varGolbal.Finish_Lot = chkFinishLot.Checked = false;
                                skipFVI = chkSkipFVI.Checked = false;
                            }));
                        }
                        break;
                    default:
                        try
                        {
                            _flag_OK_NG = true;
                            //PLC_Fx5.SetSingleBit("M1002", false);
                            //Check Sensor
                            if (PLC_Fx5.Connected)
                            {
                                PLC_Fx5.GetSingleBit("M500", out Bit_Sensor_Default);
                                PLC_Fx5.GetSingleBit("M501", out Bit_Trigger_Default);
                            }
                            if (!Bit_Sensor_Default)
                            {
                                if (PLC_Fx5.Connected)
                                    PLC_Fx5.SetSingleBit("M30", false);
                            }
                            if (c_varGolbal._isProduct || Bit_Trigger_Default)
                            {
                                //PC Send To PLC: Error
                                if (PLC_Fx5.Connected)
                                    PLC_Fx5.SetSingleBit("M30", false);
                                if (!Bit_Sensor_Default)
                                {
                                    //log.Debug($"Result:Jig position is not correct");
                                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Jig position is not correct").ShowDialog();
                                    //frm_show_MainVisonBarcode.ShowDialog();
                                    c_varGolbal._isProduct = false;
                                    continue;

                                }
                            }
                            //Sửa lại Trigger Sensor vs trigger nút bấm để nhảy vào case đọc code
                            //M100 đợi có tính hiệu nút bấm từ PLC if true next step else return
                            if (Bit_Trigger_Default || c_varGolbal._isProduct)
                            {

                                #region kiểm tra finish lot
                                if (FinishLot.ktLot)
                                {
                                    if (FinishLot.Time.AddMinutes(3) >= DateTime.Now)
                                    {
                                        if (PLC_Fx5.Connected)
                                            PLC_Fx5.SetSingleBit("M30", true);
                                        Step_ReadCode = 0;
                                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Sau khi kết thúc Lot phải chờ 3 phút mới có thể chạy tiếp\r ").ShowDialog();
                                        break;
                                    }
                                }
                                #endregion

                                if (c_varGolbal.Finish_Lot)
                                {
                                    Frm_Confirm newFrm = new Frm_Confirm(Frm_Confirm.Icon_Show.Warning, "Bạn có muốn kết thúc Lot");
                                    if (newFrm.ShowDialog() == DialogResult.Cancel)
                                    {
                                        if (PLC_Fx5.Connected)
                                            PLC_Fx5.SetSingleBit("M30", true);
                                        Step_ReadCode = 0;
                                        break;
                                    }
                                }
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    ShowResultProcessReadCode("WAIT");
                                    dgv_VisionReadcode.Rows.Clear();
                                }));
                                checkNoLock = false;
                                checkQualifyjig = false;
                                Step_ReadCode = 10; // Start Process 
                            }
                            else
                            {

                                c_varGolbal._isProduct = false;
                                Step_ReadCode = 0;
                            }
                        }
                        catch (Exception Ex)
                        {
                            //log.Debug($"Result:{Ex}");
                            //PC Send To PLC: Error
                            if (PLC_Fx5.Connected)
                                PLC_Fx5.SetSingleBit("M30", true);
                            Step_ReadCode = 0;
                            new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Error Process Scan Bit Trigger Vision Read Code \r {Ex}").ShowDialog();
                            //frm_show_MainVisonBarcode.ShowDialog();
                        }
                        break;
                }
            }
        }

        #endregion

        #region Even Oject Control from FORM MAIN 
        /// <summary>
        /// Nút nhấn tạo mới Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmCreateProgram frm = new frmCreateProgram(ID_PrgCurrent);
            frm.FormClosed += Frm_FormClosed;
            frm.ShowDialog();
        }
        /// <summary>
        /// Bắt sự kiện đóng form tạo model 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadProgram(ID_PrgCurrent);
        }
        /// <summary>
        /// Clear Selection dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_VisionReadcode_SelectionChanged(object sender, EventArgs e)
        {
            dgv_VisionReadcode.ClearSelection();
        }
        /// <summary>
        /// Event hỗ trợ di chuyển Form Main
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pn_Top_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        /// <summary>
        /// Event Resize Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            this.Close();
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in lst_CameraReader)
                {
                    item.Clear_HSmart_Window();
                }
                dgv_VisionReadcode.Rows.Clear();
                ShowResultProcessReadCode("WAIT");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private int typeModel = 0;
        /// <summary>
        /// Event Cbx Control by User (Lock Even while add data)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboProgram.SelectedValue == null)
            {
                ID_PrgCurrent = -1;
                tbPanelCamera.Controls.Clear();
                tbPanelBarcode.Controls.Remove(Barcode_Vision);
                return;
            }
            else
            {
                DataRow[] arrR = _dtProgram.Select($"ID_Program = {cboProgram.SelectedValue}");
                string a = arrR[0]["ProgramName"].ToString();
                lb_qtyCam.Text = 2.ToString();// arrR[0]["NumberCamera"].ToString();
                //lb_JigTransfer.Text = arrR[0]["TransferJig"].ToString();
                lb_ReadCodePCS.Text = arrR[0]["ReadCodePCS"].ToString();
                ID_PrgCurrent = Convert.ToInt32(cboProgram.SelectedValue);
                lb_JigPCSHaveBarcode.Text = arrR[0]["JigPCSHaveBarcode"].ToString();
                c_varGolbal.ProgramName = a;
                cbTypeModel.SelectedIndex = typeModel = Support_SQL.ToInt(arrR[0]["TypeModel"]);
                usingFVI = chkUsingFVI.Checked = Support_SQL.ToBoolean(arrR[0]["UsingFVI"]);
                c_varGolbal.QtyPcs = Support_SQL.ToInt(arrR[0]["QtyPcs"]);
                c_varGolbal.TakeJigHavePcs = Lib.ToBoolean(arrR[0]["TakeJigHavePcs"]);
            }

            //CAMERA
            //Clear giao diện và đối tượng camera
            tbPanelCamera.Controls.Clear();
            lst_CameraReader.Clear();
            //Set Table panel layout Vision
            for (int i = 1; i <= int.Parse(lb_qtyCam.Text); i++)
            {
                uc_Vision cs_Vision = new uc_Vision(ID_PrgCurrent, i);
                tbPanelCamera.Controls.Add(cs_Vision);
                lst_CameraReader.Add(cs_Vision);
            }


            //BARCODE
            //Clear giao diện và đối tượng Barcode Vision
            tbPanelBarcode.Controls.Remove(Barcode_Vision);
            Barcode_Vision = null;
            // Set Table panel layout Barcode for Vision
            uc_BarcodeVision barcode_Vision = new uc_BarcodeVision();
            tbPanelBarcode.Controls.Add(barcode_Vision, 0, 0);
            Barcode_Vision = barcode_Vision;
            barcode_Vision._ProgramID = ID_PrgCurrent;
            if (usingFVI)
            {
                col_CheckLock.Visible = true;
                col_CheckQualifyJig.Visible = true;
            }
            else
            {
                col_CheckLock.Visible = false;
                col_CheckQualifyJig.Visible = false;
            }
            //focus
            this.ActiveControl = btnRunOrStop;
            QuyenSuDung();
        }
        #endregion

        #region Function Support
        /// <summary>
        /// Ngắt toàn bộ các thiết bị đang kết nối
        /// </summary>
        private void DisconnectAllDevice()
        {
            try
            {
                //Kiểm tra Barcode Vision đang có kết nối thì sẽ ngắt kết nối
                if (Barcode_Vision._IsConected)
                {
                    Barcode_Vision.DisconnectBarcodeVision();
                }
                //Kiểm tra camera nào đang có kết nối thì sẽ ngắt kết nối
                for (int i = 0; i < lst_CameraReader.Count; i++)
                {
                    if (lst_CameraReader[i]._IsConected)
                    {
                        lst_CameraReader[i].DisconnectCamera();
                    }
                }
                //Kiểm tra PLC  đang có kết nối thì sẽ ngắt kết nối
                if (PLC_Fx5.Connected || PLC_Fx5 != null)
                {
                    PLC_Fx5.Disconnect();
                }
                //Clear GridView
                dgv_VisionReadcode.Rows.Clear();
            }
            catch (Exception ex)
            {
                Support_SQL.saveToLog(ex.ToString() + "DisconnectAllDevice");
            }
        }

        /// <summary>
        /// Load Program add Combobox Program
        /// </summary>
        private void loadProgram(int ID_PrgSelec)
        {
            // load Program from Database
            if (_dtProgram != null) _dtProgram.Clear();
            string sqlQuery = "select * from ProgramMain";
            _dtProgram = Support_SQL.GetTableData(sqlQuery);
            // Add data program to Combobox
            cboProgram.SelectedIndexChanged -= CboProgram_SelectedIndexChanged;
            cboProgram.DataSource = _dtProgram;
            cboProgram.DisplayMember = "ProgramName";
            cboProgram.ValueMember = "ID_Program";
            cboProgram.SelectedValue = -1;
            cboProgram.SelectedIndexChanged += CboProgram_SelectedIndexChanged;
            cboProgram.SelectedValue = ID_PrgSelec;
        }

        /// <summary>
        /// hiển thị các dữ liệu barcode đọc đc lên data grid view
        /// </summary>
        private void dgv_ShowInfoVision()
        {
            dgv_VisionReadcode.Rows.Clear();
            foreach (dataVision item in lst_dataVision)
            {
                using (DataGridViewRow row = new DataGridViewRow())
                {
                    row.Height = (int)20;
                    row.CreateCells(dgv_VisionReadcode);
                    row.Cells[colCamIndex.Index].Value = item.CamIndex;
                    row.Cells[colNo_VisionReadcode.Index].Value = item.PcsIndex;
                    row.Cells[colDataBarcode_VisionReadcode.Index].Value = item.PcsBarcode;
                    row.Cells[colTagJigBarcode1.Index].Value = item.TagJigHavePCS;
                    row.Cells[colTagJigBarcode2.Index].Value = item.TagJigNonePCS;
                    //HA_ADD
                    if (item.statusICT)
                    {
                        row.Cells[colStatusICT.Index].Value = "Pass ICT";
                    }
                    else if (item.PcsBarcode.Contains(c_varGolbal.Missing))
                    {
                        row.Cells[colStatusICT.Index].Value = c_varGolbal.Missing;
                    }
                    else if (item.PcsBarcode.Contains(c_varGolbal.NoReadString))
                    {
                        row.Cells[colStatusICT.Index].Value = c_varGolbal.NoReadString;
                    }
                    else
                    {
                        row.Cells[colStatusICT.Index].Value = "No ICT";
                    }
                    if (item.PcsIndex.Contains("="))
                    {
                        row.Cells[colStatusICT.Index].Value = GlobVar.doubleCode;
                    }
                    row.Cells[colStatusFinishLot.Index].Value = item.statusFinishLot;
                    if (usingFVI)
                    {
                        if (item.NoLock == 1)
                        {
                            row.Cells[col_CheckLock.Index].Value = GlobVar.NoLock;
                        }
                        else
                        {
                            row.Cells[col_CheckLock.Index].Value = GlobVar.Lock;
                        }
                        if (item.QualifyJig == 1)
                        {
                            row.Cells[col_CheckQualifyJig.Index].Value = GlobVar.OK;
                        }
                        else
                        {
                            row.Cells[col_CheckQualifyJig.Index].Value = GlobVar.NG;
                        }
                    }
                    dgv_VisionReadcode.Rows.Add(row);
                }
            }
        }
        /// <summary>
        /// Truong hop chi doc 2 BarCode
        /// </summary>
        private void dgv_ShowInfoBarCode(string CodePcs)
        {
            dgv_VisionReadcode.Rows.Clear();
            foreach (dataVision item in lst_dataVision)
            {
                using (DataGridViewRow row = new DataGridViewRow())
                {
                    row.Height = (int)20;
                    row.CreateCells(dgv_VisionReadcode);
                    row.Cells[0].Value = item.CamIndex;
                    row.Cells[1].Value = 1;
                    row.Cells[2].Value = CodePcs;
                    row.Cells[3].Value = item.TagJigNonePCS;
                    row.Cells[4].Value = item.TagJigHavePCS;
                    row.Cells[colStatusICT.Index].Value = "Pass ICT";
                    if (usingFVI)
                    {
                        if (item.NoLock == 1)
                        {
                            row.Cells[col_CheckLock.Index].Value = GlobVar.NoLock;
                        }
                        else
                        {
                            row.Cells[col_CheckLock.Index].Value = GlobVar.Lock;
                        }
                        if (item.QualifyJig == 1)
                        {
                            row.Cells[col_CheckQualifyJig.Index].Value = GlobVar.OK;
                        }
                        else
                        {
                            row.Cells[col_CheckQualifyJig.Index].Value = GlobVar.NG;
                        }
                    }
                    dgv_VisionReadcode.Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// Hiển thị trạng thái hoạt động của máy
        /// </summary>
        private void displayStatusMachine(string status)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lbStatus_BOT.Text = status;
            }));
        }
        /// <summary>
        /// Timeout lệnh thực thi
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="codeBlock"></param>
        /// <returns></returns>
        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
        /// <summary>
        /// Hiển thị kết quả chu trình đọc code
        /// </summary>
        /// <param name="result"></param>
        bool continueStep40 = false;
        public void ShowResultProcessReadCode(string result)
        {
            continueStep40 = false;
            if (result.Contains("OK"))
            {
                if (c_varGolbal.Finish_Lot)
                {
                    FinishLot.ktLot = true;
                    FinishLot.Time = DateTime.Now;
                }
            }
            else
            {
                FinishLot.ktLot = false;
            }
            switch (result)
            {
                case "OK":
                    btnResultReadcode.BackColor = Color.Green;
                    btnResultReadcode.Text = "OK";
                    btnResultReadcode.ForeColor = Color.White;

                    break;
                case "NG":
                    btnResultReadcode.BackColor = Color.Red;
                    btnResultReadcode.Text = "NG";
                    btnResultReadcode.ForeColor = Color.Black;
                    break;
                case "WAIT":
                    btnResultReadcode.BackColor = Color.FromArgb(40, 46, 56);
                    btnResultReadcode.Text = "WAIT";
                    btnResultReadcode.ForeColor = Color.White;
                    break;
                case "DOUBLE CODE":
                    btnResultReadcode.BackColor = Color.Red;
                    btnResultReadcode.Text = result;
                    btnResultReadcode.ForeColor = Color.Black;
                    break;
                default:
                    break;
            }
            if (!result.Contains("WAIT"))
            {
                BeginSnap = false;
                CompleteResult = true;
                TimerShowKQ.Stop();
                TimerShowKQ.Tick += TimerShowKQ_Tick; ; ;
                TimerShowKQ.Interval = 10 * 1000;
                TimerShowKQ.Enabled = true;
                TimerShowKQ.Start();
            }
            else
            {
                BeginSnap = false;
                CompleteResult = false;
            }
            continueStep40 = true;
        }
        private void TimerShowKQ_Tick(object sender, EventArgs e)
        {
            if (!BeginSnap && CompleteResult)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    ShowResultProcessReadCode("WAIT");
                }));
            }
            TimerShowKQ.Stop();
            TimerShowKQ.Enabled = false;
        }
        /// <summary>
        /// Upload dữ liệu file Csv.
        /// </summary>
        private void Upload_DB_Excel(List<dataReadcode> lst_Data)
        {
            // Get date time of save log
            string pathExcel = "";
            string Date_Time_Log = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string Path_FolderLog = @"C:\MMCV_ReadCode\" + Path.Combine(c_varGolbal.MPN, c_varGolbal.LotID);
            //string Path_FolderLog = @"C:\MMCV_Plasma\";
            if (!Directory.Exists(Path_FolderLog))
            {
                Directory.CreateDirectory(Path_FolderLog);
            }
            //pathExcel = System.IO.Path.Combine(Path_FolderLog, $"{c_varGolbal.NamePlasma}");
            pathExcel = System.IO.Path.Combine(Path_FolderLog, $"{c_varGolbal.NameMachine}");
            if (!File.Exists(pathExcel + ".xlsx"))
            {
                if (!Excel.CreatFileExcel_XLSX(Path_FolderLog, $"{c_varGolbal.NameMachine}"))//CreatFileExcel_CSV
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(this, "Không tạo mới được file Excel!", "Lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    return;
                }
                if (!Excel.WriteData_xlsx_gem(lst_Data))
                {
                    this.Invoke(new Action(delegate
                    {
                        MessageBox.Show(this, "Không ghi được dữ liệu xuống file Excel!", "Lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));

                }
            }
            else
            {
                if (!Excel.WriteData_xlsx_gem(lst_Data, pathExcel + ".xlsx"))
                {
                    this.Invoke(new Action(delegate
                    {
                        MessageBox.Show(this, "Không ghi được dữ liệu xuống file Excel!", "Lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            }
        }
        // --------------------------------------------------------------------------------------------------------------------//

        #endregion

        #region Button click
        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            frm_Login newfrm = new frm_Login();
            if (newfrm.ShowDialog() == DialogResult.OK)
            {
                DisconnectAllDevice();
                QuyenSuDung();
            }
        }

        private void btnLanguages_Click(object sender, EventArgs e)
        {
            if (btnLanguages.Text == "ENG")
            {
                Support_SQL.ChangeLang(this.Controls, Lang.Vie);
                btnLanguages.Text = "VIE";
            }
            else if (btnLanguages.Text == "VIE")
            {
                Support_SQL.ChangeLang(this.Controls, Lang.Eng);
                btnLanguages.Text = "ENG";
            }
        }
        private void Txt_StaffID_TextChanged(object sender, EventArgs e)
        {
            c_varGolbal.StaffID = txt_StaffID.Text;
        }

        private void btnFormData_Click(object sender, EventArgs e)
        {
            frm_Data newfrm = new frm_Data();
            newfrm.ShowDialog();

        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (btnChangeUser.Text.ToUpper() == "ADMIN" && btnRunOrStop.Text.ToUpper() == "STOP")
            {
                c_varGolbal._isProduct = true;
            }
        }

        bool isTest = false;

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (btnChangeUser.Text.ToUpper() == "ADMIN" && btnRunOrStop.Text.ToUpper() == "STOP")
            {
                if (isTest)
                {
                    isTest = false;
                }
                else
                {
                    isTest = true;
                }
            }

        }
        #endregion

        Thread _run_Auto, _reconnect_PLC;
        void Thread_Run_Auto()
        {
            _run_Auto = new Thread(new ThreadStart(_Run_Auto));
            _run_Auto.IsBackground = true;
            _run_Auto.Start();
        }

        void Thread_Reconnect_PLC()
        {
            _reconnect_PLC = new Thread(new ThreadStart(Reconnect_PLC));
            _reconnect_PLC.IsBackground = true;
            _reconnect_PLC.Start();
        }

        void _Run_Auto()
        {
            while (c_varGolbal.IsRun)
            {
                Thread.Sleep(50);
                if (isTest)
                {
                    Thread.Sleep(3000);
                    try
                    {
                        if (isTest)
                        {
                            c_varGolbal._isProduct = true;
                        }
                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
                }
            }
        }

        // TODO: NB - 19032023
        bool IsConnect_Faild = false;
        void Reconnect_PLC()
        {
            while (c_varGolbal.IsRun)
            {
                Thread.Sleep(50);
                Ping ping = new Ping();
                byte[] buffer = Encoding.ASCII.GetBytes("samplestring");
                int timeout = 220;
                PingReply pingresult = ping.Send(c_varGolbal.IP_PLC, timeout, buffer);
                if (pingresult.Status.ToString().ToLower() == "success")
                {
                    if (IsConnect_Faild || !PLC_Fx5.Connected)
                    {
                        Thread.Sleep(500);
                        if (PLC_Fx5 != null) PLC_Fx5.Disconnect();
                        PLC_Fx5.Connect(c_varGolbal.IP_PLC, c_varGolbal.Port_PLC);
                        if (PLC_Fx5.Connected)
                        {
                            IsConnect_Faild = false;
                            Thread.Sleep(100);
                            if (lblStatusConnectPLC.InvokeRequired)
                            {
                                lblStatusConnectPLC.Invoke(new Action(() =>
                                {
                                    lblStatusConnectPLC.BackColor = Color.Lime;
                                    lblStatusConnectPLC.Text = "Connected";
                                }));
                            }
                            else
                            {
                                lblStatusConnectPLC.BackColor = Color.Lime;
                                lblStatusConnectPLC.Text = "Connected";
                            }
                        }
                    }
                }
                else
                {
                    if (!IsConnect_Faild)
                    {
                        IsConnect_Faild = true;
                        if (lblStatusConnectPLC.InvokeRequired)
                        {
                            lblStatusConnectPLC.Invoke(new Action(() =>
                            {
                                lblStatusConnectPLC.BackColor = Color.Red;
                                lblStatusConnectPLC.Text = "Disconnect";
                            }));
                        }
                        else
                        {
                            lblStatusConnectPLC.BackColor = Color.Red;
                            lblStatusConnectPLC.Text = "Disconnect";
                        }
                        Support_SQL.saveToLog($"Don't connect PLC, time {DateTime.Now}");
                    }
                }
                if (ping != null)
                {
                    ping.Dispose();
                }
            }
        }

        private void BtnAn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void dgv_VisionReadcode_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.Black;
            if (e.RowIndex >= 0)
            {
                if (e.Value != null && e.Value.ToString() == c_varGolbal.NoReadString)
                {
                    e.CellStyle.BackColor = Color.Magenta;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (e.Value != null && e.Value.ToString() == c_varGolbal.Missing)
                {
                    e.CellStyle.BackColor = Color.Magenta;
                    e.CellStyle.ForeColor = Color.Black;
                }
                //HA_ADD
                ///Màu trạng thái cột Status ICT
                if (e.ColumnIndex == colStatusICT.Index)
                {
                    string chkICT = Lib.ToString(dgv_VisionReadcode[colStatusICT.Index, e.RowIndex].Value);
                    string chkNoRead = Lib.ToString(dgv_VisionReadcode[colDataBarcode_VisionReadcode.Index, e.RowIndex].Value);
                    string chkDouble = Lib.ToString(dgv_VisionReadcode[colStatusICT.Index, e.RowIndex].Value);
                    bool chkFinishLot = Support_SQL.ToBoolean(dgv_VisionReadcode[colStatusFinishLot.Index, e.RowIndex].Value);
                    if (chkFinishLot && chkNoRead.Contains(c_varGolbal.Missing))
                    {
                        e.CellStyle.BackColor = Color.Magenta;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (chkICT.Contains("No ICT") || chkNoRead.Contains(c_varGolbal.NoReadString))
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                    }
                    else if (chkDouble.Contains(GlobVar.doubleCode))
                    {
                        e.CellStyle.BackColor = Color.Orange;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }

                ///Màu trạng thái cột NO.
                if (e.ColumnIndex == colNo_VisionReadcode.Index)
                {
                    string chkICT = Lib.ToString(dgv_VisionReadcode[colStatusICT.Index, e.RowIndex].Value);
                    string chkNoRead = Lib.ToString(dgv_VisionReadcode[colDataBarcode_VisionReadcode.Index, e.RowIndex].Value);
                    bool chkFinishLot = Support_SQL.ToBoolean(dgv_VisionReadcode[colStatusFinishLot.Index, e.RowIndex].Value);
                    string chkDouble = Lib.ToString(dgv_VisionReadcode[colNo_VisionReadcode.Index, e.RowIndex].Value);
                    if (chkFinishLot && chkNoRead.Contains(c_varGolbal.Missing))
                    {
                        e.CellStyle.BackColor = Color.Magenta;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (chkICT.Contains("No ICT") || chkNoRead.Contains(c_varGolbal.NoReadString))
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                    }
                    else if (chkDouble.Contains("="))
                    {
                        e.CellStyle.BackColor = Color.Orange;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }

                ///Màu cột Pcs
                if (e.ColumnIndex == colDataBarcode_VisionReadcode.Index)
                {
                    string chkDouble = Lib.ToString(dgv_VisionReadcode[colNo_VisionReadcode.Index, e.RowIndex].Value);
                    if (chkDouble.Contains("="))
                    {
                        e.CellStyle.BackColor = Color.Orange;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }
                ///Màu cột Check Lock And Qualify Jig
                if (e.ColumnIndex == col_CheckLock.Index)
                {
                    if (e.Value != null && e.Value.ToString() == GlobVar.Lock)
                    {
                        e.CellStyle.BackColor = Color.Yellow;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }
                if (e.ColumnIndex == col_CheckQualifyJig.Index)
                {
                    if (e.Value != null && e.Value.ToString() == GlobVar.NG)
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                    }
                }

            }
        }
        List<Result> lstResult = new List<Result>();
        private void lbReadCodePCS_Click(object sender, EventArgs e)
        {
            btnReset.PerformClick();
            for (int i = 0; i < lst_CameraReader.Count; i++)
            {
                if (!lst_CameraReader[i]._IsConected)
                {
                    lst_CameraReader[i].ConnectCamera();
                }
                else
                {
                    lst_CameraReader[i].DisconnectCamera();
                    Thread.Sleep(100);
                    lst_CameraReader[i].ConnectCamera();
                }
                // if a device not conneced -> Disconnect all device 
                if (!lst_CameraReader[i]._IsConected)
                {
                    DisconnectAllDevice();

                    return;
                }
            }
            Thread.Sleep(100);
            oldPosition = 0;
            lst_dataVision.Clear();
            for (int i = 0; i < lstResult.Count; i++)
            {
                foreach (Code item in lstResult[i].ListData)
                {
                    dataVision infoVision = new dataVision();
                    infoVision.CamIndex = item.CamIndex.ToString();
                    infoVision.PcsIndex = item.IDRegion.ToString();
                    infoVision.PcsBarcode = item.Content;
                    infoVision.TagJigNonePCS = code_TagJigNonePCS;
                    infoVision.TagJigHavePCS = code_TagJigHavePCS;
                    c_varGolbal.List_DataCode.Add(item.Content);
                    lst_dataVision.Add(infoVision);
                    if (infoVision.PcsBarcode.ToUpper() == "NOREAD")
                    {
                        _flag_OK_NG = false;
                    }
                }
            }
            lstResult.Clear();
            Task.Factory.StartNew(dgv_ShowInfoVision);
        }

        private void chkSkipFVI_CheckedChanged(object sender, EventArgs e)
        {
            skipFVI = chkSkipFVI.Checked;
        }

        #region test
        private void button1_Click(object sender, EventArgs e)
        {
            c_varGolbal.StaffID = "HA";
            c_varGolbal.MPN = "MPN_TEST";
            c_varGolbal.LotID = "1234";
            List<dataReadcode> lst = new List<dataReadcode>();
            for (int i = 0; i < 1; i++)
            {
                dataReadcode dataReadCode = new dataReadcode();
                dataReadCode.TagJig1 = $"Jig_1";
                dataReadCode.TagJig2 = $"Jig_2";
                dataReadCode.PcsBarcode = $"AAA,BBB,CCC,DDD,EEE,FFF,GGG,HHH,III";
                dataReadCode.DateTimeIn = "Thời gian Jig vào";
                dataReadCode.FinishLot = "YES";
                dataReadCode.Status = "OK";
                lst.Add(dataReadCode);
            }

            //List<dataVision> lst = new List<dataVision>();
            //for (int i = 0; i < 40; i++)
            //{
            //    dataVision infoVision = new dataVision();
            //    infoVision.ID = i + 1;
            //    infoVision.CamIndex = 1 + "";
            //    infoVision.PcsIndex = i + 1 + "";
            //    infoVision.PcsBarcode = $"ABCD{i + 10}MN";
            //    infoVision.TagJigNonePCS = "JigNonePCS";
            //    infoVision.TagJigHavePCS = "JigHavePCS";
            //    infoVision.statusICT = true; //HA_ADD
            //    lst.Add(infoVision);
            //}
            //MessageBox.Show("HAHAHAHA", "", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly);
            //c_varGolbal.StaffID = "HA";
            //c_varGolbal.MPN = "MPN_TEST";
            //c_varGolbal.LotID = "1234";
            Upload_DB_Excel(lst);
            //this.Invoke(new MethodInvoker(delegate
            //{
            //    // Hiển thị dữ liệu lên data grid view plasma
            //    dgv_ShowInfoVision(lst);
            //}));
        }
        /// <summary>
        /// hiển thị các dữ liệu barcode đọc đc lên data grid view
        /// </summary>
        private void dgv_ShowInfoVision(List<dataVision> lst)
        {
            dgv_VisionReadcode.Rows.Clear();
            foreach (dataVision item in lst)
            {
                using (DataGridViewRow row = new DataGridViewRow())
                {
                    row.Height = (int)20;
                    row.CreateCells(dgv_VisionReadcode);
                    row.Cells[colCamIndex.Index].Value = item.CamIndex;
                    row.Cells[colNo_VisionReadcode.Index].Value = item.PcsIndex;
                    row.Cells[colDataBarcode_VisionReadcode.Index].Value = item.PcsBarcode;
                    row.Cells[colTagJigBarcode1.Index].Value = item.TagJigHavePCS;
                    row.Cells[colTagJigBarcode2.Index].Value = item.TagJigNonePCS;
                    //HA_ADD
                    if (item.statusICT)
                    {
                        row.Cells[colStatusICT.Index].Value = "Pass ICT";
                    }
                    else if (item.PcsBarcode.Contains(c_varGolbal.Missing))
                    {
                        row.Cells[colStatusICT.Index].Value = c_varGolbal.Missing;
                    }
                    else if (item.PcsBarcode.Contains(c_varGolbal.NoReadString))
                    {
                        row.Cells[colStatusICT.Index].Value = c_varGolbal.NoReadString;
                    }
                    else
                    {
                        row.Cells[colStatusICT.Index].Value = "No ICT";
                    }
                    if (item.PcsIndex.Contains("="))
                    {
                        row.Cells[colStatusICT.Index].Value = GlobVar.doubleCode;
                    }
                    row.Cells[colStatusFinishLot.Index].Value = item.statusFinishLot;
                    if (usingFVI)
                    {
                        if (item.NoLock == 1)
                        {
                            row.Cells[col_CheckLock.Index].Value = GlobVar.NoLock;
                        }
                        else
                        {
                            row.Cells[col_CheckLock.Index].Value = GlobVar.Lock;
                        }
                        if (item.QualifyJig == 1)
                        {
                            row.Cells[col_CheckQualifyJig.Index].Value = GlobVar.OK;
                        }
                        else
                        {
                            row.Cells[col_CheckQualifyJig.Index].Value = GlobVar.NG;
                        }
                    }
                    dgv_VisionReadcode.Rows.Add(row);
                }
            }
        }

        #endregion

        public List<string> CheckHaveDataPlasma(List<dataVision> listPcs)
        {

            List<string> Result = new List<string>();
            List<string> data = new List<string>();
            foreach (dataVision item in listPcs)
            {
                data.Add($"'{item.PcsBarcode}'");
            }
            #region dùng thư viện nhà máy
            try
            {
                Plasma56 mmcv = new Plasma56();
                Result = mmcv.CheckDataPlasma(data);
            }
            #endregion
            catch (Exception ex)
            {
                Lib.SaveToLog("ExceptionCheckData56", $"TagJigHavePCS:{listPcs[0].TagJigHavePCS}-TagJigNonePCS:{listPcs[0].TagJigNonePCS}", ex.ToString());
                Lib.ShowError(ex.ToString());
                Result.Add(GlobVar.Error56);
            }
            return Result;
        }
        private void dgv_VisionReadcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Clipboard.SetText(Convert.ToString(dgv_VisionReadcode.CurrentCell.Value));
                e.Handled = true;
            }
        }

        private void btnTestStart_Click(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            if (!txtMPN.Text.Contains("WAIT"))
            {
                TimerShowKQ.Stop();
                TimerShowKQ.Tick += TimerShowKQ_Tick; ; ;
                TimerShowKQ.Interval = 5 * 1000;
                TimerShowKQ.Enabled = true;
                TimerShowKQ.Start();
            }
            this.Invoke(new MethodInvoker(delegate
            {
                ShowResultProcessReadCode(txtMPN.Text);
            }));
        }



        // NB - 19032023
        private void ChkFinishLot_CheckedChanged(object sender, EventArgs e)
        {
            c_varGolbal.Finish_Lot = chkFinishLot.Checked;
        }

        class CompleteFinishLot
        {
            public bool ktLot { get; set; }
            public DateTime Time { get; set; }
        }
    }
}
