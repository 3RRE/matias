using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_SubCategoriaDAL {
        private readonly string _conexion;

        public GLPI_SubCategoriaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarSubCategoria(GLPI_SubCategoria subCategoria) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_SubCategoria
                SET
                    IdCategoria = @IdCategoria,
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
                    query.Parameters.AddWithValue("@Id", subCategoria.Id);
                    query.Parameters.AddWithValue("@IdCategoria", subCategoria.IdCategoria);
                    query.Parameters.AddWithValue("@Nombre", subCategoria.Nombre);
                    query.Parameters.AddWithValue("@Estado", subCategoria.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarSubCategoria(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_SubCategoria
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

        public int InsertarSubCategoria(GLPI_SubCategoria subCategoria) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_SubCategoria(IdCategoria, Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@IdCategoria, @Nombre, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCategoria", subCategoria.IdCategoria);
                    query.Parameters.AddWithValue("@Nombre", subCategoria.Nombre);
                    query.Parameters.AddWithValue("@Estado", subCategoria.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_SubCategoria ObtenerSubCategoriaPorId(int id) {
            GLPI_SubCategoria item = new GLPI_SubCategoria();
            string consulta = @"
                SELECT
	                sc.Id,
	                c.IdPartida,
	                p.Nombre AS NombrePartida,
	                sc.IdCategoria,
	                c.Nombre AS NombreCategoria,
	                sc.Nombre,
	                sc.Estado,
                    sc.FechaRegistro,
                    sc.FechaModificacion
                FROM
	                GLPI_SubCategoria AS sc
                LEFT JOIN GLPI_Categoria AS c ON c.Id = sc.IdCategoria
                LEFT JOIN GLPI_Partida AS p ON p.Id = c.IdPartida
                WHERE
	                sc.Id = @Id
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

        public List<GLPI_SubCategoria> ObtenerSubCategorias() {
            List<GLPI_SubCategoria> items = new List<GLPI_SubCategoria>();
            string consulta = @"
                SELECT
	                sc.Id,
	                c.IdPartida,
	                p.Nombre AS NombrePartida,
	                sc.IdCategoria,
	                c.Nombre AS NombreCategoria,
	                sc.Nombre,
	                sc.Estado,
                    sc.FechaRegistro,
                    sc.FechaModificacion
                FROM
	                GLPI_SubCategoria AS sc
                LEFT JOIN GLPI_Categoria AS c ON c.Id = sc.IdCategoria
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

        private GLPI_SubCategoria ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_SubCategoria {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdPartida = ManejoNulos.ManageNullInteger(dr["IdPartida"]),
                NombrePartida = ManejoNulos.ManageNullStr(dr["NombrePartida"]),
                IdCategoria = ManejoNulos.ManageNullInteger(dr["IdCategoria"]),
                NombreCategoria = ManejoNulos.ManageNullStr(dr["NombreCategoria"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
