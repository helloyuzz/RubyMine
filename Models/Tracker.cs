using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RubyMine.Models {
    public partial class Tracker {
        [Display(Name ="序号")]
        public int Id { get; set; }
        [Display(Name = "标签")]
        public string Name { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        public bool IsInChlog { get; set; }
        [Display(Name = "显示排序")]
        public int? Position { get; set; }
        public bool? IsInRoadmap { get; set; }
        public int? FieldsBits { get; set; }
        public int? DefaultStatusId { get; set; }
    }
}