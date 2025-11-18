using CapaEntidad.SatisfaccionCliente.DTO;
using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Entity;
using CapaEntidad.SatisfaccionCliente.Enum;
using CapaEntidad.SatisfaccionCliente.Reporte;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.SatisfaccionCliente {
    public class ESC_RespuestaDAL {
        private readonly string _conexion;

        public ESC_RespuestaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int InsertarRespuesta(ESC_Respuesta respuesta) {
            int idInsertado;
            string consulta = @"
                INSERT INTO ESC_Respuesta(IdRespuestaSala, IdPregunta, Puntaje, NumeroDocumento)
                OUTPUT INSERTED.Id
                VALUES (@IdRespuestaSala, @IdPregunta, @Puntaje, @NumeroDocumento)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdRespuestaSala", respuesta.IdRespuestaSala);
                    query.Parameters.AddWithValue("@IdPregunta", respuesta.IdPregunta);
                    query.Parameters.AddWithValue("@Puntaje", respuesta.Puntaje);
                    query.Parameters.AddWithValue("@NumeroDocumento", respuesta.NumeroDocumento);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public ESC_RespuestaDto ObtenerRespuestaPorId(int id) {
            ESC_RespuestaDto item = new ESC_RespuestaDto();
            string consulta = @"
                SELECT
	                res.Id,
	                pre.Id AS IdPregunta,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto AS TextoPregunta,
	                pre.EsObligatoria AS EsObligatoriaPregunta,
	                pre.Estado AS EstadoPregunta,
	                astc.Id AS IdCliente,
	                astc.Nombre AS NombreCliente,
	                astc.ApelPat AS ApellidoPaternoCliente,
	                astc.ApelMat AS ApellidoMaternoCliente,
                    res.NumeroDocumento AS NumeroDocumentoCliente,
	                res.Puntaje,
	                res.FechaRegistro
                FROM
	                ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                LEFT JOIN AST_Cliente as astc ON astc.NroDoc = res.NumeroDocumento
                WHERE
	                res.Id = @Id
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

        public ESC_RespuestaDto ObtenerUltimaRespuestaClienteDeSala(int codSala, string numeroDocumento) {
            ESC_RespuestaDto item = new ESC_RespuestaDto();
            string consulta = @"
                SELECT
	                res.Id,
	                pre.Id AS IdPregunta,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto AS TextoPregunta,
	                pre.EsObligatoria AS EsObligatoriaPregunta,
	                pre.Estado AS EstadoPregunta,
	                astc.Id AS IdCliente,
	                astc.Nombre AS NombreCliente,
	                astc.ApelPat AS ApellidoPaternoCliente,
	                astc.ApelMat AS ApellidoMaternoCliente,
                    res.NumeroDocumento AS NumeroDocumentoCliente,
	                res.Puntaje,
	                res.FechaRegistro
                FROM
	                ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                LEFT JOIN AST_Cliente as astc ON astc.NroDoc = res.NumeroDocumento
                WHERE
	                res.NumeroDocumento = @NumeroDocumento AND pre.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<ESC_RespuestaDto> ObtenerRespuestasDeClientePorMesDeSala(int codSala, string numeroDocumento, int mes) {
            List<ESC_RespuestaDto> item = new List<ESC_RespuestaDto>();
            string consulta = @"
                SELECT
	                res.Id,
	                pre.Id AS IdPregunta,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto AS TextoPregunta,
	                pre.EsObligatoria AS EsObligatoriaPregunta,
	                pre.Estado AS EstadoPregunta,
	                astc.Id AS IdCliente,
	                astc.Nombre AS NombreCliente,
	                astc.ApelPat AS ApellidoPaternoCliente,
	                astc.ApelMat AS ApellidoMaternoCliente,
                    res.NumeroDocumento AS NumeroDocumentoCliente,
	                res.Puntaje,
	                res.FechaRegistro
                FROM
	                ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                LEFT JOIN AST_Cliente as astc ON astc.NroDoc = res.NumeroDocumento
                WHERE
	                res.NumeroDocumento = @NumeroDocumento AND MONTH(res.FechaRegistro) = @Mes AND pre.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    query.Parameters.AddWithValue("@Mes", mes);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return item;
        }

        public int ObtenerCantidadRespuestasDeClientePorMesDeSala(int codSala, string numeroDocumento, int mes) {
            int cantidad = 0;
            string consulta = @"
                SELECT COUNT(res.Id)
                FROM ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                WHERE res.NumeroDocumento = @NumeroDocumento AND MONTH(res.FechaRegistro) = @Mes AND pre.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    query.Parameters.AddWithValue("@Mes", mes);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    cantidad = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }
            return cantidad;
        }

        public int ObtenerCantidadRespuestasDeClientePorFechaDeSala(int codSala, string numeroDocumento, DateTime fecha) {
            int cantidad = 0;
            string consulta = @"
                SELECT COUNT(res.Id)
                FROM ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                WHERE res.NumeroDocumento = @NumeroDocumento AND CONVERT(DATE, res.FechaRegistro) = CONVERT(DATE, @Fecha) AND pre.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    query.Parameters.AddWithValue("@Fecha", fecha.Date);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    cantidad = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }
            return cantidad;
        }

        public List<ESC_RespuestaDto> ObtenerRespuestas() {
            List<ESC_RespuestaDto> items = new List<ESC_RespuestaDto>();
            string consulta = @"
                SELECT
	                res.Id,
	                pre.Id AS IdPregunta,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto AS TextoPregunta,
	                pre.EsObligatoria AS EsObligatoriaPregunta,
	                pre.Estado AS EstadoPregunta,
	                astc.Id AS IdCliente,
	                astc.Nombre AS NombreCliente,
	                astc.ApelPat AS ApellidoPaternoCliente,
	                astc.ApelMat AS ApellidoMaternoCliente,
                    res.NumeroDocumento AS NumeroDocumentoCliente,
	                res.Puntaje,
	                res.FechaRegistro
                FROM
	                ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                LEFT JOIN AST_Cliente as astc ON astc.NroDoc = res.NumeroDocumento
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

        public List<ESC_RespuestaDto> ObtenerRespuestasPorCodSala(int codSala) {
            List<ESC_RespuestaDto> items = new List<ESC_RespuestaDto>();
            string consulta = @"
                SELECT
	                res.Id,
	                pre.Id AS IdPregunta,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto AS TextoPregunta,
	                pre.EsObligatoria AS EsObligatoriaPregunta,
	                pre.Estado AS EstadoPregunta,
	                astc.Id AS IdCliente,
	                astc.Nombre AS NombreCliente,
	                astc.ApelPat AS ApellidoPaternoCliente,
	                astc.ApelMat AS ApellidoMaternoCliente,
                    res.NumeroDocumento AS NumeroDocumentoCliente,
	                res.Puntaje,
	                res.FechaRegistro
                FROM
	                ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                LEFT JOIN AST_Cliente as astc ON astc.NroDoc = res.NumeroDocumento
                WHERE
	                pre.CodSala = @CodSala
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<ESC_RespuestaDto> ObtenerRespuestasPorCodSalaYFechas(int codSala, DateTime fechaInicio, DateTime fechaFin) {
            List<ESC_RespuestaDto> items = new List<ESC_RespuestaDto>();
            string consulta = @"
                SELECT
	                res.Id,
	                pre.Id AS IdPregunta,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto AS TextoPregunta,
	                pre.EsObligatoria AS EsObligatoriaPregunta,
	                pre.Estado AS EstadoPregunta,
	                astc.Id AS IdCliente,
	                astc.Nombre AS NombreCliente,
	                astc.ApelPat AS ApellidoPaternoCliente,
	                astc.ApelMat AS ApellidoMaternoCliente,
                    res.NumeroDocumento AS NumeroDocumentoCliente,
	                res.Puntaje,
	                res.FechaRegistro
                FROM
	                ESC_Respuesta AS res
                LEFT JOIN ESC_Pregunta AS pre ON pre.Id = res.IdPregunta
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                LEFT JOIN AST_Cliente as astc ON astc.NroDoc = res.NumeroDocumento
                WHERE
	                pre.CodSala = @CodSala
                    AND CONVERT(DATE, res.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    query.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                    query.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }


        public List<ESC_ReportePorcentaje> ObtenerReportePorcentajes(int codSala, DateTime fechaInicio, DateTime fechaFin) {
            List<ESC_ReportePorcentaje> items = new List<ESC_ReportePorcentaje>();
            string consulta = @"
                ;WITH Puntajes AS (
                    SELECT 1 AS Puntaje UNION ALL
                    SELECT 2 UNION ALL
                    SELECT 3 UNION ALL
                    SELECT 4 UNION ALL
                    SELECT 5
                ),
                TotalRespuestas AS (
                    SELECT COUNT(*) AS Total
                    FROM ESC_Respuesta R
                    INNER JOIN ESC_Pregunta P ON R.IdPregunta = P.Id
                    WHERE P.CodSala = @CodSala
                    AND CONVERT(DATE, R.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin
                ),
                RespuestasPorPuntaje AS (
                    SELECT 
                        R.Puntaje,
                        COUNT(R.Puntaje) AS CantidadRespuestas
                    FROM ESC_Respuesta R
                    INNER JOIN ESC_Pregunta P ON R.IdPregunta = P.Id
                    WHERE P.CodSala = @CodSala
                    AND CONVERT(DATE, R.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin
                    GROUP BY R.Puntaje
                )
                SELECT 
                    P.Puntaje,
                    COALESCE(T.Total, 0) AS TotalRespuestas,
                    COALESCE(RP.CantidadRespuestas, 0) AS CantidadRespuestas,
                    COALESCE(RP.CantidadRespuestas * 100.0 / NULLIF(T.Total, 0), 0) AS Porcentaje
                FROM Puntajes P
                LEFT JOIN RespuestasPorPuntaje RP ON P.Puntaje = RP.Puntaje
                CROSS JOIN TotalRespuestas T
                ORDER BY P.Puntaje;
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    query.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                    query.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoReportePorcentaje(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        private ESC_RespuestaDto ConstruirObjeto(SqlDataReader dr) {
            return new ESC_RespuestaDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Pregunta = new ESC_PreguntaDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdPregunta"]),
                    Sala = new ESC_SalaDto {
                        CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                        Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                    },
                    Texto = ManejoNulos.ManageNullStr(dr["TextoPregunta"]),
                    EsObligatoria = ManejoNulos.ManegeNullBool(dr["EsObligatoriaPregunta"]),
                    Estado = ManejoNulos.ManegeNullBool(dr["EstadoPregunta"])
                },
                Cliente = new ESC_ClienteDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdCliente"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoCliente"]),
                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoCliente"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoCliente"]),
                },
                Puntaje = (ESC_Puntaje)ManejoNulos.ManageNullInteger(dr["Puntaje"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
            };
        }

        private ESC_ReportePorcentaje ConstruirObjetoReportePorcentaje(SqlDataReader dr) {
            return new ESC_ReportePorcentaje {
                Puntaje = (ESC_Puntaje)ManejoNulos.ManageNullInteger(dr["Puntaje"]),
                TotalRespuestas = ManejoNulos.ManageNullInteger(dr["TotalRespuestas"]),
                CantidadRespuestas = ManejoNulos.ManageNullInteger(dr["CantidadRespuestas"]),
                Porcentaje = ManejoNulos.ManageNullDouble(dr["Porcentaje"]),
            };
        }
    }
}
