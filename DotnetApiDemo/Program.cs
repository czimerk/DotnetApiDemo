using GraphQLDemo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var optionsBuilder = new DbContextOptionsBuilder<DemoContext>().UseInMemoryDatabase("test");
var _options = optionsBuilder.Options;
builder.Services.AddSingleton<DbContextOptions<DemoContext>>(_options);
builder.Services.AddSingleton<DemoContext>(new DemoContext(_options));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var ctx = app.Services.GetService<DemoContext>();
ctx.Seed();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
