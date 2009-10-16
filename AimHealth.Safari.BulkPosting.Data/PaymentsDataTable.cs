using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AimHealth.Safari.BulkPosting.Data
{
    public class PaymentsDataTable
    {
        #region [Member Variables]

        private DataTable _droppedPaymentsDT;

        #endregion

        #region [Constructors]

        public PaymentsDataTable()
        {
            //"Claim Identifier{0}, Amt {1}, Contract {2}: {3}, Balance {4}, Recovered {5}, Invoice {6} Closed? {7}, Note: {8}"
            _droppedPaymentsDT = new DataTable();
            BuildColumns();

        }

        #endregion

        #region [Public Methods]

        public DataTable GetDataTable()
        {
            return _droppedPaymentsDT;
        }

        public void AddRow(string claimIdentifier, double amount, string contract, double balance, double recovered, int invoice, bool closed, string note)
        {
            DataRow dr;

            dr = _droppedPaymentsDT.NewRow();

            dr["ClaimIdentifier"] = claimIdentifier;
            dr["Amount"] = amount;
            dr["Contract"] = contract;
            dr["Balance"] = balance;
            dr["Recovered"] = recovered;
            dr["invoice"] = invoice;
            dr["closed"] = closed;
            dr["note"] = note;

            _droppedPaymentsDT.Rows.Add(dr);
        }

        public void AddDroppedRow(string claimIdentifier, double amount, string note)
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

            DataColumn dcContract = new DataColumn();
            dcContract.DataType = Type.GetType("System.Int32");
            dcContract.ColumnName = "Contract";

            _droppedPaymentsDT.Columns.Add(dcContract);

            DataColumn dcBalance = new DataColumn();
            dcBalance.DataType = Type.GetType("System.Double");
            dcBalance.ColumnName = "Balance";

            _droppedPaymentsDT.Columns.Add(dcBalance);

            DataColumn dcRecovered = new DataColumn();
            dcRecovered.DataType = Type.GetType("System.Double");
            dcRecovered.ColumnName = "Recovered";

            _droppedPaymentsDT.Columns.Add(dcRecovered);

            DataColumn dcInvoice = new DataColumn();
            dcInvoice.DataType = Type.GetType("System.Double");
            dcInvoice.ColumnName = "Invoice";

            _droppedPaymentsDT.Columns.Add(dcInvoice);

            DataColumn dcClosed = new DataColumn();
            dcClosed.DataType = Type.GetType("System.Boolean");
            dcClosed.ColumnName = "Closed";
            
            _droppedPaymentsDT.Columns.Add(dcClosed);

            DataColumn dcNote = new DataColumn();
            dcNote.DataType = Type.GetType("System.String");
            dcNote.ColumnName = "Note";

            _droppedPaymentsDT.Columns.Add(dcNote);
        }
        #endregion
    }
}
