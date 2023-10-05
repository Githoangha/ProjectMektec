
namespace ReadCode
{
    partial class ucViewImage
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
            this.WindowView = new HalconDotNet.HSmartWindowControl();
            this.SuspendLayout();
            // 
            // WindowView
            // 
            this.WindowView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowView.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowView.BackColor = System.Drawing.Color.Transparent;
            this.WindowView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowView.HDoubleClickToFitContent = true;
            this.WindowView.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowView.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.WindowView.HKeepAspectRatio = true;
            this.WindowView.HMoveContent = true;
            this.WindowView.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowView.Location = new System.Drawing.Point(0, 0);
            this.WindowView.Margin = new System.Windows.Forms.Padding(2);
            this.WindowView.Name = "WindowView";
            this.WindowView.Size = new System.Drawing.Size(1414, 622);
            this.WindowView.TabIndex = 56;
            this.WindowView.WindowSize = new System.Drawing.Size(1414, 622);
            // 
            // ucViewImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WindowView);
            this.Name = "ucViewImage";
            this.Size = new System.Drawing.Size(1414, 622);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HSmartWindowControl WindowView;
    }
}
