using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AimHealth.Safari.BulkPosting.DTO
{
    public class InsurerDTO
    {
        private string _name = "";
        private string _code = "";
        private int _key = 0;
        private bool _inactive = false;

        public InsurerDTO(int key, string code, string name, bool inactive)
        {
            _name = name;
            _code = code;
            _key = key;
            _inactive = inactive;
        }

        public InsurerDTO()
        {
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
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

        public bool Inactive
        {
            get { return _inactive; }
            set { _inactive = value; }
        }
    }
}
