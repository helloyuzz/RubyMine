using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class AgileSprint
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
