using System;
using System.Collections.Specialized;
using System.Text;
using AimHealth.Safari.BulkPosting.Exceptions;

namespace AimHealth.Safari.BulkPosting.Bus
{
    public interface IPayment
    {
        string AimClaimId { get; }
        double Amount { get; }
        string ClientUniqueIdentifier { get; }
        string ClientClaimId { get; }
        int ClaimHeaderKey { get; }
        int InsurerContractKey { get; set; }
        string InsurerContractCode { get; }
        string InsurerCode { get; set; }
        double InvoiceBalanceDue { get; }
        double RefundRecoveredAmount { get; }
        int InvoiceKey { get; }
        bool IsInvoiceClosed { get; }
        string Note { get; set; }
        StringCollection GetListOfProperties();
        void UpdateInformation(int claimHeaderKey, int invoiceKey, bool isInvoiceClosed
            , int insurerContractKey, double invoiceBalanceDue, double refundRecoveredAmount);

        void SetProperty(string property, string value);

        string GetClaimIdentifier();
    }

    public class Payment : IPayment
    {

        private string _aimClaimID;
        private double _amount;
        private string _clientUniqueIdentifier;     //UID; RecordID; InsClaimID
        private string _clientClaimID;              //PayerClaimID; TxReference
        private string _insurerContractCode;
        private string _insurerCode;
        private int _invoiceKey;
        private bool _isInvoiceClosed;
        private int _insurerContractKey;
        private double _invoiceBalanceDue;
        private double _refundRecoveredAmount;
        private int _claimHeaderKey;
// ReSharper disable FieldCanBeMadeReadOnly.Local
        private StringBuilder _note = new StringBuilder();
// ReSharper restore FieldCanBeMadeReadOnly.Local


        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public Payment()
        {
        }

        /// <summary>
        /// Payment constructor
        /// </summary>
        /// <param name="aimClaimId"></param>
        /// <param name="amount"></param>
        /// <param name="clientUniqueIdentifier"></param>
        /// <param name="clientClaimId"></param>
        /// <param name="insurerContractCode"></param>
        /// <param name="insurerCode"></param>
        public Payment(string aimClaimId, double amount, string clientUniqueIdentifier
            , string clientClaimId, string insurerContractCode, string insurerCode)
        {
            _aimClaimID = aimClaimId;
            _amount = RoundAmount(amount);
            _clientUniqueIdentifier = clientUniqueIdentifier;
            _clientClaimID = clientClaimId;
            _insurerContractCode = insurerContractCode;

            if ((string.IsNullOrEmpty(insurerCode)) & insurerContractCode != null)
            {
                _insurerCode = ExtractInsurerCodeFromContractCode(insurerContractCode);
            }
            else
            {
                VerifyInsurerCodeAgainstContractCode(insurerContractCode, insurerCode);
                _insurerCode = insurerCode;
            }
        }

        
        /// <summary>
        /// Returns a list of Properties the user can map Excel columns to.  Used
        /// in Safari.BulkPosting application.
        /// </summary>
        /// <returns></returns>
        public StringCollection GetListOfProperties()
        {
            //todo needs to use refactoring to get this information instead
            StringCollection props = new StringCollection
                                         {
                                             "AimClaimId",
                                             "Amount",
                                             "ClientUniqueIdentifier",
                                             "RecordID",
                                             "UID",
                                             "InsClaimID",
                                             "ClientClaimID",
                                             "PayerClaimID",
                                             "TxReference",
                                             "VendorReferenceID",
                                             "InsurerContractCode",
                                             "InsurerCode"
                                         };
            return props;

        }

        public void SetProperty(string property, string value)
        {
            switch (property.ToLower())
            {
                case "aimclaimid":
                    _aimClaimID = value;
                    break;
                case "clientuniqueidentifier":
                case "recordid":
                case "uid":
                case "insclaimid":
                    _clientUniqueIdentifier = value;
                    break;
                case "clientclaimid":
                case "payerclaimid":
                case "txreference":
                case "vendorreferenceid":
                    _aimClaimID = ExtractAimClaimIdFromVendorId(value);
                    _claimHeaderKey = ExtractClaimHeaderKeyFromVendorId(value);
                    break;
                case "amount":
                    if (value.Length > 0)
                        {_amount = RoundAmount(Convert.ToDouble(value));}
                    break;
                case "insurercontractcode":
                    _insurerContractCode = value;
                    _insurerCode = ExtractInsurerCodeFromContractCode(value);
                    break;
                case "insurercode":
                    _insurerCode = value;
                    break;
                default:
                    InvalidPaymentPropertyException ex = new InvalidPaymentPropertyException();
                    ExceptionHandler.ThrowException(ex, "Invalid Payment Property:{0}", property);
                    break;
            }
        }

        private int ExtractClaimHeaderKeyFromVendorId(string value)
        {
            int pos = value.IndexOf("_");
            if (pos > 0)
            {
                return Int32.Parse(value.Substring(0, pos));
            }
            else
            {
                return 0;
            }
        }

        private string ExtractAimClaimIdFromVendorId(string value)
        {
            int pos = value.IndexOf("_");
            if (pos > 0)
            {
                try
                {
                    return value.Substring(pos + 1, value.Length - pos - 1);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// Updates the payment information after the claim has been identified.
        /// </summary>
        /// <param name="claimHeaderKey"></param>
        /// <param name="invoiceKey"></param>
        /// <param name="isInvoiceClosed"></param>
        /// <param name="insurerContractKey"></param>
        /// <param name="invoiceBalanceDue"></param>
        /// <param name="refundRecoveredAmount"></param>
        public void UpdateInformation(int claimHeaderKey, int invoiceKey, bool isInvoiceClosed
            , int insurerContractKey, double invoiceBalanceDue, double refundRecoveredAmount)
        {
            _claimHeaderKey = claimHeaderKey;
            _isInvoiceClosed = isInvoiceClosed;
            _invoiceKey = invoiceKey;
            _insurerContractKey= insurerContractKey;
            _invoiceBalanceDue = RoundAmount(invoiceBalanceDue);
            _refundRecoveredAmount = RoundAmount(refundRecoveredAmount);
        }


        /// <summary>
        /// Rounds inputs to reduce errors
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static double RoundAmount(double amount)
        {
            //round, if necessary
            //if (amount. )
            amount = Math.Round(amount, 2);
            return amount;
        }

        private static string ExtractInsurerCodeFromContractCode(string insurerContractCode)
        {
            if (insurerContractCode.Length > 0)
                return insurerContractCode.Substring(0, 5);
            else
            {
                return "";
            }
        }

        private static void VerifyInsurerCodeAgainstContractCode(string insurerContractCode, string insurerCode)
        {
            
            if (!string.IsNullOrEmpty(insurerContractCode) & !string.IsNullOrEmpty(insurerCode)) 
            {
                if (ExtractInsurerCodeFromContractCode(insurerContractCode) != insurerCode)
                {throw new InvalidDataInsurerInformationException(
                    "Invalid combination of Insurer Code and Contract Code");}
            }

        }

        public string GetClaimIdentifier()
        {
            string result = "";
            bool areDone = false;
            if (!string.IsNullOrEmpty(AimClaimId))
            {
                result = AimClaimId;
                areDone = true;
            }
            if (!areDone & !string.IsNullOrEmpty(ClientUniqueIdentifier))
            {
                result = ClientUniqueIdentifier;
                areDone = true;
            }
            if (!areDone & !string.IsNullOrEmpty(ClientClaimId))
            {
                result = ClientClaimId;
                areDone = true;
            }
            if (!areDone & !string.IsNullOrEmpty(ClaimHeaderKey.ToString()))
            {
                result = ClaimHeaderKey.ToString();
            }
            return result;
        }



        /// <summary>
        /// Stores the ClaimID from ProvClmHdr or InsClmHdr
        /// </summary>
        public string AimClaimId
        {
            get { return _aimClaimID; }
        }

        /// <summary>
        /// The amount of the payment transaction being posted.
        /// </summary>
        public double Amount
        {
            get { return _amount; }
        }

        /// <summary>
        /// InsClaimID, RecordID, UID; if not provided by the client; will store the ClaimID
        /// </summary>
        public string ClientUniqueIdentifier
        {
            get { return _clientUniqueIdentifier; }
        }

        /// <summary>
        /// PayerClaimID, TxReference
        /// </summary>
        public string ClientClaimId
        {
            get { return _clientClaimID; }
        }

        /// <summary>
        /// ProvClmHdrKey if preinvoice; InsClmHdrKey if postinvoice
        /// </summary>
        public int ClaimHeaderKey { get { return _claimHeaderKey;} }

        public int InsurerContractKey { get { return _insurerContractKey; } set { _insurerContractKey = value; } }

        public int InvoiceKey
        {
            get { return _invoiceKey; }
        }

        public bool IsInvoiceClosed
        {
            get { return _isInvoiceClosed; }
        }

        public string Note 
        {
            get { return _note.ToString(); }
            set
            {
                if (_note.Length > 0)
                {
                    _note.Append(" " + value);
                }
                else
                {
                    _note.Append(value);
                }
            }
        }

        public double InvoiceBalanceDue
        {
            get { return _invoiceBalanceDue; }
        }

        public double RefundRecoveredAmount
        {
            get { return _refundRecoveredAmount; }
        }

        public string InsurerCode
        {
            get { return _insurerCode; }
            set { _insurerCode = value; }
        }

        public string InsurerContractCode
        {
            get { return _insurerContractCode; }
        }


    }
}
