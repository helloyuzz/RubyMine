using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Customs.Models {
    public class SoapIssue {
        public string Action { get; set; }
        public int module_id { get; set; }
        public int prev_issue_id { get; set; }
        public Issue Issue { get; set; }
    }
}
