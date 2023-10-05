using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActUtlTypeLib;

namespace LineGolden_PLasma
{
    public class MxComponent
    {
        public static bool ReadStringPLC(ActUtlTypeClass plc, string address, int size, out string ValueOut)
        {
            try
            {
                short[] array = new short[size];
                plc.ReadDeviceBlock2(address, array.Length, out array[0]);
                ValueOut = ConvertDataPLC.ReadStringFromPLC(array, array.Length).Replace("\0", "");
                return true;
            }
            catch (Exception)
            {
                ValueOut = "ERROR";
                return false;
            }
        }
        public static bool ReadIntPLC(ActUtlTypeClass plc, string address, int size, out int ValueOut)
        {
            try
            {
                short[] array = new short[size];
                plc.ReadDeviceBlock2(address, array.Length, out array[0]);
                ValueOut = array[0];
                return true;
            }
            catch (Exception)
            {
                ValueOut = -1;
                return false;
            }
        }
        public static bool WriteStringToPLC(ActUtlTypeClass plc,string address,int size,string ValueIn)
        {
            try
            {
                short[] array = new short[size];
                array = ConvertDataPLC.WriteStringToPLC(ValueIn, ValueIn.Length % 2 == 0 ? ValueIn.Length / 2 : ValueIn.Length / 2 + 1);
                plc.WriteDeviceBlock2(address, array.Length, ref array[0]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
