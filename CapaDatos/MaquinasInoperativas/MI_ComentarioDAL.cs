using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_ComentarioDAL {



        string conexion = string.Empty;
        public MI_ComentarioDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<MI_ComentarioEntidad> GetAllComentariosxMaquina(int cod) {
            List<MI_ComentarioEntidad> lista = new List<MI_ComentarioEntidad>();
            string consulta = @" SELECT com.[CodComentario]
                                        ,com.[Texto]
                                        ,com.[FechaRegistro]
                                        ,com.[FechaModificacion]
                                        ,com.[CodUsuario]
                                        ,com.[CodMaquinaInoperativa]
                                        ,com.[EstadoProceso]
                                        ,com.[CorreoEnviado]
                                        ,com.[Estado]
                                        ,TRIM(emp.Nombres) +' '+  TRIM(emp.ApellidosPaterno) +' '+TRIM(emp.ApellidosMaterno) NombreCompleto
                                        ,emp.Nombres
                                        ,emp.ApellidosPaterno
                                        ,emp.ApellidosMaterno
                                    FROM [BD_SEGURIDAD_PJ].[dbo].[MI_Comentario] com
                                    INNER JOIN SEG_Usuario usu ON com.CodUsuario = usu.UsuarioID
                                    INNER JOIN SEG_Empleado emp ON usu.EmpleadoID = emp.EmpleadoID
                                    WHERE com.CodMaquinaInoperativa=@pCodMaquinaInoperativa AND com.Estado=1
									ORDER BY FechaRegistro DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", cod);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_ComentarioEntidad {
                                CodComentario = ManejoNulos.ManageNullInteger(dr["CodComentario"]),
                                Texto = ManejoNulos.ManageNullStr(dr["Texto"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullInteger(dr["CodUsuario"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                EstadoProceso = ManejoNulos.ManageNullInteger(dr["EstadoProceso"]),
                                CorreoEnviado = ManejoNulos.ManageNullInteger(dr["CorreoEnviado"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"]),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"]),
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


        public MI_ComentarioEntidad GetComentarioxCod(int cod) {
            MI_ComentarioEntidad item = new MI_ComentarioEntidad();
            string consulta = @" SELECT com.[CodComentario]
                                        ,com.[CodUsuario]
                                        ,com.[Estado]
                                    FROM [BD_SEGURIDAD_PJ].[dbo].[MI_Comentario] com
                                    WHERE com.CodComentario=@pCodComentario AND com.Estado=1";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodComentario", cod);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodComentario = ManejoNulos.ManageNullInteger(dr["CodComentario"]);
                                item.CodUsuario = ManejoNulos.ManageNullInteger(dr["CodUsuario"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }

        public int InsertarComentario(MI_ComentarioEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_Comentario]
                                   ([Texto]
                                   ,[FechaRegistro]
                                   ,[FechaModificacion]
                                   ,[CodUsuario]
                                   ,[CodMaquinaInoperativa]
                                   ,[EstadoProceso]
                                   ,[CorreoEnviado]
                                   ,[Estado])
                                OUTPUT Inserted.CodComentario   
	                            VALUES
                                   (@pTexto
                                   ,@pFechaRegistro
                                   ,@pFechaModificacion
                                   ,@pCodUsuario
                                   ,@pCodMaquinaInoperativa
                                   ,@pEstadoProceso
                                   ,@pCorreoEnviado
                                   ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTexto", ManejoNulos.ManageNullStr(Entidad.Texto).Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullInteger(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.EstadoProceso));
                    query.Parameters.AddWithValue("@pCorreoEnviado", ManejoNulos.ManageNullInteger(Entidad.CorreoEnviado));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManegeNullBool(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EliminarComentario(int cod) {
            bool respuesta = false;
            string consulta = @"UPDATE [MI_Comentario] 
                                SET Estado=0
                                WHERE CodComentario  = @pCodComentario";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodComentario", ManejoNulos.ManageNullInteger(cod));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
