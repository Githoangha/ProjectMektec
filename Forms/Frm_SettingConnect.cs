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
    public partial class frm_SettingConnect : Form
    {
        public frm_SettingConnect()
        {
            InitializeComponent();
        }
       
        private void frm_SettingPLC_Load(object sender, EventArgs e)
        {
            loadData();
        }

        void loadData()
        {
            string sqlString = "Select * from SettingPLC";
            DataTable dt = Support_SQL.GetTableData(sqlString);
            if(dt.Rows.Count>0)
            {
                txtStationNumber.Text =Support_SQL.ToString(dt.Rows[0]["StationNumber"]);
                txtTriggerReset.Text = Support_SQL.ToString(dt.Rows[0]["TriggerReset"]);
                txtTriggerError.Text = Support_SQL.ToString(dt.Rows[0]["TriggerError"]);
                txtTriggerOK.Text = Support_SQL.ToString(dt.Rows[0]["TriggerOK"]);
                txtTriggerHaveData.Text = Support_SQL.ToString(dt.Rows[0]["TriggerHaveData"]);
                txtTriggerHaveDataOK.Text = Support_SQL.ToString(dt.Rows[0]["TriggerHaveDataOK"]);
                txtTriggerReadCode.Text = Support_SQL.ToString(dt.Rows[0]["TriggerReadCode"]);
                txtTriggerReadCodeOK.Text = Support_SQL.ToString(dt.Rows[0]["TriggerReadCodeOK"]);
                txtTriggerFinish.Text = Support_SQL.ToString(dt.Rows[0]["TriggerFinish"]);
                txtTriggerFinishOK.Text = Support_SQL.ToString(dt.Rows[0]["TriggerFinishOK"]);

            }    
        }
        bool save()
        {
            try
            {
                if (txtTriggerHaveData.Text.Trim() == "")
                {
                    MessageBox.Show("StationNumber không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
               
                string sqlString = "Update SettingPLC "
                                  + $"Set StationNumber={Lib.ToInt(txtTriggerHaveData.Text.Trim())}"+","
                                       + $"StationNumber='{txtStationNumber.Text.Trim()}'" + ","
                                       + $"TriggerReset='{txtTriggerReset.Text.Trim()}'" + ","
                                       + $"TriggerOK='{txtTriggerOK.Text.Trim()}'" + ","
                                       + $"TriggerError='{txtTriggerError.Text.Trim()}'"+","
                                       + $"TriggerReadCode='{txtTriggerReadCode.Text.Trim()}'"+","
                                       + $"TriggerReadCodeOK='{txtTriggerReadCodeOK.Text.Trim()}'" + ","
                                       + $"TriggerHaveData='{txtTriggerHaveData.Text.Trim()}'"+","
                                       + $"TriggerHaveDataOK='{txtTriggerHaveDataOK.Text.Trim()}'"+","
                                       + $"TriggerFinish='{txtTriggerFinish.Text.Trim()}'"+","
                                       + $"TriggerFinishOK='{txtTriggerFinishOK.Text.Trim()}'"
                                  + $"Where ID={1} ";
                Support_SQL.ExecuteQuery(sqlString);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu \r\n"+ex.Message,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
           
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (save())
            {
               // MessageBox.Show("Lưu dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                c_varGolbal.LogicalStationNumberPlasma = Lib.ToInt(txtTriggerHaveData.Text);
            }
        }

        private void txtStationNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
