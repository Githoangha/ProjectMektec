using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace LineGolden_PLasma
{
    public class Support_SQL
    {
       // string ConnectPath="D"
        
        #region BD Config and Setting
        public static int ExecuteQuery(string sqlQuery)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBConffig))
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
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBConffig))
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
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBConffig))
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

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBConffig))
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
            catch (Exception)
            {
            }
            return dt;
        }
        public static DataTable GetTableData_Test()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBConffig))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from Test", con))
                    {
                        con.Open();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        con.Close();
                    }

                }
            }
            catch (Exception)
            {
            }
            return dt;
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
                        cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";//Detele 
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
        public static int ExecuteQueryDBPlasma(string sqlQuery)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
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
                        Lib.SaveToLog("ErrorSQLite_Backup", "", ex.ToString());
                        return 0;
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
        public static object SaveStateUploadDataServer(int ProgramID, int IndexPlasma,int ID_Jig, string tagJigPlasma, string StateUpload,int CompletePlasma,int BeginBoxing)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE Plasma SET StateUploadServer = '{StateUpload}',CompletePlasma={CompletePlasma},CompleteBoxing={BeginBoxing} " +
                                    $"WHERE ID_Program = {ProgramID} AND " +
                                    $"ID_Plasma = {IndexPlasma} AND " +
                                    $"TagJigPlasma = '{tagJigPlasma}' AND ID={ID_Jig};" ;
                object value= cmd.ExecuteScalar();
                con.Close();
                return 1;
            }
            catch(Exception ex)
            {
                Lib.SaveToLog("ErrorSQLite", $"{tagJigPlasma}-ID{ID_Jig}", $"{ex.ToString()}");
                //MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        public static void SaveStateUploadDataServer(int ProgramID, int RfidIndex,int ID_Jig, string tagJigPlasma, string StateUpload)
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
                                    $"TagJigPlasma = '{tagJigPlasma}' AND ID={ID_Jig}";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void SaveStateUploadDataServer(int ProgramID, int RfidIndex, string tagJigPlasma, string numberTray, string StateUpload,string Cycletime)
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
        public static DataTable GetTableDataPlasma(string sqlQuery)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
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
                        con.Close();
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
            catch (Exception)
            {

            }
            return dt;
        }

        /// <summary>
        /// lấy Data ReadCode từ máy trước plassma
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static DataTable GetTableDataReadCode(string sqlQuery,string DBMachine)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(@"Data Source = \\" + DBMachine + "\\DB_ReadCode.db;Version=3;"))
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

            }
            return dt;
        }
        public static int ExecuteQuery(string sqlQuery,string connectdb)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(@"Data Source = \\" + connectdb + "\\DB_ReadCode.db;Version=3;"))
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
                        return 0;
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

        /// <summary>
        /// Truy xuất vào buffer DB lấy dữ liệu các jig vừa hoàn thành qua máy 
        /// </summary>
        /// <returns></returns>
        public static SQLiteDataReader LoadDataFromBufferPlasma(int IDProgram, int RfidIndex, string TagJig)
        {
            SQLiteDataReader dataReader;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT * FROM Plasma WHERE ID_Program = '{IDProgram}' AND ID_Plasma = '{RfidIndex}' AND TagJig ='{TagJig}' ORDER BY Date_Time ASC LIMIT 1";
                    dataReader = cmd.ExecuteReader();
                    con.Close();
                }
            }
            return dataReader;
        }
        /// <summary>
        /// Chèn vào buffer Plasma các thông tin của jig mới đọc ở đc tại đầu vào input Plasma
        /// </summary>
        public static bool SaveDataToBufferPlasma(int ProgramID, int RfidIndex, string tagJigTransfer, string tagJigPlasma, string codePCS, string numberTray)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        string dateIn = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                        cmd.CommandText = $"INSERT INTO Plasma(ID_Program, ID_Plasma, TagJigTransfer, TagJigPlasma, CodePCS,DateTimeInPlasma,Date_Time,NumberTray) VALUES('{ProgramID}', '{RfidIndex}', '{tagJigTransfer}','{tagJigPlasma}', '{codePCS}','{dateIn}','{dateIn}','{numberTray}')";
                        res = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception Ex)
                {

                }

                con.Close();
            }
            return res <= 0;
        }
        /// <summary>
        /// Chèn vào buffer Plasma các thông tin của jig mới đọc ở đc tại đầu vào input Plasma
        /// </summary>
        public static bool SaveDataToBufferPlasma_new(int ProgramID, int RfidIndex, string tagJigTransfer, string tagJigPlasma, string codePCS,string CodeTray,string LotID,string MPN)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        string dateIn =GlobVar.DateTimeIn= DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                        cmd.CommandText = $"INSERT INTO Plasma(ID_Program, ID_Plasma, TagJigTransfer, TagJigPlasma, CodePCS,DateTimeInPlasma,Date_Time,CodeTray,CompletePlasma,LotID,MPN) VALUES('{ProgramID}', '{RfidIndex}', '{tagJigTransfer}','{tagJigPlasma}', '{codePCS}','{dateIn}','{dateIn}','{CodeTray}',0,'{LotID}','{MPN}')";
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
        /// Lưu dữ liệu Data Plasma gồm ProgramID,PlamaIndex,tagJigTransfer,tagJigPlasma,Cycletime sau khi mở lồng
        /// </summary>
        public static bool UpdateDataPlasmaAfterTriggerUp(int ProgramID, int RfidIndex,  string tagJigPlasma,string Cycletime)
        {
            int res = 0;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"UPDATE Plasma SET Cycletime = '{Cycletime}' WHERE ID_Program = {ProgramID} AND ID_Plasma = {RfidIndex} AND TagJigPlasma = '{tagJigPlasma}'";
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res <= 0;
        }
        
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
        /// Update thông tin thời gian vào/ra máy plasma của jig
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="tagJigPlasma"></param>
        /// <param name="DatetimeOut"></param>
        /// <param name="DatetimeIn"></param>
        /// <param name="cycletime"></param>
        /// <returns></returns>
        public static bool UpdateDateTimePlasma(int ProgramID, int RfidIndex,int ID_Jig, string tagJigPlasma, string DatetimeOut, string DatetimeIn, string cycletime)
        {
            int res = 0;
            try
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source = " + Application.StartupPath + "\\Plasma\\DB_Plasma.db;Version=3;"))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = $"UPDATE Plasma SET DateTimeOutPlasma = '{DatetimeOut}',DateTimeInPlasma='{DatetimeIn}',Cycletime='{cycletime}' WHERE ID_Program = {ProgramID} AND ID_Plasma = {RfidIndex} AND TagJigPlasma = '{tagJigPlasma}' AND ID={ID_Jig}";
                        res = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Lib.SaveToLog("ErrorSQLite", $"{tagJigPlasma}", ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Update thông tin thời gian ra khỏi máy plasma của jig
        /// </summary>
        /// <param name="ProgramID"></param>
        /// <param name="RfidIndex"></param>
        /// <param name="tagJigPlasma"></param>
        /// <param name="dateTimeOutPlasma"></param>
        /// <returns></returns>
        public static bool UpdateDateTimeOutPlasma(int ProgramID, int RfidIndex,int ID_Jig,string tagJigPlasma,string dateTimeOutPlasma)
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
                                      $"ID={ID_Jig}";
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

        #region Function execute DB with LinkPathServer
        public static DataTable GetTableData(string sqlQuery, string FilePathServer)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection con = new SQLiteConnection($"Data Source={FilePathServer};Version=3;");
                SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con);

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
        #endregion

        #region Login
        public static DataTable GetTableDataUser(string sqlQuery)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(c_varGolbal.str_ConnectDBUsers))
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
            catch (Exception)
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
                SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + @"\Languages\Languages.db;Version=3;");
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
        #endregion

        #region view Time
        private static void ViewTime (Label lbTime)
        {
            while (true)
            {
                try
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
                }
                catch(Exception ex)
                {

                }
                Thread.Sleep(100);
            }
        }
        public static void ViewTimeLabel(Label lb)
        {
            Thread thread = new Thread(() =>  ViewTime(lb));
            thread.IsBackground = true;
            thread.Start();
        }
        #endregion

    }
}
