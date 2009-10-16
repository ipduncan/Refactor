using System;


namespace AimHealth.Safari.BulkPosting.DTO
{
    public class BatchDetailDTO
    {
        private readonly int _batchHeaderKey;
        private readonly int _claimHeaderKey;
        private readonly int _correctionKey;
        private readonly int _unidentifiedKey;
        private readonly string _transactionType;
        private readonly string _transactionCategory;
        private readonly string _transactionCode;
        private readonly DateTime? _batchTransactionDate;
        private readonly double _batchTransactionAmount;
        private readonly double _payerCheckAmount ;
        private readonly DateTime? _payerCheckDate;
        private readonly string _payerCheckNumber;
        private readonly bool _isPayerBulkCheck ;
        private readonly bool _isUnidentified ;
        private readonly string _userAdded;

        public BatchDetailDTO(int batchHeaderKey, int claimHeaderKey, int correctionKey
            , int unidentifiedKey, string transactionType, string transactionCategory
            , string transactionCode, DateTime? batchTransactionDate
            , double batchTransactionAmount, double payerCheckAmount
            , DateTime? payerCheckDate, string payerCheckNumber
            , string userAdded)
        {
            _batchHeaderKey = batchHeaderKey;
            _claimHeaderKey = claimHeaderKey;
            _correctionKey = correctionKey;
            _unidentifiedKey = unidentifiedKey;
            _transactionType = transactionType;
            _transactionCategory = transactionCategory;
            _transactionCode = transactionCode;
            _batchTransactionDate = batchTransactionDate;
            _batchTransactionAmount = batchTransactionAmount;
            _payerCheckAmount = payerCheckAmount;
            _payerCheckDate = payerCheckDate;
            _payerCheckNumber = payerCheckNumber;
            if (unidentifiedKey > 0) {
                _isUnidentified = true; }
            if (_payerCheckAmount > 0) {
                _isPayerBulkCheck = true; }
            _userAdded = userAdded;
        }

        public int BatchHeaderKey
        {
            get { return _batchHeaderKey; }
        }

        public int ClaimHeaderKey
        {
            get { return _claimHeaderKey; }
        }

        public int CorrectionKey
        {
            get { return _correctionKey; }
        }

        public int UnidentifiedKey
        {
            get { return _unidentifiedKey; }
        }

        public string TransactionType
        {
            get { return _transactionType; }
        }

        public string TransactionCategory
        {
            get { return _transactionCategory; }
        }

        public string TransactionCode
        {
            get { return _transactionCode; }
        }

        public DateTime? BatchTransactionDate
        {
            get { return _batchTransactionDate; }
        }

        public double BatchTransactionAmount
        {
            get { return _batchTransactionAmount; }
        }

        public double PayerCheckAmount
        {
            get { return _payerCheckAmount; }
        }

        public DateTime? PayerCheckDate
        {
            get { return _payerCheckDate; }
        }

        public string PayerCheckNumber
        {
            get { return _payerCheckNumber; }
        }

        public bool IsPayerBulkCheck
        {
            get { return _isPayerBulkCheck; }
        }

        public bool IsUnidentified
        {
            get { return _isUnidentified; }
        }

        public string UserAdded
        {
            get { return _userAdded; }
        }
    }
}
