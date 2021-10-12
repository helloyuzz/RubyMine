using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RubyMine.Models {
    public partial class Module {
        public int Id { get; set; }
        [Display(Name = "模块名称"),Required]
        public string Name { get; set; }
        [Display(Name = "显示次序")]
        public int? Index { get; set; }
        [Display(Name = "拼音检索")]
        public string Pinyin { get; set; }
        [Display(Name = "是否禁用")]
        public bool? Disabled { get; set; }
        [Display(Name = "上级Id")]
        public int? PId { get; set; }
    }
}