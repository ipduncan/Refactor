
namespace AimHealth.Safari.BulkPosting.Bus
{

    public class PaymentVerifier 
    {

        public static bool VerifyPaymentAmountInformation(double amount)
        {
            bool result = true;
            if (amount <= 0)
            {
                result = false;
            }
            return result;
        }

        public static bool VerifyPaymentReferenceInformation(string aimClaimId, string clientUniqueIdentifier, string clientClaimId)
        {
            bool result = true;
            if (string.IsNullOrEmpty(aimClaimId)
                & string.IsNullOrEmpty(clientClaimId)
                & string.IsNullOrEmpty(clientUniqueIdentifier)
                )
            {
                result = false;
            }
            return result;
        }

        public static bool VerifyContractInformation(string insurerContractCode, string insurerCode)
        {
            bool result = true;

            //there must be a contract code or an insurer code
            if ((string.IsNullOrEmpty(insurerContractCode) || insurerContractCode == "0")
                &
                (string.IsNullOrEmpty(insurerCode) || insurerCode == "0"))
            {
                result = false;
            }

            if (!string.IsNullOrEmpty(insurerContractCode))
                if (insurerContractCode.Length != 9)
                {
                    result = false;
                }

            if (!string.IsNullOrEmpty(insurerCode))
                if (insurerCode.Length != 5)
                {
                    result = false;
                }
            return result;
        }

    }
}
