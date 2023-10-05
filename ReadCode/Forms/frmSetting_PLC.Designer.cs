
namespace ReadCode.Forms
{
    partial class frmSetting_PLC
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP_PLC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort_PLC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTriggerButton = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTriggerSensor = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTriggerLamp = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTriggerError = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTriggerOK = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ConnectStringFVI = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTriggerHaveData = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTriggerHaveDataOK = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTriggerPCReady = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(656, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Image = global::ReadCode.Properties.Resources.icons8_save_50px;
            this.btnSave.Location = new System.Drawing.Point(2, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 66);
            this.btnSave.TabIndex = 0;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP_PLC";
            // 
            // txtIP_PLC
            // 
            this.txtIP_PLC.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP_PLC.Location = new System.Drawing.Point(138, 86);
            this.txtIP_PLC.Name = "txtIP_PLC";
            this.txtIP_PLC.Size = new System.Drawing.Size(133, 27);
            this.txtIP_PLC.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port_PLC";
            // 
            // txtPort_PLC
            // 
            this.txtPort_PLC.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort_PLC.Location = new System.Drawing.Point(138, 121);
            this.txtPort_PLC.Name = "txtPort_PLC";
            this.txtPort_PLC.Size = new System.Drawing.Size(133, 27);
            this.txtPort_PLC.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "TriggerButton";
            // 
            // txtTriggerButton
            // 
            this.txtTriggerButton.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerButton.Location = new System.Drawing.Point(138, 156);
            this.txtTriggerButton.Name = "txtTriggerButton";
            this.txtTriggerButton.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerButton.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "TriggerSensor";
            // 
            // txtTriggerSensor
            // 
            this.txtTriggerSensor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerSensor.Location = new System.Drawing.Point(138, 191);
            this.txtTriggerSensor.Name = "txtTriggerSensor";
            this.txtTriggerSensor.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerSensor.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(26, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "TriggerLamp";
            // 
            // txtTriggerLamp
            // 
            this.txtTriggerLamp.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerLamp.Location = new System.Drawing.Point(138, 226);
            this.txtTriggerLamp.Name = "txtTriggerLamp";
            this.txtTriggerLamp.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerLamp.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(26, 265);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "TriggerError";
            // 
            // txtTriggerError
            // 
            this.txtTriggerError.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerError.Location = new System.Drawing.Point(138, 261);
            this.txtTriggerError.Name = "txtTriggerError";
            this.txtTriggerError.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerError.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(315, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 19);
            this.label7.TabIndex = 1;
            this.label7.Text = "TriggerOK";
            // 
            // txtTriggerOK
            // 
            this.txtTriggerOK.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerOK.Location = new System.Drawing.Point(481, 191);
            this.txtTriggerOK.Name = "txtTriggerOK";
            this.txtTriggerOK.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerOK.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(315, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 19);
            this.label8.TabIndex = 1;
            this.label8.Text = "ConnectStringFVI";
            // 
            // ConnectStringFVI
            // 
            this.ConnectStringFVI.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectStringFVI.Location = new System.Drawing.Point(481, 226);
            this.ConnectStringFVI.Name = "ConnectStringFVI";
            this.ConnectStringFVI.Size = new System.Drawing.Size(133, 27);
            this.ConnectStringFVI.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(315, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 19);
            this.label9.TabIndex = 1;
            this.label9.Text = "TriggerHaveData";
            // 
            // txtTriggerHaveData
            // 
            this.txtTriggerHaveData.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerHaveData.Location = new System.Drawing.Point(481, 156);
            this.txtTriggerHaveData.Name = "txtTriggerHaveData";
            this.txtTriggerHaveData.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerHaveData.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(315, 125);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(149, 19);
            this.label10.TabIndex = 1;
            this.label10.Text = "TriggerHaveDataOK";
            // 
            // txtTriggerHaveDataOK
            // 
            this.txtTriggerHaveDataOK.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerHaveDataOK.Location = new System.Drawing.Point(481, 121);
            this.txtTriggerHaveDataOK.Name = "txtTriggerHaveDataOK";
            this.txtTriggerHaveDataOK.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerHaveDataOK.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(315, 90);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 19);
            this.label11.TabIndex = 1;
            this.label11.Text = "TriggerPCReady";
            // 
            // txtTriggerPCReady
            // 
            this.txtTriggerPCReady.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerPCReady.Location = new System.Drawing.Point(481, 86);
            this.txtTriggerPCReady.Name = "txtTriggerPCReady";
            this.txtTriggerPCReady.Size = new System.Drawing.Size(133, 27);
            this.txtTriggerPCReady.TabIndex = 2;
            // 
            // frmSetting_PLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 310);
            this.Controls.Add(this.ConnectStringFVI);
            this.Controls.Add(this.txtTriggerPCReady);
            this.Controls.Add(this.txtTriggerHaveDataOK);
            this.Controls.Add(this.txtTriggerHaveData);
            this.Controls.Add(this.txtTriggerOK);
            this.Controls.Add(this.txtTriggerError);
            this.Controls.Add(this.txtTriggerLamp);
            this.Controls.Add(this.txtTriggerSensor);
            this.Controls.Add(this.txtTriggerButton);
            this.Controls.Add(this.txtPort_PLC);
            this.Controls.Add(this.txtIP_PLC);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmSetting_PLC";
            this.Text = "Setting PLC";
            this.Load += new System.EventHandler(this.frmSetting_PLC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP_PLC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort_PLC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTriggerButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTriggerSensor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTriggerLamp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTriggerError;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTriggerOK;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ConnectStringFVI;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTriggerHaveData;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTriggerHaveDataOK;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTriggerPCReady;
    }
}