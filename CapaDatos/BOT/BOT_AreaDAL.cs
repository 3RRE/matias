using CapaEntidad.BOT.Entities;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class BOT_AreaDAL {
        private readonly string _conexion;

        public BOT_AreaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarArea(BOT_AreaEntidad area) {
            int idActualizado;
            string consulta = @"
                UPDATE BOT_Area
                SET 
                    Nombre = @Nombre,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", area.Id);
                    query.Parameters.AddWithValue("@Nombre", area.Nombre);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarArea(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM BOT_Area
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

        public int InsertarArea(BOT_AreaEntidad area) {
            int idInsertado;
            string consulta = @"
                INSERT INTO BOT_Area(Nombre)
                OUTPUT INSERTED.Id
                VALUES (@Nombre)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", area.Nombre);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public BOT_AreaEntidad ObtenerAreaPorId(int id) {
            BOT_AreaEntidad item = new BOT_AreaEntidad();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                BOT_Area
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
                            item = MapearArea(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<BOT_AreaEntidad> ObtenerAreas() {
            List<BOT_AreaEntidad> items = new List<BOT_AreaEntidad>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                BOT_Area
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearArea(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        #region Helper
        private BOT_AreaEntidad MapearArea(SqlDataReader dr) {
            return new BOT_AreaEntidad {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
        #endregion
    }
}
