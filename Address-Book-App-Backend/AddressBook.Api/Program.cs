using System.Text;
using AddressBook.Api.Middleware;
using AddressBook.Application.Interfaces;

using AddressBook.Infrastructure.Persistence;
using AddressBook.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; 
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection for Services
builder.Services.AddScoped<IJobTitleService, JobTitleService>();  
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAddressBookEntryService, AddressBookEntryService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddDirectoryBrowser();

var app = builder.Build();


app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads"
});
app.UseCors("AllowAngularClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();