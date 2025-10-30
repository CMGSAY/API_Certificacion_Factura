using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class Logs_BitacoraDTO
    {
        public int BitacoraId { get; set; }

        public string Accion { get; set; }

        public DateTime FechaAccion { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaProcesamiento { get; set; }

        public int FacturaId { get; set; }

    }
}
