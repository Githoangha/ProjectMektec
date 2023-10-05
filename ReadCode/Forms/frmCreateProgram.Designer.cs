
namespace ReadCode
{
    partial class frmCreateProgram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateProgram));
            this.txtDes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNameModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_qtyCam = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpdateModel = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddModel = new System.Windows.Forms.Button();
            this.btnDelModel = new System.Windows.Forms.Button();
            this.cbo_ModeReadCode = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grdData = new DevExpress.XtraGrid.GridControl();
            this.grvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColProgramID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColProgramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColQtyCam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColReadCodePCS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColDes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHaveBarcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_QtyPcs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.cbJigHaveBarcode = new System.Windows.Forms.ComboBox();
            this.cbTypeModel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkUsingFVI = new System.Windows.Forms.CheckBox();
            this.numQtyPcs = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chkTakeJigHavePcs = new System.Windows.Forms.CheckBox();
            this.col_TakeJigHavePcs = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyCam)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyPcs)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDes
            // 
            this.txtDes.BackColor = System.Drawing.SystemColors.Window;
            this.txtDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDes.Location = new System.Drawing.Point(129, 118);
            this.txtDes.Name = "txtDes";
            this.txtDes.ReadOnly = true;
            this.txtDes.Size = new System.Drawing.Size(326, 26);
            this.txtDes.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 119);
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
            this.txtNameModel.Location = new System.Drawing.Point(129, 75);
            this.txtNameModel.Name = "txtNameModel";
            this.txtNameModel.ReadOnly = true;
            this.txtNameModel.Size = new System.Drawing.Size(326, 26);
            this.txtNameModel.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 17;
            this.label1.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(473, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 24);
            this.label3.TabIndex = 25;
            this.label3.Text = "Qty Camera Read:";
            // 
            // txt_qtyCam
            // 
            this.txt_qtyCam.Enabled = false;
            this.txt_qtyCam.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_qtyCam.Location = new System.Drawing.Point(651, 74);
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
            this.txt_qtyCam.Size = new System.Drawing.Size(170, 29);
            this.txt_qtyCam.TabIndex = 71;
            this.txt_qtyCam.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
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
            this.panel1.Size = new System.Drawing.Size(848, 53);
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
            this.cbo_ModeReadCode.Location = new System.Drawing.Point(651, 118);
            this.cbo_ModeReadCode.Name = "cbo_ModeReadCode";
            this.cbo_ModeReadCode.Size = new System.Drawing.Size(170, 26);
            this.cbo_ModeReadCode.TabIndex = 79;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(473, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(148, 24);
            this.label9.TabIndex = 78;
            this.label9.Text = "Read CodePCS:";
            // 
            // grdData
            // 
            this.grdData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grdData.Location = new System.Drawing.Point(0, 241);
            this.grdData.MainView = this.grvData;
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(848, 282);
            this.grdData.TabIndex = 80;
            this.grdData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvData});
            // 
            // grvData
            // 
            this.grvData.ColumnPanelRowHeight = 40;
            this.grvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColProgramID,
            this.ColProgramName,
            this.ColQtyCam,
            this.ColReadCodePCS,
            this.ColDes,
            this.colHaveBarcode,
            this.col_QtyPcs,
            this.col_TakeJigHavePcs});
            this.grvData.GridControl = this.grdData;
            this.grvData.Name = "grvData";
            this.grvData.OptionsBehavior.ReadOnly = true;
            this.grvData.OptionsView.ShowGroupPanel = false;
            this.grvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.grvData_FocusedRowChanged);
            // 
            // ColProgramID
            // 
            this.ColProgramID.Caption = "ProgramID";
            this.ColProgramID.FieldName = "ID_Program";
            this.ColProgramID.Name = "ColProgramID";
            // 
            // ColProgramName
            // 
            this.ColProgramName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColProgramName.AppearanceCell.Options.UseFont = true;
            this.ColProgramName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColProgramName.AppearanceHeader.Options.UseFont = true;
            this.ColProgramName.AppearanceHeader.Options.UseForeColor = true;
            this.ColProgramName.AppearanceHeader.Options.UseTextOptions = true;
            this.ColProgramName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColProgramName.Caption = "ProgramName";
            this.ColProgramName.FieldName = "ProgramName";
            this.ColProgramName.Name = "ColProgramName";
            this.ColProgramName.Visible = true;
            this.ColProgramName.VisibleIndex = 0;
            this.ColProgramName.Width = 254;
            // 
            // ColQtyCam
            // 
            this.ColQtyCam.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColQtyCam.AppearanceCell.Options.UseFont = true;
            this.ColQtyCam.AppearanceCell.Options.UseTextOptions = true;
            this.ColQtyCam.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColQtyCam.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColQtyCam.AppearanceHeader.Options.UseFont = true;
            this.ColQtyCam.AppearanceHeader.Options.UseForeColor = true;
            this.ColQtyCam.AppearanceHeader.Options.UseTextOptions = true;
            this.ColQtyCam.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColQtyCam.Caption = "Qty Cam";
            this.ColQtyCam.FieldName = "NumberCamera";
            this.ColQtyCam.Name = "ColQtyCam";
            this.ColQtyCam.Visible = true;
            this.ColQtyCam.VisibleIndex = 1;
            this.ColQtyCam.Width = 125;
            // 
            // ColReadCodePCS
            // 
            this.ColReadCodePCS.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColReadCodePCS.AppearanceCell.Options.UseFont = true;
            this.ColReadCodePCS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColReadCodePCS.AppearanceHeader.Options.UseFont = true;
            this.ColReadCodePCS.AppearanceHeader.Options.UseForeColor = true;
            this.ColReadCodePCS.AppearanceHeader.Options.UseTextOptions = true;
            this.ColReadCodePCS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColReadCodePCS.Caption = "ReadCode PCS";
            this.ColReadCodePCS.FieldName = "ReadCodePCS";
            this.ColReadCodePCS.Name = "ColReadCodePCS";
            this.ColReadCodePCS.Visible = true;
            this.ColReadCodePCS.VisibleIndex = 2;
            this.ColReadCodePCS.Width = 126;
            // 
            // ColDes
            // 
            this.ColDes.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColDes.AppearanceCell.Options.UseFont = true;
            this.ColDes.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColDes.AppearanceHeader.Options.UseFont = true;
            this.ColDes.AppearanceHeader.Options.UseForeColor = true;
            this.ColDes.AppearanceHeader.Options.UseTextOptions = true;
            this.ColDes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColDes.Caption = "Description";
            this.ColDes.FieldName = "Description";
            this.ColDes.Name = "ColDes";
            this.ColDes.Visible = true;
            this.ColDes.VisibleIndex = 3;
            this.ColDes.Width = 183;
            // 
            // colHaveBarcode
            // 
            this.colHaveBarcode.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.colHaveBarcode.AppearanceCell.Options.UseFont = true;
            this.colHaveBarcode.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.colHaveBarcode.AppearanceHeader.Options.UseFont = true;
            this.colHaveBarcode.AppearanceHeader.Options.UseForeColor = true;
            this.colHaveBarcode.AppearanceHeader.Options.UseTextOptions = true;
            this.colHaveBarcode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colHaveBarcode.Caption = "Jig PCS HaveBarcode";
            this.colHaveBarcode.FieldName = "JigPCSHaveBarcode";
            this.colHaveBarcode.Name = "colHaveBarcode";
            this.colHaveBarcode.Width = 177;
            // 
            // col_QtyPcs
            // 
            this.col_QtyPcs.Caption = "Qty Pcs";
            this.col_QtyPcs.FieldName = "QtyPcs";
            this.col_QtyPcs.Name = "col_QtyPcs";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 24);
            this.label4.TabIndex = 78;
            this.label4.Text = "Jig PCS Have BarCode:";
            // 
            // cbJigHaveBarcode
            // 
            this.cbJigHaveBarcode.DisplayMember = "1";
            this.cbJigHaveBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbJigHaveBarcode.FormattingEnabled = true;
            this.cbJigHaveBarcode.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.cbJigHaveBarcode.Location = new System.Drawing.Point(222, 161);
            this.cbJigHaveBarcode.Name = "cbJigHaveBarcode";
            this.cbJigHaveBarcode.Size = new System.Drawing.Size(233, 26);
            this.cbJigHaveBarcode.TabIndex = 79;
            // 
            // cbTypeModel
            // 
            this.cbTypeModel.DisplayMember = "1";
            this.cbTypeModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTypeModel.FormattingEnabled = true;
            this.cbTypeModel.Items.AddRange(new object[] {
            "NULL",
            "Truong Hop 1",
            "Truong Hop 2",
            "Truong Hop 3"});
            this.cbTypeModel.Location = new System.Drawing.Point(651, 161);
            this.cbTypeModel.Name = "cbTypeModel";
            this.cbTypeModel.Size = new System.Drawing.Size(170, 26);
            this.cbTypeModel.TabIndex = 82;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(473, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 24);
            this.label5.TabIndex = 81;
            this.label5.Text = "Type Model:";
            // 
            // chkUsingFVI
            // 
            this.chkUsingFVI.AutoSize = true;
            this.chkUsingFVI.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.chkUsingFVI.Location = new System.Drawing.Point(478, 204);
            this.chkUsingFVI.Name = "chkUsingFVI";
            this.chkUsingFVI.Size = new System.Drawing.Size(111, 28);
            this.chkUsingFVI.TabIndex = 83;
            this.chkUsingFVI.Text = "Using FVI";
            this.chkUsingFVI.UseVisualStyleBackColor = true;
            // 
            // numQtyPcs
            // 
            this.numQtyPcs.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQtyPcs.Location = new System.Drawing.Point(732, 204);
            this.numQtyPcs.Name = "numQtyPcs";
            this.numQtyPcs.Size = new System.Drawing.Size(52, 29);
            this.numQtyPcs.TabIndex = 87;
            this.numQtyPcs.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(647, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 24);
            this.label6.TabIndex = 86;
            this.label6.Text = "Qty Pcs:";
            // 
            // chkTakeJigHavePcs
            // 
            this.chkTakeJigHavePcs.AutoSize = true;
            this.chkTakeJigHavePcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.chkTakeJigHavePcs.Location = new System.Drawing.Point(270, 204);
            this.chkTakeJigHavePcs.Name = "chkTakeJigHavePcs";
            this.chkTakeJigHavePcs.Size = new System.Drawing.Size(185, 28);
            this.chkTakeJigHavePcs.TabIndex = 88;
            this.chkTakeJigHavePcs.Text = "Take Jig Have Pcs";
            this.chkTakeJigHavePcs.UseVisualStyleBackColor = true;
            // 
            // col_TakeJigHavePcs
            // 
            this.col_TakeJigHavePcs.Caption = "Take Jig Have Pcs";
            this.col_TakeJigHavePcs.FieldName = "TakeJigHavePcs";
            this.col_TakeJigHavePcs.Name = "col_TakeJigHavePcs";
            // 
            // frmCreateProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(848, 523);
            this.Controls.Add(this.chkTakeJigHavePcs);
            this.Controls.Add(this.numQtyPcs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkUsingFVI);
            this.Controls.Add(this.cbTypeModel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.cbJigHaveBarcode);
            this.Controls.Add(this.cbo_ModeReadCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txt_qtyCam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNameModel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCreateProgram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Program";
            this.Load += new System.EventHandler(this.frmCreateProgram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyCam)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyPcs)).EndInit();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbo_ModeReadCode;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraGrid.GridControl grdData;
        private DevExpress.XtraGrid.Views.Grid.GridView grvData;
        private DevExpress.XtraGrid.Columns.GridColumn ColProgramID;
        private DevExpress.XtraGrid.Columns.GridColumn ColProgramName;
        private DevExpress.XtraGrid.Columns.GridColumn ColQtyCam;
        private DevExpress.XtraGrid.Columns.GridColumn ColReadCodePCS;
        private DevExpress.XtraGrid.Columns.GridColumn ColDes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbJigHaveBarcode;
        private DevExpress.XtraGrid.Columns.GridColumn colHaveBarcode;
        private System.Windows.Forms.ComboBox cbTypeModel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkUsingFVI;
        private System.Windows.Forms.NumericUpDown numQtyPcs;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraGrid.Columns.GridColumn col_QtyPcs;
        private DevExpress.XtraGrid.Columns.GridColumn col_TakeJigHavePcs;
        private System.Windows.Forms.CheckBox chkTakeJigHavePcs;
    }
}