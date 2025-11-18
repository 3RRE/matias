using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CategoriaProblemaDAL {

        string conexion = string.Empty;
        public MI_CategoriaProblemaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_CategoriaProblemaEntidad> GetAllCategoriaProblema() {
            List<MI_CategoriaProblemaEntidad> lista = new List<MI_CategoriaProblemaEntidad>();
            string consulta = @" SELECT [CodCategoriaProblema]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaProblema] (nolock) ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CategoriaProblemaEntidad {
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
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
        public List<MI_CategoriaProblemaEntidad> GetAllCategoriaProblemaActive() {
            List<MI_CategoriaProblemaEntidad> lista = new List<MI_CategoriaProblemaEntidad>();
            string consulta = @" SELECT [CodCategoriaProblema]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaProblema] WHERE [Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CategoriaProblemaEntidad {
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
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
        public MI_CategoriaProblemaEntidad GetCodCategoriaProblema(int codCategoriaProblema) {
            MI_CategoriaProblemaEntidad item = new MI_CategoriaProblemaEntidad();
            string consulta = @" SELECT [CodCategoriaProblema]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaProblema] WHERE [CodCategoriaProblema]=@pCodCategoriaProblema";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaProblema", codCategoriaProblema);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
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
        public int InsertarCategoriaProblema(MI_CategoriaProblemaEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_CategoriaProblema] ([Nombre],[Descripcion],[FechaRegistro],[FechaModificacion],[CodUsuario],[Estado])
                                OUTPUT Inserted.CodCategoriaProblema   
	                            values (@pNombre, @pDescripcion, @pFechaRegistro, @pFechaModificacion, @pCodUsuario, @pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
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
        public bool EditarCategoriaProblema(MI_CategoriaProblemaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_CategoriaProblema] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                FechaModificacion  = @pFechaModificacion,
                                Estado  = @pEstado 
                                WHERE  CodCategoriaProblema = @pCodCategoriaProblema";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodCategoriaProblema", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaProblema));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarCategoriaProblema(int codCategoriaProblema) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_CategoriaProblema] 
                                WHERE CodCategoriaProblema  = @pCodCategoriaProblema";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaProblema", ManejoNulos.ManageNullInteger(codCategoriaProblema));
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
