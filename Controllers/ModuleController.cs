using Microsoft.AspNetCore.Mvc;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RubyMine.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase {
        private readonly ILogger<ModuleController> _logger;
        private readonly RubyRemineDbContext _context;

        public ModuleController(ILogger<ModuleController> logger, RubyRemineDbContext context) {
            _context = context;
            _logger = logger;
        }
        //// GET: api/<ModuleController>
        //[HttpGet]
        //public IEnumerable<string> Get() {
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ModuleController>/5
        //[HttpGet("{id}")]
        //public string Get(int id) {
        //    return "value";
        //}

        // POST api/<ModuleController>
        [HttpPost]
        public IActionResult Post([FromBody] SwapModule module) {
            SoapResult soapResult = new SoapResult("Invalid request.");
            Module current_module = null;
            if (module.Parent_id == -1 || (module.Id > 0 && module.Parent_id > 0)) {
                switch (module.Action) {
                    case "up":  // 移动节点
                    case "down":
                        current_module = _context.Modules.FirstOrDefault(t => t.Id == module.Id);
                        if (current_module == null) {
                            soapResult.Result = "Invalid module.";
                        } else {
                            int currentIndex = current_module.Index.Value;
                            Module toModule = null;
                            switch (module.Action) {
                                case "up":
                                    if (current_module.Index > 1) {
                                        toModule = _context.Modules.Where(t => t.PId == module.Parent_id).OrderBy(t => t.Index).FirstOrDefault(t => t.Index == currentIndex - 1);
                                        toModule.Index = currentIndex;

                                        current_module.Index--;
                                    } else {
                                        soapResult.Result = "Current node is on top.";
                                    }
                                    break;
                                case "down":
                                    var count = _context.Modules.Count(t => t.PId == module.Parent_id);
                                    if (current_module.Index < count) {
                                        toModule = _context.Modules.Where(t => t.PId == module.Parent_id).OrderBy(t => t.Index).FirstOrDefault(t => t.Index == currentIndex + 1);
                                        toModule.Index = currentIndex;

                                        current_module.Index++;
                                    } else {
                                        soapResult.Result = "Current node is on bottom.";
                                    }
                                    break;
                            }
                            _context.SaveChanges();
                            soapResult.Result = "OK";
                        }
                        break;
                    case "default_module_index":   // 重置模块下的节点次序
                        var modules = _context.Modules.Where(t => t.PId == module.Id);
                        if (modules.Count() > 0) {
                            int module_index = 1;
                            foreach (Module temp_module in modules) {
                                temp_module.Index = module_index++;
                            }
                            _context.SaveChanges();
                            soapResult.Result = "OK";
                        } else {
                            soapResult.Result = "Ingore";
                        }
                        break;
                    case "default_issue_index": // 重置下级issue排序
                        int issue_index = 1;
                        var module_issues = _context.CustomValues.Where(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(module.Id.ToString()));
                        foreach (CustomValue customValue in module_issues) {
                            customValue.Position = issue_index++;
                        }
                        _context.SaveChanges();
                        soapResult.Result = "OK";
                        break;
                    case "up_level":    // 上提一级
                        var up_level_module = _context.Modules.FirstOrDefault(t => t.Id == module.Parent_id);
                        if (up_level_module.PId > 0) {  // 非一级节点
                            current_module = _context.Modules.FirstOrDefault(t => t.Id == module.Id);
                            // current_module后续的Index-1
                            // up_level_module后续的index+1
                            // current_module.pid=up_level_module.pid
                            // current_module.index = up_module.index+1
                            _context.Modules.Where(t => t.PId == module.Parent_id && t.Index > current_module.Index).Update(t => new Module { Index = t.Index - 1 });
                            _context.Modules.Where(t => t.PId == up_level_module.PId && t.Index > up_level_module.Index).Update(t => new Module { Index = t.Index + 1 });
                            current_module.PId = up_level_module.PId;
                            current_module.Index = up_level_module.Index + 1;
                            _context.SaveChanges();
                            soapResult.Result = "OK";
                        }
                        break;
                    case "down_level":  // 下降一级
                        current_module = _context.Modules.FirstOrDefault(t => t.Id == module.Id);
                        // 当前节点是第一位，无需调整
                        // 当前节点.parent_id = 前一节点.id
                        // 当前节点.index = 前一节点.nodeCount() + 1 // 最为最后节点添加
                        // 当前节点后续节点.index - 1
                        if (current_module.Index > 1) {
                            var cache_pid = current_module.PId;
                            var cache_index = current_module.Index;

                            var prev_module = _context.Modules.FirstOrDefault(t => t.PId == module.Parent_id && t.Index == current_module.Index - 1);
                            var db_count = _context.Modules.Count(t => t.PId == prev_module.Id);
                            current_module.PId = prev_module.Id;
                            current_module.Index = db_count + 1;

                            _context.Modules.Where(t => t.PId == cache_pid && t.Index > cache_index).Update(t => new Module { Index = t.Index - 1 });
                            _context.SaveChanges();
                            soapResult.Result = "OK";
                        } else {
                            soapResult.Result = "Ingore";
                        }
                        break;
                    case "update_module_name":
                        _context.Modules.FirstOrDefault(t => t.Id == module.Id).Name = module.Name;
                        _context.SaveChanges();
                        soapResult.Result = "OK";
                        break;
                    case "create_module":
                        int dbCount = _context.Modules.Count(t => t.PId == module.Id) + 1;
                       
                        Module newModule = new Module();
                        newModule.PId = module.Id;
                        newModule.Index = dbCount;
                        newModule.Name = "node-" + dbCount;
                        _context.Modules.Add(newModule);
                        _context.SaveChanges();

                        soapResult.Result = "OK";
                        soapResult.Value = JsonConvert.SerializeObject(newModule);
                        break;
                    case "show_module_true":
                    case "show_module_false":
                        ActiveNodes activeNodes = GlobalCache.ActiveNodes.FirstOrDefault(t => t.Key == module.User_id).Value;
                        if (activeNodes == null) {
                            activeNodes = new ActiveNodes();
                            GlobalCache.ActiveNodes.Add(module.User_id, activeNodes);
                        }
                        if (activeNodes.Contains(module.Id) == false) {
                            activeNodes.Add(module.Id);
                        }
                        if (module.Action.Equals("show_module_false") && activeNodes.Contains(module.Id)) {
                            activeNodes.Remove(module.Id);
                        }
                        soapResult.Result = "OK";
                        break;
                }
            }
            
            return Ok(soapResult);
        }

        //// PUT api/<ModuleController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/<ModuleController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}
    }
}
