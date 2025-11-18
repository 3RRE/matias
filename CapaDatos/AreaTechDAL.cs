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
    public class AreaTechDAL
    {
        string _conexion = string.Empty;
        public AreaTechDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<AreaTechEntidad> AreaTechListarJson()
        {
            List<AreaTechEntidad> lista = new List<AreaTechEntidad>();
            string consulta = @"SELECT [AreaTechId]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[Estado]
                              FROM [dbo].[AreaTech]";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var areatech = new AreaTechEntidad
                            {
                                AreaTechId = ManejoNulos.ManageNullInteger(dr["AreaTechId"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"])
                            };
                            lista.Add(areatech);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }
    }
}
