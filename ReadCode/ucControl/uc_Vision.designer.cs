namespace ReadCode
{
    partial class uc_Vision
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_Vision));
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_IndexCam = new System.Windows.Forms.Label();
            this.lb_X_Window = new System.Windows.Forms.Label();
            this.lb_Y_Window = new System.Windows.Forms.Label();
            this.lb_Status = new System.Windows.Forms.Label();
            this.btn_Setting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WindowControl
            // 
            this.WindowControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl.BackColor = System.Drawing.Color.Transparent;
            this.WindowControl.HDoubleClickToFitContent = true;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.WindowControl.HKeepAspectRatio = true;
            this.WindowControl.HMoveContent = true;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.Location = new System.Drawing.Point(0, 27);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(452, 269);
            this.WindowControl.TabIndex = 55;
            this.WindowControl.WindowSize = new System.Drawing.Size(452, 269);
            this.WindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.WindowControl1_HMouseMove);
            this.WindowControl.Load += new System.EventHandler(this.WindowControl1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 58;
            this.label1.Text = "Camera:";
            // 
            // lb_IndexCam
            // 
            this.lb_IndexCam.AutoSize = true;
            this.lb_IndexCam.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_IndexCam.Location = new System.Drawing.Point(60, 4);
            this.lb_IndexCam.Name = "lb_IndexCam";
            this.lb_IndexCam.Size = new System.Drawing.Size(23, 20);
            this.lb_IndexCam.TabIndex = 59;
            this.lb_IndexCam.Text = "00";
            // 
            // lb_X_Window
            // 
            this.lb_X_Window.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_X_Window.AutoSize = true;
            this.lb_X_Window.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_X_Window.Location = new System.Drawing.Point(303, 7);
            this.lb_X_Window.Name = "lb_X_Window";
            this.lb_X_Window.Size = new System.Drawing.Size(53, 13);
            this.lb_X_Window.TabIndex = 60;
            this.lb_X_Window.Text = "X = 000.0";
            // 
            // lb_Y_Window
            // 
            this.lb_Y_Window.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_Y_Window.AutoSize = true;
            this.lb_Y_Window.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Y_Window.Location = new System.Drawing.Point(361, 7);
            this.lb_Y_Window.Name = "lb_Y_Window";
            this.lb_Y_Window.Size = new System.Drawing.Size(54, 13);
            this.lb_Y_Window.TabIndex = 61;
            this.lb_Y_Window.Text = "Y = 000.0";
            // 
            // lb_Status
            // 
            this.lb_Status.AutoSize = true;
            this.lb_Status.BackColor = System.Drawing.Color.Red;
            this.lb_Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Status.ForeColor = System.Drawing.Color.Black;
            this.lb_Status.Location = new System.Drawing.Point(85, 5);
            this.lb_Status.Name = "lb_Status";
            this.lb_Status.Size = new System.Drawing.Size(87, 18);
            this.lb_Status.TabIndex = 62;
            this.lb_Status.Text = "Disconnect";
            // 
            // btn_Setting
            // 
            this.btn_Setting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Setting.BackColor = System.Drawing.Color.Yellow;
            this.btn_Setting.Image = ((System.Drawing.Image)(resources.GetObject("btn_Setting.Image")));
            this.btn_Setting.Location = new System.Drawing.Point(421, 3);
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(29, 22);
            this.btn_Setting.TabIndex = 57;
            this.btn_Setting.UseVisualStyleBackColor = false;
            this.btn_Setting.Click += new System.EventHandler(this.bnt_Setting_Click);
            // 
            // uc_Vision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lb_Status);
            this.Controls.Add(this.lb_Y_Window);
            this.Controls.Add(this.lb_X_Window);
            this.Controls.Add(this.lb_IndexCam);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Setting);
            this.Controls.Add(this.WindowControl);
            this.Name = "uc_Vision";
            this.Size = new System.Drawing.Size(452, 296);
            this.Load += new System.EventHandler(this.ucWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HSmartWindowControl WindowControl;
        private System.Windows.Forms.Button btn_Setting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_IndexCam;
        private System.Windows.Forms.Label lb_X_Window;
        private System.Windows.Forms.Label lb_Y_Window;
        private System.Windows.Forms.Label lb_Status;
    }
}
