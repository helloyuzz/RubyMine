using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Member
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool MailNotification { get; set; }
    }
}
