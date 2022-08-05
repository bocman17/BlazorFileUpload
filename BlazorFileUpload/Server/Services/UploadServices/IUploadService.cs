using BlazorFileUpload.Shared;

namespace BlazorFileUpload.Server.Services.UploadServices
{
    public interface IUploadService
    {
        Task<ServiceResponse<List<UploadResult>>> getAllFilesFromDB();
    }
}
