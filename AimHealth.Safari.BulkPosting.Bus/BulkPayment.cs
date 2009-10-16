using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AimHealth.Safari.BulkPosting.DTO;
using AimHealth.Safari.BulkPosting.Bus.BatchRepository;
using AimHealth.Safari.BulkPosting.Exceptions;
 
namespace AimHealth.Safari.BulkPosting.Bus
{

    public interface IBulkPayment
    {

        int PersistBulkPayment(string userLogin);
        void ProcessPayments(DataTable table, Mapper mapper, bool deleteFirstRow);

        string Id { get; set; }
        double Total { get; set; }
        DateTime? Date { get; set; }
        int BatchHeaderKey { get; set; }
        string DefaultInsurerContractCode { get; set; }
        int DefaultInsurerContractKey { get; }
        double SumIdentifiedPayments { get; set; }
        double SumUnidentifedPayments { get; set; }
        int CountIdentifiedPayments { get; set; }
        int CountUnidentifiedPayments { get; set; }
        int CountOpenCreditPayments { get; set; }
        double SumOpenCreditPayments { get; set; }
        int NumberDroppedPayments { get; set; }
        double SumDroppedPayments { get; set; }
        double SumPayments { get; set; }
        int CountPayments { get; set; }
        LedgerType LedgerType { get; set; }
        IUser User { get; set; }
        IList<IPayment> Payments { get; set; }
        IList<IPayment> DroppedPayments { get; set; }
    }


    public class BulkPayment: IBulkPayment
    {

        private string _id = "";
        private double _total = 0;
        private DateTime? _date;
        private string _defaultInsurerContractCode;
        private int _defaultInsurerContractKey = 0;
        private double _sumIdentifiedPayments = 0;
        private double _sumUnidentifedPayments = 0;
        private int _countIdentifiedPayments = 0;
        private int _countUnidentifiedPayments = 0;
        private double _sumOpenCreditPayments = 0;
        private int _countOpenCreditPayments = 0;
        private int _batchHeaderKey = 0;
        private LedgerType _ledgerType;
        //Payments are the list of amounts from the spreadsheet
        IList<IPayment> _payments = new List<IPayment>();
        private IUser _user;
        private int _countDroppedPayments;
        private double _sumDroppedPayments;
        IList<IPayment> _droppedPayments = new List<IPayment>();
        private double _sumPayments;
        private int _countPayments;
        private readonly IBatch _batch;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="id"></param>
        /// <param name="total"></param>
        /// <param name="date"></param>
        /// <param name="user"></param>
        public BulkPayment(LedgerType ledgerType, string id, double total, DateTime? date
             , IUser user)
            : this(ledgerType, id, total, date, user, new Batch())  //syntax to call the other constructor
        {
        }

        /// <summary>
        /// Constructor used for testing
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="id"></param>
        /// <param name="total"></param>
        /// <param name="date"></param>
        /// <param name="user"></param>
        /// <param name="batch"></param>
        internal BulkPayment(LedgerType ledgerType, string id, double total, DateTime? date
             , IUser user, IBatch batch)
        {
            _ledgerType = ledgerType;
            _id = id;
            _total = total;
            _date = date;
            _user = user;
            _batch = batch;

        }
        /// <summary>
        /// Constructor used for an existing Batch
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="key"></param>
        public BulkPayment(LedgerType ledgerType, int key)
            : this(ledgerType, key, new Batch())
        {
        }

        /// <summary>
        /// Constructor used for testing when the Batch already exists
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="batchHeaderKey"></param>
        /// <param name="batch"></param>
        internal BulkPayment(LedgerType ledgerType, int batchHeaderKey, IBatch batch)
        {
            _batchHeaderKey = batchHeaderKey;
            _ledgerType = ledgerType;
            _batch = batch;
            GetBatch(ledgerType, batchHeaderKey);
        }

        /// <summary>
        /// Adds Payments to Payments list; Verifies payments; Identifies Claims; and Identifies Transaction Types
        /// </summary>
        /// <param name="table"></param>
        /// <param name="mapper"></param>
        /// <param name="deleteFirstRow"></param>
        public void ProcessPayments(DataTable table, Mapper mapper, bool deleteFirstRow)
        {

            AddPayments(table, mapper, deleteFirstRow);

            VerifyPayments();

            IdentifyClaims();

            IdentifyTransactionTypes();

        }

        /// <summary>
        /// Creates a batch and persists in the database
        /// </summary>
        public int PersistBulkPayment(string userLogin)
        {
            BatchHeaderDTO batchHeaderDTO = UpdateToBatchHeaderDTO();
            int batchHeaderKey = 0;
            //if (batchHeaderDTO.Key == 0)
            //{
                batchHeaderKey = _batch.CreateBatch(LedgerType.PayerAccountsReceivable.ToString()
                                   , batchHeaderDTO);
            //}
            //else
            //{
            //    batchHeaderKey = batchHeaderDTO.Key;
            //}

            return batchHeaderKey;
        }

        /// <summary>
        /// Adds a list of payments to the BulkPayment object
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mapper"></param>
        /// <param name="deleteFirstRow"></param>
        private void AddPayments(DataTable dt, Mapper mapper, bool deleteFirstRow)
        {
            //add each payment to bulkPayment object

            if (deleteFirstRow)
            {
                //Deletes the first row with the column names
                dt.Rows[0].Delete();
            }
            //stores the data in the first sheet to the bulkpayment.Payments collection
                foreach (DataRow row in dt.Rows)
                {
                    IPayment payment = AddPayment();
                    try
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (mapper.ContainsKey(column.ColumnName))
                            {payment.SetProperty(mapper[column.ColumnName], row[column].ToString());}
                        }
                    }
                    catch 
                    {
                        //MessageBox.Show(ex.Message);
                        RemovePayment(payment);
                        //add to Dropped Payments
                        _droppedPayments.Add(payment);
                        _sumDroppedPayments += payment.Amount;
                        _countDroppedPayments += 1;
                    }
                }


        }

        /// <summary>
        /// To verify the information in the payment object.
        /// </summary>
        internal void VerifyPayments()
        {
            foreach (IPayment payment in _payments)
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (! PaymentVerifier.VerifyContractInformation(payment.InsurerContractCode, payment.InsurerCode))
                {
                    stringBuilder.Append(AddToNote(stringBuilder.ToString(),
                                              "Must include either InsurerCode or Contract Code."));
                }

                if (! PaymentVerifier.VerifyPaymentReferenceInformation(payment.AimClaimId, payment.ClientUniqueIdentifier, payment.ClientClaimId))
                {
                    stringBuilder.Append(AddToNote(stringBuilder.ToString(),
                                              "Payment must include the AimClaimID, RecordID, UID, ClientClaimID, or ClientUniqueIdentifier"));
                }

                if (! PaymentVerifier.VerifyPaymentAmountInformation(payment.Amount))
                {
                    stringBuilder.Append(AddToNote(stringBuilder.ToString(),
                                              "Amount must be greater than zero."));
                    
                }

                payment.Note = stringBuilder.ToString();
                if (payment.Note.Length > 0 )
                { AddNewDroppedPayment(payment);}
            }
            
            RemoveDroppedPayments();
        }

        /// <summary>
        /// Identifies payments as a specific safari claim and returns 
        /// additional information to the payment object
        /// </summary>
        internal void IdentifyClaims()
        {
            IList<PaymentDTO> paymentList = new List<PaymentDTO>();
            foreach (IPayment payment in Payments)
            {
                PaymentDTO paymentDTO = CreatePaymentDTOFromPayment(payment);
                paymentList.Add(paymentDTO);
            }

             IList<PaymentDTO> newPaymentList = _batch.IdentifyClaims(paymentList);
            _payments.Clear();

            foreach (PaymentDTO paymentDTO in newPaymentList)
            {
                if (!(paymentDTO.InsurerContractKey > 0))
                {
                    paymentDTO.InsurerCode = "";
                    paymentDTO.InsurerContractKey = DefaultInsurerContractKey;
                    paymentDTO.InsurerContractCode = DefaultInsurerContractCode;
                }
                
                AddPayment(paymentDTO);
            }

        }

        private string AddToNote (string oldStr, string newStr)
        {
            if (oldStr.Length > 0)
            {
                oldStr = oldStr + " ";
                return oldStr = oldStr + newStr;
            }
            else
            {
                return newStr;                
            }
        }

        /// <summary>
        /// Add a dropped payment to the list
        /// </summary>
        /// <param name="payment"></param>
        private void AddNewDroppedPayment(IPayment payment)
        {
            //Drop the payment
            _droppedPayments.Add(payment);
            NumberDroppedPayments += 1;
            SumDroppedPayments += payment.Amount;
        }

        /// <summary>
        /// Remove a dropped payment from the payments list
        /// </summary>
        private void RemoveDroppedPayments()
        {
            //Remove the dropped payments from the payments list
            foreach (IPayment droppedPayment in DroppedPayments)
            {
                _payments.Remove(droppedPayment);
            }

        }

        /// <summary>
        /// Classifies each payment and stores in Batch to update later
        /// </summary>
        private void IdentifyTransactionTypes()
        {
            IList<PaymentDTO> paymentList = new List<PaymentDTO>();
            foreach (IPayment payment in Payments)
            {
                PaymentDTO paymentDTO = CreatePaymentDTOFromPayment(payment);
                paymentList.Add(paymentDTO);
            }
            BatchHeaderDTO batchHeaderDTO = _batch.IdentifyTransactionTypes(paymentList);
            StoreBatchHeaderDTOSummaries(batchHeaderDTO);

            UpdatePaymentsWithUnidentifiedMoneys();

            UpdatePaymentsWithOpenCredits();
        }

        /// <summary>
        /// Updates the payment if it will be posted as open credit
        /// </summary>
        private void UpdatePaymentsWithOpenCredits()
        {
           IList<string> openCreditList = _batch.ReturnListOfPaymentsThatWillBeOpenCredits();

            foreach (string reference in openCreditList)
            {
                var query = from Payment in Payments
                              where Payment.AimClaimId == reference ||
                              Payment.ClientUniqueIdentifier == reference ||
                              Payment.ClientClaimId == reference
                              select Payment;

                foreach (var payment in query)
                {
                    StringBuilder str = new StringBuilder(payment.Note);
                    str.Append("Open Credit");
                    payment.Note = str.ToString();
                }
                
            }
        }

        /// <summary>
        /// Updates the payment if it will be posted as unidentified money
        /// </summary>
        private void UpdatePaymentsWithUnidentifiedMoneys()
        {
            IList<string> unidentifiedList = _batch.ReturnListOfPaymentsThatWillBeUnidentifiedMoneys();

            foreach (string reference in unidentifiedList)
            {
                var query = Payments.Where(Payment => Payment.AimClaimId == reference ||
                                                      Payment.ClientUniqueIdentifier == reference ||
                                                      Payment.ClientClaimId == reference);

                foreach (var payment in query)
                {
                    StringBuilder str = new StringBuilder(payment.Note);
                    str.Append("Unidentified");
                    payment.Note = str.ToString();
                }
            }
        }



        /// <summary>
        /// Stores the information returned from a BatchHeaderDTO and stores to BulkPayment properties.
        /// </summary>
        /// <param name="batchHeaderDTO"></param>
        private void StoreBatchHeaderDTOSummaries(BatchHeaderDTO batchHeaderDTO)
        {
            CountPayments += batchHeaderDTO.CountPayments;
            SumPayments += batchHeaderDTO.SumPayments;
            CountIdentifiedPayments += batchHeaderDTO.CountCommissions;
            SumIdentifiedPayments += batchHeaderDTO.SumCommissions;
            CountUnidentifiedPayments += batchHeaderDTO.CountUnidentifieds;
            SumUnidentifedPayments += batchHeaderDTO.SumUnidentifieds;
            CountOpenCreditPayments += batchHeaderDTO.CountOpenCredits;
            SumOpenCreditPayments += batchHeaderDTO.SumOpenCredits;            
        }

        /// <summary>
        /// Adds a PaymentDTO to the Payments property
        /// </summary>
        /// <param name="paymentDTO"></param>
        private void AddPayment(PaymentDTO paymentDTO)
        {

            IPayment payment = new Payment(
                paymentDTO.AimClaimId
                , paymentDTO.Amount
                , paymentDTO.ClientUniqueIdentifier
                , paymentDTO.ClientClaimId
                , paymentDTO.InsurerContractCode
                , paymentDTO.InsurerCode
                );
            payment.UpdateInformation(
                paymentDTO.ClaimHeaderKey
                , paymentDTO.InvoiceKey
                , paymentDTO.IsInvoiceClosed
                , paymentDTO.InsurerContractKey
                , paymentDTO.InvoiceBalanceDue
                , paymentDTO.RefundRecoveredAmount
                );
            _payments.Add(payment);
        }

        /// <summary>
        /// Creates a PaymentDTO from an IPayment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        private static PaymentDTO CreatePaymentDTOFromPayment(IPayment payment)
        {
            PaymentDTO paymentDTO = new PaymentDTO(
                payment.AimClaimId
                , payment.ClientUniqueIdentifier
                , payment.ClientClaimId
                , payment.InsurerContractCode
                , payment.InsurerCode
                , payment.Amount
                , payment.ClaimHeaderKey
                , payment.InsurerContractKey
                , payment.InvoiceBalanceDue
                , payment.RefundRecoveredAmount
                , payment.InvoiceKey
                , payment.IsInvoiceClosed
                , payment.Note
                );
            return paymentDTO;
        }

        /// <summary>
        /// Retrieves an existing batch from the database; brings back only header and summary information
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="key"></param>
        private void GetBatch(LedgerType ledgerType, int key)
        {
            BatchHeaderDTO batchHeaderDTO = null;
            switch (ledgerType)
            {
                case LedgerType.PayerAccountsReceivable:
                    batchHeaderDTO = _batch.GetPayerBatch(key);
                    break;
                default:
                    break;
            }

            if (batchHeaderDTO != null) {UpdateFromBatchHeaderDTO(batchHeaderDTO);}
            else {
                throw new BatchNotFoundException("Batch not found"); }
        }

        /// <summary>
        /// Updates BulkPayment properties from information returned from a BatchHeaderDTO
        /// </summary>
        /// <param name="batchHeaderDTO"></param>
        private void UpdateFromBatchHeaderDTO(BatchHeaderDTO batchHeaderDTO)
        {
            if (batchHeaderDTO != null)
            {
                _id = batchHeaderDTO.PayerCheckNumber;
                _total = batchHeaderDTO.PayerCheckAmount;
                _date = batchHeaderDTO.PayerCheckDate;
                _user = new User(0, batchHeaderDTO.UserAdded);
                _batchHeaderKey = batchHeaderDTO.Key;
                _sumPayments = batchHeaderDTO.SumPayments;
                _countPayments = batchHeaderDTO.CountPayments;
                _sumIdentifiedPayments = batchHeaderDTO.SumCommissions;
                _countIdentifiedPayments = batchHeaderDTO.CountCommissions;
                _sumUnidentifedPayments = batchHeaderDTO.SumUnidentifieds;
                _countUnidentifiedPayments = batchHeaderDTO.CountUnidentifieds;
                _sumOpenCreditPayments = batchHeaderDTO.SumOpenCredits;
                _countOpenCreditPayments = batchHeaderDTO.CountOpenCredits;

            }
        }

        /// <summary>
        /// Create a BatchHeaderDTO from bulkpayment
        /// </summary>
        private BatchHeaderDTO UpdateToBatchHeaderDTO()
        {
            BatchHeaderDTO batchHeaderDTO = new BatchHeaderDTO();
            batchHeaderDTO.PayerCheckNumber = _id;
            batchHeaderDTO.PayerCheckAmount = _total;
            batchHeaderDTO.PayerCheckDate = _date;
            batchHeaderDTO.UserAdded = _user.Login;
            batchHeaderDTO.Key = _batchHeaderKey;
            batchHeaderDTO.SumPayments = _sumPayments;
            batchHeaderDTO.CountPayments = _countPayments;
            batchHeaderDTO.SumCommissions = _sumIdentifiedPayments;
            batchHeaderDTO.CountCommissions = _countIdentifiedPayments;
            batchHeaderDTO.SumOpenCredits = _sumOpenCreditPayments;
            batchHeaderDTO.CountOpenCredits = _countOpenCreditPayments;
            batchHeaderDTO.SumUnidentifieds = _sumUnidentifedPayments;
            batchHeaderDTO.CountUnidentifieds = _countUnidentifiedPayments;
            return batchHeaderDTO;
        }

        /// <summary>
        /// Removes a payment from _payments
        /// </summary>
        /// <param name="payment"></param>
        private void RemovePayment(IPayment payment)
        {
            _payments.Remove(payment);
        }

        /// <summary>
        /// 
        /// Adds a payment to list of Payments
        /// </summary>
        private IPayment AddPayment()
        {
            IPayment payment = new Payment();
            _payments.Add(payment);
            return payment;
        }


        /// <summary>
        /// Identifies an InsurerContract given an InsurerContractCode
        /// </summary>
        /// <param name="insurerContractCode"></param>
        /// <returns></returns>
        private int IdentifyContract(string insurerContractCode)
        {
            return _batch.IdentifyContract(insurerContractCode);
        }
        

        /// <summary>
        /// Rounds the amount to the nearest cent.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static double RoundAmount(double amount)
        {
            return Math.Round(amount, 2);
        }


        /// <summary>
        /// BulkPayment identifier; ususally a check or wire number
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Total of the BulkPayment
        /// </summary>
        public double Total
        {
            get { return _total; }
            set { _total = RoundAmount(Convert.ToDouble(value)); }
        }

        /// <summary>
        /// Date of the check or wire
        /// </summary>
        public DateTime? Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// Identifier for the persisted Batch
        /// </summary>
        public int BatchHeaderKey
        {
            get { return _batchHeaderKey; }
            set { _batchHeaderKey = value; }
        }

        /// <summary>
        /// The InsurerContractCode to be used when one cannot be gotten from the payment or identified claim.
        /// Used to post an Unidentified transaction
        /// </summary>
        public string DefaultInsurerContractCode
        {
            get { return _defaultInsurerContractCode; }
            set 
            { 
                _defaultInsurerContractCode = value;
                _defaultInsurerContractKey = IdentifyContract(_defaultInsurerContractCode);
            }
        }

        /// <summary>
        ///The InsuererContractKey using when one is not provided.
        /// Updated in the DefaultInsurerContractCode property   
        /// </summary>
        public int DefaultInsurerContractKey
        {
            get { return _defaultInsurerContractKey; }
        }

        /// <summary>
        /// Sum of Payments linked to a claim with a closed invoice
        /// </summary>
        public double SumIdentifiedPayments
        {
            get { return _sumIdentifiedPayments; }
            set { _sumIdentifiedPayments = RoundAmount(Convert.ToDouble(value)); }
        }

        /// <summary>
        /// Sum of payments that cannot be linked to a claim
        /// </summary>
        public double SumUnidentifedPayments
        {
            get { return _sumUnidentifedPayments; }
            set { _sumUnidentifedPayments = RoundAmount(Convert.ToDouble(value)); }
        }

        /// <summary>
        /// Number of Payments linked to a claim with a closed invoice.
        /// </summary>
        public int CountIdentifiedPayments
        {
            get { return _countIdentifiedPayments; }
            set { _countIdentifiedPayments = value; }
        }

        /// <summary>
        /// Number of Payments that cannot be linked to a claim;
        /// May include payments already posted in the database.
        /// </summary>
        public int CountUnidentifiedPayments
        {
            get { return _countUnidentifiedPayments; }
            set { _countUnidentifiedPayments = value; }
        }

        /// <summary>
        /// Sum of payments for claims with open invoices or not yet invoiced.
        /// </summary>
        public  int CountOpenCreditPayments
        {
            get { return _countOpenCreditPayments; }
            set { _countOpenCreditPayments = value; }
        }

        /// <summary>
        /// Number of payments for claims with open invoices or not yet invoiced
        /// </summary>
        public double SumOpenCreditPayments
        {
            get { return _sumOpenCreditPayments; }
            set { _sumOpenCreditPayments = RoundAmount(Convert.ToDouble(value)); }
        }

        /// <summary>
        /// Number of payments dropped due to inadequate information.
        /// </summary>
        public int NumberDroppedPayments
        {
            get { return _countDroppedPayments; }
            set { _countDroppedPayments = value; }
        }

        /// <summary>
        /// Sum of payments dropped due to inadequate information.
        /// </summary>
        public double SumDroppedPayments
        {
            get { return _sumDroppedPayments; }
            set { _sumDroppedPayments = RoundAmount(Convert.ToDouble(value)); }
        }

        /// <summary>
        /// Total payments submitted for posting
        /// </summary>
        public double SumPayments
        {
            get { return _sumPayments; }
            set { _sumPayments = RoundAmount(Convert.ToDouble(value)); }
        }

        /// <summary>
        /// Number of payments submitted for posting.
        /// </summary>
        public int CountPayments
        {
            get { return _countPayments; }
            set { _countPayments = value; }
        }

        /// <summary>
        /// The type of ledger it will be posted to
        /// </summary>
        public LedgerType LedgerType
        {
            get { return _ledgerType; }
            set { _ledgerType = value; }
        }

        /// <summary>
        /// Current system user
        /// </summary>
        public IUser User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>
        /// List of payments submitted for posting
        /// </summary>
        public IList<IPayment> Payments
        {
            get { return _payments; }
            set { _payments = value; }
        }

        /// <summary>
        /// List of dropped payments.
        /// </summary>
        public IList<IPayment> DroppedPayments
        {
            get { return _droppedPayments; }
            set { _droppedPayments = value; }
        }


    }
    
}
