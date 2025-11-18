using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_IdentificadorDAL {
        private readonly string _conexion;

        public GLPI_IdentificadorDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarIdentificador(GLPI_Identificador identificador) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_Identificador
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
                    query.Parameters.AddWithValue("@Id", identificador.Id);
                    query.Parameters.AddWithValue("@Nombre", identificador.Nombre);
                    query.Parameters.AddWithValue("@Estado", identificador.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarIdentificador(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_Identificador
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

        public int InsertarIdentificador(GLPI_Identificador identificador) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_Identificador(Nombre, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", identificador.Nombre);
                    query.Parameters.AddWithValue("@Estado", identificador.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_Identificador ObtenerIdentificadorPorId(int id) {
            GLPI_Identificador item = new GLPI_Identificador();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Identificador
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

        public List<GLPI_Identificador> ObtenerIdentificadores() {
            List<GLPI_Identificador> items = new List<GLPI_Identificador>();
            string consulta = @"
                SELECT 
	                Id,
	                Nombre,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Identificador
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

        private GLPI_Identificador ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_Identificador {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
