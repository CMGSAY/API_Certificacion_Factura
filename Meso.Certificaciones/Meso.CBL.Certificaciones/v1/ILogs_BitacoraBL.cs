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
        Task<Logs_BitacoraDTO> CrearBitacora(Logs_BitacoraDTO bitacora);
        Task CrearBitacoraSimple(string tipo, string descripcion, int referenciaId);
    }
}
