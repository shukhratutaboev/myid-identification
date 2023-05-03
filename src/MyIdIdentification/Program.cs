using Microsoft.EntityFrameworkCore;
using MyIdIdentification.Context;
using MyIdIdentification.Middlewares;
using MyIdIdentification.Options;
using MyIdIdentification.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMyIdService, MyIdService>();
builder.Services.AddHttpClient();
builder.Services.Configure<Urls>(builder.Configuration.GetSection("MyIdUrls"));
builder.Services.AddDbContext<IdentificationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseErrorHandlerMiddleware();

app.MapControllers();

app.Run();
