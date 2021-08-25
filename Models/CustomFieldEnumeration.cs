using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class CustomFieldEnumeration
    {
        public int Id { get; set; }
        public int CustomFieldId { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int Position { get; set; }
    }
}
