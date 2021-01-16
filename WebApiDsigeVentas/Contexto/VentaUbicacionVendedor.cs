using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class VentaUbicacionVendedor
    {
        public int id { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public int operarioId { get; set; }
        public string vendedor { get; set; }
        public decimal total { get; set; }
        public int totalPedidos { get; set; }
    }
}
