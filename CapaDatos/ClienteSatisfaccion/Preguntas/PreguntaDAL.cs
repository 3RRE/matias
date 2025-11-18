using CapaEntidad.ClienteSatisfaccion;
using CapaEntidad.ClienteSatisfaccion.DTO;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using CapaEntidad.Discos;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.Preguntas {
    public class PreguntaDAL {
        string _conexion = string.Empty;

        public PreguntaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<PreguntaEntidad> ListadoPreguntas(int TipoEncuesta) {
            List<PreguntaEntidad> lista = new List<PreguntaEntidad>();
            string consulta = @"SELECT [idPregunta]
                                  ,[idTipoEncuesta]
                                  ,[texto]
                                  ,[Indicador]
                                  ,[orden]
                                  ,[random]
                                  ,[multi]
								  ,[activo]
                                  ,[FechaInicio]
                                  ,[FechaFinal]
                              FROM Pregunta 
                              WHERE idTipoEncuesta=@p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", TipoEncuesta);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new PreguntaEntidad {
                                IdPregunta = ManejoNulos.ManageNullInteger(dr["IdPregunta"]),
                                IdTipoEncuesta = ManejoNulos.ManageNullInteger(dr["IdTipoEncuesta"]),
                                Texto = ManejoNulos.ManageNullStr(dr["Texto"]),
                                Orden = ManejoNulos.ManageNullInteger(dr["Orden"]),
                                Random = ManejoNulos.ManegeNullBool(dr["Random"]),
                                Multi = ManejoNulos.ManegeNullBool(dr["Multi"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Indicador = ManejoNulos.ManageNullStr(dr["Indicador"]),
                                FechaInicio = dr["FechaInicio"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["FechaInicio"],
                                FechaFin = dr["FechaFinal"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["FechaFinal"],
                            };
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        // 🔹 Crear pregunta
        public int CrearPregunta(PreguntaEntidad entidad) {
            int idGenerado = 0;
            string consulta = @"
                INSERT INTO Pregunta (idTipoEncuesta, texto, Indicador, orden, random, multi, activo,FechaInicio,FechaFinal)
                OUTPUT INSERTED.idPregunta
                VALUES (@idTipoEncuesta, @texto, @indicador, @orden, @random, @multi, @activo,@fechaInicio,@fechaFinal)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idTipoEncuesta", entidad.IdTipoEncuesta);
                    query.Parameters.AddWithValue("@texto", entidad.Texto ?? "");
                    query.Parameters.AddWithValue("@indicador", (object)entidad.Indicador ?? DBNull.Value);
                    query.Parameters.AddWithValue("@orden", entidad.Orden);
                    query.Parameters.AddWithValue("@random", entidad.Random);
                    query.Parameters.AddWithValue("@multi", entidad.Multi);
                    query.Parameters.AddWithValue("@activo", entidad.Activo);
                    query.Parameters.AddWithValue("@fechaInicio",entidad.FechaInicio.HasValue ? (object)entidad.FechaInicio.Value : DBNull.Value);
                    query.Parameters.AddWithValue("@fechaFinal",entidad.FechaFin.HasValue ? (object)entidad.FechaFin.Value : DBNull.Value);

                    idGenerado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idGenerado;
        }

        // 🔹 Editar pregunta
        public bool EditarPregunta(PreguntaEntidad entidad) {
            bool actualizado = false;
            string consulta = @"
                UPDATE Pregunta
                SET texto = @texto,
                    Indicador = @indicador,
                    multi = @multi,
                    activo = @activo,
                    fechaInicio=@fechaInicio,
                    fechaFinal = @fechaFin
                WHERE idPregunta = @idPregunta";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idPregunta", entidad.IdPregunta);
                    query.Parameters.AddWithValue("@texto", entidad.Texto ?? "");
                    query.Parameters.AddWithValue("@indicador", (object)entidad.Indicador ?? DBNull.Value);
                    query.Parameters.AddWithValue("@multi", entidad.Multi);
                    query.Parameters.AddWithValue("@activo", entidad.Activo);
                    query.Parameters.AddWithValue("@fechaInicio",entidad.FechaInicio.HasValue ? (object)entidad.FechaInicio.Value : DBNull.Value);
                    query.Parameters.AddWithValue("@fechaFin",entidad.FechaFin.HasValue ? (object)entidad.FechaFin.Value : DBNull.Value);

                    actualizado = query.ExecuteNonQuery() > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return actualizado;
        }

        // 🔹 Eliminar pregunta
        public bool EliminarPregunta(int idPregunta) {
            bool eliminado = false;
            string consulta = "DELETE FROM Pregunta WHERE idPregunta = @idPregunta";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idPregunta", idPregunta);
                    eliminado = query.ExecuteNonQuery() > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return eliminado;
        }

        // 🔹 Obtener detalle por Id
        public PreguntaEntidad ObtenerPorId(int idPregunta) {
            PreguntaEntidad item = null;
            string consulta = @"SELECT [idPregunta]
                                      ,[idTipoEncuesta]
                                      ,[texto]
                                      ,[Indicador]
                                      ,[orden]
                                      ,[random]
                                      ,[multi]
                                      ,[activo]
                                FROM Pregunta
                                WHERE idPregunta = @idPregunta";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idPregunta", idPregunta);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.Read()) {
                            item = new PreguntaEntidad {
                                IdPregunta = ManejoNulos.ManageNullInteger(dr["IdPregunta"]),
                                IdTipoEncuesta = ManejoNulos.ManageNullInteger(dr["IdTipoEncuesta"]),
                                Texto = ManejoNulos.ManageNullStr(dr["Texto"]),
                                Orden = ManejoNulos.ManageNullInteger(dr["Orden"]),
                                Random = ManejoNulos.ManegeNullBool(dr["Random"]),
                                Multi = ManejoNulos.ManegeNullBool(dr["Multi"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Indicador = ManejoNulos.ManageNullStr(dr["Indicador"]),
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        public bool TogglePregunta(int idPregunta) {
            string consulta = @"UPDATE Pregunta 
                        SET Activo = CASE WHEN Activo = 1 THEN 0 ELSE 1 END
                        WHERE IdPregunta = @p1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@p1", idPregunta);
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error TogglePregunta: " + ex.Message);
                return false;
            }
        }


        public List<PreguntaEntidad> ObtenerPreguntasAtributo() {
            var lista = new List<PreguntaEntidad>();

            string consulta = @"SELECT IdPregunta, Texto, Indicador
                        FROM Pregunta where random=1";


            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                lista.Add(new PreguntaEntidad {
                                    IdPregunta = Convert.ToInt32(dr["IdPregunta"]),
                                    Texto = dr["Texto"].ToString(),
                                    Indicador = dr["Indicador"].ToString(),
                                });
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error ObtenerPreguntas: " + ex.Message);
            }

            return lista;
        }

        public List<PreguntaDTO> ObtenerPreguntasConOpcionesYFlujo() {
            var resultado = new List<PreguntaDTO>();

            string sql = @"
        SELECT 
            p.IdPregunta, p.Texto AS PreguntaTexto, p.Orden, p.Activo,
            o.IdOpcion, o.Texto AS OpcionTexto,
            f.IdFlujo, f.IdPreguntaActual, f.IdOpcion, f.IdPreguntaSiguiente, f.EsFinalDeBloque
        FROM Pregunta p
        LEFT JOIN Opcion o ON p.IdPregunta = o.IdPregunta
        LEFT JOIN FlujoPregunta f ON o.IdOpcion = f.IdOpcion
        WHERE p.Activo = 1
        ORDER BY p.Orden, p.IdPregunta, o.IdOpcion;";

            using(var con = new SqlConnection(_conexion)) {
                con.Open();
                using(var cmd = new SqlCommand(sql, con)) {
                    using(var dr = cmd.ExecuteReader()) {
                        var dicPreguntas = new Dictionary<int, PreguntaDTO>();

                        while(dr.Read()) {
                            int idPregunta = Convert.ToInt32(dr["IdPregunta"]);

                            // si no existe aún, la agregamos
                            if(!dicPreguntas.ContainsKey(idPregunta)) {
                                dicPreguntas[idPregunta] = new PreguntaDTO {
                                    IdPregunta = idPregunta,
                                    Texto = dr["PreguntaTexto"].ToString(),
                                    Orden = Convert.ToInt32(dr["Orden"]),
                                    Activo = Convert.ToBoolean(dr["Activo"]),
                                    Opciones = new List<OpcionDTO>()
                                };
                            }

                            // agregamos la opción
                            if(dr["IdOpcion"] != DBNull.Value) {
                                var opcion = new OpcionDTO {
                                    IdOpcion = Convert.ToInt32(dr["IdOpcion"]),
                                    Texto = dr["OpcionTexto"].ToString(),
                                    Flujo = dr["IdFlujo"] == DBNull.Value ? null : new FlujoPreguntaDTO {
                                        IdFlujo = Convert.ToInt32(dr["IdFlujo"]),
                                        IdPreguntaActual = Convert.ToInt32(dr["IdPreguntaActual"]),
                                        IdOpcion = Convert.ToInt32(dr["IdOpcion"]),
                                        IdPreguntaSiguiente = Convert.ToInt32(dr["IdPreguntaSiguiente"]),
                                        EsFinalDeBloque = Convert.ToBoolean(dr["EsFinalDeBloque"])
                                    }
                                };

                                dicPreguntas[idPregunta].Opciones.Add(opcion);
                            }
                        }

                        // devolvemos agrupadas por Orden
                        resultado = dicPreguntas.Values
                            .OrderBy(p => p.Orden)
                            .ThenBy(p => p.IdPregunta)
                            .ToList();
                    }
                }
            }

            return resultado;
        }

    }
}
