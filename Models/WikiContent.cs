using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class WikiContent
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int? AuthorId { get; set; }
        public string Text { get; set; }
        public string Comments { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Version { get; set; }
    }
}
