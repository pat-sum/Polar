using Server.Api.Infrastructure;
using Server.Api.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var connectionString = builder.Configuration.GetConnectionString("Postgres");

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

services.AddCors(options =>
    options.AddPolicy("AllowAll",
             builder =>
             {
                 builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
             }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Polar Server API",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    options.AddEnumsWithValuesFixFilters();

    options.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
    options.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();  


}

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();


// Expose the Program class for use with WebApplicationFactory<T>
public partial class Program
{
}