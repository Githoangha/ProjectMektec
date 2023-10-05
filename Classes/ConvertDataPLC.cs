using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineGolden_PLasma
{
    public static class ConvertDataPLC
    {
        //Write Bit
        public static short WriteBitToPLC(bool dataIN)
        {
            short intputPLC = 0;

            if (dataIN == true)
            {
                intputPLC = 1;
            }
            else if (dataIN == false)
            {
                intputPLC = 0;
            }
            return intputPLC;

        }
        //Write String (lib)
        public static short[] WriteStringToPLC(string dataIN, int lenghtWord)
        {
            byte[] BufferStringByte;
            short[] intputPLC = new short[lenghtWord];
            int iLengthOfBuffer;
            int iNumber;
            BufferStringByte = Encoding.Default.GetBytes(dataIN);
            iLengthOfBuffer = Math.Min(BufferStringByte.Length, lenghtWord * 2);

            for (iNumber = 0; iNumber <= iLengthOfBuffer - 2; iNumber += 2)
            {
                intputPLC[iNumber / 2] = BitConverter.ToInt16(BufferStringByte, iNumber);
            }
            if ((iLengthOfBuffer % 2) == 1)
            {
                intputPLC[(iLengthOfBuffer / 2)] = BufferStringByte[iLengthOfBuffer - 1];
            }

            return intputPLC;
        }
        //Write Dword (lib)
        public static short[] WriteDwordToPLC(int dataIN)
        {
            byte[] BufferDwordByte;
            short[] intputPLC = new short[2];
            BufferDwordByte = BitConverter.GetBytes(System.Convert.ToInt32(dataIN));
            intputPLC[0] = BitConverter.ToInt16(BufferDwordByte, 0);
            intputPLC[1] = BitConverter.ToInt16(BufferDwordByte, 2);
            return intputPLC;
        }
        //Write Float (lib)
        public static short[] WriteFloatToPLC(float dataIN)
        {
            byte[] BufferFloatdByte;
            short[] intputPLC = new short[2];
            BufferFloatdByte = BitConverter.GetBytes(System.Convert.ToSingle(dataIN));
            intputPLC[0] = BitConverter.ToInt16(BufferFloatdByte, 0);
            intputPLC[1] = BitConverter.ToInt16(BufferFloatdByte, 2);
            return intputPLC;
        }
        // Read Bit
        public static bool ReadBitPLC(short dataIN)
        {
            bool outputPLC = false;

            if (dataIN == 1)
            {
                outputPLC = true;
            }
            else if (dataIN == 0)
            {
                outputPLC = false;
            }

            return outputPLC;
        }
        //Read String (lib)
        public static string ReadStringFromPLC(short[] dataIN, int lenghtWord)
        {
            byte[] BufferStringByte = new byte[lenghtWord * 2];
            byte[] byarrTemp;
            int iNumber;
            for (iNumber = 0; iNumber <= lenghtWord - 1; iNumber++)
            {
                byarrTemp = BitConverter.GetBytes(dataIN[iNumber]);
                BufferStringByte[iNumber * 2] = byarrTemp[0];
                BufferStringByte[iNumber * 2 + 1] = byarrTemp[1];
            }
            string outputPLC = Encoding.Default.GetString(BufferStringByte);
            return outputPLC;
        }
        //Read Dword (lib)
        public static int ReadDwrodFromPLC(short[] dataIN)
        {
            byte[] byarrBufferByte = new byte[4];
            byte[] byarrTemp;
            int iNumber;
            for (iNumber = 0; iNumber <= 2 - 1; iNumber++)
            {
                byarrTemp = BitConverter.GetBytes(dataIN[iNumber]);
                byarrBufferByte[iNumber * 2] = byarrTemp[0];
                byarrBufferByte[iNumber * 2 + 1] = byarrTemp[1];
            }
            int outputPLC = System.Convert.ToInt32(BitConverter.ToInt32(byarrBufferByte, 0));
            return outputPLC;
        }
        //Read Float (lib)
        public static float ReadFloatFromPLC(short[] dataIN)
        {
            byte[] byarrBufferByte = new byte[4];
            byte[] byarrTemp;
            int iNumber;
            for (iNumber = 0; iNumber <= 2 - 1; iNumber++)
            {
                byarrTemp = BitConverter.GetBytes(dataIN[iNumber]);
                byarrBufferByte[iNumber * 2] = byarrTemp[0];
                byarrBufferByte[iNumber * 2 + 1] = byarrTemp[1];
            }
            float outputPLC = System.Convert.ToSingle(BitConverter.ToSingle(byarrBufferByte, 0));
            return outputPLC;
        }

        public static short[] ArrIntToShort(int[] tempXDataInteger)
        {
            List<short> tempListShortOut = new List<short>();
            foreach (int item in tempXDataInteger)
            {
                Tuple<short, short> tuple2Short = IntToShortConverter(item);
                tempListShortOut.Add(tuple2Short.Item1);
                tempListShortOut.Add(tuple2Short.Item2);
            }
            return tempListShortOut.ToArray();
        }

        public static Tuple<short, short> IntToShortConverter(int inputIntNumber)
        {
            byte[] bytes = BitConverter.GetBytes(inputIntNumber);
            short LSB = BitConverter.ToInt16(bytes, 0);
            short MSB = BitConverter.ToInt16(bytes, 2);

            return new Tuple<short, short>(LSB, MSB);
        }
        public static void WriteBitPlc(bool value)
        {

        }

    }
}
