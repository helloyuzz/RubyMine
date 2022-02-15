using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Models {
    public class TreeIssue {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Module_id { get; set; }
        public int Position { get; set; }
    }
}
