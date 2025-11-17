using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class ValidacionResultadoDTO
    {
        public bool Valido { get; set; }
        public List<string> Mensajes { get; set; } = new();
    }
}
