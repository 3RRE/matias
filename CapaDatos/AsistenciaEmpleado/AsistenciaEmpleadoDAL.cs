using CapaDatos.Utilitarios;
using CapaEntidad.AsistenciaEmpleado;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace CapaDatos.AsistenciaEmpleado
{
    public class AsistenciaEmpleadoDAL
    {
        string _conexion = string.Empty;
        public AsistenciaEmpleadoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public Int64 EmpleadoAsistenciaInsertarJson(EmpleadoAsistencia empleadoAsistencia)
        {
            //bool respuesta = false;
            Int64 idempleadoAsistenciaInsertado = 0;
            string consulta = @"
                INSERT INTO EmpleadoAsistencia(emd_id , emp_id , ema_fecha ,ema_latitud , ema_longitud ,ema_estado,loc_id,ema_imei,ema_asignado,ema_tipo)
	            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9) 
                SELECT SCOPE_IDENTITY()";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    if (empleadoAsistencia.emd_id == 0)
                    {
                        query.Parameters.AddWithValue("@p0", DBNull.Value);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(empleadoAsistencia.emd_id));
                    }
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(empleadoAsistencia.emp_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(empleadoAsistencia.ema_fecha));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(empleadoAsistencia.ema_latitud));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(empleadoAsistencia.ema_longitud));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(empleadoAsistencia.ema_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(empleadoAsistencia.loc_id));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(empleadoAsistencia.ema_imei));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(empleadoAsistencia.ema_asignado));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(empleadoAsistencia.ema_tipo));
                    idempleadoAsistenciaInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                idempleadoAsistenciaInsertado = 0;
            }
            return idempleadoAsistenciaInsertado;
        }

        public (List<EmpleadoDatosAsistencia> empleadoAsistencia, ClaseError error) EmpleadoAsistenciaxFechabetweenListarJson(DateTime fechaini, DateTime fechafin,string salas)
        {
            List<EmpleadoDatosAsistencia> lista = new List<EmpleadoDatosAsistencia>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT 
                                a.ema_id , 
                                a.emd_id , 
                                 a.ema_imei , 
                                e.EmpleadoID emp_id, 
                                 a.loc_id,
								l.Nombre loc_nombre,
                                e.Nombres emp_nombre,
                                e.ApellidosPaterno emp_ape_paterno,
                                e.ApellidosMaterno emp_ape_materno,
                                e.EstadoEmpleado emp_estado ,
                                a.ema_fecha ,
                                a.ema_latitud , 
                                a.ema_longitud ,
                                a.ema_estado ,
                                 a.ema_asignado
	                            FROM EmpleadoAsistencia a
                               join SEG_Empleado e on e.EmpleadoID=a.emp_id
                                left join Sala l on l.CodSala=a.loc_id
	                              where " + salas + " CONVERT(date, a.ema_fecha) between @p0 and @p1 order by a.ema_id desc;";
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
                                var empleadoAsistencia = new EmpleadoDatosAsistencia
                                {

                                    ema_id = ManejoNulos.ManageNullInteger(dr["ema_id"]),
                                    emd_id = ManejoNulos.ManageNullInteger(dr["emd_id"]),
                                    loc_id = ManejoNulos.ManageNullInteger(dr["loc_id"]),
                                    local = ManejoNulos.ManageNullStr(dr["loc_nombre"]),
                                    emp_nombre = ManejoNulos.ManageNullStr(dr["emp_nombre"]),
                                    emp_ape_paterno = ManejoNulos.ManageNullStr(dr["emp_ape_paterno"]),
                                    emp_ape_materno = ManejoNulos.ManageNullStr(dr["emp_ape_materno"]),
                                    emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]),
                                    ema_imei = ManejoNulos.ManageNullStr(dr["ema_imei"]),
                                    ema_asignado = ManejoNulos.ManageNullInteger(dr["ema_asignado"]),
                                    emp_estado = ManejoNulos.ManageNullInteger(dr["emp_estado"]),
                                    ema_fecha = ManejoNulos.ManageNullDate(dr["ema_fecha"]),
                                    ema_latitud = ManejoNulos.ManageNullStr(dr["ema_latitud"]),
                                    ema_longitud = ManejoNulos.ManageNullStr(dr["ema_longitud"]),
                                    ema_estado = ManejoNulos.ManageNullInteger(dr["ema_estado"]),

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
            return (empleadoAsistencia: lista, error);
        }
    }
}
