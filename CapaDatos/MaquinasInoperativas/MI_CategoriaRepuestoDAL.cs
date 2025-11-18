using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CategoriaRepuestoDAL {

        string conexion = string.Empty;
        public MI_CategoriaRepuestoDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_CategoriaRepuestoEntidad> GetAllCategoriaRepuesto() {
            List<MI_CategoriaRepuestoEntidad> lista = new List<MI_CategoriaRepuestoEntidad>();
            string consulta = @" SELECT [CodCategoriaRepuesto]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaRepuesto] (nolock) ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CategoriaRepuestoEntidad {
                                CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]),
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
        public List<MI_CategoriaRepuestoEntidad> GetAllCategoriaRepuestoActive() {
            List<MI_CategoriaRepuestoEntidad> lista = new List<MI_CategoriaRepuestoEntidad>();
            string consulta = @" SELECT [CodCategoriaRepuesto]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaRepuesto] WHERE [Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CategoriaRepuestoEntidad {
                                CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]),
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
        public MI_CategoriaRepuestoEntidad GetCodCategoriaRepuesto(int codCategoriaRepuesto) {
            MI_CategoriaRepuestoEntidad item = new MI_CategoriaRepuestoEntidad();
            string consulta = @" SELECT [CodCategoriaRepuesto]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaRepuesto] WHERE [CodCategoriaRepuesto]=@pCodCategoriaRepuesto ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaRepuesto", codCategoriaRepuesto);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]);
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
        public int InsertarCategoriaRepuesto(MI_CategoriaRepuestoEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_CategoriaRepuesto] ([Nombre],[Descripcion],[FechaRegistro],[FechaModificacion],[CodUsuario],[Estado])
                                OUTPUT Inserted.CodCategoriaRepuesto   
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
        public bool EditarCategoriaRepuesto(MI_CategoriaRepuestoEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_CategoriaRepuesto] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                FechaModificacion  = @pFechaModificacion,
                                Estado  = @pEstado 
                                WHERE  CodCategoriaRepuesto = @pCodCategoriaRepuesto";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodCategoriaRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaRepuesto));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarCategoriaRepuesto(int codCategoriaRepuesto) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_CategoriaRepuesto] 
                                WHERE CodCategoriaRepuesto  = @pCodCategoriaRepuesto";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaRepuesto", ManejoNulos.ManageNullInteger(codCategoriaRepuesto));
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
