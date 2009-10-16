using System.Collections.Generic;
using AimHealth.Safari.BulkPosting.DTO;
using System;

namespace AimHealth.Safari.BulkPosting.Data
{
    public interface IDataAccess
    {
        int GetContractKey(string insurerContractCode);

        PaymentDTO IdentifyClaim(PaymentDTO paymentDTO);

        PaymentDTO IdentifyClaimPostInvoice(PaymentDTO paymentDTO);

        PaymentDTO IdentifyClaimPreInvoice(PaymentDTO paymentDTO);

        int CreateBatchHeader(string ledgerType, string batchType, DateTime? arDate, string userLogin);
        int CreateFeeBatchHeader(string batchType, DateTime? arDate, string userLogin);

        int CreateBatchDetail(
            TransactionChoice connType
            , string LedgerType
            , BatchDetailDTO batchDetailDTO);

        int CreateUnidentified(string ledgerType, UnidentifiedDTO unidentifiedDTO);
        BatchHeaderDTO GetBatchHeader(string ledgerType, int key);
        IList<InsurerDTO> GetListOfInsurers(bool Inactive);
        IList<ContractDTO> GetListOfContracts(bool inactive, string serviceType);
    }

}