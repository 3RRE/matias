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
    public class AST_TipoJuegoDAL
    {
        string _conexion = string.Empty;
        public AST_TipoJuegoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<AST_TipoJuegoEntidad> GetListadoTipoJuego()
        {
            List<AST_TipoJuegoEntidad> lista = new List<AST_TipoJuegoEntidad>();
            string consulta = @"SELECT [Id]
                              ,[Nombre],[Descripcion],[Estado]
                          FROM [dbo].[AST_TipoJuego] order by Id desc";
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
                            var tipoJuego = new AST_TipoJuegoEntidad
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
        public AST_TipoJuegoEntidad GetTipoJuegoId(int tipoJuegoId)
        {
            AST_TipoJuegoEntidad tipoJuego = new AST_TipoJuegoEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Nombre],[Descripcion],[Estado]
                              FROM [dbo].[AST_TipoJuego] where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoJuegoId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tipoJuego.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                tipoJuego.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                tipoJuego.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                tipoJuego.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tipoJuego.Id = 0;
            }
            return tipoJuego;
        }
        public int GuardarTipoJuego(AST_TipoJuegoEntidad tipoJuegoId)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[AST_TipoJuego]
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
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoJuegoId.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoJuegoId.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoJuegoId.Estado));
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
        public bool EditarTipoJuego(AST_TipoJuegoEntidad tipoJuegoId)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_TipoJuego]
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
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoJuegoId.Nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(tipoJuegoId.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(tipoJuegoId.Estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tipoJuegoId.Id));
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
