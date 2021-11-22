using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RubyMine.Customs.Models;
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
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public ModuleController(ILogger<ModuleController> logger, RubyMine.DbContexts.RubyRemineDbContext context) {
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
            string soapResult = "Invalid request.";
            Module current_module = null;
            if (module.Id > 0 && module.Parent_id > 0) {
                switch (module.Action) {
                    case "up":  // 移动节点
                    case "down":
                        current_module = _context.Modules.FirstOrDefault(t => t.Id == module.Id);
                        if (current_module == null) {
                            soapResult = "Invalid module.";
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
                                        soapResult = "Current node is on top.";
                                    }
                                    break;
                                case "down":
                                    var count = _context.Modules.Count(t => t.PId == module.Parent_id);
                                    if (current_module.Index < count) {
                                        toModule = _context.Modules.Where(t => t.PId == module.Parent_id).OrderBy(t => t.Index).FirstOrDefault(t => t.Index == currentIndex + 1);
                                        toModule.Index = currentIndex;

                                        current_module.Index++;
                                    } else {
                                        soapResult = "Current node is on bottom.";
                                    }
                                    break;
                            }
                            _context.SaveChanges();
                            soapResult = "OK";
                        }
                        break;
                    case "default_index":   // 重置模块下的节点次序
                        var modules = _context.Modules.Where(t => t.PId == module.Id);
                        int module_index = 1;
                        foreach (Module temp_module in modules) {
                            temp_module.Index = module_index++;
                        }
                        _context.SaveChanges();
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
                        }
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
