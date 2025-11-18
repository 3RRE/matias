using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DestinatarioDAL
    {
        string _conexion = string.Empty;
        public DestinatarioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<DestinatarioEntidad> DestinatarioListadoJson()
        {
            List<DestinatarioEntidad> lista = new List<DestinatarioEntidad>();
            string consulta = @"select EmailID,Nombre,Email,estado from Destinatario";
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
                            var item = new DestinatarioEntidad  
                            {
                                EmailID = ManejoNulos.ManageNullInteger(dr["EmailID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Email = ManejoNulos.ManageNullStr(dr["Email"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
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
            finally
            { 
            }
            return lista;
        }
        public List<DestinatarioEntidad> DestinatarioListadoTipoEmailJson(int tipoEmail)
        {
            List<DestinatarioEntidad> lista = new List<DestinatarioEntidad>();
            string consulta = @"select Destinatario.EmailID,Nombre,Email,estado 
from Destinatario
left join Destinatario_Detalle on Destinatario_Detalle.EmailID=Destinatario.EmailID
where Destinatario_Detalle.TipoEmail=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoEmail);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new DestinatarioEntidad
                            {
                                EmailID = ManejoNulos.ManageNullInteger(dr["EmailID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Email = ManejoNulos.ManageNullStr(dr["Email"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
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
            finally
            {
            }
            return lista;
        }        
        public DestinatarioEntidad DestinatarioObtenerJson(int id)
        {
            DestinatarioEntidad destinatario = new DestinatarioEntidad();
            string consulta = @"select EmailID,Nombre,Email,estado from Destinatario where EmailID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                destinatario.EmailID = ManejoNulos.ManageNullInteger(dr["EmailID"]);
                                destinatario.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                destinatario.Email = ManejoNulos.ManageNullStr(dr["Email"]);
                                destinatario.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
            return destinatario;
        }        
        public bool DestinatarioInsertarJson(DestinatarioEntidad destinatario)
        {
            bool response = false;
            string consulta = @"insert into Destinatario(Nombre,Email,estado) values(@p0,@p1,@p2)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", destinatario.Nombre);
                    query.Parameters.AddWithValue("@p1", destinatario.Email);
                    query.Parameters.AddWithValue("@p2", destinatario.estado);
                    query.ExecuteNonQuery();
                    response= true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        public bool DestinatarioEditarJson(DestinatarioEntidad destinatario)
        {
            bool response = false;
            string consulta = @"update Destinatario set Nombre=@p0,Email=@p1,estado=@p2 where EmailID=@p3";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", destinatario.Nombre);
                    query.Parameters.AddWithValue("@p1", destinatario.Email);
                    query.Parameters.AddWithValue("@p2", destinatario.estado);
                    query.Parameters.AddWithValue("@p3", destinatario.EmailID);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public List<DestinatarioEntidad> ListarDestinatariosAsinadosTipo(int tipo)
        {
            List<DestinatarioEntidad> destinatarios = new List<DestinatarioEntidad>();

            string query = @"
            SELECT
	            destinatario.EmailID,
	            destinatario.Nombre,
	            destinatario.Email,
	            destinatario.estado
            FROM dbo.Destinatario destinatario
            INNER JOIN dbo.Destinatario_Detalle detalle ON detalle.EmailID = destinatario.EmailID
            WHERE detalle.Activo = 1 AND destinatario.estado = 1 AND detalle.TipoEmail = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", tipo);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            DestinatarioEntidad destinatario = new DestinatarioEntidad
                            {
                                EmailID = ManejoNulos.ManageNullInteger(data["EmailID"]),
                                Nombre = ManejoNulos.ManageNullStr(data["Nombre"]),
                                Email = ManejoNulos.ManageNullStr(data["Email"]),
                                estado = ManejoNulos.ManageNullInteger(data["estado"])
                            };

                            destinatarios.Add(destinatario);
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return destinatarios;
        }
    }
}
