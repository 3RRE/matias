using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDatos.Utilitarios;
using CapaEntidad;

namespace CapaNegocio
{
    public class FichaSintomatologicaBL
    {
        public FichaSintomatologicaDAL fichaSintomatologicaDAL = new FichaSintomatologicaDAL();

        public bool FichaSintomatologicaIngresoInsertarJson(FichaSintomatologicaEntidad ficha)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaIngresoInsertarJson(ficha);
        }

        public Int64 FichaSintomatologicaIngresoInsertaridJson(FichaSintomatologicaEntidad fichaSintomatologica) {
            return fichaSintomatologicaDAL.FichaSintomatologicaIngresoInsertaridJson(fichaSintomatologica);
        }
        public bool FichaSintomatologicaSalidaModificarJson(FichaSintomatologicaEntidad ficha)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaSalidaModificarJson(ficha);
        }

        public bool FichaSintomatologicaImagenModificarJson(FichaSintomatologicaEntidad ficha)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaImagenModificarJson(ficha);
        }
        public FichaSintomatologicaEntidad FichaSintomatologicaBuscarIngresoJson(int empleadoId)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaBuscarIngresoJson(empleadoId);
        }

        public (List<FichaSintomatologicaEntidad> fichaSintomatologicasLista, ClaseError error) FichaSintomatologicaFiltroListarxSalaFechaJson(string salas,DateTime inicio, DateTime fin)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaFiltroListarxSalaFechaJson(salas, inicio,fin );
        }

        public (List<FichaSintomatologicaEntidadReporte> fichaSintomatologicasLista, ClaseError error) FichaSintomatologicaListaIdObtenerJson(string salas, DateTime inicio, DateTime fin)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaListaIdObtenerJson(salas, inicio, fin);
        }

        public (FichaSintomatologicaEntidad fichaSintomatologica, ClaseError error) FichaSintomatologicaIdObtenerJson(int id)
        {
            return fichaSintomatologicaDAL.FichaSintomatologicaIdObtenerJson(id);
        }
    }
}
