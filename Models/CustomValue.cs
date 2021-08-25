using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class CustomValue
    {
        public int Id { get; set; }
        public string CustomizedType { get; set; }
        public int CustomizedId { get; set; }
        public int CustomFieldId { get; set; }
        public string Value { get; set; }
    }
}
