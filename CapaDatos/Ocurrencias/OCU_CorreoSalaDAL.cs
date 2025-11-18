using CapaEntidad;
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
    public class OCU_CorreoSalaDAL
    {
        string _conexion = string.Empty;
        public OCU_CorreoSalaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<OCU_CorreoSalaEntidad> GetListadoCorreoSala()
        {
            List<OCU_CorreoSalaEntidad> lista = new List<OCU_CorreoSalaEntidad>();
            string consulta = @"SELECT
                                  ,[CorreoId]
                                  ,[SalaId]
                              FROM [dbo].[OCU_CorreoSala]";
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
                            var correoSala = new OCU_CorreoSalaEntidad
                            {
                                CorreoId= ManejoNulos.ManageNullInteger(dr["CorreoId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                            };

                            lista.Add(correoSala);
                        }
                    }
                    foreach (var correoSala in lista)
                    {
                        SetSala(correoSala, con);
                        SetCorreo(correoSala, con);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<OCU_CorreoSalaEntidad> GetListadoCorreoSalaxSala(int SalaId)
        {
            List<OCU_CorreoSalaEntidad> lista = new List<OCU_CorreoSalaEntidad>();
            string consulta = @"SELECT   [CorreoId]
                                  ,[SalaId]
                              FROM [dbo].[OCU_CorreoSala] where SalaId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", SalaId);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var correoSala = new OCU_CorreoSalaEntidad
                            {
                                CorreoId = ManejoNulos.ManageNullInteger(dr["CorreoId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                            };

                            lista.Add(correoSala);
                        }
                    }
                    foreach (var correoSala in lista)
                    {
                        SetSala(correoSala, con);
                        SetCorreo(correoSala, con);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public int GuardarCorreoSalaVarios(string values)
        {
            int IdInsertado = 0;
            string consulta = @"
                                INSERT INTO [dbo].[OCU_CorreoSala]
                                           ([CorreoId]
                                           ,[SalaId])
                                     VALUES
                                           " + values;

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    IdInsertado = query.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EliminarCorreoSalaxCorreoId(int CorreoId)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM [dbo].[OCU_CorreoSala]
                                WHERE CorreoId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CorreoId);
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
        private void SetSala(OCU_CorreoSalaEntidad correoSala, SqlConnection context)
        {
            var command = new SqlCommand(@"SELECT [CodSala]
                                            ,[Nombre]
                FROM [dbo].[Sala] where[CodSala] = @p0", context);
            command.Parameters.AddWithValue("@p0", correoSala.SalaId);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    correoSala.Sala = new SalaEntidad()
                    {
                        CodSala = ManejoNulos.ManageNullInteger(reader["CodSala"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                    };
                }
            };
        }
        private void SetCorreo(OCU_CorreoSalaEntidad correoSala, SqlConnection context)
        {
            var command = new SqlCommand(@"SELECT [Id]
                                      ,[CodTipoCorreo]
                                      ,[Nombre]
                                      ,[Email]
                                      ,[SSL]
                                      ,[Smtp]
                                      ,[Puerto]
                                      ,[Estado]
,[Password]
                                  FROM [dbo].[OCU_Correo] where[Id] = @p0", context);
            command.Parameters.AddWithValue("@p0", correoSala.CorreoId);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    correoSala.Correo = new OCU_CorreoEntidad()
                    {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        CodTipoCorreo = ManejoNulos.ManageNullInteger(reader["CodTipoCorreo"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Email = ManejoNulos.ManageNullStr(reader["Email"]),
                        SSL = ManejoNulos.ManageNullInteger(reader["SSL"]),
                        Smtp = ManejoNulos.ManageNullStr(reader["Smtp"]),
                        Puerto = ManejoNulos.ManageNullInteger(reader["Puerto"]),
                        Estado = ManejoNulos.ManageNullInteger(reader["Estado"]),
                        Password = ManejoNulos.ManageNullStr(reader["Password"]),
                    };
                }
            };
        }
    }
}
