using System.Collections.Generic;

namespace CapaEntidad.AsistenciaCliente {
    public class ClienteSomosCasino {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Celular { get; set; }
        public string CodigoPais { get; set; }
        public int CodSala { get; set; }
        public List<int> SalasAplicaCodigoPromocional { get; set; } = new List<int>();
        public decimal MontoRecargado { get; set; } = 0;
        public bool EnviaNotificacionWhatsapp { get; } = false;
        public bool EnviaNotificacionSms { get; } = false;
        public bool EnviaNotificacionEmail { get; } = false;
        public bool LlamadaCelular { get; } = false;
        public int IdUbigeoProcedencia { get; } = 174;
        public string PaisId { get; } = "PE";
    }
}
