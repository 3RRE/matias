using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class EmpresaDAL
    {
        string _conexion = string.Empty;
        public EmpresaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<EmpresaEntidad> ListadoEmpresa()
        {
            List<EmpresaEntidad> lista = new List<EmpresaEntidad>();
            string consulta = @"SELECT CodEmpresa,CodConsorcio,CodUbigeo,RazonSocial,Ruc,Direccion,Telefono,ColorHexa,Sigla,FechaRegistro
      ,FechaModificacion,Activo,Estado,RutaArchivoFirma,CodRD,TipoEmpresa,CodUsuario,CodRRHH,CodRRHHTecnicos
      ,RutaArchivoLogo,SportBar,RutaArchivoMembrete,NombreRepresentanteLegal,DniRepresentanteLegal
  FROM Empresa (nolock) where estado = 1 ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new EmpresaEntidad
                            {
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodConsorcio = ManejoNulos.ManageNullInteger(dr["CodConsorcio"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                Ruc = ManejoNulos.ManageNullStr(dr["Ruc"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                RutaArchivoFirma = ManejoNulos.ManageNullStr(dr["RutaArchivoFirma"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                TipoEmpresa = ManejoNulos.ManageNullInteger(dr["TipoEmpresa"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                SportBar = ManejoNulos.ManageNullInteger(dr["SportBar"]),
                                RutaArchivoMembrete = ManejoNulos.ManageNullStr(dr["RutaArchivoMembrete"]),
                                NombreRepresentanteLegal = ManejoNulos.ManageNullStr(dr["NombreRepresentanteLegal"]),
                                DniRepresentanteLegal = ManejoNulos.ManageNullStr(dr["DniRepresentanteLegal"]), 
                            };
                            lista.Add(item);
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
            return lista;
        }
        public bool InsertarEmpresaJson(EmpresaEntidad empresa)
        {
            //bool respuesta = false;
            bool respuesta = false;
            string consulta = @"
                            INSERT INTO [dbo].[Empresa]
                               ([CodEmpresa]
                               ,[CodConsorcio]
                               ,[CodUbigeo]
                               ,[RazonSocial]
                               ,[Ruc]
                               ,[Direccion]
                               ,[Telefono]
                               ,[ColorHexa]
                               ,[Sigla]
                               ,[FechaRegistro]
                               ,[FechaModificacion]
                               ,[Activo]
                               ,[Estado]
                               ,[RutaArchivoFirma]
                               ,[CodRD]
                               ,[TipoEmpresa]
                               ,[CodUsuario]
                               ,[CodRRHH]
                               ,[CodRRHHTecnicos]
                               ,[RutaArchivoLogo]
                               ,[SportBar]
                               ,[RutaArchivoMembrete]
                               ,[NombreRepresentanteLegal]
                               ,[DniRepresentanteLegal])
                         VALUES
                               (@p0
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
                               ,@p23)
                            ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(empresa.CodEmpresa));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(empresa.CodConsorcio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empresa.CodUbigeo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(empresa.RazonSocial));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(empresa.Ruc));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(empresa.Direccion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(empresa.Telefono));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(empresa.ColorHexa));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(empresa.Sigla));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullDate(empresa.FechaRegistro));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullDate(empresa.FechaModificacion));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(empresa.Activo));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullInteger(empresa.Estado));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(empresa.RutaArchivoFirma));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullInteger(empresa.CodRD));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(empresa.TipoEmpresa));
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(empresa.CodUsuario));
                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullInteger(empresa.CodRRHH));
                    query.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullInteger(empresa.CodRRHHTecnicos));
                    query.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullStr(empresa.RutaArchivoLogo));
                    query.Parameters.AddWithValue("@p20", ManejoNulos.ManageNullInteger(empresa.SportBar));
                    query.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullStr(empresa.RutaArchivoMembrete));
                    query.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullStr(empresa.NombreRepresentanteLegal));
                    query.Parameters.AddWithValue("@p23", ManejoNulos.ManageNullStr(empresa.DniRepresentanteLegal));
                    //IdInsertado = Convert.ToInt32(query.ExecuteScalar());
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
        public EmpresaEntidad EmpresaObtenerporIdJson(int CodEmpresa)
        {
            EmpresaEntidad empresa = new EmpresaEntidad();
            string consulta = @"SELECT [CodEmpresa]
                                      ,[CodConsorcio]
                                      ,[CodUbigeo]
                                      ,[RazonSocial]
                                      ,[Ruc]
                                      ,[Direccion]
                                      ,[Telefono]
                                      ,[ColorHexa]
                                      ,[Sigla]
                                      ,[FechaRegistro]
                                      ,[FechaModificacion]
                                      ,[Activo]
                                      ,[Estado]
                                      ,[RutaArchivoFirma]
                                      ,[CodRD]
                                      ,[TipoEmpresa]
                                      ,[CodUsuario]
                                      ,[CodRRHH]
                                      ,[CodRRHHTecnicos]
                                      ,[RutaArchivoLogo]
                                      ,[SportBar]
                                      ,[RutaArchivoMembrete]
                                      ,[NombreRepresentanteLegal]
                                      ,[DniRepresentanteLegal]
                                  FROM [dbo].[Empresa] (nolock) where CodEmpresa = @CodEmpresa ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodEmpresa", CodEmpresa);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new EmpresaEntidad
                            {
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodConsorcio = ManejoNulos.ManageNullInteger(dr["CodConsorcio"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                Ruc = ManejoNulos.ManageNullStr(dr["Ruc"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                RutaArchivoFirma = ManejoNulos.ManageNullStr(dr["RutaArchivoFirma"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                TipoEmpresa = ManejoNulos.ManageNullInteger(dr["TipoEmpresa"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                SportBar = ManejoNulos.ManageNullInteger(dr["SportBar"]),
                                RutaArchivoMembrete = ManejoNulos.ManageNullStr(dr["RutaArchivoMembrete"]),
                                NombreRepresentanteLegal = ManejoNulos.ManageNullStr(dr["NombreRepresentanteLegal"]),
                                DniRepresentanteLegal = ManejoNulos.ManageNullStr(dr["DniRepresentanteLegal"]),
                            };
                            empresa = item;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
            return empresa;
        }
        public bool EmpresaModificarJson(EmpresaEntidad empresa)
        {

            string consulta = @"UPDATE [dbo].[Empresa]
                   SET 
                      [CodConsorcio] = @p0
                      ,[CodUbigeo] = @p1
                      ,[RazonSocial] = @p2
                      ,[Ruc] = @p3
                      ,[Direccion] = @p4
                      ,[FechaModificacion] = @p5
                      ,[Activo] = @p6
                      ,[Estado] = @p7
                      ,[TipoEmpresa] = @p8
                      ,[RutaArchivoLogo] = @p9
                 WHERE [CodEmpresa] = @p10";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(empresa.CodConsorcio));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(empresa.CodUbigeo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(empresa.RazonSocial));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(empresa.Ruc));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(empresa.Direccion));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(empresa.FechaModificacion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(empresa.Activo));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(empresa.Estado));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(empresa.TipoEmpresa));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(empresa.RutaArchivoLogo));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(empresa.CodEmpresa));
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
        }

        public EmpresaEntidad EmpresaListaIdJson(int empresaId)
        {
            EmpresaEntidad empresa = new EmpresaEntidad();
            string consulta = @"SELECT [CodEmpresa]
      ,[CodConsorcio]
      ,[CodUbigeo]
      ,[RazonSocial]
      ,[Ruc]
      ,[Direccion]
      ,[Telefono]
      ,[ColorHexa]
      ,[Sigla]
      ,[FechaRegistro]
      ,[FechaModificacion]
      ,[Activo]
      ,[Estado]
      ,[RutaArchivoFirma]
      ,[CodRD]
      ,[TipoEmpresa]
      ,[CodUsuario]
      ,[CodRRHH]
      ,[CodRRHHTecnicos]
      ,[RutaArchivoLogo]
      ,[SportBar]
      ,[RutaArchivoMembrete]
      ,[NombreRepresentanteLegal]
      ,[DniRepresentanteLegal]
  FROM [dbo].[Empresa] (nolock) where CodEmpresa = @pCodEmpresa ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodEmpresa", empresaId);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new EmpresaEntidad
                            {
                                
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodConsorcio = ManejoNulos.ManageNullInteger(dr["CodConsorcio"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                Ruc = ManejoNulos.ManageNullStr(dr["Ruc"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodRRHH = ManejoNulos.ManageNullInteger(dr["CodRRHH"]),                                
                                CodRRHHTecnicos = ManejoNulos.ManageNullInteger(dr["CodRRHHTecnicos"]),
                                RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                
                            };
                            empresa = item;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
            return empresa;
        }
    }
}
