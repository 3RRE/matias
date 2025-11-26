using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos {
    public class SalaDAL {
        string _conexion = string.Empty;
        public SalaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<SalaEntidad> ListadoSala() {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT   CodSalaMaestra,CodSala,CodEmpresa,CodUbigeo,Nombre,NombreCorto,Direccion
      ,FechaAniversario,UrlSistemaOnline,NroMaquinasRD,FechaRegistro
      ,FechaModificacion,Activo,Estado,CodRD,CodUsuario
      ,CodRRHH,NroActasSorteos,CodRRHHTecnicos ,RutaArchivoLogo,CodOB
      ,UrlProgresivo,IpPublica,UrlCuadre,UrlPlayerTracking
      ,NombresAdministrador ,ApellidosAdministrador
      ,DniAdministrador ,FirmaAdministrador ,CodigoEstablecimiento
      ,CodRegion,UrlBoveda,UrlSalaOnline,longitud,latitud,correo,tipo
  FROM Sala (nolock) where estado = 1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCorto = ManejoNulos.ManageNullStr(dr["NombreCorto"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                FechaAniversario = ManejoNulos.ManageNullDate(dr["FechaAniversario"]),
                                UrlSistemaOnline = ManejoNulos.ManageNullStr(dr["UrlSistemaOnline"]),
                                NroMaquinasRD = ManejoNulos.ManageNullInteger(dr["NroMaquinasRD"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                NroActasSorteos = ManejoNulos.ManageNullInteger(dr["NroActasSorteos"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                CodOB = ManejoNulos.ManageNullStr(dr["CodOB"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                IpPublica = ManejoNulos.ManageNullStr(dr["IpPublica"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlPlayerTracking = ManejoNulos.ManageNullStr(dr["UrlPlayerTracking"]),
                                NombresAdministrador = ManejoNulos.ManageNullStr(dr["NombresAdministrador"]),
                                ApellidosAdministrador = ManejoNulos.ManageNullStr(dr["ApellidosAdministrador"]),
                                DniAdministrador = ManejoNulos.ManageNullStr(dr["DniAdministrador"]),
                                FirmaAdministrador = ManejoNulos.ManageNullStr(dr["FirmaAdministrador"]),
                                CodigoEstablecimiento = ManejoNulos.ManageNullStr(dr["CodigoEstablecimiento"]),
                                CodRegion = ManejoNulos.ManageNullInteger(dr["CodRegion"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                latitud = ManejoNulos.ManageNullStr(dr["latitud"]),
                                longitud = ManejoNulos.ManageNullStr(dr["longitud"]),
                                correo = ManejoNulos.ManageNullStr(dr["correo"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }
        public SalaEntidad SalaListaIdJson(int salaId) {
            SalaEntidad sala = new SalaEntidad();
            string consulta = @"SELECT   CodSalaMaestra,CodSala,CodEmpresa,CodUbigeo,Nombre,NombreCorto,Direccion
      ,FechaAniversario,UrlSistemaOnline,NroMaquinasRD,FechaRegistro
      ,FechaModificacion,Activo,Estado,CodRD,CodUsuario
      ,CodRRHH,NroActasSorteos,CodRRHHTecnicos ,RutaArchivoLogo,CodOB
      ,UrlProgresivo,IpPublica,UrlCuadre,UrlPlayerTracking
      ,NombresAdministrador ,ApellidosAdministrador
      ,DniAdministrador ,FirmaAdministrador ,CodigoEstablecimiento
      ,CodRegion,UrlBoveda,UrlSalaOnline,latitud,longitud,correo,tipo,PuertoSignalr,IpPrivada,PuertoServicioWebOnline
  FROM Sala (nolock) where CodSala = @pCodSala ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", salaId);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCorto = ManejoNulos.ManageNullStr(dr["NombreCorto"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                FechaAniversario = ManejoNulos.ManageNullDate(dr["FechaAniversario"]),
                                UrlSistemaOnline = ManejoNulos.ManageNullStr(dr["UrlSistemaOnline"]),
                                NroMaquinasRD = ManejoNulos.ManageNullInteger(dr["NroMaquinasRD"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                NroActasSorteos = ManejoNulos.ManageNullInteger(dr["NroActasSorteos"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                CodOB = ManejoNulos.ManageNullStr(dr["CodOB"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                IpPublica = ManejoNulos.ManageNullStr(dr["IpPublica"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlPlayerTracking = ManejoNulos.ManageNullStr(dr["UrlPlayerTracking"]),
                                NombresAdministrador = ManejoNulos.ManageNullStr(dr["NombresAdministrador"]),
                                ApellidosAdministrador = ManejoNulos.ManageNullStr(dr["ApellidosAdministrador"]),
                                DniAdministrador = ManejoNulos.ManageNullStr(dr["DniAdministrador"]),
                                FirmaAdministrador = ManejoNulos.ManageNullStr(dr["FirmaAdministrador"]),
                                CodigoEstablecimiento = ManejoNulos.ManageNullStr(dr["CodigoEstablecimiento"]),
                                CodRegion = ManejoNulos.ManageNullInteger(dr["CodRegion"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                latitud = ManejoNulos.ManageNullStr(dr["latitud"]),
                                longitud = ManejoNulos.ManageNullStr(dr["longitud"]),
                                correo = ManejoNulos.ManageNullStr(dr["correo"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                PuertoSignalr = ManejoNulos.ManageNullInteger(dr["PuertoSignalr"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                                PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(dr["PuertoServicioWebOnline"]),
                            };
                            sala = item;
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return sala;
        }
        public bool SalaModificarJson(SalaEntidad sala) {

            string consulta = @"update Sala set CodSalaMaestra = @pCodSalaMaestra, Nombre = @pNombre , NombreCorto = @pNombreCorto , UrlProgresivo = @pUrlProgresivo , UrlBoveda = @UrlBoveda, UrlSalaOnline=@UrlSalaOnline
    , latitud=@latitud, longitud=@longitud, correo=@correo, CodUbigeo=@codUbigeo, FechaAniversario=@FechaAniversario, Direccion=@Direccion, tipo=@tipo, IpPublica=@IpPublica, RutaArchivoLogo=@RutaArchivoLogo, PuertoSignalr=@PuertoSignalr
where CodSala = @pCodSala";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", sala.CodSala);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", sala.CodSalaMaestra);
                    query.Parameters.AddWithValue("@pNombre", sala.Nombre);
                    query.Parameters.AddWithValue("@pNombreCorto", sala.NombreCorto);
                    query.Parameters.AddWithValue("@pUrlProgresivo", sala.UrlProgresivo == null ? "" : sala.UrlProgresivo);
                    query.Parameters.AddWithValue("@UrlBoveda", sala.UrlBoveda == null ? "" : sala.UrlBoveda);
                    query.Parameters.AddWithValue("@UrlSalaOnline", sala.UrlSalaOnline == null ? "" : sala.UrlSalaOnline);
                    query.Parameters.AddWithValue("@latitud", ManejoNulos.ManageNullStr(sala.latitud));
                    query.Parameters.AddWithValue("@longitud", ManejoNulos.ManageNullStr(sala.longitud));
                    query.Parameters.AddWithValue("@correo", ManejoNulos.ManageNullStr(sala.correo));
                    query.Parameters.AddWithValue("@codUbigeo", ManejoNulos.ManageNullInteger(sala.CodUbigeo));
                    query.Parameters.AddWithValue("@FechaAniversario", ManejoNulos.ManageNullDate(sala.FechaAniversario));
                    query.Parameters.AddWithValue("@Direccion", ManejoNulos.ManageNullStr(sala.Direccion));
                    query.Parameters.AddWithValue("@tipo", ManejoNulos.ManageNullInteger(sala.tipo));
                    query.Parameters.AddWithValue("@IpPublica", ManejoNulos.ManageNullStr(sala.IpPublica));
                    query.Parameters.AddWithValue("@RutaArchivoLogo", ManejoNulos.ManageNullStr(sala.RutaArchivoLogo));
                    query.Parameters.AddWithValue("@PuertoSignalr", ManejoNulos.ManageNullInteger(sala.PuertoSignalr));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
        }
        public List<SalaEntidad> ListadoSalaPorUsuario(int UsuarioId) {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT CodSalaMaestra,CodSala,CodEmpresa,Nombre,UrlProgresivo, UrlCuadre,UrlBoveda,UrlSalaOnline,tipo,PuertoSignalr,IpPrivada,PuertoServicioWebOnline
  FROM Sala (nolock) 
  inner join UsuarioSala on UsuarioSala.SalaId= Sala.CodSala
  where Sala.estado = 1 and UsuarioSala.UsuarioId=@p0 order by nombre asc
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(UsuarioId));
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                PuertoSignalr = ManejoNulos.ManageNullInteger(dr["PuertoSignalr"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                                PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(dr["PuertoServicioWebOnline"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }
        public List<SalaEntidad> ListadoSalaPorUsuarioOfisis(int UsuarioId) {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT CodSalaMaestra,CodSala,CodEmpresa,Nombre, CodEmpresaOfisis,CodSalaOfisis, CodEmpresaOfisis + '-'+CodSalaOfisis as CodOfisis
  FROM Sala (nolock) 
  inner join UsuarioSala on UsuarioSala.SalaId= Sala.CodSala
  where Sala.estado = 1 and UsuarioSala.UsuarioId=@p0 order by nombre asc
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(UsuarioId));
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSalaOfisis = ManejoNulos.ManageNullStr(dr["CodSalaOfisis"]),
                                CodOfisis = ManejoNulos.ManageNullStr(dr["CodOfisis"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }
        public bool InsertarSalaJson(SalaEntidad sala) {
            //bool respuesta = false;
            bool respuesta = false;
            string consulta = @"
                            INSERT INTO [dbo].[Sala]
                                       ([CodSalaMaestra]
                                       ,[CodSala]
                                       ,[CodEmpresa]
                                       ,[CodUbigeo]
                                       ,[Nombre]
                                       ,[NombreCorto]
                                       ,[Direccion]
                                       ,[FechaAniversario]
                                       ,[UrlSistemaOnline]
                                       ,[NroMaquinasRD]
                                       ,[FechaRegistro]
                                       ,[FechaModificacion]
                                       ,[Activo]
                                       ,[Estado]
                                       ,[CodRD]
                                       ,[CodUsuario]
                                       ,[CodRRHH]
                                       ,[NroActasSorteos]
                                       ,[CodRRHHTecnicos]
                                       ,[RutaArchivoLogo]
                                       ,[CodOB]
                                       ,[UrlProgresivo]
                                       ,[IpPublica]
                                       ,[UrlCuadre]
                                       ,[UrlPlayerTracking]
                                       ,[NombresAdministrador]
                                       ,[ApellidosAdministrador]
                                       ,[DniAdministrador]
                                       ,[FirmaAdministrador]
                                       ,[CodigoEstablecimiento]
                                       ,[CodRegion]
                                       ,[UrlBoveda]
                                       ,[UrlSalaOnline]
                                       ,[latitud]
                                       ,[longitud]
                                       ,[correo]
                                        ,[tipo]
                                        ,[PuertoSignalr])
                                 VALUES
                                       (@pCodSalaMaestra
                                       ,@p0
                                       ,@p1
                                       ,@p2
                                       ,@p3
                                       ,@p4
                                       ,@p5
                                       ,@p6
                                       ,@p7
                                       ,@p8
                                       ,@p9
                                       ,@p10
                                       ,@p11
                                       ,@p12
                                       ,@p13
                                       ,@p14
                                       ,@p15
                                       ,@p16
                                       ,@p17
                                       ,@p18
                                       ,@p19
                                       ,@p20
                                       ,@p21
                                       ,@p22
                                       ,@p23
                                       ,@p24
                                       ,@p25
                                       ,@p26
                                       ,@p27
                                       ,@p28
                                       ,@p29
                                       ,@p30
                                       ,@p31
                                       ,@p32
                                       ,@p33
                                       ,@p34    
                                        ,@p35   
                                        ,@p36)
                            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", ManejoNulos.ManageNullInteger(sala.CodSalaMaestra));
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(sala.CodSala));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(sala.CodEmpresa));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(sala.CodUbigeo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(sala.Nombre));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(sala.NombreCorto));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(sala.Direccion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(sala.FechaAniversario));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(sala.UrlSistemaOnline));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(sala.NroMaquinasRD));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullDate(sala.FechaRegistro));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullDate(sala.FechaModificacion));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(sala.Activo));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullInteger(sala.Estado));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullInteger(sala.CodRD));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(sala.CodUsuario));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(sala.CodRRHH));
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullInteger(sala.NroActasSorteos));
                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullInteger(sala.CodRRHHTecnicos));
                    query.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullStr(sala.RutaArchivoLogo));
                    query.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullStr(sala.CodOB));
                    query.Parameters.AddWithValue("@p20", ManejoNulos.ManageNullStr(sala.UrlProgresivo));
                    query.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullStr(sala.IpPublica));
                    query.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullStr(sala.UrlCuadre));
                    query.Parameters.AddWithValue("@p23", ManejoNulos.ManageNullStr(sala.UrlPlayerTracking));
                    query.Parameters.AddWithValue("@p24", ManejoNulos.ManageNullStr(sala.NombresAdministrador));
                    query.Parameters.AddWithValue("@p25", ManejoNulos.ManageNullStr(sala.ApellidosAdministrador));
                    query.Parameters.AddWithValue("@p26", ManejoNulos.ManageNullStr(sala.DniAdministrador));
                    query.Parameters.AddWithValue("@p27", ManejoNulos.ManageNullStr(sala.FirmaAdministrador));
                    query.Parameters.AddWithValue("@p28", ManejoNulos.ManageNullStr(sala.CodigoEstablecimiento));
                    query.Parameters.AddWithValue("@p29", ManejoNulos.ManageNullInteger(sala.CodRegion));
                    query.Parameters.AddWithValue("@p30", ManejoNulos.ManageNullStr(sala.UrlBoveda));
                    query.Parameters.AddWithValue("@p31", ManejoNulos.ManageNullStr(sala.UrlSalaOnline));
                    query.Parameters.AddWithValue("@p32", ManejoNulos.ManageNullStr(sala.latitud));
                    query.Parameters.AddWithValue("@p33", ManejoNulos.ManageNullStr(sala.longitud));
                    query.Parameters.AddWithValue("@p34", ManejoNulos.ManageNullStr(sala.correo));
                    query.Parameters.AddWithValue("@p35", ManejoNulos.ManageNullInteger(sala.tipo));
                    query.Parameters.AddWithValue("@p36", ManejoNulos.ManageNullInteger(sala.PuertoSignalr));
                    //IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public List<SalaEntidad> ListadoTodosSala() {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT   CodSalaMaestra,CodSala,CodEmpresa,CodUbigeo,Nombre,NombreCorto,Direccion
      ,FechaAniversario,UrlSistemaOnline,NroMaquinasRD,FechaRegistro
      ,FechaModificacion,Activo,Estado,CodRD,CodUsuario
      ,CodRRHH,NroActasSorteos,CodRRHHTecnicos ,RutaArchivoLogo,CodOB
      ,UrlProgresivo,IpPublica,UrlCuadre,UrlPlayerTracking
      ,NombresAdministrador ,ApellidosAdministrador
      ,DniAdministrador ,FirmaAdministrador ,CodigoEstablecimiento
      ,CodRegion,UrlBoveda,UrlSalaOnline,longitud,latitud,correo,tipo,HoraApertura
  FROM Sala";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCorto = ManejoNulos.ManageNullStr(dr["NombreCorto"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                FechaAniversario = ManejoNulos.ManageNullDate(dr["FechaAniversario"]),
                                UrlSistemaOnline = ManejoNulos.ManageNullStr(dr["UrlSistemaOnline"]),
                                NroMaquinasRD = ManejoNulos.ManageNullInteger(dr["NroMaquinasRD"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                NroActasSorteos = ManejoNulos.ManageNullInteger(dr["NroActasSorteos"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                CodOB = ManejoNulos.ManageNullStr(dr["CodOB"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                IpPublica = ManejoNulos.ManageNullStr(dr["IpPublica"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlPlayerTracking = ManejoNulos.ManageNullStr(dr["UrlPlayerTracking"]),
                                NombresAdministrador = ManejoNulos.ManageNullStr(dr["NombresAdministrador"]),
                                ApellidosAdministrador = ManejoNulos.ManageNullStr(dr["ApellidosAdministrador"]),
                                DniAdministrador = ManejoNulos.ManageNullStr(dr["DniAdministrador"]),
                                FirmaAdministrador = ManejoNulos.ManageNullStr(dr["FirmaAdministrador"]),
                                CodigoEstablecimiento = ManejoNulos.ManageNullStr(dr["CodigoEstablecimiento"]),
                                CodRegion = ManejoNulos.ManageNullInteger(dr["CodRegion"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                latitud = ManejoNulos.ManageNullStr(dr["latitud"]),
                                longitud = ManejoNulos.ManageNullStr(dr["longitud"]),
                                correo = ManejoNulos.ManageNullStr(dr["correo"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                HoraApertura = dr["HoraApertura"] != DBNull.Value ? (TimeSpan)dr["HoraApertura"] : TimeSpan.Zero
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<int> ListadoIdSala(int codUsuario) {
            List<int> lista = new List<int>();
            string consulta = @"select SalaId from UsuarioSala where UsuarioId   = " + codUsuario;
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            int item = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<SalaEntidad> ListadoTodosSalaActivosOrder() {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT   CodSalaMaestra,CodSala,CodEmpresa,CodUbigeo,Nombre,NombreCorto,Direccion
      ,FechaAniversario,UrlSistemaOnline,NroMaquinasRD,FechaRegistro
      ,FechaModificacion,Activo,Estado,CodRD,CodUsuario
      ,CodRRHH,NroActasSorteos,CodRRHHTecnicos ,RutaArchivoLogo,CodOB
      ,UrlProgresivo,IpPublica,UrlCuadre,UrlPlayerTracking
      ,NombresAdministrador ,ApellidosAdministrador
      ,DniAdministrador ,FirmaAdministrador ,CodigoEstablecimiento
      ,CodRegion,UrlBoveda,UrlSalaOnline,longitud,latitud,correo,tipo
  FROM Sala where Estado=1 order by Nombre Asc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCorto = ManejoNulos.ManageNullStr(dr["NombreCorto"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                FechaAniversario = ManejoNulos.ManageNullDate(dr["FechaAniversario"]),
                                UrlSistemaOnline = ManejoNulos.ManageNullStr(dr["UrlSistemaOnline"]),
                                NroMaquinasRD = ManejoNulos.ManageNullInteger(dr["NroMaquinasRD"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                NroActasSorteos = ManejoNulos.ManageNullInteger(dr["NroActasSorteos"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                CodOB = ManejoNulos.ManageNullStr(dr["CodOB"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                IpPublica = ManejoNulos.ManageNullStr(dr["IpPublica"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlPlayerTracking = ManejoNulos.ManageNullStr(dr["UrlPlayerTracking"]),
                                NombresAdministrador = ManejoNulos.ManageNullStr(dr["NombresAdministrador"]),
                                ApellidosAdministrador = ManejoNulos.ManageNullStr(dr["ApellidosAdministrador"]),
                                DniAdministrador = ManejoNulos.ManageNullStr(dr["DniAdministrador"]),
                                FirmaAdministrador = ManejoNulos.ManageNullStr(dr["FirmaAdministrador"]),
                                CodigoEstablecimiento = ManejoNulos.ManageNullStr(dr["CodigoEstablecimiento"]),
                                CodRegion = ManejoNulos.ManageNullInteger(dr["CodRegion"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                latitud = ManejoNulos.ManageNullStr(dr["latitud"]),
                                longitud = ManejoNulos.ManageNullStr(dr["longitud"]),
                                correo = ManejoNulos.ManageNullStr(dr["correo"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }
        public bool SalaModificarEstadoJson(int salaId, int Estado) {

            string consulta = @"update Sala set Estado = @Estado
                                    where CodSala = @pCodSala";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", salaId);
                    query.Parameters.AddWithValue("@Estado", Estado);
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public int GetTotalSalasActivas() {
            int result = 0;
            string consulta = @"select count(*) as total from sala where Estado=1 and (tipo=0 or tipo is null)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            result = ManejoNulos.ManageNullInteger(dr["total"]);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return result;
        }
        public List<SalaEntidad> GetListadoSalasYTotalClientes() {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            //string consulta = @"select count(cli.Id) as cantidad, cli.SalaId,sal.Nombre from AST_Cliente as cli
            //                    left join sala as sal
            //                    on cli.SalaId=sal.CodSala
            //                    where sal.Estado=1
            //                    group by cli.SalaId,sal.Nombre
            //                    order by sal.Nombre asc";  
            string consulta = @"declare @totalSala int = 0
declare @i int =1
declare @tempSala table(
	rowNumber int identity (1,1) not null,
	codSalaMaestra int not null,
	codSala int not null,
	nombreSala varchar(100) not null
)
declare @tempResultadoFinal table(
	cantidad int  null,
	nombre varchar(100)  null,
	salaid int null,
    codSalaMaestra int not null
)
insert into @tempSala (codSalaMaestra,codSala,nombreSala)
select ISNULL(CodSalaMaestra, 0),codsala,nombre from sala where estado=1 and tipo <>2

set @totalSala = (select count(rowNumber) from @tempSala)
WHILE (@i <=@totalSala )
begin
	declare @nombreSala varchar(100)=''
	declare @codSalaMaestra int = 0
	declare @codSala int = 0
	declare @cantidadClientes int = 0
	set @nombreSala = (select nombreSala from @tempSala where rowNumber=@i)
	set @codSalaMaestra = (select codSalaMaestra from @tempSala where rowNumber=@i)
	set @codSala = (select codSala from @tempSala where rowNumber=@i)
	set @cantidadClientes =( select count(id) from ast_cliente where salaid in (select codsala from sala where nombre like '%'+@nombreSala+'%'))
	insert into @tempResultadoFinal(cantidad,nombre,salaid,codSalaMaestra) values (@cantidadClientes,@nombreSala,@codSala,@codSalaMaestra)
	set @i=@i+1
end
select salaid,nombre,cantidad, codSalaMaestra from @tempResultadoFinal";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CantidadClientes = ManejoNulos.ManageNullInteger(dr["cantidad"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<SalaEntidad>();
            }
            return lista;
        }

        public SalaEntidad ObtenerSalaEmpresa(int salaId) {
            SalaEntidad sala = new SalaEntidad();

            string query = @"
            SELECT
            sala.CodSalaMaestra,
            sala.CodSala,
            sala.CodEmpresa,
            sala.CodUbigeo,
            sala.Nombre,
            sala.NombreCorto,
            sala.Direccion,
            sala.FechaAniversario,
            sala.UrlSistemaOnline,
            sala.NroMaquinasRD,
            sala.FechaRegistro,
            sala.FechaModificacion,
            sala.Activo,
            sala.Estado,
            sala.CodRD,
            sala.CodUsuario,
            sala.CodRRHH,
            sala.NroActasSorteos,
            sala.CodRRHHTecnicos,
            sala.RutaArchivoLogo,
            sala.CodOB,
            sala.UrlProgresivo,
            sala.IpPublica,
            sala.UrlCuadre,
            sala.UrlPlayerTracking,
            sala.NombresAdministrador,
            sala.ApellidosAdministrador,
            sala.DniAdministrador,
            sala.FirmaAdministrador,
            sala.CodigoEstablecimiento,
            sala.CodRegion,
            sala.UrlBoveda,
            sala.UrlSalaOnline,
            sala.latitud,
            sala.longitud,
            sala.correo,
            sala.tipo,
            empresa.RazonSocial
            FROM Sala sala (nolock) 
            INNER JOIN Empresa empresa (nolock) ON sala.CodEmpresa = empresa.CodEmpresa
            WHERE sala.CodSala = @pCodSala
            ";

            try {
                using(SqlConnection conecction = new SqlConnection(_conexion)) {
                    conecction.Open();
                    SqlCommand command = new SqlCommand(query, conecction);
                    command.Parameters.AddWithValue("@pCodSala", salaId);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.Read()) {
                            SalaEntidad item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(data["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(data["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(data["CodEmpresa"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(data["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(data["Nombre"]),
                                NombreCorto = ManejoNulos.ManageNullStr(data["NombreCorto"]),
                                Direccion = ManejoNulos.ManageNullStr(data["Direccion"]),
                                FechaAniversario = ManejoNulos.ManageNullDate(data["FechaAniversario"]),
                                UrlSistemaOnline = ManejoNulos.ManageNullStr(data["UrlSistemaOnline"]),
                                NroMaquinasRD = ManejoNulos.ManageNullInteger(data["NroMaquinasRD"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(data["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(data["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(data["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(data["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(data["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(data["CodRRHH"]),
                                NroActasSorteos = ManejoNulos.ManageNullInteger(data["NroActasSorteos"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(data["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(data["RutaArchivoLogo"]),
                                CodOB = ManejoNulos.ManageNullStr(data["CodOB"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(data["UrlProgresivo"]),
                                IpPublica = ManejoNulos.ManageNullStr(data["IpPublica"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(data["UrlCuadre"]),
                                UrlPlayerTracking = ManejoNulos.ManageNullStr(data["UrlPlayerTracking"]),
                                NombresAdministrador = ManejoNulos.ManageNullStr(data["NombresAdministrador"]),
                                ApellidosAdministrador = ManejoNulos.ManageNullStr(data["ApellidosAdministrador"]),
                                DniAdministrador = ManejoNulos.ManageNullStr(data["DniAdministrador"]),
                                FirmaAdministrador = ManejoNulos.ManageNullStr(data["FirmaAdministrador"]),
                                CodigoEstablecimiento = ManejoNulos.ManageNullStr(data["CodigoEstablecimiento"]),
                                CodRegion = ManejoNulos.ManageNullInteger(data["CodRegion"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(data["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(data["UrlSalaOnline"]),
                                latitud = ManejoNulos.ManageNullStr(data["latitud"]),
                                longitud = ManejoNulos.ManageNullStr(data["longitud"]),
                                correo = ManejoNulos.ManageNullStr(data["correo"]),
                                tipo = ManejoNulos.ManageNullInteger(data["tipo"]),
                            };

                            EmpresaEntidad empresa = new EmpresaEntidad {
                                RazonSocial = ManejoNulos.ManageNullStr(data["RazonSocial"])
                            };

                            item.Empresa = empresa;

                            sala = item;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return sala;
        }
        public int GetTotalSalas() {
            int result = 0;
            string consulta = @"select count(*) as total from sala where tipo=0 or tipo is null";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            result = ManejoNulos.ManageNullInteger(dr["total"]);
                        }
                    }
                }
            } catch(Exception ex) {
                return 0;
            }
            return result;
        }




        public bool SalaModificarCamposProgresivoJson(int salaId, string nameQuery, string value) {

            string parameter = "@p" + nameQuery;

            string consulta = @"update Sala set " + nameQuery + " = @p" + nameQuery + " where CodSala = @pCodSala";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(salaId));
                    query.Parameters.AddWithValue(parameter, ManejoNulos.ManageNullStr(value));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public List<SalaEntidad> ListadoCamposProgresivoSalas() {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT   CodSalaMaestra,CodSala,Nombre,IpPrivada,PuertoServicioWebOnline,PuertoWebOnline,CarpetaOnline
                                FROM Sala  where estado = 1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                                PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(dr["PuertoServicioWebOnline"]),
                                PuertoWebOnline = ManejoNulos.ManageNullInteger(dr["PuertoWebOnline"]),
                                CarpetaOnline = ManejoNulos.ManageNullStr(dr["CarpetaOnline"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }




        public List<SalaEntidad.PingIpPublica> ListadoIpPublicaSalas() {
            List<SalaEntidad.PingIpPublica> lista = new List<SalaEntidad.PingIpPublica>();
            string consulta = @"SELECT CodSalaMaestra,CodSala,Nombre,UrlProgresivo
                                FROM Sala  where estado = 1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad.PingIpPublica {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                IpPublica = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<SalaEntidad.PingIpPrivada> ListadoIpPrivadaSalas() {
            List<SalaEntidad.PingIpPrivada> lista = new List<SalaEntidad.PingIpPrivada>();
            string consulta = @"SELECT CodSalaMaestra,CodSala,Nombre,IpPrivada
                                FROM Sala  where estado = 1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad.PingIpPrivada {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<SalaEntidad> ListadoIpsSalas() {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"SELECT UrlProgresivo,IpPrivada
                                FROM Sala  where estado = 1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public SalaEntidad ObtenerSalaPorCodigo(int roomCode) {
            SalaEntidad sala = new SalaEntidad();

            string query = @"
            SELECT
                CodSalaMaestra,
	            CodSala,
	            Nombre,
	            UrlProgresivo,
                IpPrivada,
	            PuertoServicioWebOnline,
	            PuertoWebOnline,
	            CarpetaOnline
            FROM Sala WHERE estado = 1 AND CodSala = @p0
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p0", roomCode);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.Read()) {
                            sala.CodSalaMaestra = ManejoNulos.ManageNullInteger(data["CodSalaMaestra"]);
                            sala.CodSala = ManejoNulos.ManageNullInteger(data["CodSala"]);
                            sala.Nombre = ManejoNulos.ManageNullStr(data["Nombre"]);
                            sala.UrlProgresivo = ManejoNulos.ManageNullStr(data["UrlProgresivo"]);
                            sala.IpPrivada = ManejoNulos.ManageNullStr(data["IpPrivada"]);
                            sala.PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(data["PuertoServicioWebOnline"]);
                            sala.PuertoWebOnline = ManejoNulos.ManageNullInteger(data["PuertoWebOnline"]);
                            sala.CarpetaOnline = ManejoNulos.ManageNullStr(data["CarpetaOnline"]);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return sala;
        }

        public SalaVpnEntidad ObtenerSalaVpnPorCodigo(int roomCode) {
            SalaVpnEntidad sala = new SalaVpnEntidad();

            string query = @"
            SELECT
                sala.CodSalaMaestra,
	            sala.CodSala,
	            sala.CodEmpresa,
	            sala.Nombre,
	            empresa.RazonSocial,
	            sala.UrlProgresivo,
                sala.IpPrivada,
	            sala.PuertoServicioWebOnline,
	            sala.PuertoWebOnline,
	            sala.CarpetaOnline
            FROM Sala sala (NOLOCK)
            LEFT JOIN Empresa empresa (NOLOCK) ON sala.CodEmpresa = empresa.CodEmpresa
            WHERE sala.estado = 1 AND sala.CodSala = @p0
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p0", roomCode);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.Read()) {
                            sala.CodSalaMaestra = ManejoNulos.ManageNullInteger(data["CodSalaMaestra"]);
                            sala.CodSala = ManejoNulos.ManageNullInteger(data["CodSala"]);
                            sala.CodEmpresa = ManejoNulos.ManageNullInteger(data["CodEmpresa"]);
                            sala.Nombre = ManejoNulos.ManageNullStr(data["Nombre"]);
                            sala.NombreEmpresa = ManejoNulos.ManageNullStr(data["RazonSocial"]);
                            sala.UrlProgresivo = ManejoNulos.ManageNullStr(data["UrlProgresivo"]);
                            sala.IpPrivada = ManejoNulos.ManageNullStr(data["IpPrivada"]);
                            sala.PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(data["PuertoServicioWebOnline"]);
                            sala.PuertoWebOnline = ManejoNulos.ManageNullInteger(data["PuertoWebOnline"]);
                            sala.CarpetaOnline = ManejoNulos.ManageNullStr(data["CarpetaOnline"]);
                        }
                    }
                }
            } catch(Exception) {
                sala = new SalaVpnEntidad();
            }

            return sala;
        }

        public List<CorreoSala> ObtenerCorreosSala() {
            List<CorreoSala> lista = new List<CorreoSala>();
            string consulta = @"SELECT IdCorreoSala,Correo,SalaId,Nombre,Contrasenia
                                FROM Correo_Sala";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CorreoSala {
                                IdCorreoSala = ManejoNulos.ManageNullInteger(dr["IdCorreoSala"]),
                                Correo = ManejoNulos.ManageNullStr(dr["Correo"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Contrasenia = ManejoNulos.ManageNullStr(dr["Contrasenia"]),
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public CorreoSala ObtenerDetalleCorreosSala(int salaId) {
            CorreoSala sala = new CorreoSala();

            string query = @"
            SELECT
	            IdCorreoSala,
	            Correo,
	            SalaId,
	            Nombre,
	            Contrasenia
            FROM Correo_Sala (NOLOCK)
            WHERE IdCorreoSala = @p0
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p0", salaId);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.Read()) {
                            sala.IdCorreoSala = ManejoNulos.ManageNullInteger(data["IdCorreoSala"]);
                            sala.Correo = ManejoNulos.ManageNullStr(data["Correo"]);
                            sala.Contrasenia = ManejoNulos.ManageNullStr(data["Contrasenia"]);
                            sala.Nombre = ManejoNulos.ManageNullStr(data["Nombre"]);
                            sala.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);

                        }
                    }
                }
            } catch(Exception) {
                sala = new CorreoSala();
            }

            return sala;
        }
        public bool ActualizarCorreoSala(CorreoSala dataCorreo) {

            string consulta = @"
           UPDATE Correo_Sala set Correo = @pCorreo , SalaId = @sala , Nombre = @nombre , Contrasenia = @contrasenia
           WHERE IdCorreoSala = @IdCorreoSala
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCorreo", ManejoNulos.ManageNullStr(dataCorreo.Correo));
                    query.Parameters.AddWithValue("@nombre", ManejoNulos.ManageNullStr(dataCorreo.Nombre));
                    query.Parameters.AddWithValue("@sala", ManejoNulos.ManageNullInteger(dataCorreo.SalaId));
                    query.Parameters.AddWithValue("@contrasenia", ManejoNulos.ManageNullStr(dataCorreo.Contrasenia));
                    query.Parameters.AddWithValue("@IdCorreoSala", ManejoNulos.ManageNullStr(dataCorreo.IdCorreoSala));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #region Sala Maestra
        public List<SalaEntidad> ObtenerTodasLasSalasDeSalaMaestraPorCodigoSalaMaestra(int codSalaMaestra) {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"
                SELECT
	                sm.CodSalaMaestra,
	                sm.nombre AS Nombre,
	                s.CodSala,
	                s.CodEmpresa,
	                s.CodUbigeo,
	                --s.Nombre,
	                s.NombreCorto,
	                s.Direccion,
	                s.FechaAniversario,
	                s.UrlSistemaOnline,
	                s.NroMaquinasRD,
	                s.FechaRegistro,
	                s.FechaModificacion,
	                s.Activo,
	                s.Estado,
	                s.CodRD,
	                s.CodUsuario,
	                s.CodRRHH,
	                s.NroActasSorteos,
	                s.CodRRHHTecnicos,
	                s.RutaArchivoLogo,
	                s.CodOB,
	                s.UrlProgresivo,
	                s.IpPublica,
	                s.UrlCuadre,
	                s.UrlPlayerTracking,
	                s.NombresAdministrador,
	                s.ApellidosAdministrador,
	                s.DniAdministrador,
	                s.FirmaAdministrador,
	                s.CodigoEstablecimiento,
	                s.CodRegion,
	                s.UrlBoveda,
	                s.UrlSalaOnline,
	                s.latitud,
	                s.longitud,
	                s.correo,
	                s.tipo,
	                s.PuertoSignalr,
	                s.IpPrivada,
	                s.PuertoServicioWebOnline
                FROM
	                Sala (nolock) as s
                INNER JOIN SalaMaestra as sm ON sm.CodSalaMaestra = s.CodSalaMaestra
                WHERE
	                s.CodSalaMaestra = @pCodigoSalaMaestra
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoSalaMaestra", codSalaMaestra);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCorto = ManejoNulos.ManageNullStr(dr["NombreCorto"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                FechaAniversario = ManejoNulos.ManageNullDate(dr["FechaAniversario"]),
                                UrlSistemaOnline = ManejoNulos.ManageNullStr(dr["UrlSistemaOnline"]),
                                NroMaquinasRD = ManejoNulos.ManageNullInteger(dr["NroMaquinasRD"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                NroActasSorteos = ManejoNulos.ManageNullInteger(dr["NroActasSorteos"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                CodOB = ManejoNulos.ManageNullStr(dr["CodOB"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                IpPublica = ManejoNulos.ManageNullStr(dr["IpPublica"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlPlayerTracking = ManejoNulos.ManageNullStr(dr["UrlPlayerTracking"]),
                                NombresAdministrador = ManejoNulos.ManageNullStr(dr["NombresAdministrador"]),
                                ApellidosAdministrador = ManejoNulos.ManageNullStr(dr["ApellidosAdministrador"]),
                                DniAdministrador = ManejoNulos.ManageNullStr(dr["DniAdministrador"]),
                                FirmaAdministrador = ManejoNulos.ManageNullStr(dr["FirmaAdministrador"]),
                                CodigoEstablecimiento = ManejoNulos.ManageNullStr(dr["CodigoEstablecimiento"]),
                                CodRegion = ManejoNulos.ManageNullInteger(dr["CodRegion"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                latitud = ManejoNulos.ManageNullStr(dr["latitud"]),
                                longitud = ManejoNulos.ManageNullStr(dr["longitud"]),
                                correo = ManejoNulos.ManageNullStr(dr["correo"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                PuertoSignalr = ManejoNulos.ManageNullInteger(dr["PuertoSignalr"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                                PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(dr["PuertoServicioWebOnline"]),
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

        public List<SalaEntidad> ListadoSalaMaestraPorUsuario(int UsuarioId) {
            List<SalaEntidad> lista = new List<SalaEntidad>();
            string consulta = @"
                SELECT 
	                s.CodSalaMaestra,
	                s.CodSala,
	                s.CodEmpresa,
	                sm.Nombre as NombreSalaMaestra,
	                s.Nombre,
	                s.FechaRegistro,
	                s.UrlProgresivo,
	                s.UrlCuadre,
	                s.UrlBoveda,
	                s.UrlSalaOnline,
	                s.tipo,
	                s.PuertoSignalr,
	                s.IpPrivada,
	                s.PuertoServicioWebOnline
                FROM 
	                SalaMaestra as sm (nolock) 
                inner join Sala as s on s.CodSalaMaestra = sm.CodSalaMaestra
                inner join UsuarioSala as us on us.SalaId= s.CodSala
                where 
	                s.estado = 1 and
	                us.UsuarioId = @p0 AND
	                CONCAT(s.CodSalaMaestra,'-',s.FechaRegistro) IN (SELECT CONCAT(codsalaMaestra,'-',MAX(FechaRegistro))FROM Sala GROUP BY CodSalaMaestra)
                order by
	                sm.nombre ASC
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(UsuarioId));
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new SalaEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["NombreSalaMaestra"]),
                                UrlProgresivo = ManejoNulos.ManageNullStr(dr["UrlProgresivo"]),
                                UrlCuadre = ManejoNulos.ManageNullStr(dr["UrlCuadre"]),
                                UrlBoveda = ManejoNulos.ManageNullStr(dr["UrlBoveda"]),
                                UrlSalaOnline = ManejoNulos.ManageNullStr(dr["UrlSalaOnline"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),
                                PuertoSignalr = ManejoNulos.ManageNullInteger(dr["PuertoSignalr"]),
                                IpPrivada = ManejoNulos.ManageNullStr(dr["IpPrivada"]),
                                PuertoServicioWebOnline = ManejoNulos.ManageNullInteger(dr["PuertoServicioWebOnline"]),
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
        #endregion



        public bool ActualizarHoraApertura(long codSala, string horaApertura) {
            bool response = false;

            string query = @"
            UPDATE dbo.Sala
            SET
                HoraApertura = @HoraApertura
            WHERE
                CodSala = @CodSala
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    // Convertir string a TimeSpan
                    TimeSpan timeHoraApertura = TimeSpan.Parse(horaApertura);

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.Add("@HoraApertura", SqlDbType.Time).Value = timeHoraApertura;
                    command.Parameters.AddWithValue("@CodSala", codSala);

                    command.ExecuteNonQuery();

                    response = true;
                }
            } catch(Exception exception) {
                Trace.WriteLine(exception.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }
    }
}
