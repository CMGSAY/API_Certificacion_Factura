using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface IEmisorBL
    {
        Task<List<EmisorDTO>> ObtenerTodos();
        Task<EmisorDTO> ObtenerPorId(int id);
        Task<EmisorDTO> CrearEmisor(EmisorDTO emisor);
        Task<EmisorDTO> ActualizarEmisor(EmisorDTO emisor);
        Task<bool> EliminarEmisor(int id);
        Task<List<EmisorDTO>> ObtenerActivos();
        Task<EmisorDTO> ActivarEmisor(int id);
        Task<EmisorDTO> DesactivarEmisor(int id);
    }
}
