
namespace LineGolden_PLasma
{
    partial class FrmCreateProgram
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtDes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grvModel = new System.Windows.Forms.DataGridView();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUseMachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColQtyCam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNumJigPlasma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReadCodePCS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeRepeatJig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_GetJigHavePcs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_UseFvi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtNameModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_qtyCam = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_qtyJigBase = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpdateModel = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddModel = new System.Windows.Forms.Button();
            this.btnDelModel = new System.Windows.Forms.Button();
            this.cbo_ModeReadCode = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTimeRepeatJig = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbUseMachine = new System.Windows.Forms.ComboBox();
            this.chkGetJigHavePcs = new System.Windows.Forms.CheckBox();
            this.chkUseFvi = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPathDataBoxing = new System.Windows.Forms.TextBox();
            this.btnFilePathBoxing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grvModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyCam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyJigBase)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeRepeatJig)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDes
            // 
            this.txtDes.BackColor = System.Drawing.SystemColors.Window;
            this.txtDes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDes.Location = new System.Drawing.Point(140, 105);
            this.txtDes.Name = "txtDes";
            this.txtDes.ReadOnly = true;
            this.txtDes.Size = new System.Drawing.Size(182, 29);
            this.txtDes.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 25);
            this.label2.TabIndex = 16;
            this.label2.Text = "Description:";
            // 
            // grvModel
            // 
            this.grvModel.AllowUserToAddRows = false;
            this.grvModel.AllowUserToDeleteRows = false;
            this.grvModel.BackgroundColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvModel.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.grvModel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvModel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.colUseMachine,
            this.ColName,
            this.ColDes,
            this.ColQtyCam,
            this.ColNumJigPlasma,
            this.ReadCodePCS,
            this.TimeRepeatJig,
            this.col_GetJigHavePcs,
            this.col_UseFvi});
            this.grvModel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grvModel.GridColor = System.Drawing.SystemColors.Info;
            this.grvModel.Location = new System.Drawing.Point(0, 269);
            this.grvModel.Margin = new System.Windows.Forms.Padding(4);
            this.grvModel.Name = "grvModel";
            this.grvModel.ReadOnly = true;
            this.grvModel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvModel.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grvModel.RowHeadersVisible = false;
            this.grvModel.RowHeadersWidth = 51;
            this.grvModel.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.grvModel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grvModel.Size = new System.Drawing.Size(754, 222);
            this.grvModel.TabIndex = 20;
            this.grvModel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvModel_CellClick);
            // 
            // ColID
            // 
            this.ColID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColID.DataPropertyName = "ID_Program";
            this.ColID.HeaderText = "Prg ID";
            this.ColID.MinimumWidth = 6;
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            this.ColID.Visible = false;
            this.ColID.Width = 40;
            // 
            // colUseMachine
            // 
            this.colUseMachine.DataPropertyName = "UseMachine";
            this.colUseMachine.HeaderText = "UserMachine";
            this.colUseMachine.Name = "colUseMachine";
            this.colUseMachine.ReadOnly = true;
            this.colUseMachine.Visible = false;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.DataPropertyName = "ProgramName";
            this.ColName.FillWeight = 52.45369F;
            this.ColName.HeaderText = "Program Name";
            this.ColName.MinimumWidth = 6;
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // ColDes
            // 
            this.ColDes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColDes.DataPropertyName = "Description";
            this.ColDes.FillWeight = 52.45369F;
            this.ColDes.HeaderText = "Description";
            this.ColDes.MinimumWidth = 6;
            this.ColDes.Name = "ColDes";
            this.ColDes.ReadOnly = true;
            // 
            // ColQtyCam
            // 
            this.ColQtyCam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColQtyCam.DataPropertyName = "NumberCamera";
            this.ColQtyCam.HeaderText = "Qty Camera";
            this.ColQtyCam.Name = "ColQtyCam";
            this.ColQtyCam.ReadOnly = true;
            this.ColQtyCam.Visible = false;
            this.ColQtyCam.Width = 80;
            // 
            // ColNumJigPlasma
            // 
            this.ColNumJigPlasma.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColNumJigPlasma.DataPropertyName = "NumJigPlasmaBase";
            this.ColNumJigPlasma.HeaderText = "Qty Jig P/B";
            this.ColNumJigPlasma.Name = "ColNumJigPlasma";
            this.ColNumJigPlasma.ReadOnly = true;
            this.ColNumJigPlasma.Width = 150;
            // 
            // ReadCodePCS
            // 
            this.ReadCodePCS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ReadCodePCS.DataPropertyName = "ReadCodePCS";
            this.ReadCodePCS.HeaderText = "Read CodePCS";
            this.ReadCodePCS.Name = "ReadCodePCS";
            this.ReadCodePCS.ReadOnly = true;
            this.ReadCodePCS.Visible = false;
            this.ReadCodePCS.Width = 80;
            // 
            // TimeRepeatJig
            // 
            this.TimeRepeatJig.DataPropertyName = "TimeRepeatJig";
            this.TimeRepeatJig.HeaderText = "TimeRepeat Jig";
            this.TimeRepeatJig.Name = "TimeRepeatJig";
            this.TimeRepeatJig.ReadOnly = true;
            this.TimeRepeatJig.Width = 150;
            // 
            // col_GetJigHavePcs
            // 
            this.col_GetJigHavePcs.DataPropertyName = "GetJigHavePcs";
            this.col_GetJigHavePcs.HeaderText = "Get Jig Have Pcs";
            this.col_GetJigHavePcs.Name = "col_GetJigHavePcs";
            this.col_GetJigHavePcs.ReadOnly = true;
            this.col_GetJigHavePcs.Visible = false;
            // 
            // col_UseFvi
            // 
            this.col_UseFvi.DataPropertyName = "UseFvi";
            this.col_UseFvi.HeaderText = "Use FVI";
            this.col_UseFvi.Name = "col_UseFvi";
            this.col_UseFvi.ReadOnly = true;
            this.col_UseFvi.Visible = false;
            // 
            // txtNameModel
            // 
            this.txtNameModel.BackColor = System.Drawing.SystemColors.Window;
            this.txtNameModel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNameModel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNameModel.Location = new System.Drawing.Point(140, 65);
            this.txtNameModel.Name = "txtNameModel";
            this.txtNameModel.ReadOnly = true;
            this.txtNameModel.Size = new System.Drawing.Size(182, 29);
            this.txtNameModel.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 25);
            this.label1.TabIndex = 17;
            this.label1.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(368, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 25);
            this.label3.TabIndex = 25;
            this.label3.Text = "Qty Camera Read:";
            this.label3.Visible = false;
            // 
            // txt_qtyCam
            // 
            this.txt_qtyCam.Enabled = false;
            this.txt_qtyCam.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_qtyCam.Location = new System.Drawing.Point(579, 144);
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
            this.txt_qtyCam.Size = new System.Drawing.Size(43, 33);
            this.txt_qtyCam.TabIndex = 71;
            this.txt_qtyCam.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txt_qtyCam.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(368, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 25);
            this.label7.TabIndex = 75;
            this.label7.Text = "Qty Jig Plasma:";
            // 
            // txt_qtyJigBase
            // 
            this.txt_qtyJigBase.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_qtyJigBase.Location = new System.Drawing.Point(579, 63);
            this.txt_qtyJigBase.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txt_qtyJigBase.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txt_qtyJigBase.Name = "txt_qtyJigBase";
            this.txt_qtyJigBase.Size = new System.Drawing.Size(43, 33);
            this.txt_qtyJigBase.TabIndex = 76;
            this.txt_qtyJigBase.Value = new decimal(new int[] {
            3,
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
            this.panel1.Size = new System.Drawing.Size(754, 53);
            this.panel1.TabIndex = 77;
            // 
            // btnUpdateModel
            // 
            this.btnUpdateModel.BackColor = System.Drawing.Color.White;
            this.btnUpdateModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdateModel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateModel.Image = global::LineGolden_PLasma.Properties.Resources.icons8_edit_26px;
            this.btnUpdateModel.Location = new System.Drawing.Point(233, 5);
            this.btnUpdateModel.Name = "btnUpdateModel";
            this.btnUpdateModel.Size = new System.Drawing.Size(105, 43);
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
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::LineGolden_PLasma.Properties.Resources.icons8_return_26px;
            this.btnCancel.Location = new System.Drawing.Point(579, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 43);
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
            this.btnAddModel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddModel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAddModel.Image = global::LineGolden_PLasma.Properties.Resources.icons8_new_copy_26px;
            this.btnAddModel.Location = new System.Drawing.Point(60, 5);
            this.btnAddModel.Name = "btnAddModel";
            this.btnAddModel.Size = new System.Drawing.Size(105, 43);
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
            this.btnDelModel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelModel.Image = global::LineGolden_PLasma.Properties.Resources.icons8_delete_trash_26px;
            this.btnDelModel.Location = new System.Drawing.Point(406, 5);
            this.btnDelModel.Name = "btnDelModel";
            this.btnDelModel.Size = new System.Drawing.Size(105, 43);
            this.btnDelModel.TabIndex = 2;
            this.btnDelModel.Text = "  Delete";
            this.btnDelModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelModel.UseVisualStyleBackColor = false;
            this.btnDelModel.Click += new System.EventHandler(this.btnDelModel_Click);
            // 
            // cbo_ModeReadCode
            // 
            this.cbo_ModeReadCode.DisplayMember = "1";
            this.cbo_ModeReadCode.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_ModeReadCode.FormattingEnabled = true;
            this.cbo_ModeReadCode.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.cbo_ModeReadCode.Location = new System.Drawing.Point(165, 146);
            this.cbo_ModeReadCode.Name = "cbo_ModeReadCode";
            this.cbo_ModeReadCode.Size = new System.Drawing.Size(157, 28);
            this.cbo_ModeReadCode.TabIndex = 79;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 25);
            this.label9.TabIndex = 78;
            this.label9.Text = "Read CodePCS:";
            // 
            // txtTimeRepeatJig
            // 
            this.txtTimeRepeatJig.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimeRepeatJig.Location = new System.Drawing.Point(579, 103);
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
            this.txtTimeRepeatJig.Size = new System.Drawing.Size(43, 33);
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
            this.label11.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(368, 107);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(209, 25);
            this.label11.TabIndex = 89;
            this.label11.Text = "Time Repeat Jig (min):";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(12, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 25);
            this.label12.TabIndex = 91;
            this.label12.Text = "Use Machine:";
            // 
            // cbUseMachine
            // 
            this.cbUseMachine.DisplayMember = "1";
            this.cbUseMachine.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUseMachine.FormattingEnabled = true;
            this.cbUseMachine.Items.AddRange(new object[] {
            "NULL",
            "User FVI",
            "User Before Plasma"});
            this.cbUseMachine.Location = new System.Drawing.Point(165, 190);
            this.cbUseMachine.Name = "cbUseMachine";
            this.cbUseMachine.Size = new System.Drawing.Size(157, 28);
            this.cbUseMachine.TabIndex = 92;
            // 
            // chkGetJigHavePcs
            // 
            this.chkGetJigHavePcs.AutoSize = true;
            this.chkGetJigHavePcs.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGetJigHavePcs.Location = new System.Drawing.Point(373, 189);
            this.chkGetJigHavePcs.Name = "chkGetJigHavePcs";
            this.chkGetJigHavePcs.Size = new System.Drawing.Size(175, 29);
            this.chkGetJigHavePcs.TabIndex = 93;
            this.chkGetJigHavePcs.Text = "Get Jig Have Pcs";
            this.chkGetJigHavePcs.UseVisualStyleBackColor = true;
            // 
            // chkUseFvi
            // 
            this.chkUseFvi.AutoSize = true;
            this.chkUseFvi.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseFvi.Location = new System.Drawing.Point(579, 189);
            this.chkUseFvi.Name = "chkUseFvi";
            this.chkUseFvi.Size = new System.Drawing.Size(112, 29);
            this.chkUseFvi.TabIndex = 94;
            this.chkUseFvi.Text = "Using Fvi";
            this.chkUseFvi.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(12, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 25);
            this.label4.TabIndex = 95;
            this.label4.Text = "File Path Boxing:";
            // 
            // txtPathDataBoxing
            // 
            this.txtPathDataBoxing.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPathDataBoxing.Location = new System.Drawing.Point(179, 231);
            this.txtPathDataBoxing.Name = "txtPathDataBoxing";
            this.txtPathDataBoxing.ReadOnly = true;
            this.txtPathDataBoxing.Size = new System.Drawing.Size(490, 26);
            this.txtPathDataBoxing.TabIndex = 96;
            // 
            // btnFilePathBoxing
            // 
            this.btnFilePathBoxing.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFilePathBoxing.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilePathBoxing.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFilePathBoxing.Location = new System.Drawing.Point(675, 228);
            this.btnFilePathBoxing.Name = "btnFilePathBoxing";
            this.btnFilePathBoxing.Size = new System.Drawing.Size(67, 33);
            this.btnFilePathBoxing.TabIndex = 97;
            this.btnFilePathBoxing.Text = ". . .";
            this.btnFilePathBoxing.UseVisualStyleBackColor = false;
            this.btnFilePathBoxing.Click += new System.EventHandler(this.btnFilePathBoxing_Click);
            // 
            // FrmCreateProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(754, 491);
            this.Controls.Add(this.btnFilePathBoxing);
            this.Controls.Add(this.txtPathDataBoxing);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkUseFvi);
            this.Controls.Add(this.chkGetJigHavePcs);
            this.Controls.Add(this.cbUseMachine);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtTimeRepeatJig);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbo_ModeReadCode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txt_qtyJigBase);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_qtyCam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grvModel);
            this.Controls.Add(this.txtNameModel);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmCreateProgram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Program";
            this.Load += new System.EventHandler(this.frmCreateProgram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grvModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyCam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_qtyJigBase)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeRepeatJig)).EndInit();
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
        private System.Windows.Forms.DataGridView grvModel;
        private System.Windows.Forms.TextBox txtNameModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown txt_qtyCam;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txt_qtyJigBase;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbo_ModeReadCode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown txtTimeRepeatJig;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbUseMachine;
        private System.Windows.Forms.CheckBox chkGetJigHavePcs;
        private System.Windows.Forms.CheckBox chkUseFvi;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUseMachine;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDes;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColQtyCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNumJigPlasma;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReadCodePCS;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeRepeatJig;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_GetJigHavePcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_UseFvi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPathDataBoxing;
        private System.Windows.Forms.Button btnFilePathBoxing;
    }
}