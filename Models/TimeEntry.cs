using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class TimeEntry
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? AuthorId { get; set; }
        public int UserId { get; set; }
        public int? IssueId { get; set; }
        public float Hours { get; set; }
        public string Comments { get; set; }
        public int ActivityId { get; set; }
        public DateTime SpentOn { get; set; }
        public int Tyear { get; set; }
        public int Tmonth { get; set; }
        public int Tweek { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
