using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using AimHealth.Safari.BulkPosting.Data;

namespace DataAccessBulkManager
{
    public interface IDataManager
    {
        object Execute(TransactionChoice connType
            , CommandType commandType
            , string s
            , ExecuteType scaler
            , IList<SqlParameter> parameters
            , out ExecuteOutcome outcome
            , out string message);

        object GetData(TransactionChoice connType
            , CommandType commandType
            , string procedureName
            , IList<SqlParameter> parameters
            , int retries
            , ExecuteType executeType, out ExecuteOutcome outcome
            , out string outcomeMsg);

    }

}
