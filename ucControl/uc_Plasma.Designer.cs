
namespace LineGolden_PLasma
{
    partial class uc_Plasma
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_Plasma));
            this.lb_Status_Barcode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_IndexPlasma = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Setting = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_infoJigOutput = new System.Windows.Forms.Label();
            this.lb_infoJigInput_Wait = new System.Windows.Forms.Label();
            this.lb_infoJigMachine = new System.Windows.Forms.Label();
            this.lb_infoJigInput = new System.Windows.Forms.Label();
            this.grdViewTag = new DevExpress.XtraGrid.GridControl();
            this.grvViewTag = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colNO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID_Prg = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID_Rfid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDataBarcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodeTray = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTagJig = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_Status_PLC = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvViewTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_Status_Barcode
            // 
            this.lb_Status_Barcode.AutoSize = true;
            this.lb_Status_Barcode.BackColor = System.Drawing.Color.Red;
            this.lb_Status_Barcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Status_Barcode.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Status_Barcode.ForeColor = System.Drawing.Color.Black;
            this.lb_Status_Barcode.Location = new System.Drawing.Point(1031, 4);
            this.lb_Status_Barcode.Name = "lb_Status_Barcode";
            this.lb_Status_Barcode.Size = new System.Drawing.Size(99, 25);
            this.lb_Status_Barcode.TabIndex = 87;
            this.lb_Status_Barcode.Text = "Disconnect";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 23);
            this.label1.TabIndex = 86;
            this.label1.Text = "Plasma:";
            // 
            // lb_IndexPlasma
            // 
            this.lb_IndexPlasma.AutoSize = true;
            this.lb_IndexPlasma.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_IndexPlasma.Location = new System.Drawing.Point(77, 5);
            this.lb_IndexPlasma.Name = "lb_IndexPlasma";
            this.lb_IndexPlasma.Size = new System.Drawing.Size(30, 23);
            this.lb_IndexPlasma.TabIndex = 88;
            this.lb_IndexPlasma.Text = "00";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 100;
            this.label2.Text = "Uploaded list";
            // 
            // btn_Setting
            // 
            this.btn_Setting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Setting.AutoSize = true;
            this.btn_Setting.BackColor = System.Drawing.Color.Yellow;
            this.btn_Setting.Image = ((System.Drawing.Image)(resources.GetObject("btn_Setting.Image")));
            this.btn_Setting.Location = new System.Drawing.Point(1145, 1);
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(39, 31);
            this.btn_Setting.TabIndex = 97;
            this.btn_Setting.UseVisualStyleBackColor = false;
            this.btn_Setting.Click += new System.EventHandler(this.btn_Setting_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackgroundImage = global::LineGolden_PLasma.Properties.Resources.image_background_talelayout_220223_1034;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lb_infoJigOutput, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_infoJigInput_Wait, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_infoJigMachine, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_infoJigInput, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 47);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1176, 238);
            this.tableLayoutPanel1.TabIndex = 96;
            // 
            // lb_infoJigOutput
            // 
            this.lb_infoJigOutput.BackColor = System.Drawing.Color.DarkGreen;
            this.lb_infoJigOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_infoJigOutput.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_infoJigOutput.Location = new System.Drawing.Point(947, 30);
            this.lb_infoJigOutput.Margin = new System.Windows.Forms.Padding(65, 30, 60, 30);
            this.lb_infoJigOutput.Name = "lb_infoJigOutput";
            this.lb_infoJigOutput.Size = new System.Drawing.Size(169, 178);
            this.lb_infoJigOutput.TabIndex = 77;
            this.lb_infoJigOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_infoJigInput_Wait
            // 
            this.lb_infoJigInput_Wait.BackColor = System.Drawing.Color.DarkGreen;
            this.lb_infoJigInput_Wait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_infoJigInput_Wait.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_infoJigInput_Wait.Location = new System.Drawing.Point(359, 30);
            this.lb_infoJigInput_Wait.Margin = new System.Windows.Forms.Padding(65, 30, 60, 30);
            this.lb_infoJigInput_Wait.Name = "lb_infoJigInput_Wait";
            this.lb_infoJigInput_Wait.Size = new System.Drawing.Size(169, 178);
            this.lb_infoJigInput_Wait.TabIndex = 75;
            this.lb_infoJigInput_Wait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_infoJigMachine
            // 
            this.lb_infoJigMachine.BackColor = System.Drawing.Color.DarkGreen;
            this.lb_infoJigMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_infoJigMachine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_infoJigMachine.Location = new System.Drawing.Point(653, 30);
            this.lb_infoJigMachine.Margin = new System.Windows.Forms.Padding(65, 30, 60, 30);
            this.lb_infoJigMachine.Name = "lb_infoJigMachine";
            this.lb_infoJigMachine.Size = new System.Drawing.Size(169, 178);
            this.lb_infoJigMachine.TabIndex = 76;
            this.lb_infoJigMachine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_infoJigInput
            // 
            this.lb_infoJigInput.BackColor = System.Drawing.Color.DarkGreen;
            this.lb_infoJigInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_infoJigInput.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_infoJigInput.Location = new System.Drawing.Point(65, 30);
            this.lb_infoJigInput.Margin = new System.Windows.Forms.Padding(65, 30, 60, 30);
            this.lb_infoJigInput.Name = "lb_infoJigInput";
            this.lb_infoJigInput.Size = new System.Drawing.Size(169, 178);
            this.lb_infoJigInput.TabIndex = 75;
            this.lb_infoJigInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grdViewTag
            // 
            this.grdViewTag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdViewTag.Location = new System.Drawing.Point(8, 318);
            this.grdViewTag.MainView = this.grvViewTag;
            this.grdViewTag.Name = "grdViewTag";
            this.grdViewTag.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2});
            this.grdViewTag.Size = new System.Drawing.Size(1176, 367);
            this.grdViewTag.TabIndex = 102;
            this.grdViewTag.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvViewTag});
            // 
            // grvViewTag
            // 
            this.grvViewTag.Appearance.FocusedRow.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.grvViewTag.Appearance.FocusedRow.Options.UseBackColor = true;
            this.grvViewTag.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold);
            this.grvViewTag.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.grvViewTag.Appearance.HeaderPanel.Options.UseFont = true;
            this.grvViewTag.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.grvViewTag.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.grvViewTag.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.grvViewTag.Appearance.Row.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.grvViewTag.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.grvViewTag.Appearance.Row.Options.UseFont = true;
            this.grvViewTag.Appearance.Row.Options.UseForeColor = true;
            this.grvViewTag.Appearance.SelectedRow.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.grvViewTag.Appearance.SelectedRow.Options.UseBackColor = true;
            this.grvViewTag.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNO,
            this.colID,
            this.colID_Prg,
            this.colID_Rfid,
            this.colDataBarcode,
            this.colCodeTray,
            this.colStatus,
            this.colTagJig});
            this.grvViewTag.GridControl = this.grdViewTag;
            this.grvViewTag.Name = "grvViewTag";
            this.grvViewTag.OptionsBehavior.ReadOnly = true;
            this.grvViewTag.OptionsView.ShowAutoFilterRow = true;
            this.grvViewTag.OptionsView.ShowGroupPanel = false;
            this.grvViewTag.OptionsView.ShowIndicator = false;
            this.grvViewTag.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.grvViewTag_RowCellStyle);
            this.grvViewTag.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.grvViewTag_RowStyle);
            this.grvViewTag.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.grvViewTag_CustomUnboundColumnData);
            this.grvViewTag.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grvViewTag_MouseUp);
            // 
            // colNO
            // 
            this.colNO.AppearanceCell.Options.UseTextOptions = true;
            this.colNO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNO.Caption = "NO.";
            this.colNO.FieldName = "NO";
            this.colNO.ImageOptions.Alignment = System.Drawing.StringAlignment.Center;
            this.colNO.Name = "colNO";
            this.colNO.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.colNO.Visible = true;
            this.colNO.VisibleIndex = 0;
            this.colNO.Width = 86;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colID_Prg
            // 
            this.colID_Prg.Caption = "IDProgram";
            this.colID_Prg.FieldName = "ID_Program";
            this.colID_Prg.Name = "colID_Prg";
            // 
            // colID_Rfid
            // 
            this.colID_Rfid.Caption = "IDRfid";
            this.colID_Rfid.FieldName = "ID_Rfid";
            this.colID_Rfid.Name = "colID_Rfid";
            // 
            // colDataBarcode
            // 
            this.colDataBarcode.AppearanceCell.Options.UseTextOptions = true;
            this.colDataBarcode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDataBarcode.Caption = "Data Barcode";
            this.colDataBarcode.FieldName = "CodePCS";
            this.colDataBarcode.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("colDataBarcode.ImageOptions.Image")));
            this.colDataBarcode.Name = "colDataBarcode";
            this.colDataBarcode.Visible = true;
            this.colDataBarcode.VisibleIndex = 1;
            this.colDataBarcode.Width = 545;
            // 
            // colCodeTray
            // 
            this.colCodeTray.AppearanceCell.Options.UseTextOptions = true;
            this.colCodeTray.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCodeTray.Caption = "JigID2";
            this.colCodeTray.FieldName = "CodeTray";
            this.colCodeTray.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("colCodeTray.ImageOptions.Image")));
            this.colCodeTray.Name = "colCodeTray";
            this.colCodeTray.Visible = true;
            this.colCodeTray.VisibleIndex = 3;
            this.colCodeTray.Width = 197;
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "StateUploadServer";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 4;
            this.colStatus.Width = 112;
            // 
            // colTagJig
            // 
            this.colTagJig.Caption = "JigID1";
            this.colTagJig.FieldName = "TagJigPlasma";
            this.colTagJig.ImageOptions.Image = global::LineGolden_PLasma.Properties.Resources.tag_32x32;
            this.colTagJig.Name = "colTagJig";
            this.colTagJig.Visible = true;
            this.colTagJig.VisibleIndex = 2;
            this.colTagJig.Width = 234;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(139, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 23);
            this.label3.TabIndex = 104;
            this.label3.Text = "PLC:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(945, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 106;
            this.label4.Text = "Barcode:";
            // 
            // lb_Status_PLC
            // 
            this.lb_Status_PLC.AutoSize = true;
            this.lb_Status_PLC.BackColor = System.Drawing.Color.Red;
            this.lb_Status_PLC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Status_PLC.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Status_PLC.ForeColor = System.Drawing.Color.Black;
            this.lb_Status_PLC.Location = new System.Drawing.Point(190, 4);
            this.lb_Status_PLC.Name = "lb_Status_PLC";
            this.lb_Status_PLC.Size = new System.Drawing.Size(99, 25);
            this.lb_Status_PLC.TabIndex = 105;
            this.lb_Status_PLC.Text = "Disconnect";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 289);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 107;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click_1);
            // 
            // btnShowLog
            // 
            this.btnShowLog.Location = new System.Drawing.Point(1108, 288);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(75, 23);
            this.btnShowLog.TabIndex = 108;
            this.btnShowLog.Text = "button2";
            this.btnShowLog.UseVisualStyleBackColor = true;
            this.btnShowLog.Visible = false;
            // 
            // uc_Plasma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnShowLog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_Status_PLC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grdViewTag);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Setting);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lb_Status_Barcode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_IndexPlasma);
            this.Name = "uc_Plasma";
            this.Size = new System.Drawing.Size(1190, 688);
            this.Load += new System.EventHandler(this.uc_Rfid_Load);
            this.Resize += new System.EventHandler(this.uc_Plasma_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdViewTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvViewTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lb_infoJigInput;
        private System.Windows.Forms.Label lb_infoJigMachine;
        private System.Windows.Forms.Label lb_infoJigOutput;
        private System.Windows.Forms.Label lb_Status_Barcode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_IndexPlasma;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Setting;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraGrid.GridControl grdViewTag;
        private DevExpress.XtraGrid.Views.Grid.GridView grvViewTag;
        private DevExpress.XtraGrid.Columns.GridColumn colNO;
        private DevExpress.XtraGrid.Columns.GridColumn colDataBarcode;
        private DevExpress.XtraGrid.Columns.GridColumn colCodeTray;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colID_Prg;
        private DevExpress.XtraGrid.Columns.GridColumn colID_Rfid;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private System.Windows.Forms.Label lb_infoJigInput_Wait;
        private DevExpress.XtraGrid.Columns.GridColumn colTagJig;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lb_Status_PLC;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnShowLog;
    }
}
