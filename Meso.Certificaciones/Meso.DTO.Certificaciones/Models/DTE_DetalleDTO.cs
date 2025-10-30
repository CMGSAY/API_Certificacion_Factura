using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones.Models
{
    public class DTE_DetalleDTO
    {

        public int DetalleId { get; set; }

        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public int FacturaId { get; set; }

    }
}
