
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDatos.Disco;
using CapaEntidad.Discos;

namespace CapaNegocio.Discos {
    public class DiscoBL {
        private DiscoDAL _discoDal = new DiscoDAL();

        public List<DiscoEntidad> ListadoDisco(int codSala, DateTime fechaIni, DateTime fechaFin) {
            return _discoDal.ListadoDiscos(codSala, fechaIni, fechaFin);
        }
        public List<DiscoEntidad> ListadoDiscoAll(string codSala, DateTime fechaIni, DateTime fechaFin) {
            return _discoDal.ListadoDiscosAll(codSala, fechaIni, fechaFin);
        }

        public int AgregarDiscos(DiscoEntidad discos) {
            return _discoDal.GuardarDiscoSala(discos);
        }


        public DiscoEntidad ObtenerUltimoRegistro(int id)
        {
            return _discoDal.UltimoRegistro(id);
        }
        public List<EspacioDiscoBD> ListadoBDsAzure() {
            return _discoDal.ListadoBDsAzure();
        }
        public bool LimpiarLogBDAzure(string nombreBD, string nombreLog) {
            return _discoDal.LimpiarLogBDAzure(nombreBD,nombreLog);
        }
    }


}

