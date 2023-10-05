using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using AYNETTEK.UHFReader;
namespace LineGolden_PLasma
{
    public class Tools
    {

        #region EncodeU32

        /// <summary>
        /// Create byte array that represents unsigned 32-bit integer
        /// </summary>
        /// <param name="value">32-bit integer to encode</param>
        /// <returns>4-byte encoding of 32-bit integer</returns>
        public static byte[] EncodeU32(UInt32 value)
        {
            byte[] bytes = new byte[4];
            FromU32(bytes, 0, value);
            return bytes;
        }

        

        /// <summary>
        /// Insert unsigned 32-bit integer into big-endian byte string
        /// </summary>
        /// <param name="bytes">Target big-endian byte string</param>
        /// <param name="offset">Location to insert into</param>
        /// <param name="value">32-bit integer to insert</param>
        /// <returns>Number of bytes inserted</returns>
        public static int FromU32(byte[] bytes, int offset, UInt32 value)
        {
            int end = offset;
            bytes[end++] = (byte)((value >> 24) & 0xFF);
            bytes[end++] = (byte)((value >> 16) & 0xFF);
            bytes[end++] = (byte)((value >> 8) & 0xFF);
            bytes[end++] = (byte)((value >> 0) & 0xFF);
            return end - offset;
        }
        #endregion

        //Convert hexadecimal string to ushort
        public static ushort HexString2Ushort(string s)
        {
            ushort value = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != ' ')
                {
                    value = (ushort)(value * 16 + HexStringToByte(s, i));
                }
            }

            return value;
        }

        //Convert the mac address in the form of a byte array to the corresponding string
        public static string MacToString(byte[] mac)
        {
            string MacString = "";

            for (int i = 0; i < 6; i++)
            {
                MacString += ByteToHexString(mac[i]);
                if (i < 5)
                {
                    MacString += ":";
                }
            }

            return MacString;
        }

        //Convert the mac address in the form of a string to the corresponding byte array
        public static byte[] StringToMac(string str)
        {
            string temp = "";

            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] != ' ') && (str[i] != ':'))
                {
                    temp += str[i];
                }
            }

            return HexStringToByte(temp, 0, 6);//6 bytes long
        }

        //Check if a string is a hexadecimal string
        public static bool ValidHexString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!(((str[i] >= '0') && (str[i] <= '9')) ||
                    ((str[i] >= 'a') && (str[i] <= 'f')) ||
                    ((str[i] >= 'A') && (str[i] <= 'F'))))
                {
                    return false;
                }
            }

            return true;
        }

        //Convert a single hexadecimal character (4 bits) to byte
        private static byte HexStringToByte(string str, int pos)
        {
            byte value = 0;

            if ((str[pos] >= '0') && (str[pos] <= '9'))
            {
                value = (byte)(str[pos] - '0');
            }
            else if ((str[pos] >= 'a') && (str[pos] <= 'f'))
            {
                value = (byte)(str[pos] - 'a' + 10);
            }
            else if ((str[pos] >= 'A') && (str[pos] <= 'F'))
            {
                value = (byte)(str[pos] - 'A' + 10);
            }

            return value;
        }

        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static string strToSpaceStr(string str)
        {
            string tmpString = str.Replace(" ","");
            if (tmpString.Length % 2 != 0)
            {
                tmpString += " ";
            }

            string returnStr = "";
            for (int i = 0; i < tmpString.Length; )
            {
                returnStr += tmpString.Substring(i, 2) + " ";
                i += 2;
            }
            return returnStr;
        }
        //Start from the pos position of the string, convert it into an array, and the length of the converted array is cnt bytes
        public static byte[] HexStringToByte(string str, int pos, int cnt)
        {
            String tmp = str.Replace(" ", "");
            if ((!ValidHexString(tmp)) || ((tmp.Length - pos) >> 1 < cnt))
            {
                return null;
            }

            byte[] data = new byte[cnt];

            for (int i = 0; i < cnt; i++)
            {
                data[i] = (byte)(HexStringToByte(tmp, 2 * i + pos) * 16 + HexStringToByte(tmp, 2 * i + pos + 1));
            }

            return data;
        }

        //Start from the pos position of the string and convert it to int
        public static int HexStringToInt(string s, int pos)
        {
            string str = "";
            int len = (s.Length - pos) > 8 ? 8 : s.Length - pos;
            for (int i = pos; i < len; i++)
            {
                str += s[i];
            }
            for (int i = len; i < 8; i++)
            {
                str = "0" + str;
            }

            if (!ValidHexString(str))
            {
                return 0;
            }

            int result = 0;
            byte[] data = HexStringToByte(str, 0, 4);

            for (int i = 0; i < 4; i++)
            {
                result = (result << 8) + data[i];
            }

            return result;
        }

        //Convert byte data to hexadecimal string
        public static string ByteToHexString(byte data)
        {
            string outString = "";

            if (data < 16)
            {
                outString += "0";
            }
            outString += data.ToString("X");

            return outString;
        }


        //Convert byte data to hexadecimal string
        public static string ByteToHexString(byte[] data, int pos, int length)
        {
            string outString = "";
            for (int i = pos; i < pos + length; i++)
            {
                outString += ByteToHexString(data[i]);
            }
            return outString;
        }

        //Convert byte data to hexadecimal string
        public static string ByteToHexString(byte[] data, int pos, int length, string space)
        {
            string outString = "";

            for (int i = pos; i < pos + length; i++)
            {
                outString += ByteToHexString(data[i]);
                if (i != pos + length - 1)
                {
                    outString += space;
                }
            }

            return outString;
        }





        //Convert numeric array to time string
        public static string ValueArrayToDateString(byte[] data, int length)
        {
            string outString = "";
            outString += string.Format("{0:D4}", (data[0] << 8) | data[1])+'-';
            outString += string.Format("{0:D2}", data[2])+'-';
            outString += string.Format("{0:D2}", data[3]) + ' ';
            outString += string.Format("{0:D2}", data[4]) + ':';
            outString += string.Format("{0:D2}", data[5]) + ':';
            outString += string.Format("{0:D2}", data[6]);
            return outString;
        }
        //Convert data of type ushort to hexadecimal string
        public static string ushortToHexString(ushort[] data, int pos, int length)
        {
            string outString = "";

            for (int i = pos; i < pos + length; i++)
            {
                outString += ByteToHexString((byte)(data[i] >> 8));
                outString += ByteToHexString((byte)(data[i] & 0xFF));
            }

            return outString;
        }

        //TODO: Hoang 26/05/2022 Thêm function để ghi tag
        public static string StringToHexString(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "");
            return hexString.ToString(); 
        }
        public static string HexStringToString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Default.GetString(bytes); 
        }
        //
        /// <summary>
        /// Write the data in the DataTable to a CSV file
        /// </summary>
        /// <param name="dt">Provide data storage DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(DataGridView dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //write column names
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].HeaderText.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //Write out each row of data
            //for (int i = 0; i < dt.Rows.Count; i++)
            foreach (DataGridViewRow row in dt.Rows)
            {
                data = "";
                for (int j = 0; j < row.Cells.Count; j++)
                {
                    string str;
                    try
                    {
                        str = row.Cells[j].Value.ToString();//Format as text
                    }
                    catch
                    {
                        str = "";
                    }
                    str = str.Replace("\"", "\"\"");//Replace English colons English colons need to be replaced with two colons
                    str = string.Format("\"{0}\"", str);

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }

        //Convert numeric array to string
        public static string ValueArrayToString(byte[] data, int length)
        {
            string outString = "";
            outString = Encoding.ASCII.GetString(data, 0, length);
            return outString;
        }
        //length指复制多少个U16的source
        public static void U16ArrayToByteArray(UInt16[] source,UInt32 source_index,ref byte[] datas,UInt32 des_index,UInt32 length)
        {
            int i;
            for (i = 0; i < length; i++)
            {
                datas[des_index + i * 2] = (byte)(source[source_index+i] >> 8);
                datas[des_index + i * 2 + 1] = (byte)(source[source_index+i] & 0x00ff);
            }
        }

        //U16变量赋值给Byte[]数组
        public static void U16ToByteArray(UInt16 source, ref byte[] datas, UInt32 des_index)
        {         
            datas[des_index ] = (byte)(source >> 8);
            datas[des_index + 1] = (byte)(source & 0x00ff);      
        }

        //两个Byte合成一个UInt16
        public static UInt16 ByteToU16(byte[] data, int pos)
        {
            
            return (UInt16)(data[pos] * 256 + data[pos + 1]);
        }

        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(List<string> dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            string str = "";

            //写出各行数据
            //for (int i = 0; i < dt.Rows.Count; i++)
            foreach (string ele in dt)
            {
                data = "";

                str = ele;//设置为文本格式
                str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                str = string.Format("\"{0}\"", str);

                data += str;
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
            //Encoding encoding = Common.GetType(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            StreamReader sr = new StreamReader(fs, Encoding.Unicode);
            //StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    try
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            if ((aryLine[j][0] == '"') && (aryLine[j][aryLine[j].Length - 1] == '"') && (aryLine[j].Length >= 2))
                            {
                                dr[j] = aryLine[j].Substring(1, aryLine[j].Length - 2);
                            }
                            else
                            {
                                dr[j] = aryLine[j];
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                    catch (Exception ex) { ex.ToString(); }
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
