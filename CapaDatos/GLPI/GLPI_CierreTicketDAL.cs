using CapaEntidad.GLPI;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI {
    public class GLPI_CierreTicketDAL {
        private readonly string _conexion;

        public GLPI_CierreTicketDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int InsertarCierreTicket(GLPI_CierreTicket cierreTicket) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_CierreTicket(IdTicket, IdUsuarioCierra, IdEstadoTicketAnterior , IdEstadoTicketActual, Descripcion, IdUsuarioConfirma)
                OUTPUT INSERTED.Id
                VALUES (@IdTicket, @IdUsuarioCierra, @IdEstadoTicketAnterior, @IdEstadoTicketActual, @Descripcion, @IdUsuarioConfirma)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", cierreTicket.IdTicket);
                    query.Parameters.AddWithValue("@IdUsuarioCierra", cierreTicket.IdUsuarioCierra);
                    query.Parameters.AddWithValue("@IdEstadoTicketAnterior", cierreTicket.IdEstadoTicketAnterior);
                    query.Parameters.AddWithValue("@IdEstadoTicketActual", cierreTicket.IdEstadoTicketActual);
                    query.Parameters.AddWithValue("@Descripcion", cierreTicket.Descripcion);
                    query.Parameters.AddWithValue("@IdUsuarioConfirma", cierreTicket.IdUsuarioConfirma);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int ConfirmarCierreTicket(int idUsuario, int idTicket) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_CierreTicket
                SET IdUsuarioConfirma = @IdUsuarioConfirma
                OUTPUT INSERTED.Id
                WHERE IdTicket = @IdTicket
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdUsuarioConfirma", idUsuario);
                    query.Parameters.AddWithValue("@IdTicket", idTicket);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }

            return idActualizado;
        }

        public bool TicketEstaCerrado(int idTicket) {
            int cantidadRegistros = 0;
            string consulta = @"
                SELECT COUNT(Id)
                FROM GLPI_CierreTicket
                WHERE IdTicket = @IdTicket
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", idTicket);
                    cantidadRegistros = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }
            return cantidadRegistros >= 1;
        }
    }
}
