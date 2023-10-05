
namespace LineGolden_PLasma
{
    partial class Frm_SettingCamBarcode
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
            this.txtPlasmaIndex = new System.Windows.Forms.TextBox();
            this.txtProgram = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.pn_set = new System.Windows.Forms.Panel();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.numBarcodeCam = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbCamBarcode1 = new System.Windows.Forms.GroupBox();
            this.txt_IPCamBarcode1 = new System.Windows.Forms.TextBox();
            this.pn_set.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBarcodeCam)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbCamBarcode1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDisconnectCam1
            // 
            this.btnDisconnectCam1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDisconnectCam1.Enabled = false;
            this.btnDisconnectCam1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisconnectCam1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnectCam1.ForeColor = System.Drawing.Color.Black;
            this.btnDisconnectCam1.Location = new System.Drawing.Point(185, 191);
            this.btnDisconnectCam1.Name = "btnDisconnectCam1";
            this.btnDisconnectCam1.Size = new System.Drawing.Size(207, 36);
            this.btnDisconnectCam1.TabIndex = 54;
            this.btnDisconnectCam1.Text = "Disconnect";
            this.btnDisconnectCam1.UseVisualStyleBackColor = false;
            this.btnDisconnectCam1.Click += new System.EventHandler(this.btnDisconnectCam1_Click);
            // 
            // btnConnectCam1
            // 
            this.btnConnectCam1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnConnectCam1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnectCam1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnectCam1.ForeColor = System.Drawing.Color.Black;
            this.btnConnectCam1.Location = new System.Drawing.Point(185, 143);
            this.btnConnectCam1.Name = "btnConnectCam1";
            this.btnConnectCam1.Size = new System.Drawing.Size(207, 36);
            this.btnConnectCam1.TabIndex = 53;
            this.btnConnectCam1.Text = "Connect";
            this.btnConnectCam1.UseVisualStyleBackColor = false;
            this.btnConnectCam1.Click += new System.EventHandler(this.btnConnectCam1_Click);
            // 
            // txt_PortCamBarcode1
            // 
            this.txt_PortCamBarcode1.BackColor = System.Drawing.Color.White;
            this.txt_PortCamBarcode1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_PortCamBarcode1.Location = new System.Drawing.Point(130, 97);
            this.txt_PortCamBarcode1.Name = "txt_PortCamBarcode1";
            this.txt_PortCamBarcode1.Size = new System.Drawing.Size(317, 27);
            this.txt_PortCamBarcode1.TabIndex = 52;
            this.txt_PortCamBarcode1.Text = "502";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(19, 99);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(105, 21);
            this.label32.TabIndex = 50;
            this.label32.Text = "Port default:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(19, 49);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(91, 21);
            this.label33.TabIndex = 51;
            this.label33.Text = "IP address:";
            // 
            // txtPlasmaIndex
            // 
            this.txtPlasmaIndex.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtPlasmaIndex.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlasmaIndex.Location = new System.Drawing.Point(182, 29);
            this.txtPlasmaIndex.Name = "txtPlasmaIndex";
            this.txtPlasmaIndex.ReadOnly = true;
            this.txtPlasmaIndex.Size = new System.Drawing.Size(139, 29);
            this.txtPlasmaIndex.TabIndex = 81;
            // 
            // txtProgram
            // 
            this.txtProgram.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtProgram.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgram.Location = new System.Drawing.Point(12, 29);
            this.txtProgram.Name = "txtProgram";
            this.txtProgram.ReadOnly = true;
            this.txtProgram.Size = new System.Drawing.Size(140, 29);
            this.txtProgram.TabIndex = 82;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(182, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 17);
            this.label4.TabIndex = 79;
            this.label4.Text = "Name Plasma";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(15, 9);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(111, 17);
            this.label29.TabIndex = 80;
            this.label29.Text = "Program Current";
            // 
            // pn_set
            // 
            this.pn_set.BackColor = System.Drawing.Color.Silver;
            this.pn_set.Controls.Add(this.btnSaveSetting);
            this.pn_set.Controls.Add(this.numBarcodeCam);
            this.pn_set.Controls.Add(this.label3);
            this.pn_set.Controls.Add(this.tableLayoutPanel1);
            this.pn_set.Controls.Add(this.txtPlasmaIndex);
            this.pn_set.Controls.Add(this.label29);
            this.pn_set.Controls.Add(this.txtProgram);
            this.pn_set.Controls.Add(this.label4);
            this.pn_set.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn_set.Location = new System.Drawing.Point(0, 0);
            this.pn_set.Name = "pn_set";
            this.pn_set.Size = new System.Drawing.Size(472, 312);
            this.pn_set.TabIndex = 83;
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSaveSetting.Image = global::LineGolden_PLasma.Properties.Resources.icons8_save_50px;
            this.btnSaveSetting.Location = new System.Drawing.Point(402, 10);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(58, 50);
            this.btnSaveSetting.TabIndex = 87;
            this.btnSaveSetting.UseVisualStyleBackColor = false;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // numBarcodeCam
            // 
            this.numBarcodeCam.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBarcodeCam.Location = new System.Drawing.Point(485, 36);
            this.numBarcodeCam.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numBarcodeCam.Name = "numBarcodeCam";
            this.numBarcodeCam.Size = new System.Drawing.Size(55, 29);
            this.numBarcodeCam.TabIndex = 86;
            this.numBarcodeCam.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBarcodeCam.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(482, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 17);
            this.label3.TabIndex = 85;
            this.label3.Text = "Number Barcode";
            this.label3.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.311F));
            this.tableLayoutPanel1.Controls.Add(this.gbCamBarcode1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 66);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 246);
            this.tableLayoutPanel1.TabIndex = 84;
            // 
            // gbCamBarcode1
            // 
            this.gbCamBarcode1.Controls.Add(this.txt_IPCamBarcode1);
            this.gbCamBarcode1.Controls.Add(this.label33);
            this.gbCamBarcode1.Controls.Add(this.btnConnectCam1);
            this.gbCamBarcode1.Controls.Add(this.txt_PortCamBarcode1);
            this.gbCamBarcode1.Controls.Add(this.label32);
            this.gbCamBarcode1.Controls.Add(this.btnDisconnectCam1);
            this.gbCamBarcode1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCamBarcode1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCamBarcode1.Location = new System.Drawing.Point(4, 4);
            this.gbCamBarcode1.Name = "gbCamBarcode1";
            this.gbCamBarcode1.Size = new System.Drawing.Size(464, 238);
            this.gbCamBarcode1.TabIndex = 85;
            this.gbCamBarcode1.TabStop = false;
            this.gbCamBarcode1.Text = "BARCODE SETTING";
            // 
            // txt_IPCamBarcode1
            // 
            this.txt_IPCamBarcode1.BackColor = System.Drawing.Color.White;
            this.txt_IPCamBarcode1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_IPCamBarcode1.Location = new System.Drawing.Point(130, 46);
            this.txt_IPCamBarcode1.Name = "txt_IPCamBarcode1";
            this.txt_IPCamBarcode1.Size = new System.Drawing.Size(317, 27);
            this.txt_IPCamBarcode1.TabIndex = 55;
            // 
            // Frm_SettingCamBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(472, 312);
            this.Controls.Add(this.pn_set);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "Frm_SettingCamBarcode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting Camera Barcode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_SettingCamBarcode_FormClosing);
            this.Load += new System.EventHandler(this.Frm_SettingCamBarcode_Load);
            this.pn_set.ResumeLayout(false);
            this.pn_set.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBarcodeCam)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbCamBarcode1.ResumeLayout(false);
            this.gbCamBarcode1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnConnectCam1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtPlasmaIndex;
        private System.Windows.Forms.TextBox txtProgram;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txt_PortCamBarcode1;
        private System.Windows.Forms.Panel pn_set;
        private System.Windows.Forms.Button btnDisconnectCam1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numBarcodeCam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbCamBarcode1;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.TextBox txt_IPCamBarcode1;
    }
}