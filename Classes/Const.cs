using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineGolden_PLasma
{
    public static class ColName
    {   
        public static int testtt123123t { get; set; }
        public static string NameTag = "NameTag";
    }

    #region CÁC TABLE TRONG DB_CONFIG
    /// <summary>
    /// Table Plasma Setting
    /// </summary>
    public static class PLS
    {
        public static string NameTable = "PlasmaSetting";
        public static string ID = "ID_Program";
        public static string PlasmaIndex = "PlasmaIndex";
        public static string QtyCamBarcode = "QtyCamBarcode";
        public static string NumCamBarcode = "NumCamBarcode";
        public static string Ip_Barcode = "Ip_Barcode";
        public static string Port_Barcode = "Port_Barcode";
    }

    /// <summary>
    /// Table Program Main
    /// </summary>
    public static class PrgMain
    {
        public static string NameTable = "ProgramMain";
        public static string ID = "ID_Program";
        public static string PrgName = "ProgramName";
        public static string Description = "Description";
        public static string NumberCamera = "NumberCamera";
        public static string NumberPlasma = "NumberPlasma";
        public static string NumJigPlasma = "NumJigPlasmaBase";
        public static string TransferJig = "TransferJig";
        public static string ReadCodePCS = "ReadCodePCS";
        public static string TimeTranferJig = "TimeTranferJig";
        public static string StringHeaderTagJig = "StringHeaderTagJig";
        public static string TimeRepeatJig = "TimeRepeatJig";
    }
    // --------------------------------------------------------------------------------------------------------------------//
    #endregion
    public enum ELang
    {
        Vie = 0,
        Eng = 1
    }

    public static class Lang
    {
        public static string Vie = "Vie";
        public static string Eng = "Eng";
    }
    public static class ASCII
    {
        public static char CR = (char)13;

    }

    public static class ConstSendByte
    {
        public static byte[] TON = Encoding.ASCII.GetBytes("a");
        public static byte[] TOFF = Encoding.ASCII.GetBytes("b");
    }
}
 




