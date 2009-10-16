using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;

namespace Aim.Data.ExcelImporter.ExcelDataReader
{
    public class ExcelColumnReader
    {
        private string _fileName;
        private IList<string> _ExcelColumns;
        private DataSet _WorkBookDataDS;

        public ExcelColumnReader(string fileName)
        {
            _fileName = fileName;

            FileInfo fi = new FileInfo(_fileName);

            if (fi.Exists)
            {
                ReadExcelDataColumns();
            }
        }

        private void ReadExcelDataColumns()
        {
            IList<string> list = new List<string>();

            //CBF - In order to fix memory problem needed to get reference to Excel outside of try
            Microsoft.Office.Interop.Excel.Application oApp = null;
            try
            {
                oApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks oBooks = oApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook oBook = oBooks.Add(_fileName);
                Microsoft.Office.Interop.Excel.Worksheet oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oApp.ActiveSheet;

                string activeSheeName = oSheet.Name;
                int test1 = oSheet.Columns.Count;

                String sConnectionString = string.Empty;

                if (_fileName.ToLower().EndsWith(".xlsx"))
                {
                    sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                        "Data Source=" + _fileName + ";" +
                        "Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";
                }
                else
                {
                    sConnectionString =
                        "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + _fileName + ";" +
                        "Extended Properties=Excel 8.0;";

                }


                OleDbConnection objConn = new OleDbConnection(sConnectionString);

                objConn.Open();
                string cmdString = "SELECT * FROM [" + activeSheeName + "$]";
                OleDbCommand objCmdSelect = new OleDbCommand(cmdString, objConn);

                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                objAdapter1.SelectCommand = objCmdSelect;

                _WorkBookDataDS = new DataSet();

                objAdapter1.Fill(_WorkBookDataDS);

                objConn.Close();

                foreach (DataColumn col in _WorkBookDataDS.Tables[0].Columns)
                {
                    list.Add(col.Caption.Trim().ToString());
                }

                _ExcelColumns = list;
            }
            catch (Exception ex)
            {
                _ExcelColumns = null;
            }
            finally 
            {
                //CBF - added finally clause to check on Excel object and if not null then get it out of memory
                //this will not affect other instances of Excel in use by the user, only the instance used here
                if (oApp != null) 
                {
                oApp.Quit();
                }
            }
        }

        public IList<string> GetColumns
        {
            get { return _ExcelColumns; }
        }
        public DataSet GetWorkbookData
        {
            get { return _WorkBookDataDS; }
        }
    }
}
