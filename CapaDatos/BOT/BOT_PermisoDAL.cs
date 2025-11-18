using CapaEntidad.BOT.Entities;
using CapaEntidad.BOT.Enum;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class BOT_PermisoDAL {
        private readonly string _conexion;

        public BOT_PermisoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int CrearPermiso(BOT_PermisoEntidad permiso) {
            int idInsertado;
            string consulta = @"
                INSERT INTO BOT_Permiso(IdEmpleado,IdCargo, CodAccion)
                OUTPUT INSERTED.Id
                VALUES (@IdEmpleado, @IdCargo, @CodAccion)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleado", permiso.IdEmpleado > 0 ? (object)permiso.IdEmpleado : DBNull.Value);
                    query.Parameters.AddWithValue("@IdCargo", permiso.IdCargo > 0 ? (object)permiso.IdCargo : DBNull.Value);
                    query.Parameters.AddWithValue("@CodAccion", permiso.CodAccion);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int EliminarPermisoDeCargo(BOT_Acciones codAccion, int idCargo) {
            int idEliminado;
            string consulta = @"
                DELETE FROM BOT_Permiso
                OUTPUT DELETED.Id
                WHERE IdCargo = @IdCargo AND CodAccion = @CodAccion
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCargo", idCargo);
                    query.Parameters.AddWithValue("@CodAccion", codAccion);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public int EliminarPermisoDeEmpleado(BOT_Acciones codAccion, int idEmpleado) {
            int idEliminado;
            string consulta = @"
                DELETE FROM BOT_Permiso
                OUTPUT DELETED.Id
                WHERE IdEmpleado = @IdEmpleado AND CodAccion = @CodAccion
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    query.Parameters.AddWithValue("@CodAccion", codAccion);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public BOT_PermisoEntidad ObtenerPermisosPorCodAccionYIdCargo(BOT_Acciones codAccion, int idCargo) {
            BOT_PermisoEntidad item = new BOT_PermisoEntidad();
            string consulta = @"
                SELECT
                    Id,
                    IdEmpleado,
                    IdCargo,
                    CodAccion, 
                    Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM BOT_Permiso
                WHERE IdCargo = @IdCargo AND CodAccion = @CodAccion AND Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCargo", idCargo);
                    query.Parameters.AddWithValue("@CodAccion", codAccion);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = MapearPermiso(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosDeCargosPorCodAccion(BOT_Acciones codAccion) {
            List<BOT_PermisoEntidad> items = new List<BOT_PermisoEntidad>();
            string consulta = @"
                SELECT
                    Id,
                    IdEmpleado,
                    IdCargo,
                    CodAccion, 
                    Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM BOT_Permiso
                WHERE CodAccion = @CodAccion AND IdCargo IS NOT NULL AND IdEmpleado IS NULL AND Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodAccion", codAccion);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearPermiso(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public BOT_PermisoEntidad ObtenerPermisosPorCodAccionYIdEmpleado(BOT_Acciones codAccion, int idEmpleado) {
            BOT_PermisoEntidad item = new BOT_PermisoEntidad();
            string consulta = @"
                SELECT
                    Id,
                    IdEmpleado,
                    IdCargo,
                    CodAccion, 
                    Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM BOT_Permiso
                WHERE IdEmpleado = @IdEmpleado AND CodAccion = @CodAccion AND Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    query.Parameters.AddWithValue("@CodAccion", codAccion);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = MapearPermiso(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosDeEmpleadosPorCodAccion(BOT_Acciones codAccion) {
            List<BOT_PermisoEntidad> items = new List<BOT_PermisoEntidad>();
            string consulta = @"
                SELECT
                    Id,
                    IdEmpleado,
                    IdCargo,
                    CodAccion, 
                    Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM BOT_Permiso
                WHERE CodAccion = @CodAccion AND IdCargo IS NULL AND IdEmpleado IS NOT NULL AND Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodAccion", codAccion);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearPermiso(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosPorIdCargo(int idCargo) {
            List<BOT_PermisoEntidad> items = new List<BOT_PermisoEntidad>();
            string consulta = @"
                SELECT
                    Id,
                    IdEmpleado,
                    IdCargo,
                    CodAccion, 
                    Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM BOT_Permiso
                WHERE IdCargo = @IdCargo AND Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCargo", idCargo);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearPermiso(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosPorIdEmpleado(int idEmpleado) {
            List<BOT_PermisoEntidad> items = new List<BOT_PermisoEntidad>();
            string consulta = @"
                SELECT
                    Id,
                    IdEmpleado,
                    IdCargo,
                    CodAccion, 
                    Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM BOT_Permiso
                WHERE IdEmpleado = @IdEmpleado AND Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(MapearPermiso(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        #region Helper
        private BOT_PermisoEntidad MapearPermiso(SqlDataReader dr) {
            return new BOT_PermisoEntidad {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                CodAccion = (BOT_Acciones)ManejoNulos.ManageNullInteger(dr["CodAccion"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
        #endregion
    }
}
