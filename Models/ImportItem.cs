using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class ImportItem
    {
        public int Id { get; set; }
        public int ImportId { get; set; }
        public int Position { get; set; }
        public int? ObjId { get; set; }
        public string Message { get; set; }
        public string UniqueId { get; set; }
    }
}
