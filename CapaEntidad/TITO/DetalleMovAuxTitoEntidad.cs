using System;
using System.Collections.Generic;

namespace CapaEntidad.TITO
{
    public class DetalleMovAuxTitoEntidad
    {
        public double TitoCortesia { get; set; }
        public double TitoCortesiaNoDest { get; set; }
        public double TitoPromocion { get; set; }
        public double TitoPromocionNoDest { get; set; }
        public int Estado { get; set; }
        public DateTime FechaTicketIni { get; set; }
        public DateTime FechaTicketFin { get; set; }
    }

    public class DetalleMovAuxTitoResponse
    {
        public bool respuesta { get; set; }
        public List<DetalleMovAuxTitoEntidad> data { get; set; }
    }
}
