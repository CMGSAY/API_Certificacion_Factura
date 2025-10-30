using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class DTE_CertificacionDTO
    {
        public int CertificacionId { get; set; }

        public string NumeroAutorizacion { get; set; }

        public string Serie { get; set; }

        public int Correlativo { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaProcesamiento { get; set; }

        public DateTime FechaCertificacion { get; set; }

        public int FacturaId { get; set; }

    }
}
