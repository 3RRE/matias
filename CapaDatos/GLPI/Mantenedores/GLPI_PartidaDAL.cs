using CapaEntidad.GLPI.Enum;
using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_PartidaDAL {
        private readonly string _conexion;

        public GLPI_PartidaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarPartida(GLPI_Partida partida) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_Partida
                SET 
                    Codigo = @Codigo,
                    Nombre = @Nombre,
	                TipoGasto = @TipoGasto,
	               -- Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", partida.Id);
                    query.Parameters.AddWithValue("@Codigo", partida.Codigo);
                    query.Parameters.AddWithValue("@Nombre", partida.Nombre);
                    query.Parameters.AddWithValue("@TipoGasto", partida.TipoGasto);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarPartida(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_Partida
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

        public int InsertarPartida(GLPI_Partida partida) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_Partida(Codigo, Nombre, TipoGasto, Estado)
                OUTPUT INSERTED.Id
                VALUES (@Codigo, @Nombre, @TipoGasto, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Codigo", partida.Codigo);
                    query.Parameters.AddWithValue("@Nombre", partida.Nombre);
                    query.Parameters.AddWithValue("@TipoGasto", partida.TipoGasto);
                    query.Parameters.AddWithValue("@Estado", partida.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_Partida ObtenerPartidaPorId(int id) {
            GLPI_Partida item = new GLPI_Partida();
            string consulta = @"
                SELECT 
	                Id,
	                Codigo,
	                Nombre,
	                TipoGasto,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Partida
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

        public List<GLPI_Partida> ObtenerPartidas() {
            List<GLPI_Partida> items = new List<GLPI_Partida>();
            string consulta = @"
                SELECT 
	                Id,
	                Codigo,
	                Nombre,
	                TipoGasto,
	                Estado,
                    FechaRegistro,
                    FechaModificacion
                FROM
	                GLPI_Partida
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

        private GLPI_Partida ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_Partida {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Codigo = ManejoNulos.ManageNullStr(dr["Codigo"]),
                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                TipoGasto = (GLPI_TipoGasto)ManejoNulos.ManageNullInteger(dr["TipoGasto"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
            };
        }
    }
}
