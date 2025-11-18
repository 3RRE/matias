namespace CapaPresentacion.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADM_PozoHistorico
    {
        [Key]
        public int CodPozoHistorico { get; set; }

        public int? CodDetalleSalaProgresivo { get; set; }

        public decimal? MontoActualAutomatico { get; set; }

        public decimal? MontoActualSala { get; set; }

        public decimal? MontoOcultoActualAutomatico { get; set; }

        public decimal? MontoOcultoActualSala { get; set; }

        public DateTime? FechaOperacion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public int? Estado { get; set; }

        public bool? Activo { get; set; }

        [StringLength(100)]
        public string CodUsuario { get; set; }

        public virtual ADM_DetalleSalaProgresivo ADM_DetalleSalaProgresivo { get; set; }
    }
}
