using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class DTE_AnulacionDTO
    {

        public int AnulacionId { get; set; }

        public string Motivo { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaAnulacion { get; set; }

        public DateTime? FechaProcesamiento { get; set; }

        public int FacturaId { get; set; }

    }
}
