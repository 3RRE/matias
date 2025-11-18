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
    public class GC_SistemaDAL
    {
        string _conexion = string.Empty;
        public GC_SistemaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        

        public List<Sistema> SistemaListadoJson()
        {
            List<Sistema> lista = new List<Sistema>();
            string consulta = @"SELECT [SistemaId]
                              ,[Descripcion]
                              ,[Version]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[Estado]
                                FROM [Sistema] sis ";
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
                            var sistema = new Sistema
                            {
                                SistemaId = ManejoNulos.ManageNullInteger(dr["SistemaId"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"].Trim()),
                                Version = ManejoNulos.ManageNullStr(dr["Version"].Trim()),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim()),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"].Trim()),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"].Trim())
                                
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

        public bool EstadoSistemaActualizarJson(int sistemaId, int estado)
        {
            bool respuesta = false;
            string consulta = @"update Sistema set [Estado] = @p0  WHERE SistemaId = @p1";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", estado);
                    query.Parameters.AddWithValue("@p1", sistemaId);
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

        public bool SistemaEditarJson(Sistema sistema)
        {
            bool respuesta = false;
            string consulta = @"update Sistema set [Descripcion] = @p1
                                  ,[Version] = @p2
                                  ,[FechaModificacion] = @p3
                                    WHERE SistemaId = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sistema.SistemaId);
                    query.Parameters.AddWithValue("@p1", sistema.Descripcion);
                    query.Parameters.AddWithValue("@p2", sistema.Version);
                    query.Parameters.AddWithValue("@p3", sistema.FechaModificacion);
                    
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

        public Sistema GestionCambiosSistemaEditarJson(int sistemaId)
        {
            Sistema gcSistema = new Sistema();
            string consulta = @"select [SistemaId]
                              ,[Descripcion]
                              ,[Version]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[Estado]
                              from  [dbo].[Sistema] sis
	                       where SistemaId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sistemaId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                gcSistema.SistemaId = ManejoNulos.ManageNullInteger(dr["SistemaId"]);
                                gcSistema.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"].Trim());
                                gcSistema.Version = ManejoNulos.ManageNullStr(dr["Version"].Trim());
                                gcSistema.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim());
                                gcSistema.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"].Trim());
                                gcSistema.Estado = ManejoNulos.ManegeNullBool(dr["Estado"].Trim());

                             
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return gcSistema;
        }

        public bool SistemaGuardarJson(Sistema sistema)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[Sistema]
           ([Descripcion],[Version],[FechaRegistro],[FechaModificacion],[Estado])
                VALUES(@p0,@p1,@p2,@p3,@p4)";
            
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(sistema.Descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(sistema.Version));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(sistema.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(sistema.FechaModificacion));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(sistema.Estado));
                    
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
        /*public bool UsuarioActualizarJson(SEG_UsuarioEntidad segUsuario)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Usuario]
                        SET 
                        EmpleadoID = @p1
                       ,UsuarioNombre = @p2
                       ,[UsuarioContraseña] = @p3
                       ,FailedAttempts=@p4
                       WHERE usuarioID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", segUsuario.UsuarioID);
                    query.Parameters.AddWithValue("@p1", segUsuario.EmpleadoID);
                    query.Parameters.AddWithValue("@p2", segUsuario.UsuarioNombre);
                    query.Parameters.AddWithValue("@p3", segUsuario.UsuarioContraseña);
                    query.Parameters.AddWithValue("@p4", segUsuario.FailedAttempts);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }*/
    }
}
