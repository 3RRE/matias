using System;
using System.Collections.Generic;
using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_RecojoRemesasBL {
        private ESS_RecojoRemesasDAL _recojoRemesaDal = new ESS_RecojoRemesasDAL();

        public List<ESS_RecojoRemesaEntidad> ListadoRecojoRemesa(int[] codSala, DateTime fechaInicio, DateTime fechaFin) {
            return _recojoRemesaDal.ListadoRecojoRemesa(codSala, fechaInicio, fechaFin);
        }

        public int GuardarRegistroRecojoRemesa(ESS_RecojoRemesaEntidad remesa) {
            return _recojoRemesaDal.GuardarRegistroRecojoRemesa(remesa);
        }

        public bool ActualizarRegistroRecojoRemesa(ESS_RecojoRemesaEntidad remesa) {
            if(remesa == null)
                throw new ArgumentNullException(nameof(remesa));
            if(remesa.IdRecojoRemesa <= 0)
                throw new ArgumentException("El IdRecojoRemesa debe ser válido.");

            return _recojoRemesaDal.ActualizarRegistroRecojoRemesa(remesa);
        }

        public bool EliminarRegistroRecojoRemesa(int idRecojoRemesa) {
            if(idRecojoRemesa <= 0)
                throw new ArgumentException("El IdRecojoRemesa debe ser válido.");

            return _recojoRemesaDal.EliminarRegistroRecojoRemesa(idRecojoRemesa);
        }
        public List<ESS_EstadoFotocheckEntidad> ListadoEstadoFotocheck() {
            return _recojoRemesaDal.ListadoEstadoFotocheck();
        }
        public List<ESS_RecojoRemesaPersonalEntidad> ListadoRecojoRemesaPersonal() {
            return _recojoRemesaDal.ListadoRecojoRemesaPersonal();
        }

        public int GuardarRegistroRecojoRemesaPersonal(ESS_RecojoRemesaPersonalEntidad personal) {
            return _recojoRemesaDal.GuardarRegistroRecojoRemesaPersonal(personal);
        }
        public int ExisteRegistroPersonal(int IdTipoDocumentoRegistro, string DocumentoRegistro) {
            if(DocumentoRegistro == null)
                throw new ArgumentNullException(nameof(DocumentoRegistro));
            if(IdTipoDocumentoRegistro <= 0)
                throw new ArgumentException("El IdTipoDocumentoRegistro debe ser válido.");

            return _recojoRemesaDal.ExisteRegistroPersonal(IdTipoDocumentoRegistro,DocumentoRegistro);
        }
    }
}
