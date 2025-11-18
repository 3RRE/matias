using CapaDatos.Utilitarios;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.ControlAcceso;
using CapaEntidad.ControlAcceso.Ludopata.Dto;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ControlAcceso {
    public class CAL_LudopataDAL {
        string conexion = string.Empty;

        public CAL_LudopataDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CAL_LudopataEntidad> GetAllLudopata() {
            List<CAL_LudopataEntidad> lista = new List<CAL_LudopataEntidad>();
            string consulta = @"SELECT lud.[LudopataID]
                                      ,lud.[Nombre]
                                      ,lud.[ApellidoPaterno]
                                      ,lud.[ApellidoMaterno]
                                      ,lud.[FechaInscripcion]
                                      ,lud.[TipoExclusion]
                                      ,lud.[DNI]
                                      ,lud.[Foto]
                                      ,lud.[ContactoID]
                                      ,lud.[Telefono]
                                      ,lud.[CodRegistro]
                                      ,lud.[Estado]
                                      ,lud.[Imagen]
                                      ,lud.[TipoDoiID]
                                      ,lud.[CodUbigeo]
                                      ,doi.[Nombre] AS [DOINombre]
                                FROM [dbo].[CAL_Ludopata] lud
                                INNER JOIN [dbo].[AST_TipoDocumento] doi ON lud.TipoDoiID = doi.Id ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_LudopataEntidad {
                                LudopataID = ManejoNulos.ManageNullInteger(dr["LudopataID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]),
                                TipoExclusion = ManejoNulos.ManageNullInteger(dr["TipoExclusion"]),
                                DNI = ManejoNulos.ManageNullStr(dr["DNI"]),
                                Foto = ManejoNulos.ManageNullStr(dr["Foto"]),
                                ContactoID = ManejoNulos.ManageNullInteger(dr["ContactoID"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                CodRegistro = ManejoNulos.ManageNullStr(dr["CodRegistro"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]),
                                TipoDoiID = ManejoNulos.ManageNullInteger(dr["TipoDoiID"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                DOINombre = ManejoNulos.ManageNullStr(dr["DOINombre"]),
                            };

                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<CAL_LudopataDto> GetLudopatasActivos() {
            List<CAL_LudopataDto> lista = new List<CAL_LudopataDto>();
            string consulta = @"
                SELECT
	                ludo.CodRegistro AS CodigoRegistro,
	                ludo.Nombre AS Nombres,
	                ludo.ApellidoPaterno AS ApellidoPaterno,
	                ludo.ApellidoMaterno AS ApellidoMaterno,
	                td.Nombre AS TipoDocumento,
	                ludo.DNI AS NumeroDocumento,
	                ludo.Telefono AS Telefono,
	                ludo.FechaInscripcion AS FechaInscripcion,
	                cont.Nombre AS NombresContacto,
	                cont.ApellidoPaterno AS ApellidoPaternoContacto,
	                cont.ApellidoMaterno AS ApellidoMaternoContacto,
	                cont.Telefono AS TelefonoContacto,
	                cont.Celular AS CelularContacto
                FROM CAL_Ludopata AS ludo
                LEFT JOIN CAL_Contacto AS cont ON cont.ContactoID = ludo.ContactoID
                INNER JOIN AST_TipoDocumento as td ON td.Id = ludo.TipoDoiID
                WHERE ludo.Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CAL_LudopataDto item = new CAL_LudopataDto {
                                CodigoRegistro = ManejoNulos.ManageNullStr(dr["CodigoRegistro"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]),
                                Contacto = new CAL_ContactoLudopataDto() {
                                    Nombres = ManejoNulos.ManageNullStr(dr["NombresContacto"]),
                                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoContacto"]),
                                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoContacto"]),
                                    Telefono = ManejoNulos.ManageNullStr(dr["TelefonoContacto"]),
                                    Celular = ManejoNulos.ManageNullStr(dr["CelularContacto"]),
                                }
                            };

                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public CAL_LudopataEntidad GetIDLudopata(int id) {
            CAL_LudopataEntidad item = new CAL_LudopataEntidad();
            string consulta = @"SELECT lud.[LudopataID]
                                      ,lud.[Nombre]
                                      ,lud.[ApellidoPaterno]
                                      ,lud.[ApellidoMaterno]
                                      ,lud.[FechaInscripcion]
                                      ,lud.[TipoExclusion]
                                      ,lud.[DNI]
                                      ,lud.[Foto]
                                      ,lud.[ContactoID]
                                      ,lud.[Telefono]
                                      ,lud.[CodRegistro]
                                      ,lud.[Estado]
                                      ,lud.[Imagen]
                                      ,lud.[TipoDoiID]
                                      ,lud.[CodUbigeo]
                                      ,doi.[Nombre] AS [DOINombre]
                                FROM [dbo].[CAL_Ludopata] lud
                                left JOIN [dbo].[AST_TipoDocumento] doi ON lud.TipoDoiID = doi.Id 
                                WHERE LudopataID = @pLudopataID ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pLudopataID", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.LudopataID = ManejoNulos.ManageNullInteger(dr["LudopataID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]);
                                item.TipoExclusion = ManejoNulos.ManageNullInteger(dr["TipoExclusion"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.ContactoID = ManejoNulos.ManageNullInteger(dr["ContactoID"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.CodRegistro = ManejoNulos.ManageNullStr(dr["CodRegistro"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.TipoDoiID = ManejoNulos.ManageNullInteger(dr["TipoDoiID"]);
                                item.CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]);
                                item.DOINombre = ManejoNulos.ManageNullStr(dr["DOINombre"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarLudopata(CAL_LudopataEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_Ludopata ([Nombre]
                                      ,[ApellidoPaterno]
                                      ,[ApellidoMaterno]
                                      ,[FechaInscripcion]
                                      ,[TipoExclusion]
                                      ,[DNI]
                                      ,[Foto]
                                      ,[ContactoID]
                                      ,[Telefono]
                                      ,[CodRegistro]
                                      ,[Estado]
                                      ,[Imagen]
                                      ,[TipoDoiID]
                                      ,[CodUbigeo],[FechaRegistro],[UsuarioRegistro])
                                OUTPUT Inserted.LudopataID  
                                VALUES(@pNombre,@pApellidoPaterno,@pApellidoMaterno,@pFechaInscripcion,@pTipoExclusion,@pDNI,
                                       @pFoto,@pContactoID,@pTelefono,@pCodRegistro,@pEstado,@pImagen,@pTipoDoiID,@pCodUbigeo,@pFechaRegistro,@pUsuarioRegistro)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaInscripcion", ManejoNulos.ManageNullDate(Entidad.FechaInscripcion));
                    query.Parameters.AddWithValue("@pTipoExclusion", ManejoNulos.ManageNullInteger(Entidad.TipoExclusion));
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.DNI));
                    query.Parameters.AddWithValue("@pFoto", ManejoNulos.ManageNullStr(Entidad.Foto));
                    query.Parameters.AddWithValue("@pContactoID", ManejoNulos.ManageNullInteger(Entidad.ContactoID));
                    query.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.Telefono));
                    query.Parameters.AddWithValue("@pCodRegistro", ManejoNulos.ManageNullStr(Entidad.CodRegistro));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pImagen", ManejoNulos.ManageNullStr(Entidad.Imagen));
                    query.Parameters.AddWithValue("@pTipoDoiID", ManejoNulos.ManageNullInteger(Entidad.TipoDoiID));
                    query.Parameters.AddWithValue("@pCodUbigeo", ManejoNulos.ManageNullInteger(Entidad.CodUbigeo));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(DateTime.Now));
                    query.Parameters.AddWithValue("@pUsuarioRegistro", ManejoNulos.ManageNullInteger(Entidad.UsuarioRegistro));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarLudopata(CAL_LudopataEntidad Entidad) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_Ludopata]
                               SET [Nombre] = @pNombre
                                  ,[ApellidoPaterno] = @pApellidoPaterno
                                  ,[ApellidoMaterno] = @pApellidoMaterno
                                  ,[TipoExclusion] = @pTipoExclusion
                                  ,[DNI] = @pDNI
                                  ,[Foto] = @pFoto
                                  ,[ContactoID] = @pContactoID
                                  ,[Telefono] = @pTelefono
                                  ,[CodRegistro] = @pCodRegistro
                                  ,[Estado] = @pEstado
                                  ,[Imagen] = @pImagen
                                  ,[TipoDoiID] = @pTipoDoiID
                                  ,[CodUbigeo] = @PCodUbigeo
                                  ,[FechaInscripcion] = @pFechaInscripcion
                                         where LudopataID = @pLudopataID";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pLudopataID", ManejoNulos.ManageNullInteger(Entidad.LudopataID));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.DNI));
                    query.Parameters.AddWithValue("@pContactoID", ManejoNulos.ManageNullInteger(Entidad.ContactoID));
                    query.Parameters.AddWithValue("@pTipoExclusion", ManejoNulos.ManageNullInteger(Entidad.TipoExclusion));
                    query.Parameters.AddWithValue("@pTipoDoiID", ManejoNulos.ManageNullInteger(Entidad.TipoDoiID));
                    query.Parameters.AddWithValue("@pCodRegistro", ManejoNulos.ManageNullStr(Entidad.CodRegistro));
                    query.Parameters.AddWithValue("@pFoto", ManejoNulos.ManageNullStr(Entidad.Foto));
                    query.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.Telefono));
                    query.Parameters.AddWithValue("@pImagen", ManejoNulos.ManageNullStr(Entidad.Imagen));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@PCodUbigeo", ManejoNulos.ManageNullInteger(Entidad.CodUbigeo));
                    query.Parameters.AddWithValue("@pFechaInscripcion", ManejoNulos.ManageNullDate(Entidad.FechaInscripcion));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarLudopata(int id) {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_Ludopata 
                                WHERE LudopataID  = @pLudopataID";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pLudopataID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public int ObtenerTotalRegistrosFiltrados(string WhereQuery) {
            int TotalRegistrosFiltrados = 0;
            List<CAL_LudopataEntidad> lista = new List<CAL_LudopataEntidad>();
            try {
                using(SqlConnection con = new SqlConnection(conexion)) {
                    SqlCommand cmd = new
                        SqlCommand($@"select count(*) from CAL_Ludopata {WhereQuery}", con);
                    con.Open();
                    TotalRegistrosFiltrados = (int)cmd.ExecuteScalar();
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return TotalRegistrosFiltrados;
        }
        public int ObtenerTotalRegistros() {
            int TotalRegistros = 0;

            try {

                using(SqlConnection con = new SqlConnection(conexion)) {
                    SqlCommand cmd = new
                        SqlCommand("select count(*) from CAL_Ludopata ", con);
                    con.Open();
                    TotalRegistros = (int)cmd.ExecuteScalar();
                }
                return TotalRegistros;
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return TotalRegistros;
        }
        public List<CAL_LudopataEntidad> GetAllLudopataFiltrados(string WhereQuery) {
            List<CAL_LudopataEntidad> lista = new List<CAL_LudopataEntidad>();
            string consulta = @"SELECT lud.[LudopataID]
                                      ,lud.[Nombre]
                                      ,lud.[ApellidoPaterno]
                                      ,lud.[ApellidoMaterno]
                                      ,lud.[FechaInscripcion]
                                      ,lud.[TipoExclusion]
                                      ,lud.[DNI]
                                      ,lud.[Foto]
                                      ,lud.[ContactoID]
                                      ,lud.[Telefono]
                                      ,lud.[CodRegistro]
                                      ,lud.[Estado]
                                      ,lud.[Imagen]
                                      ,lud.[TipoDoiID]
                                      ,lud.[CodUbigeo]
                                      ,doi.[Nombre] AS [DOINombre]
                                FROM [dbo].[CAL_Ludopata] lud
                                left JOIN [dbo].[AST_TipoDocumento] doi ON lud.TipoDoiID = doi.Id " + WhereQuery;
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_LudopataEntidad {
                                LudopataID = ManejoNulos.ManageNullInteger(dr["LudopataID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]),
                                TipoExclusion = ManejoNulos.ManageNullInteger(dr["TipoExclusion"]),
                                DNI = ManejoNulos.ManageNullStr(dr["DNI"]),
                                Foto = ManejoNulos.ManageNullStr(dr["Foto"]),
                                ContactoID = ManejoNulos.ManageNullInteger(dr["ContactoID"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                CodRegistro = ManejoNulos.ManageNullStr(dr["CodRegistro"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]),
                                TipoDoiID = ManejoNulos.ManageNullInteger(dr["TipoDoiID"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                DOINombre = ManejoNulos.ManageNullStr(dr["DOINombre"]),
                            };

                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<CAL_LudopataEntidad>();
            }
            return lista;
        }
        public CAL_LudopataEntidad GetLudopataPorDNI(string dni) {
            CAL_LudopataEntidad item = new CAL_LudopataEntidad();
            string consulta = @"SELECT lud.[LudopataID]
                                      ,lud.[Nombre]
                                      ,lud.[ApellidoPaterno]
                                      ,lud.[ApellidoMaterno]
                                      ,lud.[FechaInscripcion]
                                      ,lud.[TipoExclusion]
                                      ,lud.[DNI]
                                      ,lud.[Foto]
                                      ,lud.[ContactoID]
                                      ,lud.[Telefono]
                                      ,lud.[CodRegistro]
                                      ,lud.[Estado]
                                      ,lud.[Imagen]
                                      ,lud.[TipoDoiID]
                                      ,lud.[CodUbigeo]
                                FROM [dbo].[CAL_Ludopata] lud
                                WHERE lud.DNI = @pDNI";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDNI", dni);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.LudopataID = ManejoNulos.ManageNullInteger(dr["LudopataID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]);
                                item.TipoExclusion = ManejoNulos.ManageNullInteger(dr["TipoExclusion"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.ContactoID = ManejoNulos.ManageNullInteger(dr["ContactoID"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.CodRegistro = ManejoNulos.ManageNullStr(dr["CodRegistro"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.TipoDoiID = ManejoNulos.ManageNullInteger(dr["TipoDoiID"]);
                                item.CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public void EditarFechaEnvioCorreo(CAL_LudopataEntidad Entidad) {
            string consulta = @"UPDATE [dbo].[CAL_Ludopata]
                               SET 
                                  [FechaEnvioCorreo] = @pFechaEnvioCorreo
                                         where LudopataID = @pLudopataID";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pLudopataID", ManejoNulos.ManageNullInteger(Entidad.LudopataID));
                    query.Parameters.AddWithValue("@pFechaEnvioCorreo", ManejoNulos.ManageNullDate(Entidad.FechaEnvioCorreo));
                    query.ExecuteNonQuery();
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public int ModificarEstadoLudopata(int idLudopata, bool estado) {
            int idActualizado = 0;
            string consulta = @"
                UPDATE CAL_Ludopata
                SET Estado = @Estado
                OUTPUT INSERTED.LudopataID
                WHERE LudopataID = @IdLudopata
            ";
            try {
                using(SqlConnection con = new SqlConnection(conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdLudopata", idLudopata);
                    query.Parameters.AddWithValue("@Estado", estado);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idActualizado;
        }

        public List<CAL_LudopataEntidad> ReporteLudopatasClientes() {
            List<CAL_LudopataEntidad> lista = new List<CAL_LudopataEntidad>();
            string consulta = @"SELECT ludo.[LudopataID]
                                  ,ludo.[Nombre]
                                  ,ludo.[ApellidoPaterno]
                                  ,ludo.[ApellidoMaterno]
                                  ,ludo.[FechaInscripcion]
                                  ,ludo.[DNI]
                                  ,ludo.[Foto]
                                  ,ludo.[CodRegistro]
                                  ,ludo.[Estado]
                                  ,ludo.[FechaRegistro]
                                  ,ludo.[FechaEnvioCorreo],
	                              cli.Id as IdCliente,
	                              cli.NombreCompleto as NombreCompletoCliente,
	                              cli.ApelPat as ApelPatCliente,
	                              cli.ApelMat as ApelMatCliente,
	                              cli.Nombre as NombreCliente,
	                              cli.NroDoc as NroDocCliente,
	                              cli.FechaRegistro as FechaRegistroCliente
                              FROM [BD_SEGURIDAD_PJ].[dbo].[CAL_Ludopata] as ludo
                              join AST_Cliente as cli
                              on ludo.DNI=cli.NroDoc order by LudopataID desc";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_LudopataEntidad {
                                LudopataID = ManejoNulos.ManageNullInteger(dr["LudopataID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]),
                                DNI = ManejoNulos.ManageNullStr(dr["DNI"]),
                                Foto = ManejoNulos.ManageNullStr(dr["Foto"]),
                                CodRegistro = ManejoNulos.ManageNullStr(dr["CodRegistro"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaEnvioCorreo = ManejoNulos.ManageNullDate(dr["FechaEnvioCorreo"]),
                            };
                            var cliente = new AST_ClienteEntidad() {
                                Id = ManejoNulls.ManageNullInteger(dr["IdCliente"]),
                                NombreCompleto = ManejoNulls.ManageNullStr(dr["NombreCompletoCliente"]),
                                ApelPat = ManejoNulls.ManageNullStr(dr["ApelPatCliente"]),
                                ApelMat = ManejoNulls.ManageNullStr(dr["ApelMatCliente"]),
                                Nombre = ManejoNulls.ManageNullStr(dr["NombreCliente"]),
                                NroDoc = ManejoNulls.ManageNullStr(dr["NroDocCliente"]),
                                FechaRegistro = ManejoNulls.ManageNullDate(dr["FechaRegistroCliente"]),
                            };
                            item.Cliente = cliente;
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
    }
}
