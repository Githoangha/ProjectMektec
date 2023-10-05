
namespace LineGolden_PLasma
{
    partial class FrmDataDetail
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUploadData = new System.Windows.Forms.Button();
            this.btnShowData = new System.Windows.Forms.Button();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdData = new DevExpress.XtraGrid.GridControl();
            this.grvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_IDProg = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_PlasmaIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_JigTranfer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_JigPlasma = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_CodePCS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_CodeTray = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_TimeInPlasma = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_TimeOutPLasma = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_DateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_CycleTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_StatusUpload = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_LotID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_MPN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_CompletePlasma = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_CompleteBoxing = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.chkSelectAll);
            this.panel1.Controls.Add(this.btnUploadData);
            this.panel1.Controls.Add(this.btnShowData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1206, 75);
            this.panel1.TabIndex = 79;
            // 
            // btnUploadData
            // 
            this.btnUploadData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUploadData.BackColor = System.Drawing.Color.White;
            this.btnUploadData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUploadData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadData.Image = global::LineGolden_PLasma.Properties.Resources.icons8_upload_to_the_cloud_24px;
            this.btnUploadData.Location = new System.Drawing.Point(1083, 5);
            this.btnUploadData.Name = "btnUploadData";
            this.btnUploadData.Size = new System.Drawing.Size(111, 67);
            this.btnUploadData.TabIndex = 0;
            this.btnUploadData.Text = "Upload Data";
            this.btnUploadData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUploadData.UseVisualStyleBackColor = false;
            this.btnUploadData.Click += new System.EventHandler(this.btnUploadData_Click);
            // 
            // btnShowData
            // 
            this.btnShowData.BackColor = System.Drawing.Color.White;
            this.btnShowData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnShowData.Image = global::LineGolden_PLasma.Properties.Resources.icons8_database_view_24px;
            this.btnShowData.Location = new System.Drawing.Point(12, 5);
            this.btnShowData.Name = "btnShowData";
            this.btnShowData.Size = new System.Drawing.Size(111, 67);
            this.btnShowData.TabIndex = 1;
            this.btnShowData.Text = "Show Status";
            this.btnShowData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnShowData.UseVisualStyleBackColor = false;
            this.btnShowData.Click += new System.EventHandler(this.btnShowData_Click);
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // grdData
            // 
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(0, 75);
            this.grdData.MainView = this.grvData;
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(1206, 566);
            this.grdData.TabIndex = 81;
            this.grdData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvData});
            // 
            // grvData
            // 
            this.grvData.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.grvData.Appearance.HeaderPanel.Options.UseFont = true;
            this.grvData.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.grvData.Appearance.Row.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.grvData.Appearance.Row.Options.UseFont = true;
            this.grvData.Appearance.Row.Options.UseForeColor = true;
            this.grvData.Appearance.Row.Options.UseTextOptions = true;
            this.grvData.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.grvData.ColumnPanelRowHeight = 100;
            this.grvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_NO,
            this.col_ID,
            this.col_IDProg,
            this.col_PlasmaIndex,
            this.col_JigTranfer,
            this.col_JigPlasma,
            this.col_CodePCS,
            this.col_CodeTray,
            this.col_TimeInPlasma,
            this.col_TimeOutPLasma,
            this.col_DateTime,
            this.col_CycleTime,
            this.col_StatusUpload,
            this.col_LotID,
            this.col_MPN,
            this.col_Status,
            this.col_CompletePlasma,
            this.col_CompleteBoxing});
            this.grvData.GridControl = this.grdData;
            this.grvData.Name = "grvData";
            this.grvData.OptionsBehavior.Editable = false;
            this.grvData.OptionsView.ShowAutoFilterRow = true;
            this.grvData.OptionsView.ShowGroupPanel = false;
            this.grvData.OptionsView.ShowIndicator = false;
            this.grvData.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.grvData_CustomUnboundColumnData);
            // 
            // col_NO
            // 
            this.col_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.col_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_NO.Caption = "NO.";
            this.col_NO.FieldName = "col_NO";
            this.col_NO.Name = "col_NO";
            this.col_NO.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.col_NO.Visible = true;
            this.col_NO.VisibleIndex = 0;
            this.col_NO.Width = 65;
            // 
            // col_ID
            // 
            this.col_ID.Caption = "ID";
            this.col_ID.FieldName = "ID";
            this.col_ID.Name = "col_ID";
            // 
            // col_IDProg
            // 
            this.col_IDProg.Caption = "ID Program";
            this.col_IDProg.FieldName = "ID_Program";
            this.col_IDProg.Name = "col_IDProg";
            // 
            // col_PlasmaIndex
            // 
            this.col_PlasmaIndex.AppearanceCell.Options.UseImage = true;
            this.col_PlasmaIndex.AppearanceHeader.Options.UseImage = true;
            this.col_PlasmaIndex.AppearanceHeader.Options.UseTextOptions = true;
            this.col_PlasmaIndex.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_PlasmaIndex.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_PlasmaIndex.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_PlasmaIndex.Caption = "PLasma Index";
            this.col_PlasmaIndex.FieldName = "ID_Plasma";
            this.col_PlasmaIndex.Name = "col_PlasmaIndex";
            this.col_PlasmaIndex.Width = 81;
            // 
            // col_JigTranfer
            // 
            this.col_JigTranfer.AppearanceHeader.Options.UseImage = true;
            this.col_JigTranfer.AppearanceHeader.Options.UseTextOptions = true;
            this.col_JigTranfer.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_JigTranfer.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_JigTranfer.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_JigTranfer.Caption = "Jig Transfer";
            this.col_JigTranfer.FieldName = "TagJigTransfer";
            this.col_JigTranfer.Name = "col_JigTranfer";
            this.col_JigTranfer.Width = 148;
            // 
            // col_JigPlasma
            // 
            this.col_JigPlasma.AppearanceHeader.Options.UseImage = true;
            this.col_JigPlasma.AppearanceHeader.Options.UseTextOptions = true;
            this.col_JigPlasma.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_JigPlasma.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_JigPlasma.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_JigPlasma.Caption = "Jig Plasma";
            this.col_JigPlasma.FieldName = "TagJigPlasma";
            this.col_JigPlasma.Name = "col_JigPlasma";
            this.col_JigPlasma.Visible = true;
            this.col_JigPlasma.VisibleIndex = 1;
            this.col_JigPlasma.Width = 115;
            // 
            // col_CodePCS
            // 
            this.col_CodePCS.AppearanceHeader.Options.UseImage = true;
            this.col_CodePCS.AppearanceHeader.Options.UseTextOptions = true;
            this.col_CodePCS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_CodePCS.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_CodePCS.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_CodePCS.Caption = "Code PCS";
            this.col_CodePCS.FieldName = "CodePCS";
            this.col_CodePCS.Name = "col_CodePCS";
            this.col_CodePCS.Visible = true;
            this.col_CodePCS.VisibleIndex = 2;
            this.col_CodePCS.Width = 195;
            // 
            // col_CodeTray
            // 
            this.col_CodeTray.AppearanceHeader.Options.UseTextOptions = true;
            this.col_CodeTray.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_CodeTray.Caption = "Code Tray";
            this.col_CodeTray.FieldName = "CodeTray";
            this.col_CodeTray.Name = "col_CodeTray";
            this.col_CodeTray.Visible = true;
            this.col_CodeTray.VisibleIndex = 3;
            this.col_CodeTray.Width = 170;
            // 
            // col_TimeInPlasma
            // 
            this.col_TimeInPlasma.AppearanceHeader.Options.UseImage = true;
            this.col_TimeInPlasma.AppearanceHeader.Options.UseTextOptions = true;
            this.col_TimeInPlasma.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_TimeInPlasma.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_TimeInPlasma.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_TimeInPlasma.Caption = "Time In";
            this.col_TimeInPlasma.FieldName = "DateTimeInPlasma";
            this.col_TimeInPlasma.Name = "col_TimeInPlasma";
            this.col_TimeInPlasma.Visible = true;
            this.col_TimeInPlasma.VisibleIndex = 4;
            this.col_TimeInPlasma.Width = 140;
            // 
            // col_TimeOutPLasma
            // 
            this.col_TimeOutPLasma.AppearanceHeader.Options.UseImage = true;
            this.col_TimeOutPLasma.AppearanceHeader.Options.UseTextOptions = true;
            this.col_TimeOutPLasma.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_TimeOutPLasma.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_TimeOutPLasma.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_TimeOutPLasma.Caption = "Time Out";
            this.col_TimeOutPLasma.FieldName = "DateTimeOutPlasma";
            this.col_TimeOutPLasma.Name = "col_TimeOutPLasma";
            this.col_TimeOutPLasma.Visible = true;
            this.col_TimeOutPLasma.VisibleIndex = 5;
            this.col_TimeOutPLasma.Width = 124;
            // 
            // col_DateTime
            // 
            this.col_DateTime.Caption = "Date Time";
            this.col_DateTime.FieldName = "Date_Time";
            this.col_DateTime.Name = "col_DateTime";
            // 
            // col_CycleTime
            // 
            this.col_CycleTime.AppearanceHeader.Options.UseImage = true;
            this.col_CycleTime.AppearanceHeader.Options.UseTextOptions = true;
            this.col_CycleTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_CycleTime.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_CycleTime.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_CycleTime.Caption = "Cycle Time";
            this.col_CycleTime.FieldName = "Cycletime";
            this.col_CycleTime.Name = "col_CycleTime";
            this.col_CycleTime.Visible = true;
            this.col_CycleTime.VisibleIndex = 6;
            this.col_CycleTime.Width = 79;
            // 
            // col_StatusUpload
            // 
            this.col_StatusUpload.AppearanceHeader.Options.UseImage = true;
            this.col_StatusUpload.AppearanceHeader.Options.UseTextOptions = true;
            this.col_StatusUpload.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_StatusUpload.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.col_StatusUpload.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_StatusUpload.Caption = "Status Upload Data";
            this.col_StatusUpload.FieldName = "StateUploadServer";
            this.col_StatusUpload.Name = "col_StatusUpload";
            this.col_StatusUpload.Width = 87;
            // 
            // col_LotID
            // 
            this.col_LotID.AppearanceHeader.Options.UseTextOptions = true;
            this.col_LotID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_LotID.Caption = "Lot ID";
            this.col_LotID.FieldName = "LotID";
            this.col_LotID.Name = "col_LotID";
            this.col_LotID.Visible = true;
            this.col_LotID.VisibleIndex = 7;
            this.col_LotID.Width = 131;
            // 
            // col_MPN
            // 
            this.col_MPN.AppearanceHeader.Options.UseTextOptions = true;
            this.col_MPN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_MPN.Caption = "MPN";
            this.col_MPN.FieldName = "MPN";
            this.col_MPN.Name = "col_MPN";
            this.col_MPN.Visible = true;
            this.col_MPN.VisibleIndex = 8;
            this.col_MPN.Width = 102;
            // 
            // col_Status
            // 
            this.col_Status.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Status.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Status.Caption = "Status";
            this.col_Status.FieldName = "Status";
            this.col_Status.Name = "col_Status";
            this.col_Status.Visible = true;
            this.col_Status.VisibleIndex = 9;
            this.col_Status.Width = 83;
            // 
            // col_CompletePlasma
            // 
            this.col_CompletePlasma.Caption = "Complete Plasma";
            this.col_CompletePlasma.FieldName = "CompletePlasma";
            this.col_CompletePlasma.Name = "col_CompletePlasma";
            // 
            // col_CompleteBoxing
            // 
            this.col_CompleteBoxing.Caption = "Complete Boxing";
            this.col_CompleteBoxing.FieldName = "CompleteBoxing";
            this.col_CompleteBoxing.Name = "col_CompleteBoxing";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(966, 12);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(111, 29);
            this.chkSelectAll.TabIndex = 2;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // FrmDataDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 641);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.panel1);
            this.Name = "FrmDataDetail";
            this.Text = "FrmDataDetail";
            this.Load += new System.EventHandler(this.FrmDataDetail_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUploadData;
        private System.Windows.Forms.Button btnShowData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl grdData;
        private DevExpress.XtraGrid.Views.Grid.GridView grvData;
        private DevExpress.XtraGrid.Columns.GridColumn col_NO;
        private DevExpress.XtraGrid.Columns.GridColumn col_ID;
        private DevExpress.XtraGrid.Columns.GridColumn col_IDProg;
        private DevExpress.XtraGrid.Columns.GridColumn col_PlasmaIndex;
        private DevExpress.XtraGrid.Columns.GridColumn col_JigTranfer;
        private DevExpress.XtraGrid.Columns.GridColumn col_JigPlasma;
        private DevExpress.XtraGrid.Columns.GridColumn col_CodePCS;
        private DevExpress.XtraGrid.Columns.GridColumn col_CodeTray;
        private DevExpress.XtraGrid.Columns.GridColumn col_TimeInPlasma;
        private DevExpress.XtraGrid.Columns.GridColumn col_TimeOutPLasma;
        private DevExpress.XtraGrid.Columns.GridColumn col_DateTime;
        private DevExpress.XtraGrid.Columns.GridColumn col_CycleTime;
        private DevExpress.XtraGrid.Columns.GridColumn col_StatusUpload;
        private DevExpress.XtraGrid.Columns.GridColumn col_LotID;
        private DevExpress.XtraGrid.Columns.GridColumn col_MPN;
        private DevExpress.XtraGrid.Columns.GridColumn col_CompletePlasma;
        private DevExpress.XtraGrid.Columns.GridColumn col_CompleteBoxing;
        private DevExpress.XtraGrid.Columns.GridColumn col_Status;
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}