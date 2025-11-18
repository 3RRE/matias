using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_NivelAtencionDAL {
        private readonly string _conexion;

        public GLPI_NivelAtencionDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarNivelAtencion(GLPI_NivelAtencion nivelAtencion) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_NivelAtencion
                SET 
                    Nombre = @Nombre,
                    Color = @Color,
	                Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", nivelAtencion.Id);
                    query.Parameters.AddWithValue("@Nombre", nivelAtencion.Nombre);
                    query.Parameters.AddWithValue("@Color", nivelAtencion.Color);
                    query.Parameters.AddWithValue("@Estado", nivelAtencion.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarNivelAtencion(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_NivelAtencion
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

        public int InsertarNivelAtencion(GLPI_NivelAtencion nivelAtencion) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_NivelAtencion(Nombre, Color, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Color, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", nivelAtencion.Nombre);
                    query.Parameters.AddWithValue("@Color", nivelAtencion.Color);
                    query.Parameters.AddWithValue("@Estado", nivelAtencion.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_NivelAtencion ObtenerNivelAtencionPorId(int id) {
            GLPI_NivelAtencion item = new GLPI_NivelAtencion();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
                    Color,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_NivelAtencion
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

        public List<GLPI_NivelAtencion> ObtenerNivelesAtencion() {
            List<GLPI_NivelAtencion> items = new List<GLPI_NivelAtencion>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
                    Color,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_NivelAtencion
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

        private GLPI_NivelAtencion ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_NivelAtencion {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Color = ManejoNulos.ManageNullStr(dr["Color"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
