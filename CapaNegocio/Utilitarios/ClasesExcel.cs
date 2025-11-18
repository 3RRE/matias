using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Utilitarios
{
    public class tablaexcel
    {
        public System.Data.DataTable tabla { get; set; }
        public Newtonsoft.Json.Linq.JToken definicioncolumnas { get; set; }
        public string Hojanombre { get; set; }
        public string Tablatitulo { get; set; }
    }
    public class tuplados
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }
    public class columnasexcel
    {
        public string nombre{ get; set; } 

        public string tipo { get; set; } = "STRING";
        public string alinear { get; set; } = "LEFT";
        public string sumar { get; set; } = "false";
        public string formato { get; set; } = "";

    }
}
