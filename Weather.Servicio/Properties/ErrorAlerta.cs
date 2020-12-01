using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Servicio.Properties
{
    internal class ErrorAlerta
    {
        public string Mensaje { get; set; }
        public string Modulo { get; set; } 
        public string Origen { get; set; }
        public int CantidadErrores { get; set; }
        public int Limite { get; set; }
    }
}

