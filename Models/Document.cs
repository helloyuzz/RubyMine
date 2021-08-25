using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Document
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
