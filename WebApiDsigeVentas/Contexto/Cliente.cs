using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Cliente
    {
        public int clienteId { get; set; }
        public int empresaId { get; set; }
        public string codigoInterno { get; set; }
        public int tipoClienteId { get; set; }
        public string tipo { get; set; }
        public string documentoIdentidad { get; set; }
        public string documento { get; set; }
        public string nombreCliente { get; set; }
        public int departamentoId { get; set; }
        public int provinciaId { get; set; }
        public int distritoId { get; set; }
        public string direccion { get; set; }
        public string nroCelular { get; set; }
        public int giroNegocioId { get; set; }
        public string email { get; set; }
        public string motivoNoCompra { get; set; }
        public string productoInteres { get; set; }
        public int personalVendedorId { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public int estado { get; set; }
        public int condFacturacion { get; set; }
        public string fechaVisita { get; set; }

               
        public string nombreDepartamento { get; set; }
        public string nombreProvincia { get; set; }
        public string nombreDistrito { get; set; }
        public string nombreGiroNegocio { get; set; }

        public int tipoPersonal { get; set; }        
        public int identity { get; set; }
    }
}
