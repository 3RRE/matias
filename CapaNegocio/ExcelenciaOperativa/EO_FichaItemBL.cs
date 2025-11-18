using CapaDatos.ExcelenciaOperativa;
using CapaEntidad.ExcelenciaOperativa;
using System.Collections.Generic;

namespace CapaNegocio.ExcelenciaOperativa
{
    public class EO_FichaItemBL
    {
        protected EO_FichaItemDAL fichaItemDAL = new EO_FichaItemDAL();

        public bool InsertarFichaItem(EO_FichaItemEntidad fichaItem)
        {
            return fichaItemDAL.InsertarFichaItem(fichaItem);
        }

        public bool ActualizarFichaItem(EO_FichaItemEntidad fichaItem)
        {
            return fichaItemDAL.ActualizarFichaItem(fichaItem);
        }

        public List<EO_FichaItemEntidad> FichaItemIdCategoriaObtenerJson(long id)
        {
            return fichaItemDAL.FichaItemIdCategoriaObtenerJson(id);
        }
    }
}
