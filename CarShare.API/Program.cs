
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CarShare.DAL;
using CarShare.BLL.Services;
using CarShare.DAL.Repository;
using CarShare.DAL.Repository.UserRepository;
using CarShare.DAL.Pepository.UserRepository;
using CarShare.API.Controllers;
using CarShare.BLL.Dtos;
using CarShare.DAL.Models;
using Microsoft.AspNetCore.Identity;
using CarShare.API.Hubs;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// --- إعدادات CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorePolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyMethod()  // السماح بجميع الطرق (GET, POST, PUT, DELETE...)
            .AllowAnyHeader()  // السماح بجميع الهيدرز
            .AllowCredentials();  // السماح بالـ Credentials (مهم SignalR والجلسات)
    });
});

// --- 5. Authorization Setup (Using Policy) --- 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(nameof(UserType.Admin)));
});

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger with JWT Authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarShare API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure DBContext
builder.Services.AddDbContext<CarShareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]))
    };
});

// Register your services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CarPostService>();
builder.Services.AddScoped<RentalRequestService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<FeedbackService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// تطبيق CORS قبل Authentication و Authorization
app.UseCors("CorePolicy");
app.UseStaticFiles(); // لتفعيل wwwroot

app.UseAuthentication();
app.UseAuthorization();

// تكوين SignalR
app.MapHub<FeedbackHub>("/feedbackHub");

app.MapControllers();

app.Run();
