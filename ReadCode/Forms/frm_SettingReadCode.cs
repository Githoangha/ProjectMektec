using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using log4net;
using MC_Protocol_NTT;
using System.Net.NetworkInformation;

namespace ReadCode
{
    public partial class frm_SettingReadCode : Form
    {
        //DataTable d1 = new DataTable();
        /// <summary>
        /// Declare Log 
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HWindow _Window;
        private HImage _Img;
        public WorkerThread2D _workerObject;
        public ManualResetEvent _stopEventHandle;
        public Thread _threadAcq, _threadLive, _threadIP;
        private HDrawingObject _drawing_object_Train;
        private HDrawingObject _drawing_object_Region;
        private HDrawingObject _drawing_object_Region_Setting_No_PCS;
        HTuple _dataCodeHandle;
        List<ObjectFile> _lstFile;
        bool _isLive = false;
        string _trainFilePath;
        bool _isNewOrEditROI = false;
        bool _isNewROI = false;
        bool _isCancel = false;
        bool _isNewTRain = false;
        public HTuple _frameGrabber;
        private int ProgramID = -1;
        private int CamIndex = 0;
        List<InterfaceCamera> _lstInterfaceCamera = new List<InterfaceCamera>();
        public MC_Protocol PLC_Fx5 = new MC_Protocol();

        public frm_SettingReadCode(int PrgID, int IndCAM)
        {
            ProgramID = PrgID;
            CamIndex = IndCAM;
            InitializeComponent();
        }

        /// <summary>
        /// Load form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_SettingReadCode_Load(object sender, EventArgs e)
        {
            // tạo đường dẫn file train
            log.Debug("Creat File train's link ");
            _trainFilePath = Application.StartupPath + "\\TrainFile\\";
            _stopEventHandle = new ManualResetEvent(false);

            var toolTip1 = new ToolTip();
            toolTip1.SetToolTip(btnSnap, "Snap");
            var toolTip2 = new ToolTip();
            toolTip1.SetToolTip(btnLive, "Live Camera");
            var toolTip3 = new ToolTip();
            toolTip1.SetToolTip(btnRun, "Run Program");
            // Load setting config program 
            log.Debug("Load name program");
            loadProgramName();
            log.Debug("Load setting Vision");
            loadCamInfo();
            log.Debug("Load setting Regions of Vision");
            loadRegions();
            log.Debug("Load setting File train of Vision");
            loadFileTrain();
            log.Debug("Load setting Regions CodeJig");
            loadRegionsCodeJig();

            // TODO: NB - 12042023
            //loadSettingNoPCS();

            Ping ping = new Ping();
            byte[] buffer = Encoding.ASCII.GetBytes("samplestring");
            int timeout = 120;
            PingReply pingresult = ping.Send(c_varGolbal.IP_PLC, timeout, buffer);
            if (pingresult.Status.ToString().ToLower() == "success")
            {
                if (PLC_Fx5 != null && PLC_Fx5.Connected)
                {
                    PLC_Fx5.Disconnect();
                }
                Thread.Sleep(500);
                PLC_Fx5.IP_Adress = c_varGolbal.IP_PLC;// c_varGolbal.IP_PLC;
                PLC_Fx5.Port = c_varGolbal.Port_PLC;//c_varGolbal.Port_PLC;
                PLC_Fx5.Connect();
            }
        }
        /// <summary>
        /// Form Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_SettingReadCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            _threadLive?.Abort();
            Thread.Sleep(100);
            _threadAcq?.Abort();
            Thread.Sleep(100);
            _threadIP?.Abort();
            Thread.Sleep(100);
            if (_frameGrabber != null) HOperatorSet.CloseFramegrabber(_frameGrabber);
            log.Debug("Abort all threads are running");
            if (PLC_Fx5.Connected || PLC_Fx5 != null)
            {
                PLC_Fx5.Disconnect();
            }
        }

        #region TAB SETTING CAMERA
        /// <summary>
        /// Tìm các giao diện camera đang kết nối
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetect_Click(object sender, EventArgs e)
        {
            cboInterface.Items.Clear();
            List<string> interfaces = getAvailableInterfaces();
            foreach (string item in interfaces)
                cboInterface.Items.Add(item);
            if (interfaces.Count > 0)
            {
                cboInterface.SelectedIndex = (interfaces.Count - 1);
            }
        }
        /// <summary>
        /// Kiểm tra thử kết nối với giao diện camera đang chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CheckConnect_Click(object sender, EventArgs e)
        {
            if (cboInterface.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a Interface!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboDevice.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a Camera!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_frameGrabber != null) _frameGrabber.Dispose();
            HTuple generic = -1;
            try
            {
                string _interfaceName = (cboInterface.SelectedItem).ToString();
                string _deviceName = (cboDevice.SelectedValue).ToString();
                string vendorPattern = @"(vendor:)(.+?)(\s\|)";
                string vendor = Regex.Match(_deviceName, vendorPattern).Groups[2].Value;   //đoạn này dùng để tùy biến cửa sổ cam param setting 
                                                                                           //string vendor = Regex.Match(cboDevice.SelectedValue.ToString(), vendorPattern).Groups[2].Value;   //đoạn này dùng để tùy biến cửa sổ cam param setting 

                //if (_interfaceName != "File")
                //if (vendor != "Basler" && vendor != "Hikvision")
                //{
                //    MessageBox.Show("This Device is not supported by your application!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                if (_interfaceName == "GigEVision2")
                {
                    //Nếu khi dải IP bị khác nhau giữa camera và máy tính thì phải lấy lại force_IP
                    HOperatorSet.InfoFramegrabber(_interfaceName, "info_boards", out HTuple Information, out HTuple BoardInformation);
                    //* check the string for "misconfigured"
                    HOperatorSet.TupleRegexpTest(BoardInformation, "misconfigured", out HTuple Misconfig);
                    if (Misconfig)
                    {
                        //*now extract suggested Force ip, copy it and set to device
                        //*get the "force ip " string
                        HOperatorSet.TupleStrstr(BoardInformation, "force_ip", out HTuple PositionStart);
                        HOperatorSet.TupleStrLastN(BoardInformation, PositionStart, out generic);
                        HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                                , "progressive"
                                , -1
                                , "default"
                                , generic //generic
                                , "default", _interfaceName == "File" ? _deviceName : "default"
                                , _interfaceName == "File" ? "default" : _deviceName
                                , 0
                                , -1
                                , out _frameGrabber);
                    }
                }
                else if (_interfaceName == "DirectShow")
                {
                    _deviceName = "[0] Integrated Webcam";
                    HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                        , "progressive"
                        , -1
                        , "default"
                        , generic //generic
                        , "default", _interfaceName == "File" ? _deviceName : "default"
                        , _interfaceName == "File" ? "default" : _deviceName
                        , 0
                        , -1
                        , out _frameGrabber);
                }
                else
                {
                    HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                        , "progressive"
                        , -1
                        , "default"
                        , generic //generic
                        , "default", _interfaceName == "File" ? _deviceName : "default"
                        , _interfaceName == "File" ? "default" : _deviceName
                        , 0
                        , -1
                        , out _frameGrabber);
                }
                HOperatorSet.CloseFramegrabber(_frameGrabber);
                MessageBox.Show("Connect Check Camera Ok!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                generic = -1;
                MessageBox.Show("Connect Check Camera Fail!" + Environment.NewLine + ex.Message);
                return;
            }
        }
        /// <summary>
        /// Lưu lại giao diện camera kết nối cho Program hiện tại
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCam_Click(object sender, EventArgs e)
        {
            if (cboInterface.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a Interface!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboDevice.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a Camera!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }

            string sql = $"select ID_Program from CameraSetting " +
                $"where ID_Program = {ProgramID} " +
                $"and CamIndex = {CamIndex} " +
                $"Limit 1";
            string saveSql = "";
            // Kiểm tra xem ID Program này đã có setting hay chưa nếu có thì update chưa có thì insert
            if (Convert.ToInt32(Support_SQL.ExecuteScalar(sql)) > 0)//Update
            {
                saveSql = $"update CameraSetting set InterfaceName = '{cboInterface.SelectedItem}', DeviceName = '{cboDevice.SelectedValue}'" +
                            $" where ID_Program = '{ProgramID}' " +
                            $" and CamIndex = '{CamIndex}'";
            }
            else//Insert
            {
                saveSql = $"Insert into CameraSetting(ID_Program,CamIndex,InterfaceName,DeviceName) " +
                    $" values('{ProgramID}','{CamIndex}','{cboInterface.SelectedItem}','{cboDevice.SelectedValue}')";
            }

            Support_SQL.ExecuteQuery(saveSql);
            txtCurrentInterface.Text = (cboInterface.SelectedItem).ToString(); ;
            txtCurrentCamera.Text = (cboDevice.SelectedValue).ToString();
        }
        /// <summary>
        /// Kết nối với giao diện camera để sử dụng các chứ năng khác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCurrentConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentInterface.Text))
            {
                MessageBox.Show("Please choose a Interface!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtCurrentCamera.Text))
            {
                MessageBox.Show("Please choose a Camera!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }
            if (_frameGrabber != null) _frameGrabber.Dispose();

            if (btnCurrentConnect.Text.ToLower() == "Connect".ToLower())
            {
                HTuple generic = -1;
                try
                {
                    string ImgType;
                    string _interfaceName = txtCurrentInterface.Text.Trim().ToString();
                    string _deviceName = txtCurrentCamera.Text.Trim().ToString();
                    //Update mới do sử dụng phiên bản Halcon 20.11.2 và Camera Hikvision by Hoang 21/05/2022
                    string[] deviceTemp = _deviceName.Split('|');
                    try
                    {
                        _deviceName = deviceTemp[2];
                        deviceTemp = _deviceName.Split(':');
                        _deviceName = deviceTemp[1];
                    }
                    catch (Exception)
                    {
                        deviceTemp = _deviceName.Split(':');
                        _deviceName = deviceTemp[1];
                    }

                    string vendorPattern = @"(vendor:)(.+?)(\s\|)";
                    string vendor = Regex.Match(txtCurrentCamera.Text.Trim().ToString(), vendorPattern).Groups[2].Value;   //đoạn này dùng để tùy biến cửa sổ cam param setting 
                    if (_interfaceName == "GigEVision2")
                    {
                        //Nếu khi dải IP bị khác nhau giữa camera và máy tính thì phải lấy lại force_IP
                        HOperatorSet.InfoFramegrabber(_interfaceName, "info_boards", out HTuple Information, out HTuple BoardInformation);
                        //* check the string for "misconfigured"
                        HOperatorSet.TupleRegexpTest(BoardInformation, "misconfigured", out HTuple Misconfig);
                        if (Misconfig)
                        {
                            //*now extract suggested Force ip, copy it and set to device
                            //*get the "force ip " string
                            HOperatorSet.TupleStrstr(BoardInformation, "force_ip", out HTuple PositionStart);
                            HOperatorSet.TupleStrLastN(BoardInformation, PositionStart, out generic);
                            HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                               , "progressive"
                               , -1
                               , "default"
                               , generic //generic
                               , "default", _interfaceName == "File" ? _deviceName : "default"
                               , _interfaceName == "File" ? "default" : _deviceName
                               , 0
                               , -1
                               , out _frameGrabber);
                        }
                        else
                        {
                            HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                               , "progressive"
                               , -1
                               , "default"
                               , -1 //generic
                               , "default", "default"
                               , _deviceName.Trim()
                               , 0
                               , -1
                               , out _frameGrabber);

                        }
                    }
                    else if (_interfaceName == "DirectShow")
                    {
                        _deviceName = "[0] Integrated Webcam";
                        HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                            , "progressive"
                            , -1
                            , "default"
                            , generic //generic
                            , "default", "default"
                            , _deviceName
                            , 0
                            , -1
                            , out _frameGrabber);
                    }
                    else
                    {
                        HOperatorSet.OpenFramegrabber(_interfaceName, 0, 0, 0, 0, 0, 0
                            , "progressive"
                            , -1
                            , "default"
                            , generic //generic
                            , "default", _interfaceName == "File" ? _deviceName : "default"
                            , _interfaceName == "File" ? "default" : _deviceName
                            , 0
                            , -1
                            , out _frameGrabber);
                    }

                    // if interface # "File" and DirectShow => Check mode trigger Camera , Load parameter cam
                    if (_interfaceName != "File" && _interfaceName != "DirectShow")
                    {

                        HOperatorSet.GetFramegrabberParam(_frameGrabber, "TriggerMode", out HTuple TriggerMode);
                        HOperatorSet.GetFramegrabberParam(_frameGrabber, "TriggerSource", out HTuple TriggerSource);
                        if (TriggerMode.S.ToLower() == "off")
                        {
                            HOperatorSet.GrabImage(out HObject hObject, _frameGrabber);
                            _Img = new HImage(hObject);
                            _Img.GetImagePointer1(out ImgType, out int ImgWidth, out int ImgHeight);
                            _Window.SetPart(0, 0, ImgHeight - 1, ImgWidth - 1);
                            _Img?.Dispose();
                            hObject.Dispose();
                            btnSnap.Enabled = true;
                            btnLive.Enabled = true;
                        }
                        if (TriggerMode.S.ToLower() == "on")
                        {
                            btnSnap.Enabled = false;
                            btnLive.Enabled = false;
                            MessageBox.Show("Camera trigger mode using by extend!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        //load parmeter camera add cbx
                        try
                        {
                            HOperatorSet.GetFramegrabberParam(_frameGrabber, "available_param_names", out HTuple allParamName);
                            List<string> lstAll = new List<string>(allParamName.SArr);
                            cboParametter.DataSource = lstAll;
                            pl_SetPrameter.Enabled = true;
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                    _Window.SetDraw("margin");
                    _Window.SetLineWidth(1);
                    // Change status control
                    btnCurrentConnect.Text = "DISCONNECT";
                    btnRun.Enabled = btnLive.Enabled = btnSnap.Enabled = true;
                }
                catch (Exception ex)
                {
                    generic = -1;
                    MessageBox.Show($"Error Connect Camera ! \r {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // Disconnect
            else
            {
                btnCurrentConnect.Text = "CONNECT";
                btnRun.Enabled = btnLive.Enabled = btnSnap.Enabled = false;
                pl_SetPrameter.Enabled = false;
                if (_isLive)
                {
                    btnLive_Click(null, null);
                }
                if (_threadAcq != null) _threadAcq.Abort();
                if (_threadIP != null) _threadIP.Abort();
                try
                {
                    _Img?.Dispose();
                    _Window.DetachBackgroundFromWindow();
                }
                catch
                {
                }
                HOperatorSet.CloseFramegrabber(_frameGrabber);
                _frameGrabber.Dispose();
                _frameGrabber = null;
            }
        }
        /// <summary>
        /// Ghi setting vào trong camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveParam_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(_frameGrabber, (cboParametter.SelectedValue).ToString(), new HTuple(((int)txtValue.Value)));
                // HOperatorSet.SetFramegrabberParam(_frameGrabber, "ExposureTime", 150000);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        ///  Cbx chọn đến parameter nào set load thông số của parameter đó hiển thị trên texbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboParametter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valueParamName = (cboParametter.SelectedValue).ToString();
            if (string.IsNullOrWhiteSpace(valueParamName)) return;
            HOperatorSet.GetFramegrabberParam(_frameGrabber, valueParamName + "_access", out HTuple valueAccess);
            try
            {
                HOperatorSet.GetFramegrabberParam(_frameGrabber, valueParamName + "_range", out HTuple valueRange);
                txtValue.Visible = true;
                txtValue.Minimum = (decimal)valueRange[0].D;
                txtValue.Maximum = (decimal)valueRange[1].D;
                txtValue.Value = (decimal)valueRange[3].D;
                if (valueAccess.S.ToLower() == "r")
                {
                    txtValue.Enabled = false;
                }
            }
            catch
            {
                txtValue.Visible = false;
            }

            try
            {
                HOperatorSet.GetFramegrabberParam(_frameGrabber, valueParamName + "_values", out HTuple values);
                cboValue.Visible = true;
                HOperatorSet.GetFramegrabberParam(_frameGrabber, valueParamName, out HTuple paramValue);
                cboValue.DataSource = values.SArr;
                cboValue.SelectedItem = paramValue.S;
                if (valueAccess.S.ToLower() == "r")
                {
                    cboValue.Enabled = false;
                }
            }
            catch
            {
                cboValue.Visible = false;
            }
        }

        private void cboInterface_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string interfaceName = (cboInterface.SelectedItem).ToString();
            List<InterfaceCamera> lst = _lstInterfaceCamera.Where(o => o.NameInterface == interfaceName).ToList();
            cboDevice.DataSource = lst;
            cboDevice.DisplayMember = "NameDevice";
            cboDevice.ValueMember = "NameDevice";
        }

        #endregion

        #region TAB SETTING REGIONS
        /// <summary>
        /// Button New - Tạo ra một vùng roi trên Hwindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            // NB - 31032023
            int number = 0;
            string sql = $"SELECT Position,X1,Y1,X2,Y2 FROM RegionPosition WHERE ID_Program = {ProgramID} AND CamIndex = {CamIndex} ORDER BY ID DESC";
            DataTable dtRegionPosition = Support_SQL.GetTableData(sql);
            if (dtRegionPosition.Rows.Count > 0)
            {
                //if (dtRegionPosition.Rows.Count >= 2)
                //{
                //    number = Support_SQL.ToInt(dtRegionPosition.Rows[0]["Y1"]);
                //}
                //else
                //    number = 0;
                txtPosition.Value = Support_SQL.ToDecimal(dtRegionPosition.Rows[0]["Position"]) + 1;
                decimal X1 = txtX1.Value = Support_SQL.ToDecimal(dtRegionPosition.Rows[0]["X1"]);
                decimal Y1 = txtY1.Value = Support_SQL.ToDecimal(dtRegionPosition.Rows[0]["Y1"]);
                decimal X2 = txtX2.Value = Support_SQL.ToDecimal(dtRegionPosition.Rows[0]["X2"]);
                decimal Y2 = txtY2.Value = Support_SQL.ToDecimal(dtRegionPosition.Rows[0]["Y2"]);
                creatModelRegion(Support_SQL.ToInt(X1), Support_SQL.ToInt(Y1), Support_SQL.ToInt(X2), Support_SQL.ToInt(Y2));
            }
            else
            {
                txtPosition.Value = 1;
                creatModelRegion(100, 100, 500, 500);
            }
            _isCancel = true;
            _isNewROI = true;
            _isNewOrEditROI = true;
            set_BtnMsRegions(true);
        }
        /// <summary>
        /// Tạo roi Cho 1 vùng roi cho CodeJig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewCodeJig_Click(object sender, EventArgs e)
        {
            creatModelRegionCodeJig(100, 100, 500, 500);
            _isNewROI = true;
            _isNewOrEditROI = true;
            _isCancel = true;
            set_BtnMsRegionsCodeJig(true);
        }
        /// <summary>
        /// Button Edit - lấy tọa độ vị trí cần edit từ dgv vẽ ra vẽ ra vùng roi tại đúng vị trí đó
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grvRegionPosition.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please choose a position in list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int x1 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colX1"].Value);
            int y1 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colY1"].Value);
            int x2 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colX2"].Value);
            int y2 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colY2"].Value);
            int p = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colPosition"].Value);
            txtPosition.Value = p;
            txtX1.Value = x1;
            txtY1.Value = y1;
            txtX2.Value = x2;
            txtY2.Value = y2;
            txtQtyCode.Value = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colQtyCode"].Value);
            creatModelRegion(x1, y1, x2, y2);
            _isNewOrEditROI = false;
            _isNewROI = true;
            _isCancel = true;
            set_BtnMsRegions(true);
            txtPosition.Enabled = false;
        }
        /// <summary>
        /// Edit tọa độ vị trí của CodeJig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCodeJig_Click(object sender, EventArgs e)
        {
            if (grvPositionCodeJig.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please choose a position in list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int x1 = Convert.ToInt32(grvPositionCodeJig.SelectedRows[0].Cells["colX1_CodeJig"].Value);
            int y1 = Convert.ToInt32(grvPositionCodeJig.SelectedRows[0].Cells["colY1_CodeJig"].Value);
            int x2 = Convert.ToInt32(grvPositionCodeJig.SelectedRows[0].Cells["colX2_CodeJig"].Value);
            int y2 = Convert.ToInt32(grvPositionCodeJig.SelectedRows[0].Cells["colY2_CodeJig"].Value);
            txtPosition.Value = 0;
            txtPosition.BackColor = Color.Red;
            txtX1.Value = x1;
            txtY1.Value = y1;
            txtX2.Value = x2;
            txtY2.Value = y2;
            creatModelRegionCodeJig(x1, y1, x2, y2);
            _isNewOrEditROI = false;
            _isNewROI = true;
            set_BtnMsRegionsCodeJig(true);
            txtPosition.Enabled = false;
        }

        /// <summary>
        /// Button Delete - Xóa bỏ vùng vị trí đang chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grvRegionPosition.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please choose a position in list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pos = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells[colPosition.Name].Value);
            Support_SQL.ExecuteQuery(string.Format(@"delete FROM RegionPosition where Position = {0} and ID_Program = {1} and CamIndex ={2}",
                pos, ProgramID, CamIndex));

            loadRegions();
        }
        /// <summary>
        /// Xóa bỏ vùng vị trí CodeJig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCodeJig_Click(object sender, EventArgs e)
        {
            if (grvPositionCodeJig.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please choose a position CodeJig .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pos = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells[colPosition.Name].Value);
            Support_SQL.ExecuteQuery(string.Format($"delete FROM RegionPositionCodeJig where Position='CodeJig' and ID_Program = {ProgramID} and CamIndex ={CamIndex}"));
            loadRegionsCodeJig();
        }

        /// <summary>
        /// Button Save - insert hoặc update các vị trí vừa thao tác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPosition.Value == 0)
                {
                    MessageBox.Show("Please type a Position.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtQtyCode.Value == 0)
                {
                    MessageBox.Show("Please type a Qty Code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtX1.Value == 0 && txtX2.Value == 0 && txtY1.Value == 0 && txtY2.Value == 0)
                {
                    MessageBox.Show("Please choose a Region.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Tạo mới tọa độ
                string sql = string.Empty;
                if (_isNewOrEditROI)
                {
                    //check exist
                    DataTable dt = Support_SQL.GetTableData(string.Format("SELECT * from RegionPosition WHERE Position = {0} AND ID_Program = {1} AND CamIndex ={2}",
                        txtPosition.Value,
                        ProgramID,
                        CamIndex
                        ));
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("This position is exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // TODO: NB - 25032023
                    sql = string.Format(@"insert into RegionPosition(Position,X1,Y1,X2,Y2,ID_Program,QtyCode,CamIndex) 
                                                    Values({0},{1},{2},{3},{4},{5},{6},{7})",
                        txtPosition.Value,
                        txtX1.Value,
                        txtY1.Value,
                        txtX2.Value,
                        txtY2.Value,
                        ProgramID,
                        txtQtyCode.Value,
                        CamIndex
                    );
                }
                // sửa mới tọa độ
                else
                {
                    sql = string.Format(@"update RegionPosition set X1 = {0}, Y1 = {1}, X2 = {2}, Y2 = {3}, Position={4}, QtyCode = {5} where ID_Program = {6} AND CamIndex = {7} AND Position={8}",
                        txtX1.Value,
                        txtY1.Value,
                        txtX2.Value,
                        txtY2.Value,
                        txtPosition.Value,
                        txtQtyCode.Value,
                        ProgramID,
                        CamIndex,
                        txtPosition.Value
                    );
                }
                Support_SQL.ExecuteQuery(sql);

                if (_drawing_object_Region != null)
                {
                    _Window.DetachDrawingObjectFromWindow(_drawing_object_Region);
                    _drawing_object_Region.Dispose();
                    _drawing_object_Region = null;
                }
                loadRegions();

                // NB - 27032023
                grvRegionPosition.ClearSelection();
                int post = 0;
                if (txtPosition.Value < grvRegionPosition.RowCount)
                    post = Support_SQL.ToInt(txtPosition.Value);
                else
                    post = Support_SQL.ToInt(txtPosition.Value) - 1;
                grvRegionPosition.Rows[post].Selected = true;
                grvRegionPosition.Rows[post].Cells[0].Selected = true;

                _isNewOrEditROI = false;
                _isNewROI = false;
                _isCancel = false;
                set_BtnMsRegions(false);
                txtPosition.Value = 0;
                txtQtyCode.Value = 1;
                txtPosition.Enabled = true;

            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Save _Insert or Update Posision CodeJig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCodeJig_Click(object sender, EventArgs e)
        {
            if (txtX1.Value == 0 && txtX2.Value == 0 && txtY1.Value == 0 && txtY2.Value == 0)
            {
                MessageBox.Show("Please choose a Region.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Tạo mới tọa độ
            if (_isNewOrEditROI)
            {
                //check exist
                DataTable dt = Support_SQL.GetTableData(string.Format("SELECT * from RegionPositionCodeJig WHERE Position ='CodeJig' AND ID_Program = {0} AND CamIndex ={1}",
                    ProgramID,
                    CamIndex
                    ));
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("This position is exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Support_SQL.ExecuteQuery(string.Format(@"insert into RegionPositionCodeJig(Position,QtyCode,X1,Y1,X2,Y2,ID_Program,CamIndex) 
                                                    Values('CodeJig',1,{0},{1},{2},{3},{4},{5})",
                    txtX1.Value,
                    txtY1.Value,
                    txtX2.Value,
                    txtY2.Value,
                    ProgramID,
                    CamIndex

                ));
            }
            // sửa mới tọa độ
            else
            {

                Support_SQL.ExecuteQuery(string.Format(@"update RegionPositionCodeJig set X1 = {0},Y1 = {1},X2 = {2},Y2 = {3}
                                                    where  ID_Program = {4} AND CamIndex = {5} AND Position='CodeJig'",
                    txtX1.Value,
                    txtY1.Value,
                    txtX2.Value,
                    txtY2.Value,
                    ProgramID,
                    CamIndex
                ));
            }

            if (_drawing_object_Region != null)
            {
                _Window.DetachDrawingObjectFromWindow(_drawing_object_Region);
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }

            loadRegionsCodeJig();
            _isNewOrEditROI = false;
            _isNewROI = false;
            set_BtnMsRegionsCodeJig(false);
            txtPosition.Value = 0;
            txtQtyCode.Value = 1;
            txtPosition.Enabled = true;
        }

        /// <summary>
        /// Button Cancel - Loại bỏ các thao tác vừa làm trở về ban đầu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_drawing_object_Region != null)
            {
                _Window.DetachDrawingObjectFromWindow(_drawing_object_Region);
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }
            txtPosition.Enabled = true;
            _isNewROI = false;
            _isCancel = false;
            set_BtnMsRegions(false);
            txtPosition.Value = 0;
            txtQtyCode.Value = 1;
        }
        /// <summary>
        /// Button Cancel của CodeJig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelCodeJig_Click(object sender, EventArgs e)
        {
            if (_drawing_object_Region != null)
            {
                _Window.DetachDrawingObjectFromWindow(_drawing_object_Region);
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }
            txtPosition.Enabled = true;
            _isNewROI = false;
            set_BtnMsRegionsCodeJig(false);
            txtPosition.BackColor = Color.WhiteSmoke;
            txtPosition.Value = 0;
            txtQtyCode.Value = 1;
        }
        /// <summary>
        /// thay đổi nhanh trạng thái các nút trên menu strip
        /// </summary>
        /// <param name="isEdit"></param>
        private void set_BtnMsRegions(bool isEdit)
        {
            btnSave.Visible = isEdit;
            btnCancel.Visible = isEdit;
            btnNew.Visible = !isEdit;
            btnEdit.Visible = !isEdit;
            btnDelete.Visible = !isEdit;
        }
        private void set_BtnMsRegionsCodeJig(bool isEdit)
        {
            btnSaveCodeJig.Visible = isEdit;
            btnCancelCodeJig.Visible = isEdit;
            btnNewCodeJig.Visible = !isEdit;
            btnEditCodeJig.Visible = !isEdit;
            btnDeleteCodeJig.Visible = !isEdit;
        }
        /// <summary>
        ///  Even SelectedRows đưa data khi chọn tại dgv vào các ô text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grvRegionPosition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x1 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colX1"].Value);
            int y1 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colY1"].Value);
            int x2 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colX2"].Value);
            int y2 = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colY2"].Value);
            int p = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colPosition"].Value);
            txtPosition.Value = p;
            txtX1.Value = x1;
            txtY1.Value = y1;
            txtX2.Value = x2;
            txtY2.Value = y2;
            txtQtyCode.Value = Convert.ToInt32(grvRegionPosition.SelectedRows[0].Cells["colQtyCode"].Value);
            if (_drawing_object_Region != null)
            {
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }
            _drawing_object_Region = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, x1, y1, x2, y2);
            _drawing_object_Region.SetDrawingObjectParams("color", "red");
            _drawing_object_Region.OnDrag(GetPosition);
            _drawing_object_Region.OnResize(GetPosition);
            _Window.AttachDrawingObjectToWindow(_drawing_object_Region);


            // TODO: NB - 11042023
            string mediumGray = Convert.ToString(grvRegionPosition.SelectedRows[0].Cells["colMediumGray"].Value);
            string ratioHavePCS = Convert.ToString(grvRegionPosition.SelectedRows[0].Cells["colRatioHavePCS"].Value);
            string ratioNonePCS = Convert.ToString(grvRegionPosition.SelectedRows[0].Cells["colRatioNonePCS"].Value);
            string realRatioPCS = Convert.ToString(grvRegionPosition.SelectedRows[0].Cells["colRealRatioPCS"].Value);
            InvokeTextBox(txtMaxGrayPCS, mediumGray);
            InvokeTextBox(txtRatioHavePCS, ratioHavePCS);
            InvokeTextBox(txtRatioNonePCS, ratioNonePCS);
            InvokeTextBox(txtRealRatioPCS, realRatioPCS);
        }

        // TODO: NB - 11042023
        private void InvokeTextBox(TextBox txt, string data)
        {
            if (txt.InvokeRequired)
                txt.Invoke(new Action(() => txt.Text = data));
            else
                txt.Text = data;
        }
        #endregion

        #region TAB TRAINS CODE
        /// <summary>
        /// Button New - Tạo ra một vùng Roi Train
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewTrain_Click(object sender, EventArgs e)
        {
            if (_drawing_object_Train != null)
            {
                _drawing_object_Train.Dispose();
                _drawing_object_Train = null;
            }

            _drawing_object_Train = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                100, 100, 500, 500);
            _drawing_object_Train.SetDrawingObjectParams("color", "magenta");
            _Window.AttachDrawingObjectToWindow(_drawing_object_Train);
            string sqlCommand = $"SELECT STT FROM TrainFile WHERE ID_Program = {ProgramID} AND CamIndex = {CamIndex} ORDER BY ID DESC";
            txtSTT.Value = Support_SQL.ToDecimal(Support_SQL.ExecuteScalar(sqlCommand));
            txtFileName.Text = "";
            _isNewTRain = true;
            set_BtnMsTrain(true);
        }
        /// <summary>
        /// Button Delete - Xóa File train trong thư mục và tên file trên hệ thống
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteTrain_Click(object sender, EventArgs e)
        {
            if (grvTrainFile.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please choose a Train File.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }

            string fileName = (grvTrainFile.SelectedRows[0].Cells[1].Value).ToString();
            string stt = (grvTrainFile.SelectedRows[0].Cells[0].Value).ToString();

            DialogResult result = MessageBox.Show("Are you sure delete this train file [STT = " + stt + "]?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            string sql = string.Format("delete from TrainFile where STT = {0} and FileName = '{1}' and ID_Program = {2} and CamIndex = {3}"
                    , stt
                    , fileName
                    , ProgramID
                    , CamIndex
                );
            if (Support_SQL.ExecuteQuery(sql) == 1)
            {
                try
                {
                    File.Delete(_trainFilePath + fileName);
                }
                catch
                {
                }

                loadFileTrain();
            }
            else
            {
                MessageBox.Show("Error delete Train.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }
        /// <summary>
        /// Button Train - Train code có trong vùng Roi vừa được chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTrain_Click(object sender, EventArgs e)
        {
            if (_drawing_object_Train == null)
            {
                MessageBox.Show("Please create a model train.");
                return;
            }
            _Window.SetColor("green");
            _Window.SetDraw("margin");
            _Window.SetLineWidth(3);
            HRegion region = new HRegion(_drawing_object_Train.GetDrawingObjectIconic());

            HImage ImgReduced = _Img.ReduceDomain(region);

            HOperatorSet.CreateDataCode2dModel(c_varGolbal.CodeType, null, null, out _dataCodeHandle);
            //TODO: VINH
            HOperatorSet.SetDataCode2dParam(_dataCodeHandle, "contrast_tolerance", "high");
            HOperatorSet.SetDataCode2dParam(_dataCodeHandle, "candidate_selection", "extensive");
            HOperatorSet.SetDataCode2dParam(_dataCodeHandle, "module_grid", "any");
            ///
            HOperatorSet.FindDataCode2d(ImgReduced, out HObject symbolXLDs, _dataCodeHandle
                , new HTuple(new string[] { "train", "stop_after_result_num" })
                , new HTuple(new object[] { "all", 1 })
                , out HTuple resultHandes
                , out HTuple decodDataStrings);

            HOperatorSet.RegionFeatures(region, new HTuple(new string[] { "row1", "column1", "row2", "column2" }), out HTuple values);
            _Window.DispText((decodDataStrings.Length > 0 ? decodDataStrings.ToString() : c_varGolbal.NoReadString), "image", values[2].D + 5, values[1].D, new HTuple(), new HTuple(), new HTuple());
            _Window.DispObj(symbolXLDs);

            if (_drawing_object_Train != null)
            {
                _Window.DetachDrawingObjectFromWindow(_drawing_object_Train);
                _drawing_object_Train.Dispose();
                _drawing_object_Train = null;
            }
        }
        /// <summary>
        ///  Button Save - Lưu lại file train và tên file lên hệ thống
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTrain_Click(object sender, EventArgs e)
        {
            if (ProgramID < 0)
            {
                MessageBox.Show("Please choose a Program.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtSTT.Value == 0)
            {
                MessageBox.Show("Please type STT.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtFileName.Text.Trim() == "")
            {
                MessageBox.Show("Please type FileName.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_dataCodeHandle == null)
            {
                MessageBox.Show("Please train a model code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }
            int stt = 0;

            if (_lstFile.Count == 0)
            {
                stt = 1;
            }
            else
            {
                stt = _lstFile.Max(o => o.STT) + 1;
            }

            string filePath = string.Format("{0}{1}_{2}.dcm", _trainFilePath, ProgramID, txtFileName.Text.Trim());

            if (File.Exists(filePath))
            {
                MessageBox.Show("Train File is exist.\nPlease create a new file name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sqlCommand = string.Format("insert into TrainFile(STT,ID_Program,FileName,CamIndex) values({0},{1},'{2}',{3})"
                    , stt
                    , ProgramID
                    , Path.GetFileName(filePath)
                    , CamIndex
                );
            if (Support_SQL.ExecuteQuery(sqlCommand) == 1)
            {
                HOperatorSet.WriteDataCode2dModel(_dataCodeHandle, filePath);
                _dataCodeHandle = null;
            }
            else
            {
                MessageBox.Show("Error create Train File!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }

            loadFileTrain();
            _isNewTRain = false;
            set_BtnMsTrain(false);
            txtSTT.Value = 0;
            txtFileName.Text = "";
        }
        /// <summary>
        /// Button Cancel - Loại bỏ các thao tác vừa làm trở về ban đầu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelTrain_Click(object sender, EventArgs e)
        {
            if (_drawing_object_Train != null)
            {
                _Window.DetachDrawingObjectFromWindow(_drawing_object_Train);
                _drawing_object_Train.Dispose();
                _drawing_object_Train = null;
            }
            set_BtnMsTrain(false);
            _isNewTRain = false;
            txtSTT.Value = 0;
            txtFileName.Text = "";
        }
        /// <summary>
        /// thay đổi nhanh trạng thái các nút trên menu strip
        /// </summary>
        /// <param name="isEdit"></param>
        private void set_BtnMsTrain(bool isEdit)
        {
            btnSaveTrain.Visible = isEdit;
            btnCancelTrain.Visible = isEdit;
            btnTrain.Visible = isEdit;
            btnAddTrain.Visible = !isEdit;
            btnDeleteTrain.Visible = !isEdit;
        }


        #endregion

        #region SUPPORT
        /// <summary>
        /// Lấy thông tin Setting camera từ database
        /// </summary>
        void loadCamInfo()
        {
            try
            {
                string sql = $"select * from CameraSetting " +
                    $"where ID_Program = {ProgramID} " +
                    $"and CamIndex = {CamIndex} " +
                    $"Limit 1";
                DataTable dt = Support_SQL.GetTableData(sql);
                if (dt.Rows.Count > 0)
                {
                    txtCurrentInterface.Text = dt.Rows[0]["InterfaceName"].ToString(); ;
                    txtCurrentCamera.Text = dt.Rows[0]["DeviceName"].ToString();
                }
                else
                {
                    txtCurrentInterface.Text = "";
                    txtCurrentCamera.Text = "";
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Tải dữ liệu Name Program từ DB vision khi đã có dự liệu Program ID và CamIndex
        /// </summary>
        void loadProgramName()
        {
            try
            {
                DataTable dt = Support_SQL.GetTableData($"SELECT ProgramName from ProgramMain WHERE ID_Program = '{ProgramID}'");
                txtProgram.Text = dt.Rows[0][0].ToString();
                txtCamIndex.Text = "CAM" + CamIndex.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool _isCodeJig = false;
        DataTable dtDataRegion = new DataTable();
        /// <summary>
        /// Load các tọa độ vùng ROI
        /// </summary>
        void loadRegions()
        {
            dtDataRegion = Support_SQL.GetTableData($"select * from RegionPosition where ID_Program = {ProgramID}  and CamIndex={CamIndex} Order by Position");
            grvRegionPosition.DataSource = null;
            grvRegionPosition.AutoGenerateColumns = false;
            grvRegionPosition.DataSource = dtDataRegion;
        }
        void loadRegionsCodeJig()
        {
            DataTable dtDataRegionCodeJig = Support_SQL.GetTableData($"select * from RegionPositionCodeJig where ID_Program = {ProgramID} and CamIndex={CamIndex}");
            grvPositionCodeJig.DataSource = null;
            grvPositionCodeJig.AutoGenerateColumns = false;
            grvPositionCodeJig.DataSource = dtDataRegionCodeJig;
        }

        /// <summary>
        /// Load các tọa độ vùng ROI
        /// </summary>
        void loadSettingNoPCS()
        {
            DataTable dtSettingNoPCS = Support_SQL.GetTableData($"SELECT * FROM SettingNoPCS WHERE ID_Program = {ProgramID}  and CamIndex={CamIndex}");
            if (dtSettingNoPCS.Rows.Count > 0)
            {
                txtMaxGrayPCS.Text = Support_SQL.ToString(dtSettingNoPCS.Rows[0]["MaxGrayPCS"]);
                txtRatioHavePCS.Text = Support_SQL.ToString(dtSettingNoPCS.Rows[0]["RatioHavePCS"]);
                txtRatioNonePCS.Text = Support_SQL.ToString(dtSettingNoPCS.Rows[0]["RatioNonePCS"]);
                txtRealRatioPCS.Text = Support_SQL.ToString(dtSettingNoPCS.Rows[0]["RealRatioPCS"]);
            }
        }

        /// <summary>
        /// Tải các thông tin file train
        /// </summary>
        void loadFileTrain()
        {
            DataTable dt = Support_SQL.GetTableData("select * from TrainFile where ID_Program = " + ProgramID + " and CamIndex = " + CamIndex + " Order by STT");

            _lstFile = new List<ObjectFile>();
            _lstFile.Clear();
            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                ObjectFile file = new ObjectFile();
                file.STT = Convert.ToInt32(dt.Rows[i]["STT"]);
                file.Program = ProgramID;
                file.FileName = dt.Rows[i]["FileName"].ToString();
                file.FilePath = _trainFilePath + file.FileName;

                _lstFile.Add(file);
            }
            grvTrainFile.DataSource = null;
            grvTrainFile.AutoGenerateColumns = false;
            grvTrainFile.DataSource = _lstFile;
        }

        #endregion

        #region Config Halcon Window
        public HWindow GetHalconWindow()
        {
            return WindowControl.HalconWindow;
        }
        public void ResetControls()
        {
            btnLive.Enabled = true;
            _Window.ClearWindow();
        }
        private List<string> getAvailableInterfaces()
        {
            _lstInterfaceCamera = new List<InterfaceCamera>();

            // Detect the HALCON binary folder
            List<string> availableInterfaces = new List<string>();
            string halconRoot = Environment.GetEnvironmentVariable("HALCONROOT");
            string halconArch = Environment.GetEnvironmentVariable("HALCONARCH");

            string a = halconRoot + "/bin/" + halconArch;

            // Querry all available interfaces
            var acquisitionInterfaces = Directory.EnumerateFiles(a, "hacq*.dll");

            // For each Interface (check for non XL version) we test with InfoFramegrabber if devices are connected
            foreach (string item in acquisitionInterfaces)
            {
                //HOperatorSet.CountSeconds(out HTuple StartTime);
                // Extract the interface name with an regular expression
                string interfaceName = Regex.Match(item, "hAcq(.+)(?:\\.dll)").Groups[1].Value;
                if (interfaceName == "DirectFile") continue;
                if (interfaceName == "File")
                {
                    InterfaceCamera interfaceCamera = new InterfaceCamera();
                    interfaceCamera.NameInterface = interfaceName;
                    interfaceCamera.NameDevice = Application.StartupPath + "\\Images_Test";
                    _lstInterfaceCamera.Add(interfaceCamera);
                    availableInterfaces.Add(interfaceName);
                    continue;
                }
                try
                {
                    // Querry available devices
                    HTuple devices;
                    HInfo.InfoFramegrabber(interfaceName, "info_boards", out devices);
                    // In case that devices were found add it to the available interfaces
                    if (devices.Length > 0)
                    {
                        foreach (var itemDevice in devices.SArr)
                        {
                            InterfaceCamera interfaceCamera = new InterfaceCamera();
                            interfaceCamera.NameInterface = interfaceName;
                            interfaceCamera.NameDevice = itemDevice;
                            _lstInterfaceCamera.Add(interfaceCamera);
                        }
                        availableInterfaces.Add(interfaceName);
                    }
                }
                catch (Exception)
                { }

            }

            return availableInterfaces;
        }
        private void WindowControl_Load(object sender, EventArgs e)
        {
            _Window = WindowControl.HalconWindow;
            this.MouseWheel += my_MouseWheel;
        }
        private void my_MouseWheel(object sender, MouseEventArgs e)
        {
            Point pt = WindowControl.Location;
            MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
            WindowControl.HSmartWindowControl_MouseWheel(sender, newe);
        }
        private void creatModelRegion(int x1, int y1, int x2, int y2)
        {
            if (_drawing_object_Region != null)
            {
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }

            _drawing_object_Region = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, x1, y1, x2, y2); //Color.Yellow
            _drawing_object_Region.SetDrawingObjectParams("color", "green");//blue   
            _drawing_object_Region.OnDrag(GetPosition);
            _drawing_object_Region.OnResize(GetPosition);
            _Window.AttachDrawingObjectToWindow(_drawing_object_Region);
        }
        private void creatModelRegionCodeJig(int x1, int y1, int x2, int y2)
        {
            if (_drawing_object_Region != null)
            {
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }

            _drawing_object_Region = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, x1, y1, x2, y2);
            _drawing_object_Region.SetDrawingObjectParams("color", "red");
            _drawing_object_Region.OnDrag(GetPosition);
            _drawing_object_Region.OnResize(GetPosition);
            _Window.AttachDrawingObjectToWindow(_drawing_object_Region);
        }

        private void GetPosition(HDrawingObject dobj, HWindow hwin, string type)
        {
            HRegion region = new HRegion(dobj.GetDrawingObjectIconic());
            HOperatorSet.RegionFeatures(region, new HTuple(new string[] { "row1", "column1", "row2", "column2" }), out HTuple values);
            double[] arrPosition = values.ToDArr();
            txtX1.Value = Convert.ToDecimal(arrPosition[0]);
            txtY1.Value = Convert.ToDecimal(arrPosition[1]);
            txtX2.Value = Convert.ToDecimal(arrPosition[2]);
            txtY2.Value = Convert.ToDecimal(arrPosition[3]);
        }
        #endregion

        #region Main Form
        /// <summary>
        /// Nút nhấn chụp ảnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSnap_Click(object sender, EventArgs e)
        {
            //HA Test
            if (PLC_Fx5.Connected)
            {
                PLC_Fx5.GetSingleBit("M20", out bool value_Lamp_On);
                if (!value_Lamp_On)
                    PLC_Fx5.SetSingleBit("M20", true);
            }
            // END

            if (_frameGrabber == null) return;
            if (_isLive)
            {
                btnLive_Click(null, null);
                Thread.Sleep(100);
            }
            _Img?.Dispose();
            HOperatorSet.GrabImage(out HObject image, _frameGrabber);
            _Img = new HImage(image);
            _Img.DispObj(_Window);
            if (_drawing_object_Region != null)
            {
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
                WindowControl.HalconWindow.ClearWindow();
                WindowControl.HalconWindow.DispObj(_Img);
            }
            image.Dispose();

            //HA Test
            if (PLC_Fx5.Connected)
            {
                PLC_Fx5.GetSingleBit("M20", out bool value_Lamp_On);
                if (value_Lamp_On)
                    PLC_Fx5.SetSingleBit("M20", false);
            }
            // END
        }

        HTuple _list_Result_decode_text_NG;
        HTuple _list_Result_decode_row_NG;
        HTuple _list_Result_decode_column_NG;
        HTuple _list_Result_decode_text_Miss;
        HTuple _list_Result_decode_row_Miss;
        HTuple _list_Result_decode_column_Miss;
        ucViewImage ImageResult;
        /// <summary>
        /// Chạy test chụp anh đọc code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            //HA Test
            // NB - 26032023
            if (PLC_Fx5.Connected)
            {
                PLC_Fx5.GetSingleBit("M20", out bool value_Lamp_On);
                if (!value_Lamp_On)
                    PLC_Fx5.SetSingleBit("M20", true);
            }
            //END

            //NG
            _list_Result_decode_text_NG = new HTuple();
            _list_Result_decode_row_NG = new HTuple();
            _list_Result_decode_column_NG = new HTuple();
            //Miss
            _list_Result_decode_text_Miss = new HTuple();
            _list_Result_decode_row_Miss = new HTuple();
            _list_Result_decode_column_Miss = new HTuple();
            if (_frameGrabber == null)
            {
                MessageBox.Show("Please connect to a camera.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Kiểm tra file train
            if (_lstFile.Count == 0)
            {
                MessageBox.Show("Please create train file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Nếu đang ở chế độ Live thì phải tắt đi rồi ms cho phép chạy Run trigger
            if (_isLive)
            {
                btnLive_Click(null, null);
                Thread.Sleep(500);
            }
            _Window.SetColor("green");
            _Window.SetDraw("margin");
            _Window.SetLineWidth(3);
            // tải các vị trí đọc code
            DataTable dtRegion = Support_SQL.GetTableData(string.Format(@"select * from RegionPosition where ID_Program = {0} AND CamIndex ={1} ", ProgramID, CamIndex));
            try
            {
                List<string> lstResult = new List<string>();
                int countRegions = dtRegion.Rows.Count;
                if (countRegions <= 0)
                {
                    return;
                }
                _Img?.Dispose();
                //Chụp ảnh
                HOperatorSet.GrabImage(out HObject image, _frameGrabber);
                _Img = new HImage(image);
                image.Dispose();
                _Img.DispImage(_Window); // hiển thị ảnh lên cửa sổ Hsmart

                // TODO NB - 01042023
                // Đưa ảnh vào 1 HSWINDOW khác để Lưu ảnh
                ImageResult = new ucViewImage();
                ImageResult.ViewImage(_Img);
                ReadBarCode(ProgramID, CamIndex);
                //Duyệt danh sách vùng chọn thiết lập để cắt ảnh
                for (int i = 0; i < countRegions; i++)
                {
                    DataRow item = dtRegion.Rows[i];
                    //Lấy ra vùng chọn rồi cắt ảnh theo vùng chọn
                    double X1, X2, Y1, Y2;
                    X1 = Convert.ToInt32(item["X1"]);
                    X2 = Convert.ToInt32(item["X2"]);
                    Y1 = Convert.ToInt32(item["Y1"]);
                    Y2 = Convert.ToInt32(item["Y2"]);
                    int numCode = Convert.ToInt32(item["QtyCode"]);
                    // TODO: 11042023
                    double _mediumGray = Convert.ToDouble(item["MediumGray"]);
                    double _realRatioPCS = Convert.ToDouble(item["RealRatioPCS"]);

                    HRegion region = new HRegion(X1, Y1, X2, Y2);
                    HImage ImgReduced = _Img.ReduceDomain(region);
                    region.Dispose();
                    //Duyệt qua danh sách các file train đến khi file train nào trả ra kết quả thì thôi, nếu ko thì trả ra noread
                    string itemText = string.Empty;
                    HTuple DecodedDataStrings = new HTuple();
                    for (int j = 0; j < _lstFile.Count; j++)
                    {
                        ObjectFile file = _lstFile[j];
                        HOperatorSet.ReadDataCode2dModel(file.FilePath, out HTuple dataCodeHandle);
                        HOperatorSet.SetDataCode2dParam(dataCodeHandle, "contrast_tolerance", "high");
                        HOperatorSet.SetDataCode2dParam(dataCodeHandle, "candidate_selection", "extensive");
                        HOperatorSet.SetDataCode2dParam(dataCodeHandle, "module_grid", "any");
                        HOperatorSet.FindDataCode2d(ImgReduced, out HObject SymbolXLDs, dataCodeHandle, "stop_after_result_num"
                            , numCode, out HTuple ResultHandles, out DecodedDataStrings);
                        if (DecodedDataStrings.Length > 0)
                        {
                            _Window.SetColor("green");
                            _Window.DispObj(SymbolXLDs);
                            itemText = item["Position"] + " - " + DecodedDataStrings.ToString();
                        }
                        SymbolXLDs.Dispose();
                        DecodedDataStrings.Dispose();
                        dataCodeHandle.Dispose();
                        ResultHandles.Dispose();
                        //TODO: NB - 23032023
                        if (DecodedDataStrings.Length > 0)
                            break;
                        else
                            continue;
                    }
                    // NB - 03042023 -> CHỈNH SỬA LẠI KIỂM TRA NOPCS, NOREAD
                    if (DecodedDataStrings.Length == 0)
                    {
                        string _color = string.Empty;
                        DataTable dtSettingNoPCS = Support_SQL.GetTableData($"SELECT * FROM SettingNoPCS WHERE ID_Program = {ProgramID} AND CamIndex ={CamIndex}");
                        // trả ra diện tích vùng đen của pcs theo roi
                        // TODO: 11042023
                        HOperatorSet.Threshold(ImgReduced, out HObject RegionP, 0, _mediumGray); // nếu đèn sáng hơn thì tăng 50 lên. -> pixel đen
                        HOperatorSet.ClosingRectangle1(RegionP, out RegionP, 9, 9);
                        HOperatorSet.Connection(RegionP, out RegionP);
                        HOperatorSet.SelectShapeStd(RegionP, out RegionP, "max_area", 60);
                        HOperatorSet.AreaCenter(RegionP, out HTuple AreaP, out HTuple RowP, out HTuple ColumnP);
                        // đọc ra vùng vẽ roi -> trả ra diện tích full roi
                        HOperatorSet.Threshold(ImgReduced, out HObject RegionFull, 0, 255);
                        HOperatorSet.AreaCenter(RegionFull, out HTuple AreaFull, out HTuple RowFull, out HTuple ColumnFull); // tổng roi
                        double ratio = Math.Round(AreaP.D / AreaFull.D, 6) * 100;
                        // TODO: 11042023
                        if (ratio < _realRatioPCS)
                        {
                            // NO PCS
                            _color = "magenta";
                            _Window.SetColor(_color);
                            itemText = item["Position"] + " - " + c_varGolbal.Missing + $"\r\n[{ratio}%] < [{_realRatioPCS}%]";
                            HOperatorSet.TupleConcat(_list_Result_decode_text_Miss, (HTuple)itemText, out _list_Result_decode_text_Miss);
                            HOperatorSet.TupleConcat(_list_Result_decode_row_Miss, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_Miss);
                            HOperatorSet.TupleConcat(_list_Result_decode_column_Miss, (HTuple)(Y1 - 100 + (Y2 - Y1) / 2), out _list_Result_decode_column_Miss);
                            // NB - 01042023
                            ImageResult.ViewImage(_Img);
                            ImageResult.ViewResult(itemText, (X1 + (X2 - X1) / 2), (Y1 - 100 + (Y2 - Y1) / 2), new HTuple("magenta"));
                            ImageResult.SaveImage(c_varGolbal.CodeJig, c_varGolbal.Missing.Replace(" ", ""),CamIndex);
                        }
                        else
                        {
                            // NO READ -> sử dụng thêm 1 filter để đọc code
                            HOperatorSet.ScaleImage(ImgReduced, out HObject ImageScaled, 2.38318, -253);
                            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(), out HTuple dataCodeHandle1);
                            HOperatorSet.FindDataCode2d(ImageScaled, out HObject SymbolXLDs1, dataCodeHandle1, new HTuple("stop_after_result_num")
                            , new HTuple(numCode), out HTuple ResultHandles1, out HTuple DecodedDataStrings_Train);
                            if (DecodedDataStrings_Train.Length > 0)
                            {
                                _color = "green";
                                _Window.SetColor("green");
                                _Window.DispObj(SymbolXLDs1);
                                itemText = item["Position"] + " - " + DecodedDataStrings_Train.ToString();
                            }
                            SymbolXLDs1.Dispose();
                            DecodedDataStrings_Train.Dispose();
                            dataCodeHandle1.Dispose();
                            ResultHandles1.Dispose();
                            if (DecodedDataStrings_Train.Length == 0)
                            {
                                _color = "red";
                                _Window.SetColor(_color);
                                itemText = item["Position"] + " - " + c_varGolbal.NoReadString + $"\r\n[{ratio}%] > [{_realRatioPCS}%]";
                                HOperatorSet.TupleConcat(_list_Result_decode_text_NG, (HTuple)itemText, out _list_Result_decode_text_NG);
                                HOperatorSet.TupleConcat(_list_Result_decode_row_NG, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_NG);
                                HOperatorSet.TupleConcat(_list_Result_decode_column_NG, (HTuple)(Y1 - 100 + (Y2 - Y1) / 2), out _list_Result_decode_column_NG);
                                // NB - 01042023
                                ImageResult.ViewImage(_Img);
                                ImageResult.ViewResult(itemText, (X1 + (X2 - X1) / 2), (Y1 - 100 + (Y2 - Y1) / 2), new HTuple("red"));
                                ImageResult.SaveImage(c_varGolbal.CodeJig, c_varGolbal.NoReadString, CamIndex);
                            }
                        }
                        if (_color == "magenta" || _color == "red")
                        {
                            HDrawingObject regin_show = new HDrawingObject();
                            HOperatorSet.GenRectangle1(out HObject Rect, X1, Y1, X2, Y2);
                            HOperatorSet.SetColor(_Window, _color);
                            Rect.DispObj(_Window);
                        }
                    }

                    #region CHUYỂN CÁCH XỬ LÝ BÊN TRÊN
                    //TODO: NB - 26032023
                    //if (DecodedDataStrings.Length <= 0)
                    //{
                    //    HOperatorSet.ScaleImage(ImgReduced, out HObject ImageScaled, 2.38318, -253);
                    //    HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(), out HTuple dataCodeHandle1);
                    //    HOperatorSet.FindDataCode2d(ImageScaled, out HObject SymbolXLDs1, dataCodeHandle1, new HTuple("train", "stop_after_result_num")
                    //    , new HTuple("all", numCode), out HTuple ResultHandles1, out HTuple DecodedDataStrings_Train);
                    //    if (DecodedDataStrings_Train.Length > 0)
                    //    {
                    //        _Window.SetColor("green");
                    //        _Window.DispObj(SymbolXLDs1);
                    //        itemText = item["Position"] + " - " + DecodedDataStrings_Train.ToString();
                    //    }
                    //    SymbolXLDs1.Dispose();
                    //    DecodedDataStrings_Train.Dispose();
                    //    dataCodeHandle1.Dispose();
                    //    ResultHandles1.Dispose();
                    //    if (DecodedDataStrings_Train.Length > 0)
                    //        continue;
                    //}
                    //if (string.IsNullOrEmpty(itemText))
                    //{
                    //string _color = string.Empty;
                    //// TODO: NB - 26032023
                    //DataTable dtSettingNoPCS = Support_SQL.GetTableData($"SELECT * FROM SettingNoPCS WHERE ID_Program = {ProgramID} AND CamIndex ={CamIndex}");
                    //if (dtSettingNoPCS.Rows.Count == 0)
                    //{
                    //    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"XIN VUI LÒNG KIỂM TRA SETTINGS NO PCS\r\n[ID_Program = {ProgramID}, CamIndex ={CamIndex}]", 3000).ShowDialog();
                    //    break;
                    //}
                    //// trả ra diện tích vùng đen của pcs theo roi
                    //HOperatorSet.Threshold(ImgReduced, out HObject RegionP, 0, Convert.ToDouble(dtSettingNoPCS.Rows[0]["MaxGrayPCS"])); // nếu đèn sáng hơn thì tăng 50 lên. -> pixel đen
                    //HOperatorSet.ClosingRectangle1(RegionP, out RegionP, 9, 9);
                    //HOperatorSet.Connection(RegionP, out RegionP);
                    //HOperatorSet.SelectShapeStd(RegionP, out RegionP, "max_area", 60);
                    //HOperatorSet.AreaCenter(RegionP, out HTuple AreaP, out HTuple RowP, out HTuple ColumnP);
                    //// đọc ra vùng vẽ roi -> trả ra diện tích full roi
                    //HOperatorSet.Threshold(ImgReduced, out HObject RegionFull, 0, 255);
                    //HOperatorSet.AreaCenter(RegionFull, out HTuple AreaFull, out HTuple RowFull, out HTuple ColumnFull); // tổng roi
                    //// NB - 28032023
                    //double ratio = Math.Round(AreaP.D / AreaFull.D, 6) * 100;
                    //double _realRatioPCS = Convert.ToDouble(dtSettingNoPCS.Rows[0]["RealRatioPCS"]);
                    //if (ratio > _realRatioPCS)
                    //{
                    //    _color = "red";
                    //    _Window.SetColor(_color);
                    //    itemText = item["Position"] + " - " + c_varGolbal.NoReadString + $"\r\n[{ratio}%] > [{_realRatioPCS}%]"; // "NoRead";
                    //    HOperatorSet.TupleConcat(_list_Result_decode_text_NG, (HTuple)itemText, out _list_Result_decode_text_NG);
                    //    HOperatorSet.TupleConcat(_list_Result_decode_row_NG, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_NG);//X2+2
                    //    HOperatorSet.TupleConcat(_list_Result_decode_column_NG, (HTuple)(Y1 + (Y2 - Y1) / 2), out _list_Result_decode_column_NG);
                    //    // NB - 01042023
                    //    ImageResult.ViewImage(_Img);
                    //    ImageResult.ViewResult(itemText, (X1 + (X2 - X1) / 2), (Y1 + (Y2 - Y1) / 2), new HTuple("red"));
                    //    ImageResult.SaveImage(c_varGolbal.CodeJig, c_varGolbal.NoReadString);
                    //}
                    //else
                    //{
                    //    _color = "magenta";
                    //    _Window.SetColor(_color);
                    //    itemText = item["Position"] + " - " + c_varGolbal.Missing + $"\r\n[{ratio}%] < [{_realRatioPCS}%]"; ; // "Nopcs";
                    //    HOperatorSet.TupleConcat(_list_Result_decode_text_Miss, (HTuple)itemText, out _list_Result_decode_text_Miss);
                    //    HOperatorSet.TupleConcat(_list_Result_decode_row_Miss, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_Miss);
                    //    HOperatorSet.TupleConcat(_list_Result_decode_column_Miss, (HTuple)(Y1 + (Y2 - Y1) / 2), out _list_Result_decode_column_Miss);
                    //    // NB - 01042023
                    //    ImageResult.ViewImage(_Img);
                    //    ImageResult.ViewResult(itemText, (X1 + (X2 - X1) / 2), (Y1 + (Y2 - Y1) / 2), new HTuple("magenta"));
                    //    ImageResult.SaveImage(c_varGolbal.CodeJig, c_varGolbal.Missing.Replace(" ", ""));
                    //}
                    //HDrawingObject regin_show = new HDrawingObject();
                    //HOperatorSet.GenRectangle1(out HObject Rect, X1, Y1, X2, Y2);
                    //HOperatorSet.SetColor(_Window, _color);
                    //Rect.DispObj(_Window);
                    //}
                    #endregion

                    if (_list_Result_decode_text_NG.Length > 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _Window.DispText(_list_Result_decode_text_NG, "image", _list_Result_decode_row_NG, _list_Result_decode_column_NG, new HTuple("red"), new HTuple(), new HTuple());
                        }));
                    }
                    if (_list_Result_decode_text_Miss.Length > 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _Window.DispText(_list_Result_decode_text_Miss, "image", _list_Result_decode_row_Miss, _list_Result_decode_column_Miss, new HTuple("magenta"), new HTuple(), new HTuple());
                        }));
                    }
                    ImgReduced.Dispose();
                    ImageResult?.Dispose();
                    ImageResult = new ucViewImage();
                }
                if (PLC_Fx5.Connected)
                {
                    PLC_Fx5.GetSingleBit("M20", out bool value_Lamp_Off);
                    if (value_Lamp_Off)
                        PLC_Fx5.SetSingleBit("M20", false);
                }
            }
            catch (Exception ex)
            {
                // log file Error
                string fileName = string.Format("Error_{0}_{1}_{2}.txt", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                File.AppendAllText(Application.StartupPath + "/" + fileName,
                    DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ": " + ex.ToString() + Environment.NewLine);
            }
        }

        /// <summary>
        /// Lay vi tri Barcode Jig va Decode
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="CamIndex"></param>
        private void ReadBarCode(int ProgramID, int CamIndex)
        {
            //c_varGolbal.CodeJig = string.Empty;
            //tai vi tri doc Barcode Jig
            DataTable dtRegionBarCode = Support_SQL.GetTableData(string.Format(@"SELECT * from RegionPositionCodeJig Where ID_Program = {0} AND CamIndex ={1} ", ProgramID, CamIndex));
            try
            {
                List<string> lstResult = new List<string>();
                int countRegions = dtRegionBarCode.Rows.Count;
                if (countRegions <= 0)
                    return;
                for (int i = 0; i < countRegions; i++)
                {
                    DataRow item = dtRegionBarCode.Rows[i];
                    //Lấy ra vùng chọn rồi cắt ảnh theo vùng chọn
                    double X1, X2, Y1, Y2;
                    X1 = Convert.ToInt32(item["X1"]);
                    X2 = Convert.ToInt32(item["X2"]);
                    Y1 = Convert.ToInt32(item["Y1"]);
                    Y2 = Convert.ToInt32(item["Y2"]);
                    int numCode = Convert.ToInt32(item["QtyCode"]);
                    HRegion region = new HRegion(X1, Y1, X2, Y2);
                    HImage ImgReduced = _Img.ReduceDomain(region);
                    region.Dispose();
                    //Duyệt qua danh sách các file train đến khi file train nào trả ra kết quả thì thôi, nếu ko thì trả ra noread
                    string itemText = "";
                    for (int j = 0; j < _lstFile.Count; j++)
                    {
                        ObjectFile file = _lstFile[j];

                        HOperatorSet.ReadDataCode2dModel(file.FilePath, out HTuple dataCodeHandle);
                        HOperatorSet.FindDataCode2d(ImgReduced, out HObject SymbolXLDs, dataCodeHandle, "stop_after_result_num"
                            , numCode, out HTuple ResultHandles, out HTuple DecodedDataStrings);

                        if (DecodedDataStrings.Length > 0)
                        {
                            _Window.DispObj(SymbolXLDs);
                            // NB - 01042023
                            c_varGolbal.CodeJig = DecodedDataStrings.O.ToString();
                            itemText = item["Position"] + " - " + DecodedDataStrings.O;
                            SymbolXLDs.Dispose();
                            DecodedDataStrings.Dispose();
                            dataCodeHandle.Dispose();
                            ResultHandles.Dispose();
                            break;
                        }
                        else
                        {
                            SymbolXLDs.Dispose();
                            DecodedDataStrings.Dispose();
                            dataCodeHandle.Dispose();
                            ResultHandles.Dispose();
                            continue;
                        }
                    }
                    if (string.IsNullOrEmpty(itemText))
                    {
                        itemText = item["Position"] + " - " + c_varGolbal.NoReadString;// "NoRead";
                    }
                    // Show dữ liệu code lên windown
                    _Window.DispText(itemText, "image", X2 + 5, Y1, new HTuple(), new HTuple(), new HTuple());
                    ImgReduced.Dispose();
                }
            }
            catch (Exception ex)
            {
                string fileName = string.Format("Error_{0}_{1}_{2}.txt", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                File.AppendAllText(Application.StartupPath + "/" + fileName,
                    DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ": " + ex.ToString() + Environment.NewLine);
                throw;
            }
        }

        /// <summary>
        ///  Nút nhấn live
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLive_Click(object sender, EventArgs e)
        {
            if (_frameGrabber == null) return;
            if (!_isLive)
            {
                //panelTrainningSetting.Enabled = panelPosition.Enabled = btnSaveConfig.Enabled = btnSnap1.Enabled = cboProgram.Enabled = btnRun.Enabled = false;
                tabControlMain.Enabled = false;
                _workerObject = new WorkerThread2D(this);
                _stopEventHandle.Reset();

                _threadAcq = new Thread(new ThreadStart(_workerObject.ImgAcqRun));
                _threadIP = new Thread(new ThreadStart(_workerObject.IPRun));
                _workerObject.Init();
                _threadAcq.Start();
                _threadIP.Start();
                btnLive.BackColor = Color.Red;
                _isLive = true;
            }
            else
            {
                _stopEventHandle.Set();
                btnLive.BackColor = Color.FromArgb(255, 192, 128);
                _isLive = false;
                tabControlMain.Enabled = true;
                //panelTrainningSetting.Enabled = panelPosition.Enabled = btnSaveConfig.Enabled = btnSnap1.Enabled = cboProgram.Enabled = btnRun.Enabled = true;
            }
        }
        #endregion

        /// <summary>
        /// Lock tap control is mode Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_isNewROI)
            {
                tabControlMain.SelectTab("tb_SetRegions");
            }
            else if (_isNewTRain)
            {
                tabControlMain.SelectTab("tb_TrainsCode");
            }
        }
        private void BtnLightOn_Click(object sender, EventArgs e)
        {
            PLC_Fx5.SetSingleBit("M20", true);
        }

        private void BtnLightOff_Click(object sender, EventArgs e)
        {
            PLC_Fx5.SetSingleBit("M20", false);
        }
        /// <summary>
        ///  HuyNV 27-02-2023
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //HuyNV 27-02-2023
            if (_isCancel) return;

            if (_drawing_object_Region != null)
            {
                _Window.DetachDrawingObjectFromWindow(_drawing_object_Region);
                _drawing_object_Region.Dispose();
                _drawing_object_Region = null;
            }
            if (!_isNewROI) _isNewROI = false;
            if (!_isNewOrEditROI) _isNewOrEditROI = false;
        }

        #region SETTINGS NOPCS
        /// <summary>
        /// lấy pixcel màu đen trung bình
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRatioHaveOrNonePCS_Click(object sender, EventArgs e)
        {
            // TODO: NB - 10042023
            GetRatioHaveOrNonePCS(txtRatioNonePCS, false);
            btnGetRealRatioPCS.Enabled = true;
            btnGetRealRatioPCS.Focus();
            btnRatioNonePCS.Enabled = false;
        }

        double RatioPCS;
        HDrawingObject hDrawingObject;
        HTuple grayValue = new HTuple(0);
        HTuple mingrayValue = new HTuple(0);
        HTuple mingrayValue_Old = new HTuple(0);
        HTuple maxgrayValue = new HTuple(0);
        HTuple maxgrayValue_Old = new HTuple(0);
        HTuple rangegrayValue = new HTuple(0);
        // TODO: NB - 11042023
        List<double> lstMediumGray = new List<double>();
        /// <summary>
        /// train all theo pixcel đen và roi full đã train ở phần đọc qr
        /// -> lấy ra giá trị trung bình 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTrainAll_SettingNoPCS_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: NB - 11042023
                lstMediumGray.Clear();
                lstRatioHavePCS.Clear();
                lstRatioNonePCS.Clear();
                // TODO: NB - 31032023
                maxgrayValue_Old.D = 0;
                mingrayValue_Old.D = 0;
                int countRegions = dtDataRegion.Rows.Count;
                for (int i = 0; i < countRegions; i++)
                {
                    double X1, X2, Y1, Y2;
                    DataRow item = dtDataRegion.Rows[i];
                    int ID = Convert.ToInt16(item["ID"]);
                    X1 = Convert.ToInt32(item["X1"]);
                    X2 = Convert.ToInt32(item["X2"]);
                    Y1 = Convert.ToInt32(item["Y1"]);
                    Y2 = Convert.ToInt32(item["Y2"]);
                    if (hDrawingObject != null)
                    {
                        hDrawingObject.Dispose();
                        hDrawingObject = null;
                    }
                    hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, X1, Y1, X2, Y2);
                    HObject regionSetting = new HRegion();
                    regionSetting = hDrawingObject.GetDrawingObjectIconic();
                    HOperatorSet.ReduceDomain(_Img, regionSetting, out HObject ImgRdc);
                    HOperatorSet.BinaryThreshold(ImgRdc, out HObject Region, "max_separability", "dark", out HTuple __);
                    HOperatorSet.ReduceDomain(ImgRdc, Region, out HObject imageReduced);
                    HOperatorSet.MinMaxGray(Region, imageReduced, 0, out mingrayValue, out maxgrayValue, out rangegrayValue);

                    // TODO: NB - 11042023
                    if (maxgrayValue.D > 0)
                    {
                        double mediumGray = (double)(maxgrayValue.D + mingrayValue.D) / 2;
                        lstMediumGray.Add(mediumGray);
                    }
                }
                GetRatioHaveOrNonePCS(txtRatioHavePCS, true);
                btnRatioNonePCS.Enabled = true;
                btnRatioNonePCS.Focus();
                btnTrainAll_SettingNoPCS.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }

        HTuple _list_Result_Text;
        HTuple lst_X1_Setting_No_PCS;
        HTuple lst_Y1_Setting_No_PCS;
        HObject Roi_No_PCS = new HObject();
        HObject Regions_No_PCS = new HObject();
        // TODO: NB - 11042023
        List<double> lstRatioHavePCS = new List<double>();
        List<double> lstRatioNonePCS = new List<double>();
        /// <summary>
        /// hàm xử lý lấy pixcel đen theo full roi
        /// </summary>
        /// <param name="txt"></param>1
        private void GetRatioHaveOrNonePCS(TextBox txt, bool type)
        {
            // TODO: NB - 31032023
            _Img?.Dispose();
            //HA Test
            if (PLC_Fx5.Connected)
            {
                PLC_Fx5.GetSingleBit("M20", out bool value_Lamp_On);
                if (!value_Lamp_On)
                    PLC_Fx5.SetSingleBit("M20", true);
            }
            //END

            if (_frameGrabber == null) return;
            if (_isLive)
            {
                btnLive_Click(null, null);
                Thread.Sleep(100);
            }
            HOperatorSet.GrabImage(out HObject image, _frameGrabber);
            _Img = new HImage(image);
            _Img.DispObj(_Window);
            image.Dispose();

            //Ha Test
            if (PLC_Fx5.Connected)
            {
                PLC_Fx5.GetSingleBit("M20", out bool value_Lamp_On);
                if (value_Lamp_On)
                    PLC_Fx5.SetSingleBit("M20", false);
            }
            //END 

            RatioPCS = 0;
            HOperatorSet.GenEmptyObj(out Roi_No_PCS);
            HOperatorSet.GenEmptyObj(out Regions_No_PCS);
            lst_X1_Setting_No_PCS = new HTuple();
            _list_Result_Text = new HTuple();
            lst_Y1_Setting_No_PCS = new HTuple();
            _Window.SetColor("green");
            _Window.SetDraw("margin");
            _Window.SetLineWidth(2);
            for (int i = 0; i < dtDataRegion.Rows.Count; i++)
            {
                DataRow item = dtDataRegion.Rows[i];
                //Lấy ra vùng chọn rồi cắt ảnh theo vùng chọn
                double X1, X2, Y1, Y2, mediumGray;
                int ID = Convert.ToInt16(item["ID"]);
                X1 = Convert.ToInt32(item["X1"]);
                X2 = Convert.ToInt32(item["X2"]);
                Y1 = Convert.ToInt32(item["Y1"]);
                Y2 = Convert.ToInt32(item["Y2"]);
                int numCode = Convert.ToInt32(item["QtyCode"]);
                HRegion region = new HRegion(X1, Y1, X2, Y2);
                HOperatorSet.ConcatObj(Roi_No_PCS, region, out Roi_No_PCS);
                HImage ImgReduced = _Img.ReduceDomain(region);
                // TODO: NB - 11042023
                // mediumGray = Convert.ToInt32(item["MediumGray"]);
                mediumGray = Convert.ToDouble(lstMediumGray[i]);
                HOperatorSet.Threshold(ImgReduced, out HObject RegionP, 0, mediumGray); // nếu đèn sáng hơn thì tăng 50 lên. -> pixel đen
                HOperatorSet.ClosingRectangle1(RegionP, out RegionP, 9, 9);
                HOperatorSet.Connection(RegionP, out RegionP);
                HOperatorSet.SelectShapeStd(RegionP, out RegionP, "max_area", 60);
                HOperatorSet.ConcatObj(Regions_No_PCS, RegionP, out Regions_No_PCS);
                HOperatorSet.AreaCenter(RegionP, out HTuple AreaP, out HTuple RowP, out HTuple ColumnP);
                // đọc ra vùng vẽ roi -> trả ra diện tích full roi
                HOperatorSet.Threshold(ImgReduced, out HObject RegionFull, 0, 255);
                HOperatorSet.AreaCenter(RegionFull, out HTuple AreaFull, out HTuple RowFull, out HTuple ColumnFull); // tổng roi
                // NB - 10042023
                double ratioItem = Math.Round(AreaP.D / AreaFull.D, 6) * 100;
                string text = Support_SQL.ToString(ratioItem + "%");
                HOperatorSet.TupleConcat(_list_Result_Text, (HTuple)text, out _list_Result_Text);
                HOperatorSet.TupleConcat(lst_X1_Setting_No_PCS, (HTuple)(X1 + (X2 - X1) / 2), out lst_X1_Setting_No_PCS);
                HOperatorSet.TupleConcat(lst_Y1_Setting_No_PCS, (HTuple)(Y1 + (Y2 - Y1) / 2), out lst_Y1_Setting_No_PCS);
                // NB - 11042023
                if (type) // có pcs -> lấy giá trị min
                    lstRatioHavePCS.Add(ratioItem);
                else // không có pcs -> lấy giá trị max
                    lstRatioNonePCS.Add(ratioItem);
            }
            WindowControl.HalconWindow.ClearWindow();
            WindowControl.HalconWindow.DispObj(_Img);
            WindowControl.HalconWindow.SetColor("green");
            WindowControl.HalconWindow.SetDraw("fill");
            WindowControl.HalconWindow.DispObj(Regions_No_PCS);
            WindowControl.HalconWindow.SetColor("red");
            WindowControl.HalconWindow.SetDraw("margin");
            WindowControl.HalconWindow.DispObj(Roi_No_PCS);
            WindowControl.HalconWindow.DispText(_list_Result_Text, "image", lst_X1_Setting_No_PCS, lst_Y1_Setting_No_PCS, new HTuple("black"), new HTuple(), new HTuple());
        }

        /// <summary>
        /// lưu lại thông tin trong bảng settings nopcs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSettingsNoPCS_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dtDataRegion.Rows.Count; i++)
            {
                DataRow item = dtDataRegion.Rows[i];
                int ID = Convert.ToInt16(item["ID"]);
                string sql = $"UPDATE RegionPosition SET MediumGray = {lstMediumGray[i]}, RatioHavePCS = {lstRatioHavePCS[i]}, RatioNonePCS = {lstRatioNonePCS[i]}, RealRatioPCS = {lstRealRatioPCS[i]} WHERE ID = {ID}";
                Support_SQL.ExecuteQuery(sql);
            }
            if (_drawing_object_Region_Setting_No_PCS != null)
            {
                _drawing_object_Region_Setting_No_PCS.Dispose();
                _drawing_object_Region_Setting_No_PCS = null;
                WindowControl.HalconWindow.ClearWindow();
                WindowControl.HalconWindow.DispObj(_Img);
            }
            loadRegions();
            grvRegionPosition_CellClick(null, null);
            btnSave_SettingNoPCS.Enabled = btnRatioNonePCS.Enabled = btnGetRealRatioPCS.Enabled = false;
            btnTrainAll_SettingNoPCS.Enabled = true;
            btnTrainAll_SettingNoPCS.Focus();
        }

        List<decimal> lstRealRatioPCS = new List<decimal>();
        /// <summary>
        /// hàm lấy ra giá trị trung bình thực tế sau khi đã trừ đi phần chênh lệch offset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRealRatioPCS_Click(object sender, EventArgs e)
        {
            // TODO: NB - 11042023
            for (int i = 0; i < dtDataRegion.Rows.Count; i++)
            {
                DataRow item = dtDataRegion.Rows[i];
                int ID = Convert.ToInt16(item["ID"]);
                decimal ratioHavePCS = Convert.ToDecimal(lstRatioHavePCS[i]);
                decimal ratioNonePCS = Convert.ToDecimal(lstRatioNonePCS[i]);
                if (ratioHavePCS > 0)
                {
                    decimal realRatioPCS = (ratioHavePCS - ratioNonePCS) / 2;
                    lstRealRatioPCS.Add(realRatioPCS);
                }
            }
            btnSave_SettingNoPCS.Enabled = true;
            btnSave_SettingNoPCS.Focus();
            btnGetRealRatioPCS.Enabled = false;
        }

        private void btnCancel_SettingNoPCS_Click(object sender, EventArgs e)
        {
            if (_drawing_object_Region_Setting_No_PCS != null)
            {
                _drawing_object_Region_Setting_No_PCS.Dispose();
                _drawing_object_Region_Setting_No_PCS = null;
            }
            WindowControl.HalconWindow.ClearWindow();
            WindowControl.HalconWindow.DispObj(_Img);
            btnSave_SettingNoPCS.Enabled = btnGetRealRatioPCS.Enabled = btnRatioNonePCS.Enabled = false;
            btnTrainAll_SettingNoPCS.Enabled = true;
            btnTrainAll_SettingNoPCS.Focus();
        }
        #endregion

        class ObjectFile
        {
            public int STT { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public int Program { get; set; }
        }

        class InterfaceCamera
        {
            public InterfaceCamera() { }
            public string NameInterface { get; set; }
            public string NameDevice { get; set; }
        }
    }
}
