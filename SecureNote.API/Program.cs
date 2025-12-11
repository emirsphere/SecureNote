using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // HATA ÇÖZÜMÜ 1: Swagger modelleri için gerekli
using SecureNote.Application.Interfaces;
using SecureNote.Application.Services;
using SecureNote.Application.Validators;
using SecureNote.Infrastructure.Data;
using SecureNote.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantýsý (Null Check ile Güvenli Hale Getirildi)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<SecureNoteDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// 3. JWT Authentication Ayarlarý
// HATA ÇÖZÜMÜ 2: SecretKey null gelirse uygulama patlasýn (Null Check)
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JwtSettings:SecretKey is missing in appsettings.json");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Artýk null olamaz
    };
});

// 4. Dependency Injection (Baðýmlýlýklar)
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IEncryptionService, AesEncryptionService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 5. Swagger Konfigürasyonu (Düzeltildi)
builder.Services.AddSwaggerGen(c =>
{
    // "Bearer" þemasý tanýmlanýyor
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Bütün endpointlerde bu güvenlik þemasýnýn geçerli olduðu belirtiliyor
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
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- KRÝTÝK SIRALAMA ---
app.UseAuthentication(); // Kimlik Doðrulama
app.UseAuthorization();  // Yetkilendirme
// -----------------------

app.MapControllers();

app.Run();