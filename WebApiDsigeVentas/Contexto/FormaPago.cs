using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class FormaPago
    {
        public int formaPagoId { get; set; }
        public string descripcion { get; set; }
        public int diasVencimiento { get; set; }
        public int estado { get; set; }        
    }
}
