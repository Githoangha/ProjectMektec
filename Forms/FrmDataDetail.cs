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
    public partial class FrmDataDetail : Form
    {
        public DataTable dtDetail = new DataTable();
        public FrmDataDetail()
        {
            InitializeComponent();
        }

        private void FrmDataDetail_Load(object sender, EventArgs e)
        {
            dtDetail.Columns.Add("Status", typeof(string));
            grdData.DataSource = dtDetail;
        }

        private void grvData_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                e.Value = e.ListSourceRowIndex + 1;
            }
        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                try
                {
                    string lotID = Lib.ToString(dtDetail.Rows[i]["LotID"]);
                    string PcsCode = Lib.ToString(dtDetail.Rows[i]["CodePcs"]);
                    string sql = $"Select Top 1 * From PcsResults Where RouteId ='{c_varGolbal.RouteID.Trim()}' AND LotNo='{lotID}' AND PcsBarcode='{PcsCode}'";
                    var dt = new SQL_Execution().Execute_DataTable(c_varGolbal.IP_SMT, sql);
                    if (dt.Rows.Count > 0)
                    {
                        dtDetail.Rows[i]["Status"] = GlobVar.OK;
                    }
                    else
                    {
                        dtDetail.Rows[i]["Status"] = GlobVar.WAIT;
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(delegate
                    {
                        dtDetail.Rows[i]["Status"] = GlobVar.Error;
                        //MessageBox.Show(ex.ToString());

                    }));
                }
            }

        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                for (int i = 0; i < grvData.RowCount; i++)
                {
                    int index = i;
                    if (Lib.ToString(grvData.GetRowCellValue(index, col_Status)).ToUpper() == GlobVar.WAIT)
                    {
                        dataPlasma data = new dataPlasma();
                        data.TagJigPlasma = Lib.ToString(grvData.GetRowCellValue(index, col_JigPlasma));
                        data.PcsBarcode = Lib.ToString(grvData.GetRowCellValue(index, col_CodePCS));
                        data.CodeTray = Lib.ToString(grvData.GetRowCellValue(index, col_CodeTray));
                        string LotID = Lib.ToString(grvData.GetRowCellValue(index, col_LotID));
                        string MPN = Lib.ToString(grvData.GetRowCellValue(index, col_MPN));
                        List<string> ListCodePCS = new List<string>();
                        ListCodePCS.Add(data.PcsBarcode.Trim());
                        if (Upload_DataPlasma(c_varGolbal.LineID, c_varGolbal.DeviceID, MPN, c_varGolbal.StaffID, LotID, ListCodePCS, data.TagJigPlasma, data.CodeTray))
                        {
                            grvData.SetRowCellValue(index, col_Status, GlobVar.OK);
                        }
                    }
                }
            }
            else
            {
                if (grvData.FocusedRowHandle >= 0)
                {
                    int index = grvData.FocusedRowHandle;
                    dataPlasma data = new dataPlasma();
                    data.TagJigPlasma = Lib.ToString(grvData.GetFocusedRowCellValue(col_JigPlasma));
                    data.PcsBarcode = Lib.ToString(grvData.GetFocusedRowCellValue(col_CodePCS));
                    data.CodeTray = Lib.ToString(grvData.GetFocusedRowCellValue(col_CodeTray));
                    string LotID = Lib.ToString(grvData.GetFocusedRowCellValue(col_LotID));
                    string MPN = Lib.ToString(grvData.GetFocusedRowCellValue(col_MPN));
                    List<string> ListCodePCS = new List<string>();
                    ListCodePCS.Add(data.PcsBarcode.Trim());
                    if (Lib.ToString(grvData.GetFocusedRowCellValue(col_Status)).Trim().ToUpper() == GlobVar.OK)
                    {

                    }
                    else if (Lib.ToString(grvData.GetFocusedRowCellValue(col_Status)).Trim().ToUpper() == GlobVar.WAIT)
                    {
                        if (Upload_DataPlasma(c_varGolbal.LineID, c_varGolbal.DeviceID, MPN, c_varGolbal.StaffID, LotID, ListCodePCS, data.TagJigPlasma, data.CodeTray))
                        {
                            grvData.SetRowCellValue(index, col_Status, GlobVar.OK);
                        }
                    }
                    else if (Lib.ToString(grvData.GetFocusedRowCellValue(col_Status)).Trim().ToUpper() == GlobVar.Error)
                    {
                        //grvData.SetRowCellValue(index, col_Status, "TEST");
                    }

                }
                else
                {
                    Lib.ShowWarning("Hãy Chọn barcode để upload");
                }
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
                    Lib.SaveToLog("ErrorUploadServer_in_DataDetail", tagJigPlasma, ResultProcess);
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
                Lib.SaveToLog("ExceptionUploadServer_in_DataDetail", tagJigPlasma, Ex.ToString());
                return false;
            }
        }
    }
}
