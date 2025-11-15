using Meso.BLL.Certificaciones.v1;
using Meso.CBL.Certificaciones.v1;
using Meso.DAL.Certificaciones;
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


            services.AddScoped<IDTE_AnulacionBL, DTE_AnulacionBL>();
            services.AddScoped<IDTE_CertificacionBL, DTE_CertificacionBL>();
            services.AddScoped<IDTE_DetalleBL, DTE_DetalleBL>();
            services.AddScoped<IDTE_FacturaBL, DTE_FacturaBL>();
            services.AddScoped<IEmisorBL, EmisorBL>();
            services.AddScoped<IEstablecimientoBL, EstablecimientoBL>();
            services.AddScoped<ILogs_BitacoraBL, Logs_BitacoraBL>();

            return services;
        }

    }
}
