using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System.Text;
using TaskApi.BLL.Dto;
using TaskApi.BLL.Interfaces;
using TaskBLL.Configuration;

namespace TaskApi.BLL.Services
{
    public class MinioFileStorage : IFileStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly IFileStorageHelper _minioHelper;
        private readonly string _bucketName;
        
        public MinioFileStorage(IMinioClient minioClient, IOptions<MinioConfiguration> options, IFileStorageHelper dateHelper)
        {
            _minioClient = minioClient;
            _bucketName = options.Value.Bucket;
            _minioHelper = dateHelper;
        }
        public async Task<IEnumerable<TicketFileDto>> AddNewFiles(ICollection<IFormFile> files)
        {
            var taskFilesDto = new List<TicketFileDto>();
            foreach (var file in files)
            {
                try
                {
                    //Issue in minio. BucketExistsAsync returns true even if it does not exist 
                    var buckets = await _minioClient.ListBucketsAsync();
                    if (buckets.Buckets == null || !buckets.Buckets.Select(x => x.Name).Contains(_bucketName))
                    {
                        var mbArgs = new MakeBucketArgs()
                            .WithBucket(_bucketName)
                            .WithLocation("us-east-1");
                        await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                    }
                    
                    var newbuckets = await _minioClient.ListBucketsAsync();
                    // Upload a file to bucket.
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(_bucketName)
                        .WithObject(_minioHelper.GenerateFileName(file.FileName))
                        .WithObjectSize(file.Length)
                        .WithStreamData(file.OpenReadStream())
                        .WithContentType(file.ContentType);

                    var savedFile = await _minioClient.PutObjectAsync(putObjectArgs);
                    taskFilesDto.Add(new TicketFileDto
                    {
                        Id = _minioHelper.GetIdFromFilePath(savedFile.ObjectName),
                        FileName = _minioHelper.GetFileNameFromFilePath(savedFile.ObjectName),
                        Hash = _minioHelper.CalculateHash(file)
                    });
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return taskFilesDto;
        }

        public async Task<IEnumerable<TicketFileDto>> AddFiles(ICollection<IFormFile> files, Dictionary<string, string> fileHashes)
        {
            //Auxiliary dictionary with file names
            var fileNamesHash = fileHashes.Select(x => new { newKey = _minioHelper.GetFileNameFromFilePath(x.Key), val = x.Value })
                .ToDictionary(x => x.newKey, x => x.val);

            // New Files
            var filesToAdd = files.Where(x => !fileNamesHash.ContainsKey(x.FileName)).ToList();
            //Files To Update
            var filesToUpdate = files.Where(x => fileNamesHash.ContainsKey(x.FileName) && _minioHelper.CalculateHash(x) != fileNamesHash[x.FileName]).ToList();
            var fileNamesToUpdate = filesToUpdate.Select(x=>x.FileName).ToList();

            var filePathesForUpdate = fileHashes
                .Where(x => fileNamesToUpdate.Contains(_minioHelper.GetFileNameFromFilePath(x.Key)))
                .Select(x => x.Key)
                .ToList();

            var addedFiles = await AddNewFiles(filesToAdd);
            var updatedFiles = await UpdateFiles(filesToUpdate, filePathesForUpdate);

            return addedFiles.Concat(updatedFiles);
        }

        public async Task<IEnumerable<TicketFileDto>> UpdateFiles(ICollection<IFormFile> files, IList<string> filePathesForUpdate)
        {
            await RemoveFiles(filePathesForUpdate);
            return await AddNewFiles(files);
        }

        public async Task RemoveFiles(IList<string> files)
        {
            var error = new StringBuilder();
            try
            {
                if (files is not null && files.Count != 0)
                {
                    try
                    {
                        var objArgs = new RemoveObjectsArgs()
                            .WithBucket(_bucketName)
                            .WithObjects(files);
                        foreach (var objDeleteError in await _minioClient.RemoveObjectsAsync(objArgs).ConfigureAwait(false))
                            error.Append($"Object: {objDeleteError.Key}");
                        if (error.Length > 0) { 
                            throw new Exception(error.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
