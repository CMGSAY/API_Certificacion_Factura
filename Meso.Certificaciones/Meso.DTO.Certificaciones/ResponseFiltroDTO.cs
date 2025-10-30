using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meso.DTO.Certificaciones
{
    public class ResponseFiltroDTO<T>
    {
        public bool Success { get; set; } = true;
        public T SingleResult { get; set; }
        public string DisplayMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
