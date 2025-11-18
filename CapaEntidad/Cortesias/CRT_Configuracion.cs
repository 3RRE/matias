using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Cortesias {

    public class CRT_Configuracion {
        public string Nombre { get; set; }
        public int Valor { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public enum KeysCortesiaConfiguracion {
        CantidadMaquinasPorAnfitriona,
        MasDeUnaAnfitrionaPorMaquina,
        TiempoAtencion,
        ValidacionTurno,
        SolicitaDocumentoIdentidad,
        CantidadMaximaProductosPorPedido
    }
}
