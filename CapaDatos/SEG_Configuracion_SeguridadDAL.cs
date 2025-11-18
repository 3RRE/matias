using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class SEG_Configuracion_SeguridadDAL
    {
        string _conexion = string.Empty;
        public SEG_Configuracion_SeguridadDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarConfiguracionSeguridad(SEG_Configuracion_SeguridadEntidad cs)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_Configuracion_Seguridad]
           (linkInterno,linkExterno,cantidadLetraNombre,cantidadLetraApePaterno,cantidadLetraApeMaterno,cantidadLetraDNI,ordenNombre,ordenApePaterno,ordenApeMaterno,ordenDNI,mensajeEmail)
VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)";

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

        public bool ActualizarConfiguracionSeguridad(SEG_Configuracion_SeguridadEntidad cs)
        {
            bool respuesta = false;
            string consulta = @"update [dbo].[SEG_Configuracion_Seguridad] set linkInterno=@p0,linkExterno=@p1,cantidadLetraNombre=@p2,cantidadLetraApePaterno=@p3,cantidadLetraApeMaterno=@p4,cantidadLetraDNI=@p5,ordenNombre=@p6,ordenApePaterno=@p7,ordenApeMaterno=@p8,ordenDNI=@p9,mensajeEmail=@p10
where codWebConfiguracionSeguridad=@p11";

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
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cs.codWebConfiguracionSeguridad));
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
        public SEG_Configuracion_SeguridadEntidad ConfiguracionSeguridadObtenerJson()
        {
            SEG_Configuracion_SeguridadEntidad Entity = null;
            string consulta = @"select top 1 * from SEG_Configuracion_Seguridad";
            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                var query = new SqlCommand(consulta, con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Entity = new SEG_Configuracion_SeguridadEntidad();
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
