using System;
using System.Collections.Generic;
using AimHealth.Safari.BulkPosting.Data;
using AimHealth.Safari.BulkPosting.DTO;
using AimHealth.Safari.BulkPosting.Exceptions;
using System.Text;

namespace AimHealth.Safari.BulkPosting.Bus.BatchRepository
{

    public interface IBatch
    {
        BatchHeaderDTO GetPayerBatch(int batchHeaderKey);

        IList<PaymentDTO> IdentifyClaims(IList<PaymentDTO> paymentList);

        BatchHeaderDTO IdentifyTransactionTypes(IList<PaymentDTO> paymentList);

        int IdentifyContract(string insurerContractCode);

        int CreateBatch(string ledgerType, BatchHeaderDTO batchHeaderDTO);

        IList<ITransaction> Transactions { get; set; }
        IList<IUnidentifiedMoney> UnidentifiedMoneys { get; set; }
        IList<IUnidentifiedMoney> OpenCredits { get; set; }

        IList<string> ReturnListOfPaymentsThatWillBeOpenCredits();

        IList<string> ReturnListOfPaymentsThatWillBeUnidentifiedMoneys();
    }

    public class Batch : IBatch
    {

        private readonly IDataAccess _dataAccess;
        internal IList<ITransaction> _transactions = new List<ITransaction>();
        internal IList<IUnidentifiedMoney> _unidentifiedMoneys = new List<IUnidentifiedMoney>();
        internal IList<IUnidentifiedMoney> _openCredits = new List<IUnidentifiedMoney>();
        internal BatchHeaderDTO _batchHeaderDTO = new BatchHeaderDTO();

        public Batch(): this(new DataAccess())
        {
            
        }
        internal Batch(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public IList<PaymentDTO> IdentifyClaims(IList<PaymentDTO> paymentList)
        {
            IList<PaymentDTO> newPaymentList = new List<PaymentDTO>();
            PaymentDTO newPaymentDTO;

            foreach (PaymentDTO paymentDTO in paymentList)
            {
                try
                {
                    newPaymentDTO = _dataAccess.IdentifyClaim(paymentDTO);
                }
                catch(Exception ex)
                {
                    newPaymentDTO = paymentDTO;
                    StringBuilder str = new StringBuilder(paymentDTO.Note);
                    str.AppendFormat("Error identifying claim: {0}", GetClaimIdentifier(paymentDTO));
                    str.AppendLine(ex.Message);
                    newPaymentDTO.Note = str.ToString();
                }
                if (newPaymentDTO == null)
                {
                    newPaymentDTO = paymentDTO;
                }
                newPaymentList.Add(newPaymentDTO);
            }

            return newPaymentList;
        }

        public int IdentifyContract(string insurerContractCode)
        {
            return _dataAccess.GetContractKey(insurerContractCode);
        }

        public BatchHeaderDTO GetPayerBatch(int batchHeaderKey)
        {

            BatchHeaderDTO batchHeaderDTO = _dataAccess.GetBatchHeader(
                "PayerAccountsReceivable", batchHeaderKey);

            return batchHeaderDTO;

        }

        public BatchHeaderDTO IdentifyTransactionTypes(IList<PaymentDTO> paymentList)
        {
            _transactions.Clear();
            _unidentifiedMoneys.Clear();
            _openCredits.Clear();

            BatchHeaderDTO batchHeaderDTO = new BatchHeaderDTO();

            IList<PaymentDTO> futureTransactions = new List<PaymentDTO>();
            IList<PaymentDTO> futureUnidentifiedMoneys = new List<PaymentDTO>();
            IList<PaymentDTO> futureOpenCredits = new List<PaymentDTO>();

            foreach (PaymentDTO payment in paymentList)
            {
                batchHeaderDTO.CountPayments += 1;
                batchHeaderDTO.SumPayments += payment.Amount;

                if (payment.ClaimHeaderKey > 0)
                {
                    if (payment.IsInvoiceClosed)
                    {
                        futureTransactions.Add(payment);
                        batchHeaderDTO.SumCommissions += payment.Amount;
                        batchHeaderDTO.CountCommissions += 1;
                    }
                    else
                    {
                        batchHeaderDTO.SumOpenCredits += payment.Amount;
                        batchHeaderDTO.CountOpenCredits += 1;
                        futureOpenCredits.Add(payment);
                    }
                }
                else
                {
                    batchHeaderDTO.SumUnidentifieds += payment.Amount;
                    batchHeaderDTO.CountUnidentifieds += 1;
                    futureUnidentifiedMoneys.Add(payment);
                }
            }

            _unidentifiedMoneys = CreateUnidentifiedMoneyFromPayments(futureUnidentifiedMoneys, false);
            _openCredits = CreateUnidentifiedMoneyFromPayments(futureOpenCredits, true);
            _transactions = CreateTransactionsFromPayments(futureTransactions, TransactionCode.COM, DateTime.Now, DateTime.Now, 0);

            return batchHeaderDTO;
        }

        public IList<string> ReturnListOfPaymentsThatWillBeOpenCredits()
        {
            IList<string> resultList = new List<string>();
            foreach (IUnidentifiedMoney openCredit in OpenCredits)
            {
                resultList.Add(openCredit.Reference);
            }
            return resultList;
        }

        public IList<string> ReturnListOfPaymentsThatWillBeUnidentifiedMoneys()
        {
            IList<string> resultList = new List<string>();
            foreach (IUnidentifiedMoney unidentified in UnidentifiedMoneys)
            {
                resultList.Add(unidentified.Reference);
            }
            return resultList;
        }

        public int CreateBatch(string ledgerType, BatchHeaderDTO batchHeaderDTO)
        {
            _batchHeaderDTO = batchHeaderDTO;
            int batchHeaderKey = 0;
            if (batchHeaderDTO.Key == 0)
            {
                batchHeaderKey = CreateBatchHeader(ledgerType
                        , TransactionType.P, DateTime.Now, batchHeaderDTO.UserAdded);
                batchHeaderDTO.Key = batchHeaderKey;
            }
            else
            {
                batchHeaderKey = batchHeaderDTO.Key;
            }   

            if (!EvaluateResult(batchHeaderKey))
            { throw new BatchNotCreatedException("Error saving batch."); }

            bool result = CreateBatchDetails(ledgerType);
            if (!result)
            { throw new BatchNotCreatedException("Error saving batch details."); }

            return batchHeaderKey;
        }




        private bool CreateBatchDetails(string ledgerType)
        {
            foreach (Transaction transaction in _transactions)
            {
                int batchDetailKey = CreateBatchDetail(ledgerType, transaction);
                if (EvaluateResult(batchDetailKey))
                {
                    transaction.Key = batchDetailKey;
                }
                else
                {
                    string message = CreateMessage("Error creating batch detail for {0}", transaction.ClaimHeaderKey);
                    throw new BatchNotCreatedException(message);
                }
            }

            foreach (UnidentifiedMoney unidentified in _unidentifiedMoneys)
            {
                int undKey = CreateUnidentified(ledgerType, unidentified);
                if (EvaluateResult(undKey))
                {
                    unidentified.Key = undKey;
                }
                else
                {
                    string message = CreateMessage("Error creating unidentified for {0}", unidentified.Reference);
                    throw new BatchNotCreatedException(message);
                }
                int batchDetailKey = CreateBatchDetail(ledgerType, unidentified);
                if (EvaluateResult(batchDetailKey))
                {
                    unidentified.Key = batchDetailKey;
                }
                else
                {
                    string message = CreateMessage("Error creating batchdetail for {0}", unidentified.Reference);
                    throw new BatchNotCreatedException(message);
                }
            }

            foreach (UnidentifiedMoney openCredit in _openCredits)
            {
                int openCreditKey = CreateOpenCredit(ledgerType, openCredit);
                if (EvaluateResult(openCreditKey))
                {
                    openCredit.Key = openCreditKey;
                }
                else
                {
                    string message = CreateMessage("Error creating open credit for {0}", openCredit.Reference);
                    throw new BatchNotCreatedException(message);
                }
                int batchDetailKey = CreateBatchDetail(ledgerType, openCredit);
                if (EvaluateResult(batchDetailKey))
                {
                    openCredit.Key = batchDetailKey;
                }
                else
                {
                    string message = CreateMessage("Error creating batch detail for {0}", openCredit.Reference);
                    throw new BatchNotCreatedException(message);
                }
            }
            return true;
        }


        private static IList<IUnidentifiedMoney> CreateUnidentifiedMoneyFromPayments(
            // ReSharper disable SuggestBaseTypeForParameter
            IList<PaymentDTO> paymentDtos
            // ReSharper restore SuggestBaseTypeForParameter
            , bool isOpenCredit
            )
        {
            if (paymentDtos == null) throw new ArgumentNullException("paymentDtos");
            IList<IUnidentifiedMoney> transactions = new List<IUnidentifiedMoney>();
            foreach (PaymentDTO paymentDTO in paymentDtos)
            {
                string referenceId = ResolveReferenceID(paymentDTO.AimClaimId
                                                        , paymentDTO.ClientClaimId
                                                        , paymentDTO.ClientUniqueIdentifier);

                IUnidentifiedMoney transaction = new UnidentifiedMoney(
                    0
                    , paymentDTO.InsurerContractKey
                    , paymentDTO.Amount
                    , referenceId
                    , isOpenCredit
                    );
                transactions.Add(transaction);
            }
            return transactions;
        }

        private static string ResolveReferenceID(string aimClaimId, string clientClaimId, string clientUniqueIdentifier)
        {
            string result;
            if (aimClaimId != null) result = aimClaimId;
            else if (clientClaimId != null) { result = clientClaimId; }
            else if (clientUniqueIdentifier != null) { result = clientUniqueIdentifier; }
            else result = "Unidentified";
            return result;
        }

        private static IList<ITransaction> CreateTransactionsFromPayments(
            // ReSharper disable SuggestBaseTypeForParameter
            IList<PaymentDTO> paymentDtos
            // ReSharper restore SuggestBaseTypeForParameter
            , TransactionCode transactionCode
            , DateTime transactionDate
            , DateTime transactionARDate
            , int unidentifiedMoneyKey)
        {
            if (paymentDtos == null) throw new ArgumentNullException("paymentDtos");
            IList<ITransaction> transactions = new List<ITransaction>();
            foreach (PaymentDTO paymentDTO in paymentDtos)
            {
                ITransaction transaction = new Transaction(
                    0
                    , transactionCode
                    , paymentDTO.ClaimHeaderKey
                    , paymentDTO.Amount
                    , transactionDate
                    , transactionARDate
                    , null
                    , unidentifiedMoneyKey
                    );
                transactions.Add(transaction);
            }
            return transactions;
        }


        private int CreateBatchHeader(string ledgerType, TransactionType transactionType, DateTime dateCreated, string userLogin)
        {
            int _return = _dataAccess.CreateBatchHeader(ledgerType, transactionType.ToString(), dateCreated, userLogin);
            return _return;
        }

        private static string CreateMessage(string message, params object[] list)
        {
            string textToSend = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                {
                    string placeHolder = "{" + i + "}";
                    textToSend = message.Replace(placeHolder, list[i].ToString());
                }
            }
            return textToSend;
        }

        
        /// <summary>
        /// Create an open credit record
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="openCredit"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        private int CreateOpenCredit(string ledgerType, IUnidentifiedMoney openCredit)
        {
            // ReSharper disable RedundantAssignment
            int result = 0;
            // ReSharper restore RedundantAssignment
            UnidentifiedDTO unidentifiedDTO = new UnidentifiedDTO(openCredit.ContractKey
                , openCredit.Reference, openCredit.Amount, openCredit.IsOpenCredit, _batchHeaderDTO.UserAdded);

            result = _dataAccess.CreateUnidentified(ledgerType
              , unidentifiedDTO);

            return result;
        }


        /// <summary>
        /// Create the Batch Detail for an unidentifed claim or a claim not ready for fee payment
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        private int CreateBatchDetail(string ledgerType, IUnidentifiedMoney money)
        {
            TransactionCode transactionCode = money.IsOpenCredit ? TransactionCode.OC : TransactionCode.UND;

            BatchDetailDTO batchDetailDTO = new BatchDetailDTO(
                _batchHeaderDTO.Key
                , 0, 0, money.Key
                , TransactionType.P.ToString(), TransactionCategory.PYMT.ToString()
                , transactionCode.ToString(), DateTime.Now
                , money.Amount, _batchHeaderDTO.PayerCheckAmount
                , _batchHeaderDTO.PayerCheckDate, _batchHeaderDTO.PayerCheckNumber
                , _batchHeaderDTO.UserAdded
                );

            int result = _dataAccess.CreateBatchDetail(
                TransactionChoice.WithTransaction
                , ledgerType
                , batchDetailDTO);

            return result;
        }

        /// <summary>
        /// Create an unidentifed record
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="unidentified"></param>
        /// <returns></returns>
        private int CreateUnidentified(string ledgerType, IUnidentifiedMoney unidentified)
        {
            // ReSharper disable RedundantAssignment
            int result = 0;
            // ReSharper restore RedundantAssignment
            UnidentifiedDTO unidentifiedDTO = new UnidentifiedDTO(unidentified.ContractKey
                , unidentified.Reference, unidentified.Amount, unidentified.IsOpenCredit, _batchHeaderDTO.UserAdded);

            result = _dataAccess.CreateUnidentified(ledgerType
              , unidentifiedDTO);

            return result;
        }


        /// <summary>
        /// Creates the Batch Detail for an identified claim ready for fee payment.
        /// </summary>
        /// <param name="ledgerType"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        private int CreateBatchDetail(string ledgerType, ITransaction payment)
        {
            int result;
            BatchDetailDTO batchDetailDTO = new BatchDetailDTO(
                _batchHeaderDTO.Key
                , payment.ClaimHeaderKey, 0, payment.UnidentifiedKey
                , payment.TransactionType.ToString(), payment.TransactionCategory.ToString()
                , payment.TransactionCode.ToString(), payment.ArDate
                , payment.Amount, _batchHeaderDTO.PayerCheckAmount
                , _batchHeaderDTO.PayerCheckDate
                , _batchHeaderDTO.PayerCheckNumber
                , _batchHeaderDTO.UserAdded
                );
            try
            {
                result = _dataAccess.CreateBatchDetail(
                    AimHealth.Safari.BulkPosting.Data.TransactionChoice.WithTransaction
                    , ledgerType
                    , batchDetailDTO);
                if (!EvaluateResult(result))
                {
                    string message = CreateMessage("Error creating batch detail for {0}", batchDetailDTO.ClaimHeaderKey);
                    throw new BatchNotCreatedException(message);
                }

            }
            catch (Exception ex)
            {
                string message = CreateMessage("Error creating batch detail for {0}", batchDetailDTO.ClaimHeaderKey);
                throw new BatchNotCreatedException(message, ex);
            }
            return result;
        }


        private static bool EvaluateResult(int result)
        {
            bool eval = result > 0;
            return eval;
        }


        private static string GetClaimIdentifier(PaymentDTO paymentDTO)
        {
            string result = "";
            bool areDone = false;
            if (!string.IsNullOrEmpty(paymentDTO.AimClaimId))
            {
                result = paymentDTO.AimClaimId;
                areDone = true;
            }
            if (!areDone & !string.IsNullOrEmpty(paymentDTO.ClientUniqueIdentifier))
            {
                result = paymentDTO.ClientUniqueIdentifier;
                areDone = true;
            }
            if (!areDone & !string.IsNullOrEmpty(paymentDTO.ClientClaimId))
            {
                result = paymentDTO.ClientClaimId;
                areDone = true;
            }
            if (!areDone & !string.IsNullOrEmpty(paymentDTO.ClaimHeaderKey.ToString()))
            {
                result = paymentDTO.ClaimHeaderKey.ToString();
            }
            return result;
        }


        public IList<ITransaction> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }

        public IList<IUnidentifiedMoney> UnidentifiedMoneys
        {
            get { return _unidentifiedMoneys; }
            set { _unidentifiedMoneys = value; }
        }

        public IList<IUnidentifiedMoney> OpenCredits
        {
            get { return _openCredits; }
            set { _openCredits = value; }
        }

    }
}
