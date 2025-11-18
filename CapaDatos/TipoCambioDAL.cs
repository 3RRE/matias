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
    public class TipoCambioDAL
    {
        string _conexion = string.Empty;
        public TipoCambioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<TipoCambioEntidad> ListaTipoCambioJson()
        {
            List<TipoCambioEntidad> lista = new List<TipoCambioEntidad>();

            string consulta = @"select t.TipoCambioId,t.Descripcion,t.Estado from TipoCambio t";
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
                            var tipocambio = new TipoCambioEntidad
                            {
                                TipoCambioId = ManejoNulos.ManageNullInteger(dr["TipoCambioId"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(tipocambio);
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
