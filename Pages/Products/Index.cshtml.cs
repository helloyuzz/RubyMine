using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.CustomClass;
using RubyMine.DIP;
using RubyMine.Models;
using RubyMine.Models.CustomModels;

namespace RubyMine.Pages.Products {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        private readonly RubyMine.DIP.ISysSetting _config;
        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context, ISysSetting config) {
            _context = context;
            _config = config;
        }
        [BindProperty]
        public IList<Project> Product { get; set; }
        [BindProperty]
        public IList<User> Project { get; set; }
        public IList<Member> Member { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            Product = await _context.Projects.Where(t => t.IsPublic.Value).Select(t => new Project { Id = t.Id, Name = t.Name, Description = t.Description }).OrderBy(t => t.Id).ToListAsync();
            Project = await _context.Users.Where(t => t.Lastname.Equals("p")).Select(t => new User { Id = t.Id, Firstname = t.Firstname, pinyin = t.pinyin }).ToListAsync();
            Member = await _context.Members.Select(t => new Member { Id = t.Id, UserId = t.UserId, ProjectId = t.ProjectId }).ToListAsync();

            // ϵͳԼ����������Ŀ��p_��ͷ
            // ��һ������ȡ���е���Ŀ������Ϣ
            //IList<User> db_Project = await _context.Users.Where(x => x.Login.StartsWith("p_")).ToListAsync();
            //List<string> db_ProjectIds = db_Project.Select(t => t.Id).ToList().ConvertAll<string>(x => x.ToString());

            // �ڶ�������ȡ���е��Զ�������
            //List<CustomField> db_CustomFields = await _context.CustomFields.Where(a => a.Type.Equals("UserCustomField") && a.Description.Contains("p")).OrderBy(x => x.Position).ToListAsync();

            // ��ѯ��Project��ص��Զ�������ֵ������Ϊ�����Զ������Ե�Id��Ϊ��ѯ����
            //List<int> ids = db_CustomFields.Select(x => x.Id).ToList();

            // ����������ȡ�����Զ�������ֵ,�ɸ���userid��ѯ����CustomValue��Ҳ���ǻ�ȡĳ�û����Զ�������ֵ
            //List<CustomValue> db_CustomValues = await _context.CustomValues.Where(a => ids.Contains(a.CustomFieldId)).OrderBy(x => x.CustomizedId).ToListAsync();

            // ���Ĳ����õǼ���Դͳ������
            //int sourceProjectId = _config.CustomFieldsMappings().FirstOrDefault(t => t.CustomKey.Equals(CustomFieldKeys.source_project)).CustomFieldId.Value;

            //List<TrackerCountItem> db_TrackerCount = new List<TrackerCountItem>();
            //using (var cmd = _context.Database.GetDbConnection().CreateCommand()) {
            //    cmd.CommandText = "select a.value,a.tracker_id,count(a.custom_field_id) as 'count' from(SELECT c.custom_field_id, c.value, i.tracker_id from custom_values c left join issues i on c.customized_id = i.id where c.custom_field_id = 2 and c.value is not null) a GROUP BY a.tracker_id,a.value order by a.value,a.tracker_id";
            //    _context.Database.OpenConnection();
            //    using (var db_Result = cmd.ExecuteReader()) {
            //        while (db_Result.Read()) {
            //            TrackerCountItem trackerCountItem = new TrackerCountItem();
            //            trackerCountItem.UserId = RMUtils.ParseInt(db_Result[0]);
            //            trackerCountItem.TrackerId = RMUtils.ParseInt(db_Result[1]);
            //            trackerCountItem.Count = RMUtils.ParseInt(db_Result[2]);
            //            db_TrackerCount.Add(trackerCountItem);
            //        }
            //    }
            //}

            // ��ȡ����ʵʩ��Ա
            //var employees = db_CustomValues.Where(t => t.CustomFieldId == 9 && string.IsNullOrEmpty(t.Value) == false);
            //var employee_ids = db_CustomValues.Select(x => RMUtils.ParseInt(x.Value)).ToList();
            //var all_employee = await _context.Users.Where(t => employee_ids.Contains(t.Id)).ToListAsync();

            // ���Ĳ����ṹ����������CustomFieldת��Ϊ==>CustomField1
            //RMProjects = RMUtils.ParseToRMUser(db_Project, db_CustomFields, db_CustomValues);// new List<RMUser>();

            //List<TrackerMapping> db_TrackerMapping = _config.TrackerMapping();
            //foreach (RMUser item in RMProjects) {
            //    // ʵʩԱ��
            //    var get_employee_id = employees.FirstOrDefault(t => t.CustomizedId == item.Id);
            //    if (get_employee_id != null) {
            //        item.Employee = all_employee.FirstOrDefault(t => t.Id == RMUtils.ParseInt(get_employee_id.Value));
            //    }

            //    // ����ͳ������
            //    List<TrackerCountItem> user_TrackerCount = db_TrackerCount.Where(t => t.UserId == item.Id).ToList();
            //    int? tracker_id = db_TrackerMapping.FirstOrDefault(x => x.TrackerKey.Equals(TrackerMappingKey.requirement)).TrackerId;
            //    if (user_TrackerCount.Exists(t => t.TrackerId == tracker_id)) {
            //        item.IssueNumber = user_TrackerCount.FirstOrDefault(d => d.TrackerId == tracker_id).Count;
            //    }

            //    tracker_id = db_TrackerMapping.FirstOrDefault(x => x.TrackerKey.Equals(TrackerMappingKey.bug)).TrackerId;
            //    if (user_TrackerCount.Exists(t => t.TrackerId == tracker_id)) {
            //        item.BugNumber = user_TrackerCount.FirstOrDefault(d => d.TrackerId == tracker_id).Count;
            //    }

            //    tracker_id = db_TrackerMapping.FirstOrDefault(x => x.TrackerKey.Equals(TrackerMappingKey.his)).TrackerId;
            //    if (user_TrackerCount.Exists(t => t.TrackerId == tracker_id)) {
            //        item.HisNumber = user_TrackerCount.FirstOrDefault(d => d.TrackerId == tracker_id).Count;
            //    }

            //    tracker_id = db_TrackerMapping.FirstOrDefault(x => x.TrackerKey.Equals(TrackerMappingKey.device)).TrackerId;
            //    if (user_TrackerCount.Exists(t => t.TrackerId == tracker_id)) {
            //        item.DeviceNumber = user_TrackerCount.FirstOrDefault(d => d.TrackerId == tracker_id).Count;
            //    }

            //    tracker_id = db_TrackerMapping.FirstOrDefault(x => x.TrackerKey.Equals(TrackerMappingKey.dept)).TrackerId;
            //    if (user_TrackerCount.Exists(t => t.TrackerId == tracker_id)) {
            //        item.DeptNumber = user_TrackerCount.FirstOrDefault(d => d.TrackerId == tracker_id).Count;
            //    }

            //    tracker_id = db_TrackerMapping.FirstOrDefault(x => x.TrackerKey.Equals(TrackerMappingKey.portal)).TrackerId;
            //    if (user_TrackerCount.Exists(t => t.TrackerId == tracker_id)) {
            //        item.PortalNumber = user_TrackerCount.FirstOrDefault(d => d.TrackerId == tracker_id).Count;
            //    }
            //}

            return Page();
        }
    }
}