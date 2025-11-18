using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public class tuplastring
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class tupla
    {
        public string nombre { get; set; }
        public decimal valor { get; set; }
    }


    public static class filtro
    {
        public static string Texto
        {
            get
            {
                return "text";
            }
        }
        public static string SinBuscador
        {
            get
            {
                return "null";
            }

        }
        public static string FechaRango
        {
            get
            {
                return "date-range";
            }

        }
        public static string Fecha
        {
            get
            {
                return "date";
            }

        }

        public static string Select
        {
            get
            {
                return "select";
            }

        }
    }

    public static class tipobusqueda
    {

        public static string palabraexacta
        {
            get
            {
                return "palabraexacta";
            }
        }
        public static string conporcentaje
        {
            get
            {
                return "conporcentaje";
            }
        }
    }
    public static class suma
    {

        public static string sinsuma
        {
            get
            {
                return "sinsuma";
            }
        }
        public static string sumarcolumna
        {
            get
            {
                return "sumarcolumna";
            }
        }
    }

    public class datatablesparametros
    {

        // private var filtro = new { Texto = "text", SinBuscador = "null", Fecha = "date-range" };
        // private var tipobusqueda = new { palabraexacta = "palabraexacta", conporcentaje = "conporcentaje" };
        private string defaulttipobuscador = filtro.Texto.ToString();
        private string defaultbusquedaexacta = tipobusqueda.conporcentaje.ToString();
        private string defaultvalorescombo = "";
        private string defaultestilo = "wrap";//"tdcenter";
        private string defaultsuma = "sinsuma";
        private bool defaultatributodata = false;
        private bool defaultatributovisible = true;

        /// <summary>
        /// atributodata => variable boolean para agregar atributo data en tr  <tr data-nombrecolumna>  
        /// </summary>
        public bool atributodata { get { return defaultatributodata; } set { defaultatributodata = value; } }


        public bool visible { get { return defaultatributovisible; } set { defaultatributovisible = value; } }


        public string columnanombre { get; set; }
        public string anchocolumna { get; set; }


        /// <summary>
        /// tipobuscador => variable string para tipo de buscador de columna =>  texto por defecto
        /// </summary>
        public string tipobuscador
        {
            get
            {
                return defaulttipobuscador;
            }
            set
            {
                defaulttipobuscador = value;
            }
        }

        public string busquedaexacta
        {
            get
            {
                return defaultbusquedaexacta;
            }
            set
            {
                defaultbusquedaexacta = value;
            }
        }
        public string valorescombo
        {
            get
            {
                return defaultvalorescombo;
            }
            set
            {
                defaultvalorescombo = value;
            }
        }

        /// <summary>
        /// classname => nombre de clase de td
        /// </summary>
        public string classname
        {
            get
            {
                return defaultestilo;
            }
            set
            {
                defaultestilo = value;
            }
        }

        public string suma
        {
            get
            {
                return defaultsuma;
            }
            set
            {
                defaultsuma = value;
            }
        }

    }
}