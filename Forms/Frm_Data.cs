using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseInterface_MMCV;

namespace LineGolden_PLasma
{
    public partial class Frm_Data : Form
    {
        public string _LineID { get; set; }
        public string _DeviceID { get; set; }
        DAL MMCV_DB = new DAL();
        public Frm_Data()
        {
            InitializeComponent();
        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            if (cboStatusData.SelectedIndex != -1)
            {
                string sqlSelectData = "";
                if (cboStatusData.Text == "ALL")
                {
                    sqlSelectData = $"SELECT * FROM Plasma WHERE StateUploadServer IN('WAITING','OK')";
                }
                else
                {
                    sqlSelectData = $"SELECT * FROM Plasma WHERE StateUploadServer IN('{cboStatusData.Text.Trim()}')";
                }
                DataTable dt = Support_SQL.GetTableDataPlasma(sqlSelectData);
                grdData.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Hãy chọn trạng thái của Data");
                return;
            }
        }


        private void Frm_Data_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(c_varGolbal.StaffID) || c_varGolbal.StaffID.ToUpper() == "MISSING")
            {
                txtstaffID.Text = "";
            }
            else
            {
                txtstaffID.Text = c_varGolbal.StaffID;
            }
            cboStatusData.SelectedIndex = 0;
        }
        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
                return false;
            }
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtstaffID.Text.Trim()))
            {
                new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Hãy điền StaffID").ShowDialog();
                return;
            }
            try
            {
                string Select_Jig_None = $"SELECT *FROM Plasma WHERE StateUploadServer='WAITING'";
                DataTable dt = Support_SQL.GetTableDataPlasma(Select_Jig_None);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        string tempCodePCS;
                        List<string> ListDataCodePCS = new List<string>();
                        dataPlasma data = new dataPlasma();
                        int _ProgramID = Support_SQL.ToInt(item["ID_Program"]);
                        int _RfidIndex = Support_SQL.ToInt(item["ID_Plasma"]);
                        data.ID = Lib.ToInt(item["ID"]);
                        data.TagJigTransfer = item["TagJigTransfer"].ToString();
                        data.TagJigPlasma = item["TagJigPlasma"].ToString();
                        data.PcsBarcode = item["CodePCS"].ToString();
                        data.CodeTray = item["CodeTray"].ToString();
                        data.DateTimeInPlasma = item["DateTimeInPlasma"].ToString();
                        data.StatusPlasma = "OK";
                        data.CycleTime = item["Cycletime"].ToString();
                        tempCodePCS = item["CodePCS"].ToString();
                        tempCodePCS = tempCodePCS.Replace("NoRead", "");
                        ListDataCodePCS = tempCodePCS.Split(',').ToList();
                        string LotID = item["LotID"].ToString().Trim();
                        string MPN = item["MPN"].ToString().Trim();
                        _LineID = c_varGolbal.LineID;
                        _DeviceID = c_varGolbal.DeviceID;

                        #region Kiểm tra những mã code nào đã có trên server 
                        List<string> lstUpload = new List<string>();
                        foreach (string element in ListDataCodePCS)
                        {
                            string sql = $"Select Top 1 * From PcsResults Where RouteId ='{c_varGolbal.RouteID.Trim()}' AND LotNo='{LotID}' AND PcsBarcode='{element}'";
                            var table = new SQL_Execution().Execute_DataTable(c_varGolbal.IP_SMT, sql);
                            if (table.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                lstUpload.Add(element);
                            }
                        }
                        #endregion
                        if (lstUpload.Count > 0)
                        {
                            if (Upload_DataPlasma(_LineID, _DeviceID, MPN, txtstaffID.Text.Trim(), LotID, lstUpload, data.TagJigPlasma, data.CodeTray))
                            {
                                Support_SQL.SaveStateUploadDataServer(_ProgramID, _RfidIndex, data.ID, data.TagJigPlasma, "OK");
                            }
                        }
                        else
                        {
                            Support_SQL.SaveStateUploadDataServer(_ProgramID, _RfidIndex, data.ID, data.TagJigPlasma, "OK");
                        }
                    }
                }
                btnShowData.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool Upload_DataPlasma(string LineId, string DeviceId, string MPN, string StaffID, string LotID, List<string> listTagPCS, string tagJigPlasma, string CodeTray)
        {
            try
            {
                Plasma56 MMCV_DBPlasma_New = new Plasma56();
                //MMCV_DBPlasma_New.IP_SERVER_SMT = c_varGolbal.IP_SMT;
                //MMCV_DBPlasma_New.RouteId = c_varGolbal.RouteID;

                string ResultProcess = "";
                bool kq = false;
                ResultProcess = MMCV_DBPlasma_New.PlasmaReading(listTagPCS, tagJigPlasma, CodeTray, LotID, StaffID, LineId, DeviceId);
                if (ResultProcess.Contains("OK"))
                {
                    kq = true;
                }
                else
                {
                    kq = false;
                    Lib.SaveToLog("ErrorUploadServer_in_FormData", tagJigPlasma, ResultProcess);
                }
                //ResultProcess = MMCV_DBPlasma_New.PlasmaReading(listTagPCS, CodeTray, tagJigPlasma, LotID, StaffID, LineId, DeviceId);
                if (kq)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                Lib.SaveToLog("ExceptionUploadServer_in_FormData", tagJigPlasma, Ex.ToString());
                return false;
            }
        }

        private void grvData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            string status = Lib.ToString(grvData.GetRowCellValue(e.RowHandle, col_StatusUpload.Name));

        }

        private void grvData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            string status = Lib.ToString(grvData.GetRowCellValue(e.RowHandle, col_StatusUpload.FieldName));
            if (status == "OK")
            {
                grvData.Columns[col_StatusUpload.FieldName].AppearanceCell.BackColor = Color.LimeGreen;
            }
            else if (status == "WAITING")
            {
                grvData.Columns[col_StatusUpload.FieldName].AppearanceCell.BackColor = Color.Orange;
            }
        }

        private void grvData_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column == col_CodePCS)
            {
                if (string.IsNullOrWhiteSpace(txtstaffID.Text))
                {
                    new Frm_ShowDialog(Frm_ShowDialog.Icon_Show.Warning, "Hãy điền StaffID").ShowDialog();
                    return;
                }
                else
                {
                    DataRow row= grvData.GetDataRow(e.RowHandle);
                    FrmDataDetail newfrm = new FrmDataDetail();
                    newfrm.dtDetail = Support_SQL.GetTableDataPlasma($"Select * From Plasma Where ID=0");
                    List<string> CodePcs = Lib.ToString(row["CodePcs"]).Trim().Split(',').ToList();
                    foreach(string item in CodePcs)
                    {
                        DataRow newrow = newfrm.dtDetail.NewRow();
                        for(int index=0;index < row.Table.Columns.Count; index++)
                        {
                            newrow[index] = row[index];
                        }
                        newrow["CodePcs"] = item;
                        newfrm.dtDetail.Rows.InsertAt(newrow, 0);
                    }
                    newfrm.ShowDialog();
                }
            }
            else
            {

            }
        }

        private void grvData_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                e.Value = e.ListSourceRowIndex +1;// + 1;
            }
        }

        private void txtstaffID_TextChanged(object sender, EventArgs e)
        {
            c_varGolbal.StaffID = txtstaffID.Text.Trim();
        }
    }
}
