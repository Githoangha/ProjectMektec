
namespace ReadCode
{
    partial class frm_SettingCamBarcode
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
            this.btnDisconnectCam1 = new System.Windows.Forms.Button();
            this.btnConnectCam1 = new System.Windows.Forms.Button();
            this.txt_PortCamBarcode1 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtMachineIndex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.pn_set = new System.Windows.Forms.Panel();
            this.gbCamBarcode1 = new System.Windows.Forms.GroupBox();
            this.txt_IPCamBarcode1 = new System.Windows.Forms.TextBox();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.cbProgram = new System.Windows.Forms.ComboBox();
            this.pn_set.SuspendLayout();
            this.gbCamBarcode1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDisconnectCam1
            // 
            this.btnDisconnectCam1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisconnectCam1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDisconnectCam1.Enabled = false;
            this.btnDisconnectCam1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisconnectCam1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnectCam1.ForeColor = System.Drawing.Color.Black;
            this.btnDisconnectCam1.Location = new System.Drawing.Point(130, 188);
            this.btnDisconnectCam1.Name = "btnDisconnectCam1";
            this.btnDisconnectCam1.Size = new System.Drawing.Size(319, 36);
            this.btnDisconnectCam1.TabIndex = 54;
            this.btnDisconnectCam1.Text = "Disconnect";
            this.btnDisconnectCam1.UseVisualStyleBackColor = false;
            this.btnDisconnectCam1.Click += new System.EventHandler(this.btnDisconnectCam1_Click);
            // 
            // btnConnectCam1
            // 
            this.btnConnectCam1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectCam1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnConnectCam1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnectCam1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnectCam1.ForeColor = System.Drawing.Color.Black;
            this.btnConnectCam1.Location = new System.Drawing.Point(130, 140);
            this.btnConnectCam1.Name = "btnConnectCam1";
            this.btnConnectCam1.Size = new System.Drawing.Size(319, 36);
            this.btnConnectCam1.TabIndex = 53;
            this.btnConnectCam1.Text = "Connect";
            this.btnConnectCam1.UseVisualStyleBackColor = false;
            this.btnConnectCam1.Click += new System.EventHandler(this.btnConnectCam1_Click);
            // 
            // txt_PortCamBarcode1
            // 
            this.txt_PortCamBarcode1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_PortCamBarcode1.BackColor = System.Drawing.Color.White;
            this.txt_PortCamBarcode1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_PortCamBarcode1.Location = new System.Drawing.Point(130, 94);
            this.txt_PortCamBarcode1.Name = "txt_PortCamBarcode1";
            this.txt_PortCamBarcode1.Size = new System.Drawing.Size(319, 27);
            this.txt_PortCamBarcode1.TabIndex = 52;
            this.txt_PortCamBarcode1.Text = "502";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(19, 96);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(113, 19);
            this.label32.TabIndex = 50;
            this.label32.Text = "Port default:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(19, 46);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(101, 19);
            this.label33.TabIndex = 51;
            this.label33.Text = "IP address:";
            // 
            // txtMachineIndex
            // 
            this.txtMachineIndex.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtMachineIndex.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMachineIndex.Location = new System.Drawing.Point(234, 40);
            this.txtMachineIndex.Name = "txtMachineIndex";
            this.txtMachineIndex.ReadOnly = true;
            this.txtMachineIndex.Size = new System.Drawing.Size(139, 29);
            this.txtMachineIndex.TabIndex = 81;
            this.txtMachineIndex.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(234, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 18);
            this.label4.TabIndex = 79;
            this.label4.Text = "Name Machine";
            this.label4.Visible = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(12, 9);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(133, 18);
            this.label29.TabIndex = 80;
            this.label29.Text = "Program Current";
            // 
            // pn_set
            // 
            this.pn_set.BackColor = System.Drawing.Color.Silver;
            this.pn_set.Controls.Add(this.cbProgram);
            this.pn_set.Controls.Add(this.gbCamBarcode1);
            this.pn_set.Controls.Add(this.btnSaveSetting);
            this.pn_set.Controls.Add(this.txtMachineIndex);
            this.pn_set.Controls.Add(this.label29);
            this.pn_set.Controls.Add(this.label4);
            this.pn_set.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn_set.Location = new System.Drawing.Point(0, 0);
            this.pn_set.Name = "pn_set";
            this.pn_set.Size = new System.Drawing.Size(475, 341);
            this.pn_set.TabIndex = 83;
            // 
            // gbCamBarcode1
            // 
            this.gbCamBarcode1.Controls.Add(this.txt_IPCamBarcode1);
            this.gbCamBarcode1.Controls.Add(this.label33);
            this.gbCamBarcode1.Controls.Add(this.btnConnectCam1);
            this.gbCamBarcode1.Controls.Add(this.txt_PortCamBarcode1);
            this.gbCamBarcode1.Controls.Add(this.label32);
            this.gbCamBarcode1.Controls.Add(this.btnDisconnectCam1);
            this.gbCamBarcode1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbCamBarcode1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCamBarcode1.Location = new System.Drawing.Point(0, 81);
            this.gbCamBarcode1.Name = "gbCamBarcode1";
            this.gbCamBarcode1.Size = new System.Drawing.Size(475, 260);
            this.gbCamBarcode1.TabIndex = 85;
            this.gbCamBarcode1.TabStop = false;
            this.gbCamBarcode1.Text = "SETTING BARCODE ";
            // 
            // txt_IPCamBarcode1
            // 
            this.txt_IPCamBarcode1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_IPCamBarcode1.BackColor = System.Drawing.Color.White;
            this.txt_IPCamBarcode1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_IPCamBarcode1.Location = new System.Drawing.Point(130, 43);
            this.txt_IPCamBarcode1.Name = "txt_IPCamBarcode1";
            this.txt_IPCamBarcode1.Size = new System.Drawing.Size(319, 27);
            this.txt_IPCamBarcode1.TabIndex = 55;
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSetting.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSaveSetting.Image = global::ReadCode.Properties.Resources.icons8_save_50px;
            this.btnSaveSetting.Location = new System.Drawing.Point(402, 9);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(61, 60);
            this.btnSaveSetting.TabIndex = 87;
            this.btnSaveSetting.UseVisualStyleBackColor = false;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // cbProgram
            // 
            this.cbProgram.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbProgram.FormattingEnabled = true;
            this.cbProgram.Location = new System.Drawing.Point(15, 40);
            this.cbProgram.Name = "cbProgram";
            this.cbProgram.Size = new System.Drawing.Size(188, 29);
            this.cbProgram.TabIndex = 88;
            this.cbProgram.SelectedValueChanged += new System.EventHandler(this.cbProgram_SelectedValueChanged);
            // 
            // frm_SettingCamBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(475, 341);
            this.Controls.Add(this.pn_set);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IsMdiContainer = true;
            this.Name = "frm_SettingCamBarcode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SETTING CAMERA BACODE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_SettingCamBarcode_FormClosing);
            this.Load += new System.EventHandler(this.Frm_SettingCamBarcode_Load);
            this.pn_set.ResumeLayout(false);
            this.pn_set.PerformLayout();
            this.gbCamBarcode1.ResumeLayout(false);
            this.gbCamBarcode1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnConnectCam1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtMachineIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txt_PortCamBarcode1;
        private System.Windows.Forms.Panel pn_set;
        private System.Windows.Forms.Button btnDisconnectCam1;
        private System.Windows.Forms.GroupBox gbCamBarcode1;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.TextBox txt_IPCamBarcode1;
        private System.Windows.Forms.ComboBox cbProgram;
    }
}