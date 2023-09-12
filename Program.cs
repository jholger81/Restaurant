using System.Runtime.Serialization;
using Microsoft.AspNetCore.Authentication;

namespace Restaurant
{
    public class Program
    {
        static Settings settings = new Settings();
        public static void Main(string[] args)
        {
            settings = settings.Load();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
           
            builder.Services.AddWindowsService();

            //Basic Authentication
            app.UseMiddleware<AuthenticationMiddleware>();

            if (!settings.Port.HasValue)
                throw new Exception("No port configured");
            else
                app.Run($"https://localhost:{settings.Port.Value.ToString()}");
                //app.Run($"https://192.168.1.81:{settings.Port.Value.ToString()}");
        }
    }
}