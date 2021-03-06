﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Producto
    {
        public int productoId { get; set; }
        public string codigoProducto { get; set; }
        public string nombreProducto { get; set; }
        public string descripcionProducto { get; set; }
        public string abreviaturaProducto { get; set; }
        public decimal stock { get; set; }
        public decimal precio { get; set; }
        public string nombreCategoria { get; set; }
        public string nombreMarca { get; set; }
        public decimal factor { get; set; }
        public decimal precio2 { get; set; }
        public decimal precioMayMenor { get; set; }
        public decimal precioMayMayor { get; set; }
        public decimal rangoCajaHorizontal { get; set; }
        public decimal rangoCajaMayorista { get; set; }        
    }
}
    