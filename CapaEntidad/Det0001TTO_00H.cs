using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Det0001TTO_00H
    {
        public int Item { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public string Tipo_venta { get; set; }
        public string Punto_venta { get; set; }
        public DateTime Tito_fechaini { get; set; }
        public string Tito_NroTicket { get; set; }
        public double Tito_MontoTicket { get; set; }
        public double Tito_MTicket_NoCobrable { get; set; }
        public string Tipo_venta_fin { get; set; }
        public string Punto_venta_fin { get; set; }
        public DateTime Tito_fechafin { get; set; }
        public int codclie { get; set; }
        public string tipo_ticket { get; set; }
        public int Estado { get; set; }
        public int IdTipoMoneda { get; set; }
        public string Motivo { get; set; }
        public int IdTipoPago { get; set; }
        public int Tipo_Proceso { get; set; }
        public int r_Estado { get; set; }
        public int Tipo_Ingreso { get; set; }
        public DateTime? Fecha_Apertura_Real { get; set; }
        public string PuntoVentaMin { get; set; }
        public DateTime fecha_reactiva { get; set; }
        public int turno { get; set; }
        public int codCaja { get; set; }
        public int player_tracking { get; set; }

        public string MaquinaCaja { get; set; }
        public string juego { get; set; }
        public string marca { get; set; }
        public int codAperturaCajaIni { get; set; }
        public string CodigoMaquina { get; set; }
        public int CodSala { get; set; }
        public string Apeclie { get; set; }
        public string NomClie { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public string FechaNacimiento { get; set; }

    }

    public class objetoAT
    {
        public List<Det0001TTO_00H> data { get; set; }
        public bool respuesta { get; set; }
        public string mensaje { get; set; }
    }

    public class HistorialTicketAT
    {
        public int id { get; set; }
        public int Item { get; set; }

        public int CodSala { get; set; }
        public string nombreSala { get; set; }
        public string Tito_NroTicket { get; set; }
        public double Tito_MontoTicket { get; set; }

        public string MaquinaCaja { get; set; }
        public string tipo_ticket { get; set; }
        public DateTime Tito_fechaini { get; set; }
        public string juego { get; set; }
        public string marca { get; set; }
        public int codAperturaCajaIni { get; set; }
        public string CodigoMaquina { get; set; }
        public int IdTipoMoneda { get; set; }
        public DateTime fecharegistro { get; set; }
        public int UsuarioID { get; set; }
        public string UsuarioNombre { get; set; }
    }
}
