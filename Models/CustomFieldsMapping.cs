using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class CustomFieldsMapping
    {
        public int Id { get; set; }
        public string CustomKey { get; set; }
        public string CustomName { get; set; }
        public int? CustomFieldId { get; set; }
    }
}
