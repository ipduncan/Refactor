using System;

namespace AimHealth.Safari.BulkPosting.DTO
{
    public class BatchHeaderDTO
    {
        private int _key {get;set;}
        private DateTime? _arDate { get; set; }
        private string _userAdded {get;set;}
        private DateTime? _dateAdded { get; set; }
        private double _sumPayments { get; set; }
        private int _countPayments { get; set; }
        private string _payerCheckNumber { get; set; }
        private double _payerCheckAmount { get; set; }
        private DateTime? _payerCheckDate { get; set; }
        private double _sumCommissions { get; set; }
        private int _countCommissions { get; set; }
        private double _sumUnidentifieds { get; set; }
        private int _countUnidentifieds { get; set; }
        private double _sumOpenCredits { get; set; }
        private int _countOpenCredits { get; set; }

        public BatchHeaderDTO()
        {
        }

        public BatchHeaderDTO(
                int key
                , DateTime? arDate
                , string userAdded
                , DateTime? dateAdded
                , double sumPayments
                , int countPayments
                , string payerCheckNumber
                , double payerCheckAmount
                , DateTime? payerCheckDate
                , double sumCommissions
                , int countCommissions
                , double sumUnidentifieds
                , int countUnidentifieds
                , double sumOpenCredits
                , int countOpenCredits
            )
        {
            _key = key;
            _arDate = arDate;
            _userAdded = userAdded;
            _dateAdded = dateAdded;
            _sumPayments = sumPayments;
            _countPayments = countPayments;
            _payerCheckNumber = payerCheckNumber;
            _payerCheckAmount = payerCheckAmount;
            _payerCheckDate = payerCheckDate;
            _sumCommissions = sumCommissions;
            _countCommissions = countCommissions;
            _sumUnidentifieds = sumUnidentifieds;
            _countUnidentifieds = countUnidentifieds;
            _sumOpenCredits = sumOpenCredits;
            _countOpenCredits = countOpenCredits;
        }

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public DateTime? ArDate
        {
            get { return _arDate; }
            set { _arDate = value; }
        }

        public string UserAdded
        {
            get { return _userAdded; }
            set { _userAdded = value; }
        }

        public DateTime? DateAdded
        {
            get { return _dateAdded; }
            set { _dateAdded = value; }
        }

        public double SumPayments
        {
            get { return _sumPayments; }
            set { _sumPayments = value; }
        }

        public int CountPayments
        {
            set { _countPayments = value; }
            get { return _countPayments; }
        }

        public string PayerCheckNumber
        {
            get { return _payerCheckNumber; }
            set { _payerCheckNumber = value; }
        }

        public double PayerCheckAmount
        {
            get { return _payerCheckAmount; }
            set { _payerCheckAmount = value; }
        }

        public DateTime? PayerCheckDate
        {
            get { return _payerCheckDate; }
            set { _payerCheckDate = value; }
        }

        public double SumCommissions
        {
            get { return _sumCommissions; }
            set { _sumCommissions = value; }
        }

        public int CountCommissions
        {
            get { return _countCommissions; }
            set { _countCommissions = value; }
        }

        public double SumUnidentifieds
        {
            set { _sumUnidentifieds = value; }
            get { return _sumUnidentifieds; }
        }

        public int CountUnidentifieds
        {
            set { _countUnidentifieds = value; }
            get { return _countUnidentifieds; }
        }

        public double SumOpenCredits
        {
            get { return _sumOpenCredits; }
            set { _sumOpenCredits = value; }
        }

        public int CountOpenCredits
        {
            get { return _countOpenCredits; }
            set { _countOpenCredits = value; }
        }

    }
}
