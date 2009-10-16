using System;
using System.Collections.Generic;
using System.Text;

namespace Aim.Data.ExcelImporter.ExcelDataReader
{
    public static class ExcelFieldHelper
    {
        public static Nullable<DateTime> TryGetExcelDateTime(object excelDate)
        {
            try
            {
                double parseExcelDateDouble;
                if (double.TryParse(excelDate.ToString().Trim(), out parseExcelDateDouble))
                {
                    return DateTime.FromOADate(parseExcelDateDouble);
                }

                DateTime parseExcelDateDateTime;
                if (DateTime.TryParse(excelDate.ToString().Trim(), out parseExcelDateDateTime))
                {
                    return parseExcelDateDateTime;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
