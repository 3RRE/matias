using System;

namespace CapaEntidad.ControlAcceso.Ludopata.Dto {
    public class CAL_LudopataDto {
        public string CodigoRegistro { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public CAL_ContactoLudopataDto Contacto { get; set; } = new CAL_ContactoLudopataDto();
    }
}
