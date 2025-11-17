using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface IDTE_DetalleBL
    {
        Task<List<DTE_DetalleDTO>> ObtenerPorFacturaId(int facturaId);
        Task<DTE_DetalleDTO> CrearDetalle(DTE_DetalleDTO detalle);
        Task<bool> EliminarPorFacturaId(int facturaId);
        Task<DTE_DetalleDTO> AgregarDetalle(int facturaId, DTE_DetalleDTO dto);
    }
}
