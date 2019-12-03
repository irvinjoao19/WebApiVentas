using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class PedidoDetalle
    {
        public int pedidoDetalleId { get; set; }
        public int pedidoId { get; set; }
        public int pedidoItem { get; set; }
        public int productoId { get; set; }
        public decimal precioVenta { get; set; }
        public decimal porcentajeDescuento { get; set; }
        public decimal descuentoPedido { get; set; }
        public decimal cantidad { get; set; }
        public decimal porcentajeIGV { get; set; }
        public decimal totalPedido { get; set; }
        public string numeroPedido { get; set; }
    }
}
