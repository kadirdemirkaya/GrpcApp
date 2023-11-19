using Consul;
using gRPCClientApp.Constants;
using gRPCClientApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IBookDataClient, BookDataClient>();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    var address = builder.Configuration["ConsulConfig:Address"];
    consulConfig.Address = new Uri(address);
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Inject(app, app.Services.GetRequiredService<IHostApplicationLifetime>());

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();


void Inject(IApplicationBuilder app, IHostApplicationLifetime lifetime)
{
    var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
    var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
    var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

    var registration = new AgentServiceRegistration
    {
        ID = grpcClientApp.Application.ID,
        Name = grpcClientApp.Application.Name,
        Address = grpcClientApp.Application.Host,
        Port = int.Parse(grpcClientApp.Application.Port),
        Tags = new[] { grpcClientApp.Application.Name, grpcClientApp.Application.Tag }
    };

    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
    consulClient.Agent.ServiceRegister(registration).Wait();

    lifetime.ApplicationStopped.Register(() =>
    {
        consulClient.Agent.ServiceDeregister(registration.ID);
    });
}