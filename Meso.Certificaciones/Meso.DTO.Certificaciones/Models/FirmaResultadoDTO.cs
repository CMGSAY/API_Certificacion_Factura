using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class FirmaResultadoDTO
    {
        public bool Firmado { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
