using FleetManagementLibrary;
using FleetManagementShared;
using Microsoft.OpenApi.Models;


namespace FleetManagementServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register WebSocketManager as a singleton
            builder.Services.AddSingleton<WebSocketsManager>();

            // Add Swagger services
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fleet Management", Version = "v1" });
            });

            // Build configuration
            IConfiguration configuration = builder.Configuration;
            string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection");
            string pgPassword = Environment.GetEnvironmentVariable("PG_PASSWORD");
            string connectionString = connectionStringTemplate.Replace("%PG_PASSWORD%", pgPassword);

            // Register EndpointsRepository with WebSocketManager dependency
            builder.Services.AddScoped<EndpointsRepository>(serviceProvider =>
            {
                var webSocketManager = serviceProvider.GetRequiredService<WebSocketsManager>();
                return new EndpointsRepository(connectionString, webSocketManager);
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            // Enable WebSocket support
            app.UseWebSockets();

            app.MapControllers();

            app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var webSocketManager = context.RequestServices.GetRequiredService<WebSocketsManager>();
                    await webSocketManager.HandleWebSocketAsync(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            });

            app.Run();
        }
    }
}