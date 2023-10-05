
namespace ReadCode
{
    partial class frmCreateProgram_New
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateProgram_New));
            this.txtDes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNameModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_qtyCam = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cbo_ModeJig = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpdateModel = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddModel = new System.Windows.Forms.Button();
            this.btnDelModel = new System.Windows.Forms.Button();
            this.cbo_ModeReadCode = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTimeTranferJig = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.lbStrHeader = new System.Windows.Forms.Label();
            this.txtStrHeaderTag = new System.Windows.Forms.TextBox();
            this.txtTimeRepeatJig = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.grdData = new DevExpress.XtraGrid.GridControl();
            this.grvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID_Program = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProgramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumberCamera = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTransferJig = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReadCodePCS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStringHeaderTagJig = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTimeTranferJig = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTimeRepeatJig = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyCam)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeTranferJig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeRepeatJig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDes
            // 
            this.txtDes.BackColor = System.Drawing.SystemColors.Window;
            this.txtDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDes.Location = new System.Drawing.Point(133, 99);
            this.txtDes.Name = "txtDes";
            this.txtDes.ReadOnly = true;
            this.txtDes.Size = new System.Drawing.Size(217, 26);
            this.txtDes.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 24);
            this.label2.TabIndex = 16;
            this.label2.Text = "Description:";
            // 
            // txtNameModel
            // 
            this.txtNameModel.BackColor = System.Drawing.SystemColors.Window;
            this.txtNameModel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNameModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNameModel.Location = new System.Drawing.Point(133, 67);
            this.txtNameModel.Name = "txtNameModel";
            this.txtNameModel.ReadOnly = true;
            this.txtNameModel.Size = new System.Drawing.Size(217, 26);
            this.txtNameModel.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 17;
            this.label1.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(387, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 24);
            this.label3.TabIndex = 25;
            this.label3.Text = "Qty Camera Read:";
            // 
            // txt_qtyCam
            // 
            this.txt_qtyCam.Enabled = false;
            this.txt_qtyCam.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_qtyCam.Location = new System.Drawing.Point(593, 99);
            this.txt_qtyCam.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txt_qtyCam.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txt_qtyCam.Name = "txt_qtyCam";
            this.txt_qtyCam.Size = new System.Drawing.Size(133, 26);
            this.txt_qtyCam.TabIndex = 71;
            this.txt_qtyCam.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 24);
            this.label6.TabIndex = 73;
            this.label6.Text = "Model Jig Transfer:";
            // 
            // cbo_ModeJig
            // 
            this.cbo_ModeJig.DisplayMember = "1";
            this.cbo_ModeJig.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_ModeJig.FormattingEnabled = true;
            this.cbo_ModeJig.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.cbo_ModeJig.Location = new System.Drawing.Point(196, 136);
            this.cbo_ModeJig.Name = "cbo_ModeJig";
            this.cbo_ModeJig.Size = new System.Drawing.Size(156, 26);
            this.cbo_ModeJig.TabIndex = 74;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.btnUpdateModel);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnAddModel);
            this.panel1.Controls.Add(this.btnDelModel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(891, 53);
            this.panel1.TabIndex = 77;
            // 
            // btnUpdateModel
            // 
            this.btnUpdateModel.BackColor = System.Drawing.Color.White;
            this.btnUpdateModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdateModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateModel.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateModel.Image")));
            this.btnUpdateModel.Location = new System.Drawing.Point(233, 5);
            this.btnUpdateModel.Name = "btnUpdateModel";
            this.btnUpdateModel.Size = new System.Drawing.Size(100, 43);
            this.btnUpdateModel.TabIndex = 0;
            this.btnUpdateModel.Text = "  Edit";
            this.btnUpdateModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdateModel.UseVisualStyleBackColor = false;
            this.btnUpdateModel.Click += new System.EventHandler(this.btnUpdateModel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(579, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 43);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = " Cancel  ";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddModel
            // 
            this.btnAddModel.BackColor = System.Drawing.Color.White;
            this.btnAddModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddModel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAddModel.Image = ((System.Drawing.Image)(resources.GetObject("btnAddModel.Image")));
            this.btnAddModel.Location = new System.Drawing.Point(60, 5);
            this.btnAddModel.Name = "btnAddModel";
            this.btnAddModel.Size = new System.Drawing.Size(100, 43);
            this.btnAddModel.TabIndex = 1;
            this.btnAddModel.Text = "  New";
            this.btnAddModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddModel.UseVisualStyleBackColor = false;
            this.btnAddModel.Click += new System.EventHandler(this.btnAddModel_Click);
            // 
            // btnDelModel
            // 
            this.btnDelModel.BackColor = System.Drawing.Color.White;
            this.btnDelModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelModel.Image = ((System.Drawing.Image)(resources.GetObject("btnDelModel.Image")));
            this.btnDelModel.Location = new System.Drawing.Point(406, 5);
            this.btnDelModel.Name = "btnDelModel";
            this.btnDelModel.Size = new System.Drawing.Size(100, 43);
            this.btnDelModel.TabIndex = 2;
            this.btnDelModel.Text = "  Delete";
            this.btnDelModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelModel.UseVisualStyleBackColor = false;
            this.btnDelModel.Click += new System.EventHandler(this.btnDelModel_Click);
            // 
            // cbo_ModeReadCode
            // 
            this.cbo_ModeReadCode.DisplayMember = "1";
            this.cbo_ModeReadCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_ModeReadCode.FormattingEnabled = true;
            this.cbo_ModeReadCode.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.cbo_ModeReadCode.Location = new System.Drawing.Point(196, 175);
            this.cbo_ModeReadCode.Name = "cbo_ModeReadCode";
            this.cbo_ModeReadCode.Size = new System.Drawing.Size(156, 26);
            this.cbo_ModeReadCode.TabIndex = 79;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(18, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(148, 24);
            this.label9.TabIndex = 78;
            this.label9.Text = "Read CodePCS:";
            // 
            // txtTimeTranferJig
            // 
            this.txtTimeTranferJig.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimeTranferJig.Location = new System.Drawing.Point(593, 136);
            this.txtTimeTranferJig.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtTimeTranferJig.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTimeTranferJig.Name = "txtTimeTranferJig";
            this.txtTimeTranferJig.Size = new System.Drawing.Size(133, 26);
            this.txtTimeTranferJig.TabIndex = 86;
            this.txtTimeTranferJig.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(387, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 24);
            this.label5.TabIndex = 85;
            this.label5.Text = "Time TranferJig (s):";
            // 
            // lbStrHeader
            // 
            this.lbStrHeader.AutoSize = true;
            this.lbStrHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStrHeader.Location = new System.Drawing.Point(387, 68);
            this.lbStrHeader.Name = "lbStrHeader";
            this.lbStrHeader.Size = new System.Drawing.Size(198, 24);
            this.lbStrHeader.TabIndex = 87;
            this.lbStrHeader.Text = "String Header Tag Jig:";
            // 
            // txtStrHeaderTag
            // 
            this.txtStrHeaderTag.BackColor = System.Drawing.SystemColors.Window;
            this.txtStrHeaderTag.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStrHeaderTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStrHeaderTag.Location = new System.Drawing.Point(591, 67);
            this.txtStrHeaderTag.Name = "txtStrHeaderTag";
            this.txtStrHeaderTag.Size = new System.Drawing.Size(135, 26);
            this.txtStrHeaderTag.TabIndex = 88;
            // 
            // txtTimeRepeatJig
            // 
            this.txtTimeRepeatJig.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimeRepeatJig.Location = new System.Drawing.Point(593, 175);
            this.txtTimeRepeatJig.Maximum = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.txtTimeRepeatJig.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTimeRepeatJig.Name = "txtTimeRepeatJig";
            this.txtTimeRepeatJig.Size = new System.Drawing.Size(133, 26);
            this.txtTimeRepeatJig.TabIndex = 90;
            this.txtTimeRepeatJig.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(387, 176);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(200, 24);
            this.label11.TabIndex = 89;
            this.label11.Text = "Time Repeat Jig (min):";
            // 
            // grdData
            // 
            this.grdData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grdData.Location = new System.Drawing.Point(0, 222);
            this.grdData.MainView = this.grvData;
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(891, 297);
            this.grdData.TabIndex = 91;
            this.grdData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvData});
            // 
            // grvData
            // 
            this.grvData.ColumnPanelRowHeight = 45;
            this.grvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID_Program,
            this.colProgramName,
            this.colNumberCamera,
            this.colTransferJig,
            this.colReadCodePCS,
            this.colStringHeaderTagJig,
            this.colTimeTranferJig,
            this.colTimeRepeatJig,
            this.colDescription});
            this.grvData.GridControl = this.grdData;
            this.grvData.Name = "grvData";
            this.grvData.OptionsBehavior.ReadOnly = true;
            this.grvData.OptionsView.ShowGroupPanel = false;
            this.grvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.grvData_FocusedRowChanged);
            // 
            // colID_Program
            // 
            this.colID_Program.Caption = "ID_Program";
            this.colID_Program.FieldName = "ID_Program";
            this.colID_Program.Name = "colID_Program";
            // 
            // colProgramName
            // 
            this.colProgramName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colProgramName.AppearanceCell.Options.UseFont = true;
            this.colProgramName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colProgramName.AppearanceHeader.Options.UseFont = true;
            this.colProgramName.AppearanceHeader.Options.UseForeColor = true;
            this.colProgramName.AppearanceHeader.Options.UseTextOptions = true;
            this.colProgramName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProgramName.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProgramName.Caption = "Program Name";
            this.colProgramName.FieldName = "ProgramName";
            this.colProgramName.Name = "colProgramName";
            this.colProgramName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colProgramName.OptionsFilter.AllowAutoFilter = false;
            this.colProgramName.OptionsFilter.AllowFilter = false;
            this.colProgramName.Visible = true;
            this.colProgramName.VisibleIndex = 0;
            this.colProgramName.Width = 264;
            // 
            // colNumberCamera
            // 
            this.colNumberCamera.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colNumberCamera.AppearanceCell.Options.UseFont = true;
            this.colNumberCamera.AppearanceCell.Options.UseTextOptions = true;
            this.colNumberCamera.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colNumberCamera.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colNumberCamera.AppearanceHeader.Options.UseFont = true;
            this.colNumberCamera.AppearanceHeader.Options.UseForeColor = true;
            this.colNumberCamera.AppearanceHeader.Options.UseTextOptions = true;
            this.colNumberCamera.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNumberCamera.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colNumberCamera.Caption = "Qty Camera";
            this.colNumberCamera.FieldName = "NumberCamera";
            this.colNumberCamera.Name = "colNumberCamera";
            this.colNumberCamera.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colNumberCamera.OptionsFilter.AllowAutoFilter = false;
            this.colNumberCamera.OptionsFilter.AllowFilter = false;
            this.colNumberCamera.Visible = true;
            this.colNumberCamera.VisibleIndex = 1;
            this.colNumberCamera.Width = 100;
            // 
            // colTransferJig
            // 
            this.colTransferJig.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colTransferJig.AppearanceCell.Options.UseFont = true;
            this.colTransferJig.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colTransferJig.AppearanceHeader.Options.UseFont = true;
            this.colTransferJig.AppearanceHeader.Options.UseForeColor = true;
            this.colTransferJig.AppearanceHeader.Options.UseTextOptions = true;
            this.colTransferJig.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTransferJig.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colTransferJig.Caption = "Jig Transfer";
            this.colTransferJig.FieldName = "TransferJig";
            this.colTransferJig.Name = "colTransferJig";
            this.colTransferJig.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTransferJig.OptionsFilter.AllowAutoFilter = false;
            this.colTransferJig.OptionsFilter.AllowFilter = false;
            this.colTransferJig.Visible = true;
            this.colTransferJig.VisibleIndex = 2;
            this.colTransferJig.Width = 102;
            // 
            // colReadCodePCS
            // 
            this.colReadCodePCS.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colReadCodePCS.AppearanceCell.Options.UseFont = true;
            this.colReadCodePCS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colReadCodePCS.AppearanceHeader.Options.UseFont = true;
            this.colReadCodePCS.AppearanceHeader.Options.UseForeColor = true;
            this.colReadCodePCS.AppearanceHeader.Options.UseTextOptions = true;
            this.colReadCodePCS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colReadCodePCS.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colReadCodePCS.Caption = "Read CodePCS";
            this.colReadCodePCS.FieldName = "ReadCodePCS";
            this.colReadCodePCS.Name = "colReadCodePCS";
            this.colReadCodePCS.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colReadCodePCS.OptionsFilter.AllowAutoFilter = false;
            this.colReadCodePCS.OptionsFilter.AllowFilter = false;
            this.colReadCodePCS.Visible = true;
            this.colReadCodePCS.VisibleIndex = 3;
            this.colReadCodePCS.Width = 95;
            // 
            // colStringHeaderTagJig
            // 
            this.colStringHeaderTagJig.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colStringHeaderTagJig.AppearanceCell.Options.UseFont = true;
            this.colStringHeaderTagJig.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colStringHeaderTagJig.AppearanceHeader.Options.UseFont = true;
            this.colStringHeaderTagJig.AppearanceHeader.Options.UseForeColor = true;
            this.colStringHeaderTagJig.AppearanceHeader.Options.UseTextOptions = true;
            this.colStringHeaderTagJig.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStringHeaderTagJig.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colStringHeaderTagJig.Caption = "Str HeaderTag";
            this.colStringHeaderTagJig.FieldName = "StringHeaderTagJig";
            this.colStringHeaderTagJig.Name = "colStringHeaderTagJig";
            this.colStringHeaderTagJig.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colStringHeaderTagJig.OptionsFilter.AllowAutoFilter = false;
            this.colStringHeaderTagJig.OptionsFilter.AllowFilter = false;
            this.colStringHeaderTagJig.Visible = true;
            this.colStringHeaderTagJig.VisibleIndex = 4;
            this.colStringHeaderTagJig.Width = 94;
            // 
            // colTimeTranferJig
            // 
            this.colTimeTranferJig.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colTimeTranferJig.AppearanceCell.Options.UseFont = true;
            this.colTimeTranferJig.AppearanceCell.Options.UseTextOptions = true;
            this.colTimeTranferJig.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTimeTranferJig.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colTimeTranferJig.AppearanceHeader.Options.UseFont = true;
            this.colTimeTranferJig.AppearanceHeader.Options.UseForeColor = true;
            this.colTimeTranferJig.AppearanceHeader.Options.UseTextOptions = true;
            this.colTimeTranferJig.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTimeTranferJig.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colTimeTranferJig.Caption = "TimeTranfer Jig";
            this.colTimeTranferJig.FieldName = "TimeTranferJig";
            this.colTimeTranferJig.Name = "colTimeTranferJig";
            this.colTimeTranferJig.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTimeTranferJig.OptionsFilter.AllowAutoFilter = false;
            this.colTimeTranferJig.OptionsFilter.AllowFilter = false;
            this.colTimeTranferJig.Visible = true;
            this.colTimeTranferJig.VisibleIndex = 5;
            this.colTimeTranferJig.Width = 109;
            // 
            // colTimeRepeatJig
            // 
            this.colTimeRepeatJig.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colTimeRepeatJig.AppearanceCell.Options.UseFont = true;
            this.colTimeRepeatJig.AppearanceCell.Options.UseTextOptions = true;
            this.colTimeRepeatJig.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTimeRepeatJig.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colTimeRepeatJig.AppearanceHeader.Options.UseFont = true;
            this.colTimeRepeatJig.AppearanceHeader.Options.UseForeColor = true;
            this.colTimeRepeatJig.AppearanceHeader.Options.UseTextOptions = true;
            this.colTimeRepeatJig.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTimeRepeatJig.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colTimeRepeatJig.Caption = "TimeRepeat Jig";
            this.colTimeRepeatJig.FieldName = "TimeRepeatJig";
            this.colTimeRepeatJig.Name = "colTimeRepeatJig";
            this.colTimeRepeatJig.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTimeRepeatJig.OptionsFilter.AllowAutoFilter = false;
            this.colTimeRepeatJig.OptionsFilter.AllowFilter = false;
            this.colTimeRepeatJig.Visible = true;
            this.colTimeRepeatJig.VisibleIndex = 6;
            this.colTimeRepeatJig.Width = 102;
            // 
            // colDescription
            // 
            this.colDescription.Caption = "Description";
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            // 
            // frmCreateProgram_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(891, 519);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.txtTimeRepeatJig);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtStrHeaderTag);
            this.Controls.Add(this.lbStrHeader);
            this.Controls.Add(this.txtTimeTranferJig);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbo_ModeReadCode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbo_ModeJig);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_qtyCam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNameModel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCreateProgram_New";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Program";
            this.Load += new System.EventHandler(this.frmCreateProgram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyCam)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeTranferJig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeRepeatJig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnUpdateModel;
        private System.Windows.Forms.Button btnAddModel;
        private System.Windows.Forms.Button btnDelModel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtDes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNameModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown txt_qtyCam;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbo_ModeJig;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbo_ModeReadCode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown txtTimeTranferJig;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbStrHeader;
        private System.Windows.Forms.TextBox txtStrHeaderTag;
        private System.Windows.Forms.NumericUpDown txtTimeRepeatJig;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraGrid.GridControl grdData;
        private DevExpress.XtraGrid.Views.Grid.GridView grvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID_Program;
        private DevExpress.XtraGrid.Columns.GridColumn colProgramName;
        private DevExpress.XtraGrid.Columns.GridColumn colNumberCamera;
        private DevExpress.XtraGrid.Columns.GridColumn colTransferJig;
        private DevExpress.XtraGrid.Columns.GridColumn colReadCodePCS;
        private DevExpress.XtraGrid.Columns.GridColumn colStringHeaderTagJig;
        private DevExpress.XtraGrid.Columns.GridColumn colTimeTranferJig;
        private DevExpress.XtraGrid.Columns.GridColumn colTimeRepeatJig;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
    }
}