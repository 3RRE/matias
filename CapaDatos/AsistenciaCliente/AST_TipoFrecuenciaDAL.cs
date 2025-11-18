using CapaEntidad.AsistenciaCliente;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.AsistenciaCliente
{
    public class AST_TipoFrecuenciaDAL
    {
        string _conexion = string.Empty;
        public AST_TipoFrecuenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<AST_TipoFrecuenciaEntidad> GetListadoTipoFrecuencia()
        {
            List<AST_TipoFrecuenciaEntidad> lista = new List<AST_TipoFrecuenciaEntidad>();
            string consulta = @"SELECT [Id]
                              ,[Nombre],[Descripcion],[Estado]
                          FROM [dbo].[AST_TipoFrecuencia]";
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
                            var tipoFrecuencia = new AST_TipoFrecuenciaEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                            };

                            lista.Add(tipoFrecuencia);
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
        public AST_TipoFrecuenciaEntidad GetTipoFrecuenciaId(int tipoFrecuenciaId)
        {
            AST_TipoFrecuenciaEntidad tipoFrecuencia = new AST_TipoFrecuenciaEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Nombre],[Descripcion],[Estado]
                              FROM [dbo].[AST_TipoFrecuencia] where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoFrecuenciaId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tipoFrecuencia.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                tipoFrecuencia.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                tipoFrecuencia.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                tipoFrecuencia.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tipoFrecuencia.Id = 0;
            }
            return tipoFrecuencia;
        }
        public int GuardarTipoFrecuencia(AST_TipoFrecuenciaEntidad tipoFrecuencia)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[AST_TipoFrecuencia]
                       ([Nombre],[Descripcion],[Estado])
                        Output Inserted.Id
                        VALUES
                       (@p0,@p1,@p2);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoFrecuencia.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoFrecuencia.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoFrecuencia.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
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
        public bool EditarTipoFrecuencia(AST_TipoFrecuenciaEntidad tipoFrecuencia)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_TipoFrecuencia]
                               SET [Nombre] = @p0
                                  ,[Descripcion] = @p1
                                  ,[Estado] = @p2
                             WHERE Id=@p3";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoFrecuencia.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoFrecuencia.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoFrecuencia.Estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tipoFrecuencia.Id));
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
    }
}
