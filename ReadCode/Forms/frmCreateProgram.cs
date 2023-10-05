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
    public partial class frmCreateProgram : Form
    {
        private int modelID { get; set; }
        bool _edit = false;
        bool _add = false;
        DataTable dtModel;

        public frmCreateProgram(int CurrentProgram)
        {
            InitializeComponent();
            modelID = CurrentProgram;

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
            cbo_ModeReadCode.Enabled = cbJigHaveBarcode.Enabled = cbTypeModel.Enabled=chkUsingFVI.Enabled = false;
            numQtyPcs.Enabled = false;
            chkTakeJigHavePcs.Enabled = false;
            loadProgram(modelID);
        }


        /// <summary>
        /// Load program Add DGV
        /// </summary>
        /// <param name="ModelID"></param>
        private void loadProgram(int ModelID)
        {
            dtModel = Support_SQL.GetTableData($"SELECT * FROM ProgramMain");
            grdData.DataSource = dtModel;

            if (grvData.RowCount <= 0)
                return;
            // nếu program đang chạy , mặc định Selected row 0
            if (ModelID == -1)
            {
                grvData.FocusedRowHandle = 0;
                txtNameModel.Text = Support_SQL.ToString(dtModel.Rows[0]["ProgramName"]);
                txtDes.Text = Support_SQL.ToString(dtModel.Rows[0]["Description"]);
                txt_qtyCam.Value = Support_SQL.ToInt(dtModel.Rows[0]["NumberCamera"]);
                numQtyPcs.Value = Support_SQL.ToDecimal(dtModel.Rows[0]["QtyPcs"]);
                if (Support_SQL.ToString(dtModel.Rows[0]["ReadCodePCS"]).Trim().ToUpper() == "ENABLE")
                {
                    cbo_ModeReadCode.SelectedIndex = 0;
                }
                else
                {
                    cbo_ModeReadCode.SelectedIndex = 1;
                }
                if (Support_SQL.ToString(dtModel.Rows[0]["JigPCSHaveBarcode"]).Trim().ToUpper() == "ENABLE")
                {
                    cbJigHaveBarcode.SelectedIndex = 0;
                }
                else
                {
                    cbJigHaveBarcode.SelectedIndex = 1;
                }
                chkUsingFVI.Checked = Support_SQL.ToBoolean(dtModel.Rows[0]["UsingFVI"]);
                chkTakeJigHavePcs.Checked = Lib.ToBoolean(dtModel.Rows[0]["TakeJigHavePcs"]);
            }
            else
            {
                int index = 0;
                for (int i = 0; i < dtModel.Rows.Count; i++)
                {
                    DataRow item = dtModel.Rows[i];
                    if (Support_SQL.ToInt(item["ID_Program"]) == ModelID)
                    {
                        txtNameModel.Text = Support_SQL.ToString(item["ProgramName"]);
                        txtDes.Text = Support_SQL.ToString(item["Description"]);
                        txt_qtyCam.Value = Support_SQL.ToInt(item["NumberCamera"]);
                        numQtyPcs.Value = Support_SQL.ToDecimal(dtModel.Rows[0]["QtyPcs"]);
                        chkUsingFVI.Checked = Lib.ToBoolean(item["UsingFvi"]);
                        chkTakeJigHavePcs.Checked = Lib.ToBoolean(item["TakeJigHavePcs"]);
                        if (Support_SQL.ToString(item["ReadCodePCS"]).Trim().ToUpper() == "ENABLE")
                        {
                            cbo_ModeReadCode.SelectedIndex = 0;
                        }
                        else if(Support_SQL.ToString(item["ReadCodePCS"]).Trim().ToUpper() == "DISABLE")
                        {
                            cbo_ModeReadCode.SelectedIndex = 1;
                        }
                        if (Support_SQL.ToString(dtModel.Rows[0]["JigPCSHaveBarcode"]).Trim().ToUpper() == "ENABLE")
                        {
                            cbJigHaveBarcode.SelectedIndex = 0;
                        }
                        else if(Support_SQL.ToString(dtModel.Rows[0]["JigPCSHaveBarcode"]).Trim().ToUpper() == "DISABLE")
                        {
                            cbJigHaveBarcode.SelectedIndex = 1;
                        }
                        cbTypeModel.SelectedIndex = Support_SQL.ToInt(item["TypeModel"]);
                        index = i;
                    }
                }
                grvData.FocusedRowHandle = index;


            }
            //ShowPosition(modelID);
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
                cbo_ModeReadCode.SelectedIndex = 1;
                cbTypeModel.SelectedIndex = 1;
                txtDes.ReadOnly = txtNameModel.ReadOnly = false;
                cbo_ModeReadCode.Enabled = cbJigHaveBarcode.Enabled =chkUsingFVI.Enabled= cbTypeModel.Enabled = true;
                numQtyPcs.Enabled = true;
                chkTakeJigHavePcs.Enabled = true;
                chkTakeJigHavePcs.Checked = false;
                numQtyPcs.Value = 30;
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
                    cbo_ModeReadCode.SelectedIndex = 1;
                    cbTypeModel.SelectedIndex = 1;
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
                                            $"ReadCodePCS," +
                                            $"JigPCSHaveBarcode," +
                                            $"TypeModel," +
                                            $"UsingFVI," +
                                            $"QtyPcs," +
                                            $"TakeJigHavePcs" +
                                            $") VALUES (" +
                                            $"'{txtNameModel.Text}'," +
                                            $"'{txtDes.Text}'," +
                                            $"'{txt_qtyCam.Value}'," +
                                            $"'{cbo_ModeReadCode.Text}'," +
                                            $"'{cbJigHaveBarcode.Text}'," +
                                            $"'{cbTypeModel.SelectedIndex}'," +
                                            $"{chkUsingFVI.Checked}," +
                                            $"{numQtyPcs.Value}," +
                                            $"{chkTakeJigHavePcs.Checked});" +
                                            $" select last_insert_rowid() as ID";
                int ID = Support_SQL.ToInt(Support_SQL.ExecuteScalar(sql_Insert_Program));


                //Insert Setting Cam nếu đã có setting cam của con cũ 
                string sql_Settingcam = "select * from CameraSetting ORDER by ID_Program ASC Limit 2";
                DataTable dt_setting_cam = Support_SQL.GetTableDataUser(sql_Settingcam);
                if (dt_setting_cam.Rows.Count > 0 && ID > 0)
                {
                    for (int i = 0; i < dt_setting_cam.Rows.Count; i++)
                    {
                        int camIndex = Support_SQL.ToInt(dt_setting_cam.Rows[i]["CamIndex"]);
                        string InterfaceName = Support_SQL.ToString(dt_setting_cam.Rows[i]["InterfaceName"]);
                        string DeviceName = Support_SQL.ToString(dt_setting_cam.Rows[i]["DeviceName"]);
                        string insertSettingCam = $"INSERT into CameraSetting (ID_Program,CamIndex,InterfaceName,DeviceName) VALUES ({ID},{camIndex},'{InterfaceName}','{DeviceName}')";
                        Support_SQL.ExecuteQuery(insertSettingCam);
                    }
                }
                //Insert Setting Barcode
                string sql_SettingBarcode = "select * from BarcodeSetting LIMIT 1";
                DataTable dt_setting_barcode = Support_SQL.GetTableDataUser(sql_SettingBarcode);
                if (dt_setting_barcode.Rows.Count > 0 && ID > 0)
                {
                    for (int i = 0; i < dt_setting_barcode.Rows.Count; i++)
                    {
                        string Ip_Barcode = Support_SQL.ToString(dt_setting_barcode.Rows[i]["Ip_Barcode"]);
                        int Port_Barcode = Support_SQL.ToInt(dt_setting_barcode.Rows[i]["Port_Barcode"]);
                        string insertSettingbarcode = $"INSERT into BarcodeSetting (ID_Program,Ip_Barcode,Port_Barcode) VALUES ({ID},'{Ip_Barcode}',{Port_Barcode})";
                        Support_SQL.ExecuteQuery(insertSettingbarcode);
                    }
                }


                txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
                txtDes.ReadOnly = txtNameModel.ReadOnly = true; cbo_ModeReadCode.Enabled = chkUsingFVI.Enabled=cbJigHaveBarcode.Enabled = cbTypeModel.Enabled = false;
                numQtyPcs.Enabled = false;
                chkTakeJigHavePcs.Enabled = false;
                btnAddModel.Text = "New";
                btnUpdateModel.Enabled = true;
                btnDelModel.Enabled = true;
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
                cbo_ModeReadCode.Enabled = cbJigHaveBarcode.Enabled = cbTypeModel.Enabled =chkUsingFVI.Enabled= true;
                numQtyPcs.Enabled = true;
                chkTakeJigHavePcs.Enabled = true;
                txtDes.BackColor = txtNameModel.BackColor = Color.White;
                btnUpdateModel.Text = "Update";
                _edit = true;
                btnAddModel.Enabled = false;
                btnDelModel.Enabled = false;
            }
            else
            {
                int ID = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(ColProgramID));
                //Update
                DataTable dt = Support_SQL.GetTableData(string.Format("SELECT ProgramName from ProgramMain WHERE ProgramName = '{0}' and ID_Program <>{1}", txtNameModel.Text, ID));

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    MessageBox.Show("This Model is exist !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dt.Dispose();
                    return;
                }

                // Update Name Program hoặc Description
                string sql_Update_Progam = $"UPDATE ProgramMain SET " +
                                          $"ProgramName='{txtNameModel.Text}'," +
                                          $"Description='{txtDes.Text}'," +
                                          $"NumberCamera={txt_qtyCam.Value}," +
                                          $"ReadCodePCS='{cbo_ModeReadCode.Text}'," +
                                          $"JigPCSHaveBarcode='{cbJigHaveBarcode.Text}'," +
                                          $"TypeModel={cbTypeModel.SelectedIndex}," +
                                          $"UsingFVI={chkUsingFVI.Checked}," +
                                          $"QtyPcs={numQtyPcs.Value}," +
                                          $"TakeJigHavePcs={chkTakeJigHavePcs.Checked}" +
                                          $" WHERE ID_Program='{ID}'";
                Support_SQL.ExecuteQuery(sql_Update_Progam);


                txtDes.BackColor = txtNameModel.BackColor = Color.FromArgb(255, 128, 0);
                txtDes.ReadOnly = txtNameModel.ReadOnly = true;
                btnUpdateModel.Text = "Edit";
                btnAddModel.Enabled = true;
                btnDelModel.Enabled = true;
                _edit = cbo_ModeReadCode.Enabled = cbJigHaveBarcode.Enabled = chkUsingFVI.Enabled=cbTypeModel.Enabled = false;
                numQtyPcs.Enabled = false;
                chkTakeJigHavePcs.Enabled = false;
                loadProgram(ID);

            }
        }

        /// <summary>
        /// Button Delete Modell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelModel_Click(object sender, EventArgs e)
        {
            string Model = Support_SQL.ToString(grvData.GetFocusedRowCellValue(ColProgramName));
            DialogResult rep = MessageBox.Show($"Are you sure you want to delete this Model:{Model} ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rep == DialogResult.Yes)
            {

                int id = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(ColProgramID));
                grvData.DeleteSelectedRows();
                // Xóa các dữ liệu Setting liên quan đến ID_Program đó
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM ProgramMain where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM CameraSetting where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM RegionPosition where ID_Program = '{0}'", id));
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM BarcodeSetting where ID_Program = '{0}'", id));
                // Xóa các file train
                DeleteFileTrain(id);
                Support_SQL.ExecuteQuery(string.Format(@"delete FROM TrainFile where ID_Program = '{0}'", id));

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
            txtDes.ReadOnly = txtNameModel.ReadOnly = true; cbo_ModeReadCode.Enabled = cbJigHaveBarcode.Enabled = chkUsingFVI.Enabled=cbTypeModel.Enabled = false;
            numQtyPcs.Enabled = false;
            chkTakeJigHavePcs.Enabled = false;
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

        private void grvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            txtDes.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(ColDes));
            txtNameModel.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(ColProgramName));
            txt_qtyCam.Value = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(ColQtyCam));
            numQtyPcs.Value = Support_SQL.ToDecimal(grvData.GetFocusedRowCellValue(col_QtyPcs));
            chkTakeJigHavePcs.Checked = Lib.ToBoolean(grvData.GetFocusedRowCellValue(col_TakeJigHavePcs));
            if (Support_SQL.ToString(grvData.GetFocusedRowCellValue(ColQtyCam)).Trim().ToLower() == "enable")
            {
                cbo_ModeReadCode.SelectedIndex = 0;
            }
            else if(Support_SQL.ToString(grvData.GetFocusedRowCellValue(ColQtyCam)).Trim().ToLower() == "disable")//DISABLE
            {
                cbo_ModeReadCode.SelectedIndex = 1;
            }

            if (Support_SQL.ToString(grvData.GetFocusedRowCellValue(colHaveBarcode)).Trim().ToLower() == "enable")
            {
                cbJigHaveBarcode.SelectedIndex = 0;
            }
            else if(Support_SQL.ToString(grvData.GetFocusedRowCellValue(colHaveBarcode)).Trim().ToLower() == "disable")
            {
                cbJigHaveBarcode.SelectedIndex = 1;
            }
            int ID = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(ColProgramID));
            DataTable dt = Support_SQL.GetTableData($"Select * from ProgramMain where ID_Program={ID}");
            if(dt.Rows.Count>0)
            {
                chkUsingFVI.Checked=Support_SQL.ToBoolean(dt.Rows[0]["UsingFVI"]);
                int typeModel = Support_SQL.ToInt(dt.Rows[0]["TypeModel"]);
                if (typeModel > 0)
                    cbTypeModel.SelectedIndex = typeModel;
                else
                    cbTypeModel.SelectedIndex = 0;
            }







            // NB - 20022023
            //int typeModel = Support_SQL.ToInt(Support_SQL.ExecuteScalar($"SELECT TypeModel FROM ProgramMain WHERE ProgramName = '{txtNameModel.Text.Trim()}'"));
            //if (typeModel > 0)
            //    cbTypeModel.SelectedIndex = typeModel;
            //else
            //    cbTypeModel.SelectedIndex = 0;
        }
    }
}
