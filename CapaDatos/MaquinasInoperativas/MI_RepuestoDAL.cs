using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_RepuestoDAL {

        string conexion = string.Empty;
        public MI_RepuestoDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_RepuestoEntidad> GetAllRepuesto() {
            List<MI_RepuestoEntidad> lista = new List<MI_RepuestoEntidad>();
            string consulta = @" SELECT [CodRepuesto]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[CostoReferencial]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaRepuesto]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaRepuesto
                              FROM [MI_Repuesto] p
							  INNER JOIN MI_CategoriaRepuesto cp
							  ON cp.CodCategoriaRepuesto = p.CodCategoriaRepuesto ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_RepuestoEntidad {
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                CostoReferencial = ManejoNulos.ManageNullFloat(dr["CostoReferencial"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaRepuesto = ManejoNulos.ManageNullStr(dr["NombreCategoriaRepuesto"]),
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
        public List<MI_RepuestoEntidad> GetAllRepuestoActive() {
            List<MI_RepuestoEntidad> lista = new List<MI_RepuestoEntidad>();
            string consulta = @" SELECT [CodRepuesto]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[CostoReferencial]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaRepuesto]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaRepuesto
                              FROM [MI_Repuesto] p
							  INNER JOIN MI_CategoriaRepuesto cp
							  ON cp.CodCategoriaRepuesto = p.CodCategoriaRepuesto
							  WHERE p.[Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_RepuestoEntidad {
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                CostoReferencial = ManejoNulos.ManageNullFloat(dr["CostoReferencial"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaRepuesto = ManejoNulos.ManageNullStr(dr["NombreCategoriaRepuesto"]),
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
        public List<MI_RepuestoEntidad> GetAllRepuestoxCategoria(int cod) {
            List<MI_RepuestoEntidad> lista = new List<MI_RepuestoEntidad>();
            string consulta = @" SELECT [CodRepuesto]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[CostoReferencial]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaRepuesto]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaRepuesto
                              FROM [MI_Repuesto] p
							  INNER JOIN MI_CategoriaRepuesto cp
							  ON cp.CodCategoriaRepuesto = p.CodCategoriaRepuesto
							  WHERE p.[Estado]=1 AND p.[CodCategoriaRepuesto]=@pCodCategoriaRepuesto ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaRepuesto", cod);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_RepuestoEntidad {
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                CostoReferencial = ManejoNulos.ManageNullFloat(dr["CostoReferencial"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaRepuesto = ManejoNulos.ManageNullStr(dr["NombreCategoriaRepuesto"]),
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
        public MI_RepuestoEntidad GetCodRepuesto(int codRepuesto) {
            MI_RepuestoEntidad item = new MI_RepuestoEntidad();
            string consulta = @"  SELECT [CodRepuesto]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[CostoReferencial]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaRepuesto]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaRepuesto
                              FROM [MI_Repuesto] p
							  INNER JOIN MI_CategoriaRepuesto cp
							  ON cp.CodCategoriaRepuesto = p.CodCategoriaRepuesto
							  WHERE [CodRepuesto]=@pCodRepuesto";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodRepuesto", codRepuesto);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.CostoReferencial = ManejoNulos.ManageNullFloat(dr["CostoReferencial"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]);
                                item.CodCategoriaRepuesto = ManejoNulos.ManageNullInteger(dr["CodCategoriaRepuesto"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.NombreCategoriaRepuesto = ManejoNulos.ManageNullStr(dr["NombreCategoriaRepuesto"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarRepuesto(MI_RepuestoEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_Repuesto] ([Nombre],[Descripcion],[CostoReferencial],[FechaRegistro],[FechaModificacion],[CodUsuario],[CodCategoriaRepuesto],[Estado])
                                OUTPUT Inserted.CodRepuesto   
	                            values (@pNombre, @pDescripcion, @pCostoReferencial, @pFechaRegistro ,@pFechaModificacion ,@pCodUsuario ,@pCodCategoriaRepuesto ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pCostoReferencial", ManejoNulos.ManageNullFloat(Entidad.CostoReferencial));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pCodCategoriaRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaRepuesto));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarRepuesto(MI_RepuestoEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_Repuesto] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                CostoReferencial = @pCostoReferencial, 
                                FechaModificacion  = @pFechaModificacion,
                                CodCategoriaRepuesto  = @pCodCategoriaRepuesto, 
                                Estado  = @pEstado 
                                WHERE  CodRepuesto = @pCodRepuesto";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pCostoReferencial", ManejoNulos.ManageNullFloat(Entidad.CostoReferencial));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodCategoriaRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaRepuesto));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarRepuesto(int codRepuesto) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_Repuesto] 
                                WHERE CodRepuesto  = @pCodRepuesto";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(codRepuesto));
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
