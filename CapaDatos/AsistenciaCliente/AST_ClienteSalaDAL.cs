using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.AsistenciaCliente {
    public class AST_ClienteSalaDAL {
        string _conexion = string.Empty;

        public AST_ClienteSalaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        
        public bool GuardarClienteSala(AST_ClienteSalaEntidad clienteSala) {
            bool respuesta = false;
            string consulta = @"
                INSERT INTO [dbo].[AST_ClienteSala](
                    [ClienteId]
                    ,[SalaId]
                    ,[TipoFrecuenciaId]
                    ,[TipoJuegoId]
                    ,[TipoClienteId]
                    ,[TipoRegistro]
                    ,[EnviaNotificacionWhatsapp]
                    ,[EnviaNotificacionSms]
                    ,[EnviaNotificacionEmail]
                    ,[LlamadaCelular]
                ) VALUES (
                    @p0
                    ,@p1
                    ,@p2
                    ,@p3
                    ,@p4
                    ,@p5
                    ,@p6
                    ,@p7
                    ,@p8
                    ,@p9
                )
            ;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(clienteSala.ClienteId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(clienteSala.SalaId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(clienteSala.TipoFrecuenciaId));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(clienteSala.TipoJuegoId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(clienteSala.TipoClienteId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(clienteSala.TipoRegistro));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManegeNullBool(clienteSala.EnviaNotificacionWhatsapp));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManegeNullBool(clienteSala.EnviaNotificacionSms));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManegeNullBool(clienteSala.EnviaNotificacionEmail));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManegeNullBool(clienteSala.LlamadaCelular));
                    //IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }
        
        public bool EditarClienteSala(AST_ClienteSalaEntidad clienteSala) {
            bool respuesta = false;
            string consulta = @"
                UPDATE [dbo].[AST_ClienteSala]
                SET 
                    [TipoFrecuenciaId] = @p0
                    ,[TipoJuegoId] = @p1
                    ,[TipoClienteId]=@p4
                WHERE [ClienteId] = @p2 and [SalaId] = @p3
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(clienteSala.TipoFrecuenciaId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(clienteSala.TipoJuegoId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(clienteSala.ClienteId));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(clienteSala.SalaId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(clienteSala.TipoClienteId));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }
        
        public bool EditarClienteSalaCompleto(AST_ClienteSalaEntidad clienteSala) {
            bool respuesta = false;
            string consulta = @"
                UPDATE [dbo].[AST_ClienteSala]
                SET 
                    [TipoFrecuenciaId] = @p0
                    ,[TipoJuegoId] = @p1
                    ,[ApuestaImportante]=@p2
                    ,[TipoClienteId]=@p5
                WHERE [ClienteId] = @p3 and [SalaId] = @p4
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(clienteSala.TipoFrecuenciaId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(clienteSala.TipoJuegoId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDouble(clienteSala.ApuestaImportante));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(clienteSala.ClienteId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(clienteSala.SalaId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(clienteSala.TipoClienteId));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }
        
        public List<AST_ClienteSalaEntidad> GetListadoClienteSala(int ClienteId) {
            List<AST_ClienteSalaEntidad> lista = new List<AST_ClienteSalaEntidad>();
            string consulta = @"
                SELECT 
                    cs.[ClienteId]
                    ,cs.[SalaId]
                    ,cs.[TipoFrecuenciaId]
                    ,cs.[TipoJuegoId]
                    ,cs.[ApuestaImportante]
                    ,cs.[TipoClienteId]
                    ,cs.[FechaRegistro]
                    ,cs.[TipoRegistro]
                    ,cs.[EnviaNotificacionWhatsapp]
                    ,cs.[EnviaNotificacionSms]
                    ,cs.[EnviaNotificacionEmail]
                    ,cs.[LlamadaCelular]
                    ,s.[Nombre] AS NombreSala
                FROM [dbo].[AST_ClienteSala] AS cs
                INNER JOIN Sala AS s ON cs.SalaId = s.CodSala
                WHERE cs.clienteId=@p0
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ClienteId);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var clienteSala = new AST_ClienteSalaEntidad {
                                ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]),
                                TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]),
                                ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]),
                                TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]),
                                EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]),
                                EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]),
                                EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]),
                                LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]),
                                Sala = new SalaEntidad {
                                    CodSala = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                }
                            };

                            lista.Add(clienteSala);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        
        public AST_ClienteSalaEntidad GetClienteSalaID(int ClienteId, int SalaId) {
            AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad();
            string consulta = @"
                SELECT 
                    [ClienteId]
                    ,[SalaId]
                    ,[TipoFrecuenciaId]
                    ,[TipoJuegoId]
                    ,[ApuestaImportante]
                    ,[TipoClienteId]
                    ,[FechaRegistro]
                    ,[TipoRegistro]
                    ,[EnviaNotificacionWhatsapp]
                    ,[EnviaNotificacionSms]
                    ,[EnviaNotificacionEmail]
                    ,[LlamadaCelular]
                FROM [dbo].[AST_ClienteSala]
                WHERE clienteId=@p0 AND SalaId=@p1
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ClienteId);
                    query.Parameters.AddWithValue("@p1", SalaId);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                clienteSala.ClienteId = ManejoNulos.ManageNullInteger(dr["ClienteId"]);
                                clienteSala.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                clienteSala.TipoFrecuenciaId = ManejoNulos.ManageNullInteger(dr["TipoFrecuenciaId"]);
                                clienteSala.TipoJuegoId = ManejoNulos.ManageNullInteger(dr["TipoJuegoId"]);
                                clienteSala.ApuestaImportante = ManejoNulos.ManageNullDouble(dr["ApuestaImportante"]);
                                clienteSala.TipoClienteId = ManejoNulos.ManageNullInteger(dr["TipoClienteId"]);
                                clienteSala.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                clienteSala.TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]);
                                clienteSala.EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]);
                                clienteSala.EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]);
                                clienteSala.EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]);
                                clienteSala.LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]);
                            }
                        }
                    };
                    SetTipoFrecuencia(clienteSala, con);
                    SetTipoJuego(clienteSala, con);
                    SetTipoCliente(clienteSala, con);
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return clienteSala;
        }
        
        private void SetTipoJuego(AST_ClienteSalaEntidad clienteSala, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                      ,[Nombre]
                      ,[Descripcion]
                      ,[Estado]
                  FROM [dbo].[AST_TipoJuego] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", clienteSala.TipoJuegoId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    clienteSala.TipoJuego = new AST_TipoJuegoEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion = ManejoNulos.ManageNullStr(reader["Descripcion"]),
                        Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                    };
                }
            };
        }
        private void SetTipoFrecuencia(AST_ClienteSalaEntidad clienteSala, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                      ,[Nombre]
                      ,[Descripcion]
                      ,[Estado]
                  FROM [dbo].[AST_TipoFrecuencia] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", clienteSala.TipoFrecuenciaId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    clienteSala.TipoFrecuencia = new AST_TipoFrecuenciaEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion = ManejoNulos.ManageNullStr(reader["Descripcion"]),
                        Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                    };
                }
            };
        }
        private void SetTipoCliente(AST_ClienteSalaEntidad clienteSala, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                      ,[Nombre]
                      ,[Descripcion]
                      ,[Estado]
                  FROM [dbo].[AST_TipoCliente] where Id=@p0", context);
            command.Parameters.AddWithValue("@p0", clienteSala.TipoClienteId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    clienteSala.TipoCliente = new AST_TipoClienteEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                        Descripcion = ManejoNulos.ManageNullStr(reader["Descripcion"]),
                        Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                    };
                }
            };
        }


        public int ObtenerTotalRegistrosFiltrados(string WhereQuery, string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            int TotalRegistrosFiltrados = 0;
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

                SELECT COUNT(*)
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
                        cliente.Celular1 as Celular,
                        ISNULL(cliente.Mail,'') as Mail,
                        FORMAT(cliente.FechaNacimiento,'dd-MM-yyyy') as FechaNacimiento, 
                        FORMAT(ISNULL(clienteSala.FechaRegistro, cliente.FechaRegistro),'dd-MM-yyyy HH:mm','en-eu') as FechaRegistro,
                        clienteSala.EnviaNotificacionWhatsapp,
                        clienteSala.EnviaNotificacionSms,
                        clienteSala.EnviaNotificacionEmail,
                        clienteSala.LlamadaCelular,
                        CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata
                    FROM [dbo].[AST_ClienteSala] AS clienteSala
                    INNER JOIN [dbo].[AST_Cliente] AS cliente on cliente.Id = clienteSala.ClienteId
                    INNER JOIN [dbo].[Sala] AS sala ON sala.CodSala = clienteSala.SalaId
                    LEFT JOIN [dbo].[AST_TipoDocumento] AS td ON td.Id = cliente.TipoDocumentoId
                    LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON ludo.DNI = TRIM(cliente.NroDoc) AND ludo.Estado = 1
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
                        cliente.Celular1 as Celular,
                        ISNULL(cliente.Mail,'') as Mail,
                        FORMAT(cliente.FechaNacimiento,'dd-MM-yyyy') as FechaNacimiento,
                        FORMAT(cliente.FechaRegistro,'dd-MM-yyyy HH:mm','en-eu') as FechaRegistro,
                        ISNULL(clienteSala.EnviaNotificacionWhatsapp, 1) AS EnviaNotificacionWhatsapp,
                        ISNULL(clienteSala.EnviaNotificacionSms, 1) AS EnviaNotificacionSms,
                        ISNULL(clienteSala.EnviaNotificacionEmail, 1) AS EnviaNotificacionEmail,
                        ISNULL(clienteSala.LlamadaCelular, 1) AS LlamadaCelular,
                        CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata
                    FROM [dbo].[AST_Cliente] AS cliente
                    INNER JOIN [dbo].[Sala] AS sala ON sala.CodSala = cliente.SalaId
                    LEFT JOIN [dbo].[AST_ClienteSala] AS clienteSala ON clienteSala.ClienteId = cliente.Id AND clienteSala.SalaId = cliente.SalaId
                    LEFT JOIN [dbo].[AST_TipoDocumento] AS td ON td.Id = cliente.TipoDocumentoId
                    LEFT JOIN [dbo].[CAL_Ludopata] AS ludo ON ludo.DNI = TRIM(cliente.NroDoc) AND ludo.Estado = 1
                    WHERE sala.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)
                ) AS cliente
                WHERE (CONVERT(date, cliente.FechaRegistro) between CONVERT(date, @p1) and CONVERT(date, @p2)) {WhereQuery}
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    TotalRegistrosFiltrados = (int)query.ExecuteScalar();
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return TotalRegistrosFiltrados;
        }
        public int ObtenerTotalRegistros() {
            int TotalRegistros = 0;
            string consulta = @"select count(*) from AST_Cliente";
            try {

                using(SqlConnection con = new SqlConnection(_conexion)) {
                    SqlCommand cmd = new
                        SqlCommand(consulta, con);
                    con.Open();
                    TotalRegistros = (int)cmd.ExecuteScalar();
                }
                return TotalRegistros;
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return TotalRegistros;
        }
        public List<AST_ClienteSala> GetAllClientesFiltrados(string WhereQuery, string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
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
                WHERE CONVERT(date, cliente.FechaRegistro) between @p1 and @p2 {WhereQuery}
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var clienteSala = new AST_ClienteSala {
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
                            lista.Add(clienteSala);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<AST_ClienteSala>();
            }
            return lista;
        }
        
        public bool GuardarClienteSalaConFecha(AST_ClienteSalaEntidad clienteSala) {
            bool respuesta = false;
            string consulta = @"
                INSERT INTO [dbo].[AST_ClienteSala](
                    [ClienteId]
                    ,[SalaId]
                    ,[TipoFrecuenciaId]
                    ,[TipoJuegoId]
                    ,[TipoClienteId]
                    ,[FechaRegistro]
                    ,[TipoRegistro]
                    ,[EnviaNotificacionWhatsapp]
                    ,[EnviaNotificacionSms]
                    ,[EnviaNotificacionEmail]
                    ,[LlamadaCelular]
                ) VALUES (
                    @p0
                    ,@p1
                    ,@p2
                    ,@p3
                    ,@p4
                    ,@p5
                    ,@p6
                    ,@p7
                    ,@p8
                    ,@p9
                    ,@p10
                )
            ;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(clienteSala.ClienteId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(clienteSala.SalaId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(clienteSala.TipoFrecuenciaId));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(clienteSala.TipoJuegoId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(clienteSala.TipoClienteId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(clienteSala.FechaRegistro));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(clienteSala.TipoRegistro));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManegeNullBool(clienteSala.EnviaNotificacionWhatsapp));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManegeNullBool(clienteSala.EnviaNotificacionSms));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManegeNullBool(clienteSala.EnviaNotificacionEmail));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManegeNullBool(clienteSala.LlamadaCelular));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }

        #region Envio Notificacion
        public int ActualizarEnvioNotificacionWhatsapp(int idCliente, int codSala, bool enviaNotificacion) {
            int idActualizado;
            string consulta = @"
                UPDATE AST_ClienteSala
                SET EnviaNotificacionWhatsapp = @EnviaNotificacionWhatsapp
                OUTPUT INSERTED.ClienteId
                WHERE ClienteId = @ClienteId AND SalaId = @SalaId
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@EnviaNotificacionWhatsapp", enviaNotificacion);
                    query.Parameters.AddWithValue("@ClienteId", idCliente);
                    query.Parameters.AddWithValue("@SalaId", codSala);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int ActualizarEnvioNotificacionSms(int idCliente, int codSala, bool enviaNotificacion) {
            int idActualizado;
            string consulta = @"
                UPDATE AST_ClienteSala
                SET EnviaNotificacionSms = @EnviaNotificacionSms
                OUTPUT INSERTED.ClienteId
                WHERE ClienteId = @ClienteId AND SalaId = @SalaId
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@EnviaNotificacionSms", enviaNotificacion);
                    query.Parameters.AddWithValue("@ClienteId", idCliente);
                    query.Parameters.AddWithValue("@SalaId", codSala);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int ActualizarEnvioNotificacionEmail(int idCliente, int codSala, bool enviaNotificacion) {
            int idActualizado;
            string consulta = @"
                UPDATE AST_ClienteSala
                SET EnviaNotificacionEmail = @EnviaNotificacionEmail
                OUTPUT INSERTED.ClienteId
                WHERE ClienteId = @ClienteId AND SalaId = @SalaId
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@EnviaNotificacionEmail", enviaNotificacion);
                    query.Parameters.AddWithValue("@ClienteId", idCliente);
                    query.Parameters.AddWithValue("@SalaId", codSala);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int ActualizarLlamadaCelular(int idCliente, int codSala, bool llamadaCelular) {
            int idActualizado;
            string consulta = @"
                UPDATE AST_ClienteSala
                SET LlamadaCelular = @LlamadaCelular
                OUTPUT INSERTED.ClienteId
                WHERE ClienteId = @ClienteId AND SalaId = @SalaId
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@LlamadaCelular", llamadaCelular);
                    query.Parameters.AddWithValue("@ClienteId", idCliente);
                    query.Parameters.AddWithValue("@SalaId", codSala);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public List<AST_ClienteSala> ObtenerClientesParaEnvioNotificacion(string whereQuery, string codsSalas) {
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
                WHERE sala.CodSala IN ({codsSalas});

                SELECT 
                    astc.Id AS IdCliente,
                    astcs.SalaId AS CodSala,
                    s.Nombre AS NombreSala,
                    TRIM(astc.NroDoc) AS NumeroDocumento,
                    astc.NombreCompleto AS NombreCompleto,
                    astcs.TipoRegistro AS TipoRegistro,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE astcs.EnviaNotificacionWhatsapp END AS EnviaNotificacionWhatsapp,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE astcs.EnviaNotificacionSms END AS EnviaNotificacionSms,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE astcs.EnviaNotificacionEmail END AS EnviaNotificacionEmail,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE astcs.LlamadaCelular END AS LlamadaCelular,
                    CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                    CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                    CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                FROM AST_ClienteSala AS astcs
                INNER JOIN Sala AS s ON s.CodSala = astcs.SalaId
                INNER JOIN AST_Cliente AS astc ON astc.Id = astcs.ClienteId
                LEFT JOIN CAL_Ludopata AS ludo ON TRIM(ludo.DNI) = TRIM(astc.NroDoc) AND ludo.Estado = 1
                LEFT JOIN CAL_Timador AS tima ON TRIM(tima.DNI) = TRIM(astc.NroDoc) AND tima.Estado = 1
                LEFT JOIN CAL_RobaStackersBilletero AS rsb ON TRIM(rsb.DNI) = TRIM(astc.NroDoc) AND rsb.Estado = 1
                WHERE (s.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)) AND ({whereQuery})

                UNION

                SELECT 
                    astc.Id AS IdCliente,
                    astc.SalaId AS CodSala,
                    s.Nombre AS NombreSala,
                    TRIM(astc.NroDoc) AS NumeroDocumento,
                    astc.NombreCompleto AS NombreCompleto,
                    astc.TipoRegistro AS TipoRegistro,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(astcs.EnviaNotificacionWhatsapp, 1) END AS EnviaNotificacionWhatsapp,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(astcs.EnviaNotificacionSms, 1) END AS EnviaNotificacionSms,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(astcs.EnviaNotificacionEmail, 1) END AS EnviaNotificacionEmail,
                    CASE WHEN ludo.LudopataID IS NOT NULL OR tima.TimadorID IS NOT NULL OR rsb.RobaStackersBilleteroID IS NOT NULL THEN 0 ELSE ISNULL(astcs.LlamadaCelular, 1) END AS LlamadaCelular,
                    CASE WHEN ludo.LudopataID IS NULL THEN 0 ELSE 1 END AS EsLudopata,
                    CASE WHEN tima.TimadorID IS NULL THEN 0 ELSE 1 END AS EsProhibido,
                    CASE WHEN rsb.RobaStackersBilleteroID IS NULL THEN 0 ELSE 1 END AS EsRobaStacker
                FROM AST_Cliente AS astc
                INNER JOIN Sala AS s ON s.CodSala = astc.SalaId
                LEFT JOIN AST_ClienteSala AS astcs ON astcs.ClienteId = astc.Id AND astcs.SalaId = astc.SalaId
                LEFT JOIN CAL_Ludopata AS ludo ON TRIM(ludo.DNI) = TRIM(astc.NroDoc) AND ludo.Estado = 1
                LEFT JOIN CAL_Timador AS tima ON TRIM(tima.DNI) = TRIM(astc.NroDoc) AND tima.Estado = 1
                LEFT JOIN CAL_RobaStackersBilletero AS rsb ON TRIM(rsb.DNI) = TRIM(astc.NroDoc) AND rsb.Estado = 1
                WHERE (s.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)) AND ({whereQuery})
                ORDER BY astc.Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            AST_ClienteSala clienteSala = new AST_ClienteSala {
                                codSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                IdCliente = ManejoNulos.ManageNullInteger(dr["IdCliente"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]),
                                EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]),
                                EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]),
                                EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]),
                                LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]),
                                EsLudopata = ManejoNulos.ManegeNullBool(dr["EsLudopata"]),
                                EsProhibido = ManejoNulos.ManegeNullBool(dr["EsProhibido"]),
                                EsRobaStacker = ManejoNulos.ManegeNullBool(dr["EsRobaStacker"]),
                            };
                            lista.Add(clienteSala);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<AST_ClienteSala>();
            }
            return lista;
        }
        #endregion
    }
}
