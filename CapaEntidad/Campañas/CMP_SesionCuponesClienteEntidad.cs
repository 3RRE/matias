using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_SesionCuponesClienteEntidad
    {
        public Int64 SesionId { get; set; }
        public string CodMaquina { get; set; }
        public Int64 CgId { get; set; }
        public int Terminado { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string NombreSala { get; set; }
        public string NroDocumento { get; set; }
        public int Estado_Envio { get; set; }
        public int UsuarioIdIAS { get; set; }
        public string Prefijo { get; set; }
        public double CoinOutIAS { get; set; }
        public int TopeCuponesxJugada { get; set; }
        public string ParametrosImpresion { get; set; }

        //Nuevos Campos
        public int HijoTerminado { get; set; }
        public int HijoEnviado { get; set; }
        public int CantidadCupones { get; set; }
        public int CantidadJugadas { get; set; }
        public string SerieIni { get; set; }
        public string SerieFin { get; set; }
        public int CampaniaId { get; set; }
        public string Correo { get; set; }
    }
}
