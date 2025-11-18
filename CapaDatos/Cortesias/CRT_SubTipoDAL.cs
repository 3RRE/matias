using CapaEntidad.Cortesias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class CRT_SubTipoDAL {
        private readonly string _conexion;

        public CRT_SubTipoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarSubTipo(CRT_SubTipo subTipo) {
            int idActualizado;
            string consulta = @"
                UPDATE CRT_SubTipo
                SET
                    IdTipo = @IdTipo,
                    Nombre = @Nombre,
                    ImagenUrl = @ImagenUrl,
	                Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", subTipo.Id);
                    query.Parameters.AddWithValue("@IdTipo", subTipo.IdTipo);
                    query.Parameters.AddWithValue("@Nombre", subTipo.Nombre);
                    query.Parameters.AddWithValue("@ImagenUrl", subTipo.ImagenUrl);
                    query.Parameters.AddWithValue("@Estado", subTipo.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarSubTipo(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM CRT_SubTipo
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

        public int InsertarSubTipo(CRT_SubTipo subTipo) {
            int idInsertado;
            string consulta = @"
                INSERT INTO CRT_SubTipo(IdTipo, Nombre, ImagenUrl, IdUsuario, Estado)
                OUTPUT INSERTED.Id
                VALUES (@IdTipo, @Nombre, @ImagenUrl, @IdUsuario, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTipo", subTipo.IdTipo);
                    query.Parameters.AddWithValue("@Nombre", subTipo.Nombre);
                    query.Parameters.AddWithValue("@ImagenUrl", subTipo.ImagenUrl);
                    query.Parameters.AddWithValue("@IdUsuario", subTipo.IdUsuario);
                    query.Parameters.AddWithValue("@Estado", subTipo.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public CRT_SubTipo ObtenerSubTipoPorId(int id) {
            CRT_SubTipo item = new CRT_SubTipo();
            string consulta = @"
                SELECT
	                st.Id,
                    st.IdTipo,
	                st.Nombre,
                    st.ImagenUrl,
	                st.IdUsuario,
	                st.Estado,
                    st.FechaRegistro,
                    st.FechaModificacion,
	                t.Nombre AS NombreTipo
                FROM
	                CRT_SubTipo AS st
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                WHERE
	                st.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CRT_SubTipo {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                            };
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<CRT_SubTipo> ObtenerSubTipos() {
            List<CRT_SubTipo> items = new List<CRT_SubTipo>();
            string consulta = @"
                SELECT
	                st.Id,
                    st.IdTipo,
	                st.Nombre,
                    st.ImagenUrl,
	                st.IdUsuario,
	                st.Estado,
                    st.FechaRegistro,
                    st.FechaModificacion,
	                t.Nombre AS NombreTipo
                FROM
	                CRT_SubTipo AS st
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                ORDER BY t.Id, st.Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_SubTipo item = new CRT_SubTipo {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                            };
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<CRT_SubTipo> ObtenerSubTiposPorIdsTipo(string idsTipoStr) {
            List<CRT_SubTipo> items = new List<CRT_SubTipo>();
            string consulta = $@"
                SELECT
	                st.Id,
                    st.IdTipo,
	                st.Nombre,
                    st.ImagenUrl,
	                st.IdUsuario,
	                st.Estado,
                    st.FechaRegistro,
                    st.FechaModificacion,
	                t.Nombre AS NombreTipo
                FROM
	                CRT_SubTipo AS st
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                WHERE st.IdTipo IN ({idsTipoStr})
                ORDER BY t.Id, st.Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_SubTipo item = new CRT_SubTipo {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                            };
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }
    }
}
