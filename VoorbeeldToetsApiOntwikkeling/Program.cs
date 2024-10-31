
using VoorbeeldToetsApiOntwikkeling.Services;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace VoorbeeldToetsApiOntwikkeling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();

            //logging
            var logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341/")
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            builder.Logging.AddSerilog(logger);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IRealEstateData, InMemoryRealEstateData>();

            //ratelimiting
            builder.Services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "fixed", options =>
            {
                options.PermitLimit = 4; //number of requests allowed
                options.Window = TimeSpan.FromSeconds(4); //time window
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //process oldest requests first
                options.QueueLimit = 2; //maximum number of q'd requests
            }));

            var app = builder.Build();

            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
