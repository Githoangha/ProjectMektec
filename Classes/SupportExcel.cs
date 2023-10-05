using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//using xl = Microsoft.Office.Interop.Excel;
using GemBox.Spreadsheet;

namespace LineGolden_PLasma
{
    class SupportExcel
    {
        Hashtable sheets;
        string pathSaveCSV = "";
        string pathSaveXLSX = "";
        /// <summary>
        /// Create file .csv . 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        public bool CreatFileExcel_CSV(string Path, string NameFile)
        {
            try
            {
                string sourceFile = System.IO.Path.Combine(Application.StartupPath, "Temp.csv");
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                string destFile = System.IO.Path.Combine(Path, NameFile + ".csv");
                File.Copy(sourceFile, destFile, true);
                pathSaveXLSX = destFile;
                pathSaveCSV = destFile;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi không thể tạo được file Excel " + e.ToString());
                return false;
            }
        }
        /// <summary>
        /// Create file .xlsx 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        public bool CreatFileExcel_XLSX(string Path, string NameFile)
        {
            try
            {
                string sourceFile = System.IO.Path.Combine(Application.StartupPath, "Temp.xlsx");
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                string destFile = System.IO.Path.Combine(Path, NameFile + ".xlsx");
                File.Copy(sourceFile, destFile, true);
                pathSaveXLSX = destFile;
                pathSaveCSV = destFile;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi không thể tạo được file Excel " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Open file Excel
        /// </summary>
       
        
        public string PathFileLog
        {
            get { return pathSaveCSV; }
        }
        /// <summary>
        /// kiểm tra file có sử dụng hay không
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool FileIsUsed(string filename)
        {
            bool Locked = false;
            try
            {
                FileStream fs =
                    File.Open(filename, FileMode.OpenOrCreate,
                        FileAccess.ReadWrite, FileShare.None);
                fs.Close();
            }
            catch (IOException)
            {
                Locked = true;
            }

            return Locked;
        }

        #region Ghi xuống file .csv vừa tạo
        /// <summary>
        /// Ghi xuống file .csv vừa tạo
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="model"></param>
        /// <param name="lotID"></param>
        /// <param name="lineID"></param>
        /// <param name="deviceID"></param>
        /// <param name="namePlasma"></param>
        /// <param name="list_DataPlasma"></param>
        /// <returns></returns>
        public bool WriteData_csv(string staffID, string model, string lotID, string lineID, string deviceID, string namePlasma, List<dataPlasma> list_DataPlasma)
        {

            try
            {
                if (FileIsUsed(pathSaveCSV))
                {
                    return false;
                }
                SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
                ExcelFile workbook = ExcelFile.Load(pathSaveCSV);
                ExcelWorksheet worksheet = workbook.Worksheets[0];
                CellRange a = worksheet.GetUsedCellRange(false);
                int lastUsedCol = a.LastColumnIndex + 1;
                int lastUsedRow = a.LastRowIndex;


                //worksheet.Cells[0, 1].Value = staffID;
                //worksheet.Cells[1, 1].Value = model;
                //worksheet.Cells[2, 1].Value = lotID;
                //worksheet.Cells[0, 5].Value = lineID;
                //worksheet.Cells[1, 5].Value = deviceID;
                //worksheet.Cells[2, 5].Value = namePlasma;
                for (int i = 0; i < list_DataPlasma.Count; i++)
                {
                    worksheet.Cells[lastUsedRow + 1 + i, 0].Value = list_DataPlasma[i].TagJigTransfer;
                    worksheet.Cells[lastUsedRow + 1 + i, 1].Value = list_DataPlasma[i].TagJigPlasma;
                    List<string> lst_PCS = list_DataPlasma[i].PcsBarcode.Split(',').ToList();
                    if (lst_PCS.Count > 1)
                    {
                        for (int j = 0; j <= lst_PCS.Count; j++)
                        {
                            if (j == lst_PCS.Count)
                            {
                                worksheet.Cells[lastUsedRow + 1 + i + j, 2].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 3].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 4].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 5].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 6].Value = " ";
                            }
                            else
                            {
                                worksheet.Cells[lastUsedRow + 1 + i + j, 2].Value = lst_PCS[j];
                                worksheet.Cells[lastUsedRow + 1 + i + j, 3].Value = list_DataPlasma[i].DateTimeInPlasma;
                                worksheet.Cells[lastUsedRow + 1 + i + j, 4].Value = list_DataPlasma[i].DateTimeOutPlasma;
                                worksheet.Cells[lastUsedRow + 1 + i + j, 5].Value = list_DataPlasma[i].CycleTime;
                                worksheet.Cells[lastUsedRow + 1 + i + j, 6].Value = list_DataPlasma[i].StatusPlasma;
                            }


                        }
                        lastUsedRow = lastUsedRow + lst_PCS.Count - 1;
                    }
                    else
                    {
                        worksheet.Cells[lastUsedRow + 1 + i, 2].Value = list_DataPlasma[i].PcsBarcode;
                        worksheet.Cells[lastUsedRow + 1 + i, 3].Value = list_DataPlasma[i].DateTimeInPlasma;
                        worksheet.Cells[lastUsedRow + 1 + i, 4].Value = list_DataPlasma[i].DateTimeOutPlasma;
                        worksheet.Cells[lastUsedRow + 1 + i, 5].Value = list_DataPlasma[i].CycleTime;
                        worksheet.Cells[lastUsedRow + 1 + i, 6].Value = list_DataPlasma[i].StatusPlasma;
                    }
                }
                workbook.Save(pathSaveCSV);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }

        }
        #endregion

        #region Ghi xuống file .csv đã tồn tại
        /// <summary>
        /// Ghi xuống file .csv đã tồn tại
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="model"></param>
        /// <param name="lotID"></param>
        /// <param name="lineID"></param>
        /// <param name="deviceID"></param>
        /// <param name="namePlasma"></param>
        /// <param name="list_DataPlasma"></param>
        /// <param name="pathExcel"></param>
        /// <returns></returns>
        public bool WriteData_csv(string staffID, string model, string lotID, string lineID, string deviceID, string namePlasma, List<dataPlasma> list_DataPlasma, string pathExcel)
        {
            try
            {
                if (FileIsUsed(pathExcel))
                {
                    return false;
                }
                SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
                ExcelFile workbook = ExcelFile.Load(pathExcel);
                ExcelWorksheet worksheet = workbook.Worksheets[0];
                CellRange a = worksheet.GetUsedCellRange(false);
                int lastUsedCol = a.LastColumnIndex + 1;
                int lastUsedRow = a.LastRowIndex;

                worksheet.Cells[0, 1].Value = staffID;
                worksheet.Cells[1, 1].Value = model;
                worksheet.Cells[2, 1].Value = lotID;
                worksheet.Cells[0, 5].Value = lineID;
                worksheet.Cells[1, 5].Value = deviceID;
                worksheet.Cells[2, 5].Value = namePlasma;
                for (int i = 0; i < list_DataPlasma.Count; i++)
                {
                    worksheet.Cells[lastUsedRow + 1 + i, 0].Value = list_DataPlasma[i].TagJigTransfer;
                    worksheet.Cells[lastUsedRow + 1 + i, 1].Value = list_DataPlasma[i].TagJigPlasma;
                    List<string> lst_PCS = list_DataPlasma[i].PcsBarcode.Split(',').ToList();
                    if (lst_PCS.Count > 1)
                    {
                        for (int j = 0; j <= lst_PCS.Count; j++)
                        {
                            if (j == lst_PCS.Count)
                            {
                                worksheet.Cells[lastUsedRow + 1 + i + j, 2].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 3].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 4].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 5].Value = " ";
                                worksheet.Cells[lastUsedRow + 1 + i + j, 6].Value = " ";
                            }
                            else
                            {
                                worksheet.Cells[lastUsedRow + 1 + i + j, 2].Value = lst_PCS[j];
                                worksheet.Cells[lastUsedRow + 1 + i + j, 3].Value = list_DataPlasma[i].DateTimeInPlasma;
                                worksheet.Cells[lastUsedRow + 1 + i + j, 4].Value = list_DataPlasma[i].DateTimeOutPlasma;
                                worksheet.Cells[lastUsedRow + 1 + i + j, 5].Value = list_DataPlasma[i].CycleTime;
                                worksheet.Cells[lastUsedRow + 1 + i + j, 6].Value = list_DataPlasma[i].StatusPlasma;
                            }
                        }
                        lastUsedRow = lastUsedRow + lst_PCS.Count - 1;
                    }
                    else
                    {
                        worksheet.Cells[lastUsedRow + 1 + i, 2].Value = list_DataPlasma[i].PcsBarcode;
                        worksheet.Cells[lastUsedRow + 1 + i, 3].Value = list_DataPlasma[i].DateTimeInPlasma;
                        worksheet.Cells[lastUsedRow + 1 + i, 4].Value = list_DataPlasma[i].DateTimeOutPlasma;
                        worksheet.Cells[lastUsedRow + 1 + i, 5].Value = list_DataPlasma[i].CycleTime;
                        worksheet.Cells[lastUsedRow + 1 + i, 6].Value = list_DataPlasma[i].StatusPlasma;
                    }
                }
                workbook.Save(pathExcel);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }

        }
        #endregion



        #region Ghi xuống file .xlsx vừa tạo
        /// <summary>
        ///  Ghi xuống file .xlsx vừa tạo
        /// </summary>
        /// <param name="numberJig">Số lượng jig trên 1 tray </param>
        /// <param name="list_DataPlasma"></param>
        /// <returns></returns>
        public bool WriteData_xlsx_gem(List<dataPlasma> list_DataPlasma)
        {

            try
            {
                if (list_DataPlasma.Count <= 0) return false;
                if (FileIsUsed(pathSaveXLSX))
                {
                    return false;
                }
                SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
                ExcelFile workbook = ExcelFile.Load(pathSaveXLSX);
                ExcelWorksheet worksheet = workbook.Worksheets[0];
                CellRange a = worksheet.GetUsedCellRange(false);
                int lastUsedCol = a.LastColumnIndex ;
                int lastUsedRow = a.LastRowIndex;

                List<string> JigID2 = list_DataPlasma[0].CodeTray.Split(',').ToList();
                JigID2.Remove("");
                int first=0;
                for (int i = 0; i < list_DataPlasma.Count; i++)
                {

                    //do chỉ có 2 code tray nên chỉ ghi vào 2 dòng 
                    if (i == 0)
                    {
                        worksheet.Cells[lastUsedRow + i, 4].Value =JigID2[0].ToString();
                        worksheet.Cells[lastUsedRow + i, 0].Value = 1;
                        first = lastUsedRow;
                    }

                    //HA thêm phần này để khi chỉ có 1 Jig nhưng vẫn điền đủ 2 code Tray
                    if (list_DataPlasma.Count == 1)
                    {
                        worksheet.Cells[lastUsedRow + i + 1, 4].Value = JigID2[1].ToString();
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 1, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 4, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 7, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 10, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 13, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 16, 2, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 18, 2, 1).Merged = true;
                    }
                    ////////

                    if (i==1)
                    {
                        worksheet.Cells[lastUsedRow + i, 4].Value = JigID2[1].ToString();
                    }       
                    worksheet.Cells[lastUsedRow + i, 1].Value = list_DataPlasma[i].TagJigPlasma;
                    worksheet.Cells[lastUsedRow + i, 7].Value = list_DataPlasma[i].PcsBarcode;
                    worksheet.Cells[lastUsedRow + i, 10].Value = list_DataPlasma[i].DateTimeInPlasma;
                    worksheet.Cells[lastUsedRow + i, 13].Value = list_DataPlasma[i].DateTimeOutPlasma;
                    worksheet.Cells[lastUsedRow + i, 16].Value = list_DataPlasma[i].CycleTime;
                    worksheet.Cells[lastUsedRow + i, 18].Value = list_DataPlasma[i].StatusPlasma;

                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 1, 3, 1).Merged=true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 4, 3, 1).Merged=true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 7, 3, 1).Merged=true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 10, 3, 1).Merged=true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 13, 3, 1).Merged=true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 16, 2, 1).Merged=true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 18, 2, 1).Merged = true;
                }

                worksheet.Cells.GetSubrangeRelative(first, 0, 1, list_DataPlasma.Count).Merged=true;
                workbook.Save(pathSaveXLSX);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }

        }
        #endregion

        #region Ghi xuống file .xlsx đã tồn tại
        /// <summary>
        /// Ghi xuống file .xlsx đã tồn tại
        /// </summary>
        /// <param name="numberJig"></param>
        /// <param name="list_DataPlasma"></param>
        /// <param name="pathExcel"></param>
        /// <returns></returns>
        public bool WriteData_xlsx_gem(List<dataPlasma> list_DataPlasma, string pathExcel)
        {
            try
            {
                if (FileIsUsed(pathExcel))
                {
                    return false;
                }
                SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
                ExcelFile workbook = ExcelFile.Load(pathExcel);
                ExcelWorksheet worksheet = workbook.Worksheets[0];
                CellRange a = worksheet.GetUsedCellRange(false);
                
                int lastUsedCol = a.LastColumnIndex + 1;
                int lastUsedRow = a.LastRowIndex+1;

                List<string> JigID2 = list_DataPlasma[0].CodeTray.Split(',').ToList();
                JigID2.Remove("");
                int first = 0;
                for (int i = 0; i < list_DataPlasma.Count; i++)
                {

                    //do chỉ có 2 code tray nên chỉ ghi vào 2 dòng 
                    if (i == 0)
                    {
                        for(int count = 1; count < a.LastRowIndex - 4; count++)
                        {
                            if(!string.IsNullOrEmpty(worksheet.Cells[lastUsedRow-count, 0].Value?.ToString()))
                            {
                                int va = Support_SQL.ToInt(worksheet.Cells[lastUsedRow - count, 0].Value);
                                worksheet.Cells[lastUsedRow + i, 0].Value = va + 1;
                                worksheet.Cells[lastUsedRow + i, 4].Value = JigID2[0].ToString();
                                first = lastUsedRow;
                                break;
                            }
                        }
                        
                    }

                    //HA thêm phần này để khi chỉ có 1 Jig nhưng vẫn điền đủ 2 code Tray
                    if (list_DataPlasma.Count == 1)
                    {
                        worksheet.Cells[lastUsedRow + i + 1, 4].Value = JigID2[1].ToString();
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 1, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 4, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 7, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 10, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 13, 3, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 16, 2, 1).Merged = true;
                        worksheet.Cells.GetSubrangeRelative(lastUsedRow + i + 1, 18, 2, 1).Merged = true;
                    }
                    ////////

                    if (i == 1)
                    {
                        worksheet.Cells[lastUsedRow + i, 4].Value = JigID2[1].ToString();
                    }
                    worksheet.Cells[lastUsedRow+ i, 1].Value = list_DataPlasma[i].TagJigPlasma;
                    worksheet.Cells[lastUsedRow + i, 7].Value = list_DataPlasma[i].PcsBarcode;
                    worksheet.Cells[lastUsedRow + i, 10].Value = list_DataPlasma[i].DateTimeInPlasma;
                    worksheet.Cells[lastUsedRow + i, 13].Value = list_DataPlasma[i].DateTimeOutPlasma;
                    worksheet.Cells[lastUsedRow + i, 16].Value = list_DataPlasma[i].CycleTime;
                    worksheet.Cells[lastUsedRow + i, 18].Value = list_DataPlasma[i].StatusPlasma;

                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 1, 3, 1).Merged = true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 4, 3, 1).Merged = true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 7, 3, 1).Merged = true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 10, 3, 1).Merged = true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 13, 3, 1).Merged = true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 16, 2, 1).Merged = true;
                    worksheet.Cells.GetSubrangeRelative(lastUsedRow + i, 18, 2, 1).Merged = true;
                }
                worksheet.Cells.GetSubrangeRelative(first, 0, 1, list_DataPlasma.Count).Merged=true;
                workbook.Save(pathExcel);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }

        }
        #endregion
        private int AutoSizeMergedCells(CellRange myMergedCells, string text)
        {
            var file = new ExcelFile();
            file.Worksheets.Add("AutoSize");
            var ws = file.Worksheets[0];

            ws.Cells[0, 0].Column.Width = myMergedCells.Sum(x => x.Column.Width);
            ws.Cells[0, 0].Value = text;
            ws.Cells[0, 0].Style.WrapText = true;
            ws.Cells[0, 0].Row.AutoFit();
            var result = ws.Cells[0, 0].Row.Height;
            file = null;
            return result;
        }


    }
}
