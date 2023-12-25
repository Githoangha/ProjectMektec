using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MC_Protocol_NTT;
using ActUtlTypeLib;
using DatabaseInterface_MMCV;
using System.Net.Sockets;
using GemBox.Spreadsheet;
//using WaitWnd;

namespace LineGolden_PLasma
{
    public partial class uc_Plasma : UserControl
    {

        #region variable Genenal
        // Genenal
        public static bool IsExecuteFunctionReadTag = false;
        private string tagJigTransfer, tagJigPlasma, codePCS, dateTimeIn, dateTimeOut, cycleTimePlasma;
        public bool IsConnectPLC = false;
        public string NameFileLog_Main = "FileLog_Main";
        public string NameFileLog_ProcessAuto = "FileLog_ProcessAuto";

        // --------------------------------------------------------------------------------------------------------------------//
        #endregion
        #region variable Oject Lock Thread
        // Oject Lock Thread
        static readonly object lockUpdateDataSever = new object();
        static readonly object lockSetDataDBPlasma = new object();
        static readonly object lockDisplayDataPlasma = new object();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable List
        // List
        List<Control> lst_Control = new List<Control>();
        dataPlasma dataPlasma = new dataPlasma();
        List<dataPlasma> lst_dataPlasma = new List<dataPlasma>();
        public static List<string> List_DataTagPlasmaInput = new List<string>();
        public static List<string> List_DataCodeTray = new List<string>();
        List<string> List_DataTagPlasmaOutput = new List<string>();
        List<CamBarcode> Lst_CamBarcode = new List<CamBarcode>();


        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Socket (Camera Barcode)
        // variable Socket

        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Excel
        // Excel
        SupportExcel Excel = new SupportExcel();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable PLC
        // PLC
        //MC_Protocol PLC_Fx5Plasma = new MC_Protocol();

        public ActUtlTypeClass PLC_Fx3Plasma = new ActUtlTypeClass();

        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Thread
        // Thread
        private Thread Main_ReadTagPlasma;
        private Thread Main_UpDateDataPlasma;
        private Thread Main_ResetPlasma; //Không dùng 
        private Thread Main_AutoUploadDataWait;
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Sever MMCV
        // Sever MMCV
        Plasma56 MMCV_DBPlasma = new Plasma56();
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region variable Contruction
        // variable Contruction
        internal Form Parent { get; set; }
        internal int ProgramID { get; set; }
        internal int _UcPlasmaIndex { get; set; }
        internal int QtyCamBarcode { get; set; }
        internal int PLasmaIndex { get; set; }
        internal int NumJigPlasma { get; set; }
        internal string ModeReadcodePCS { get; set; }
        public bool IsConnectCam { get; set; }
        public string _LineID { get; set; }
        public string _DeviceID { get; set; }
        public string bitAutoManual { get; set; }
        public string NamePlasma { get; set; }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        public uc_Plasma(Form parent, int PrgID, int indPlasma, int UcPlasmaIndex, int NumJigPlasma, string ModeReadcodePCS)
        {
            InitializeComponent();
            Parent = parent;
            ProgramID = PrgID;
            PLasmaIndex = indPlasma;
            _UcPlasmaIndex = UcPlasmaIndex;
            this.NumJigPlasma = NumJigPlasma;
            this.ModeReadcodePCS = ModeReadcodePCS;
            lst_Control.Add(lb_infoJigInput);
            lst_Control.Add(lb_infoJigMachine);
            lst_Control.Add(lb_infoJigOutput);
            _LineID = c_varGolbal.LineID;
            _DeviceID = c_varGolbal.DeviceID;
            NamePlasma = c_varGolbal.NamePlasma;
        }
        /// <summary>
        /// from load use control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Rfid_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            lb_IndexPlasma.Text = PLasmaIndex.ToString();
            lb_Status_Barcode.BackColor = Color.Red;
            lb_Status_Barcode.Text = "Disconnect";
            lbStatusUpload.Text = "Đang upload data lên server MMCV";
            lbStatusUpload.Text = "Đang chờ máy boxing xử lý xong tray";
            //load data field Name

            string sqlDataHistory = $"select * from Plasma where ID_Plasma=0";
            dtTest = Support_SQL.GetTableDataPlasma(sqlDataHistory);
            grdViewTag.DataSource = dtTest;
        }
        private string Formast_String(string Values)
        {

            if (Values != "")
            {
                List<string> arrResult = Values.Split('/').ToList();
                return string.Join("\r\n", arrResult);
            }
            return "";
        }

        void loadListCamBarcode()
        {
            Lst_CamBarcode.Clear();
            DataTable dt = Support_SQL.GetTableData($"SELECT *FROM {PLS.NameTable} WHERE {PLS.ID}={ProgramID} AND {PLS.PlasmaIndex}={PLasmaIndex} ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string ip = dt.Rows[i][PLS.Ip_Barcode].ToString();
                int port = Lib.ToInt(dt.Rows[i][PLS.Port_Barcode]);
                CamBarcode cam = new CamBarcode(ip, port, NumJigPlasma);
                Lst_CamBarcode.Add(cam);
            }
        }
        #region Process Read Tag Plasma 
        private int Step_ReadTagPlasma = 0;
        public bool lastUpdate = false;
        /// <summary>
        /// Luồng chạy process đọc tag Barcode đầu vào máy Plasma
        /// </summary>
        public void MainThreadReadTagPlasma()
        {
            Main_ReadTagPlasma = new Thread(new ThreadStart(ProcessReadTagPlasma));
            Main_ReadTagPlasma.IsBackground = true;
            Main_ReadTagPlasma.Start();
        }
        public void Stop_MainThreadReadTagPlasma()
        {
            if (Main_ReadTagPlasma != null)
            {
                Main_ReadTagPlasma.Abort();
            }
        }

        bool _Flag_CodePCS = true;
        /// <summary>
        /// Process đọc tag Barcode đầu vào máy Plasma
        /// </summary>
        private void ProcessReadTagPlasma()
        {
            //SET :Reset  TriggerHaveData,TriggerError,TriggerOK,TriggerReadCode
            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 0);
            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveDataOK, 0);
            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerOK, 0);
            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 0);

            int step = 0;
            while (c_varGolbal.IsRun) //Thread Read Tag
            {
                Thread.Sleep(100);
                try
                {
                    switch (Step_ReadTagPlasma)
                    {
                        case 10: // Đọc dữ liệu tag ở đầu vào                         
                            {

                                if (step == 0)
                                {
                                    List_DataTagPlasmaInput = new List<string>();
                                }
                                //Clear List DataTagPlasmaInput
                                List<string> lstJigPlasmaError = new List<string>();
                                //Trigger BarCode 
                                
                                //SET :Reset TriggerReadCodeOK
                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerReadCodeOK, 0);
                                StartTriggerCam(ConstSendByte.TON);
                                DateTime start = DateTime.Now;
                                bool Completed = ExecuteWithTimeLimit(TimeSpan.FromSeconds(5), () => { StartReadTag(); });
                                DateTime stop = DateTime.Now;
                                TimeSpan kq_test_time = stop - start;

                                //HuyNV
                                //Check số lượng CodeJig có bằng số lượng TagJig đã khai báo hay chưa và Đã đủ số lượng CodeTray =2 không.

                                Lib.ShowLabelResult(Color.White, lbStatusReadJig, "Begin Read Jig");
                                Lib.EnableLabel(lbStatusReadJig, true);
                                if (List_DataTagPlasmaInput.Count != NumJigPlasma || List_DataCodeTray.Count != 2)
                                {
                                    if (step <= 3)
                                    {
                                        step = step + 1;
                                        //StartTriggerCam(ConstSendByte.TON);
                                        Completed = true;
                                        continue;
                                    }
                                }
                                List_DataTagPlasmaInput= List_DataTagPlasmaInput.FindAll(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrEmpty(x));
                                if (List_DataTagPlasmaInput.Count <= 0)
                                {
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                    //SET :TriggerHaveData
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                    Step_ReadTagPlasma = 0; //EndStep khi không đọc được jig nào
                                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"không đọc được bất kì jig nào {string.Join("\r\n", List_DataTagPlasmaInput)}").ShowDialog();
                                    continue;
                                }
                                Lib.ShowLabelResult(Color.White, lbStatusReadJig, "kiểm tra số lượng code jig và code tray");
                                if (List_DataCodeTray.Count == 2)
                                {
                                    //Số lượng Code bằng số lượng TagJig 
                                    if (List_DataTagPlasmaInput.Count == NumJigPlasma)
                                    {
                                        //HuyNV-16/02/2023
                                        //xóa bỏ dữ liệu cũ của con codeJig khi đưa lại con đấy vào.
                                        //
                                        //
                                        lstJigPlasmaError = checkDateTimePlasma(List_DataTagPlasmaInput);
                                        if (lstJigPlasmaError.Count == 0)
                                        {
                                            Step_ReadTagPlasma = 20; //Nextstep
                                        }
                                        else if (lstJigPlasmaError.Count >= 0)
                                        {
                                            string strtagJigPlasmaError = string.Join("\r\n", lstJigPlasmaError);

                                            //SET : TriggerError
                                            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                            //SET :TriggerHaveData
                                            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                            Step_ReadTagPlasma = 0;// End Step khi jig chưa đủ thời gian ở máy plasma
                                            new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error,
                                                $"Check Time Jig In Plasma {_UcPlasmaIndex + 1}:\r\n" +
                                                $"Làm ơn kiểm tra lại thời gian Jig lặp lại trong Plasma {_UcPlasmaIndex + 1}:\r\n" +
                                                $"Có {lstJigPlasmaError.Count} Jig không đạt, Jig đó là:\r\n{strtagJigPlasmaError}").ShowDialog();
                                        }
                                        step = 0;
                                    }
                                    //Số lượng Code nhiều hơn số lượng  TagJig
                                    else if (List_DataTagPlasmaInput.Count > NumJigPlasma)
                                    {
                                        //SET : TriggerError
                                        PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                        //SET :TriggerHaveData
                                        PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                        Step_ReadTagPlasma = 0;//end step khi số jig nhiều hơn số setting
                                        new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error,
                                                $"Check NumberJig In Plasma .\r\n" +
                                                $"Số lượng CodeJig vượt quá qui định.\r\n" +
                                                $"Vui lòng kiểm tra lại").ShowDialog();

                                    }
                                    //Số lượng Code ít hơn số lượng TagJig
                                    else
                                    {
                                        Frm_Confirm newfrm = new Frm_Confirm(Frm_Confirm.Icon_Show.Warning,
                                                $"Check NumberJig In Plasma .\r\n" +
                                                $"Số lượng CodeJig ít hơn số lượng quy định.\r\n" +
                                                $"{string.Join("\r\n", List_DataTagPlasmaInput)}" +
                                                $"Nhấn Confirm để tiếp tục.");
                                        if (newfrm.ShowDialog() == DialogResult.OK)
                                        {
                                            lstJigPlasmaError = checkDateTimePlasma(List_DataTagPlasmaInput);

                                            if (lstJigPlasmaError.Count == 0)
                                            {
                                                Step_ReadTagPlasma = 20; //Nextstep khi confirm OK
                                            }
                                            else if (lstJigPlasmaError.Count >= 0)
                                            {
                                                string strtagJigPlasmaError = string.Join("\r\n", lstJigPlasmaError);

                                                //SET : TriggerError
                                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                                //SET :TriggerHaveData
                                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                                Step_ReadTagPlasma = 0;// End Step khi confirm NG
                                                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error,
                                                    $"Check Time Jig In Plasma {_UcPlasmaIndex + 1}:\r\n" +
                                                    $"Làm ơn kiểm tra lại thời gian Jig lặp lại trong Plasma {_UcPlasmaIndex + 1}: \r\n" +
                                                    $"Có {lstJigPlasmaError.Count} Jig không đạt, Jig đó là:\r\n{strtagJigPlasmaError}").ShowDialog();
                                            }
                                            step = 0;
                                        }
                                        else
                                        {
                                            //SET : TriggerError
                                            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                            Step_ReadTagPlasma = 0; //EndStep
                                                                    //SET :TriggerHaveData
                                            PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                        }
                                    }
                                }
                                else
                                {
                                    //SET : TriggerError
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                    //SET :TriggerHaveData
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                    Step_ReadTagPlasma = 0; //EndStep khi thiếu code tray
                                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Thiếu số lượng Code Tray\r\n code tray đọc được {string.Join("\r\n", List_DataCodeTray)}").ShowDialog();

                                }
                            }
                            step = 0;
                            break;
                        case 20:// Xử lý dữ liệu tag đọc được và đưa vào buffer Plasma
                            {
                                //Kiểm tra xem TagJig đã có dữ liệu trong sản phẩm hay chưa 
                                switch (ModeReadcodePCS)
                                {
                                    case "Disable":
                                        {
                                            try
                                            {
                                                lock (lockSetDataDBPlasma)
                                                {
                                                    foreach (string item in List_DataTagPlasmaInput)
                                                    {
                                                        Support_SQL.SaveDataToBufferPlasma_new(ProgramID, PLasmaIndex, "", item, "", String.Join(",", List_DataCodeTray), c_varGolbal.LotID, c_varGolbal.MPN);
                                                    }
                                                    Step_ReadTagPlasma = 30;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //SET : TriggerError
                                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                                //SET :TriggerHaveData
                                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                                Step_ReadTagPlasma = 0;
                                                //throw;
                                            }

                                            break;
                                        }
                                    case "Enable":
                                        {
                                            try
                                            {
                                                lock (lockSetDataDBPlasma)
                                                {
                                                    List<string> list_Jig_False = new List<string>(); // List của code jig để kiểm tra data trên máy before plasma 
                                                    List<string> list_Lock_Qualify = new List<string>();
                                                    DataTable dtreadcode = new DataTable();
                                                    DataTable dtAllJig = null;
                                                    List<ID_Jig> lst_ID_Jig = new List<ID_Jig>();
                                                    bool checkPcsExit = false;
                                                    bool checkLot = true;
                                                    //HuyNV 01-03-2023
                                                    //Nếu k có máy FVI thì là Code_TagJigNonePCS --- Nếu có FVI thì là Code_TagJigHavePCS
                                                    string NameCol = "";
                                                    if (c_varGolbal.GetJigHavePcs)
                                                    {
                                                        NameCol = "Code_TagJigHavePCS";
                                                    }
                                                    else
                                                    {
                                                        NameCol = "Code_TagJigNonePCS";
                                                    }
                                                    Lib.ShowLabelResult(Color.White, lbStatusReadJig, "lấy data sản phẩm đã đọc ở máy readcode");
                                                    foreach (string item in List_DataTagPlasmaInput) // kiểm tra jig đã qua máy before plasma chưa ?
                                                    {
                                                        string sql_readCode = $"select * from readcode where {NameCol}='{item.Replace("\n", "").Replace("\0", "")}' AND StatusUpload = false ORDER BY ID DESC LIMIT 1 ";//or Code_TagJigNonePCS='{item}'
                                                        //new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Infor, $"{sql_readCode} Usemachine:{c_varGolbal.UseMachine}").ShowDialog();
                                                        //Check xem máy plasma lấy dữ liệu từ máy nào ? FVI hay Berfore Plasma
                                                        if (c_varGolbal.UseMachine == 2) //line có 2 máy before plasma sẽ xử lý khác với line có 1 máy
                                                        {
                                                            if (c_varGolbal.QtyBeforePlasma == 2)
                                                            {
                                                                foreach (PathFile linkFile in c_varGolbal.List_LinkDB)
                                                                {
                                                                    if (linkFile.isUse)
                                                                    {
                                                                        dtreadcode = Support_SQL.GetTableDataReadCode(sql_readCode, linkFile.LinkDB);
                                                                        if (dtreadcode.Rows.Count > 0)
                                                                        {
                                                                            ID_Jig newItem = new ID_Jig();
                                                                            newItem.ID = Support_SQL.ToInt(dtreadcode.Rows[0]["ID"]);
                                                                            newItem.NameJig = dtreadcode.Rows[0][NameCol].ToString();
                                                                            newItem.link = linkFile.LinkDB;
                                                                            newItem.lstPcs = new List<string>();
                                                                            newItem.LotID = Lib.ToString(dtreadcode.Rows[0]["LotID"]);
                                                                            lst_ID_Jig.Add(newItem);
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dtreadcode = Support_SQL.GetTableDataReadCode(sql_readCode, c_varGolbal.str_ConnectDBReadCode_BeforePlasma1);
                                                                
                                                                
                                                                if (dtreadcode.Rows.Count > 0)
                                                                {
                                                                    
                                                                    ID_Jig newItem = new ID_Jig();
                                                                    newItem.ID = Support_SQL.ToInt(dtreadcode.Rows[0]["ID"]);
                                                                    newItem.NameJig = Lib.ToString(dtreadcode.Rows[0][NameCol]);
                                                                    newItem.link = c_varGolbal.str_ConnectDBReadCode_BeforePlasma1;
                                                                    newItem.lstPcs = new List<string>();
                                                                    newItem.LotID = Lib.ToString(dtreadcode.Rows[0]["LotID"]);
                                                                    lst_ID_Jig.Add(newItem);
                                                                }
                                                            }

                                                        }
                                                        else if (c_varGolbal.UseMachine == 1)
                                                        {
                                                            dtreadcode = Support_SQL.GetTableDataReadCode(sql_readCode, c_varGolbal.str_ConnectDBReadCode_FVI);
                                                            //new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Infor, $"{c_varGolbal.str_ConnectDBReadCode_FVI}").ShowDialog();
                                                            if (dtreadcode.Rows.Count > 0)
                                                            {
                                                                ID_Jig newItem = new ID_Jig();
                                                                newItem.ID = Support_SQL.ToInt(dtreadcode.Rows[0]["ID"]);
                                                                newItem.NameJig = Lib.ToString(dtreadcode.Rows[0][NameCol]);
                                                                newItem.link = c_varGolbal.str_ConnectDBReadCode_FVI;
                                                                newItem.lstPcs = new List<string>();
                                                                newItem.LotID = Lib.ToString(dtreadcode.Rows[0]["LotID"]);
                                                                lst_ID_Jig.Add(newItem);
                                                            }
                                                        }

                                                        if (dtAllJig == null)
                                                        {
                                                            dtAllJig = dtreadcode.Clone();
                                                        }
                                                        if (dtreadcode.Rows.Count > 0)
                                                        {
                                                            int index = lst_ID_Jig.FindIndex(x => x.NameJig.Contains(item));

                                                            #region Kiểm tra những mã code nào đã có trên server 
                                                            List<string> listPcs = new List<string>();
                                                            listPcs = Lib.ToString(dtreadcode.Rows[0]["CodePCS"]).Split(',').ToList();
                                                            lst_ID_Jig[index].lstPcs = CheckDataHavePlasma(listPcs, lst_ID_Jig[index]);
                                                            if (lst_ID_Jig[index].lstPcs.Count > 0)
                                                            {
                                                                checkPcsExit = true;
                                                            }

                                                            #endregion
                                                            #region Kiểm tra xem có giống LotID không
                                                            if (!lst_ID_Jig[index].LotID.Contains(c_varGolbal.LotID))
                                                            {
                                                                checkLot = false;
                                                            }
                                                            #endregion

                                                            if (c_varGolbal.UseFvi)
                                                            {
                                                                int Nolock = Lib.ToInt(dtreadcode.Rows[0]["NoLock"]);
                                                                int QualifyJig = Lib.ToInt(dtreadcode.Rows[0]["QualifyJig"]);
                                                                if (Nolock != 1 || QualifyJig != 1)
                                                                {
                                                                    list_Lock_Qualify.Add(item);
                                                                }
                                                                else
                                                                {
                                                                    dtAllJig.Merge(dtreadcode);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dtAllJig.Merge(dtreadcode);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            list_Jig_False.Add(item); //Add vào List_false để thông báo những jig chưa qua plasma 
                                                        }
                                                    }
                                                    Lib.ShowLabelResult(Color.White, lbStatusReadJig, "show thông tin sản phẩm khi kiểm tra xong");
                                                    if (list_Jig_False.Count > 0 || (list_Lock_Qualify.Count > 0 && c_varGolbal.UseFvi) || checkPcsExit || !checkLot)
                                                    {
                                                        PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                                        //SET :TriggerHaveData
                                                        PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                                        if (list_Jig_False.Count > 0)
                                                        {
                                                            new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Không tìm thấy CodePCS của Jig\r\n {string.Join("\r\n", list_Jig_False)}").ShowDialog();
                                                        }
                                                        if (list_Lock_Qualify.Count > 0 && c_varGolbal.UseFvi)
                                                        {
                                                            new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, $"Những Jig đã bị Lock Hoặc NoQualify\r\n {string.Join("\r\n", list_Lock_Qualify)}").ShowDialog();
                                                        }
                                                        if (checkPcsExit)
                                                        {
                                                            foreach (ID_Jig item in lst_ID_Jig)
                                                            {
                                                                if (item.lstPcs.Count > 0)
                                                                {
                                                                    string strJoin = string.Join("\r\n", item.lstPcs);
                                                                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Jig {item.NameJig} có những Pcs đã có trên server\r\n{strJoin}").ShowDialog();
                                                                }
                                                            }
                                                        }
                                                        if (!checkLot)
                                                        {
                                                            foreach (ID_Jig item in lst_ID_Jig)
                                                            {
                                                                if (!item.LotID.Contains(c_varGolbal.LotID))
                                                                {
                                                                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Jig {item.NameJig}\r\nCó LotID là:{item.LotID} KHÁC với LotID hiện tại:{c_varGolbal.LotID}").ShowDialog();
                                                                }
                                                            }
                                                        }
                                                        Step_ReadTagPlasma = 0;//khi bị lỗi thiếu codePcs,check lot.....
                                                        list_Jig_False.Clear();
                                                        List_DataTagPlasmaInput.Clear();
                                                        List_DataCodeTray.Clear();
                                                        list_Lock_Qualify.Clear();
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Lib.ShowLabelResult(Color.White, lbStatusReadJig, "lưu dữ liệu vào máy và update trạng thái ở máy readcode");
                                                        int checkUpdate = 1;
                                                        for (int i = 0; i < dtAllJig.Rows.Count; i++)
                                                        {
                                                            string _tagJigTransfer = "";
                                                            if (c_varGolbal.GetJigHavePcs)
                                                            {
                                                                _tagJigTransfer = Support_SQL.ToString(dtAllJig.Rows[i]["Code_TagJigNonePCS"]);
                                                            }
                                                            else
                                                            {
                                                                _tagJigTransfer = Support_SQL.ToString(dtAllJig.Rows[i]["Code_TagJigHavePCS"]);
                                                            }
                                                            string codePCS_ = Support_SQL.ToString(dtAllJig.Rows[i]["CodePCS"]);
                                                            Support_SQL.SaveDataToBufferPlasma_new(ProgramID, PLasmaIndex, _tagJigTransfer, List_DataTagPlasmaInput[i], codePCS_, String.Join(",", List_DataCodeTray), c_varGolbal.LotID, c_varGolbal.MPN);
                                                            if (c_varGolbal.UseMachine == 2)
                                                            {
                                                                if (c_varGolbal.QtyBeforePlasma == 2)
                                                                {
                                                                    List<ID_Jig> Jig_Current = lst_ID_Jig.FindAll(x => x.ID == Support_SQL.ToInt(dtAllJig.Rows[i]["ID"]) && x.NameJig.Contains(dtAllJig.Rows[i][NameCol].ToString()));
                                                                    if (Jig_Current.Count > 0)
                                                                    {
                                                                        checkUpdate = Support_SQL.ExecuteQuery($"UPDATE readcode SET StatusUpload = true WHERE ID = {Jig_Current[0].ID}", Jig_Current[0].link);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    checkUpdate = Support_SQL.ExecuteQuery($"UPDATE readcode SET StatusUpload = true WHERE ID = {Support_SQL.ToInt(dtAllJig.Rows[i]["ID"])}", c_varGolbal.str_ConnectDBReadCode_BeforePlasma1);
                                                                }
                                                                Step_ReadTagPlasma = 30;//Next step 
                                                            }
                                                            else if (c_varGolbal.UseMachine == 1)
                                                            {
                                                                checkUpdate = Support_SQL.ExecuteQuery($"UPDATE readcode SET StatusUpload = true WHERE ID = {Support_SQL.ToInt(dtAllJig.Rows[i]["ID"])}", c_varGolbal.str_ConnectDBReadCode_FVI);
                                                                Step_ReadTagPlasma = 30;//Next step 
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //SET : TriggerError
                                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                                //SET :TriggerHaveData
                                                PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                                Step_ReadTagPlasma = 0;//end step khi có exception ở case 20
                                                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"{ex.ToString()}").ShowDialog();
                                                //throw;
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case 30:
                            {
                                //Đoc code và kiểm tra dữ liệu CodeJig thành công
                                //Gửi dữ liệu CodeJig cho PLC
                                Lib.ShowLabelResult(Color.White, lbStatusReadJig, "Gửi dữ liệu xuống plc");
                                string resultJoin = String.Join("/", List_DataTagPlasmaInput);
                                if (!MxComponent.WriteStringToPLC(PLC_Fx3Plasma, "D900", resultJoin.Length, resultJoin))
                                {
                                    //HA_Add
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 1);
                                    //SET :TriggerHaveData
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                    Lib.ShowError("Gửi giá trị xuống thanh ghi D900 bị lỗi\r\nHãy xác nhận để gửi lại ");
                                    List_DataTagPlasmaInput.Clear();
                                    List_DataCodeTray.Clear();
                                    Step_ReadTagPlasma = 0; //end step khi có lỗi gửi dữ liệu xuống plc
                                    break;
                                }
                                else
                                {
                                    OEE Processing = new OEE();
                                    Processing.SetParam(_DeviceID, JsonValueDE.stateProcess, "1", GlobVar.DateTimeIn);

                                    ExecuteWithTimeLimit(TimeSpan.FromSeconds(5), () =>
                                    {
                                        JsonFunc.OEEsubmit(Processing, out string msg1);
                                        Lib.SaveToLog("LogOEE", "", $"\r\nProcessing-{msg1}");
                                    });
                                    //SET : TriggerOK
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerOK, 1);
                                    //SET :TriggerHaveData
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 1);
                                    List_DataTagPlasmaInput.Clear();
                                    List_DataCodeTray.Clear();
                                    Step_ReadTagPlasma = 0; //end step khi hoàn thành
                                }
                                break;
                            }
                        default:
                            {
                                // Quét liên tục giá trị Trigger ở đầu vào
                                Lib.EnableLabel(lbStatusReadJig, false);
                                //GET: TriggerHaveDataOK
                                PLC_Fx3Plasma.GetDevice(c_varGolbal.TriggerHaveDataOK, out int Value_TriggerHaveDataOK);
                                PLC_Fx3Plasma.GetDevice("M710", out int Value_TriggerProcessPLC);
                                if (Value_TriggerHaveDataOK == 1 || Value_TriggerProcessPLC == 0)
                                {
                                    //SET :Reset  TriggerHaveData,TriggerError,TriggerOK
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerHaveData, 0);
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerOK, 0);
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerError, 0);

                                }
                                //GET :TriggerReadCode
                                PLC_Fx3Plasma.GetDevice(c_varGolbal.TriggerReadCode, out int Bit_TriggerPlasma);
                                if (Bit_TriggerPlasma == 1)
                                {
                                    //Lib.EnableLabel(lbStatusReadJig, true);
                                    //SET :TriggerReadCodeOK
                                    step = 0;
                                    List_DataTagPlasmaInput = new List<string>();
                                    List_DataCodeTray.Clear();
                                    PLC_Fx3Plasma.SetDevice(c_varGolbal.TriggerReadCodeOK, 1);
                                    Step_ReadTagPlasma = 10; // Chạy chu trình đọc dữ liệu tag ở đầu vào
                                }

                                break;
                            }
                    }
                }
                catch (ThreadAbortException Ex)
                {
                    //
                }
            }
        }
        /// <summary>
        /// Function kiểm tra jig vào máy plasma đã sau 15p hay chưa
        /// </summary>
        public List<string> checkDateTimePlasma(List<string> lstDataReadTagInput)
        {
            List<string> lstTagJigNotEnoughTime = new List<string>();
            List<dataPlasma> lstDataPlasmaNG = new List<dataPlasma>();
            string strDateTimeInPlasma = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int TimeCheck;
            for (int i = 0; i < lstDataReadTagInput.Count; i++)
            {
                try
                {
                    string cmdStr = $"SELECT * FROM Plasma WHERE TagJigPlasma = '{lstDataReadTagInput[i]}' AND Date_Time NOT IN('') AND CompletePlasma=1 ORDER by Date_Time DESC LIMIT 1;";
                    DataTable dt = Support_SQL.GetTableDataPlasma(cmdStr);
                    if (dt.Rows.Count > 0)
                    {
                        ///code kiểm tra thời gian của JIG 
                        string strDateTimeOutPlasma = dt.Rows[0]["Date_Time"].ToString();
                        DateTime dateTimeInPlasma = DateTime.ParseExact(strDateTimeInPlasma, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dateTimeOutPlasma = DateTime.ParseExact(strDateTimeOutPlasma, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        TimeSpan timeSpan = dateTimeInPlasma - dateTimeOutPlasma;
                        //TimeSpan timeSpan = dateTimeOutPlasma - dateTimeInPlasma;
                        TimeCheck = Int32.Parse(timeSpan.TotalSeconds.ToString());
                        if (TimeCheck <= c_varGolbal.TimeRepeatJig * 60)
                        {
                            lstTagJigNotEnoughTime.Add(lstDataReadTagInput[i]);
                            dataPlasma data = new dataPlasma();
                            data.TagJigTransfer = "";
                            data.TagJigPlasma = lstDataReadTagInput[i];
                            data.PcsBarcode = "";
                            data.CodeTray = string.Join(",", List_DataCodeTray);
                            data.DateTimeInPlasma = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            data.DateTimeOutPlasma = "N/A";
                            data.StatusPlasma = "NG";
                            data.CycleTime = "N/A";
                            lstDataPlasmaNG.Add(data);
                        }
                        else
                        {
                            //Support_SQL.ClearRecordTagFinish(ProgramID, lstDataReadTagInput[i]);//xóa dữ liệu JIG
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            if (lstDataPlasmaNG.Count > 0)
            {
                Upload_DB_Excel(lstDataPlasmaNG);
            }
            return lstTagJigNotEnoughTime;
        }
        #endregion



        #region Process Upload Data To Sever
        /// <summary>
        /// 
        /// Luồng chạy process upload dữ liệu lên Sever
        /// </summary>
        public void MainThreadUpdateDataPlasma()
        {
            Main_UpDateDataPlasma = new Thread(new ThreadStart(ProcessUpdateDataPlasma));
            Main_UpDateDataPlasma.IsBackground = true;
            Main_UpDateDataPlasma.Start();
        }
        public void Stop_MainThreadUpdateDataPlasma()
        {
            if (Main_UpDateDataPlasma != null)
            {
                Main_UpDateDataPlasma.Abort();
            }
        }
        int Step_UploadDataPlasma = 0;

        string CodePosition_3_Update = "";
        private List<string> List_CodeJig = new List<string>();
        private List<ID_Jig> lst_Jig_Upload = new List<ID_Jig>();
        bool uploadDataPlasma = false;
        bool checkUpdate = false;

        /// <summary>
        /// Process upload dữ liệu lên Sever
        /// </summary>
        private void ProcessUpdateDataPlasma()
        {
            ///reset trigger
            //SET: Reset Trigger lấy DataOK
            PLC_Fx3Plasma.SetDevice("M133", 0);
            //SET: Reset Trigger UploadData -> Server 
            PLC_Fx3Plasma.SetDevice("M135", 0);
            while (c_varGolbal.IsRun)  //Thread Upload server
            {

                Thread.Sleep(100);
                try
                {
                    switch (Step_UploadDataPlasma)
                    {
                        case 10:
                            {
                                //List<dataPlasma> List_dataPlasma = new List<dataPlasma>();
                                lst_dataPlasma = new List<dataPlasma>();
                                if (lst_Jig_Upload.Count > 0)
                                {
                                    Lib.EnableLabel(lbStatusUpload, true);
                                    Lib.ShowLabelResult(Color.Orange, lbStatusUpload, "Đang upload dữ liệu lên server...");

                                    #region Upload Data Plasma 
                                    foreach (var JIG in lst_Jig_Upload)
                                    {
                                        //WaitWndFun frm_Wait_Upload = new WaitWndFun();
                                        checkUpdate = false;
                                        try
                                        {
                                            //HuyNV Thêm Điều kiện StateUploadServer ISNULL vào 
                                            string stringLoadPlasma = $"Select * from Plasma where ID={JIG.ID} AND ID_Program={ProgramID} AND ID_Plasma={PLasmaIndex} AND CompletePlasma=0 AND CompleteBoxing=0 ORDER by ID DESC LIMIT 1 ";
                                            DataTable dtPlasma = Support_SQL.GetTableDataPlasma(stringLoadPlasma);

                                            if (dtPlasma.Rows.Count <= 0 || dtPlasma == null)
                                            {
                                                Lib.SaveToLog("ErrorSQLite", $"{JIG.NameJig} - Process Upload Data (Case 10)", $"{stringLoadPlasma}");
                                                continue;
                                            }
                                            else
                                            {
                                                dataPlasma data = new dataPlasma();
                                                data.ID = Lib.ToInt(dtPlasma.Rows[0]["ID"]);
                                                data.TagJigPlasma = Lib.ToString(dtPlasma.Rows[0]["TagJigPlasma"]);
                                                data.TagJigTransfer = Lib.ToString(dtPlasma.Rows[0]["TagJigTransfer"]);
                                                data.PcsBarcode = Support_SQL.ToString(dtPlasma.Rows[0]["CodePCS"]);
                                                data.StatusPlasma = Lib.ToString(dtPlasma.Rows[0]["StateUploadServer"]);
                                                data.CodeTray = Lib.ToString(dtPlasma.Rows[0]["CodeTray"]);

                                                //HA-04032023
                                                data.DateTimeInPlasma = Lib.ToString(dtPlasma.Rows[0]["DateTimeInPlasma"]);
                                                try
                                                {
                                                    data.DateTimeOutPlasma = Lib.ToString(dtPlasma.Rows[0]["DateTimeOutPlasma"]);
                                                    data.CycleTime = Lib.ToString(dtPlasma.Rows[0]["Cycletime"]);

                                                }
                                                catch (Exception ex)
                                                {
                                                    data.DateTimeOutPlasma = Lib.ToString(Lib.ToDate(data.DateTimeInPlasma).AddSeconds(44));
                                                    data.CycleTime = 44 + "";
                                                }

                                                GlobVar.DateTimeOut = data.DateTimeOutPlasma;
                                                GlobVar.TimeCT = data.CycleTime;

                                                //////////-------------------------------------------------------------------------------------------------

                                                checkUpdate = true;
                                                List<string> List_DataCodePcs = data.PcsBarcode.Replace("NoRead", "").Split(',').ToList();
                                                DateTime start = DateTime.Now;
                                                DateTime stop = new DateTime();
                                                string ResultLog = "";
                                                bool reUploadServer = false;
                                                List<string> lstCheckReUpload = new List<string>();
                                                uploadDataPlasma = false;
                                                do
                                                {
                                                    //if (frm_Wait_Upload != null) frm_Wait_Upload.Show(Parent,"Upload Server");
                                                    start = DateTime.Now;
                                                    //ImgLoad.Show();
                                                    int Completeplasma = Lib.ToInt(Support_SQL.ExecuteScalarDBPlasma($"Select CompletePlasma from Plasma where ID={data.ID} ORDER by ID DESC LIMIT 1"));//kiểm tra trong db là data đã upload chưa
                                                    if (Completeplasma == 1)
                                                    {
                                                        uploadDataPlasma = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Lib.ShowLabelResult(Color.Orange, lbStatusUpload, "Đang upload data lên server MMCV...");

                                                        #region kiểm tra và gửi data lên server, sẽ gửi lại nếu lỗi
                                                        if (reUploadServer)
                                                        {
                                                            lstCheckReUpload = CheckDataHavePlasma(List_DataCodePcs, JIG);
                                                            if (lstCheckReUpload.Contains(GlobVar.Error56))
                                                            {
                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                if (lstCheckReUpload.Count < List_DataCodePcs.Count && lstCheckReUpload.Count >= 1)
                                                                {
                                                                    string strJoin = string.Join("\r\n", lstCheckReUpload);
                                                                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Jig {data.TagJigPlasma} có những Pcs đã có trên server\r\n{strJoin}").ShowDialog();
                                                                    uploadDataPlasma = true;
                                                                }
                                                                else if (lstCheckReUpload.Count == List_DataCodePcs.Count)
                                                                {
                                                                    uploadDataPlasma = true;
                                                                }
                                                                else if (lstCheckReUpload.Count <= 0)
                                                                {
                                                                    uploadDataPlasma = Upload_DataPlasma(_LineID, _DeviceID, c_varGolbal.MPN, c_varGolbal.StaffID, c_varGolbal.LotID, List_DataCodePcs, data.TagJigPlasma, data.CodeTray, ref ResultLog);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            uploadDataPlasma = Upload_DataPlasma(_LineID, _DeviceID, c_varGolbal.MPN, c_varGolbal.StaffID, c_varGolbal.LotID, List_DataCodePcs, data.TagJigPlasma, data.CodeTray, ref ResultLog);
                                                        }
                                                        #endregion

                                                        #region Lưu log các dữ liệu đã gửi lên server
                                                        stop = DateTime.Now;
                                                        TimeSpan kq = stop - start;
                                                        int LengthSpace = start.ToString("HH:mm:ss").Length;
                                                        string strSpace = Lib.CreateString(LengthSpace, " ");
                                                        if (reUploadServer)
                                                        {
                                                            if (lstCheckReUpload.Count == List_DataCodePcs.Count)
                                                            {
                                                                Lib.SaveToLog(NameFileLog_Main, start, stop, $"Send:Reupload PlasmaReading ({string.Join(",", List_DataCodePcs)})\r\n{strSpace} LotID:{c_varGolbal.LotID}", $"Receive: (Số Pcs check bằng số lượng upload - {string.Join(",", lstCheckReUpload)})\r\nStatus:{uploadDataPlasma}-StatusPlasma:{Completeplasma}", data.TagJigPlasma, kq.TotalSeconds);
                                                            }
                                                            else
                                                            {
                                                                Lib.SaveToLog(NameFileLog_Main, start, stop, $"Send:Reupload PlasmaReading ({string.Join(",", List_DataCodePcs)})\r\n{strSpace} LotID:{c_varGolbal.LotID}", $"Receive: ({ResultLog})\r\nStatus:{uploadDataPlasma}-StatusPlasma:{Completeplasma}", data.TagJigPlasma, kq.TotalSeconds);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            Lib.SaveToLog(NameFileLog_Main, start, stop, $"Send:PlasmaReading ({string.Join(",", List_DataCodePcs)})\r\n{strSpace} LotID:{c_varGolbal.LotID}", $"Receive: ({ResultLog})\r\nStatus:{uploadDataPlasma}-StatusPlasma:{Completeplasma}", data.TagJigPlasma, kq.TotalSeconds);
                                                        }
                                                        #endregion

                                                        bool checkSave = false;
                                                        if (uploadDataPlasma)
                                                        {
                                                            do
                                                            {
                                                                if (Lib.ToInt(Support_SQL.SaveStateUploadDataServer(ProgramID, PLasmaIndex, data.ID, data.TagJigPlasma, "OK", 1, 0)) == 1)
                                                                {
                                                                    checkSave = true;
                                                                    break;
                                                                }
                                                            }
                                                            while (!checkSave);
                                                            data.StatusPlasma = "OK";
                                                        }
                                                        else
                                                        {
                                                            reUploadServer = true;
                                                            new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Fail Upload Data Plasma To Server!", 3000).ShowDialog();
                                                        }
                                                    }
                                                }
                                                while (!uploadDataPlasma && c_varGolbal.IsRun);
                                                lst_dataPlasma.Add(data);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //if (frm_Wait_Upload != null) frm_Wait_Upload.Close();
                                            checkUpdate = false;
                                            Lib.SaveToLog("ExceptionProcessUploadServer", "", ex.ToString());
                                            MessageBox.Show(ex.Message);
                                        }

                                    } 
                                    #endregion

                                    #region Upload OEE 
                                    if (List_CodeJig.Count > 0)
                                    {

                                        OEE WAIT = new OEE();
                                        WAIT.SetParam(_DeviceID, JsonValueDE.stateWait, "1", GlobVar.DateTimeOut);

                                        OEE CT = new OEE();
                                        CT.SetParam(_DeviceID, JsonValueDE.stateCT, GlobVar.TimeCT, GlobVar.DateTimeOut);

                                        ExecuteWithTimeLimit(TimeSpan.FromSeconds(5), () =>
                                        {
                                            //JsonFunc.OEEsubmit(Processing, out string msg1);
                                            JsonFunc.OEEsubmit(WAIT, out string msg2);
                                            JsonFunc.OEEsubmit(CT, out string msg3);
                                            Lib.SaveToLog("LogOEE", "", $"\r\nWait-{msg2}\r\nCT-{msg3}");
                                        });


                                    }
                                    #endregion

                                    #region Hiển thị lên grid

                                    if (checkUpdate && lst_dataPlasma.Count > 0)
                                    {
                                        Upload_DB_Excel(lst_dataPlasma);
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            // Hiển thị dữ liệu lên data grid view plasma
                                            showgrdData_ViewTags(lst_dataPlasma);
                                        }));
                                    }
                                    #endregion

                                    #region kiểm tra máy boxing đã xử lý xong chưa mới xử lý tiếp

                                    #region bỏ
                                    //DataTable tableBoxingPlate = new DataTable();
                                    //bool repeat = false;
                                    //do
                                    //{
                                    //    bool status = false;
                                    //    tableBoxingPlate = Support_SQL.GetTableData($"SELECT *FROM TempPlate", GlobVar.PathFileBoxing, ref status);
                                    //    if (status)
                                    //    {
                                    //        repeat = tableBoxingPlate.Rows.Count > 0 ? true : false;
                                    //    }
                                    //    else
                                    //    {
                                    //        repeat = true;
                                    //    }
                                    //    if (repeat)
                                    //    {
                                    //        Lib.ShowLabelResult(Color.Orange, lbStatusUpload, "Đang chờ đọc xong plate ở máy Boxing...");
                                    //    }
                                    //}
                                    //while (repeat);
                                    //Lib.ShowLabelResult(Color.Green, lbStatusUpload, "Hoàn thành");
                                    //Lib.EnableLabel(lbStatusUpload, false); 
                                    #endregion

                                    DataTable dtcheckDone = new DataTable();
                                    bool repeat = false;
                                    List<int> lstID = new List<int>();
                                    foreach (var item in lst_Jig_Upload)
                                    {
                                        lstID.Add(item.ID);
                                    }
                                    string strLstId = string.Join(",", lstID);
                                    do
                                    {
                                        bool status = false;
                                        dtcheckDone = Support_SQL.GetTableDataPlasma($"SELECT *FROM Plasma WHERE ID NOT in ({strLstId}) AND LotID='{c_varGolbal.LotID}' AND CompletePlasma=1 AND CompleteBoxing=0;", ref status);
                                        if (status)
                                        {
                                            repeat = dtcheckDone.Rows.Count > 0 ? true : false;
                                        }
                                        else
                                        {
                                            repeat = true;
                                        }
                                        if (repeat)
                                        {
                                            Lib.ShowLabelResult(Color.Orange, lbStatusUpload, "Đang chờ đọc xong plate ở máy Boxing...");
                                            Thread.Sleep(30);
                                        }
                                    }
                                    while (repeat);
                                    Lib.ShowLabelResult(Color.Green, lbStatusUpload, "Hoàn thành");
                                    Lib.EnableLabel(lbStatusUpload, false);
                                    Step_UploadDataPlasma = 20;// sang step end đẩy jig ra ngoài
                                    #endregion
                                }
                                else
                                {

                                }
                            }

                            break;

                        case 20:
                            {
                                try
                                {
                                    PLC_Fx3Plasma.GetDevice("M135", out int M135);
                                    PLC_Fx3Plasma.GetDevice("M135", out int M136);
                                    if (M135 == 0)
                                    {
                                        //SET: Reset Trigger lấy DataOK
                                        PLC_Fx3Plasma.SetDevice("M133", 0);
                                        //SET: Trigger UploadData -> Server 
                                        PLC_Fx3Plasma.SetDevice("M135", 1);
                                        //Reset List DataPlasma
                                        lst_dataPlasma.Clear();
                                        //Reset List_CodeJig
                                        List_CodeJig.Clear();
                                        Thread.Sleep(100);
                                        PLC_Fx3Plasma.GetDevice("M135", out M135);
                                        if (M135 == 1)
                                        {
                                            // SET: Reset Trigger lấy DataOK
                                            PLC_Fx3Plasma.SetDevice("M133", 0);
                                            //SET: Trigger UploadData -> Server 
                                            PLC_Fx3Plasma.SetDevice("M135", 0);
                                        }
                                        Step_UploadDataPlasma = 0;//End step khi ở case 20
                                    }
                                }
                                catch (Exception ex)
                                {

                                    throw;
                                }
                                break;
                            }
                        default:
                            {
                                #region Bỏ
                                ////Lấy Trigger Úp lồng xuống để Reset Trigger DataOK
                                //PLC_Fx3Plasma.GetDevice("M130", out int Trigger_down);
                                ////M910 bit Cancel
                                //PLC_Fx3Plasma.GetDevice("M910", out int Trigger_cancel);
                                //PLC_Fx3Plasma.GetDevice("M136", out int Trigger_ULSOK);
                                //if (Trigger_down == 1 || Trigger_ULSOK == 1 || Trigger_cancel == 0)
                                //{
                                //    if (Trigger_down == 1)
                                //    {
                                //        //SET: Trigger_downOK
                                //        PLC_Fx3Plasma.SetDevice("M131", 1);
                                //    }
                                //    //SET: Reset Trigger lấy DataOK
                                //    PLC_Fx3Plasma.SetDevice("M133", 0);
                                //    //SET: Reset Trigger UploadData -> Server 
                                //    PLC_Fx3Plasma.SetDevice("M135", 0);
                                //}
                                //if (Trigger_down == 0)
                                //{
                                //    //SET: Trigger_downOK
                                //    PLC_Fx3Plasma.SetDevice("M131", 0);
                                //} 
                                #endregion

                                //Lấy Trigger Mở lồng lên lấy dữ liệu update vào DB Plama
                                PLC_Fx3Plasma.GetDevice("M133", out int M133);
                                PLC_Fx3Plasma.GetDevice("M133", out int M135);
                                if (M133 == 0 && M135 == 0)
                                {
                                    PLC_Fx3Plasma.GetDevice("M132", out int Trigger_Up);//đọc thành công set M133 lên 1
                                    if (Trigger_Up == 1)
                                    {

                                        //Lấy Giá Trị Code Tại vị trí máy Plasma từ PLC
                                        MxComponent.ReadStringPLC(PLC_Fx3Plasma, "D1200", 90, out CodePosition_3_Update);
                                        string codeJig = CodePosition_3_Update.Replace("\0", "").Replace("\r", "").Replace("\n", "");


                                        //Lấy Giá trị thời gian khi máy úp xuống và mở lên từ PLC
                                        MxComponent.ReadIntPLC(PLC_Fx3Plasma, "D777", 1, out int time);
                                        time = time / 10;
                                        string cycletime = time + "";


                                        if (time <= 30)
                                        {
                                            //SET : TriggerError
                                            PLC_Fx3Plasma.SetDevice("M104", 1);
                                            //SET: Trigger_UPOK && Lấy Data từ PLC OK
                                            PLC_Fx3Plasma.SetDevice("M133", 1);
                                            new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Jig {codeJig}\r\nThời gian trong lồng chưa đủ mới được {time} s").ShowDialog();
                                            Lib.SaveToLog("ErrorWithPLC", $"{codeJig}", $"LỖI CYCLE TIME - GIÁ TRỊ THỰC={time}");
                                            Step_UploadDataPlasma = 0;//EndStep khi time<=30
                                            continue;
                                        }
                                        else
                                        {
                                            List_CodeJig = new List<string>();
                                            lst_Jig_Upload = new List<ID_Jig>();
                                            List<string>lst_temp= codeJig.Split('/').Distinct().ToList().FindAll(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrEmpty(x));
                                            List_CodeJig = codeJig.Split('/').Distinct().ToList();
                                            List_CodeJig = List_CodeJig.FindAll(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrEmpty(x));
                                            if (List_CodeJig.Count > 0)
                                            {
                                                for (int i = 0; i < List_CodeJig.Count; i++)
                                                {
                                                    List_CodeJig[i] = List_CodeJig[i].Trim();
                                                }
                                                List_CodeJig = List_CodeJig.Distinct().ToList();
                                            }
                                            if (List_CodeJig.Count != lst_temp.Count)
                                            {
                                                Lib.SaveToLog("KTDoubleJig", string.Join(",", lst_temp), " ");
                                            }
                                            //HA-04032023
                                            if (List_CodeJig.Count <= 0)
                                            {
                                                Lib.SaveToLog("ErrorWithPLC", "List_CodeJig nhỏ hơn 0", $"Kết quả lấy từ plc:{codeJig}");
                                                Step_UploadDataPlasma = 0;//end step khi ko có dữ liệu plc
                                                continue;
                                            }
                                            else if (List_CodeJig.Count > 0)
                                            {
                                                DateTime datetimeOUT = DateTime.Now;
                                                DateTime datetimeIN = datetimeOUT - TimeSpan.FromSeconds(Lib.ToDouble(cycletime));
                                                string IN = datetimeIN.ToString("yyyy-MM-dd HH:mm:ss");
                                                string OUT = datetimeOUT.ToString("yyyy-MM-dd HH:mm:ss");
                                                bool check = true;
                                                foreach (var item in List_CodeJig)
                                                {
                                                    string select = $"SELECT ID FROM Plasma WHERE TagJigPlasma = '{item.Trim()}' AND ID_Program={ProgramID} AND CompletePlasma=0 AND CompleteBoxing=0 AND LotID='{c_varGolbal.LotID}' ORDER BY ID DESC LIMIT 1";
                                                    int ID = Lib.ToInt(Support_SQL.ExecuteScalarDBPlasma(select));

                                                    if (ID > 0)
                                                    {
                                                        if (cycletime == "0")
                                                        {
                                                            string sqlselect = $"SELECT * FROM Plasma WHERE ID={ID} ORDER BY ID DESC LIMIT 1";
                                                            DataTable dt = Support_SQL.GetTableDataPlasma(sqlselect);
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                cycletime = "43";
                                                                datetimeIN = datetimeOUT - TimeSpan.FromSeconds(Lib.ToDouble(cycletime));
                                                                IN = datetimeIN.ToString("yyyy-MM-dd HH:mm:ss");
                                                                Support_SQL.UpdateDateTimePlasma(ProgramID, PLasmaIndex, ID, item.Trim(), OUT, IN, cycletime);
                                                                lst_Jig_Upload.Add(new ID_Jig { ID = ID, NameJig = item.Trim(), LotID = c_varGolbal.LotID });
                                                            }
                                                            else
                                                            {
                                                                Lib.SaveToLog("ErrorSaveOut", $"ID Jig đó là:", $"{ID}");
                                                                Lib.SaveToLog("ErrorSaveOut", $"Câu lệch truy vấn là:", $"{select}");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string sqlselect = $"SELECT * FROM Plasma WHERE ID={ID} ORDER BY ID DESC LIMIT 1";
                                                            DataTable dt = Support_SQL.GetTableDataPlasma(sqlselect);
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                Support_SQL.UpdateDateTimePlasma(ProgramID, PLasmaIndex, ID, item.Trim(), OUT, IN, cycletime);
                                                                lst_Jig_Upload.Add(new ID_Jig { ID = ID, NameJig = item.Trim(), LotID = c_varGolbal.LotID });
                                                            }
                                                            else
                                                            {
                                                                Lib.SaveToLog("ErrorSaveOut", $"ID Jig đó là:", $"{ID}");
                                                                Lib.SaveToLog("ErrorSaveOut", $"Câu lệch truy vấn là:", $"{select}");
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        Lib.SaveToLog("ErrorUploadData_FindJig", $"Câu lệch truy vấn là:", $"{select}");
                                                        check = false;
                                                    }
                                                }
                                                if (check)
                                                {
                                                    Step_UploadDataPlasma = 10;//sang step upload data server
                                                    //SET: Trigger_UPOK && Lấy Data từ PLC OK
                                                    PLC_Fx3Plasma.SetDevice("M133", 1);
                                                }
                                                else
                                                {
                                                    Step_UploadDataPlasma = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PLC_Fx3Plasma.SetDevice("M133", 0);
                                    PLC_Fx3Plasma.SetDevice("M135", 0);
                                }
                                break;
                            }

                    }
                }
                catch (ThreadAbortException Ex)
                {
                    //
                }
            }
        }
        #endregion



        #region ProcessDisplayDataPlasma

        Thread Main_DisplayDataPlasma;
        /// <summary>
        /// Luồng chạy hiển thị dữ liệu Tag cho User control Plasma
        /// </summary>
        public void MainThreadDisplayDataPlasma()
        {
            Main_DisplayDataPlasma = new Thread(new ThreadStart(ProcessDisplayDataPlasma));
            Main_DisplayDataPlasma.IsBackground = true;
            Main_DisplayDataPlasma.Start();
        }

        string CodePosition_1_Current = "";
        string CodePosition_1_Old = "";
        string CodePosition_2_Current = "";
        string CodePosition_2_Old = "";
        string CodePosition_3_Current = "";
        string CodePosition_3_Old = "";
        string CodePosition_4_Current = "";
        string CodePosition_4_Old = "";
        //HuyNV 
        int p1_old = 0;
        int p2_old = 0;
        int p3_old = 0;
        int p4_old = 0;
        /// <summary>
        /// Hiển thị dữ liệu của Code TagJig ở vị trí 2 
        /// </summary>
        private void ProcessDisplayDataPlasma()
        {

            while (c_varGolbal.IsRun) //Thread View vị trí 1,2,3,4
            {
                Thread.Sleep(100);
                try
                {
                    Thread.Sleep(100);
                    //Position 1
                    PLC_Fx3Plasma.GetDevice("M150", out int p1);//Trigger có dữ liệu tại vị trí 1
                    if (p1 != p1_old)
                    {
                        if (p1 == 1)
                        {
                            p1_old = p1;
                            MxComponent.ReadStringPLC(PLC_Fx3Plasma, "D1000", 90, out CodePosition_1_Current);


                            string v1 = CodePosition_1_Current.Replace("\0", "").Replace("\r", "").Replace("\n", "");

                            //HuyNV
                            if (v1 != CodePosition_1_Old)
                            {
                                CodePosition_1_Old = v1;
                                DisplayPositionTray(lb_infoJigInput, v1);
                            }
                        }
                        else
                        {
                            p1_old = p1;
                            DisplayPositionTray(lb_infoJigInput, "");
                        }

                    }
                    //trạng thái Auto hay Manual
                    //GET :Trigger Auto 
                    PLC_Fx3Plasma.GetDevice(bitAutoManual, out int IsAuto);
                    if (IsAuto == 1)
                    {
                        //Reset lại dữ liệu trên màn hình
                    }
                }
                catch (Exception ex)
                {

                }

                try
                {
                    //Position 2
                    PLC_Fx3Plasma.GetDevice("M151", out int p2);//Trigger có dữ liệu tại vị trí 2
                    if (p2 != p2_old)
                    {
                        if (p2 == 1)
                        {
                            p2_old = p2;
                            MxComponent.ReadStringPLC(PLC_Fx3Plasma, "D1100", 90, out CodePosition_2_Current);
                            string v2 = CodePosition_2_Current.Replace("\0", "").Replace("\r", "").Replace("\n", "");
                            //HuyNV
                            if (v2 != CodePosition_2_Old)
                            {
                                CodePosition_2_Old = v2;
                                DisplayPositionTray(lb_infoJigInput_Wait, v2);
                            }
                        }
                        else
                        {
                            p2_old = p2;
                            DisplayPositionTray(lb_infoJigInput_Wait, "");
                        }
                    }
                }
                catch (ThreadAbortException Ex)
                {

                }

                try
                {
                    //Position 3
                    PLC_Fx3Plasma.GetDevice("M152", out int p3);//Trigger có dữ liệu tại vị trí 3
                    if (p3 != p3_old)
                    {
                        if (p3 == 1)
                        {
                            p3_old = p3;
                            MxComponent.ReadStringPLC(PLC_Fx3Plasma, "D1200", 90, out CodePosition_3_Current);
                            string v3 = CodePosition_3_Current.Replace("\0", "").Replace("\r", "").Replace("\n", "");

                            if (v3 != CodePosition_3_Old)
                            {
                                CodePosition_3_Old = v3;
                                DisplayPositionTray(lb_infoJigMachine, v3);
                            }
                        }
                        else
                        {
                            p3_old = p3;
                            DisplayPositionTray(lb_infoJigMachine, "");
                        }

                    }
                }
                catch (ThreadAbortException Ex)
                {

                }

                try
                {
                    //Position 4
                    PLC_Fx3Plasma.GetDevice("M153", out int p4);//Trigger có dữ liệu tại vị trí 4
                    if (p4 != p4_old)
                    {
                        if (p4 == 1)
                        {
                            p4_old = p4;
                            MxComponent.ReadStringPLC(PLC_Fx3Plasma, "D1300", 90, out CodePosition_4_Current);
                            string v4 = CodePosition_4_Current.Replace("\0", "").Replace("\r", "").Replace("\n", "");
                            //HuyNV
                            if (v4 != CodePosition_4_Old)
                            {
                                CodePosition_4_Old = v4;
                                DisplayPositionTray(lb_infoJigOutput, v4);
                            }
                        }
                        else
                        {
                            p4_old = p4;
                            DisplayPositionTray(lb_infoJigOutput, "");
                        }

                    }
                }
                catch (ThreadAbortException Ex)
                {

                }
            }
        }

        public void InitValue()
        {
            p1_old = 0;
            p2_old = 0;
            p3_old = 0;
            p4_old = 0;
        }

        public void DisplayPositionTray(Label lb, string values)
        {
            //if(values=="")
            //{

            //}    
            if (lb.InvokeRequired)
            {
                lb.Invoke(new Action(() =>
                {
                    lb.Text = Formast_String(values);
                    DisplayColorLabel(lb, Color.Lime);
                }));
            }
            else
            {
                lb.Text = Formast_String(values);
                DisplayColorLabel(lb, Color.Lime);
            }
        }
        #endregion

        #region Process Reset Data
        /// <summary>
        /// Luồng chạy reset
        /// </summary>
        public void MainThreadReset()
        {
            Main_ResetPlasma = new Thread(ProcessResetPlasma);
            Main_ResetPlasma.IsBackground = true;
            Main_ResetPlasma.Start();
        }
        public void Stop_ThreadAll()
        {
            try
            {
                if (Main_ReadTagPlasma != null)
                {
                    Main_ReadTagPlasma.Abort();
                }
            }
            catch (Exception ex) { }

            try
            {
                if (Main_UpDateDataPlasma != null)
                {
                    Main_UpDateDataPlasma?.Abort();
                }

                Main_ReadTagPlasma?.Abort();
            }
            catch (Exception ex) { }

            try
            {
                if (Main_DisplayDataPlasma != null)
                {
                    Main_DisplayDataPlasma?.Abort();
                }
            }
            catch (Exception ex) { }

            //Main_ThreadAlive_PLC?.Abort();
            //Main_ResetPlasma?.Abort();


        }
        static readonly object lockSave_1_to_2 = new object();
        static readonly object lockSave_2_to_3 = new object();


       
        /// <summary>
        /// Process reset plasma
        /// </summary>
        private void ProcessResetPlasma()
        {

        }
        #endregion

        #region Process UploadDataWaiting
        /// <summary>
        /// Luồng upload những dữ liệu máy plasma bị Wait
        /// </summary>
        public void MainThreadUploadDataWaiting()
        {
            Main_AutoUploadDataWait = new Thread(ProcessUploadDataNONE);
            Main_AutoUploadDataWait.IsBackground = true;
            Main_AutoUploadDataWait.Start();
        }

        private void ProcessUploadDataNONE()
        {
            while (c_varGolbal.IsRun)  //Thread Upload data Waiting
            {
                Thread.Sleep(6000);
                if (!c_varGolbal.IsRun) return;
                try
                {
                    string Select_Jig_None = $"SELECT *FROM Plasma WHERE ID_Program={ProgramID} AND ID_Plasma={PLasmaIndex} AND StateUploadServer='WAITING'";
                    DataTable dt = Support_SQL.GetTableDataPlasma(Select_Jig_None);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            string tempCodePCS;
                            List<string> ListDataCodePCS = new List<string>();
                            dataPlasma data = new dataPlasma();
                            data.ID = Lib.ToInt(item["ID"]);
                            data.TagJigTransfer = item["TagJigTransfer"].ToString();
                            data.TagJigPlasma = item["TagJigPlasma"].ToString();
                            data.PcsBarcode = item["CodePCS"].ToString();
                            data.CodeTray = item["CodeTray"].ToString();
                            data.DateTimeInPlasma = item["DateTimeInPlasma"].ToString();
                            data.StatusPlasma = "OK";
                            data.CycleTime = item["Cycletime"].ToString();
                            tempCodePCS = item["CodePCS"].ToString();
                            tempCodePCS = tempCodePCS.Replace("NoRead", "");
                            ListDataCodePCS = tempCodePCS.Split(',').ToList();
                            string LotID = item["LotID"].ToString().Trim();
                            string MPN = item["MPN"].ToString().Trim();
                            List<string> lstUpload = new List<string>();
                            ID_Jig jig = new ID_Jig(data.TagJigPlasma, LotID);
                            #region Kiểm tra những mã code nào đã có trên server 

                            lstUpload = CheckNoDataPlasma(ListDataCodePCS, jig);
                            #endregion
                            if (lstUpload.Contains(GlobVar.Error56))
                            {
                                continue;
                            }
                            if (lstUpload.Count > 0)
                            {
                                string strJoin = string.Join(",", lstUpload);
                                //Lib.SaveToLog("UploadDataWait", $"{data.TagJigPlasma}-{LotID}", strJoin);
                                string ResultLog = "";
                                DateTime start = DateTime.Now;
                                if (Upload_DataPlasma(_LineID, _DeviceID, MPN, c_varGolbal.StaffID, LotID, lstUpload, data.TagJigPlasma, data.CodeTray, ref ResultLog))
                                {
                                    Support_SQL.SaveStateUploadDataServer(ProgramID, PLasmaIndex, data.ID, data.TagJigPlasma, "OK");
                                    this.Invoke(new Action(delegate
                                    {
                                        grvViewTag.BeginUpdate();
                                        grvViewTag.EndUpdate();
                                    }));
                                }
                                DateTime stop = DateTime.Now;
                                TimeSpan kq = stop - start;
                                string strSpace = Lib.CreateString(start.ToString("HH:mm:ss").Length, " ");
                                Lib.SaveToLog(NameFileLog_ProcessAuto, start, stop, $"Send:PlasmaReading ({ strJoin})\r\n{strSpace} LotID:{LotID}", $"Receive: ({ResultLog})", $"Jig:{data.TagJigPlasma}", kq.TotalSeconds);
                            }
                            else
                            {
                                Support_SQL.SaveStateUploadDataServer(ProgramID, PLasmaIndex, data.ID, data.TagJigPlasma, "OK");
                                this.Invoke(new Action(delegate
                                {
                                    grvViewTag.BeginUpdate();
                                    grvViewTag.EndUpdate();
                                }));
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    Lib.SaveToLog("ErrorAutoUpload", "Except auto upload", ex.ToString());
                }
            }
        }
        #endregion


        #region Function Support
        /// <summary>
        /// Timeout lệnh thực thi
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="codeBlock"></param>
        /// <returns></returns>
        private static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            IsExecuteFunctionReadTag = true;
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                IsExecuteFunctionReadTag = false;
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
        private static bool ExecuteWithTimeLimitOrigin(TimeSpan timeSpan, Action codeBlock)
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
        /// Disable nút bấm setting
        /// </summary>
        public void DisableSettingButton()
        {
            this.Invoke(new Action(delegate
            {
                btn_Setting.Enabled = false;
            }));

        }

        /// <summary>
        /// Enable nút bấm setting
        /// </summary>
        public void EnableSettingButton()
        {
            this.Invoke(new Action(delegate
            {
                btn_Setting.Enabled = true;
            }));

        }



        #endregion


        #region hiển thị quá trình và gridview
        //
        /// <summary>
        /// hiển thị thông tin dữ liệu upload server Plasma
        /// </summary>
        private void showgrdData_ViewTags(List<dataPlasma> dataShow)
        {
            //dgv_ViewTags.Rows.Clear();
            foreach (dataPlasma item in dataShow)
            {
                DataRow RowOK = dtTest.NewRow();
                RowOK[colID.FieldName] = item.ID;
                RowOK[colDataBarcode.FieldName] = item.PcsBarcode;
                RowOK[colCodeTray.FieldName] = item.TagJigPlasma;
                RowOK[colStatus.FieldName] = item.StatusPlasma;
                RowOK[colTagJig.FieldName] = item.CodeTray;
                //dtTest.Rows.Add(RowOK);
                dtTest.Rows.InsertAt(RowOK, 0);
                grvViewTag.FocusedRowHandle = 0;

                //Support_SQL.ExecuteScalarDBPlasma($"INSERT INTO DataHistory (ID_Program,PlasmaIndex,BarcodePCS,TagJig) VALUES({ProgramID},{PLasmaIndex},'{item.PcsBarcode}','{item.TagJigPlasma}');");
            }
        }



        /// <summary>
        /// hiện thị thông tin các tag jig ở 3 vị trí khi cylinder transfer đưa sản phẩm vào/ra
        /// </summary>
        private void display_InfoChangedJig()
        {
            lb_infoJigOutput.Text = lb_infoJigMachine.Text;
            lb_infoJigMachine.Text = lb_infoJigInput.Text;
            lb_infoJigInput.Text = "";
        }

        /// <summary>
        /// hiển thị thông tin tag jig đọc được ở đầu vào
        /// </summary>
        /// <param name="tagJigInput"></param>
        private void display_InfoJigInput(string tagJigInput)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lb_infoJigInput.Text = tagJigInput;
                lb_infoJigInput.BackColor = Color.Lime;
                //lb_infoJigInput.TextAlign=
            }));
        }

        /// <summary>
        /// đổi màu trạng thái của máy
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="colorchange"></param>
        public void DisplayColorLabel(Label lb, Color colorchange)
        {
            if (c_varGolbal.IsRun == false)
                return;
            if (lb.Text == "")
            {
                //lb.BackColor= Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                lb.BackColor = Color.DarkGreen;
            }
            else
            {
                lb.BackColor = colorchange;
            }
        }

        public void clearDisplay()
        {
            lb_infoJigInput.Text = "";
            lb_infoJigInput.BackColor = Color.DarkGreen;
            lb_infoJigMachine.Text = "";
            lb_infoJigMachine.BackColor = Color.DarkGreen;
            lb_infoJigOutput.Text = "";
            lb_infoJigOutput.BackColor = Color.DarkGreen;
            lb_infoJigInput_Wait.Text = "";
            lb_infoJigInput_Wait.BackColor = Color.DarkGreen;
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Function Upload Serve,Upload Excel
        //Function Upload Serve,Upload Excel

        /// <summary>
        /// Update dữ liệu lên Sever qua Router 56 - Plasma
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="RouteId"></param>
        /// <param name="DeviceId"></param>
        /// <param name="MPN"></param>
        /// <param name="StaffID"></param>
        /// <param name="LotID"></param>
        /// <param name="TagJig"></param>
        /// <param name="list_CodePCS"></param>
        /// <returns></returns>
        private bool Upload_DataPlasma(string LineId, string DeviceId, string MPN, string StaffID, string LotID, List<string> listTagPCS, string tagJigPlasma, string CodeTray, ref string ResultProcess)
        {
            try
            {
                Plasma56 MMCV_DBPlasma_New = new Plasma56();
                //MMCV_DBPlasma_New.IP_SERVER_SMT = c_varGolbal.IP_SMT;
                //MMCV_DBPlasma_New.RouteId = c_varGolbal.RouteID;

                //string ResultProcess = "";
                bool kq = false;
                ResultProcess = MMCV_DBPlasma_New.PlasmaReading(listTagPCS, tagJigPlasma, CodeTray, LotID, StaffID, LineId, DeviceId);
                if (ResultProcess.ToUpper().Contains("OK"))
                {
                    kq = true;
                }
                else
                {
                    kq = false;
                    //Lib.SaveToLog("ErrorUploadServer", tagJigPlasma, ResultProcess);
                }
                if (kq)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                //Lib.SaveToLog("ErrorUploadServer", tagJigPlasma + "-Exception", Ex.ToString());
                ResultProcess = Ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// Upload dữ liệu file Csv.
        /// </summary>
        private void Upload_DB_Excel(List<dataPlasma> lst_dataPlasma)
        {
            // Get date time of save log
            string pathExcel = "";
            string Date_Time_Log = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string Path_FolderLog = @"C:\MMCV_Plasma\" + Path.Combine(c_varGolbal.MPN, c_varGolbal.LotID);
            //string Path_FolderLog = @"C:\MMCV_Plasma\";
            if (!Directory.Exists(Path_FolderLog))
            {
                Directory.CreateDirectory(Path_FolderLog);
            }
            //pathExcel = System.IO.Path.Combine(Path_FolderLog, $"{c_varGolbal.NamePlasma}");
            pathExcel = System.IO.Path.Combine(Path_FolderLog, $"{c_varGolbal.NamePlasma}");
            if (!File.Exists(pathExcel + ".xlsx"))
            {
                if (!Excel.CreatFileExcel_XLSX(Path_FolderLog, $"{c_varGolbal.NamePlasma}"))//CreatFileExcel_CSV
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(this, "Không tạo mới được file Excel!", "Lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    return;
                }
                if (!Excel.WriteData_xlsx_gem(lst_dataPlasma))
                {
                    MessageBox.Show(this, "Không ghi được dữ liệu xuống file Excel!", "Lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (!Excel.WriteData_xlsx_gem(lst_dataPlasma, pathExcel + ".xlsx"))
                {
                    MessageBox.Show(this, "Không ghi được dữ liệu xuống file Excel!", "Lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region PLC
        //PLC
        /// <summary>
        /// Kết nối với PLC theo Port cho process Plasma
        /// </summary>
        public void Connect_PLCPlasma()
        {
            //PLC_Fx5Plasma.IP_Adress = c_varGolbal.IP_PLC;
            PLC_Fx3Plasma.ActLogicalStationNumber = c_varGolbal.LogicalStationNumberPlasma;// +_UcPlasmaIndex;
                                                                                           //PLC_Fx5Plasma.Port = c_varGolbal.Port_PLC + _UcPlasmaIndex + 1;
            int Values = PLC_Fx3Plasma.Open();
            if (Values != 0)
            {
                PLC_Fx3Plasma.Close();
                IsConnectPLC = false;
                lb_Status_PLC.BackColor = Color.Red;
                lb_Status_PLC.Text = "Disconnect";
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Error Connect To PLC Process Plasma ").ShowDialog();
                return;
            }
            else
            {
                lb_Status_PLC.BackColor = Color.Lime;
                lb_Status_PLC.Text = "Connected";
                IsConnectPLC = true;
            }

        }
        /// <summary>
        /// Ngắt kết nối với PLC theo port cho process Plasma
        /// </summary>
        public void Disconnect_PLCPlasma()
        {
            lb_Status_PLC.BackColor = Color.Red;
            lb_Status_PLC.Text = "Disconnect";
            PLC_Fx3Plasma.Close();
        }
        // --------------------------------------------------------------------------------------------------------------------//
        #endregion

        #region Cam Barcode
        /// <summary>
        /// Connect to RFID
        /// </summary>    
        public void Connect_CamBarcodePlasma()
        {
            try
            {
                IsConnectCam = true;
                // Tải dữ liệu setting Camera Barcode theo Program
                DataTable dtPlasmaSettting = Support_SQL.GetTableData($"select * from {PLS.NameTable} " +
                                                    $"where {PLS.ID} = {ProgramID} " +
                                                    $"and {PLS.PlasmaIndex} = {PLasmaIndex} ");
                if (dtPlasmaSettting.Rows.Count <= 0)
                {
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Program is currently not config setting Camera Barcode tại PLasma:{PLasmaIndex}", new Font("Segoe UI", 13F, FontStyle.Bold)).ShowDialog();
                    IsConnectCam = false;
                    return;
                }
                loadListCamBarcode();
                for (int i = 0; i < Lst_CamBarcode.Count; i++)
                {
                    if (!Lst_CamBarcode[i].Connect())//!Lst_CamBarcode[i].Connect()
                    {
                        new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Position PLasma:{PLasmaIndex}\rCan't connect Camera Barcode:{Lst_CamBarcode[i].NameCam}", new Font("Segoe UI", 13F, FontStyle.Bold)).ShowDialog();
                        IsConnectCam = false;
                    }
                }
                if (!IsConnectCam)
                {
                    lb_Status_Barcode.BackColor = Color.Red;
                    lb_Status_Barcode.Text = "Disconnect";
                }
                else
                {
                    lb_Status_Barcode.BackColor = Color.Lime;
                    lb_Status_Barcode.Text = "Connected";
                }


            }
            catch (Exception Ex)
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Error, $"Position PLasma:{PLasmaIndex}\rCan't connect Camera Barcode", new Font("Segoe UI", 13F, FontStyle.Bold)).ShowDialog();
                IsConnectCam = false;
            }
        }

        public void Disconnect_CamBarcodePlasma()
        {
            foreach (CamBarcode item in Lst_CamBarcode)
            {
                if (item != null)
                {
                    IsConnectCam = item.Disconnect();
                    //IsConnectCam = item.Disconnect_ModBus();
                }
                lb_Status_Barcode.BackColor = Color.Red;
                lb_Status_Barcode.Text = "Disconnect";
            }
        }

        /// <summary>
        /// Bắt đầu luồng đọc Code Jig bằng Cam Barcode
        /// </summary>
        private void StartReadTag()
        {
            bool complete = false;

            IsExecuteFunctionReadTag = true;
            if (Lst_CamBarcode.Count > 0)
            {
                foreach (CamBarcode item in Lst_CamBarcode)
                {
                    item.IsComplete = false;
                    Task.Factory.StartNew(item.StartReadTag);
                    //item.StartReadTag();
                }
            }

            while (IsExecuteFunctionReadTag)
            {
                foreach (CamBarcode item in Lst_CamBarcode)
                {
                    bool kt = item.IsComplete;
                    if (item.IsComplete)
                    {
                        complete = true;
                    }
                    else
                    {
                        complete = false;
                    }
                }
                if (complete)
                {
                    break;
                }
            }
        }
        private void StartReadTag_new()
        {
            if (Lst_CamBarcode.Count > 0)
            {
                foreach (CamBarcode item in Lst_CamBarcode)
                {
                    item.StartReadTagNew();
                }
            }
        }


        /// <summary>
        /// Tắt luông quét đọc thẻ
        /// </summary>
        private void StopReadTag()
        {
            try
            {

            }
            catch (Exception Ex)
            {


            }

        }
        private void StatusLable(Label label, Color color, string statusString)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() =>
                {
                    label.BackColor = color;
                    label.Text = statusString;
                }));

            }
            else
            {
                label.BackColor = color;
                label.Text = statusString;
            }
        }

        private void StartTriggerCam(byte[] bytesend)
        {
            if (Lst_CamBarcode.Count > 0)
            {
                foreach (CamBarcode item in Lst_CamBarcode)
                {
                    item.SendSignal(bytesend);
                    //item.SendSignal_Modbus(bytesend);
                }
            }

        }
        #endregion

        #region Event user control
        /// <summary>
        /// Button setting on uc plasma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Setting_Click(object sender, EventArgs e)
        {
            Frm_SettingCamBarcode newFrm = new Frm_SettingCamBarcode();
            newFrm.ProgramID = ProgramID;
            newFrm.PlasmaIndex = PLasmaIndex;
            if (newFrm.ShowDialog() == DialogResult.OK)
            {
                loadListCamBarcode();
            }
        }
        /// <summary>
        /// Sự kiện khi use control thay đổi kích thước
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Plasma_Resize(object sender, EventArgs e)
        {
            //controlAutoSize(pcb_Plasma, lst_Control);
        }
        #endregion


        #region event với Grid control
        private void grvViewTag_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                e.Value = grvViewTag.RowCount - grvViewTag.GetRowHandle(e.ListSourceRowIndex);// + 1;
            }
        }

        private void grvViewTag_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {

            int index = Lib.ToInt(grvViewTag.GetRowCellValue(e.RowHandle, colNO));

            if (index == grvViewTag.RowCount)
            {
                e.Appearance.BackColor = SystemColors.ActiveCaption;
            }

        }

        private void grvViewTag_MouseUp(object sender, MouseEventArgs e)
        {

        }
        #endregion

        #region Auto Size lable show text plasma
        public List<controlRect> oldCtrl = new List<controlRect>();
        int ctrlNo = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow RowOK = dtTest.NewRow();
            RowOK[colDataBarcode.FieldName] = "CodePCS" + grvViewTag.GetRowHandle(grvViewTag.RowCount);
            RowOK[colCodeTray.FieldName] = "TagJig" + grvViewTag.GetRowHandle(grvViewTag.RowCount);
            RowOK[colStatus.FieldName] = "Test" + grvViewTag.GetRowHandle(grvViewTag.RowCount);
            RowOK[colTagJig.FieldName] = "CodeTray" + grvViewTag.GetRowHandle(grvViewTag.RowCount);
            //dtTest.Rows.Add(RowOK);
            dtTest.Rows.InsertAt(RowOK, 0);
        }
        private void grvViewTag_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    if (e.Column == colStatus)
                    {
                        if (e.CellValue.ToString().Contains("WAITING"))
                        {
                            int ID = Lib.ToInt(grvViewTag.GetRowCellValue(e.RowHandle, colID.FieldName));
                            string select = $"SELECT StateUploadServer FROM Plasma WHERE ID={ID}";
                            string status = Lib.ToString(Support_SQL.ExecuteScalarDBPlasma(select));
                            if (status.Contains("OK"))
                            {

                                grvViewTag.SetRowCellValue(e.RowHandle, colStatus, "OK");

                            }
                            //grvViewTag.RefreshRowCell(e.RowHandle, colStatus);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public struct controlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            grvViewTag.BeginUpdate();
            grvViewTag.EndUpdate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string update = "UPDATE Plasma SET StateUploadServer='OK'";
            Support_SQL.ExecuteScalarDBPlasma(update);
        }

        private void controllInitializeSize(Control Crl_defined, List<Control> Crl_change)
        {
            controlRect cR;
            cR.Left = Crl_defined.Left; cR.Top = Crl_defined.Top; cR.Width = Crl_defined.Width; cR.Height = Crl_defined.Height;
            oldCtrl.Add(cR);
            AddControl(Crl_change);
        }
        private void controlAutoSize(Control Crl_defined, List<Control> Crl_change)
        {
            if (ctrlNo == 0)
            {
                controlRect cR;
                cR.Left = 0; cR.Top = 0; cR.Width = Crl_defined.PreferredSize.Width; cR.Height = Crl_defined.PreferredSize.Height;

                oldCtrl.Add(cR);
                AddControl(Crl_change);
            }
            float wScale = (float)Crl_defined.Width / (float)oldCtrl[0].Width;
            float hScale = (float)Crl_defined.Height / (float)oldCtrl[0].Height;//.Height;
            ctrlNo = 1;
            AutoScaleControl(Crl_change, wScale, hScale);
        }

        private void AddControl(List<Control> Crl_change)
        {
            foreach (Control c in Crl_change)
            {
                controlRect objCtrl;
                objCtrl.Left = c.Left; objCtrl.Top = c.Top; objCtrl.Width = c.Width; objCtrl.Height = c.Height;
                oldCtrl.Add(objCtrl);
            }
        }

        private void AutoScaleControl(List<Control> Crl_change, float wScale, float hScale)
        {
            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
            foreach (Control c in Crl_change)
            {
                ctrLeft0 = oldCtrl[ctrlNo].Left;
                ctrTop0 = oldCtrl[ctrlNo].Top;
                ctrWidth0 = oldCtrl[ctrlNo].Width;
                ctrHeight0 = oldCtrl[ctrlNo].Height;
                c.Left = (int)((ctrLeft0) * wScale);
                c.Top = (int)((ctrTop0) * hScale);
                c.Width = (int)(ctrWidth0 * wScale);
                c.Height = (int)(ctrHeight0 * hScale);
                ctrlNo++;
            }
        }
        #endregion

        #region Class Jig
        public class ID_Jig
        {
            public int ID { get; set; }
            public string NameJig { get; set; }
            public string link { get; set; }
            public int NoLock { get; set; }
            public int QualifyJig { get; set; }
            public List<string> lstPcs { get; set; }
            public string LotID { get; set; }
            public string MPN { get; set; }

            public ID_Jig()
            {

            }
            public ID_Jig(string name, string Lot)
            {
                NameJig = name;
                LotID = Lot;
            }
        }

        #endregion

        #region Các hàm check dữ liệu plasma ở server MMCV
        public List<string> CheckDataHavePlasma(List<string> listPcs, ID_Jig Jig)
        {

            List<string> Result = new List<string>();
            DateTime start = new DateTime(), stop = new DateTime();
            #region dùng thư viện nhà máy
            try
            {
                Plasma56 mmcv = new Plasma56();
                start = DateTime.Now;
                Result = mmcv.CheckDataPlasma(listPcs);
                stop = DateTime.Now;
            }
            #endregion
            catch (Exception ex)
            {
                stop = DateTime.Now;
                //Lib.SaveToLog("ExceptionCheckData56", $"{Jig.NameJig}", ex.ToString());
                Lib.ShowError(ex.ToString());
                Result.Add(GlobVar.Error56);
                Result.Add(ex.ToString());
            }
            string strSend = string.Join(",", listPcs);
            string strReceive = string.Join(",", Result);
            TimeSpan kq = stop - start;
            //Lib.SaveToLog("CheckHaveData56", $"{Jig.NameJig}\r\n{start.ToString("dd-MM HH:mm:ss")} Data56Check:{strSend}", $"{stop.ToString("dd-MM HH:mm:ss")} DataHave56:{strReceive}");
            Lib.SaveToLog(NameFileLog_Main, start, stop, $"Send:CheckDataPlasma ({strSend})", $"Receive: ({strReceive})", Jig.NameJig, kq.TotalSeconds);
            return Result;
        }

        public List<string> CheckNoDataPlasma(List<string> listPcs, ID_Jig Jig)
        {

            List<string> Result = new List<string>();
            List<string> data = new List<string>();
            DateTime start = new DateTime(), stop = new DateTime();
            #region dùng thư viện nhà máy
            try
            {
                Boxing64 mmcv = new Boxing64();
                mmcv.LineId = c_varGolbal.LineID;
                mmcv.IP_SERVER_SMT = c_varGolbal.IP_SMT;
                mmcv.LotNo = Jig.LotID;
                start = DateTime.Now;
                Result = mmcv.CheckDataPlasma(listPcs);
                stop = DateTime.Now;
            }
            #endregion
            catch (Exception ex)
            {
                stop = DateTime.Now;
                //Lib.SaveToLog("ExceptionCheckData56", $"{Jig.NameJig}", ex.ToString());
                Lib.ShowError(ex.ToString());
                Result.Add(GlobVar.Error56);
                Result.Add(ex.ToString());
            }
            TimeSpan kq = stop - start;
            string strSend = string.Join(",", listPcs);
            string strReceive = string.Join(",", Result);
            //Lib.SaveToLog("CheckNoneData56", $"{Jig.NameJig}\r\n{start.ToString("dd-MM HH:mm:ss")} Data56Check:{strSend}", $"{stop.ToString("dd-MM HH:mm:ss")} Data56None:{strReceive}");
            Lib.SaveToLog(NameFileLog_ProcessAuto, start, stop, $"Send:CheckNoneDataPlasma ({strSend})", $"Receive: ({strReceive})", Jig.NameJig, kq.TotalSeconds);
            return Result;
        }
        #endregion

        #region TEST
        DataTable dtTest = new DataTable();
        Random rnd = new Random(100);
        Random rnd2 = new Random(100);
        private void Button1_Click_1(object sender, EventArgs e)
        {
            //c_varGolbal.StaffID = "HA";
            //c_varGolbal.MPN = "MPN_TEST";
            //c_varGolbal.LotID = "1234";
            //List<dataPlasma> lst = new List<dataPlasma>();
            //for (int i = 0; i < 1; i++)
            //{
            //    dataPlasma.TagIndex = $"{i}";
            //    dataPlasma.TagJigPlasma = $"Jig_Plasma{rnd2.Next(10, 20)}";
            //    dataPlasma.TagJigTransfer = $"Jig_Plasma{rnd2.Next(10, 20)}";
            //    dataPlasma.PcsBarcode = $"AAA{rnd.Next(30, 40)},BBB{rnd.Next(30, 40)},CCC{rnd.Next(30, 40)},DDD{rnd.Next(30, 40)},EEE{rnd.Next(30, 40)},FFF{rnd.Next(30, 40)},GGG{rnd.Next(30, 40)},HHH{rnd.Next(30, 40)},III{rnd.Next(30, 40)}";
            //    dataPlasma.DateTimeInPlasma = "Thời gian Jig vào";
            //    dataPlasma.DateTimeOutPlasma = "Thời gian Jig ra";
            //    dataPlasma.CodeTray = "CodeTray1,CodeTray2";
            //    dataPlasma.CycleTime = "35 giây";
            //    dataPlasma.StatusPlasma = "OK";
            //    ProgramID = 29;
            //    PLasmaIndex = 1;
            //    string sql = $"INSERT INTO Plasma(ID_Program, ID_Plasma, TagJigTransfer, TagJigPlasma, CodePCS,DateTimeInPlasma,Date_Time,DateTimeOutPlasma,CodeTray,Cycletime,StateUploadServer,CompletePlasma,CompleteBoxing,LotID,MPN) " +
            //                                    $"VALUES('{ProgramID}', '{PLasmaIndex}'," +
            //                                    $" '{dataPlasma.TagJigTransfer}','{dataPlasma.TagJigPlasma}', '{dataPlasma.PcsBarcode}'," +
            //                                    $"'{dataPlasma.DateTimeInPlasma}','{dataPlasma.DateTimeInPlasma}','{dataPlasma.DateTimeOutPlasma}','{dataPlasma.CodeTray}','{dataPlasma.CycleTime}'," +
            //                                    $"'{dataPlasma.StatusPlasma}',1,0,'{c_varGolbal.LotID}','{c_varGolbal.MPN}');" +
            //                                    $" SELECT last_insert_rowid() as ID;";
            //    dataPlasma.ID = Lib.ToInt(Support_SQL.ExecuteScalarDBPlasma(sql));
            //    lst.Add(dataPlasma);
            //}
            MessageBox.Show("HAHAHAHA", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //c_varGolbal.StaffID = "HA";
            //c_varGolbal.MPN = "MPN_TEST";
            //c_varGolbal.LotID = "1234";
            //Upload_DB_Excel(lst);
            //this.Invoke(new MethodInvoker(delegate
            //{
            //    // Hiển thị dữ liệu lên data grid view plasma
            //    showgrdData_ViewTags(lst);
            //}));

        }
        #endregion
    }
}
