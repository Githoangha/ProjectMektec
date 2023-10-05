
namespace ReadCode
{
    partial class frm_Data
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
            this.button1 = new System.Windows.Forms.Button();
            this.grdData = new DevExpress.XtraGrid.GridControl();
            this.grvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID_Progarm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCode_TagJigNonePCS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCode_TagJigHavePCS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodePCS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDate_Time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatusUpload = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.button1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1351, 57);
            this.panelControl1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button1.Image = global::ReadCode.Properties.Resources.ExportToXLSX_32x32;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(1147, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(199, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "Upload Data Waiting";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grdData
            // 
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(0, 57);
            this.grdData.MainView = this.grvData;
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(1351, 701);
            this.grdData.TabIndex = 2;
            this.grdData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvData});
            // 
            // grvData
            // 
            this.grvData.ColumnPanelRowHeight = 40;
            this.grvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colID_Progarm,
            this.colCode_TagJigNonePCS,
            this.colCode_TagJigHavePCS,
            this.colCodePCS,
            this.colDate_Time,
            this.colStatusUpload});
            this.grvData.GridControl = this.grdData;
            this.grvData.Name = "grvData";
            this.grvData.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colID_Progarm
            // 
            this.colID_Progarm.Caption = "ID_Progarm";
            this.colID_Progarm.FieldName = "ID_Progarm";
            this.colID_Progarm.Name = "colID_Progarm";
            // 
            // colCode_TagJigNonePCS
            // 
            this.colCode_TagJigNonePCS.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colCode_TagJigNonePCS.AppearanceCell.Options.UseFont = true;
            this.colCode_TagJigNonePCS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colCode_TagJigNonePCS.AppearanceHeader.Options.UseFont = true;
            this.colCode_TagJigNonePCS.AppearanceHeader.Options.UseForeColor = true;
            this.colCode_TagJigNonePCS.AppearanceHeader.Options.UseTextOptions = true;
            this.colCode_TagJigNonePCS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCode_TagJigNonePCS.Caption = "Code TagJigNonePCS";
            this.colCode_TagJigNonePCS.FieldName = "Code_TagJigNonePCS";
            this.colCode_TagJigNonePCS.Name = "colCode_TagJigNonePCS";
            this.colCode_TagJigNonePCS.OptionsColumn.ReadOnly = true;
            this.colCode_TagJigNonePCS.Visible = true;
            this.colCode_TagJigNonePCS.VisibleIndex = 0;
            this.colCode_TagJigNonePCS.Width = 189;
            // 
            // colCode_TagJigHavePCS
            // 
            this.colCode_TagJigHavePCS.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colCode_TagJigHavePCS.AppearanceCell.Options.UseFont = true;
            this.colCode_TagJigHavePCS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colCode_TagJigHavePCS.AppearanceHeader.Options.UseFont = true;
            this.colCode_TagJigHavePCS.AppearanceHeader.Options.UseForeColor = true;
            this.colCode_TagJigHavePCS.AppearanceHeader.Options.UseTextOptions = true;
            this.colCode_TagJigHavePCS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCode_TagJigHavePCS.Caption = "Code TagJigHavePCS";
            this.colCode_TagJigHavePCS.FieldName = "Code_TagJigHavePCS";
            this.colCode_TagJigHavePCS.Name = "colCode_TagJigHavePCS";
            this.colCode_TagJigHavePCS.OptionsColumn.ReadOnly = true;
            this.colCode_TagJigHavePCS.Visible = true;
            this.colCode_TagJigHavePCS.VisibleIndex = 1;
            this.colCode_TagJigHavePCS.Width = 178;
            // 
            // colCodePCS
            // 
            this.colCodePCS.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colCodePCS.AppearanceCell.Options.UseFont = true;
            this.colCodePCS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colCodePCS.AppearanceHeader.Options.UseFont = true;
            this.colCodePCS.AppearanceHeader.Options.UseForeColor = true;
            this.colCodePCS.AppearanceHeader.Options.UseTextOptions = true;
            this.colCodePCS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCodePCS.Caption = "CodePCS";
            this.colCodePCS.FieldName = "CodePCS";
            this.colCodePCS.Name = "colCodePCS";
            this.colCodePCS.OptionsColumn.ReadOnly = true;
            this.colCodePCS.Visible = true;
            this.colCodePCS.VisibleIndex = 2;
            this.colCodePCS.Width = 570;
            // 
            // colDate_Time
            // 
            this.colDate_Time.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colDate_Time.AppearanceCell.Options.UseFont = true;
            this.colDate_Time.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colDate_Time.AppearanceHeader.Options.UseFont = true;
            this.colDate_Time.AppearanceHeader.Options.UseForeColor = true;
            this.colDate_Time.AppearanceHeader.Options.UseTextOptions = true;
            this.colDate_Time.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDate_Time.Caption = "Date_Time";
            this.colDate_Time.FieldName = "Date_Time";
            this.colDate_Time.Name = "colDate_Time";
            this.colDate_Time.OptionsColumn.ReadOnly = true;
            this.colDate_Time.Visible = true;
            this.colDate_Time.VisibleIndex = 3;
            this.colDate_Time.Width = 232;
            // 
            // colStatusUpload
            // 
            this.colStatusUpload.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colStatusUpload.AppearanceCell.Options.UseFont = true;
            this.colStatusUpload.AppearanceCell.Options.UseTextOptions = true;
            this.colStatusUpload.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatusUpload.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colStatusUpload.AppearanceHeader.Options.UseFont = true;
            this.colStatusUpload.AppearanceHeader.Options.UseForeColor = true;
            this.colStatusUpload.AppearanceHeader.Options.UseTextOptions = true;
            this.colStatusUpload.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatusUpload.Caption = "StatusUpload";
            this.colStatusUpload.FieldName = "StatusUpload";
            this.colStatusUpload.Name = "colStatusUpload";
            this.colStatusUpload.OptionsColumn.ReadOnly = true;
            this.colStatusUpload.Visible = true;
            this.colStatusUpload.VisibleIndex = 4;
            this.colStatusUpload.Width = 152;
            // 
            // frm_Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 758);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.panelControl1);
            this.Name = "frm_Data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show Data ReadCode";
            this.Load += new System.EventHandler(this.frm_Data_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl grdData;
        private DevExpress.XtraGrid.Views.Grid.GridView grvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID_Progarm;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colCode_TagJigNonePCS;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraGrid.Columns.GridColumn colCode_TagJigHavePCS;
        private DevExpress.XtraGrid.Columns.GridColumn colCodePCS;
        private DevExpress.XtraGrid.Columns.GridColumn colDate_Time;
        private DevExpress.XtraGrid.Columns.GridColumn colStatusUpload;
    }
}