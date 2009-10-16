using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Aim.Data
{
    public interface IDataSetProvider
    {
        DataSet GetDataSet();
    }
}
