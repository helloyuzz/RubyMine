using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class WikiPage
    {
        public int Id { get; set; }
        public int WikiId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Protected { get; set; }
        public int? ParentId { get; set; }
    }
}
