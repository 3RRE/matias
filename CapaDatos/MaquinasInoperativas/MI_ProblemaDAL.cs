using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_ProblemaDAL {

        string conexion = string.Empty;
        public MI_ProblemaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_ProblemaEntidad> GetAllProblema() {
            List<MI_ProblemaEntidad> lista = new List<MI_ProblemaEntidad>();
            string consulta = @" SELECT [CodProblema]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaProblema]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaProblema
                              FROM [MI_Problema] p
							  INNER JOIN MI_CategoriaProblema cp
							  ON cp.CodCategoriaProblema = p.CodCategoriaProblema ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_ProblemaEntidad {
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaProblema = ManejoNulos.ManageNullStr(dr["NombreCategoriaProblema"]),
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
        public List<MI_ProblemaEntidad> GetAllProblemaActive() {
            List<MI_ProblemaEntidad> lista = new List<MI_ProblemaEntidad>();
            string consulta = @" SELECT [CodProblema]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaProblema]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaProblema
                              FROM [MI_Problema] p
							  INNER JOIN MI_CategoriaProblema cp
							  ON cp.CodCategoriaProblema = p.CodCategoriaProblema
							  WHERE p.[Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_ProblemaEntidad {
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaProblema = ManejoNulos.ManageNullStr(dr["NombreCategoriaProblema"]),
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
        public List<MI_ProblemaEntidad> GetAllProblemaxCategoria(int cod) {
            List<MI_ProblemaEntidad> lista = new List<MI_ProblemaEntidad>();
            string consulta = @" SELECT [CodProblema]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaProblema]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaProblema
                              FROM [MI_Problema] p
							  INNER JOIN MI_CategoriaProblema cp
							  ON cp.CodCategoriaProblema = p.CodCategoriaProblema
							  WHERE p.[Estado]=1 AND p.[CodCategoriaProblema]=@pCodCategoriaProblema ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCategoriaProblema", cod);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_ProblemaEntidad {
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaProblema = ManejoNulos.ManageNullStr(dr["NombreCategoriaProblema"]),
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
        public List<MI_ProblemaEntidad> GetAllProblemaxCategoriaLista(string queryLista) {
            List<MI_ProblemaEntidad> lista = new List<MI_ProblemaEntidad>();
            string consulta = @" SELECT [CodProblema]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaProblema]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaProblema
                              FROM [MI_Problema] p
							  INNER JOIN MI_CategoriaProblema cp
							  ON cp.CodCategoriaProblema = p.CodCategoriaProblema
							  WHERE p.[Estado]=1 AND p.[CodCategoriaProblema] IN "+queryLista;
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_ProblemaEntidad {
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCategoriaProblema = ManejoNulos.ManageNullStr(dr["NombreCategoriaProblema"]),
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
        
        public MI_ProblemaEntidad GetCodProblema(int codProblema) {
            MI_ProblemaEntidad item = new MI_ProblemaEntidad();
            string consulta = @"  SELECT [CodProblema]
                                  ,p.[Nombre]
                                  ,p.[Descripcion]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuario]
                                  ,p.[CodCategoriaProblema]
                                  ,p.[Estado]
                                  ,cp.[Nombre] as NombreCategoriaProblema
                              FROM [MI_Problema] p
							  INNER JOIN MI_CategoriaProblema cp
							  ON cp.CodCategoriaProblema = p.CodCategoriaProblema
							  WHERE [CodProblema]=@pCodProblema";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodProblema", codProblema);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]);
                                item.CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.NombreCategoriaProblema = ManejoNulos.ManageNullStr(dr["NombreCategoriaProblema"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarProblema(MI_ProblemaEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_Problema] ([Nombre],[Descripcion],[FechaRegistro],[FechaModificacion],[CodUsuario],[CodCategoriaProblema],[Estado])
                                OUTPUT Inserted.CodProblema   
	                            values (@pNombre, @pDescripcion, @pFechaRegistro ,@pFechaModificacion ,@pCodUsuario ,@pCodCategoriaProblema ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pCodCategoriaProblema", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaProblema));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarProblema(MI_ProblemaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_Problema] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                FechaModificacion  = @pFechaModificacion,
                                CodCategoriaProblema  = @pCodCategoriaProblema, 
                                Estado  = @pEstado 
                                WHERE  CodProblema = @pCodProblema";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodCategoriaProblema", ManejoNulos.ManageNullInteger(Entidad.CodCategoriaProblema));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodProblema", ManejoNulos.ManageNullInteger(Entidad.CodProblema));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarProblema(int codProblema) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_Problema] 
                                WHERE CodProblema  = @pCodProblema";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodProblema", ManejoNulos.ManageNullInteger(codProblema));
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
