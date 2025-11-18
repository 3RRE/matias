using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Alertas
{

    public class EventoEntidad
    {
        public int coqMaquina { get; set; }
        public int evento { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
    }

    public class LogAlertaBilleterosEvento
    {
        public int Id { get; set; }
        public long Cod_Even_OL { get; set; }
        public DateTime Fecha { get; set; }
        public string CodTarjeta { get; set; }
        public string CodMaquina { get; set; }
        public int Cod_Evento { get; set; }
        public string COD_EMPRESA { get; set; }
        public string COD_SALA { get; set; }
        public string Evento { get; set; }
        public int EstadoEnvio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public ListaEventoDispositivo[] ListaEventoDispositivo { get; set; }
    }
    public class ListaEventoDispositivo
    {
        public int DispositivoId { get; set; }
        public int EventoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DispositivoNombre { get; set; }
        public object Usuario { get; set; }
    }

    public class EVT_EventosOnlineEntidad
    {
        public int Id { get; set; }
        public int Cod_Even_OL { get; set; }
        public DateTime Fecha { get; set; }
        public string CodTarjeta { get; set; }
        public string CodMaquina { get; set; }
        public int Cod_Evento { get; set; }
        public string COD_EMPRESA { get; set; }
        public string COD_SALA { get; set; }
        public string Evento { get; set; }
        public int EstadoEnvio { get; set; }
        public DateTime FechaRegistro { get; set; }
        //Relaciones
        public List<EVT_EventoDispositivoEntidad> ListaEventoDispositivo { get; set; }
        public EVT_EventosOnlineEntidad()
        {
            this.ListaEventoDispositivo = new List<EVT_EventoDispositivoEntidad>();
        }
    }
}
