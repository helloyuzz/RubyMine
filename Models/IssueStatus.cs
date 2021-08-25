using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RubyMine.Models {
    public partial class IssueStatus {
        [Display(Name = "序号")]
        public int Id { get; set; }
        [Display(Name = "状态名称")]
        public string Name { get; set; }
        [Display(Name = "已关闭的问题")]
        public bool IsClosed { get; set; }
        [Display(Name = "显示次序")]
        public int? Position { get; set; }
        public int? DefaultDoneRatio { get; set; }
    }
}