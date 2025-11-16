using System;
using Meso.BSV.Certificaciones.v1;
using Meso.CSV.Certificaciones.v1;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.BSV.Certificaciones.Config
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddCertificacionesServices(this IServiceCollection services)
        {

            services.AddTransient<ICertificacionesServiceV1, CertificacionesServiceV1>();
            return services;
        }
    }
}