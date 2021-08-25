using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class AuthSource
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Account { get; set; }
        public string AccountPassword { get; set; }
        public string BaseDn { get; set; }
        public string AttrLogin { get; set; }
        public string AttrFirstname { get; set; }
        public string AttrLastname { get; set; }
        public string AttrMail { get; set; }
        public bool OntheflyRegister { get; set; }
        public bool Tls { get; set; }
        public string Filter { get; set; }
        public int? Timeout { get; set; }
        public bool? VerifyPeer { get; set; }
    }
}
