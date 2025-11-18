using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CapaPresentacion.Context
{
    public partial class SeguridadPJContext : DbContext
    {
        public SeguridadPJContext()
            : base("conexion")
        {
        }

        public virtual DbSet<ADM_DetalleContadoresGame> ADM_DetalleContadoresGame { get; set; }
        public virtual DbSet<ADM_DetalleSalaProgresivo> ADM_DetalleSalaProgresivo { get; set; }
        public virtual DbSet<ADM_Ganador> ADM_Ganador { get; set; }
        public virtual DbSet<ADM_HistorialMaquina> ADM_HistorialMaquina { get; set; }
        public virtual DbSet<ADM_Maquina> ADM_Maquina { get; set; }
        public virtual DbSet<ADM_MaquinaSalaProgresivo> ADM_MaquinaSalaProgresivo { get; set; }
        public virtual DbSet<ADM_PozoHistorico> ADM_PozoHistorico { get; set; }
        public virtual DbSet<ADM_SalaProgresivo> ADM_SalaProgresivo { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CoinInIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CoinInFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CoinOutIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CoinOutFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.JackpotIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.JackpotFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.HandPayIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.HandPayFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CancelCreditIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CancelCreditFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.GamesPlayedIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.GamesPlayedFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.ProduccionPorSlot1)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.ProduccionPorSlot2Reset)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.ProduccionPorSlot3Rollover)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.ProduccionPorSlot4Prueba)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.ProduccionTotalPorSlot5Dia)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.TipoCambio)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.SaldoCoinIn)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.SaldoCoinOut)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.SaldoJackpot)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.SaldoGamesPlayed)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_DetalleContadoresGame>()
        //        .Property(e => e.TiempoJuego)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.MontoBase)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.MontoIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.MontoFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.MontoOcultoBase)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.MontoOcultoIni)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.MontoOcultoFin)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.Incremento)
        //        .HasPrecision(18, 4);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.IncrementoPozoOculto)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_DetalleSalaProgresivo>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_Ganador>()
        //        .Property(e => e.MontoProgresivo)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Ganador>()
        //        .Property(e => e.Token)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Ganador>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.CodMaquinaLey)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.CodAlterno)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.NroFabricacion)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.NroSerie)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.ValorComercial)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.ApuestaMaxima)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.ApuestaMinima)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.CreditoFicha)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.Token)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.PorcentajeDevolucion)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.ResumenCambios)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_HistorialMaquina>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.CodMaquinaLey)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.CodAlterno)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.NroFabricacion)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.NroSerie)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.ValorComercial)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.ApuestaMaxima)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.ApuestaMinima)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.CreditoFicha)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.Token)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.PorcentajeDevolucion)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_Maquina>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_MaquinaSalaProgresivo>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_PozoHistorico>()
        //        .Property(e => e.MontoActualAutomatico)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_PozoHistorico>()
        //        .Property(e => e.MontoActualSala)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_PozoHistorico>()
        //        .Property(e => e.MontoOcultoActualAutomatico)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_PozoHistorico>()
        //        .Property(e => e.MontoOcultoActualSala)
        //        .HasPrecision(18, 3);

        //    modelBuilder.Entity<ADM_PozoHistorico>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_SalaProgresivo>()
        //        .Property(e => e.ColorHexa)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_SalaProgresivo>()
        //        .Property(e => e.Sigla)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_SalaProgresivo>()
        //        .Property(e => e.Url)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_SalaProgresivo>()
        //        .Property(e => e.CodUsuario)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_SalaProgresivo>()
        //        .Property(e => e.Nombre)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<ADM_SalaProgresivo>()
        //        .Property(e => e.TipoProgresivo)
        //        .IsUnicode(false);
        //}
    }
}
