using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Grupo
    {
        public int detalleTablaId { get; set; }
        public int grupoTablaId { get; set; }
        public string codigoDetalle { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }
    }
}