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
    public class TipoDOIDAL
    {
        string _conexion = string.Empty;
        public TipoDOIDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<TipoDOIEntidad> TipoDocumentoListarJson()
        {
            List<TipoDOIEntidad> lista = new List<TipoDOIEntidad>();
            string consulta = @"SELECT [DOIID]
                                  ,[DESCRIPCION]
                                  ,[Estado]
                              FROM [dbo].[TipoDOI]
                              where Estado=1
	                          order by DOIID Desc";
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
                            var doi = new TipoDOIEntidad
                            {
                                DOIID = ManejoNulos.ManageNullInteger(dr["DOIID"]),
                                DESCRIPCION = ManejoNulos.ManageNullStr(dr["DESCRIPCION"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim())
                            };

                            lista.Add(doi);
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
