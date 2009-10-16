using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AimHealth.Safari.BulkPosting.DTO
{
    public class ContractDTO
    {
        private string _code = "";
        private int _key = 0;
        private string _serviceType;
        private bool _inactive = false;

        public ContractDTO()
        {
        }

        public ContractDTO(int key, string code, string servicetype, bool inactive)
        {
            _key = key;
            _code = code;
            _serviceType = servicetype;
            _inactive = inactive;
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }

        public bool Inactive
        {
            get { return _inactive; }
            set { _inactive = value; }
        }
    }
}
