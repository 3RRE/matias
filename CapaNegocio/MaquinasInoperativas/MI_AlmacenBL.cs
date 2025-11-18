using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_AlmacenBL {

        MI_AlmacenDAL capaDatos = new MI_AlmacenDAL();

        public List<MI_AlmacenEntidad> AlmacenListadoCompletoJson() {
            return capaDatos.GetAllAlmacen();
        }
        public List<MI_AlmacenEntidad> AlmacenListadoCompletoxSalasUsuarioJson(int codUsuario)
        {
            return capaDatos.GetAllAlmacenxSalasUsuario(codUsuario);
        }
        public List<MI_AlmacenEntidad> AlmacenListadoActiveJson(int codUsuario) {
            return capaDatos.GetAllAlmacenActive(codUsuario);
        }
        public MI_AlmacenEntidad AlmacenCodObtenerJson(int cod) {
            return capaDatos.GetCodAlmacen(cod);
        }
        public int AlmacenInsertarJson(MI_AlmacenEntidad Entidad) {
            var cod = capaDatos.InsertarAlmacen(Entidad);

            return cod;
        }
        public bool AlmacenEditarJson(MI_AlmacenEntidad Entidad) {
            var status = capaDatos.EditarAlmacen(Entidad);

            return status;
        }
        public bool AlmacenEliminarJson(int cod) {
            var status = capaDatos.EliminarAlmacen(cod);

            return status;
        }
        public bool AsignarUsuarioAlmacen(int codAlmacen, int codUsuario) {
            var status = capaDatos.AsignarUsuarioAlmacen(codAlmacen, codUsuario);

            return status;
        }
        public bool QuitarUsuarioAlmacen(int codAlmacen, int codUsuario) {
            var status = capaDatos.QuitarUsuarioAlmacen(codAlmacen, codUsuario);

            return status;
        }
        public List<MI_AlmacenEntidad> GetAllAlmacenxUsuario(int codUsuario) {
            return capaDatos.GetAllAlmacenxUsuario(codUsuario);
        }
        public List<MI_AlmacenEntidad> GetAllUsuarioxAlmacen(int codAlmacen) {
            return capaDatos.GetAllUsuarioxAlmacen(codAlmacen);
        }
        public MI_AlmacenEntidad GetCodAlmacenCodUsuario(int codAlmacen, int codUsuario) {
            return capaDatos.GetCodAlmacenCodUsuario(codAlmacen,codUsuario);
        }
        public List<MI_AlmacenEntidad> GetAllAlmacenxSala(int codSala) {
            return capaDatos.GetAllAlmacenxSala(codSala);
        }

        
    }
}
