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
    public class EstadoSolicitudCambioDAL
    {
        string _conexion = string.Empty;
        public EstadoSolicitudCambioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<EstadoSolicitudCambioEntidad> EstadoSolicitudCambioListadoJson()
        {
            List<EstadoSolicitudCambioEntidad> lista = new List<EstadoSolicitudCambioEntidad>();
            string consulta = @"select e.EstadoSolicitudCambioId,e.Descripcion,e.Estado
                                from EstadoSolicitudCambio e";
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
                            var sistema = new EstadoSolicitudCambioEntidad
                            {
                                EstadoSolicitudCambioId = ManejoNulos.ManageNullInteger(dr["EstadoSolicitudCambioId"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(sistema);
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
