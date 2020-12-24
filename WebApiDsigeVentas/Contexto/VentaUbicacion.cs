using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class VentaUbicacion
    {
        public int pedidoCabId { get; set; }
        public int clienteId { get; set; }
        public string nroDocCliente { get; set; }
        public string nombreCliente { get; set; }
        public string direccion { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public decimal total { get; set; }
    }
}
