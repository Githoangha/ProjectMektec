using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineGolden_PLasma
{
    public static class c_varGolbal
    {
        // NB - 19032023
        public static string str_MachineVersion { get; set; }
        public static string str_ConnectDBConffig { get; set; }
        public static string str_ConnectDBReadCode_BeforePlasma1 { get; set; }
        public static string str_ConnectDBReadCode_BeforePlasma2 { get; set; }
        public static int QtyBeforePlasma { set; get; }
        public static List<PathFile> List_LinkDB { get; set; }
        public static string str_ConnectDBReadCode_FVI { get; set; }
        public static string RouteID { get; set; }
        public static string NameMachine { get; set; }
        public static string ProgramName { get; set; }
        public static int UseMachine { get; set; }
        public static string LineID { get; set; }
        public static string DeviceID { get; set; }
        public static string NamePlasma { get; set; }
        public static string MPN { get; set; }
        public static string LotID { get; set; }
        public static string StaffID { get; set; }
        public static int NumJigPlasma { get; set; }
        public static bool IsUploadPlasma { get; set; }
        public static string str_ConnectDBUsers { get; set; }
        public static bool IsAdmin { get; set; }
        public static bool GetJigHavePcs { get; set; }

        public static bool UseFvi { get; set; }

        public static bool IsRun { get; set; }

        public static List<string> List_Model_CodeTray = new List<string>();

        public static string IP_SMT { get; set; }
        /// <summary>
        /// Biến lưu thời gian cho phép chạy lại Jig ở máy Plasma theo phút
        /// </summary>
        public static int TimeRepeatJig { get; set; }

        #region  Logical Station Number của MXComponent
        public static int LogicalStationNumberPlasma { get; set; }
        #endregion



        #region   Trigger PLC

        //TriggerData
        /// <summary>
        /// 
        /// </summary>
        public static string TriggerOK { get; set; }
        public static string TriggerError { get; set; }

        //Trigger have Data
        //PC->PLC
        public static string TriggerHaveData { get; set; }

        //PLC->PC
        public static string TriggerHaveDataOK { get; set; }

        //Trigger ReadCode
        //PLC->PC
        public static string TriggerReadCode { get; set; }

        //PC->PLC
        public static string TriggerReadCodeOK { get; set; }

        //Trigger Finish
        //PLC->PC
        public static string TriggerFinish { get; set; }

        //PC->PLC
        public static string TriggerFinishOK { get; set; }

        //Trigger Reset
        //PC->PLC  (0.5->1s)
        public static string TriggerResest { get; set; }

        #endregion


    }

    #region các class của vision chụp sản phẩm

    public class dataVision
    {
        public string PcsIndex { get; set; }
        public string PcsBarcode { get; set; }
        public string TagJigTransfer { get; set; }
        public string TagJigPlasma { get; set; }
    }
    #endregion
    public class dataPlasma
    {
        public int ID { get; set; }
        public string TagIndex { get; set; }
        public string TagJigTransfer { get; set; }
        public string TagJigPlasma { get; set; }
        public string CodeTray { get; set; }
        public string PcsBarcode { get; set; }
        public string DateTimeInPlasma { get; set; }
        public string DateTimeOutPlasma { get; set; }
        public string CycleTime { get; set; }
        public string StatusPlasma { get; set; }
    }

    public class PathFile
    {
        public int Number { get; set; }
        public string LinkDB { get; set; }
        public bool isUse { get; set; }
        public PathFile(int number,string link,bool isuse)
        {
            Number = number;
            LinkDB = link;
            this.isUse = isuse;
        }
        public void SetValue(string link, bool isuse)
        {
            LinkDB = link;
            this.isUse = isuse;
        }
        public void SetValue(bool isuse)
        {
            this.isUse = isuse;
        }
    }
}
