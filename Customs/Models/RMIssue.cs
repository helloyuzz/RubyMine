using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Models.CustomModels {
    public class RMIssue:Issue {
        public List<CustomField1>? CustomFields { get; set; } = new List<CustomField1>();
    }
}
