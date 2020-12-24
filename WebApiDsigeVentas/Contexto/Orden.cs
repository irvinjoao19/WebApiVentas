using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Orden
    {
       public Cliente cliente { get; set; }
       public Pedido pedido { get; set; }
    }
}
