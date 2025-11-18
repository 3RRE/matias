using CapaEntidad.Cortesias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class CRT_TipoDAL {
        private readonly string _conexion;

        public CRT_TipoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarTipo(CRT_Tipo tipo) {
            int idActualizado;
            string consulta = @"
                UPDATE CRT_Tipo
                SET 
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
                    query.Parameters.AddWithValue("@Id", tipo.Id);
                    query.Parameters.AddWithValue("@Nombre", tipo.Nombre);
                    query.Parameters.AddWithValue("@ImagenUrl", tipo.ImagenUrl);
                    query.Parameters.AddWithValue("@Estado", tipo.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarTipo(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM CRT_Tipo
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

        public int InsertarTipo(CRT_Tipo tipo) {
            int idInsertado;
            string consulta = @"
                INSERT INTO CRT_Tipo(Nombre, ImagenUrl, IdUsuario, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @ImagenUrl, @IdUsuario, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", tipo.Nombre);
                    query.Parameters.AddWithValue("@ImagenUrl", tipo.ImagenUrl);
                    query.Parameters.AddWithValue("@IdUsuario", tipo.IdUsuario);
                    query.Parameters.AddWithValue("@Estado", tipo.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public CRT_Tipo ObtenerTipoPorId(int id) {
            CRT_Tipo item = new CRT_Tipo();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
                    ImagenUrl,
	                IdUsuario,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                CRT_Tipo
                WHERE
	                Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CRT_Tipo {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                            };
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<CRT_Tipo> ObtenerTipos() {
            List<CRT_Tipo> items = new List<CRT_Tipo>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
                    ImagenUrl,
	                IdUsuario,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                CRT_Tipo
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_Tipo item = new CRT_Tipo {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
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
