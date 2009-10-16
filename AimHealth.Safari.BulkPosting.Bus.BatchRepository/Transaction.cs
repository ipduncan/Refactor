using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AimHealth.Safari.BulkPosting.Bus.BatchRepository
{
    #region Enumerations
    public enum TransactionType
    { P }

    public enum TransactionCode
    { COM, UND, OC }

    public enum TransactionCategory
    { PYMT }

    #endregion

    #region Interface

    public interface ITransaction
    {
        int Key { get; set; }
        TransactionType TransactionType { get; set; }
        TransactionCategory TransactionCategory { get; set; }
        TransactionCode TransactionCode { get; set; }
        int ClaimHeaderKey { get; set; }
        double Amount { get; set; }
        DateTime TransactionDate { get; set; }
        DateTime ArDate { get; set; }
        DateTime? PostDate { get; set; }
        int UnidentifiedKey { get; set; }
        IEnumerator GetEnumerator();
    }

    #endregion

    public class Transaction : ITransaction
    {

        #region Variables

        private int _key = 0;
        private int _claimHeaderKey = 0;
        private double _amount = 0;
        private DateTime _transactionDate;
        private DateTime _arDate;
        private DateTime? _postDate = null;
        private int _unidentifiedKey = 0;

        private TransactionType _transactionType = new TransactionType();
        private TransactionCategory _transactionCategory = new TransactionCategory();
        private TransactionCode _transactionCode = new TransactionCode();

        #endregion

        #region Constructors

        public Transaction()
        {
        }

        public Transaction(int key, TransactionCode code, int claimHeaderKey, double amount, DateTime transactionDate, DateTime arDate, DateTime? postDate, int unidentifiedKey)
        {
            _key = key;
            SetMembers(code);
            _claimHeaderKey = claimHeaderKey;
            _amount = amount;
            _transactionDate = transactionDate;
            _arDate = arDate;
            _postDate = postDate;
            _unidentifiedKey = unidentifiedKey;
        }

        public Transaction(TransactionCode code, int claimHeaderKey, double amount, DateTime transactionDate, DateTime arDate, DateTime? postDate, int unidentifiedKey)
        {
            _claimHeaderKey = claimHeaderKey;
            SetMembers(code);
            _amount = amount;
            _transactionDate = transactionDate;
            _arDate = arDate;
            _postDate = postDate;
            _unidentifiedKey = unidentifiedKey;

        }

        #endregion

        #region Methods

        private void SetMembers(TransactionCode code)
        {

            switch (code)
            {
                case TransactionCode.COM:
                    _transactionCode = code;
                    _transactionCategory = TransactionCategory.PYMT;
                    _transactionType = TransactionType.P;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Properties

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        public TransactionCategory TransactionCategory
        {
            get { return _transactionCategory; }
            set { _transactionCategory = value; }
        }

        public TransactionCode TransactionCode
        {
            get { return _transactionCode; }
            set { _transactionCode = value; }
        }



        public int ClaimHeaderKey
        {
            get { return _claimHeaderKey; }
            set { _claimHeaderKey = value; }
        }

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }

        public DateTime ArDate
        {
            get { return _arDate; }
            set { _arDate = value; }
        }

        public DateTime? PostDate
        {
            get { return _postDate; }
            set { _postDate = value; }
        }

        public int UnidentifiedKey
        {
            get { return _unidentifiedKey; }
            set { _unidentifiedKey = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        #endregion

    }
}
