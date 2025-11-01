using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface IDTE_FacturaBL
    {
        Task<List<DTE_FacturaDTO>> ObtenerTodos();
        Task<DTE_FacturaDTO> ObtenerPorId(int id);
        Task<DTE_FacturaDTO> CrearFactura(DTE_FacturaDTO factura);
        Task<DTE_FacturaDTO> ActualizarFactura(DTE_FacturaDTO factura);
        Task<bool> EliminarFactura(int id);
        Task<DTE_FacturaDTO> MarcarComoProcesada(int id);
        Task<DTE_FacturaDTO> ActualizarEstado(int id, int nuevoEstado);
    }
}
