using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Customs.Models {
    public class DisplayIssue {
        public DisplayIssue(Issue issue) {
            Issue = issue;
        }

        public Issue Issue { get; set; }
        public int Position { get; set; }
    }
}
