using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using TaskApi.BLL.Helpers;
using TaskApi.BLL.Interfaces;
using TaskApi.BLL.Services;
using TaskApi.DAL.Infrastructure;
using TaskApi.DAL.Interfaces;
using TaskApi.DAL.Repositories;
using TaskBLL.Configuration;

namespace TaskApi.BLL.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static void AddBLLServices(this IServiceCollection services, IOptions<MinioConfiguration> options)
        {
            var accessKey = options.Value.AccessKey;
            var secretKey = options.Value.SecretKey;
            var endpoint = options.Value.Endpoint;
            var bucket = options.Value.Bucket;

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .Build());

            services.AddDALServices();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IFileStorageService, MinioFileStorage>();
            services.AddScoped<IFileStorageHelper, MinioStorageHelper>();
        }
    }
}
