using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaEntidad.ProgresivoRuleta.Filtro;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ProgresivoRuleta {
    public class PRU_GanadorDAL {
        private readonly string _conexion;

        public PRU_GanadorDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<PRU_GanadorDto> ObtenerGanadoresByFechas(int salaId, DateTime fechaInicial, DateTime fechaFinal) {
            List<PRU_GanadorDto> items = new List<PRU_GanadorDto>();

            string query = @"
                SELECT
                    gnd.Id,
                    sal.CodSala AS SalaId,
                    sal.Nombre AS SalaNombre,
                    rlt.IdRuleta AS RuletaId,
                    rlt.NombreRuleta AS RuletaNombre,
                    gnd.CodMaquina,
                    gnd.Monto,
                    gnd.EsAcreditado,
                    gnd.FechaGanador,
                    gnd.FechaRegistro
                FROM PRU_Ganador gnd
                LEFT JOIN Sala sal ON sal.CodSala = gnd.CodSala
                LEFT JOIN PRU_Configuracion rlt ON rlt.IdRuleta = gnd.IdRuleta
                WHERE
                    gnd.CodSala = @SalaId
                    AND (gnd.EsAcreditado = 1 OR gnd.EsAcreditado = 2)
                    AND CONVERT(DATE, gnd.FechaGanador) BETWEEN CONVERT(DATE, @FechaInicial) AND CONVERT(DATE, @FechaFinal)
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@SalaId", salaId);
                    command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            items.Add(ConstruirObjetoDto(data));
                        }
                    }
                }
            } catch { }

            return items;
        }

        public PRU_GanadorDto ObtenerUltimoGanadorPorFiltro(PRU_Filtro filtro) {
            PRU_GanadorDto item = new PRU_GanadorDto();

            string query = @"
                SELECT TOP 1
                    gnd.Id,
                    sal.CodSala AS SalaId,
                    sal.Nombre AS SalaNombre,
                    rlt.IdRuleta AS RuletaId,
                    rlt.NombreRuleta AS RuletaNombre,
                    gnd.CodMaquina,
                    gnd.Monto,
                    gnd.EsAcreditado,
                    gnd.FechaGanador,
                    gnd.FechaRegistro
                FROM PRU_Ganador gnd
                LEFT JOIN Sala sal ON sal.CodSala = gnd.CodSala
                LEFT JOIN PRU_Configuracion rlt ON rlt.IdRuleta = gnd.IdRuleta
                WHERE
                    gnd.CodSala = @CodSala
                    AND gnd.IdRuleta = @IdRuleta
                    AND gnd.EsAcreditado = 1
                ORDER BY gnd.FechaGanador DESC, gnd.Id DESC
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CodSala", filtro.CodSala);
                    command.Parameters.AddWithValue("@IdRuleta", filtro.IdRuleta);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            item = ConstruirObjetoDto(data);
                        }
                    }
                }
            } catch { }

            return item;
        }

        public int InsertarGanador(PRU_Ganador ganador) {
            int idInsertado;

            string consulta = @"
                INSERT INTO PRU_Ganador(CodSala, IdRuleta, CodMaquina, Monto, EsAcreditado, FechaGanador)
                OUTPUT INSERTED.Id
                VALUES (@CodSala, @IdRuleta, @CodMaquina, @Monto, @EsAcreditado, @FechaGanador)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(ganador.CodSala));
                    query.Parameters.AddWithValue("@IdRuleta", ManejoNulos.ManageNullInteger(ganador.IdRuleta));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(ganador.CodMaquina));
                    query.Parameters.AddWithValue("@Monto", ManejoNulos.ManageNullDecimal(ganador.Monto));
                    query.Parameters.AddWithValue("@EsAcreditado", ManejoNulos.ManageNullInteger(ganador.EsAcreditado));
                    query.Parameters.AddWithValue("@FechaGanador", ManejoNulos.ManageNullDate(ganador.FechaGanador));
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int ActualizarAcreditacionGanador(int id, int esAcreditado) {
            int idActualizado;
            string consulta = @"
                UPDATE PRU_Ganador
                SET EsAcreditado = @EsAcreditado
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", ManejoNulos.ManageNullInteger(id));
                    query.Parameters.AddWithValue("@EsAcreditado", ManejoNulos.ManageNullInteger(esAcreditado));
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        private PRU_GanadorDto ConstruirObjetoDto(SqlDataReader data) {
            return new PRU_GanadorDto {
                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                Sala = new PRU_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(data["SalaId"]),
                    Nombre = ManejoNulos.ManageNullStr(data["SalaNombre"])
                },
                Ruleta = new PRU_RuletaDto {
                    Id = ManejoNulos.ManageNullInteger(data["RuletaId"]),
                    Nombre = ManejoNulos.ManageNullStr(data["RuletaNombre"])
                },
                CodMaquina = ManejoNulos.ManageNullStr(data["CodMaquina"]),
                Monto = ManejoNulos.ManageNullDecimal(data["Monto"]),
                EsAcreditado = ManejoNulos.ManageNullInteger(data["EsAcreditado"]),
                FechaGanador = ManejoNulos.ManageNullDate(data["FechaGanador"]),
                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"])
            };
        }
    }
}
