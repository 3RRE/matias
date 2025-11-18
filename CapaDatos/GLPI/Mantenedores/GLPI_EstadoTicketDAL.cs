using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_EstadoTicketDAL {
        private readonly string _conexion;

        public GLPI_EstadoTicketDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarEstadoTicket(GLPI_EstadoTicket estadoTicket) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_EstadoTicket
                SET 
                    Nombre = @Nombre,
	               -- Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", estadoTicket.Id);
                    query.Parameters.AddWithValue("@Nombre", estadoTicket.Nombre);
                    //query.Parameters.AddWithValue("@Estado", estadoTicket.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch {
                idActualizado = 0;
            }
            return idActualizado;
        }
        
        public int EliminarEstadoTicket(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_EstadoTicket
                OUTPUT DELETED.Id
                WHERE Id = @Id
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public int InsertarEstadoTicket(GLPI_EstadoTicket estadoTicket) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_EstadoTicket(Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Estado)
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", estadoTicket.Nombre);
                    query.Parameters.AddWithValue("@Estado", estadoTicket.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_EstadoTicket ObtenerEstadoTicketPorId(int id) {
            GLPI_EstadoTicket item = new GLPI_EstadoTicket();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_EstadoTicket
                WHERE
	                Id = @Id
            ";
            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            }
            catch { }
            return item;
        }

        public List<GLPI_EstadoTicket> ObtenerEstadosTicket() {
            List<GLPI_EstadoTicket> items = new List<GLPI_EstadoTicket>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_EstadoTicket
            ";
            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using (SqlDataReader dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            }
            catch { }
            return items;
        }

        private GLPI_EstadoTicket ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_EstadoTicket {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
