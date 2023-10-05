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
    public partial class Frm_Login : Form
    {
        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {
            // Load dữ liệu User và password trên DB
            c_varGolbal.str_ConnectDBUsers = "Data Source = " + Application.StartupPath + "\\Config\\User.db;Version=3;";
            loadData();
            txtPassword.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cboUsers.Text == "User")
            {
                c_varGolbal.IsAdmin = false;
            }
            else if(cboUsers.Text == "Admin")
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
            Frm_change_password newfrm = new Frm_change_password();
            this.Hide();
            if (newfrm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Đổi mật khẩu thành công");
                loadData();
            }
            this.Show();
        }
        private void loadData()
        {
            DataTable db = Support_SQL.GetTableDataUser($"SELECT * from User");
            cboUsers.DataSource = db;
            cboUsers.DisplayMember = "NameUser";
            cboUsers.ValueMember = "Password";
            cboUsers.SelectedIndex = 1;
            txtPassword.Focus();
            txtPassword.SelectAll();
        }

        private void Frm_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void txtPassword_DoubleClick(object sender, EventArgs e)
        {
            GlobVar.OnKeyBoard();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

