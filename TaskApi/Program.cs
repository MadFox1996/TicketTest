using AutoMapper;
using Microsoft.Extensions.Options;
using TaskApi.AutoMapper;
using TaskApi.BLL.Automapper;
using TaskApi.BLL.Infrastructure;
using TaskApi.BLL.Interfaces;
using TaskApi.BLL.Services;
using TaskBLL.Configuration;

namespace TaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuration init
            var minioConfiguration = builder.Configuration.GetSection(nameof(MinioConfiguration));
            builder.Services.Configure<MinioConfiguration>(minioConfiguration);

            // Add application services to the container
            AddApplicationServices(builder);

            // AutoMapper init
            AutoMapperInit(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void AddApplicationServices(WebApplicationBuilder? builder)
        {
            var minioConfiguration = new MinioConfiguration();
            builder.Configuration.Bind(nameof(MinioConfiguration), minioConfiguration);
            IOptions<MinioConfiguration> minioOptions = Options.Create(minioConfiguration);

            builder.Services.AddBLLServices(minioOptions);
            builder.Services.AddTransient<ITicketService, TicketService>();
        }

        private static void AutoMapperInit(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TaskApiMappingProfile());
                mc.AddProfile(new BLLMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
