using CapaEntidad.AsistenciaEmpleado;
using S3k.Utilitario;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.AsistenciaEmpleado
{
    public class EmpleadoVisitaDAL
    {
        string _conexion = string.Empty;
        public EmpleadoVisitaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarVisita(EmpleadoVisitaEntidad visita)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[Visita]
           ([empleado_id]
           ,[titulo]
           ,[descripcion]
           ,[imei]
           ,[fechaRegistro]
           ,[sala_id])
Output Inserted.vis_id
     VALUES
           (@empleado_id
           ,@titulo
           ,@descripcion
           ,@imei
           ,@fechaRegistro
           ,@sala_id);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@empleado_id", ManejoNulos.ManageNullInteger(visita.empleado_id));
                    query.Parameters.AddWithValue("@imei", ManejoNulos.ManageNullStr(visita.imei));
                    query.Parameters.AddWithValue("@titulo", ManejoNulos.ManageNullStr(visita.titulo));
                    query.Parameters.AddWithValue("@descripcion", ManejoNulos.ManageNullStr(visita.descripcion));
                    query.Parameters.AddWithValue("@fechaRegistro", ManejoNulos.ManageNullDate(visita.fechaRegistro));
                    query.Parameters.AddWithValue("@sala_id", ManejoNulos.ManageNullInteger(visita.sala_id));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public int GuardarVisitaDetalle(EmpleadoVisitaDetalleEntidad visita)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[VisitaDetalle]
           ([visita_id]
           ,[nombre]
           ,[descripcion]
           ,[imagen]
           ,[fechaRegistro])
Output Inserted.visd_id
     VALUES
           (@visita_id
           ,@nombre
           ,@descripcion
           ,@imagen
           ,@fechaRegistro);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@visita_id", ManejoNulos.ManageNullInteger64(visita.visita_id));
                    query.Parameters.AddWithValue("@nombre", ManejoNulos.ManageNullStr(visita.nombre));
                    query.Parameters.AddWithValue("@descripcion", ManejoNulos.ManageNullStr(visita.descripcion));
                    query.Parameters.AddWithValue("@imagen", ManejoNulos.ManageNullStr(visita.imagen));
                    query.Parameters.AddWithValue("@fechaRegistro", ManejoNulos.ManageNullDate(visita.fechaRegistro));

                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public List<EmpleadoVisitaEmpleadoEntidad> VisitaListaxFechabetweenListarJson(DateTime fechaini, DateTime fechafin, string salas)
        {
            List<EmpleadoVisitaEmpleadoEntidad> lista = new List<EmpleadoVisitaEmpleadoEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT 
                                a.vis_id , 
                                a.empleado_id , 
                                 a.imei , 
                                a.titulo , 
                                a.descripcion , 
                                 a.sala_id,
                                e.Nombres emp_nombre,
                                e.ApellidosPaterno emp_ape_paterno,
                                e.ApellidosMaterno emp_ape_materno,
                                a.fechaRegistro 
	                            FROM Visita a
                               join SEG_Empleado e on e.EmpleadoID=a.empleado_id
                                left join Sala l on l.CodSala=a.sala_id
	                              where " + salas + " CONVERT(date, a.fechaRegistro) between @p0 and @p1 order by a.vis_id desc;";
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
                                var empleadoAsistencia = new EmpleadoVisitaEmpleadoEntidad
                                {

                                    vis_id = ManejoNulos.ManageNullInteger64(dr["vis_id"]),
                                    empleado_id = ManejoNulos.ManageNullInteger(dr["empleado_id"]),
                                    titulo = ManejoNulos.ManageNullStr(dr["titulo"]),
                                    descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                    emp_nombre = ManejoNulos.ManageNullStr(dr["emp_nombre"]),
                                    emp_ape_paterno = ManejoNulos.ManageNullStr(dr["emp_ape_paterno"]),
                                    emp_ape_materno = ManejoNulos.ManageNullStr(dr["emp_ape_materno"]),
                                    imei = ManejoNulos.ManageNullStr(dr["imei"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),

                                };

                                lista.Add(empleadoAsistencia);
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
            return lista;
        }

        public List<EmpleadoVisitaDetalleEntidad> VisitaListaDetalleJson(Int64 visita_id)
        {
            List<EmpleadoVisitaDetalleEntidad> lista = new List<EmpleadoVisitaDetalleEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT [visd_id]
      ,[visita_id]
      ,[nombre]
      ,[descripcion]
      ,[imagen]
      ,[fechaRegistro]
  FROM [dbo].[VisitaDetalle]
 where visita_id =  @p0  order by visd_id desc;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", visita_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var empleadoAsistencia = new EmpleadoVisitaDetalleEntidad
                                {

                                    visd_id = ManejoNulos.ManageNullInteger64(dr["visd_id"]),
                                    visita_id = ManejoNulos.ManageNullInteger64(dr["visita_id"]),
                                    nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                    descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                    imagen = ManejoNulos.ManageNullStr(dr["imagen"]),
                                    fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),

                                };

                                lista.Add(empleadoAsistencia);
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
            return lista;
        }

        public EmpleadoVisitaDetalleEntidad visitadetalleId(Int64 visdid)
        {
            EmpleadoVisitaDetalleEntidad empresa = new EmpleadoVisitaDetalleEntidad();
            string consulta = @"SELECT [visd_id]
      ,[visita_id]
      ,[nombre]
      ,[descripcion]
      ,[imagen]
      ,[fechaRegistro]
  FROM [dbo].[VisitaDetalle] nolock
  where visd_id = @visd_id ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@visd_id", visdid);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new EmpleadoVisitaDetalleEntidad
                            {

                                visd_id = ManejoNulos.ManageNullInteger64(dr["visd_id"]),
                                visita_id = ManejoNulos.ManageNullInteger64(dr["visita_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                imagen = ManejoNulos.ManageNullStr(dr["imagen"]),
                                fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),

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
