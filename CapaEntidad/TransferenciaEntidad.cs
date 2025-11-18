using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TransferenciaEntidad
    {
    }

    public class Transferencia
    {
        public int TransferenciaID { get; set; }
        public int TransferenciaSala { get; set; }
        public int Codsala { get; set; }
        public string nombresala { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApelPat { get; set; }
        public string ClienteApelMat { get; set; }
        public string TipoDocNombre { get; set; }
        public string ClienteNroDoc { get; set; }
        public int DetalleID { get; set; }
            
        public double Monto { get; set; }
        public string BancoNombre { get; set; }
        public string NroCuenta { get; set; }
        public string NroOperacion { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaReg { get; set; }
        public string FechaRegString { get; set; }
        
        public DateTime FechaAct { get; set; }
        public int Estado { get; set; }
        public int UsuarioID { get; set; }
        public string UsuarioNombre { get; set; }
        public string ImagenVoucher { get; set; }
        public int SolicitudTransferenciaID { get; set; }
        public string usuariosala { get; set; }
        public int NroTickets { get; set; }
        public string imagenbase64voucher { get; set; }
        public string UsuarioNombreReg { get; set; }
        public int SolicitudSala { get; set; }
    }

    public class SolicitudTransferencia
    {
        public int SolicitudID { get; set; }
        public int SolicitudSala { get; set; }
        public int Codsala { get; set; }
        public string nombresala { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApelPat { get; set; }
        public string ClienteApelMat { get; set; }
        public string TipoDocNombre { get; set; }
        public string ClienteNroDoc { get; set; }

        public double Monto { get; set; }
        public string NroTickets { get; set; }
        public DateTime FechaReg { get; set; }
        public int Estado { get; set; }
        public List<SolicitudTicket> Tickets { get; set; }

        public string BancoNombre { get; set; }
        public string NroCuenta { get; set; }
        public string FechaRegString {get;set;}
        public string UsuarioNombreReg { get; set; }
        public SolicitudTransferencia()
        {
            this.Tickets = new List<SolicitudTicket>();
        }
        
    }

    public class SolicitudTicket
    {
        public int SolicitudTicketID { get; set; }
        public string NroTicketTito { get; set; }
        public int SolicitudID { get; set; }
        
        public DateTime FechaReg { get; set; }
        public double Monto { get; set; }

    }
}

