using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CapaDatos.Campaña {
    public class CMP_CampañaDAL {
        string _conexion = string.Empty;

        public CMP_CampañaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_CampañaEntidad> GetListadoCampañacompleto() {
            List<CMP_CampañaEntidad> lista = new List<CMP_CampañaEntidad>();
            string consulta = @"
                SELECT
                    [id],
                    [sala_id],
                    [nombre],
                    [descripcion],
                    [fechareg],
                    [fechaini],
                    [fechafin],
                    [usuario_id],
                    [estado],
                    [mensajeWhatsApp],
                    [duracionCodigoDias],
                    [duracionCodigoHoras],
                    [codigoSeReactiva],
                    [duracionReactivacionCodigoDias],
                    [duracionReactivacionCodigoHoras],
                    [mensajeWhatsAppReactivacion],
                    tipo
                FROM 
                    [dbo].[CMP_Campaña]
                ORDER BY 
                    id DESC
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_CampañaEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                fechaini = ManejoNulos.ManageNullDate(dr["fechaini"]),
                                fechafin = ManejoNulos.ManageNullDate(dr["fechafin"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                mensajeWhatsApp = ManejoNulos.ManageNullStr(dr["mensajeWhatsApp"]),
                                duracionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionCodigoDias"]),
                                duracionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionCodigoHoras"]),
                                codigoSeReactiva = ManejoNulos.ManegeNullBool(dr["codigoSeReactiva"]),
                                duracionReactivacionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoDias"]),
                                duracionReactivacionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoHoras"]),
                                mensajeWhatsAppReactivacion = ManejoNulos.ManageNullStr(dr["mensajeWhatsAppReactivacion"]),
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
        public List<CMP_CampañaEntidad> GetListadoCampaña(string salas, DateTime fechaini, DateTime fechafin) {
            List<CMP_CampañaEntidad> lista = new List<CMP_CampañaEntidad>();

            string consulta = @" select c.[id]
                                ,c.[sala_id]
                                ,s.Nombre nombresala
                                ,c.[nombre]
                                ,c.[descripcion]
                                ,c.[fechareg]
                                ,c.[fechaini]
                                ,c.[fechafin]
                                ,c.[estado]
                                ,c.[usuario_id]
                                ,u.UsuarioNombre 
                                ,c.[estado]
                                ,c.[mensajeWhatsApp]
                                ,c.[duracionCodigoDias]
                                ,c.[duracionCodigoHoras]
                                ,c.[codigoSeReactiva]
                                ,c.[duracionReactivacionCodigoDias]
                                ,c.[duracionReactivacionCodigoHoras]
                                ,c.[mensajeWhatsAppReactivacion]
                                ,c.tipo
                                ,s.UrlProgresivo
                            FROM 
                    [dbo].[CMP_Campaña] c
                    join Sala s on s.CodSala= c.sala_id
                    join SEG_Usuario u on u.UsuarioID=c.usuario_id 
                     where " + salas + " CONVERT(date, c.fechaini) between @p0 and @p1 order by c.id desc;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_CampañaEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                fechaini = ManejoNulos.ManageNullDate(dr["fechaini"]),
                                fechafin = ManejoNulos.ManageNullDate(dr["fechafin"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                usuarionombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                mensajeWhatsApp = ManejoNulos.ManageNullStr(dr["mensajeWhatsApp"]),
                                duracionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionCodigoDias"]),
                                duracionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionCodigoHoras"]),
                                codigoSeReactiva = ManejoNulos.ManegeNullBool(dr["codigoSeReactiva"]),
                                duracionReactivacionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoDias"]),
                                duracionReactivacionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoHoras"]),
                                mensajeWhatsAppReactivacion = ManejoNulos.ManageNullStr(dr["mensajeWhatsAppReactivacion"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
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
        public CMP_CampañaEntidad GetCampañaID(Int64 id) {
            CMP_CampañaEntidad campaña = new CMP_CampañaEntidad();
            string consulta = @"
                SELECT 
                    id,
                    sala_id,
                    cc.nombre,
                    descripcion,
                    fechareg,
                    fechaini,
                    fechafin,
                    usuario_id,
                    cc.estado,
                    mensajeWhatsApp,
                    duracionCodigoDias,
                    duracionCodigoHoras,
                    codigoSeReactiva,
                    duracionReactivacionCodigoDias,
                    duracionReactivacionCodigoHoras,
                    mensajeWhatsAppReactivacion,
                    cc.tipo,
                    s.Nombre AS nombreSala
                FROM 
                    dbo.CMP_Campaña cc
                INNER JOIN 
                    dbo.Sala s ON cc.sala_id = s.CodSala
                WHERE 
                    id = @p0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                campaña.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                campaña.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]);
                                campaña.nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
                                campaña.descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]);
                                campaña.fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]);
                                campaña.fechaini = ManejoNulos.ManageNullDate(dr["fechaini"]);
                                campaña.fechafin = ManejoNulos.ManageNullDate(dr["fechafin"]);
                                campaña.usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]);
                                campaña.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                                campaña.tipo = ManejoNulos.ManageNullInteger(dr["tipo"]);
                                campaña.mensajeWhatsApp = ManejoNulos.ManageNullStr(dr["mensajeWhatsApp"]);
                                campaña.duracionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionCodigoDias"]);
                                campaña.duracionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionCodigoHoras"]);
                                campaña.codigoSeReactiva = ManejoNulos.ManegeNullBool(dr["codigoSeReactiva"]);
                                campaña.duracionReactivacionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoDias"]);
                                campaña.duracionReactivacionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoHoras"]);
                                campaña.mensajeWhatsAppReactivacion = ManejoNulos.ManageNullStr(dr["mensajeWhatsAppReactivacion"]);
                                campaña.nombresala = ManejoNulos.ManageNullStr(dr["nombreSala"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return campaña;
        }

        public int GuardarCampaña(CMP_CampañaEntidad campaña) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
                INSERT INTO [dbo].[CMP_Campaña] (
                    [sala_id]
                    ,[nombre]
                    ,[descripcion]
                    ,[fechareg]
                    ,[fechaini]
                    ,[fechafin]
                    ,[usuario_id]
                    ,[estado]
                    ,[tipo]
                    ,[mensajeWhatsApp]
                    ,[duracionCodigoDias]
                    ,[duracionCodigoHoras]
                    ,[codigoSeReactiva]
                    ,[duracionReactivacionCodigoDias]
                    ,[duracionReactivacionCodigoHoras]
                    ,[mensajeWhatsAppReactivacion]
                )
                Output Inserted.id
                VALUES (
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
                    ,@p11
                    ,@p12
                    ,@p13
                    ,@p14
                    ,@p15
                );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(campaña.sala_id) == 0 ? SqlInt32.Null : campaña.sala_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(campaña.nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(campaña.descripcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(campaña.fechareg));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(campaña.fechaini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(campaña.fechafin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(campaña.usuario_id));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(campaña.estado));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(campaña.tipo));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(campaña.mensajeWhatsApp));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(campaña.duracionCodigoDias));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(campaña.duracionCodigoHoras));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManegeNullBool(campaña.codigoSeReactiva));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullInteger(campaña.duracionReactivacionCodigoDias));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullInteger(campaña.duracionReactivacionCodigoHoras));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullStr(campaña.mensajeWhatsAppReactivacion));

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
        public bool EditarCampaña(CMP_CampañaEntidad campaña) {
            bool respuesta = false;
            string consulta = @"
                UPDATE [dbo].[CMP_Campaña]
                SET
                    [sala_id] = @p0
                    ,[nombre] = @p1
                    ,[descripcion] = @p2
                    ,[fechaini] = @p4
                    ,[fechafin] = @p5
                    ,[estado] = @p6
                    ,[tipo]=@p7
                    ,[duracionCodigoDias]=@p8
                    ,[duracionCodigoHoras]=@p9
                    ,[codigoSeReactiva]=@p10
                    ,[duracionReactivacionCodigoDias]=@p11
                    ,[duracionReactivacionCodigoHoras]=@p12
                WHERE id=@p3
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(campaña.sala_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(campaña.nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(campaña.descripcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(campaña.id));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(campaña.fechaini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(campaña.fechafin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(campaña.estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(campaña.tipo));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(campaña.duracionCodigoDias));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(campaña.duracionCodigoHoras));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManegeNullBool(campaña.codigoSeReactiva));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(campaña.duracionReactivacionCodigoDias));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullInteger(campaña.duracionReactivacionCodigoHoras));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }
        public bool EditarEstadoCampaña(CMP_CampañaEntidad campaña) {
            bool respuesta = false;
            string consulta = @"
                UPDATE [dbo].[CMP_Campaña]
                SET [estado] = @p0
                WHERE id=@p1
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", campaña.estado);
                    query.Parameters.AddWithValue("@p1", campaña.id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public int ActualizarMensajeWhatsAppCampania(string mensajeWhatsApp, string mensajeWhatsAppReactivacion, long idCamapania) {
            int idActualizado = 0;
            string consulta = @"
                UPDATE CMP_Campaña
                SET
                    mensajeWhatsApp = @mensajeWhatsApp,
                    mensajeWhatsAppReactivacion = @mensajeWhatsAppReactivacion
                OUTPUT INSERTED.id
                WHERE id = @idCampania
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idCampania", idCamapania);
                    query.Parameters.AddWithValue("@mensajeWhatsApp", mensajeWhatsApp);
                    query.Parameters.AddWithValue("@mensajeWhatsAppReactivacion", mensajeWhatsAppReactivacion);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idActualizado;
        }
        public bool EditarMultipleEstadoCampaña(string id_lista, int estado) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_Campaña]
                   SET [estado] = @p0
                 WHERE " + id_lista;
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", estado);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public List<CMP_CampañaEntidad> GetListadoCampaniaxTipoyFechas(string salas, DateTime fechaini, DateTime fechafin, int tipo) {
            List<CMP_CampañaEntidad> lista = new List<CMP_CampañaEntidad>();

            string consulta = @" select c.[id]
                                ,c.[sala_id]
                                ,s.Nombre nombresala
                                ,c.[nombre]
                                ,c.[descripcion]
                                ,c.[fechareg]
                                ,c.[fechaini]
                                ,c.[fechafin]
                                ,c.[estado]
                                ,c.[usuario_id]
                                ,u.UsuarioNombre 
                                ,c.[estado]
                                ,c.tipo
                                ,c.[mensajeWhatsApp]
                                ,c.[duracionCodigoDias]
                                ,c.[duracionCodigoHoras]
                                ,c.[codigoSeReactiva]
                                ,c.[duracionReactivacionCodigoDias]
                                ,c.[duracionReactivacionCodigoHoras]
                                ,c.[mensajeWhatsAppReactivacion]
                            FROM 
                    [dbo].[CMP_Campaña] c
                    join Sala s on s.CodSala= c.sala_id
                    join SEG_Usuario u on u.UsuarioID=c.usuario_id 
                     where (c.tipo=@tipo or c.tipo is null) and " + salas + " CONVERT(date, c.fechaini) between @p0 and @p1 order by c.id desc;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    query.Parameters.AddWithValue("@tipo", tipo);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_CampañaEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                fechaini = ManejoNulos.ManageNullDate(dr["fechaini"]),
                                fechafin = ManejoNulos.ManageNullDate(dr["fechafin"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                usuarionombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                mensajeWhatsApp = ManejoNulos.ManageNullStr(dr["mensajeWhatsApp"]),
                                duracionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionCodigoDias"]),
                                duracionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionCodigoHoras"]),
                                codigoSeReactiva = ManejoNulos.ManegeNullBool(dr["codigoSeReactiva"]),
                                duracionReactivacionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoDias"]),
                                duracionReactivacionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoHoras"]),
                                mensajeWhatsAppReactivacion = ManejoNulos.ManageNullStr(dr["mensajeWhatsAppReactivacion"]),
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
        public List<CMP_CampañaEntidad> GetListadoCampaniaSorteoReporte(string salas, DateTime fechaini, DateTime fechafin) {
            List<CMP_CampañaEntidad> lista = new List<CMP_CampañaEntidad>();

            string consulta = @" select c.[id]
                                ,c.[sala_id]
                                ,s.Nombre as nombresala
                                ,c.[nombre]
                                ,c.[descripcion]
                                ,c.[fechareg]
                                ,c.[fechaini]
                                ,c.[fechafin]
                                ,c.[estado]
                                ,c.[usuario_id]
                                ,u.UsuarioNombre 
                                ,c.[estado]
                                ,c.tipo
                                ,c.[mensajeWhatsApp]
                                ,c.[duracionCodigoDias]
                                ,c.[duracionCodigoHoras]
                                ,c.[codigoSeReactiva]
                                ,c.[duracionReactivacionCodigoDias]
                                ,c.[duracionReactivacionCodigoHoras]
                                ,c.[mensajeWhatsAppReactivacion]
                            FROM 
                    [dbo].[CMP_Campaña] c
                    join Sala s on s.CodSala= c.sala_id
                    join SEG_Usuario u on u.UsuarioID=c.usuario_id 
                     where (c.tipo=1) 
and c.id in (select CampaniaId from CMP_CuponesGenerados as cup where convert(date, cup.Fecha) between @p0 and @p1)
and " + salas + " order by c.id desc;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_CampañaEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                fechaini = ManejoNulos.ManageNullDate(dr["fechaini"]),
                                fechafin = ManejoNulos.ManageNullDate(dr["fechafin"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                usuarionombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                mensajeWhatsApp = ManejoNulos.ManageNullStr(dr["mensajeWhatsApp"]),
                                duracionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionCodigoDias"]),
                                duracionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionCodigoHoras"]),
                                codigoSeReactiva = ManejoNulos.ManegeNullBool(dr["codigoSeReactiva"]),
                                duracionReactivacionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoDias"]),
                                duracionReactivacionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoHoras"]),
                                mensajeWhatsAppReactivacion = ManejoNulos.ManageNullStr(dr["mensajeWhatsAppReactivacion"]),
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

        public List<CMP_CampañaEntidad> ListarCampaniasEstadoTipo(int roomId, int cmpStatus, int cmpType) {
            List<CMP_CampañaEntidad> list = new List<CMP_CampañaEntidad>();

            string query = @"
                SELECT 
                    c.[id],
                    c.[sala_id],
                    s.Nombre nombresala,
                    c.[nombre],
                    c.[descripcion],
                    c.[fechareg],
                    c.[fechaini],
                    c.[fechafin],
                    c.[estado],
                    c.[usuario_id],
                    u.UsuarioNombre,
                    c.[estado],
                    c.[mensajeWhatsApp],
                    c.[duracionCodigoDias],
                    c.[duracionCodigoHoras],
                    c.[codigoSeReactiva],
                    c.[duracionReactivacionCodigoDias],
                    c.[duracionReactivacionCodigoHoras],
                    c.[mensajeWhatsAppReactivacion],
                    c.tipo,
                    s.UrlProgresivo
                FROM 
                    [dbo].[CMP_Campaña] c
                INNER JOIN Sala s on s.CodSala= c.sala_id
                INNER JOIN SEG_Usuario u on u.UsuarioID=c.usuario_id
                WHERE (c.sala_id = @p1 AND c.estado = @p2 AND c.tipo = @p3) OR (mostrarTodasSalas = 1 AND c.estado = @p2)
                ORDER BY c.id DESC
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", roomId);
                    command.Parameters.AddWithValue("@p2", cmpStatus);
                    command.Parameters.AddWithValue("@p3", cmpType);

                    using(SqlDataReader dr = command.ExecuteReader()) {
                        while(dr.Read()) {
                            CMP_CampañaEntidad campania = new CMP_CampañaEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                fechaini = ManejoNulos.ManageNullDate(dr["fechaini"]),
                                fechafin = ManejoNulos.ManageNullDate(dr["fechafin"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                mensajeWhatsApp = ManejoNulos.ManageNullStr(dr["mensajeWhatsApp"]),
                                duracionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionCodigoDias"]),
                                duracionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionCodigoHoras"]),
                                codigoSeReactiva = ManejoNulos.ManegeNullBool(dr["codigoSeReactiva"]),
                                duracionReactivacionCodigoDias = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoDias"]),
                                duracionReactivacionCodigoHoras = ManejoNulos.ManageNullInteger(dr["duracionReactivacionCodigoHoras"]),
                                mensajeWhatsAppReactivacion = ManejoNulos.ManageNullStr(dr["mensajeWhatsAppReactivacion"]),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"]),
                                usuarionombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"])
                            };

                            list.Add(campania);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return list;
        }
    }
}
