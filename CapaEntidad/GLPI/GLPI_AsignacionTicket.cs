using System;
using System.Collections.Generic;

namespace CapaEntidad.GLPI {
    public class GLPI_AsignacionTicket : GLPI_BaseClass {
        public int IdTicket { get; set; } // no se envia, automatico
        public int IdUsuarioAsigna { get; set; } // no se envia, automatico de la sesion
        public int IdEstadoTicket { get; set; } // dbe ser el primer registro
        public int IdUsuarioAsignado { get; set; } // se envia, debe ser uno de los usuarios qyue tiene el permiso Recibir ticket
        public DateTime FechaTentativaTermino { get; set; } // se emvia
        public string Correos { get; set; } = string.Empty;
        public List<string> Destinatarios { get; set; } = new List<string>();// se enia, es opcional
        public DateTime FechaModificacion { get; set; } //no se envia
    }
}
