using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface IEstablecimientoBL
    {
        Task<List<EstablecimientoDTO>> ObtenerTodos();
        Task<EstablecimientoDTO> ObtenerPorId(int id);
        Task<EstablecimientoDTO> CrearEstablecimiento(EstablecimientoDTO establecimiento);
        Task<EstablecimientoDTO> ActualizarEstablecimiento(EstablecimientoDTO establecimiento);
        Task<bool> EliminarEstablecimiento(int id);
        Task<List<EstablecimientoDTO>> ObtenerPorEmisor(int emisorId);
    }
}
