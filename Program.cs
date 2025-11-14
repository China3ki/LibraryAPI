
using LibraryAPI.Controllers;
using LibraryAPI.Entities;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace LibraryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Cors policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // SQL version
            var version = new MySqlServerVersion(new Version(10, 4, 32));

            // DB context
            builder.Services.AddDbContext<LibraryContext>(option =>
                option.UseMySql(builder.Configuration.GetConnectionString("Default"), version)
            );

            // Context
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<HasherService>();
            builder.Services.AddSingleton<UploadService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            // Static files
            app.UseStaticFiles();
            app.MapStaticAssets();
            app.UseDefaultFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
