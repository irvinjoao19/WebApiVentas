
namespace Contexto
{
    public class RepartoDetalle
    {
        public int detalleId { get; set; }
        public int repartoId { get; set; }
        public int pedidoItem { get; set; }
        public int productoId { get; set; }
        public decimal precioVenta { get; set; }
        public decimal porcentajeDescuento { get; set; }
        public decimal descuento { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadExacta { get; set; }
        public decimal porcentajeIGV { get; set; }
        public decimal total { get; set; }
        public string numeroPedido { get; set; }
        public string nombreProducto { get; set; }
        public string codigoProducto { get; set; }
        public int estado { get; set; }
    }
}
