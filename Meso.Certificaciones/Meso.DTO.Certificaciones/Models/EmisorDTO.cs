using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class EmisorDTO
    {

        public int EmisorId { get; set; }

        public string Nit { get; set; }

        public string RazonSocial { get; set; }

        public bool EstadoActivo { get; set; }

        public bool PuedeCertificar { get; set; }

        public DateTime FechaRegistro { get; set; }

    }
}
