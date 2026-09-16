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
using Microsoft.OpenApi.Models;
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
var GeminiApi = configuration.GetSection("Gemini_API_Settings");
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
        // 1. Definir el esquema de seguridad Bearer JWT
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Introduce tu token JWT aquí (ejemplo: Bearer eyJhbGci...).",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        // 2. Registrar el esquema en los componentes del documento
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", securityScheme);

        // 3. Aplicar el requisito de seguridad globalmente a todos los endpoints
        var securityRequirement = new OpenApiSecurityRequirement
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
        };

        document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
        document.SecurityRequirements.Add(securityRequirement);

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
services.AddScoped<IPDFService, PDFService>();
services.AddScoped<IGeminiService, GeminiService>();
services.AddScoped<IDocumentAnalysis, DocumentAnalysis>();

// Filtros de Autorización
services.AddScoped<ValidateTokenFilter>();
services.AddScoped<AdminOnlyFilter>();
services.AddScoped<GlobalExceptionFilter>();
services.AddScoped<ValidationFilter>();

// Servicios de exportacion de Http Request mediante HttpClient (Los servicios se registran igualmente)
services.AddHttpClient<IGeminiService, GeminiService>(client => {

    client.BaseAddress= new Uri("https://generativelanguage.googleapis.com/");
    client.DefaultRequestHeaders.Add("x-goog-api-key", GeminiApi["GEMINI_API_KEY"]);
});

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
