
namespace AimHealth.Safari.BulkPosting.Data
{
    public enum TransactionChoice
    {
        WithTransaction = 0, WithoutTransaction = 1
    }

    public enum ExecuteType
    {
        ExecuteReader = 0
        ,
        ExecuteScaler = 1
            , ExecuteNonQuery = 2
    }

    public enum ExecuteOutcome
    {
        Success = 0
        ,
        Failure = 1
            , Timeout = 2
    }


}