using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public class Maquina
    {
        public string SlotID { set; get; }
        public string Canal { set; get; }
        public string Juego { set; get; }
        public double Toquen { set; get; }
        public int Estado { set; get; }
        public int MarcaID { set; get; }
        public int ModeloID { set; get; }
        public string codigo_alterno { set; get; }
        public string nombre_marca { set; get; }
        public string nombre_modelo { set; get; }
    }
}