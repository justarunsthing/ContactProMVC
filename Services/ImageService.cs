using ContactProMVC.Interaces;

namespace ContactProMVC.Services
{
    public class ImageService : IImageService
    {
        public string ConvertByteArrayToFile(byte[] fileData, string extension)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}