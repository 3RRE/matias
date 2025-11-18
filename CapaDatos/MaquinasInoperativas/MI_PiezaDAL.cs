using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_PiezaDAL {

        string conexion = string.Empty;
        public MI_PiezaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_PiezaEntidad> GetAllPieza() {
            List<MI_PiezaEntidad> lista = new List<MI_PiezaEntidad>();
            string consulta = @" SELECT [CodPieza]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaPieza]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaPieza
                              FROM [MI_Pieza] p
							  INNER JOIN MI_CategoriaPieza cp
							  ON cp.CodCategoriaPieza = p.CodCategoriaPieza";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaEntidad {
                                CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaPieza = ManejoNulos.ManageNullStr(dr["NombreCategoriaPieza"]),
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
        public List<MI_PiezaEntidad> GetAllPiezaActive() {
            List<MI_PiezaEntidad> lista = new List<MI_PiezaEntidad>();
            string consulta = @" SELECT [CodPieza]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaPieza]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaPieza
                              FROM [MI_Pieza] p
							  INNER JOIN MI_CategoriaPieza cp
							  ON cp.CodCategoriaPieza = p.CodCategoriaPieza
							  WHERE p.[Estado]=1";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaEntidad {
                                CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaPieza = ManejoNulos.ManageNullStr(dr["NombreCategoriaPieza"]),
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
        public List<MI_PiezaEntidad> GetAllPiezaxCategoria(int cod) {
            List<MI_PiezaEntidad> lista = new List<MI_PiezaEntidad>();
            string consulta = @" SELECT [CodPieza]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaPieza]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaPieza
                              FROM [MI_Pieza] p
							  INNER JOIN MI_CategoriaPieza cp
							  ON cp.CodCategoriaPieza = p.CodCategoriaPieza
							  WHERE p.[Estado]=1 AND p.[CodCategoriaPieza]=@pCodCategoriaPieza";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaPieza", cod);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaEntidad {
                                CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaPieza = ManejoNulos.ManageNullStr(dr["NombreCategoriaPieza"]),
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
        public MI_PiezaEntidad GetCodPieza(int codPieza) {
            MI_PiezaEntidad item = new MI_PiezaEntidad();
            string consulta = @" SELECT [CodPieza]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaPieza]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaPieza
                              FROM [MI_Pieza] p
							  INNER JOIN MI_CategoriaPieza cp
							  ON cp.CodCategoriaPieza = p.CodCategoriaPieza
							  WHERE [CodPieza]=@pCodPieza";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPieza", codPieza);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodPieza = ManejoNulos.ManageNullInteger(dr["CodPieza"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]);
                                item.CodCategoriaPieza = ManejoNulos.ManageNullInteger(dr["CodCategoriaPieza"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.NombreCategoriaPieza = ManejoNulos.ManageNullStr(dr["NombreCategoriaPieza"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarPieza(MI_PiezaEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_Pieza] ([Nombre],[Descripcion],[FechaRegistro],[FechaModificacion],[CodUsuario],[CodCategoriaPieza],[Estado])
                                OUTPUT Inserted.CodPieza   
	                            values (@pNombre, @pDescripcion, @pFechaRegistro ,@pFechaModificacion ,@pCodUsuario ,@pCodCategoriaPieza ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pCodCategoriaPieza", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaPieza));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarPieza(MI_PiezaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_Pieza] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                FechaModificacion  = @pFechaModificacion,
                                CodCategoriaPieza  = @pCodCategoriaPieza, 
                                Estado  = @pEstado 
                                WHERE  CodPieza = @pCodPieza";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodCategoriaPieza", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaPieza));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodPieza", ManejoNulos.ManageNullInteger(Entidad.CodPieza));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarPieza(int codPieza) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_Pieza] 
                                WHERE CodPieza  = @pCodPieza";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPieza", ManejoNulos.ManageNullInteger(codPieza));
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
