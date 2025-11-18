using System;

namespace CapaEntidad
{
    public class DestinatarioEntidad
    {
        public Int32 EmailID { get; set; }
        public String Nombre { get; set; }
        public String Email { get; set; }
        public int estado { get; set; }
    }

    public class WEB_DestinatarioEntidad
    {
        public int WEB_DestID { get; set; }
        public string WEB_DestTitular { get; set; }
        public string WEB_DestCorreo { get; set; }
        public string WEB_DestEstado { get; set; }
        public int WEB_Tipo { get; set; }
        public DateTime WEB_DestFechaRegistro { get; set; }
    }
}
