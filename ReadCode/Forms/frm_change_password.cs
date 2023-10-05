using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCode
{
    public partial class frm_change_password : Form
    {
        string oldPassword = "";
        public frm_change_password()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void Frm_change_password_Load(object sender, EventArgs e)
        {
            c_varGolbal.str_ConnectDBUsers = "Data Source = " + Application.StartupPath + "\\Config\\User.db;Version=3;";
            DataTable db = Support_SQL.GetTableDataUser($"SELECT * FROM User WHERE NameUser='Admin'");
            if (db.Rows.Count > 0)
                oldPassword = db.Rows[0]["Password"].ToString();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            errorProvider1.SetError(txt_OldPassword, "");
            errorProvider1.SetError(txt_NewPassword, "");
            errorProvider1.SetError(txt_ConfirmPassword, "");
            if (txt_OldPassword.Text == "")
            {
                MessageBox.Show("Hãy điền mật khẩu cũ");
                return;
            }
            if (txt_NewPassword.Text == "")
            {
                MessageBox.Show("Hãy điền mật khẩu mới");
                return;
            }
            if (txt_ConfirmPassword.Text == "")
            {
                MessageBox.Show("Hãy xác thực mật khẩu mới");
                return;
            }
            if (txt_OldPassword.Text != oldPassword)
            {
                errorProvider1.SetError(txt_OldPassword, "mật khẩu cũ không đúng");
                return;
            }
            if (txt_NewPassword.Text != txt_ConfirmPassword.Text)
            {
                errorProvider1.SetError(txt_ConfirmPassword, "xác thực mật khẩu sai");
                return;
            }
            string sql_update = $"UPDATE User SET Password='{txt_ConfirmPassword.Text}' WHERE NameUser='Admin' ";
            Support_SQL.ExecuteScalarUser(sql_update);
            DialogResult = DialogResult.OK;
        }
    }
}
