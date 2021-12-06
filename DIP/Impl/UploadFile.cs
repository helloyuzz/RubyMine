using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RubyMine.DbContexts;
using RubyMine.Jwt;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.DIP.Impl {
    public class UploadFile:IUploadFile {
        #region Property  
        private IHostingEnvironment _hostingEnvironment;
        private RubyRemineDbContext _context;
        private IConfiguration _config;
        #endregion

        #region Constructor  
        public UploadFile(IHostingEnvironment hostingEnvironment, RubyRemineDbContext context,IConfiguration config) {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _config = config;
        }
        #endregion

        #region Upload File  
        public void Upload(List<IFormFile> files, int issue_id, int user_id) {
            //subDirectory = subDirectory ?? string.Empty;
            //var target = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);
            //Directory.CreateDirectory(target);

            string path = _config["AppSettings:UploadPath"];

            var disk_directory = DateTime.Now.ToString("yyyy/MM");
            path += "\\" + disk_directory;
            if (Directory.Exists(path) == false) {
                Directory.CreateDirectory(path);
            }

            files.ForEach(async file => {
                if (file.Length <= 0) return;

                var fileName = file.FileName;
                var fileExt = fileName.Substring(fileName.LastIndexOf("."));
                var disk_Filename = DateTime.Now.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString().Replace("-", "") + fileExt;
                var fileSize = file.Length;
                var fileHash = Guid.NewGuid().ToString().Replace("-", "");

                var filePath = Path.Combine(path, disk_Filename);
                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    file.CopyTo(stream);

                    Journal journal = new Journal();
                    journal.JournalizedId = issue_id;
                    journal.JournalizedType = "Issue";
                    journal.UserId = user_id;
                    journal.CreatedOn = DateTime.Now;
                    _context.Journals.Add(journal);
                    _context.SaveChanges();

                    JournalDetail journalDetail = new JournalDetail();
                    journalDetail.JournalId = journal.Id;
                    journalDetail.Property = "attachment";
                    journalDetail.PropKey = journal.Id.ToString();
                    journalDetail.Value = fileName;
                    _context.JournalDetails.Add(journalDetail);

                    Attachment attachment = new Attachment();
                    attachment.ContainerId = issue_id;
                    attachment.AuthorId = user_id;
                    attachment.ContainerType = "Issue";
                    attachment.Filename = file.FileName;
                    attachment.DiskFilename = disk_Filename;
                    attachment.ContentType = file.ContentType;
                    attachment.Digest = fileHash;
                    attachment.CreatedOn = DateTime.Now;
                    attachment.DiskDirectory = disk_directory;
                    attachment.Filesize = file.Length;
                    _context.Attachments.Add(attachment);
                    _context.SaveChanges();

                }
            });
        }
        #endregion

        #region Download File  
        //public (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory) {
        //    var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

        //    var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory)).ToList();

        //    using (var memoryStream = new MemoryStream()) {
        //        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true)) {
        //            files.ForEach(file => {
        //                var theFile = archive.CreateEntry(file);
        //                using (var streamWriter = new StreamWriter(theFile.Open())) {
        //                    streamWriter.Write(File.ReadAllText(file));
        //                }
        //            });
        //        }

        //        return ("application/zip", memoryStream.ToArray(), zipName);
        //    }

        //}
        #endregion

        #region Size Converter  
        public string SizeNormal(long bytes) {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            switch (fileSize) {
                case var _ when fileSize < kilobyte:
                    return $"1KB";
                case var _ when fileSize < megabyte:
                    return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
                case var _ when fileSize < gigabyte:
                    return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
                case var _ when fileSize >= gigabyte:
                    return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
                default:
                    return "n/a";
            }
        }
        #endregion
    }
}
