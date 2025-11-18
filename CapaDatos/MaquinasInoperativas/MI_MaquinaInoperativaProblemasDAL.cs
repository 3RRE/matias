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
    public class MI_MaquinaInoperativaProblemasDAL {

        string conexion = string.Empty;
        public MI_MaquinaInoperativaProblemasDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_MaquinaInoperativaProblemasEntidad> GetAllMaquinaInoperativaProblemas() {
            List<MI_MaquinaInoperativaProblemasEntidad> lista = new List<MI_MaquinaInoperativaProblemasEntidad>();
            string consulta = @" SELECT [CodMaquinaInoperativaProblemas]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodProblema]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaProblemas]";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaProblemasEntidad {
                                CodMaquinaInoperativaProblemas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaProblemas"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
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
        public List<MI_MaquinaInoperativaProblemasEntidad> GetAllMaquinaInoperativaProblemasActive() {
            List<MI_MaquinaInoperativaProblemasEntidad> lista = new List<MI_MaquinaInoperativaProblemasEntidad>();
            string consulta = @" SELECT [CodMaquinaInoperativaProblemas]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodProblema]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaProblemas] WHERE [Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaProblemasEntidad {
                                CodMaquinaInoperativaProblemas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaProblemas"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
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
        public List<MI_MaquinaInoperativaProblemasEntidad> GetAllMaquinaInoperativaProblemasxMaquinaInoperativa(int codMaquinaInoperativa) {
            List<MI_MaquinaInoperativaProblemasEntidad> lista = new List<MI_MaquinaInoperativaProblemasEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativaProblemas]
                                  ,maq.[CodMaquinaInoperativa]
                                  ,maq.[CodProblema]
                                  ,maq.[FechaRegistro]
                                  ,maq.[FechaModificacion]
                                  ,maq.[CodUsuario]
                                  ,maq.[Estado]
                                  ,pro.[Nombre] as NombreProblema
                                  ,pro.[Descripcion] as DescripcionProblema
                                  ,pro.[CodCategoriaProblema] as CodCategoriaProblema
                                  ,maq.[NombreRepuesto]
                              FROM [MI_MaquinaInoperativaProblemas] maq
                              INNER JOIN [MI_Problema] pro ON maq.CodProblema = pro.CodProblema
                              WHERE [CodMaquinaInoperativa]=@pcodMaquinaInoperativa";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pcodMaquinaInoperativa", codMaquinaInoperativa);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaProblemasEntidad {
                                CodMaquinaInoperativaProblemas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaProblemas"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreProblema = ManejoNulos.ManageNullStr(dr["NombreProblema"]),
                                DescripcionProblema = ManejoNulos.ManageNullStr(dr["NombreProblema"]),
                                CodCategoriaProblema = ManejoNulos.ManageNullInteger(dr["CodCategoriaProblema"]),
                                NombreRepuesto = ManejoNulos.ManageNullStr(dr["NombreRepuesto"]),
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
        public MI_MaquinaInoperativaProblemasEntidad GetCodMaquinaInoperativaProblemas(int codMaquinaInoperativaProblemas) {
            MI_MaquinaInoperativaProblemasEntidad item = new MI_MaquinaInoperativaProblemasEntidad();
            string consulta = @" SELECT [CodMaquinaInoperativaProblemas]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodProblema]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaProblemas] WHERE [CodMaquinaInoperativaProblemas]=@pCodMaquinaInoperativaProblemas";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaProblemas", codMaquinaInoperativaProblemas);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodMaquinaInoperativaProblemas = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaProblemas"]);
                                item.CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]);
                                item.CodProblema = ManejoNulos.ManageNullInteger(dr["CodProblema"]);
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
        public int InsertarMaquinaInoperativaProblemas(MI_MaquinaInoperativaProblemasEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [dbo].[MI_MaquinaInoperativaProblemas]
           ([CodMaquinaInoperativa]
           ,[CodProblema]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[CodUsuario]
           ,[Estado])
     OUTPUT Inserted.CodMaquinaInoperativaProblemas   
     VALUES
           (@pCodMaquinaInoperativa
           ,@pCodProblema
           ,@pFechaRegistro
           ,@pFechaModificacion
           ,@pCodUsuario
           ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodProblema", ManejoNulos.ManageNullInteger(Entidad.CodProblema));
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
        public bool EditarMaquinaInoperativaProblemas(MI_MaquinaInoperativaProblemasEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaProblemas] SET 
                               
                                  [FechaModificacion] = @pFechaModificacion
                                  ,[CodUsuario] = @pCodUsuario
                                  ,[Estado] = @pEstado
                                  ,[NombreRepuesto] = @pNombreRepuesto
                                WHERE  CodMaquinaInoperativaProblemas = @pCodMaquinaInoperativaProblemas";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pNombreRepuesto", ManejoNulos.ManageNullStr(Entidad.NombreRepuesto));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaProblemas", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativaProblemas));
                    int rows = query.ExecuteNonQuery();
                    if(rows == 0) {
                        return respuesta;

                    }
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarMaquinaInoperativaProblemas(int codMaquinaInoperativaProblemas) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativaProblemas] 
                                WHERE CodMaquinaInoperativaProblemas  = @pCodMaquinaInoperativaProblemas";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaProblemas", ManejoNulos.ManageNullInteger(codMaquinaInoperativaProblemas));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarMaquinaInoperativaProblemasxMaquina(int codMaquinaInoperativa) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativaProblemas] 
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
        public bool AceptarMaquinaInoperativaProblemas(int cod)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaProblemas] SET [Estado] = 2
                                WHERE  CodMaquinaInoperativaProblemas = @pCodMaquinaInoperativaProblemas";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaProblemas", ManejoNulos.ManageNullInteger(cod));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool RechazarMaquinaInoperativaProblemas(int cod)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaProblemas] SET [Estado] = 3
                                WHERE  CodMaquinaInoperativaProblemas = @pCodMaquinaInoperativaProblemas";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaProblemas", ManejoNulos.ManageNullInteger(cod));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
