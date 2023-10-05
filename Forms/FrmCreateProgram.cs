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
using System.Xml;

namespace LineGolden_PLasma
{
    public partial class FrmCreateProgram : Form
    {
        private int modelID { get; set; }
        bool _edit = false;
        bool _add = false;

        public FrmCreateProgram(int CurrentProgram)
        {
            InitializeComponent();
            modelID = CurrentProgram;
            grvModel.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FEFFCA");
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
            txt_qtyJigBase.Enabled = false;
            txtTimeRepeatJig.Enabled = false;
            cbo_ModeReadCode.Enabled = cbUseMachine.Enabled = false;
            chkUseFvi.Enabled = false;
            chkGetJigHavePcs.Enabled = false;
            loadProgram(modelID);
            try
            {
                XmlDocument xmlCF = new XmlDocument();
                xmlCF.Load("Config_Plasma_Boxing.xml");
                XmlNodeList xmlListCF = xmlCF.DocumentElement.SelectNodes("/Config");
                foreach (XmlNode xmlNode in xmlListCF)
                {
                    txtPathDataBoxing.Text= xmlNode.SelectSingleNode("Path_Boxing").InnerText;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Error Process Load Parameter file:Config_Plasma_Boxing.xml Error \r\n " + Ex.ToString(), "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Load program Add DGV
        /// </summary>
        /// <param name="ModelID"></param>
        private void loadProgram(int ModelID)
        {
            DataTable dtModel = Support_SQL.GetTableData($"SELECT * FROM {PrgMain.NameTable}");
            grvModel.DataSource = null;
            grvModel.AutoGenerateColumns = false;
            grvModel.DataSource = dtModel;

            if (grvModel.RowCount <= 0)
                return;
            // nếu program đang chạy , mặc định Selected row 0
            if (ModelID == -1)
            {
                grvModel.Rows[0].Selected = true;
                txtNameModel.Text = grvModel.Rows[0].Cells[ColName.Name].Value.ToString();
                txtDes.Text = grvModel.Rows[0].Cells[ColDes.Name].Value.ToString();
                txt_qtyCam.Value = Convert.ToInt32(grvModel.Rows[0].Cells[ColQtyCam.Name].Value);
                txt_qtyJigBase.Value = Convert.ToInt32(grvModel.Rows[0].Cells[ColNumJigPlasma.Name].Value);
                cbo_ModeReadCode.Text = grvModel.Rows[0].Cells["ReadCodePCS"].Value.ToString();
                txtTimeRepeatJig.Value = Convert.ToInt32(grvModel.Rows[0].Cells["TimeRepeatJig"].Value);
                c_varGolbal.TimeRepeatJig = Convert.ToInt32(grvModel.Rows[0].Cells["TimeRepeatJig"].Value);
                //21-02-2023
                cbUseMachine.SelectedIndex = Support_SQL.ToInt(grvModel.Rows[0].Cells["colUseMachine"].Value.ToString());
                chkGetJigHavePcs.Checked = Lib.ToBoolean(grvModel.Rows[0].Cells[col_GetJigHavePcs.Index].Value);
                chkUseFvi.Checked = Lib.ToBoolean(grvModel.Rows[0].Cells[col_UseFvi.Index].Value);
            }
            else
            {
                // duyệt danh sách program - nếu khớp với program đang chạy hiện tại sẽ tiến hành Selected trên dgv
                foreach (DataGridViewRow row in grvModel.Rows)
                {
                    if (Convert.ToInt32(row.Cells["colID"].Value) == ModelID)
                    {
                        grvModel.ClearSelection();
                        row.Selected = true;
                        txtNameModel.Text = row.Cells["ColName"].Value.ToString();
                        txtDes.Text = row.Cells["colDes"].Value.ToString();
                        txt_qtyCam.Value = Convert.ToInt32(row.Cells["ColQtyCam"].Value);
                        txt_qtyJigBase.Value = Convert.ToInt32(row.Cells[ColNumJigPlasma.Name].Value);
                        cbo_ModeReadCode.Text = row.Cells["ReadCodePCS"].Value.ToString();
                        txtTimeRepeatJig.Value = Convert.ToInt32(grvModel.Rows[0].Cells["TimeRepeatJig"].Value);
                        c_varGolbal.TimeRepeatJig = Convert.ToInt32(grvModel.Rows[0].Cells["TimeRepeatJig"].Value);
                        //21-02-2023
                        cbUseMachine.SelectedIndex = Support_SQL.ToInt(row.Cells["colUseMachine"].Value.ToString());
                        chkGetJigHavePcs.Checked = Lib.ToBoolean(row.Cells[col_GetJigHavePcs.Index].Value);
                        chkUseFvi.Checked = Lib.ToBoolean(row.Cells[col_UseFvi.Index].Value);
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
                txt_qtyJigBase.Value = 3;
                cbo_ModeReadCode.SelectedIndex = 1;
                txtTimeRepeatJig.Value = 5;
                chkGetJigHavePcs.Checked = false;
                chkUseFvi.Checked = false;

                txtDes.ReadOnly = txtNameModel.ReadOnly = false;
                txt_qtyJigBase.Enabled = true;
                cbo_ModeReadCode.Enabled = cbUseMachine.Enabled = true;
                txtTimeRepeatJig.Enabled = true;
                chkGetJigHavePcs.Enabled = true;
                chkUseFvi.Enabled = true;
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
                DataTable dt = Support_SQL.GetTableData($"SELECT ProgramName FROM ProgramMain WHERE ProgramName = '{txtNameModel.Text}'");
                if (dt.Rows.Count > 0)
                {
                    txtDes.Text = "";
                    txtNameModel.Text = "";
                    txt_qtyCam.Value = 1;
                    txt_qtyJigBase.Value = 1;
                    cbo_ModeReadCode.SelectedIndex = 1;

                    dt.Dispose();
                    MessageBox.Show("This Model is exist !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                if (txtNameModel.Text == "")
                {

                    MessageBox.Show("Model name can not empty !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                string sql_Insert_Program = $"INSERT INTO ProgramMain (" +
                                            $"ProgramName," +
                                            $"Description," +
                                            $"NumberCamera," +
                                            $"NumJigPlasmaBase," +
                                            $"ReadCodePCS," +
                                            $"TimeRepeatJig," +
                                            $"UseMachine," +
                                            $"GetJigHavePcs," +
                                            $"UseFvi" +
                                            $") VALUES (" +
                                            $"'{txtNameModel.Text}'," +
                                            $"'{txtDes.Text}'," +
                                            $"'{txt_qtyCam.Value}'," +
                                            $"'{txt_qtyJigBase.Value}'," +
                                            $"'{cbo_ModeReadCode.Text}'," +
                                            $"'{txtTimeRepeatJig.Value}'," +
                                            $"'{cbUseMachine.SelectedIndex}'," +
                                            $"{chkGetJigHavePcs.Checked}," +
                                            $"{chkUseFvi.Checked});" +
                                            $" select last_insert_rowid() as ID";
                int ID = Support_SQL.ToInt(Support_SQL.ExecuteScalar(sql_Insert_Program));
                txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
                txtDes.ReadOnly = txtNameModel.ReadOnly = true;
                txt_qtyJigBase.Enabled = cbo_ModeReadCode.Enabled = false;
                chkGetJigHavePcs.Enabled = false;
                chkUseFvi.Enabled = false;
                btnAddModel.Text = "New";
                btnUpdateModel.Enabled = btnDelModel.Enabled = true;
                _add = false;
                // sau khi thêm xong program sẽ tiến hành load lại program
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
                txt_qtyJigBase.Enabled = true;
                cbo_ModeReadCode.Enabled = cbUseMachine.Enabled = true;
                chkGetJigHavePcs.Enabled = true;
                chkUseFvi.Enabled = true;
                txtDes.BackColor = txtNameModel.BackColor = Color.White;
                btnUpdateModel.Text = "Update";
                _edit = true;
                btnAddModel.Enabled = false;
                btnDelModel.Enabled = false;
                txtTimeRepeatJig.Enabled = true;
            }
            else
            {
                //Update
                int row = grvModel.CurrentCell.RowIndex;
                string name = grvModel.SelectedRows[0].Cells["colName"].Value.ToString();
                // Kiểm tra nếu thay đổi tên chương trình thì sẽ phải kiểm tra trong cơ sở dữ liệu nếu giống với tên program đã có trong cơ sở thì không cho phép update program đó
                if (txtNameModel.Text != grvModel.SelectedRows[0].Cells["colName"].Value.ToString())
                {
                    DataTable dt = Support_SQL.GetTableData(string.Format("SELECT ProgramName from ProgramMain WHERE ProgramName = '{0}'", txtNameModel.Text));

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        MessageBox.Show("This Model is exist !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dt.Dispose();
                        return;
                    }
                }
                // Update Name Program hoặc Description
                int ID_PrgCurrent = Convert.ToInt32(grvModel.SelectedRows[0].Cells["colID"].Value);
                string sql_Update_Progam = $"UPDATE ProgramMain SET " +
                                          $"ProgramName='{txtNameModel.Text}'," +
                                          $"Description='{txtDes.Text}'," +
                                          $"NumberCamera='{txt_qtyCam.Value}'," +
                                          $"NumJigPlasmaBase='{txt_qtyJigBase.Value}'," +
                                          $"ReadCodePCS='{cbo_ModeReadCode.Text}'," +
                                          $"TimeRepeatJig='{txtTimeRepeatJig.Value}', " +
                                          $"UseMachine='{cbUseMachine.SelectedIndex}'," +
                                          $"GetJigHavePcs={chkGetJigHavePcs.Checked}," +
                                          $"UseFvi={chkUseFvi.Checked} " +
                                          $" WHERE ID_Program='{ID_PrgCurrent}'";
                Support_SQL.ExecuteQuery(sql_Update_Progam);

                txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
                txtDes.ReadOnly = txtNameModel.ReadOnly = true;
                txt_qtyJigBase.Enabled = false;
                cbo_ModeReadCode.Enabled = cbUseMachine.Enabled = false;
                txtTimeRepeatJig.Enabled = false;
                chkGetJigHavePcs.Enabled = false;
                chkUseFvi.Enabled = false;
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
        private void grvModel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != 6)
            {
                try
                {
                    txtNameModel.Text = grvModel.Rows[e.RowIndex].Cells["colName"].Value.ToString();
                    txtDes.Text = grvModel.Rows[e.RowIndex].Cells["colDes"].Value.ToString();
                    txt_qtyCam.Value = Convert.ToInt32(grvModel.Rows[e.RowIndex].Cells["ColQtyCam"].Value);
                    txt_qtyJigBase.Value = Convert.ToInt32(grvModel.Rows[e.RowIndex].Cells[ColNumJigPlasma.Name].Value);
                    cbo_ModeReadCode.Text = grvModel.Rows[e.RowIndex].Cells["ReadCodePCS"].Value.ToString();
                    txtTimeRepeatJig.Value = Convert.ToInt32(grvModel.Rows[e.RowIndex].Cells["TimeRepeatJig"].Value);
                    c_varGolbal.TimeRepeatJig = Convert.ToInt32(grvModel.Rows[e.RowIndex].Cells["TimeRepeatJig"].Value);
                    //
                    int ID = Support_SQL.ToInt(grvModel[ColID.Name, e.RowIndex].Value);

                    cbUseMachine.SelectedIndex = Support_SQL.ToInt(grvModel.Rows[e.RowIndex].Cells["colUseMachine"].Value);
                    chkGetJigHavePcs.Checked = Lib.ToBoolean(grvModel.Rows[e.RowIndex].Cells[col_GetJigHavePcs.Index].Value);
                    chkUseFvi.Checked = Lib.ToBoolean(grvModel.Rows[e.RowIndex].Cells[col_UseFvi.Index].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// Button Delete Modell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelModel_Click(object sender, EventArgs e)
        {
            string Model = grvModel[ColName.Name, grvModel.CurrentRow.Index].Value.ToString();
            DialogResult rep = MessageBox.Show($"Are you sure you want to delete this Model:{Model} ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rep == DialogResult.Yes)
            {
                int row = grvModel.CurrentCell.RowIndex;
                int id = Convert.ToInt32(grvModel.Rows[row].Cells["colID"].Value);
                grvModel.Rows.RemoveAt(row);
                // Xóa các dữ liệu Setting liên quan đến ID_Program đó
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM ProgramMain where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM CameraSetting where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM PositionMachinePlasma where ID_Program = '{0}'", id));

                loadProgram(-1);
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
            txtDes.ReadOnly = txtNameModel.ReadOnly = true; txt_qtyJigBase.Enabled = cbo_ModeReadCode.Enabled = txtTimeRepeatJig.Enabled = cbUseMachine.Enabled = false;
            chkGetJigHavePcs.Enabled = false;
            chkUseFvi.Enabled = false;
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
            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                File.Delete(Application.StartupPath + "\\TrainFile\\" + dt.Rows[i]["FileName"].ToString());
            }

        }

        private void btnFilePathBoxing_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = ".xml|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPathDataBoxing.Text = ofd.FileName;
                    XmlDocument xmlCF = new XmlDocument();
                    xmlCF.Load("Config_Plasma_Boxing.xml");
                    XmlNodeList xmlListCF = xmlCF.DocumentElement.SelectNodes("/Config");
                    foreach (XmlNode xmlNode in xmlListCF)
                    {
                        xmlNode.SelectSingleNode("Path_Boxing").InnerText = txtPathDataBoxing.Text + "";
                    }
                    xmlCF.Save(@"Config_Plasma_Boxing.xml");
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Error Process Update Parameter file:Config_Plasma_Boxing.xml Error \r\n " + Ex.ToString(), "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
