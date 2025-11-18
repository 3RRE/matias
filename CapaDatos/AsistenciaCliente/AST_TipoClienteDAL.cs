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
    public class AST_TipoClienteDAL
    {
        string _conexion = string.Empty;
        public AST_TipoClienteDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<AST_TipoClienteEntidad> GetListadoTipoCliente()
        {
            List<AST_TipoClienteEntidad> lista = new List<AST_TipoClienteEntidad>();
            string consulta = @"SELECT [Id]
                              ,[Nombre],[Descripcion],[Estado]
                          FROM [dbo].[AST_TipoCliente]";
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
                            var tipoCliente = new AST_TipoClienteEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                            };

                            lista.Add(tipoCliente);
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
        public AST_TipoClienteEntidad GetTipoClienteId(int TipoClienteId)
        {
            AST_TipoClienteEntidad tipocliente = new AST_TipoClienteEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Nombre],[Descripcion],[Estado]
                              FROM [dbo].[AST_TipoCliente] where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", TipoClienteId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tipocliente.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                tipocliente.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                tipocliente.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                tipocliente.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tipocliente.Id = 0;
            }
            return tipocliente;
        }
        public int GuardarTipoCliente(AST_TipoClienteEntidad tipoCliente)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[AST_TipoCliente]
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
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoCliente.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoCliente.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoCliente.Estado));
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
        public bool EditarTipoCliente(AST_TipoClienteEntidad tipoCliente)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_TipoCliente]
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
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoCliente.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoCliente.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoCliente.Estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tipoCliente.Id));
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
