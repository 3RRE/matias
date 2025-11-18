using CapaEntidad.Cortesias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class CRT_MarcaDAL {
        private readonly string _conexion;

        public CRT_MarcaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarMarca(CRT_Marca marca) {
            int idActualizado;
            string consulta = @"
                UPDATE CRT_Marca
                SET 
                    Nombre = @Nombre,
	                Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", marca.Id);
                    query.Parameters.AddWithValue("@Nombre", marca.Nombre);
                    query.Parameters.AddWithValue("@Estado", marca.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarMarca(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM CRT_Marca
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

        public int InsertarMarca(CRT_Marca marca) {
            int idInsertado;
            string consulta = @"
                INSERT INTO CRT_Marca(Nombre, IdUsuario, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @IdUsuario, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", marca.Nombre);
                    query.Parameters.AddWithValue("@IdUsuario", marca.IdUsuario);
                    query.Parameters.AddWithValue("@Estado", marca.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public CRT_Marca ObtenerMarcaPorId(int id) {
            CRT_Marca item = new CRT_Marca();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                IdUsuario,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                CRT_Marca
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
                            item = new CRT_Marca {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
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

        public List<CRT_Marca> ObtenerMarcas() {
            List<CRT_Marca> items = new List<CRT_Marca>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                IdUsuario,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                CRT_Marca
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_Marca item = new CRT_Marca {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
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
