using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ConfiguracionSeguridadDAL
    {
        private readonly string _conexion;

        public ConfiguracionSeguridadDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarConfiguracionSeguridad(ConfiguracionSeguridadEntidad cs)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO SEG_Configuracion_Seguridad
           (linkInterno
           ,linkExterno
           ,cantidadLetraNombre
           ,cantidadLetraApePaterno
           ,cantidadLetraApeMaterno
           ,cantidadLetraDNI
           ,ordenNombre
           ,ordenApePaterno
           ,ordenApeMaterno
           ,ordenDNI
           ,mensajeEmail)
     VALUES
           (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cs.linkInterno));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cs.linkExterno));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(cs.cantidadLetraNombre));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(cs.cantidadLetraApePaterno));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(cs.cantidadLetraApeMaterno));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(cs.cantidadLetraDNI));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(cs.ordenNombre));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(cs.ordenApePaterno));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(cs.ordenApeMaterno));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cs.ordenDNI));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(cs.mensajeEmail));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public bool ActualizarConfiguracionSeguridad(ConfiguracionSeguridadEntidad cs)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Configuracion_Seguridad]
                               SET [linkInterno] = @p1
                                  ,[linkExterno] = @p2
                                  ,[cantidadLetraNombre] = @p3
                                  ,[cantidadLetraApePaterno] = @p4
                                  ,[cantidadLetraApeMaterno] = @p5
                                  ,[cantidadLetraDNI] = @p6
                                  ,[ordenNombre] = @p7
                                  ,[ordenApePaterno] = @p8
                                  ,[ordenApeMaterno] = @p9
                                  ,[ordenDNI] = @p10
                                  ,[mensajeEmail] = @p11
                             WHERE codWebConfiguracionSeguridad = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(cs.codWebConfiguracionSeguridad));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cs.linkInterno));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cs.linkExterno));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(cs.cantidadLetraNombre));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(cs.cantidadLetraApePaterno));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(cs.cantidadLetraApeMaterno));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(cs.cantidadLetraDNI));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(cs.ordenNombre));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(cs.ordenApePaterno));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cs.ordenApeMaterno));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cs.ordenDNI));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(cs.mensajeEmail));
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public ConfiguracionSeguridadEntidad ObtenerConfiguracionSeguridad()
        {
            ConfiguracionSeguridadEntidad Entity = null;
            string consulta = @"SELECT [codWebConfiguracionSeguridad]
                              ,linkInterno
                              ,linkExterno
                              ,[cantidadLetraNombre]
                              ,[cantidadLetraApePaterno]
                              ,[cantidadLetraApeMaterno]
                              ,[cantidadLetraDNI]
                              ,[ordenNombre]
                              ,[ordenApePaterno]
                              ,[ordenApeMaterno]
                              ,[ordenDNI]
                              ,mensajeEmail
                          FROM [SEG_Configuracion_Seguridad]";
            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                var query = new SqlCommand(consulta, con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Entity = new ConfiguracionSeguridadEntidad();
                        Entity.linkInterno = ManejoNulos.ManageNullStr(dr["linkInterno"]);
                        Entity.linkExterno = ManejoNulos.ManageNullStr(dr["linkExterno"]);
                        Entity.cantidadLetraNombre = ManejoNulos.ManageNullInteger(dr["cantidadLetraNombre"]);
                        Entity.cantidadLetraApePaterno = ManejoNulos.ManageNullInteger(dr["cantidadLetraApePaterno"]);
                        Entity.cantidadLetraApeMaterno = ManejoNulos.ManageNullInteger(dr["cantidadLetraApeMaterno"]);
                        Entity.cantidadLetraDNI = ManejoNulos.ManageNullInteger(dr["cantidadLetraDNI"]);
                        Entity.ordenNombre = ManejoNulos.ManageNullInteger(dr["ordenNombre"]);
                        Entity.ordenApePaterno = ManejoNulos.ManageNullInteger(dr["ordenApePaterno"]);
                        Entity.ordenApeMaterno = ManejoNulos.ManageNullInteger(dr["ordenApeMaterno"]);
                        Entity.ordenDNI = ManejoNulos.ManageNullInteger(dr["ordenDNI"]);
                        Entity.mensajeEmail = ManejoNulos.ManageNullStr(dr["mensajeEmail"]);
                        Entity.codWebConfiguracionSeguridad = ManejoNulos.ManageNullInteger(dr["codWebConfiguracionSeguridad"]);
                    }
                }
            }
            return Entity;
        }
    }
}
