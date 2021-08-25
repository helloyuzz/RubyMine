using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RubyMine.Models {
    public partial class User {
        [Display(Name ="序号")]
        public int Id { get; set; }
        [Display(Name = "帐号")]
        public string Login { get; set; }
        public string HashedPassword { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool Admin { get; set; }
        [Display(Name = "状态")]
        public int Status { get; set; }
        [Display(Name = "最近登录")]
        public DateTime? LastLoginOn { get; set; }
        public string Language { get; set; }
        public int? AuthSourceId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Type { get; set; }
        public string IdentityUrl { get; set; }
        public string MailNotification { get; set; }
        public string Salt { get; set; }
        public bool MustChangePasswd { get; set; }
        public DateTime? PasswdChangedOn { get; set; }
        public string pinyin { get; set; }
    }
}