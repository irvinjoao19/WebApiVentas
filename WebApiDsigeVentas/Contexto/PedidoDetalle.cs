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
        public int identity { get; set; }
        public int identityDetalle { get; set; }
        public int localId { get; set; }

        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string abreviaturaProducto { get; set; }
        public decimal unidadMedida { get; set; }
        public decimal stockMinimo { get; set; }
        public decimal subTotal { get; set; }

        public decimal factor { get; set; }
        public decimal precio1 { get; set; }
        public decimal precio2 { get; set; }
        public decimal precioMayMenor { get; set; }
        public decimal precioMayMayor { get; set; }
        public decimal rangoCajaHorizontal { get; set; }
        public decimal rangoCajaMayorista { get; set; }

        public int estado { get; set; }
        public int active { get; set; }
    }
}
