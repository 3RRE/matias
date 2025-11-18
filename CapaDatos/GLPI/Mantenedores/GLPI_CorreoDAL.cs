using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_CorreoDAL {
        private readonly string _conexion;

        public GLPI_CorreoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int InsertarCorreo(GLPI_Correo correo) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_Correo(Correo, IdUsuaroRegistra, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Correo, @IdUsuaroRegistra, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Correo", correo.Correo);
                    query.Parameters.AddWithValue("@IdUsuaroRegistra", correo.IdUsuaroRegistra);
                    query.Parameters.AddWithValue("@Estado", correo.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_Correo ObtenerCorreoPorCorreo(string correo) {
            GLPI_Correo item = new GLPI_Correo();
            string consulta = @"
                SELECT 
	                Id,
	                Correo,
	                IdUsuaroRegistra,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Correo
                WHERE
	                Correo = @Correo
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Correo", correo);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<GLPI_Correo> ObtenerCorreos() {
            List<GLPI_Correo> items = new List<GLPI_Correo>();
            string consulta = @"
                SELECT 
	                Id,
	                Correo,
	                IdUsuaroRegistra,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Correo
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

        public List<GLPI_Correo> ObtenerCorreosActivos() {
            List<GLPI_Correo> items = new List<GLPI_Correo>();
            string consulta = @"
                SELECT 
	                Id,
	                Correo,
	                IdUsuaroRegistra,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Correo
                WHERE
                    Estado = 1
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

        private GLPI_Correo ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_Correo {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Correo = ManejoNulos.ManageNullStr(dr["Correo"]),
                IdUsuaroRegistra = ManejoNulos.ManageNullInteger(dr["IdUsuaroRegistra"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
