using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Resumen
    {
        public decimal totalVenta { get; set; }
        public int countPedidoVenta { get; set; }
        public int countClientes { get; set; }
        public int vendedorId { get; set; }
        public string   mejorVendedor { get; set; }
        public decimal mejorVendedorSoles { get; set; }  
        public int productoId { get; set; }
        public string   mejorProducto { get; set; }
        public decimal mejorProductoSoles { get; set; }
        public decimal totalDevolucion { get; set; }
        public int peorVendedorId { get; set; }
        public string peorVendedor { get; set; }
        public decimal peorVendedorSoles { get; set; } 
    }
}
