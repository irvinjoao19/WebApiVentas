using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Filtro
    {
        public int usuarioId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string search { get; set; }
        public string login { get; set; }
        public string pass { get; set; }
        public string imei { get; set; }
        public string version { get; set; }
    }
}
