using CapaEntidad;
using CapaEntidad.Sala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos.Sala
{
    public class SL_ZonaDAL
    {
        private readonly string _conexion = string.Empty;

        public SL_ZonaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int GuardarZona(SL_ZonaEntidad zona)
        {
            int insertedId = 0;

            string query = @"
            INSERT INTO dbo.SL_Zona
            (
                SalaId,
                Nombre,
                FechaRegistro,
                Estado
            )
            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4
            );

            SELECT SCOPE_IDENTITY()
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand commmand = new SqlCommand(query, connection);

                    commmand.Parameters.AddWithValue("@p1", zona.SalaId);
                    commmand.Parameters.AddWithValue("@p2", zona.Nombre);
                    commmand.Parameters.AddWithValue("@p3", DateTime.Now);
                    commmand.Parameters.AddWithValue("@p4", zona.Estado);

                    insertedId = Convert.ToInt32(commmand.ExecuteScalar());
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return insertedId;
        }

        public bool ActualizarZona(SL_ZonaEntidad zona)
        {
            bool response = false;

            string query = @"
            UPDATE dbo.SL_Zona
            SET
                SalaId = @p1,
                Nombre = @p2,
                FechaModificacion = @p3,
                Estado = @p4
            WHERE
                Id = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", zona.SalaId);
                    command.Parameters.AddWithValue("@p2", zona.Nombre);
                    command.Parameters.AddWithValue("@p3", DateTime.Now);
                    command.Parameters.AddWithValue("@p4", zona.Estado);
                    command.Parameters.AddWithValue("@w1", zona.Id);

                    command.ExecuteNonQuery();

                    response = true;
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }

        public bool ActualizarEstadoZona(byte estado, int zonaId)
        {
            bool response = false;

            string query = @"
            UPDATE dbo.SL_Zona
            SET
                Estado = @p1
            WHERE
                Id = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", estado);
                    command.Parameters.AddWithValue("@w1", zonaId);

                    command.ExecuteNonQuery();

                    response = true;
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }

        public SL_ZonaEntidad ObtenerZona(int zonaId)
        {
            SL_ZonaEntidad zona = new SL_ZonaEntidad();

            string query = @"
            SELECT
                Id,
                SalaId,
                Nombre,
                FechaRegistro,
                FechaModificacion,
                Estado
            FROM dbo.SL_Zona
            WHERE Id = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", zonaId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            if (data.Read())
                            {
                                zona.Id = ManejoNulos.ManageNullInteger(data["Id"]);
                                zona.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                                zona.Nombre = ManejoNulos.ManageNullStr(data["Nombre"]);
                                zona.FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]);
                                zona.FechaModificacion = ManejoNulos.ManageNullDate(data["FechaModificacion"]);
                                zona.Estado = (byte) data["Estado"];
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return zona;
        }

        public List<SL_ZonaEntidad> ListarZona(List<int> salaIds)
        {
            List<SL_ZonaEntidad> listaZona = new List<SL_ZonaEntidad>();

            string query = $@"
            SELECT
                zona.Id,
                zona.SalaId,
                zona.Nombre,
                zona.FechaRegistro,
                zona.FechaModificacion,
                zona.Estado,
                sala.Nombre AS NombreSala
            FROM dbo.SL_Zona zona
            INNER JOIN dbo.Sala sala ON sala.CodSala = zona.SalaId
            WHERE zona.SalaId IN ({ string.Join(",", salaIds) })
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                SL_ZonaEntidad zona = new SL_ZonaEntidad
                                {
                                    Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                    SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                    Nombre = ManejoNulos.ManageNullStr(data["Nombre"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(data["FechaModificacion"]),
                                    Estado = (byte) data["Estado"],
                                    SalaNombre = ManejoNulos.ManageNullStr(data["NombreSala"])
                                };

                                listaZona.Add(zona);
                            }
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return listaZona;
        }

        public List<SL_ZonaEntidad> ListarZonasPorSala(int salaId)
        {
            List<SL_ZonaEntidad> listaZona = new List<SL_ZonaEntidad>();

            string query = $@"
            SELECT
                zona.Id,
                zona.SalaId,
                zona.Nombre,
                zona.FechaRegistro,
                zona.FechaModificacion,
                zona.Estado,
                sala.Nombre AS NombreSala
            FROM dbo.SL_Zona zona
            INNER JOIN dbo.Sala sala ON sala.CodSala = zona.SalaId
            WHERE zona.SalaId = @w1 AND zona.Estado = @w2
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", 1);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                SL_ZonaEntidad zona = new SL_ZonaEntidad
                                {
                                    Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                    SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                    Nombre = ManejoNulos.ManageNullStr(data["Nombre"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(data["FechaModificacion"]),
                                    Estado = (byte)data["Estado"],
                                    SalaNombre = ManejoNulos.ManageNullStr(data["NombreSala"])
                                };

                                listaZona.Add(zona);
                            }
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return listaZona;
        }
    }
}
