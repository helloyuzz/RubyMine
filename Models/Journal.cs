using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Journal
    {
        public int Id { get; set; }
        public int JournalizedId { get; set; }
        public string JournalizedType { get; set; }
        public int UserId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool PrivateNotes { get; set; }
    }
}
