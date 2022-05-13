using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MyGarage.Controllers;

[Authorize]
[ApiController]
[Route("api/file")]
public class UploadFilesController : AuthorizedApiController
{
    private AppSettings _appSettings;

    public UploadFilesController(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFileToS3(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString(); // GenerateS3FileName(file.FileName);

        try
        {
            using (var client = new AmazonS3Client(_appSettings.AWSAccessKey, _appSettings.AWSSecretKey, RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(newMemoryStream);
                
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = fileName, // filename
                        BucketName = _appSettings.AWSBucket // bucket name of S3
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Ok(new
        {
            fileId = fileName
        });
    }
}