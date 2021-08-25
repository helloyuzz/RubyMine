using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CommentsCount { get; set; }
    }
}
