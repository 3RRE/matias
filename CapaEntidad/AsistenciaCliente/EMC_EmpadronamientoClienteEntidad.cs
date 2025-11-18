using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.AsistenciaCliente
{
    public class EMC_EmpadronamientoClienteEntidad
    {
        public Int64 id { get; set; }
        public Int64 cliente_id { get; set; }
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string NroDoc { get; set; }
        public Int64 usuario_id { get; set; }
        public string UsuarioNombre { get; set; }

        public Int32 cod_sala { get; set; }
        public string NombreSala { get; set; }
        public DateTime fecha { get; set; }

        public float apuestaImportante { get; set; }
        public Int32 tipocliente_id { get; set; }
        public Int32 tipofrecuencia_id { get; set; }
        public Int32 tipojuego_id { get; set; }
        public string codMaquina { get; set; }

        public string observacion { get; set; }
        public Boolean entrega_dni { get; set; }
        public Boolean reniec { get; set; }

        public DateTime FechaSalida { get; set; }
        public int ZonaIdIn { get; set; }
        public int ZonaIdOut { get; set; }
        public string ZonaNombreIn { get; set; }
        public string ZonaNombreOut { get; set; }
        public int Estado { get; set; }
        public int UsuarioIdOut { get; set; }
        public int RegistroEntrada { get; set; }
        public int RegistroSalida { get; set; }
    }

    public class EMC_EmpadronamientoMobil
    {     
        public float apuesta { get; set; }
        public Boolean entrega_dni { get; set; }
        public Boolean reniec { get; set; }
        public Int32 cliente_id { get; set; }
        public Int32 usuario_id { get; set; }
        public string observacion { get; set; }
        public Int32 zona_id { get; set; }
        public string nombre { get; set; }
        public string paterno { get; set; }
        public string materno { get; set; }
        public string nroDoc { get; set; }

    }
}
