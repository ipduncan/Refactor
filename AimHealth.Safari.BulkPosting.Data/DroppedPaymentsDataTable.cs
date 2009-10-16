using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AimHealth.Safari.BulkPosting.Data
{
    public class DroppedPaymentsDataTable
    {

                #region [Member Variables]

        private DataTable _droppedPaymentsDT;

        #endregion

        #region [Constructors]

        public DroppedPaymentsDataTable()
        {
            //"Claim Identifier{0}, Amt {1}, Note: {2}"
            _droppedPaymentsDT = new DataTable();
            BuildColumns();
        }

        #endregion

        #region [Public Methods]

        public DataTable GetDataTable()
        {
            return _droppedPaymentsDT;
        }

        public void AddRow(string claimIdentifier, double amount, string note)
        {
            DataRow dr;

            dr = _droppedPaymentsDT.NewRow();

            dr["ClaimIdentifier"] = claimIdentifier;
            dr["Amount"] = amount;
            dr["note"] = note;

            _droppedPaymentsDT.Rows.Add(dr);
        }
        #endregion

        #region [Private Methods]

        private void BuildColumns()
        {
            DataColumn dcClaimIdent = new DataColumn();
            dcClaimIdent.DataType = Type.GetType("System.String");
            dcClaimIdent.ColumnName = "ClaimIdentifier";

            _droppedPaymentsDT.Columns.Add(dcClaimIdent);

            DataColumn dcAmount = new DataColumn();
            dcAmount.DataType = Type.GetType("System.Double");
            dcAmount.ColumnName = "Amount";

            _droppedPaymentsDT.Columns.Add(dcAmount);

            DataColumn dcNote = new DataColumn();
            dcNote.DataType = Type.GetType("System.String");
            dcNote.ColumnName = "Note";

            _droppedPaymentsDT.Columns.Add(dcNote);
        }
        #endregion


    }
}
