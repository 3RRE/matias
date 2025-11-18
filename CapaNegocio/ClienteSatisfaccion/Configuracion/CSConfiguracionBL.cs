using CapaDatos.ClienteSatisfaccion.Configuracion;
using CapaDatos.ClienteSatisfaccion.Flujo;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.Configuracion {
    public class CSConfiguracionBL {
        private CSConfiguracionDAL configuracionDAL = new CSConfiguracionDAL();
        public List<CSConfiguracionEntidad> ListadoConfiguraciones() { 
            return configuracionDAL.ListadoConfiguraciones();

        }

        public bool ActualizarValorBit(int idConfiguracion, bool nuevoValor, int idSala) { 
            return configuracionDAL.ActualizarValorBit(idConfiguracion, nuevoValor, idSala);

        }

        public bool PuedeResponderEncuesta(int idSala, string nroDocumento) { 
            return configuracionDAL.PuedeResponderEncuesta(idSala, nroDocumento);

        }


        public List<CSConfiguracionEntidad> ListadoConfiguracionesPorSala(int idSala) {
            return configuracionDAL.ListadoConfiguracionesPorSala(idSala);

        }
    }
}
