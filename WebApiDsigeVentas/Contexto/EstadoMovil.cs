﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class EstadoMovil
    {
        public int operarioId { get; set; }
        public int gpsActivo { get; set; }
        public int estadoBateria { get; set; }
        public string fecha { get; set; }
        public int modoAvion { get; set; }
        public int planDatos { get; set; }
    }
     
}
