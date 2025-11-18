using System;

namespace CapaEntidad.Campañas {
    public class CMP_TicketEntidad {
        public Int64 id { get; set; }
        public Int64 cliente_id { get; set; }
        public int campaña_id { get; set; }
        public string nroticket { get; set; }
        public double monto { get; set; }
        public DateTime fechareg { get; set; } = DateTime.Now;
        public Int64 item { get; set; }
        public DateTime fecharegsala { get; set; }
        public string origen { get; set; }
        public int usuario_id { get; set; }
        public string nombre_usuario { get; set; }
        public int estado { get; set; }

        public string Apeclie { get; set; }
        public string NomClie { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public string FechaNacimiento { get; set; }


        public string NroDoc { get; set; }
        public string NombreCompleto { get; set; }
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public string Nombre { get; set; }
        public string Celular1 { get; set; }

        public DateTime FechaApertura { get; set; }
        public int SalaOrigen { get; set; }
        public bool SalaFisica { get; set; }

        public CMP_CampañaEntidad Campania { get; set; }
    }

    public class CMP_TicketReporteEntidad {
        public long Id { get; set; }
        public long Item { get; set; }
        public string NroTicket { get; set; }
        public double Monto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Estado { get; set; }
        public long UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public long ClienteId { get; set; }
        public string ClienteDOI { get; set; }
        public string ClienteNombres { get; set; }
        public long CampaniaId { get; set; }
        public string CampaniaNombre { get; set; }
        public int CampaniaTipo { get; set; }
        public long SalaId { get; set; }
        public string SalaNombre { get; set; }
    }
}
