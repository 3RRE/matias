namespace CapaPresentacion.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADM_MaquinaSalaProgresivo
    {
        [Key]
        public int CodMaquinaSalaProgresivo { get; set; }

        public int? CodMaquina { get; set; }

        public int? CodSalaProgresivo { get; set; }

        public DateTime? FechaEnlace { get; set; }

        public DateTime? FechaDesactivacion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Activo { get; set; }

        public int? Estado { get; set; }

        [StringLength(100)]
        public string CodUsuario { get; set; }

        public virtual ADM_Maquina ADM_Maquina { get; set; }

        public virtual ADM_SalaProgresivo ADM_SalaProgresivo { get; set; }
    }
}
