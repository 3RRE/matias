using CapaDatos.ExcelenciaOperativa;
using CapaDatos.Utilitarios;
using CapaEntidad.ExcelenciaOperativa;
using System.Collections.Generic;

namespace CapaNegocio.ExcelenciaOperativa
{
    public class EO_FichaCategoriaBL
    {
        protected EO_FichaCategoriaDAL fichaCategoriaDAL = new EO_FichaCategoriaDAL();

        public long InsertarFichaCategoria(EO_FichaCategoriaEntidad fichaCategoria)
        {
            return fichaCategoriaDAL.InsertarFichaCategoria(fichaCategoria);
        }

        public bool ActualizarFichaCategoria(EO_FichaCategoriaEntidad fichaCategoria)
        {
            return fichaCategoriaDAL.ActualizarFichaCategoria(fichaCategoria);
        }

        public List<EO_FichaCategoriaEntidad> FichaCategoriaIdFichaObtenerJson(long id)
        {
            return fichaCategoriaDAL.FichaCategoriaIdFichaObtenerJson(id);
        }
    }
}
