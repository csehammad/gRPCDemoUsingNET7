using gRPCUsingNET7Demo.Services;

namespace gRPCUsingNET7Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            // Add services to the container.
            builder.Services.AddGrpc().AddJsonTranscoding();
            builder.Services.AddGrpcReflection();
            builder.Services.AddGrpcSwagger();

            builder.Services.AddSwaggerGen( c=>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo { Title = "gRPC using .NET 7 Demo", Version = "v1" }


                    );

            });


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPC using .NET7 Demo");
            }
            );

            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGrpcService<SalesDataService>();
            app.MapGrpcReflectionService();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}