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
    public class MI_MaquinaInoperativaRepuestosDAL {

        string conexion = string.Empty;
        public MI_MaquinaInoperativaRepuestosDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_MaquinaInoperativaRepuestosEntidad> GetAllMaquinaInoperativaRepuestos() {
            List<MI_MaquinaInoperativaRepuestosEntidad> lista = new List<MI_MaquinaInoperativaRepuestosEntidad>();
            string consulta = @" SELECT [CodMaquinaInoperativaRepuestos]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodRepuesto]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaRepuestos]";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaRepuestosEntidad {
                                CodMaquinaInoperativaRepuestos = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaRepuestos"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
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
        public List<MI_MaquinaInoperativaRepuestosEntidad> GetAllMaquinaInoperativaRepuestosActive() {
            List<MI_MaquinaInoperativaRepuestosEntidad> lista = new List<MI_MaquinaInoperativaRepuestosEntidad>();
            string consulta = @" SELECT [CodMaquinaInoperativaRepuestos]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodRepuesto]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaRepuestos] WHERE [Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaRepuestosEntidad {
                                CodMaquinaInoperativaRepuestos = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaRepuestos"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
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
        public List<MI_MaquinaInoperativaRepuestosEntidad> GetAllMaquinaInoperativaRepuestosxMaquinaInoperativa(int codMaquinaInoperativa) {
            List<MI_MaquinaInoperativaRepuestosEntidad> lista = new List<MI_MaquinaInoperativaRepuestosEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativaRepuestos]
                                  ,maq.[CodMaquinaInoperativa]
                                  ,maq.[CodRepuesto]
                                  ,maq.[Cantidad]
                                  ,maq.[FechaRegistro]
                                  ,maq.[FechaModificacion]
                                  ,maq.[CodUsuario]
                                  ,maq.[Estado]
                                  ,rep.[Nombre] as NombreRepuesto
                              FROM [MI_MaquinaInoperativaRepuestos] maq
                              INNER JOIN [MI_Repuesto] rep ON maq.CodRepuesto = rep.CodRepuesto
                              WHERE [CodMaquinaInoperativa]=@pcodMaquinaInoperativa ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pcodMaquinaInoperativa", codMaquinaInoperativa);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaRepuestosEntidad {
                                CodMaquinaInoperativaRepuestos = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaRepuestos"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public List<MI_MaquinaInoperativaRepuestosEntidad> GetAllMaquinaInoperativaRepuestosAgregadosxMaquinaInoperativa(int codMaquinaInoperativa) {
            List<MI_MaquinaInoperativaRepuestosEntidad> lista = new List<MI_MaquinaInoperativaRepuestosEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativaRepuestos]
                                  ,maq.[CodMaquinaInoperativa]
                                  ,maq.[CodRepuesto]
                                  ,maq.[Cantidad]
                                  ,maq.[FechaRegistro]
                                  ,maq.[FechaModificacion]
                                  ,maq.[CodUsuario]
                                  ,maq.[Estado]
                                  ,rep.[Nombre] as NombreRepuesto
                              FROM [MI_MaquinaInoperativaRepuestos] maq
                              INNER JOIN [MI_Repuesto] rep ON maq.CodRepuesto = rep.CodRepuesto
                              WHERE [CodMaquinaInoperativa]=@pcodMaquinaInoperativa AND maq.Estado IN (0,2,3) ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pcodMaquinaInoperativa", codMaquinaInoperativa);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaRepuestosEntidad {
                                CodMaquinaInoperativaRepuestos = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaRepuestos"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public MI_MaquinaInoperativaRepuestosEntidad GetCodMaquinaInoperativaRepuestos(int codMaquinaInoperativaRepuestos) {
            MI_MaquinaInoperativaRepuestosEntidad item = new MI_MaquinaInoperativaRepuestosEntidad();
            string consulta = @" SELECT [CodMaquinaInoperativaRepuestos]
                                  ,[CodMaquinaInoperativa]
                                  ,[CodRepuesto]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[CodUsuario]
                                  ,[Estado]
                              FROM [MI_MaquinaInoperativaRepuestos] WHERE [CodMaquinaInoperativaRepuestos]=@pCodMaquinaInoperativaRepuestos";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaRepuestos", codMaquinaInoperativaRepuestos);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodMaquinaInoperativaRepuestos = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativaRepuestos"]);
                                item.CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]);
                                item.CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]);
                                item.Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]);
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
        public int InsertarMaquinaInoperativaRepuestos(MI_MaquinaInoperativaRepuestosEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [dbo].[MI_MaquinaInoperativaRepuestos]
           ([CodMaquinaInoperativa]
           ,[CodRepuesto]
           ,[Cantidad]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[CodUsuario]
           ,[Estado])
     OUTPUT Inserted.CodMaquinaInoperativaRepuestos   
     VALUES
           (@pCodMaquinaInoperativa
           ,@pCodRepuesto
           ,@pCantidad
           ,@pFechaRegistro
           ,@pFechaModificacion
           ,@pCodUsuario
           ,@pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
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
        public bool EditarMaquinaInoperativaRepuestos(MI_MaquinaInoperativaRepuestosEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaRepuestos] SET 
                                 [CodMaquinaInoperativa] = @pCodMaquinaInoperativa
                                  ,[CodRepuesto] = @pCodRepuesto
                                  ,[Cantidad] = @pCantidad
                                  ,[FechaRegistro] = @pFechaRegistro
                                  ,[FechaModificacion] = @pFechaModificacion
                                  ,[CodUsuario] = @pCodUsuario
                                  ,[Estado] = @pEstado
                                WHERE  CodMaquinaInoperativaRepuestos = @pCodMaquinaInoperativaRepuestos";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullStr(Entidad.CodUsuario));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaRepuestos", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativaRepuestos));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarMaquinaInoperativaRepuestos(int codMaquinaInoperativaRepuestos) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativaRepuestos] 
                                WHERE CodMaquinaInoperativaRepuestos  = @pCodMaquinaInoperativaRepuestos";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativaRepuestos", ManejoNulos.ManageNullInteger(codMaquinaInoperativaRepuestos));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarMaquinaInoperativaRepuestosxMaquina(int codMaquinaInoperativa) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativaRepuestos] 
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


        public bool AceptarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaRepuestos] SET 
                                Estado  = 2,
                                FechaModificacion  = @pFechaModificacion
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa AND CodRepuesto=@pCodRepuesto AND Cantidad=@pCantidad AND Estado=1 ";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(DateTime.Now));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
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


        public bool RechazarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativaRepuestos] SET 
                                Estado  = 3,
                                FechaModificacion  = @pFechaModificacion
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa AND CodRepuesto=@pCodRepuesto AND Cantidad=@pCantidad AND Estado=1";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(DateTime.Now));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
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
