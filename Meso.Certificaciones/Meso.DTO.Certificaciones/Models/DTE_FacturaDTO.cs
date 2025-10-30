using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class DTE_FacturaDTO
    {
        public int FacturaId { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string ClienteNIT { get; set; }

        public string ClienteRazonSocial { get; set; }

        public decimal Total { get; set; }

        public byte Estado { get; set; }

        public DateTime? FechaProcesamiento { get; set; }

        public int EstablecimientoId { get; set; }

    }
}
