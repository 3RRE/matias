using CapaEntidad.EntradaSalidaSala;
using CapaDatos.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {

    public class ESS_BienMaterialBL {
        private ESS_BienMaterialDAL _bienmaterialDal = new ESS_BienMaterialDAL();

        public List<ESS_BienMaterialEntidad> ListadoBienMaterial(int[] codSala, int idtipobienmaterial, DateTime fechaIni, DateTime fechaFin) {
            return _bienmaterialDal.ListadoBienMaterial(codSala, idtipobienmaterial, fechaIni, fechaFin);
        }
        public int GuardarRegistroBienMaterial(ESS_BienMaterialEntidad registro) {
            return _bienmaterialDal.GuardarRegistroBienMaterial(registro);
        }
        public bool EditarBienesMateriales(ESS_BienMaterialEntidad registro) {
            var status = _bienmaterialDal.EditarBienesMateriales(registro);
            return status;
        }
        public bool EliminarRegistroBienMaterial(ESS_BienMaterialEntidad entidad) {
            return _bienmaterialDal.EliminarRegistroBienMaterial(entidad.IdBienMaterial);
        }
        public bool ActualizarRutaImagen(int idBienMaterial, string rutaImagen) => _bienmaterialDal.ActualizarRutaImagen(idBienMaterial, rutaImagen);
        public bool FinalizarHoraRegistroBienMaterial(int  idregistro) {
            var status = _bienmaterialDal.FinalizarHoraRegistroBienMaterial(idregistro); 
            return status;
        }
        public ESS_BienMaterialEntidad GetBienMaterialPorId(int id) => _bienmaterialDal.GetBienMaterialPorId(id);

    }

}
