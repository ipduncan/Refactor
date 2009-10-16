using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AimHealth.Safari.BulkPosting.DTO
{
    public class UnidentifiedDTO
    {
        private int _insurerContractKey;
        private string _reference;
        private double _amount;
        private bool _isOpenCredit;
        private string _userAdded;

        public UnidentifiedDTO(int insurerContractKey, string reference, double amount, bool isOpenCredit, string userAdded)
        {
            _insurerContractKey = insurerContractKey;
            _reference = reference;
            _amount = amount;
            _isOpenCredit = isOpenCredit;
            _userAdded = userAdded;
        }

        public int InsurerContractKey
        {
            get { return _insurerContractKey; }
        }

        public string Reference
        {
            get { return _reference; }
        }

        public double Amount
        {
            get { return _amount; }
        }

        public bool IsOpenCredit
        {
            get { return _isOpenCredit; }
        }

        public string UserAdded
        {
            get { return _userAdded; }
        }
    }
}
