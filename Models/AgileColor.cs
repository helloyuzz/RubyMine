using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class AgileColor
    {
        public int Id { get; set; }
        public string ContainerType { get; set; }
        public int? ContainerId { get; set; }
        public string Color { get; set; }
    }
}
