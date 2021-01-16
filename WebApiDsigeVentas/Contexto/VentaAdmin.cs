
namespace Contexto
{
    public class VentaAdmin
    {
        public int localId { get; set; }
        public int vendedorId { get; set; }
        public string vendedor { get; set; }
        public decimal vtaMes { get; set; }
        public decimal devMes { get; set; }
        public decimal vtaRealMes { get; set; }
        public decimal vtaDia { get; set; }
        public int pedidoDia { get; set; }
        public int tipo { get; set; }
    }
}
