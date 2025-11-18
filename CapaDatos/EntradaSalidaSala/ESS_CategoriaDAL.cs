using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.EntradaSalidaSala {
    public class ESS_CategoriaDAL {
        string conexion = string.Empty;
        public ESS_CategoriaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_CategoriaEntidad> ListarCategoria() {
            List<ESS_CategoriaEntidad> lista = new List<ESS_CategoriaEntidad>();
            string consulta = @"SELECT [IdCategoria]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_Categoria]";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_CategoriaEntidad {
                                IdCategoria = ManejoNulos.ManageNullInteger(dr["IdCategoria"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                            };
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                lista = new List<ESS_CategoriaEntidad>();
            }
            return lista;
        }
        public List<ESS_CategoriaEntidad> ListarCategoriaPorEstado(int estado) {
            List<ESS_CategoriaEntidad> lista = new List<ESS_CategoriaEntidad>();
            string consulta = @"SELECT [IdCategoria]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_Categoria] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_CategoriaEntidad {
                                IdCategoria = ManejoNulos.ManageNullInteger(dr["IdCategoria"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                            };
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                lista = new List<ESS_CategoriaEntidad>();
            }
            return lista;
        }
        public ESS_CategoriaEntidad ObtenerCategoriaPorId(int id) {
            ESS_CategoriaEntidad item = new ESS_CategoriaEntidad();
            string consulta = @"SELECT [IdCategoria]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_Categoria] where IdCategoria = @IdCategoria";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCategoria", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.IdCategoria = ManejoNulos.ManageNullInteger(dr["IdCategoria"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]);
                                item.UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                item = new ESS_CategoriaEntidad();
            }
            return item;

        }
        public int InsertarCategoria(ESS_CategoriaEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[ESS_Categoria]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdCategoria
     VALUES
           (@Nombre
            ,@Estado
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro).ToUpper().Trim());
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarCategoria(ESS_CategoriaEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ESS_Categoria]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdCategoria = @IdCategoria";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdCategoria", ManejoNulos.ManageNullInteger(model.IdCategoria));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
