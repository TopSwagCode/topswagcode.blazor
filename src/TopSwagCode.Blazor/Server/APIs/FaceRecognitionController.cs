using AntDesign;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TopSwagCode.Blazor.Server.Services;
using TopSwagCode.Blazor.Shared;

namespace TopSwagCode.Blazor.Server.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class FaceRecognitionController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FaceRecognitionController> _logger;
        private readonly IComputerVisionService _computerVisionService;
        private readonly IHub _sentryHub;

        public FaceRecognitionController(IWebHostEnvironment webHostEnvironment, ILogger<FaceRecognitionController> logger, IComputerVisionService computerVisionService, IHub sentryHub)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _computerVisionService = computerVisionService;
            _sentryHub = sentryHub;
        }

        [HttpPost]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(
        [FromForm] IEnumerable<IFormFile> files)
        {
            var maxAllowedFiles = 1;
            long maxFileSize = 1024 * 1024 * 15;
            var filesProcessed = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> uploadResults = new();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        _logger.LogInformation("{FileName} length is 0 (Err: 1)", file.FileName);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        _logger.LogInformation("{FileName} of {Length} bytes is larger than the limit of {Limit} bytes (Err: 2)",
                            file.FileName, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            byte[] imageData = ms.ToArray();

                            var fileGuid = await _computerVisionService.FaceRec(imageData);

                            uploadResult.Uploaded = true;
                            uploadResult.FileGuid = fileGuid;
                        }
                        catch (IOException ex)
                        {
                            _logger.LogError("Rrror on upload (Err: 3): {Message}",
                                ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    _logger.LogInformation("Not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }

                uploadResult.FileResultUrl = $"/api/facerecognition/{uploadResult.FileGuid}";

                uploadResults.Add(uploadResult);
            }

            return new CreatedResult(resourcePath, uploadResults);
        }
    
        [HttpGet("{imgGuid}")]
        public async Task<ActionResult> GetImageByGuid(string imgGuid)
        {
            var image = System.IO.File.OpenRead($"{imgGuid}.jpg");
            return File(image, "image/jpeg");

            return Ok();
        }

        [HttpPost("test")]
        public async Task<ActionResult> testupload([FromForm] IEnumerable<IFormFile> files)
        {
            var file = files.First();

            var uploadResult = new UploadResult();

            try
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                byte[] imageData = ms.ToArray();

                var fileGuid = await _computerVisionService.FaceRec(imageData);

                uploadResult.Uploaded = true;
                uploadResult.FileGuid = fileGuid;
            }
            catch (IOException ex)
            {
                _logger.LogError("Rrror on upload (Err: 3): {Message}",
                    ex.Message);
                uploadResult.ErrorCode = 3;
            }

            uploadResult.FileResultUrl = $"/api/facerecognition/{uploadResult.FileGuid}";
      
            return Ok(new ResponseModel()
            {
                name = uploadResult.FileGuid,
                thumbUrl = uploadResult.FileResultUrl,
                status = "Ok",
                url = uploadResult.FileResultUrl
            });
        }
    
    }

    public class ResponseModel
    {
        public string name { get; set; }

        public string status { get; set; }

        public string url { get; set; }

        public string thumbUrl { get; set; }
    }

}
