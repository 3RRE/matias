using CapaEntidad.SatisfaccionCliente.DTO.Configuracion;
using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Entity.Configuracion;
using CapaEntidad.SatisfaccionCliente.Enum;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.SatisfaccionCliente.Configuracion {
    public class ESC_ConfiguracionDAL {
        private readonly string _conexion;

        public ESC_ConfiguracionDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarConfiguracion(ESC_Configuracion configuracion) {
            int idActualizado;
            string consulta = @"
                UPDATE ESC_Configuracion
                SET
                    TipoValidacionEnvioRespuesta = @TipoValidacionEnvioRespuesta,
                    TiempoEsperaRespuesta = @TiempoEsperaRespuesta,
                    MensajeTiempoEsperaRespuesta = @MensajeTiempoEsperaRespuesta,
                    EnvioMaximoDiario = @EnvioMaximoDiario,
                    MensajeEnvioMaximoDiario = @MensajeEnvioMaximoDiario,
                    EnvioMaximoMensual = @EnvioMaximoMensual,
                    MensajeEnvioMaximoMensual = @MensajeEnvioMaximoMensual,
	                RespuestasAnonimas = @RespuestasAnonimas,
                    EncuestaActiva = @EncuestaActiva,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", configuracion.Id);
                    query.Parameters.AddWithValue("@TipoValidacionEnvioRespuesta", configuracion.TipoValidacionEnvioRespuesta);
                    query.Parameters.AddWithValue("@TiempoEsperaRespuesta", configuracion.TiempoEsperaRespuesta);
                    query.Parameters.AddWithValue("@MensajeTiempoEsperaRespuesta", configuracion.MensajeTiempoEsperaRespuesta);
                    query.Parameters.AddWithValue("@EnvioMaximoDiario", configuracion.EnvioMaximoDiario);
                    query.Parameters.AddWithValue("@MensajeEnvioMaximoDiario", configuracion.MensajeEnvioMaximoDiario);
                    query.Parameters.AddWithValue("@EnvioMaximoMensual", configuracion.EnvioMaximoMensual);
                    query.Parameters.AddWithValue("@MensajeEnvioMaximoMensual", configuracion.MensajeEnvioMaximoMensual);
                    query.Parameters.AddWithValue("@RespuestasAnonimas", configuracion.RespuestasAnonimas);
                    query.Parameters.AddWithValue("@EncuestaActiva", configuracion.EncuestaActiva);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int InsertarConfiguracion(ESC_Configuracion configuracion) {
            int idInsertado;
            string consulta = @"
                INSERT INTO ESC_Configuracion(CodSala, TipoValidacionEnvioRespuesta, TiempoEsperaRespuesta, MensajeTiempoEsperaRespuesta, EnvioMaximoDiario, MensajeEnvioMaximoDiario, EnvioMaximoMensual, MensajeEnvioMaximoMensual, RespuestasAnonimas, EncuestaActiva)
                OUTPUT INSERTED.Id
                VALUES (@CodSala, @TipoValidacionEnvioRespuesta, @TiempoEsperaRespuesta, @MensajeTiempoEsperaRespuesta, @EnvioMaximoDiario, @MensajeEnvioMaximoDiario, @EnvioMaximoMensual, @MensajeEnvioMaximoMensual, @RespuestasAnonimas, @EncuestaActiva)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", configuracion.CodSala);
                    query.Parameters.AddWithValue("@TipoValidacionEnvioRespuesta", configuracion.TipoValidacionEnvioRespuesta);
                    query.Parameters.AddWithValue("@TiempoEsperaRespuesta", configuracion.TiempoEsperaRespuesta);
                    query.Parameters.AddWithValue("@MensajeTiempoEsperaRespuesta", configuracion.MensajeTiempoEsperaRespuesta);
                    query.Parameters.AddWithValue("@EnvioMaximoDiario", configuracion.EnvioMaximoDiario);
                    query.Parameters.AddWithValue("@MensajeEnvioMaximoDiario", configuracion.MensajeEnvioMaximoDiario);
                    query.Parameters.AddWithValue("@EnvioMaximoMensual", configuracion.EnvioMaximoMensual);
                    query.Parameters.AddWithValue("@MensajeEnvioMaximoMensual", configuracion.MensajeEnvioMaximoMensual);
                    query.Parameters.AddWithValue("@RespuestasAnonimas", configuracion.RespuestasAnonimas);
                    query.Parameters.AddWithValue("@EncuestaActiva", configuracion.EncuestaActiva);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public ESC_ConfiguracionDto ObtenerConfiguracionPorId(int id) {
            ESC_ConfiguracionDto item = new ESC_ConfiguracionDto();
            string consulta = @"
                SELECT
	                con.Id,
	                con.CodSala,
	                s.Nombre AS NombreSala,
                    con.TipoValidacionEnvioRespuesta,
	                con.TiempoEsperaRespuesta,
	                con.MensajeTiempoEsperaRespuesta,
                    con.EnvioMaximoDiario,
                    con.MensajeEnvioMaximoDiario,
	                con.EnvioMaximoMensual,
	                con.MensajeEnvioMaximoMensual,
	                con.RespuestasAnonimas,
	                con.EncuestaActiva
                FROM
	                ESC_Configuracion AS con
                INNER JOIN Sala AS s ON s.CodSala = con.CodSala
                WHERE
	                con.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<ESC_ConfiguracionDto> ObtenerConfiguraciones() {
            List<ESC_ConfiguracionDto> items = new List<ESC_ConfiguracionDto>();
            string consulta = @"
                SELECT
	                con.Id,
	                con.CodSala,
	                s.Nombre AS NombreSala,
                    con.TipoValidacionEnvioRespuesta,
	                con.TiempoEsperaRespuesta,
	                con.MensajeTiempoEsperaRespuesta,
                    con.EnvioMaximoDiario,
                    con.MensajeEnvioMaximoDiario,
	                con.EnvioMaximoMensual,
	                con.MensajeEnvioMaximoMensual,
	                con.RespuestasAnonimas,
	                con.EncuestaActiva
                FROM
	                ESC_Configuracion AS con
                INNER JOIN Sala AS s ON s.CodSala = con.CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public ESC_ConfiguracionDto ObtenerConfiguracionPorCodSala(int codSala) {
            ESC_ConfiguracionDto items = new ESC_ConfiguracionDto();
            string consulta = @"
                SELECT
	                con.Id,
	                con.CodSala,
	                s.Nombre AS NombreSala,
                    con.TipoValidacionEnvioRespuesta,
	                con.TiempoEsperaRespuesta,
	                con.MensajeTiempoEsperaRespuesta,
                    con.EnvioMaximoDiario,
                    con.MensajeEnvioMaximoDiario,
	                con.EnvioMaximoMensual,
	                con.MensajeEnvioMaximoMensual,
	                con.RespuestasAnonimas,
	                con.EncuestaActiva
                FROM
	                ESC_Configuracion AS con
                INNER JOIN Sala AS s ON s.CodSala = con.CodSala
                WHERE
	                con.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return items;
        }

        private ESC_ConfiguracionDto ConstruirObjeto(SqlDataReader dr) {
            return new ESC_ConfiguracionDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Sala = new ESC_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                },
                TipoValidacionEnvioRespuesta = (ESC_TipoValidacionEnvioRespuesta)ManejoNulos.ManageNullInteger(dr["TipoValidacionEnvioRespuesta"]),
                TiempoEsperaRespuesta = ManejoNulos.ManageNullInteger(dr["TiempoEsperaRespuesta"]),
                MensajeTiempoEsperaRespuesta = ManejoNulos.ManageNullStr(dr["MensajeTiempoEsperaRespuesta"]),
                EnvioMaximoDiario = ManejoNulos.ManageNullInteger(dr["EnvioMaximoDiario"]),
                MensajeEnvioMaximoDiario = ManejoNulos.ManageNullStr(dr["MensajeEnvioMaximoDiario"]),
                EnvioMaximoMensual = ManejoNulos.ManageNullInteger(dr["EnvioMaximoMensual"]),
                MensajeEnvioMaximoMensual = ManejoNulos.ManageNullStr(dr["MensajeEnvioMaximoMensual"]),
                RespuestasAnonimas = ManejoNulos.ManegeNullBool(dr["RespuestasAnonimas"]),
                EncuestaActiva = ManejoNulos.ManegeNullBool(dr["EncuestaActiva"])
            };
        }
    }
}
