﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Enumeration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }
        public bool IsDefault { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }
        public int? ProjectId { get; set; }
        public int? ParentId { get; set; }
        public string PositionName { get; set; }
    }
}
