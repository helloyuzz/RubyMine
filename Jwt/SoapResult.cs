using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Jwt {
    public class SoapResult {        
        public SoapResult(string defaultValue) {
            Result = defaultValue;
        }

        public string Result { get;  set; }
        public string Value { get; set; }
    }
}
