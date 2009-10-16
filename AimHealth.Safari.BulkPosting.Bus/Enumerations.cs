namespace AimHealth.Safari.BulkPosting.Bus
{
        public enum PayerType
        { Insurer, Provider }

        public enum LedgerType
        { ProviderAccountsReceivable, PayerAccountsReceivable, AccountsPayable }

        public enum SecurityAuthorizedRoles
        { SafariRecovery, DbItOpsFinancial, SafariPayerFeeBatchCreate}

        public enum SecurityPermittedAccess
        { PayerFeeBatchCreate}


}