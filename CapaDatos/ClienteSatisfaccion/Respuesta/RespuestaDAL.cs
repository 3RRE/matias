using CapaEntidad.ClienteSatisfaccion.DTO;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.Respuesta {
    public class RespuestaDAL {
        string _conexion = string.Empty;

        public RespuestaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int GuardarRespuestaEncuesta(RespuestaEncuestaEntidad respuestaEncuesta) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO RespuestaEncuesta (IdSala, IdTablet, NroDocumento, TipoDocumento, FechaRespuesta, IdTipoEncuesta,nombre, correo,celular)
                                OUTPUT Inserted.IdRespuestaEncuesta
                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5,@p6,@p7, @p8);";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(respuestaEncuesta.IdSala));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(respuestaEncuesta.IdTablet));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(respuestaEncuesta.NroDocumento));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(respuestaEncuesta.TipoDocumento));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(respuestaEncuesta.FechaRespuesta));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(respuestaEncuesta.IdTipoEncuesta));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(respuestaEncuesta.Nombre));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(respuestaEncuesta.Correo));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(respuestaEncuesta.Celular));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public int GuardarRespuestaPregunta(RespuestaPreguntaEntidad respuestaPregunta) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"insert into RespuestaPregunta (idRespuestaEncuesta,idPregunta,idOpcion,Comentario)
                                Output Inserted.IdRespuestaPregunta
                                VALUES(@p0, @p1, @p2, @p3)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(respuestaPregunta.IdRespuestaEncuesta));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(respuestaPregunta.IdPregunta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(respuestaPregunta.IdOpcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(respuestaPregunta.Comentario));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public NpsResultadoDTO ObtenerNPSIndicador(DateTime fechaInicio, DateTime fechaFin) {
            NpsResultadoDTO resultado = new NpsResultadoDTO();

            string consulta = @"
                        SELECT 
                            COUNT(*) AS TotalRespuestas,
                            SUM(CASE WHEN o.Valor IN (1,2) THEN 1 ELSE 0 END) AS CantDetractores,
                            SUM(CASE WHEN o.Valor IN (3,4) THEN 1 ELSE 0 END) AS CantPasivos,
                            SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) AS CantPromotores,

                            ROUND(CAST(SUM(CASE WHEN o.Valor IN (1,2) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS PctDetractores,
                            ROUND(CAST(SUM(CASE WHEN o.Valor IN (3,4) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS PctPasivos,
                            ROUND(CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS PctPromotores,

                            CAST(ROUND(
                                (CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
                              - (CAST(SUM(CASE WHEN o.Valor IN (1,2) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT)),0
                            ) AS INT) AS NPS
                        FROM RespuestaPregunta rp
                        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
                        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
                        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
                        WHERE p.Indicador = 'NPS'
                          AND  CONVERT(date, re.FechaRespuesta) between @p1 and @p2";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaInicio.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.Read()) {
                            // Cantidades
                            resultado.TotalRespuestas = dr["TotalRespuestas"] != DBNull.Value ? Convert.ToInt32(dr["TotalRespuestas"]) : 0;
                            resultado.CantDetractores = dr["CantDetractores"] != DBNull.Value ? Convert.ToInt32(dr["CantDetractores"]) : 0;
                            resultado.CantPasivos = dr["CantPasivos"] != DBNull.Value ? Convert.ToInt32(dr["CantPasivos"]) : 0;
                            resultado.CantPromotores = dr["CantPromotores"] != DBNull.Value ? Convert.ToInt32(dr["CantPromotores"]) : 0;

                            // Porcentajes
                            resultado.PctDetractores = dr["PctDetractores"] != DBNull.Value ? Convert.ToDouble(dr["PctDetractores"]) : 0.0;
                            resultado.PctPasivos = dr["PctPasivos"] != DBNull.Value ? Convert.ToDouble(dr["PctPasivos"]) : 0.0;
                            resultado.PctPromotores = dr["PctPromotores"] != DBNull.Value ? Convert.ToDouble(dr["PctPromotores"]) : 0.0;

                            // Indicador
                            resultado.NPS = dr["NPS"] != DBNull.Value ? Convert.ToInt32(dr["NPS"]) : 0;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerNPSIndicador: " + ex.Message);
                resultado = new NpsResultadoDTO(); // retorno en ceros
            }

            return resultado;
        }


        public List<NpsMensualDTO> ObtenerNPSMensual(DateTime fechaInicio, DateTime fechaFin) {
            List<NpsMensualDTO> lista = new List<NpsMensualDTO>();

            string consulta = @"
                SELECT
                    DATEPART(YEAR, re.FechaRespuesta) AS Anio,
                    DATEPART(MONTH, re.FechaRespuesta) AS Mes,
                    FORMAT(re.FechaRespuesta, 'yyyy-MM') AS PeriodoMes,
                    COUNT(*) AS TotalRespuestas,
                    CAST(ROUND(
                        (CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
                      - (CAST(SUM(CASE WHEN o.Valor IN (1,2) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
                    ,0) AS INT) AS NPS
                FROM RespuestaPregunta rp
                JOIN Opcion o             ON rp.IdOpcion = o.IdOpcion
                JOIN Pregunta p           ON rp.IdPregunta = p.IdPregunta
                JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
                WHERE p.Indicador = 'NPS'
                  AND re.FechaRespuesta BETWEEN @FechaInicio AND @FechaFin
                GROUP BY DATEPART(YEAR, re.FechaRespuesta), DATEPART(MONTH, re.FechaRespuesta), FORMAT(re.FechaRespuesta, 'yyyy-MM')
                ORDER BY Anio, Mes;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var dto = new NpsMensualDTO {
                                    Anio = ManejoNulos.ManageNullInteger(dr["Anio"]),
                                    Mes = ManejoNulos.ManageNullInteger(dr["Mes"]),
                                    PeriodoMes = ManejoNulos.ManageNullStr(dr["PeriodoMes"]),
                                    TotalRespuestas = ManejoNulos.ManageNullInteger(dr["TotalRespuestas"]),
                                    NPS = ManejoNulos.ManageNullInteger(dr["NPS"])
                                };

                                lista.Add(dto);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }


        public CsatResultadoDTO ObtenerCSATIndicador(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            var resultado = new CsatResultadoDTO();

            string consulta = @"
        SELECT 
            COUNT(*) AS TotalRespuestas,
            SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) AS CantMuyInsatisfecho,
            SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) AS CantInsatisfecho,
            SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) AS CantNeutral,
            SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) AS CantSatisfecho,
            SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) AS CantMuySatisfecho,

            ROUND(CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS MuyInsatisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Insatisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Neutral,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Satisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS MuySatisfecho,

            ROUND(CAST(SUM(CASE WHEN o.Valor IN (4,5) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS CSAT
        FROM RespuestaPregunta rp
        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
        WHERE p.Indicador = 'CSAT' AND re.idSala = @p3
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@p2", fechaFin.Date);
                        cmd.Parameters.AddWithValue("@p3", salaId);

                        using(var reader = cmd.ExecuteReader()) {
                            if(reader.Read()) {
                                // Cantidades
                                resultado.TotalRespuestas = reader["TotalRespuestas"] != DBNull.Value ? Convert.ToInt32(reader["TotalRespuestas"]) : 0;
                                resultado.CantMuyInsatisfecho = reader["CantMuyInsatisfecho"] != DBNull.Value ? Convert.ToInt32(reader["CantMuyInsatisfecho"]) : 0;
                                resultado.CantInsatisfecho = reader["CantInsatisfecho"] != DBNull.Value ? Convert.ToInt32(reader["CantInsatisfecho"]) : 0;
                                resultado.CantNeutral = reader["CantNeutral"] != DBNull.Value ? Convert.ToInt32(reader["CantNeutral"]) : 0;
                                resultado.CantSatisfecho = reader["CantSatisfecho"] != DBNull.Value ? Convert.ToInt32(reader["CantSatisfecho"]) : 0;
                                resultado.CantMuySatisfecho = reader["CantMuySatisfecho"] != DBNull.Value ? Convert.ToInt32(reader["CantMuySatisfecho"]) : 0;

                                // Porcentajes
                                resultado.MuyInsatisfecho = reader["MuyInsatisfecho"] != DBNull.Value ? Convert.ToDouble(reader["MuyInsatisfecho"]) : 0;
                                resultado.Insatisfecho = reader["Insatisfecho"] != DBNull.Value ? Convert.ToDouble(reader["Insatisfecho"]) : 0;
                                resultado.Neutral = reader["Neutral"] != DBNull.Value ? Convert.ToDouble(reader["Neutral"]) : 0;
                                resultado.Satisfecho = reader["Satisfecho"] != DBNull.Value ? Convert.ToDouble(reader["Satisfecho"]) : 0;
                                resultado.MuySatisfecho = reader["MuySatisfecho"] != DBNull.Value ? Convert.ToDouble(reader["MuySatisfecho"]) : 0;

                                // Indicador principal
                                resultado.CSAT = reader["CSAT"] != DBNull.Value ? Convert.ToDouble(reader["CSAT"]) : 0;
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerCSATIndicador: " + ex.Message);
                resultado = new CsatResultadoDTO(); // todo en 0
            }

            return resultado;
        }

        public List<CsatMensualDTO> ObtenerCSATMensual(DateTime fechaInicio, DateTime fechaFin) {
            List<CsatMensualDTO> resultado = new List<CsatMensualDTO>();

            string consulta = @"
        SELECT
            DATEPART(YEAR, re.FechaRespuesta) AS Anio,
            DATEPART(MONTH, re.FechaRespuesta) AS Mes,
            FORMAT(re.FechaRespuesta, 'yyyy-MM') AS PeriodoMes,
            COUNT(*) AS TotalRespuestas,

            -- Cantidades
            SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) AS CantMuyInsatisfecho,
            SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) AS CantInsatisfecho,
            SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) AS CantNeutral,
            SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) AS CantSatisfecho,
            SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) AS CantMuySatisfecho,

            -- Porcentajes
            ROUND(CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS MuyInsatisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Insatisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Neutral,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Satisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS MuySatisfecho,

            -- CSAT
            ROUND(CAST(SUM(CASE WHEN o.Valor IN (4,5) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS CSAT
        FROM RespuestaPregunta rp
        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
        WHERE p.Indicador = 'CSAT'
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2
        GROUP BY DATEPART(YEAR, re.FechaRespuesta), DATEPART(MONTH, re.FechaRespuesta), FORMAT(re.FechaRespuesta, 'yyyy-MM')
        ORDER BY Anio, Mes;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaInicio.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            resultado.Add(new CsatMensualDTO {
                                Anio = Convert.ToInt32(dr["Anio"]),
                                Mes = Convert.ToInt32(dr["Mes"]),
                                PeriodoMes = dr["PeriodoMes"].ToString(),
                                TotalRespuestas = dr["TotalRespuestas"] != DBNull.Value ? Convert.ToInt32(dr["TotalRespuestas"]) : 0,

                                CantMuyInsatisfecho = dr["CantMuyInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuyInsatisfecho"]) : 0,
                                CantInsatisfecho = dr["CantInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantInsatisfecho"]) : 0,
                                CantNeutral = dr["CantNeutral"] != DBNull.Value ? Convert.ToInt32(dr["CantNeutral"]) : 0,
                                CantSatisfecho = dr["CantSatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantSatisfecho"]) : 0,
                                CantMuySatisfecho = dr["CantMuySatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuySatisfecho"]) : 0,

                                MuyInsatisfecho = dr["MuyInsatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["MuyInsatisfecho"]) : 0,
                                Insatisfecho = dr["Insatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["Insatisfecho"]) : 0,
                                Neutral = dr["Neutral"] != DBNull.Value ? Convert.ToDouble(dr["Neutral"]) : 0,
                                Satisfecho = dr["Satisfecho"] != DBNull.Value ? Convert.ToDouble(dr["Satisfecho"]) : 0,
                                MuySatisfecho = dr["MuySatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["MuySatisfecho"]) : 0,
                                CSAT = dr["CSAT"] != DBNull.Value ? Convert.ToDouble(dr["CSAT"]) : 0,
                            });
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerCSATMensual: " + ex.Message);
                resultado = new List<CsatMensualDTO>(); // retorno vacío
            }

            return resultado;
        }
        //LISTA DEL INDICADOR DESEADO DIARIO
        public List<IndicadorRespuesta> ObtenerListaIndicadorRespuestas(string indicador, DateTime fechaInicio, DateTime fechaFin, int salaId) {
            var resultado = new List<IndicadorRespuesta>();

            string consulta = @"
        SELECT 
            re.IdSala,
            re.FechaRespuesta,
            re.IdTablet,
            t.Nombre AS NombreTablet,
            o.Valor,
            p.Indicador
        FROM RespuestaPregunta rp
        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
        LEFT JOIN Tablet t ON re.IdTablet = t.IdTablet
        WHERE p.Indicador = @indicador
          AND re.IdSala = @p3
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2;
    ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@indicador", indicador);
                        cmd.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@p2", fechaFin.Date);
                        cmd.Parameters.AddWithValue("@p3", salaId);

                        using(var reader = cmd.ExecuteReader()) {
                            while(reader.Read()) {
                                var item = new IndicadorRespuesta {
                                    IdSala = reader.GetInt32(reader.GetOrdinal("IdSala")),
                                    FechaRespuesta = reader.GetDateTime(reader.GetOrdinal("FechaRespuesta")),
                                    IdTablet = reader.GetInt32(reader.GetOrdinal("IdTablet")),
                                    NombreTablet = reader["NombreTablet"] != DBNull.Value ? reader["NombreTablet"].ToString() : string.Empty,
                                    Valor = Convert.ToInt32(reader["Valor"]),
                                    Indicador = reader["Indicador"].ToString()
                                };
                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerListaIndicadorRespuestas: " + ex.Message);
                resultado = new List<IndicadorRespuesta>(); // devuelve lista vacía si hay error
            }

            return resultado;
        }
        public IndicadorResultadoDTO ObtenerIndicador(string indicador, DateTime fechaInicio, DateTime fechaFin) {
            var resultado = new IndicadorResultadoDTO();

            string consulta = @"
SELECT 
    p.Indicador,
    COUNT(*) AS TotalRespuestas,

    SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) AS CantMuyInsatisfecho,
    SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) AS CantInsatisfecho,
    SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) AS CantNeutral,
    SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) AS CantSatisfecho,
    SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) AS CantMuySatisfecho,

    ROUND(CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)), 2) AS PctMuyInsatisfecho,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)), 2) AS PctInsatisfecho,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)), 2) AS PctNeutral,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)), 2) AS PctSatisfecho,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)), 2) AS PctMuySatisfecho,

    CAST(ROUND(
        (CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
      - (CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
    , 0) AS INT) AS IndicadorValor

FROM RespuestaPregunta rp
INNER JOIN Opcion o              ON rp.IdOpcion = o.IdOpcion
INNER JOIN Pregunta p            ON rp.IdPregunta = p.IdPregunta
INNER JOIN RespuestaEncuesta re  ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
WHERE p.Indicador = @indicador
  AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2
GROUP BY p.Indicador;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@indicador", indicador);
                        cmd.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@p2", fechaFin.Date);

                        using(var dr = cmd.ExecuteReader()) {
                            if(dr.Read()) {
                                resultado.Indicador = dr["Indicador"]?.ToString();
                                resultado.TotalRespuestas = dr["TotalRespuestas"] != DBNull.Value ? Convert.ToInt32(dr["TotalRespuestas"]) : 0;

                                resultado.CantMuyInsatisfecho = dr["CantMuyInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuyInsatisfecho"]) : 0;
                                resultado.CantInsatisfecho = dr["CantInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantInsatisfecho"]) : 0;
                                resultado.CantNeutral = dr["CantNeutral"] != DBNull.Value ? Convert.ToInt32(dr["CantNeutral"]) : 0;
                                resultado.CantSatisfecho = dr["CantSatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantSatisfecho"]) : 0;
                                resultado.CantMuySatisfecho = dr["CantMuySatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuySatisfecho"]) : 0;

                                resultado.PctMuyInsatisfecho = dr["PctMuyInsatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctMuyInsatisfecho"]) : 0.0;
                                resultado.PctInsatisfecho = dr["PctInsatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctInsatisfecho"]) : 0.0;
                                resultado.PctNeutral = dr["PctNeutral"] != DBNull.Value ? Convert.ToDouble(dr["PctNeutral"]) : 0.0;
                                resultado.PctSatisfecho = dr["PctSatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctSatisfecho"]) : 0.0;
                                resultado.PctMuySatisfecho = dr["PctMuySatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctMuySatisfecho"]) : 0.0;

                                resultado.IndicadorValor = dr["IndicadorValor"] != DBNull.Value ? Convert.ToInt32(dr["IndicadorValor"]) : 0;
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerIndicador: " + ex.Message);
                resultado = new IndicadorResultadoDTO(); // retorno en ceros
            }

            return resultado;
        }



        public List<IndicadorDiarioDTO> ObtenerIndicadorDiario(DateTime fechaInicio, DateTime fechaFin, string indicador) {
            var resultado = new List<IndicadorDiarioDTO>();

            string consulta = @"
SELECT
    CONVERT(date, re.FechaRespuesta) AS Fecha,
    COUNT(*) AS TotalRespuestas,

    -- Cantidades por nivel 1..5
    SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) AS CantMuyInsatisfecho,
    SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) AS CantInsatisfecho,
    SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) AS CantNeutral,
    SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) AS CantSatisfecho,
    SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) AS CantMuySatisfecho,

    -- Porcentajes por nivel (2 decimales)
    ROUND(CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)),2) AS PctMuyInsatisfecho,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)),2) AS PctInsatisfecho,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)),2) AS PctNeutral,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)),2) AS PctSatisfecho,
    ROUND(CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(6,2)),2) AS PctMuySatisfecho,

    -- Indicador diario: %MuySatisfecho - %MuyInsatisfecho
    CAST(ROUND(
        (CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
      - (CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS FLOAT))
    ,0) AS INT) AS IndicadorValor
FROM RespuestaPregunta rp
INNER JOIN Opcion o              ON rp.IdOpcion = o.IdOpcion
INNER JOIN Pregunta p            ON rp.IdPregunta = p.IdPregunta
INNER JOIN RespuestaEncuesta re  ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
WHERE p.Indicador = @Indicador
  AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2
GROUP BY CONVERT(date, re.FechaRespuesta)
ORDER BY Fecha;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        query.Parameters.AddWithValue("@p2", fechaFin.Date);
                        query.Parameters.AddWithValue("@Indicador", indicador);

                        using(var dr = query.ExecuteReader()) {
                            while(dr.Read()) {
                                var item = new IndicadorDiarioDTO {
                                    Fecha = dr["Fecha"] != DBNull.Value ? Convert.ToDateTime(dr["Fecha"]) : DateTime.MinValue,
                                    TotalRespuestas = dr["TotalRespuestas"] != DBNull.Value ? Convert.ToInt32(dr["TotalRespuestas"]) : 0,

                                    CantMuyInsatisfecho = dr["CantMuyInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuyInsatisfecho"]) : 0,
                                    CantInsatisfecho = dr["CantInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantInsatisfecho"]) : 0,
                                    CantNeutral = dr["CantNeutral"] != DBNull.Value ? Convert.ToInt32(dr["CantNeutral"]) : 0,
                                    CantSatisfecho = dr["CantSatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantSatisfecho"]) : 0,
                                    CantMuySatisfecho = dr["CantMuySatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuySatisfecho"]) : 0,

                                    PctMuyInsatisfecho = dr["PctMuyInsatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctMuyInsatisfecho"]) : 0.0,
                                    PctInsatisfecho = dr["PctInsatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctInsatisfecho"]) : 0.0,
                                    PctNeutral = dr["PctNeutral"] != DBNull.Value ? Convert.ToDouble(dr["PctNeutral"]) : 0.0,
                                    PctSatisfecho = dr["PctSatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctSatisfecho"]) : 0.0,
                                    PctMuySatisfecho = dr["PctMuySatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["PctMuySatisfecho"]) : 0.0,

                                    IndicadorValor = dr["IndicadorValor"] != DBNull.Value ? Convert.ToInt32(dr["IndicadorValor"]) : 0
                                };

                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerIndicadorDiario: " + ex.Message);
                resultado = new List<IndicadorDiarioDTO>(); // retorno vacío
            }

            return resultado;
        }




        public List<CsatRespuesta> ObtenerListaCSATIRespuestas(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            var resultado = new List<CsatRespuesta>();

            string consulta = @"
                        SELECT 
                            re.IdSala,
                            re.fechaRespuesta,
                            re.IdTablet,
                            t.Nombre AS NombreTablet,
                            o.Valor
                        FROM RespuestaPregunta rp
                        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
                        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
                        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
                        LEFT JOIN Tablet t ON re.IdTablet = t.IdTablet
                        WHERE p.Indicador = 'CSAT' 
                          AND re.IdSala = @p3
                          AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2;
                    ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@p2", fechaFin.Date);
                        cmd.Parameters.AddWithValue("@p3", salaId);

                        using(var reader = cmd.ExecuteReader()) {
                            while(reader.Read()) {
                                var csat = new CsatRespuesta {
                                    IdSala = reader.GetInt32(reader.GetOrdinal("IdSala")),
                                    FechaRespuesta = reader.GetDateTime(reader.GetOrdinal("FechaRespuesta")),
                                    IdTablet = reader.GetInt32(reader.GetOrdinal("IdTablet")),
                                    NombreTablet = reader["NombreTablet"] != DBNull.Value ? reader["NombreTablet"].ToString() : string.Empty,
                                    Valor = Convert.ToInt32(reader["Valor"])
                                };
                                resultado.Add(csat);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerCSATIndicador: " + ex.Message);
                resultado = new List<CsatRespuesta>(); // devuelve lista vacía si hay error
            }

            return resultado;
        }


        public List<CsatDiarioDTO> ObtenerCSATDiario(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            var resultado = new List<CsatDiarioDTO>();

            string consulta = @"
        SELECT
            CONVERT(date, re.FechaRespuesta) AS Fecha,
            COUNT(*) AS TotalRespuestas,

            -- Cantidades
            SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) AS CantMuyInsatisfecho,
            SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) AS CantInsatisfecho,
            SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) AS CantNeutral,
            SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) AS CantSatisfecho,
            SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) AS CantMuySatisfecho,

            -- Porcentajes
            ROUND(CAST(SUM(CASE WHEN o.Valor = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS MuyInsatisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 2 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Insatisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 3 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Neutral,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 4 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS Satisfecho,
            ROUND(CAST(SUM(CASE WHEN o.Valor = 5 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS MuySatisfecho,

            -- CSAT
            ROUND(CAST(SUM(CASE WHEN o.Valor IN (4,5) THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)),2) AS CSAT
        FROM RespuestaPregunta rp
        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
        WHERE p.Indicador = 'CSAT' and re.idSala=@p3
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2
        GROUP BY CONVERT(date, re.FechaRespuesta)
        ORDER BY Fecha;
    ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        query.Parameters.AddWithValue("@p2", fechaFin.Date);
                        query.Parameters.AddWithValue("@p3", salaId);

                        using(var dr = query.ExecuteReader()) {
                            while(dr.Read()) {
                                resultado.Add(new CsatDiarioDTO {
                                    Fecha = dr["Fecha"] != DBNull.Value ? Convert.ToDateTime(dr["Fecha"]) : DateTime.MinValue,
                                    TotalRespuestas = dr["TotalRespuestas"] != DBNull.Value ? Convert.ToInt32(dr["TotalRespuestas"]) : 0,

                                    CantMuyInsatisfecho = dr["CantMuyInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuyInsatisfecho"]) : 0,
                                    CantInsatisfecho = dr["CantInsatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantInsatisfecho"]) : 0,
                                    CantNeutral = dr["CantNeutral"] != DBNull.Value ? Convert.ToInt32(dr["CantNeutral"]) : 0,
                                    CantSatisfecho = dr["CantSatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantSatisfecho"]) : 0,
                                    CantMuySatisfecho = dr["CantMuySatisfecho"] != DBNull.Value ? Convert.ToInt32(dr["CantMuySatisfecho"]) : 0,

                                    MuyInsatisfecho = dr["MuyInsatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["MuyInsatisfecho"]) : 0,
                                    Insatisfecho = dr["Insatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["Insatisfecho"]) : 0,
                                    Neutral = dr["Neutral"] != DBNull.Value ? Convert.ToDouble(dr["Neutral"]) : 0,
                                    Satisfecho = dr["Satisfecho"] != DBNull.Value ? Convert.ToDouble(dr["Satisfecho"]) : 0,
                                    MuySatisfecho = dr["MuySatisfecho"] != DBNull.Value ? Convert.ToDouble(dr["MuySatisfecho"]) : 0,
                                    CSAT = dr["CSAT"] != DBNull.Value ? Convert.ToDouble(dr["CSAT"]) : 0,
                                });
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerCSATDiario: " + ex.Message);
                resultado = new List<CsatDiarioDTO>();
            }

            return resultado;
        }

        public List<NpsRespuestaDTO> ObtenerNPSRespuestas( DateTime fechaInicio, DateTime fechaFin, int salaId) {
            var respuestas = new List<NpsRespuestaDTO>();

            string consulta = @"
        SELECT 
            re.IdSala,
            re.FechaRespuesta,
            re.IdTablet,
            t.Nombre AS NombreTablet,
            o.Valor
        FROM RespuestaPregunta rp
        INNER JOIN Opcion o ON rp.IdOpcion = o.IdOpcion
        INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
        INNER JOIN RespuestaEncuesta re ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
        LEFT JOIN Tablet t ON re.IdTablet = t.IdTablet
        WHERE p.Indicador = 'NPS'
          AND re.IdSala = @p0
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", salaId);
                    query.Parameters.AddWithValue("@p1", fechaInicio.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            respuestas.Add(new NpsRespuestaDTO {
                                IdSala = dr["IdSala"] != DBNull.Value ? Convert.ToInt32(dr["IdSala"]) : 0,
                                FechaRespuesta = dr["FechaRespuesta"] != DBNull.Value ? Convert.ToDateTime(dr["FechaRespuesta"]) : DateTime.MinValue,
                                IdTablet = dr["IdTablet"] != DBNull.Value ? Convert.ToInt32(dr["IdTablet"]) : 0,
                                NombreTablet = dr["NombreTablet"] != DBNull.Value ? dr["NombreTablet"].ToString() : string.Empty,
                                Valor = dr["Valor"] != DBNull.Value ? Convert.ToInt32(dr["Valor"]) : 0
                            });
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerNPSRespuestas: " + ex.Message);
                respuestas = new List<NpsRespuestaDTO>(); // retorno vacío en caso de error
            }

            return respuestas;
        }


        public List<RespuestaIndicadorDTO> ObtenerRespuestasIndicador( DateTime fechaInicio, DateTime fechaFin,string indicador,int salaId) {
            var resultado = new List<RespuestaIndicadorDTO>();

            string consulta = @"
                    SELECT 
                        re.IdSala,
                        re.IdTablet,
                        t.Nombre AS NombreTablet,
                        CONVERT(date, re.FechaRespuesta) AS Fecha,
                        o.Valor
                    FROM RespuestaPregunta rp
                    INNER JOIN Opcion o              ON rp.IdOpcion = o.IdOpcion
                    INNER JOIN Pregunta p            ON rp.IdPregunta = p.IdPregunta
                    INNER JOIN RespuestaEncuesta re  ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
                    LEFT JOIN Tablet t               ON re.IdTablet = t.IdTablet
                    WHERE p.Indicador = @indicador and re.idSala=@p3
                      AND CONVERT(date, re.FechaRespuesta) BETWEEN @p1 AND @p2
                    ORDER BY re.FechaRespuesta;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@indicador", indicador);
                        cmd.Parameters.AddWithValue("@p1", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@p2", fechaFin.Date);
                        cmd.Parameters.AddWithValue("@p3", salaId);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var item = new RespuestaIndicadorDTO {
                                    IdSala = dr["IdSala"] != DBNull.Value ? Convert.ToInt32(dr["IdSala"]) : 0,
                                    IdTablet = dr["IdTablet"] != DBNull.Value ? Convert.ToInt32(dr["IdTablet"]) : 0,
                                    NombreTablet = dr["NombreTablet"]?.ToString(),
                                    Fecha = dr["Fecha"] != DBNull.Value ? Convert.ToDateTime(dr["Fecha"]) : DateTime.MinValue,
                                    Valor = dr["Valor"] != DBNull.Value ? Convert.ToInt32(dr["Valor"]) : 0
                                };

                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerRespuestasIndicador: " + ex.Message);
                resultado = new List<RespuestaIndicadorDTO>(); // retorno vacío
            }

            return resultado;
        }


        public List<RespuestaPlanoDTO> ObtenerRespuestasEncuestas(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            var resultado = new List<RespuestaPlanoDTO>();

            string consulta = @"
        SELECT
            re.IdRespuestaEncuesta,
            re.NroDocumento,
            re.Nombre,
            re.Celular,
            re.Correo,
            re.FechaRespuesta,
            re.IdTablet,
            t.Nombre AS NombreTablet,
            p.IdPregunta,
            p.Texto AS Pregunta,
            p.Indicador,
            p.Multi,
            o.Texto AS TextoOpcion,
            o.Valor AS ValorOpcion,
            o.TieneComentario,
            rp.Comentario
        FROM RespuestaEncuesta re
        LEFT JOIN RespuestaPregunta rp ON re.IdRespuestaEncuesta = rp.IdRespuestaEncuesta
        LEFT JOIN Pregunta p          ON rp.IdPregunta = p.IdPregunta
        LEFT JOIN Opcion o            ON rp.IdOpcion = o.IdOpcion
        LEFT JOIN Tablet t            ON re.IdTablet = t.IdTablet
        WHERE re.IdSala = @pSala
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @pInicio AND @pFin
        ORDER BY re.IdRespuestaEncuesta, p.Orden;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@pSala", salaId);
                        cmd.Parameters.AddWithValue("@pInicio", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@pFin", fechaFin.Date);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var item = new RespuestaPlanoDTO {
                                    IdRespuestaEncuesta = dr["IdRespuestaEncuesta"] != DBNull.Value ? Convert.ToInt32(dr["IdRespuestaEncuesta"]) : 0,
                                    NroDocumento = dr["NroDocumento"]?.ToString(),
                                    Nombre = dr["Nombre"]?.ToString(),
                                    Celular = dr["Celular"]?.ToString(),
                                    Correo = dr["Correo"]?.ToString(),
                                    FechaRespuesta = dr["FechaRespuesta"] != DBNull.Value ? Convert.ToDateTime(dr["FechaRespuesta"]) : DateTime.MinValue,

                                    IdTablet = dr["IdTablet"] != DBNull.Value ? Convert.ToInt32(dr["IdTablet"]) : 0,
                                    NombreTablet = dr["NombreTablet"]?.ToString(),

                                    IdPregunta = dr["IdPregunta"] != DBNull.Value ? Convert.ToInt32(dr["IdPregunta"]) : 0,
                                    Pregunta = dr["Pregunta"]?.ToString(),
                                    Indicador = dr["Indicador"]?.ToString(),
                                    Multi = dr["Multi"] != DBNull.Value ? Convert.ToBoolean(dr["Multi"]) : false,

                                    TextoOpcion = dr["TextoOpcion"]?.ToString(),
                                    ValorOpcion = dr["ValorOpcion"] != DBNull.Value ? Convert.ToInt32(dr["ValorOpcion"]) : (int?)null,
                                    TieneComentario = dr["TieneComentario"] != DBNull.Value ? Convert.ToBoolean(dr["TieneComentario"]) : false,
                                    Comentario = dr["Comentario"]?.ToString()
                                };

                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerRespuestasEncuestas: " + ex.Message);
                resultado = new List<RespuestaPlanoDTO>(); // retorno vacío
            }

            return resultado;
        }



        public List<PreguntaIndicadorDTO> ObtenerPreguntasIndicadores() {
            var resultado = new List<PreguntaIndicadorDTO>();

            string consulta = @"
                    SELECT 
                        p.IdPregunta,
                        p.Texto AS Pregunta,
                        p.Indicador,
                        p.Orden,
                        p.multi
                    FROM Pregunta p
                    WHERE p.Indicador IS NULL OR p.Indicador <> 'NPS';
                    ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var item = new PreguntaIndicadorDTO {
                                    IdPregunta = dr["IdPregunta"] != DBNull.Value ? Convert.ToInt32(dr["IdPregunta"]) : 0,
                                    Pregunta = dr["Pregunta"]?.ToString(),
                                    Indicador = dr["Indicador"]?.ToString(),
                                    Orden = dr["Orden"] != DBNull.Value ? Convert.ToInt32(dr["Orden"]) : 0,
                                    Multi = dr["Multi"] != DBNull.Value && Convert.ToBoolean(dr["Multi"]),
                                };

                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerPreguntasIndicadores: " + ex.Message);
                resultado = new List<PreguntaIndicadorDTO>(); // retorno vacío
            }

            return resultado;
        }

        public List<EncuestadoDTO> ObtenerEncuestados(int salaId, DateTime fechaInicio, DateTime fechaFin) {
            var resultado = new List<EncuestadoDTO>();

            string consulta = @"
        SELECT 
            re.IdRespuestaEncuesta,
            re.IdSala,
            re.IdTablet,
            re.NroDocumento,
            re.TipoDocumento,
            re.FechaRespuesta,
            re.Nombre,
            re.Correo,
            re.Celular,
            p.Texto AS PreguntaNPS,
            o.Valor AS ValorRespuesta,
            CASE 
                WHEN o.Valor IN (1,2) THEN 'Detractor'
                WHEN o.Valor = 3 THEN 'Neutral'
                WHEN o.Valor IN (4,5) THEN 'Promotor'
                ELSE 'No Clasificado'
            END AS Clasificacion
        FROM RespuestaEncuesta re
        INNER JOIN RespuestaPregunta rp 
            ON re.IdRespuestaEncuesta = rp.IdRespuestaEncuesta
        INNER JOIN Pregunta p 
            ON rp.IdPregunta = p.IdPregunta
        INNER JOIN Opcion o 
            ON rp.IdOpcion = o.IdOpcion
        WHERE p.Indicador = 'NPS'
          AND re.IdSala = @salaId
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @fechaInicio AND @fechaFin;
    ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@salaId", salaId);
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var item = new EncuestadoDTO {
                                    IdRespuestaEncuesta = Convert.ToInt32(dr["IdRespuestaEncuesta"]),
                                    IdSala = Convert.ToInt32(dr["IdSala"]),
                                    IdTablet = Convert.ToInt32(dr["IdTablet"]),
                                    NroDocumento = dr["NroDocumento"]?.ToString(),
                                    TipoDocumento = dr["TipoDocumento"]?.ToString(),
                                    FechaRespuesta = Convert.ToDateTime(dr["FechaRespuesta"]),
                                    Nombre = dr["Nombre"]?.ToString(),
                                    Correo = dr["Correo"]?.ToString(),
                                    Celular = dr["Celular"]?.ToString(),
                                    PreguntaNPS = dr["PreguntaNPS"]?.ToString(),
                                    ValorRespuesta = dr["ValorRespuesta"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["ValorRespuesta"]),
                                    Clasificacion = dr["Clasificacion"]?.ToString()
                                };

                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerEncuestadosNps: " + ex.Message);
                resultado = new List<EncuestadoDTO>();
            }

            return resultado;
        }
        public List<RespuestaAtributoDTO> ObtenerRespuestasAtributos(int salaId, DateTime fechaInicio, DateTime fechaFin) {
            var resultado = new List<RespuestaAtributoDTO>();

            string consulta = @"
        SELECT 
            re.IdRespuestaEncuesta,
            p.IdPregunta,
            p.Texto AS Pregunta,
            o.Texto AS Opcion,
            rp.Comentario
        FROM RespuestaPregunta rp
        INNER JOIN RespuestaEncuesta re 
            ON rp.IdRespuestaEncuesta = re.IdRespuestaEncuesta
        INNER JOIN Pregunta p 
            ON rp.IdPregunta = p.IdPregunta
        INNER JOIN Opcion o
            ON rp.IdOpcion = o.IdOpcion
        WHERE re.IdSala = @salaId
          AND CONVERT(date, re.FechaRespuesta) BETWEEN @fechaInicio AND @fechaFin
          AND (p.Indicador IS NULL OR LTRIM(RTRIM(p.Indicador)) = '')
    ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@salaId", salaId);
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var item = new RespuestaAtributoDTO {
                                    IdRespuestaEncuesta = Convert.ToInt32(dr["IdRespuestaEncuesta"]),
                                    IdPregunta = Convert.ToInt32(dr["IdPregunta"]),
                                    Pregunta = dr["Pregunta"]?.ToString(),
                                    Opcion = dr["Opcion"]?.ToString(),
                                    Comentario = dr["Comentario"]?.ToString()
                                };

                                // ⚡ Agregamos el IdRespuestaEncuesta para luego mapear
                                item.GetType().GetProperty("IdRespuestaEncuesta")
                                    ?.SetValue(item, Convert.ToInt32(dr["IdRespuestaEncuesta"]));

                                resultado.Add(item);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerRespuestasAtributos: " + ex.Message);
                resultado = new List<RespuestaAtributoDTO>();
            }

            return resultado;
        }


        public int ObtenerCantidadRespuestasAtributos(int salaId, DateTime fechaInicio, DateTime fechaFin, string indicador) {
            int total = 0;

            string consulta = @"
            SELECT COUNT(DISTINCT re.NroDocumento) AS TotalEncuestasUnicas
            FROM RespuestaEncuesta re
            INNER JOIN RespuestaPregunta rp ON re.IdRespuestaEncuesta = rp.IdRespuestaEncuesta
            INNER JOIN Pregunta p ON rp.IdPregunta = p.IdPregunta
            WHERE p.Indicador = @indicador
              AND re.IdSala = @salaId
            AND re.FechaRespuesta >= @fechaInicio
            AND re.FechaRespuesta < DATEADD(DAY, 1, @fechaFin)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@salaId", salaId);
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                        cmd.Parameters.AddWithValue("@indicador", indicador);

                        var result = cmd.ExecuteScalar();
                        if(result != null && result != DBNull.Value) {
                            total = Convert.ToInt32(result);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ObtenerCantidadRespuestasAtributos: " + ex.Message);
                total = 0;
            }

            return total;
        }




    }
}
