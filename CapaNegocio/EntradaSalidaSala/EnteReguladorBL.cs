using CapaEntidad.EntradaSalidaSala;
using CapaDatos.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {

    public class ESS_EnteReguladorBL {
        private ESS_EnteReguladorDAL _entereguladorDal = new ESS_EnteReguladorDAL();


        public List<ESS_EnteReguladorEntidad> ListadoEnteRegulador(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return _entereguladorDal.ListadoEnteRegulador(codSala, fechaIni, fechaFin);
        }


        public int GuardarRegistroEnteRegulador(ESS_EnteReguladorEntidad registro) {
            return _entereguladorDal.GuardarRegistroEnteRegulador(registro);
        }

        public bool ActualizarRegistroEnteRegulador(ESS_EnteReguladorEntidad registro) {
            var status = _entereguladorDal.ActualizarRegistroEnteRegulador(registro);
            return status;
        }


        //public bool EliminarRegistroEnteRegulador(ESS_EnteReguladorEntidad entidad) {
        //    return _entereguladorDal.EliminarRegistroEnteRegulador(entidad.IdEnteRegulador);
        //}
        public bool EliminarRegistroEnteRegulador(int idregistro) {
            return _entereguladorDal.EliminarRegistroEnteRegulador(idregistro);
        }



        public bool ActualizarRutaImagen(int idEnteRegulador, string rutaImagen) => _entereguladorDal.ActualizarRutaImagen(idEnteRegulador, rutaImagen);
        public bool FinalizarHoraRegistroEnteRegulador(int idregistro, DateTime horaSalida) {
            var status = _entereguladorDal.FinalizarHoraRegistroEnteRegulador(idregistro, horaSalida);
            return status;
        }

        public List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> ObtenerPersonasActivasPorTermino(int entidadPublicaID, string term) {
            return _entereguladorDal.ObtenerPersonasActivasPorTermino(entidadPublicaID, term);
        }
        public List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> ObtenerPersonasActivasPorEntidadPublica(int entidadPublicaID) {
            return _entereguladorDal.ObtenerPersonasActivasPorEntidadPublica(entidadPublicaID);
        }
        public int GuardarRegistroEnteRegulador_ImportarExcel(ESS_EnteReguladorEntidad registro) {
            return _entereguladorDal.GuardarRegistroEnteRegulador_ImportarExcel(registro);
        }

    }
}
