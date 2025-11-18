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
    public class MarcaMaquinaDAL
    {
        string _conexion_s3k_ = string.Empty;
        public MarcaMaquinaDAL()
        {
            _conexion_s3k_ = ConfigurationManager.ConnectionStrings["conexion_s3k_administrativo"].ConnectionString;
        }

        public List<MarcaMaquinaEntidad> MarcaMaquinaListaJson()
        {
            List<MarcaMaquinaEntidad> lista = new List<MarcaMaquinaEntidad>();
            string consulta = @"SELECT [CodMarcaMaquina]
                              ,[Nombre]
                              ,[ColorHexa]
                              ,[Sigla]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[Activo]
                              ,[Estado]
                              ,[CodRD]
                              ,[CodUsuario]
                          FROM [MarcaMaquina]";
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
                            var marcamaquina = new MarcaMaquinaEntidad
                            {
                                //NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                CodMarcaMaquina = ManejoNulos.ManageNullInteger(dr["CodMarcaMaquina"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManageNullInteger(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRd"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"])                                
                            };

                            lista.Add(marcamaquina);
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
