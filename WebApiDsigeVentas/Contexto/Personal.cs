using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Personal
    {
        public int personalId { get; set; }
        public string nombrePersonal { get; set; }
        public int countPedidos { get; set; }
        public int countClientes { get; set; }
        public int countProductos { get; set; }
        public decimal total { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
    
    }
}
