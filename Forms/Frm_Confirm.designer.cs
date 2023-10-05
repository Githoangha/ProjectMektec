
namespace LineGolden_PLasma
{
    partial class Frm_Confirm
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
            this.pl_Title = new System.Windows.Forms.Panel();
            this.lb_Title = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.btn_NG = new System.Windows.Forms.Button();
            this.lb_Conten = new System.Windows.Forms.RichTextBox();
            this.pc_logo = new System.Windows.Forms.PictureBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.pl_Title.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pc_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // pl_Title
            // 
            this.pl_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(56)))));
            this.pl_Title.Controls.Add(this.lb_Title);
            this.pl_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.pl_Title.ForeColor = System.Drawing.Color.White;
            this.pl_Title.Location = new System.Drawing.Point(0, 0);
            this.pl_Title.Name = "pl_Title";
            this.pl_Title.Size = new System.Drawing.Size(734, 31);
            this.pl_Title.TabIndex = 1;
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Title.ForeColor = System.Drawing.Color.White;
            this.lb_Title.Location = new System.Drawing.Point(4, 4);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(58, 25);
            this.lb_Title.TabIndex = 0;
            this.lb_Title.Text = "Error";
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.btn_NG);
            this.panelMain.Controls.Add(this.lb_Conten);
            this.panelMain.Controls.Add(this.pc_logo);
            this.panelMain.Controls.Add(this.btn_OK);
            this.panelMain.Location = new System.Drawing.Point(0, 37);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(728, 311);
            this.panelMain.TabIndex = 3;
            // 
            // btn_NG
            // 
            this.btn_NG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_NG.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_NG.Image = global::LineGolden_PLasma.Properties.Resources.icons8_cancel_32;
            this.btn_NG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_NG.Location = new System.Drawing.Point(499, 256);
            this.btn_NG.Name = "btn_NG";
            this.btn_NG.Size = new System.Drawing.Size(168, 46);
            this.btn_NG.TabIndex = 6;
            this.btn_NG.Text = "      NG";
            this.btn_NG.UseVisualStyleBackColor = false;
            this.btn_NG.Click += new System.EventHandler(this.Btn_NG_Click);
            // 
            // lb_Conten
            // 
            this.lb_Conten.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Conten.Location = new System.Drawing.Point(136, 6);
            this.lb_Conten.Name = "lb_Conten";
            this.lb_Conten.Size = new System.Drawing.Size(586, 244);
            this.lb_Conten.TabIndex = 5;
            this.lb_Conten.Text = "Content Error Or Warning\n";
            // 
            // pc_logo
            // 
            this.pc_logo.Image = global::LineGolden_PLasma.Properties.Resources.Warning_gif;
            this.pc_logo.Location = new System.Drawing.Point(10, 6);
            this.pc_logo.Name = "pc_logo";
            this.pc_logo.Size = new System.Drawing.Size(120, 106);
            this.pc_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pc_logo.TabIndex = 3;
            this.pc_logo.TabStop = false;
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.Image = global::LineGolden_PLasma.Properties.Resources.icons8_check_circle_32;
            this.btn_OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_OK.Location = new System.Drawing.Point(185, 256);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(168, 46);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // Frm_Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(734, 351);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.pl_Title);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Confirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show Dialog";
            this.pl_Title.ResumeLayout(false);
            this.pl_Title.PerformLayout();
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pc_logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pl_Title;
        private System.Windows.Forms.Label lb_Title;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.RichTextBox lb_Conten;
        private System.Windows.Forms.PictureBox pc_logo;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_NG;
    }
}