using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Reparto
    {
        public int repartoId { get; set; }
        public string numeroPedido { get; set; }
        public int almacenId { get; set; }
        public string descripcion { get; set; }
        public int personalVendedorId { get; set; }
        public string apellidoPersonal { get; set; }
        public int clienteId { get; set; }
        public string apellidoNombreCliente { get; set; }
        public string direccion { get; set; }
        public string fechaEntrega { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
    }
}
