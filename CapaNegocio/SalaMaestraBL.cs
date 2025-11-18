using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio {
    public class SalaMaestraBL {
        private readonly SalaMaestraDAL _salaMaestraDal;

        public SalaMaestraBL() {
            _salaMaestraDal = new SalaMaestraDAL();
        }

        public List<SalaMaestraEntidad> ObtenerTodasLasSalasMaestras() {
            return _salaMaestraDal.ObtenerTodasLasSalasMaestras();
        }

        public List<SalaMaestraEntidad> ObtenerTodasLasSalasMaestrasActivas() {
            return _salaMaestraDal.ObtenerTodasLasSalasMaestrasActivas();
        }

        public SalaMaestraEntidad ObtenerSalaMaestraPorCodigo(int codSalaMaestra) {
            return _salaMaestraDal.ObtenerSalaMaestraPorCodigo(codSalaMaestra);
        }

        public SalaMaestraEntidad ObtenerSalaMaestraPorCodigoSala(int codSala) {
            return _salaMaestraDal.ObtenerSalaMaestraPorCodigoSala(codSala);
        }

        public bool InsertarSalaMaestra(SalaMaestraEntidad salaMaestra) {
            return _salaMaestraDal.InsertarSalaMaestra(salaMaestra);
        }

        public bool ActualizarSalaMaestra(SalaMaestraEntidad salaMaestra) {
            return _salaMaestraDal.ActualizarSalaMaestra(salaMaestra);
        }

        public bool EliminarSalaMaestra(int codSalaMaestra) {
            return _salaMaestraDal.EliminarSalaMaestra(codSalaMaestra);
        }

        public bool ActualizarEstadoDeSalaMaestra(int codSalaMaestra, bool estado) {
            return estado ? _salaMaestraDal.ActivarSalaMaestra(codSalaMaestra) : _salaMaestraDal.DesactivarSalaMaestra(codSalaMaestra);
        }
    }
}
