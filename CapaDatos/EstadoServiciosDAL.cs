using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class EstadoServiciosDAL
    {
        string _conexion = string.Empty;

        public EstadoServiciosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<EstadoServiciosEntidad> GetEstadoServiciosAll()
        {
            List<EstadoServiciosEntidad> lista = new List<EstadoServiciosEntidad>();
            string consulta = @"SELECT est.[Id]
                                      ,est.[CodSala]
                                      ,est.[EstadoWebOnline]
                                      ,est.[EstadoGladconServices]
                                      ,est.[FechaRegistro]
                                      ,sal.Nombre as NombreSala
                                  FROM [EstadoServicios] est
                                  INNER JOIN Sala sal  
                                  ON est.CodSala = sal.CodSala
                                  ORDER BY Id DESC";

            try
            {

                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta,con);

                    using (var dr = query.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            var item = new EstadoServiciosEntidad();

                            item.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                            item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            item.EstadoWebOnline = ManejoNulos.ManegeNullBool(dr["EstadoWebOnline"]);
                            item.EstadoGladconServices = ManejoNulos.ManegeNullBool(dr["EstadoGladconServices"]);
                            item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);

                            lista.Add(item);

                        }
                    }
                }

            }catch(Exception ex){
                Console.WriteLine(ex.Message);
            };

            return lista;

        }


        public List<EstadoServiciosEntidad> GetEstadoServiciosxSala(int codSala)
        {
            List<EstadoServiciosEntidad> lista = new List<EstadoServiciosEntidad>();
            string consulta = @"SELECT est.[Id]
                                      ,est.[CodSala]
                                      ,est.[EstadoWebOnline]
                                      ,est.[EstadoGladconServices]
                                      ,est.[FechaRegistro]
                                      ,sal.Nombre as NombreSala
                                  FROM [EstadoServicios] est
                                  INNER JOIN Sala sal  
                                  ON est.CodSala = sal.CodSala
                                  WHERE est.CodSala = @pCodSala
                                  ORDER BY Id DESC";

            try
            {

                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new EstadoServiciosEntidad();

                            item.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                            item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            item.EstadoWebOnline = ManejoNulos.ManegeNullBool(dr["EstadoWebOnline"]);
                            item.EstadoGladconServices = ManejoNulos.ManegeNullBool(dr["EstadoGladconServices"]);
                            item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);

                            lista.Add(item);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };

            return lista;

        }

    }
}
