using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TecnicoBL
    {
        private TecnicoDAL tecnicoDAL = new TecnicoDAL();
        public List<TecnicoEntidad> TecnicoListarJson()
        {
            return tecnicoDAL.TecnicoListarJson();
        }
        public bool TecnicoGuardarJson(TecnicoEntidad tecnico)
        {
            return tecnicoDAL.TecnicoGuardarJson(tecnico);
        }

        public TecnicoEntidad TecnicoIdObtenerJson(int TecnicoId)
        {
            return tecnicoDAL.TecnicoIdObtenerJson(TecnicoId);
        }

        public bool TecnicoEditarJson(TecnicoEntidad tecnico)
        {
            return tecnicoDAL.TecnicoEditarJson(tecnico);
        }

        public bool ActualizarEstadoTecnico(int tecnicoId, int tecnicoEstado)
        {
            return tecnicoDAL.ActualizarEstadoTecnico(tecnicoId, tecnicoEstado);
        }
    }
}
