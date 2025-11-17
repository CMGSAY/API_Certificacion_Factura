using System;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.CBL.Certificaciones.v1
{
    public interface ILogs_BitacoraBL
    {
        Task<List<Logs_BitacoraDTO>> ObtenerTodos();
        Task<Logs_BitacoraDTO> ObtenerPorId(int id);
        Task<Logs_BitacoraDTO> CrearBitacora(Logs_BitacoraDTO bitacora);
        Task<Logs_BitacoraDTO> ActualizarBitacora(Logs_BitacoraDTO bitacora);
        Task<bool> EliminarBitacora(int id);

        Task<List<Logs_BitacoraDTO>> ObtenerPorFactura(int facturaId);
        Task<List<Logs_BitacoraDTO>> ObtenerPorAccion(string accion);
        Task<List<Logs_BitacoraDTO>> ObtenerPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<List<Logs_BitacoraDTO>> ObtenerNoProcesados();

        Task<Logs_BitacoraDTO> MarcarComoProcesado(int bitacoraId);
        Task<int> CrearBitacoraSimple(string tipo, string descripcion, int referenciaId);

        Task<bool> LimpiarBitacorasAntiguas(DateTime fechaLimite);
    }
}
