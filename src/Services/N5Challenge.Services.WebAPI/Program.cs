using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Application.Interfaces.Kafka;
using N5Challenge.Application.UseCases.Permission.Commands.RequestPermission;
using N5Challenge.Infrastructure.Persistence.Context;
using N5Challenge.Infrastructure.Persistence.Extensions;
using N5Challenge.Infrastructure.Services.ElasticSearch;
using N5Challenge.Infrastructure.Services.Kafka;
using N5Challenge.Seed;
using System.Security;

var builder = WebApplication.CreateBuilder(args);

#region Configuración de Base de Datos (EF Core)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});
#endregion

#region Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.ToString());
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "N5 Challenge API",
        Version = "v1",
        Description = "API para la gestión de permisos de empleados. Implementación con .NET 8, CQRS, Kafka y Elasticsearch.",
        Contact = new OpenApiContact
        {
            Name = "Alejandro Martin Achinelli",
            Email = "alejandromartin.achinelli@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License"
        }
    });

    // JWT Ready (comentado hasta que se active autenticación)
    // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     Name = "Authorization",
    //     Type = SecuritySchemeType.ApiKey,
    //     Scheme = "Bearer",
    //     BearerFormat = "JWT",
    //     In = ParameterLocation.Header,
    //     Description = "Ingrese un token JWT válido."
    // });

    // c.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer"
    //             }
    //         },
    //         Array.Empty<string>()
    //     }
    // });
});
#endregion

#region Configuración de MediatR y AutoMapper
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RequestPermissionCommand).Assembly));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Inyección de dependencias
builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();
builder.Services.AddPersistenceInfrastructure();
builder.Services.AddOpenApi();
#endregion

builder.Services.AddControllers();

var app = builder.Build();

#region Seed de base de datos (solo en desarrollo o inicialización)
bool dbConnected = false;
int attempts = 0;
while (!dbConnected && attempts < 5)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        // Seeds
        if (!context.PermissionTypes.Any())
        {
            var tipos = PermissionTypeSeed.InitialData();
            context.PermissionTypes.AddRange(tipos);
            context.SaveChanges();
        }

        if (!context.Permissions.Any())
        {
            var permisos = PermissionSeed.InitialData();
            context.Permissions.AddRange(permisos);
            context.SaveChanges();
        }

        dbConnected = true;
    }
    catch (Exception ex)
    {
        attempts++;
        Console.WriteLine($"Intento {attempts}: Esperando conexión a SQL Server...");
        Thread.Sleep(3000);
    }
}
#endregion

#region Configuración del pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "N5 Challenge API v1");
    c.RoutePrefix = string.Empty; // Para que Swagger esté en la raíz
});

app.UseHttpsRedirection();
// app.UseAuthentication(); Agregar cuando se implemente JWT
// app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();
#endregion

app.Run();
