using CapaEntidad.Settings;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Settings
{
    public class App_SettingsDAL
    {
        private readonly string _conexion = string.Empty;

        public App_SettingsDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<App_SettingsEntidad> ListarAppSettings()
        {
            List<App_SettingsEntidad> list = new List<App_SettingsEntidad>();

            string query = @"
            SELECT
	            apstt.AS_Key,
	            apstt.AS_Value,
	            apstt.AS_Description,
                apstt.AS_CUserId,
                apstt.AS_UUserId,
	            apstt.AS_Created,
	            apstt.AS_Updated
            FROM App_Settings apstt
            ORDER BY apstt.AS_Created DESC
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
                            App_SettingsEntidad appSetting = new App_SettingsEntidad
                            {
                                AS_Key = ManejoNulos.ManageNullStr(data["AS_Key"]),
                                AS_Value = ManejoNulos.ManageNullStr(data["AS_Value"]),
                                AS_Description = ManejoNulos.ManageNullStr(data["AS_Description"]),
                                AS_CUserId = ManejoNulos.ManageNullInteger(data["AS_CUserId"]),
                                AS_UUserId = ManejoNulos.ManageNullInteger(data["AS_UUserId"]),
                                AS_Created = ManejoNulos.ManageNullDate(data["AS_Created"]),
                                AS_Updated = ManejoNulos.ManageNullDate(data["AS_Updated"])
                            };

                            list.Add(appSetting);
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = new List<App_SettingsEntidad>();
            }

            return list;
        }

        public App_SettingsEntidad ObtenerAppSetting(string asKey)
        {
            App_SettingsEntidad appSetting = new App_SettingsEntidad();

            string query = @"
            SELECT
	            apstt.AS_Key,
	            apstt.AS_Value,
	            apstt.AS_Description,
                apstt.AS_CUserId,
                apstt.AS_UUserId,
	            apstt.AS_Created,
	            apstt.AS_Updated
            FROM App_Settings apstt
            WHERE apstt.AS_Key = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", asKey);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            if (data.Read())
                            {
                                appSetting.AS_Key = ManejoNulos.ManageNullStr(data["AS_Key"]);
                                appSetting.AS_Value = ManejoNulos.ManageNullStr(data["AS_Value"]);
                                appSetting.AS_Description = ManejoNulos.ManageNullStr(data["AS_Description"]);
                                appSetting.AS_CUserId = ManejoNulos.ManageNullInteger(data["AS_CUserId"]);
                                appSetting.AS_UUserId = ManejoNulos.ManageNullInteger(data["AS_UUserId"]);
                                appSetting.AS_Created = ManejoNulos.ManageNullDate(data["AS_Created"]);
                                appSetting.AS_Updated = ManejoNulos.ManageNullDate(data["AS_Updated"]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                appSetting = new App_SettingsEntidad();
            }

            return appSetting;
        }

        public string GuardarAppSetting(App_SettingsEntidad appSetting)
        {
            string insertedKey = string.Empty;

            string query = @"
            INSERT INTO App_Settings
            (
                AS_Key,
                AS_Value,
                AS_Description,
                AS_CUserId,
                AS_Created
            )

            OUTPUT INSERTED.AS_Key

            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4,
                GETDATE()
            )
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(appSetting.AS_Key));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(appSetting.AS_Value));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(appSetting.AS_Description));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(appSetting.AS_CUserId));

                    insertedKey = Convert.ToString(command.ExecuteScalar());
                }
            }
            catch (Exception)
            {
                insertedKey = string.Empty;
            }

            return insertedKey;
        }

        public bool ActualizarAppSetting(App_SettingsEntidad appSetting)
        {
            bool updated = false;

            string query = @"
            UPDATE App_Settings
            SET
	            AS_Value = @p1,
	            AS_Description = @p2,
                AS_UUserId = @p3,
	            AS_Updated = GETDATE()
            WHERE
                AS_Key = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", appSetting.AS_Value);
                    command.Parameters.AddWithValue("@p2", appSetting.AS_Description);
                    command.Parameters.AddWithValue("@p3", appSetting.AS_UUserId);
                    command.Parameters.AddWithValue("@w1", appSetting.AS_Key);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        updated = true;
                    }
                }
            }
            catch (Exception)
            {
                updated = false;
            }

            return updated;
        }

        public bool EliminarAppSetting(string asKey)
        {
            bool deleted = false;

            string query = @"
            DELETE FROM App_Settings
            WHERE
                AS_Key = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", asKey);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        deleted = true;
                    }
                }
            }
            catch (Exception)
            {
                deleted = false;
            }

            return deleted;
        }

        public bool ExisteAppSetting(string asKey)
        {
            bool exists = false;

            string query = @"
            SELECT
	            COUNT(apstt.AS_Key) AS Total
            FROM App_Settings apstt
            WHERE
	            apstt.AS_Key = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", asKey);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.Read())
                        {
                            int rows = ManejoNulos.ManageNullInteger(data["Total"]);

                            if(rows > 0)
                            {
                                exists = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                exists = false;
            }

            return exists;
        }
    }
}
