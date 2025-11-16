using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Meso.SVL.Certificaciones.Infraestructure
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var descripcion in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(descripcion.GroupName, InformacionVersionApi(descripcion));
            }
        }

        public static OpenApiInfo InformacionVersionApi(ApiVersionDescription descripcion)
        {
            var info = new OpenApiInfo()
            {
                Title = "API Meso Certificaciones.",
                Version = descripcion.ApiVersion.ToString(),
                Description = "Documentacion",
                Contact = new OpenApiContact() { Name = "Carlos y Eliada" }
            };

            if (descripcion.IsDeprecated)
            {
                info.Description += "Esta version de la API es la mas pro";
            }
            return info;
        }
    }
}