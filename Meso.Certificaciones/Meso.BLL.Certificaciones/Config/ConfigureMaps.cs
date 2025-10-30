using AutoMapper;
using Meso.DAL.Certificaciones.Entities;
using Meso.DTO.Certificaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.BLL.Certificaciones.Config
{
    public class ConfigureMaps : Profile
    {
        public ConfigureMaps()
        {
            CreateMap<DTE_AnulacionDTO, DTE_Anulacion>().ReverseMap();
            CreateMap<DTE_CertificacionDTO, DTE_Certificacion>().ReverseMap();
            CreateMap<DTE_DetalleDTO, DTE_Detalle>().ReverseMap();
            CreateMap<DTE_FacturaDTO, DTE_Factura>().ReverseMap();
            CreateMap<EmisorDTO, Emisor>().ReverseMap();
            CreateMap<EstablecimientoDTO, Establecimiento>().ReverseMap();
            CreateMap<Logs_BitacoraDTO, Logs_Bitacora>().ReverseMap();

        }
    }
}
