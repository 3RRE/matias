using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.AsistenciaCliente {
    public class AST_AsistenciaClienteSalaDAL {
        string _conexion = string.Empty;
        public AST_AsistenciaClienteSalaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaClienteSala(int ClienteId, int SalaId) {
            List<AST_AsistenciaClienteSalaEntidad> lista = new List<AST_AsistenciaClienteSalaEntidad>();
            string consulta = @"SELECT [Id]
                              ,[ClienteId]
                              ,[SalaId]
                              ,[FechaRegistro],
[TipoFrecuenciaId],[TipoJuegoId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId]
                          FROM [dbo].[AST_AsistenciaClienteSala]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_AsistenciaClienteSalaEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]),
                                TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]),
                                ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                JuegoMaquina = ManejoNulos.ManageNullStr(dr["JuegoMaquina"]),
                                MarcaMaquina = ManejoNulos.ManageNullStr(dr["MarcaMaquina"]),
                                TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]),
                            };

                            lista.Add(asistenciaCliente);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public AST_AsistenciaClienteSalaEntidad GetAsistenciaClienteSalaID(int asistenciaId) {
            AST_AsistenciaClienteSalaEntidad asistenciaCliente = new AST_AsistenciaClienteSalaEntidad();
            string consulta = @"SELECT [Id]
                                  ,[ClienteId]
                                  ,[SalaId]
                                  ,[FechaRegistro],[TipoJuegoId],[TipoFrecuenciaId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId]
                              FROM [dbo].[AST_AsistenciaClienteSala] where [Id]=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", asistenciaId);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                asistenciaCliente.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                asistenciaCliente.ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]);
                                asistenciaCliente.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                asistenciaCliente.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                asistenciaCliente.TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuencia"]);
                                asistenciaCliente.TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuego"]);
                                asistenciaCliente.ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]);
                                asistenciaCliente.CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]);
                                asistenciaCliente.JuegoMaquina = ManejoNulos.ManageNullStr(dr["JuegoMaquina"]);
                                asistenciaCliente.MarcaMaquina = ManejoNulos.ManageNullStr(dr["MarcaMaquina"]);
                                asistenciaCliente.TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                asistenciaCliente.Id = 0;
            }
            return asistenciaCliente;
        }
        public int GuardarAsistenciaClienteSala(AST_AsistenciaClienteSalaEntidad asistenciaCliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[AST_AsistenciaClienteSala]
                       ([ClienteId],[SalaId],[FechaRegistro],[TipoFrecuenciaId],[TipoJuegoId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId])
                        Output Inserted.Id
                        VALUES
                       (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9);";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(asistenciaCliente.ClienteId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(asistenciaCliente.SalaId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(asistenciaCliente.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(asistenciaCliente.TipoFrecuenciaId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(asistenciaCliente.TipoJuegoId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDouble(asistenciaCliente.ApuestaImportante));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(asistenciaCliente.CodMaquina));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(asistenciaCliente.JuegoMaquina));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(asistenciaCliente.MarcaMaquina));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(asistenciaCliente.TipoClienteId));
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
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaCliente(int ClienteId) {
            List<AST_AsistenciaClienteSalaEntidad> lista = new List<AST_AsistenciaClienteSalaEntidad>();
            string consulta = @"SELECT [Id]
                              ,[ClienteId]
                              ,[SalaId]
                              ,[FechaRegistro],[TipoFrecuenciaId],[TipoJuegoId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId]
                          FROM [dbo].[AST_AsistenciaClienteSala] where ClienteId=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ClienteId);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_AsistenciaClienteSalaEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]),
                                TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]),
                                ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                JuegoMaquina = ManejoNulos.ManageNullStr(dr["JuegoMaquina"]),
                                MarcaMaquina = ManejoNulos.ManageNullStr(dr["MarcaMaquina"]),
                                TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]),
                            };
                            lista.Add(asistenciaCliente);
                        }
                    }
                    //Set Cliente y Sala
                    foreach(var m in lista) {
                        SetCliente(m, con);
                        SetSala(m, con);
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaClienteFiltros(int ClienteId, DateTime fechaIni, DateTime fechaFin, int SalaId) {
            List<AST_AsistenciaClienteSalaEntidad> lista = new List<AST_AsistenciaClienteSalaEntidad>();
            string consulta = @"SELECT [Id]
                              ,[ClienteId]
                              ,[SalaId]
                              ,[FechaRegistro]
,[TipoFrecuenciaId],[TipoJuegoId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId]
                          FROM [dbo].[AST_AsistenciaClienteSala] where ClienteId=@p0 and SalaId=@p3 and CONVERT(date, FechaRegistro) between @p1 and @p2";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ClienteId);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    query.Parameters.AddWithValue("@p3", SalaId);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_AsistenciaClienteSalaEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]),
                                TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]),
                                ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                JuegoMaquina = ManejoNulos.ManageNullStr(dr["JuegoMaquina"]),
                                MarcaMaquina = ManejoNulos.ManageNullStr(dr["MarcaMaquina"]),
                                TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]),
                            };
                            lista.Add(asistenciaCliente);
                        }
                    }
                    //Set Cliente y Sala
                    foreach(var m in lista) {
                        SetCliente(m, con);
                        SetSala(m, con);
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaSalaFiltros(string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            List<AST_AsistenciaClienteSalaEntidad> lista = new List<AST_AsistenciaClienteSalaEntidad>();
            string consulta = @"SELECT [Id]
                              ,[ClienteId]
                              ,[SalaId]
                              ,[FechaRegistro],[TipoFrecuenciaId],[TipoJuegoId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId]
                          FROM [dbo].[AST_AsistenciaClienteSala] where " + ArraySalaId + " CONVERT(date, FechaRegistro) between @p1 and @p2";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_AsistenciaClienteSalaEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]),
                                TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]),
                                ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                JuegoMaquina = ManejoNulos.ManageNullStr(dr["JuegoMaquina"]),
                                MarcaMaquina = ManejoNulos.ManageNullStr(dr["MarcaMaquina"]),
                                TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]),
                            };
                            lista.Add(asistenciaCliente);
                        }
                    }
                    //Set Cliente y Sala
                    foreach(var m in lista) {
                        SetCliente(m, con);
                        SetSala(m, con);
                        SetTipoJuego(m, con);
                        SetTipoFrecuencia(m, con);
                        SetTipoCliente(m, con);
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<AST_ClienteSalaGlobal> GetAllClientesSala(int idSala) {
            List<AST_ClienteSalaGlobal> lista = new List<AST_ClienteSalaGlobal>();
            string consulta = @"
                SET DATEFORMAT dmy
                DECLARE @CodSalaMaestra INT = 0;

                -- Obtener la sala maestra
                SELECT @CodSalaMaestra = ISNULL(CodSalaMaestra, 0) 
                FROM Sala 
                WHERE CodSala = @CodSala;

               SELECT 
                    sala.CodSala AS CodSala,
                    sala.Nombre AS NombreSala,
                    td.Nombre AS TipoDocumento,
                    cliente.NroDoc AS Documento,
                    cliente.NombreCompleto AS NombreCliente,
                    CASE
                        WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR (clienteSala.EnviaNotificacionSms = 0 AND clienteSala.EnviaNotificacionWhatsapp = 0 AND clienteSala.LlamadaCelular = 0))
                        THEN 'NO AUTORIZÓ'
                        ELSE ISNULL(cliente.Celular1, '') 
                    END AS Celular,
                    CASE
                        WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR clienteSala.EnviaNotificacionEmail = 0) 
                        THEN 'NO AUTORIZÓ'
                        ELSE ISNULL(cliente.Mail, '') 
                    END AS Mail,
                    FORMAT(ISNULL(clienteSala.FechaRegistro, cliente.FechaRegistro),'dd-MM-yyyy HH:mm','en-eu')  AS FechaRegistro,
                    cliente.TipoRegistro AS TipoRegistro,
                    cliente.FechaNacimiento AS FechaNacimiento,
                    CASE 
                        WHEN u.PaisId = 'PE' THEN 'PERU'
                        ELSE u.Nombre
                    END AS Nacionalidad,
                    u.PaisId,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionWhatsapp END AS EnviaNotificacionWhatsapp,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionSms END AS EnviaNotificacionSms,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionEmail END AS EnviaNotificacionEmail,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.LlamadaCelular END AS LlamadaCelular,
                    CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                    CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                    CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                FROM AST_ClienteSala AS clienteSala
                INNER JOIN AST_Cliente AS cliente on cliente.Id = clienteSala.ClienteId
                LEFT JOIN Ubigeo AS u ON cliente.UbigeoProcedenciaId = u.CodUbigeo
                LEFT JOIN AST_TipoDocumento AS td ON td.Id = cliente.TipoDocumentoId
                LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON TRIM(ludo.DNI) = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                LEFT JOIN [dbo].[CAL_Timador] AS tima ON TRIM(tima.DNI) = TRIM(cliente.NroDoc) AND tima.Estado = 1
                LEFT JOIN [dbo].[CAL_RobaStackersBilletero] AS rsb ON TRIM(rsb.DNI) = TRIM(cliente.NroDoc) AND rsb.Estado = 1
                INNER JOIN Sala AS sala ON sala.CodSala = clienteSala.SalaId
                WHERE sala.CodSalaMaestra = @CodSalaMaestra

                UNION

                SELECT
                    sala.CodSala AS CodSala,
                    sala.Nombre AS NombreSala,
                    td.Nombre AS TipoDocumento,
                    cliente.NroDoc AS Documento,
                    cliente.NombreCompleto AS NombreCliente,
                    CASE
                        WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR (clienteSala.EnviaNotificacionSms = 0 AND clienteSala.EnviaNotificacionWhatsapp = 0 AND clienteSala.LlamadaCelular = 0))
                        THEN 'NO AUTORIZÓ'
                        ELSE ISNULL(cliente.Celular1, '') 
                    END AS Celular,
                    CASE
                        WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR clienteSala.EnviaNotificacionEmail = 0) 
                        THEN 'NO AUTORIZÓ'
                        ELSE ISNULL(cliente.Mail, '') 
                    END AS Mail,
                    FORMAT(cliente.FechaRegistro,'dd-MM-yyyy HH:mm','en-eu') AS FechaRegistro,
                    cliente.TipoRegistro AS TipoRegistro,
                    cliente.FechaNacimiento AS FechaNacimiento,
                    CASE 
                        WHEN u.PaisId = 'PE' THEN 'PERU'
                        ELSE u.Nombre
                    END AS Nacionalidad,
                    u.PaisId,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionWhatsapp, 1) END AS EnviaNotificacionWhatsapp,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionSms, 1) END AS EnviaNotificacionSms,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionEmail, 1) END AS EnviaNotificacionEmail,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.LlamadaCelular, 1) END AS LlamadaCelular,
                    CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                    CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                    CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                FROM AST_Cliente AS cliente
                INNER JOIN Sala AS sala ON sala.CodSala = cliente.SalaId
                LEFT JOIN AST_ClienteSala AS clienteSala ON clienteSala.ClienteId = cliente.Id AND clienteSala.SalaId = cliente.SalaId
                LEFT JOIN AST_TipoDocumento AS td ON td.Id = cliente.TipoDocumentoId
                LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON TRIM(ludo.DNI) = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                LEFT JOIN [dbo].[CAL_Timador] AS tima ON TRIM(tima.DNI) = TRIM(cliente.NroDoc) AND tima.Estado = 1
                LEFT JOIN [dbo].[CAL_RobaStackersBilletero] AS rsb ON TRIM(rsb.DNI) = TRIM(cliente.NroDoc) AND rsb.Estado = 1
                LEFT JOIN Ubigeo AS u ON cliente.UbigeoProcedenciaId = u.CodUbigeo

                WHERE sala.CodSalaMaestra = @CodSalaMaestra
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", idSala);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_ClienteSalaGlobal {
                                codSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["Documento"]),
                                NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                Celular = ManejoNulos.ManageNullStr(dr["Celular"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                                TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]),
                                PaisId = ManejoNulos.ManageNullStr(dr["PaisId"]),
                                EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]),
                                EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]),
                                EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]),
                                LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]),
                                EsLudopata = ManejoNulos.ManegeNullBool(dr["EsLudopata"]),
                                EsProhibido = ManejoNulos.ManegeNullBool(dr["EsProhibido"]),
                                EsRobaStacker = ManejoNulos.ManegeNullBool(dr["EsRobaStacker"]),
                            };
                            lista.Add(asistenciaCliente);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        
        public List<AST_ClienteSala> GetReporteListaClienteSala(string array_salas, DateTime fechaIni, DateTime fechaFin, bool verInfoContacto) {
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();
            string consulta = $@"
                SET DATEFORMAT dmy
                DECLARE @SalasMaestras TABLE (
                    CodSalaMaestra INT
                );

                INSERT INTO @SalasMaestras (CodSalaMaestra)
                SELECT DISTINCT CodSalaMaestra
                FROM Sala as sala
                WHERE {array_salas}

                SELECT *
                FROM (
                    SELECT
                        sala.CodSala as codSala,
                        sala.Nombre as NombreSala,
                        td.Nombre as TipoDocumento,
                        cliente.NroDoc,
                        CASE
                            WHEN (cliente.Nombre IS NULL OR cliente.Nombre='') THEN cliente.NombreCompleto
                            ELSE CONCAT(cliente.Nombre,' ',cliente.ApelPat,' ', cliente.ApelMat)
                        END AS NombreCliente,
                        cliente.nro_dosis as cantDosis,
                        CASE
                            WHEN ((ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR (clienteSala.EnviaNotificacionSms = 0 AND clienteSala.EnviaNotificacionWhatsapp = 0 AND clienteSala.LlamadaCelular = 0)) AND 0 = @VerInfoContacto)
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Celular1, '') 
                        END AS Celular,
                        CASE
                            WHEN ((ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR clienteSala.EnviaNotificacionEmail = 0) AND 0 = @VerInfoContacto) 
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Mail, '') 
                        END AS Mail,
                        FORMAT(cliente.FechaNacimiento,'dd-MM-yyyy') as FechaNacimiento, 
                        FORMAT(ISNULL(clienteSala.FechaRegistro, cliente.FechaRegistro),'dd-MM-yyyy HH:mm','en-eu') as FechaRegistro,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionWhatsapp END AS EnviaNotificacionWhatsapp,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionSms END AS EnviaNotificacionSms,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionEmail END AS EnviaNotificacionEmail,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.LlamadaCelular END AS LlamadaCelular,
                        CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                        CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                        CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                    FROM [dbo].[AST_ClienteSala] AS clienteSala
                    INNER JOIN [dbo].[AST_Cliente] AS cliente on cliente.Id = clienteSala.ClienteId
                    INNER JOIN [dbo].[Sala] AS sala ON sala.CodSala = clienteSala.SalaId
                    LEFT JOIN [dbo].[AST_TipoDocumento] AS td ON td.Id = cliente.TipoDocumentoId
                    LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON TRIM(ludo.DNI) = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                    LEFT JOIN [dbo].[CAL_Timador] AS tima ON TRIM(tima.DNI) = TRIM(cliente.NroDoc) AND tima.Estado = 1
                    LEFT JOIN [dbo].[CAL_RobaStackersBilletero] AS rsb ON TRIM(rsb.DNI) = TRIM(cliente.NroDoc) AND rsb.Estado = 1
                    WHERE sala.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)

                    UNION

                    SELECT
                        sala.CodSala as codSala,
                        sala.Nombre as NombreSala,
                        td.Nombre as TipoDocumento,
                        cliente.NroDoc,
                        CASE
                            WHEN (cliente.Nombre IS NULL OR cliente.Nombre='') THEN cliente.NombreCompleto
                            ELSE CONCAT(cliente.Nombre,' ',cliente.ApelPat,' ', cliente.ApelMat)
                        END AS NombreCliente,
                        cliente.nro_dosis as cantDosis,
                        CASE
                            WHEN ((ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR (clienteSala.EnviaNotificacionSms = 0 AND clienteSala.EnviaNotificacionWhatsapp = 0 AND clienteSala.LlamadaCelular = 0)) AND 0 = @VerInfoContacto)
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Celular1, '') 
                        END AS Celular,
                        CASE
                            WHEN ((ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR clienteSala.EnviaNotificacionEmail = 0) AND 0 = @VerInfoContacto)
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Mail, '') 
                        END AS Mail,
                        FORMAT(cliente.FechaNacimiento,'dd-MM-yyyy') as FechaNacimiento,
                        FORMAT(cliente.FechaRegistro,'dd-MM-yyyy HH:mm','en-eu') as FechaRegistro,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionWhatsapp, 1) END AS EnviaNotificacionWhatsapp,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionSms, 1) END AS EnviaNotificacionSms,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionEmail, 1) END AS EnviaNotificacionEmail,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.LlamadaCelular, 1) END AS LlamadaCelular,
                        CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                        CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                        CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                    FROM [dbo].[AST_Cliente] AS cliente
                    INNER JOIN [dbo].[Sala] AS sala ON sala.CodSala = cliente.SalaId
                    LEFT JOIN [dbo].[AST_ClienteSala] AS clienteSala ON clienteSala.ClienteId = cliente.Id AND clienteSala.SalaId = cliente.SalaId
                    LEFT JOIN [dbo].[AST_TipoDocumento] AS td ON td.Id = cliente.TipoDocumentoId
                    LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON TRIM(ludo.DNI) = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                    LEFT JOIN [dbo].[CAL_Timador] AS tima ON TRIM(tima.DNI) = TRIM(cliente.NroDoc) AND tima.Estado = 1
                    LEFT JOIN [dbo].[CAL_RobaStackersBilletero] AS rsb ON TRIM(rsb.DNI) = TRIM(cliente.NroDoc) AND rsb.Estado = 1
                    WHERE sala.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)
                 ) AS cliente
                 WHERE CONVERT(date, cliente.FechaRegistro) between @p1 and @p2 

                 ORDER BY CLIENTE.codSala ASC
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@yearmonth", YearMonth);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    query.Parameters.AddWithValue("@VerInfoContacto", verInfoContacto);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_ClienteSala {
                                codSala = ManejoNulos.ManageNullInteger(dr["codSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                cantDosis = ManejoNulos.ManageNullInteger(dr["cantDosis"]),
                                Celular = ManejoNulos.ManageNullStr(dr["Celular"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]),
                                EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]),
                                EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]),
                                LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]),
                                EsLudopata = ManejoNulos.ManegeNullBool(dr["EsLudopata"]),
                                EsProhibido = ManejoNulos.ManegeNullBool(dr["EsProhibido"]),
                                EsRobaStacker = ManejoNulos.ManegeNullBool(dr["EsRobaStacker"]),
                            };
                            lista.Add(asistenciaCliente);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<AST_ClienteSala> GetListadoClienteSala(string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();
            string consulta = $@"
                SET DATEFORMAT dmy
                DECLARE @SalasMaestras TABLE (
                    CodSalaMaestra INT
                );

                -- Insertar las salas maestras correspondientes
                INSERT INTO @SalasMaestras (CodSalaMaestra)
                SELECT DISTINCT CodSalaMaestra
                FROM Sala as sala
                WHERE {ArraySalaId}

                SELECT * 
                FROM (
                    SELECT
                        sala.CodSala as codSala,
                        sala.Nombre as NombreSala,
                        td.Nombre as TipoDocumento,
                        cliente.NroDoc,
                        CASE
                            WHEN (cliente.Nombre IS NULL OR cliente.Nombre='') THEN cliente.NombreCompleto
                            ELSE CONCAT(cliente.Nombre,' ',cliente.ApelPat,' ', cliente.ApelMat)
                        END AS NombreCliente,
                        cliente.nro_dosis as cantDosis,
                        CASE
                            WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR (clienteSala.EnviaNotificacionSms = 0 AND clienteSala.EnviaNotificacionWhatsapp = 0 AND clienteSala.LlamadaCelular = 0))
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Celular1, '') 
                        END AS Celular,
                        CASE
                            WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR clienteSala.EnviaNotificacionEmail = 0) 
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Mail, '') 
                        END AS Mail,
                        FORMAT(cliente.FechaNacimiento,'dd-MM-yyyy') as FechaNacimiento, 
                        FORMAT(ISNULL(clienteSala.FechaRegistro, cliente.FechaRegistro),'dd-MM-yyyy HH:mm','en-eu') as FechaRegistro,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionWhatsapp END AS EnviaNotificacionWhatsapp,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionSms END AS EnviaNotificacionSms,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.EnviaNotificacionEmail END AS EnviaNotificacionEmail,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE clienteSala.LlamadaCelular END AS LlamadaCelular,
                        CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                        CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                        CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                    FROM [dbo].[AST_ClienteSala] AS clienteSala
                    INNER JOIN [dbo].[AST_Cliente] AS cliente on cliente.Id = clienteSala.ClienteId
                    INNER JOIN [dbo].[Sala] AS sala ON sala.CodSala = clienteSala.SalaId
                    LEFT JOIN [dbo].[AST_TipoDocumento] AS td ON td.Id = cliente.TipoDocumentoId
                    LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON TRIM(ludo.DNI) = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                    LEFT JOIN [dbo].[CAL_Timador] AS tima ON TRIM(tima.DNI) = TRIM(cliente.NroDoc) AND tima.Estado = 1
                    LEFT JOIN [dbo].[CAL_RobaStackersBilletero] AS rsb ON TRIM(rsb.DNI) = TRIM(cliente.NroDoc) AND rsb.Estado = 1
                    WHERE sala.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)

                    UNION

                    SELECT
                        sala.CodSala as codSala,
                        sala.Nombre as NombreSala,
                        td.Nombre as TipoDocumento,
                        cliente.NroDoc,
                        CASE
                            WHEN (cliente.Nombre IS NULL OR cliente.Nombre='') THEN cliente.NombreCompleto
                            ELSE CONCAT(cliente.Nombre,' ',cliente.ApelPat,' ', cliente.ApelMat)
                        END AS NombreCliente,
                        cliente.nro_dosis as cantDosis,
                        CASE
                            WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR (clienteSala.EnviaNotificacionSms = 0 AND clienteSala.EnviaNotificacionWhatsapp = 0 AND clienteSala.LlamadaCelular = 0))
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Celular1, '') 
                        END AS Celular,
                        CASE
                            WHEN (ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL OR clienteSala.EnviaNotificacionEmail = 0) 
                            THEN 'NO AUTORIZÓ'
                            ELSE ISNULL(cliente.Mail, '') 
                        END AS Mail,
                        FORMAT(cliente.FechaNacimiento,'dd-MM-yyyy') as FechaNacimiento,
                        FORMAT(cliente.FechaRegistro,'dd-MM-yyyy HH:mm','en-eu') as FechaRegistro,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionWhatsapp, 1) END AS EnviaNotificacionWhatsapp,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionSms, 1) END AS EnviaNotificacionSms,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.EnviaNotificacionEmail, 1) END AS EnviaNotificacionEmail,
                        CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(clienteSala.LlamadaCelular, 1) END AS LlamadaCelular,
                        CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                        CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                        CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                    FROM [dbo].[AST_Cliente] AS cliente
                    INNER JOIN [dbo].[Sala] AS sala ON sala.CodSala = cliente.SalaId
                    LEFT JOIN [dbo].[AST_ClienteSala] AS clienteSala ON clienteSala.ClienteId = cliente.Id AND clienteSala.SalaId = cliente.SalaId
                    LEFT JOIN [dbo].[AST_TipoDocumento] AS td ON td.Id = cliente.TipoDocumentoId
                    LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON TRIM(ludo.DNI) = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                    LEFT JOIN [dbo].[CAL_Timador] AS tima ON TRIM(tima.DNI) = TRIM(cliente.NroDoc) AND tima.Estado = 1
                    LEFT JOIN [dbo].[CAL_RobaStackersBilletero] AS rsb ON TRIM(rsb.DNI) = TRIM(cliente.NroDoc) AND rsb.Estado = 1
                    WHERE sala.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)
                ) AS cliente
                WHERE CONVERT(date, cliente.FechaRegistro) between @p1 and @p2 
                ORDER BY CLIENTE.codSala ASC
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var asistenciaCliente = new AST_ClienteSala {
                                codSala = ManejoNulos.ManageNullInteger(dr["codSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                cantDosis = ManejoNulos.ManageNullInteger(dr["cantDosis"]),
                                Celular = ManejoNulos.ManageNullStr(dr["Celular"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]),
                                EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]),
                                EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]),
                                LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]),
                                EsLudopata = ManejoNulos.ManegeNullBool(dr["EsLudopata"]),
                                EsProhibido = ManejoNulos.ManegeNullBool(dr["EsProhibido"]),
                                EsRobaStacker = ManejoNulos.ManegeNullBool(dr["EsRobaStacker"]),
                            };
                            lista.Add(asistenciaCliente);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public AST_AsistenciaClienteSalaEntidad GetUltimaAsistenciaClienteSalaID(int ClienteId, int SalaId) {
            AST_AsistenciaClienteSalaEntidad asistenciaCliente = new AST_AsistenciaClienteSalaEntidad();
            string consulta = @"SELECT top 1 [Id]
                                  ,[ClienteId]
                                  ,[SalaId]
                                  ,[FechaRegistro],[TipoJuegoId],[TipoFrecuenciaId],[ApuestaImportante],[CodMaquina],[JuegoMaquina],[MarcaMaquina],[TipoClienteId]
                              FROM [dbo].[AST_AsistenciaClienteSala] where [ClienteId]=@p0 and [SalaId]=@p1 order by FechaRegistro desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ClienteId);
                    query.Parameters.AddWithValue("@p1", SalaId);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                asistenciaCliente.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                asistenciaCliente.ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]);
                                asistenciaCliente.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                asistenciaCliente.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                asistenciaCliente.TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]);
                                asistenciaCliente.TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]);
                                asistenciaCliente.ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]);
                                asistenciaCliente.CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]);
                                asistenciaCliente.JuegoMaquina = ManejoNulos.ManageNullStr(dr["JuegoMaquina"]);
                                asistenciaCliente.MarcaMaquina = ManejoNulos.ManageNullStr(dr["MarcaMaquina"]);
                                asistenciaCliente.TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]);
                            }
                        }
                    }
                    if(asistenciaCliente.Id != 0) {
                        SetCliente(asistenciaCliente, con);
                        SetSala(asistenciaCliente, con);
                        SetTipoFrecuencia(asistenciaCliente, con);
                        SetTipoJuego(asistenciaCliente, con);
                        SetTipoCliente(asistenciaCliente, con);
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                asistenciaCliente.Id = 0;
            }
            return asistenciaCliente;
        }
        public bool EliminarAsistenciaClienteSala(int AsistenciaId) {

            string consulta = @"DELETE FROM [dbo].[AST_AsistenciaClienteSala]
                                WHERE Id=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", AsistenciaId);

                    query.ExecuteNonQuery();
                    return true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private void SetCliente(AST_AsistenciaClienteSalaEntidad asistencia, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                                          ,[NroDoc]
                                          ,[Nombre]
                                          ,[ApelPat]
                                          ,[ApelMat]
                                          ,[Genero]
                                          ,[Celular1]
                                          ,[Celular2]
                                          ,[Mail]
                                          ,[FechaNacimiento]
                                          ,[AsistioDespuesCuarentena]
                                          ,[TipoDocumentoId]
                                          ,[UbigeoProcedenciaId]
                                          ,[Estado],[NombreCompleto]   
                                      FROM [dbo].[AST_Cliente] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", asistencia.ClienteId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    asistencia.Cliente = new AST_ClienteEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        ApelPat = ManejoNulos.ManageNullStr(reader["ApelPat"]),
                        ApelMat = ManejoNulos.ManageNullStr(reader["ApelMat"]),
                        NombreCompleto = ManejoNulos.ManageNullStr(reader["NombreCompleto"]),
                    };
                }
            };
        }
        private void SetSala(AST_AsistenciaClienteSalaEntidad asistencia, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [CodSala]
                                          ,[CodEmpresa]
                                          ,[CodUbigeo]
                                          ,[Nombre]
                                          ,[NombreCorto]
                                          ,[Direccion]
                                          ,[FechaAniversario]
                                          ,[UrlSistemaOnline]
                                          ,[NroMaquinasRD]
                                          ,[FechaRegistro]
                                          ,[FechaModificacion]
                                          ,[Activo]
                                          ,[Estado]
                                          ,[CodRD]
                                          ,[CodUsuario]
                                          ,[CodRRHH]
                                          ,[NroActasSorteos]
                                          ,[CodRRHHTecnicos]
                                          ,[RutaArchivoLogo]
                                          ,[CodOB]
                                          ,[UrlProgresivo]
                                          ,[IpPublica]
                                          ,[UrlCuadre]
                                          ,[UrlPlayerTracking]
                                          ,[NombresAdministrador]
                                          ,[ApellidosAdministrador]
                                          ,[DniAdministrador]
                                          ,[FirmaAdministrador]
                                          ,[CodigoEstablecimiento]
                                          ,[CodRegion]
                                          ,[UrlBoveda]
                                          ,[UrlSalaOnline]
                                      FROM [dbo].[Sala] where CodSala=@p0", context);
            command.Parameters.AddWithValue("@p0", asistencia.SalaId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    asistencia.Sala = new SalaEntidad() {
                        CodSala = ManejoNulos.ManageNullInteger(reader["CodSala"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        NombreCorto = ManejoNulos.ManageNullStr(reader["NombreCorto"]),
                    };
                }
            };
        }
        private void SetTipoJuego(AST_AsistenciaClienteSalaEntidad asistencia, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                      ,[Nombre]
                      ,[Descripcion]
                      ,[Estado]
                  FROM [dbo].[AST_TipoJuego] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", asistencia.TipoJuegoId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    asistencia.TipoJuego = new AST_TipoJuegoEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion = ManejoNulos.ManageNullStr(reader["Descripcion"]),
                        Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                    };
                }
            };
        }
        private void SetTipoFrecuencia(AST_AsistenciaClienteSalaEntidad asistencia, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                      ,[Nombre]
                      ,[Descripcion]
                      ,[Estado]
                  FROM [dbo].[AST_TipoFrecuencia] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", asistencia.TipoFrecuenciaId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    asistencia.TipoFrecuencia = new AST_TipoFrecuenciaEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion = ManejoNulos.ManageNullStr(reader["Descripcion"]),
                        Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                    };
                }
            };
        }
        private void SetTipoCliente(AST_AsistenciaClienteSalaEntidad asistencia, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                      ,[Nombre]
                      ,[Descripcion]
                      ,[Estado]
                  FROM [dbo].[AST_TipoCliente] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", asistencia.TipoClienteId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    asistencia.TipoCliente = new AST_TipoClienteEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion = ManejoNulos.ManageNullStr(reader["Descripcion"]),
                        Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                    };
                }
            };
        }
    }
}
