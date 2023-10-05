
namespace LineGolden_PLasma
{
    partial class FrmImageLoad
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
            this.picImageLoad = new System.Windows.Forms.PictureBox();
            this.btn_Close = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picImageLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Close)).BeginInit();
            this.SuspendLayout();
            // 
            // picImageLoad
            // 
            this.picImageLoad.BackColor = System.Drawing.Color.White;
            this.picImageLoad.Image = global::LineGolden_PLasma.Properties.Resources.loader1;
            this.picImageLoad.Location = new System.Drawing.Point(12, 53);
            this.picImageLoad.Name = "picImageLoad";
            this.picImageLoad.Size = new System.Drawing.Size(433, 319);
            this.picImageLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picImageLoad.TabIndex = 0;
            this.picImageLoad.TabStop = false;
            // 
            // btn_Close
            // 
            this.btn_Close.Image = global::LineGolden_PLasma.Properties.Resources.icons8_Close_48px;
            this.btn_Close.InitialImage = null;
            this.btn_Close.Location = new System.Drawing.Point(397, 12);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(48, 36);
            this.btn_Close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Close.TabIndex = 72;
            this.btn_Close.TabStop = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // FrmImageLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(457, 384);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.picImageLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmImageLoad";
            this.Text = "FrmImageLoad";
            this.Load += new System.EventHandler(this.FrmImageLoad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picImageLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Close)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picImageLoad;
        private System.Windows.Forms.PictureBox btn_Close;
    }
}