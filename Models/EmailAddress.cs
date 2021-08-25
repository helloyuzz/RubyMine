using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class EmailAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
        public bool? Notify { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
