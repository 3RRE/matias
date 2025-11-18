using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Entity.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.SatisfaccionCliente.Mantenedores {
    public class ESC_PreguntaDAL {
        private readonly string _conexion;

        public ESC_PreguntaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarPregunta(ESC_Pregunta pregunta) {
            int idActualizado;
            string consulta = @"
                UPDATE ESC_Pregunta
                SET
                    Texto = @Texto,
	                EsObligatoria = @EsObligatoria,
                    Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", pregunta.Id);
                    query.Parameters.AddWithValue("@Texto", pregunta.Texto);
                    query.Parameters.AddWithValue("@EsObligatoria", pregunta.EsObligatoria);
                    query.Parameters.AddWithValue("@Estado", pregunta.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarPregunta(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM ESC_Pregunta
                OUTPUT DELETED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public int InsertarPregunta(ESC_Pregunta pregunta) {
            int idInsertado;
            string consulta = @"
                INSERT INTO ESC_Pregunta(CodSala, Texto, EsObligatoria, Estado)
                OUTPUT INSERTED.Id
                VALUES (@CodSala, @Texto, @EsObligatoria, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", pregunta.CodSala);
                    query.Parameters.AddWithValue("@Texto", pregunta.Texto);
                    query.Parameters.AddWithValue("@EsObligatoria", pregunta.EsObligatoria);
                    query.Parameters.AddWithValue("@Estado", pregunta.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public ESC_PreguntaDto ObtenerPreguntaPorId(int id) {
            ESC_PreguntaDto item = new ESC_PreguntaDto();
            string consulta = @"
                SELECT
	                pre.Id,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto,
	                pre.EsObligatoria,
	                pre.Estado
                FROM
	                ESC_Pregunta AS pre
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
                WHERE
	                pre.Id = @Id
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

        public List<ESC_PreguntaDto> ObtenerPreguntas() {
            List<ESC_PreguntaDto> items = new List<ESC_PreguntaDto>();
            string consulta = @"
                SELECT
	                pre.Id,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto,
	                pre.EsObligatoria,
	                pre.Estado
                FROM
	                ESC_Pregunta AS pre
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
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

        public List<ESC_PreguntaDto> ObtenerPreguntasPorCodSala(int codSala) {
            List<ESC_PreguntaDto> items = new List<ESC_PreguntaDto>();
            string consulta = @"
                SELECT
	                pre.Id,
	                pre.CodSala,
	                s.Nombre AS NombreSala,
	                pre.Texto,
	                pre.EsObligatoria,
	                pre.Estado
                FROM
	                ESC_Pregunta AS pre
                LEFT JOIN Sala AS s ON s.CodSala = pre.CodSala
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

        public bool PreguntaEsDeSala(int codSala, int idPregunta) {
            int idInsertado;
            string consulta = @"
                SELECT COUNT(Id)
                FROM ESC_Pregunta
                WHERE CodSala = @CodSala AND Id = @IdPregunta
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    query.Parameters.AddWithValue("@IdPregunta", idPregunta);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado == 1;
        }

        private ESC_PreguntaDto ConstruirObjeto(SqlDataReader dr) {
            return new ESC_PreguntaDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Sala = new ESC_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                },
                Texto = ManejoNulos.ManageNullStr(dr["Texto"]),
                EsObligatoria = ManejoNulos.ManegeNullBool(dr["EsObligatoria"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"])
            };
        }
    }
}
