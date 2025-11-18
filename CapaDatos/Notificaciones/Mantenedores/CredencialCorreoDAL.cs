using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Notificaciones.Mantenedores {
    public class CredencialCorreoDAL {
        private readonly string _conexion;

        public CredencialCorreoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion_notificaciones"].ConnectionString;
        }

        public int InsertarCredencialCorreo(CredencialCorreo credencial) {
            int idInsertado;
            string consulta = @"
                INSERT INTO CredencialCorreo 
                    (IdSistema,
                    NombreRemitente,
                    Correo,
                    ClaveSMTP,      
                    ServidorSMTP, 
                    PuertoSMTP, 
                    SSLHabilitado,  
                    CuotaDiaria,    
                    Prioridad,
                    Estado,
                    FechaRegistro)
                OUTPUT INSERTED.Id
                VALUES 
                    (@IdSistema,
                    @NombreRemitente,
                    @Correo,    
                    @ClaveSMTP, 
                    @ServidorSMTP, 
                    @PuertoSMTP, 
                    @SSLHabilitado,
                    @CuotaDiaria,
                    @Prioridad, 
                    @Estado,    
                    @FechaRegistro)";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdSistema", credencial.IdSistema);
                    query.Parameters.AddWithValue("@NombreRemitente", credencial.NombreRemitente);
                    query.Parameters.AddWithValue("@Correo", credencial.Correo);
                    query.Parameters.AddWithValue("@ClaveSMTP", credencial.ClaveSMTP);
                    query.Parameters.AddWithValue("@ServidorSMTP", credencial.ServidorSMTP);
                    query.Parameters.AddWithValue("@PuertoSMTP", credencial.PuertoSMTP);
                    query.Parameters.AddWithValue("@SSLHabilitado", credencial.SSLHabilitado);
                    query.Parameters.AddWithValue("@CuotaDiaria", credencial.CuotaDiaria);
                    query.Parameters.AddWithValue("@Prioridad", credencial.Prioridad);
                    query.Parameters.AddWithValue("@Estado", credencial.Estado);
                    query.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }
            return idInsertado;
        }

        public int ActualizarCredencialCorreo(CredencialCorreo credencial) {
            int idActualizado;
            string consulta = @"
                UPDATE CredencialCorreo
                SET IdSistema = @IdSistema,
                    NombreRemitente = @NombreRemitente,
                    Correo = @Correo,
                    ClaveSMTP = @ClaveSMTP,
                    ServidorSMTP = @ServidorSMTP,
                    PuertoSMTP = @PuertoSMTP,
                    SSLHabilitado = @SSLHabilitado,
                    CuotaDiaria = @CuotaDiaria,
                    Prioridad = @Prioridad,
                    Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", credencial.Id);
                    query.Parameters.AddWithValue("@IdSistema", credencial.IdSistema);
                    query.Parameters.AddWithValue("@NombreRemitente", credencial.NombreRemitente);
                    query.Parameters.AddWithValue("@Correo", credencial.Correo);
                    query.Parameters.AddWithValue("@ClaveSMTP", credencial.ClaveSMTP);
                    query.Parameters.AddWithValue("@ServidorSMTP", credencial.ServidorSMTP);
                    query.Parameters.AddWithValue("@PuertoSMTP", credencial.PuertoSMTP);
                    query.Parameters.AddWithValue("@SSLHabilitado", credencial.SSLHabilitado);
                    query.Parameters.AddWithValue("@CuotaDiaria", credencial.CuotaDiaria);
                    query.Parameters.AddWithValue("@Prioridad", credencial.Prioridad);
                    query.Parameters.AddWithValue("@Estado", credencial.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public List<CredencialCorreoDto> ObtenerCredencialesCorreo() {
            List<CredencialCorreoDto> items = new List<CredencialCorreoDto>();

            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.NombreRemitente,
                       c.Correo,
                       c.ClaveSMTP, 
                       c.ServidorSMTP,
                       c.PuertoSMTP,
                       c.SSLHabilitado, 
                       c.CuotaDiaria,
                       c.Prioridad, 
                       c.Estado, 
                       c.FechaRegistro, 
                       c.FechaModificacion
                FROM CredencialCorreo c
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
        public List<CredencialCorreoDto> ObtenerCredencialesCorreoPorSistema(int IdSistema) {
            List<CredencialCorreoDto> items = new List<CredencialCorreoDto>();

            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.NombreRemitente,
                       c.Correo,
                       c.ClaveSMTP, 
                       c.ServidorSMTP,
                       c.PuertoSMTP,
                       c.SSLHabilitado,
                       c.CuotaDiaria,
                       c.Prioridad,
                       c.Estado, 
                       c.FechaRegistro,
                       c.FechaModificacion
                FROM CredencialCorreo c
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

        public CredencialCorreoDto ObtenerCredencialCorreoPorId(int id) {
            CredencialCorreoDto item = new CredencialCorreoDto();

            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.NombreRemitente,
                       c.Correo,
                       c.ClaveSMTP, 
                       c.ServidorSMTP,
                       c.PuertoSMTP,
                       c.SSLHabilitado,
                       c.CuotaDiaria,
                       c.Prioridad,
                       c.Estado, 
                       c.FechaRegistro,
                       c.FechaModificacion
                FROM CredencialCorreo c
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

        public CredencialCorreoDto ObtenerCredencialCorreoPorCorreo(string correo) {
            CredencialCorreoDto item = new CredencialCorreoDto();

            string consulta = @"
                SELECT c.Id,
                       c.IdSistema,
                       s.Nombre AS NombreSistema,
                       c.NombreRemitente,
                       c.Correo,
                       c.ClaveSMTP, 
                       c.ServidorSMTP,
                       c.PuertoSMTP,
                       c.SSLHabilitado,
                       c.CuotaDiaria,
                       c.Prioridad,
                       c.Estado, 
                       c.FechaRegistro,
                       c.FechaModificacion
                FROM CredencialCorreo c
                INNER JOIN Sistema s ON c.IdSistema = s.Id
                WHERE c.Correo = @Correo";

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

        public int EliminarCredencialCorreo(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM CredencialCorreo 
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

        private CredencialCorreoDto ConstruirObjeto(SqlDataReader dr) {
            return new CredencialCorreoDto {
                Id = ManejoNulos.ManageNullInteger(dr["ID"]),
                IdSistema = ManejoNulos.ManageNullInteger(dr["IdSistema"]),
                NombreSistema = ManejoNulos.ManageNullStr(dr["NombreSistema"]),
                NombreRemitente = ManejoNulos.ManageNullStr(dr["NombreRemitente"]),
                Correo = ManejoNulos.ManageNullStr(dr["Correo"]),
                ClaveSMTP = ManejoNulos.ManageNullStr(dr["ClaveSMTP"]),
                ServidorSMTP = ManejoNulos.ManageNullStr(dr["ServidorSMTP"]),
                PuertoSMTP = ManejoNulos.ManageNullInteger(dr["PuertoSMTP"]),
                SSLHabilitado = ManejoNulos.ManegeNullBool(dr["SSLHabilitado"]),
                CuotaDiaria = ManejoNulos.ManageNullInteger(dr["CuotaDiaria"]),
                Prioridad = ManejoNulos.ManageNullInteger(dr["Prioridad"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
