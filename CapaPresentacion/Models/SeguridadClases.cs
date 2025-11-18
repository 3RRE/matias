using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CapaPresentacion.Models
{
    public class SeguridadClases
    {


    }

    public class Metodo_clase
    {
        public string   Controller  { get; set; }
        public string   Action{ get; set; }
        public string ReturnType { get; set; }
        public string Attributes { get; set; }
        public IList<CustomAttributeData> AttributesControlador { get; set; }
        public string AttributesControladorString { get; set; }
        public IList<CustomAttributeData> AttributesMetodo { get; set; }
        public string AttributesMetodostring { get; set; }
    }

    public class Metodo_atributos
    {
        public string Controlador { get; set; }
        public string Metodo { get; set; }
        public bool seguridad{ get; set; }
        public string modulo { get; set; }
        public string descripcion{ get; set; }
    }
}                        
                        
