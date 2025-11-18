using CapaDatos.Cortesias;
using CapaEntidad;
using CapaEntidad.Cortesias;
using CapaEntidad.TITO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Cortesias {
    public class CRT_AnfitrionaBL {

        private readonly CRT_AnfitrionaDAL anfitrionaDAL;

        public CRT_AnfitrionaBL() {
            anfitrionaDAL = new CRT_AnfitrionaDAL();
        }

        public List<SalaEntidad> GetSalasByCod(string empresa, string sala) {
            return anfitrionaDAL.GetSalasByCod(empresa, sala);
        }
        public List<CRT_Sala> GetSalas() {
            return anfitrionaDAL.GetSalas();
        }
        public List<CRT_Empleado> GetAnfitrionas() {
            return anfitrionaDAL.GetAnfitrionas();
        }
        public List<CRT_Empleado> GetAnfitrionasBySala(string empresa,string sala) {
            return anfitrionaDAL.GetAnfitrionasBySala(empresa,sala);
        }
        public CRT_Empleado GetAnfitrionasByNroDoc(string doi) {
            return anfitrionaDAL.GetAnfitrionasByNroDoc(doi);
        }
    }
}
