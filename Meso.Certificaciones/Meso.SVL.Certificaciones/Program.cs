using Asp.Versioning.ApiExplorer;
using Meso.BLL.Certificaciones.Config;
using Meso.BSV.Certificaciones.Config;
using Meso.SVL.Certificaciones.Config;


var builder = WebApplication.CreateBuilder(args);
const string CORS_POLICY = "CorsPolicy";
var corsValue = builder.Configuration.GetSection(CORS_POLICY).Value;

// configuraciones de capas 
builder.Services.AddBLLConfig();
builder.Services.AddCertificacionesServices();
builder.Services.AddSVLConfig(corsValue);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var route = "api-certificaciones";
    var documentName = "{documentName}";
    var servicesName = "CertificacionesAPI";

    app.UseSwagger(options => { options.RouteTemplate = $"{route}/{documentName}/{servicesName}.json"; });
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = route;
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/{route}/{description.GroupName}/{servicesName}.json",
              description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseCors(corsValue);



app.UseAuthorization();

app.MapControllers();

app.Run();