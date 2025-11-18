using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Notificaciones.Mantenedores {
    public class SistemaDAL {
        private readonly string _conexion;

        public SistemaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion_notificaciones"].ConnectionString;
        }

        public List<SistemaDto> ObtenerSistema() {
            List<SistemaDto> items = new List<SistemaDto>();
            string consulta = @"
                SELECT
                    Id,
                    Nombre,
                    Descripcion,
                    FechaRegistro,
                    FechaModificacion
                FROM
                    Sistema";

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
        public int InsertarSistema(Sistema sistema) {
            int idInsertado;
            string consulta = @"
                INSERT INTO Sistema (Nombre, Descripcion, FechaRegistro)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Descripcion, @FechaRegistro)";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", sistema.Nombre);
                    query.Parameters.AddWithValue("@Descripcion", sistema.Descripcion);
                    query.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }
            return idInsertado;
        }

        public int ActualizarSistema(Sistema sistema) {
            int idActualizado;
            string consulta = @"
                UPDATE Sistema
                SET
                    Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", sistema.Id);
                    query.Parameters.AddWithValue("@Nombre", sistema.Nombre);
                    query.Parameters.AddWithValue("@Descripcion", sistema.Descripcion);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public SistemaDto ObtenerSistemaPorId(int id) {
            SistemaDto item = new SistemaDto();
            string consulta = @"
                SELECT
                    sis.Id,
                    sis.Nombre,
                    sis.Descripcion
                FROM
                    Sistema AS sis
                WHERE
                    sis.Id = @Id";

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

        public int EliminarSistema (int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM Sistema
                OUTPUT DELETED.Id
                WHERE Id = @Id";
        
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

        private SistemaDto ConstruirObjeto(SqlDataReader dr) {
            return new SistemaDto {
                Id = ManejoNulos.ManageNullInteger(dr["ID"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"])
            };
        }
    }
}
