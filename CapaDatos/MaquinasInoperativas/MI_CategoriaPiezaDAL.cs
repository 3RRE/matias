using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CategoriaPiezaDAL {

        string conexion = string.Empty;
        public MI_CategoriaPiezaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_CategoriaPiezaEntidad> GetAllCategoriaPieza() {
            List<MI_CategoriaPiezaEntidad> lista = new List<MI_CategoriaPiezaEntidad>();
            string consulta = @" SELECT [CodCategoriaPieza]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaPieza] (nolock) ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CategoriaPiezaEntidad {
                                CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]),
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
        public List<MI_CategoriaPiezaEntidad> GetAllCategoriaPiezaActive() {
            List<MI_CategoriaPiezaEntidad> lista = new List<MI_CategoriaPiezaEntidad>();
            string consulta = @" SELECT [CodCategoriaPieza]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaPieza] WHERE [Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CategoriaPiezaEntidad {
                                CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]),
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
        public MI_CategoriaPiezaEntidad GetCodCategoriaPieza(int codCategoriaPieza) {
            MI_CategoriaPiezaEntidad item = new MI_CategoriaPiezaEntidad();
            string consulta = @" SELECT [CodCategoriaPieza]
                                  ,[Nombre]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_CategoriaPieza] WHERE [CodCategoriaPieza]=@pCodCategoriaPieza ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaPieza", codCategoriaPieza);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]);
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
        public int InsertarCategoriaPieza(MI_CategoriaPiezaEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_CategoriaPieza] ([Nombre],[Descripcion],[FechaRegistro],[FechaModificacion],[CodUsuario],[Estado])
                                OUTPUT Inserted.CodCategoriaPieza   
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
        public bool EditarCategoriaPieza(MI_CategoriaPiezaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_CategoriaPieza] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                FechaModificacion  = @pFechaModificacion,
                                Estado  = @pEstado 
                                WHERE  CodCategoriaPieza = @pCodCategoriaPieza";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodCategoriaPieza", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaPieza));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarCategoriaPieza(int codCategoriaPieza) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_CategoriaPieza] 
                                WHERE CodCategoriaPieza  = @pCodCategoriaPieza";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaPieza", ManejoNulos.ManageNullInteger(codCategoriaPieza));
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
