using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaEntidad.AsistenciaEmpleado
{
    public class EmpleadoAsistencia
    {
        public int ema_id { get; set; }
        public int emd_id { get; set; }
        public string ema_imei { get; set; }
        public int emp_id { get; set; }
        public int loc_id { get; set; }
        public DateTime ema_fecha { get; set; }
        public int ema_tipo { get; set; }
        public int ema_asignado { get; set; }
        public string ema_latitud { get; set; }
        public string ema_longitud { get; set; }
        public int ema_estado { get; set; }
    }
    public class EmpleadoDatosAsistencia
    {
        public string emp_nombre { get; set; }
        public string emp_ape_paterno { get; set; }
        public string emp_ape_materno { get; set; }
        public string cargo { get; set; }
        public int loc_id { get; set; }
        public string local { get; set; }
        public int emp_estado { get; set; }
        public int ema_id { get; set; }
        public int emd_id { get; set; }
        public string ema_imei { get; set; }
        public int ema_asignado { get; set; }
        public int emp_id { get; set; }
        public DateTime ema_fecha { get; set; }
        public string ema_latitud { get; set; }
        public string ema_longitud { get; set; }
        public int ema_estado { get; set; }


    }

    public class CodigoUsuario
    {
        public Int64 cdu_id { get; set; }
        public Int64 empleado_id { get; set; }
        public string cdu_latitud { get; set; }
        public string cdu_longitud { get; set; }
        public string cdu_firebasetoken { get; set; }
        public string cdu_codigo { get; set; }
        public string cdu_imei { get; set; }
        public DateTime cdu_created { get; set; }
        public DateTime cdu_updated { get; set; }
        public DateTime fecha { get; set; }
        public bool cdu_estado { get; set; }
    }


    public class CodigoUsuarioGet
    {
        public Int64 cdu_id { get; set; }
        public string cdu_latitud { get; set; }
        public string cdu_longitud { get; set; }
        public string cdu_firebasetoken { get; set; }
        public string cdu_imei { get; set; }     
        public DateTime fecha { get; set; }
    }

    public class BitacoraMarcacion
    {
        public Int64 sala_id { get; set; }
        public string bit_nrodocumento { get; set; }
        public string bit_fechaHora { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public Int64 cdu_id { get; set; }
        public bool bit_mobile { get; set; }
        public int bit_procedencia { get; set; }
    }

    public class BitacoraMarcacionResponse {
        public bool respuesta { get; set; }
        public string message { get; set; }
    }

    public class ServiceResponse
    {
        public bool response { get; set; }
        public string message { get; set; }
    }
}