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
    public partial class Frm_ShowDialog : Form
    {
        public enum Icon_Show
        {
            Error = 0,
            Warning = 1,
            Infor=2
        }
        public Frm_ShowDialog(Icon_Show _icon, string _Conten)
        {
            InitializeComponent();
            this.TopMost = true;
            //this.Activate();
            timer1.Enabled = false;
            timer1.Stop();
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
        public Frm_ShowDialog(Icon_Show _icon, string _Conten, Font fontStyle)
        {
            InitializeComponent();
            lb_Conten.Font = fontStyle;
            this.TopMost = true;
            //this.Activate();
            timer1.Enabled = false;
            timer1.Stop();
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
        public Frm_ShowDialog(Icon_Show _icon, string _Conten,int time)
        {
            InitializeComponent();
            this.TopMost = true;
            //this.Activate();
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
            timer1.Enabled = true;
            timer1.Interval = time;
            timer1.Start();
        }
        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }
    }
}
