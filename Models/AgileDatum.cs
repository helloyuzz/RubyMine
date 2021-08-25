using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class AgileDatum
    {
        public int Id { get; set; }
        public int? IssueId { get; set; }
        public int? Position { get; set; }
        public int? StoryPoints { get; set; }
        public int? AgileSprintId { get; set; }
    }
}
