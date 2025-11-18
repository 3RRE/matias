using System;

namespace CapaEntidad.Reportes.AperturaCaja
{
    public class AperturarCajaEntidad
    {
        public int Item { get; set; }
        public string CodEmpresa { get; set; }
        public string CodSala { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
        public int Turno { get; set; }
        public int CodCaja { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreSala { get; set; }
        public int TipoCaja { get; set; }
    }
}
