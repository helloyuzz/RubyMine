using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyMine.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.DIP {
    public interface IUploadFile {
        void Upload(List<IFormFile> files, int issue_id, int user_id);
        //(string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory);
        string SizeNormal(long bytes);
    }
}
