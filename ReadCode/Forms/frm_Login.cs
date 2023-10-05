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
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_Login_Load(object sender, EventArgs e)
        {
            // NB - 15022023
            string path = Application.StartupPath + "\\path_IP_PVI_Plassma_Server.txt";
            if (File.Exists(path))
            {
                string connectDBName = File.ReadAllText(path);
                c_varGolbal.str_ConnectDBConffig_FVI_Server = @"Data Source = " + connectDBName + ";Version=3;";
            }

            // gán str_ConnectDBConffig
            c_varGolbal.str_ConnectDB = "Data Source = " + Application.StartupPath + "\\DB\\DB_ReadCode.db;Version=3;";
            if(!Directory.Exists(Application.StartupPath + "\\DB\\BACK_UP"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\DB\\BACK_UP");
                
            }
            if(!Lib.IsFile(Application.StartupPath + "\\DB\\BACK_UP\\" + "DB_ReadCode.db"))
            {
                Lib.CopyFileTo(Application.StartupPath + "\\DB\\DB_ReadCode.db", Application.StartupPath + "\\DB\\BACK_UP\\" + "DB_ReadCode.db");
            }
            c_varGolbal.str_ConnectDB_Backup= "Data Source = " + Application.StartupPath + "\\DB\\BACK_UP\\" + "DB_ReadCode.db;Version=3;";
            loadData();

            if (c_varGolbal.IsAdmin)
            {
                cboUsers.Text = "Admin";
                DataRow [] rows = dtUser.Select($"UserName = '{cboUsers.Text}'");
                txtPassword.Text = rows[0]["PassWord"].ToString();
            }
            else
            {
                cboUsers.Text = "User";
                txtPassword.Clear();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cboUsers.Text == "User")
            {
                c_varGolbal.IsAdmin = false;
            }
            else if (cboUsers.Text == "Admin")
            {
                if (cboUsers.SelectedValue.ToString() == txtPassword.Text)
                {
                    c_varGolbal.IsAdmin = true;
                }
                else
                {
                    MessageBox.Show("Nhập sai mật khẩu, xin vui lòng nhập lại!");
                    txtPassword.Clear();
                    return;
                }
            }
            txtPassword.Clear();
            this.DialogResult = DialogResult.OK;

        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void CboUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            frm_change_password newfrm = new frm_change_password();
            this.Hide();
            if (newfrm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Đổi mật khẩu thành công");
                loadData();
            }
            this.Show();
        }

        DataTable dtUser;
        private void loadData()
        {
            dtUser = Support_SQL.GetTableData("SELECT * from Users");
            cboUsers.DataSource = dtUser;
            if (dtUser.Rows.Count > 0)
            {
                cboUsers.DisplayMember = "UserName";
                cboUsers.ValueMember = "PassWord";
                cboUsers.SelectedIndex = 1;
            }
            txtPassword.Focus();

        }

        private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPassword.Clear();
        }
    }
}
