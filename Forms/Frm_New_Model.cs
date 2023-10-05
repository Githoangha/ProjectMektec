using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineGolden_PLasma
{
    public partial class Frm_New_Model : Form
    {
        #region variable
        private bool Update = false;

        #endregion
        public Frm_New_Model()
        {
            InitializeComponent();
        }

        private void Frm_New_Model_Load(object sender, EventArgs e)
        {
            if (Update)
            {
                btnEdit.Text = "Update CodeTray";
            }
            else
            {
                btnEdit.Text = "Edit CodeTray";
            }
            //load Data
            LoadData();

        }
        void LoadData()
        {
            string sql_select_model = "Select * from ModelCodeTray";
            DataTable dt = Support_SQL.GetTableData(sql_select_model);
            if (dt.Rows.Count > 0)
            {
                grdData.DataSource = dt;
            }
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtValuesCodeTray.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập Model Code Tray để tạo mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string date_time = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                string sql_add = $"INSERT INTO ModelCodeTray (CodeTray,DateTime) VALUES ('{txtValuesCodeTray.Text.Trim()}','{date_time}')";
                //if (MessageBox.Show($"Bạn có muốn thêm Model Code Tray {txtValuesCodeTray.Text.Trim()}?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                Support_SQL.ExecuteQuery(sql_add);
                txtValuesCodeTray.Text = "";
                LoadData();
                //}
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //Button là Edit
                if (!Update)
                {
                    if (Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colID)) <= 0)
                    {
                        return;
                    }
                    btnCreate.Enabled = btnDelete.Enabled = false;
                    btnEdit.Text = "Update CodeTray";
                    Update = true;
                    txtValuesCodeTray.Text = Support_SQL.ToString(grvData.GetFocusedRowCellValue(colCodeTray));
                }
                //Button là Update
                else
                {
                    btnCreate.Enabled = btnDelete.Enabled = true;

                    // if (MessageBox.Show("Bạn có muốn lưu giá trị CodeTray vừa sửa không ??", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    int ID = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colID));
                    string sql_update = $"UPDATE ModelCodeTray SET CodeTray='{txtValuesCodeTray.Text.Trim()}' WHERE ID={ID}";
                    Support_SQL.ExecuteQuery(sql_update);
                    LoadData();
                    //}
                    btnEdit.Text = "Edit CodeTray";
                    Update = false;

                    txtValuesCodeTray.Text = "";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int ID = Support_SQL.ToInt(grvData.GetFocusedRowCellValue(colID));
                if (ID > 0)
                {
                    string sql_delete_model = $"DELETE FROM ModelCodeTray WHERE ID={ID}";
                    Support_SQL.ExecuteQuery(sql_delete_model);
                    //LoadData();
                    grvData.DeleteSelectedRows();

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void Frm_New_Model_FormClosed(object sender, FormClosedEventArgs e)
        {
            c_varGolbal.List_Model_CodeTray.Clear();
            //Load Lại List CodeTray
            DataTable dt = Support_SQL.GetTableData("Select * from ModelCodeTray");
            foreach (DataRow item in dt.Rows)
            {
                c_varGolbal.List_Model_CodeTray.Add(Support_SQL.ToString(item["CodeTray"]));
            }
        }
    }
}
