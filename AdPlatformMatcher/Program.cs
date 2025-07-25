
using AdPlatformMatcher.Controllers;
using AdPlatformMatcher.Helpers;
using AdPlatformMatcher.Services;
using Microsoft.AspNetCore.Antiforgery;

namespace AdPlatformMatcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IPlatformService, PlatformService>();
            builder.Services.AddSingleton<IFileHelper, FileHelper>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           

            app.MapAdPlatformEndpoints();
            app.MapHealthEndpoints();
            

            app.Run();
        }
    }
}
