using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class UserPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Others { get; set; }
        public bool? HideMail { get; set; }
        public string TimeZone { get; set; }
    }
}
