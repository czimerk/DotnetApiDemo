using DotnetApiDemo;
using DotnetApiDemo.Middleware;
using DotnetApiDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var optionsBuilder = new DbContextOptionsBuilder<DemoContext>()
    .UseInMemoryDatabase("test");
var _options = optionsBuilder.Options;
builder.Services
    .AddSingleton<DbContextOptions<DemoContext>>(_options);
builder.Services
    .AddSingleton<DemoContext>(new DemoContext(_options));

builder.Services.AddScoped<IArticleService, ArticleService>();

//JWT Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

var policy = new AuthorizationPolicyBuilder()
                     .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                     .RequireAuthenticatedUser()
                     .RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

builder.Services.AddAuthorization(opt =>
{
    opt.DefaultPolicy = policy.Build();
});

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new AuthorizeFilter(policy.Build()));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
var ctx = app.Services.GetService<DemoContext>();
ctx.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("ping"))
    {
        await context.Response.WriteAsync("pong");
    }
    else
    {
        await next(context);
    }
});

app.UseMiddleware<CustomMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UsePathBase(new PathString("/api"));
app.UseRouting();

app.MapControllers();
app.Run();
