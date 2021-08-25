using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Token
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
