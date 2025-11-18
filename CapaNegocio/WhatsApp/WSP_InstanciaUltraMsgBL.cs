using CapaDatos.WhatsApp;
using CapaEntidad.WhatsApp;
using System.Collections.Generic;

namespace CapaNegocio.WhatsApp {
    public class WSP_InstanciaUltraMsgBL {
        private readonly WSP_InstanciaUltraMsgDAL _instanciaDAL;

        public WSP_InstanciaUltraMsgBL() {
            _instanciaDAL = new WSP_InstanciaUltraMsgDAL();
        }

        public List<WSP_InstanciaUltraMsgEntidad> ObtenerTodasLasInstancias(int usuarioId) {
            return _instanciaDAL.ObtenerTodasLasInstancias(usuarioId);
        }

        public WSP_InstanciaUltraMsgEntidad ObtenerInstanciaPorIdInstancia(int idInstancia) {
            return _instanciaDAL.ObtenerInstanciaPorIdInstancia(idInstancia);
        }

        public WSP_InstanciaUltraMsgEntidad ObtenerInstanciaPorCodSala(int codSala) {
            return _instanciaDAL.ObtenerInstanciaPorCodSala(codSala);
        }

        public bool SalaTieneInstancia(int codSala) {
            return _instanciaDAL.ObtenerInstanciaPorCodSala(codSala).IdInstanciaUltraMsg > 0;

        }

        public bool InsertarInstancia(WSP_InstanciaUltraMsgEntidad instancia) {
            return _instanciaDAL.InsertarInstancia(instancia) != 0;
        }

        public bool ActualizarInstancia(WSP_InstanciaUltraMsgEntidad instancia) {
            return _instanciaDAL.ActualizarInstancia(instancia) != 0;
        }

        public bool EliminarInstancia(int idInstancia) {
            return _instanciaDAL.EliminarInstancia(idInstancia) != 0;
        }
    }
}
