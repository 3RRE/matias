using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_movaux_por_maquinaEntidad
    {
        public int item_maqui { get; set; }
        public int item { get; set; }
        public int idtipoficha { get; set; }
        public int idtipomoneda { get; set; }
        public double valorficha { get; set; }
        public int nro { get; set; }
        public string maq_alterno { get; set; }
        public string modelo { get; set; }
        public string tipovalor { get; set; }
        public double salida { get; set; }
        public double ingreso { get; set; }
        public double pagomanual { get; set; }
        public string valor_es_un { get; set; }
        public string estado { get; set; }
        public string tipomaquina { get; set; }
        public int item_pmr { get; set; }
        public string nro_boleta { get; set; }
        public double CancelldCredit_Fin { get; set; }
        public double JackPot_Fin { get; set; }
        public DateTime hora_registro { get; set; }
        public int estado_registro { get; set; }
        public int codPer { get; set; }
        public int Cod_Proceso { get; set; }
        public int Cod_Caja { get; set; }
        public string NroTicket { get; set; }
        public string cliente_dni { get; set; }
        public string serie_num { get; set; }
        public int cod_asignacion_ficha { get; set; }
        public int IngresoManual { get; set; }
        public string CAJERA_GSI { get; set; }
        public int var_tablet { get; set; }
    }
}
