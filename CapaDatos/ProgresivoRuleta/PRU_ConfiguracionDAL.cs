using CapaEntidad.ProgresivoRuleta.Config;
using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaEntidad.ProgresivoRuleta.Filtro;
using S3k.Utilitario;
using S3k.Utilitario.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ProgresivoRuleta {
    public class PRU_ConfiguracionDAL {
        private readonly string _conexion;

        public PRU_ConfiguracionDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarConfiguracion(PRU_Configuracion configuracion) {
            int idActualizado;
            string consulta = @"
                UPDATE PRU_Configuracion
                SET
                    NombreRuleta = @NombreRuleta,
                    TotalSlots = @TotalSlots,
                    CodMaquinas = @CodMaquinas,
                    Posiciones = @Posiciones,
                    MinSlotsPlaying = @MinSlotsPlaying,
                    CoinInPercent = @CoinInPercent,
                    MinBet = @MinBet,
                    Ip = @Ip,
                    StatusOk = @StatusOk,
                    HoraInicio = @HoraInicio,
                    HoraFin = @HoraFin,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.IdRuleta
                WHERE IdRuleta = @IdRuleta
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdRuleta", ManejoNulos.ManageNullInteger(configuracion.IdRuleta));
                    query.Parameters.AddWithValue("@NombreRuleta", ManejoNulos.ManageNullStr(configuracion.NombreRuleta));
                    query.Parameters.AddWithValue("@TotalSlots", ManejoNulos.ManageNullInteger(configuracion.TotalSlots));
                    query.Parameters.AddWithValue("@CodMaquinas", ManejoNulos.ManageNullStr(configuracion.CodMaquinas));
                    query.Parameters.AddWithValue("@Posiciones", ManejoNulos.ManageNullStr(configuracion.Posiciones));
                    query.Parameters.AddWithValue("@MinSlotsPlaying", ManejoNulos.ManageNullInteger(configuracion.MinSlotsPlaying));
                    query.Parameters.AddWithValue("@CoinInPercent", ManejoNulos.ManageNullDecimal(configuracion.CoinInPercent));
                    query.Parameters.AddWithValue("@MinBet", ManejoNulos.ManageNullInteger(configuracion.MinBet));
                    query.Parameters.AddWithValue("@Ip", ManejoNulos.ManageNullStr(configuracion.Ip));
                    query.Parameters.AddWithValue("@StatusOk", ManejoNulos.ManegeNullBool(configuracion.StatusOk));
                    query.Parameters.AddWithValue("@HoraInicio", ManejoNulos.ManageNullTimespan(configuracion.HoraInicio));
                    query.Parameters.AddWithValue("@HoraFin", ManejoNulos.ManageNullTimespan(configuracion.HoraFin));
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int InsertarConfiguracion(PRU_Configuracion configuracion) {
            int idInsertado;
            string consulta = @"
                INSERT INTO PRU_Configuracion(CodSala, NombreRuleta, TotalSlots, CodMaquinas, Posiciones, MinSlotsPlaying, CoinInPercent, MinBet, Ip, StatusOk, HoraInicio, HoraFin)
                OUTPUT INSERTED.IdRuleta
                VALUES (@CodSala, @NombreRuleta, @TotalSlots, @CodMaquinas, @Posiciones, @MinSlotsPlaying, @CoinInPercent, @MinBet, @Ip, @StatusOk, @HoraInicio, @HoraFin)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(configuracion.CodSala));
                    query.Parameters.AddWithValue("@NombreRuleta", ManejoNulos.ManageNullStr(configuracion.NombreRuleta));
                    query.Parameters.AddWithValue("@TotalSlots", ManejoNulos.ManageNullInteger(configuracion.TotalSlots));
                    query.Parameters.AddWithValue("@CodMaquinas", ManejoNulos.ManageNullStr(configuracion.CodMaquinas));
                    query.Parameters.AddWithValue("@Posiciones", ManejoNulos.ManageNullStr(configuracion.Posiciones));
                    query.Parameters.AddWithValue("@MinSlotsPlaying", ManejoNulos.ManageNullInteger(configuracion.MinSlotsPlaying));
                    query.Parameters.AddWithValue("@CoinInPercent", ManejoNulos.ManageNullDecimal(configuracion.CoinInPercent));
                    query.Parameters.AddWithValue("@MinBet", ManejoNulos.ManageNullInteger(configuracion.MinBet));
                    query.Parameters.AddWithValue("@Ip", ManejoNulos.ManageNullStr(configuracion.Ip));
                    query.Parameters.AddWithValue("@StatusOk", ManejoNulos.ManegeNullBool(configuracion.StatusOk));
                    query.Parameters.AddWithValue("@HoraInicio", ManejoNulos.ManageNullTimespan(configuracion.HoraInicio));
                    query.Parameters.AddWithValue("@HoraFin", ManejoNulos.ManageNullTimespan(configuracion.HoraFin));
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public PRU_ConfiguracionDto ObtenerConfiguracionPorId(int id) {
            PRU_ConfiguracionDto item = new PRU_ConfiguracionDto();
            string consulta = @"
                SELECT
                    c.IdRuleta,
                    s.CodSala,
                    s.Nombre AS NombreSala,
                    c.NombreRuleta,
                    c.TotalSlots,
                    c.CodMaquinas,
                    c.Posiciones,
                    c.MinSlotsPlaying,
                    c.CoinInPercent,
                    c.MinBet,
                    c.Ip,
                    c.StatusOk,
                    c.HoraInicio,
                    c.HoraFin
                FROM PRU_Configuracion AS c
                INNER JOIN Sala AS s ON s.CodSala = c.CodSala
                WHERE c.IdRuleta = @IdRuleta
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdRuleta", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjetoDto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<PRU_ConfiguracionDto> ObtenerConfiguraciones() {
            List<PRU_ConfiguracionDto> items = new List<PRU_ConfiguracionDto>();
            string consulta = @"
                SELECT
                    c.IdRuleta,
                    s.CodSala,
                    s.Nombre AS NombreSala,
                    c.NombreRuleta,
                    c.TotalSlots,
                    c.CodMaquinas,
                    c.Posiciones,
                    c.MinSlotsPlaying,
                    c.CoinInPercent,
                    c.MinBet,
                    c.Ip,
                    c.StatusOk,
                    c.HoraInicio,
                    c.HoraFin
                FROM PRU_Configuracion AS c
                INNER JOIN Sala AS s ON s.CodSala = c.CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoDto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<PRU_ConfiguracionDto> ObtenerConfiguracionesPorCodSala(int codSala) {
            List<PRU_ConfiguracionDto> items = new List<PRU_ConfiguracionDto>();
            string consulta = @"
                SELECT
                    c.IdRuleta,
                    s.CodSala,
                    s.Nombre AS NombreSala,
                    c.NombreRuleta,
                    c.TotalSlots,
                    c.CodMaquinas,
                    c.Posiciones,
                    c.MinSlotsPlaying,
                    c.CoinInPercent,
                    c.MinBet,
                    c.Ip,
                    c.StatusOk,
                    c.HoraInicio,
                    c.HoraFin
                FROM PRU_Configuracion AS c
                INNER JOIN Sala AS s ON s.CodSala = c.CodSala
                WHERE c.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoDto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public Pru_MisteryConfig ObtenerConfiguracionPorFiltro(PRU_Filtro filtro) {
            Pru_MisteryConfig item = new Pru_MisteryConfig();
            string consulta = @"
                SELECT
                    TotalSlots,
                    CodMaquinas,
                    Posiciones,
                    MinSlotsPlaying,
                    CoinInPercent,
                    MinBet,
                    Ip,
                    StatusOk,
                    HoraInicio,
                    HoraFin
                FROM PRU_Configuracion
                WHERE CodSala = @CodSala AND IdRuleta = @IdRuleta
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", filtro.CodSala);
                    query.Parameters.AddWithValue("@IdRuleta", filtro.IdRuleta);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjetoConfig(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        private PRU_ConfiguracionDto ConstruirObjetoDto(SqlDataReader dr) {
            return new PRU_ConfiguracionDto {
                IdRuleta = ManejoNulos.ManageNullInteger(dr["IdRuleta"]),
                Sala = new PRU_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"])
                },
                NombreRuleta = ManejoNulos.ManageNullStr(dr["NombreRuleta"]),
                TotalSlots = ManejoNulos.ManageNullInteger(dr["TotalSlots"]),
                CodMaquinas = ManejoNulos.ManageNullStr(dr["CodMaquinas"]),
                Posiciones = ManejoNulos.ManageNullStr(dr["Posiciones"]),
                SlotHexValues = ManejoNulos.ManageNullStr(dr["CodMaquinas"]).ToCleanedList(),
                SlotHexPositions = ManejoNulos.ManageNullStr(dr["Posiciones"]).ToCleanedIntList(),
                MinSlotsPlaying = ManejoNulos.ManageNullInteger(dr["MinSlotsPlaying"]),
                CoinInPercent = ManejoNulos.ManageNullDecimal(dr["CoinInPercent"]),
                MinBet = ManejoNulos.ManageNullInteger(dr["MinBet"]),
                Ip = ManejoNulos.ManageNullStr(dr["Ip"]),
                StatusOk = ManejoNulos.ManegeNullBool(dr["StatusOk"]),
                HoraInicio = ManejoNulos.ManageNullTimespan(dr["HoraInicio"]),
                HoraFin = ManejoNulos.ManageNullTimespan(dr["HoraFin"]),
            };
        }

        private Pru_MisteryConfig ConstruirObjetoConfig(SqlDataReader dr) {
            return new Pru_MisteryConfig {
                TotalSlots = ManejoNulos.ManageNullInteger(dr["TotalSlots"]),
                SlotHexValues = ManejoNulos.ManageNullStr(dr["CodMaquinas"]).ToCleanedList(),
                SlotHexPositions = ManejoNulos.ManageNullStr(dr["Posiciones"]).ToCleanedIntList(),
                MinSlotsPlaying = ManejoNulos.ManageNullInteger(dr["MinSlotsPlaying"]),
                CoinInPercent = ManejoNulos.ManageNullDecimal(dr["CoinInPercent"]),
                MinBet = ManejoNulos.ManageNullInteger(dr["MinBet"]),
                Ip = ManejoNulos.ManageNullStr(dr["Ip"]),
                StatusOk = ManejoNulos.ManegeNullBool(dr["StatusOk"]),
                HoraInicio = ManejoNulos.ManageNullTimespan(dr["HoraInicio"]).ToString(@"hh\:mm"),
                HoraFin = ManejoNulos.ManageNullTimespan(dr["HoraFin"]).ToString(@"hh\:mm"),
            };
        }
    }
}
