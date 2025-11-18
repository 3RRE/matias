using CapaEntidad;
using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Campaña
{
    public class CMP_impresoraDAL
    {
        string _conexion = string.Empty;

        public CMP_impresoraDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CMP_impresoraEntidad> GetListado()
        {
            List<CMP_impresoraEntidad> lista = new List<CMP_impresoraEntidad>();
            string consulta = @"SELECT i.[id]
                                  ,i.[sala_id]
                                    ,s.[nombre] sala_nombre
                                  ,i.[nombre]
                                  ,i.[ip]
                                  ,i.[puerto]
                                  ,i.[estado]
                              FROM [dbo].[CMP_impresora] i
                            join Sala s on s.CodSala=i.sala_id
                             order by i.id desc";
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
                            var impresora = new CMP_impresoraEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger64(dr["sala_id"]),
                                sala_nombre = ManejoNulos.ManageNullStr(dr["sala_nombre"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                ip = ManejoNulos.ManageNullStr(dr["ip"]),
                                puerto = ManejoNulos.ManageNullStr(dr["puerto"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),                          
                            };
                            lista.Add(impresora);
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

        public List<CMP_impresoraEntidad> GetListadoxSala_id(Int64 sala_id)
        {
            List<CMP_impresoraEntidad> lista = new List<CMP_impresoraEntidad>();
            string consulta = @"SELECT i.[id]
                                  ,i.[sala_id]
                                    ,s.[nombre] sala_nombre
                                  ,i.[nombre]
                                  ,i.[ip]
                                  ,i.[puerto]
                                  ,i.[estado]
                              FROM [dbo].[CMP_impresora] i
                            join Sala s on s.CodSala=i.sala_id
                            where i.sala_id=@p0
                             order by i.id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sala_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var impresora = new CMP_impresoraEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger64(dr["sala_id"]),
                                sala_nombre = ManejoNulos.ManageNullStr(dr["sala_nombre"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                ip = ManejoNulos.ManageNullStr(dr["ip"]),
                                puerto = ManejoNulos.ManageNullStr(dr["puerto"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                            };
                            lista.Add(impresora);
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

        public List<CMP_impresoraEntidad> GetListadoxSala_idxUsuarioid(Int64 sala_id,Int64 usuario_id)
        {
            List<CMP_impresoraEntidad> lista = new List<CMP_impresoraEntidad>();
            string consulta = @"SELECT i.[id]
                                  ,i.[sala_id]
                                    ,s.[nombre] sala_nombre
                                  ,i.[nombre]
                                  ,i.[ip]
                                  ,i.[puerto]
                                  ,i.[estado]
                              FROM [dbo].[CMP_impresora] i
                            left join Sala s on s.CodSala=i.sala_id
                            left join CMP_impresora_usuario impu on impu.impresora_id=i.id
                            where i.sala_id=@p0 and impu.usuario_id=@p1
                             order by i.id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sala_id);
                    query.Parameters.AddWithValue("@p1", usuario_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var impresora = new CMP_impresoraEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger64(dr["sala_id"]),
                                sala_nombre = ManejoNulos.ManageNullStr(dr["sala_nombre"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                ip = ManejoNulos.ManageNullStr(dr["ip"]),
                                puerto = ManejoNulos.ManageNullStr(dr["puerto"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                            };
                            lista.Add(impresora);
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

        public CMP_impresoraEntidad GetImpresoraID(Int64 id)
        {
            CMP_impresoraEntidad campaña = new CMP_impresoraEntidad();
            string consulta = @"SELECT [id]
                                  ,[sala_id]
                                  ,[nombre]
                                  ,[ip]
                                  ,[puerto]
                                  ,[estado]
                              FROM [dbo].[CMP_impresora] where id=@p0";
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
                                campaña.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                campaña.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]);
                                campaña.nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
                                campaña.ip = ManejoNulos.ManageNullStr(dr["ip"]);
                                campaña.puerto = ManejoNulos.ManageNullStr(dr["puerto"]);
                                campaña.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return campaña;
        }

        public Int64 GuardarImpresora(CMP_impresoraEntidad impresora)
        {
            //bool respuesta = false;
            Int64 IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_impresora]
           ([sala_id]
           ,[nombre]
           ,[ip]
           ,[puerto]
           ,[estado])
Output Inserted.id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
          );";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(impresora.sala_id) == 0 ? SqlInt64.Null : impresora.sala_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(impresora.nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(impresora.ip));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(impresora.puerto));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(impresora.estado));
                    IdInsertado = Convert.ToInt64(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarImpresora(CMP_impresoraEntidad impresora)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_impresora]
                   SET [sala_id] = @p0
                      ,[nombre] = @p1
                      ,[ip] = @p2   
                        ,[puerto] = @p3
                      ,[estado] = @p4   
                 WHERE id=@p5                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(impresora.sala_id) == 0 ? SqlInt64.Null : impresora.sala_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(impresora.nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(impresora.ip));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(impresora.puerto));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(impresora.estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger64(impresora.id));
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
        public bool EliminarImpresora(Int64 id)
        {
            bool response = false;
            string consulta = @"Delete from [CMP_impresora]
                 WHERE id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    query.ExecuteNonQuery();
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
