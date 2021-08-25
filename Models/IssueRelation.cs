using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class IssueRelation
    {
        public int Id { get; set; }
        public int IssueFromId { get; set; }
        public int IssueToId { get; set; }
        public string RelationType { get; set; }
        public int? Delay { get; set; }
    }
}
