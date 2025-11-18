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
    public class NivelTecnicoDAL
    {
        string _conexion = string.Empty;
        public NivelTecnicoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<NivelTecnicoEntidad> NivelTecnicoListarJson()
        {
            List<NivelTecnicoEntidad> lista = new List<NivelTecnicoEntidad>();
            string consulta = @"SELECT [NivelTecnicoId]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[Estado]
                              FROM [dbo].[NivelTecnico]";
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
                            var NivelTecnico = new NivelTecnicoEntidad
                            {
                                NivelTecnicoId = ManejoNulos.ManageNullInteger(dr["NivelTecnicoId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"])
                            };
                            lista.Add(NivelTecnico);
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
