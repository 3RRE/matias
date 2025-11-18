using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.Utilitarios;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_TipoExclusionDAL
    {

        string conexion = string.Empty;
        public CAL_TipoExclusionDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_TipoExclusionEntidad> GetAllTipoExclusion()
        {
            List<CAL_TipoExclusionEntidad> lista = new List<CAL_TipoExclusionEntidad>();
            string consulta = @"SELECT TipoExclusionID
                                      ,Descripcion
                                  FROM CAL_TipoExclusion(nolock) ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new CAL_TipoExclusionEntidad
                            {
                                TipoExclusionID = ManejoNulos.ManageNullInteger(dr["TipoExclusionID"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                            };

                            lista.Add(item);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
    }
}
