using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Aim.Excel.Importer.Presentation.Properties;
using AimHealth.Safari.BulkPosting.Bus;
using AimHealth.Safari.BulkPosting.Exceptions;
using AIMHealth.Safari.Logging;
using Aim.Data.ExcelImporter.ExcelDataReader;
using System.Data.OleDb;
 
namespace Aim.Excel.Importer.Presentation
{
    public partial class MainForm : Form
    {

        #region [Member Variables]

//        private IList<string> _excelColumns;
        private Log _log = null;
        private readonly string _logPath = null;
        private IBulkPayment _bulkPayment;
        private User _user = null;
        private Impersonation _impersonation = null;


        #endregion

        #region [Constructors]

        public MainForm()
        {
            InitializeComponent();
            Properties.Settings settings = Properties.Settings.Default;
            _logPath = settings.LogLocation;

        }

        #endregion

        #region [Form Events]

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            //ContractCode is required
            if (txtContractCode.Text.Length == 0)
            {
                MessageBox.Show("Contract Code is a required field.  Please go back and enter.");
            }
            else
            {
                flowLayoutPanel1.Controls.Clear();

                openFileDialogExcel.Filter = "Excel Files (*.xlsx*)|*.xls*";

                if (openFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    txtExcelFilename.Text = openFileDialogExcel.FileName;
                }
            }
        }

        private void txtExcelFilename_TextChanged(object sender, EventArgs e)
        {
            if (txtExcelFilename.Text.Length > 0)
            {
                ShowStatus("Processing", "Importing excel file column fields.");
                try
                {
                    Application.DoEvents();
                    ExcelColumnReader objExcelColRdr
                        = new ExcelColumnReader(txtExcelFilename.Text);

                    foreach (string value in objExcelColRdr.GetColumns)
                    {
                        AddFileFieldControl(value);
                    }

                    btnProcessFile.Enabled = flowLayoutPanel1.Controls.Count > 0;

                }
                catch (Exception ex)
                {
                    ErrorHandler(ex);
                }
                finally
                {
                    ShowStatus();
                }
            }
        }

        //private void btnPage1Next_Click(object sender, EventArgs e)
        //{
        //    tabControl1.SelectedTab = tabPage2;
        //}

        private void MainForm_Load(object sender, EventArgs e)
        {
            BindLedgerTypes(cboLedger1);

            // set status strip panels
            ToolStripStatusLabel tsiStatus = new ToolStripStatusLabel {AutoSize = true, Text = "Ready"};

            ToolStripSeparator tisSep = new ToolStripSeparator();
            ToolStripStatusLabel tsiAction = new ToolStripStatusLabel {AutoSize = true, Text = string.Empty};

            statusStrip1.Items.Add(tsiStatus);
            statusStrip1.Items.Add(tisSep);
            statusStrip1.Items.Add(tsiAction);

            rbNewBatch.Checked = true;
            gbNewBatch.Visible = true;

            dtpCheckDate.Value = DateTime.Today;

        }

        //private void btnPage1Next_Click_1(object sender, EventArgs e)
        //{
        //    int p = 1;
        //}

        private void txtBatchHdrKey_TextChanged(object sender, EventArgs e)
        {
            btnOpenFile.Enabled = txtBatchHdrKey.Text.Length > 0;
        }

        private void dtpCheckDate_TextChanged(object sender, EventArgs e)
        {
            if (txtCheckNo.Text.Length > 0 && dtpCheckDate.Text.Length > 0)
            {
                btnOpenFile.Enabled = true;
            }
            else
            {
                btnOpenFile.Enabled = false;
            }
        }

        private void txtCheckNo_TextChanged(object sender, EventArgs e)
        {
            if (txtCheckNo.Text.Length > 0 && dtpCheckDate.Text.Length > 0)
            {
                btnOpenFile.Enabled = true;
            }
            else
            {
                btnOpenFile.Enabled = false;
            }
        }

        private void txtProcess_Click(object sender, EventArgs e)
        {
            if (_bulkPayment != null) {
                _bulkPayment = null; }

            try
            {
                ShowStatus("Processing", "Reading data please wait");
                Application.DoEvents(); // prevents the UI from showing a hang or not responding

                string insurerContractCode = txtContractCode.Text;
                IBulkPayment bulkPayment;

                _user = new User();
                EvaluateUserPermissions(_user, SecurityPermittedAccess.PayerFeeBatchCreate);
                //Authorization succeeded

                //Impersonate system user for permission
                if (_impersonation == null)
                {
                    ImpersonateSystemUser();
                }
                string filename;
                if (rbExistingBatch.Checked & txtBatchHdrKey.Text.Length > 0)
                {
                    // existing batch
                    int batchHeaderKey = int.Parse(txtBatchHdrKey.Text);
                    IBulkPayment ibp = new BulkPayment((LedgerType)cboLedger1.SelectedIndex, batchHeaderKey);
                    bulkPayment = ibp;
                    DateTime checkDate = (DateTime)bulkPayment.Date;
                    filename = DateTime.Now.ToString("yyyyMMdd_hh_mm_ss")
                        + "_" + "BulkPosting_" + bulkPayment.Id
                        + "_to_" + bulkPayment.LedgerType
                        + "_" + checkDate.ToString("yyyyMMdd") + ".txt";
                }
                else
                {
                    // new batch

                    double checkAmount = double.Parse(txtCheckAmount.Text);
                    DateTime checkDate = DateTime.Parse(dtpCheckDate.Text);
                    string checkNumber = txtCheckNo.Text;
                    LedgerType ledgerType = (LedgerType)cboLedger1.SelectedIndex;

                    IBulkPayment ibp = new BulkPayment(ledgerType
                        , checkNumber, checkAmount, checkDate, _user);
                    bulkPayment = ibp;
                    filename = DateTime.Now.ToString("yyyyMMdd_hh_mm_ss")
                        + "_" + "BulkPosting_" + checkNumber + "_to_" + ledgerType
                        + "_" + checkDate.ToString("yyyyMMdd") + ".txt";
                }
                //Begin logging
                _log = new Log(_logPath, filename);

                VerifyLogFileCreated();

                WriteLogHeader();

                bulkPayment.DefaultInsurerContractCode = insurerContractCode;
                //Revert back after data access is complete
                RevertImpersonation();
                if (bulkPayment.DefaultInsurerContractKey == 0)
                {
                    MessageBox.Show("Invalid Contract Code.  Please update and try again.");
                    bulkPayment = null;
                }
                else
                {
                    //user needs to drag and drop columns to map
                    Mapper mapper = CreateMapperObject();

                    LogBulkPaymentInformation(bulkPayment);

                    LogMappingResults(mapper);

                    DataTable dt = ReadPaymentsFromFile(mapper);

                    // check for Excel office version -- old versions
                    bool deleteFirstRow = false;
                    string fileName = Path.GetFileName(txtExcelFilename.Text);
                    if (fileName.ToLower().EndsWith(".xls"))
                    {
                        deleteFirstRow = true;
                    }

                    bulkPayment.ProcessPayments(dt, mapper, deleteFirstRow);

                    //Display Dropped payments to user
                    LogDroppedPayments(bulkPayment);

                    //Provide summary to user
                    LogSummaryInformation(bulkPayment);
                    //bulkPayment.su

                    //Display new payments to user
                    LogListOfPayments(bulkPayment);

                    _bulkPayment = bulkPayment;

                    btnPostClaims.Enabled = true;
                    ShowStatus();
                }

                //Impersonate system user for permission
                if (_impersonation == null)
                {
                    RevertImpersonation();
                }

            }
            catch(Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_user.IsAuthorizedPayerFeeBatchCreate)
            {
                try
                {
                    if (_impersonation == null)
                    {
                        ImpersonateSystemUser();
                    }

                    ShowStatus("Processing", "Posting batch, please wait");

                    btnPostClaims.Enabled = false;

                    IBulkPayment bulkPayment = _bulkPayment;

                    //user decides to post the batch
                    int batchHeaderKey = bulkPayment.PersistBulkPayment(_user.Login);
                    _log.WriteLog("Batch {0} created on {1}", batchHeaderKey, DateTime.Now);

                    txtSummary.Text = txtSummary.Text + Environment.NewLine + Environment.NewLine +
                                      "Your batch header key: " + batchHeaderKey;

                    ShowStatus();

                    if (_impersonation != null)
                    {
                        RevertImpersonation();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler(ex);
                }
            }
            else
            {
                ErrorHandler("User not authorized to create a batch.");
            }
        }

        private void rbExistingBatch_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExistingBatch.Checked)
                ToggleBatchType();
        }

        private void rbNewBatch_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNewBatch.Checked)
                ToggleBatchType();
        }

        private void cboLedger1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboLedger1.SelectedItem.ToString() != "Payer Accounts Receivable")
            {
                MessageBox.Show("At this time, no other selections are valid.");
                cboLedger1.SelectedItem = "Payer Accounts Receivable";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _bulkPayment = null;
            _log = null;
            txtContractCode.Clear();
            txtCheckNo.Clear();
            txtCheckAmount.Clear();
            txtBatchHdrKey.Clear();
            txtExcelFilename.Clear();
            txtSummary.Clear();
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Refresh();
            dgvDroppedPayments.DataSource = null;
            dgvDroppedPayments.Refresh();
            dgvReturnedPayments.DataSource = null;
            dgvReturnedPayments.Refresh();

            if (_impersonation != null)
            {
                RevertImpersonation();
            }
        }

        #endregion

        #region [Private Methods]

        private void ToggleBatchType()
        {
            gbNewBatch.Visible = !gbNewBatch.Visible;
            gbExistingBatch.Visible = !gbExistingBatch.Visible;
        }

        private void LogSummaryInformation(IBulkPayment bulkPayment)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Total from Inputs: {0}"
                , bulkPayment.Total.ToString("$#,###.##"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Total Dropped: {0}, #: {1}"
                , bulkPayment.SumDroppedPayments.ToString("$#,###.##"),
                bulkPayment.NumberDroppedPayments.ToString("#,###"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Total Commission Payments: {0}, #: {1}"
                , bulkPayment.SumIdentifiedPayments.ToString("$#,###.##"),
                bulkPayment.CountIdentifiedPayments.ToString("#,###"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Total Unidentified: {0}, #: {1}"
                , bulkPayment.SumUnidentifedPayments.ToString("$#,###.##"),
                bulkPayment.CountUnidentifiedPayments.ToString("#,###"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Total Open Credit: {0}, #: {1}"
                , bulkPayment.SumOpenCreditPayments.ToString("$#,###.##"),
                bulkPayment.CountOpenCreditPayments.ToString("#,###"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            double ssTotal = bulkPayment.SumIdentifiedPayments + bulkPayment.SumUnidentifedPayments
                             + bulkPayment.SumOpenCreditPayments;
            int ssNumber = bulkPayment.CountIdentifiedPayments + bulkPayment.CountUnidentifiedPayments
                           + bulkPayment.CountOpenCreditPayments;
            stringBuilder.AppendFormat("Total From Batch: {0}, #: {1}", ssTotal.ToString("$#,###.##")
                , ssNumber.ToString("#,###"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            //have not been able to figure out why this rounding here is necessary
            double diff = Math.Round(ssTotal - bulkPayment.Total, 2);

            string direction = "over";
            if (diff < 0) { direction = "under"; }

            if (diff == 0)
            { stringBuilder.AppendFormat("Batch is in balance."); }
            else
            { stringBuilder.AppendFormat("Batch will be {0} by {1}.", direction, diff.ToString("$#,###.##")); }

            txtSummary.Text = stringBuilder.ToString();

            _log.WriteLog(stringBuilder.ToString());
            _log.WriteLog("");
        }

        private void LogDroppedPayments(IBulkPayment bulkPayment)
        {
            StringBuilder stringBuilder = new StringBuilder();

            AimHealth.Safari.BulkPosting.Data.DroppedPaymentsDataTable dpo 
                = new AimHealth.Safari.BulkPosting.Data.DroppedPaymentsDataTable();

            stringBuilder.AppendLine("Dropped Payments:");
            foreach (IPayment payment in bulkPayment.DroppedPayments)
            {
                stringBuilder.AppendFormat(
                    "Claim Identifier{0}, Amt {1}, Note: {2}"
                      , payment.GetClaimIdentifier(), payment.Amount
                      , payment.Note);

                dpo.AddRow(payment.GetClaimIdentifier(),
                    payment.Amount, payment.Note);
            }

            dgvDroppedPayments.DataSource = dpo.GetDataTable();
            foreach (DataGridViewTextBoxColumn column in dgvDroppedPayments.Columns)
            {
                if (column.Name == "Amount")
                {
                    dgvDroppedPayments.Columns[column.Name].DefaultCellStyle.Format = "c";
                }
                if (column.Name == "Note")
                {
                    dgvDroppedPayments.Columns[column.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                
            }

            _log.WriteLog(stringBuilder.ToString());
            _log.WriteLog("");

        }

        private DataTable ReadPaymentsFromFile(Mapper mapper)
        {
            string fileName = Path.GetFileName(txtExcelFilename.Text);
            DataTable dt = new DataTable();

            // check for Excel office version -- old versions
            if (fileName.ToLower().EndsWith(".xls")) 
            {
                FileStream stream = new FileStream(txtExcelFilename.Text, FileMode.Open);
                ExcelDataReader excelReader
                    = new ExcelDataReader(stream);

                DataSet ds = excelReader.GetDataSet();
                dt = ds.Tables[0];

                //this stores the mapped column names to the column names in Excel
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (mapper.ContainsKey(ds.Tables[0].Rows[0][i].ToString()))
                    {
                        dt.Columns[i].ColumnName = (string)ds.Tables[0].Rows[0][i];
                    }
                }
                stream.Close();
            }
            else
            {
                // Office 2007 file parser

                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + txtExcelFilename.Text + ";" +
                "Extended Properties=\"Excel 12.0;HDR=YES;\"";

                // if you don't want to show the header row (first row)
                // use 'HDR=NO' in the string

                string oleSql = "SELECT * FROM [{0}]";
                DataSet ds = new DataSet();

                using (OleDbConnection oleCnn = new OleDbConnection(connectionString))
                {
                    oleCnn.Open();
                    
                    DataTable schema = oleCnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string sheet1Name = schema.Rows[0]["TABLE_NAME"].ToString();
                    oleSql = String.Format(oleSql, sheet1Name);

                    using (OleDbDataAdapter oleDA = new OleDbDataAdapter(oleSql, oleCnn))
                    {
                        oleDA.Fill(ds);
                    }
                }
                dt = ds.Tables[0];

            }

            return dt;

        }

        private void LogBulkPaymentInformation(IBulkPayment bulkPayment)
        {
            _log.WriteLog("Bulk Payment Information:");
            _log.WriteLog("User: {0}" , bulkPayment.User.Login);
            _log.WriteLog("Ledger: {0}", bulkPayment.LedgerType);
            _log.WriteLog("Check Number: {0}", bulkPayment.Id);
            _log.WriteLog("Check Amount: {0}", bulkPayment.Total);
            _log.WriteLog("Check Date: {0}", bulkPayment.Date);
            _log.WriteLog("");
        }

        private void WriteLogHeader()
        {
            Properties.Settings settings = Properties.Settings.Default;
            _log.WriteLog("Log: {0}", _log.FullName);
            _log.WriteLog("Begin Time: {0}", DateTime.Now);
            _log.WriteLog("Connection String: {0}", settings.SafariConnection.ToString());
            _log.WriteLog("");
        }

        private void ErrorHandler(string message)
        {
            Exception ex = new Exception(message);
            ErrorHandler(ex);
        }

        private void ErrorHandler(Exception ex)
        {
            MessageBox.Show(ex.Message);
            if (_log == null) MessageBox.Show("Log file not created at this point.  Cannot log error.");
            else
            {
                _log.DocumentError(_log, "Application Error", ex);
                _log.WriteLog("");
            }

            if (_impersonation != null)
            {
                RevertImpersonation();
            }


        }

        private void VerifyLogFileCreated()
        {
            //check to make sure log was successfully created
            FileInfo newLog = new FileInfo(_log.FullName);
            if (!newLog.Exists || _log == null)
            {
                ErrorHandler("Failure to initialize log file.");
            }
        }

        private void EvaluateUserPermissions( IUser user, SecurityPermittedAccess requestedAccess)
        {
            user.IdentifyWindowsUser();
            if (user.IsAuthenticated)
            {
                switch (requestedAccess)
                {
                    case SecurityPermittedAccess.PayerFeeBatchCreate:
                        {
                            StringCollection allowedRoles = Properties.Settings.Default.SecurityPayerFeeBatchCreate;
                            user.IsAuthorizedPayerFeeBatchCreate = user.AuthorizeAction(allowedRoles);
                            if (!user.IsAuthorizedPayerFeeBatchCreate)
                            {
                                UserNotAuthorizedException ex = new UserNotAuthorizedException("User not authorized to complete task.");
                                ErrorHandler(ex);
                            }
                            break;
                        }
                }
           }
            else
            {
                UserNotAuthorizedException ex = new UserNotAuthorizedException("User could not be properly identified to complete task.");
                ErrorHandler(ex);
            }

        }

        private Mapper CreateMapperObject()
        {
            Mapper mapper = new Mapper();
            string labelValue = string.Empty;
            string comboValue = string.Empty;

            foreach (Control ctl in flowLayoutPanel1.Controls)
            {
                Type ctlType = ctl.GetType();

                if (ctlType == typeof(Label))
                {
                    labelValue = ctl.Text;
                }
                else if (ctlType == typeof(ComboBox))
                {
                    comboValue = ctl.Text;
                }

                if (labelValue != string.Empty && comboValue != string.Empty && comboValue != string.Empty)
                {
                    mapper.Add(labelValue, comboValue);

                    labelValue = string.Empty;
                    comboValue = string.Empty;
                }

            }

            return mapper;
        }


        private void LogMappingResults(Mapper mapper)
        {
            //displays the mapping results
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Mapping Results:");
            foreach (DictionaryEntry de in mapper)
            {
                sb.AppendFormat("Excel Column: {0}, Payment Property: {1};", de.Key, de.Value);
                sb.AppendLine();
            }
            _log.WriteLog(sb.ToString());
            _log.WriteLog("");

        }

        private void LogListOfPayments(IBulkPayment bulkPayment)
        {
            //These are the remaining payments that will be part of the batch
            StringBuilder stringBuilder = new StringBuilder();

            AimHealth.Safari.BulkPosting.Data.PaymentsDataTable dpo = new AimHealth.Safari.BulkPosting.Data.PaymentsDataTable();

            foreach (IPayment payment in bulkPayment.Payments)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat(
                    "Claim Identifier {0}, ClaimHeaderKey {1}, Amt {2}, Contract {3}: {4}, Balance {5}, Recovered {6}, Invoice {7} Closed? {8}, Note: {9}"
                    , payment.GetClaimIdentifier()
                    , payment.ClaimHeaderKey
                    , payment.Amount
                    , payment.InsurerContractKey, payment.InsurerContractCode
                    , payment.InvoiceBalanceDue, payment.RefundRecoveredAmount
                    , payment.InvoiceKey
                    , payment.IsInvoiceClosed
                    , payment.Note);

                dpo.AddRow(payment.GetClaimIdentifier(),
                    payment.Amount, payment.InsurerContractCode,
                    payment.InvoiceBalanceDue,
                    payment.RefundRecoveredAmount,
                    payment.InvoiceKey,
                    payment.IsInvoiceClosed,
                    payment.Note);
            }

            dgvReturnedPayments.DataSource = dpo.GetDataTable();
            try
            {
                foreach (DataGridViewTextBoxColumn column in dgvReturnedPayments.Columns)
                {
                    switch (column.Name)
                    {
                        case "Amount":
                        case "Recovered":
                        case "Balance":
                            dgvReturnedPayments.Columns[column.Name].DefaultCellStyle.Format = "c";
                            dgvReturnedPayments.Columns[column.Name].DefaultCellStyle.Alignment =
                                DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "Invoice":
                        case "Contract":
                            dgvReturnedPayments.Columns[column.Name].DefaultCellStyle.Alignment =
                                DataGridViewContentAlignment.MiddleRight;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch 
            {
                //do nothing
            }

            _log.WriteLog(stringBuilder.ToString());
            _log.WriteLog("");
        }

        private static void BindLedgerTypes(ComboBox cbo)
        {
            cbo.Items.Add("Provider Accounts Receivable");
            cbo.Items.Add("Payer Accounts Receivable");
            cbo.Items.Add("Accounts Payable");
            cbo.SelectedIndex = 1;
        }

        private void AddFileFieldControl(string field)
        {
            Label lblField = new Label {Width = 120, Text = field};

            ComboBox cbo = new ComboBox {Width = 140};
            StringCollection propertyList = new Mapper().GetPaymentProperties();

            //add payment properties to second list
            foreach (string str in propertyList)
            {
                cbo.Items.Add(str);

                if (field.ToLower() == str.ToLower())
                {
                    cbo.SelectedItem = str;
                }
            }

            flowLayoutPanel1.Controls.Add(lblField);
            flowLayoutPanel1.Controls.Add(cbo);
            flowLayoutPanel1.SetFlowBreak(cbo, true);
        }


        private void ShowStatus(string status, string action)
        {
            statusStrip1.Items[0].Text = status;
            statusStrip1.Items[2].Text = action;
            Cursor.Current = Cursors.WaitCursor;
        }
        private void ShowStatus()
        {
            statusStrip1.Items[0].Text = "Ready";
            statusStrip1.Items[2].Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }

        private void ImpersonateSystemUser()
        {
            _impersonation = new Impersonation();
            _impersonation.Impersonate();
        }

        private void RevertImpersonation()
        {
            if (_impersonation != null)
            {
                _impersonation.Revert();
            }
        }


        #endregion


        
    }
}