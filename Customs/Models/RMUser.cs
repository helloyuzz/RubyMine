using System;
using System.Collections.Generic;

namespace RubyMine.Models.CustomModels {
    public class RMUser : User, ICloneable {
        public List<CustomField1>? CustomFields { get; set; } = new List<CustomField1>();
        public int IssueNumber { get; set; }
        public int BugNumber { get; set; }
        public int HisNumber { get; set; }
        public int DeviceNumber { get; set; }
        public int DeptNumber { get; set; }
        public int PortalNumber { get; set; }
        public User Employee { get; set; }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
