using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas
{
    public class MI_SalaCorreosDAL
    {

        string conexion = string.Empty;
        public MI_SalaCorreosDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<MI_SalaCorreosEntidad> GetAllSalaCorreos()
        {
            List<MI_SalaCorreosEntidad> lista = new List<MI_SalaCorreosEntidad>();
            string consulta = @"  SELECT
                                   sac.CodSalaCorreos,
                                   sac.CodSala,
                                   sac.CodUsuario,
                                   sac.CodTipo,
                                   emp.MailJob
                                   FROM MI_SalaCorreos sac
                                   INNER JOIN SEG_Usuario usu ON usu.UsuarioID=sac.CodUsuario
                                   INNER JOIN SEG_Empleado emp ON emp.EmpleadoID= usu.EmpleadoID";
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
                            var item = new MI_SalaCorreosEntidad
                            {
                                CodSalaCorreos = ManejoNulos.ManageNullInteger(dr["CodSalaCorreos"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodUsuario = ManejoNulos.ManageNullInteger(dr["CodUsuario"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                Mail = ManejoNulos.ManageNullStr(dr["MailJob"]),
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
        public List<MI_SalaCorreosEntidad> GetCorreosxSala(int codSala)
        {
            List<MI_SalaCorreosEntidad> lista = new List<MI_SalaCorreosEntidad>();
            string consulta = @"  SELECT
                                   sac.CodSalaCorreos,
                                   sac.CodSala,
                                   sac.CodUsuario,
                                   sac.CodTipo,
                                   emp.MailJob,
                                   usu.UsuarioNombre
                                   FROM MI_SalaCorreos sac
                                   INNER JOIN SEG_Usuario usu ON usu.UsuarioID=sac.CodUsuario
                                   INNER JOIN SEG_Empleado emp ON emp.EmpleadoID= usu.EmpleadoID WHERE sac.CodSala=@pCodSala";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(codSala));

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_SalaCorreosEntidad
                            {
                                CodSalaCorreos = ManejoNulos.ManageNullInteger(dr["CodSalaCorreos"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodUsuario = ManejoNulos.ManageNullInteger(dr["CodUsuario"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                Mail = ManejoNulos.ManageNullStr(dr["MailJob"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"])
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

        public bool AgregarCorreoSala(int codSala,int codUsuario, int codTipo)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [MI_SalaCorreos]   ([CodSala],[CodUsuario],[CodTipo])
                                VALUES (@pCodSala,@pCodUsuario,@pCodTipo)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(codSala));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullInteger(codUsuario));
                    query.Parameters.AddWithValue("@pCodTipo", ManejoNulos.ManageNullInteger(codTipo));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool QuitarCorreoSala(int codSalaCorreos)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM MI_SalaCorreos WHERE CodSalaCorreos=@pCodSalaCorreos";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSalaCorreos", ManejoNulos.ManageNullInteger(codSalaCorreos));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }



        public List<MI_SalaCorreosEntidad> GetAllUsuarioCorreos()
        {
            List<MI_SalaCorreosEntidad> lista = new List<MI_SalaCorreosEntidad>();
            string consulta = @"    SELECT usu.UsuarioID, usu.UsuarioNombre, emp.MailJob FROM SEG_Usuario usu
                                    INNER JOIN SEG_Empleado emp ON emp.EmpleadoID = usu.EmpleadoID WHERE usu.Estado=1";
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
                            var item = new MI_SalaCorreosEntidad
                            {
                                CodUsuario = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                Mail = ManejoNulos.ManageNullStr(dr["MailJob"]),
                            };

                            item.NombreMail = item.Mail + " ("+item.UsuarioNombre+")";

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
