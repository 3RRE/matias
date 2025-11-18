using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente.DataWarehouse;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;

namespace CapaDatos.AsistenciaCliente {
    public class AST_ClienteDAL {
        string _conexion = string.Empty;

        public AST_ClienteDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        //public List<AST_ClienteEntidad> GetListadoCliente()
        //{
        //    List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
        //    string consulta = @"SELECT [Id]
        //                              ,[NroDoc]
        //                              ,[Nombre]
        //                              ,[ApelPat]
        //                              ,[ApelMat]
        //                              ,[Genero]
        //                              ,[Celular1]
        //                              ,[Celular2]
        //                              ,[Mail]
        //                              ,[FechaNacimiento]
        //                              ,[AsistioDespuesCuarentena]
        //                              ,[TipoDocumentoId]
        //                              ,[UbigeoProcedenciaId]
        //                              ,[Estado],[NombreCompleto]
        //                      FROM [dbo].[AST_Cliente]";
        //    try
        //    {
        //        using (var con = new SqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new SqlCommand(consulta, con);


        //            using (var dr = query.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    var cliente = new AST_ClienteEntidad
        //                    {
        //                        Id = ManejoNulos.ManageNullInteger(dr["Id"]),
        //                        NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
        //                        Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
        //                        ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
        //                        ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
        //                        Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
        //                        Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
        //                        Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
        //                        Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
        //                        FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
        //                        AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
        //                        TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
        //                        UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
        //                        Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
        //                        NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
        //                    };

        //                    lista.Add(cliente);
        //                }
        //            }
        //            foreach(var cliente in lista)
        //            {
        //                if (cliente.TipoDocumentoId!=0 && cliente.UbigeoProcedenciaId!=0)
        //                {
        //                    SetTipoDocumento(cliente, con);
        //                    SetUbigeoProcedencia(cliente, con);
        //                }
        //                //SetUbigeoRegistro(cliente, con);
        //                //SetSalaRegistro(cliente, con);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return lista;
        //}
        public List<AST_ClienteEntidad> GetListadoCliente() {
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
            string consulta = @"SELECT cliente.[Id]
                                        ,cliente.[NroDoc]
                                        ,cliente.[Nombre]
                                        ,cliente.[ApelPat]
                                        ,cliente.[ApelMat]
                                        ,cliente.[Genero]
                                        ,cliente.[Celular1]
                                        ,cliente.[Celular2]
                                        ,cliente.[Mail]
                                        ,cliente.[FechaNacimiento]
                                        ,cliente.[AsistioDespuesCuarentena]
                                        ,cliente.[TipoDocumentoId]
                                        ,cliente.[UbigeoProcedenciaId]
                                        ,cliente.[Estado]
                                        ,cliente.[NombreCompleto]
                                        ,tipodocumento.Nombre as TipoDocumento
                                        ,ubigeo.Nombre as NombreUbigeo
                                    FROM [dbo].[AST_Cliente] as cliente
                                    left join dbo.AST_TipoDocumento as tipodocumento
                                    on cliente.TipoDocumentoId=tipodocumento.Id
                                    left join Ubigeo as ubigeo on cliente.UbigeoProcedenciaId=ubigeo.CodUbigeo order by cliente.Id desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var cliente = new AST_ClienteEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                TipoDocumento = new AST_TipoDocumentoEntidad() {
                                    Nombre = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                    Id = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                },
                                UbigeoProcedencia = new UbigeoEntidad() {
                                    Nombre = ManejoNulos.ManageNullStr(dr["NombreUbigeo"]),
                                    CodUbigeo = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                }
                            };

                            lista.Add(cliente);
                        }
                    }
                    //foreach (var cliente in lista)
                    //{
                    //    if (cliente.TipoDocumentoId != 0 && cliente.UbigeoProcedenciaId != 0)
                    //    {
                    //        SetTipoDocumento(cliente, con);
                    //        SetUbigeoProcedencia(cliente, con);
                    //    }
                    //    //SetUbigeoRegistro(cliente, con);
                    //    //SetSalaRegistro(cliente, con);
                    //}
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<AST_ClienteEntidad> GetListadoClienteFiltrados(string whereQuery) {
            List<AST_ClienteEntidad> customerList = new List<AST_ClienteEntidad>();

            string query = @"
            SELECT
                cliente.Id,
                cliente.Nombre,
                cliente.ApelPat,
                cliente.ApelMat,
                cliente.NombreCompleto,
                cliente.TipoDocumentoId,
                tipodocumento.Nombre AS TipoDocumento,
                cliente.NroDoc,
                cliente.FechaNacimiento,
                Convert(Integer, DATEDIFF(YEAR, cliente.FechaNacimiento, GETDATE())) AS 'Edad',
                cliente.Genero,
                cliente.codigoPais,
                cliente.Celular1,
                cliente.Celular2,
                cliente.Mail,
                cliente.AsistioDespuesCuarentena,
                cliente.Estado,
                cliente.UbigeoProcedenciaId,
                ubigeo.Nombre as NombreUbigeo
            FROM dbo.AST_Cliente AS cliente
            LEFT JOIN dbo.AST_TipoDocumento AS tipodocumento ON cliente.TipoDocumentoId = tipodocumento.Id
            LEFT JOIN Ubigeo AS ubigeo ON cliente.UbigeoProcedenciaId = ubigeo.CodUbigeo
            " + whereQuery;

            try {
                using(SqlConnection conecction = new SqlConnection(_conexion)) {
                    conecction.Open();
                    SqlCommand command = new SqlCommand(query, conecction);
                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            AST_ClienteEntidad customer = new AST_ClienteEntidad {
                                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(data["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(data["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(data["ApelMat"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(data["NombreCompleto"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(data["TipoDocumentoId"]),
                                NroDoc = ManejoNulos.ManageNullStr(data["NroDoc"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(data["FechaNacimiento"]),
                                Edad = ManejoNulos.ManageNullInteger(data["Edad"]),
                                Genero = ManejoNulos.ManageNullStr(data["Genero"]),
                                CodigoPais = ManejoNulos.ManageNullStr(data["codigoPais"]),
                                Celular1 = ManejoNulos.ManageNullStr(data["Celular1"]),
                                Celular2 = ManejoNulos.ManageNullStr(data["Celular2"]),
                                Mail = ManejoNulos.ManageNullStr(data["Mail"]),
                                AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(data["AsistioDespuesCuarentena"]),
                                Estado = ManejoNulos.ManageNullStr(data["Estado"]),
                                UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(data["UbigeoProcedenciaId"]),
                                TipoDocumento = new AST_TipoDocumentoEntidad() {
                                    Id = ManejoNulos.ManageNullInteger(data["TipoDocumentoId"]),
                                    Nombre = ManejoNulos.ManageNullStr(data["TipoDocumento"])
                                },
                                UbigeoProcedencia = new UbigeoEntidad() {
                                    CodUbigeo = ManejoNulos.ManageNullInteger(data["UbigeoProcedenciaId"]),
                                    Nombre = ManejoNulos.ManageNullStr(data["NombreUbigeo"])
                                }
                            };

                            customerList.Add(customer);
                        }
                    }
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return customerList;
        }

        public int GetListadoClienteFiltradosTotal(string whereQuery) {
            int total = 0;

            string query = @"
            SELECT COUNT(*) AS 'Total'
            FROM dbo.AST_Cliente AS cliente
            LEFT JOIN dbo.AST_TipoDocumento AS tipodocumento ON cliente.TipoDocumentoId = tipodocumento.Id
            LEFT JOIN Ubigeo AS ubigeo ON cliente.UbigeoProcedenciaId = ubigeo.CodUbigeo
            " + whereQuery;

            try {
                using(SqlConnection conecction = new SqlConnection(_conexion)) {
                    conecction.Open();
                    SqlCommand command = new SqlCommand(query, conecction);
                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.Read()) {
                            total = ManejoNulos.ManageNullInteger(data["Total"]);
                        }
                    }
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return total;
        }

        public List<AST_ClienteEntidad> GetListadoClienteCoincidencia(string coincidencia, string campo) {
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();

            string consulta = @"SELECT [Id]
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
                                 FROM [dbo].[AST_Cliente] where [" + campo + "] like @p2 order by [Nombre] asc;";
            //   string consulta = @"SELECT [Id]
            //                             ,[NroDoc]
            //                             ,[Nombre]
            //                             ,[ApelPat]
            //                             ,[ApelMat]
            //                             ,[Genero]
            //                             ,[Celular1]
            //                             ,[Celular2]
            //                             ,[Mail]
            //                             ,[FechaNacimiento]
            //                             ,[AsistioDespuesCuarentena]
            //                             ,[TipoDocumentoId]
            //                             ,[UbigeoProcedenciaId]
            //                             ,[Estado]
            //                     FROM [dbo].[AST_Cliente] where (SELECT DIFFERENCE(ApelPat,@p0))>=3 
            //or (SELECT DIFFERENCE(ApelMat,@p1))>=3 or NroDoc like @p2 order by [ApelPat] asc;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@p0", coincidencia);
                    //query.Parameters.AddWithValue("@p1", coincidencia);
                    query.Parameters.AddWithValue("@p2", "%" + coincidencia + "%");

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var cliente = new AST_ClienteEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                            };

                            lista.Add(cliente);
                        }
                    }
                    foreach(var cliente in lista) {
                        SetTipoDocumento(cliente, con);
                        SetUbigeoProcedencia(cliente, con);
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<AST_ClienteEntidad> GetListadoClientePorNombresyApellidos(string coincidencia) {
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();

            string consulta = @"SELECT [Id]
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
                                 FROM [dbo].[AST_Cliente] where [NombreCompleto] like @p2 or
                                                    [ApelPat] like @p3 or
                                                    [ApelMat] like @p4 or
                                                    [Nombre] like @p5
                                                    order by [Nombre] asc;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@p0", coincidencia);
                    //query.Parameters.AddWithValue("@p1", coincidencia);
                    query.Parameters.AddWithValue("@p2", "%" + coincidencia + "%");
                    query.Parameters.AddWithValue("@p3", "%" + coincidencia + "%");
                    query.Parameters.AddWithValue("@p4", "%" + coincidencia + "%");
                    query.Parameters.AddWithValue("@p5", "%" + coincidencia + "%");

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var cliente = new AST_ClienteEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                            };

                            lista.Add(cliente);
                        }
                    }
                    foreach(var cliente in lista) {
                        SetTipoDocumento(cliente, con);
                        SetUbigeoProcedencia(cliente, con);
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public AST_ClienteEntidad GetClienteID(int ClienteId) {
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            string consulta = @"SELECT [Id]
                                      ,[NroDoc]
                                      ,[Nombre]
                                      ,[ApelPat]
                                      ,[ApelMat]
                                      ,[Genero]
                                      ,[codigoPais]
                                      ,[Celular1]
                                      ,[Celular2]
                                      ,[Mail]
                                      ,[FechaNacimiento]
                                      ,[AsistioDespuesCuarentena]
                                      ,[TipoDocumentoId]
                                      ,[UbigeoProcedenciaId]
                                      ,[Estado],[NombreCompleto],sala_vacunacion,nro_dosis,fecha_ultima_dosis,NombreCompleto,TipoRegistro
                                 ,fecha_emision,Ciudadano,PaisId, FechaRegistro
                              FROM [dbo].[AST_Cliente] where [Id]=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ClienteId);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                cliente.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                cliente.NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]);
                                cliente.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                cliente.ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]);
                                cliente.ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]);
                                cliente.Genero = ManejoNulos.ManageNullStr(dr["Genero"]);
                                cliente.CodigoPais = ManejoNulos.ManageNullStr(dr["codigoPais"]);
                                cliente.Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]);
                                cliente.Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]);
                                cliente.Mail = ManejoNulos.ManageNullStr(dr["Mail"]);
                                cliente.FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]);
                                cliente.AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]);
                                cliente.TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]);
                                cliente.UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]);
                                cliente.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                                cliente.NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]);
                                cliente.sala_vacunacion = ManejoNulos.ManageNullInteger(dr["sala_vacunacion"]);
                                cliente.nro_dosis = ManejoNulos.ManageNullInteger(dr["nro_dosis"]);
                                cliente.fecha_ultima_dosis = ManejoNulos.ManageNullDate(dr["fecha_ultima_dosis"]);
                                cliente.fecha_emision = ManejoNulos.ManageNullDate(dr["fecha_emision"]);
                                cliente.TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]);
                                cliente.Ciudadano = ManejoNulos.ManegeNullBool(dr["Ciudadano"]);
                                cliente.PaisId = ManejoNulos.ManageNullStr(dr["PaisId"]);
                                cliente.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            }
                        }
                    };

                    SetTipoDocumento(cliente, con);
                    SetUbigeoProcedencia(cliente, con);
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return cliente;
        }
        public AST_ClienteEntidad GetClientexNroyTipoDoc(int tipoDocumentoId, string NroDocumento) {
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            string consulta = @"
                SELECT 
                    cli.[Id]
                    ,TRIM(cli.[NroDoc]) as NroDoc
                    ,cli.[Nombre]
                    ,cli.[ApelPat]
                    ,cli.[ApelMat]
                    ,cli.[Genero]
                    ,cli.[Celular1]
                    ,cli.[Celular2]
                    ,cli.[Mail]
                    ,cli.[FechaNacimiento]
                    ,cli.[AsistioDespuesCuarentena]
                    ,cli.[TipoDocumentoId]
                    ,cli.[UbigeoProcedenciaId]
                    ,cli.[Estado]
                    ,cli.[NombreCompleto]
                    ,cli.[FechaRegistro]
                    ,cli.[SalaId]
                    ,s.[Nombre] AS NombreSala
                FROM [dbo].[AST_Cliente] AS cli
                LEFT JOIN Sala AS s ON s.CodSala = cli.SalaId
                WHERE cli.[TipoDocumentoId]=@p0 and TRIM(cli.[NroDoc])=TRIM(@p1)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoDocumentoId);
                    query.Parameters.AddWithValue("@p1", NroDocumento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                cliente.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                cliente.NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]);
                                cliente.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                cliente.ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]);
                                cliente.ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]);
                                cliente.Genero = ManejoNulos.ManageNullStr(dr["Genero"]);
                                cliente.Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]);
                                cliente.Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]);
                                cliente.Mail = ManejoNulos.ManageNullStr(dr["Mail"]);
                                cliente.FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]);
                                cliente.AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]);
                                cliente.TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]);
                                cliente.UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]);
                                cliente.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                                cliente.NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]);
                                cliente.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                cliente.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                cliente.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                cliente.Id = 0;
            }
            return cliente;
        }
        public AST_ClienteEntidad GetClientexNroDoc(string NroDocumento) {
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            string consulta = @"
                SELECT 
                    [Id]
                    ,TRIM([NroDoc]) as NroDoc
                    ,[Nombre]
                    ,[ApelPat]
                    ,[ApelMat]
                    ,[Genero]
                    ,[Celular1]
                    ,[Celular2]
                    ,[Mail]
                    ,[FechaNacimiento]
                    ,FechaRegistro
                    ,[AsistioDespuesCuarentena]
                    ,[TipoDocumentoId]
                    ,[UbigeoProcedenciaId]
                    ,[Estado]
                    ,SalaID,[NombreCompleto]
                    ,[TipoRegistro]
                FROM [dbo].[AST_Cliente]
                WHERE TRIM([NroDoc])=TRIM(@p0)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", NroDocumento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                cliente.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                cliente.NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]);
                                cliente.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                cliente.ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]);
                                cliente.ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]);
                                cliente.Genero = ManejoNulos.ManageNullStr(dr["Genero"]);
                                cliente.Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]);
                                cliente.Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]);
                                cliente.Mail = ManejoNulos.ManageNullStr(dr["Mail"]);
                                cliente.FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]);
                                cliente.AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]);
                                cliente.TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]);
                                cliente.UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]);
                                cliente.Estado = ManejoNulos.ManageNullStr(dr["Estado"]);
                                cliente.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                cliente.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                cliente.NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]);
                                cliente.TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                cliente.Id = 0;
            }
            return cliente;
        }
        public List<AST_ClienteEntidad> GetListaClientesxNroDoc(string NroDocumento) {
            List<AST_ClienteEntidad> ListaClientes = new List<AST_ClienteEntidad>();
            string consulta = @"SELECT cliente.[Id]
                                      ,cliente.[NroDoc]
                                      ,cliente.[Nombre]
                                      ,cliente.[ApelPat]
                                      ,cliente.[ApelMat]
                                      ,cliente.[Genero]
                                      ,cliente.[Celular1]
                                      ,cliente.[Celular2]
                                      ,cliente.[Mail]
                                      ,cliente.[FechaNacimiento]
                                        ,cliente.FechaRegistro
                                      ,cliente.[AsistioDespuesCuarentena]
                                      ,cliente.[TipoDocumentoId]
                                      ,cliente.[UbigeoProcedenciaId]
                                      ,cliente.[Estado]
                                        ,cliente.SalaID,cliente.[NombreCompleto]
                                            ,tipodocumento.Nombre as TipoDocumento
                                        ,ubigeo.Nombre as NombreUbigeo
                              FROM [dbo].[AST_Cliente] as cliente 
                                left join dbo.AST_TipoDocumento as tipodocumento
                                on cliente.TipoDocumentoId=tipodocumento.Id
                                left join Ubigeo as ubigeo on cliente.UbigeoProcedenciaId=ubigeo.CodUbigeo
                              where TRIM([NroDoc])=TRIM(@p0)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", NroDocumento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var cliente = new AST_ClienteEntidad() {
                                    Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                    NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                    ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                    Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                    Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                    Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                    Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                    FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                    AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
                                    TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                    TipoDocumento = new AST_TipoDocumentoEntidad() {
                                        Nombre = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                        Id = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    },
                                    UbigeoProcedencia = new UbigeoEntidad() {
                                        Nombre = ManejoNulos.ManageNullStr(dr["NombreUbigeo"]),
                                        CodUbigeo = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    }
                                };
                                ListaClientes.Add(cliente);
                            }
                        }
                    };
                }
            } catch(Exception) {
                ListaClientes = new List<AST_ClienteEntidad>();
            }
            return ListaClientes;
        }


        public List<AST_ClienteEntidad> GetListaClientesxNroDocMetodoBusqueda(string NroDocumento) {
            List<AST_ClienteEntidad> ListaClientes = new List<AST_ClienteEntidad>();
            string consulta = @"SELECT TOP (5) cliente.[Id]
                                      ,cliente.[NroDoc]
                                      ,cliente.[Nombre]
                                      ,cliente.[ApelPat]
                                      ,cliente.[ApelMat]
                                      ,cliente.[Genero]
                                      ,cliente.[Celular1]
                                      ,cliente.[Celular2]
                                      ,cliente.[Mail]
                                      ,cliente.[FechaNacimiento]
                                        ,cliente.FechaRegistro
                                      ,cliente.[AsistioDespuesCuarentena]
                                      ,cliente.[TipoDocumentoId]
                                      ,cliente.[UbigeoProcedenciaId]
                                      ,cliente.[Estado]
                                        ,cliente.SalaID,cliente.[NombreCompleto]
                              FROM [dbo].[AST_Cliente] as cliente 
                              where TRIM([NroDoc])=TRIM(@p0)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", NroDocumento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var cliente = new AST_ClienteEntidad() {
                                    Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                    NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                    ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                    Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                    Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                    Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                    Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                    FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                    AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
                                    TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                    TipoDocumento = new AST_TipoDocumentoEntidad() {
                                        Id = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    },
                                    UbigeoProcedencia = new UbigeoEntidad() {
                                        CodUbigeo = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    }
                                };
                                ListaClientes.Add(cliente);
                            }
                        }
                    };
                }
            } catch(Exception) {
                ListaClientes = new List<AST_ClienteEntidad>();
            }
            return ListaClientes;
        }

        public List<AST_ClienteEntidad> GetListaClientesxNombreMetodoBusqueda(string NombreCompleto) {
            List<AST_ClienteEntidad> ListaClientes = new List<AST_ClienteEntidad>();
            string consulta = @"SELECT TOP (50)  cliente.[Id]
                                      ,cliente.[NroDoc]
                                      ,cliente.[Nombre]
                                      ,cliente.[ApelPat]
                                      ,cliente.[ApelMat]
                                      ,cliente.[Genero]
                                      ,cliente.[Celular1]
                                      ,cliente.[Celular2]
                                      ,cliente.[Mail]
                                      ,cliente.[FechaNacimiento]
                                        ,cliente.FechaRegistro
                                      ,cliente.[AsistioDespuesCuarentena]
                                      ,cliente.[TipoDocumentoId]
                                      ,cliente.[UbigeoProcedenciaId]
                                      ,cliente.[Estado]
                                        ,cliente.SalaID,cliente.[NombreCompleto]
                              FROM [dbo].[AST_Cliente] as cliente 
                              where TRIM(cliente.NombreCompleto) like '%'+TRIM(@p0)+'%'";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", NombreCompleto);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var cliente = new AST_ClienteEntidad() {
                                    Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                    NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                    ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                    Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                    Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                    Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                    Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                    FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                    AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(dr["AsistioDespuesCuarentena"]),
                                    TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                    TipoDocumento = new AST_TipoDocumentoEntidad() {
                                        Id = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    },
                                    UbigeoProcedencia = new UbigeoEntidad() {
                                        CodUbigeo = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    }
                                };
                                ListaClientes.Add(cliente);
                            }
                        }
                    };
                }
            } catch(Exception) {
                ListaClientes = new List<AST_ClienteEntidad>();
            }
            return ListaClientes;
        }

        public int GuardarCliente(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = string.Empty;
            string fecha_ultima_dosis = cliente.fecha_ultima_dosis.ToString("dd/MM/yyyy");
            string fecha_emision = cliente.fecha_emision.ToString("dd/MM/yyyy");

            if((fecha_ultima_dosis == "01/01/0001" || fecha_ultima_dosis == "31/12/1752")) {
                cliente.fecha_ultima_dosis = Convert.ToDateTime("01/01/1753");
            }
            if((fecha_emision == "01/01/0001" || fecha_emision == "31/12/1752")) {
                cliente.fecha_emision = Convert.ToDateTime("01/01/1753");
            }

            consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
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
           ,[Estado],[FechaRegistro],[TipoRegistro],SalaID,[NombreCompleto],sala_vacunacion,nro_dosis,fecha_ultima_dosis,fecha_emision,usuario_reg,PaisId,Ciudadano)
Output Inserted.Id
     VALUES
           (@p0
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
           ,@p11
           ,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    //if (cliente.fecha_emision == null)
                    //{

                    //}SqlInt64.Null : ManejoNulos.ManageNullDate(cliente.fecha_emision))
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.Genero));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.Celular1));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Celular2));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.Mail));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(cliente.FechaNacimiento));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cliente.AsistioDespuesCuarentena));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId) == 0 ? SqlInt32.Null : cliente.TipoDocumentoId);
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.UbigeoProcedenciaId) == 0 ? SqlInt32.Null : cliente.UbigeoProcedenciaId);
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullDate(cliente.FechaRegistro));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(cliente.NombreCompleto));

                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullInteger(cliente.sala_vacunacion));
                    query.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullInteger(cliente.nro_dosis));
                    query.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullDate(cliente.fecha_ultima_dosis));
                    query.Parameters.AddWithValue("@p20", ManejoNulos.ManageNullDate(cliente.fecha_emision));
                    query.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullInteger(cliente.usuario_reg));

                    query.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullStr(cliente.PaisId));
                    query.Parameters.AddWithValue("@p23", ManejoNulos.ManegeNullBool(cliente.Ciudadano));

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


        public int GuardarClienteSinFechas(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = string.Empty;
            string fecha_ultima_dosis = cliente.fecha_ultima_dosis.ToString("dd/MM/yyyy");
            string fecha_emision = cliente.fecha_emision.ToString("dd/MM/yyyy");
            if((fecha_ultima_dosis == "01/01/0001" || fecha_ultima_dosis == "31/12/1752")) {
                cliente.fecha_ultima_dosis = Convert.ToDateTime("01/01/1753");
            }
            if((fecha_emision == "01/01/0001" || fecha_emision == "31/12/1752")) {
                cliente.fecha_emision = Convert.ToDateTime("01/01/1753");
            }
            consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
           ,[Nombre]
           ,[ApelPat]
           ,[ApelMat]
           ,[Genero]
           ,[Celular1]
           ,[Celular2]
           ,[Mail]
           ,[AsistioDespuesCuarentena]
           ,[TipoDocumentoId]
           ,[UbigeoProcedenciaId]
           ,[Estado],[TipoRegistro],SalaID,[NombreCompleto],sala_vacunacion,nro_dosis,usuario_reg,FechaRegistro)
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
           ,@p5
           ,@p6
           ,@p7
           ,@p9
           ,@p10
           ,@p11
           ,@p12,@p14,@p15,@p16,@p17,@p18,@p21,@p22
          );";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    //if (cliente.fecha_emision == null)
                    //{

                    //}SqlInt64.Null : ManejoNulos.ManageNullDate(cliente.fecha_emision))
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.Genero));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.Celular1));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Celular2));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.Mail));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cliente.AsistioDespuesCuarentena));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId) == 0 ? SqlInt32.Null : cliente.TipoDocumentoId);
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.UbigeoProcedenciaId) == 0 ? SqlInt32.Null : cliente.UbigeoProcedenciaId);
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullInteger(cliente.sala_vacunacion));
                    query.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullInteger(cliente.nro_dosis));
                    query.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullInteger(cliente.usuario_reg));
                    query.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullDate(cliente.FechaRegistro));
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



        public int GuardarClienteCampania(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
           ,[Nombre]
           ,[ApelPat]
           ,[ApelMat]
           ,[Celular1]
           ,[FechaNacimiento]
           ,[Estado],[FechaRegistro],SalaID,[NombreCompleto],Mail,usuario_reg,TipoRegistro,TipoDocumentoId)
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
           ,@p5
           ,@p6
           ,@p7
,@p8,@p9,@p10,@p11,@p12,@p13
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc.Trim()));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.Celular1));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(cliente.FechaNacimiento));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(DateTime.Now));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(cliente.Mail));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.usuario_reg));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId));
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

        public int GuardarClienteEmpadronamiento(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
           ,[Nombre]
           ,[ApelPat]
           ,[ApelMat]
           ,[Estado],[FechaRegistro],SalaID,[NombreCompleto],usuario_reg,TipoRegistro)
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p6
           ,@p7
,@p8,@p9,@p11,@p12
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(DateTime.Now));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.usuario_reg));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
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
        public bool EditarCliente(AST_ClienteEntidad cliente) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_Cliente]
                   SET [NroDoc] = @p0
                      ,[Nombre] = @p1
                      ,[ApelPat] = @p2
                      ,[ApelMat] = @p3
                      ,[Genero] = @p4
                      ,[Celular1] = @p5
                      ,[Celular2] = @p6
                      ,[Mail] = @p7
                      ,[FechaNacimiento] = @p8
                      ,[TipoDocumentoId] = @p9
                      ,[UbigeoProcedenciaId] = @p10
                        ,[NombreCompleto]=@p12
                        ,sala_vacunacion=@p13,
                        nro_dosis=@p14, PaisId=@p17, Ciudadano=@p18";
            string fecha_ultima_dosis = cliente.fecha_ultima_dosis.ToString("dd/MM/yyyy");
            string fecha_emision = cliente.fecha_emision.ToString("dd/MM/yyyy");

            if((fecha_ultima_dosis != "01/01/0001" && fecha_ultima_dosis != "31/12/1752")) {
                consulta += " ,fecha_ultima_dosis=@p15 ";
            }
            if((fecha_emision != "01/01/0001" && fecha_emision != "31/12/1752")) {
                consulta += " ,fecha_emision=@p16 ";
            }

            consulta += " WHERE Id=@p11";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.Genero));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.Celular1));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Celular2));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.Mail));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(cliente.FechaNacimiento));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cliente.UbigeoProcedenciaId));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.Id));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.NombreCompleto));

                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullInteger(cliente.sala_vacunacion));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullInteger(cliente.nro_dosis));

                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullStr(cliente.PaisId));
                    query.Parameters.AddWithValue("@p18", ManejoNulos.ManegeNullBool(cliente.Ciudadano));

                    //query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDate(cliente.fecha_ultima_dosis));
                    //query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullDate(cliente.fecha_emision));
                    if((fecha_ultima_dosis != "01/01/0001" && fecha_ultima_dosis != "31/12/1752")) {
                        query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDate(cliente.fecha_ultima_dosis));
                    }
                    if((fecha_emision != "01/01/0001" && fecha_emision != "31/12/1752")) {
                        query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullDate(cliente.fecha_emision));
                    }

                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public bool ActualizarContactoCliente(AST_ClienteEntidad cliente) {
            bool respuesta = false;
            string consulta = @"UPDATE
                    AST_Cliente
                SET 
                    codigoPais = @codigoPais,
                    Celular1 = @Celular1,
                    Celular2 = @Celular2
                WHERE
                    Id = @idCliente
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codigoPais", ManejoNulos.ManageNullStr(cliente.CodigoPais));
                    query.Parameters.AddWithValue("@Celular1", ManejoNulos.ManageNullStr(cliente.Celular1));
                    query.Parameters.AddWithValue("@Celular2", ManejoNulos.ManageNullStr(cliente.Celular2));
                    query.Parameters.AddWithValue("@idCLiente", ManejoNulos.ManageNullStr(cliente.Id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public bool EditarEstadoCliente(AST_ClienteEntidad cliente) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_Cliente]
                   SET [Estado] = @p0
                 WHERE Id=@p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", cliente.Estado);
                    query.Parameters.AddWithValue("@p1", cliente.Id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EditarAsistenciaDespuesCuarentena(AST_ClienteEntidad cliente) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[AST_Cliente]
                   SET [AsistioDespuesCuarentena] = @p0
                 WHERE Id=@p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", cliente.AsistioDespuesCuarentena);
                    query.Parameters.AddWithValue("@p1", cliente.Id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        private void SetUbigeoProcedencia(AST_ClienteEntidad cliente, SqlConnection context) {
            var command = new SqlCommand("SELECT CodUbigeo,PaisId,DepartamentoId,ProvinciaId,DistritoId,Nombre FROM Ubigeo WHERE CodUbigeo=@pCodUbigeo", context);

            command.Parameters.AddWithValue("@pCodUbigeo", cliente.UbigeoProcedenciaId);

            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    cliente.UbigeoProcedencia = new UbigeoEntidad() {
                        CodUbigeo = ManejoNulos.ManageNullInteger(reader["CodUbigeo"]),
                        PaisId = ManejoNulos.ManageNullStr(reader["PaisId"]),
                        DepartamentoId = ManejoNulos.ManageNullInteger(reader["DepartamentoId"]),
                        ProvinciaId = ManejoNulos.ManageNullInteger(reader["ProvinciaId"]),
                        DistritoId = ManejoNulos.ManageNullInteger(reader["DistritoId"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                    };
                }
            };
        }
        private void SetTipoDocumento(AST_ClienteEntidad cliente, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [Id]
                                  ,[Nombre]
                              FROM[dbo].[AST_TipoDocumento] where[Id] = @p0", context);
            command.Parameters.AddWithValue("@p0", cliente.TipoDocumentoId);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    cliente.TipoDocumento = new AST_TipoDocumentoEntidad() {
                        Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                        Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                    };
                }
            };
        }

        public int GuardarClienteLudopatas(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
           ,[Estado],[FechaRegistro],SalaID,[NombreCompleto],usuario_reg,TipoRegistro,Nombre,ApelPat,ApelMat,TipoDocumentoId)
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
           ,@p5
           ,@p6,@p7,@p8,@p9,@p10
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(cliente.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(cliente.SalaId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(cliente.usuario_reg));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId));
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

        public int GetTotalClientes() {
            int result = 0;
            string consulta = @"SELECT count(*) as total
                              FROM [dbo].[AST_Cliente]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                result = ManejoNulos.ManageNullInteger(dr["total"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return result;
        }
        public List<AST_ClienteEntidad> GetListaCumpleanios(int CodSala) {
            List<AST_ClienteEntidad> ListaClientes = new List<AST_ClienteEntidad>();
            string consulta = @" declare @fechaActual datetime = (select GETDATE())
declare @limiteEdad int = 18
select top 200
fechaCalculada =
case 
    when convert(date,(dateadd(year,year(@fechaActual)-year(fechaNacimiento),fechaNacimiento)))>=convert(date,@fechaActual) then dateadd(year,year(@fechaActual)-year(fechaNacimiento),fechaNacimiento)
    else dateadd(year,year(@fechaActual)+1-year(fechaNacimiento),fechaNacimiento)
end,
AST_Cliente.id,
AST_Cliente.NombreCompleto, 
AST_Cliente.NroDoc, 
AST_Cliente.FechaNacimiento
 from AST_Cliente
 where 
 --dateadd(year,year(getdate())-year(fechanacimiento),fechaNacimiento)>=GETDATE() and 
 year(@fechaActual)-year(fechanacimiento)>=@limiteEdad
 and SalaId=@CodSala
 order by fechacalculada asc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", CodSala);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var cliente = new AST_ClienteEntidad() {
                                    Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                    NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                    FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                };
                                ListaClientes.Add(cliente);
                            }
                        }
                    };
                }
            } catch(Exception) {
                ListaClientes = new List<AST_ClienteEntidad>();
            }
            return ListaClientes;
        }

        public List<AST_ClienteEntidad> GetListaMasivoClientesxNroDoc(List<string> numberDocuments) {

            List<AST_ClienteEntidad> customersList = new List<AST_ClienteEntidad>();

            string query = $@"
              SELECT
                cliente.Id,
                cliente.NroDoc,
                cliente.Nombre,
                cliente.ApelPat,
                cliente.ApelMat,
                cliente.Genero,
                cliente.Celular1,
                cliente.Celular2,
                cliente.Mail,
                cliente.FechaNacimiento,
                cliente.FechaRegistro,
                cliente.AsistioDespuesCuarentena,
                cliente.TipoDocumentoId,
                cliente.UbigeoProcedenciaId,
                cliente.Estado,
                cliente.SalaID,
                cliente.NombreCompleto
              FROM AST_Cliente AS cliente 
              WHERE cliente.NroDoc IN ({string.Join(",", numberDocuments.Select(x => string.Format("'{0}'", x)).ToList())})
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    using(SqlDataReader reader = command.ExecuteReader()) {
                        if(reader.HasRows) {
                            while(reader.Read()) {
                                AST_ClienteEntidad customer = new AST_ClienteEntidad() {
                                    Id = ManejoNulos.ManageNullInteger(reader["Id"]),
                                    NroDoc = ManejoNulos.ManageNullStr(reader["NroDoc"]),
                                    Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                                    ApelPat = ManejoNulos.ManageNullStr(reader["ApelPat"]),
                                    ApelMat = ManejoNulos.ManageNullStr(reader["ApelMat"]),
                                    Genero = ManejoNulos.ManageNullStr(reader["Genero"]),
                                    Celular1 = ManejoNulos.ManageNullStr(reader["Celular1"]),
                                    Celular2 = ManejoNulos.ManageNullStr(reader["Celular2"]),
                                    Mail = ManejoNulos.ManageNullStr(reader["Mail"]),
                                    FechaNacimiento = ManejoNulos.ManageNullDate(reader["FechaNacimiento"]),
                                    AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(reader["AsistioDespuesCuarentena"]),
                                    TipoDocumentoId = ManejoNulos.ManageNullInteger(reader["TipoDocumentoId"]),
                                    UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(reader["UbigeoProcedenciaId"]),
                                    Estado = ManejoNulos.ManageNullStr(reader["Estado"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(reader["FechaRegistro"]),
                                    SalaId = ManejoNulos.ManageNullInteger(reader["SalaId"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(reader["NombreCompleto"])
                                };

                                customersList.Add(customer);
                            }
                        }
                    };
                }
            } catch(Exception) {
                customersList = new List<AST_ClienteEntidad>();
            }

            return customersList;
        }
        public List<dynamic> GetTotalClientesPorAnio(int anio) {
            List<dynamic> listaDynamics = new List<dynamic>();
            string query = $@"
             SELECT
                  DATEPART(YEAR, FechaRegistro) AS year,
                  DATEPART(MONTH, FechaRegistro) AS month,
                  COUNT(id) AS count
                FROM ast_cliente
                where FechaRegistro is not null
                and year(FechaRegistro)=@anio
                GROUP BY
                  DATEPART(MONTH, FechaRegistro),
                  DATEPART(YEAR, FechaRegistro)
                  order by year,month
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@anio", anio);

                    using(SqlDataReader reader = command.ExecuteReader()) {
                        if(reader.HasRows) {
                            while(reader.Read()) {
                                int mes = ManejoNulos.ManageNullInteger(reader["month"]);
                                int cantidad = ManejoNulos.ManageNullInteger(reader["count"]);
                                object obj = new {
                                    mes = mes,
                                    cantidad = cantidad,
                                };
                                listaDynamics.Add(obj);
                            }
                        }
                    };
                }
            } catch(Exception) {
                listaDynamics = new List<dynamic>();
            }

            return listaDynamics;
        }

        public int GuardarClienteCampaniaATP(AST_ClienteEntidad cliente) {
            int IdInsertado = 0;

            string query = @"
            INSERT INTO AST_Cliente (
                NroDoc,
                NombreCompleto,
                Nombre,
                ApelPat,
                ApelMat,
                Genero,
                Celular1,
                Mail,
                FechaNacimiento,
                TipoDocumentoId,
                Estado,
                FechaRegistro,
                TipoRegistro,
                SalaId,
                usuario_reg
            )

            OUTPUT Inserted.Id

            VALUES (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6,
                @p7,
                @p8,
                @p9,
                @p10,
                @p11,
                @p12,
                @p13,
                @p14,
                @p15
            )
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.Nombre));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Genero));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.Celular1));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(cliente.Mail));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullDate(cliente.FechaNacimiento));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(cliente.Estado));
                    command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullDate(cliente.FechaRegistro));
                    command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(cliente.usuario_reg));

                    IdInsertado = Convert.ToInt32(command.ExecuteScalar());
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);

                IdInsertado = 0;
            }

            return IdInsertado;
        }

        public AST_ClienteEntidad ObtenerClienteById(int clienteId) {
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();

            string query = @"
            SELECT
                Id,
                NroDoc,
                Nombre,
                ApelPat,
                ApelMat,
                Genero,
                Celular1,
                Celular2,
                Mail,
                FechaNacimiento,
                FechaRegistro,
                AsistioDespuesCuarentena,
                TipoDocumentoId,
                UbigeoProcedenciaId,
                Estado,
                SalaID,
                NombreCompleto
            FROM dbo.AST_Cliente
            WHERE Id = @w1
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", clienteId);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.Read()) {
                            cliente.Id = ManejoNulos.ManageNullInteger(data["Id"]);
                            cliente.NroDoc = ManejoNulos.ManageNullStr(data["NroDoc"]);
                            cliente.Nombre = ManejoNulos.ManageNullStr(data["Nombre"]);
                            cliente.ApelPat = ManejoNulos.ManageNullStr(data["ApelPat"]);
                            cliente.ApelMat = ManejoNulos.ManageNullStr(data["ApelMat"]);
                            cliente.Genero = ManejoNulos.ManageNullStr(data["Genero"]);
                            cliente.Celular1 = ManejoNulos.ManageNullStr(data["Celular1"]);
                            cliente.Celular2 = ManejoNulos.ManageNullStr(data["Celular2"]);
                            cliente.Mail = ManejoNulos.ManageNullStr(data["Mail"]);
                            cliente.FechaNacimiento = ManejoNulos.ManageNullDate(data["FechaNacimiento"]);
                            cliente.AsistioDespuesCuarentena = ManejoNulos.ManageNullInteger(data["AsistioDespuesCuarentena"]);
                            cliente.TipoDocumentoId = ManejoNulos.ManageNullInteger(data["TipoDocumentoId"]);
                            cliente.UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(data["UbigeoProcedenciaId"]);
                            cliente.Estado = ManejoNulos.ManageNullStr(data["Estado"]);
                            cliente.FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]);
                            cliente.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                            cliente.NombreCompleto = ManejoNulos.ManageNullStr(data["NombreCompleto"]);
                        }
                    };
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);

                cliente.Id = 0;
            }

            return cliente;
        }

        public int GuardarClienteEmpadronamientoV2(AST_ClienteEntidad cliente) {
            int IdInsertado = 0;

            string query = @"
            INSERT INTO dbo.AST_Cliente (
                TipoDocumentoId,
                NroDoc,
                NombreCompleto,
                Nombre,
                ApelPat,
                ApelMat,
                Estado,
                FechaRegistro,
                TipoRegistro,
                SalaId,
                usuario_reg
            )

            OUTPUT Inserted.Id

            VALUES (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6,
                @p7,
                @p8,
                @p9,
                @p10,
                @p11
            )
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId) == 0 ? SqlInt32.Null : cliente.TipoDocumentoId);
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.Nombre));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.Estado));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(DateTime.Now));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.usuario_reg));

                    IdInsertado = Convert.ToInt32(command.ExecuteScalar());
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
                IdInsertado = 0;
            }

            return IdInsertado;
        }

        public List<AST_ClienteMigracion> ListarClienteMigracion(int Id) {
            List<AST_ClienteMigracion> lista = new List<AST_ClienteMigracion>();
            string consulta = @"
SELECT 
          cliente.[NroDoc]
          ,cliente.[NombreCompleto]
          ,cliente.[ApelPat]
          ,cliente.[ApelMat]
          ,Genero= 
            case
                when trim(cliente.[Genero])='M' then 'MASCULINO'
                when trim(cliente.[Genero])='F' then 'FEMENINO'
                else ''
            end
          ,cliente.[Celular1]
          ,cliente.[Celular2]
          ,cliente.[Mail]
          ,cliente.[FechaNacimiento]
          ,cliente.[FechaRegistro]
          ,cliente.[Nombre]
          ,isnull(sala.Nombre,'') as Sala
          ,isnull(tipodoc.Nombre,'') as TipoDocumento
          ,cliente.[Id],
          (
          select concat(
            (distrito.nombre)
            ,' - ',(select provincia.nombre from [BD_SEGURIDAD_PJ].[dbo].Ubigeo as provincia where provincia.PaisId=distrito.PaisId and provincia.DepartamentoId=distrito.DepartamentoId and provincia.ProvinciaId=distrito.ProvinciaId and provincia.DistritoId=0)
            ,' - ',(select REPLACE(departamento.nombre,'DEPARTAMENTO ','')from [BD_SEGURIDAD_PJ].[dbo].Ubigeo as departamento where departamento.PaisId=distrito.PaisId and departamento.DepartamentoId=distrito.DepartamentoId and departamento.ProvinciaId=0 and departamento.DistritoId=0)
            ,' - ',(select pais.nombre from [BD_SEGURIDAD_PJ].[dbo].Ubigeo as pais where pais.PaisId=distrito.PaisId and pais.DepartamentoId=0 and pais.ProvinciaId=0 and pais.DistritoId=0)
            )
            from [BD_SEGURIDAD_PJ].[dbo].Ubigeo as distrito where distrito.CodUbigeo=cliente.UbigeoProcedenciaId
          ) as ubigeo
      FROM [BD_SEGURIDAD_PJ].[dbo].[AST_Cliente] as cliente
      left join [BD_SEGURIDAD_PJ].dbo.AST_TipoDocumento as tipodoc
      on cliente.TipoDocumentoId=tipodoc.Id
      left join BD_SEGURIDAD_PJ.dbo.Sala as sala
      on cliente.SalaId=sala.CodSala
      where cliente.Id>=@Id
      order by id desc
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Id", Id);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var cliente = new AST_ClienteMigracion {
                                IdIas = ManejoNulos.ManageNullInteger(dr["Id"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                NombreTipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NombreUbigeo = ManejoNulos.ManageNullStr(dr["ubigeo"]),
                                NombreGenero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["Sala"]),

                            };

                            lista.Add(cliente);
                        }
                    }
                }
            } catch(Exception ex) {
                lista = new List<AST_ClienteMigracion>();
            }
            return lista;
        }
        public int GuardarClienteSorteoExterno(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
           ,[Nombre]
           ,[ApelPat]
           ,[ApelMat]
           ,[Celular1]
           ,[Estado],[FechaRegistro],SalaID,[NombreCompleto],Mail,usuario_reg,TipoRegistro,[TipoDocumentoId])
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
           ,@p6
           ,@p7
,@p8,@p9,@p10,@p11,@p12,@p13
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.Celular1));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(DateTime.Now));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(cliente.Mail));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(cliente.usuario_reg));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public int GuardarClienteCampaniaWhatsApp(AST_ClienteEntidad cliente) {
            int IdInsertado = 0;

            string query = @"
                INSERT INTO dbo.AST_Cliente (
                    Nombre,
                    ApelPat,
                    ApelMat,
                    NombreCompleto,
                    TipoDocumentoId,
                    NroDoc,
                    FechaNacimiento,
                    Genero,
                    UbigeoProcedenciaId,
                    PaisId,
                    Ciudadano,
                    Celular1,
                    codigoPais,
                    Estado,
                    FechaRegistro,
                    TipoRegistro,
                    SalaId
                )

                OUTPUT Inserted.Id

                VALUES (
                    @p1,
                    @p2,
                    @p3,
                    @p4,
                    @p5,
                    @p6,
                    @p7,
                    @p8,
                    @p9,
                    @p10,
                    @p11,
                    @p12,
                    @p13,
                    @p14,
                    @p15,
                    @p16,
                    @p17
                )
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Nombre));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId) == 0 ? SqlInt32.Null : cliente.TipoDocumentoId);
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(cliente.FechaNacimiento));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(cliente.Genero));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cliente.UbigeoProcedenciaId));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(cliente.PaisId));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManegeNullBool(cliente.Ciudadano));
                    command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(cliente.Celular1));
                    command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(cliente.CodigoPais));
                    command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(cliente.Estado));
                    command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDate(cliente.FechaRegistro));
                    command.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    command.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullInteger(cliente.SalaId) == 0 ? SqlInt32.Null : cliente.SalaId);

                    IdInsertado = Convert.ToInt32(command.ExecuteScalar());
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
                IdInsertado = 0;
            }

            return IdInsertado;
        }

        public List<AST_ClienteEntidad> GetExistenciaDeClienteParaCampaniaWhatsApp(string documentNumber, int idDocumentType, string phoneNumber, int codSala) {
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
            string consulta = @"
                SELECT 
                    astc.Id,
                    TRIM(astc.NroDoc) as NroDoc,
                    astc.Nombre,
                    astc.ApelPat,
                    astc.ApelMat,
                    astc.Genero,
                    astc.Celular1,
                    cmpc.NumeroCelular AS Celular2,
                    astc.Mail,
                    astc.FechaNacimiento,
                    astc.TipoDocumentoId,
                    astc.UbigeoProcedenciaId,
                    astc.Estado,
                    astc.NombreCompleto,
                    astc.TipoRegistro,
                    astc.FechaRegistro,
                    astc.SalaId as SalaAST_Cliente,
                    cmpca.sala_id AS SalaCMP_Campania,
                    astcs.SalaId AS AST_ClienteSala
                FROM 
                    AST_Cliente AS astc
                LEFT JOIN CMP_Cliente AS cmpc ON cmpc.cliente_id = astc.Id
                LEFT JOIN CMP_Campaña AS cmpca ON cmpca.id = cmpc.campania_id
                LEFT JOIN AST_ClienteSala AS astcs ON astcs.ClienteId = astc.Id
                WHERE 
                    (astc.SalaId = @codSala OR cmpca.sala_id = @codSala OR astcs.SalaId = @codSala) AND
                    ((TipoDocumentoId=@idDocumentNumber AND TRIM(NroDoc)=TRIM(@documentNumber)) OR
                    (Celular1 = @phoneNumber OR Celular2 = @phoneNumber) OR cmpc.NumeroCelular = @phoneNumber)
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idDocumentNumber", idDocumentType);
                    query.Parameters.AddWithValue("@documentNumber", documentNumber);
                    query.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    query.Parameters.AddWithValue("@codSala", codSala);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var cliente = new AST_ClienteEntidad {
                                    Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                    NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                    ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                    Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                                    Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                    Celular2 = ManejoNulos.ManageNullStr(dr["Celular2"]),
                                    Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                    FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                    TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    UbigeoProcedenciaId = ManejoNulos.ManageNullInteger(dr["UbigeoProcedenciaId"]),
                                    Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                    TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                                };

                                lista.Add(cliente);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public bool VerificarSiExisteCampania(int codSala) {
            string consulta = @"
            SELECT COUNT(*) FROM CMP_Campaña WHERE sala_id = @codSala AND estado = 1 and tipo = 2
        ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codSala", codSala);

                    int count = (int)query.ExecuteScalar();

                    return count > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public int GuardarClienteSorteosSala(AST_ClienteEntidad cliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[AST_Cliente]
           ([NroDoc]
           ,[Estado],[FechaRegistro],SalaID,[NombreCompleto],TipoRegistro,Nombre,ApelPat,ApelMat,TipoDocumentoId,Mail,Celular1)
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
           ,@p5
           ,@p6,@p7,@p8,@p9,@p10,@p11
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(cliente.NroDoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(cliente.Estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(cliente.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(cliente.SalaId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(cliente.NombreCompleto));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.TipoRegistro));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(cliente.Nombre));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(cliente.ApelPat));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(cliente.ApelMat));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(cliente.TipoDocumentoId));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(cliente.Mail));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(cliente.Celular1));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public List<AST_ClienteCortesia> GetClientesCortesia() {
            List<AST_ClienteCortesia> lista = new List<AST_ClienteCortesia>();
            string consulta = @"SELECT  [NroDoc],
                                        [NombreCompleto]
                                    FROM [dbo].[AST_Cliente]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var cliente = new AST_ClienteCortesia {
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                            };

                            lista.Add(cliente);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        #region Para PTK
        public AST_ClienteEntidad ObtenerPermisosDeContactoDeCliente(string numeroDocumento, int idTipoDocumento, int codSala) {
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            string consulta = @"
                DECLARE @CodSalaMaestra INT = 0;
                SELECT @CodSalaMaestra = ISNULL(CodSalaMaestra, 0) FROM Sala WHERE CodSala = @CodSala;

                SELECT
                    TRIM(astc.NroDoc) AS NroDoc,
                    TRIM(astc.NombreCompleto) AS NombreCompleto,
                    TRIM(astc.Nombre) AS Nombre,
                    TRIM(astc.ApelPat) AS ApelPat,
                    TRIM(astc.ApelMat) AS ApelMat,
                    ISNULL(astcs.EnviaNotificacionWhatsapp, 0) AS EnviaNotificacionWhatsapp,
                    ISNULL(astcs.EnviaNotificacionEmail, 0) AS EnviaNotificacionEmail,
                    ISNULL(astcs.EnviaNotificacionSms, 0) AS EnviaNotificacionSms,
                    ISNULL(astcs.LlamadaCelular, 0) AS LlamadaCelular
                FROM AST_ClienteSala AS astcs
                RIGHT JOIN AST_Cliente AS astc ON astc.Id = astcs.ClienteId
                LEFT JOIN Sala AS sala ON sala.CodSala = astcs.SalaId OR sala.CodSala = astc.SalaId
                WHERE
                    astc.TipoDocumentoId = @idTipoDocumento
                    AND astc.NroDoc = @numeroDocumento
                    AND sala.CodSalaMaestra = @CodSalaMaestra
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idTipoDocumento", idTipoDocumento);
                    query.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
                    query.Parameters.AddWithValue("@codSala", codSala);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                cliente = new AST_ClienteEntidad {
                                    NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                    ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                    EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionWhatsapp"]),
                                    EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionEmail"]),
                                    EnviaNotificacionSms = ManejoNulos.ManegeNullBool(dr["EnviaNotificacionSms"]),
                                    LlamadaCelular = ManejoNulos.ManegeNullBool(dr["LlamadaCelular"]),
                                };
                            }
                        }
                    };
                }
            } catch { }
            return cliente;
        }
        #endregion

        #region Migracion DWH
        public List<AST_ClienteMigracionDwhEntidad> ObtenerClientesControlAccesoParaDwh(AST_ClienteMigracionDwhFiltro filtro) {
            List<AST_ClienteMigracionDwhEntidad> items = new List<AST_ClienteMigracionDwhEntidad>();
            string consulta = $@"
                SELECT TOP (@CantidadRegistros) * FROM (
                    SELECT
                        cliente.Id AS IdCliente,
                        sala.CodSala AS CodSala,
                        sala.Nombre AS NombreSala,
                        ISNULL(td.Id, '') AS IdTipoDocumento,
                        ISNULL(td.Nombre, '') AS TipoDocumento,
                        cliente.NroDoc AS NumeroDocumento,
                        cliente.NombreCompleto AS NombreCliente,
                        FORMAT(ISNULL(clienteSala.FechaRegistro, cliente.FechaRegistro),'dd-MM-yyyy HH:mm','en-eu')  AS FechaRegistro,
                        clienteSala.TipoRegistro AS TipoRegistro,
                        cliente.FechaNacimiento AS FechaNacimiento
                    FROM AST_ClienteSala AS clienteSala
                    INNER JOIN AST_Cliente AS cliente on cliente.Id = clienteSala.ClienteId
                    LEFT JOIN AST_TipoDocumento AS td ON td.Id = cliente.TipoDocumentoId
                    INNER JOIN Sala AS sala ON sala.CodSala = clienteSala.SalaId
                    WHERE clienteSala.TipoRegistro = 'SYSLUDOPATAS' AND clienteSala.FechaMigracionDwh IS NULL

                    UNION

                    SELECT
                        cliente.Id AS IdCliente,
                        sala.CodSala AS CodSala,
                        sala.Nombre AS NombreSala,
                        ISNULL(td.Id, '') AS IdTipoDocumento,
                        ISNULL(td.Nombre, '') AS TipoDocumento,
                        cliente.NroDoc AS NumeroDocumento,
                        cliente.NombreCompleto AS NombreCliente,
                        FORMAT(cliente.FechaRegistro,'dd-MM-yyyy HH:mm','en-eu') AS FechaRegistro,
                        cliente.TipoRegistro AS TipoRegistro,
                        cliente.FechaNacimiento AS FechaNacimiento
                    FROM AST_Cliente AS cliente
                    INNER JOIN Sala AS sala ON sala.CodSala = cliente.SalaId
                    LEFT JOIN AST_ClienteSala AS clienteSala ON clienteSala.ClienteId = cliente.Id AND clienteSala.SalaId = cliente.SalaId
                    LEFT JOIN AST_TipoDocumento AS td ON td.Id = cliente.TipoDocumentoId
                    WHERE cliente.TipoRegistro = 'SYSLUDOPATAS' AND cliente.FechaMigracionDwh IS NULL
                ) t
                ORDER BY FechaRegistro ASC;
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CantidadRegistros", filtro.CantidadRegistros);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public int ActualizarEstadoMigracionDwh(AST_ClienteEstadoMigracion ids, DateTime? fechaMigracionDwh) {
            int idActualizado = 0;
            string consulta = $@"
                UPDATE AST_Cliente
                SET FechaMigracionDwh = @FechaMigracionDwh
                OUTPUT inserted.Id
                WHERE Id = @IdCliente

                UPDATE AST_ClienteSala
                SET FechaMigracionDwh = @FechaMigracionDwh
                OUTPUT inserted.ClienteId
                WHERE ClienteId = @IdCliente AND SalaId = @CodSala
            ";

            using(SqlConnection con = new SqlConnection(_conexion)) {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try {
                    using(SqlCommand query = new SqlCommand(consulta, con, tran)) {
                        query.Parameters.AddWithValue("@IdCliente", ids.IdCliente);
                        query.Parameters.AddWithValue("@CodSala", ids.CodSala);
                        query.Parameters.AddWithValue("@FechaMigracionDwh", (object)fechaMigracionDwh ?? DBNull.Value);

                        using(SqlDataReader reader = query.ExecuteReader()) {
                            if(reader.Read()) {
                                idActualizado = reader.GetInt32(0);
                            }

                            if(reader.NextResult() && reader.Read()) {
                                int clienteSalaId = reader.GetInt32(0);
                            }
                        }
                    }
                    tran.Commit();
                } catch {
                    tran.Rollback();
                }
            }
            return idActualizado;
        }

        private AST_ClienteMigracionDwhEntidad ConstruirObjeto(SqlDataReader dr) {
            return new AST_ClienteMigracionDwhEntidad {
                IdCliente = ManejoNulos.ManageNullInteger(dr["IdCliente"]),
                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                IdTipoDocumento = ManejoNulos.ManageNullInteger(dr["IdTipoDocumento"]),
                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                TipoRegistro = ManejoNulos.ManageNullStr(dr["TipoRegistro"]),
                FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"])
            };
        }
        #endregion
    }
}
