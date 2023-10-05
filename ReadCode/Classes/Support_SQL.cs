using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ReadCode
{
    public class Support_SQL
    {

        #region BD Config and Setting
        public static int ExecuteQuery(string sqlQuery)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            con.Open();
                            cmd.Connection = con;
                            cmd.CommandText = sqlQuery;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            return 1;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                return 0;
            }


        }
        public static int ExecuteDeleteQuery(string table, string field, object value)
        {
            try
            {
                string sqlQuery = string.Format(@"delete from {0} where {1} = {2}", table, field, value);
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return 1;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static object ExecuteScalar(string sqlQuery)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                        object value = cmd.ExecuteScalar();
                        con.Close();
                        return value;
                    }

                }
            }
            catch (Exception ex)
            {
                Lib.SaveToLog("ErrorSQL","",ex.ToString());
                return null;
            }
        }
        public static object ExecuteScalar(string sqlQuery,string str_Data)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(str_Data))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                        object value = cmd.ExecuteScalar();
                        con.Close();
                        return value;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataTable GetTableData(string sqlQuery)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
            {
                try
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con))
                    {
                        con.Open();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        con.Close();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }
        #endregion
        #region Buffer DB Readcode
        /// <summary>
        /// Lưu dữ liệu vào DB 
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="tagJigTransfer"></param>
        /// <param name="tagJigPlasma"></param>
        /// <param name="codePCS"></param>
        /// <returns></returns>
        public static bool SaveDataToBufferReadcode(int ProgramID, string Code_TagJigNonePCS, string Code_TagJigHavePCS, string codePCS,string datetime,string LotID,string MPN)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
            {

                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = $"INSERT INTO Readcode(ID_Program, Code_TagJigNonePCS, Code_TagJigHavePCS, CodePCS, Date_Time,StatusUpload,LotID,MPN) VALUES('{ProgramID}', '{Code_TagJigNonePCS}', '{Code_TagJigHavePCS}', '{codePCS}', '{datetime}',false,'{LotID}','{MPN}')";
                        res = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception Ex)
                {

                }
                finally
                {
                    con.Close();
                }


            }
            return res <= 0;
        }
        public static bool SaveDataToBufferReadcode(int ProgramID, string Code_TagJigNonePCS, string Code_TagJigHavePCS, string codePCS, string datetime,string LotID,string MPN,string str_Data)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection(str_Data))
            {

                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = $"INSERT INTO Readcode(ID_Program, Code_TagJigNonePCS, Code_TagJigHavePCS, CodePCS, Date_Time,StatusUpload,LotID,MPN) VALUES('{ProgramID}', '{Code_TagJigNonePCS}', '{Code_TagJigHavePCS}', '{codePCS}', '{datetime}',false,'{LotID}','{MPN}')";
                        res = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception Ex)
                {

                }
                finally
                {
                    con.Close();
                }


            }
            return res <= 0;
        }

        public static bool SaveDataToBufferReadcode(int ProgramID, string Code_TagJigNonePCS, string Code_TagJigHavePCS, string codePCS, string datetime, int Nolock, int QualifyJig,string LotID,string MPN)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
            {

                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = $"INSERT INTO Readcode(ID_Program, Code_TagJigNonePCS, Code_TagJigHavePCS, CodePCS, Date_Time,StatusUpload,NoLock,QualifyJig,LotID,MPN) VALUES('{ProgramID}', '{Code_TagJigNonePCS}', '{Code_TagJigHavePCS}', '{codePCS}', '{datetime}',false,{Nolock},{QualifyJig},'{LotID}','{MPN}')";
                        res = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception Ex)
                {

                }
                finally
                {
                    con.Close();
                }


            }
            return res <= 0;
        }
        public static bool SaveDataToBufferReadcode(int ProgramID, string Code_TagJigNonePCS, string Code_TagJigHavePCS, string codePCS, string datetime, int Nolock, int QualifyJig,string LotID,string MPN,string str_Data)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection(str_Data))
            {

                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = $"INSERT INTO Readcode(ID_Program, Code_TagJigNonePCS, Code_TagJigHavePCS, CodePCS, Date_Time,StatusUpload,NoLock,QualifyJig,LotID,MPN) VALUES('{ProgramID}', '{Code_TagJigNonePCS}', '{Code_TagJigHavePCS}', '{codePCS}', '{datetime}',false,{Nolock},{QualifyJig},'{LotID}','{MPN}')";
                        res = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception Ex)
                {

                }
                finally
                {
                    con.Close();
                }


            }
            return res <= 0;
        }


        public static bool UpdatePcsToDB_FVI_SERVER(string Code_TagJigNonePCS_Staff_2,string codePCS)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBConffig_FVI_Server))
            {

                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = $"Update Readcode set CodePCS='{codePCS}',StatusFVI=True WHERE (CodePCS IS NULL OR CodePCS = '') AND StatusType_FVI_Upload=2 AND Code_TagJigHavePCS='{Code_TagJigNonePCS_Staff_2}'";
                        res = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception Ex)
                {

                }
                finally
                {
                    con.Close();
                }


            }
            return res <= 0;
        }


        /// <summary>
        /// Lấy dữ liệu từ DB Readcode định dạng DataReader
        /// </summary>
        /// <param name="IDProgram"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="TagJig"></param>
        /// <returns></returns>
        public static SQLiteDataReader LoadDataFromBufferReadCode(int IDProgram, int RfidIndex, string tagJigPlasma)
        {
            SQLiteDataReader dataReader;
            using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT * FROM Readcode WHERE ID_Program = '{IDProgram}' AND ID_Readcode = '{RfidIndex}' AND TagJigPlasma ='{tagJigPlasma}' ORDER BY Date_Time ASC LIMIT 1";
                    dataReader = cmd.ExecuteReader();
                    con.Close();
                }
            }
            return dataReader;
        }
        /// <summary>
        /// Lấy dữ liệu từ DB Readcode định dạng DataTable
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static DataTable GetTableDataReadCode(string sqlQuery)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con))
                    {
                        con.Open();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public static DataTable GetTableDataReadCode(string sqlQuery,string str_Data)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(str_Data))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con))
                    {
                        con.Open();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        /// <summary>
        /// Update status
        /// </summary>
        /// <param name="ID_Progarm"></param>
        /// <param name="Code_TagJigNonePCS"></param>
        /// <param name="Code_TagJigHavePCS"></param>
        /// <param name="StatusUpload"></param>
        public static void SaveStateUploadDataReadCode(int ID_Progarm, string Code_TagJigNonePCS, string Code_TagJigHavePCS, string StatusUpload)
        {

            SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB);
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE Readcode SET StatusUpload ='{StatusUpload}' " +
                                    $"WHERE ID_Program = {ID_Progarm} AND " +
                                    $"Code_TagJigNonePCS = '{Code_TagJigNonePCS}' AND " +
                                    $"Code_TagJigHavePCS = '{Code_TagJigHavePCS}' AND ";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public static void SaveStateUploadByID(int ID,string StatusUpload)
        {

            SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB);
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE Readcode SET StatusUpload ='{StatusUpload}' " +
                                    $"WHERE ID_Program = {ID} AND ";
                                    
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// Xóa dữ liệu trong DB ReadCode 
        /// </summary>
        /// <param name="IDProgram"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="TagJig"></param>
        /// <returns></returns>
        public static bool DeleteDataBufferReadCode(int IDProgram, int RfidIndex, string tagJigPlasma)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"DELETE FROM Readcode WHERE ID_Program = {IDProgram} AND ID_Readcode = {RfidIndex} AND TagJigPlasma = '{tagJigPlasma}'";
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res <= 0;
        }
        #endregion
        #region Buffer DB Plasma

        public static object ExecuteScalarDBPlasma(string sqlQuery)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                        object value = cmd.ExecuteScalar();
                        con.Close();
                        return value;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void SaveStateUploadDataServer(int ProgramID, int RfidIndex, string tagJigPlasma, string numberTray, string StateUpload)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE Plasma SET StateUploadServer = '{StateUpload}' " +
                                    $"WHERE ID_Program = {ProgramID} AND " +
                                    $"ID_Plasma = {RfidIndex} AND " +
                                    $"TagJigPlasma = '{tagJigPlasma}' AND " +
                                    $"NumberTray='{numberTray}'";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static void SaveStateUploadDataServer(int ProgramID, int RfidIndex, string tagJigPlasma, string numberTray, string StateUpload, string Cycletime)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE Plasma SET StateUploadServer = '{StateUpload}', Cycletime='{Cycletime}' " +
                                    $"WHERE ID_Program = {ProgramID} AND " +
                                    $"ID_Plasma = {RfidIndex} AND " +
                                    $"TagJigPlasma = '{tagJigPlasma}' AND " +
                                    $"NumberTray='{numberTray}'";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //public static DataTable GetTableDataPlasma(string sqlQuery)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //        {
        //            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con))
        //            {
        //                con.Open();
        //                da.Fill(ds);
        //                dt = ds.Tables[0];
        //                con.Close();
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return dt;
        //}

        /// <summary>
        /// 
        /// Function Chèn mã của từng tray khi đi vào máy plasma
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="ID_Tray"></param>
        //public static void SaveCodeTrayOfPlasma(int ProgramID, int RfidIndex,string ID_Tray,string position)
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        con.Open();
        //        try
        //        {
        //            using (SQLiteCommand cmd = new SQLiteCommand())
        //            {
        //                cmd.Connection = con;
        //                string dateIn = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        //                cmd.CommandText = $"INSERT INTO Tray (ID_Prg," +
        //                                                    $"ID_Rfid," +
        //                                                    $"ID_Tray," +
        //                                                    $"Date_Time_Tray," +
        //                                                    $"Position) " +
        //                                                    $"VALUES ({ProgramID}," +
        //                                                    $"{RfidIndex}," +
        //                                                    $"'{ID_Tray}'," +
        //                                                    $"'{dateIn}'," +
        //                                                    $"'{position}')";
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            MessageBox.Show(Ex.Message);
        //        }
        //        con.Close();
        //    }
        //}
        /// <summary>
        /// Update Position Tray follow oldPosition
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="OldPosition"></param>
        /// <param name="NewPosition"></param>
        //public static void UpdatePositionTrayOfPlasma(int ProgramID, int RfidIndex, string OldPosition,string NewPosition)
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        con.Open();
        //        try
        //        {
        //            using (SQLiteCommand cmd = new SQLiteCommand())
        //            {
        //                cmd.Connection = con;
        //                string dateIn = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        //                cmd.CommandText = $"UPDATE Tray SET Position='{NewPosition}' WHERE Position='{OldPosition}' AND ID_Prg={ProgramID} AND ID_Rfid={RfidIndex} ";
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            MessageBox.Show(Ex.Message);
        //        }
        //        con.Close();
        //    }
        //}

        /// <summary>
        /// Update Position Tray follow ID_Tray
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="ID_Tray"></param>
        /// <param name="NewPosition"></param>
        /// <param name="OldPosition"></param>
        //public static void UpdatePositionTrayOfPlasma(int ProgramID, int RfidIndex, string ID_Tray,string NewPosition,string OldPosition)
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        con.Open();
        //        try
        //        {
        //            using (SQLiteCommand cmd = new SQLiteCommand())
        //            {
        //                cmd.Connection = con;
        //                string dateIn = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        //                cmd.CommandText = $"UPDATE Tray SET Position='{NewPosition}' WHERE ID_Tray='{ID_Tray}' AND ID_Prg={ProgramID} AND ID_Rfid={RfidIndex} AND Position='{OldPosition}' ";
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            MessageBox.Show(Ex.Message);
        //        }
        //        con.Close();
        //    }
        //}


        /// <summary>
        /// Function Xóa tray đã hoàn thành quá trình Plasma dựa vào mã tray
        /// </summary>
        /// <param name="IDProgram"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="ID_Tray"></param>
        //public static void DeleteTrayFinish(int IDProgram, int RfidIndex, string ID_Tray)
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        using (SQLiteCommand cmd = new SQLiteCommand())
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandText = $"DELETE FROM Tray WHERE ID_Prg = {IDProgram} AND ID_Rfid = {RfidIndex} AND ID_Tray = '{ID_Tray}'";
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //    }
        //}


        /// <summary>
        /// Function Xóa mã tray trong DB
        /// </summary>
        /// <param name="RfidIndex"></param>
        //public static void DeleteAllTray(int RfidIndex)
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        using (SQLiteCommand cmd = new SQLiteCommand())
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandText = $"DELETE FROM Tray WHERE ID_Rfid={RfidIndex}";
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //    }
        //}

        /// <summary>
        /// xóa toàn bộ dữ liệu của tray dùng khi start
        /// </summary>
        //public static void DeleteAllTray()
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        using (SQLiteCommand cmd = new SQLiteCommand())
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandText = $"DELETE FROM Tray";
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //    }
        //}

        /// <summary>
        /// Truy xuất vào buffer DB lấy dữ liệu các jig vừa hoàn thành qua máy 
        /// </summary>
        /// <returns></returns>
        //public static SQLiteDataReader LoadDataFromBufferPlasma(int IDProgram, int RfidIndex, string TagJig)
        //{
        //    SQLiteDataReader dataReader;
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        using (SQLiteCommand cmd = new SQLiteCommand())
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandText = $"SELECT * FROM Plasma WHERE ID_Program = '{IDProgram}' AND ID_Plasma = '{RfidIndex}' AND TagJig ='{TagJig}' ORDER BY Date_Time ASC LIMIT 1";
        //            dataReader = cmd.ExecuteReader();
        //            con.Close();
        //        }
        //    }
        //    return dataReader;
        //}
        /// <summary>
        /// Chèn vào buffer Plasma các thông tin của jig mới đọc ở đc tại đầu vào input Plasma
        /// </summary>
        //public static bool SaveDataToBufferPlasma(int ProgramID, int RfidIndex, string tagJigTransfer, string tagJigPlasma, string codePCS, string numberTray)
        //{
        //    int res = 0;
        //    using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
        //    {
        //        con.Open();
        //        try
        //        {
        //            using (SQLiteCommand cmd = new SQLiteCommand())
        //            {
        //                cmd.Connection = con;
        //                string dateIn = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        //                cmd.CommandText = $"INSERT INTO Plasma(ID_Program, ID_Plasma, TagJigTransfer, TagJigPlasma, CodePCS,DateTimeInPlasma,Date_Time,NumberTray) VALUES('{ProgramID}', '{RfidIndex}', '{tagJigTransfer}','{tagJigPlasma}', '{codePCS}','{dateIn}','{dateIn}','{numberTray}')";
        //                res = cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception Ex)
        //        {

        //        }

        //        con.Close();
        //    }
        //    return res <= 0;
        //}
        /// <summary>
        /// Xóa toàn bộ dữ liệu trong buffer
        /// </summary>
        public static bool ClearAllBufferPlasma()
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_ConfigDevice.db;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"DELETE FROM Plasma";
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res <= 0;
        }
        /// <summary>
        /// Clear Record Tag after upload server plasma finish
        /// </summary>
        /// <param name="tag_Base"></param>
        /// <param name="tag_Cover"></param>
        public static bool ClearRecordTagFinish(int IDProgram, string TagJigPlasma)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"DELETE FROM Plasma WHERE ID_Program = {IDProgram} AND TagJigPlasma = '{TagJigPlasma}'";
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res <= 0;
        }
        /// <summary>
        /// Update thông tin thời gian vào máy plasma của jig
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="tagJigTransfer"></param>
        /// <param name="tagJigPlasma"></param>
        /// <param name="dateTimeInPlasma"></param>
        /// <returns></returns>
        public static bool UpdateDateTimeInPlasma(int ProgramID, int RfidIndex, string tagJigPlasma, string dateTimeInPlasma)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;

                    cmd.CommandText = $"UPDATE Plasma SET DateTimeInPlasma = '{dateTimeInPlasma}' WHERE ID_Program = {ProgramID} AND ID_Plasma = {RfidIndex} AND TagJigPlasma = '{tagJigPlasma}'";
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res <= 0;
        }
        /// <summary>
        /// Update thông tin thời gian ra khỏi máy plasma của jig
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="tagJigPlasma"></param>
        /// <param name="dateTimeOutPlasma"></param>
        /// <returns></returns>
        public static bool UpdateDateTimeOutPlasma(int ProgramID, int RfidIndex, string tagJigPlasma, string numberTray, string dateTimeOutPlasma)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;

                    cmd.CommandText = $"UPDATE Plasma SET DateTimeOutPlasma = '{dateTimeOutPlasma}' " +
                                      $"WHERE ID_Program = {ProgramID} " +
                                      $"AND ID_Plasma = {RfidIndex} AND " +
                                      $"TagJigPlasma = '{tagJigPlasma}' AND " +
                                      $"NumberTray='{numberTray}'";
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res <= 0;
        }
        /// <summary>
        /// kiểm tra số lượng các jig còn trong trong buffer theo các máy plasma
        /// </summary>
        /// <param name="num_Plasma"></param>
        /// <param name="num_Jig"></param>
        /// <returns></returns>
        //public static bool RefreshCheckBuferPlasma(in int num_Plasma , out int[,] num_Jig)
        //{

        //    //using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_ConfigDevice.db;Version=3;"))
        //    //{
        //    //    using (SQLiteCommand cmd = new SQLiteCommand())
        //    //    {
        //    //        con.Open();
        //    //        cmd.Connection = con;
        //    //        for (int i = 1; i <= num_Plasma; i++)
        //    //        {
        //    //            cmd.CommandText = $"SELECT * FROM Plasma WHERE ID_Plasma = {i}";
        //    //            using (SQLiteDataReader dataReader = cmd.ExecuteReader())
        //    //            {
        //    //                if (dataReader.HasRows)
        //    //                {
        //    //                    while (dataReader.Read())
        //    //                    {
        //    //                        num_Jig[i] dataReader.StepCount;
        //    //                        //ID_Plasma = dataReader.GetInt32(0).ToString();
        //    //                        //TagBase = dataReader.GetString(1);
        //    //                        //TagCover = dataReader.GetString(2);
        //    //                        //TagJig = dataReader.GetString(3);
        //    //                        //DateTime = dataReader.GetString(4);
        //    //                        //Step_Plasma = 10;// Next Step
        //    //                    }
        //    //                }
        //    //            }
        //    //        }

        //    //        con.Close();
        //    //    }
        //    //}
        //    //return dataReader;
        //}
        #endregion
        #region Login
        public static DataTable GetTableDataUser(string sqlQuery)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con))
                    {
                        con.Open();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public static object ExecuteScalarUser(string sqlQuery)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBUsers))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                        object value = cmd.ExecuteScalar();
                        con.Close();
                        return value;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Languages
        /// <summary>
        /// Lấy dữ liệu đổi ngôn ngữ từ Database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetTableDataLanguages(string sql)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDB);
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con);

                con.Open();
                da.Fill(ds);
                dt = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return dt;
        }
        /// <summary>
        /// Function thực hiện đổi ngôn ngữ
        /// </summary>
        /// <param name="dtLang"></param>
        /// <param name="control"></param>
        /// <param name="langeName"></param>
        private static void ChangeLang_Execute(DataTable dtLang, Control.ControlCollection control, string langeName)
        {
            foreach (Control item in control)
            {

                if (item.GetType() == typeof(DataGridView))
                {
                    DataGridView gridView = ((DataGridView)item);
                    foreach (DataGridViewColumn column in gridView.Columns)
                    {
                        DataRow row = dtLang.Rows.Find(column.Name);
                        if (row != null)
                            column.HeaderText = ToString(row[langeName]);
                    }
                }

                if (item.Tag != null)
                {
                    DataRow row = dtLang.Rows.Find(item.Tag.ToString());
                    if (row != null)
                        item.Text = ToString(row[langeName]);
                }

                if (item.Controls != null)
                    ChangeLang_Execute(dtLang, item.Controls, langeName);
            }
        }
        /// <summary>
        /// Function đổi ngôn ngữ
        /// </summary>
        /// <param name="control"></param>
        /// <param name="langeName"></param>
        public static void ChangeLang(Control.ControlCollection control, string langeName)
        {
            string sql = "SELECT *FROM Languages";
            DataTable dt = GetTableDataLanguages(sql);
            var keys = new DataColumn[1];
            keys[0] = dt.Columns[ColName.NameTag];
            dt.PrimaryKey = keys;
            ChangeLang_Execute(dt, control, langeName);
            GlobVar.LangChoose = langeName;
        }
        #endregion

        #region Chuyển kiểu, ép kiểu

        /// <summary>
        /// Chuyển giá trị sang kiểu String
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToString(object x)
        {
            if (x != null)
            {
                return x.ToString().Trim();
            }
            return "";
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu integer
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static int ToInt(object x)
        {
            try
            {
                return Convert.ToInt32(x);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Chuyển giá trị sang kiểu Decimal
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static Decimal ToDecimal(object x)
        {
            try
            {
                return Convert.ToDecimal(x);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu bool
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static bool ToBoolean(object x)
        {
            try
            {
                return Convert.ToBoolean(x);
            }
            catch
            {
                return false;
            }
        }

        #endregion
        #region view Time
        private static void ViewTime(Label lbTime)
        {
            while (true)
            {
                if (lbTime.InvokeRequired)
                {
                    lbTime.Invoke((MethodInvoker)delegate
                    {
                        lbTime.Text = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
                    });
                }
                else
                {
                    lbTime.Text = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
                }
                Thread.Sleep(100);
            }
        }
        public static void ViewTimeLabel(Label lb)
        {
            Thread thread = new Thread(() => ViewTime(lb));
            thread.IsBackground = true;
            thread.Start();
        }
        #endregion

        public static void saveToLog(string err)
        {
            try
            {
                string path = Application.StartupPath + @"\ErrorConnectBarcodeVisionFail";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "";
                fileName = path + "\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
                string fullMessage = string.Empty;
                fullMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " - ERROR" + "\n" + "Message lỗi: " + "\n" + err;
                if (!File.Exists(fileName))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine(fullMessage);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(fullMessage);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
