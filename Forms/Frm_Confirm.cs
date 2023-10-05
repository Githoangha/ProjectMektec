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
    public partial class Frm_Confirm : Form
    {
        public enum Icon_Show
        {
            Error = 0,
            Warning = 1,
            Infor = 2
        }
        public Frm_Confirm(Icon_Show _icon, string _Conten)
        {
            InitializeComponent();
            this.TopMost = true;
            if (_icon == Icon_Show.Error)
            {
                lb_Title.Text = "ERROR";
                lb_Conten.BackColor = Color.FromArgb(255, 128, 128);
                this.pc_logo.Image = Properties.Resources.Error_gif;
            }
            else if (_icon == Icon_Show.Warning)
            {
                lb_Title.Text = "WARNING";
                lb_Conten.BackColor = Color.FromArgb(255, 255, 128);
                this.pc_logo.Image = Properties.Resources.Warning_gif;
            }
            else if (_icon == Icon_Show.Infor)
            {
                lb_Title.Text = "INFOR";
                lb_Conten.BackColor = Color.FromArgb(255, 255, 255);
                this.pc_logo.Image = Properties.Resources.Infor;
            }
            lb_Conten.Text = _Conten;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void Btn_NG_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
