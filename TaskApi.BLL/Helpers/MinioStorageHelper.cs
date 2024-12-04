using Microsoft.AspNetCore.Http;
using TaskApi.BLL.Interfaces;
using TaskApi.DAL.Interfaces;
using XSystem.Security.Cryptography;

namespace TaskApi.BLL.Helpers
{
    public class MinioStorageHelper : IFileStorageHelper
    {
        //Minio file path example:  04122024/dee1fb9e-3e12-4310-946c-d42ff5081e9e1.txt
        private const int OffsetForFileName = 45;
        private const int LeftOffsetForId = 9;
        private const int RightOffsetForId = 36;
        public const string BucketSeparator = "/";
        
        private readonly IDateHelper _dateHelper;

        public MinioStorageHelper(IDateHelper dateHelper)
        {
            _dateHelper = dateHelper;
        }

        public string GenerateFileName(string fileName)
        {
            return _dateHelper.Today + BucketSeparator + Guid.NewGuid() + fileName;
        }

        public string GetFileName(DateTime dateTime, Guid guid, string fileName)
        {
            return dateTime.ToString(_dateHelper.DateFormat) + BucketSeparator + guid.ToString() + fileName;
        }

        public string CalculateHash(IFormFile file)
        {
            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(file.OpenReadStream());
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }

        public string GetFileNameFromFilePath(string path)
        {
            return path[OffsetForFileName..];           
        }

        public Guid GetIdFromFilePath(string path)
        {
            return new Guid(path.Substring(LeftOffsetForId, RightOffsetForId));
        }
    }
}
