using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SA.EntityFramework.EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Files")]
    public class FilesController : BaseController<Core.Model.File>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FilesController(
            IEntityRepository<Core.Model.File> repository,
            IWebHostEnvironment hostingEnvironment,
            IMapper mapper)
            : base(repository, mapper)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize("admin")]
        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadFiles")]
        public ActionResult UploadFiles()
        {
            try
            {
                var fileList = new List<Core.Model.File>();
                var fileData = Request.Form.Files;
                var rootPath = _hostingEnvironment.WebRootPath;
                var recordsPath = Path.Combine(rootPath, "tempFiles");

                if (!Directory.Exists(recordsPath))
                {
                    Directory.CreateDirectory(recordsPath);
                }

                if (fileData.Any())
                {
                    foreach (var file in fileData)
                    {
                        string fName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        string fullPath = Path.Combine(recordsPath, fName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            fileList.Add(new Core.Model.File
                            {
                                Created = DateTime.Now,
                                Name = fName,
                            });
                        }
                    }
                }
                return Json(fileList);

            }
            catch (Exception e)
            {
                return Json(new
                {
                    message = e.Message,
                    inner = e.InnerException,
                    stack = e.StackTrace,
                });
            }
        }

        [Authorize("admin")]
        [Route("insertFiles")]
        [HttpPost]
        public async Task<IActionResult> InsertFiles([FromBody] List<Core.Model.File> files)
        {
            foreach (var file in files)
            {
                await _repository.AddAsync(file);
            }
            return Json(true);
        }
    }
}