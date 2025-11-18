using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ControlAcceso {
    public class CAL_RobaStackersBilleteroDAL {

        string conexion = string.Empty;
        public CAL_RobaStackersBilleteroDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_RobaStackersBilleteroEntidad> GetAllRobaStackersBilletero() {
            List<CAL_RobaStackersBilleteroEntidad> lista = new List<CAL_RobaStackersBilleteroEntidad>();
            string consulta = @"SELECT tim.[RobaStackersBilleteroID]
                              ,tim.[Nombre]
                              ,tim.[ApellidoPaterno]
                              ,tim.[ApellidoMaterno]                             
                              ,tim.[FechaRegistro]
                              ,tim.[DNI]
                              ,tim.[Foto]
                              ,tim.[Telefono]
                              ,tim.[Estado]
                              ,tim.[Imagen]
                              ,tim.[EmpleadoID] 
                              ,tim.[CodSala]
                              ,tim.[Observacion]
                              ,tim.[TipoDOI]
                                ,emp.[Nombres] AS [EmpleadoNombres]
                                ,emp.[ApellidosPaterno] AS [EmpleadoApellidoPaterno]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_RobaStackersBilletero] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.EmpleadoID = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.CodSala = sal.CodSala  ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_RobaStackersBilleteroEntidad {
                                RobaStackersBilleteroID = ManejoNulos.ManageNullInteger(dr["RobaStackersBilleteroID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                DNI = ManejoNulos.ManageNullStr(dr["DNI"]),
                                Foto = ManejoNulos.ManageNullStr(dr["Foto"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]),
                                EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]),
                                EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]),
                                SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]),
                                TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]),


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
        public CAL_RobaStackersBilleteroEntidad GetIDRobaStackersBilletero(int id) {
            CAL_RobaStackersBilleteroEntidad item = new CAL_RobaStackersBilleteroEntidad();
            string consulta = @"SELECT tim.[RobaStackersBilleteroID]
                              ,tim.[Nombre]
                              ,tim.[ApellidoPaterno]
                              ,tim.[ApellidoMaterno]                             
                              ,tim.[FechaRegistro]
                              ,tim.[DNI]
                              ,tim.[Foto]
                              ,tim.[Telefono]
                              ,tim.[Estado]
                              ,tim.[Imagen]
                              ,tim.[EmpleadoID] 
                              ,tim.[CodSala]
                              ,tim.[Observacion]
                              ,tim.[TipoDOI]
                                ,emp.[Nombres] AS [EmpleadoNombres]
                                ,emp.[ApellidosPaterno] AS [EmpleadoApellidoPaterno]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_RobaStackersBilletero] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.EmpleadoID = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.CodSala = sal.CodSala  
                              WHERE RobaStackersBilleteroID = @pRobaStackersBilleteroID ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pRobaStackersBilleteroID", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.RobaStackersBilleteroID = ManejoNulos.ManageNullInteger(dr["RobaStackersBilleteroID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]);
                                item.EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]);
                                item.SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]);
                                item.TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarRobaStackersBilletero(CAL_RobaStackersBilleteroEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_RobaStackersBilletero ([Nombre]
                               ,[ApellidoPaterno]
                               ,[ApellidoMaterno]
                               ,[FechaRegistro]
                               ,[DNI]
                               ,[Foto]
                               ,[Telefono]
                               ,[Imagen]
                               ,[Estado]
                               ,[CodSala]
                               ,[Observacion]
                               ,[EmpleadoID]
                               ,[TipoDOI])
                                OUTPUT Inserted.RobaStackersBilleteroID  
                                VALUES(@pNombre 
                               ,@pApellidoPaterno 
                               ,@pApellidoMaterno 
                               ,@pFechaRegistro 
                               ,@pDNI 
                               ,@pFoto 
                               ,@pTelefono 
                               ,@pImagen 
                               ,@pEstado
                               ,@pCodSala
                               ,@pObservacion
                               ,@pEmpleadoID
                               ,@pTipoDOI)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.DNI));
                    query.Parameters.AddWithValue("@pFoto", ManejoNulos.ManageNullStr(Entidad.Foto));
                    query.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.Telefono));
                    query.Parameters.AddWithValue("@pImagen", ManejoNulos.ManageNullStr(Entidad.Imagen));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pObservacion", ManejoNulos.ManageNullStr(Entidad.Observacion));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDOI));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarRobaStackersBilletero(CAL_RobaStackersBilleteroEntidad Entidad) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_RobaStackersBilletero]
                               SET [Nombre] = @pNombre
                                  ,[ApellidoPaterno] = @pApellidoPaterno
                                  ,[ApellidoMaterno] = @pApellidoMaterno             
                                  ,[DNI] = @pDNI
                                  ,[Foto] =@pFoto 
                                  ,[Telefono] = @pTelefono 
                                  ,[Imagen] = @pImagen 
                                  ,[Estado] = @pEstado 
                                  ,[CodSala] = @pCodSala
                                  ,[EmpleadoID] = @pEmpleadoID  
                                  ,[Observacion] = @pObservacion
                                  ,[TipoDOI] = @pTipoDOI
                                   WHERE RobaStackersBilleteroID = @pRobaStackersBilleteroID";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pRobaStackersBilleteroID", ManejoNulos.ManageNullInteger(Entidad.RobaStackersBilleteroID));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.DNI));
                    query.Parameters.AddWithValue("@pFoto", ManejoNulos.ManageNullStr(Entidad.Foto));
                    query.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.Telefono));
                    query.Parameters.AddWithValue("@pImagen", ManejoNulos.ManageNullStr(Entidad.Imagen));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pObservacion", ManejoNulos.ManageNullStr(Entidad.Observacion));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDOI));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarRobaStackersBilletero(int id) {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_RobaStackersBilletero 
                                WHERE RobaStackersBilleteroID  = @pRobaStackersBilleteroID";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pRobaStackersBilleteroID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public CAL_RobaStackersBilleteroEntidad GetRobaStackersBilleteroPorDNI(string dni) {
            CAL_RobaStackersBilleteroEntidad item = new CAL_RobaStackersBilleteroEntidad();
            string consulta = @"SELECT [RobaStackersBilleteroID]
                              ,[Nombre]
                              ,[ApellidoPaterno]
                              ,[ApellidoMaterno]     
                              ,[DNI]
                              ,[Foto]
                              ,[Telefono]
                              ,[Estado]
                              ,[Imagen]
                              ,[CodSala]
                              ,[Observacion]
                              ,[TipoDOI]
                              FROM [CAL_RobaStackersBilletero](nolock) 
                              WHERE DNI = @pDNI ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDNI", dni);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.RobaStackersBilleteroID = ManejoNulos.ManageNullInteger(dr["RobaStackersBilleteroID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
    }
}
