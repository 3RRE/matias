using CapaEntidad.Ocurrencias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Ocurrencias
{
    public class OCU_TipoOcurrenciaDAL
    {
        string _conexion = string.Empty;
        public OCU_TipoOcurrenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<OCU_TipoOcurrenciaEntidad> GetListadoTipoOcurrencia()
        {
            List<OCU_TipoOcurrenciaEntidad> lista = new List<OCU_TipoOcurrenciaEntidad>();
            string consulta = @"SELECT [Id]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[Estado]
                              FROM [dbo].[OCU_TipoOcurrencia]";
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
                            var cliente = new OCU_TipoOcurrenciaEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(cliente);
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
        public int GuardarTipoOcurrencia(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[OCU_TipoOcurrencia]
                                   ([Nombre]
                                   ,[Descripcion]
                                   ,[Estado])
                                    Output Inserted.Id
                             VALUES
                                   (@p0
                                   ,@p1
                                   ,@p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoOcurrencia.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoOcurrencia.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(tipoOcurrencia.Estado));
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
        public bool EditarTipoOcurrencia(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[OCU_TipoOcurrencia]
                           SET [Nombre] = @p0
                              ,[Descripcion] = @p1
                              ,[Estado] = @p2
                         WHERE [Id]=@p3";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoOcurrencia.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoOcurrencia.Descripcion));
                    query.Parameters.AddWithValue("@p2", tipoOcurrencia.Estado);
                    query.Parameters.AddWithValue("@p3", tipoOcurrencia.Id);
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
        public bool EditarEstadoTipoOcurrencia(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[OCU_TipoOcurrencia]
                           SET [Estado] = @p0
                         WHERE [Id]=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoOcurrencia.Estado);
                    query.Parameters.AddWithValue("@p1", tipoOcurrencia.Id);
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
        public OCU_TipoOcurrenciaEntidad GetTipoOcurrenciaID(int TipoOcurrenciaId)
        {
            OCU_TipoOcurrenciaEntidad tipoOcurrencia= new OCU_TipoOcurrenciaEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[Estado]
                              FROM [dbo].[OCU_TipoOcurrencia]
                             where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", TipoOcurrenciaId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tipoOcurrencia.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                tipoOcurrencia.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                tipoOcurrencia.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                tipoOcurrencia.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tipoOcurrencia;
        }
    }
}
