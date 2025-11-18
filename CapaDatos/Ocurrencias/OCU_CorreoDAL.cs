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
    public class OCU_CorreoDAL
    {
        string _conexion = string.Empty;
        public OCU_CorreoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<OCU_CorreoEntidad> GetListadoCorreos()
        {
            List<OCU_CorreoEntidad> lista = new List<OCU_CorreoEntidad>();
            string consulta = @"SELECT [Id]
                                  ,[CodTipoCorreo]
                                  ,[Nombre]
                                  ,[Email]
                                  ,[Password]
                                  ,[SSL]
                                  ,[Smtp]
                                  ,[Puerto]
                                  ,[Estado]
                              FROM [dbo].[OCU_Correo]";
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
                            var cliente = new OCU_CorreoEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                CodTipoCorreo = ManejoNulos.ManageNullInteger(dr["CodTipoCorreo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Email = ManejoNulos.ManageNullStr(dr["Email"]),
                                Password = ManejoNulos.ManageNullStr(dr["Password"]),
                                SSL = ManejoNulos.ManageNullInteger(dr["SSL"]),
                                Smtp = ManejoNulos.ManageNullStr(dr["Smtp"]),
                                Puerto = ManejoNulos.ManageNullInteger(dr["Puerto"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(cliente);
                        }
                    }
                    //Seteo detalle
                    foreach (var correo in lista)
                    {
                        SetDetalleCorreoSala(correo, con);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public int GuardarCorreos(string values)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
                                INSERT INTO [dbo].[OCU_Correo]
                                           ([CodTipoCorreo]
                                           ,[CodSalas]
                                           ,[Nombre]
                                           ,[Email]
                                           ,[Password]
                                           ,[SSL]
                                           ,[Smtp]
                                           ,[Puerto]
                                           ,[Estado])
                                     VALUES
                                           " + values;

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    IdInsertado = query.ExecuteNonQuery();
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
        public int GuardarCorreo(OCU_CorreoEntidad correo)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[OCU_Correo]
                                   ([CodTipoCorreo]
                                   ,[Nombre]
                                   ,[Email]
                                   ,[Password]
                                   ,[SSL]
                                   ,[Smtp]
                                   ,[Puerto]
                                   ,[Estado])
                                    Output Inserted.Id
                             VALUES
                                   (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(correo.CodTipoCorreo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(correo.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(correo.Email));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(correo.Password));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(correo.SSL));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(correo.Smtp));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(correo.Puerto));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(correo.Estado));
                 
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
        public bool EditarCorreo(OCU_CorreoEntidad correo)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[OCU_Correo]
                       SET [CodTipoCorreo] = @p0
                          ,[Nombre] = @p1
                          ,[Email] = @p2
                          ,[Password] =@p3
                          ,[SSL] = @p4
                          ,[Smtp] = @p5
                          ,[Puerto] = @p6
                     WHERE [Id]=@p7";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", correo.CodTipoCorreo);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(correo.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(correo.Email));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(correo.Password));
                    query.Parameters.AddWithValue("@p4", correo.SSL);
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(correo.Smtp));
                    query.Parameters.AddWithValue("@p6", correo.Puerto);
                    query.Parameters.AddWithValue("@p7", correo.Id);
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
        public bool EditarEstadoCorreo(OCU_CorreoEntidad correo)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[OCU_Correo]
                   SET [Estado] = @p0
                 WHERE Id=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", correo.Estado);
                    query.Parameters.AddWithValue("@p1", correo.Id);
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
        public OCU_CorreoEntidad GetCorreoID(int CorreoId)
        {
            OCU_CorreoEntidad correo = new OCU_CorreoEntidad();
            string consulta = @"SELECT [Id]
                                  ,[CodTipoCorreo]
                                  ,[Nombre]
                                  ,[Email]
                                  ,[SSL]
                                  ,[Smtp]
                                  ,[Puerto]
                                  ,[Estado],[Password]
                              FROM [dbo].[OCU_Correo]
                             where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CorreoId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                correo.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                correo.CodTipoCorreo = ManejoNulos.ManageNullInteger(dr["CodTipoCorreo"]);
                                correo.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                correo.Email = ManejoNulos.ManageNullStr(dr["Email"]);
                                correo.SSL = ManejoNulos.ManageNullInteger(dr["SSL"]);
                                correo.Smtp = ManejoNulos.ManageNullStr(dr["Smtp"]);
                                correo.Puerto = ManejoNulos.ManageNullInteger(dr["Puerto"]);
                                correo.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                correo.Password = ManejoNulos.ManageNullStr(dr["Password"]);
                            }
                        }
                    };
                    SetDetalleCorreoSala(correo, con);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return correo;
        }
        private void SetDetalleCorreoSala(OCU_CorreoEntidad correo, SqlConnection context)
        {
            List<OCU_CorreoSalaEntidad> listaCorreoSala = new List<OCU_CorreoSalaEntidad>();
            var command = new SqlCommand(@"SELECT ocu.[CorreoId]
                                                      ,ocu.[SalaId],sala.[Nombre]
                                                  FROM [dbo].[OCU_CorreoSala] as ocu join  [dbo].[Sala] as sala
                                                  on ocu.[SalaId]=sala.[CodSala] where [CorreoId] = @p0", context);
            command.Parameters.AddWithValue("@p0", correo.Id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var sala = new OCU_CorreoSalaEntidad()
                        {
                            CorreoId = ManejoNulos.ManageNullInteger(reader["CorreoId"]),
                            SalaId = ManejoNulos.ManageNullInteger(reader["SalaId"]),
                        };
                        sala.Sala.Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]);
                        listaCorreoSala.Add(sala);
                    }
                }
            };
            correo.ListaCorreoSala = listaCorreoSala;
        }

    }
}
