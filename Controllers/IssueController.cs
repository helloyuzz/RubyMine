using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RubyMine.Customs.Models;
using RubyMine.DbContexts;
using RubyMine.Jwt;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace RubyMine.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase {
        private readonly ILogger<IssueController> _logger;
        private readonly RubyRemineDbContext _context;
        private readonly IConfiguration _config;
        public IssueController(ILogger<IssueController> logger, RubyMine.DbContexts.RubyRemineDbContext context, IConfiguration _configuration) {
            _context = context;
            _logger = logger;
            _config = _configuration;
        }
        // GET: api/<JwtController>
        //[HttpGet]
        //public IEnumerable<string> Get() {
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<IssueController>/5
        [HttpGet("{id}")]
        public ActionResult<SoapDescription> Get(int id) {
            SoapDescription desc = new SoapDescription("Invalid request.");
            if (id > 0) {
                string prev_url = _config["AppSettings:Attachment_PreviewUrl"];
                string download_url = _config["AppSettings:Attachment_DownloadUrl"];

                desc.issue_id = id;
                desc.description = _context.Issues.FirstOrDefault(t => t.Id == id).Description;     // 获取Issue的Description
                desc.Journals = _context.Journals.Where(t => t.JournalizedId == id).ToList();       // 获取Issue的变更记录

                List<int> user_ids = desc.Journals.Select(t=>t.UserId).Distinct().ToList();         // 获取Issue变更记录中的User.id
                List<int> issue_ids = desc.Journals.Where(t => string.IsNullOrEmpty(t.Notes)).Select(t => t.Id).ToList();   // 获取Issue变更记录的Issue.id
                List<int> attachment_ids = null;                                                    // 获取Issue的附件id,Accachment.id
                List<Attachment> attachment = null;                                                 // 所有附件
                if (issue_ids.Count > 0) {
                    desc.JournalDetails = _context.JournalDetails.Where(t => issue_ids.Contains(t.JournalId)).ToList();     // 获取变更详情 && t.Property.Equals("attr")
                    attachment_ids = desc.JournalDetails.Where(t => t.Property.Equals("attachment")).Select(t => int.Parse(t.PropKey)).ToList();
                }
                var edit_user = _context.Users.Where(t => user_ids.Contains(t.Id)).Select(t => new User { Id = t.Id, Firstname = t.Firstname }).ToList();   // 用户名称
                if (attachment_ids != null) {                                                                                                               // 获取附件
                    attachment = _context.Attachments.Where(t => t.ContainerType.Equals("Issue") && t.ContainerId == desc.issue_id).ToList();                // 获取Issue的Attachment
                }

                // 历史记录
                int rowCount = desc.Journals.Count();
                desc.history_length = rowCount;
                foreach (var item in desc.Journals.OrderByDescending(t => t.CreatedOn)) {
                    desc.history_tr += "<p class=\"m-0 p-0\">";
                    desc.history_tr += "<span class=\"badge bg-primary fs-9 m-0 p-1 mr-2 fw-normal\">" + rowCount + "</span>";
                    desc.history_tr += "由&nbsp<span class=\"text-269\">" + edit_user.FirstOrDefault(t => t.Id == item.UserId).Firstname + "</span>&nbsp;更新于&nbsp;<span class=\"text-269\">" + item.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") + "</span>";
                    if (string.IsNullOrEmpty(item.Notes) == false) {
                        desc.history_tr += "<span class=\"fw-bold\">&nbsp;说明</span> 已更新。";
                        desc.history_tr += "</p>";
                        desc.history_tr += "<span class=\"text-666 ps-4 fw-bold\">" + item.Notes + "</span>";
                    } else {
                        var temp_content = desc.JournalDetails.FirstOrDefault(t => t.JournalId == item.Id);
                        if (temp_content != null) {
                            desc.history_tr += "<span class=\"fw-bold\">&nbsp" + RMUtils.ParseJournalType(temp_content.Property, temp_content.PropKey) + "</span> 已更新。";
                            desc.history_tr += "<p class=\"m-0 p-0 ps-4\"><span class=\"text-666 text-decoration-line-through\">" + temp_content.OldValue + "</span> 更改为：<br>";
                            if (temp_content.Property.Equals("attachment")) {
                                desc.history_tr += "<a href=\"" + prev_url + temp_content.PropKey + "\" target=\"_blank\">" + temp_content.Value + "</a>&nbsp;<a href=\"" + download_url + temp_content.PropKey + "/" + temp_content.Value + "\" title=\"下载\" target=\"_blank\"><img src=\"/images/file_download.png\"/></a>";
                            } else {
                                desc.history_tr += temp_content.Value;
                            }
                            desc.history_tr += "</p>";
                        }
                    }
                    desc.history_tr += "<hr class=\"m-1 p-0\">";
                    rowCount--;
                }

                // 备注更改，notes == null
                var notes_Journals = desc.Journals.Where(t => string.IsNullOrEmpty(t.Notes) == false).OrderByDescending(t => t.Id);
                rowCount = notes_Journals.Count();
                desc.notes_length = rowCount;
                foreach (var item in notes_Journals) {
                    desc.notes_tr += "<tr>";
                    desc.notes_tr += "  <td class=\"td td-row-index\">" + rowCount + "</td>";
                    desc.notes_tr += "  <td class=\"td td-author text-269\">" + edit_user.FirstOrDefault(t => t.Id == item.UserId).Firstname + "</td>";
                    desc.notes_tr += "  <td class=\"td td-create-on\">" + item.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                    desc.notes_tr += "  <td class=\"td\">" + item.Notes + "</td>";
                    desc.notes_tr += "</tr>";
                    rowCount--;
                }

                // 属性更改
                if (desc.JournalDetails != null) {
                    var properties_Journals = desc.JournalDetails.OrderByDescending(t => t.Id);
                    rowCount = properties_Journals.Count();
                    desc.property_length = rowCount;
                    foreach (var item in properties_Journals) {
                        var journal = desc.Journals.FirstOrDefault(t => t.Id == item.JournalId);

                        desc.properties_tr += "<tr>";
                        desc.properties_tr += " <td class=\"td td-row-index\">" + rowCount + "</td>";
                        desc.properties_tr += " <td class=\"td td-author text-269\">" + edit_user.FirstOrDefault(t => t.Id == journal.UserId).Firstname + "</td>";
                        desc.properties_tr += " <td class=\"td td-create-on\">" + journal.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                        desc.properties_tr += " <td class=\"td\"><span class=\"fw-bold\">" + RMUtils.ParseJournalType(item.Property, item.PropKey) + "</span> 已更新。<p class=\"m-0 p-0\"><span class=\"text-666 text-decoration-line-through\">" + item.OldValue + "</span> 更改为：<br>";
                        if (item.Property.Equals("attachment")) {
                            desc.properties_tr += "<a href=\"" + prev_url + item.PropKey + "\" target=\"_blank\">" + item.Value + "</a>&nbsp;<a href=\"" + download_url + item.PropKey + "/" + item.Value + "\" title=\"下载\" target=\"_blank\"><img src=\"/images/file_download.png\"/></a>";
                        } else {
                            desc.properties_tr += item.Value;
                        }
                        desc.properties_tr += "</p></td>";                        
                        desc.properties_tr += "</tr>";

                        rowCount--;
                    }
                }

                // 附件
                if (attachment != null) {
                    rowCount = attachment.Count();
                    desc.attachment_length = attachment.Count();
                    foreach (var item in attachment.OrderByDescending(t=>t.Id)) {
                        //var journal_id = desc.JournalDetails.FirstOrDefault(t => t.Property.Equals("attachment") && t.PropKey.Equals(item.Id.ToString())).JournalId;
                        //var journal = desc.Journals.FirstOrDefault(t => t.Id == journal_id);

                        desc.attachment_tr += "<tr>";
                        desc.attachment_tr += " <td class=\"td td-row-index\">" + rowCount + "</td>";
                        desc.attachment_tr += " <td class=\"td td-author text-269\">" + edit_user.FirstOrDefault(t => t.Id == item.AuthorId).Firstname + "</td>";
                        //desc.attachment_tr += " <td class=\"td td-create-on\">" + item.CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                        desc.attachment_tr += " <td class=\"td\"><a href=\"" + prev_url + item.Id + "\" target=\"_blank\">" + item.Filename + "</a>&nbsp;<a href=\"" + download_url + item.Id + "/" + item.Filename + "\" title=\"下载\" target=\"_blank\"><img src=\"/images/file_download.png\"/></a><br>" + item.CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                        desc.attachment_tr += "</tr>";
                        rowCount--;
                    }
                }
                desc.result = "OK";
            }
            var resultXml = JsonConvert.SerializeObject(desc);
            return Ok(resultXml);
        }

        // POST api/<IssueController>
        [HttpPost]
        public ActionResult<SoapResult> Post([FromBody] SoapIssue value) {
            SoapResult result = new SoapResult("Invalid Request.");
            if (value != null) {
                switch (value.Action) {
                    case "create_issue": // 新增
                        if (value.module_id > 0) {
                            /* ==========================================================================================
                             * 新增程序逻辑说明
                             * cache_position = 查询前一条记录.Position
                             * 更新Position = Position + 1，条件为.Position > cacha_position && module_id = value.module_id
                             * 当前记录.Position = cache_position + 1
                             * 新增当前记录 db.savechanges()
                             */
                            Issue prevIssue = null;
                            int before_position = 0;

                            // 调整排序
                            if (value.prev_issue_id > 0) {
                                prevIssue = _context.Issues.FirstOrDefault(t => t.Id == value.prev_issue_id);
                                before_position = _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(value.module_id.ToString()) && t.CustomizedId == prevIssue.Id).Position.Value;

                                // 更新排序position+1
                                _context.CustomValues.Where(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(value.module_id.ToString()) && t.Position > before_position).Update(t => new CustomValue { Position = t.Position + 1 });
                            }

                            // 新增Issue
                            value.Issue.PriorityId = 2;
                            value.Issue.CreatedOn = DateTime.Now;
                            value.Issue.UpdatedOn = DateTime.Now;
                            value.Issue.CategoryId = 0;
                            value.Issue.StatusId = 1;
                            value.Issue.TrackerId = 2;      // 需求
                            value.Issue.ProjectId = 5;      // 3.0项目
                            value.Issue.Lft = 1;
                            value.Issue.Rgt = 2;
                            _context.Issues.Add(value.Issue);
                            _context.SaveChanges();         // 先保存到数据库，获取Issue.id

                            // 新增custom_value，排序=before_position+1
                            var new_custom_value = new CustomValue();
                            new_custom_value.CustomFieldId = 54;
                            new_custom_value.CustomizedType = "Issue";
                            new_custom_value.CustomizedId = value.Issue.Id;
                            new_custom_value.Value = value.module_id.ToString();
                            new_custom_value.Position = before_position + 1;
                            _context.CustomValues.Add(new_custom_value);
                            _context.SaveChanges();
                            
                            result.Result = "OK";
                        } else {
                            result.Result = "module id is null.";
                        }
                        break;
                    case "edit_issue":   // 修改
                        var issue = _context.Issues.FirstOrDefault(t => t.Id == value.Issue.Id);
                        string old_subject = issue.Subject;
                        issue.Subject = value.Issue.Subject;
                        issue.UpdatedOn = DateTime.Now;

                        Journal edit_journal = new Journal();
                        edit_journal.CreatedOn = DateTime.Now;
                        edit_journal.JournalizedId = value.Issue.Id;
                        edit_journal.JournalizedType = "Issue";
                        edit_journal.UserId = value.Issue.AuthorId;
                        _context.Journals.Add(edit_journal);
                        _context.SaveChanges();

                        JournalDetail issue_journal_detail = new JournalDetail();
                        issue_journal_detail.JournalId = edit_journal.Id;
                        issue_journal_detail.Property = "attr";
                        issue_journal_detail.PropKey = "subject";
                        issue_journal_detail.OldValue = old_subject;
                        issue_journal_detail.Value = value.Issue.Subject;
                        _context.JournalDetails.Add(issue_journal_detail);
                        _context.SaveChanges();

                        result.Result = "OK";
                        result.Value = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                        break;
                    case "move_up":
                        // 当前位置
                        CustomValue upValue = _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.CustomizedId == value.Issue.Id);
                        if (upValue.Position > 1) {
                            // 交换次序
                            _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(value.module_id.ToString()) && t.Position == upValue.Position - 1).Position = upValue.Position;
                            upValue.Position = upValue.Position - 1;
                            _context.SaveChanges();
                            result.Result = "OK";
                        } else {
                            result.Result = "Ingore";
                        }
                        break;
                    case "move_down":
                        CustomValue downValue = _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.CustomizedId == value.Issue.Id);
                        int module_count = _context.CustomValues.Count(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(value.module_id.ToString()));
                        if (downValue.Position < module_count) {
                            // 交换次序
                            _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(value.module_id.ToString()) && t.Position == downValue.Position + 1).Position = downValue.Position;
                            downValue.Position = downValue.Position + 1;
                            _context.SaveChanges();
                            result.Result = "OK";
                        } else {
                            result.Result = "Ingore";
                        }
                        break;
                    case "update_desc":
                        Issue edit_issue = _context.Issues.FirstOrDefault(t => t.Id == value.Issue.Id);                        
                        string old_value = edit_issue.Description;
                        edit_issue.Description = value.Issue.Subject;   // subject==>description

                        Journal journal = new Journal();
                        journal.CreatedOn = DateTime.Now;
                        journal.JournalizedType = "Issue";
                        journal.JournalizedId = value.Issue.Id;
                        journal.UserId = value.Issue.AuthorId;

                        _context.Journals.Add(journal);
                        _context.SaveChanges();

                        JournalDetail journalDetail = new JournalDetail();
                        journalDetail.JournalId = journal.Id;
                        journalDetail.Property = "attr";
                        journalDetail.PropKey = "description";
                        journalDetail.OldValue = old_value;
                        journalDetail.Value = edit_issue.Description;
                        _context.JournalDetails.Add(journalDetail);
                        _context.SaveChanges();

                        result.Result = "OK";
                        break;
                    case "add_notes":
                        Journal notes = new Journal();
                        notes.CreatedOn = DateTime.Now;
                        notes.JournalizedType = "Issue";
                        notes.JournalizedId = value.Issue.Id;
                        notes.UserId = value.Issue.AuthorId;
                        notes.Notes = value.Issue.Subject;  // subject ==> notes
                        _context.Journals.Add(notes);
                        _context.SaveChanges();

                        result.Result = "OK";
                        break;
                    case "move_to_module":
                        if (_context.Modules.Any(t => t.Id == value.module_id) == false) {
                            result.Result = "模块id[" + value.module_id + "]不存在。";
                        } else {
                            // 获取需调整的Issue
                            CustomValue customValue = _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.CustomizedId == value.Issue.Id );
                            _context.CustomValues.Where(t => t.CustomFieldId == 54 && t.Value.Equals(customValue.Value) && t.Position > customValue.Position).Update(t => new CustomValue { Position = t.Position - 1 });

                            // 统计目标模块下的Issue数量
                            int issueCount = _context.CustomValues.Count(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(value.module_id.ToString()));
                            customValue.Position = issueCount + 1;
                            customValue.Value = value.module_id.ToString();

                            _context.SaveChanges();
                            result.Result = "OK";
                        }
                        break;
                    case "update_issue_name":
                        var update_item = _context.Issues.FirstOrDefault(t => t.Id == value.Issue.Id);
                        if (update_item != null) {
                            update_item.Subject = value.Issue.Subject;
                            _context.SaveChanges();
                            result.Result = "OK";
                        } else {
                            result.Result = "Ingore";
                        }
                        break;
                }
            } else {
                result.Result = "SoapIssue is null!";
            }
            return Ok(result);
        }

        // PUT api/<IssueController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/<IssueController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}

        //[HttpPost("authentication")]
        //public ActionResult<string> AddIssue(Issue issue) {
        //    return "aaa_";
        //}
        //[HttpPost("authentication")]
        //public ActionResult<string> UpdateIssue(int id,string subject) {
        //    return "aaa_" + id;
        //}

    }
}
