using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCode
{
    public partial class frmCreateProgram_New : Form
    {
        private int modelID { get; set; }
        bool _edit = false;
        bool _add = false;

        public frmCreateProgram_New(int CurrentProgram)
        {
            InitializeComponent();
            modelID = CurrentProgram;
            //grvModel.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FEFFCA");
        }
        /// <summary>
        /// Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCreateProgram_Load(object sender, EventArgs e)
        {
            txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
            txtDes.ReadOnly = txtNameModel.ReadOnly = true;
            txtTimeTranferJig.Enabled = false;
            txtStrHeaderTag.Enabled = false;
            txtTimeRepeatJig.Enabled = false;
            cbo_ModeReadCode.Enabled = false;
            loadData();
            loadProgram(modelID);
        }
        /// <summary>
        /// Load Data GridControl
        /// </summary>
        void loadData()
        {
            DataTable dtModel = Support_SQL.GetTableData($"SELECT * FROM ProgramMain");
            grdData.DataSource = null;
            grdData.DataSource = dtModel;
        }
        /// <summary>
        /// Load program Add DGV
        /// </summary>
        /// <param name="ModelID"></param>
        private void loadProgram(int ModelID)
        {

            if (grvData.RowCount <= 0)
                return;
            // nếu program đang chạy , mặc định Selected row 0
            if (ModelID <= 0)
            {
                //grvData.FocusedRowHandle = 0;
                txtNameModel.Text = Support_SQL.ToString(grvData.GetRowCellValue(0, colProgramName));
                txtDes.Text = Support_SQL.ToString(grvData.GetRowCellValue(0, colDescription));
                txt_qtyCam.Value = Support_SQL.ToDecimal(grvData.GetRowCellValue(0, colNumberCamera));
                cbo_ModeJig.Text = Support_SQL.ToString(grvData.GetRowCellValue(0, colTransferJig));
                cbo_ModeReadCode.Text = Support_SQL.ToString(grvData.GetRowCellValue(0, colReadCodePCS));
                txtTimeTranferJig.Value = Support_SQL.ToDecimal(grvData.GetRowCellValue(0, colTimeTranferJig));
                txtStrHeaderTag.Text = Support_SQL.ToString(grvData.GetRowCellValue(0, colStringHeaderTagJig));
                c_varGolbal.StrHeaderTagJig = Support_SQL.ToString(grvData.GetRowCellValue(0, colStringHeaderTagJig));
                txtTimeRepeatJig.Value = Support_SQL.ToDecimal(grvData.GetRowCellValue(0, colTimeRepeatJig));
                c_varGolbal.TimeRepeatJig = Support_SQL.ToInt(grvData.GetRowCellValue(0, colTimeRepeatJig));
            }
            else
            {
                // duyệt danh sách program - nếu khớp với program đang chạy hiện tại sẽ tiến hành Selected trên dgv
                for (int i = 0; i < grvData.RowCount; i++)
                {
                    if (Support_SQL.ToInt(grvData.GetRowCellValue(i, colID_Program)) == ModelID)
                    {
                        txtNameModel.Text = Support_SQL.ToString(grvData.GetRowCellValue(i, colProgramName));
                        txtDes.Text = Support_SQL.ToString(grvData.GetRowCellValue(i, colDescription));
                        txt_qtyCam.Value = Support_SQL.ToDecimal(grvData.GetRowCellValue(i, colNumberCamera));
                        cbo_ModeJig.Text = Support_SQL.ToString(grvData.GetRowCellValue(i, colTransferJig));
                        cbo_ModeReadCode.Text = Support_SQL.ToString(grvData.GetRowCellValue(i, colReadCodePCS));
                        txtTimeTranferJig.Value = Support_SQL.ToDecimal(grvData.GetRowCellValue(i, colTimeTranferJig));
                        txtStrHeaderTag.Text = Support_SQL.ToString(grvData.GetRowCellValue(i, colStringHeaderTagJig));
                        c_varGolbal.StrHeaderTagJig = Support_SQL.ToString(grvData.GetRowCellValue(i, colStringHeaderTagJig));
                        txtTimeRepeatJig.Value = Support_SQL.ToInt(grvData.GetRowCellValue(i, colTimeRepeatJig));
                        c_varGolbal.TimeRepeatJig = Support_SQL.ToInt(grvData.GetRowCellValue(i, colTimeRepeatJig));
                    }
                }

            }
            btnAddModel.Focus();
        }
        /// <summary>
        /// Button Add Or New
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddModel_Click(object sender, EventArgs e)
        {
            // nút bấm 2 trạng thái
            if (!_add)
            {
                txtDes.Text = "";
                txtNameModel.Text = "";
                txt_qtyCam.Value = 2;
                cbo_ModeJig.SelectedIndex = 1;
                cbo_ModeReadCode.SelectedIndex = 1;
                txtTimeRepeatJig.Value = 15;

                txtDes.ReadOnly = txtNameModel.ReadOnly = false;
                txtTimeTranferJig.Enabled = true;
                txtStrHeaderTag.Enabled = true;
                cbo_ModeReadCode.Enabled = true;
                txtTimeRepeatJig.Enabled = true;

                txtDes.BackColor = txtNameModel.BackColor = Color.White;

                this.ActiveControl = txtNameModel;
                btnAddModel.Text = "Add";
                _add = true;
                btnUpdateModel.Enabled = false;
                btnDelModel.Enabled = false;
            }
            else
            {
                // inset program new
                DataTable dt = Support_SQL.GetTableData($"SELECT ProgramName FROM ProgramMain WHERE ProgramName = '{txtNameModel.Text.Trim()}'");
                if (dt.Rows.Count > 0)
                {
                    txtDes.Text = "";
                    txtNameModel.Text = "";
                    txt_qtyCam.Value = 1;
                    //txtStrHeaderTag.Text = "";
                    cbo_ModeJig.SelectedIndex = 1;
                    cbo_ModeReadCode.SelectedIndex = 1;

                    dt.Dispose();
                    MessageBox.Show("This Model is exist !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                //ShowPosition(-1);
                if (txtNameModel.Text == "")
                {

                    MessageBox.Show("Model name can not empty !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                string sql_Insert_Program = $"INSERT INTO ProgramMain (" +
                                            $"ProgramName," +
                                            $"Description," +
                                            $"NumberCamera," +
                                            $"TransferJig," +
                                            $"ReadCodePCS," +
                                            $"StringHeaderTagJig," +
                                            $"TimeRepeatJig" +
                                            $"TimeTranferJig" +
                                            $") VALUES (" +
                                            $"'{txtNameModel.Text}'," +
                                            $"'{txtDes.Text}'," +
                                            $"'{txt_qtyCam.Value}'," +
                                            $"'{cbo_ModeJig.Text}'," +
                                            $"'{cbo_ModeReadCode.Text}'," +
                                            $"'{txtStrHeaderTag.Text}'," +
                                            $"'{txtTimeRepeatJig.Value}');"+
                                            $"'{txtTimeTranferJig.Value}');" +
                                            $" select last_insert_rowid() as ID";
                int ID = Support_SQL.ToInt(Support_SQL.ExecuteScalar(sql_Insert_Program));

                txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
                btnAddModel.Text = "New";
                btnUpdateModel.Enabled = true;
                btnDelModel.Enabled = true;
                _add = false;
                // sau khi thêm xong program sẽ tiến hành load lại program
                loadData();
                loadProgram(ID);
            }
        }
        /// <summary>
        /// Button Edit Or Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateModel_Click(object sender, EventArgs e)
        {
            if (!_edit)
            {
                //Edit
                txtDes.ReadOnly = txtNameModel.ReadOnly = false;
                txtTimeTranferJig.Enabled = true;
                cbo_ModeReadCode.Enabled = true;
                txtDes.BackColor = txtNameModel.BackColor = Color.White;
                btnUpdateModel.Text = "Update";
                _edit = true;
                btnAddModel.Enabled = false;
                btnDelModel.Enabled = false;
                txtTimeTranferJig.Enabled = true;
                txtStrHeaderTag.Enabled = true;
                txtTimeRepeatJig.Enabled = true;
            }
            else
            {
                //Update
                int ID = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colID_Program));
                if (ID <= 0) return;
                string name = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colProgramName));
                if (Support_SQL.ToString(txtStrHeaderTag.Text.Trim()) == "")
                {
                    MessageBox.Show(this, "Xin vui lòng nhập String Header Tag Jig: \r ", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Kiểm tra nếu thay đổi tên chương trình thì sẽ phải kiểm tra trong cơ sở dữ liệu nếu giống với tên program đã có trong cơ sở thì không cho phép update program đó
                DataTable dt = Support_SQL.GetTableData(string.Format("SELECT * from ProgramMain WHERE ProgramName = '{0}' AND ID_Program <> {1}", txtNameModel.Text.Trim(),ID));
                if (dt.Rows.Count > 0)
                {
                   // dt.Dispose();
                    MessageBox.Show("This Model is exist !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //dt.Dispose();
                    return;
                }
                // Update Name Program hoặc Description
                int ID_PrgCurrent = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colID_Program));
                string sql_Update_Progam = $"UPDATE ProgramMain SET " +
                                          $"ProgramName='{txtNameModel.Text.Trim()}'," +
                                          $"Description='{txtDes.Text.Trim()}'," +
                                          $"NumberCamera='{txt_qtyCam.Value}'," +
                                          $"TransferJig='{cbo_ModeJig.Text.Trim()}'," +
                                          $"ReadCodePCS='{cbo_ModeReadCode.Text.Trim()}'," +
                                          $"TimeTranferJig='{txtTimeTranferJig.Value}', " +
                                          $"StringHeaderTagJig='{txtStrHeaderTag.Text.Trim()}', " +
                                          $"TimeRepeatJig='{txtTimeRepeatJig.Value}' " +
                                          $"WHERE ID_Program='{ID_PrgCurrent}'";
                Support_SQL.ExecuteQuery(sql_Update_Progam);

                txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
                txtDes.ReadOnly = txtNameModel.ReadOnly = true;
                txtTimeTranferJig.Enabled = false;
                cbo_ModeReadCode.Enabled = false;
                txtStrHeaderTag.Enabled = false;
                txtTimeRepeatJig.Enabled = false;
                btnUpdateModel.Text = "Edit";
                btnAddModel.Enabled = true;
                btnDelModel.Enabled = true;
                _edit = false;

                loadProgram(ID_PrgCurrent);

            }
        }
        /// <summary>
        /// Even click cell on dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
                grvData.CloseEditor();
                txtNameModel.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colProgramName));
                txtDes.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colDescription));
                txt_qtyCam.Value = Support_SQL.ToDecimal(grvData.GetFocusedRowCellValue(colNumberCamera));
                cbo_ModeJig.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colTransferJig));
                cbo_ModeReadCode.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colReadCodePCS));
                txtTimeTranferJig.Value = Support_SQL.ToDecimal(grvData.GetFocusedRowCellValue(colTimeTranferJig));
                txtStrHeaderTag.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colStringHeaderTagJig));
                c_varGolbal.StrHeaderTagJig = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colStringHeaderTagJig));
                txtTimeRepeatJig.Value = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colTimeRepeatJig));
                c_varGolbal.TimeRepeatJig = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colTimeRepeatJig));
        }
       
        /// <summary>
        /// Button Delete Modell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelModel_Click(object sender, EventArgs e)
        {
            int id = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colID_Program));
            if (id <= 0) return;
            string Model =Support_SQL.ToString(grvData.GetFocusedRowCellValue(colProgramName));
            DialogResult rep = MessageBox.Show($"Are you sure you want to delete this Model:{Model} ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rep == DialogResult.Yes)
            {
                // Xóa các dữ liệu Setting liên quan đến ID_Program đó
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM ProgramMain where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM CameraSetting where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM RegionPosition where ID_Program = '{0}'", id));
                // Xóa các file train
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM TrainFile where ID_Program = '{0}'", id));
                DeleteFileTrain(id);
                grvData.DeleteSelectedRows();
            }
        }
        /// <summary>
        /// Button Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {

            txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
            btnAddModel.Text = "New";
            btnUpdateModel.Text = "Edit";
            btnUpdateModel.Enabled = true;
            btnDelModel.Enabled = true;
            btnAddModel.Enabled = true;
            _add = false;
            _edit = false;
            //
            loadProgram(modelID);
        }
        /// <summary>
        /// Xóa tất cả các file train liên quan đến ID Program đó
        /// </summary>
        private void DeleteFileTrain(int ID_Prg)
        {
            DataTable dt = Support_SQL.GetTableData("select * from TrainFile where ID_Program = " + ID_Prg + "");
            if (dt.Rows.Count <= 0) return;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                File.Delete(Application.StartupPath + "\\TrainFile\\" + dt.Rows[i]["FileName"].ToString());
            }

        }

       
    }
}
