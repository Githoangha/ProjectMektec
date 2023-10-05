
namespace LineGolden_PLasma
{
    partial class Frm_ShowDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pl_Title = new System.Windows.Forms.Panel();
            this.lb_Title = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.pc_logo = new System.Windows.Forms.PictureBox();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lb_Conten = new System.Windows.Forms.TextBox();
            this.pl_Title.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pc_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // pl_Title
            // 
            this.pl_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(56)))));
            this.pl_Title.Controls.Add(this.lb_Title);
            this.pl_Title.ForeColor = System.Drawing.Color.White;
            this.pl_Title.Location = new System.Drawing.Point(0, 0);
            this.pl_Title.Name = "pl_Title";
            this.pl_Title.Size = new System.Drawing.Size(638, 25);
            this.pl_Title.TabIndex = 1;
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Title.ForeColor = System.Drawing.Color.White;
            this.lb_Title.Location = new System.Drawing.Point(4, 4);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(57, 22);
            this.lb_Title.TabIndex = 0;
            this.lb_Title.Text = "Error";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.lb_Conten);
            this.panelMain.Controls.Add(this.pc_logo);
            this.panelMain.Controls.Add(this.btn_Confirm);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(962, 488);
            this.panelMain.TabIndex = 3;
            // 
            // pc_logo
            // 
            this.pc_logo.Image = global::LineGolden_PLasma.Properties.Resources.Infor;
            this.pc_logo.Location = new System.Drawing.Point(10, 6);
            this.pc_logo.Name = "pc_logo";
            this.pc_logo.Size = new System.Drawing.Size(120, 106);
            this.pc_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pc_logo.TabIndex = 3;
            this.pc_logo.TabStop = false;
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Confirm.Font = new System.Drawing.Font("Book Antiqua", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Confirm.Image = global::LineGolden_PLasma.Properties.Resources.icons8_verified_account_48px;
            this.btn_Confirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Confirm.Location = new System.Drawing.Point(459, 430);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(168, 46);
            this.btn_Confirm.TabIndex = 4;
            this.btn_Confirm.Text = "      Confirm";
            this.btn_Confirm.UseVisualStyleBackColor = false;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // lb_Conten
            // 
            this.lb_Conten.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Conten.Location = new System.Drawing.Point(136, 12);
            this.lb_Conten.Multiline = true;
            this.lb_Conten.Name = "lb_Conten";
            this.lb_Conten.Size = new System.Drawing.Size(814, 412);
            this.lb_Conten.TabIndex = 5;
            this.lb_Conten.Text = "Context Information";
            // 
            // Frm_ShowDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(962, 488);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.pl_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_ShowDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show Dialog";
            this.pl_Title.ResumeLayout(false);
            this.pl_Title.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pc_logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pl_Title;
        private System.Windows.Forms.Label lb_Title;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox pc_logo;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox lb_Conten;
    }
}