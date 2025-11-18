using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_MaquinaInoperativaRepuestosBL {

        MI_MaquinaInoperativaRepuestosDAL capaDatos = new MI_MaquinaInoperativaRepuestosDAL();
        public List<MI_MaquinaInoperativaRepuestosEntidad> MaquinaInoperativaRepuestosListadoCompletoJson() {
            return capaDatos.GetAllMaquinaInoperativaRepuestos();
        }
        public List<MI_MaquinaInoperativaRepuestosEntidad> MaquinaInoperativaRepuestosListadoActiveJson() {
            return capaDatos.GetAllMaquinaInoperativaRepuestosActive();
        }
        public List<MI_MaquinaInoperativaRepuestosEntidad> MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(int cod) {
            return capaDatos.GetAllMaquinaInoperativaRepuestosxMaquinaInoperativa(cod);
        }
        public List<MI_MaquinaInoperativaRepuestosEntidad> MaquinaInoperativaRepuestosAgregadosListadoxMaquinaInoperativaJson(int cod) {
            return capaDatos.GetAllMaquinaInoperativaRepuestosAgregadosxMaquinaInoperativa(cod);
        }
        public MI_MaquinaInoperativaRepuestosEntidad MaquinaInoperativaRepuestosCodObtenerJson(int cod) {
            return capaDatos.GetCodMaquinaInoperativaRepuestos(cod);
        }
        public int MaquinaInoperativaRepuestosInsertarJson(MI_MaquinaInoperativaRepuestosEntidad Entidad) {
            var cod = capaDatos.InsertarMaquinaInoperativaRepuestos(Entidad);

            return cod;
        }
        public bool MaquinaInoperativaRepuestosEditarJson(MI_MaquinaInoperativaRepuestosEntidad Entidad) {
            var status = capaDatos.EditarMaquinaInoperativaRepuestos(Entidad);

            return status;
        }
        public bool MaquinaInoperativaRepuestosEliminarJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativaRepuestos(cod);

            return status;
        }
        public bool MaquinaInoperativaRepuestosEliminarxMaquinaJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativaRepuestosxMaquina(cod);

            return status;
        }

        public bool AceptarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            var status = capaDatos.AceptarTraspasoRepuestoAlmacen(Entidad);

            return status;
        }
        public bool RechazarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            var status = capaDatos.RechazarTraspasoRepuestoAlmacen(Entidad);

            return status;
        }


    }
}
