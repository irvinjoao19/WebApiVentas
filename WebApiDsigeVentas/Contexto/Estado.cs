using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Estado
    {
        public int estadoId { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string tipoProceso { get; set; }
        public string descripcionTipoProceso { get; set; }
        public int moduloId { get; set; }
        public int backColor { get; set; }
        public string forecolor { get; set; }
        public int estado { get; set; }
    }
}
