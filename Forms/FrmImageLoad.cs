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
    
    public partial class FrmImageLoad : Form
    {
        Timer timer1 = new Timer();
        public FrmImageLoad()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        private void FrmImageLoad_Load(object sender, EventArgs e)
        {
            picImageLoad.Image= global::LineGolden_PLasma.Properties.Resources.loader1;
            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Frm_Main.BackupSuccess)
            {
                timer1.Stop();
                this.Close();
            }
        }
    }
}
