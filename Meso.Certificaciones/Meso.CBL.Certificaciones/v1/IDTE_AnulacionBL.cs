using Meso.DTO.Certificaciones;
using Meso.DTO.Certificaciones.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meso.CSV.Certificaciones.v1
{
    public interface ICertificacionesServiceV1
    {

        // CERTIFICACIÓN
        Task<ResponseDTO<object>> CrearCertificacion(DTE_CertificacionDTO dto);
        Task<ResponseDTO<List<DTE_CertificacionDTO>>> ObtenerCertificaciones();
        Task<ResponseDTO<DTE_CertificacionDTO>> ObtenerCertificacionPorId(int id);


        // ANULACIÓN
        Task<ResponseDTO<object>> CrearAnulacion(DTE_AnulacionDTO dto);

        // EMISOR

        Task<ResponseDTO<EmisorDTO>> ActivarEmisor(int emisorId);
        Task<ResponseDTO<EmisorDTO>> DesactivarEmisor(int emisorId);
    }
}
