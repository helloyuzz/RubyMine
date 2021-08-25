using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RubyMine.Models {
    public partial class Project {
        [Display(Name ="序号")]
        public int Id { get; set; }
        [Display(Name = "项目名称")]
        public string Name { get; set; }
        [Display(Name = "项目描述")]
        public string Description { get; set; }
        public string Homepage { get; set; }
        [Display(Name = "是否公开")]
        public bool? IsPublic { get; set; }
        public int? ParentId { get; set; }
        [Display(Name = "创建日期")]
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [Display(Name = "项目唯一标识")]
        public string Identifier { get; set; }
        [Display(Name = "状态")]
        public int Status { get; set; }
        public int? Lft { get; set; }
        public int? Rgt { get; set; }
        public bool InheritMembers { get; set; }
        public int? DefaultVersionId { get; set; }
        public int? DefaultAssignedToId { get; set; }
    }
}