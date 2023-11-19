using gRPCServerApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcArticleService>();

    endpoints.MapGet("/protos/article.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/article.proto"));
    });
});

app.UseAuthorization();

app.MapControllers();

app.Run();
