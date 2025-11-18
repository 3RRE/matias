using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_CategoriaDAL {
        private readonly string _conexion;

        public GLPI_CategoriaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarCategoria(GLPI_Categoria categoria) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_Categoria
                SET
                    IdPartida = @IdPartida,
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
                    query.Parameters.AddWithValue("@Id", categoria.Id);
                    query.Parameters.AddWithValue("@IdPartida", categoria.IdPartida);
                    query.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    query.Parameters.AddWithValue("@Estado", categoria.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarCategoria(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_Categoria
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

        public int InsertarCategoria(GLPI_Categoria categoria) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_Categoria(IdPartida, Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@IdPartida, @Nombre, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdPartida", categoria.IdPartida);
                    query.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    query.Parameters.AddWithValue("@Estado", categoria.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_Categoria ObtenerCategoriaPorId(int id) {
            GLPI_Categoria item = new GLPI_Categoria();
            string consulta = @"
                SELECT 
	                c.Id,
	                c.IdPartida,
	                p.Nombre AS NombrePartida,
	                c.Nombre,
	                c.Estado,
                    c.FechaRegistro,
                    c.FechaModificacion
                FROM
	                GLPI_Categoria AS c
                LEFT JOIN GLPI_Partida AS p ON p.Id = c.IdPartida
                WHERE
	                c.Id = @Id
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

        public List<GLPI_Categoria> ObtenerCategorias() {
            List<GLPI_Categoria> items = new List<GLPI_Categoria>();
            string consulta = @"
                SELECT 
	                c.Id,
	                c.IdPartida,
	                p.Nombre AS NombrePartida,
	                c.Nombre,
	                c.Estado,
                    c.FechaRegistro,
                    c.FechaModificacion
                FROM
	                GLPI_Categoria AS c
                LEFT JOIN GLPI_Partida AS p ON p.Id = c.IdPartida
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

        private GLPI_Categoria ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_Categoria {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdPartida = ManejoNulos.ManageNullInteger(dr["IdPartida"]),
                NombrePartida = ManejoNulos.ManageNullStr(dr["NombrePartida"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
