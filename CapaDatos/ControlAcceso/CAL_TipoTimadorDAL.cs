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
    public class CAL_TipoTimadorDAL
    {
        string conexion = string.Empty;
        public CAL_TipoTimadorDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_TipoTimadorEntidad> GetAllTipoTimador()
        {
            List<CAL_TipoTimadorEntidad> lista = new List<CAL_TipoTimadorEntidad>();
            string consulta = @"SELECT TipoTimadorID
                                      ,Descripcion
                                  FROM CAL_TipoTimador(nolock) ";
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
                            var item = new CAL_TipoTimadorEntidad
                            {
                                TipoTimadorID = ManejoNulos.ManageNullInteger(dr["TipoTimadorID"]),
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
