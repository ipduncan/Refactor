

using System;

namespace AimHealth.Safari.BulkPosting.DTO
{
    public class PaymentDTO
    {
        private readonly string _aimClaimId;
        private readonly string _clientUniqueIdentifier;
        private readonly string _clientClaimId;
        private string _insurerContractCode;
        private string _insurerCode;
        private readonly double _amount;
        private int _claimHeaderKey;
        private int _insurerContractKey;
        private double _invoiceBalanceDue;
        private double _refundRecoveredAmount;
        private int _invoiceKey;
        private bool _isInvoiceClosed;
        private string _note;

        public PaymentDTO(string aimClaimID, string clientUniqueIdentifier
            , string clientClaimID, string insurerContractCode
            , string insurerCode, double amount
            )
        {
            _aimClaimId = aimClaimID;
            _clientUniqueIdentifier = clientUniqueIdentifier;
            _clientClaimId = clientClaimID;
            _insurerContractCode = insurerContractCode;
            _insurerCode = insurerCode;
            _amount = amount;
        }

        public PaymentDTO(string aimClaimID, string clientUniqueIdentifier
            , string clientClaimID, string insurerContractCode
            , string insurerCode, double amount
            , int claimHeaderKey, int insurerContractKey
            , double invoiceBalanceDue, double refundRecoveredAmount
            , int invoiceKey, bool isInvoiceClosed
            , string note
            )
        {
            _aimClaimId = aimClaimID;
            _clientUniqueIdentifier = clientUniqueIdentifier;
            _clientClaimId = clientClaimID;
            _insurerContractCode = insurerContractCode;
            _insurerCode = insurerCode;
            _amount = amount;
            _claimHeaderKey = claimHeaderKey;
            _insurerContractKey = insurerContractKey;
            _invoiceBalanceDue = invoiceBalanceDue;
            _refundRecoveredAmount = refundRecoveredAmount;
            _invoiceKey = invoiceKey;
            _isInvoiceClosed = isInvoiceClosed;
            _note = note;
        }


        public string AimClaimId
        {
            get { return _aimClaimId; }
        }

        public string ClientUniqueIdentifier
        {
            get { return _clientUniqueIdentifier; }
        }

        public string ClientClaimId
        {
            get { return _clientClaimId; }
        }

        public string InsurerContractCode
        {
            get { return _insurerContractCode; }
            set { _insurerContractCode = value; }
        }

        public string InsurerCode
        {
            get { return _insurerCode; }
            set { _insurerCode = value; }
        }

        public double Amount
        {
            get { return _amount; }
        }

        public int ClaimHeaderKey
        {
            get { return _claimHeaderKey; }
            set { _claimHeaderKey = value; }
        }

        public int InsurerContractKey
        {
            get { return _insurerContractKey; }
            set { _insurerContractKey = value; }
        }

        public double InvoiceBalanceDue
        {
            get { return _invoiceBalanceDue; }
            set { _invoiceBalanceDue = value; }
        }

        public double RefundRecoveredAmount
        {
            get { return _refundRecoveredAmount; }
            set { _refundRecoveredAmount = value; }
        }

        public int InvoiceKey
        {
            get { return _invoiceKey; }
            set { _invoiceKey = value; }
        }

        public bool IsInvoiceClosed
        {
            get { return _isInvoiceClosed; }
            set { _isInvoiceClosed = value; }
        }

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

    }
}
