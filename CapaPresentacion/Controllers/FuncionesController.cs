using CapaNegocio.Utilitarios.reporte_botones;
using CapaPresentacion.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [seguridad(false)]
    public class FuncionesController : Controller
    {
        /// <summary>
        /// botonacciones =>   funcion para generar excel , pdf e imprimir
        /// </summary>
        [seguridad(false)]
        public void botonaccionesant()
        {
            //  var Request = System.Web.HttpContext.Current.Request;

            string accion = Request.Form["accion"];
            string datos = Request.Form["datos"];
            string nombrearchivo = Request.Form["nombrearchivo"];
            DateTime fecha = Convert.ToDateTime(Request.Form["fecha"]);
            DateTime fechahasta = Convert.ToDateTime(Request.Form["fechahasta"]);
            String porcentajes = Request.Form["porcentajes"];
            var errormensaje = "";
            string definicioncolumnas = Request.Form["definicioncolumnas"];

            string cabecerasnuevas = Request.Form["cabecerasnuevas"];

            var jsonObject = new JObject();
            dynamic reporte = jsonObject;
            reporte.Tablas = new JArray() as dynamic;
            reporte.nombrearchivo = nombrearchivo;

            reporte.porcentajes = porcentajes;
            reporte.visibles = Request.Form["visibles"];
            reporte.invisibles = Request.Form["invisibles"];

            reporte.tituloreporte = Request.Form["tituloreporte"];


            reporte.Cabecera = new JArray() as dynamic;
            dynamic cabezastit = new JObject();
            var sala = System.Web.HttpContext.Current.Session["Sala"] != null ? System.Web.HttpContext.Current.Session["Sala"].ToString() : "";
            var Empresa = System.Web.HttpContext.Current.Session["Empresa"] != null ? System.Web.HttpContext.Current.Session["Empresa"].ToString() : "";
            var cabe = new[] {
                                            new {nombre="Reporte",  valor=nombrearchivo.Replace("_"," ")},
                                            new {nombre="Fecha",  valor=DateTime.Now.ToString("dd/MM/yyyy HH:mm tt")},
                                            new {nombre="Empresa",  valor=Empresa},
                                            new {nombre="Sala",     valor=sala }

                                               };

            if (cabecerasnuevas != null && cabecerasnuevas != "")
            {
                var nuevo = cabe.ToList();
                var jsoncabecerasnuevas = JToken.Parse(cabecerasnuevas);
                foreach (var cabec in jsoncabecerasnuevas)
                {

                    nuevo.Add(new { nombre = (string)cabec["nombre"], valor = (string)cabec["valor"] });


                }
                cabe = nuevo.ToArray();
            }


            cabezastit = JToken.FromObject(cabe);
            reporte.Cabecera = (cabezastit);


            if (definicioncolumnas != null && definicioncolumnas != "")
            {
                dynamic tabladefinicioncolumnas = new JObject();
                tabladefinicioncolumnas = JToken.FromObject(definicioncolumnas);
                reporte.tabladefinicioncolumnas = (tabladefinicioncolumnas);
            }




            dynamic tabla = new JObject();
            tabla = JToken.FromObject(JToken.Parse(datos));
            reporte.Tablas.Add((tabla));

            reporte.columnasporc = new JArray() as dynamic;
            // JObject json = JObject.Parse(datos);
            dynamic porc = new JObject();
            List<double> por = new List<double>();
            double[] porcentajescolumnas = { 8, 22, 20, 16, 6, 9, 15 };///debe sumar 100
            foreach (var p in porcentajescolumnas)
            {
                por.Add(p);
            }
            porc = JToken.FromObject(por);
            reporte.columnasporc.Add(porc);

            if (accion == "pdf")
            {
                //funciones.generarpdf(reporte);
            }
            else if (accion == "imprimir")
            {
                // funciones.imprimir(reporte);
            }
            else if (accion == "excel")
            {
                funciones.generarexcel(reporte);
            }


        }

        [seguridad(false)]
        public void botonacciones()
        {
            //  var Request = System.Web.HttpContext.Current.Request;

            string accion = Request.Form["accion"];
            string datos = Request.Form["datos"];
            string nombrearchivo = Request.Form["nombrearchivo"];
            DateTime fecha = Convert.ToDateTime(Request.Form["fecha"]);
            DateTime fechahasta = Convert.ToDateTime(Request.Form["fechahasta"]);
            String porcentajes = Request.Form["porcentajes"];
            var errormensaje = "";
            string definicioncolumnas = Request.Form["definicioncolumnas"];
            string cabeceras = Request.Form["cabeceras"]; ////cabeceras por DEFECTO
            string cabecerasnuevas = Request.Form["cabecerasnuevas"];


            string aoHeader = Request.Form["aoHeader"];


            var jsonObject = new JObject();
            dynamic reporte = jsonObject;
            reporte.Tablas = new JArray() as dynamic;
            reporte.nombrearchivo = nombrearchivo;

            reporte.porcentajes = porcentajes;
            reporte.visibles = Request.Form["visibles"];
            reporte.invisibles = Request.Form["invisibles"];





            reporte.tituloreporte = Request.Form["tituloreporte"];


            reporte.Cabecera = new JArray() as dynamic;
            dynamic cabezastit = new JObject();
            var cabe = new[] {
                                            new {nombre="Reporte",  valor=nombrearchivo.Replace("_"," ")},
                                            new {nombre="Fecha",  valor=DateTime.Now.ToString("dd/MM/yyyy HH:mm tt")},
                                            //new {nombre="Empresa",  valor=System.Web.HttpContext.Current.Session["Empresa"].ToString()},
                                            //new {nombre="Sala",     valor=System.Web.HttpContext.Current.Session["Sala"].ToString() }

                                               };

            //if (cabecerasnuevas != null && cabecerasnuevas != "")
            //{
            //    var nuevo = cabe.ToList();
            //    var jsoncabecerasnuevas = JToken.Parse(cabecerasnuevas);
            //    foreach (var cabec in jsoncabecerasnuevas)
            //    {

            //        nuevo.Add(new { nombre = (string)cabec["nombre"], valor = (string)cabec["valor"] });


            //    }
            //    cabe = nuevo.ToArray();
            //}

            var listacabeceras = new List<tuplastring>();
            if (cabeceras != null && cabeceras != "")
            {
                var jsoncabeceras = JToken.Parse(cabeceras);
                foreach (var cabec in jsoncabeceras)
                {
                    listacabeceras.Add(new tuplastring { nombre = (string)cabec["nombre"], valor = (string)cabec["valor"] });
                }
            }
            if (cabecerasnuevas != null && cabecerasnuevas != "")
            {
                List<tuplastring> nuevo = listacabeceras;// cabe.ToList();
                var jsoncabecerasnuevas = JToken.Parse(cabecerasnuevas);
                foreach (var cabec in jsoncabecerasnuevas)
                {
                    listacabeceras.Add(new tuplastring { nombre = (string)cabec["nombre"], valor = (string)cabec["valor"] });
                }
            }
            cabezastit = JToken.FromObject(listacabeceras);
            reporte.Cabecera = (cabezastit);


            if (definicioncolumnas != null && definicioncolumnas != "")
            {
                dynamic tabladefinicioncolumnas = new JObject();
                tabladefinicioncolumnas = JToken.FromObject(definicioncolumnas);
                reporte.tabladefinicioncolumnas = (tabladefinicioncolumnas);
            }

            if (aoHeader != null && aoHeader != "")
            {
                dynamic tablaaoHeader = new JObject();
                tablaaoHeader = JToken.FromObject(aoHeader);
                reporte.tablaaoHeader = (tablaaoHeader);
            }




            dynamic tabla = new JObject();
            tabla = JToken.FromObject(JToken.Parse(datos));
            reporte.Tablas.Add((tabla));





            reporte.columnasporc = new JArray() as dynamic;
            // JObject json = JObject.Parse(datos);
            dynamic porc = new JObject();
            List<double> por = new List<double>();
            double[] porcentajescolumnas = { 8, 22, 20, 16, 6, 9, 15 };///debe sumar 100
            foreach (var p in porcentajescolumnas)
            {
                por.Add(p);
            }
            porc = JToken.FromObject(por);
            reporte.columnasporc.Add(porc);


            funciones.generarexcelNuevo(reporte);


        }

        /// <summary>
        /// botonaccionesfinalPJ  EN WEBONLINE 
        /// </summary>
        /// <param name="DATOS"></param>
        [seguridad(true)]
        [ValidateInput(false)]
        public void BOTON_REPORTES(object DATOS)
        {
            //  var Request = System.Web.HttpContext.Current.Request;
            var array_tablas = Request.Form;
            string nombrearchivo = "";

            var jobject = new JObject();
            var REPORTES_objeto = new REPORTES_OBJ();
            //REPORTES_objeto.TABLAS_DATOS = new JArray() as dynamic;
            string accion = "";

            foreach (var TABLA in array_tablas)
            {
                string nombre_tabla = (string)TABLA;
                var request_unvalidated = Request.Unvalidated;
                var valor = array_tablas[nombre_tabla];
                //var valor = "";
                JObject request_form = JObject.Parse(valor);
                if (nombre_tabla == "tabla_0")
                {
                    nombrearchivo = (string)request_form["nombrearchivo"];
                    REPORTES_objeto.nombrearchivo = nombrearchivo;
                    accion = (string)request_form["accion"];
                }

                string datos = (string)request_form["datos"];
                String porcentajes = (string)request_form["porcentajes"];
                string definicioncolumnas = (string)request_form["definicioncolumnas"];
                string cabeceras = (string)request_form["cabeceras"]; ////cabeceras por DEFECTO
                string cabecerasnuevas = (string)request_form["cabecerasnuevas"];

                string aoHeader = (string)request_form["aoHeader"];
                bool usardatatable = (bool)request_form["usardatatable"];
                string tituloreporte = (string)request_form["tituloreporte"];
                string titulo_subtitulo = (string)request_form["titulo_subtitulo"];
                string nombrehoja = (string)request_form["nombrehoja"];
                bool mostrar_headers_tabla = (bool)request_form["mostrar_headers_tabla"];
                bool multiplestablas = (bool)request_form["multiplestablas"];
                var estilos_tabla = (string)request_form["estilos_tabla"];



                var jsonObject = new CapaNegocio.Utilitarios.reporte_botones.ClaseReporte();
                ClaseReporte reporte = jsonObject;
                reporte.Tablas = new JArray() as dynamic;
                reporte.nombrearchivo = nombrearchivo;

                reporte.porcentajes = porcentajes;
                reporte.usardatatable = usardatatable;
                reporte.tituloreporte = tituloreporte;
                reporte.titulo_subtitulo = titulo_subtitulo;
                reporte.nombrehoja = nombrehoja;
                reporte.multiplestablas = multiplestablas;
                reporte.mostrar_headers_tabla = mostrar_headers_tabla;


                if (estilos_tabla != null && estilos_tabla != "null")
                {
                    //var estilos_reporte = JToken.FromObject(estilos_tabla);
                    var estilos_reporte = JsonConvert.DeserializeObject<EstilosReporte>(estilos_tabla);
                    if (estilos_reporte.ToString() != "null")
                    {
                        reporte.estilos_reporte = (estilos_reporte);
                    }
                }


                ////cabeceras archivo
                reporte.Cabecera = new JArray() as dynamic;
                var listacabeceras = new List<tuplastring>();
                if (cabeceras != null && cabeceras != "")
                {
                    var jsoncabeceras = JToken.Parse(cabeceras);
                    foreach (var cabec in jsoncabeceras)
                    {
                        listacabeceras.Add(new tuplastring { nombre = (string)cabec["nombre"], valor = (string)cabec["valor"] });
                    }
                }
                var listacabeceras_reporte = new List<tuplastring>();
                if (cabecerasnuevas != null && cabecerasnuevas != "")
                {
                    List<tuplastring> nuevo = listacabeceras;// cabe.ToList();
                    var jsoncabecerasnuevas = JToken.Parse(cabecerasnuevas);
                    foreach (var cabec in jsoncabecerasnuevas)
                    {
                        listacabeceras_reporte.Add(new tuplastring { nombre = (string)cabec["nombre"], valor = (string)cabec["valor"] });
                    }
                }
                dynamic cabezastit = new JObject();
                cabezastit = JToken.FromObject(listacabeceras);
                reporte.Cabecera = (cabezastit);

                //dynamic cabezas_reporte = new JObject();
                var cabezas_reporte = JToken.FromObject(listacabeceras_reporte);
                reporte.Cabecera_reporte = cabezas_reporte;
                ////fin cabeceras archivo

                if (definicioncolumnas != null && definicioncolumnas != "")
                {
                    dynamic tabladefinicioncolumnas = new JObject();
                    tabladefinicioncolumnas = JToken.FromObject(definicioncolumnas);
                    reporte.tabladefinicioncolumnas = (tabladefinicioncolumnas);
                }

                if (aoHeader != null && aoHeader != "")
                {
                    dynamic tablaaoHeader = new JObject();
                    tablaaoHeader = JToken.FromObject(aoHeader);
                    reporte.tablaaoHeader = (tablaaoHeader);
                }

                dynamic tabla = new JObject();
                tabla = JToken.FromObject(JToken.Parse(datos));
                reporte.Tablas.Add((tabla));
                REPORTES_objeto.TABLAS_DATOS.Add(reporte);
            }
            JObject REPORTES = JObject.FromObject(REPORTES_objeto);
            //if (accion.ToUpper() == "PDF")
            //{
            //    funciones.generarpdf_funcion_botones(REPORTES);
            //}
            //else if (accion.ToUpper() == "IMPRIMIR")
            //{
            //    funciones.imprimir_funcion_botones(REPORTES);
            //}
            //else 
            if (accion.ToUpper() == "EXCEL")
            {
                //funciones.generarexcelNuevoPJ(REPORTES); 
                funciones.generarexcel_funcion_botones(REPORTES);
            }
        }

    }
}