using BlazorFileUpload.Shared;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorFileUpload.Client.Services.UploadService
{
    public interface IUploadService
    {
        event Action OnChange;
        List<UploadResult> Files { get; set; }
        Task GetFilesFromDb();
        Task InputFileChange(InputFileChangeEventArgs e);
        string Message { get; set; }
        List<string> FileNames { get; set; }
        List<UploadResult> UploadResults { get; set; }

    }
}
