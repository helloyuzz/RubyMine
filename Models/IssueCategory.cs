using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class IssueCategory
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int? AssignedToId { get; set; }
    }
}
