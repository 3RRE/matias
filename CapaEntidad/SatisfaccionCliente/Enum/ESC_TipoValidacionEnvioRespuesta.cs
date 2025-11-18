using System.ComponentModel;

namespace CapaEntidad.SatisfaccionCliente.Enum {
    public enum ESC_TipoValidacionEnvioRespuesta {
        [Description("Por fechas")]
        PorFecha = 1,
        [Description("Por tiempo")]
        PorTiempo = 2
    }
}
