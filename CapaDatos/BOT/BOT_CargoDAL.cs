using CapaEntidad.BOT.Entities;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class BOT_CargoDAL {
        private readonly string _conexion;

        public BOT_CargoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarCargo(BOT_CargoEntidad cargo) {
            int idActualizado;
            string consulta = @"
                UPDATE BOT_Cargo
                SET 
                    IdArea = @IdArea,
                    Nombre = @Nombre,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", cargo.Id);
                    query.Parameters.AddWithValue("@IdArea", cargo.IdArea);
                    query.Parameters.AddWithValue("@Nombre", cargo.Nombre);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarCargo(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM BOT_Cargo
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

        public int InsertarCargo(BOT_CargoEntidad cargo) {
            int idInsertado;
            string consulta = @"
                INSERT INTO BOT_Cargo(IdArea, Nombre)
                OUTPUT INSERTED.Id
                VALUES (@IdArea, @Nombre)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdArea", cargo.IdArea);
                    query.Parameters.AddWithValue("@Nombre", cargo.Nombre);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public BOT_CargoEntidad ObtenerCargoPorId(int id) {
            BOT_CargoEntidad item = new BOT_CargoEntidad();
            string consulta = @"
                SELECT 
	                c.Id,
	                c.IdArea,
	                a.Nombre AS NombreArea,
	                c.Nombre,
                    c.FechaRegistro,
                    c.FechaModificacion,
                    a.Id AS IdArea,
                    a.FechaRegistro AS FechaRegistroArea,
                    a.FechaModificacion AS FechaModificacionArea
                FROM BOT_Cargo as c
                LEFT JOIN BOT_Area AS a ON a.Id = c.IdArea
                WHERE c.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = MapearCargo(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<BOT_CargoEntidad> ObtenerCargos() {
            List<BOT_CargoEntidad> items = new List<BOT_CargoEntidad>();
            string consulta = @"
                SELECT 
	                c.Id,
	                c.IdArea,
	                a.Nombre AS NombreArea,
	                c.Nombre,
                    c.FechaRegistro,
                    c.FechaModificacion,
                    a.Id AS IdArea,
                    a.FechaRegistro AS FechaRegistroArea,
                    a.FechaModificacion AS FechaModificacionArea
                FROM BOT_Cargo as c
                LEFT JOIN BOT_Area AS a ON a.Id = c.IdArea
                ORDER BY a.Nombre, c.Nombre
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearCargo(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        #region Helper
        private BOT_CargoEntidad MapearCargo(SqlDataReader dr) {
            return new BOT_CargoEntidad {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdArea = ManejoNulos.ManageNullInteger(dr["IdArea"]),
                NombreArea = ManejoNulos.ManageNullStr(dr["NombreArea"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                Area = new BOT_AreaEntidad() {
                    Id = ManejoNulos.ManageNullInteger(dr["IdArea"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreArea"]),
                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistroArea"]),
                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacionArea"]),
                }
            };
        }
        #endregion
    }
}
