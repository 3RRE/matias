using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class GC_SistemaBL

    {
        private GC_SistemaDAL gcSistemaDal = new GC_SistemaDAL();
        public List<Sistema> SistemaListadoJson()
        {
            return gcSistemaDal.SistemaListadoJson();
        }

        public bool SistemaGuardarJson(Sistema sistema)
        {
            return gcSistemaDal.SistemaGuardarJson(sistema);
        }

       
        public Sistema GestionCambiosSistemaEditarJson(int sistemaId)
        {
            return gcSistemaDal.GestionCambiosSistemaEditarJson(sistemaId);
        }

        public bool SistemaEditarJson(Sistema sistema)
        {
            return gcSistemaDal.SistemaEditarJson(sistema);
        }

        public bool EstadoSistemaActualizarJson(int sistemaId, int estado)
        {
            return gcSistemaDal.EstadoSistemaActualizarJson(sistemaId, estado);
        }
    }
}
