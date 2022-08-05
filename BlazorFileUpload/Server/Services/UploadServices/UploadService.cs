using BlazorFileUpload.Server.Data;
using BlazorFileUpload.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorFileUpload.Server.Services.UploadServices
{
    public class UploadService : IUploadService
    {
        private readonly DataContext _context;

        public UploadService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<UploadResult>>> getAllFilesFromDB()
        {
            var response = new ServiceResponse<List<UploadResult>>
            {
                Data = await _context.UploadResults.ToListAsync()
            };
            return response;
        }
    }
}
