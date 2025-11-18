using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public class ClaseLibre
    {
    }
    public class EmpleadoConfiguracionSeguridad
    {
        public int orden { get; set; }
        public int cantidadLetras { get; set; }
        public string valor { get; set; }
    }
    public class GanadorResponse
    {
        public int ProgresivoID { get; set; }
        public int DetalleProgresivoID { get; set; }
        public string ProcesosID { get; set; }
        public int GanadorID { get; set; }
        public string SlotID { get; set; }
        public double Monto { get; set; }
        public double Valor { get; set; }
        public int TipoPozo { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public double CoinInAct { get; set; }
        public double CoinInAnt { get; set; }
        public double Toquen { get; set; }
        public double CoinOut { get; set; }
        public double Jackpot { get; set; }
        public double Cancelcredits { get; set; }
        public double Billetero { get; set; }
        public double BonusWinAct { get; set; }
        public double BonusWinAnt { get; set; }
        public double CreditAct { get; set; }
        public double CreditAnt { get; set; }
        public int NroJugadores { get; set; }
        public double ValorReal { get; set; }
        public double NroJugada { get; set; }
        public object Usuario { get; set; }
        public double Pagado { get; set; }
        public object ClienteDNI { get; set; }
        public object Operador { get; set; }
        public DateTime FechaPago { get; set; }
        public int indice { get; set; }
        public string desc_pozo { get; set; }
        public string desc_estado { get; set; }
        public string desc_modelo { get; set; }
        public string desc_marca { get; set; }
        public string desc_imagen_progresivo { get; set; }
        public int cod_imagen_progresivo { get; set; }
        public int Cantidad { get; set; }
        public string desc_fecha { get; set; }
        public object desc_fecha_pago { get; set; }
        public string desc_hora_pago { get; set; }
        public string codalterno { get; set; }
        public object ListaGanador { get; set; }
        public string File { get; set; }
    }
    public class DetalleContadores
    {
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodMaq { get; set; }
        public double codevento { get; set; }
        public double Bonus1 { get; set; }
        public double Dif_Bonus1 { get; set; }
        public double Bonus2 { get; set; }
        public double Dif_Bonus2 { get; set; }
        public DateTime FechaCompleta { get; set; }
        public double CurrentCredits { get; set; }
    }
    public class ListadoImagen
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public object Archivo { get; set; }
    }
    public class ProgresivoActivo
    {
        public int ProgresivoID { get; set; }
        public int NroPozos { get; set; }
        public bool PorCredito { get; set; }
        public bool BaseOculto { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public int NroJugadores { get; set; }
        public int ProgresivoImagenID { get; set; }
        public bool PagoCaja { get; set; }
        public int DuracionPantalla { get; set; }
        public string Simbolo { get; set; }
        public int Estado { get; set; }
        public object FechaIni_desc { get; set; }
        public object FechaFin_desc { get; set; }
        public int indice { get; set; }
        public object Estado_desc { get; set; }
        public object ProgresivoImagen_desc { get; set; }
        public bool RegHistorico { get; set; }
    }
    public class PozosActuales
    {
        public int ProgresivoID { get; set; }
        public int DetalleProgresivoID { get; set; }
        public int PozoID { get; set; }
        public double Actual { get; set; }
        public double Anterior { get; set; }
        public double ActualOculto { get; set; }
        public double AnteriorOculto { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoPozo { get; set; }
        public int Estado { get; set; }
    }
    public class ProgresivoDetalle
    {
        public int ProgresivoID { get; set; }
        public int DetalleProgresivoID { get; set; }
        public int TipoPozo { get; set; }
        public double MontoMin { get; set; }
        public double MontoBase { get; set; }
        public double MontoMax { get; set; }
        public double IncPozo1 { get; set; }
        public double IncPozo2 { get; set; }
        public double MontoOcMin { get; set; }
        public double MontoOcMax { get; set; }
        public double IncOcPozo1 { get; set; }
        public double IncOcPozo2 { get; set; }
        public bool Parametro { get; set; }
        public double Punto { get; set; }
        public double Prob1 { get; set; }
        public double Prob2 { get; set; }
        public int Indice { get; set; }
        public int EstadoInicial { get; set; }
        public int Dificultad { get; set; }
        public int Estado { get; set; }
        public int RsJugadores { get; set; }
        public int RsApuesta { get; set; }
        public string Dificultad_desc { get; set; }
        public string Estado_desc { get; set; }
        public double TrigMin { get; set; }
        public double TrigMax { get; set; }
        public int Top { get; set; }
        public int TopAnt { get; set; }
        public object TMin { get; set; }
        public object TMax { get; set; }
    }
    public class DetalleProgresivo
    {
        public Int64 ProgresivoID { get; set; }
        public Int64 DetalleProgresivoID { get; set; }
        public Int64 TipoPozo { get; set; }
        public Double MontoMin { get; set; }
        public Double MontoBase { get; set; }
        public Double MontoMax { get; set; }
        public Double IncPozo1 { get; set; }
        public Double IncPozo2 { get; set; }
        public Double MontoOcMin { get; set; }
        public Double MontoOcMax { get; set; }
        public Double IncOcPozo1 { get; set; }
        public Double IncOcPozo2 { get; set; }
        public bool Parametro { get; set; }
        public double Punto { get; set; }
        public Double Prob1 { get; set; }
        public Double Prob2 { get; set; }
        public Int64 Indice { get; set; }
        public Int64 EstadoInicial { get; set; }
        public Int64 Dificultad { get; set; }
        public Int64 Estado { get; set; }

        public int RsJugadores { get; set; }
        public Int64 RsApuesta { get; set; }
        public String Dificultad_desc { get; set; }
        public String Estado_desc { get; set; }
        public double TrigMin
        {
            get;
            set;
        }
        public double TrigMax
        {
            get;
            set;
        }
        /// <summary>
        /// diferencia de resta para el maxi y min
        /// </summary>
        /// <value>The top.</value>
        public int Top
        {
            get;
            set;
        }

        public int TopAnt
        {
            get;
            set;
        }
        public double[] TMin { get; set; }
        public double[] TMax { get; set; }

        public double Actual { get; set; }        
    }

    public class ProgresivoHistorico
    {
        public ProgresivoActivo ProgresivoActivo { get; set; }
        public List<PozosActuales> PozosActuales { get; set; }
        public List<DetalleProgresivo> DetalleProgresivo { get; set; }
    }

    public class signalrUser
    {
        public string conection_id { get; set; }
        public int usuario_id { get; set; }
    }


    public class ReporteProgresivoGanadorResponse
    {
        public int CodSala { get; set; }
        public int CodProgresivo { get; set; }
        public int ProgresivoID { get; set; }
        public int DetalleProgresivoID { get; set; }
        public string ProcesosID { get; set; }
        public int GanadorID { get; set; }
        public string SlotID { get; set; }
        public double Monto { get; set; }
        public double Valor { get; set; }
        public int TipoPozo { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public double CoinInAct { get; set; }
        public double CoinInAnt { get; set; }
        public double Toquen { get; set; }
        public double CoinOut { get; set; }
        public double Jackpot { get; set; }
        public double Cancelcredits { get; set; }
        public double Billetero { get; set; }
        public double BonusWinAct { get; set; }
        public double BonusWinAnt { get; set; }
        public double CreditAct { get; set; }
        public double CreditAnt { get; set; }
        public int NroJugadores { get; set; }
        public double ValorReal { get; set; }
        public double NroJugada { get; set; }
        public object Usuario { get; set; }
        public double Pagado { get; set; }
        public object ClienteDNI { get; set; }
        public object Operador { get; set; }
        public DateTime FechaPago { get; set; }
        public int indice { get; set; }
        public string desc_pozo { get; set; }
        public string desc_estado { get; set; }
        public string desc_modelo { get; set; }
        public string desc_marca { get; set; }
        public string desc_imagen_progresivo { get; set; }
        public int cod_imagen_progresivo { get; set; }
        public int Cantidad { get; set; }
        public string desc_fecha { get; set; }
        public object desc_fecha_pago { get; set; }
        public string desc_hora_pago { get; set; }
        public string codalterno { get; set; }
        public object ListaGanador { get; set; }
        public string File { get; set; }
    }

}