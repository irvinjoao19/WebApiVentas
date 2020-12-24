using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class VentaVendedor
    {
       public decimal ventaReal { get; set; }
       public decimal ventaMes { get; set; }
       public decimal devolucion { get; set; }
       public int pedidoDia { get; set; }
       public decimal ventaDia { get; set; }
       public string fechaEmision { get; set; }
       public decimal total { get; set; }
    }
}
