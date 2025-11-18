using CapaEntidad.AsistenciaCliente;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.AsistenciaCliente
{
    public class AST_TipoJDAL
    {
        string _conexion = string.Empty;
        public AST_TipoJDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<AST_TipoJEntidad> GetListadoTipoJ()
        {
            List<AST_TipoJEntidad> lista = new List<AST_TipoJEntidad>();
            string consulta = @"SELECT [Id]
                              ,[Nombre],[Descripcion],[Estado]
                          FROM [dbo].[AST_TipoJ] order by Id desc";
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
                            var tipoJuego = new AST_TipoJEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                            };

                            lista.Add(tipoJuego);
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
        public AST_TipoJEntidad GetTipoJId(int tipoJId)
        {
            AST_TipoJEntidad tipoJ = new AST_TipoJEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Nombre],[Descripcion],[Estado]
                              FROM [dbo].[AST_TipoJ] where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoJId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tipoJ.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                tipoJ.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                tipoJ.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                tipoJ.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tipoJ.Id = 0;
            }
            return tipoJ;
        }
        public int GuardarTipoJ(AST_TipoJEntidad tipoJId)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[AST_TipoJ]
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
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoJId.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoJId.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoJId.Estado));
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
        public bool EditarTipoJ(AST_TipoJEntidad tipoJId)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_TipoJ]
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
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoJId.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoJId.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoJId.Estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tipoJId.Id));
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
