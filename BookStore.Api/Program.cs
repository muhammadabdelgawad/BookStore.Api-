
using Asp.Versioning;
using BookStore.Api.Helper;
using Data_Access.Data;
using Data_Access.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add MemoryCaching 
            builder.Services.AddMemoryCache();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // Add API Versioning

            builder.Services.AddApiVersioning(options =>
            {
                      // Configure API versioning options
                      options.DefaultApiVersion = new ApiVersion(1); // Default version
                      options.AssumeDefaultVersionWhenUnspecified = true; // Assume default version when not specified
                      options.ReportApiVersions = true;   // Report API versions in response headers 

                      options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
                     //  For Swagger support with API Versioning
                     .AddApiExplorer(options =>
                     {
                         options.GroupNameFormat = "'v'V";
                         options.SubstituteApiVersionInUrl = true;
                     });
             

            builder.Services.AddResponseCaching();//Response caching
            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Defualt60", new CacheProfile
                {
                    Duration =60,
                    Location = ResponseCacheLocation.Client,
                    NoStore = false
                });
                options.CacheProfiles.Add("Short30", new CacheProfile
                {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any,
                });
            });
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseResponseCaching(); // Enable response caching

            app.MapControllers();

            app.Run();
        }
    }
}
