using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadCode
{
    public static class c_varGolbal
    {
        // NB - 19032023
        public static string str_MachineVersion { get; set; }
        public static string str_ConnectDB { get; set; }
        public static string str_ConnectDB_Backup { get; set; }
        //public static string str_ConnectDBConffig
        public static string str_ConnectDBConffig_FVI_Server { get; set; }
        public static string IP_SMT { get; set; }
        public static string CodeType { get; set; }
        public static string NoReadString { get; set; }
        public static string IP_PLC { get; set; }
        public static int Port_PLC { get; set; }
        public static string RouteID { get; set; }
        public static string NameMachine { get; set; }
        public static string ProgramName { get; set; }
        public static string LineID { get; set; }
        public static string DeviceID { get; set; }
        public static string MPN { get; set; }
        public static string LotID { get; set; }
        public static string StaffID { get; set; }
        public static string str_ConnectDBUsers { get; set; }
        public static bool IsAdmin { get; set; }
        public static string CodeJig { get; set; }

        // NB - 19032023
        public static bool Finish_Lot { get; set; }
        public static bool IsRun { get; set; }
        public static bool IsFilter { get; set; } = true;

        public static string Missing = "No PCS";

        public static int QtyPcs { get; set; }
        public static bool TakeJigHavePcs { get; set; }
       
        public static int TimeTranferJig { get; set; }    
        /// <summary>
        /// Biến lưu giá trị cài đặt kí tự đầu của jig Plasma
        /// </summary>
        public static string StrHeaderTagJig { get; set; }
        /// <summary>
        /// Biến lưu thời gian cho phép chạy lại Jig ở máy Plasma theo phút
        /// </summary>
        public static int TimeRepeatJig { get; set; }

        /// <summary>
        /// Biến khi người dùng bấm nút có sản phẩm
        /// </summary>
        public static bool _isProduct { get; set; }

        public static List<string> List_JigPlasma { get; set; }

        public static List<string> List_DataCode { get; set; }

        // NB - 26032023
        public static double CamIndex { get; set; }
        public static double MaxGray { get; set; }
        public static double RatioPCS { get; set; }
        public static double Offset { get; set; }
        public static double RealRatioPCS { get; set; }
        //

        static c_varGolbal()
        {
            List_DataCode = new List<string>();
            List_JigPlasma = new List<string>();
        }
        #region bit IO VC3000
        //bit 
        public static bool BitIO1 { get; set; }
        public static bool BitIO2 { get; set; }
        public static bool BitIO3 { get; set; }
        #endregion

    }
    public class ObjectFile
    {
        public int STT { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Program { get; set; }

    }
    public class Result
    {
        public Result()
        {
            ListData = new List<Code>();
        }
        public List<Code> ListData { get; set; }

    }
    public class Code
    {
        public int CamIndex { get; set; }
        public int IDRegion { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }

        public bool statusICT { get; set; }
        public Code(int _CamIndex,int _IDRegion, string _Type, string _Content,bool isIct)
        {
            CamIndex = _CamIndex;
            IDRegion = _IDRegion;
            Content = _Content;
            Type = _Type;
            statusICT = isIct;
        }
    }
    public class dataVision
    {
        public int ID { get; set; }
        public string CamIndex { get; set; }
        public string PcsIndex { get; set; }
        public string PcsBarcode { get; set; }
        public string TagJigNonePCS { get; set; }
        public string TagJigHavePCS { get; set; }
        public bool statusICT { get; set; }
        public bool statusFinishLot { get; set; }
        public bool status56 { get; set; }
        public int NoLock { get; set; }
        public int QualifyJig { get; set; }
    }
    public class dataUploadServer
    {//List<string> barcodeList, string ToolJig, string ToolJig1, string LotNo, string StaffNo, string LineID_, string DeviceID_
        public List<string> barcodeList { get; set; }
        public string ToolJig { get; set; }//HavePCS
        public string ToolJig1 { get; set; }//NonePCS
        public string LotNo { get; set; }
        public string StaffNo { get; set; }
        public string LineID_ { get; set; }
        public string DeviceID_ { get; set; }
    }
    public class dataReadcode
    {
        public string TagJig1 { get; set; }
        public string TagJig2 { get; set; }
        public string PcsBarcode { get; set; }
        
        public string DateTimeIn { get; set; }
        public string FinishLot { get; set; }
        public string Status { get; set; }
    }

}
