

namespace AimHealth.Safari.BulkPosting.DTO
{
    public class ClaimDTO
    {

        private readonly int _key;
        private readonly int _invoiceKey;
        private readonly bool _isInvoiceClosed;
        private readonly int _insurerContractKey;
        private readonly double _invoiceBalanceDue;
        private readonly double _refundRecoveredAmount;
        private readonly string _note;

        public ClaimDTO()
        {
        }

        public ClaimDTO(int key, int invoiceKey, bool isInvoiceClosed, int insurerContractKey
            , double invoiceBalanceDue, double refundRecoveredAmount, string note)
        {
            _key = key;
            _invoiceKey = invoiceKey;
            _isInvoiceClosed = isInvoiceClosed;
            _insurerContractKey = insurerContractKey;
            _invoiceBalanceDue = invoiceBalanceDue;
            _refundRecoveredAmount = refundRecoveredAmount;
            _note = note;
        }

        public int Key
        {
            get { return _key; }
        }

        public int InvoiceKey
        {
            get { return _invoiceKey; }
        }

        public bool IsInvoiceClosed
        {
            get { return _isInvoiceClosed; }
        }

        public int InsurerContractKey
        {
            get { return _insurerContractKey; }
        }

        public double InvoiceBalanceDue
        {
            get { return _invoiceBalanceDue; }
        }

        public double RefundRecoveredAmount
        {
            get { return _refundRecoveredAmount; }
        }

        public string Note
        {
            get { return _note; }
        }
    }
}
