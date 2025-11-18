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
    public class MI_MaquinaInoperativaPiezasDAL {

        string conexion = string.Empty;
        public MI_MaquinaInoperativaPiezasDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_MaquinaInoperativaPiezasEntidad> GetAllMaquinaInoperativaPiezas() {
            List<MI_MaquinaInoperativaPiezasEntidad> lista = new List<MI_MaquinaInoperativaPiezasEntidad>();
            string consulta = @" SELECT [CodMaquinaInoperativaPiezas]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodPieza]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaPiezas]";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaPiezasEntidad {
                                CodMaquinaInoperativaPiezas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaPiezas"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public List<MI_MaquinaInoperativaPiezasEntidad> GetAllMaquinaInoperativaPiezasActive() {
            List<MI_MaquinaInoperativaPiezasEntidad> lista = new List<MI_MaquinaInoperativaPiezasEntidad>();
            string consulta = @" SELECT [CodMaquinaInoperativaPiezas]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodPieza]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaPiezas] WHERE [Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaPiezasEntidad {
                                CodMaquinaInoperativaPiezas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaPiezas"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public List<MI_MaquinaInoperativaPiezasEntidad> GetAllMaquinaInoperativaPiezasxMaquinaInoperativa(int codMaquinaInoperativa) {
            List<MI_MaquinaInoperativaPiezasEntidad> lista = new List<MI_MaquinaInoperativaPiezasEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativaPiezas]
                                  ,maq.[CodMaquinaInoperativa]
                                  ,maq.[CodPieza]
                                  ,maq.[Cantidad]
                                  ,maq.[FechaRegistro]
                                  ,maq.[FechaModificacion]
                                  ,maq.[CodUsuario]
                                  ,maq.[Estado]
                                  ,pie.Nombre as NombrePieza
                                  ,pie.Descripcion as DescripcionPieza
                              FROM [MI_MaquinaInoperativaPiezas] maq
                              INNER JOIN [MI_Pieza] pie ON maq.CodPieza = pie.CodPieza
                              WHERE [CodMaquinaInoperativa]=@pcodMaquinaInoperativa ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pcodMaquinaInoperativa", codMaquinaInoperativa);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaPiezasEntidad {
                                CodMaquinaInoperativaPiezas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaPiezas"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombrePieza = ManejoNulos.ManageNullStr(dr["NombrePieza"]),
                                DescripcionPieza = ManejoNulos.ManageNullStr(dr["DescripcionPieza"]),
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
        public MI_MaquinaInoperativaPiezasEntidad GetCodMaquinaInoperativaPiezas(int codMaquinaInoperativaPiezas) {
            MI_MaquinaInoperativaPiezasEntidad item = new MI_MaquinaInoperativaPiezasEntidad();
            string consulta = @" SELECT [CodMaquinaInoperativaPiezas]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodPieza]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaPiezas] WHERE [CodMaquinaInoperativaPiezas]=@pCodMaquinaInoperativaPiezas";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaPiezas", codMaquinaInoperativaPiezas);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodMaquinaInoperativaPiezas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaPiezas"]);
                                item.CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]);
                                item.CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]);
                                item.Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]);
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
        public int InsertarMaquinaInoperativaPiezas(MI_MaquinaInoperativaPiezasEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [dbo].[MI_MaquinaInoperativaPiezas]
           ([CodMaquinaInoperativa]
           ,[CodPieza]
           ,[Cantidad]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[CodUsuario]
           ,[Estado])
     OUTPUT Inserted.CodMaquinaInoperativaPiezas   
     VALUES
           (@pCodMaquinaInoperativa
           ,@pCodPieza
           ,@pCantidad
           ,@pFechaRegistro
           ,@pFechaModificacion
           ,@pCodUsuario
           ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodPieza", ManejoNulos.ManageNullInteger(Entidad.CodPieza));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarMaquinaInoperativaPiezas(MI_MaquinaInoperativaPiezasEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaPiezas] SET 
                                 [CodMaquinaInoperativa] = @pCodMaquinaInoperativa
                                  ,[CodPieza] = @pCodPieza
                                  ,[Cantidad] = @pCantidad
                                  ,[FechaRegistro] = @pFechaRegistro
                                  ,[FechaModificacion] = @pFechaModificacion
                                  ,[CodUsuario] = @pCodUsuario
                                  ,[Estado] = @pEstado
                                WHERE  CodMaquinaInoperativaPiezas = @pCodMaquinaInoperativaPiezas";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodPieza", ManejoNulos.ManageNullInteger(Entidad.CodPieza));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaPiezas", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativaPiezas));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarMaquinaInoperativaPiezas(int codMaquinaInoperativaPiezas) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativaPiezas] 
                                WHERE CodMaquinaInoperativaPiezas  = @pCodMaquinaInoperativaPiezas";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaPiezas", ManejoNulos.ManageNullInteger(codMaquinaInoperativaPiezas));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarMaquinaInoperativaPiezasxMaquina(int codMaquinaInoperativa) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativaPiezas] 
                                WHERE CodMaquinaInoperativa  = @pCodMaquinaInoperativa";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(codMaquinaInoperativa));
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
