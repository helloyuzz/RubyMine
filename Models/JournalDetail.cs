using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class JournalDetail
    {
        public int Id { get; set; }
        public int JournalId { get; set; }
        public string Property { get; set; }
        public string PropKey { get; set; }
        public string OldValue { get; set; }
        public string Value { get; set; }
    }
}
