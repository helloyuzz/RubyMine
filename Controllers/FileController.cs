using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyMine.DIP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase {
        private readonly IUploadFile _uploadFile;
        public FileController(IUploadFile uploadFile) {
            _uploadFile = uploadFile;
        }

        [HttpPost(nameof(Upload))]
        public IActionResult Upload([Required] List<IFormFile> formFiles, [Required] int issue_id,[Required] int user_id) {
            try {
                _uploadFile.Upload(formFiles, issue_id,user_id);

                return Ok(
                    new {
                        formFiles.Count,
                        Size = _uploadFile.SizeNormal(formFiles.Sum(f => f.Length))
                    });
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet(nameof(Download))]
        //public IActionResult Download([Required] string subDirectory) {

        //    try {
        //        var (fileType, archiveData, archiveName) = _uploadFile.DownloadFiles(subDirectory);

        //        return File(archiveData, fileType, archiveName);
        //    } catch (Exception ex) {
        //        return BadRequest(ex.Message);
        //    }

        //}
    }
}
