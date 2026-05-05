var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll",policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//log 
app.Use(async (context, next) =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("==================== Incoming Request ====================");
    Console.WriteLine($"{context.Request.Method} {context.Request.Headers.Host}{context.Request.Path}");
    Console.ResetColor();
    await next();
});

// reverseproxy ở cuối
app.MapReverseProxy();


app.Run();
