using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class UsuarioSalaDAL
    {
        string _conexion = string.Empty;
        public UsuarioSalaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<UsuarioSalaEntidad> UsuarioSalasListarIdJson(int usuarioId)
        {
            List<UsuarioSalaEntidad> lista = new List<UsuarioSalaEntidad>();
            string consulta = @"SELECT UsuarioSalaId
      ,SalaId
      ,UsuarioId
      ,FechaRegistro
      ,Estado
  FROM  UsuarioSala (nolock) where UsuarioId = @pUsuarioId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                { 
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pUsuarioId", usuarioId);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new UsuarioSalaEntidad
                            {
                                UsuarioSalaId = ManejoNulos.ManageNullInteger(dr["UsuarioSalaId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Estado   = ManejoNulos.ManegeNullBool(dr["Estado"]), 
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

        public List<UsuarioSalaEntidad> UsuarioSalasListarxsalaidJson(string salaid)
        {
            List<UsuarioSalaEntidad> lista = new List<UsuarioSalaEntidad>();
            string consulta = @"SELECT UsuarioSalaId
      ,SalaId
      ,UsuarioId
      ,FechaRegistro
      ,Estado
  FROM  UsuarioSala (nolock) where SalaId in"+salaid;
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
                            var item = new UsuarioSalaEntidad
                            {
                                UsuarioSalaId = ManejoNulos.ManageNullInteger(dr["UsuarioSalaId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
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

        public UsuarioSalaEntidad UsuarioSalaListarIdJson(int usuarioId, int salaId)
        {
            UsuarioSalaEntidad usuarioSala = new UsuarioSalaEntidad();
            string consulta = @"SELECT UsuarioSalaId
      ,SalaId
      ,UsuarioId
      ,FechaRegistro
      ,Estado
  FROM UsuarioSala (nolock)
  where UsuarioId = @pUsuarioId and SalaId = @pSalaId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pUsuarioId", usuarioId);
                    query.Parameters.AddWithValue("@pSalaId", salaId);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuarioSala.UsuarioSalaId = ManejoNulos.ManageNullInteger(dr["UsuarioSalaId"]);
                                usuarioSala.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                usuarioSala.UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"].Trim());
                                usuarioSala.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim());
                                usuarioSala.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]); 
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return usuarioSala;
        }
        public EmpleadoUsuarioEntidad UsuarioEmpleadoListarIdJson(int usuarioId)
        {
            EmpleadoUsuarioEntidad empleadoUsuario = new EmpleadoUsuarioEntidad();
            string consulta = @"SELECT UsuarioID,TipoUsuarioID      ,UsuarioNombre      ,FailedAttempts,	   e.EmpleadoID      ,Nombres      ,ApellidosPaterno      ,ApellidosMaterno
      ,CargoID      ,Direccion      ,DOIID      ,DOI      ,Telefono      ,Movil      ,Genero      ,MailPersonal      ,MailJob      ,FechaAlta
  FROM SEG_Empleado e (nolock) inner join seg_usuario u (nolock) 
  on e.EmpleadoID = u.EmpleadoID where UsuarioID = @pUsuarioId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pUsuarioId", usuarioId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                empleadoUsuario.UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]);
                                empleadoUsuario.TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"]); 
                                empleadoUsuario.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim());
                                empleadoUsuario.FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"].Trim());
                                empleadoUsuario.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                empleadoUsuario.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                empleadoUsuario.ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim());
                                empleadoUsuario.ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim());
                                empleadoUsuario.CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]);
                                empleadoUsuario.Direccion = ManejoNulos.ManageNullStr(dr["Direccion"].Trim());
                                empleadoUsuario.DOIID = ManejoNulos.ManageNullInteger(dr["DOIID"]);
                                empleadoUsuario.DOI = ManejoNulos.ManageNullStr(dr["DOI"]);
                                empleadoUsuario.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                empleadoUsuario.Movil = ManejoNulos.ManageNullStr(dr["Movil"]);
                                empleadoUsuario.Genero = ManejoNulos.ManageNullStr(dr["Genero"]);
                                empleadoUsuario.MailPersonal = ManejoNulos.ManageNullStr(dr["MailPersonal"]);
                                empleadoUsuario.MailJob = ManejoNulos.ManageNullStr(dr["MailJob"]);
                                empleadoUsuario.FechaAlta = ManejoNulos.ManageNullDate(dr["FechaAlta"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return empleadoUsuario;
        }
        public bool UsuarioSalaInsertarJson(int usuarioId, int salaId)
        {
          
            string consulta = @"INSERT INTO UsuarioSala
           (SalaId           ,UsuarioId           ,FechaRegistro           ,Estado)
     VALUES           (@pSalaId           ,@pUsuarioId           ,getDate()           ,1)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pSalaId", salaId);
                    query.Parameters.AddWithValue("@pUsuarioId", usuarioId); 
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            } 
        }
        public bool UsuarioSalaEliminarJson(int usuarioId, int salaId)
        {

            string consulta = @"delete from UsuarioSala where SalaId = @pSalaId and UsuarioId = @pUsuarioId ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pSalaId", salaId);
                    query.Parameters.AddWithValue("@pUsuarioId", usuarioId);
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
        }

        public bool UsuarioSalaAsignar(int usuarioId, List<int> salaIds)
        {
            bool response = false;

            string query = @"
            INSERT INTO UsuarioSala (SalaId, UsuarioId, FechaRegistro, Estado)
            VALUES (@p1, @p2, GETDATE(), 1)
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    foreach(int salaId in salaIds)
                    {
                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@p1", salaId);
                        command.Parameters.AddWithValue("@p2", usuarioId);
                        command.ExecuteNonQuery();
                    }

                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                response = false;
            }

            return response;
        }

        public bool UsuarioSalaDenegar(int usuarioId, List<int> salaIds)
        {
            bool response = false;

            string query = $@"
            DELETE FROM UsuarioSala
            WHERE SalaId IN ({ string.Join(",", salaIds) })
            AND UsuarioId = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", usuarioId);
                    command.ExecuteNonQuery();

                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                response = false;
            }

            return response;
        }
    }
}
