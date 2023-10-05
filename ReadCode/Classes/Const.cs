using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadCode
{
    public static class ColName
    {
        public static string NameTag = "NameTag";
    }

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
        public static byte[] TON = Encoding.ASCII.GetBytes("T");
        public static byte[] TOFF = Encoding.ASCII.GetBytes("TO");
    }
}
