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
    public class OCU_OcurrenciaDAL
    {
        string _conexion = string.Empty;
        public OCU_OcurrenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarOcurrencia(OCU_OcurrenciaEntidad ocurrencia)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[OCU_Ocurrencia]
                                               ([Fecha]
                                               ,[Nombres]
                                               ,[ApelPat]
                                               ,[ApelMat]
                                               ,[TipoDocId]
                                               ,[NroDoc]
                                               ,[TipoOcurrenciaId]
                                               ,[Descripcion]
                                               ,[JefeSala]
                                               ,[SeInformoA]
                                               ,[CodSala]
                                               ,[UsuarioReg])
                                                Output Inserted.Id
                                         VALUES
                                               (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)
                                    ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullDate(ocurrencia.Fecha));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(ocurrencia.Nombres));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(ocurrencia.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(ocurrencia.ApelMat));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(ocurrencia.TipoDocId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(ocurrencia.NroDoc));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(ocurrencia.TipoOcurrenciaId));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(ocurrencia.Descripcion));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(ocurrencia.JefeSala));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(ocurrencia.SeInformoA));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(ocurrencia.CodSala));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(ocurrencia.UsuarioReg));
              
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
        public bool EditarEstadoEnvioOcurrencia(OCU_OcurrenciaEntidad ocurrencia)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[OCU_Ocurrencia]
                           SET [Enviado] = @p0
                         WHERE [Id]=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(ocurrencia.Enviado));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(ocurrencia.Id));
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
        public List<OCU_OcurrenciaEntidad> GetListadoOcurrencia(DateTime fechaIni,DateTime fechaFin, string condicion="")
        {
            List<OCU_OcurrenciaEntidad> lista = new List<OCU_OcurrenciaEntidad>();
            string consulta = @"SELECT [Id]
                                      ,[Fecha]
                                      ,[Nombres]
                                      ,[ApelPat]
                                      ,[ApelMat]
                                      ,[TipoDocId]
                                      ,[NroDoc]
                                      ,[TipoOcurrenciaId]
                                      ,[Descripcion]
                                      ,[JefeSala]
                                      ,[SeInformoA]
                                      ,[CodSala]
                                      ,[UsuarioReg]
                                      ,[Enviado]
                                  FROM [dbo].[OCU_Ocurrencia] where "+condicion+ " CONVERT(date, Fecha) between @p1 and @p2 ;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ocurrencia = new OCU_OcurrenciaEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Nombres= ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                TipoDocId = ManejoNulos.ManageNullInteger(dr["TipoDocId"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                TipoOcurrenciaId = ManejoNulos.ManageNullInteger(dr["TipoOcurrenciaId"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                JefeSala = ManejoNulos.ManageNullStr(dr["JefeSala"]),
                                SeInformoA = ManejoNulos.ManageNullStr(dr["SeInformoA"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                UsuarioReg = ManejoNulos.ManageNullInteger(dr["UsuarioReg"]),
                                Enviado = ManejoNulos.ManageNullInteger(dr["Enviado"]),
                                Hash = ClaseEncriptacion.Base64ForUrlEncode(ManejoNulos.ManageNullStr(dr["Id"]))
                        };

                            lista.Add(ocurrencia);
                        }
                    }
                    foreach (var ocurrencia in lista)
                    {
                        SetTipoDocumento(ocurrencia, con);
                        SetTipoOcurrencia(ocurrencia, con);
                        SetUsuarioRegistro(ocurrencia, con);
                        SetSala(ocurrencia, con);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public OCU_OcurrenciaEntidad GetOcurrenciaId(int ocurrenciaId)
        {
            OCU_OcurrenciaEntidad ocurrencia = new OCU_OcurrenciaEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Fecha]
                                  ,[Nombres]
                                  ,[ApelPat]
                                  ,[ApelMat]
                                  ,[TipoDocId]
                                  ,[NroDoc]
                                  ,[TipoOcurrenciaId]
                                  ,[Descripcion]
                                  ,[JefeSala]
                                  ,[SeInformoA]
                                  ,[CodSala]
                                  ,[UsuarioReg]
                                  ,[Enviado]
                              FROM [dbo].[OCU_Ocurrencia]
                             where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ocurrenciaId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ocurrencia.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                ocurrencia.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                ocurrencia.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                ocurrencia.ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]);
                                ocurrencia.ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]);
                                ocurrencia.TipoDocId = ManejoNulos.ManageNullInteger(dr["TipoDocId"]);
                                ocurrencia.NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]);
                                ocurrencia.TipoOcurrenciaId = ManejoNulos.ManageNullInteger(dr["TipoOcurrenciaId"]);
                                ocurrencia.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                ocurrencia.JefeSala = ManejoNulos.ManageNullStr(dr["JefeSala"]);
                                ocurrencia.SeInformoA = ManejoNulos.ManageNullStr(dr["SeInformoA"]);
                                ocurrencia.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                ocurrencia.UsuarioReg = ManejoNulos.ManageNullInteger(dr["UsuarioReg"]);
                                ocurrencia.Enviado = ManejoNulos.ManageNullInteger(dr["Enviado"]);
                                ocurrencia.Hash = ClaseEncriptacion.Base64ForUrlEncode(ManejoNulos.ManageNullStr(dr["Id"]));
                            }
                        }
                    };
                    if (ocurrencia.Id != 0)
                    {
                        SetTipoDocumento(ocurrencia, con);
                        SetTipoOcurrencia(ocurrencia, con);
                        SetUsuarioRegistro(ocurrencia, con);
                        SetSala(ocurrencia, con);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ocurrencia;
        }

        private void SetTipoDocumento(OCU_OcurrenciaEntidad ocurrencia, SqlConnection context)
        {
            var command = new SqlCommand(@"SELECT [DOIID]
                                  ,[DESCRIPCION]
                                  ,[Estado]
                              FROM [dbo].[TipoDOI] where[DOIID] = @p0", context);
            command.Parameters.AddWithValue("@p0", ocurrencia.TipoDocId);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    ocurrencia.TipoDocumento = new TipoDOIEntidad()
                    {
                        DOIID= ManejoNulos.ManageNullInteger(reader["DOIID"]),
                        DESCRIPCION = ManejoNulos.ManageNullStr(reader["DESCRIPCION"]).ToUpper(),
                    };
                }
            };
        }
        private void SetTipoOcurrencia(OCU_OcurrenciaEntidad ocurrencia, SqlConnection context)
        {
            var command = new SqlCommand(@"SELECT [Id]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[Estado]
                              FROM [dbo].[OCU_TipoOcurrencia] where[Id] = @p0", context);
            command.Parameters.AddWithValue("@p0", ocurrencia.TipoOcurrenciaId);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    ocurrencia.TipoOcurrencia = new OCU_TipoOcurrenciaEntidad()
                    {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre= ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion= ManejoNulos.ManageNullStr(reader["Descripcion"]),
                    };
                }
            };
        }
        private void SetUsuarioRegistro(OCU_OcurrenciaEntidad ocurrencia, SqlConnection context)
        {
            var command = new SqlCommand(@"SELECT [UsuarioID]
                                              ,[UsuarioNombre]
                                          FROM [dbo].[SEG_Usuario] where[UsuarioID] = @p0", context);
            command.Parameters.AddWithValue("@p0", ocurrencia.UsuarioReg);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    ocurrencia.SEG_Usuario = new SEG_UsuarioEntidad()
                    {
                        UsuarioID = ManejoNulos.ManageNullInteger(reader["UsuarioID"]),
                        UsuarioNombre = ManejoNulos.ManageNullStr(reader["UsuarioNombre"]),
                    };
                }
            };
        }
        private void SetSala(OCU_OcurrenciaEntidad ocurrencia, SqlConnection context)
        {
            var command = new SqlCommand(@"SELECT [CodSala]
                                            ,[Nombre]
                FROM [dbo].[Sala] where[CodSala] = @p0", context);
            command.Parameters.AddWithValue("@p0", ocurrencia.CodSala);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    ocurrencia.Sala = new SalaEntidad()
                    {
                        CodSala = ManejoNulos.ManageNullInteger(reader["CodSala"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                    };
                }
            };
        }
    }
}
