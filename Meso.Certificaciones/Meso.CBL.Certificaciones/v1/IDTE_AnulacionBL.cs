using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface IDTE_AnulacionBL
    {
        Task<List<DTE_AnulacionDTO>> ObtenerTodos();
        Task<DTE_AnulacionDTO> ObtenerPorId(int id);
        Task<DTE_AnulacionDTO> CrearAnulacion(DTE_AnulacionDTO anulacion);
        Task<List<DTE_AnulacionDTO>> ObtenerPorFacturaId(int facturaId);
    }
}
