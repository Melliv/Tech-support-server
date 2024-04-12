using BLL.App;
using Contract.DAL.App;
using Contracts.BLL.App;
using DAL.APP.EF;
using Microsoft.EntityFrameworkCore;
using Tech_support_server.Middlewares;
using WebSocket;

namespace Tech_support_server;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options
                .UseInMemoryDatabase("OnMemoryConnection")
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
        );
        // services.AddCors(options =>
        //     {
        //         options.AddPolicy("CorsAllowAll", builder =>
        //         {
        //             builder.AllowAnyHeader();
        //             builder.AllowAnyMethod();
        //             builder.AllowAnyOrigin();
        //         });
        //     }
        // );
        
        services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
        services.AddScoped<IAppBLL, AppBLL>();
        services.AddScoped<NotificationHub>();
        
        services.AddAutoMapper(
            typeof(DAL.App.DTO.MappingProfiles.AutoMapperProfile),
            typeof(BLL.App.DTO.MappingProfiles.AutoMapperProfile),
            typeof(DTO.App.V1.MappingProfiles.AutoMapperProfile)
        );
        
        services.AddSwaggerGen();
        services.AddControllers();
        services.AddSignalR();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // app.UseHttpsRedirection();

        app.UseSwagger();
        app.UseSwaggerUI();
        
        // app.UseCors("CorsAllowAll");
        app.UseRouting();

        app.UseExceptionHandlerMiddlewareExtensionHandler();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("/notifications");
        });
    }

}