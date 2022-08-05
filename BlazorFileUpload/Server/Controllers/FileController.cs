using BlazorFileUpload.Server.Data;
using BlazorFileUpload.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlazorFileUpload.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;
        private readonly IUploadService _uploadService;

        public FileController(IWebHostEnvironment env, DataContext context, IUploadService uploadService)
        {
            _env = env;
            _context = context;
            _uploadService = uploadService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UploadResult>>>> GetAllFiles()
        {
            var result = await _uploadService.getAllFilesFromDB();
            return Ok(result);
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var uploadResult = await _context.UploadResults.FirstOrDefaultAsync(u =>
                u.StoredFileName.Equals(fileName));

            var path = Path.Combine(_env.ContentRootPath, "uploads", fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, uploadResult.ContentType, Path.GetFileName(path));
        }

        [HttpPost]
        public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
        {
            List<UploadResult> uploadResults = new List<UploadResult>();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;

                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResult.ContentType = file.ContentType;
                uploadResults.Add(uploadResult);

                _context.UploadResults.Add(uploadResult);
            }

            await _context.SaveChangesAsync();

            return Ok(uploadResults);
        }
    }
}
