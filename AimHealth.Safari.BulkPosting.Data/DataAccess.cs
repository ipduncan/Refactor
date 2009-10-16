using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using AimHealth.Safari.BulkPosting.DTO;
using DataAccessBulkManager;

namespace AimHealth.Safari.BulkPosting.Data
{

    public class DataAccess : IDataAccess
    {
        public int GetContractKey(string insurerContractCode)
        {
            IDataManager manager;

            try
            {
                manager = SqlDataManager.UniqueInstance;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message, ex);
                throw new Exception("Problem calling SqlDataManager", ex);
            }

            ExecuteOutcome outcome;
            string outcomeMessage;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Select ContractKey From dbo.InsContract Where ContractCode = '{0}' and Inactive = 0",
                             insurerContractCode);
            int result = 0;
            try
            {
                result = (int) manager.Execute(
                                       TransactionChoice.WithoutTransaction
                                       , CommandType.Text
                                       , sql.ToString()
                                       , ExecuteType.ExecuteScaler
                                       , null
                                       , out outcome
                                       , out outcomeMessage);
                EvaluateOutcome(outcome, outcomeMessage, "GetContractKey");
            }
            catch(Exception ex)
            {
                result = 0;
            }

            return result;

        }

        /// <summary>
        /// Tries to tie a submitted payment to a specific claim
        /// </summary>
        /// <param name="paymentDTO"></param>
        /// <returns></returns>
        public PaymentDTO IdentifyClaim(PaymentDTO paymentDTO)
        {
            PaymentDTO newPaymentDTO;


            try
            {
                newPaymentDTO = IdentifyClaimPostInvoice(paymentDTO);
            }
            catch (Exception ex)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("Problem identifying post invoice on claim {0}", GetClaimIdentifier(paymentDTO));
                str.AppendLine(ex.Message);
                newPaymentDTO = paymentDTO;
                newPaymentDTO.Note = str.ToString();
            }

            if (newPaymentDTO == null)
            {
                try
                {
                    newPaymentDTO = IdentifyClaimPreInvoice(paymentDTO);
                }
                catch(Exception ex)
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendFormat("Problem identifying preinvoice on claim {0}", GetClaimIdentifier(paymentDTO));
                    str.AppendLine(ex.Message);
                    newPaymentDTO = paymentDTO;
                    newPaymentDTO.Note = str.ToString();
                }

                if (newPaymentDTO == null)
                {
                    //could not identify claim
                }
            }
            return newPaymentDTO;    
        }

        /// <summary>
        /// Returns the InsClmHdrKey of a claim based on provided parameters
        /// </summary>
        public PaymentDTO IdentifyClaimPostInvoice(PaymentDTO paymentDTO)
        {
            PaymentDTO newPaymentDTO = null;
            int headerKey = 0;
            IDataManager manager;
            int invoiceKey = 0;
            bool isInvoiceClosed = false;
            int insurerContractKey = 0;
            double invoiceBalanceDue= 0;
            double refundRecoveredAmount = 0;
            string note = "";
            string aimClaimId = "";
            string clientUniqueIdentifier = "";
            string payerClaimId = "";
            string insurerContractCode = "";
            string insurerCode = "";

            try
            {
               manager = SqlDataManager.UniqueInstance;
            }
            catch(Exception ex)
            {
                throw new Exception("Problem calling SqlDataManager", ex);        
            }
            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters = AddSqlParameter(parameters, "@ClaimID", paymentDTO.AimClaimId, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsClaimID", paymentDTO.ClientUniqueIdentifier, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@PayerClaimID", paymentDTO.ClientClaimId, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@Amount", paymentDTO.Amount.ToString(), SqlDbType.Money , "in");
            parameters = AddSqlParameter(parameters, "@ContractCode", paymentDTO.InsurerContractCode, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsCode", paymentDTO.InsurerCode, SqlDbType.VarChar, "in");
            if (paymentDTO.ClaimHeaderKey > 0)
            {
                parameters = AddSqlParameter(parameters, "@ProvClmHdrKey", paymentDTO.ClaimHeaderKey.ToString(), SqlDbType.Int, "in");
            }

            ExecuteOutcome outcome ;
            string outcomeMessage;
            SqlDataReader reader = (SqlDataReader)manager.Execute(
                TransactionChoice.WithoutTransaction
                , CommandType.StoredProcedure
                , "corBulkPosting_IdentifyClaimInsClmHdr"
                , ExecuteType.ExecuteReader
                , parameters
                , out outcome
                , out outcomeMessage);

            EvaluateOutcome( outcome, outcomeMessage, "IdentifyClaimPostInvoice");

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[reader.FieldCount];
                        int fieldCount = reader.GetValues(values);

                        if (values[0] != null & values[0].ToString().Length > 0)
                        {
                            for (int i = 0; i < fieldCount; i++)
                            {
                                try
                                {
                                    if (values[i] != null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                headerKey = (int)values[i];
                                                break;
                                            case 1:
                                                invoiceKey = (int)values[i];
                                                break;
                                            case 2:
                                                int invoiceStatus = (int)values[i];
                                                isInvoiceClosed = invoiceStatus != 0;
                                                break;
                                            case 3:
                                                insurerContractKey = (int)values[i];
                                              break;
                                            case 4:
                                                invoiceBalanceDue = Double.Parse(values[i].ToString());
                                                break;
                                            case 5:
                                                refundRecoveredAmount = Double.Parse(values[i].ToString());
                                                break;
                                            case 6:
                                                aimClaimId = (string)values[i];
                                                break;
                                            case 7:
                                                clientUniqueIdentifier = (string)values[i];
                                                break;
                                            case 8:
                                                if (values[i] != null)
                                                {payerClaimId = (string)values[i];}
                                                break;
                                            case 9:
                                                insurerContractCode = (string)values[i];
                                                break;
                                            case 10:
                                                insurerCode = (string)values[i];
                                                break;
                                            default:
                                                //do nothing; no more columns at this time
                                                break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    StringBuilder str = new StringBuilder();
                                    str.AppendLine(note);
                                    str.AppendFormat("Problem returning post invoice information:{0} for field {1}. ",
                                                     headerKey, i);
                                    str.AppendLine(ex.Message);
                                    note = str.ToString();
                                }
                            }
                            newPaymentDTO = new PaymentDTO(aimClaimId, clientUniqueIdentifier, payerClaimId
                                                           , insurerContractCode, insurerCode, paymentDTO.Amount
                                                           , headerKey, insurerContractKey, invoiceBalanceDue
                                                           , refundRecoveredAmount, invoiceKey, isInvoiceClosed, note);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return newPaymentDTO;
            
        }

        /// <summary>
        /// Returns a ProvClmHdrKey based on provided information
        /// </summary>
        /// <param name="paymentDTO"></param>
        /// <returns></returns>
        public  PaymentDTO IdentifyClaimPreInvoice(PaymentDTO paymentDTO        )
        {
            int headerKey = 0;
            int contractKey = 0;
            double recoveredAmount = 0;
            string claimId = "";
            string insClaimId = "";
            string txReference = "";
            string insurerContractCode = "";
            string insurerCode = "";
            string note = "";

            IDataManager manager = SqlDataManager.UniqueInstance;

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters = AddSqlParameter(parameters, "@ClaimID", paymentDTO.AimClaimId, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsClaimID", paymentDTO.ClientUniqueIdentifier, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@PayerClaimID", paymentDTO.ClientClaimId, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@ContractCode", paymentDTO.InsurerContractCode, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsCode", paymentDTO.InsurerCode, SqlDbType.VarChar, "in");
            if (paymentDTO.ClaimHeaderKey > 0)
            {
                parameters = AddSqlParameter(parameters, "@ProvClmHdrKey", paymentDTO.ClaimHeaderKey.ToString(), SqlDbType.Int, "in");
            }

            SqlDataReader reader;
            try
            {
                ExecuteOutcome outcome ;
                string outcomeMessage;
                reader = (SqlDataReader) manager.Execute(
                                                           TransactionChoice.WithoutTransaction
                                                           , CommandType.StoredProcedure
                                                           , "corBulkPosting_IdentifyClaimProvClmHdr"
                                                           , ExecuteType.ExecuteReader
                                                           , parameters
                                                           , out outcome, out outcomeMessage);

                EvaluateOutcome(outcome, outcomeMessage, "IdentifyClaimPreInvoice");

                if (reader == null)
                {
                    throw new Exception("IdentifyClaimPreInvoice: Reader does not return result.");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("IdentifyClaimPreInvoice: Error executing stored procedure.", ex);
            }
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[reader.FieldCount];
                        int fieldCount = reader.GetValues(values);

                        if (values[0] != null & values[0].ToString().Length > 0)
                        {

                            for (int i = 0; i < fieldCount; i++)
                            {
                                try
                                {
                                    if (values[i] != null)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                headerKey = (int) values[i];
                                                break;
                                            case 1:
                                                contractKey = (int) values[i];
                                                break;
                                            case 2:
                                                recoveredAmount = Double.Parse(values[i].ToString());
                                                break;
                                            case 3:
                                                claimId = (string) values[i];
                                                break;
                                            case 4:
                                                insClaimId = (string) values[i];
                                                break;
                                            case 5:
                                                if (values[i] != null)
                                                {txReference = (string)values[i];}
                                                break;
                                            case 6:
                                                insurerContractCode = (string) values[i];
                                                break;
                                            case 7:
                                                insurerCode = (string) values[i];
                                                break;
                                            default:
                                                //do nothing; no more columns at this time
                                                break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    StringBuilder str = new StringBuilder();
                                    str.AppendLine(note);
                                    str.AppendFormat("Problem returning post invoice information:{0} for field {1}",
                                                     headerKey, i);
                                    str.AppendLine(ex.Message);
                                    note = str.ToString();
                                }
                            }
                        }

                    }
                }
            }
            finally
            {
                reader.Close();
            }
            PaymentDTO newPaymentDTO = null;
            if (headerKey > 0)
            {
                newPaymentDTO = new PaymentDTO(claimId, insClaimId, txReference
                    , insurerContractCode, insurerCode, paymentDTO.Amount
                    , headerKey, contractKey, 0, recoveredAmount, 0, false, note);
            }
            return newPaymentDTO;

        }

        public int CreateBatchHeader(string ledgerType, string batchType, DateTime? arDate, string userLogin)
        {
            int batchHeaderKey = 0;
            switch (ledgerType)
            {
                case "PayerAccountsReceivable":
                    batchHeaderKey = CreateFeeBatchHeader(batchType, arDate, userLogin);
                    break;
                default:
                    //do nothing; other options available in later releases
                    break;
            }

            return batchHeaderKey;
        }
        public int CreateFeeBatchHeader (string batchType, DateTime? arDate, string userLogin)
        {

        IDataManager manager = SqlDataManager.UniqueInstance;

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters = AddSqlParameter(parameters, "@BatchType", batchType, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsBatARDate", arDate.ToString(), SqlDbType.DateTime, "in");
            parameters = AddSqlParameter(parameters, "@UserAdded", userLogin, SqlDbType.VarChar, "in");

            ExecuteOutcome outcome;
            string outcomeMessage;
            int headerKey = (int) manager.Execute(
                                      TransactionChoice.WithoutTransaction
                                      , CommandType.StoredProcedure
                                      , "corInsClmBatchHdrINS"
                                      , ExecuteType.ExecuteScaler
                                      , parameters
                                      , out outcome, out outcomeMessage);

            EvaluateOutcome(outcome, outcomeMessage, "CreateFeeBatchHeader");

            return headerKey;
        }

        public int CreateBatchDetail(
            TransactionChoice connType
            , string LedgerType
            , BatchDetailDTO batchDetailDTO)
        {

            int result = 0;
            switch (LedgerType)
            {
                case "PayerAccountsReceivable":
                    result = CreateFeeBatchDetail(            
                        connType
                        , batchDetailDTO);
                    break;
                default:
                    //no nothing for now; will implement other ledgers in another phase
                    break;
            }

            return result;
        }

        private int CreateFeeBatchDetail(TransactionChoice connType
            , BatchDetailDTO batchDetailDTO)
        {
            IDataManager manager = SqlDataManager.UniqueInstance;

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters = AddSqlParameter(parameters, "@InsBatHdrKey", batchDetailDTO.BatchHeaderKey.ToString(), SqlDbType.Int, "in");
            parameters = AddSqlParameter(parameters, "@InsClmHdrKey", batchDetailDTO.ClaimHeaderKey.ToString(), SqlDbType.Int, "in");
            parameters = AddSqlParameter(parameters, "@InsCorrKey", batchDetailDTO.CorrectionKey.ToString(), SqlDbType.Int, "in");
            parameters = AddSqlParameter(parameters, "@InsUndKey", batchDetailDTO.UnidentifiedKey.ToString(), SqlDbType.Int, "in");
            parameters = AddSqlParameter(parameters, "@TxType", batchDetailDTO.TransactionType, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@TxCtgy", batchDetailDTO.TransactionCategory, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@TxCode", batchDetailDTO.TransactionCode, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsBatTxDate", batchDetailDTO.BatchTransactionDate.ToString(),
                                         SqlDbType.Date, "in");
            parameters = AddSqlParameter(parameters, "@InsBatTxAmt", batchDetailDTO.BatchTransactionAmount.ToString(),
                                         SqlDbType.Money, "in");
            parameters = AddSqlParameter(parameters, "@InsChkAmt", batchDetailDTO.PayerCheckAmount.ToString(),
                                         SqlDbType.Money, "in");
            parameters = AddSqlParameter(parameters, "@InsChkDate", batchDetailDTO.PayerCheckDate.ToString(),
                                         SqlDbType.Date, "in");
            parameters = AddSqlParameter(parameters, "@InsCheckNo", batchDetailDTO.PayerCheckNumber, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@InsBulkChk", batchDetailDTO.IsPayerBulkCheck.ToString(),
                                         SqlDbType.Bit, "in");
            parameters = AddSqlParameter(parameters, "@Unidentified", batchDetailDTO.IsUnidentified.ToString(),
                                         SqlDbType.Bit, "in");
            parameters = AddSqlParameter(parameters, "@UserAdded", batchDetailDTO.UserAdded, SqlDbType.VarChar, "in");


            ExecuteOutcome outcome;
            string outcomeMessage;
            int headerKey = (int)manager.Execute(
                                      connType
                                      , CommandType.StoredProcedure
                                      , "corInsClmBatchDetINS"
                                      , ExecuteType.ExecuteScaler
                                      , parameters
                                      , out outcome, out outcomeMessage);

            EvaluateOutcome(outcome, outcomeMessage, "CreateFeeBatchDetail");

            int result = headerKey;
            return result;
        }

        public int CreateUnidentified(string ledgerType, UnidentifiedDTO unidentifiedDTO)
        {
            int result;
            IDataManager manager = SqlDataManager.UniqueInstance;

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters = AddSqlParameter(parameters, "@ContractKey", unidentifiedDTO.InsurerContractKey.ToString(),
                                         SqlDbType.Int, "in");
            parameters = AddSqlParameter(parameters, "@ClaimID", unidentifiedDTO.Reference, SqlDbType.VarChar, "in");
            parameters = AddSqlParameter(parameters, "@ChkAmt", unidentifiedDTO.Amount.ToString(), SqlDbType.Money, "in");
            parameters = AddSqlParameter(parameters, "@OpenCr", unidentifiedDTO.IsOpenCredit.ToString(), SqlDbType.Bit,
                                         "in");
            parameters = AddSqlParameter(parameters, "@UserAdded", unidentifiedDTO.UserAdded, SqlDbType.VarChar, "in");

            ExecuteOutcome outcome;
            string outcomeMessage;

            if (ledgerType == "PayerAccountsReceivable")
            {
                result = (int) manager.Execute(
                                   TransactionChoice.WithoutTransaction
                                   , CommandType.StoredProcedure
                                   , "corBulkPosting_InsUnidentifiedINS"
                                   , ExecuteType.ExecuteScaler
                                   , parameters
                                   , out outcome, out outcomeMessage);
            }
            else
            {
                throw new Exception("Invalid Ledger Type");
            }

            EvaluateOutcome( outcome, outcomeMessage, "CreateUnidentified");

            return result;
            
        }

        public BatchHeaderDTO GetBatchHeader(string ledgerType, int key)
        {
            IDataManager manager = SqlDataManager.UniqueInstance;
            string name;
            int thisKey = 0;
            DateTime? arDate = null;
            string userAdded = "";
            DateTime? dateAdded = null;
            double sumPayments = 0;
            int countPayments = 0;
            string payerCheckNumber = "";
            double payerCheckAmount = 0;
            DateTime? payerCheckDate = null;
            double sumCommissionPayments = 0;
            int countCommissionPayments = 0;
            double sumUnidentifiedPayments = 0;
            int countUnidentifiedPayments = 0;
            double sumOpenCreditPayments = 0;
            int countOpenCreditPayments = 0;
            BatchHeaderDTO batchHeaderDTO = null;

            if (ledgerType == "PayerAccountsReceivable")
            {
                name = "corBulkPosting_GetInsClmBatchHdr";
            }
            else
            {
                throw new Exception("Invalid Ledger Type");
            }
            const ExecuteType executeType = ExecuteType.ExecuteReader;
            SqlDataReader reader;
            ExecuteOutcome outcome;
            string outcomeMsg;
            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters = AddSqlParameter(parameters, "@InsBatHdrKey", key.ToString(), SqlDbType.Int, "in");

            try
            {
                reader = (SqlDataReader)manager.GetData(
                    TransactionChoice.WithoutTransaction
                    , CommandType.StoredProcedure
                    , name, parameters
                    , 3, executeType, out outcome, out outcomeMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("DataAccess:GetBatchHeader:Error retrieving data.", ex);
            }

            EvaluateOutcome(outcome, outcomeMsg, "GetBatchHeader");

            if (reader != null)
            {
                bool hasRows = reader.HasRows;

                if (hasRows)
                {
                    while (reader.Read())
                    {
                        //get the number of fields and store values into an array of objects
                        Object[] values = new Object[reader.FieldCount];
                        int fieldCount = reader.GetValues(values);


                        for (int i = 0; i < fieldCount; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    thisKey = (int)values[i];
                                    break;
                                case 1:
                                    arDate = (DateTime?)values[i];
                                    break;
                                case 2:
                                    userAdded = (string)values[i];
                                    break;
                                case 3:
                                    dateAdded = (DateTime?)values[i];
                                    break;
                                case 4:
                                    if (values[i].ToString().Length > 0)
                                    {sumPayments = Double.Parse(values[i].ToString());
                                        sumPayments = Math.Abs(sumPayments); 
                                    }
                                    break;
                                case 5:
                                    if (values[i].ToString().Length > 0)
                                    {countPayments = (int) values[i];}
                                    break;
                                case 6:
                                    if (values[i].ToString().Length > 0)
                                    {payerCheckNumber = (string)values[i];}
                                    break;
                                case 7:
                                    if (values[i].ToString().Length > 0)
                                    {payerCheckAmount = Double.Parse(values[i].ToString());}
                                    break;
                                case 8:
                                    payerCheckDate = (DateTime?) values[i];
                                    break;
                                case 9:
                                    if (values[i].ToString().Length > 0)
                                    {sumCommissionPayments = Double.Parse(values[i].ToString());
                                    sumCommissionPayments = Math.Abs(sumCommissionPayments);
                                    }
                                    break;
                                case 10:
                                    if (values[i].ToString().Length > 0)
                                    {countCommissionPayments = (int) values[i];}
                                    break;
                                case 11:
                                    if (values[i].ToString().Length > 0)
                                    {sumUnidentifiedPayments = Double.Parse(values[i].ToString());
                                    sumUnidentifiedPayments = Math.Abs(sumUnidentifiedPayments); }
                                    break;
                                case 12:
                                    if (values[i].ToString().Length > 0)
                                    {countUnidentifiedPayments = (int) values[i];}
                                    break;
                                case 13:
                                    if (values[i].ToString().Length > 0)
                                    {sumOpenCreditPayments = Double.Parse(values[i].ToString());
                                    sumOpenCreditPayments = Math.Abs(sumOpenCreditPayments);
                                    }
                                    break;
                                case 14:
                                    if (values[i].ToString().Length > 0)
                                    {countOpenCreditPayments = (int) values[i];}
                                    break;
                                default:
                                    //do nothing; no more columns at this time
                                    break;
                            }
                        }
                        if (thisKey != key)
                        {
                            throw new Exception("Key not found"); }

                    }
                    //store values into DTO object
                    batchHeaderDTO = new BatchHeaderDTO(key
                        , arDate, userAdded, dateAdded, sumPayments, countPayments
                        , payerCheckNumber, payerCheckAmount, payerCheckDate
                        , sumCommissionPayments, countCommissionPayments
                        , sumUnidentifiedPayments, countUnidentifiedPayments
                        , sumOpenCreditPayments, countOpenCreditPayments);
                }
                reader.Close();
                reader.Dispose();

            }

            return batchHeaderDTO;

        }


        public IList<InsurerDTO> GetListOfInsurers(bool Inactive)
        {

            IList<InsurerDTO> list = new List<InsurerDTO>();
            IDataManager manager = SqlDataManager.UniqueInstance;
            const string name = "corBulkPosting_GetListOfInsurers";
            const ExecuteType executeType = ExecuteType.ExecuteReader;
            SqlDataReader reader;

            ExecuteOutcome outcome ;
            string outcomeMsg;

            try
            {
                reader = (SqlDataReader)manager.GetData(
                    TransactionChoice.WithoutTransaction
                    , CommandType.StoredProcedure
                    , name, null, 3, executeType, out outcome, out outcomeMsg);
            }
            catch( Exception ex)
            {
                throw new Exception("DataAccess:GetListofInsurers:Error retrieving data.", ex);
            }

            EvaluateOutcome(outcome, outcomeMsg, "GetListofInsurers");

            if (reader != null)
            {
                bool hasRows = reader.HasRows;

                if (hasRows)
                {
                    while (reader.Read())
                    {
                        //get the number of fields and store values into an array of objects
                        Object[] values = new Object[reader.FieldCount];
                        int fieldCount = reader.GetValues(values);

                        string _name = "";
                        string _code = ""; 
                        int _key = 0;
                        bool _inactive = false;

                        for (int i = 0; i < fieldCount; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    _key = (int)values[i];
                                    break;
                                case 1:
                                    _code = (string)values[i];
                                    break;
                                case 2:
                                    _name = (string)values[i];
                                    break;
                                case 3:
                                    _inactive = (bool)values[i];
                                    break;
                                default:
                                    //do nothing; no more columns at this time
                                    break;
                            }
                        }

                        //store values into DTO object
                        InsurerDTO insurer = new InsurerDTO
                                                 {
                                                     Key = _key,
                                                     Code = _code,
                                                     Name = _name,
                                                     Inactive = _inactive
                                                 };
                        list.Add(insurer);

                    }
                }
                reader.Close();
                reader.Dispose();
            }

            return list;


        }

        public IList<ContractDTO> GetListOfContracts(bool inactive, string serviceType)
        {
            throw new NotImplementedException("GetListOfContracts has not been implemented.");
        }

        private static void EvaluateOutcome (ExecuteOutcome outcome
            , string outcomeMessage, string methodName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (outcome == ExecuteOutcome.Failure)
            {
                stringBuilder.AppendFormat("DataAccess:{0}:{1}", methodName, outcomeMessage);
                throw new Exception(stringBuilder.ToString());
            }

        }

        private static IList<SqlParameter> AddSqlParameter(
            IList<SqlParameter> parameters
            , string name
            , string value
            , SqlDbType dbType 
            ,string direction)
        {
            SqlParameter param;
            switch (dbType)
            {
                case SqlDbType.Money:
                    param = new SqlParameter(name, double.Parse(value));
                    break;
                case SqlDbType.DateTime:
                    param = new SqlParameter(name, DateTime.Parse(value));
                    break;
                case SqlDbType.Int:
                    param = new SqlParameter(name, int.Parse(value));
                    break;
                case SqlDbType.Bit:
                    switch (value.ToLower())
                    {
                        case "0":
                        case "1":
                            param = new SqlParameter(name , int.Parse(value));
                            break;
                        case "true":
                            param = new SqlParameter(name, 1);
                            break;
                        case "false":
                            param = new SqlParameter(name, 0);
                            break;
                        default:
                            param = new SqlParameter(name, null);
                            break;
                    }
                    break;
                default:
                    param = new SqlParameter(name, value);
                    break;
            }

            param.SqlDbType = dbType;
            if (direction.ToLower() == "out")
                {
                    param.Direction = ParameterDirection.Output;
                }
            parameters.Add(param);
            return parameters;
            
        }

        private static string GetClaimIdentifier(PaymentDTO paymentDTO)
        {
            string result = "";
            bool areDone = false;
            if (!string.IsNullOrEmpty(paymentDTO.AimClaimId)) { result = paymentDTO.AimClaimId;
                areDone = true; }
            if (!areDone & !string.IsNullOrEmpty(paymentDTO.ClientUniqueIdentifier))
            { result = paymentDTO.ClientUniqueIdentifier;
                areDone = true; }
            if (!areDone & !string.IsNullOrEmpty(paymentDTO.ClientClaimId))
            { result = paymentDTO.ClientClaimId;
                areDone = true; }
            if (!areDone & !string.IsNullOrEmpty(paymentDTO.ClaimHeaderKey.ToString()))
            {
                result = paymentDTO.ClaimHeaderKey.ToString();
            }
            return result;
        }

    }


}
