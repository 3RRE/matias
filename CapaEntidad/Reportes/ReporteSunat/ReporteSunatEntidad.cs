using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Reportes.ReporteSunat
{
    public class ReporteSunatEntidad
    {
        public DateTime fecha;
        public dynamic trama;
        public int cereo;
        public int idconsunat;
        public bool envio;
        public string idCereo;
        public DateTime Fecha_Proceso;
        public DateTime FechaEnvio;
        public string motivo;
        public int idConfSunat;
        public bool bandbusq;
    }


    public class SunatJsonEntidad
    {
        public string NumeroMaquina;
        public string Coint_int;
        public string Coint_out;
        public string jackpok;
        public string game_played;
        public string bonus_in;
        public string ticket_in;
        public string ticket_out;
        public string EftIn;
        public string EftOut;

    }


}
