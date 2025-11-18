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
    public class ESS_CargoExternoDAL {
        string conexion = string.Empty;
        public ESS_CargoExternoDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_CargoEntidad> ListarCargo() {
            List<ESS_CargoEntidad> lista = new List<ESS_CargoEntidad>();
            string consulta = @"SELECT [IdCargo]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_CargoExterno]";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_CargoEntidad {
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
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
                lista = new List<ESS_CargoEntidad>();
            }
            return lista;
        }
        public List<ESS_CargoEntidad> ListarCargoPorEstado(int estado) {
            List<ESS_CargoEntidad> lista = new List<ESS_CargoEntidad>();
            string consulta = @"SELECT [IdCargo]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_CargoExterno] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_CargoEntidad {
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
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
                lista = new List<ESS_CargoEntidad>();
            }
            return lista;
        }
        public ESS_CargoEntidad ObtenerCargoPorId(int id) {
            ESS_CargoEntidad item = new ESS_CargoEntidad();
            string consulta = @"SELECT [IdCargo]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [dbo].[ESS_CargoExterno] where IdCargo = @IdCargo";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCargo", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]);
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
                item = new ESS_CargoEntidad();
            }
            return item;

        }
        public int InsertarCargo(ESS_CargoEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[ESS_CargoExterno]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdCargo
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
        public bool EditarCargo(ESS_CargoEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ESS_CargoExterno]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdCargo = @IdCargo";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(model.IdCargo));
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
