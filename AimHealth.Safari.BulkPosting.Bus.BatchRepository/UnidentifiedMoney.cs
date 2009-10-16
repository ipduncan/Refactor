using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AimHealth.Safari.BulkPosting.Bus.BatchRepository
{
    public interface IUnidentifiedMoney
    {
        int Key { get; set; }
        int ContractKey { get; set; }
        double Amount { get; set; }
        string Reference { get; set; }
        bool IsOpenCredit { get; set; }
    }

    public class UnidentifiedMoney : IUnidentifiedMoney
    {
        private int _key = 0;
        private int _contractKey = 0;
        private double _amount = 0;
        private string _reference = "";
        private bool _isOpenCredit;

        public UnidentifiedMoney(int key, int contractKey, double amount, string reference, bool isOpenCredit)
        {
            _key = key;
            _contractKey = contractKey;
            _amount = amount;
            _reference = reference;
            _isOpenCredit = isOpenCredit;
        }

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public int ContractKey
        {
            get { return _contractKey; }
            set { _contractKey = value; }
        }

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }
        public bool IsOpenCredit
        {
            get { return _isOpenCredit; }
            set { _isOpenCredit = value; }
        }

    }
}
