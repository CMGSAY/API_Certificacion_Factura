using Meso.BLL.Certificaciones;

using Meso.DAL.Certificaciones;
using Meso.DAL.Certificaciones.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.BLL.Certificaciones.Config
{
    public static class ConfigureServices
    {
        private const string DBCONNECTION = "CertificacionConnection";
        public static IServiceCollection AddBLLConfig(this IServiceCollection services)
        {
            services.AddSqlServer<CertificadorDBContext>($"Name={DBCONNECTION}");
            services.AddAutoMapper(typeof(ConfigureMaps));



            return services;
        }

    }
}
