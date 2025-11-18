using CapaDatos;
using CapaDatos.Disco;
using CapaDatos.Imei;
using CapaEntidad.Discos;
using CapaEntidad.Imei;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Imei {
    public class ControlImeiBL {

        private ControlImeiDAL _constrolImeiDAL = new ControlImeiDAL();

        public List<ControlImeiEntidad> ListadoControlImei() {
            return _constrolImeiDAL.ListadoControlImei();
        }


        public int AgregarControlImei(ControlImeiEntidad controlImei) {
            return _constrolImeiDAL.GuardarControlImei(controlImei);
        }


        public bool RechazarImei(int idControlImei) { 
            return _constrolImeiDAL.RechazarControlImei(idControlImei);
        }

        public bool AceptarImei(int idControlImei) {
            return _constrolImeiDAL.AceptarControlImei(idControlImei);
        }


        public int RegistrarNuevoImei(ControlImeiEntidad Entidad) {
            return _constrolImeiDAL.RegistrarNuevoImei(Entidad);
        }
        public bool EditarNuevoImei(ControlImeiEntidad Entidad) {
            return _constrolImeiDAL.EditarNuevoImei(Entidad);
        }
        public ControlImeiEntidad ObtenerRegistroPendienteImei(int IdEmpleado) {
            return _constrolImeiDAL.ObtenerRegistroPendienteImei(IdEmpleado);
        }

    }
}
