using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Campaña {
    public class CMP_ClienteDAL {
        string _conexion = string.Empty;

        public CMP_ClienteDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CMP_ClienteEntidad> GetClientesCampania(Int64 id) {
            List<CMP_ClienteEntidad> lista = new List<CMP_ClienteEntidad>();
            string consulta = @"SELECT cli.[id]
                      ,cli.[cliente_id]
                      ,cli.[campania_id]
                      ,ascli.ApelPat
                      ,ascli.ApelMat
                      ,ascli.Nombre
                      ,ascli.NombreCompleto
                      ,ascli.NroDoc
                      ,ascli.FechaNacimiento
                      ,ascli.SalaId
                      ,s.Nombre NombreSala
                        ,ascli.Mail
                  FROM [dbo].[CMP_Cliente] cli
                  right join AST_Cliente ascli on ascli.Id=cli.cliente_id
                  right join Sala s on s.CodSala= ascli.SalaId where cli.campania_id=@p0 order by ascli.NombreCompleto desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(id));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["campania_id"]),

                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                            };

                            lista.Add(campaña);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<CMP_ClienteEntidad> GetClientesCampaniaBuscar(string valor) {
            List<CMP_ClienteEntidad> lista = new List<CMP_ClienteEntidad>();
            string where = string.Empty;
            if(valor == "") {
                where = "";
            } else {
                where = "where (ascli.NombreCompleto like '%" + valor + "%' or ascli.NroDoc='" + valor + "')";
            }
            string consulta = @"Select ascli.id
                      ,ascli.ApelPat
                      ,ascli.ApelMat
                      ,ascli.Nombre
                      ,ascli.NombreCompleto
                      ,ascli.NroDoc
                      ,ascli.FechaNacimiento
                      ,ascli.SalaId
                      ,s.Nombre NombreSala
                        ,cli.id CMPcliente_id
                        ,cli.campania_id
 ,ascli.Mail
                  FROM 
                   AST_Cliente ascli 
                  left join Sala s on s.CodSala= ascli.SalaId 
                  left join CMP_Cliente cli on cli.cliente_id=ascli.id  " + where + " order by ascli.NombreCompleto asc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(valor));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                CMPcliente_id = ManejoNulos.ManageNullInteger64(dr["CMPcliente_id"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["campania_id"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                            };

                            lista.Add(campaña);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public int GuardarClienteCampania(CMP_ClienteEntidad campaniaCliente) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
                INSERT INTO [dbo].[CMP_Cliente] (
                    [cliente_id],
                    [campania_id],
                    [fecha_reg],
                    [codigo],
                    [codigoCanjeado],
                    [fechaGeneracionCodigo],
                    [fechaExpiracionCodigo],
                    [codigoExpirado],
                    [codigoEnviado],
                    [codigoPais],
                    [NumeroCelular],
                    [procedenciaRegistro],
                    [CodigoCanjeableEn],
                    [CodigoCanjeableMultiplesSalas],
                    [MontoRecargado]
                )
                Output Inserted.id
                VALUES(
                    @p0,
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
                    @p14
                );
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(campaniaCliente.cliente_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(campaniaCliente.campania_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(campaniaCliente.fecha_reg));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(campaniaCliente.Codigo));
                    query.Parameters.AddWithValue("@p4", false);
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(campaniaCliente.FechaGeneracionCodigo));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(campaniaCliente.FechaExpiracionCodigo));
                    query.Parameters.AddWithValue("@p7", false);
                    query.Parameters.AddWithValue("@p8", false);
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(campaniaCliente.CodigoPais));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(campaniaCliente.NumeroCelular));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(campaniaCliente.ProcedenciaRegistro));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(campaniaCliente.CodigoCanjeableEn));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManegeNullBool(campaniaCliente.CodigoCanjeableMultiplesSalas));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullDecimal(campaniaCliente.MontoRecargado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool eliminarCampaniaCliente(Int64 id) {
            bool respuesta = false;
            string consulta = @"Delete from [dbo].[CMP_Cliente]
                 WHERE id=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public List<CMP_ClienteEntidad> GetClientesCampaniaxCliente(string whereQuery, DateTime fechaIni, DateTime fechaFin) {
            List<CMP_ClienteEntidad> lista = new List<CMP_ClienteEntidad>();
            string consulta = @"SELECT cli.[id]
            ,cli.[cliente_id]
            ,cli.[campania_id],camp.nombre,cl.NombreCompleto, cl.NroDoc
            ,s.Nombre as NombreSala
            FROM [dbo].[CMP_Cliente] as cli
            join dbo.CMP_Campaña as camp on camp.id=cli.campania_id  
            join dbo.AST_Cliente as cl on cl.Id=cli.cliente_id
            join Sala s on s.CodSala = camp.sala_id
                where " + whereQuery + " and CONVERT(date, camp.fechareg) between @p0 and @p1";

            //join CMP_Tickets as tick on tick.campaña_id=camp.id where " + whereQuery+ " and CONVERT(date, tick.fechareg) between @p0 and @p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaIni);
                    query.Parameters.AddWithValue("@p1", fechaFin);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campania = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["campania_id"]),
                                CampaniaNombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"])
                            };
                            lista.Add(campania);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }

        public List<CMP_ClienteEntidad> ObtenerClientesDeCampaniaWhatsAppPorIdCampania(long idCampania) {
            List<CMP_ClienteEntidad> lista = new List<CMP_ClienteEntidad>();
            string consulta = @"
                SELECT
                    cc.id AS CampaniaClienteId,
                    cc.campania_id AS CampaniaId,
                    cc.cliente_id AS ClienteId,
                    ac.NombreCompleto AS NombreCompletoCliente,
                    ac.Nombre AS NombreCliente,
                    ac.ApelPat AS ApellidoPaternoCliente,
                    ac.ApelMat AS ApellidoMaternoCliente,
                    cc.codigoPais AS CodigoPais,
                    cc.NumeroCelular AS NumeroCelular,
                    atd.Nombre AS TipoDocumento,
                    ac.NroDoc AS NumeroDocumento,
                    cc.codigo AS Codigo,
                    cc.codigoEnviado AS CodigoEnviado,
                    cc.codigoCanjeado AS CodigoCanjeado,
                    cc.fechaGeneracionCodigo AS FechaGeneracionCodigo,
                    cc.fechaCanjeoCodigo AS FechaCanjeoCodigo,
                    cc.fechaExpiracionCodigo AS FechaExpiracionCodigo,
                    cc.codigoExpirado as CodigoExpirado,
                    cc.procedenciaRegistro as ProcedenciaRegistro,
                    cc.MontoRecargado AS MontoRecargado,
                    u.Nombre as Nacionalidad,
                    ac.FechaNacimiento AS FechaNacimiento
                FROM CMP_Cliente AS cc
                INNER JOIN AST_Cliente AS ac ON cc.cliente_id = ac.id
                INNER JOIN CMP_Campaña AS cca ON cca.id = cc.campania_id
                INNER JOIN AST_TipoDocumento AS atd ON atd.Id = ac.TipoDocumentoId
                LEFT JOIN Ubigeo AS u ON u.PaisId = ac.PaisId AND u.DepartamentoId = 0 AND u.ProvinciaId = 0 AND u.DistritoId = 0
                WHERE
                    cc.campania_id = @idCampania AND
                    cca.tipo = 2
                ORDER BY cc.fechaGeneracionCodigo DESC
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idCampania", idCampania);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campania = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["CampaniaClienteId"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["ClienteId"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompletoCliente"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApellidoPaternoCliente"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApellidoMaternoCliente"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                NumeroCelular = ManejoNulos.ManageNullStr(dr["NumeroCelular"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Codigo = ManejoNulos.ManageNullStr(dr["Codigo"]),
                                CodigoCanjeado = ManejoNulos.ManegeNullBool(dr["CodigoCanjeado"]),
                                CodigoEnviado = ManejoNulos.ManegeNullBool(dr["CodigoEnviado"]),
                                FechaGeneracionCodigo = ManejoNulos.ManageNullDate(dr["FechaGeneracionCodigo"]),
                                FechaCanjeoCodigo = ManejoNulos.ManageNullDate(dr["FechaCanjeoCodigo"]),
                                FechaExpiracionCodigo = ManejoNulos.ManageNullDate(dr["FechaExpiracionCodigo"]),
                                CodigoExpirado = ManejoNulos.ManegeNullBool(dr["CodigoExpirado"]),
                                ProcedenciaRegistro = ManejoNulos.ManageNullStr(dr["ProcedenciaRegistro"]),
                                MontoRecargado = ManejoNulos.ManageNullDecimal(dr["MontoRecargado"]),
                                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"])
                            };
                            lista.Add(campania);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }

        public CMP_ClienteEntidad ObtenerClienteDeCampaniaWhatsAppPorIdCampaniaNumeroDocumento(long idCampania, string numeroDocumento) {
            CMP_ClienteEntidad item = new CMP_ClienteEntidad();
            string consulta = @"
                SELECT
                    cc.id AS CampaniaClienteId,
                    cc.campania_id AS CampaniaId,
                    cc.cliente_id AS ClienteId,
                    ac.NombreCompleto AS NombreCompletoCliente,
                    ac.Nombre AS NombreCliente,
                    ac.ApelPat AS ApellidoPaternoCliente,
                    ac.ApelMat AS ApellidoMaternoCliente,
                    cc.codigoPais AS CodigoPais,
                    cc.NumeroCelular AS NumeroCelular,
                    atd.Nombre AS TipoDocumento,
                    ac.NroDoc AS NumeroDocumento,
                    cc.codigo AS Codigo,
                    cc.codigoEnviado AS CodigoEnviado,
                    cc.codigoCanjeado AS CodigoCanjeado,
                    cc.fechaGeneracionCodigo AS FechaGeneracionCodigo,
                    cc.fechaCanjeoCodigo AS FechaCanjeoCodigo,
	                cc.CodigoCanjeadoEn AS CodigoCanjeadoEn,
                    cc.fechaExpiracionCodigo AS FechaExpiracionCodigo,
                    cc.codigoExpirado as CodigoExpirado,
                    cc.procedenciaRegistro as ProcedenciaRegistro,
                    cc.MontoRecargado AS MontoRecargado,
                    u.Nombre as Nacionalidad,
                    ac.FechaNacimiento AS FechaNacimiento
                FROM CMP_Cliente AS cc
                INNER JOIN AST_Cliente AS ac ON cc.cliente_id = ac.id
                INNER JOIN CMP_Campaña AS cca ON cca.id = cc.campania_id
                INNER JOIN AST_TipoDocumento AS atd ON atd.Id = ac.TipoDocumentoId
                LEFT JOIN Ubigeo AS u ON u.PaisId = ac.PaisId AND u.DepartamentoId = 0 AND u.ProvinciaId = 0 AND u.DistritoId = 0
                WHERE
                    cc.campania_id = @idCampania AND
	                ac.NroDoc = @numeroDocumento AND
                    cca.tipo = 2
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idCampania", idCampania);
                    query.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["CampaniaClienteId"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["ClienteId"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompletoCliente"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApellidoPaternoCliente"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApellidoMaternoCliente"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                NumeroCelular = ManejoNulos.ManageNullStr(dr["NumeroCelular"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Codigo = ManejoNulos.ManageNullStr(dr["Codigo"]),
                                CodigoCanjeado = ManejoNulos.ManegeNullBool(dr["CodigoCanjeado"]),
                                CodigoEnviado = ManejoNulos.ManegeNullBool(dr["CodigoEnviado"]),
                                FechaGeneracionCodigo = ManejoNulos.ManageNullDate(dr["FechaGeneracionCodigo"]),
                                FechaCanjeoCodigo = ManejoNulos.ManageNullDate(dr["FechaCanjeoCodigo"]),
                                CodigoCanjeadoEn = ManejoNulos.ManageNullInteger(dr["CodigoCanjeadoEn"]),
                                FechaExpiracionCodigo = ManejoNulos.ManageNullDate(dr["FechaExpiracionCodigo"]),
                                CodigoExpirado = ManejoNulos.ManegeNullBool(dr["CodigoExpirado"]),
                                ProcedenciaRegistro = ManejoNulos.ManageNullStr(dr["ProcedenciaRegistro"]),
                                MontoRecargado = ManejoNulos.ManageNullDecimal(dr["MontoRecargado"]),
                                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        public CMP_ClienteEntidad ObtenerClientePorCodigoPromocional(string promotionalCode) {
            CMP_ClienteEntidad item = new CMP_ClienteEntidad();

            string consulta = @"
                SELECT 
                    cmp.id,
                    cmp.cliente_id,
                    cmp.campania_id,
                    cmp.fecha_reg,
                    cmp.codigo,
                    cmp.codigoCanjeado,
                    cmp.fechaExpiracionCodigo,
                    cmp.fechaGeneracionCodigo,
                    cmp.fechaCanjeoCodigo,
                    cmp.codigoExpirado,
                    cmp.codigoEnviado,
                    cmp.procedenciaRegistro,
                    cmp.CodigoCanjeableMultiplesSalas,
                    cmp.CodigoCanjeableEn,
                    cmp.MontoRecargado,
                    u.Nombre as Nacionalidad,
                    cli.FechaNacimiento,
                    cli.NroDoc,
                    cli.TipoDocumentoId,
                    cli.NombreCompleto,
                    cli.Nombre,
                    cli.ApelPat,
                    cli.ApelMat,
                    COALESCE(NULLIF(cmp.codigoPais, ''), '51') AS codigoPais,
                    cmp.NumeroCelular,
                    sala.Nombre as Sala,
                    sala.CodSala,
                    td.Nombre as TipoDocumento
                FROM CMP_Cliente cmp
                INNER JOIN AST_Cliente cli ON cmp.cliente_id=cli.Id
                INNER JOIN CMP_Campaña cmpc ON cmpc.id = cmp.campania_id
                INNER JOIN AST_TipoDocumento td ON td.Id = cli.TipoDocumentoId
                INNER JOIN Sala sala ON cmpc.sala_id = sala.CodSala
                LEFT JOIN Ubigeo AS u ON u.PaisId = cli.PaisId AND u.DepartamentoId = 0 AND u.ProvinciaId = 0 AND u.DistritoId = 0
                WHERE cmp.codigo=@pcodigo";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pcodigo", ManejoNulos.ManageNullStr(promotionalCode));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["campania_id"]),
                                fecha_reg = ManejoNulos.ManageNullDate(dr["fecha_reg"]),
                                Codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
                                CodigoCanjeado = ManejoNulos.ManegeNullBool(dr["codigoCanjeado"]),
                                CodigoEnviado = ManejoNulos.ManegeNullBool(dr["codigoEnviado"]),
                                FechaExpiracionCodigo = ManejoNulos.ManageNullDate(dr["fechaExpiracionCodigo"]),
                                FechaGeneracionCodigo = ManejoNulos.ManageNullDate(dr["fechaGeneracionCodigo"]),
                                FechaCanjeoCodigo = ManejoNulos.ManageNullDate(dr["fechaCanjeoCodigo"]),
                                CodigoExpirado = ManejoNulos.ManegeNullBool(dr["codigoExpirado"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["codigoPais"]),
                                NumeroCelular = ManejoNulos.ManageNullStr(dr["NumeroCelular"]),
                                ProcedenciaRegistro = ManejoNulos.ManageNullStr(dr["procedenciaRegistro"]),
                                CodigoCanjeableMultiplesSalas = ManejoNulos.ManegeNullBool(dr["CodigoCanjeableMultiplesSalas"]),
                                CodigoCanjeableEn = ManejoNulos.ManageNullStr(dr["CodigoCanjeableEn"]),
                                MontoRecargado = ManejoNulos.ManageNullDecimal(dr["MontoRecargado"]),
                                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["Sala"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["CodSala"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        public CMP_ClienteEntidad BuscarClienteExistenteCMPCliente(int idCliente, long idCamapania) {
            CMP_ClienteEntidad item = new CMP_ClienteEntidad();

            string consulta = @"
                SELECT 
                    cmp.id,
                    cmp.cliente_id,
                    cmp.campania_id,
                    cmp.fecha_reg,
                    cmp.codigo,
                    cmp.codigoCanjeado,
                    cmp.codigoEnviado,
                    cmp.fechaExpiracionCodigo,
                    cmp.fechaGeneracionCodigo,
                    cmp.fechaCanjeoCodigo,
                    cmp.codigoExpirado,
                    cmp.procedenciaRegistro,
                    cmp.MontoRecargado,
                    cli.NroDoc,
                    cli.TipoDocumentoId,
                    cli.NombreCompleto,
                    cli.Nombre,
                    cli.ApelPat,
                    cli.ApelMat,
                    u.Nombre as Nacionalidad,
                    cli.FechaNacimiento,
                    COALESCE(NULLIF(cmp.codigoPais, ''), '51') AS codigoPais,
                    cmp.NumeroCelular,
                    sala.Nombre as Sala,
                    sala.CodSala,
                    td.Nombre as TipoDocumento
                FROM CMP_Cliente cmp
                INNER JOIN AST_Cliente cli ON cmp.cliente_id=cli.Id
                INNER JOIN CMP_Campaña cmpc ON cmpc.id = cmp.campania_id
                INNER JOIN AST_TipoDocumento td ON td.Id = cli.TipoDocumentoId
                INNER JOIN Sala sala ON cmpc.sala_id = sala.CodSala
                LEFT JOIN Ubigeo AS u ON u.PaisId = cli.PaisId AND u.DepartamentoId = 0 AND u.ProvinciaId = 0 AND u.DistritoId = 0
                WHERE
                    cmp.cliente_id=@pcliente_id AND 
                    cmp.campania_id = @pcampania_id
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pcliente_id", ManejoNulos.ManageNullInteger64(idCliente));
                    query.Parameters.AddWithValue("@pcampania_id", ManejoNulos.ManageNullInteger64(idCamapania));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["campania_id"]),
                                fecha_reg = ManejoNulos.ManageNullDate(dr["fecha_reg"]),
                                Codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
                                CodigoCanjeado = ManejoNulos.ManegeNullBool(dr["codigoCanjeado"]),
                                CodigoEnviado = ManejoNulos.ManegeNullBool(dr["codigoEnviado"]),
                                FechaExpiracionCodigo = ManejoNulos.ManageNullDate(dr["fechaExpiracionCodigo"]),
                                FechaGeneracionCodigo = ManejoNulos.ManageNullDate(dr["fechaGeneracionCodigo"]),
                                FechaCanjeoCodigo = ManejoNulos.ManageNullDate(dr["fechaCanjeoCodigo"]),
                                CodigoExpirado = ManejoNulos.ManegeNullBool(dr["codigoExpirado"]),
                                ProcedenciaRegistro = ManejoNulos.ManageNullStr(dr["procedenciaRegistro"]),
                                MontoRecargado = ManejoNulos.ManageNullDecimal(dr["MontoRecargado"]),
                                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["codigoPais"]),
                                NumeroCelular = ManejoNulos.ManageNullStr(dr["NumeroCelular"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["Sala"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["CodSala"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        public CMP_ClienteEntidad ObtenerClientePorIdCampaniaCliente(long idCampaniaCliente) {
            CMP_ClienteEntidad item = new CMP_ClienteEntidad();

            string consulta = @"
                SELECT 
                    cmp.id,
                    cmp.cliente_id,
                    cmp.campania_id,
                    cmp.fecha_reg,
                    cmp.codigo,
                    cmp.codigoCanjeado,
                    cmp.codigoEnviado,
                    cmp.fechaExpiracionCodigo,
                    cmp.fechaGeneracionCodigo,
                    cmp.fechaCanjeoCodigo,
                    cmp.codigoExpirado,
                    cmp.procedenciaRegistro,
                    cmp.MontoRecargado,
                    u.Nombre as Nacionalidad,
                    cli.FechaNacimiento,
                    cli.NroDoc,
                    cli.TipoDocumentoId,
                    cli.NombreCompleto,
                    cli.Nombre,
                    cli.ApelPat,
                    cli.ApelMat,
                    COALESCE(NULLIF(cmp.codigoPais, ''), '51') AS codigoPais,
                    cmp.NumeroCelular,
                    sala.Nombre as Sala,
                    sala.CodSala,
                    td.Nombre as TipoDocumento
                FROM CMP_Cliente cmp
                INNER JOIN AST_Cliente cli ON cmp.cliente_id=cli.Id
                INNER JOIN CMP_Campaña cmpc ON cmpc.id = cmp.campania_id
                INNER JOIN AST_TipoDocumento td ON td.Id = cli.TipoDocumentoId
                INNER JOIN Sala sala ON cmpc.sala_id = sala.CodSala
                LEFT JOIN Ubigeo AS u ON u.PaisId = cli.PaisId AND u.DepartamentoId = 0 AND u.ProvinciaId = 0 AND u.DistritoId = 0
                WHERE cmp.id=@idCampaniaCliente
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idCampaniaCliente", ManejoNulos.ManageNullInteger64(idCampaniaCliente));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CMP_ClienteEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]),
                                campania_id = ManejoNulos.ManageNullInteger64(dr["campania_id"]),
                                fecha_reg = ManejoNulos.ManageNullDate(dr["fecha_reg"]),
                                Codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
                                CodigoCanjeado = ManejoNulos.ManegeNullBool(dr["codigoCanjeado"]),
                                CodigoEnviado = ManejoNulos.ManegeNullBool(dr["codigoEnviado"]),
                                FechaExpiracionCodigo = ManejoNulos.ManageNullDate(dr["fechaExpiracionCodigo"]),
                                FechaGeneracionCodigo = ManejoNulos.ManageNullDate(dr["fechaGeneracionCodigo"]),
                                FechaCanjeoCodigo = ManejoNulos.ManageNullDate(dr["fechaCanjeoCodigo"]),
                                CodigoExpirado = ManejoNulos.ManegeNullBool(dr["codigoExpirado"]),
                                ProcedenciaRegistro = ManejoNulos.ManageNullStr(dr["procedenciaRegistro"]),
                                MontoRecargado = ManejoNulos.ManageNullDecimal(dr["MontoRecargado"]),
                                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["codigoPais"]),
                                NumeroCelular = ManejoNulos.ManageNullStr(dr["NumeroCelular"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["Sala"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["CodSala"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        public int ActualizarCelularCliente(CMP_ClienteEntidad cliente) {
            int idActualizado = 0;
            string consulta = @"
                UPDATE CMP_Cliente
                SET codigoPais = @codigoPais,
                    NumeroCelular = @numeroCelular
                OUTPUT INSERTED.Id
                WHERE Id = @idClienteEntidad
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codigoPais", cliente.CodigoPais);
                    query.Parameters.AddWithValue("@numeroCelular", cliente.NumeroCelular);
                    query.Parameters.AddWithValue("@idClienteEntidad", cliente.id);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idActualizado = 0;
            }
            return idActualizado;
        }

        public bool MarcarCodigoEnviado(Int64 id) {
            bool success = false;
            string consulta = @"
                UPDATE
                    CMP_Cliente 
                SET
                    codigoEnviado = 1
                WHERE
                    id = @id";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", id);
                    query.ExecuteNonQuery();
                    success = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                success = false;
            }
            return success;
        }

        public bool CanjearCodigoPromocional(Int64 id, int codSala) {
            bool success = false;
            string consulta = @"
                UPDATE
                    CMP_Cliente 
                SET
                    codigoCanjeado = 1,
                    CodigoCanjeadoEn = @codSala,
                    fechaCanjeoCodigo = GETDATE()
                WHERE
                    id = @id";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", id);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    query.ExecuteNonQuery();
                    success = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                success = false;
            }
            return success;
        }

        public bool ExisteCodigoPromocional(string promotionalCode) {
            bool exists = false;
            string consulta = @"
                SELECT
                    COUNT(codigo)
                FROM
                    CMP_Cliente
                WHERE
                    codigo = @codigo";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codigo", promotionalCode);
                    int numeroDeResultados = (int)query.ExecuteScalar();
                    exists = numeroDeResultados >= 1;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                exists = false;
            }
            return exists;
        }
    }
}
