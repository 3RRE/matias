namespace CapaPresentacion.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADM_SalaProgresivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADM_SalaProgresivo()
        {
            ADM_DetalleSalaProgresivo = new HashSet<ADM_DetalleSalaProgresivo>();
            ADM_MaquinaSalaProgresivo = new HashSet<ADM_MaquinaSalaProgresivo>();
        }

        [Key]
        public int CodSalaProgresivo { get; set; }

        public int? CodSala { get; set; }

        public int? CodProgresivo { get; set; }

        public int? NroPozos { get; set; }

        public int? NroJugadores { get; set; }

        public int? SubidaCreditos { get; set; }

        public DateTime? FechaInstalacion { get; set; }

        public DateTime? FechaDesinstalacion { get; set; }

        [StringLength(16)]
        public string ColorHexa { get; set; }

        [StringLength(10)]
        public string Sigla { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Activo { get; set; }

        public int? Estado { get; set; }

        [StringLength(100)]
        public string Url { get; set; }

        [StringLength(100)]
        public string CodUsuario { get; set; }

        public int? CodProgresivoWO { get; set; }

        public int? CodTipoConfiguracionProgresivo { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string TipoProgresivo { get; set; }
        [StringLength(200)]
        public string NombreSala { get; set; }
        [StringLength(200)]
        public string RazonSocial { get; set; }
        public string ClaseProgresivo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADM_DetalleSalaProgresivo> ADM_DetalleSalaProgresivo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADM_MaquinaSalaProgresivo> ADM_MaquinaSalaProgresivo { get; set; }
    }
}
