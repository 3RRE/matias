using System;

namespace CapaEntidad
{
    public class ProyectadoProgresivoInsertEntidad
    {
        public int IdProyectadoProgresivo { get; set; }
        public string Descripcion { get; set; }
        public int NroMaquina { get; set; }
        public string TotalJugMes { get; set; }
        public string TipoCambio { get; set; }
        public string Retencion { get; set; }
        public string PremioBasePozoInferior { get; set; }
        public string PremioBasePozoMedio { get; set; }
        public string PremioBasePozoSuperior { get; set; }
        public string PremioMinimoPozoInferior { get; set; }
        public string PremioMinimoPozoMedio { get; set; }
        public string PremioMinimoPozoSuperior { get; set; }
        public string PremioMaximoPozoInferior { get; set; }
        public string PremioMaximoPozoMedio { get; set; }
        public string PremioMaximoPozoSuperior { get; set; }
        public string IncrementoPozoInferior { get; set; }
        public string IncrementoPozoMedio { get; set; }
        public string IncrementoPozoSuperior { get; set; }
    }

    public class ProyectadoProgresivoEntidad
    {
        public int IdProyectadoProgresivo { get; set; }
        public string Descripcion { get; set; }
        public int NroMaquina { get; set; }
        public decimal TotalJugMes { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal Retencion { get; set; }
        public decimal PremioBasePozoInferior { get; set; }
        public decimal PremioBasePozoMedio { get; set; }
        public decimal PremioBasePozoSuperior { get; set; }
        public decimal PremioMinimoPozoInferior { get; set; }
        public decimal PremioMinimoPozoMedio { get; set; }
        public decimal PremioMinimoPozoSuperior { get; set; }
        public decimal PremioMaximoPozoInferior { get; set; }
        public decimal PremioMaximoPozoMedio { get; set; }
        public decimal PremioMaximoPozoSuperior { get; set; }
        public decimal IncrementoPozoInferior { get; set; }
        public decimal IncrementoPozoMedio { get; set; }
        public decimal IncrementoPozoSuperior { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
