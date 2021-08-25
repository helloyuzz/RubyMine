using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Wiki
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string StartPage { get; set; }
        public int Status { get; set; }
    }
}
