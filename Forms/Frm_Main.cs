using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseInterface_MMCV;
using System.Xml;
using MC_Protocol_NTT;
using ActUtlTypeLib;
using System.Diagnostics;
using System.IO;
using WaitWnd;

namespace LineGolden_PLasma
{
    public partial class Frm_Main : Form
    {

        // Cho phép di chuyển Form Main
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        // --------------------------------------------------------------------------------------------------------------------//

        #region variable Genenal
        // Genenal
        private int ID_PrgCurrent = -1;
        private string Status_Machine = "STOP";
        public static bool BackupSuccess = false;
        bool CompleteStart = false;

        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable DataTable
        // DataTable
        DataTable _dtProgram;//datatable chương trình lựa chọn để chạy
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable List
        // List
        List<uc_Plasma> Lst_Plasma = new List<uc_Plasma>();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Server Mektec
        // Server Mektec
        DAL MMCV_DBGetMPN = new DAL();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable PLC
        // PLC
        MC_Protocol PLC_Fx5 = new MC_Protocol();

        //Use MXComponent
        ActUtlTypeClass PLC_Fx3 = new ActUtlTypeClass();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Thread
        private Thread Main_ThreadAlive;
        private Thread MainKill_Process;
        #endregion

        public Frm_Main()
        {
            InitializeComponent();
           
        }

        #region Load Frm_Main
        /// <summary>
        /// Load Display Main Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            GlobVar.LockEvent = true;
            c_varGolbal.str_ConnectDBConffig = "Data Source = " + Application.StartupPath + "\\Config\\DB_ConfigDevice.db;Version=3;";
            c_varGolbal.List_LinkDB = new List<PathFile>();
            //HuyNV 24-02-2023
            //Load Model CodeTray
            DataTable dt_Model_CodeTray = Support_SQL.GetTableData("Select * from ModelCodeTray");

            foreach (DataRow item in dt_Model_CodeTray.Rows)
            {
                c_varGolbal.List_Model_CodeTray.Add(Support_SQL.ToString(item["CodeTray"]));
            }

            try
            {
                Support_SQL.ViewTimeLabel(lbViewTime);
            }
            catch (Exception Ex)
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Error Process Loading Data Setting! \r {Ex}").ShowDialog();
            }


            //Load SationNumber
            string sqlString = "Select * from SettingPLC";
            DataTable dt = Support_SQL.GetTableData(sqlString);
            if (dt.Rows.Count > 0)
            {
                c_varGolbal.LogicalStationNumberPlasma = Lib.ToInt(dt.Rows[0]["StationNumber"]);
            }
            else
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Station Number is not found").ShowDialog();
            }

            #region load file Config.xml
            /// load file API.xml
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("Config.xml");
                XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/Config");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    c_varGolbal.IP_SMT = xmlNode.SelectSingleNode("IP_SERVER_SMT").InnerText;
                }
            }
            catch (Exception ex)
            {
                Lib.ShowError(ex);
                Application.Exit();
                return;
            }
            #endregion

            #region load file API.xml
            /// load file API.xml
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("API.xml");
                XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/Config");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    GlobVar.IPServer = xmlNode.SelectSingleNode("IP").InnerText;
                    GlobVar.PortServer = xmlNode.SelectSingleNode("Port").InnerText;
                    GlobVar.MODOEE = xmlNode.SelectSingleNode("OEE").InnerText;
                    GlobVar.MODMES = xmlNode.SelectSingleNode("MES").InnerText;
                    GlobVar.UploadServer = (Lib.ToInt(xmlNode.SelectSingleNode("UploadServer").InnerText) == 1) ? true : false;
                }
            }
            catch (Exception ex)
            {
                Lib.ShowError(ex);
                Application.Exit();
                return;
            }


            #endregion

            #region load file SettingMachine.xml
            // load setting machine
            DisplayStatusMachine("Load setting machine");
            c_varGolbal.str_ConnectDBConffig = "Data Source = " + Application.StartupPath + "\\Config\\DB_ConfigDevice.db;Version=3;";
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("SettingMachine.xml");
                XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/Setting");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    c_varGolbal.IsUploadPlasma = xmlNode.SelectSingleNode("UploadPlasma").InnerText == "true";
                }
                //Support_SQL.ViewTimeLabel(lbViewTime);
            }
            catch (Exception Ex)
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Error Process Loading Data Setting! \r {Ex}").ShowDialog();
            }
            // --------------------------------------------------------------------------------------------------------------------//
            #endregion

            #region load file ConfigPlasma.xml
            // Load Line ID and Device ID of each Plasma machine from ConfigPlasma.xml
            try
            {
                XmlDocument xmlCF = new XmlDocument();
                xmlCF.Load("ConfigPlasma.xml");
                XmlNodeList xmlListCF = xmlCF.DocumentElement.SelectNodes("/Config");
                foreach (XmlNode xmlNode in xmlListCF)
                {
                    c_varGolbal.LineID = xmlNode.SelectSingleNode("LineID").InnerText;
                    txt_LineID.Text = c_varGolbal.LineID;
                    c_varGolbal.DeviceID = xmlNode.SelectSingleNode("DeviceID").InnerText;
                    txt_DeviceID.Text = c_varGolbal.DeviceID;
                    c_varGolbal.NameMachine = xmlNode.SelectSingleNode("NamePlasma").InnerText;
                    c_varGolbal.NamePlasma = c_varGolbal.NameMachine;
                    txt_Machine.Text = c_varGolbal.NameMachine;
                    c_varGolbal.str_ConnectDBReadCode_BeforePlasma1 = xmlNode.SelectSingleNode("DB_ReadCode_BeforePlasma1").InnerText;
                    c_varGolbal.str_ConnectDBReadCode_BeforePlasma2 = xmlNode.SelectSingleNode("DB_ReadCode_BeforePlasma2").InnerText;
                    c_varGolbal.QtyBeforePlasma = Support_SQL.ToInt(xmlNode.SelectSingleNode("QtyBeforePlasma").InnerText);
                    c_varGolbal.str_ConnectDBReadCode_FVI = xmlNode.SelectSingleNode("DB_ReadCode_FVI").InnerText;
                    c_varGolbal.RouteID = xmlNode.SelectSingleNode("RouteID").InnerText;
                    c_varGolbal.str_MachineVersion = xmlNode.SelectSingleNode("Version").InnerText;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Load ConfigPlasma.xml of Plasma machine false \r " + Ex.ToString(), "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            // --------------------------------------------------------------------------------------------------------------------//
            #endregion

            #region Check version dll MMCV
            try
            {
                Plasma56 mmcv = new Plasma56();
                string result = mmcv.CheckVersion();
                if (!result.ToUpper().Contains("OK"))
                {
                    Lib.ShowError(result);
                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                Lib.ShowError("Exception", ex);
                Application.Exit();
            }

            #endregion

            lblStauts_MachineName_Version.Text = c_varGolbal.str_MachineVersion;
            //
            txt_Machine.Text = c_varGolbal.NameMachine;
            // Set PLC
            DisplayStatusMachine("Set IP, Port from setting machine file for PLC");
            // Load Initial Program
            DisplayStatusMachine("Load initial program");
            LoadProgram(-1);

            #region Setting link DB máy ReadCode
            if (c_varGolbal.QtyBeforePlasma == 2)
            {
                c_varGolbal.List_LinkDB = new List<PathFile>();
                PathFile file1 = new PathFile(1, c_varGolbal.str_ConnectDBReadCode_BeforePlasma1, true);
                PathFile file2 = new PathFile(2, c_varGolbal.str_ConnectDBReadCode_BeforePlasma2, false);
                c_varGolbal.List_LinkDB.Add(file1);
                c_varGolbal.List_LinkDB.Add(file2);
                chkLinkDB_ReadCode1.Checked = true;
                chkLinkDB_ReadCode2.Checked = false;
            }
            else
            {
                chkLinkDB_ReadCode1.Visible = false;
                chkLinkDB_ReadCode2.Visible = false;
            }
            #endregion

            if (c_varGolbal.IsUploadPlasma)
            {
                txt_MPN.Enabled = false;
            }
            else
            {
                txt_MPN.Enabled = true;
            }

            btnRunOrStop.Text = "RUN";
            btnRunOrStop.BackColor = Color.Green;
            QuyenSuDung();
            GlobVar.LockEvent = false;
            CompleteStart = false;
            MainThreadKillProcess();
        }

        private void LoadProgram(int ID_PrgSelect)
        {
            if (_dtProgram != null) _dtProgram.Clear();
            string sqlQuery = "select * from ProgramMain";
            _dtProgram = Support_SQL.GetTableData(sqlQuery);
            //Add data program to combobox
            cboProgram.SelectedIndexChanged -= CboProgram_SelectedIndexChanged;
            cboProgram.DataSource = _dtProgram;
            cboProgram.DisplayMember = $"{PrgMain.PrgName}";
            cboProgram.ValueMember = $"{PrgMain.ID}";
            cboProgram.SelectedValue = -1;
            cboProgram.SelectedIndexChanged += CboProgram_SelectedIndexChanged;
            cboProgram.SelectedValue = ID_PrgSelect;
        }



        #endregion

        #region Control thanh Taskbar
        private void btnExit_Click(object sender, EventArgs e)
        {
            //ConnectAllInPlasma();
            OEE WAIT = new OEE();
            WAIT.SetParam(c_varGolbal.DeviceID, JsonValueDE.stateWait, "1", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            if (MainKill_Process != null) MainKill_Process.Abort();
            //JsonFunc.OEEsubmit(WAIT, out string msg);
            this.Close();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                StartPosition = FormStartPosition.CenterScreen;
                btnMaximize.Image = global::LineGolden_PLasma.Properties.Resources.icons8_maximize_button_24px;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                btnMaximize.Image = global::LineGolden_PLasma.Properties.Resources.icons8_restore_down_24px;
            }
        }

        private void pn_Top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WindowState = FormWindowState.Normal;
                btnMaximize.Image = global::LineGolden_PLasma.Properties.Resources.icons8_maximize_button_24px;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region Event Object Control from Frm_Main 
        /// <summary>
        /// Nút nhấn tạo mới Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            FrmCreateProgram frm = new FrmCreateProgram(ID_PrgCurrent);
            frm.FormClosed += Frm_FormClosed;
            frm.ShowDialog();
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadProgram(ID_PrgCurrent);
        }
        #endregion

        #region lựa chọn Program
        //lựa chọn Program
        private void CboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProgram.SelectedItem == null || cboProgram.SelectedValue == null)
            {
                ID_PrgCurrent = -1;
                tbPanelPlasma.Controls.Clear();
                return;
            }
            else
            {

                DataRow[] arrR = _dtProgram.Select($"{PrgMain.ID} = {cboProgram.SelectedValue}");
                string ProgramName = arrR[0][PrgMain.PrgName].ToString();
                //lb_qtyCam.Text = arrR[0][PrgMain.NumberCamera].ToString();
                lb_JigTransfer.Text = arrR[0][PrgMain.TransferJig].ToString();
                lb_ReadCodePCS.Text = arrR[0][PrgMain.ReadCodePCS].ToString();
                ID_PrgCurrent = Convert.ToInt32(cboProgram.SelectedValue);
                c_varGolbal.NumJigPlasma = Convert.ToInt16(arrR[0][PrgMain.NumJigPlasma]);
                c_varGolbal.TimeRepeatJig = Convert.ToInt32(arrR[0][PrgMain.TimeRepeatJig]);
                lb_TimeRepeatJig.Text = c_varGolbal.TimeRepeatJig + " (min)";
                btnModel.Text = ProgramName;
                c_varGolbal.ProgramName = ProgramName;
                cbUseMachine.SelectedIndex = Support_SQL.ToInt(arrR[0]["UseMachine"]);
                c_varGolbal.UseMachine = Support_SQL.ToInt(arrR[0]["UseMachine"]);
                c_varGolbal.GetJigHavePcs = Lib.ToBoolean(arrR[0]["GetJigHavePcs"]);
                c_varGolbal.UseFvi = Lib.ToBoolean(arrR[0]["UseFvi"]);
            }

            //Clear giao diện và đối tượng Plasma
            tbPanelPlasma.Controls.Clear();
            Lst_Plasma.Clear();

            //Set Table panel layout Plasma
            tbPanelPlasma.SuspendLayout();
            tbPanelPlasma.RowStyles.Clear();
            tbPanelPlasma.ColumnStyles.Clear();
            tbPanelPlasma.Controls.Clear();
            tbPanelPlasma.ColumnCount = 1;
            tbPanelPlasma.RowCount = 1;

            tbPanelPlasma.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            uc_Plasma cs_Plasma = new uc_Plasma(this, ID_PrgCurrent, 1, 1 - 1, c_varGolbal.NumJigPlasma, lb_ReadCodePCS.Text);//Chỉ có 1 plasma => plasma index ==1
            cs_Plasma.Dock = DockStyle.Fill;
            tbPanelPlasma.Controls.Add(cs_Plasma, 0, 0);
            Lst_Plasma.Add(cs_Plasma);

            tbPanelPlasma.ResumeLayout();
            #region load file Config_Plasma_Boxing.xml
            try
            {
                XmlDocument xmlCF = new XmlDocument();
                xmlCF.Load("Config_Plasma_Boxing.xml");
                XmlNodeList xmlListCF = xmlCF.DocumentElement.SelectNodes("/Config");
                foreach (XmlNode xmlNode in xmlListCF)
                {
                    GlobVar.PathFileBoxing = xmlNode.SelectSingleNode("Path_Boxing").InnerText;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Error Process Load Parameter file:Config_Plasma_Boxing.xml Error \r\n " + Ex.ToString(), "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                return;
            }
            #endregion
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Show Form Data
        private void btnFormData_Click(object sender, EventArgs e)
        {
            Frm_Data newfrm = new Frm_Data();
            newfrm.ShowDialog();
        }
        #endregion

        #region Hiển thị trạng thái hoạt động của máy
        /// <summary>
        /// Hiển thị trạng thái hoạt động của máy
        /// </summary>
        private void DisplayStatusMachine(string status)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                txtStatus.Text = status;
            }));
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Click RUN
        //
        private void btnRunOrStop_Click(object sender, EventArgs e)
        {
            if (CompleteStart) return;

            c_varGolbal.IsRun = true;  //Khi start
           

            LoadTriggerPLC();
            CompleteStart = true;
            if (Status_Machine == "STOP")
            {

                #region KIỂM TRA CÁC ĐIỀU KIỆN ĐỂ START CHƯƠNG TRÌNH
                // KIỂM TRA CÁC ĐIỀU KIỆN ĐỂ START CHƯƠNG TRÌNH
                if (cboProgram.SelectedValue == null)
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Vui lòng lựa chọn Program !").ShowDialog();
                    return;
                }
                if (txt_LineID.Text == "Missing" || txt_LineID.Text == "" || txt_LineID.Text == null)
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Vui lòng điền LineID !").ShowDialog();
                    return;
                }
                if (txt_DeviceID.Text == "Missing" || txt_DeviceID.Text == "" || txt_DeviceID.Text == null)
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Vui lòng điền DeviceID !").ShowDialog();
                    return;
                }
                if (txt_LotID.Text == "Missing" || txt_LotID.Text == "" || txt_LotID.Text == null)
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Vui lòng điền LotID !").ShowDialog();
                    return;
                }
                if (txt_StaffID.Text == "Missing" || txt_StaffID.Text == "" || txt_StaffID.Text == null)
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Vui lòng điền StaffID !").ShowDialog();
                    return;
                }
                if (txt_StaffID.Text.Trim().Length != 6)
                {
                    CompleteStart = false;
                    Lib.ShowError("Hãy điền đúng 6 kí tự ở cột StaffID !");
                    return;
                }
                if (!chkLinkDB_ReadCode1.Checked && !chkLinkDB_ReadCode2.Checked && c_varGolbal.QtyBeforePlasma == 2)
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Vui lòng chọn ít nhất 1 đường dẫn file ").ShowDialog();
                    return;
                }
                // --------------------------------------------------------------------------------------------------------------------//
                #endregion

                #region KIỂM TRA KẾT NỐI SERVER NHÀ MÁY
                txtStatus.Text = "Get datas of MPN from Sever MMCV";
                if (c_varGolbal.IsUploadPlasma)
                {
                    try
                    {
                        c_varGolbal.LotID = txt_LotID.Text.Trim();
                        txt_MPN.Text = MMCV_DBGetMPN.GetMPN(c_varGolbal.LotID);
                        c_varGolbal.MPN = txt_MPN.Text.Trim();
                    }
                    catch (Exception Ex)
                    {
                        CompleteStart = false;
                        new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Error Get MPN from Sever \r {Ex}").ShowDialog();
                    }
                }
                if (txt_MPN.Text.Trim() == "Missing" || string.IsNullOrWhiteSpace(txt_MPN.Text.Trim()) || string.IsNullOrEmpty(txt_MPN.Text.Trim()))
                {
                    CompleteStart = false;
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Please input the MPN !", new Font("Segoe UI", 13f, FontStyle.Bold)).ShowDialog();
                    return;
                }
                //// --------------------------------------------------------------------------------------------------------------------//
                #endregion

                #region Kết nối Server local Boxing
                //KẾT NỐI SERVER LOCAL BOXING 
                    try
                    {
                        bool chkExist = false;
                        bool chkTimeOver = Lib.ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(5000), () => { chkExist = File.Exists(GlobVar.PathFileBoxing); });
                        if (!chkExist || !chkTimeOver)
                        {
                            CompleteStart = false;
                            Lib.ShowError($"Don't connect to Server local Boxing  {GlobVar.PathFileBoxing} ");
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        CompleteStart = false;
                        Lib.ShowError(Ex.ToString());
                        return;
                    }
                    // --------------------------------------------------------------------------------------------------------------------//
                #endregion

                #region Xác nhận đổi Lot và Backup data plasma không thuộc Lot hiện tại

                DataTable tableBoxing = Support_SQL.GetTableData($"SELECT * FROM ParameterCurrent WHERE ID=1;", GlobVar.PathFileBoxing);
                if (tableBoxing.Rows.Count > 0)
                {
                    
                    WaitWndFun frm_Wait_Upload = new WaitWndFun();
                    try
                    {
                        string LotCurrentBoxing = Lib.ToString(tableBoxing.Rows[0]["LotID"]);
                        if (!c_varGolbal.LotID.Contains(LotCurrentBoxing))
                        {
                            CompleteStart = false;
                            new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"LotID:({c_varGolbal.LotID}) hiện tại đang khác mã LotID:({LotCurrentBoxing}) ở công đoạn Boxing\r\n" +
                                                                               $"Hãy tiến hành đổi mã Lot và start tại công đoạn boxing trước").ShowDialog();
                            return;
                        }
                        DataTable dtCheck = Support_SQL.GetTableDataPlasma($"Select *From Plasma WHERE LotID NOT IN ('{c_varGolbal.LotID}')");
                        if (dtCheck.Rows.Count > 0)
                        {
                            this.Invoke(new Action(delegate
                            {
                                if (frm_Wait_Upload != null) frm_Wait_Upload.Show(this, "Backup Data");

                                string insertAll = $"INSERT INTO BackUpPlasma SELECT *FROM Plasma WHERE LotID NOT IN ('{txt_LotID.Text.Trim()}') ORDER BY ID ASC";
                                bool checkinsert = false;
                                do
                                {
                                    if (Support_SQL.ExecuteQueryDBPlasma(insertAll) == 1)
                                    {
                                        checkinsert = true;
                                    }
                                }
                                while (!checkinsert);
                                string deletePlasma = $"DELETE FROM Plasma WHERE LotID NOT IN ('{txt_LotID.Text.Trim()}')";
                                Support_SQL.ExecuteScalarDBPlasma(deletePlasma);
                                if (frm_Wait_Upload != null) frm_Wait_Upload.Close();
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(delegate
                        {
                            CompleteStart = false;
                            if (frm_Wait_Upload != null) frm_Wait_Upload.Close();
                            Lib.ShowError(ex.ToString());
                            return;
                        }));
                    }
                }
                else
                {
                    Lib.ShowError($"Kiểm tra kết nối Datalocal Boxing\r\n  {GlobVar.PathFileBoxing} ");
                    CompleteStart = false;
                    return;
                }
                

                #endregion

                #region KẾT NỐI CAM BARCODE & PLC MÁY PLASMA
                // KẾT NỐI CAM BARCODE & PLC MÁY PLASMA
                ConnectAllInPlasma();
                if (!c_varGolbal.IsRun)
                {
                    CompleteStart = false;
                    DisconnectAllDevice();
                    return; //khi mất kết nối CAM hoặc PLC thì sẽ return
                }

                // --------------------------------------------------------------------------------------------------------------------//
                #endregion

                #region KHỞI TẠO CHẠY CÁC LUỒNG ĐIỀU KHIỂN CHÍNH TRONG CHƯƠNG TRÌNH
                // KHỞI TẠO CHẠY CÁC LUỒNG ĐIỀU KHIỂN CHÍNH TRONG CHƯƠNG TRÌNH
                try
                {
                    foreach (uc_Plasma item in Lst_Plasma)
                    {
                        item.InitValue();
                        item.MainThreadDisplayDataPlasma();
                        item.MainThreadReadTagPlasma();
                        item.MainThreadUpdateDataPlasma();
                        //item.MainThreadUploadDataWaiting();
                    }
                }
                catch (Exception ex)
                {
                    CompleteStart = false;
                    DisconnectAllDevice();
                    Stop_Allthread();
                    return;
                }
                // --------------------------------------------------------------------------------------------------------------------//
                #endregion
                btnRunOrStop.Text = "STOP";
                btnRunOrStop.BackColor = Color.Red;
                btnFormData.Enabled = false;
                btnChangeUser.Enabled = false;
                cboProgram.Enabled = false;
                txt_LotID.ReadOnly = true;
                txt_StaffID.ReadOnly = true;
                if (c_varGolbal.QtyBeforePlasma == 2)
                {
                    chkLinkDB_ReadCode1.Enabled = false;
                    chkLinkDB_ReadCode2.Enabled = false;
                }
                Status_Machine = "RUN";
                DisplayStatusMachine("Running");
                QuyenSuDung();
                CompleteStart = false;
            }
            else if (Status_Machine == "RUN")
            {
                DisconnectAllDevice();
                btnRunOrStop.Text = "RUN";
                btnRunOrStop.BackColor = Color.Green;
                Status_Machine = "STOP";
                btnFormData.Enabled = true;
                btnChangeUser.Enabled = true;
                cboProgram.Enabled = true;
                txt_LotID.ReadOnly = false;
                txt_StaffID.ReadOnly = false; ;
                DisplayStatusMachine("System Stop");
                c_varGolbal.IsRun = false; //khi Stop
                if (c_varGolbal.QtyBeforePlasma == 2)
                {
                    chkLinkDB_ReadCode1.Enabled = true;
                    chkLinkDB_ReadCode2.Enabled = true;
                }
                QuyenSuDung();
                Stop_Allthread();
                CompleteStart = false;
            }
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        private void LoadTriggerPLC()
        {
            string sqlstring = "Select * from SettingPLC";
            DataTable dt = Support_SQL.GetTableData(sqlstring);
            if (dt.Rows.Count > 0)
            {
                c_varGolbal.LogicalStationNumberPlasma = Support_SQL.ToInt(dt.Rows[0]["StationNumber"]);
                c_varGolbal.TriggerResest = Support_SQL.ToString(dt.Rows[0]["TriggerReset"]);
                c_varGolbal.TriggerError = Support_SQL.ToString(dt.Rows[0]["TriggerError"]);
                c_varGolbal.TriggerOK = Support_SQL.ToString(dt.Rows[0]["TriggerOK"]);
                c_varGolbal.TriggerHaveData = Support_SQL.ToString(dt.Rows[0]["TriggerHaveData"]);
                c_varGolbal.TriggerHaveDataOK = Support_SQL.ToString(dt.Rows[0]["TriggerHaveDataOK"]);
                c_varGolbal.TriggerReadCode = Support_SQL.ToString(dt.Rows[0]["TriggerReadCode"]);
                c_varGolbal.TriggerReadCodeOK = Support_SQL.ToString(dt.Rows[0]["TriggerReadCodeOK"]);
                c_varGolbal.TriggerFinish = Support_SQL.ToString(dt.Rows[0]["TriggerFinish"]);
                c_varGolbal.TriggerFinishOK = Support_SQL.ToString(dt.Rows[0]["TriggerFinishOK"]);

            }
            else
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, "Setting PLC is Error").ShowDialog();
            }
        }

        private void ConnectAllInPlasma()
        {

            try
            {


                #region KẾT NỐI CAM BARCODE TRONG PLASMA
                //KẾT NỐI CAM BARCODE TRONG PLASMA
                foreach (uc_Plasma item in Lst_Plasma)
                {
                    if (item.IsConnectCam)
                    {
                        item.Disconnect_CamBarcodePlasma();
                    }
                    else
                    {
                        item.Connect_CamBarcodePlasma();
                    }
                    if (!item.IsConnectCam)
                    {
                        item.Disconnect_CamBarcodePlasma();
                        c_varGolbal.IsRun = false; //flag return khi không có kết nối CAM
                    }
                }
                // --------------------------------------------------------------------------------------------------------------------//
                #endregion

                #region KẾT NỐI PLC TRONG PLASMA
                //KẾT NỐI PLC TRONG PLASMA
                foreach (uc_Plasma item in Lst_Plasma)
                {
                    item.Connect_PLCPlasma();
                    if (!item.IsConnectPLC)
                    {
                        c_varGolbal.IsRun = false; //flag return khi không có kết nối PLC
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void DisconnectAllDevice()
        {
            //NGẮT KẾT NỐI CÁC THIẾT BỊ TRONG PLASMA
            foreach (uc_Plasma item in Lst_Plasma)
            {
                if (item.IsConnectCam)
                {
                    item.Disconnect_CamBarcodePlasma();
                }
                //Disconect PLC
                item.Disconnect_PLCPlasma();


                item.clearDisplay();
            }

        }

        void Stop_Allthread()
        {
            
            foreach (uc_Plasma item in Lst_Plasma)
            {
                try 
                {
                    item.Stop_ThreadAll();
                }
                catch(Exception ex)
                {

                }

            }
        }
        #region PLC
        //PLC
        /// <summary>
        /// Kết nối với PLC theo Port cho process Plasma
        /// </summary>
        private void Connect_PLCPlasma()
        {
            //PLC_Fx5Plasma.IP_Adress = c_varGolbal.IP_PLC;
            PLC_Fx3.ActLogicalStationNumber = c_varGolbal.LogicalStationNumberPlasma;
            //PLC_Fx5Plasma.Port = c_varGolbal.Port_PLC + _UcPlasmaIndex + 1;

            //if (PLC_Fx3.Open() == 0)//!PLC_Fx5Plasma.Connected
            //{
            //    try
            //    {
            //        //PLC_Fx5Plasma.Connect();
            //        PLC_Fx3.Open();
            //    }
            //    catch (Exception Ex)
            //    {
            //        new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Error Connect To PLC Process Plasma \r {Ex}").ShowDialog();
            //    }
            //}
            //else
            //{
            //    //PLC_Fx5Plasma.Disconnect();
            //    PLC_Fx3.Close();
            //    Thread.Sleep(100);
            //    //PLC_Fx5Plasma.Connect();
            //    PLC_Fx3.Open();
            //}
            //if (PLC_Fx3.Open() == 0)//!PLC_Fx5Plasma.Connected
            //{
            //    //DisconnectAllDevice();
            //    return;
            //}
            try
            {
                //PLC_Fx5Plasma.Connect();
                PLC_Fx3.Open();
            }
            catch (Exception Ex)
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Error Connect To PLC Process Plasma \r {Ex}").ShowDialog();
            }
        }
        /// <summary>
        /// Ngắt kết nối với PLC theo port cho process Plasma
        /// </summary>
        private void Disconnect_PLCPlasma()
        {
            //if (PLC_Fx3.Open() == 1)//PLC_Fx5Plasma.Connected
            // {
            //PLC_Fx5Plasma.Disconnect();
            PLC_Fx3.Close();
            //}
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Change User
        //Change User
        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            Frm_Login newfrm = new Frm_Login();
            if (newfrm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    QuyenSuDung();
                    //DisconnectAllDevice();
                    btnRunOrStop.Text = "RUN";
                    btnRunOrStop.BackColor = Color.Green;
                    DisplayStatusMachine("System Stop");
                    txt_LotID.Enabled = true;
                    txt_StaffID.Enabled = true;

                    if (btnChangeUser.Text.ToUpper() == "ADMIN")
                    {
                        LoadButton(true);
                    }
                    else if (btnChangeUser.Text.ToUpper() == "USER")
                    {
                        LoadButton(false);
                    }

                }
                catch (Exception Ex)
                {

                }
            }

        }

        void LoadButton(bool values)
        {
            if (values)
            {
                btnSettingConnect.Visible = btnModel.Visible = btnFormData.Visible = btnLanguages.Visible = values;
            }
            else
            {
                btnSettingConnect.Visible = btnModel.Visible = btnFormData.Visible = btnLanguages.Visible = values;
            }

        }

        void QuyenSuDung()
        {
            if (!c_varGolbal.IsAdmin)
            {
                try
                {
                    for (int i = 0; i < Lst_Plasma.Count; i++)
                    {
                        Lst_Plasma[i].DisableSettingButton();
                    }
                    this.Invoke(new Action(delegate
                    {
                        btnChangeUser.Text = "USER";
                        btnSetting.Enabled = false;
                        btnFormData.Visible = false;
                    }));
                }
                catch (Exception Ex)
                {

                }
            }
            else
            {
                try
                {
                    if (c_varGolbal.IsRun)
                    {
                        for (int i = 0; i < Lst_Plasma.Count; i++)
                        {
                            Lst_Plasma[i].DisableSettingButton();
                        }
                        this.Invoke(new Action(delegate
                        {
                            btnChangeUser.Text = "ADMIN";
                            btnSetting.Enabled = false;
                            btnFormData.Visible = true;
                        }));
                    }
                    else
                    {
                        for (int i = 0; i < Lst_Plasma.Count; i++)
                        {
                            Lst_Plasma[i].EnableSettingButton();
                        }
                        this.Invoke(new Action(delegate
                        {
                            btnChangeUser.Text = "ADMIN";
                            btnSetting.Enabled = true;
                            btnFormData.Visible = true;
                        }));
                    }
                }
                catch (Exception Ex)
                {

                }
            }
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Text Change
        //Change User
        private void txt_MPN_TextChanged(object sender, EventArgs e)
        {
            c_varGolbal.MPN = txt_MPN.Text.Trim();
        }

        private void txt_LotID_TextChanged(object sender, EventArgs e)
        {
            c_varGolbal.LotID = txt_LotID.Text.Trim();
        }

        private void txt_StaffID_TextChanged(object sender, EventArgs e)
        {

            c_varGolbal.StaffID = txt_StaffID.Text;
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Thread PCAlivePLC
        /// <summary>
        /// Thread gửi tín hiệu PC Alive PLC
        /// </summary>
        private void Start_AlivePLC()
        {
            Main_ThreadAlive = new Thread(new ThreadStart(AlivePLC));
            Main_ThreadAlive.IsBackground = true;
            Main_ThreadAlive.Start();

        }
        private void Stop_AlivePLC()
        {
            if (Main_ThreadAlive != null)
            {
                Main_ThreadAlive.Abort();
            }
        }

        bool flagPCAlive = false;
        private void AlivePLC()
        {
            while (c_varGolbal.IsRun) //Alive PLC
            {
                Thread.Sleep(1000);
                if (flagPCAlive == false)
                {
                    PLC_Fx3.SetDevice("M112", 1);
                    flagPCAlive = true;
                }
                else
                {
                    PLC_Fx3.SetDevice("M112", 0);
                    flagPCAlive = false;
                }
            }
        }
        #endregion

        #region Thread kill process
        private void MainThreadKillProcess()
        {
            MainKill_Process = new Thread(ProcessKill);
            MainKill_Process.IsBackground = true;
            MainKill_Process.Start();
        }

        private void ProcessKill()
        {
            try
            {
                bool checkExist = false;
                bool checkKill = false;
                while (true)
                {
                    string processName = "HxTsr"; // Tên của tiến trình (process) cần kết thúc

                    Process[] processes = Process.GetProcessesByName(processName);

                    if (processes.Length > 0)
                    {
                        try
                        {
                            checkExist = true;
                            foreach (Process process in processes)
                            {
                                process.Kill(); // Kết thúc tiến trình
                                checkKill = true;
                                //Lib.ShowInfor($"Đã kết thúc tiến trình {process.ProcessName}.");
                            }
                        }
                        catch(Exception ex)
                        {
                            checkKill = false;
                        }
                    }
                    else
                    {
                        checkExist = false;
                        //Lib.ShowInfor($"Không tìm thấy tiến trình có tên {processName}.");
                    }
                    
                    if (checkKill&&checkExist==false)
                    {
                        Thread.Sleep(new TimeSpan(1, 0, 0));
                    }
                    else
                    {
                        Thread.Sleep(new TimeSpan(0, 0, 10));
                    }
                }
            }
            catch(Exception ex)
            {

            }
            
        }
        #endregion

        private void btnSettingConnect_Click(object sender, EventArgs e)
        {
            if (btnRunOrStop.Text == "RUN" && btnChangeUser.Text.ToUpper() == "ADMIN")
            {
                frm_SettingConnect frm = new frm_SettingConnect();
                frm.ShowDialog();
            }

        }

        private void btnModel_Click(object sender, EventArgs e)
        {

            if (btnChangeUser.Text == "ADMIN" && btnRunOrStop.Text == "RUN")
            {
                Frm_New_Model frm = new Frm_New_Model();
                frm.ShowDialog();
            }

        }



        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void chkLinkDB_ReadCode1_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobVar.LockEvent || c_varGolbal.QtyBeforePlasma == 1)
                return;

            for (int i = 0; i < c_varGolbal.List_LinkDB.Count; i++)
            {
                if (c_varGolbal.List_LinkDB[i].Number == 1)
                {
                    c_varGolbal.List_LinkDB[i].SetValue(chkLinkDB_ReadCode1.Checked);
                    //Lib.ShowInfor($"Link:{c_varGolbal.List_LinkDB[i].LinkDB}\r\nNumber:{c_varGolbal.List_LinkDB[i].Number}\r\nStatus:{c_varGolbal.List_LinkDB[i].isUse}");//Test PM
                }
            }
        }

        private void chkLinkDB_ReadCode2_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobVar.LockEvent || c_varGolbal.QtyBeforePlasma == 1)
                return;
            for (int i = 0; i < c_varGolbal.List_LinkDB.Count; i++)
            {
                if (c_varGolbal.List_LinkDB[i].Number == 2)
                {
                    c_varGolbal.List_LinkDB[i].SetValue(chkLinkDB_ReadCode2.Checked);
                    //Lib.ShowInfor($"Link:{c_varGolbal.List_LinkDB[i].LinkDB}\r\nNumber:{c_varGolbal.List_LinkDB[i].Number}\r\nStatus:{c_varGolbal.List_LinkDB[i].isUse}");//Test PM
                }
            }
        }

        private void txt_LotID_DoubleClick(object sender, EventArgs e)
        {
            if (c_varGolbal.IsRun)
                return;
            GlobVar.OnKeyBoard();
            //System.Diagnostics.Process.Start("Osk.exe");
        }

        private void txt_StaffID_DoubleClick(object sender, EventArgs e)
        {
            if (c_varGolbal.IsRun)
                return;
            GlobVar.OnKeyBoard();
        }
    }
}
