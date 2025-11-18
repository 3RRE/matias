namespace CapaPresentacion.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADM_DetalleSalaProgresivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADM_DetalleSalaProgresivo()
        {
            ADM_Ganador = new HashSet<ADM_Ganador>();
            ADM_PozoHistorico = new HashSet<ADM_PozoHistorico>();
        }

        [Key]
        public int CodDetalleSalaProgresivo { get; set; }

        public int? CodSalaProgresivo { get; set; }

        public int? NroPozo { get; set; }

        public int? Dificultad { get; set; }

        public decimal? MontoBase { get; set; }

        public decimal? MontoIni { get; set; }

        public decimal? MontoFin { get; set; }

        public int? Modalidad { get; set; }

        public decimal? MontoOcultoBase { get; set; }

        public decimal? MontoOcultoIni { get; set; }

        public decimal? MontoOcultoFin { get; set; }

        public decimal? Incremento { get; set; }

        public decimal? IncrementoPozoOculto { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Activo { get; set; }

        public int? Estado { get; set; }

        public int? CodProgresivoExterno { get; set; }

        [StringLength(100)]
        public string CodUsuario { get; set; }

        public DateTime? fechaIni { get; set; }

        public DateTime? fechaFin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADM_Ganador> ADM_Ganador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADM_PozoHistorico> ADM_PozoHistorico { get; set; }

        public virtual ADM_SalaProgresivo ADM_SalaProgresivo { get; set; }
    }
}
