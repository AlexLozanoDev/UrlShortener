using Api.Common.Errors;
using Api.Common.Responses;
using Api.Extensions;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Servicios de la aplicacion
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Manejo de errores
builder.Services.AddApiExceptionHandling();

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var correlationId = Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier;

            var details = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors.Select(error => new ApiErrorDetail(
                    JsonNamingPolicy.CamelCase.ConvertName(e.Key),
                    string.IsNullOrEmpty(error.ErrorMessage)
                        ? ApiErrorCodes.ValidationError
                        : error.ErrorMessage,
                    ResolveMessage(error, e.Key))))
                .ToArray();

            return new BadRequestObjectResult(
                ApiResponse.Fail(
                    new ApiError(
                        ApiErrorCodes.ValidationError,
                        "Uno o mas errores de validacion ocurrieron",
                        details),
                    correlationId));
        };
    });

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddOpenApi();


// Autenticacion JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"]!;

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            if (context.Response.HasStarted)
                return;

            await context.HttpContext.WriteApiErrorAsync(
                StatusCodes.Status401Unauthorized,
                "UNAUTHENTICATED",
                "El token de acceso es invalido o ha expirado",
                context.HttpContext.RequestAborted);
        },
        OnForbidden = async context =>
        {
            if (context.Response.HasStarted)
                return;

            await context.HttpContext.WriteApiErrorAsync(
                StatusCodes.Status403Forbidden,
                "FORBIDDEN",
                "No tiene permisos para acceder a este recurso",
                context.HttpContext.RequestAborted);
        }
    };

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseApiErrorHandling();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static string ResolveMessage(ModelError error, string field) =>
    !string.IsNullOrWhiteSpace(error.ErrorMessage)
        ? error.ErrorMessage
        : error.Exception?.Message ?? $"El campo '{field}' no es válido.";