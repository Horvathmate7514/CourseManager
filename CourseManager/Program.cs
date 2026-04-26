using CourseManager.DataContext.Context;
using CourseManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;

namespace CourseManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<NeptunDbContext>(options =>
                options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CourseManagerDB_LIOA2U;Trusted_Connection=True;TrustServerCertificate=True;"));

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Ez a sor mondja meg a .NETnek h az Enumokat szövegként  is fogadja be és adja ki
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CourseManager API",
                    Version = "v1",
                });
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CourseManager.DataContext.Context.NeptunDbContext>();
                    await DataContext.Context.Seeders.DataSeeder.SeedAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hiba történt az adatbázis feltöltésekor: " + ex.Message);
                }
            }

            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseManager API v1");


                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}