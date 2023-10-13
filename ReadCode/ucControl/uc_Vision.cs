using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using MC_Protocol_NTT;
using System.Threading;
using System.Net.NetworkInformation;
using DatabaseInterface_MMCV;

namespace ReadCode
{
    public partial class uc_Vision : UserControl
    {
        // KHAI BÁO
        List<ObjectFile> _lstFile;
        string _trainFilePath = Application.StartupPath + "\\TrainFile\\";
        private HTuple _FrameGrabber = null;
        private HWindow _Window = null;

        private ucViewImage ImageResult;
        public static string nameJig = "";
        public int _CamIndex { get; set; }
        public int _ProgramID { get; set; }
        public bool _IsConected { get; set; }
        public bool _IsRunning { get; set; }

        public uc_Vision(int PrgID, int IndCAM)
        {
            _ProgramID = PrgID;
            _CamIndex = IndCAM;
            InitializeComponent();
        }

        /// <summary>
        /// load uc 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucWindow_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            WindowControl.HKeepAspectRatio = false;
            _Window = WindowControl.HalconWindow;
            _Window.SetDraw("margin");
            _Window.SetLineWidth(3);
            lb_IndexCam.Text = _CamIndex.ToString();
            lb_Status.BackColor = Color.Red;
            lb_Status.Text = "Disconnect";
        }
        /// <summary>
        /// Connect Cameara
        /// </summary>
        public void ConnectCamera()
        {
            try
            {
                // Tải dữ liệu setting camera theo Program
                DataTable dt = Support_SQL.GetTableData($"select * from CameraSetting " +
                                                    $"where ID_Program = {_ProgramID} " +
                                                    $"and CamIndex = {_CamIndex} " +
                                                    $"limit 1");
                if (dt.Rows.Count <= 0)
                {
                    new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, "Program is currently not config setting camera: " + _CamIndex).ShowDialog();
                    _IsConected = false;
                    return;
                }
                DataRow row = dt.Rows[0];
                string _interfaceName = row["InterfaceName"].ToString().Trim();
                string _deviceName = row["DeviceName"].ToString().Trim();
                Connect(_interfaceName, _deviceName);
                // NB - 27032023
                //_IsConected = true;
            }
            catch (Exception ex)
            {
                _IsConected = false;
            }
            btn_Setting.Enabled = !_IsConected;
        }
        /// <summary>
        /// Disconnect Camera
        /// </summary>
        public void DisconnectCamera()
        {
            HOperatorSet.CloseFramegrabber(_FrameGrabber);
            _IsConected = false;
            if (lb_Status.InvokeRequired)
            {
                lb_Status.Invoke(new Action(() =>
                {
                    lb_Status.BackColor = Color.Red;
                    lb_Status.Text = "Disconnect";
                }));
            }
            else
            {
                lb_Status.BackColor = Color.Red;
                lb_Status.Text = "Disconnect";
            }
            WindowControl.HalconWindow.ClearWindow();
            btn_Setting.Enabled = !_IsConected;
        }
        /// <summary>
        /// Trigger vision read code
        /// </summary>
        /// <returns></returns>
        public Result Run_Readcode(HImage image)
        {
            //.ListData.Clear();
            return runItem(image);
        }

        internal HImage SnapImageEx()
        {
            try
            {
                HOperatorSet.GrabImage(out HObject _CurrentObject, _FrameGrabber);
                if (_CurrentObject == null)
                    return null;
                else
                    return new HImage(_CurrentObject);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Support Connect
        /// </summary>
        /// <param name="interfaceCamera"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        private void Connect(string interfaceCamera, string deviceName)
        {
            try
            {
                string vendorPattern = @"(vendor:)(.+?)(\s\|)";
                string vendor = Regex.Match(deviceName, vendorPattern).Groups[2].Value;   //đoạn này dùng để tùy biến cửa sổ cam param setting 
                //Update mới do sử dụng phiên bản Halcon 20.11.2 và Camera Hikvision by Hoang 21/05/2022
                string[] deviceTemp = deviceName.Split('|');
                try
                {
                    deviceName = deviceTemp[2];
                    deviceTemp = deviceName.Split(':');
                    deviceName = deviceTemp[1].Trim();
                }
                catch (Exception)
                {
                    deviceTemp = deviceName.Split(':');
                    deviceName = deviceTemp[1].Trim();
                }
                //
                if (interfaceCamera != "File")
                    if (vendor != "Basler" && vendor != "Hikrobot")
                    {
                        new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, "This Device is not supported by your application!").ShowDialog();
                        return;
                    }
                HOperatorSet.OpenFramegrabber(interfaceCamera, 0, 0, 0, 0, 0, 0
                                    , "progressive"
                                    , -1
                                    , "default"
                                    , -1//generic
                                    , "default", interfaceCamera == "File" ? deviceName : "default"
                                    , interfaceCamera == "File" ? "default" : deviceName
                                    , 0
                                    , -1
                                    , out _FrameGrabber);
                // Đã kết nối vào cam - chụp thử ảnh
                HOperatorSet.GrabImage(out HObject hObject, _FrameGrabber);
                HImage Img = new HImage(hObject);
                SmartSetPart(Img, WindowControl);
                Img.Dispose();
                hObject.Dispose();
                if (lb_Status.InvokeRequired)
                {
                    lb_Status.Invoke(new Action(() =>
                    {
                        lb_Status.BackColor = Color.Lime;
                        lb_Status.Text = "Connected";
                    }));
                }
                else
                {
                    lb_Status.BackColor = Color.Lime;
                    lb_Status.Text = "Connected";
                }
                // NB - 27032023
                _IsConected = true;
            }
            catch (Exception Ex)
            {
                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Process Connect Camera Vision Error! \r {Ex}").ShowDialog();
                lb_Status.BackColor = Color.Red;
                lb_Status.Text = "Disconnect";
                _IsConected = false;
            }
        }


        // TODO: 08032023
        HTuple _list_Result_decode_text_OK;
        HTuple _list_Result_decode_row_OK;
        HTuple _list_Result_decode_column_OK;
        HTuple _list_Result_decode_text_NG;
        HTuple _list_Result_decode_row_NG;
        HTuple _list_Result_decode_column_NG;
        HTuple _list_Result_decode_text_Miss;
        HTuple _list_Result_decode_row_Miss;
        HTuple _list_Result_decode_column_Miss;
        /// <summary>
        /// Process trigger and decode barcode
        /// </summary>
        /// <param name="hImage"></param>
        /// <param name="camIndex"></param>
        /// <returns></returns>
        public Result runItem(HImage image)
        {
            //OK
            _list_Result_decode_text_OK = new HTuple();
            _list_Result_decode_row_OK = new HTuple();
            _list_Result_decode_column_OK = new HTuple();
            //NG
            _list_Result_decode_text_NG = new HTuple();
            _list_Result_decode_row_NG = new HTuple();
            _list_Result_decode_column_NG = new HTuple();
            //Miss
            _list_Result_decode_text_Miss = new HTuple();
            _list_Result_decode_row_Miss = new HTuple();
            _list_Result_decode_column_Miss = new HTuple();

            //HA_ADD
            HTuple _list_Result_decode_text_NoICT;
            HTuple _list_Result_decode_row_NoICT;
            HTuple _list_Result_decode_column_NoICT;

            HTuple _list_Result_decode_text_PassICT;
            HTuple _list_Result_decode_row_PassICT;
            HTuple _list_Result_decode_column_PassICT;
            //End HA_ADD

            //int Oldposition = frm_Main.oldPosition;
            Result OutResult = new Result();
            _IsRunning = true;
            // load vị trí các điểm đọc barcode 
            DataTable dtRegion = Support_SQL.GetTableData(string.Format(@"select * from RegionPosition where ID_Program ={0} AND CamIndex={1} ORDER by Position", _ProgramID, _CamIndex));
            if (dtRegion.Rows.Count <= 0)
            {
                // Lỗi không có dữ liệu toạn độ roi
            }
            loadFileTrain();
            Stopwatch stopwatch = new Stopwatch();
            HObject _CurrentObject = new HObject();
            _Window.SetDraw("margin");
            _Window.SetLineWidth(3);
            stopwatch.Start();
            _Window.DetachBackgroundFromWindow();
            // Hiển thị ảnh lên HsmartWindown
            HImage _CurrentImage = image;
            _CurrentImage.DispImage(_Window);
            _CurrentObject.Dispose();

            //Đưa ảnh vào 1 HSWINDOW khác để Lưu ảnh
            ImageResult = new ucViewImage();
            ImageResult.ViewImage(_CurrentImage);

            // Đọc code Jig Trước 
            ReadBarCode(_CurrentImage, _ProgramID, _CamIndex);

            DateTime start = DateTime.Now;
            //Duyệt danh sách vùng ROI để thiết lập để cắt ảnh và giải mã Barocde
            int countRegions = dtRegion.Rows.Count;
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
                int camIndex = Support_SQL.ToInt(item["CamIndex"]);
                // TODO: 11042023
                double _mediumGray = Convert.ToDouble(item["MediumGray"]);
                double _realRatioPCS = Convert.ToDouble(item["RealRatioPCS"]);

                HRegion region = new HRegion(X1, Y1, X2, Y2);
                HImage ImgReduced = _CurrentImage.ReduceDomain(region);
                region.Dispose();
                //Duyệt qua danh sách các file train đến khi file train nào trả ra kết quả thì thôi, nếu ko thì trả ra noread
                string itemText = string.Empty;
                string itemTextOut = string.Empty;
                string genvalue = string.Empty;
                //int position = frm_Main.oldPosition;
                int position = Support_SQL.ToInt(item["Position"])+ frm_Main.oldPosition;
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
                    HOperatorSet.GetDataCode2dParam(dataCodeHandle, "symbol_type", out HTuple GenParamValue);
                    genvalue = GenParamValue.ToString(); ;
                    if (DecodedDataStrings.Length > 0)
                    {
                        _Window.SetColor("green");
                        _Window.DispObj(SymbolXLDs);

                        //Add kết quả OK của từng con hàng chụp vào ảnh sẽ lưu
                        ImageResult.DispObj(SymbolXLDs, "green");
                        itemText = position + " - " + DecodedDataStrings.ToString();
                        itemTextOut = DecodedDataStrings.ToString();
                        HOperatorSet.TupleConcat(_list_Result_decode_text_OK, (HTuple)itemText, out _list_Result_decode_text_OK);
                        HOperatorSet.TupleConcat(_list_Result_decode_row_OK, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_OK);
                        HOperatorSet.TupleConcat(_list_Result_decode_column_OK, (HTuple)(Y1 - 100 + (Y2 - Y1) / 2), out _list_Result_decode_column_OK);
                    }
                    SymbolXLDs.Dispose();
                    GenParamValue.Dispose();
                    DecodedDataStrings.Dispose();
                    dataCodeHandle.Dispose();
                    ResultHandles.Dispose();
                    //TODO: NB - 23032023
                    if (DecodedDataStrings.Length > 0)
                        break;
                    else
                        continue;
                }

                // NB - 03042023 -> CHỈNH SỬA LẠI KIỂM TRA NOPCS, NOREAD LÊN TRƯỚC
                if (DecodedDataStrings.Length == 0)
                {
                    string _color = string.Empty;
                    
                    // TODO: 11042023
                    HOperatorSet.Threshold(ImgReduced, out HObject RegionP, 0, _mediumGray);
                    HOperatorSet.ClosingRectangle1(RegionP, out RegionP, 9, 9);
                    HOperatorSet.Connection(RegionP, out RegionP);
                    HOperatorSet.SelectShapeStd(RegionP, out RegionP, "max_area", 60);
                    HOperatorSet.AreaCenter(RegionP, out HTuple AreaP, out HTuple RowP, out HTuple ColumnP);
                    // đọc ra vùng vẽ roi -> trả ra diện tích full roi
                    HOperatorSet.Threshold(ImgReduced, out HObject RegionFull, 0, 255);
                    HOperatorSet.AreaCenter(RegionFull, out HTuple AreaFull, out HTuple RowFull, out HTuple ColumnFull);
                    double ratio = Math.Round(AreaP.D / AreaFull.D, 6) * 100;
                    // TODO: 11042023
                    if (ratio < _realRatioPCS)
                    {
                        // NO PCS
                        _color = "magenta";
                        _Window.SetColor(_color);
                        itemTextOut = c_varGolbal.Missing;
                        itemText = item["Position"] + " - " + c_varGolbal.Missing + $"\r\n[{ratio}%] < [{_realRatioPCS}%]"; ;
                        HOperatorSet.TupleConcat(_list_Result_decode_text_Miss, (HTuple)itemText, out _list_Result_decode_text_Miss);
                        HOperatorSet.TupleConcat(_list_Result_decode_row_Miss, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_Miss);
                        HOperatorSet.TupleConcat(_list_Result_decode_column_Miss, (HTuple)(Y1 - 100 + (Y2 - Y1) / 2), out _list_Result_decode_column_Miss);
                        ImageResult.ViewImage(_CurrentImage);
                        ImageResult.ViewResult(itemText, (X1 + (X2 - X1) / 2), (Y1 - 100 + (Y2 - Y1) / 2), new HTuple("magenta"));
                    }
                    else
                    {
                        HOperatorSet.ScaleImage(ImgReduced, out HObject ImageScaled, 2.38318, -253);
                        HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(), out HTuple dataCodeHandle1);
                        HOperatorSet.FindDataCode2d(ImageScaled, out HObject SymbolXLDs1, dataCodeHandle1, new HTuple("stop_after_result_num")
                        , new HTuple(numCode), out HTuple ResultHandles1, out HTuple DecodedDataStrings_Train);
                        if (DecodedDataStrings_Train.Length > 0)
                        {
                            _Window.SetColor("green");
                            _Window.DispObj(SymbolXLDs1);
                            itemText = position + " - " + DecodedDataStrings_Train.ToString();
                            itemTextOut = DecodedDataStrings_Train.ToString();

                            HOperatorSet.TupleConcat(_list_Result_decode_text_OK, (HTuple)itemText, out _list_Result_decode_text_OK);
                            HOperatorSet.TupleConcat(_list_Result_decode_row_OK, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_OK);
                            HOperatorSet.TupleConcat(_list_Result_decode_column_OK, (HTuple)(Y1 - 100 + (Y2 - Y1) / 2), out _list_Result_decode_column_OK);
                        }
                        SymbolXLDs1.Dispose();
                        DecodedDataStrings_Train.Dispose();
                        dataCodeHandle1.Dispose();
                        ResultHandles1.Dispose();
                        if (DecodedDataStrings_Train.Length == 0)
                        {
                            // NO READ
                            _color = "red";
                            _Window.SetColor(_color);
                            itemTextOut = c_varGolbal.NoReadString;
                            // NB - 26032023
                            itemText = item["Position"] + " - " + c_varGolbal.NoReadString + $"\r\n[{ratio}%] > [{_realRatioPCS}%]";
                            HOperatorSet.TupleConcat(_list_Result_decode_text_NG, (HTuple)itemText, out _list_Result_decode_text_NG);
                            HOperatorSet.TupleConcat(_list_Result_decode_row_NG, (HTuple)(X1 + (X2 - X1) / 2), out _list_Result_decode_row_NG);
                            HOperatorSet.TupleConcat(_list_Result_decode_column_NG, (HTuple)(Y1 - 100 + (Y2 - Y1) / 2), out _list_Result_decode_column_NG);
                            // NB - 01042023
                            ImageResult.ViewImage(_CurrentImage);
                            ImageResult.ViewResult(itemText, (X1 + (X2 - X1) / 2), (Y1 - 100 + (Y2 - Y1) / 2), new HTuple("red"));
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
                stopwatch.Stop();
                // đẩy dữ liệu code thu được đưa vào list class "Code"
                Code data = new Code(camIndex, position, genvalue, itemTextOut.Replace('"', ' ').Trim(), false); //HA_ADD
                OutResult.ListData.Add(data);
                ImgReduced.Dispose();
                if (i == countRegions - 1)
                {
                    frm_Main.oldPosition = position;
                }
                
            }
            if (_list_Result_decode_text_NG.Length > 0)
            {
                ImageResult.SaveImage(c_varGolbal.CodeJig, "NG", _CamIndex);
            }
            
            // TODO: 01042023
            ImageResult?.Dispose();
            ImageResult = new ucViewImage();
            
            //HA_ADD
            //Test
            _list_Result_decode_text_NoICT = new HTuple();
            _list_Result_decode_row_NoICT = new HTuple();
            _list_Result_decode_column_NoICT = new HTuple();

            _list_Result_decode_text_PassICT = new HTuple();
            _list_Result_decode_row_PassICT = new HTuple();
            _list_Result_decode_column_PassICT = new HTuple();

            checkDataICT(ref OutResult);
            DateTime Stop = DateTime.Now;
            TimeSpan kq = Stop - start;
            List<Code> checkICT = OutResult.ListData.FindAll(x => !x.Content.Contains(c_varGolbal.NoReadString) && !x.Content.Contains(c_varGolbal.Missing));
            for (int i = 0; i < checkICT.Count; i++)
            {

                string content = _list_Result_decode_text_OK.SArr[i];
                double row = _list_Result_decode_row_OK.DArr[i];
                double col = _list_Result_decode_column_OK.DArr[i];
                if (checkICT[i].statusICT)
                {
                    HOperatorSet.TupleConcat(_list_Result_decode_text_PassICT, (HTuple)content, out _list_Result_decode_text_PassICT);
                    HOperatorSet.TupleConcat(_list_Result_decode_row_PassICT, (HTuple)row, out _list_Result_decode_row_PassICT);
                    HOperatorSet.TupleConcat(_list_Result_decode_column_PassICT, (HTuple)col, out _list_Result_decode_column_PassICT);
                }
                else
                {
                    HOperatorSet.TupleConcat(_list_Result_decode_text_NoICT, (HTuple)(content + "No Data ICT"), out _list_Result_decode_text_NoICT);
                    HOperatorSet.TupleConcat(_list_Result_decode_row_NoICT, (HTuple)row, out _list_Result_decode_row_NoICT);
                    HOperatorSet.TupleConcat(_list_Result_decode_column_NoICT, (HTuple)col, out _list_Result_decode_column_NoICT);
                }
            }
            // End HA_ADD


            //
            //HA_ADD
            //pass ICT
            if (_list_Result_decode_text_PassICT.Length > 0)
            {
                this.Invoke(new Action(() =>
                {
                    _Window.DispText(_list_Result_decode_text_PassICT, "image", _list_Result_decode_row_PassICT, _list_Result_decode_column_PassICT, new HTuple("green"), new HTuple(), new HTuple());
                }));
            }
            //    //No ICT
            if (_list_Result_decode_text_NoICT.Length > 0)
            {
                this.Invoke(new Action(() =>
                {
                    _Window.DispText(_list_Result_decode_text_NoICT, "image", _list_Result_decode_row_NoICT, _list_Result_decode_column_NoICT, new HTuple("red"), new HTuple(), new HTuple());
                }));
            }
            // END HA_ADD
            //


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
            _CurrentImage.Dispose();
            _IsRunning = false;
            return OutResult;
        }

        public void SnapImage()
        {
            //Load file train
            loadFileTrain();
            Stopwatch stopwatch = new Stopwatch();
            HObject _CurrentObject = new HObject();
            _Window.SetDraw("margin");
            _Window.SetLineWidth(3);
            stopwatch.Start();
            //Chụp ảnh
            try
            {
                _Window.DetachBackgroundFromWindow();
                try
                {
                    HOperatorSet.GrabImage(out _CurrentObject, _FrameGrabber);
                }
                catch { return; }
            }
            catch (Exception ex)
            {
            }
            // Hiển thị ảnh lên HsmartWindown
            HImage _CurrentImage = new HImage(_CurrentObject);
            _CurrentImage.DispImage(_Window);
            _CurrentObject.Dispose();
            stopwatch.Stop();
            ReadBarCode(_CurrentImage, _ProgramID, _CamIndex);
        }

        List<string> CodeTagJig = new List<string>();
        private void ReadBarCode(HImage _Img, int ProgramID, int CamIndex)
        {
            CodeTagJig.Clear();
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
                    string itemText = string.Empty;
                    for (int j = 0; j < _lstFile.Count; j++)
                    {
                        ObjectFile file = _lstFile[j];

                        HOperatorSet.ReadDataCode2dModel(file.FilePath, out HTuple dataCodeHandle);
                        HOperatorSet.FindDataCode2d(ImgReduced, out HObject SymbolXLDs, dataCodeHandle, "stop_after_result_num"
                            , numCode, out HTuple ResultHandles, out HTuple DecodedDataStrings);
                        if (DecodedDataStrings.Length > 0)
                        {
                            _Window.SetColor("green");
                            _Window.DispObj(SymbolXLDs);
                            c_varGolbal.CodeJig = DecodedDataStrings.O.ToString();
                            itemText = item["Position"] + " - " + DecodedDataStrings.ToString();
                            CodeTagJig.Add(DecodedDataStrings);
                            break;
                        }
                        SymbolXLDs.Dispose();
                        DecodedDataStrings.Dispose();
                        dataCodeHandle.Dispose();
                        ResultHandles.Dispose();

                        if (DecodedDataStrings.Length > 0)
                            break;
                        else
                            continue;
                    }

                    if (string.IsNullOrEmpty(itemText))
                    {
                        // Todo: Vinh them ngày 04/03/23
                        if (c_varGolbal.IsFilter)
                        {
                            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(), out HTuple DataCodeHandle);
                            HOperatorSet.ScaleImage(ImgReduced, out HObject ImageCode, 6.71053, -161);
                            HOperatorSet.FindDataCode2d(ImageCode, out HObject SymbolXLDs, DataCodeHandle, new HTuple("train", "stop_after_result_num"),
                                new HTuple("all", 1), out HTuple ResultHandles, out HTuple DecodedDataStrings);
                            // TODO: NB - 06032023
                            if (DecodedDataStrings.Length > 0)
                            {
                                CodeTagJig.Add(DecodedDataStrings);
                                _Window.SetColor("green");
                                _Window.DispObj(SymbolXLDs);
                                itemText = item["Position"] + " - " + DecodedDataStrings.ToString();
                                c_varGolbal.CodeJig = DecodedDataStrings.O.ToString();
                                SymbolXLDs.Dispose();
                                DecodedDataStrings.Dispose();
                            }
                            else
                            {
                                HOperatorSet.ScaleImage(ImgReduced, out ImageCode, 11.591, -12);
                                HOperatorSet.FindDataCode2d(ImageCode, out SymbolXLDs, DataCodeHandle, new HTuple("train", "stop_after_result_num"), new HTuple("all", 1), out ResultHandles, out DecodedDataStrings);
                                if (DecodedDataStrings.Length > 0)
                                {
                                    CodeTagJig.Add(DecodedDataStrings);
                                    _Window.SetColor("green");
                                    _Window.DispObj(SymbolXLDs);
                                    itemText = item["Position"] + " - " + DecodedDataStrings.ToString();
                                    c_varGolbal.CodeJig = DecodedDataStrings.O.ToString();
                                    SymbolXLDs.Dispose();
                                    DecodedDataStrings.Dispose();
                                }
                                else
                                {
                                    _Window.SetColor("red");
                                    _Window.DispObj(SymbolXLDs);
                                    itemText = item["Position"] + " - " + c_varGolbal.NoReadString;// "NoRead";
                                    SymbolXLDs.Dispose();
                                    DecodedDataStrings.Dispose();
                                    ResultHandles.Dispose();
                                    HOperatorSet.WriteImage(_Img, "bmp", new HTuple(0), Application.StartupPath + @"\ErrorImage" + @"\Image_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                                }
                            }
                        }
                    }
                    // Show dữ liệu code lên windown
                    _Window.DispText(itemText, "image", X2 + 5, Y1, new HTuple(), new HTuple(), new HTuple());
                    ImgReduced.Dispose();
                }
            }
            catch (Exception ex)
            {
                string path = Application.StartupPath + "/ErrorLog"; //pathExportFile + lblMachineID.Text;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = string.Format("Error_{0}_{1}_{2}.txt", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                File.AppendAllText(path + "/" + fileName,
                    DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ": " + ex.ToString() + System.Environment.NewLine);
                throw;
            }
        }

        public List<string> Run_ReadCodeJig()
        {
            return CodeTagJig;//ReadCodeJig();
        }
        /// <summary>
        /// lấy danh sách các file train và load các file đó từ thư mục
        /// </summary>
        /// <param name="CamIndex"></param>
        private void loadFileTrain()
        {
            DataTable dt = Support_SQL.GetTableData("select * from TrainFile where ID_Program = " + _ProgramID + " and CamIndex = " + _CamIndex + " Order by STT");

            _lstFile = new List<ObjectFile>();
            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                ObjectFile file = new ObjectFile();
                file.STT = Convert.ToInt32(dt.Rows[i]["STT"]);
                file.Program = _ProgramID;
                file.FileName = dt.Rows[i]["FileName"].ToString();
                file.FilePath = _trainFilePath + file.FileName;
                _lstFile.Add(file);
            }
        }
        /// <summary>
        /// Disable nút bấm setting
        /// </summary>
        public void DisableSettingButton()
        {
            btn_Setting.Enabled = false;
        }
        /// <summary>
        /// Enable nút bấm setting
        /// </summary>
        public void EnableSettingButton()
        {
            btn_Setting.Enabled = true;
        }
        private void my_MouseWheel(object sender, MouseEventArgs e)
        {
            HSmartWindowControl w = (HSmartWindowControl)sender;
            Point pt = w.Location;
            MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
            w.HSmartWindowControl_MouseWheel(sender, newe);
        }
        private void WindowControl1_Load(object sender, EventArgs e)
        {
            HSmartWindowControl w = (HSmartWindowControl)sender;
            w.MouseWheel += my_MouseWheel;
        }

        private void WindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                (sender as HSmartWindowControl).HalconWindow.ConvertCoordinatesWindowToImage(e.Y, e.X, out double rowInImage, out double columnInImage);

                lb_X_Window.Text = "X = " + e.Y.ToString("F1");
                lb_Y_Window.Text = "Y = " + e.X.ToString("F1");
            }
            catch (Exception)
            {
            }
        }
        private void SmartSetPart(HObject Image, HSmartWindowControl HSWindow)
        {
            HOperatorSet.GetImageSize(Image, out HTuple width, out HTuple height);
            int wdWidth = HSWindow.Width;
            int wdHeight = HSWindow.Height;

            int imgRow, imgCol;
            int imgWidth, imgHeight;
            if (((float)wdHeight / (float)wdWidth) > ((float)height / (float)width))
            {
                imgCol = 0;
                imgWidth = width;
                imgHeight = wdHeight * imgWidth / wdWidth;
                imgRow = (height - imgHeight) / 2;
            }
            else
            {
                imgRow = 0;
                imgHeight = height;
                imgWidth = wdWidth * imgHeight / wdHeight;
                imgCol = (width - imgWidth) / 2;
            }
            HSWindow.HalconWindow.SetPart(new HTuple(imgRow), new HTuple(imgCol), new HTuple(imgHeight + imgRow), new HTuple(imgWidth + imgCol));
        }
        private void bnt_Setting_Click(object sender, EventArgs e)
        {
            frm_SettingReadCode frm = new frm_SettingReadCode(_ProgramID, _CamIndex);

            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("192.168.125.40", 1000);
                if (reply != null && reply.Status.ToString().ToUpper() == "SUCCESS")
                {
                    ConnectPLC();
                    frm.PLC_Fx5 = PLC_Fx5;
                    frm.ShowDialog();
                }
                else
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public void Clear_HSmart_Window()
        {
            WindowControl.HalconWindow.ClearWindow();
        }
        MC_Protocol PLC_Fx5 = new MC_Protocol();
        void ConnectPLC()
        {
            try
            {
                PLC_Fx5.IP_Adress = "192.168.125.40";
                PLC_Fx5.Port = 2000;

                if (PLC_Fx5.Connected)
                {
                    PLC_Fx5.Disconnect();
                    Thread.Sleep(100);
                    PLC_Fx5.Connect();
                }
                else
                {
                    PLC_Fx5.Connect();
                }
            }
            catch (Exception ex)
            {
                new frm_ShowDialog(frm_ShowDialog.Icon_Show.Error, $"Error Connect PLC \r {ex}").ShowDialog();
                return;
            }

        }

        void checkDataICT(ref Result outResult)
        {
            // TH1:0.03s
            List<Code> checkICT = outResult.ListData.FindAll(x => !x.Content.Contains(c_varGolbal.NoReadString) && !x.Content.Contains(c_varGolbal.Missing));

            DateTime start = DateTime.Now;
            List<string> data = new List<string>();
            foreach (Code item in checkICT)
            {
                data.Add($"'{item.Content}'");
            }
            List<string> NoIct = new List<string>();
            try
            {
                Plasma56 mmcv = new Plasma56();
                NoIct=mmcv.CheckDataICT(data);
                if (NoIct.Count > 0)
                {
                    foreach (Code item in checkICT)
                    {
                        int index = outResult.ListData.FindIndex(x => x.Content.Contains(item.Content));
                        if (index >= 0)
                        {
                            if (NoIct.Contains(item.Content))
                            {
                                outResult.ListData[index].statusICT = false;
                            }
                            else
                            {
                                outResult.ListData[index].statusICT = true;
                            }
                            
                        }
                    }
                }
                else
                {
                    foreach (Code item in checkICT)
                    {
                        int index = outResult.ListData.FindIndex(x => x.Content.Contains(item.Content));
                        if (index >= 0)
                        {
                            outResult.ListData[index].statusICT = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(delegate
                {
                    MessageBox.Show(ex.ToString());
                }));
            }
            DateTime stop = DateTime.Now;
            TimeSpan exetime = stop - start;
            var kq = exetime.TotalSeconds;
        }
    }
}
