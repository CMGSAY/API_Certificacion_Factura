using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface IDTE_CertificacionBL
    {
        Task<List<DTE_CertificacionDTO>> ObtenerTodos();
        Task<DTE_CertificacionDTO> ObtenerPorId(int id);
        Task<DTE_CertificacionDTO> CrearCertificacion(DTE_CertificacionDTO certificacion);
        Task<DTE_CertificacionDTO> ActualizarCertificacion(DTE_CertificacionDTO certificacion);
        Task<bool> EliminarCertificacion(int id);
    }
}
