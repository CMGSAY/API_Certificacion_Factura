using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class EstablecimientoDTO
    {
        public int EstablecimientoId { get; set; }

        public string NombreEstablecimiento { get; set; }

        public string NIT { get; set; }

        public string Direccion { get; set; }

        public bool EstadoActivo { get; set; }

        public string CodigoUnicoEstablecimiento { get; set; }

        public int EmisorId { get; set; }


    }
}
