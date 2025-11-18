using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Alertas
{
    public class ALT_AlertaSalaEntidad
    {
        public Int64 alts_id { get; set; }
        public Int64 AlertaID { get; set; }
        public String CodEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public String CodSala { get; set; }
        public string NombreSala { get; set; }
        public String CodMaquina { get; set; }
        public String CodMarcaMaquina { get; set; }
        public string Juego { get; set; }
        public string fecha_registro { get; set; }
        public string fecha_termino { get; set; }
        public Int32 cod_tipo_alerta { get; set; }
        public string descripcion_alerta { get; set; }
        public string ColorAlerta { get; set; }
        public decimal contador_bill_parcial { get; set; }
        public decimal contador_bill_billetero { get; set; }
        public int estado { get; set; }
        public DateTime alts_fechareg { get; set; }
    }

    public class ALT_AlertaDeviceEntidad
    {
        public Int64 emd_id { get; set; }
        public String emd_imei { get; set; }
        public int emp_id { get; set; }
        public String id { get; set; }
        public int CargoID { get; set; }
        public int sala_id { get; set; }
        public int tipo { get; set; }

    }

    public class AlertBillNotificationReqEntidad
    {
        public Int32 AlertaID { get; set; }
        public String CodEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public String CodSala { get; set; }
        public string NombreSala { get; set; }
        public String CodMaquina { get; set; }
        public String CodMarcaMaquina { get; set; }
        public string Juego { get; set; }
        public string fecha_registro { get; set; }
        public string fecha_termino { get; set; }
        public Int32 cod_tipo_alerta { get; set; }

        public string descripcion_alerta { get; set; }
        public string ColorAlerta { get; set; }
        public double contador_bill_parcial { get; set; }
        public double contador_bill_billetero { get; set; }
        public string estado { get; set; }

        public string Nom_Sala { get; set; }
        public string RazonSocial { get; set; }

        //Relaciones
        public List<EVT_AlertaDispositivoEntidad> ListaAlertaDispositivo { get; set; }
        public AlertBillNotificationReqEntidad()
        {
            this.ListaAlertaDispositivo = new List<EVT_AlertaDispositivoEntidad>();
        }
    }

    public class estadoentidad
    {
        public int estado { get; set; }

    }
    public class EVT_AlertaDispositivoEntidad
    {
        public int AlertaId { get; set; }
        public int DispositivoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DispositivoNombre { get; set; }
        public string Usuario { get; set; }
    }
}
