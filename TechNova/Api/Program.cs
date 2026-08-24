using Core.Infraestructure;
using Core.Services.Imp;
using Core.Services.Interfaces;
using FluentValidation;
using Infraestructure.Data;
using Infraestructure.Filters;
using Infraestructure.Persistence;
using Infraestructure.Repositories;
using Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;

/// <summary>
/// =========================================================================================
/// CAPA: API -> Program.cs
/// =========================================================================================
/// PROPÓSITO:
/// Configuración integral del pipeline de ASP.NET Core con autenticación JWT nativa.
/// =========================================================================================
/// </summary>

const string CORS_POLICY_NAME = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// 1. Controladores y Filtros Globales
services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// 2. Configuración de Autenticación JWT Nativa
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "TechNova_Super_Secure_Secret_Key_For_Jwt_Tokens_2026_Unisangil!";
var key = Encoding.UTF8.GetBytes(secretKey);

services.AddAuthentication(options =>
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
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "TechNovaApi",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "TechNovaClients",
        ClockSkew = TimeSpan.Zero
    };
});

// 3. Configuración de OpenAPI y Scalar con soporte Bearer
services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Introduce tu token JWT aquí (sin la palabra Bearer)."
        };

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer")] = []
        });

        return Task.CompletedTask;
    });
});

// 4. Conexión a Base de Datos PostgreSQL
var connectionString = configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(30);
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null
            );
        }),
    ServiceLifetime.Scoped
);

// 5. Inyección de Dependencias (IoC)
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Seguridad y Criptografía
services.AddScoped<IPasswordHasherService, PasswordHasherService>();
services.AddScoped<IJwtService, JwtService>();

// Repositorios y Persistencia
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddScoped<IProductRepository, ProductRepository>();

// Servicios de Negocio
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<IProductService, ProductService>();

// Filtros de Autorización
services.AddScoped<ValidateTokenFilter>();
services.AddScoped<AdminOnlyFilter>();
services.AddScoped<GlobalExceptionFilter>();
services.AddScoped<ValidationFilter>();

// 6. FluentValidation
services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// 7. CORS
services.AddCors(options => options.AddPolicy(CORS_POLICY_NAME, policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .WithExposedHeaders("Content-Disposition");
}));

var app = builder.Build();

// 8. Pipeline HTTP
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("TechNova API Documentation")
               .WithTheme(ScalarTheme.Moon);
    });
}

app.UseCors(CORS_POLICY_NAME);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
