using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ControlAcceso {
    public class CAL_PersonaProhibidoIngresoDAL {
        private readonly string conexion;
        
        public CAL_PersonaProhibidoIngresoDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        
        public List<CAL_PersonaProhibidoIngresoEntidad> GetAllTimador() {
            List<CAL_PersonaProhibidoIngresoEntidad> lista = new List<CAL_PersonaProhibidoIngresoEntidad>();
            string consulta = @"SELECT tim.[TimadorID]
                              ,tim.[Nombre]
                              ,tim.[ApellidoPaterno]
                              ,tim.[ApellidoMaterno]                             
                              ,tim.[FechaRegistro]
                              ,tim.[DNI]
                              ,tim.[Foto]
                              ,tim.[Telefono]
                              ,tim.[Estado]
                              ,tim.[Imagen]
                              ,tim.[TipoTimadorID]
                              ,tim.[EmpleadoID] 
                              ,tim.[BandaID] 
                              ,tim.[CodSala]
                              ,tim.[Observacion]
                              ,tim.[SustentoLegal]
                              ,tim.[TipoDOI]
                              ,tim.[Prohibir]
                              ,tim.[ConAtenuante]
                              ,tim.[DescripcionAtenuante]
                                ,emp.[Nombres] AS [EmpleadoNombres]
                                ,emp.[ApellidosPaterno] AS [EmpleadoApellidoPaterno]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_Timador] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.EmpleadoID = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.CodSala = sal.CodSala  ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CAL_PersonaProhibidoIngresoEntidad item = new CAL_PersonaProhibidoIngresoEntidad {
                                TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                DNI = ManejoNulos.ManageNullStr(dr["DNI"]),
                                Foto = ManejoNulos.ManageNullStr(dr["Foto"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                TipoTimadorID = ManejoNulos.ManageNullInteger(dr["TipoTimadorID"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                BandaID = ManejoNulos.ManageNullInteger(dr["BandaID"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]),
                                SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]),
                                TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]),
                                Prohibir = ManejoNulos.ManageNullInteger(dr["Prohibir"]),
                                ConAtenuante = ManejoNulos.ManegeNullBool(dr["ConAtenuante"]),
                                DescripcionAtenuante = ManejoNulos.ManageNullStr(dr["DescripcionAtenuante"]),
                                
                                EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]),
                                EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]),
                                SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]),
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
        
        public CAL_PersonaProhibidoIngresoEntidad GetIDTimador(int id) {
            CAL_PersonaProhibidoIngresoEntidad item = new CAL_PersonaProhibidoIngresoEntidad();
            string consulta = @"SELECT tim.[TimadorID]
                              ,tim.[Nombre]
                              ,tim.[ApellidoPaterno]
                              ,tim.[ApellidoMaterno]                             
                              ,tim.[FechaRegistro]
                              ,tim.[DNI]
                              ,tim.[Foto]
                              ,tim.[Telefono]
                              ,tim.[Estado]
                              ,tim.[Imagen]
                              ,tim.[TipoTimadorID]
                              ,tim.[EmpleadoID] 
                              ,tim.[BandaID] 
                              ,tim.[CodSala]
                              ,tim.[Observacion]
                              ,tim.[SustentoLegal]
                              ,tim.[TipoDOI]
                              ,tim.[Prohibir]
                              ,tim.[ConAtenuante]
                              ,tim.[DescripcionAtenuante]
                                ,emp.[Nombres] AS [EmpleadoNombres]
                                ,emp.[ApellidosPaterno] AS [EmpleadoApellidoPaterno]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_Timador] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.EmpleadoID = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.CodSala = sal.CodSala  
                              WHERE TimadorID = @pTimadorID ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorID", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.TipoTimadorID = ManejoNulos.ManageNullInteger(dr["TipoTimadorID"]);
                                item.BandaID = ManejoNulos.ManageNullInteger(dr["BandaID"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]);
                                item.TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]);
                                item.Prohibir = ManejoNulos.ManageNullInteger(dr["Prohibir"]);
                                item.ConAtenuante = ManejoNulos.ManegeNullBool(dr["ConAtenuante"]);
                                item.DescripcionAtenuante = ManejoNulos.ManageNullStr(dr["DescripcionAtenuante"]);

                                item.EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]);
                                item.EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]);
                                item.SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;
        }
        
        public int InsertarTimador(CAL_PersonaProhibidoIngresoEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_Timador ([Nombre]
                               ,[ApellidoPaterno]
                               ,[ApellidoMaterno]
                               ,[FechaRegistro]
                               ,[DNI]
                               ,[Foto]
                               ,[Telefono]
                               ,[Imagen]
                               ,[Estado]
                               ,[TipoTimadorID]
                               ,[CodSala]
                               ,[Observacion]
                               ,[EmpleadoID]
                               ,[BandaID]
                               ,[TipoDOI]
                               ,[Prohibir]
                               ,[SustentoLegal]
                               ,[ConAtenuante]
                               ,[DescripcionAtenuante])
                                OUTPUT Inserted.TimadorID  
                                VALUES(@pNombre 
                               ,@pApellidoPaterno 
                               ,@pApellidoMaterno 
                               ,@pFechaRegistro 
                               ,@pDNI 
                               ,@pFoto 
                               ,@pTelefono 
                               ,@pImagen 
                               ,@pEstado
                               ,@pTipoTimadorID
                               ,@pCodSala
                               ,@pObservacion
                               ,@pEmpleadoID
                               ,@pBandaID
                               ,@pTipoDOI
                               ,@pProhibir
                               ,@pSustentoLegal
                               ,@pConAtenuante
                               ,@pDescripcionAtenuante)";
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
                    query.Parameters.AddWithValue("@pTipoTimadorID", ManejoNulos.ManageNullInteger(Entidad.TipoTimadorID));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pObservacion", ManejoNulos.ManageNullStr(Entidad.Observacion));
                    query.Parameters.AddWithValue("@pBandaID", ManejoNulos.ManageNullInteger(Entidad.BandaID));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDOI));
                    query.Parameters.AddWithValue("@pProhibir", ManejoNulos.ManageNullInteger(Entidad.Prohibir));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pSustentoLegal", ManejoNulos.ManageNullInteger(Entidad.SustentoLegal));
                    query.Parameters.AddWithValue("@pConAtenuante", ManejoNulos.ManegeNullBool(Entidad.ConAtenuante));
                    query.Parameters.AddWithValue("@pDescripcionAtenuante", ManejoNulos.ManageNullStr(Entidad.DescripcionAtenuante));
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
        
        public bool EditarTimador(CAL_PersonaProhibidoIngresoEntidad Entidad) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_Timador]
                               SET [Nombre] = @pNombre
                                  ,[ApellidoPaterno] = @pApellidoPaterno
                                  ,[ApellidoMaterno] = @pApellidoMaterno             
                                  ,[DNI] = @pDNI
                                  ,[Foto] =@pFoto 
                                  ,[Telefono] = @pTelefono 
                                  ,[Imagen] = @pImagen 
                                  ,[Estado] = @pEstado 
                                  ,[TipoTimadorID] = @pTipoTimadorID
                                  ,[CodSala] = @pCodSala
                                  ,[EmpleadoID] = @pEmpleadoID  
                                  ,[BandaID] = @pBandaID
                                  ,[TipoDOI] = @pTipoDOI
                                  ,[Observacion] = @pObservacion
                                  ,[SustentoLegal] = @pSustentoLegal
                                  ,[Prohibir] = @pProhibir
                                  ,[ConAtenuante] = @pConAtenuante
                                  ,[DescripcionAtenuante] = @pDescripcionAtenuante
                                   WHERE TimadorID = @pTimadorID";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorID", ManejoNulos.ManageNullInteger(Entidad.TimadorID));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.DNI));
                    query.Parameters.AddWithValue("@pFoto", ManejoNulos.ManageNullStr(Entidad.Foto));
                    query.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.Telefono));
                    query.Parameters.AddWithValue("@pImagen", ManejoNulos.ManageNullStr(Entidad.Imagen));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pTipoTimadorID", ManejoNulos.ManageNullInteger(Entidad.TipoTimadorID));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pObservacion", ManejoNulos.ManageNullStr(Entidad.Observacion));
                    query.Parameters.AddWithValue("@pBandaID", ManejoNulos.ManageNullInteger(Entidad.BandaID));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDOI));
                    query.Parameters.AddWithValue("@pProhibir", ManejoNulos.ManageNullInteger(Entidad.Prohibir));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pSustentoLegal", ManejoNulos.ManageNullInteger(Entidad.SustentoLegal));
                    query.Parameters.AddWithValue("@pConAtenuante", ManejoNulos.ManegeNullBool(Entidad.ConAtenuante));
                    query.Parameters.AddWithValue("@pDescripcionAtenuante", ManejoNulos.ManageNullStr(Entidad.DescripcionAtenuante));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        
        public bool EliminarTimador(int id) {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_Timador 
                                WHERE TimadorID  = @pTimadorID";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        
        public CAL_PersonaProhibidoIngresoEntidad GetTimadorPorDNI(string dni) {
            CAL_PersonaProhibidoIngresoEntidad item = new CAL_PersonaProhibidoIngresoEntidad();
            string consulta = @"SELECT [TimadorID]
                              ,[Nombre]
                              ,[ApellidoPaterno]
                              ,[ApellidoMaterno]     
                              ,[DNI]
                              ,[Foto]
                              ,[Telefono]
                              ,[Estado]
                              ,[Imagen]
                              ,[TipoTimadorID]
                              ,[BandaID] 
                              ,[CodSala]
                              ,[Observacion]
                              ,[SustentoLegal]
                              ,[TipoDOI]
                              ,[Prohibir]
                              ,[ConAtenuante]
                              ,[DescripcionAtenuante]
                              FROM [CAL_Timador](nolock) 
                              WHERE DNI = @pDNI ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDNI", dni);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.TipoTimadorID = ManejoNulos.ManageNullInteger(dr["TipoTimadorID"]);
                                item.BandaID = ManejoNulos.ManageNullInteger(dr["BandaID"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]);
                                item.TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]);
                                item.Prohibir = ManejoNulos.ManageNullInteger(dr["Prohibir"]);
                                item.ConAtenuante = ManejoNulos.ManegeNullBool(dr["ConAtenuante"]);
                                item.DescripcionAtenuante = ManejoNulos.ManageNullStr(dr["DescripcionAtenuante"]);
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
