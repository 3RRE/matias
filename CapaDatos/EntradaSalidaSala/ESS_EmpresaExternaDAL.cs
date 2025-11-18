using CapaEntidad.ControlAcceso;
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
    public class ESS_EmpresaExternaDAL {
        string conexion = string.Empty;
        public ESS_EmpresaExternaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_EmpresaEntidad> ListarEmpresa() {
            List<ESS_EmpresaEntidad> lista = new List<ESS_EmpresaEntidad>();
            string consulta = @"SELECT [IdEmpresaExterna]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_EmpresaExterna]";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EmpresaEntidad {
                                IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]),
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
                lista = new List<ESS_EmpresaEntidad>();
            }
            return lista;
        }
        public List<ESS_EmpresaEntidad> ListarEmpresaPorEstado(int estado) {
            List<ESS_EmpresaEntidad> lista = new List<ESS_EmpresaEntidad>();
            string consulta = @"SELECT [IdEmpresaExterna]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_EmpresaExterna] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EmpresaEntidad {
                                IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]),
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
                lista = new List<ESS_EmpresaEntidad>();
            }
            return lista;
        }
        public ESS_EmpresaEntidad ObtenerEmpresaPorId(int id) {
            ESS_EmpresaEntidad item = new ESS_EmpresaEntidad();
            string consulta = @"SELECT [IdEmpresaExterna]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_EmpresaExterna] where IdEmpresaExterna = @IdEmpresaExterna";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpresaExterna", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]);
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
                item = new ESS_EmpresaEntidad();
            }
            return item;

        }
        public int InsertarEmpresa(ESS_EmpresaEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[ESS_EmpresaExterna]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdEmpresaExterna
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
        public bool EditarEmpresa(ESS_EmpresaEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ESS_EmpresaExterna]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdEmpresaExterna = @IdEmpresaExterna";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdEmpresaExterna", ManejoNulos.ManageNullInteger(model.IdEmpresaExterna));
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
