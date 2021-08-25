using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public int? ParentId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int? AuthorId { get; set; }
        public int RepliesCount { get; set; }
        public int? LastReplyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool? Locked { get; set; }
        public int? Sticky { get; set; }
    }
}
