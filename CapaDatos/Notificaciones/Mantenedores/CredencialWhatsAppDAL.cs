using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Notificaciones.Mantenedores {
    public class CredencialWhatsAppDAL {
        private readonly string _conexion;

        public CredencialWhatsAppDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion_notificaciones"].ConnectionString;
        }

        public List<CredencialWhatsAppDto> ObtenerCredencialesWhatsApp() {
            List<CredencialWhatsAppDto> items = new List<CredencialWhatsAppDto>();
            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.UrlBase,
                       c.Instancia,
                       c.Token, 
                       c.FechaRegistro,
                       c.FechaModificacion
                FROM CredencialWhatsAppUltraMsg c
                INNER JOIN Sistema s ON c.IdSistema = s.Id";

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
        
        public int InsertarCredencialWhatsApp(CredencialWhatsApp credencial) {
            int idInsertado;
            string consulta = @"
                INSERT INTO CredencialWhatsAppUltraMsg (IdSistema, UrlBase, Instancia, Token, FechaRegistro)
                OUTPUT INSERTED.Id
                VALUES (@IdSistema, @UrlBase, @Instancia, @Token, @FechaRegistro)";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdSistema", credencial.IdSistema);
                    query.Parameters.AddWithValue("@UrlBase", credencial.UrlBase);
                    query.Parameters.AddWithValue("@Instancia", credencial.Instancia);
                    query.Parameters.AddWithValue("@Token", credencial.Token);
                    query.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }
            return idInsertado;
        }

        public int ActualizarCredencialWhatsApp(CredencialWhatsApp credencial) {
            int idActualizado;
            string consulta = @"
                UPDATE CredencialWhatsAppUltraMsg
                SET IdSistema = @IdSistema,
                    UrlBase = @UrlBase,
                    Instancia = @Instancia,
                    Token = @Token,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", credencial.Id);
                    query.Parameters.AddWithValue("@IdSistema", credencial.IdSistema);
                    query.Parameters.AddWithValue("@UrlBase", credencial.UrlBase);
                    query.Parameters.AddWithValue("@Instancia", credencial.Instancia);
                    query.Parameters.AddWithValue("@Token", credencial.Token);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }
        
        public List<CredencialWhatsAppDto> ObtenerCredencialesWhatsAppPorSistema(int IdSistema) {
            List<CredencialWhatsAppDto> items = new List<CredencialWhatsAppDto>();
            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.UrlBase,
                       c.Instancia,
                       c.Token, 
                       c.FechaRegistro,
                       c.FechaModificacion
                FROM CredencialWhatsAppUltraMsg c
                INNER JOIN Sistema s ON c.IdSistema = s.Id
                WHERE c.IdSistema = @IdSistema";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdSistema", IdSistema);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }
        
        public CredencialWhatsAppDto ObtenerCredencialWhatsAppPorId(int id) {
            CredencialWhatsAppDto item = new CredencialWhatsAppDto();
            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.UrlBase,
                       c.Instancia,
                       c.Token, 
                       c.FechaRegistro,
                       c.FechaModificacion
                FROM CredencialWhatsAppUltraMsg c
                INNER JOIN Sistema s ON c.IdSistema = s.Id
                WHERE c.Id = @Id";

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

        public int EliminarCredencialWhatsApp(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM CredencialWhatsAppUltraMsg
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

        private CredencialWhatsAppDto ConstruirObjeto(SqlDataReader dr) {
            return new CredencialWhatsAppDto {
                Id = ManejoNulos.ManageNullInteger(dr["ID"]),
                IdSistema = ManejoNulos.ManageNullInteger(dr["IdSistema"]),
                NombreSistema = ManejoNulos.ManageNullStr(dr["NombreSistema"]),
                UrlBase = ManejoNulos.ManageNullStr(dr["UrlBase"]),
                Instancia = ManejoNulos.ManageNullStr(dr["Instancia"]),
                Token = ManejoNulos.ManageNullStr(dr["Token"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
