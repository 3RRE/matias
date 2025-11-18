using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ProgresivoRuleta {
    public class PRU_AlertaDAL {
        private readonly string _conexion;

        public PRU_AlertaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<PRU_AlertaDto> ObtenerAlertasByFechas(int salaId, DateTime fechaInicial, DateTime fechaFinal) {
            List<PRU_AlertaDto> items = new List<PRU_AlertaDto>();

            string query = @"
            SELECT
                alt.Id,
                sal.CodSala AS SalaId,
                sal.Nombre AS SalaNombre,
                rlt.IdRuleta AS RuletaId,
                rlt.NombreRuleta AS RuletaNombre,
                alt.Detalle,
                alt.CodMaquina,
                alt.Monto,
                alt.FechaAlerta,
                alt.FechaRegistro
            FROM PRU_Alerta alt
            LEFT JOIN Sala sal ON sal.CodSala = alt.CodSala
            LEFT JOIN PRU_Configuracion rlt ON rlt.IdRuleta = alt.IdRuleta
            WHERE
                alt.CodSala = @SalaId
                AND CONVERT(DATE, alt.FechaAlerta) BETWEEN CONVERT(DATE, @FechaInicial) AND CONVERT(DATE, @FechaFinal)
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

        public int InsertarAlerta(PRU_Alerta alerta) {
            int idInsertado;

            string consulta = @"
                INSERT INTO PRU_Alerta(CodSala, IdRuleta, Detalle, CodMaquina, Monto, FechaAlerta)
                OUTPUT INSERTED.Id
                VALUES (@CodSala, @IdRuleta, @Detalle, @CodMaquina, @Monto, @FechaAlerta)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(alerta.CodSala));
                    query.Parameters.AddWithValue("@IdRuleta", ManejoNulos.ManageNullInteger(alerta.IdRuleta));
                    query.Parameters.AddWithValue("@Detalle", ManejoNulos.ManageNullStr(alerta.Detalle));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(alerta.CodMaquina));
                    query.Parameters.AddWithValue("@Monto", ManejoNulos.ManageNullDecimal(alerta.Monto));
                    query.Parameters.AddWithValue("@FechaAlerta", ManejoNulos.ManageNullDate(alerta.FechaAlerta));
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        private PRU_AlertaDto ConstruirObjetoDto(SqlDataReader data) {
            return new PRU_AlertaDto {
                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                Sala = new PRU_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(data["SalaId"]),
                    Nombre = ManejoNulos.ManageNullStr(data["SalaNombre"])
                },
                Ruleta = new PRU_RuletaDto {
                    Id = ManejoNulos.ManageNullInteger(data["RuletaId"]),
                    Nombre = ManejoNulos.ManageNullStr(data["RuletaNombre"])
                },
                Detalle = ManejoNulos.ManageNullStr(data["Detalle"]),
                CodMaquina = ManejoNulos.ManageNullStr(data["CodMaquina"]),
                Monto = ManejoNulos.ManageNullDecimal(data["Monto"]),
                FechaAlerta = ManejoNulos.ManageNullDate(data["FechaAlerta"]),
                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"])
            };
        }
    }
}
