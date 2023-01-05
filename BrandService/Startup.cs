using System.Text.Json.Serialization;
using BrandService.Utils;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BrandService;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Brand Service API", Version = "v1" });
        });

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                "Server=172.17.0.7,1433;Database=BrandServices;User=sa;Password=Passw0rd123.?!;Encrypt=True;TrustServerCertificate=True");
        });

        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthentication();

        //app.UseAuthorization();

        app.UseHttpsRedirection();

        app.UseSwagger();

        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Brand Service API V1"); });

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}