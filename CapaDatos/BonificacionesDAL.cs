using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos
{
    public class BonificacionesDAL
    {
        string _conexion = string.Empty;
        public BonificacionesDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<BonificacionesEntidad> BonificacionesBuscarJson(string nrodocumento,string nroticket)
        {
            List<BonificacionesEntidad> lista = new List<BonificacionesEntidad>();
            string consulta = @"SELECT [bon_id]
                                      ,[CodSala]
                                      ,[bon_fecha]
                                      ,[bon_documento]
                                      ,[bon_nombre]
                                      ,[bon_apepaterno]
                                      ,[bon_apematerno]
                                      ,[bon_monto]
                                      ,[bon_ticket]
                                      ,[bon_fecharegistro]
                                      ,[UsuarioID]
                                      ,[bon_estado]
                              FROM [dbo].[bonificaciones]
                              where bon_documento=@p1 and  bon_ticket = @p2 and bon_estado=0
	                          order by bon_id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", nrodocumento.Trim());
                    query.Parameters.AddWithValue("@p2", nroticket.Trim());
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new BonificacionesEntidad
                            {
                                bon_id = ManejoNulos.ManageNullInteger64(dr["bon_id"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                bon_fecha = ManejoNulos.ManageNullDate(dr["bon_fecha"]),
                                bon_nombre = ManejoNulos.ManageNullStr(dr["bon_nombre"].Trim()),
                                bon_apepaterno = ManejoNulos.ManageNullStr(dr["bon_apepaterno"].Trim()),
                                bon_apematerno = ManejoNulos.ManageNullStr(dr["bon_apematerno"].Trim()),
                                bon_monto = ManejoNulos.ManageNullFloat(dr["bon_monto"]),
                                bon_ticket = ManejoNulos.ManageNullStr(dr["bon_ticket"]),
                                bon_documento = ManejoNulos.ManageNullStr(dr["bon_documento"].Trim()),
                                bon_fecharegistro = ManejoNulos.ManageNullDate(dr["bon_fecharegistro"]),
                                bon_estado = ManejoNulos.ManageNullInteger(dr["bon_estado"]),
                               
                            };

                            lista.Add(doi);
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

        public List<BonificacionesEntidad> BonificacionesListarJson(DateTime fechaini, DateTime fechafin, string codsala)
        {
            List<BonificacionesEntidad> lista = new List<BonificacionesEntidad>();
            string consulta = @"SELECT b.[bon_id]
                                      ,b.[CodSala]
                                      ,s.Nombre
                                      ,b.[bon_fecha]
                                      ,b.[bon_documento]
                                      ,b.[bon_nombre]
                                      ,b.[bon_apepaterno]
                                      ,b.[bon_apematerno]
                                      ,b.[bon_monto]
                                      ,b.[bon_ticket]
                                      ,b.[bon_fecharegistro]
                                      ,b.[UsuarioID]
                                        ,us.UsuarioNombre
                                      ,b.[bon_estado]
                              FROM [dbo].[bonificaciones] b
                                left join Sala s on s.CodSala=b.CodSala
                                left join SEG_Usuario us on us.UsuarioID=b.UsuarioID
                              where  "+codsala+ "  CONVERT(date, b.bon_fecharegistro) between @p1 and @p2 and bon_estado=1 order by b.bon_fecharegistro desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaini);
                    query.Parameters.AddWithValue("@p2", fechafin);
                    query.Parameters.AddWithValue("@codsala", codsala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new BonificacionesEntidad
                            {
                                bon_id = ManejoNulos.ManageNullInteger(dr["bon_id"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                nombresala = ManejoNulos.ManageNullStr(dr["Nombre"].Trim()),
                                bon_fecha = ManejoNulos.ManageNullDate(dr["bon_fecha"]),
                                bon_nombre = ManejoNulos.ManageNullStr(dr["bon_nombre"].Trim()),
                                bon_apepaterno = ManejoNulos.ManageNullStr(dr["bon_apepaterno"].Trim()),
                                bon_apematerno = ManejoNulos.ManageNullStr(dr["bon_apematerno"].Trim()),
                                bon_monto = ManejoNulos.ManageNullFloat(dr["bon_monto"].Trim()),
                                bon_ticket = ManejoNulos.ManageNullStr(dr["bon_ticket"]),
                                bon_documento = ManejoNulos.ManageNullStr(dr["bon_documento"].Trim()),
                                bon_fecharegistro = ManejoNulos.ManageNullDate(dr["bon_fecharegistro"]),
                                nombreusuario = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim()),
                                bon_estado = ManejoNulos.ManageNullInteger(dr["bon_estado"]),

                            };

                            lista.Add(doi);
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

        public int BonificacionesInsertarJson(BonificacionesEntidad Bonificacion)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[bonificaciones]
                               ([CodSala]
                               ,[bon_fecha]
                               ,[bon_documento]
                               ,[bon_nombre]
                               ,[bon_apepaterno]
                               ,[bon_apematerno]
                               ,[bon_monto]
                               ,[bon_ticket]
                               ,[bon_fecharegistro]
                               ,[UsuarioID]
                               ,[bon_estado]) Output Inserted.bon_id values (@CodSala
                               ,@bon_fecha
                               ,@bon_documento
                               ,@bon_nombre
                               ,@bon_apepaterno
                               ,@bon_apematerno
                               ,@bon_monto
                               ,@bon_ticket
                               ,@bon_fecharegistro
                               ,@UsuarioID
                               ,@bon_estado)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", Bonificacion.CodSala);
                    query.Parameters.AddWithValue("@bon_fecha", Bonificacion.bon_fecha);
                    query.Parameters.AddWithValue("@bon_documento", Bonificacion.bon_documento);
                    query.Parameters.AddWithValue("@bon_nombre", Bonificacion.bon_nombre);
                    query.Parameters.AddWithValue("@bon_apepaterno", Bonificacion.bon_apepaterno);
                    query.Parameters.AddWithValue("@bon_apematerno", Bonificacion.bon_apematerno);
                    query.Parameters.AddWithValue("@bon_monto", Bonificacion.bon_monto);
                    query.Parameters.AddWithValue("@bon_ticket", Bonificacion.bon_ticket);
                    query.Parameters.AddWithValue("@bon_fecharegistro", Bonificacion.bon_fecharegistro);
                    query.Parameters.AddWithValue("@UsuarioID", Bonificacion.UsuarioID);
                    query.Parameters.AddWithValue("@bon_estado", Bonificacion.bon_estado);

                    //query.ExecuteNonQuery();
                    //response = true;
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool BonificacionesActualizarJson(BonificacionesEntidad Bonificacion)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[bonificaciones]
                        SET 
                        CodSala = @p1
                       ,bon_fecharegistro = @p2
                       ,[UsuarioID] = @p3
                       ,bon_estado=@p4
                       WHERE bon_id = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Bonificacion.bon_id);
                    query.Parameters.AddWithValue("@p1", Bonificacion.CodSala);
                    query.Parameters.AddWithValue("@p2", Bonificacion.bon_fecharegistro);
                    query.Parameters.AddWithValue("@p3", Bonificacion.UsuarioID);
                    query.Parameters.AddWithValue("@p4", Bonificacion.bon_estado);
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

    }
}
