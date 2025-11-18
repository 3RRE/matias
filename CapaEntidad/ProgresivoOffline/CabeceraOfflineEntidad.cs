using CapaEntidad.ProgresivoOffline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

    public class CabeceraOfflineEntidad
    {

        public int IdCabeceraProgresivo { get; set; }
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
        public string FechaStr { get; set; }
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
        public List<DetalleOfflineEntidad> listaDetalleOfflineEntidad { get; set; }
    }

    public class CabeceraOfflineComparacion
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
