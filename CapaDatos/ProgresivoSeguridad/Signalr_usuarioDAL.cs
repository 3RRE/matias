using CapaDatos.Utilitarios;
using CapaEntidad.ProgresivoSeguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos.ProgresivoSeguridad
{
    public class Signalr_usuarioDAL
    {
        string _conexion = string.Empty;
        public Signalr_usuarioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarSignalr(Signalr_usuarioEntidad registro)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO Signalr_usuario
           ([usuario_id],[sgn_conection_id],[sgn_fechaUpdate],sgn_estado)
            VALUES(@p0,@p1,@p2,0)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullStr(registro.usuario_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulls.ManageNullStr(registro.sgn_conection_id));
                    query.Parameters.AddWithValue("@p2", DateTime.Now);
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public Int64 GuardarSignalr_returnID(Signalr_usuarioEntidad registro)
        {
            //bool response = false;
            Int64 idInsertado = 0;
            string consulta = @"
            INSERT INTO Signalr_usuario
           ([usuario_id],[sgn_conection_id],[sgn_fechaUpdate],sgn_estado)
            VALUES(@p0,@p1,@p2,0) 
                SELECT SCOPE_IDENTITY()";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullInteger(registro.usuario_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulls.ManageNullStr(registro.sgn_conection_id));
                    query.Parameters.AddWithValue("@p2", DateTime.Now);
                    idInsertado = Int64.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return idInsertado;
        }

        public List<Signalr_usuarioEntidad> GetListaSignalr_usuario()
        {
            List<Signalr_usuarioEntidad> lista = new List<Signalr_usuarioEntidad>();
            string consulta = @"SELECT sg.[sgn_id]
                                  ,sg.[usuario_id]
                                   ,seg.UsuarioNombre
                                    ,rol.WEB_RolNombre RolNombre
                                    ,sala.Nombre SalaNombre
                                    ,sg.sala_id
                                  ,sg.[sgn_conection_id]
                                  ,sg.[sgn_token]
                                  ,sg.[sgn_fechaUpdate]
                                   ,sg.sgn_estado
                              FROM [dbo].[Signalr_usuario] sg
                            left join SEG_usuario seg on seg.UsuarioID=sg.usuario_id
                            left join SEG_RolUsuario segrol on segrol.UsuarioID= seg.UsuarioID
                            left join SEG_rol rol on rol.WEB_RolID=segrol.WEB_RolID
                            left join Sala sala on sala.CodSala = sg.sala_id
                            where sg.sgn_estado=1
                            order by sg.sgn_id Desc";
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
                            var webRol = new Signalr_usuarioEntidad
                            {
                                sgn_id = ManejoNulls.ManageNullInteger64(dr["sgn_id"]),
                                usuario_id = ManejoNulls.ManageNullInteger(dr["usuario_id"]),
                                UsuarioNombre = ManejoNulls.ManageNullStr(dr["UsuarioNombre"].Trim()),
                                RolNombre = ManejoNulls.ManageNullStr(dr["RolNombre"].Trim()),
                                sgn_conection_id = ManejoNulls.ManageNullStr(dr["sgn_conection_id"].Trim()),
                                sgn_token = ManejoNulls.ManageNullStr(dr["sgn_token"].Trim()),
                                sgn_fechaUpdate = ManejoNulls.ManageNullDate(dr["sgn_fechaUpdate"]),
                                sgn_estado = ManejoNulls.ManageNullInteger(dr["sgn_estado"]),
                                sala_id = ManejoNulls.ManageNullInteger(dr["sala_id"]),
                                SalaNombre = ManejoNulls.ManageNullStr(dr["SalaNombre"].Trim())
                            };

                            lista.Add(webRol);
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

       
        public Signalr_usuarioEntidad GetSignalr_usuarioId(Int64 sgn_id)
        {
            Signalr_usuarioEntidad sign = new Signalr_usuarioEntidad();
            string consulta = @"SELECT [sgn_id]
      ,[usuario_id]
      ,[sgn_conection_id]
      ,[sgn_token]
      ,[sgn_fechaUpdate]
        ,sgn_estado
  FROM [dbo].[Signalr_usuario] where sgn_id =@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sgn_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                sign.sgn_id = ManejoNulls.ManageNullInteger64(dr["sgn_id"]);
                                sign.usuario_id = ManejoNulls.ManageNullInteger(dr["usuario_id"]);
                                sign.sgn_conection_id = ManejoNulls.ManageNullStr(dr["sgn_conection_id"].Trim());
                                sign.sgn_token = ManejoNulls.ManageNullStr(dr["sgn_token"].Trim());
                                sign.sgn_fechaUpdate = ManejoNulls.ManageNullDate(dr["sgn_fechaUpdate"].Trim());
                                sign.sgn_estado = ManejoNulls.ManageNullInteger(dr["sgn_estado"].Trim());
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return sign;
        }

        public List<Signalr_usuarioEntidad> GetSignalr_usuarioIdxUsuarioID(Int64 usuario_id)
        {
            List<Signalr_usuarioEntidad> lista = new List<Signalr_usuarioEntidad>();
            string consulta = @"SELECT [sgn_id]
                                  ,[usuario_id]
                                  ,[sgn_conection_id]
                                  ,[sgn_token]
                                  ,[sgn_fechaUpdate]
                                    ,sgn_estado
                              FROM [dbo].[Signalr_usuario] where sgn_id =@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuario_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webRol = new Signalr_usuarioEntidad
                            {
                                sgn_id = ManejoNulls.ManageNullInteger64(dr["sgn_id"]),
                                usuario_id = ManejoNulls.ManageNullInteger(dr["usuario_id"]),
                                sgn_conection_id = ManejoNulls.ManageNullStr(dr["sgn_conection_id"].Trim()),
                                sgn_token = ManejoNulls.ManageNullStr(dr["sgn_token"].Trim()),
                                sgn_fechaUpdate = ManejoNulls.ManageNullDate(dr["sgn_fechaUpdate"]),
                                sgn_estado = ManejoNulls.ManageNullInteger(dr["sgn_estado"]),
                            };
                            lista.Add(webRol);
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

        public bool ActualizarSignalruser(Signalr_usuarioEntidad sign)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [Signalr_usuario]
                            SET [usuario_id] = @p1,
                                [sgn_conection_id] = @p2
                            ,[sgn_token]= @p3 
                            ,[sgn_fechaUpdate] = @p4
                            WHERE sgn_id = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sign.sgn_id);
                    query.Parameters.AddWithValue("@p1", sign.usuario_id);
                    query.Parameters.AddWithValue("@p2", sign.sgn_conection_id);
                    query.Parameters.AddWithValue("@p3", sign.sgn_token);
                    query.Parameters.AddWithValue("@p4", sign.sgn_fechaUpdate);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool ActualizarToken(string token, Int64 sgn_id, DateTime fecha ,int estado)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [Signalr_usuario]
                            SET [sgn_token] = @p1,
                            [sgn_fechaUpdate] = @p2,
                            sgn_estado = @p3
                            WHERE sgn_id = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", token);
                    query.Parameters.AddWithValue("@p2", fecha);
                    query.Parameters.AddWithValue("@p3", estado);
                    query.Parameters.AddWithValue("@p0", sgn_id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return respuesta;
        }
        public bool ActualizarConection_id(string conectid, Int64 usu_id, DateTime fecha)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [Signalr_usuario]
                            SET [sgn_conection_id] = @p1,
                            [sgn_fechaUpdate] = @p2
                            WHERE usuario_id = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", conectid);
                    query.Parameters.AddWithValue("@p2", fecha);
                    query.Parameters.AddWithValue("@p0", usu_id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return respuesta;
        }
        public bool EliminarSignalrid(Int64 sgn_id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM [dbo].[Signalr_usuario] WHERE sgn_id = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sgn_id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public bool ActualizarSalaId(int sala_id, long sgn_id) {
            bool response = false;

            string query = @"UPDATE Signalr_usuario SET sala_id = @p1 WHERE sgn_id = @p0";
            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", sala_id);
                    command.Parameters.AddWithValue("@p0", sgn_id);
                    command.ExecuteNonQuery();

                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }
    }
}
