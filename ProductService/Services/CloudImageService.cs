using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ProductService.Services;

public interface ICloudImageService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName);
}


public class CloudImageService : ICloudImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudImageService(IConfiguration configuration)
    {
        var settings = configuration.GetSection("Cloudinary");
        var account = new Account(
            settings["CloudName"],
            settings["ApiKey"],
            settings["ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true; // Sử dụng HTTPS
    }
    public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new ArgumentException("File stream is empty", nameof(fileStream));
        }
        var uploadParam = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = "product-images", // Tùy chọn: lưu trong thư mục riêng trên
            UseFilename = true, // Giữ nguyên tên file
            UniqueFilename = false // Không tạo tên file duy nhất

        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParam);
        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"Image upload failed: {uploadResult.Error?.Message}");
        }
        return uploadResult.SecureUrl.ToString(); // trả về URL của ảnh đã upload lên Cloudinary

    }
}