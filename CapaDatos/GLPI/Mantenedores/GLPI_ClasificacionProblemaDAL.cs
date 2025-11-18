using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_ClasificacionProblemaDAL {
        private readonly string _conexion;

        public GLPI_ClasificacionProblemaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarClasificacionProblema(GLPI_ClasificacionProblema clasificacionProblema) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_ClasificacionProblema
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
                    query.Parameters.AddWithValue("@Id", clasificacionProblema.Id);
                    query.Parameters.AddWithValue("@Nombre", clasificacionProblema.Nombre);
                    query.Parameters.AddWithValue("@Estado", clasificacionProblema.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarClasificacionProblema(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_ClasificacionProblema
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

        public int InsertarClasificacionProblema(GLPI_ClasificacionProblema clasificacionProblema) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_ClasificacionProblema(Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", clasificacionProblema.Nombre);
                    query.Parameters.AddWithValue("@Estado", clasificacionProblema.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_ClasificacionProblema ObtenerClasificacionProblemaPorId(int id) {
            GLPI_ClasificacionProblema item = new GLPI_ClasificacionProblema();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_ClasificacionProblema
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

        public List<GLPI_ClasificacionProblema> ObtenerClasificacionProblemas() {
            List<GLPI_ClasificacionProblema> items = new List<GLPI_ClasificacionProblema>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_ClasificacionProblema
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

        private GLPI_ClasificacionProblema ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_ClasificacionProblema {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
