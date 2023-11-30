
using Microsoft.AspNetCore;

namespace DbfReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            options.AddPolicy(name: "_myAllowSpecificOrigin",
            policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            }));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<DbfReader.Services.AbstractDbfReader>(x => new DbfReader.Services.AbstractDbfReader(builder.Configuration[$"Bases:default"] ?? ""));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("_myAllowSpecificOrigin");
          
            app.MapControllers();

            app.Run();
        }
    }
}