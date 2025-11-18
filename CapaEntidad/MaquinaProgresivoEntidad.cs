using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MaquinaProgresivoEntidad
    {
        public int indice { set; get; }
        public string SlotID { set; get; }
        public string Juego { set; get; }
        public string Canal { set; get; }
        public string Toquen { set; get; }
        public string _Toquen { set; get; }
        public int Estado { set; get; }
        public int MarcaId { set; get; }
        public int ModeloID { set; get; }
        public string Estado_desc { set; get; }
        public string nombre_marca { set; get; }
        public string nombre_modelo { set; get; }
        public int codigo_alterno { set; get; }
    }
}
