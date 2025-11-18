using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_TipoOperacionDAL {
        private readonly string _conexion;

        public GLPI_TipoOperacionDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarTipoOperacion(GLPI_TipoOperacion tipoOperacion) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_TipoOperacion
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
                    query.Parameters.AddWithValue("@Id", tipoOperacion.Id);
                    query.Parameters.AddWithValue("@Nombre", tipoOperacion.Nombre);
                    query.Parameters.AddWithValue("@Estado", tipoOperacion.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarTipoOperacion(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_TipoOperacion
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

        public int InsertarTipoOperacion(GLPI_TipoOperacion tipoOperacion) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_TipoOperacion(Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", tipoOperacion.Nombre);
                    query.Parameters.AddWithValue("@Estado", tipoOperacion.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_TipoOperacion ObtenerTipoOperacionPorId(int id) {
            GLPI_TipoOperacion item = new GLPI_TipoOperacion();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_TipoOperacion
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
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<GLPI_TipoOperacion> ObtenerTiposOperacion() {
            List<GLPI_TipoOperacion> items = new List<GLPI_TipoOperacion>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_TipoOperacion
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

        private GLPI_TipoOperacion ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_TipoOperacion {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
