using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Usuario
    {
        public int existe { get; set; }
        public int usuarioId { get; set; }
        public string documento { get; set; }
        public string apellidos { get; set; }
        public string nombres { get; set; }
        public int tipo { get; set; }
        public int cargoId { get; set; }
        public string nombreCargo { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string pass { get; set; }
        public string envioOnline { get; set; }
        public int perfil { get; set; }
        public string descripcionPerfil { get; set; }
        public int estado { get; set; }  
        public int localId { get; set; }  
    }
}
