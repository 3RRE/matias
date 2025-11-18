using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_RecaudacionPersonalBL {
        private ESS_RecaudacionPersonalDAL _recaudacionpersonalDal = new ESS_RecaudacionPersonalDAL();
        public List<ESS_RecaudacionPersonalEntidad> ListadoRecaudacionPersonal(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return _recaudacionpersonalDal.ListadoRecaudacionPersonal(codSala, fechaIni, fechaFin);
        }
        public int GuardarRecaudacionPersonal(ESS_RecaudacionPersonalEntidad registro) {
            return _recaudacionpersonalDal.GuardarRecaudacionPersonal(registro);
        }

        public bool ActualizarRecaudacionPersonal(ESS_RecaudacionPersonalEntidad registro) {
            var status = _recaudacionpersonalDal.ActualizarRecaudacionPersonal(registro);
            return status;
        }
        public bool EliminarRecaudacionPersonal(int idregistro) {
            return _recaudacionpersonalDal.EliminarRecaudacionPersonal(idregistro);
        }
        public List<ESS_FuncionEntidad> ListarFuncionPorEstado(int estado) => _recaudacionpersonalDal.ListarFuncionPorEstado(estado);
        public List<ESS_CargoRPEntidad> ListarCargoRPPorEstado(int estado) => _recaudacionpersonalDal.ListarCargoRPPorEstado(estado);


        public List<ESS_FuncionEntidad> ListarFuncion() => _recaudacionpersonalDal.ListarFuncion();

        public int InsertarFuncion(ESS_FuncionEntidad model) => _recaudacionpersonalDal.InsertarFuncion(model);

        public bool EditarFuncion(ESS_FuncionEntidad model) => _recaudacionpersonalDal.EditarFuncion(model);

    }



}
