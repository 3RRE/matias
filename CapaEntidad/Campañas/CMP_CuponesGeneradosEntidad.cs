using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_CuponesGeneradosEntidad
    {
        public Int64 CgId { get; set; }
        public Int64 CampaniaId { get; set; }
        public Int64 ClienteId { get; set; }
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string Mail { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string NroDoc { get; set; }
        public int CodSala { get; set; }
        public string nombreSala { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string SlotId { get; set; }
        public string Juego { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Win { get; set; }
        public int Parametro { get; set; }
        public int ValorJuego { get; set; }
        public double CantidadCupones { get; set; }
        public double SaldoCupIni { get; set; }
        public double SaldoCupFin { get; set; }
        public string SerieIni { get; set; }
        public string SerieFin { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int Estado { get; set; }
        public List<CMP_DetalleCuponesGeneradosEntidad> DetalleCuponesGenerados { get; set; }
        public List<CMP_DetalleCuponesImpresosEntidad> DetalleCuponesImpresos { get; set; }
        //Impresora seleccionada
        public int impresora_id { get; set; }
        //Nombre Campania
        public string CampaniaNombre { get; set; }
        public string TipoCampania { get; set; }
        public long SesionId { get; set; }
        public CMP_CuponesGeneradosEntidad()
        {
            this.DetalleCuponesGenerados = new List<CMP_DetalleCuponesGeneradosEntidad>();
            this.DetalleCuponesImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
        }

    }
}
