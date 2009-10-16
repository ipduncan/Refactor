namespace Aim.Excel.Importer.Presentation
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnExit = new System.Windows.Forms.Button();
            this.openFileDialogExcel = new System.Windows.Forms.OpenFileDialog();
            this.btnPostClaims = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnProcessFile = new System.Windows.Forms.Button();
            this.txtExcelFilename = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gbInputs = new System.Windows.Forms.GroupBox();
            this.gbBatchType = new System.Windows.Forms.GroupBox();
            this.rbExistingBatch = new System.Windows.Forms.RadioButton();
            this.rbNewBatch = new System.Windows.Forms.RadioButton();
            this.cboLedger1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtContractCode = new System.Windows.Forms.TextBox();
            this.gbNewBatch = new System.Windows.Forms.GroupBox();
            this.dtpCheckDate = new System.Windows.Forms.DateTimePicker();
            this.txtCheckAmount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCheckNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbExistingBatch = new System.Windows.Forms.GroupBox();
            this.txtBatchHdrKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvDroppedPayments = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgvReturnedPayments = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.tabPage3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbInputs.SuspendLayout();
            this.gbBatchType.SuspendLayout();
            this.gbNewBatch.SuspendLayout();
            this.gbExistingBatch.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDroppedPayments)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnedPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(468, 354);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "&Close";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPostClaims
            // 
            this.btnPostClaims.Enabled = false;
            this.btnPostClaims.Location = new System.Drawing.Point(377, 265);
            this.btnPostClaims.Name = "btnPostClaims";
            this.btnPostClaims.Size = new System.Drawing.Size(134, 23);
            this.btnPostClaims.TabIndex = 3;
            this.btnPostClaims.Text = "Process Batch";
            this.btnPostClaims.UseVisualStyleBackColor = true;
            this.btnPostClaims.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 384);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(556, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Controls.Add(this.btnPostClaims);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(533, 310);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Summary";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtSummary);
            this.groupBox6.Location = new System.Drawing.Point(13, 14);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(504, 245);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "File Import Summary";
            // 
            // txtSummary
            // 
            this.txtSummary.Location = new System.Drawing.Point(6, 19);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Size = new System.Drawing.Size(492, 220);
            this.txtSummary.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(533, 310);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Import File";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(6, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(521, 206);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Field Mapper";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(515, 187);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnProcessFile);
            this.groupBox1.Controls.Add(this.txtExcelFilename);
            this.groupBox1.Controls.Add(this.btnOpenFile);
            this.groupBox1.Location = new System.Drawing.Point(6, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 50);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Open Excel Document";
            // 
            // btnProcessFile
            // 
            this.btnProcessFile.Enabled = false;
            this.btnProcessFile.Location = new System.Drawing.Point(382, 19);
            this.btnProcessFile.Name = "btnProcessFile";
            this.btnProcessFile.Size = new System.Drawing.Size(103, 23);
            this.btnProcessFile.TabIndex = 2;
            this.btnProcessFile.Text = "Process";
            this.btnProcessFile.UseVisualStyleBackColor = true;
            this.btnProcessFile.Click += new System.EventHandler(this.txtProcess_Click);
            // 
            // txtExcelFilename
            // 
            this.txtExcelFilename.Location = new System.Drawing.Point(6, 20);
            this.txtExcelFilename.Name = "txtExcelFilename";
            this.txtExcelFilename.Size = new System.Drawing.Size(306, 20);
            this.txtExcelFilename.TabIndex = 1;
            this.txtExcelFilename.TextChanged += new System.EventHandler(this.txtExcelFilename_TextChanged);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Enabled = false;
            this.btnOpenFile.Location = new System.Drawing.Point(318, 19);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(28, 19);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gbInputs);
            this.tabPage1.Controls.Add(this.gbNewBatch);
            this.tabPage1.Controls.Add(this.gbExistingBatch);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(533, 310);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Batch Information";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gbInputs
            // 
            this.gbInputs.Controls.Add(this.gbBatchType);
            this.gbInputs.Controls.Add(this.cboLedger1);
            this.gbInputs.Controls.Add(this.label5);
            this.gbInputs.Controls.Add(this.label2);
            this.gbInputs.Controls.Add(this.label6);
            this.gbInputs.Controls.Add(this.txtContractCode);
            this.gbInputs.Location = new System.Drawing.Point(9, 6);
            this.gbInputs.Name = "gbInputs";
            this.gbInputs.Size = new System.Drawing.Size(518, 117);
            this.gbInputs.TabIndex = 0;
            this.gbInputs.TabStop = false;
            this.gbInputs.Text = "Inputs";
            // 
            // gbBatchType
            // 
            this.gbBatchType.Controls.Add(this.rbExistingBatch);
            this.gbBatchType.Controls.Add(this.rbNewBatch);
            this.gbBatchType.Location = new System.Drawing.Point(12, 19);
            this.gbBatchType.Name = "gbBatchType";
            this.gbBatchType.Size = new System.Drawing.Size(488, 25);
            this.gbBatchType.TabIndex = 0;
            this.gbBatchType.TabStop = false;
            this.gbBatchType.Text = "Batch Type";
            // 
            // rbExistingBatch
            // 
            this.rbExistingBatch.AutoSize = true;
            this.rbExistingBatch.Location = new System.Drawing.Point(178, 7);
            this.rbExistingBatch.Name = "rbExistingBatch";
            this.rbExistingBatch.Size = new System.Drawing.Size(92, 17);
            this.rbExistingBatch.TabIndex = 1;
            this.rbExistingBatch.Text = "Existing Batch";
            this.rbExistingBatch.UseVisualStyleBackColor = true;
            this.rbExistingBatch.CheckedChanged += new System.EventHandler(this.rbExistingBatch_CheckedChanged);
            // 
            // rbNewBatch
            // 
            this.rbNewBatch.AutoSize = true;
            this.rbNewBatch.Checked = true;
            this.rbNewBatch.Location = new System.Drawing.Point(78, 8);
            this.rbNewBatch.Name = "rbNewBatch";
            this.rbNewBatch.Size = new System.Drawing.Size(78, 17);
            this.rbNewBatch.TabIndex = 0;
            this.rbNewBatch.TabStop = true;
            this.rbNewBatch.Text = "New Batch";
            this.rbNewBatch.UseVisualStyleBackColor = true;
            this.rbNewBatch.CheckedChanged += new System.EventHandler(this.rbNewBatch_CheckedChanged);
            // 
            // cboLedger1
            // 
            this.cboLedger1.FormattingEnabled = true;
            this.cboLedger1.Location = new System.Drawing.Point(117, 60);
            this.cboLedger1.Name = "cboLedger1";
            this.cboLedger1.Size = new System.Drawing.Size(176, 21);
            this.cboLedger1.TabIndex = 2;
            this.cboLedger1.SelectedValueChanged += new System.EventHandler(this.cboLedger1_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Ledger Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Contract Code";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label6.Location = new System.Drawing.Point(226, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(391, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "This will only be used when the ContractCode is not provided on the spreadsheet.";
            // 
            // txtContractCode
            // 
            this.txtContractCode.Location = new System.Drawing.Point(117, 87);
            this.txtContractCode.Name = "txtContractCode";
            this.txtContractCode.Size = new System.Drawing.Size(103, 20);
            this.txtContractCode.TabIndex = 4;
            // 
            // gbNewBatch
            // 
            this.gbNewBatch.Controls.Add(this.dtpCheckDate);
            this.gbNewBatch.Controls.Add(this.txtCheckAmount);
            this.gbNewBatch.Controls.Add(this.label7);
            this.gbNewBatch.Controls.Add(this.txtCheckNo);
            this.gbNewBatch.Controls.Add(this.label4);
            this.gbNewBatch.Controls.Add(this.label3);
            this.gbNewBatch.Location = new System.Drawing.Point(6, 129);
            this.gbNewBatch.Name = "gbNewBatch";
            this.gbNewBatch.Size = new System.Drawing.Size(518, 109);
            this.gbNewBatch.TabIndex = 2;
            this.gbNewBatch.TabStop = false;
            this.gbNewBatch.Text = "New Batch";
            this.gbNewBatch.Visible = false;
            // 
            // dtpCheckDate
            // 
            this.dtpCheckDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCheckDate.Location = new System.Drawing.Point(93, 74);
            this.dtpCheckDate.Name = "dtpCheckDate";
            this.dtpCheckDate.Size = new System.Drawing.Size(93, 20);
            this.dtpCheckDate.TabIndex = 5;
            this.dtpCheckDate.Value = new System.DateTime(2009, 8, 3, 0, 0, 0, 0);
            this.dtpCheckDate.ValueChanged += new System.EventHandler(this.dtpCheckDate_TextChanged);
            // 
            // txtCheckAmount
            // 
            this.txtCheckAmount.Location = new System.Drawing.Point(93, 48);
            this.txtCheckAmount.Name = "txtCheckAmount";
            this.txtCheckAmount.Size = new System.Drawing.Size(82, 20);
            this.txtCheckAmount.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Check Amount";
            // 
            // txtCheckNo
            // 
            this.txtCheckNo.Location = new System.Drawing.Point(93, 19);
            this.txtCheckNo.Name = "txtCheckNo";
            this.txtCheckNo.Size = new System.Drawing.Size(94, 20);
            this.txtCheckNo.TabIndex = 1;
            this.txtCheckNo.TextChanged += new System.EventHandler(this.txtCheckNo_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Check Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Check Number";
            // 
            // gbExistingBatch
            // 
            this.gbExistingBatch.Controls.Add(this.txtBatchHdrKey);
            this.gbExistingBatch.Controls.Add(this.label1);
            this.gbExistingBatch.Location = new System.Drawing.Point(9, 129);
            this.gbExistingBatch.Name = "gbExistingBatch";
            this.gbExistingBatch.Size = new System.Drawing.Size(518, 60);
            this.gbExistingBatch.TabIndex = 0;
            this.gbExistingBatch.TabStop = false;
            this.gbExistingBatch.Text = "Existing Batch";
            this.gbExistingBatch.Visible = false;
            // 
            // txtBatchHdrKey
            // 
            this.txtBatchHdrKey.Location = new System.Drawing.Point(117, 19);
            this.txtBatchHdrKey.Name = "txtBatchHdrKey";
            this.txtBatchHdrKey.Size = new System.Drawing.Size(108, 20);
            this.txtBatchHdrKey.TabIndex = 0;
            this.txtBatchHdrKey.TextChanged += new System.EventHandler(this.txtBatchHdrKey_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Batch Header Key";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(541, 336);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvDroppedPayments);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(533, 310);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Dropped Payments";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvDroppedPayments
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDroppedPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDroppedPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDroppedPayments.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDroppedPayments.Location = new System.Drawing.Point(7, 7);
            this.dgvDroppedPayments.Name = "dgvDroppedPayments";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDroppedPayments.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvDroppedPayments.Size = new System.Drawing.Size(520, 297);
            this.dgvDroppedPayments.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dgvReturnedPayments);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(533, 310);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Returned Payments";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dgvReturnedPayments
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReturnedPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvReturnedPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReturnedPayments.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvReturnedPayments.Location = new System.Drawing.Point(6, 7);
            this.dgvReturnedPayments.Name = "dgvReturnedPayments";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReturnedPayments.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvReturnedPayments.Size = new System.Drawing.Size(520, 297);
            this.dgvReturnedPayments.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(371, 354);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 406);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AIMHealth Excel Importer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabPage3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.gbInputs.ResumeLayout(false);
            this.gbInputs.PerformLayout();
            this.gbBatchType.ResumeLayout(false);
            this.gbBatchType.PerformLayout();
            this.gbNewBatch.ResumeLayout(false);
            this.gbNewBatch.PerformLayout();
            this.gbExistingBatch.ResumeLayout(false);
            this.gbExistingBatch.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDroppedPayments)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnedPayments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.OpenFileDialog openFileDialogExcel;
        private System.Windows.Forms.Button btnPostClaims;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtExcelFilename;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gbInputs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtContractCode;
        private System.Windows.Forms.GroupBox gbNewBatch;
        private System.Windows.Forms.TextBox txtCheckNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbExistingBatch;
        private System.Windows.Forms.TextBox txtBatchHdrKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnProcessFile;
        private System.Windows.Forms.TextBox txtCheckAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvDroppedPayments;
        private System.Windows.Forms.DateTimePicker dtpCheckDate;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.DataGridView dgvReturnedPayments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLedger1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbBatchType;
        private System.Windows.Forms.RadioButton rbExistingBatch;
        private System.Windows.Forms.RadioButton rbNewBatch;
        private System.Windows.Forms.Button btnClear;
    }
}