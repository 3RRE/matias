using System.ComponentModel;

namespace CapaEntidad.GLPI.Enum {
    public enum GLPI_FaseTicket {
        [Description("Creado")]
        Creado = 1,
        [Description("Asignado")]
        Asignado = 2,
        [Description("En Proceso")]
        EnProceso = 3,
        [Description("Finalizado")]
        Finalizado = 4,
        [Description("Cerrado")]
        Cerrado = 5
    }
}
