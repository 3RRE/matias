using CapaEntidad.BOT.Entities;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class BOT_ContactoDAL {
        private readonly string _conexion;

        public BOT_ContactoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarContacto(BOT_ContactoEntidad contacto) {
            int idActualizado;
            string consulta = @"
                UPDATE BOT_Contacto
                SET
                    IdCargo = @IdCargo,
                    Nombre = @Nombre,
                    CodigoPaisCelular = @CodigoPaisCelular,
                    Celular = @Celular,
                    Correo = @Correo,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", contacto.Id);
                    query.Parameters.AddWithValue("@IdCargo", contacto.IdCargo);
                    query.Parameters.AddWithValue("@Nombre", contacto.Nombre);
                    query.Parameters.AddWithValue("@CodigoPaisCelular", contacto.CodigoPaisCelular);
                    query.Parameters.AddWithValue("@Celular", contacto.Celular);
                    query.Parameters.AddWithValue("@Correo", contacto.Correo);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarContacto(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM BOT_Contacto
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

        public int InsertarContacto(BOT_ContactoEntidad contacto) {
            int idInsertado;
            string consulta = @"
                INSERT INTO BOT_Contacto(IdCargo, Nombre, CodigoPaisCelular, Celular, Correo)
                OUTPUT INSERTED.Id
                VALUES (@IdCargo, @Nombre, @CodigoPaisCelular, @Celular, @Correo)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCargo", contacto.IdCargo);
                    query.Parameters.AddWithValue("@Nombre", contacto.Nombre);
                    query.Parameters.AddWithValue("@CodigoPaisCelular", contacto.CodigoPaisCelular);
                    query.Parameters.AddWithValue("@Celular", contacto.Celular);
                    query.Parameters.AddWithValue("@Correo", contacto.Correo);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public BOT_ContactoEntidad ObtenerContactoPorId(int id) {
            BOT_ContactoEntidad item = new BOT_ContactoEntidad();
            string consulta = @"
                SELECT
                    co.Id,
                    co.IdCargo,
                    ar.Nombre AS NombreArea,
                    ca.Nombre AS NombreCargo,
                    co.Nombre,
                    co.CodigoPaisCelular,
                    co.Celular,
                    co.Correo,
                    co.FechaRegistro,
                    co.FechaModificacion
                FROM
                    BOT_Contacto as co
                LEFT JOIN BOT_Cargo AS ca ON ca.Id = co.IdCargo
                LEFT JOIN BOT_Area AS ar ON ar.Id = ca.IdArea
                WHERE
                    co.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = MapearContacto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<BOT_ContactoEntidad> ObtenerContactos() {
            List<BOT_ContactoEntidad> items = new List<BOT_ContactoEntidad>();
            string consulta = @"
                SELECT
                    co.Id,
                    co.IdCargo,
                    ar.Nombre AS NombreArea,
                    ca.Nombre AS NombreCargo,
                    co.Nombre,
                    co.CodigoPaisCelular,
                    co.Celular,
                    co.Correo,
                    co.FechaRegistro,
                    co.FechaModificacion
                FROM
                    BOT_Contacto as co
                LEFT JOIN BOT_Cargo AS ca ON ca.Id = co.IdCargo
                LEFT JOIN BOT_Area AS ar ON ar.Id = ca.IdArea
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearContacto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        #region Helper
        private BOT_ContactoEntidad MapearContacto(SqlDataReader dr) {
            return new BOT_ContactoEntidad {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                NombreArea = ManejoNulos.ManageNullStr(dr["NombreArea"]),
                NombreCargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                CodigoPaisCelular = ManejoNulos.ManageNullStr(dr["CodigoPaisCelular"]),
                Celular = ManejoNulos.ManageNullStr(dr["Celular"]),
                Correo = ManejoNulos.ManageNullStr(dr["Correo"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
        #endregion
    }
}
