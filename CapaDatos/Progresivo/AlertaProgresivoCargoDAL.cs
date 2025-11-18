using CapaEntidad.Alertas;
using CapaEntidad.ContadoresNegativos;
using CapaEntidad.Progresivo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Progresivo
{
    public class AlertaProgresivoCargoDAL
    {
        private readonly string _conexion = string.Empty;

        public AlertaProgresivoCargoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<AlertaProgresivoCargoEntidad> ListarAlertaProgresivoCargo()
        {
            List<AlertaProgresivoCargoEntidad> lista = new List<AlertaProgresivoCargoEntidad>();

            string query = @"
            SELECT 
                proapc.alerta_id,
                proapc.cargo_id,
                proapc.sala_id,
                proapc.fecha_registro
            FROM dbo.PRO_Alerta_ProgresivoCargo proapc
            ORDER BY proapc.alerta_id DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            AlertaProgresivoCargoEntidad alertaCargo = new AlertaProgresivoCargoEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(data["alerta_id"]),
                                CargoId = ManejoNulos.ManageNullInteger(data["cargo_id"]),
                                SalaId = ManejoNulos.ManageNullInteger(data["sala_id"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["fecha_registro"])
                            };

                            lista.Add(alertaCargo);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return lista;
        }

        public int GuardarAlertaProgresivoCargo(AlertaProgresivoCargoEntidad alertaCargo)
        {
            int insertedId = 0;

            string query = @"
            INSERT INTO dbo.PRO_Alerta_ProgresivoCargo
            (
                cargo_id,
                sala_id,
                fecha_registro
            )

            VALUES
            (
                @p1,
                @p2,
                @p3
            )

            SELECT SCOPE_IDENTITY()
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(alertaCargo.CargoId));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(alertaCargo.SalaId));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(alertaCargo.FechaRegistro));

                    insertedId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return insertedId;
        }

        public bool EliminarAlertaProgresivoCargo(int alertaId)
        {
            bool response = false;

            string query = @"
            DELETE FROM dbo.PRO_Alerta_ProgresivoCargo
            WHERE alerta_id = @w1
            ";
            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", alertaId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return response;
        }

        public List<ALT_AlertaDeviceEntidad> ListarAlertaDeviceSala(int salaId)
        {
            List<ALT_AlertaDeviceEntidad> lista = new List<ALT_AlertaDeviceEntidad>();

            string query = @"
            SELECT
	            emdis.emd_id,
                emdis.emd_imei,
	            emdis.emp_id,
	            emdis.emd_firebaseid,
	            empleado.CargoID,
	            alertacargo.sala_id
            FROM dbo.EmpleadoDispositivo emdis
            JOIN SEG_Empleado empleado ON empleado.EmpleadoID = emdis.emp_id
            JOIN PRO_Alerta_ProgresivoCargo alertacargo ON alertacargo.cargo_id = empleado.CargoID
            JOIN SEG_Usuario usuario ON usuario.EmpleadoID = emdis.emp_id
            JOIN UsuarioSala usuariosala ON usuariosala.UsuarioId = usuario.UsuarioID
            WHERE alertacargo.sala_id = @w1 AND usuariosala.SalaId = @w2 AND emdis.emd_firebaseid IS not NUll
            ORDER BY emdis.emd_id DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", salaId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            ALT_AlertaDeviceEntidad alertaDevice = new ALT_AlertaDeviceEntidad
                            {
                                emd_id = ManejoNulos.ManageNullInteger64(data["emd_id"]),
                                emd_imei = ManejoNulos.ManageNullStr(data["emd_imei"]),
                                emp_id = ManejoNulos.ManageNullInteger(data["emp_id"]),
                                id = ManejoNulos.ManageNullStr(data["emd_firebaseid"]),
                                CargoID = ManejoNulos.ManageNullInteger(data["CargoID"]),
                                sala_id = ManejoNulos.ManageNullInteger(data["sala_id"])
                            };

                            lista.Add(alertaDevice);
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return lista;
        }

        public List<string> ListarAlertaCorreosSala(int salaId)
        {
            List<string> emails = new List<string>();

            string query = @"
            SELECT
	            empleado.MailJob AS mail
            FROM SEG_Empleado empleado
            JOIN dbo.PRO_Alerta_ProgresivoCargo progresivocargo ON progresivocargo.cargo_id = empleado.CargoID
            JOIN SEG_Usuario usuario ON usuario.EmpleadoID = empleado.EmpleadoID
            JOIN UsuarioSala usuariosala ON usuariosala.UsuarioId = usuario.UsuarioID
            WHERE progresivocargo.sala_id = @w1 AND usuariosala.SalaId = @w2
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", salaId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            string email = ManejoNulos.ManageNullStr(data["mail"]);

                            emails.Add(email);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return emails;
        }
    }
}
