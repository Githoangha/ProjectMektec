
namespace ReadCode
{
    partial class uc_BarcodeVision
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_BarcodeVision));
            this.lb_Status = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_SettingBarcode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_Status
            // 
            this.lb_Status.AutoSize = true;
            this.lb_Status.BackColor = System.Drawing.Color.Red;
            this.lb_Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Status.ForeColor = System.Drawing.Color.Black;
            this.lb_Status.Location = new System.Drawing.Point(127, 3);
            this.lb_Status.Name = "lb_Status";
            this.lb_Status.Size = new System.Drawing.Size(87, 18);
            this.lb_Status.TabIndex = 93;
            this.lb_Status.Text = "Disconnect";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(3, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(108, 20);
            this.label14.TabIndex = 92;
            this.label14.Text = "Barcode Vision:";
            // 
            // btn_SettingBarcode
            // 
            this.btn_SettingBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SettingBarcode.BackColor = System.Drawing.Color.Yellow;
            this.btn_SettingBarcode.Image = ((System.Drawing.Image)(resources.GetObject("btn_SettingBarcode.Image")));
            this.btn_SettingBarcode.Location = new System.Drawing.Point(347, 1);
            this.btn_SettingBarcode.Name = "btn_SettingBarcode";
            this.btn_SettingBarcode.Size = new System.Drawing.Size(29, 22);
            this.btn_SettingBarcode.TabIndex = 94;
            this.btn_SettingBarcode.UseVisualStyleBackColor = false;
            this.btn_SettingBarcode.Click += new System.EventHandler(this.bnt_SettingBarcode_Vision_Click);
            // 
            // uc_BarcodeVision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btn_SettingBarcode);
            this.Controls.Add(this.lb_Status);
            this.Controls.Add(this.label14);
            this.Name = "uc_BarcodeVision";
            this.Size = new System.Drawing.Size(378, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_SettingBarcode;
        private System.Windows.Forms.Label lb_Status;
        private System.Windows.Forms.Label label14;
    }
}
