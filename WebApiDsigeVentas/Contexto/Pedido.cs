using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Pedido
    {
        public int pedidoId { get; set; }
        public int empresaId { get; set; }
        public string numeroPedido { get; set; }
        public string codigoInternoSuministro { get; set; }
        public int almacenId { get; set; }
        public int tipoDocumento { get; set; }
        public int puntoVentaId { get; set; }
        public int cuadrillaId { get; set; }
        public int personalVendedorId { get; set; }
        public int formaPagoId { get; set; }
        public int monedaId { get; set; }
        public decimal tipoCambio { get; set; }
        public string codigoInternoCliente { get; set; }
        public int clienteId { get; set; }
        public string direccionPedido { get; set; }
        public decimal porcentajeIGV { get; set; }
        public string observacion { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public int estado { get; set; }
        public decimal subtotal { get; set; }
        public decimal totalIgv { get; set; }
        public decimal totalNeto { get; set; }
        public string numeroDocumento { get; set; }
        public string fechaFacturaPedido { get; set; }
        public int localId { get; set; }
        public List<PedidoDetalle> detalles { get; set; }
    }
}
