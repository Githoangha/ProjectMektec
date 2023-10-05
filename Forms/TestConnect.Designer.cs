
namespace LineGolden_PLasma
{
    partial class TestConnect
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddressTrigger = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.txtStationNumber = new DevExpress.XtraEditors.TextEdit();
            this.txtValuesAddress = new DevExpress.XtraEditors.TextEdit();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPing = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValuesAddress.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(502, 32);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(125, 27);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.Location = new System.Drawing.Point(651, 32);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(125, 27);
            this.btnDisconnect.TabIndex = 0;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Station number";
            // 
            // txtAddressTrigger
            // 
            this.txtAddressTrigger.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddressTrigger.Location = new System.Drawing.Point(158, 93);
            this.txtAddressTrigger.Name = "txtAddressTrigger";
            this.txtAddressTrigger.Size = new System.Drawing.Size(285, 27);
            this.txtAddressTrigger.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "TriggerAddress";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Values Address ";
            // 
            // btnRead
            // 
            this.btnRead.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.Location = new System.Drawing.Point(502, 92);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(125, 27);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWrite.Location = new System.Drawing.Point(651, 92);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(125, 27);
            this.btnWrite.TabIndex = 0;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // txtStationNumber
            // 
            this.txtStationNumber.Location = new System.Drawing.Point(158, 31);
            this.txtStationNumber.Name = "txtStationNumber";
            this.txtStationNumber.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtStationNumber.Properties.Appearance.Options.UseFont = true;
            this.txtStationNumber.Properties.DisplayFormat.FormatString = "N0";
            this.txtStationNumber.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtStationNumber.Properties.EditFormat.FormatString = "n0";
            this.txtStationNumber.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtStationNumber.Size = new System.Drawing.Size(285, 26);
            this.txtStationNumber.TabIndex = 3;
            this.txtStationNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStationNumber_KeyPress);
            // 
            // txtValuesAddress
            // 
            this.txtValuesAddress.Location = new System.Drawing.Point(158, 155);
            this.txtValuesAddress.Name = "txtValuesAddress";
            this.txtValuesAddress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtValuesAddress.Properties.Appearance.Options.UseFont = true;
            this.txtValuesAddress.Properties.DisplayFormat.FormatString = "n0";
            this.txtValuesAddress.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValuesAddress.Properties.EditFormat.FormatString = "n0";
            this.txtValuesAddress.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValuesAddress.Size = new System.Drawing.Size(285, 26);
            this.txtValuesAddress.TabIndex = 4;
            this.txtValuesAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStationNumber_KeyPress);
            // 
            // txtIP
            // 
            this.txtIP.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtIP.Location = new System.Drawing.Point(85, 254);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(269, 27);
            this.txtIP.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(43, 262);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "IP";
            // 
            // btnPing
            // 
            this.btnPing.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPing.Location = new System.Drawing.Point(408, 253);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(125, 27);
            this.btnPing.TabIndex = 0;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(128, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "Result :";
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResult.Location = new System.Drawing.Point(186, 315);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(0, 19);
            this.lbResult.TabIndex = 2;
            // 
            // TestConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 427);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtValuesAddress);
            this.Controls.Add(this.txtStationNumber);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAddressTrigger);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnConnect);
            this.Name = "TestConnect";
            this.Text = "TestConnect";
            this.Load += new System.EventHandler(this.TestConnect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtStationNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValuesAddress.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddressTrigger;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private DevExpress.XtraEditors.TextEdit txtStationNumber;
        private DevExpress.XtraEditors.TextEdit txtValuesAddress;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbResult;
    }
}