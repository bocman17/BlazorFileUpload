using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace BlazorFileUpload.Client.Services.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly HttpClient _httpClient;

        public UploadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<UploadResult> Files { get; set; } = new List<UploadResult>();

        public event Action OnChange;
        public string Message { get; set; } = "Loading files...";
        public List<string> FileNames { get; set; } = new List<string>();
        public List<UploadResult> UploadResults { get; set; } = new List<UploadResult>();

        public async Task GetFilesFromDb()
        {
            var result =
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<UploadResult>>>("/api/file");

            if (result != null && result.Data != null)
            {
                Files = result.Data;
            }
        }

        public async Task InputFileChange(InputFileChangeEventArgs e)
        {
            using var content = new MultipartFormDataContent();

            foreach (var file in e.GetMultipleFiles(int.MaxValue))
            {
                var fileContent = new StreamContent(file.OpenReadStream(long.MaxValue));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                FileNames.Add(file.Name);

                content.Add(
                    content: fileContent,
                    name: "\"files\"",
                    fileName: file.Name
                    );
            }

            var response = await _httpClient.PostAsync("/api/File", content);
            var newUploadResults = await response.Content.ReadFromJsonAsync<List<UploadResult>>();

            if (newUploadResults is not null)
            {
                Files = Files.Concat(newUploadResults).ToList();
            }

            OnChange.Invoke();
        }
    }
}
