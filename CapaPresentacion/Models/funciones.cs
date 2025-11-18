using CapaNegocio.Utilitarios;
using CapaNegocio.Utilitarios.reporte_botones;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CapaPresentacion.Models
{
    public static class funciones
    {
        /// <summary>
        ///CONEXION => BD_SEGURIDAD_PJ2
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static String consulta(string tabla, string query = "")
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartArray();

            if (query.Length == 0)
            {
                query = "SELECT * FROM " + tabla + "";
            }
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        jsonWriter.WriteStartObject();

                        int fields = reader.FieldCount;
                        for (int i = 0; i < fields; i++)
                        {
                            jsonWriter.WritePropertyName(reader.GetName(i));
                            jsonWriter.WriteValue(reader[i]);
                        }

                        jsonWriter.WriteEndObject();


                    }
                    jsonWriter.WriteEndArray();
                }
            }
            return sb.ToString();

        }


        public static void generarexcel(JObject Data)
        {
            var response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=" + Data["nombrearchivo"] + ".xlsx");
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.BinaryWrite(Excel.ExportarExcel(Data));
            response.End();
        }

        public static void generarexcelNuevo(JObject Data)
        {
            var response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=" + Data["nombrearchivo"] + ".xlsx");
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.BinaryWrite(Excel.ExportarExcelNuevo(Data));
            response.End();
        }

        public static void BorrarCookie(string nombrecookie)
        {
            HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies[nombrecookie];
            HttpContext.Current.Response.Cookies.Remove(nombrecookie);
            if (currentUserCookie != null)
            {
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Current.Response.SetCookie(currentUserCookie);

            }
        }



        public static void generarexcel_funcion_botones(JObject Data)
        {
            var response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=" + Data["nombrearchivo"] + ".xlsx");
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.BinaryWrite(Excel_botones.ExportarExcel_funcion_botones(Data));
            response.End();
        }

    }
}