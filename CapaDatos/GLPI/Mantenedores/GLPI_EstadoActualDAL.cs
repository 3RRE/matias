using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_EstadoActualDAL {
        private readonly string _conexion;

        public GLPI_EstadoActualDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarEstadoActual(GLPI_EstadoActual estadoActual) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_EstadoActual
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
                    query.Parameters.AddWithValue("@Id", estadoActual.Id);
                    query.Parameters.AddWithValue("@Nombre", estadoActual.Nombre);
                    query.Parameters.AddWithValue("@Estado", estadoActual.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarEstadoActual(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_EstadoActual
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

        public int InsertarEstadoActual(GLPI_EstadoActual estadoActual) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_EstadoActual(Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", estadoActual.Nombre);
                    query.Parameters.AddWithValue("@Estado", estadoActual.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_EstadoActual ObtenerEstadoActualPorId(int id) {
            GLPI_EstadoActual item = new GLPI_EstadoActual();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_EstadoActual
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

        public List<GLPI_EstadoActual> ObtenerEstadosActuales() {
            List<GLPI_EstadoActual> items = new List<GLPI_EstadoActual>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_EstadoActual
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

        private GLPI_EstadoActual ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_EstadoActual {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
