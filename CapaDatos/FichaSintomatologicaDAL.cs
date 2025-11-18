using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3k.Utilitario;
using CapaEntidad;
using System.Diagnostics;
using CapaDatos.Utilitarios;

namespace CapaDatos
{
    public class FichaSintomatologicaDAL
    {
        string _conexion = string.Empty;
        public FichaSintomatologicaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        //Ingreso formulario
        public bool FichaSintomatologicaIngresoInsertarJson(FichaSintomatologicaEntidad fichaSintomatologica)
        {
            bool response = false;
            string consulta = @"INSERT INTO [dbo].[FichaSintomatologica]
                                       ([EmpleadoId]
                                       ,[CodSala]
                                       ,[DOI]
                                       ,[FechaIngreso]
                                       ,[TemperaturaIngreso]
                                       ,[Signo1Ingreso]
                                       ,[Signo2Ingreso]
                                       ,[Signo3Ingreso]
                                       ,[Signo4Ingreso]
                                       ,[Signo5Ingreso]
                                       ,[Signo6Ingreso]
                                       ,[Activo])
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
                                       ,@p11)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fichaSintomatologica.EmpleadoId);
                    query.Parameters.AddWithValue("@p1", fichaSintomatologica.CodSala);
                    query.Parameters.AddWithValue("@p2", fichaSintomatologica.DOI);
                    query.Parameters.AddWithValue("@p3", fichaSintomatologica.FechaIngreso);
                    query.Parameters.AddWithValue("@p4", fichaSintomatologica.TemperaturaIngreso);
                    query.Parameters.AddWithValue("@p5", fichaSintomatologica.Signo1Ingreso);
                    query.Parameters.AddWithValue("@p6", fichaSintomatologica.Signo2Ingreso);
                    query.Parameters.AddWithValue("@p7", fichaSintomatologica.Signo3Ingreso);
                    query.Parameters.AddWithValue("@p8", fichaSintomatologica.Signo4Ingreso);
                    query.Parameters.AddWithValue("@p9", fichaSintomatologica.Signo5Ingreso);
                    query.Parameters.AddWithValue("@p10", fichaSintomatologica.Signo6Ingreso);
                    query.Parameters.AddWithValue("@p11", fichaSintomatologica.Activo);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }

        public Int64 FichaSintomatologicaIngresoInsertaridJson(FichaSintomatologicaEntidad fichaSintomatologica)
        {
            Int64 IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[FichaSintomatologica]
                                       ([EmpleadoId]
                                       ,[CodSala]
                                       ,[DOI]
                                       ,[FechaIngreso]
                                       ,[TemperaturaIngreso]
                                       ,[Signo1Ingreso]
                                       ,[Signo2Ingreso]
                                       ,[Signo3Ingreso]
                                       ,[Signo4Ingreso]
                                       ,[Signo5Ingreso]
                                       ,[Signo6Ingreso]
                                       ,[Activo])
Output Inserted.FichaId
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
                                       ,@p11)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fichaSintomatologica.EmpleadoId);
                    query.Parameters.AddWithValue("@p1", fichaSintomatologica.CodSala);
                    query.Parameters.AddWithValue("@p2", fichaSintomatologica.DOI);
                    query.Parameters.AddWithValue("@p3", fichaSintomatologica.FechaIngreso);
                    query.Parameters.AddWithValue("@p4", fichaSintomatologica.TemperaturaIngreso);
                    query.Parameters.AddWithValue("@p5", fichaSintomatologica.Signo1Ingreso);
                    query.Parameters.AddWithValue("@p6", fichaSintomatologica.Signo2Ingreso);
                    query.Parameters.AddWithValue("@p7", fichaSintomatologica.Signo3Ingreso);
                    query.Parameters.AddWithValue("@p8", fichaSintomatologica.Signo4Ingreso);
                    query.Parameters.AddWithValue("@p9", fichaSintomatologica.Signo5Ingreso);
                    query.Parameters.AddWithValue("@p10", fichaSintomatologica.Signo6Ingreso);
                    query.Parameters.AddWithValue("@p11", fichaSintomatologica.Activo);
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        //Salida formulario
        public bool FichaSintomatologicaSalidaModificarJson(FichaSintomatologicaEntidad fichaSintomatologica)
        {
            string consulta = @"UPDATE [dbo].[FichaSintomatologica]
                   SET 
                      [FechaSalida] = @p0
                      ,[TemperaturaSalida] = @p1
                      ,[Signo1Salida] = @p2
                      ,[Signo2Salida] = @p3
                      ,[Signo3Salida] = @p4
                      ,[Signo4Salida] = @p5
                      ,[Signo5Salida] = @p6
                      ,[Signo6Salida] = @p7
            
                    ,[Signo1Ingreso] = @p12
                   ,[Signo2Ingreso] = @p13
                   ,[Signo3Ingreso] = @p14
                   ,[Signo4Ingreso] = @p15
                   ,[Signo5Ingreso] = @p16
                   ,[Signo6Ingreso] = @p17
    
                      ,[Activo] = @p8
                      ,[Firma] = @p9
                 WHERE ([DOI] = @p10) AND ([Activo] = @p11)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fichaSintomatologica.FechaSalida);
                    query.Parameters.AddWithValue("@p1", fichaSintomatologica.TemperaturaSalida);
                    query.Parameters.AddWithValue("@p2", fichaSintomatologica.Signo1Salida);
                    query.Parameters.AddWithValue("@p3", fichaSintomatologica.Signo2Salida);
                    query.Parameters.AddWithValue("@p4", fichaSintomatologica.Signo3Salida);
                    query.Parameters.AddWithValue("@p5", fichaSintomatologica.Signo4Salida);
                    query.Parameters.AddWithValue("@p6", fichaSintomatologica.Signo5Salida);
                    query.Parameters.AddWithValue("@p7", fichaSintomatologica.Signo6Salida);
                    query.Parameters.AddWithValue("@p8", fichaSintomatologica.Activo);
                    query.Parameters.AddWithValue("@p9", fichaSintomatologica.Firma);
                    query.Parameters.AddWithValue("@p10", fichaSintomatologica.DOI);
                    query.Parameters.AddWithValue("@p11", "True");

                    query.Parameters.AddWithValue("@p12", fichaSintomatologica.Signo1Ingreso);
                    query.Parameters.AddWithValue("@p13", fichaSintomatologica.Signo2Ingreso);
                    query.Parameters.AddWithValue("@p14", fichaSintomatologica.Signo3Ingreso);
                    query.Parameters.AddWithValue("@p15", fichaSintomatologica.Signo4Ingreso);
                    query.Parameters.AddWithValue("@p16", fichaSintomatologica.Signo5Ingreso);
                    query.Parameters.AddWithValue("@p17", fichaSintomatologica.Signo6Ingreso);

                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
                return false;
            }
        }

        public bool FichaSintomatologicaImagenModificarJson(FichaSintomatologicaEntidad fichaSintomatologica)
        {
            string consulta = @"UPDATE [dbo].[FichaSintomatologica]
                   SET [Firma] = @p1
                 WHERE FichaId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    
                    query.Parameters.AddWithValue("@p1", fichaSintomatologica.Firma);
                    query.Parameters.AddWithValue("@p0", fichaSintomatologica.FichaId);

                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
                return false;
            }
        }
        //Buscar si ya ingreso 
        public FichaSintomatologicaEntidad FichaSintomatologicaBuscarIngresoJson(int empleadoId)
        {
            FichaSintomatologicaEntidad fichaSintomatologica= new FichaSintomatologicaEntidad();

            string consulta = @"SELECT TOP(1) 
                            [FichaId]
                          ,[DOI]
                          ,[FechaIngreso]
                          ,[FechaSalida]
                          ,[TemperaturaIngreso]
                          ,[TemperaturaSalida]
                          ,[Signo1Ingreso]
                          ,[Signo2Ingreso]
                          ,[Signo3Ingreso]
                          ,[Signo4Ingreso]
                          ,[Signo5Ingreso]
                          ,[Signo6Ingreso]
                          ,[Signo1Salida]
                          ,[Signo2Salida]
                          ,[Signo3Salida]
                          ,[Signo4Salida]
                          ,[Signo5Salida]
                          ,[Signo6Salida]
                          ,[Firma]
                          ,[Activo]
                          ,[EmpleadoId]
                          ,[CodSala]
                           FROM [dbo].[FichaSintomatologica]
                           WHERE ([EmpleadoId] = @p0) AND ([Activo] = @p1)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@p0", empleadoId);
                    query.Parameters.AddWithValue("@p1", true);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                fichaSintomatologica.FichaId = ManejoNulos.ManageNullInteger64(dr["FichaId"]);
                                fichaSintomatologica.DOI = ManejoNulos.ManageNullStr(dr["DOI"]);
                                fichaSintomatologica.FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]);
                                fichaSintomatologica.FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]);
                                fichaSintomatologica.TemperaturaIngreso = ManejoNulos.ManageNullDouble(dr["TemperaturaIngreso"]);
                                fichaSintomatologica.TemperaturaSalida = ManejoNulos.ManageNullDouble(dr["TemperaturaSalida"]);
                                fichaSintomatologica.Signo1Ingreso = ManejoNulos.ManegeNullBool(dr["Signo1Ingreso"]);
                                fichaSintomatologica.Signo2Ingreso = ManejoNulos.ManegeNullBool(dr["Signo2Ingreso"]);
                                fichaSintomatologica.Signo3Ingreso = ManejoNulos.ManegeNullBool(dr["Signo3Ingreso"]);
                                fichaSintomatologica.Signo4Ingreso = ManejoNulos.ManegeNullBool(dr["Signo4Ingreso"]);
                                fichaSintomatologica.Signo5Ingreso = ManejoNulos.ManegeNullBool(dr["Signo5Ingreso"]);
                                fichaSintomatologica.Signo6Ingreso = ManejoNulos.ManegeNullBool(dr["Signo6Ingreso"]);
                                fichaSintomatologica.Signo1Salida = ManejoNulos.ManegeNullBool(dr["Signo1Salida"]);
                                fichaSintomatologica.Signo2Salida = ManejoNulos.ManegeNullBool(dr["Signo2Salida"]);
                                fichaSintomatologica.Signo3Salida = ManejoNulos.ManegeNullBool(dr["Signo3Salida"]);
                                fichaSintomatologica.Signo4Salida = ManejoNulos.ManegeNullBool(dr["Signo4Salida"]);
                                fichaSintomatologica.Signo5Salida = ManejoNulos.ManegeNullBool(dr["Signo5Salida"]);
                                fichaSintomatologica.Signo6Salida = ManejoNulos.ManegeNullBool(dr["Signo6Salida"]);
                                fichaSintomatologica.Firma = ManejoNulos.ManageNullStr(dr["Firma"]);
                                fichaSintomatologica.Activo = ManejoNulos.ManegeNullBool(dr["Activo"]);
                                fichaSintomatologica.EmpleadoId = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]);
                                fichaSintomatologica.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            finally
            {
            }

            return fichaSintomatologica;
        }

        //Reporte filtro
        public (List<FichaSintomatologicaEntidad> fichaSintomatologicasLista, ClaseError error) FichaSintomatologicaFiltroListarxSalaFechaJson(string salas, DateTime fechaini, DateTime fechafin)
        {
            List<FichaSintomatologicaEntidad> lista = new List<FichaSintomatologicaEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT  ficha.FichaId , ficha.FechaIngreso , empresa.RazonSocial ,empresa.Direccion, 
empleado.Empresa empleadoEmpresa,empleado.Ruc empleadoRuc,
sala.Nombre salaNombre, ficha.DOI , empleado.Nombres , empleado.AreaTrabajo ,
ficha.FechaSalida,empleado.ApellidosPaterno,empleado.ApellidosMaterno
FROM [FichaSintomatologica] as ficha
JOIN SEG_Empleado as empleado ON ficha.DOI = empleado.DOI
JOIN Sala as sala ON ficha.CodSala = sala.CodSala
JOIN Empresa as empresa ON sala.CodEmpresa = empresa.CodEmpresa
                                 where " + salas + " CONVERT(date, FechaIngreso) between @p0 and @p1 order by FichaId desc;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var fila = new FichaSintomatologicaEntidad
                                {
                                    FichaId = ManejoNulos.ManageNullInteger64(dr["FichaId"]),
                                    Fecha = ManejoNulos.ManageNullDate(dr["FechaIngreso"]).ToString("dd/MM/yyyy hh:mm tt"),
                                    FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]),
                                    Empresa = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                    Sala = ManejoNulos.ManageNullStr(dr["salaNombre"]),
                                    DOI = ManejoNulos.ManageNullStr(dr["DOI"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"]),
                                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"]),
                                    Area = ManejoNulos.ManageNullStr(dr["AreaTrabajo"]),
                                };

                                lista.Add(fila);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (fichaSintomatologicasLista: lista, error);
        }

        public (FichaSintomatologicaEntidad fichaSintomatologica, ClaseError error) FichaSintomatologicaIdObtenerJson(int id)
        {
            FichaSintomatologicaEntidad objeto = new FichaSintomatologicaEntidad();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT ficha.FichaId as resp1, ficha.FechaIngreso as resp2,empleado.Empresa resp3,empleado.Ruc resp4,ficha.CodSala resp5,
	                               empleado.Nombres as resp6, empleado.ApellidosPaterno as resp7, empleado.ApellidosMaterno as resp8, 
	                              empleado.DOI as resp9, empleado.Direccion as resp10, empleado.Telefono as resp11, empleado.AreaTrabajo as resp12, 
	                              ficha.TemperaturaIngreso as resp13, ficha.Signo1Ingreso as resp14, ficha.Signo2Ingreso as resp15, ficha.Signo3Ingreso as resp16, 
	                              ficha.Signo4Ingreso as resp17, ficha.Signo5Ingreso as resp18, ficha.Signo6Ingreso as resp19, ficha.TemperaturaSalida as resp20, 
	                              ficha.Signo1Salida as resp21, ficha.Signo2Salida as resp22, ficha.Signo3Salida as resp23, ficha.Signo4Salida as resp24, 
	                              ficha.Signo5Salida as resp25, ficha.Signo6Salida as resp26, ficha.Firma as resp27
                                  FROM [FichaSintomatologica] as ficha 
                                  JOIN SEG_Empleado as empleado ON ficha.DOI=empleado.DOI 
                                  where ficha.FichaId=@p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                objeto.FichaId = ManejoNulos.ManageNullInteger64(dr["resp1"]);
                                objeto.Fecha = ManejoNulos.ManageNullDate(dr["resp2"]).ToString("dd/MM/yyyy hh:mm:ss tt");
                                objeto.Empresa = ManejoNulos.ManageNullStr(dr["resp3"]);
                                objeto.RUC = ManejoNulos.ManageNullStr(dr["resp4"]);
                                objeto.CodSala =ManejoNulos.ManageNullInteger(dr["resp5"]);
                                objeto.Nombre = ManejoNulos.ManageNullStr(dr["resp6"]);
                                objeto.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["resp7"]);
                                objeto.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["resp8"]);
                                objeto.DOI = ManejoNulos.ManageNullStr(dr["resp9"]);
                                objeto.Direccion = ManejoNulos.ManageNullStr(dr["resp10"]);
                                objeto.Celular = ManejoNulos.ManageNullStr(dr["resp11"]);
                                objeto.Area = ManejoNulos.ManageNullStr(dr["resp12"]);
                                objeto.TemperaturaIngreso = ManejoNulos.ManageNullDouble(dr["resp13"]);
                                objeto.Signo1Ingreso = ManejoNulos.ManegeNullBool(dr["resp14"]);
                                objeto.Signo2Ingreso = ManejoNulos.ManegeNullBool(dr["resp15"]);
                                objeto.Signo3Ingreso = ManejoNulos.ManegeNullBool(dr["resp16"]);
                                objeto.Signo4Ingreso = ManejoNulos.ManegeNullBool(dr["resp17"]);
                                objeto.Signo5Ingreso = ManejoNulos.ManegeNullBool(dr["resp18"]);
                                objeto.Signo6Ingreso = ManejoNulos.ManegeNullBool(dr["resp19"]);
                                objeto.TemperaturaSalida = ManejoNulos.ManageNullDouble(dr["resp20"]);
                                objeto.Signo1Salida = ManejoNulos.ManegeNullBool(dr["resp21"]);
                                objeto.Signo2Salida = ManejoNulos.ManegeNullBool(dr["resp22"]);
                                objeto.Signo3Salida = ManejoNulos.ManegeNullBool(dr["resp23"]);
                                objeto.Signo4Salida = ManejoNulos.ManegeNullBool(dr["resp24"]);
                                objeto.Signo5Salida = ManejoNulos.ManegeNullBool(dr["resp25"]);
                                objeto.Signo6Salida = ManejoNulos.ManegeNullBool(dr["resp26"]);
                                objeto.Firma = ManejoNulos.ManageNullStr(dr["resp27"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (fichaSintomatologica: objeto, error);
        }

        public (List<FichaSintomatologicaEntidadReporte> fichaSintomatologicasLista, ClaseError error) FichaSintomatologicaListaIdObtenerJson(string salas, DateTime fechaini, DateTime fechafin)
        {
            List<FichaSintomatologicaEntidadReporte> lista = new List<FichaSintomatologicaEntidadReporte>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT ficha.FichaId , ficha.FechaIngreso ,ficha.CodSala,sala.Nombre salaNombre,sala.RutaArchivoLogo, empresa.RazonSocial , empresa.Ruc,empresa.Direccion DireccionEmpresa, 
empleado.Empresa empleadoEmpresa,empleado.Ruc empleadoRuc,
empleado.Nombres as resp6, empleado.ApellidosPaterno as resp7, empleado.ApellidosMaterno as resp8, 
empleado.DOI as resp9, empleado.Direccion as resp10, empleado.Telefono as resp11, empleado.AreaTrabajo as resp12, 

	ficha.TemperaturaIngreso as resp13, ficha.Signo1Ingreso as resp14, ficha.Signo2Ingreso as resp15, ficha.Signo3Ingreso as resp16, 
	ficha.Signo4Ingreso as resp17, ficha.Signo5Ingreso as resp18, ficha.Signo6Ingreso as resp19, ficha.TemperaturaSalida as resp20, 
	ficha.Signo1Salida as resp21, ficha.Signo2Salida as resp22, ficha.Signo3Salida as resp23, ficha.Signo4Salida as resp24, 
	ficha.Signo5Salida as resp25, ficha.Signo6Salida as resp26, ficha.Firma as resp27
    FROM [FichaSintomatologica] as ficha 
    JOIN SEG_Empleado as empleado ON ficha.DOI=empleado.DOI 
    JOIN Sala as sala ON ficha.CodSala = sala.CodSala
	JOIN Empresa as empresa ON sala.CodEmpresa = empresa.CodEmpresa
                                    where " + salas + " CONVERT(date, ficha.FechaIngreso) between @p0 and @p1 order by ficha.FichaId desc;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var fila = new FichaSintomatologicaEntidadReporte
                                {

                                    FichaId = ManejoNulos.ManageNullInteger64(dr["FichaId"]),
                                    Fecha = ManejoNulos.ManageNullDate(dr["FechaIngreso"]).ToString("dd/MM/yyyy hh:mm:ss tt"),
                                    Empresa = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                    RUC = ManejoNulos.ManageNullStr(dr["Ruc"]),
                                    DireccionEmpresa = ManejoNulos.ManageNullStr(dr["DireccionEmpresa"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    salaNombre = ManejoNulos.ManageNullStr(dr["salaNombre"]),
                                    RutaArchivoLogo = ManejoNulos.ManageNullStr(dr["RutaArchivoLogo"]),
                                    empleadoEmpresa = ManejoNulos.ManageNullStr(dr["empleadoEmpresa"]),
                                    empleadoRuc = ManejoNulos.ManageNullStr(dr["empleadoRuc"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["resp6"]),
                                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["resp7"]),
                                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["resp8"]),
                                    DOI = ManejoNulos.ManageNullStr(dr["resp9"]),
                                    Direccion = ManejoNulos.ManageNullStr(dr["resp10"]),
                                    Celular = ManejoNulos.ManageNullStr(dr["resp11"]),
                                    Area = ManejoNulos.ManageNullStr(dr["resp12"]),
                                    TemperaturaIngreso = ManejoNulos.ManageNullDouble(dr["resp13"]),
                                    Signo1Ingreso = ManejoNulos.ManegeNullBool(dr["resp14"]),
                                    Signo2Ingreso = ManejoNulos.ManegeNullBool(dr["resp15"]),
                                    Signo3Ingreso = ManejoNulos.ManegeNullBool(dr["resp16"]),
                                    Signo4Ingreso = ManejoNulos.ManegeNullBool(dr["resp17"]),
                                    Signo5Ingreso = ManejoNulos.ManegeNullBool(dr["resp18"]),
                                    Signo6Ingreso = ManejoNulos.ManegeNullBool(dr["resp19"]),
                                    TemperaturaSalida = ManejoNulos.ManageNullDouble(dr["resp20"]),
                                    Signo1Salida = ManejoNulos.ManegeNullBool(dr["resp21"]),
                                    Signo2Salida = ManejoNulos.ManegeNullBool(dr["resp22"]),
                                    Signo3Salida = ManejoNulos.ManegeNullBool(dr["resp23"]),
                                    Signo4Salida = ManejoNulos.ManegeNullBool(dr["resp24"]),
                                    Signo5Salida = ManejoNulos.ManegeNullBool(dr["resp25"]),
                                    Signo6Salida = ManejoNulos.ManegeNullBool(dr["resp26"]),
                                    Firma = ManejoNulos.ManageNullStr(dr["resp27"]),
                                };

                                lista.Add(fila);
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (fichaSintomatologicasLista: lista, error);
        }
    }
}
