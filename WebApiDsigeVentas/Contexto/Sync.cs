using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Sync
    {
        public List<Identidad> identidades { get; set; }
        public List<Departamento> departamentos { get; set; }
        public List<Provincia> provincias { get; set; }
        public List<Distrito> distritos { get; set; }
        public List<GiroNegocio> negocios { get; set; }
        public List<Producto> productos { get; set; }
        public List<Cliente> clientes { get; set; }
        public List<FormaPago> formaPagos { get; set; }
        public List<Reparto> repartos { get; set; }
        public List<Estado> estados { get; set; }
        public List<Grupo> grupos { get; set; }
        public List<Local> locales { get; set; }
        public List<Pedido> pedidos { get; set; }
        public string mensaje { get; set; }
    }
}
