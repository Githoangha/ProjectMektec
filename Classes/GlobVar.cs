using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LineGolden_PLasma
{
    public static class GlobVar
    {
        public static string LangChoose = Lang.Eng;

        public static bool LockEvent { get; set; }


        public static string PathFileBoxing { get; set; }
        public static string DateTimeIn { get; set; }
        public static string DateTimeOut { get; set; }
        public static string TimeCT { get; set; }

        #region Json Api
        /// Json Api Mektec
        public static string IPServer { get; set; }
        public static string PortServer { get; set; }
        public static string MODOEE { get; set; }
        public static string MODMES { get; set; }
        public static bool UploadServer { get; set; }
        #endregion


        public static void OnKeyBoard()
        {
            string Path = $"{Application.StartupPath} \\KeyBoard\\Oskeyboard.exe";
            if (Lib.ProgramIsRunning(Path))
            {

            }
            else
            {
                System.Diagnostics.Process.Start(Path);
            }
            
        }

        public static string WAIT = "WAIT";
        public static string OK = "OK";
        public static string Error = "ERROR";
        public static string Waiting = "WAITING";
        public static string Error64 = "Error64";
        public static string Error56 = "Error56";
    }
    public class MachineBoxing
    {
        public static string NameTable = "MachineBoxing";
        public static string ID = "ID";
        public static string ID_ProgMain = "ID_ProgMain";
        public static string NameBoxing = "NameBoxing";
        public static string indexBoxing = "IndexBoxing";
        public static string ID_ProcessRB = "ID_ProcessRB";
        //public static string CodeProcessRobot = "CodeProcessRobot";
        //public static string ProcessRobot = "ProcessRobot";
        public static string NameBarcode = "NameBarcode";
        public static string IPBarcode = "IPBarcode";
        public static string PortBarcode = "PortBarcode";
        public static string NamePLC = "NamePLC";
        public static string IPPLC = "IPPLC";
        public static string PortPLC = "PortPLC";
        public static string TranferJig = "TranferJig";

    }
    public class ProgramMain
    {
        public static string NameTable = "ProgramMain";
        public static string ID = "ID";
        public static string ProgName = "ProgramName";
        public static string QtyBoxing = "QtyBoxing";
        public static string QtyPlasma = "QtyPlasma";
        public static string NumberJigTranfer = "NumberJigTranfer";
        public static string TranferJig = "TranferJig";
        public static string TimeTranfer = "TimeTranferJig";
        public static string HeaderTag = "HeaderTag";
        public static string TextJigPlasma = "TextJigPlasma";
        public static string TextJigTranfer = "TextJigTranfer";
        public static string NameBT = "NameBarcodeTranfer";
        public static string IPBT = "IPBarcodeTranfer";
        public static string PortBT = "PortBarcodeTranfer";
        public static string LineID = "LineID";
        public static string RouteID = "RouteID";
        public static string PathFilePlasma1 = "FileDataPLasma1";
        public static string PathFilePlasma2 = "FileDataPLasma2";
    }
    public class ProgramRobot
    {
        public static string NameTable = "ProgramRobot";
        public static string ID = "ID";
        public static string Name = "Name";
        public static string Code = "Code";
        public static string IndexBoxing = "IndexBoxing";
    }
    public class TB_Status
    {
        public static string NameTable = "Status";
        public static string ID = "ID";
        public static string Value = "Value";
        public static string NameE_D = "NameEna_Disa";
        public static string NameT_F = "Name_T_F";
        public static string Value2 = "Value2";
    }

    public class Plasma_Boxing
    {
        public static string NameTable = "Plasma_Boxing";
        public static string ID = "ID";
        public static string JigPlasma = "JigPlasma";
        public static string JigTransfer = "JigTransfer";
        public static string CodePCS = "CodePCS";
        public static string CodeTray = "CodeTray";

    }

    public class User
    {
        public static string NameTable = "User";
        public static string ID = "ID";
        public static string NameUser = "NameUser";
        public static string PassWord = "Password";
    }
}
