using Otus_SocialNetwork.Extensions;
using Otus_SocialNetwork.Features.Users.Services;
using OtusSocialNetwork.Database;
using OtusSocialNetwork.Filters;
using OtusSocialNetwork.Middlewares;

namespace Otus_SocialNetwork
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();

            var app = builder.Build();

            app.ConfigureApp();

            app.Run();
        }

        private static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddControllersAsServices()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressInferBindingSourcesForParameters = true;
                });
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add services to the container.
            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
            builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
            builder.Services.AddSwagger(builder.Environment);
        }

        private static void ConfigureApp(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseCors();
            app.MapControllers();
        }
    }
}