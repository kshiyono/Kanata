using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Code
    {
        public string _code { get; private set; }
        public string _name { get; private set; }

        public Code(string code, string name = "")
        {
            _code   = code;
            _name   = name;
        }
    }
}
