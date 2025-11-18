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
    public class JuegoDAL
    {
        string _conexion_s3k_ = string.Empty;
        public JuegoDAL()
        {
            _conexion_s3k_ = ConfigurationManager.ConnectionStrings["conexion_s3k_administrativo"].ConnectionString;
        }

        public List<JuegoEntidad> ListaJuegoMaquinaJson(int id)
        {
            List<JuegoEntidad> lista = new List<JuegoEntidad>();
            string consulta = @"select * from Juego j
                                where j.CodMarcaMaquina ="+ id;
            try
            {
                using (var con = new SqlConnection(_conexion_s3k_))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var juegoentidad = new JuegoEntidad
                            {
                                CodJuego = ManejoNulos.ManageNullInteger(dr["CodJuego"]),
                                CodMarcaMaquina = ManejoNulos.ManageNullInteger(dr["CodMarcaMaquina"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                JuegosAlternos = ManejoNulos.ManageNullStr(dr["JuegosAlternos"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManageNullInteger(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRd"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"])
                            };

                            lista.Add(juegoentidad);
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
