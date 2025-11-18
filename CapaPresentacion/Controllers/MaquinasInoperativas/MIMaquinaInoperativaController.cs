using CapaDatos.MaquinasInoperativas;
using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.MaquinasInoperativas;
using CapaEntidad.TITO;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio;
using CapaNegocio.MaquinasInoperativas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CapaEntidad.ControlAcceso;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using System.Collections;
using OfficeOpenXml.DataValidation;
using CapaNegocio.Utilitarios;
using CapaPresentacion.Utilitarios;
using System.Text;
using ImageResizer.Plugins.Basic;
using OfficeOpenXml.Table;
using System.Windows.Documents;

namespace CapaPresentacion.Controllers.MaquinasInoperativas
{
    [seguridad]
    public class MIMaquinaInoperativaController : Controller {

        MI_MaquinaInoperativaBL maquinaInoperativaBL = new MI_MaquinaInoperativaBL();
        MI_MaquinaInoperativaProblemasBL maquinaInoperativaProblemasBL = new MI_MaquinaInoperativaProblemasBL();
        MI_MaquinaInoperativaPiezasBL maquinaInoperativaPiezasBL = new MI_MaquinaInoperativaPiezasBL();
        MI_MaquinaInoperativaRepuestosBL maquinaInoperativaRepuestosBL = new MI_MaquinaInoperativaRepuestosBL();
        MI_PiezaRepuestoAlmacenBL piezaRepuestoAlmacenBL = new MI_PiezaRepuestoAlmacenBL();
        MI_AlmacenBL almacenBL = new MI_AlmacenBL();
        MI_TraspasoRepuestoAlmacenBL traspasoRepuestoAlmacenBL = new MI_TraspasoRepuestoAlmacenBL();
        MI_SalaCorreosBL salaCorreosBL = new MI_SalaCorreosBL();
        MI_ComentarioBL comentarioBL = new MI_ComentarioBL();
        Correo correoBL = new Correo();
        SEG_EmpleadoBL empleadoBL = new SEG_EmpleadoBL();
        MI_CorreoBL maquinaInoperativaCorreosBL = new MI_CorreoBL();

        public ActionResult ReporteCategoriaProblemas() {
            return View("~/Views/MaquinasInoperativas/ReporteCategoriaProblemas.cshtml");
        }
        public ActionResult ListadoSalaCorreos() {
            return View("~/Views/MaquinasInoperativas/ListadoSalaCorreos.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativa() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativa.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaAtencion() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativaAtencion.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaAtendida() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativaAtendida.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaCreado() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativaCreado.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaAtendidaOperativa() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativaAtendidaOperativa.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaAtendidaInoperativa() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativaAtendidaInoperativa.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaAtendidaInoperativaSolicitud() {
            return View("~/Views/MaquinasInoperativas/ListadoMaquinaInoperativaAtendidaInoperativaSolicitud.cshtml");
        }
        public ActionResult MaquinaInoperativaInsertarVista() {
            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaInsertarVista.cshtml");
        }
        /*public ActionResult MaquinaInoperativaEditarVista() {
            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaEditarVista.cshtml");
        }*/

        public ActionResult EditarMaquinaInoperativa(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaEditarVista.cshtml");
        }

        public ActionResult AtenderMaquinaInoperativaVista(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaAtenderVista.cshtml");
        }
        public ActionResult AtenderMaquinaInoperativaRepuestosAgregadosVista(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                //listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);


                //Repuestos Agregados Maquina Inoperativa

                var listaRepuestosAgregados = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestosAgregados = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosAgregadosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);


                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaRepuestosAgregados = listaRepuestosAgregados;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaAtenderRepuestosAgregadosVista.cshtml");
        }
        public ActionResult DetalleMaquinaInoperativa(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Problemas Maquina Inoperativa

                var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemasArray = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);




                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaProblemasArray = listaProblemasArray;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaDetalleVista.cshtml");
        }
        public ActionResult HistoricoMaquinaInoperativa(int id = 0, int id2 = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {
                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Problemas Maquina Inoperativa

                //var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                //listaProblemasArray = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Traspasos Maquina Inoperativa

                var listaTraspasos = new List<MI_TraspasoRepuestoAlmacenEntidad>();
                bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));

                if(estadoAlmacenes) {
                    listaTraspasos = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);
                } else {
                    listaTraspasos = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJsonSinAlmacenes(maquinaInoperativa.CodMaquinaInoperativa);
                }


                //Compras Maquina Inoperativa

                var listaCompras = new List<MI_TraspasoRepuestoAlmacenEntidad>();
                //listaCompras = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaTraspasos = listaTraspasos;
                ViewBag.listaCompras = listaCompras;
                ViewBag.estadoActual = id2.ToString();

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaHistoricoVista.cshtml");
        }


        public ActionResult AtenderSolicitudMaquinaInoperativaVista(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Problemas Maquina Inoperativa

                var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemasArray = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaProblemasArray = listaProblemasArray;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaAtenderSolicitudVista.cshtml");
        }

        public ActionResult AprobarSolicitudMaquinaInoperativaVista(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Problemas Maquina Inoperativa

                var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemasArray = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);




                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaProblemasArray = listaProblemasArray;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/MaquinaInoperativaAprobarSolicitudVista.cshtml");
        }

        public ActionResult ListadoDetalleAtencionMaquinaInoperativa(int id = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach(var item in listaPro) {

                    if(i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach(var cod in listaCategoriaProblemas) {
                            if(cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if(add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;

                }

                //Piezas Maquina Inoperativa

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Repuestos Maquina Inoperativa

                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                //Problemas Maquina Inoperativa

                var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemasArray = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);




                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaProblemasArray = listaProblemasArray;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativas/ListadoDetalleAtencionMaquinaInoperativa.cshtml");
        }

        public ActionResult AtenderMaquinaInoperativa(int CodMaquinaInoperativa, int[] listaRepuestosCodRepuesto, int[] listaRepuestosCodPiezaRepuestoAlmacen, int[] listaRepuestosCantidad, int[] listaRepuestosEstado, int[] listaRepuestosCodAlmacenOrigen, int[] listaRepuestosCodAlmacenDestino) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuario = Convert.ToInt32(Session["UsuarioID"]);
            bool respuesta = true;

            //DateTime fechaRecepcion = SoloFechaRecepcion.AddHours(SoloHoraRecepcion.Hour).AddMinutes(SoloHoraRecepcion.Minute).AddSeconds(SoloHoraRecepcion.Second);
            //DateTime fechaEnvio = SoloFechaEnvio.AddHours(SoloHoraEnvio.Hour).AddMinutes(SoloHoraEnvio.Minute).AddSeconds(SoloHoraEnvio.Second);

            try {

                //Data Maquina Inoperativa
                //maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(CodMaquinaInoperativa);
                maquinaInoperativa.CodMaquinaInoperativa = CodMaquinaInoperativa;



                if(listaRepuestosCodPiezaRepuestoAlmacen != null) {

                    int i = 0;
                    foreach(var item in listaRepuestosCodPiezaRepuestoAlmacen) {

                        if(listaRepuestosEstado[i] == 0) {

                            int respuestaConsultaRepuesto = 0;
                            MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                            maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                            maquinaInoperativaRepuestosEntidad.CodRepuesto = listaRepuestosCodRepuesto[i];
                            maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[i];
                            maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                            maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                            maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.ToString();
                            maquinaInoperativaRepuestosEntidad.Estado = 0;
                            respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);

                            if(respuestaConsultaRepuesto == 0) {
                                mensaje = "Error al insertar respuestos , LLame Administrador";
                                respuesta = false;
                                return Json(new { respuesta = respuesta, mensaje = mensaje });
                            } else {

                                bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));


                                if(estadoAlmacenes) {

                                    bool respuestaPiezaDescontada = piezaRepuestoAlmacenBL.DescontarCantidadPiezaRepuestoAlmacen(item, listaRepuestosCantidad[i]);

                                    if(!respuestaPiezaDescontada) {
                                        mensaje = "Error al descontar respuestos, LLame Administrador";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    }
                                }

                            }


                            maquinaInoperativa.CodEstadoProceso = 2;

                        } else {

                            MI_TraspasoRepuestoAlmacenEntidad repuesto = new MI_TraspasoRepuestoAlmacenEntidad();


                            repuesto.CodMaquinaInoperativa = CodMaquinaInoperativa;
                            repuesto.CodAlmacenOrigen = listaRepuestosCodAlmacenOrigen[i];
                            repuesto.CodAlmacenDestino = listaRepuestosCodAlmacenDestino[i];
                            repuesto.CodRepuesto = listaRepuestosCodRepuesto[i];
                            repuesto.CodPiezaRepuestoAlmacen = listaRepuestosCodPiezaRepuestoAlmacen[i];
                            repuesto.Cantidad = listaRepuestosCantidad[i];
                            repuesto.FechaRegistro = DateTime.Now;
                            repuesto.FechaModificacion = DateTime.Now;
                            repuesto.CodUsuarioRemitente = usuario;
                            repuesto.CodUsuarioDestinatario = 0;
                            repuesto.Estado = 0;

                            bool respuestaPiezaAgregada = true;
                            bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));

                            if(estadoAlmacenes) {
                                respuestaPiezaAgregada = false;
                                respuestaPiezaAgregada = piezaRepuestoAlmacenBL.AgregarCantidadPendientePiezaRepuestoAlmacen(item, listaRepuestosCantidad[i]);
                            }

                            if(respuestaPiezaAgregada) {
                                int respuestaPiezaPedida = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenInsertarJson(repuesto);


                                if(respuestaPiezaPedida == 0) {
                                    mensaje = "Error al traspasar repuestos, LLame Administrador";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                                } else {

                                    int respuestaConsultaRepuesto = 0;
                                    MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                                    maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                                    maquinaInoperativaRepuestosEntidad.CodRepuesto = listaRepuestosCodRepuesto[i];
                                    maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[i];
                                    maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                                    maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                                    maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.ToString();
                                    maquinaInoperativaRepuestosEntidad.Estado = 1;

                                    respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);

                                    if(respuestaConsultaRepuesto == 0) {
                                        mensaje = "Error al insertar respuestos , LLame Administrador";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    }
                                }

                            } else {

                                mensaje = "Error al agregar respuestos, LLame Administrador";
                                respuesta = false;
                                return Json(new { respuesta = respuesta, mensaje = mensaje });
                            }


                        }


                        i++;

                    }
                }


                if(respuesta) {

                    maquinaInoperativa.FechaAtendidaInoperativaSolicitado = DateTime.Now;
                    maquinaInoperativa.FechaAtendidaInoperativaAprobado = DateTime.Now;
                    maquinaInoperativa.CodUsuarioAtendidaInoperativaSolicitado = Convert.ToInt32(usuario);
                    maquinaInoperativa.CodUsuarioAtendidaInoperativaAprobado = Convert.ToInt32(usuario);
                    maquinaInoperativa.CodEstadoProceso = 4;
                    respuesta = maquinaInoperativaBL.AtenderSolicitudMaquinaInoperativa(maquinaInoperativa);
                    mensaje = "Maquina inoperativa atendida";
                    respuesta = true;

                    //MI_MaquinaInoperativaEntidad maquinaObtenida = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(maquinaInoperativa.CodMaquinaInoperativa);
                    //EnviarCorreoMaquinaInoperativa(maquinaObtenida.CodSala, 3);

                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                } else {

                    mensaje = "Error al atender maquina, LLame Administrador";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje });
        }


        public ActionResult AtenderMaquinaInoperativaNuevamente(int CodMaquinaInoperativa, int[] listaRepuestosCodRepuesto, int[] listaRepuestosCodPiezaRepuestoAlmacen, int[] listaRepuestosCantidad, int[] listaRepuestosEstado, int[] listaRepuestosCodAlmacenOrigen, int[] listaRepuestosCodAlmacenDestino, bool[] listaEnviar) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuario = Convert.ToInt32(Session["UsuarioID"]);
            bool respuesta = false;

            try {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(CodMaquinaInoperativa);

                //maquinaInoperativa.FechaModificacion = DateTime.Now;
                //maquinaInoperativa.CodUsuario = usuario.ToString();
                //maquinaInoperativa.Estado = 3;


                if(listaRepuestosCodPiezaRepuestoAlmacen != null) {

                    int i = 0;
                    foreach(var item in listaRepuestosCodPiezaRepuestoAlmacen) {

                        if(listaEnviar[i]) {


                            if(listaRepuestosEstado[i] == 0) {

                                int respuestaConsultaRepuesto = 0;
                                MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                                maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                                maquinaInoperativaRepuestosEntidad.CodRepuesto = listaRepuestosCodRepuesto[i];
                                maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[i];
                                maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                                maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                                maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.ToString();
                                maquinaInoperativaRepuestosEntidad.Estado = 0;
                                respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);

                                if(respuestaConsultaRepuesto == 0) {
                                    mensaje = "Error al insertar respuestos , LLame Administrador";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                                } else {

                                    bool respuestaPiezaDescontada = piezaRepuestoAlmacenBL.DescontarCantidadPiezaRepuestoAlmacen(item, listaRepuestosCantidad[i]);

                                    if(!respuestaPiezaDescontada) {
                                        mensaje = "Error al descontar respuestos, LLame Administrador";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    }

                                }



                            } else {

                                MI_TraspasoRepuestoAlmacenEntidad repuesto = new MI_TraspasoRepuestoAlmacenEntidad();


                                repuesto.CodMaquinaInoperativa = CodMaquinaInoperativa;
                                repuesto.CodAlmacenOrigen = listaRepuestosCodAlmacenOrigen[i];
                                repuesto.CodAlmacenDestino = listaRepuestosCodAlmacenDestino[i];
                                repuesto.CodRepuesto = listaRepuestosCodRepuesto[i];
                                repuesto.CodPiezaRepuestoAlmacen = listaRepuestosCodPiezaRepuestoAlmacen[i];
                                repuesto.Cantidad = listaRepuestosCantidad[i];
                                repuesto.FechaRegistro = DateTime.Now;
                                repuesto.FechaModificacion = DateTime.Now;
                                repuesto.CodUsuarioRemitente = usuario;
                                repuesto.CodUsuarioDestinatario = 0;
                                repuesto.Estado = 0;


                                bool respuestaPiezaAgregada = piezaRepuestoAlmacenBL.AgregarCantidadPendientePiezaRepuestoAlmacen(item, listaRepuestosCantidad[i]);

                                if(respuestaPiezaAgregada) {
                                    int respuestaPiezaPedida = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenInsertarJson(repuesto);

                                    if(respuestaPiezaPedida == 0) {
                                        mensaje = "Error al traspasar repuestos, LLame Administrador";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    } else {

                                        int respuestaConsultaRepuesto = 0;
                                        MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                                        maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                                        maquinaInoperativaRepuestosEntidad.CodRepuesto = listaRepuestosCodRepuesto[i];
                                        maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[i];
                                        maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                                        maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                                        maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.ToString();
                                        maquinaInoperativaRepuestosEntidad.Estado = 1;
                                        respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);

                                        if(respuestaConsultaRepuesto == 0) {
                                            mensaje = "Error al insertar respuestos , LLame Administrador";
                                            respuesta = false;
                                            return Json(new { respuesta = respuesta, mensaje = mensaje });
                                        }
                                    }

                                } else {

                                    mensaje = "Error al agregar respuestos, LLame Administrador";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                                }




                                //maquinaInoperativa.Estado = 2;
                            }

                        }

                        i++;

                    }
                }

                var ok = maquinaInoperativaBL.CambiarEstadoMaquinaInoperativa(CodMaquinaInoperativa, 2);

                mensaje = "Maquina inoperativa atendida nuevamente, LLame Administrador";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje });
        }


        [HttpPost]
        public JsonResult ListarMaquinaInoperativaJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {

                //lista = maquinaInoperativaBL.MaquinaInoperativaListadoCompletoJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMaquinaInoperativaxSalasUsuarioJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {
                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaxUsuario(codUsuario);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMaquinaInoperativaxSalasUsuarioxFechasJson(DateTime fechaIni, DateTime fechaFin, int filtroEstado = 0) {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {
                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);

                if(filtroEstado == 0) {
                    lista = maquinaInoperativaBL.GetAllMaquinaInoperativaxUsuarioxFechas(codUsuario, fechaIni, fechaFin);
                } else {
                    lista = maquinaInoperativaBL.GetAllMaquinaInoperativaxUsuarioxFechasxEstado(codUsuario, fechaIni, fechaFin, filtroEstado);
                }


            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarMaquinaInoperativaKPIxSalasUsuarioxFechasJson(DateTime fechaIni, DateTime fechaFin, int filtroEstado = 0) {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {
                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);

                lista = maquinaInoperativaBL.ReporteMaquinaInoperativa(fechaIni, fechaFin, filtroEstado);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = "Datos vacios" }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult HistoricoListadoMaquinaInoperativaKPIDescargarExcelxFechasJson(DateTime fechaIni, DateTime fechaFin, int filtroEstado = 0) {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> listaMaquina = new List<MI_MaquinaInoperativaEntidad>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            try {
                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                listaMaquina = maquinaInoperativaBL.ReporteMaquinaInoperativa(fechaIni, fechaFin, filtroEstado);

                if(listaMaquina != null && listaMaquina.Count > 0) {
                    var worksheet = excel.Workbook.Worksheets.Add("Maquinas Inoperativas");

                    // Crear encabezados
                    string[] headers = {
                        "QTY","Incidente", "Stat.inicial", "Stat.final", "Zona", "Sala", "Fecha Reportada",
                        "SERIE", "Mincetur", "Propietario", "FAB", "Modelo", "Nombre_Repuestos",
                        "Resuelto_por_Compra", "Resuelto_por_Stock", "Gasto_S/","Gasto_$", "OC",
                        "Fecha OC", "Fecha Resuelto", "Días inop", "Observaciónes"
                    };

                    for(int i = 0; i < headers.Length; i++) {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White); 
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(197, 90, 17));
                        worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }


                    int row = 2;
                    foreach(var item in listaMaquina) {
                        worksheet.Cells[row, 1].Value = row - 1;
                        worksheet.Cells[row, 2].Value = item.NombreProblema;
                        //worksheet.Cells[row, 4].Value = item.CodEstadoProceso;
                        worksheet.Cells[row, 5].Value = item.NombreZona;
                        worksheet.Cells[row, 6].Value = item.NombreSala;
                        worksheet.Cells[row, 7].Value = item.FechaCreado.ToString("dd-MM-yyyy");
                        worksheet.Cells[row, 8].Value = item.MaquinaNumeroSerie;
                        worksheet.Cells[row, 9].Value = item.CodMaquina.ToString().PadLeft(8, '0');
                        worksheet.Cells[row, 10].Value = item.MaquinaPropietario;
                        worksheet.Cells[row, 11].Value = item.MaquinaMarca;
                        worksheet.Cells[row, 12].Value = item.MaquinaModelo;
                        worksheet.Cells[row, 13].Value = item.NombreRepuesto;
                        worksheet.Cells[row, 14].Value = string.Empty;
                        worksheet.Cells[row, 15].Value = string.Empty;
                        worksheet.Cells[row, 16].Value = string.Empty; // GASTO S/
                        worksheet.Cells[row, 17].Value = string.Empty; // GASTO $
                        worksheet.Cells[row, 18].Value = item.OrdenCompra;
                        
                        DateTime fechaMinimaValida = new DateTime(1900, 1, 2);

                        if(item.FechaOrdenCompra != null &&
                            item.FechaOrdenCompra >= fechaMinimaValida) {
                            worksheet.Cells[row, 19].Value = item.FechaOrdenCompra.ToString("dd-MM-yyyy");
                        }  else {
                            worksheet.Cells[row, 19].Value = "";
                        }

                        if(item.FechaAtendidaInoperativa != null &&
                            item.FechaAtendidaInoperativa >= fechaMinimaValida) {
                            worksheet.Cells[row, 20].Value = item.FechaAtendidaInoperativa.Value.ToString("dd-MM-yyyy");
                        } else if(item.FechaAtendidaOperativa != null &&
                                   item.FechaAtendidaOperativa >= fechaMinimaValida) {
                            worksheet.Cells[row, 20].Value = item.FechaAtendidaOperativa.Value.ToString("dd-MM-yyyy");
                        } else {
                            worksheet.Cells[row, 20].Value = "";
                        }

                        worksheet.Cells[row, 21].Value = item.DiasInoperativos;
                        worksheet.Cells[row, 22].Value = item.ObservacionAtencion;

                        // Colorear según el Estado Inicial
                        int estado = item.CodEstadoInoperativa;
                        System.Drawing.Color bgColor = System.Drawing.Color.White;
                        System.Drawing.Color textColor = System.Drawing.Color.White;
                        string estadoTexto = string.Empty;

                        switch(estado) {
                            case 1:
                                estadoTexto = "O/PROB";
                                bgColor = System.Drawing.Color.FromArgb(255, 192, 17);
                                textColor = System.Drawing.Color.FromArgb(255, 0, 0);
                                break;
                            case 2:
                                estadoTexto = "INOP";
                                bgColor = System.Drawing.Color.FromArgb(255, 0, 0);
                                textColor = System.Drawing.Color.FromArgb(255, 229, 153);
                                break;
                            default:

                                break;
                        }


                        worksheet.Cells[row, 3].Value = estadoTexto;

                        worksheet.Cells[row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(bgColor);
                        worksheet.Cells[row, 3].Style.Font.Color.SetColor(textColor);
                        worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                        // Colorear según el Estado Final
                        int estadoFinal = item.CodEstadoProceso;
                        int CodEstadoReparacion = item.CodEstadoReparacion;
                        System.Drawing.Color bgColorFinal = System.Drawing.Color.White;
                        string estadoTextoFinal = string.Empty;

                        switch(estadoFinal) {
                            case 2: // Atendida operativa
                                if(CodEstadoReparacion == 1) {
                                    estadoTextoFinal = "ATENDIDA OPERATIVA - REPARADA";
                                } else if(CodEstadoReparacion == 2) {
                                    estadoTextoFinal = "ATENDIDA OPERATIVA - NO REPARADA";
                                } else {
                                    estadoTextoFinal = "ATENDIDA OPERATIVA";
                                }
                                bgColorFinal = System.Drawing.Color.FromArgb(217, 234, 211); // Verde suave
                                break;

                            case 3: // Atendida inoperativa
                                estadoTextoFinal = "ATENDIDA INOPERATIVA";
                                bgColorFinal = System.Drawing.Color.FromArgb(255, 0, 0); // Rojo
                                break;

                            case 4:
                                estadoTextoFinal = "EN ESPERA SOLICITUD";
                                bgColorFinal = System.Drawing.Color.FromArgb(255, 193, 7); // Similar a warning
                                break;

                            case 5:
                                estadoTextoFinal = "REPUESTOS AGREGADOS";
                                bgColorFinal = System.Drawing.Color.FromArgb(0, 123, 255); // Azul primario
                                break;

                            default:
                                estadoTextoFinal = "CREADO";
                                bgColorFinal = System.Drawing.Color.LightGray;
                                break;
                        }


                        worksheet.Cells[row, 4].Value = estadoTextoFinal;

                        worksheet.Cells[row, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[row, 4].Style.Fill.BackgroundColor.SetColor(bgColorFinal);
                        worksheet.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        System.Drawing.Color fontColorFinal = System.Drawing.Color.Black;
                        double brightness = (bgColorFinal.R * 0.299 + bgColorFinal.G * 0.587 + bgColorFinal.B * 0.114);
                        if(brightness < 140) {
                            fontColorFinal = System.Drawing.Color.White; // fondo oscuro → texto blanco
                        }

                        worksheet.Cells[row, 4].Style.Font.Color.SetColor(fontColorFinal);


                        // Agregar bordes a toda la fila
                        for(int col = 1; col <= 22; col++) {
                            worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        row++;
                    }

                    // Ajustar el ancho automáticamente
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Convertir el rango a tabla
                    var tblRange = worksheet.Cells[1, 1, row - 1, 22];
                    var table = worksheet.Tables.Add(tblRange, "MaquinasTable");
                    table.TableStyle = TableStyles.None;

                    // Guardar y convertir a base64
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                    excelName = "ListadoMaquinaInoperativa_" + fecha + ".xlsx";
                } else {
                    mensaje = "No se pudo generar el archivo porque no hay datos.";
                }
            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame al Administrador";
            }

            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
        }


        [seguridad(false)]

        [HttpPost]
        public JsonResult ListarMaquinaInoperativaCreadoJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaCreado(codUsuario);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMaquinaInoperativaAtendidaOperativaJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaAtendidaOperativa(codUsuario);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMaquinaInoperativaAtendidaInoperativaJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaAtendidaInoperativa(codUsuario);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMaquinaInoperativaAtendidaInoperativaSolicitudJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaAtendidaInoperativaSolicitud(codUsuario);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMaquinaInoperativaAtendidaJson() {
            var errormensaje = "";
            var lista = new List<MI_MaquinaInoperativaEntidad>();

            try {

                //lista = maquinaInoperativaBL.MaquinaInoperativaAtendidaListadoCompletoJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MaquinaInoperativaAtenderJson(MI_MaquinaInoperativaEntidad maquinaInoperativa, int TipoAtencion, int[] listaProblemas, int[] listaPiezasAnt, int[] listaPiezas, int[] listaPiezasCantidad, int[] listaRepuestos, int[] listaRepuestosCantidad, int[] listaRepuestosCodPiezaRepuestoAlmacen) {

            var errormensaje = "";
            //bool respuestaConsulta = false;
            bool respuesta = true;
            int respuestaConsultaProblema = 0;
            int respuestaConsultaPieza = 0;
            int respuestaConsultaRepuesto = 0;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];

            try {

                if(listaRepuestosCodPiezaRepuestoAlmacen != null) {

                    int i = 0;
                    foreach(var item in listaRepuestosCodPiezaRepuestoAlmacen) {

                        MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                        maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                        maquinaInoperativaRepuestosEntidad.CodRepuesto = listaRepuestos[i];
                        maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[i];
                        maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                        maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                        maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.UsuarioNombre;
                        maquinaInoperativaRepuestosEntidad.Estado = 0;
                        respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);

                        if(respuestaConsultaRepuesto == 0) {
                            errormensaje = "Error al insertar respuestos , LLame Administrador";
                            respuesta = false;
                            return Json(new { respuesta = respuesta, mensaje = errormensaje });
                        } else {


                            bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));

                            if(estadoAlmacenes) {

                                bool respuestaPiezaDescontada = piezaRepuestoAlmacenBL.DescontarCantidadPiezaRepuestoAlmacen(item, listaRepuestosCantidad[i]);

                                if(!respuestaPiezaDescontada) {
                                    errormensaje = "Error al descontar respuestos, LLame Administrador";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = errormensaje });
                                }

                            }


                        }

                        maquinaInoperativa.CodEstadoProceso = 2;

                        i++;

                    }
                }


                if(respuesta) {

                    errormensaje = "Registro de Maquina Inoperativa Actualizado Correctamente";
                    respuesta = true;

                    //AGREGAR MAQUINA INOPERATIVA PROBLEMAS
                    if(listaProblemas != null) {

                        bool limpiarLista = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasEliminarxMaquinaJson(maquinaInoperativa.CodMaquinaInoperativa);

                        if(limpiarLista) {
                            foreach(var item in listaProblemas) {

                                respuestaConsultaProblema = 0;
                                MI_MaquinaInoperativaProblemasEntidad maquinaInoperativaProblemasEntidad = new MI_MaquinaInoperativaProblemasEntidad();

                                maquinaInoperativaProblemasEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                                maquinaInoperativaProblemasEntidad.CodProblema = item;
                                maquinaInoperativaProblemasEntidad.FechaRegistro = DateTime.Now;
                                maquinaInoperativaProblemasEntidad.FechaModificacion = DateTime.Now;
                                maquinaInoperativaProblemasEntidad.CodUsuario = usuario.UsuarioNombre;
                                maquinaInoperativaProblemasEntidad.Estado = 1;
                                respuestaConsultaProblema = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasInsertarJson(maquinaInoperativaProblemasEntidad);

                                if(respuestaConsultaProblema == 0) {

                                    errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en los Problemas , LLame Administrador";
                                    respuesta = false;
                                    break;
                                }
                            }
                        } else {
                            errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en limpieza de los Problemas , LLame Administrador";
                            respuesta = false;
                        }
                    }


                    //AGREGAR MAQUINA INOPERATIVA PIEZAS
                    if(listaPiezas != null) {

                        bool limpiarLista = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasEliminarxMaquinaJson(maquinaInoperativa.CodMaquinaInoperativa);

                        if(limpiarLista) {

                            int contPieza = 0;
                            foreach(var item in listaPiezas) {

                                respuestaConsultaPieza = 0;
                                MI_MaquinaInoperativaPiezasEntidad maquinaInoperativaPiezasEntidad = new MI_MaquinaInoperativaPiezasEntidad();

                                maquinaInoperativaPiezasEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                                maquinaInoperativaPiezasEntidad.CodPieza = item;
                                maquinaInoperativaPiezasEntidad.Cantidad = listaPiezasCantidad[contPieza];
                                maquinaInoperativaPiezasEntidad.FechaRegistro = DateTime.Now;
                                maquinaInoperativaPiezasEntidad.FechaModificacion = DateTime.Now;
                                maquinaInoperativaPiezasEntidad.CodUsuario = usuario.UsuarioNombre;
                                maquinaInoperativaPiezasEntidad.Estado = 1;
                                respuestaConsultaPieza = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasInsertarJson(maquinaInoperativaPiezasEntidad);
                                contPieza++;
                                if(respuestaConsultaPieza == 0) {

                                    errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en las Piezas , LLame Administrador";
                                    respuesta = true;
                                    break;
                                }
                            }
                        } else {
                            errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en limpieza de los Piezas , LLame Administrador";
                            respuesta = false;
                        }
                    } else {
                        bool limpiarLista = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasEliminarxMaquinaJson(maquinaInoperativa.CodMaquinaInoperativa);
                        if(!limpiarLista) {

                            errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en limpieza de los Piezas , LLame Administrador";
                            respuesta = false;
                        }
                    }

                    //AGREGAR MAQUINA INOPERATIVA REPUESTOS
                    //if(listaRepuestos != null) {

                    //    bool limpiarLista = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosEliminarxMaquinaJson(maquinaInoperativa.CodMaquinaInoperativa);

                    //    if(limpiarLista) {

                    //        int contRepuesto = 0;
                    //        foreach(var item in listaRepuestos) {

                    //            respuestaConsultaRepuesto = 0;
                    //            MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                    //            maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
                    //            maquinaInoperativaRepuestosEntidad.CodRepuesto = item;
                    //            maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[contRepuesto];
                    //            maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                    //            maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                    //            maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.UsuarioNombre;
                    //            maquinaInoperativaRepuestosEntidad.Estado = 1;
                    //            respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);
                    //            contRepuesto++;

                    //            if(respuestaConsultaRepuesto == 0) {

                    //                errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en los Repuestos , LLame Administrador";
                    //                respuesta = true;
                    //                break;
                    //            }
                    //            else
                    //            {

                    //                bool respuestaPiezaDescontada = piezaRepuestoAlmacenBL.DescontarCantidadPiezaRepuestoAlmacen(item, maquinaInoperativaRepuestosEntidad.Cantidad);

                    //                if (!respuestaPiezaDescontada)
                    //                {
                    //                    errormensaje = "Error al descontar respuestos, LLame Administrador";
                    //                    respuesta = false;
                    //                    return Json(new { respuesta = respuesta, mensaje = errormensaje });
                    //                }

                    //            }




                    //            maquinaInoperativa.CodEstadoProceso = 2;

                    //        }
                    //    } else {
                    //        errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en limpieza de los Repuestos , LLame Administrador";
                    //        respuesta = false;
                    //    }
                    //}

                } else {
                    errormensaje = "Error al Actualizar Maquina Inoperativa , LLame Administrador";
                    respuesta = false;
                }

                if(respuesta) {

                    if(TipoAtencion == 1) {
                        maquinaInoperativa.FechaAtendidaOperativa = DateTime.Now;
                        maquinaInoperativa.CodUsuarioAtendidaOperativa = Convert.ToInt32(usuario.UsuarioID);
                        maquinaInoperativa.CodEstadoProceso = 2;
                        respuesta = maquinaInoperativaBL.AtenderMaquinaInoperativaOperativa(maquinaInoperativa);

                        //MI_MaquinaInoperativaEntidad maquinaObtenida = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(maquinaInoperativa.CodMaquinaInoperativa);
                        //EnviarCorreoMaquinaInoperativa(maquinaObtenida.CodSala,2);
                    } else if(TipoAtencion == 2) {
                        maquinaInoperativa.FechaAtendidaInoperativa = DateTime.Now;
                        maquinaInoperativa.CodUsuarioAtendidaInoperativa = Convert.ToInt32(usuario.UsuarioID);
                        maquinaInoperativa.CodEstadoProceso = 3;
                        respuesta = maquinaInoperativaBL.AtenderMaquinaInoperativaInoperativa(maquinaInoperativa);

                        //MI_MaquinaInoperativaEntidad maquinaObtenida = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(maquinaInoperativa.CodMaquinaInoperativa);
                        //EnviarCorreoMaquinaInoperativa(maquinaObtenida.CodSala, 2);
                    } else {

                        errormensaje = "Error Tipo de Atencion Invalido, LLame Administrador";
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = errormensaje });
                    }
                }


            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult MaquinaInoperativaGuardarJson(MI_MaquinaInoperativaEntidad maquinaInoperativa, int[] listaProblemas, int[] listaPiezas, int[] listaPiezasCantidad, int[] listaRepuestos, int[] listaRepuestosCantidad, int[] listaCorreos) {
            var errormensaje = "";
            int respuestaConsulta = 0;
            bool respuesta = false;
            int respuestaConsultaProblema = 0;
            int respuestaConsultaPieza = 0;
            int respuestaConsultaRepuesto = 0;

            try {

                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                maquinaInoperativa.FechaCreado = DateTime.Now;
                maquinaInoperativa.FechaInoperativa= DateTime.Now;
                maquinaInoperativa.CodEstadoProceso = 1;
                maquinaInoperativa.CodUsuarioCreado = usuario.UsuarioID;

                respuestaConsulta = maquinaInoperativaBL.InsertarMaquinaInoperativaCreado(maquinaInoperativa);

                if(respuestaConsulta > 0) {

                    errormensaje = "Registro de Maquina Inoperativa Guardado Correctamente";
                    respuesta = true;

                    //AGREGAR MAQUINA INOPERATIVA PROBLEMAS
                    if(listaProblemas != null) {
                        foreach(var item in listaProblemas) {

                            respuestaConsultaProblema = 0;
                            MI_MaquinaInoperativaProblemasEntidad maquinaInoperativaProblemasEntidad = new MI_MaquinaInoperativaProblemasEntidad();

                            maquinaInoperativaProblemasEntidad.CodMaquinaInoperativa = respuestaConsulta;
                            maquinaInoperativaProblemasEntidad.CodProblema = item;
                            maquinaInoperativaProblemasEntidad.FechaRegistro = DateTime.Now;
                            maquinaInoperativaProblemasEntidad.FechaModificacion = DateTime.Now;
                            maquinaInoperativaProblemasEntidad.CodUsuario = usuario.UsuarioNombre;
                            maquinaInoperativaProblemasEntidad.Estado = 1;
                            respuestaConsultaProblema = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasInsertarJson(maquinaInoperativaProblemasEntidad);

                            if(respuestaConsultaProblema == 0) {

                                errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en los Problemas , LLame Administrador";
                                respuesta = true;
                                break;
                            }
                        }
                    }


                    //AGREGAR MAQUINA INOPERATIVA PIEZAS
                    if(listaPiezas != null) {
                        int contPieza = 0;
                        foreach(var item in listaPiezas) {

                            respuestaConsultaPieza = 0;
                            MI_MaquinaInoperativaPiezasEntidad maquinaInoperativaPiezasEntidad = new MI_MaquinaInoperativaPiezasEntidad();

                            maquinaInoperativaPiezasEntidad.CodMaquinaInoperativa = respuestaConsulta;
                            maquinaInoperativaPiezasEntidad.CodPieza = item;
                            maquinaInoperativaPiezasEntidad.Cantidad = listaPiezasCantidad[contPieza];
                            maquinaInoperativaPiezasEntidad.FechaRegistro = DateTime.Now;
                            maquinaInoperativaPiezasEntidad.FechaModificacion = DateTime.Now;
                            maquinaInoperativaPiezasEntidad.CodUsuario = usuario.UsuarioNombre;
                            maquinaInoperativaPiezasEntidad.Estado = 1;
                            respuestaConsultaPieza = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasInsertarJson(maquinaInoperativaPiezasEntidad);
                            contPieza++;
                            if(respuestaConsultaPieza == 0) {

                                errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en las Piezas , LLame Administrador";
                                respuesta = true;
                                break;
                            }
                        }
                    }

                    //AGREGAR MAQUINA INOPERATIVA REPUESTOS
                    if(listaRepuestos != null) {
                        int contRepuesto = 0;
                        foreach(var item in listaRepuestos) {

                            respuestaConsultaRepuesto = 0;
                            MI_MaquinaInoperativaRepuestosEntidad maquinaInoperativaRepuestosEntidad = new MI_MaquinaInoperativaRepuestosEntidad();

                            maquinaInoperativaRepuestosEntidad.CodMaquinaInoperativa = respuestaConsulta;
                            maquinaInoperativaRepuestosEntidad.CodRepuesto = item;
                            maquinaInoperativaRepuestosEntidad.Cantidad = listaRepuestosCantidad[contRepuesto];
                            maquinaInoperativaRepuestosEntidad.FechaRegistro = DateTime.Now;
                            maquinaInoperativaRepuestosEntidad.FechaModificacion = DateTime.Now;
                            maquinaInoperativaRepuestosEntidad.CodUsuario = usuario.UsuarioNombre;
                            maquinaInoperativaRepuestosEntidad.Estado = 1;
                            respuestaConsultaRepuesto = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosInsertarJson(maquinaInoperativaRepuestosEntidad);
                            contRepuesto++;

                            if(respuestaConsultaRepuesto == 0) {

                                errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en los Repuestos , LLame Administrador";
                                respuesta = true;
                                break;
                            }
                        }
                    }


                    //AGREGAR MAQUINA INOPERATIVA CORREOS
                    if(listaCorreos != null) {
                        int contCorreo = 0;
                        foreach(var item in listaCorreos) {

                            MI_CorreoEntidad correo = new MI_CorreoEntidad();

                            correo.CodMaquinaInoperativa = respuestaConsulta;
                            correo.CodUsuario = listaCorreos[contCorreo];
                            correo.CodEstadoProceso = 1;
                            bool respuestaConsultaCorreo = maquinaInoperativaCorreosBL.AgregarCorreo(correo);
                            contCorreo++;

                            if(!respuestaConsultaCorreo) {

                                errormensaje = "Registro de Correos error , LLame Administrador";
                                respuesta = false;
                                break;
                            }

                            respuesta = true;

                        }
                        if(respuesta) {
                            EnviarCorreoMaquinaInoperativa(respuestaConsulta, 1);
                        }
                    }



                } else {
                    errormensaje = "Error al Crear Maquina Inoperativa , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
                respuesta = false;
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarMaquinasAdministrativo() {
            int status = 0;
            var listMaquinas = new List<MI_MaquinaDetalleEntidad>();

            try {
                var response = string.Empty;
                string uriAdministrativo = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                string uri = $"{uriAdministrativo}Administrativo/ListarMaquinasAdministrativo";

                dynamic data = new
                {
                };

                string json = JsonConvert.SerializeObject(data);

                using(WebClient webClient = new WebClient()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                MI_MaquinaDetalleListResponse jsonResponse = JsonConvert.DeserializeObject<MI_MaquinaDetalleListResponse>(response, settings);

                if(!jsonResponse.respuesta) {
                    return Json(new
                    {
                        status,
                        message = "No se encontro datos"
                    });
                }

                listMaquinas = jsonResponse.data;

                status = 1;
            } catch(Exception ex) {
                return Json(new
                {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listMaquinas.ToList()
                });
            }

            var jsonResult = Json(new
            {
                status,
                message = "Datos obtenidos correctamente",
                data = listMaquinas.ToList()
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarMaquinasAdministrativoxSala(int cod) {
            int status = 0;
            var listMaquinas = new List<MI_MaquinaDetalleEntidad>();

            try {
                var response = string.Empty;
                string uriAdministrativo = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                string uri = $"{uriAdministrativo}Administrativo/ListarMaquinasAdministrativoxSala";

                dynamic data = new
                {
                    codSala = cod
                };

                string json = JsonConvert.SerializeObject(data);

                using(WebClient webClient = new WebClient()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                MI_MaquinaDetalleListResponse jsonResponse = JsonConvert.DeserializeObject<MI_MaquinaDetalleListResponse>(response, settings);

                if(!jsonResponse.respuesta) {
                    return Json(new
                    {
                        status,
                        message = "No se encontro datos"
                    });
                }

                listMaquinas = jsonResponse.data;

                status = 1;
            } catch(Exception ex) {
                return Json(new
                {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listMaquinas.ToList()
                });
            }

            var jsonResult = Json(new
            {
                status,
                message = "Datos obtenidos correctamente",
                data = listMaquinas.ToList()
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarMaquinaDetalleAdministrativo(int codMaquina) {
            int status = 0;
            var maquina = new MI_MaquinaDetalleEntidad();

            try {
                var response = string.Empty;
                string uriAdministrativo = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                string uri = $"{uriAdministrativo}Administrativo/ListarMaquinaDetalleAdministrativo";

                dynamic data = new
                {
                    codMaquina = codMaquina,
                };

                string json = JsonConvert.SerializeObject(data);

                using(WebClient webClient = new WebClient()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                MI_MaquinaDetalleResponse jsonResponse = JsonConvert.DeserializeObject<MI_MaquinaDetalleResponse>(response, settings);

                if(!jsonResponse.respuesta) {
                    return Json(new
                    {
                        status,
                        message = "No se encontró datos"
                    });
                }

                maquina = jsonResponse.data;

                status = 1;
            } catch(Exception ex) {
                return Json(new
                {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = maquina
                });
            }

            var jsonResult = Json(new
            {
                status,
                message = "Datos obtenidos correctamente",
                data = maquina
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        /*
        [seguridad(false)]
        [HttpPost]
        public JsonResult InventarioDescontarRepuestos(int[] listaRepuestos, int[] listaRepuestosCantidad) {
            var errormensaje = "";
            var respuesta = false;
            var errorInventario = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            var listaEditar = new List<MI_PiezaRepuestoAlmacenEntidad>();

            int cont = 0;

            try {

                lista = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoActiveJson();

                foreach(var item in listaRepuestos) {
                    var result = lista.Where(x=> x.CodTipo==2).FirstOrDefault(x => x.CodPiezaRepuesto == item );
                    if(result != null) {

                        if(listaRepuestosCantidad[cont] <= result.Cantidad) {
                            MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacenEntidad = new MI_PiezaRepuestoAlmacenEntidad();
                            result.Cantidad = result.Cantidad - listaRepuestosCantidad[cont];
                            result.FechaModificacion = DateTime.Now;
                            if(result.Cantidad <= 0) {
                                result.Estado = 1;
                            }
                            listaEditar.Add(result);
                        } else {

                            errormensaje = "No hay stock para el repuesto con codigo "+result.CodPiezaRepuesto+" , solo quedan "+result.Cantidad+" repuestos.";
                            errorInventario= true;
                            break;
                        }
                        cont++;


                    }else {

                        errormensaje = "No se encontraron algunas piezas en los almacenes." + ",Llame Administrador";
                        break;
                    }
                }

                if(cont == listaRepuestos.Length) {

                    foreach(var item in listaEditar) {
                        bool agregado = piezaRepuestoAlmacenBL.EditarCantidadPiezaRepuestoAlmacen(item);
                        if(!agregado) {
                            errormensaje = "Error al descontar piezas del almacen" + ",Llame Administrador";
                            respuesta = false;
                        }
                    }

                    errormensaje = "Repuestos descontados correctamente" + ",Llame Administrador";
                    respuesta = true;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(),respuesta=respuesta, errorInventario= errorInventario, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        */

        [HttpPost]
        public JsonResult InventarioDescontarRepuestos(int[] listaRepuestos, int[] listaRepuestosCantidad, int[] listaRepuestosAlmacen) {
            var errormensaje = "";
            var respuesta = false;
            var errorInventario = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            var listaEditar = new List<MI_PiezaRepuestoAlmacenEntidad>();

            int cont = 0;

            try {

                var listaTemp = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoActiveJson().Where(x => x.CodTipo == 2);

                foreach(var item in listaRepuestos) {
                    var result2 = listaTemp.Where(x => x.CodPiezaRepuesto == item).Where(x => x.CodAlmacen == listaRepuestosAlmacen[cont]);
                    if(result2 != null) {
                        lista.AddRange(result2);

                        var result = lista[0];

                        if(listaRepuestosCantidad[cont] <= result.Cantidad) {
                            MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacenEntidad = new MI_PiezaRepuestoAlmacenEntidad();
                            result.Cantidad = result.Cantidad - listaRepuestosCantidad[cont];
                            result.FechaModificacion = DateTime.Now;
                            if(result.Cantidad <= 0) {
                                result.Estado = 1;
                            }
                            listaEditar.Add(result);
                        } else {

                            errormensaje = "No hay stock para el repuesto con codigo " + result.CodPiezaRepuesto + " , solo quedan " + result.Cantidad + " repuestos.";
                            errorInventario = true;
                            break;
                        }
                        cont++;


                    } else {

                        errormensaje = "No se encontraron algunas piezas en los almacenes." + ",Llame Administrador";
                        break;
                    }
                }

                if(cont == listaRepuestos.Length) {

                    foreach(var item in listaEditar) {
                        bool agregado = piezaRepuestoAlmacenBL.EditarCantidadPiezaRepuestoAlmacen(item);
                        if(!agregado) {
                            errormensaje = "Error al descontar piezas del almacen" + ",Llame Administrador";
                            respuesta = false;
                        }
                    }

                    errormensaje = "Repuestos descontados correctamente" + ",Llame Administrador";
                    respuesta = true;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, errorInventario = errorInventario, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult InventarioEditarDescontarRepuestos(int[] listaRepuestos, int[] listaRepuestosCantidad, int[] listaRepuestosAnt, int[] listaRepuestosCantidadAnt) {
            var errormensaje = "";
            var respuesta = false;
            var errorInventario = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            var listaEditar = new List<MI_PiezaRepuestoAlmacenEntidad>();
            var listaAgregar = new List<MI_PiezaRepuestoAlmacenEntidad>();


            try {

                lista = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoActiveJson();

                if(listaRepuestosAnt != null && listaRepuestos != null) {

                    //ADD ANT CANT
                    int contAnt = 0;
                    foreach(var itemAnt in listaRepuestosAnt) {
                        var result = lista.Where(x => x.CodTipo == 2).FirstOrDefault(x => x.CodPiezaRepuesto == itemAnt);
                        if(result != null) {

                            int conti = 0;
                            foreach(var i in lista) {
                                if(i.CodPiezaRepuesto == itemAnt) {
                                    break;
                                }
                                conti++;
                            }

                            lista.ElementAt(conti).Cantidad = lista.ElementAt(conti).Cantidad + listaRepuestosCantidadAnt[contAnt];
                            lista.ElementAt(conti).FechaModificacion = DateTime.Now;
                            lista.ElementAt(conti).Estado = 1;
                            listaAgregar.Add(lista.ElementAt(conti));

                        } else {

                            errormensaje = "No se encontraron algunas piezas en los almacenes." + ",Llame Administrador";
                            break;
                        }
                        contAnt++;
                    }

                    // SUBSTRACT CANT

                    int cont = 0;
                    foreach(var item in listaRepuestos) {
                        var result = lista.Where(x => x.CodTipo == 2).FirstOrDefault(x => x.CodPiezaRepuesto == item);

                        if(result != null) {


                            if(listaRepuestosCantidad[cont] <= result.Cantidad) {
                                MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacenEntidad = new MI_PiezaRepuestoAlmacenEntidad();
                                result.Cantidad = result.Cantidad - listaRepuestosCantidad[cont];
                                result.FechaModificacion = DateTime.Now;
                                result.Estado = 1;
                                listaEditar.Add(result);
                            } else {

                                errormensaje = "No hay stock para el repuesto con codigo " + result.CodPiezaRepuesto + " , solo quedan " + result.Cantidad + " repuestos.";
                                errorInventario = true;
                                break;
                            }
                            cont++;


                        } else {

                            errormensaje = "No se encontraron algunas piezas en los almacenes." + ",Llame Administrador";
                            break;
                        }
                    }

                    if(contAnt == listaRepuestosAnt.Length && cont == listaRepuestos.Length) {

                        foreach(var item in listaAgregar) {
                            bool agregado = piezaRepuestoAlmacenBL.EditarCantidadPiezaRepuestoAlmacen(item);
                            if(!agregado) {
                                errormensaje = "Error al descontar piezas del almacen" + ",Llame Administrador";
                                respuesta = false;
                            }
                        }

                        foreach(var item in listaEditar) {
                            bool agregado = piezaRepuestoAlmacenBL.EditarCantidadPiezaRepuestoAlmacen(item);
                            if(!agregado) {
                                errormensaje = "Error al descontar piezas del almacen" + ",Llame Administrador";
                                respuesta = false;
                            }
                        }

                        errormensaje = "Repuestos descontados correctamente" + ",Llame Administrador";
                        respuesta = true;
                    }
                } else {

                    errormensaje = "Piezas correctamente" + ",Llame Administrador";
                    respuesta = true;
                }


            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, errorInventario = errorInventario, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult BuscarInventarioRepuestoAlmacenes(int cod, int cantidad) {
            var errormensaje = "";
            var respuesta = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            var listaAyuda = new List<MI_PiezaRepuestoAlmacenEntidad>();


            try {

                var listaCompleta = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoActiveJson().Where(x => x.CodTipo == 2).Where(x => x.CodPiezaRepuesto == cod).Where(x => x.Cantidad >= cantidad);
                //var result = listaCompleta.Where(x => x.CodTipo == 2).Where(x => x.CodPiezaRepuesto == cod);

                listaAyuda.AddRange(listaCompleta);

                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                var almacenesPropios = almacenBL.GetAllAlmacenxUsuario(usuario.UsuarioID);

                if(almacenesPropios.Count > 0) {
                    //listaAyuda.Clear();

                    foreach(var item in almacenesPropios) {
                        var listaTemp = listaCompleta.FirstOrDefault(x => x.CodAlmacen == item.CodAlmacen);
                        if(listaTemp != null) {
                            lista.Add(listaTemp);
                            listaAyuda.Remove(listaTemp);
                        }
                    }


                }


                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), data2 = listaAyuda.Distinct().ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult BuscarInventarioRepuestoAlmacenesPropios(int cod) {
            var errormensaje = "";
            var respuesta = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();


            try {
                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                lista = piezaRepuestoAlmacenBL.GetPiezaRepuestoAlmacenPropioxCodPiezaRepuesto(cod, usuario.UsuarioID);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BuscarInventarioRepuestoAlmacenesAjenos(int cod) {
            var errormensaje = "";
            var respuesta = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();


            try {
                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];

                lista = piezaRepuestoAlmacenBL.GetPiezaRepuestoAlmacenTodoxCodPiezaRepuesto(cod);

                var listaUsuario = piezaRepuestoAlmacenBL.GetPiezaRepuestoAlmacenAjenoxCodPiezaRepuesto(cod, usuario.UsuarioID);

                foreach(var item in listaUsuario) {
                    lista = lista.Where(x => x.CodPiezaRepuestoAlmacen != item.CodPiezaRepuestoAlmacen).ToList();
                }

                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        //MANTENEDORES

        [HttpPost]
        public JsonResult ListarPiezasxMaquinaJson(int cod) {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_MaquinaInoperativaPiezasEntidad>();

            try {

                lista = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(cod);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProblemasxMaquinaJson(int cod) {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_MaquinaInoperativaProblemasEntidad>();

            try {

                lista = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(cod);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarRepuestosxMaquinaJson(int cod) {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_MaquinaInoperativaRepuestosEntidad>();

            try {

                lista = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(cod);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult HistoricoxMaquinaInoperativaDescargarExcelJson(int codMaquinaInoperativa) {
        //    string fecha = DateTime.Now.ToString("dd_MM_yyyy");
        //    string mensaje = string.Empty;
        //    string mensajeConsola = string.Empty;
        //    bool respuesta = false;
        //    string base64String = "";
        //    string excelName = string.Empty;
        //    MI_MaquinaInoperativaEntidad maquina = new MI_MaquinaInoperativaEntidad();
        //    var strElementos = String.Empty;
        //    var strElementos_ = String.Empty;
        //    var nombresala = new List<dynamic>();
        //    var salasSeleccionadas = String.Empty;

        //    //Nuevo Metodo Excel con Collapse
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    ExcelPackage excel = new ExcelPackage();
        //    var ws = excel.Workbook.Worksheets.Add("Reporte Historico Maquina Inoperativa");
        //    ws.Cells["B4"].Value = "Reporte Historico Maquina Inoperativa";
        //    ws.Cells["B4:J4"].Style.Font.Bold = true;
        //    ws.Cells["B4"].Style.Font.Size = 20;
        //    ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    ws.Cells["B4:J4"].Merge = true;
        //    ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    int fila = 7, inicioGrupo = 0, finGrupo = 0;

        //    try {


        //        //Data Maquina Inoperativa

        //        maquina = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(codMaquinaInoperativa);

        //        //Problemas Maquina Inoperativa

        //        var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
        //        listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(codMaquinaInoperativa);

        //        //Piezas Maquina Inoperativa

        //        var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
        //        listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(codMaquinaInoperativa);

        //        //Repuestos Maquina Inoperativa

        //        var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
        //        listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(codMaquinaInoperativa);

        //        //Traspasos Maquina Inoperativa

        //        var listaTraspasos = new List<MI_TraspasoRepuestoAlmacenEntidad>();
        //        listaTraspasos = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJson(codMaquinaInoperativa);


        //        if(maquina != null) {

        //            int filaInicio = 0;
        //            int filaFinal = 0;



        //            if(maquina.CodEstadoProceso >= 1) {

        //                //**CREADO INICIO

        //                ws.Cells[string.Format("B{0}", fila)].Value = "CREADO";

        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaCreado.ToString("dd/MM/yyyy hh:mm:ss");
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioCreado.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                fila++;


        //                //DATOS MAQUINA

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS MAQUINA";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Ley: " + maquina.MaquinaLey.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Modelo: " + maquina.MaquinaModelo.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Linea: " + maquina.MaquinaLinea.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Sala: " + maquina.MaquinaSala.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Juego: " + maquina.MaquinaJuego.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Numero Serie: " + maquina.MaquinaNumeroSerie.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Propietario: " + maquina.MaquinaPropietario.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Ficha: " + maquina.MaquinaFicha.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Marca: " + maquina.MaquinaMarca.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Token: " + maquina.MaquinaToken.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                //DATOS GENERALES

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                string estadoInoperativa = maquina.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquina.CodEstadoInoperativa == 2 ? "Inoperativa" : "Atendida en Sala");
        //                ws.Cells[string.Format("B{0}", fila)].Value = "Estado Inoperativa: " + estadoInoperativa;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                string prioridad = maquina.CodEstadoInoperativa == 1 ? "Baja" : (maquina.CodEstadoInoperativa == 2 ? "Media" : "Alta");
        //                ws.Cells[string.Format("B{0}", fila)].Value = "Prioridad: " + prioridad;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha Inoperativa: " + maquina.FechaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoCreado.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionCreado.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


        //                //PROBLEMAS INICIO

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;


        //                ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
        //                ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
        //                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                //Styles
        //                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;

        //                //Border
        //                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                fila++;


        //                if(listaProblemas.Count > 0) {

        //                    foreach(var item in listaProblemas) {

        //                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
        //                        ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
        //                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;


        //                    }

        //                } else {

        //                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    //Border
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                }


        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;


        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                //PROBLEMAS FINAL


        //                //PIEZAS INICIO

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;


        //                ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
        //                ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                //Styles
        //                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;

        //                //Border
        //                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                fila++;


        //                if(listaPiezas.Count > 0) {

        //                    foreach(var item in listaPiezas) {

        //                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
        //                        ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;


        //                    }

        //                } else {

        //                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    //Border
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;

        //                }


        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                //PIEZAS FINAL

        //                //**CREADO FIN
        //            }




        //            if(maquina.CodEstadoProceso == 2 && listaTraspasos.Count == 0) {

        //                //**ATENDIDA OPERATIVA INICIO

        //                ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                fila++;

        //                inicioGrupo = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaOperativa.ToString("dd/MM/yyyy hh:mm:ss");
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaOperativa.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                fila++;


        //                //DATOS GENERALES

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                //Styles
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



        //                //PIEZAS INICIO

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;


        //                ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
        //                ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                //Styles
        //                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;

        //                //Border
        //                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                fila++;


        //                if(listaPiezas.Count > 0) {

        //                    foreach(var item in listaPiezas) {

        //                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
        //                        ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;


        //                    }

        //                } else {

        //                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    //Border
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;

        //                }


        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                //PIEZAS FINAL


        //                //REPUESTOS INICIO

        //                filaInicio = fila;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //Styles
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;


        //                ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
        //                ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
        //                ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

        //                //Styles
        //                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;

        //                //Border
        //                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                fila++;


        //                if(listaRepuestos.Count > 0) {

        //                    foreach(var item in listaRepuestos) {
        //                        string estado = string.Empty;

        //                        if(item.Estado == 0) {
        //                            estado = "En Stock";
        //                        } else if(item.Estado == 1) {
        //                            estado = "Pedido";
        //                        } else if(item.Estado == 2) {
        //                            estado = "Aceptado";
        //                        } else if(item.Estado == 3) {
        //                            estado = "Rechazado";
        //                        } else {
        //                            estado = "Error";
        //                        }

        //                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                        ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                        ws.Cells[string.Format("G{0}", fila)].Value = estado;
        //                        ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;


        //                    }

        //                } else {

        //                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    //Border
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                }


        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;


        //                filaFinal = fila - 1;

        //                ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                ws.Row(fila).OutlineLevel = 1;
        //                ws.Row(fila).Collapsed = true;
        //                fila++;

        //                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                //REPUESTOS FINAL


        //                //**ATENDIDA OPERATIVA FIN


        //            } else if(maquina.CodEstadoProceso > 1) {



        //                if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count >= 0) {

        //                    //**ATENDIDA INOPERATIVA INICIO

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA INOPERATIVA";

        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;

        //                    inicioGrupo = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;


        //                    //DATOS GENERALES

        //                    filaInicio = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    //Styles
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    //Styles
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    //Styles
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    filaFinal = fila - 1;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



        //                    //PIEZAS INICIO

        //                    filaInicio = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    //Styles
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
        //                    ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                    ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                    //Styles
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;

        //                    //Border
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    fila++;


        //                    if(listaPiezas.Count > 0) {

        //                        foreach(var item in listaPiezas) {

        //                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
        //                            ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                            ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            //Styles
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;


        //                        }

        //                    } else {

        //                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        //Border
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;

        //                    }


        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    filaFinal = fila - 1;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    //PIEZAS FINAL


        //                    //REPUESTOS INICIO

        //                    filaInicio = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    //Styles
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
        //                    ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                    ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
        //                    ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

        //                    //Styles
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;

        //                    //Border
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    fila++;


        //                    if(listaRepuestos.Count > 0) {

        //                        foreach(var item in listaRepuestos) {

        //                            string estado = string.Empty;

        //                            if(item.Estado == 0) {
        //                                estado = "En Stock";
        //                            } else if(item.Estado == 1) {
        //                                estado = "Pedido";
        //                            } else if(item.Estado == 2) {
        //                                estado = "Aceptado";
        //                            } else if(item.Estado == 3) {
        //                                estado = "Rechazado";
        //                            } else {
        //                                estado = "Error";
        //                            }

        //                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                            ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                            ws.Cells[string.Format("G{0}", fila)].Value = estado;
        //                            ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            //Styles
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;


        //                        }

        //                    } else {

        //                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    }


        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    filaFinal = fila - 1;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    //REPUESTOS FINAL


        //                    //**ATENDIDA INOPERATIVA FIN
        //                }

        //                if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count > 0) {

        //                    //**ATENCION REVISADA INICIO

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENCION REVISADA";

        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;

        //                    inicioGrupo = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaSolicitado.ToString("dd/MM/yyyy hh:mm:ss");
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaSolicitado.ToString();
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;




        //                    //REPUESTOS INICIO

        //                    filaInicio = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    //Styles
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
        //                    ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                    ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
        //                    ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

        //                    //Styles
        //                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;

        //                    //Border
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    fila++;


        //                    if(listaRepuestos.Count > 0) {

        //                        foreach(var item in listaRepuestos) {

        //                            string estado = string.Empty;

        //                            if(item.Estado == 0) {
        //                                estado = "En Stock";
        //                            } else if(item.Estado == 1) {
        //                                estado = "Pedido";
        //                            } else if(item.Estado == 2) {
        //                                estado = "Aceptado";
        //                            } else if(item.Estado == 3) {
        //                                estado = "Rechazado";
        //                            } else {
        //                                estado = "Error";
        //                            }

        //                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                            ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                            ws.Cells[string.Format("G{0}", fila)].Value = estado;
        //                            ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            //Styles
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;


        //                        }

        //                    } else {

        //                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    }


        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    filaFinal = fila - 1;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    //REPUESTOS FINAL


        //                    //**ATENCION REVISADA FIN
        //                }

        //                if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count > 0) {

        //                    //**SOLICITUDES INICIO

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "SOLICITUDES";

        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;

        //                    inicioGrupo = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;




        //                    //TRASPASOS INICIO

        //                    filaInicio = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "TRASPASOS";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    //Styles
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    ws.Cells[string.Format("C{0}", fila)].Value = "Sala";
        //                    ws.Cells[string.Format("D{0}", fila)].Value = "Almacen Origen";
        //                    ws.Cells[string.Format("E{0}", fila)].Value = "Almacen Destino";
        //                    ws.Cells[string.Format("F{0}", fila)].Value = "Repuesto";
        //                    ws.Cells[string.Format("G{0}", fila)].Value = "Cantidad";
        //                    ws.Cells[string.Format("H{0}", fila)].Value = "Estado";
        //                    ws.Cells[string.Format("I{0}", fila)].Value = "Fecha";

        //                    //Styles
        //                    ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;

        //                    //Border
        //                    ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    fila++;


        //                    if(listaTraspasos.Count > 0) {

        //                        foreach(var item in listaTraspasos) {


        //                            ws.Cells[string.Format("C{0}", fila)].Value = item.NombreSala.ToString();
        //                            ws.Cells[string.Format("D{0}", fila)].Value = item.NombreAlmacenOrigen.ToString();
        //                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreAlmacenDestino.ToString();
        //                            ws.Cells[string.Format("F{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                            ws.Cells[string.Format("G{0}", fila)].Value = item.Cantidad.ToString();
        //                            ws.Cells[string.Format("H{0}", fila)].Value = item.Estado == 1 ? "Aceptado" : (item.Estado == 2 ? "Rechazado" : "Pendiente").ToString();
        //                            ws.Cells[string.Format("I{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            //Styles
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            //Border
        //                            ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;


        //                        }

        //                    } else {

        //                        ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron traspasos.";
        //                        ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("C{0}:I{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        //Border
        //                        ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    }


        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;


        //                    filaFinal = fila - 1;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Row(fila).OutlineLevel = 1;
        //                    ws.Row(fila).Collapsed = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                    //TRASPASOS FINAL


        //                    //**SOLICITUDES FIN
        //                }

        //                if(maquina.CodEstadoProceso == 2 && listaTraspasos.Count > 0) {

        //                    //**ATENDIDA OPERATIVA INICIO

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;

        //                    inicioGrupo = fila;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    fila++;

        //                    ws.Cells[string.Format("B{0}", fila)].Value = "MAQUINA ATENDIDA OPERATIVA";
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    ws.Cells[string.Format("B{0}", fila)].Style.Font.Size = 18;
        //                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                    fila++;


        //                    //**ATENDIDA OPERATIVA FIN
        //                }

        //            }



        //            //FOOTER

        //            ws.Cells["A:AZ"].AutoFitColumns();

        //            excelName = "HistoricoMaquinaInoperativa_" + fecha + ".xlsx";
        //            var memoryStream = new MemoryStream();
        //            excel.SaveAs(memoryStream);
        //            base64String = Convert.ToBase64String(memoryStream.ToArray());
        //            mensaje = "Descargando Archivo";
        //            respuesta = true;
        //        } else {
        //            mensaje = "No se Pudo generar Archivo";
        //        }

        //    } catch(Exception exp) {
        //        respuesta = false;
        //        mensaje = exp.Message + ", Llame Administrador";
        //    }
        //    return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        //}

        [HttpPost]
        public ActionResult HistoricoxMaquinaInoperativaDescargarExcelJson(int codMaquinaInoperativa) {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            MI_MaquinaInoperativaEntidad maquina = new MI_MaquinaInoperativaEntidad();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            //Nuevo Metodo Excel con Collapse
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("Reporte Historico Maquina Inoperativa");
            ws.Cells["B4"].Value = "Reporte Historico Maquina Inoperativa";
            ws.Cells["B4:J4"].Style.Font.Bold = true;
            ws.Cells["B4"].Style.Font.Size = 20;
            ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B4:J4"].Merge = true;
            ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            int fila = 7, inicioGrupo = 0, finGrupo = 0;

            try {


                //Data Maquina Inoperativa

                maquina = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(codMaquinaInoperativa);

                //Problemas Maquina Inoperativa

                var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(codMaquinaInoperativa);

                var listaProblemasNuevo = listaProblemas.Where(x => x.Estado == 2).ToList();
                listaProblemas = listaProblemas.Where(x => x.Estado == 1).ToList();

                if (maquina != null) {

                    int filaInicio = 0;
                    int filaFinal = 0;



                    if (maquina.CodEstadoProceso >= 1) {

                        //**CREADO INICIO

                        ws.Cells[string.Format("B{0}", fila)].Value = "CREADO";

                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaCreado.ToString("dd/MM/yyyy hh:mm:ss");
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioCreado.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        fila++;


                        //DATOS MAQUINA

                        filaInicio = fila;

                        ws.Cells[string.Format("B{0}", fila)].Value = "DATOS MAQUINA";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Ley: " + maquina.MaquinaLey.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Modelo: " + maquina.MaquinaModelo.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Linea: " + maquina.MaquinaLinea.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Sala: " + maquina.MaquinaSala.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Juego: " + maquina.MaquinaJuego.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Numero Serie: " + maquina.MaquinaNumeroSerie.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Propietario: " + maquina.MaquinaPropietario.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Ficha: " + maquina.MaquinaFicha.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Marca: " + maquina.MaquinaMarca.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Token: " + maquina.MaquinaToken.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        filaFinal = fila - 1;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //DATOS GENERALES

                        filaInicio = fila;

                        ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        string estadoInoperativa = maquina.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquina.CodEstadoInoperativa == 2 ? "Inoperativa" : "Inoperativa");
                        ws.Cells[string.Format("B{0}", fila)].Value = "Estado Inoperativa: " + estadoInoperativa;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        string prioridad = maquina.CodPrioridad == 1 ? "Urgente" : (maquina.CodPrioridad == 2 ? "Normal" : "Normal");
                        ws.Cells[string.Format("B{0}", fila)].Value = "Prioridad: " + prioridad;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Fecha Inoperativa: " + maquina.FechaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionCreado.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        filaFinal = fila - 1;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                        //PROBLEMAS INICIO

                        filaInicio = fila;

                        ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;


                        ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                        ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                        ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                        //Styles
                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;

                        //Border
                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        fila++;


                        if (listaProblemas.Count > 0) {

                            foreach (var item in listaProblemas) {

                                ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                fila++;


                            }

                        } else {

                            ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            //Border
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }


                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;


                        filaFinal = fila - 1;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //PROBLEMAS FINAL



                        //**CREADO FIN
                    }

                    if(maquina.CodEstadoProceso == 3) {
                        //**ATENDIDA INOPERATIVA INICIO

                        ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA INOPERATIVA";

                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        fila++;

                        inicioGrupo = fila;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        fila++;


                        //DATOS GENERALES

                        filaInicio = fila;

                        ws.Cells[string.Format("B{0}", fila)].Value = "DATOS ATENCION";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        //Styles
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        filaFinal = fila - 1;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                        //PROBLEMAS INICIO

                        filaInicio = fila;

                        ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;


                        ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                        ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                        ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                        //Styles
                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;

                        //Border
                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        fila++;


                        if(listaProblemasNuevo.Count > 0) {

                            foreach(var item in listaProblemasNuevo) {

                                ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                fila++;


                            }

                        } else {

                            ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            //Border
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }


                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;


                        filaFinal = fila - 1;

                        ws.Cells[string.Format("B{0}", fila)].Value = "";
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                        ws.Row(fila).OutlineLevel = 1;
                        ws.Row(fila).Collapsed = true;
                        fila++;

                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //PROBLEMAS FINAL


                        //**ATENDIDA INOPERATIVA FIN
                    }


                    if (maquina.CodEstadoProceso == 2) {

                        if(maquina.CodEstadoReparacion == 0) {

                            //**ATENDIDA OPERATIVA INICIO

                            ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            inicioGrupo = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaOperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaOperativa.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            fila++;


                            //DATOS GENERALES

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "DATOS ATENCION";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim()==""?"No tiene":maquina.OrdenCompra.ToString());
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                            //PROBLEMAS INICIO

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;


                            ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                            ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                            ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                            //Styles
                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;

                            //Border
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            fila++;


                            if(listaProblemasNuevo.Count > 0) {

                                foreach(var item in listaProblemasNuevo) {

                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                    ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                    ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;


                                }

                            } else {

                                ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }


                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;


                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //PROBLEMAS FINAL

                            //**ATENDIDA OPERATIVA FIN
                        } else {

                            //**ATENDIDA INOPERATIVA INICIO

                            ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA ATENCION";

                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            inicioGrupo = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            fila++;


                            //DATOS GENERALES

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                            //PROBLEMAS INICIO

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;


                            ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                            ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                            ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                            //Styles
                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;

                            //Border
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            fila++;


                            if(listaProblemasNuevo.Count > 0) {

                                foreach(var item in listaProblemasNuevo) {

                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                    ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                    ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;


                                }

                            } else {

                                ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }


                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;


                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //PROBLEMAS FINAL


                            //**ATENDIDA INOPERATIVA FIN

                            //**ATENDIDA OPERATIVA INICIO

                            ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            inicioGrupo = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            fila++;


                            //DATOS GENERALES

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES ATENCION";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                            //PROBLEMAS INICIO

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;


                            ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                            ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                            ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                            //Styles
                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;

                            //Border
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            fila++;


                            if(listaProblemasNuevo.Count > 0) {

                                foreach(var item in listaProblemasNuevo) {

                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                    ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                    ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;


                                }

                            } else {

                                ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }


                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;


                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //PROBLEMAS FINAL


                            //DATOS GENERALES REPARACION

                            filaInicio = fila;

                            ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES REPARACION";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencionNuevo.ToString();
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            filaFinal = fila - 1;

                            ws.Cells[string.Format("B{0}", fila)].Value = "";
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            fila++;

                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                            //**ATENDIDA OPERATIVA FIN
                        }




                    } 



                    //FOOTER

                    ws.Cells["A:AZ"].AutoFitColumns();

                    excelName = "HistoricoMaquinaInoperativa_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch (Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }

        [HttpPost]
        public ActionResult HistoricoListadoMaquinaInoperativaDescargarExcelJson() {

            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> listaMaquina = new List<MI_MaquinaInoperativaEntidad>();
            //Nuevo Metodo Excel con Collapse
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                listaMaquina = maquinaInoperativaBL.GetAllMaquinaInoperativaxUsuario(codUsuario);

                if(listaMaquina != null) {

                    var strElementos = String.Empty;
                    var strElementos_ = String.Empty;
                    var nombresala = new List<dynamic>();
                    var salasSeleccionadas = String.Empty;

                    var ws = excel.Workbook.Worksheets.Add("Reporte Historico Maquina Inoperativa ");
                    ws.Cells["B4"].Value = "Listado Reporte Historico Maquina Inoperativa ";
                    ws.Cells["B4:J4"].Style.Font.Bold = true;
                    ws.Cells["B4"].Style.Font.Size = 20;
                    ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B4:J4"].Merge = true;
                    ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    int fila = 7, inicioGrupo = 0, finGrupo = 0;

                    ws.Cells[string.Format("C{0}", fila)].Value = "ID";
                    ws.Cells[string.Format("D{0}", fila)].Value = "Código Maquina";
                    ws.Cells[string.Format("E{0}", fila)].Value = "Sala";
                    ws.Cells[string.Format("F{0}", fila)].Value = "Estado";
                    ws.Cells[string.Format("G{0}", fila)].Value = "Acción";

                    //Styles
                    ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Bold = true;
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    //Border
                    ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    fila++;


                    if(listaMaquina.Count > 0) {

                        foreach(var item in listaMaquina) {

                            string estado = string.Empty;

                            if(item.CodEstadoProceso == 1) {
                                estado = "Creado";
                            }
                            if(item.CodEstadoProceso == 2) {
                                estado = "Atendida Operativa";
                            }
                            if(item.CodEstadoProceso == 3) {
                                estado = "Atendida Inoperativa";
                            }
                            if(item.CodEstadoProceso == 4) {
                                estado = "En espera solicitud";
                            }
                            if(item.CodEstadoProceso == 5) {
                                estado = "Repuestos Agregados";
                            }

                            ws.Cells[string.Format("C{0}", fila)].Value = item.CodMaquinaInoperativa.ToString();
                            ws.Cells[string.Format("D{0}", fila)].Value = item.MaquinaLey.ToString();
                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreSala.ToString();
                            ws.Cells[string.Format("F{0}", fila)].Value = estado;
                            ws.Cells[string.Format("G{0}", fila)].Value = "Ver Maquina Inoperativa";

                            // Formula
                            ws.Cells[string.Format("G{0}", fila)].Hyperlink = new Uri($"#'{item.CodMaquinaInoperativa.ToString()}'!A1", UriKind.Relative);

                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            //Border
                            ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;


                        }

                    } else {

                        ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron maquinas inoperativas.";
                        ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("C{0}:G{0}", fila)].Merge = true;
                        fila++;

                        //Border
                        ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    //FOOTER

                    ws.Cells["A:AZ"].AutoFitColumns();


                } else {
                    respuesta = false;
                    mensaje = "Sin data";
                    return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
                }



                foreach(var maquinaActual in listaMaquina) {

                    var strElementos = String.Empty;
                    var strElementos_ = String.Empty;
                    var nombresala = new List<dynamic>();
                    var salasSeleccionadas = String.Empty;

                    var ws = excel.Workbook.Worksheets.Add(maquinaActual.CodMaquinaInoperativa.ToString());
                    ws.Cells["B4"].Value = "Reporte Historico Maquina Inoperativa " + maquinaActual.CodMaquinaInoperativa.ToString();
                    ws.Cells["B4:J4"].Style.Font.Bold = true;
                    ws.Cells["B4"].Style.Font.Size = 20;
                    ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B4:J4"].Merge = true;
                    ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    int fila = 7, inicioGrupo = 0, finGrupo = 0;

                    try {


                        //Data Maquina Inoperativa

                        MI_MaquinaInoperativaEntidad maquina = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(maquinaActual.CodMaquinaInoperativa);

                        //Problemas Maquina Inoperativa

                        var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
                        listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

                        var listaProblemasNuevo = listaProblemas.Where(x => x.Estado == 2).ToList();
                        listaProblemas = listaProblemas.Where(x => x.Estado == 1).ToList();

                        //Piezas Maquina Inoperativa

                        var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                        listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

                        //Repuestos Maquina Inoperativa

                        var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                        listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

                        //Traspasos Maquina Inoperativa

                        var listaTraspasos = new List<MI_TraspasoRepuestoAlmacenEntidad>();
                        listaTraspasos = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);


                        if(maquina != null) {

                            int filaInicio = 0;
                            int filaFinal = 0;



                            if(maquina.CodEstadoProceso >= 1) {

                                //**CREADO INICIO

                                ws.Cells[string.Format("B{0}", fila)].Value = "CREADO";

                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaCreado.ToString("dd/MM/yyyy hh:mm:ss");
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioCreado.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;


                                //DATOS MAQUINA

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS MAQUINA";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Ley: " + maquina.MaquinaLey.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Modelo: " + maquina.MaquinaModelo.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Linea: " + maquina.MaquinaLinea.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Sala: " + maquina.MaquinaSala.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Juego: " + maquina.MaquinaJuego.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Numero Serie: " + maquina.MaquinaNumeroSerie.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Propietario: " + maquina.MaquinaPropietario.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Ficha: " + maquina.MaquinaFicha.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Marca: " + maquina.MaquinaMarca.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Token: " + maquina.MaquinaToken.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //DATOS GENERALES

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                string estadoInoperativa = maquina.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquina.CodEstadoInoperativa == 2 ? "Inoperativa" : "Inoperativa");
                                ws.Cells[string.Format("B{0}", fila)].Value = "Estado Inoperativa: " + estadoInoperativa;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                string prioridad = maquina.CodEstadoInoperativa == 1 ? "Urgente" : (maquina.CodEstadoInoperativa == 2 ? "Normal" : "Normal");
                                ws.Cells[string.Format("B{0}", fila)].Value = "Prioridad: " + prioridad;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha Inoperativa: " + maquina.FechaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionCreado.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                //PROBLEMAS INICIO

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                                ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                //Styles
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                fila++;


                                if(listaProblemas.Count > 0) {

                                    foreach(var item in listaProblemas) {

                                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                        ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        //Styles
                                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        fila++;


                                    }

                                } else {

                                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                }


                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //PROBLEMAS FINAL


                                //**CREADO FIN
                            }




                            if(maquina.CodEstadoProceso == 2 && listaTraspasos.Count == 0) {

                                //**ATENDIDA OPERATIVA INICIO

                                ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                fila++;

                                inicioGrupo = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaOperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaOperativa.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;


                                //DATOS GENERALES

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                                //PIEZAS INICIO

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
                                ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
                                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                //Styles
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                fila++;


                                if(listaPiezas.Count > 0) {

                                    foreach(var item in listaPiezas) {

                                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
                                        ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
                                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        //Styles
                                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        fila++;


                                    }

                                } else {

                                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    //Border
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                }


                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //PIEZAS FINAL


                                //REPUESTOS INICIO

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
                                ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
                                ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
                                ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

                                //Styles
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                fila++;


                                if(listaRepuestos.Count > 0) {

                                    foreach(var item in listaRepuestos) {
                                        string estado = string.Empty;

                                        if(item.Estado == 0) {
                                            estado = "En Stock";
                                        } else if(item.Estado == 1) {
                                            estado = "Pedido";
                                        } else if(item.Estado == 2) {
                                            estado = "Aceptado";
                                        } else if(item.Estado == 3) {
                                            estado = "Rechazado";
                                        } else {
                                            estado = "Error";
                                        }

                                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
                                        ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
                                        ws.Cells[string.Format("G{0}", fila)].Value = estado;
                                        ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        //Styles
                                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        fila++;


                                    }

                                } else {

                                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                }


                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //REPUESTOS FINAL


                                //**ATENDIDA OPERATIVA FIN


                            } else if(maquina.CodEstadoProceso > 1) {



                                if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count >= 0) {

                                    //**ATENDIDA INOPERATIVA INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA INOPERATIVA";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;


                                    //DATOS GENERALES

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                                    //PIEZAS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaPiezas.Count > 0) {

                                        foreach(var item in listaPiezas) {

                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        //Border
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        fila++;

                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //PIEZAS FINAL


                                    //REPUESTOS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
                                    ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaRepuestos.Count > 0) {

                                        foreach(var item in listaRepuestos) {

                                            string estado = string.Empty;

                                            if(item.Estado == 0) {
                                                estado = "En Stock";
                                            } else if(item.Estado == 1) {
                                                estado = "Pedido";
                                            } else if(item.Estado == 2) {
                                                estado = "Aceptado";
                                            } else if(item.Estado == 3) {
                                                estado = "Rechazado";
                                            } else {
                                                estado = "Error";
                                            }

                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = estado;
                                            ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        fila++;

                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //REPUESTOS FINAL


                                    //**ATENDIDA INOPERATIVA FIN
                                }

                                if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count > 0) {

                                    //**ATENCION REVISADA INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENCION REVISADA";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaSolicitado.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaSolicitado.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;




                                    //REPUESTOS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
                                    ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaRepuestos.Count > 0) {

                                        foreach(var item in listaRepuestos) {

                                            string estado = string.Empty;

                                            if(item.Estado == 0) {
                                                estado = "En Stock";
                                            } else if(item.Estado == 1) {
                                                estado = "Pedido";
                                            } else if(item.Estado == 2) {
                                                estado = "Aceptado";
                                            } else if(item.Estado == 3) {
                                                estado = "Rechazado";
                                            } else {
                                                estado = "Error";
                                            }

                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = estado;
                                            ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        fila++;

                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //REPUESTOS FINAL


                                    //**ATENCION REVISADA FIN
                                }

                                if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count > 0) {

                                    //**SOLICITUDES INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "SOLICITUDES";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;




                                    //TRASPASOS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "TRASPASOS";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("C{0}", fila)].Value = "Sala";
                                    ws.Cells[string.Format("D{0}", fila)].Value = "Almacen Origen";
                                    ws.Cells[string.Format("E{0}", fila)].Value = "Almacen Destino";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Repuesto";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Cantidad";
                                    ws.Cells[string.Format("H{0}", fila)].Value = "Estado";
                                    ws.Cells[string.Format("I{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaTraspasos.Count > 0) {

                                        foreach(var item in listaTraspasos) {


                                            ws.Cells[string.Format("C{0}", fila)].Value = item.NombreSala.ToString();
                                            ws.Cells[string.Format("D{0}", fila)].Value = item.NombreAlmacenOrigen.ToString();
                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreAlmacenDestino.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.NombreRepuesto.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = item.Cantidad.ToString();
                                            ws.Cells[string.Format("H{0}", fila)].Value = item.Estado == 1 ? "Aceptado" : (item.Estado == 2 ? "Rechazado" : "Pendiente").ToString();
                                            ws.Cells[string.Format("I{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron traspasos.";
                                        ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("C{0}:I{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        fila++;

                                        //Border
                                        ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //TRASPASOS FINAL


                                    //**SOLICITUDES FIN
                                }

                                if(maquina.CodEstadoProceso == 2 && listaTraspasos.Count > 0) {

                                    //**ATENDIDA OPERATIVA INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "MAQUINA ATENDIDA OPERATIVA";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}", fila)].Style.Font.Size = 18;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    fila++;


                                    //**ATENDIDA OPERATIVA FIN
                                }

                            }



                            //FOOTER

                            ws.Cells["A:AZ"].AutoFitColumns();

                        } else {
                            mensaje = "No se Pudo generar Archivo";
                        }

                    } catch(Exception exp) {
                        respuesta = false;
                        mensaje = exp.Message + ", Llame Administrador";
                        return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
                    }


                }

                excelName = "HistoricoMaquinaInoperativa_" + fecha + ".xlsx";
                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
                mensaje = "Descargando Archivo";
                respuesta = true;
                return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
                return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
            }


        }


        //[HttpPost]
        //public ActionResult HistoricoListadoMaquinaInoperativaDescargarExcelxFechasJson(DateTime fechaIni, DateTime fechaFin) {

        //    string fecha = DateTime.Now.ToString("dd_MM_yyyy");
        //    string mensaje = string.Empty;
        //    string mensajeConsola = string.Empty;
        //    bool respuesta = false;
        //    string base64String = "";
        //    string excelName = string.Empty;
        //    List<MI_MaquinaInoperativaEntidad> listaMaquina = new List<MI_MaquinaInoperativaEntidad>();
        //    //Nuevo Metodo Excel con Collapse
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    ExcelPackage excel = new ExcelPackage();

        //    try {

        //        int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
        //        listaMaquina = maquinaInoperativaBL.GetAllMaquinaInoperativaxUsuarioxFechas(codUsuario, fechaIni, fechaFin);

        //        if(listaMaquina != null) {

        //            var strElementos = String.Empty;
        //            var strElementos_ = String.Empty;
        //            var nombresala = new List<dynamic>();
        //            var salasSeleccionadas = String.Empty;

        //            var ws = excel.Workbook.Worksheets.Add("Reporte Historico Maquina Inoperativa ");
        //            ws.Cells["B4"].Value = "Listado Reporte Historico Maquina Inoperativa ";
        //            ws.Cells["B4:J4"].Style.Font.Bold = true;
        //            ws.Cells["B4"].Style.Font.Size = 20;
        //            ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            ws.Cells["B4:J4"].Merge = true;
        //            ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            int fila = 7, inicioGrupo = 0, finGrupo = 0;

        //            ws.Cells[string.Format("C{0}", fila)].Value = "ID";
        //            ws.Cells[string.Format("D{0}", fila)].Value = "Código Maquina";
        //            ws.Cells[string.Format("E{0}", fila)].Value = "Sala";
        //            ws.Cells[string.Format("F{0}", fila)].Value = "Estado";
        //            ws.Cells[string.Format("G{0}", fila)].Value = "Acción";

        //            //Styles
        //            ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Bold = true;
        //            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //            //Border
        //            ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //            fila++;


        //            if(listaMaquina.Count > 0) {

        //                foreach(var item in listaMaquina) {

        //                    string estado = string.Empty;

        //                    if(item.CodEstadoProceso == 1) {
        //                        estado = "Creado";
        //                    }
        //                    if(item.CodEstadoProceso == 2) {
        //                        estado = "Atendida Operativa";
        //                    }
        //                    if(item.CodEstadoProceso == 3) {
        //                        estado = "Atendida Inoperativa";
        //                    }
        //                    if(item.CodEstadoProceso == 4) {
        //                        estado = "En espera solicitud";
        //                    }
        //                    if(item.CodEstadoProceso == 5) {
        //                        estado = "Repuestos Agregados";
        //                    }

        //                    ws.Cells[string.Format("C{0}", fila)].Value = item.CodMaquinaInoperativa.ToString();
        //                    ws.Cells[string.Format("D{0}", fila)].Value = item.MaquinaLey.ToString();
        //                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreSala.ToString();
        //                    ws.Cells[string.Format("F{0}", fila)].Value = estado;
        //                    ws.Cells[string.Format("G{0}", fila)].Value = "Ver Maquina Inoperativa";

        //                    // Formula
        //                    ws.Cells[string.Format("G{0}", fila)].Hyperlink = new Uri($"#'{item.CodMaquinaInoperativa.ToString()}'!A1", UriKind.Relative);

        //                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                    //Styles
        //                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                    //Border
        //                    ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    fila++;


        //                }

        //            } else {

        //                ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron maquinas inoperativas.";
        //                ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Bold = true;
        //                ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                ws.Cells[string.Format("C{0}:G{0}", fila)].Merge = true;
        //                fila++;

        //                //Border
        //                ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            }


        //            //FOOTER

        //            ws.Cells["A:AZ"].AutoFitColumns();


        //        } else {
        //            respuesta = false;
        //            mensaje = "Sin data";
        //            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
        //        }



        //        foreach(var maquinaActual in listaMaquina) {

        //            var strElementos = String.Empty;
        //            var strElementos_ = String.Empty;
        //            var nombresala = new List<dynamic>();
        //            var salasSeleccionadas = String.Empty;

        //            var ws = excel.Workbook.Worksheets.Add(maquinaActual.CodMaquinaInoperativa.ToString());
        //            ws.Cells["B4"].Value = "Reporte Historico Maquina Inoperativa " + maquinaActual.CodMaquinaInoperativa.ToString();
        //            ws.Cells["B4:J4"].Style.Font.Bold = true;
        //            ws.Cells["B4"].Style.Font.Size = 20;
        //            ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            ws.Cells["B4:J4"].Merge = true;
        //            ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //            int fila = 7, inicioGrupo = 0, finGrupo = 0;

        //            try {


        //                //Data Maquina Inoperativa

        //                MI_MaquinaInoperativaEntidad maquina = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(maquinaActual.CodMaquinaInoperativa);

        //                //Problemas Maquina Inoperativa

        //                var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
        //                listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

        //                //Piezas Maquina Inoperativa

        //                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
        //                listaPiezas = maquinaInoperativaPiezasBL.MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

        //                //Repuestos Maquina Inoperativa

        //                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
        //                listaRepuestos = maquinaInoperativaRepuestosBL.MaquinaInoperativaRepuestosListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

        //                //Traspasos Maquina Inoperativa

        //                var listaTraspasos = new List<MI_TraspasoRepuestoAlmacenEntidad>();
        //                listaTraspasos = traspasoRepuestoAlmacenBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);


        //                if(maquina != null) {

        //                    int filaInicio = 0;
        //                    int filaFinal = 0;



        //                    if(maquina.CodEstadoProceso >= 1) {

        //                        //**CREADO INICIO

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "CREADO";

        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaCreado.ToString("dd/MM/yyyy hh:mm:ss");
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioCreado.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        fila++;


        //                        //DATOS MAQUINA

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "DATOS MAQUINA";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Ley: " + maquina.MaquinaLey.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Modelo: " + maquina.MaquinaModelo.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Linea: " + maquina.MaquinaLinea.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Sala: " + maquina.MaquinaSala.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Juego: " + maquina.MaquinaJuego.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Numero Serie: " + maquina.MaquinaNumeroSerie.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Propietario: " + maquina.MaquinaPropietario.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Ficha: " + maquina.MaquinaFicha.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Marca: " + maquina.MaquinaMarca.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Token: " + maquina.MaquinaToken.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        //DATOS GENERALES

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        string estadoInoperativa = maquina.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquina.CodEstadoInoperativa == 2 ? "Inoperativa" : "Atendida en Sala");
        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Estado Inoperativa: " + estadoInoperativa;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        string prioridad = maquina.CodEstadoInoperativa == 1 ? "Baja" : (maquina.CodEstadoInoperativa == 2 ? "Media" : "Alta");
        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Prioridad: " + prioridad;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Fecha Inoperativa: " + maquina.FechaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoCreado.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionCreado.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


        //                        //PROBLEMAS INICIO

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;


        //                        ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
        //                        ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
        //                        ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                        //Styles
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;

        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        fila++;


        //                        if(listaProblemas.Count > 0) {

        //                            foreach(var item in listaProblemas) {

        //                                ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
        //                                ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
        //                                ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                //Styles
        //                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                //Border
        //                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                fila++;


        //                            }

        //                        } else {

        //                            ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        }


        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;


        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        //PROBLEMAS FINAL


        //                        //PIEZAS INICIO

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;


        //                        ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
        //                        ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                        ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                        //Styles
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;

        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        fila++;


        //                        if(listaPiezas.Count > 0) {

        //                            foreach(var item in listaPiezas) {

        //                                ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
        //                                ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                                ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                //Styles
        //                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                //Border
        //                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                fila++;


        //                            }

        //                        } else {

        //                            ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            //Border
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;

        //                        }


        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        //PIEZAS FINAL

        //                        //**CREADO FIN
        //                    }




        //                    if(maquina.CodEstadoProceso == 2 && listaTraspasos.Count == 0) {

        //                        //**ATENDIDA OPERATIVA INICIO

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        fila++;

        //                        inicioGrupo = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaOperativa.ToString("dd/MM/yyyy hh:mm:ss");
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaOperativa.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        fila++;


        //                        //DATOS GENERALES

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        //Styles
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



        //                        //PIEZAS INICIO

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;


        //                        ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
        //                        ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                        ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                        //Styles
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;

        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        fila++;


        //                        if(listaPiezas.Count > 0) {

        //                            foreach(var item in listaPiezas) {

        //                                ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
        //                                ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                                ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                //Styles
        //                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                //Border
        //                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                fila++;


        //                            }

        //                        } else {

        //                            ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            //Border
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;

        //                        }


        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        //PIEZAS FINAL


        //                        //REPUESTOS INICIO

        //                        filaInicio = fila;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        //Styles
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;


        //                        ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
        //                        ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                        ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
        //                        ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

        //                        //Styles
        //                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                        ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;

        //                        //Border
        //                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        fila++;


        //                        if(listaRepuestos.Count > 0) {

        //                            foreach(var item in listaRepuestos) {
        //                                string estado = string.Empty;

        //                                if(item.Estado == 0) {
        //                                    estado = "En Stock";
        //                                } else if(item.Estado == 1) {
        //                                    estado = "Pedido";
        //                                } else if(item.Estado == 2) {
        //                                    estado = "Aceptado";
        //                                } else if(item.Estado == 3) {
        //                                    estado = "Rechazado";
        //                                } else {
        //                                    estado = "Error";
        //                                }

        //                                ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                                ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                                ws.Cells[string.Format("G{0}", fila)].Value = estado;
        //                                ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                //Styles
        //                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                //Border
        //                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                fila++;


        //                            }

        //                        } else {

        //                            ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        }


        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;


        //                        filaFinal = fila - 1;

        //                        ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                        ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                        ws.Row(fila).OutlineLevel = 1;
        //                        ws.Row(fila).Collapsed = true;
        //                        fila++;

        //                        ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        //REPUESTOS FINAL


        //                        //**ATENDIDA OPERATIVA FIN


        //                    } else if(maquina.CodEstadoProceso > 1) {



        //                        if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count >= 0) {

        //                            //**ATENDIDA INOPERATIVA INICIO

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA INOPERATIVA";

        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;

        //                            inicioGrupo = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;


        //                            //DATOS GENERALES

        //                            filaInicio = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            //Styles
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            //Styles
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            //Styles
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            filaFinal = fila - 1;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



        //                            //PIEZAS INICIO

        //                            filaInicio = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "PIEZAS";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            //Styles
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            ws.Cells[string.Format("E{0}", fila)].Value = "Pieza";
        //                            ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                            ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

        //                            //Styles
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;

        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            fila++;


        //                            if(listaPiezas.Count > 0) {

        //                                foreach(var item in listaPiezas) {

        //                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombrePieza.ToString();
        //                                    ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                                    ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                    //Styles
        //                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                    ws.Row(fila).OutlineLevel = 1;
        //                                    ws.Row(fila).Collapsed = true;
        //                                    //Border
        //                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    fila++;


        //                                }

        //                            } else {

        //                                ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron piezas.";
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                //Border
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                fila++;

        //                            }


        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            filaFinal = fila - 1;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            //PIEZAS FINAL


        //                            //REPUESTOS INICIO

        //                            filaInicio = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            //Styles
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
        //                            ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                            ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
        //                            ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

        //                            //Styles
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;

        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            fila++;


        //                            if(listaRepuestos.Count > 0) {

        //                                foreach(var item in listaRepuestos) {

        //                                    string estado = string.Empty;

        //                                    if(item.Estado == 0) {
        //                                        estado = "En Stock";
        //                                    } else if(item.Estado == 1) {
        //                                        estado = "Pedido";
        //                                    } else if(item.Estado == 2) {
        //                                        estado = "Aceptado";
        //                                    } else if(item.Estado == 3) {
        //                                        estado = "Rechazado";
        //                                    } else {
        //                                        estado = "Error";
        //                                    }

        //                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                                    ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                                    ws.Cells[string.Format("G{0}", fila)].Value = estado;
        //                                    ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                    //Styles
        //                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                    ws.Row(fila).OutlineLevel = 1;
        //                                    ws.Row(fila).Collapsed = true;
        //                                    //Border
        //                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    fila++;


        //                                }

        //                            } else {

        //                                ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                fila++;

        //                                //Border
        //                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            }


        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            filaFinal = fila - 1;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            //REPUESTOS FINAL


        //                            //**ATENDIDA INOPERATIVA FIN
        //                        }

        //                        if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count > 0) {

        //                            //**ATENCION REVISADA INICIO

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "ATENCION REVISADA";

        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;

        //                            inicioGrupo = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaSolicitado.ToString("dd/MM/yyyy hh:mm:ss");
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaSolicitado.ToString();
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;




        //                            //REPUESTOS INICIO

        //                            filaInicio = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "REPUESTOS";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            //Styles
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            ws.Cells[string.Format("E{0}", fila)].Value = "Nombre";
        //                            ws.Cells[string.Format("F{0}", fila)].Value = "Cantidad";
        //                            ws.Cells[string.Format("G{0}", fila)].Value = "Estado";
        //                            ws.Cells[string.Format("H{0}", fila)].Value = "Fecha";

        //                            //Styles
        //                            ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;

        //                            //Border
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            fila++;


        //                            if(listaRepuestos.Count > 0) {

        //                                foreach(var item in listaRepuestos) {

        //                                    string estado = string.Empty;

        //                                    if(item.Estado == 0) {
        //                                        estado = "En Stock";
        //                                    } else if(item.Estado == 1) {
        //                                        estado = "Pedido";
        //                                    } else if(item.Estado == 2) {
        //                                        estado = "Aceptado";
        //                                    } else if(item.Estado == 3) {
        //                                        estado = "Rechazado";
        //                                    } else {
        //                                        estado = "Error";
        //                                    }

        //                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                                    ws.Cells[string.Format("F{0}", fila)].Value = item.Cantidad.ToString();
        //                                    ws.Cells[string.Format("G{0}", fila)].Value = estado;
        //                                    ws.Cells[string.Format("H{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                    //Styles
        //                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                    ws.Cells[string.Format("E{0}:H{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                    ws.Row(fila).OutlineLevel = 1;
        //                                    ws.Row(fila).Collapsed = true;
        //                                    //Border
        //                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    fila++;


        //                                }

        //                            } else {

        //                                ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                                ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                fila++;

        //                                //Border
        //                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            }


        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            filaFinal = fila - 1;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            //REPUESTOS FINAL


        //                            //**ATENCION REVISADA FIN
        //                        }

        //                        if(maquina.CodEstadoProceso <= 5 && listaTraspasos.Count > 0) {

        //                            //**SOLICITUDES INICIO

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "SOLICITUDES";

        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;

        //                            inicioGrupo = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;




        //                            //TRASPASOS INICIO

        //                            filaInicio = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "TRASPASOS";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            //Styles
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            ws.Cells[string.Format("C{0}", fila)].Value = "Sala";
        //                            ws.Cells[string.Format("D{0}", fila)].Value = "Almacen Origen";
        //                            ws.Cells[string.Format("E{0}", fila)].Value = "Almacen Destino";
        //                            ws.Cells[string.Format("F{0}", fila)].Value = "Repuesto";
        //                            ws.Cells[string.Format("G{0}", fila)].Value = "Cantidad";
        //                            ws.Cells[string.Format("H{0}", fila)].Value = "Estado";
        //                            ws.Cells[string.Format("I{0}", fila)].Value = "Fecha";

        //                            //Styles
        //                            ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;

        //                            //Border
        //                            ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            ws.Cells[string.Format("I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            fila++;


        //                            if(listaTraspasos.Count > 0) {

        //                                foreach(var item in listaTraspasos) {


        //                                    ws.Cells[string.Format("C{0}", fila)].Value = item.NombreSala.ToString();
        //                                    ws.Cells[string.Format("D{0}", fila)].Value = item.NombreAlmacenOrigen.ToString();
        //                                    ws.Cells[string.Format("E{0}", fila)].Value = item.NombreAlmacenDestino.ToString();
        //                                    ws.Cells[string.Format("F{0}", fila)].Value = item.NombreRepuesto.ToString();
        //                                    ws.Cells[string.Format("G{0}", fila)].Value = item.Cantidad.ToString();
        //                                    ws.Cells[string.Format("H{0}", fila)].Value = item.Estado == 1 ? "Aceptado" : (item.Estado == 2 ? "Rechazado" : "Pendiente").ToString();
        //                                    ws.Cells[string.Format("I{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
        //                                    //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
        //                                    //Styles
        //                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
        //                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                                    ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.Black);
        //                                    ws.Row(fila).OutlineLevel = 1;
        //                                    ws.Row(fila).Collapsed = true;
        //                                    //Border
        //                                    ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("H{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    ws.Cells[string.Format("I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                                    fila++;


        //                                }

        //                            } else {

        //                                ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron traspasos.";
        //                                ws.Cells[string.Format("C{0}:I{0}", fila)].Style.Font.Bold = true;
        //                                ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                                ws.Cells[string.Format("C{0}:I{0}", fila)].Merge = true;
        //                                ws.Row(fila).OutlineLevel = 1;
        //                                ws.Row(fila).Collapsed = true;
        //                                fila++;

        //                                //Border
        //                                ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            }


        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;


        //                            filaFinal = fila - 1;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Row(fila).OutlineLevel = 1;
        //                            ws.Row(fila).Collapsed = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                            //TRASPASOS FINAL


        //                            //**SOLICITUDES FIN
        //                        }

        //                        if(maquina.CodEstadoProceso == 2 && listaTraspasos.Count > 0) {

        //                            //**ATENDIDA OPERATIVA INICIO

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                            fila++;

        //                            inicioGrupo = fila;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            fila++;

        //                            ws.Cells[string.Format("B{0}", fila)].Value = "MAQUINA ATENDIDA OPERATIVA";
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
        //                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                            ws.Cells[string.Format("B{0}", fila)].Style.Font.Size = 18;
        //                            ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
        //                            fila++;


        //                            //**ATENDIDA OPERATIVA FIN
        //                        }

        //                    }



        //                    //FOOTER

        //                    ws.Cells["A:AZ"].AutoFitColumns();

        //                } else {
        //                    mensaje = "No se Pudo generar Archivo";
        //                }

        //            } catch(Exception exp) {
        //                respuesta = false;
        //                mensaje = exp.Message + ", Llame Administrador";
        //                return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
        //            }


        //        }

        //        excelName = "HistoricoMaquinaInoperativa_" + fecha + ".xlsx";
        //        var memoryStream = new MemoryStream();
        //        excel.SaveAs(memoryStream);
        //        base64String = Convert.ToBase64String(memoryStream.ToArray());
        //        mensaje = "Descargando Archivo";
        //        respuesta = true;
        //        return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        //    } catch(Exception exp) {
        //        respuesta = false;
        //        mensaje = exp.Message + ", Llame Administrador";
        //        return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
        //    }


        //}


        [HttpPost]
        public ActionResult HistoricoListadoMaquinaInoperativaDescargarExcelxFechasJson(DateTime fechaIni, DateTime fechaFin) {

            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> listaMaquina = new List<MI_MaquinaInoperativaEntidad>();
            //Nuevo Metodo Excel con Collapse
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                listaMaquina = maquinaInoperativaBL.GetAllMaquinaInoperativaxUsuarioxFechas(codUsuario, fechaIni, fechaFin);

                if(listaMaquina != null) {

                    var strElementos = String.Empty;
                    var strElementos_ = String.Empty;
                    var nombresala = new List<dynamic>();
                    var salasSeleccionadas = String.Empty;

                    var ws = excel.Workbook.Worksheets.Add("Reporte Historico Maquina Inoperativa ");
                    ws.Cells["B4"].Value = "Listado Reporte Historico Maquina Inoperativa ";
                    ws.Cells["B4:J4"].Style.Font.Bold = true;
                    ws.Cells["B4"].Style.Font.Size = 20;
                    ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B4:J4"].Merge = true;
                    ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    int fila = 7, inicioGrupo = 0, finGrupo = 0;

                    ws.Cells[string.Format("C{0}", fila)].Value = "ID";
                    ws.Cells[string.Format("D{0}", fila)].Value = "Código Maquina";
                    ws.Cells[string.Format("E{0}", fila)].Value = "Sala";
                    ws.Cells[string.Format("F{0}", fila)].Value = "Estado";
                    ws.Cells[string.Format("G{0}", fila)].Value = "Acción";

                    //Styles
                    ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Bold = true;
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    //Border
                    ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    fila++;


                    if(listaMaquina.Count > 0) {

                        foreach(var item in listaMaquina) {

                            string estado = string.Empty;

                            if(item.CodEstadoProceso == 1) {
                                estado = "Creado";
                            }
                            else if(item.CodEstadoProceso == 2 && item.CodEstadoReparacion == 0) {
                                estado = "Atendida Operativa";
                            }
                            else if(item.CodEstadoProceso == 3) {
                                estado = "Atendida Inoperativa";
                            }
                            else if(item.CodEstadoProceso == 2 && item.CodEstadoReparacion==1) {
                                estado = "Atendida Operativa - Reparada";
                            } else {
                                estado = "Desconocido";
                            }

                            ws.Cells[string.Format("C{0}", fila)].Value = item.CodMaquinaInoperativa.ToString();
                            ws.Cells[string.Format("D{0}", fila)].Value = item.MaquinaLey.ToString();
                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreSala.ToString();
                            ws.Cells[string.Format("F{0}", fila)].Value = estado;
                            ws.Cells[string.Format("G{0}", fila)].Value = "Ver Maquina Inoperativa";

                            // Formula
                            ws.Cells[string.Format("G{0}", fila)].Hyperlink = new Uri($"#'{item.CodMaquinaInoperativa.ToString()}'!A1", UriKind.Relative);

                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                            //Styles
                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            //Border
                            ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("D{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;


                        }

                    } else {

                        ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron maquinas inoperativas.";
                        ws.Cells[string.Format("C{0}:G{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("C{0}:G{0}", fila)].Merge = true;
                        fila++;

                        //Border
                        ws.Cells[string.Format("C{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }


                    //FOOTER

                    ws.Cells["A:AZ"].AutoFitColumns();


                } else {
                    respuesta = false;
                    mensaje = "Sin data";
                    return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
                }



                foreach(var maquinaActual in listaMaquina) {

                    var strElementos = String.Empty;
                    var strElementos_ = String.Empty;
                    var nombresala = new List<dynamic>();
                    var salasSeleccionadas = String.Empty;

                    var ws = excel.Workbook.Worksheets.Add(maquinaActual.CodMaquinaInoperativa.ToString());
                    ws.Cells["B4"].Value = "Reporte Historico Maquina Inoperativa " + maquinaActual.CodMaquinaInoperativa.ToString();
                    ws.Cells["B4:J4"].Style.Font.Bold = true;
                    ws.Cells["B4"].Style.Font.Size = 20;
                    ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B4:J4"].Merge = true;
                    ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    int fila = 7, inicioGrupo = 0, finGrupo = 0;

                    try {


                        //Data Maquina Inoperativa

                        var maquina = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(maquinaActual.CodMaquinaInoperativa);

                        //Problemas Maquina Inoperativa

                        var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
                        listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaActual.CodMaquinaInoperativa);

                        var listaProblemasNuevo = listaProblemas.Where(x => x.Estado == 2).ToList();
                        listaProblemas = listaProblemas.Where(x => x.Estado == 1).ToList();

                        if(maquina != null) {

                            int filaInicio = 0;
                            int filaFinal = 0;



                            if(maquina.CodEstadoProceso >= 1) {

                                //**CREADO INICIO

                                ws.Cells[string.Format("B{0}", fila)].Value = "CREADO";

                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaCreado.ToString("dd/MM/yyyy hh:mm:ss");
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioCreado.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;


                                //DATOS MAQUINA

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS MAQUINA";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Ley: " + maquina.MaquinaLey.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Modelo: " + maquina.MaquinaModelo.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Linea: " + maquina.MaquinaLinea.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Sala: " + maquina.MaquinaSala.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Juego: " + maquina.MaquinaJuego.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Numero Serie: " + maquina.MaquinaNumeroSerie.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Propietario: " + maquina.MaquinaPropietario.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Ficha: " + maquina.MaquinaFicha.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Marca: " + maquina.MaquinaMarca.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Token: " + maquina.MaquinaToken.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //DATOS GENERALES

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                string estadoInoperativa = maquina.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquina.CodEstadoInoperativa == 2 ? "Inoperativa" : "Inoperativa");
                                ws.Cells[string.Format("B{0}", fila)].Value = "Estado Inoperativa: " + estadoInoperativa;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                string prioridad = maquina.CodPrioridad == 1 ? "Urgente" : (maquina.CodPrioridad == 2 ? "Normal" : "Normal");
                                ws.Cells[string.Format("B{0}", fila)].Value = "Prioridad: " + prioridad;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha Inoperativa: " + maquina.FechaInoperativa.ToString("dd/MM/yyyy hh:mm:ss");
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionCreado.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                //PROBLEMAS INICIO

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                                ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                //Styles
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                fila++;


                                if(listaProblemas.Count > 0) {

                                    foreach(var item in listaProblemas) {

                                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                        ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        //Styles
                                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        fila++;


                                    }

                                } else {

                                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                }


                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //PROBLEMAS FINAL



                                //**CREADO FIN
                            }

                            if(maquina.CodEstadoProceso == 3) {
                                //**ATENDIDA INOPERATIVA INICIO

                                ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA INOPERATIVA";

                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                fila++;

                                inicioGrupo = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                fila++;


                                //DATOS GENERALES

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "DATOS ATENCION";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                //Styles
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                //PROBLEMAS INICIO

                                filaInicio = fila;

                                ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //Styles
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                                ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                                ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                //Styles
                                ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;

                                //Border
                                ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                fila++;


                                if(listaProblemasNuevo.Count > 0) {

                                    foreach(var item in listaProblemasNuevo) {

                                        ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                        ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                        ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                        //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        //Styles
                                        //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        fila++;


                                    }

                                } else {

                                    ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                }


                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;


                                filaFinal = fila - 1;

                                ws.Cells[string.Format("B{0}", fila)].Value = "";
                                ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                fila++;

                                ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                //PROBLEMAS FINAL


                                //**ATENDIDA INOPERATIVA FIN
                            }


                            if(maquina.CodEstadoProceso == 2) {

                                if(maquina.CodEstadoReparacion == 0) {

                                    //**ATENDIDA OPERATIVA INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaOperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaOperativa.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;


                                    //DATOS GENERALES

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "DATOS ATENCION";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                                    //PROBLEMAS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaProblemasNuevo.Count > 0) {

                                        foreach(var item in listaProblemasNuevo) {

                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        fila++;

                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //PROBLEMAS FINAL

                                    //**ATENDIDA OPERATIVA FIN
                                } else {

                                    //**ATENDIDA INOPERATIVA INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA ATENCION";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativa?.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativa.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;


                                    //DATOS GENERALES

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                    //PROBLEMAS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaProblemasNuevo.Count > 0) {

                                        foreach(var item in listaProblemasNuevo) {

                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        fila++;

                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //PROBLEMAS FINAL


                                    //**ATENDIDA INOPERATIVA FIN

                                    //**ATENDIDA OPERATIVA INICIO

                                    ws.Cells[string.Format("B{0}", fila)].Value = "ATENDIDA OPERATIVA";

                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    fila++;

                                    inicioGrupo = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Fecha: " + maquina.FechaAtendidaInoperativaAprobado.ToString("dd/MM/yyyy hh:mm:ss");
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Usuario: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    fila++;


                                    //DATOS GENERALES

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES ATENCION";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.TecnicoAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencion.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "IST: " + maquina.IST.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Orden de compra: " + (maquina.OrdenCompra.Trim() == "" ? "No tiene" : maquina.OrdenCompra.ToString());
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                                    //PROBLEMAS INICIO

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "PROBLEMAS REAL";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    ws.Cells[string.Format("E{0}", fila)].Value = "Problema";
                                    ws.Cells[string.Format("F{0}", fila)].Value = "Descripcion";
                                    ws.Cells[string.Format("G{0}", fila)].Value = "Fecha";

                                    //Styles
                                    ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.White);
                                    ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;

                                    //Border
                                    ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    fila++;


                                    if(listaProblemasNuevo.Count > 0) {

                                        foreach(var item in listaProblemasNuevo) {

                                            ws.Cells[string.Format("E{0}", fila)].Value = item.NombreProblema.ToString();
                                            ws.Cells[string.Format("F{0}", fila)].Value = item.DescripcionProblema.ToString();
                                            ws.Cells[string.Format("G{0}", fila)].Value = item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                            //ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                            //Styles
                                            //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                            ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                            ws.Row(fila).OutlineLevel = 1;
                                            ws.Row(fila).Collapsed = true;
                                            //Border
                                            ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("F{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            ws.Cells[string.Format("G{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            fila++;


                                        }

                                    } else {

                                        ws.Cells[string.Format("E{0}", fila)].Value = "No se encontraron problemas.";
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Style.Font.Bold = true;
                                        ws.Cells[string.Format("E{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[string.Format("E{0}:G{0}", fila)].Merge = true;
                                        ws.Row(fila).OutlineLevel = 1;
                                        ws.Row(fila).Collapsed = true;
                                        fila++;

                                        //Border
                                        ws.Cells[string.Format("E{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }


                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;


                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    //PROBLEMAS FINAL


                                    //DATOS GENERALES REPARACION

                                    filaInicio = fila;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "DATOS GENERALES REPARACION";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    //Styles
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Tecnico: " + maquina.NombreUsuarioAtendidaInoperativaAprobado.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "Observaciones: " + maquina.ObservacionAtencionNuevo.ToString();
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    //Styles
                                    //ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    filaFinal = fila - 1;

                                    ws.Cells[string.Format("B{0}", fila)].Value = "";
                                    ws.Cells[string.Format("B{0}:J{0}", fila)].Merge = true;
                                    ws.Row(fila).OutlineLevel = 1;
                                    ws.Row(fila).Collapsed = true;
                                    fila++;

                                    ws.Cells[string.Format("B{0}:J{1}", filaInicio, filaFinal)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                    //**ATENDIDA OPERATIVA FIN
                                }




                            }



                            //FOOTER

                            ws.Cells["A:AZ"].AutoFitColumns();

                        } else {
                            mensaje = "No se Pudo generar Archivo";
                        }

                    } catch(Exception exp) {
                        respuesta = false;
                        mensaje = exp.Message + ", Llame Administrador";
                        return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
                    }


                }

                excelName = "HistoricoMaquinaInoperativa_" + fecha + ".xlsx";
                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
                mensaje = "Descargando Archivo";
                respuesta = true;
                return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
                return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
            }


        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCorreosxSala(int codSala) {
            var errormensaje = "";
            var respuesta = false;
            var lista = new List<MI_SalaCorreosEntidad>();
            var lista1 = new List<MI_SalaCorreosEntidad>();
            var lista2 = new List<MI_SalaCorreosEntidad>();
            var lista3 = new List<MI_SalaCorreosEntidad>();
            var lista4 = new List<MI_SalaCorreosEntidad>();

            try {
                lista = salaCorreosBL.GetCorreosxSala(codSala);
                lista1 = lista.Where(x => x.CodTipo == 1).ToList();
                lista2 = lista.Where(x => x.CodTipo == 2).ToList();
                lista3 = lista.Where(x => x.CodTipo == 3).ToList();
                lista4 = lista.Where(x => x.CodTipo == 4).ToList();

                respuesta = true;
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), data1 = lista1.ToList(), data2 = lista2.ToList(), data3 = lista3.ToList(), data4 = lista4.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarUsuarioCorreos() {
            var errormensaje = "";
            var respuesta = false;
            var lista = new List<MI_SalaCorreosEntidad>();

            try {
                lista = salaCorreosBL.GetAllUsuarioCorreos();

                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AgregarCorreoSala(int codSala, int codUsuario, int codTipo) {
            var errormensaje = "Agregado correctamente";
            var respuesta = false;

            try {
                var lista = new List<MI_SalaCorreosEntidad>();
                lista = salaCorreosBL.GetCorreosxSala(codSala);

                var existe = lista.FirstOrDefault(x => x.CodUsuario == codUsuario && x.CodTipo == codTipo);

                if(existe != null) {

                    errormensaje = "Correo ya agregado.";
                } else {
                    respuesta = salaCorreosBL.AgregarCorreoSala(codSala, codUsuario, codTipo);
                }


            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EliminarCorreoSala(int codSalaCorreos) {
            var errormensaje = "Quitado correctamente";
            var respuesta = false;

            try {
                respuesta = salaCorreosBL.QuitarCorreoSala(codSalaCorreos);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult EnviarCorreoMaquinaInoperativa(int codMaquinaInoperativa,int codTipo, string uriAplicacion = "")
        {
            var errormensaje = "Correos enviados correctamente";
            var respuesta = false;
            var lista = new List<MI_CorreoEntidad>();
            var listaAll = new List<MI_CorreoEntidad>();
            var asunto = string.Empty;
            var mensaje = string.Empty;
            if (uriAplicacion == "")
            {
                uriAplicacion = "http://" + Request.Url.Authority + Request.ApplicationPath + "/";
            }

            string urlModelo = string.Empty;
            string titleModelo = string.Empty;
            string headerTextModelo = string.Empty;

            //Data Maquina Inoperativa
            MI_MaquinaInoperativaEntidad maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(codMaquinaInoperativa);

            try
            {
                switch (codTipo)
                {
                    case 1:
                        asunto = "Maquinas Inoperativas - Atender Maquina Inoperativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/AtenderMaquinaInoperativaCreada/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Atender Maquina Inoperativa";
                        headerTextModelo = "Se ha registrado la maquina " + maquinaInoperativa.MaquinaLey + " de la sala " + maquinaInoperativa.NombreSala + " como inoperativa, para más detalles revisarlo en el siguiente formulario:";
                        break;
                    case 2:
                        asunto = "Maquinas Inoperativas - Maquina Operativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Maquina Operativa";
                        headerTextModelo = "Se ha solucionado los problemas en la maquina " + maquinaInoperativa.MaquinaLey + " de la sala " + maquinaInoperativa.NombreSala + " y ya está operativa, para más detalles revisarlo en el siguiente formulario:";
                        break;
                    case 3:
                        asunto = "Maquinas Inoperativas - Reparar Maquina Inoperativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/AtenderSolicitudMaquinaInoperativaVista/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Reparar Maquina Inoperativa";
                        headerTextModelo = "Se requiere reparar la maquina " + maquinaInoperativa.MaquinaLey + " de la sala " + maquinaInoperativa.NombreSala + ", revisarlo en el siguiente formulario:";
                        break;
                    case 4:
                        asunto = "Maquinas Inoperativas - Maquina Reparada Operativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Reparacion Terminada";
                        headerTextModelo = "Se ha terminado de reparar la maquina " + maquinaInoperativa.MaquinaLey + " de la sala " + maquinaInoperativa.NombreSala + " y ya está operativa, revisarlo en el siguiente formulario:";
                        break;
                    default:
                        return Json(new { respuesta = false, mensaje = "Error codigo proceso." }, JsonRequestBehavior.AllowGet);
                }

                mensaje = ModeloCorreoLink(urlModelo, titleModelo, headerTextModelo);

                lista = maquinaInoperativaCorreosBL.GetCorreosxMaquina(codMaquinaInoperativa);
                lista = lista.Where(x => x.CodEstadoProceso == codTipo).ToList();
                List<string> listaMails = lista.Select(x => x.UsuarioMail).ToList();
                listaMails = listaMails.Distinct().ToList();
                respuesta = true;

                var listac = String.Join(",", listaMails);

                try
                {

                    if (listac.Trim() != "")
                    {

                        if (codTipo == 3)
                        {

                            //Problemas Maquina Inoperativa
                            var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
                            var listaProblemasAtencion = new List<MI_MaquinaInoperativaProblemasEntidad>();
                            listaProblemas = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa).Where(x => x.Estado == 1).ToList();
                            listaProblemasAtencion = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa).Where(x => x.Estado == 2).ToList();

                            var mensajeAdmin = string.Empty;
                            var listaAdmin = new List<MI_SalaCorreosEntidad>();

                            //PASA A REPARACION
                            mensajeAdmin = correoModeloInforme(maquinaInoperativa, listaProblemas, listaProblemasAtencion);
                            correoBL.EnviarCorreo(listac, asunto, mensajeAdmin, true);

                        }
                        else
                        {

                            correoBL.EnviarCorreo(listac, asunto, mensaje, true);

                        }

                        foreach (var item in lista)
                        {
                            bool updated = maquinaInoperativaCorreosBL.ActulizarCantEnviosCorreo(item.CodCorreo);

                            if (!updated)
                            {

                                respuesta = false;
                                errormensaje = "Error en el envio de correos" + ",Llame Administrador";
                                return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
                            }

                        }


                    }



                }
                catch (Exception exp)
                {

                    errormensaje = exp.Message + ",Llame Administrador";
                    respuesta = false;
                }

            }
            catch (Exception exp)
            {
                respuesta = false;
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ListarMaquinaInoperativaCreadoExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaCreado(codUsuario);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Listado Maquina Inoperativa - Creado");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Codigo Maquina";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Estado Inoperativa ";
                    workSheet.Cells[3, 6].Value = "Prioridad";
                    workSheet.Cells[3, 7].Value = "Fecha Creacion";
                    workSheet.Cells[3, 8].Value = "Usuario Creación";
                    workSheet.Cells[3, 9].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodMaquinaInoperativa;
                        workSheet.Cells[recordIndex, 3].Value = registro.MaquinaLey;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodEstadoInoperativa == 1 ? "Op. Problemas" : registro.CodEstadoInoperativa == 2 ? "Inoperativa" : "Inoperativa";
                        workSheet.Cells[recordIndex, 6].Value = registro.CodPrioridad == 1 ? "Urgente" : registro.CodPrioridad == 2 ? "Normal" : "Normal";
                        workSheet.Cells[recordIndex, 7].Value = registro.FechaCreado.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 8].Value = registro.NombreUsuarioCreado.ToString();
                        workSheet.Cells[recordIndex, 9].Value = "CREADO";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:I3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:I3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:I" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:I" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 40;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 40;
                    workSheet.Column(8).Width = 30;
                    workSheet.Column(9).Width = 30;
                    excelName = "ListadoMaquinaInoperativa_Creado_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }

        [HttpPost]
        public ActionResult ListarMaquinaInoperativaAtendidaInoperativaExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try {

                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaAtendidaInoperativa(codUsuario);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Listado Maquina Inoperativa - Creado");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Codigo Maquina";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Estado Inoperativa ";
                    workSheet.Cells[3, 6].Value = "Prioridad";
                    workSheet.Cells[3, 7].Value = "Fecha Atención";
                    workSheet.Cells[3, 8].Value = "Tecnico Atención";
                    workSheet.Cells[3, 9].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodMaquinaInoperativa;
                        workSheet.Cells[recordIndex, 3].Value = registro.MaquinaLey;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodEstadoInoperativa == 1 ? "Op. Problemas" : registro.CodEstadoInoperativa == 2 ? "Inoperativa" : "Inoperativa";
                        workSheet.Cells[recordIndex, 6].Value = registro.CodPrioridad == 1 ? "Urgente" : registro.CodPrioridad == 2 ? "Normal" : "Normal";
                        workSheet.Cells[recordIndex, 7].Value = registro.FechaAtendidaInoperativa?.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 8].Value = registro.NombreUsuarioAtendidaInoperativa.ToString();
                        workSheet.Cells[recordIndex, 9].Value = "ATENDIDA INOPERATIVA";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:I3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:I3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:I" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:I" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 40;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 40;
                    workSheet.Column(8).Width = 30;
                    workSheet.Column(9).Width = 30;
                    excelName = "ListadoMaquinaInoperativa_AtendidaInoperativa_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }

        [HttpPost]
        public ActionResult ListarMaquinaInoperativaAtendidaInoperativaSolicitudExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try {
                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaAtendidaInoperativaSolicitud(codUsuario);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Listado Maquina Inoperativa - Creado");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Codigo Maquina";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Estado Inoperativa ";
                    workSheet.Cells[3, 6].Value = "Prioridad";
                    workSheet.Cells[3, 7].Value = "Tecnico";
                    workSheet.Cells[3, 8].Value = "Fecha Creacion";
                    workSheet.Cells[3, 9].Value = "Usuario";
                    workSheet.Cells[3, 10].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodMaquinaInoperativa;
                        workSheet.Cells[recordIndex, 3].Value = registro.MaquinaLey;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodEstadoInoperativa == 1 ? "Op. Problemas" : registro.CodEstadoInoperativa == 2 ? "Inoperativa" : "Atendida en Sala";
                        workSheet.Cells[recordIndex, 6].Value = registro.CodPrioridad == 1 ? "Baja" : registro.CodPrioridad == 2 ? "Media" : "Alta";
                        workSheet.Cells[recordIndex, 7].Value = registro.TecnicoCreado.ToString();
                        workSheet.Cells[recordIndex, 8].Value = registro.FechaCreado.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 9].Value = registro.NombreUsuarioCreado.ToString();
                        workSheet.Cells[recordIndex, 10].Value = "EN ESPERA SOLICITUD";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:J3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:J3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:J3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:J3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:J3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:J" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:J" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 40;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 40;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 30;
                    excelName = "ListadoMaquinaInoperativa_Solicitudes_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }
        [HttpPost]
        public ActionResult ListarMaquinaInoperativaAtendidaOperativaExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try {
                int codUsuario = Convert.ToInt32(Session["UsuarioID"]);
                lista = maquinaInoperativaBL.GetAllMaquinaInoperativaAtendidaOperativa(codUsuario);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Listado Maquina Inoperativa - Creado");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Codigo Maquina";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Estado Inoperativa ";
                    workSheet.Cells[3, 6].Value = "Prioridad";
                    workSheet.Cells[3, 7].Value = "Tecnico";
                    workSheet.Cells[3, 8].Value = "Fecha Creacion";
                    workSheet.Cells[3, 9].Value = "Usuario";
                    workSheet.Cells[3, 10].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodMaquinaInoperativa;
                        workSheet.Cells[recordIndex, 3].Value = registro.MaquinaLey;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodEstadoInoperativa == 1 ? "Op. Problemas" : registro.CodEstadoInoperativa == 2 ? "Inoperativa" : "Atendida en Sala";
                        workSheet.Cells[recordIndex, 6].Value = registro.CodPrioridad == 1 ? "Baja" : registro.CodPrioridad == 2 ? "Media" : "Alta";
                        workSheet.Cells[recordIndex, 7].Value = registro.TecnicoCreado.ToString();
                        workSheet.Cells[recordIndex, 8].Value = registro.FechaCreado.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 9].Value = registro.NombreUsuarioCreado.ToString();
                        workSheet.Cells[recordIndex, 10].Value = "ATENDIDA OPERATIVA";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:J3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:J3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:J3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:J3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:J3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:J" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:J" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 40;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 40;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 30;
                    excelName = "ListadoMaquinaInoperativa_Solicitudes_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }



        [HttpPost]
        public JsonResult ReporteCategoriaProblemasListaJsonxFechas(DateTime fechaIni, DateTime fechaFin, int[] listaCategoriaProblema) {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_MaquinaInoperativaEntidad>();
            int cantElementos = (listaCategoriaProblema == null) ? 0 : listaCategoriaProblema.Length;
            var strElementos = String.Empty;

            try {

                if(cantElementos > 0) {
                    strElementos = " and mcp.CodCategoriaProblema IN (" + "'" + String.Join("','", listaCategoriaProblema) + "'" + ")";
                }
                lista = maquinaInoperativaBL.ReporteCategoriaProblemasListaJsonxFechas(fechaIni, fechaFin, strElementos);
                lista.Distinct();
                respuesta = true;
                if(lista.Count == 0) {

                    respuesta = false;
                    errormensaje = "No se encontraron registros.";
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ReporteCategoriaProblemasExcelJsonxFechas(DateTime fechaIni, DateTime fechaFin, int[] listaCategoriaProblema) {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            var lista = new List<MI_MaquinaInoperativaEntidad>();
            int cantElementos = (listaCategoriaProblema == null) ? 0 : listaCategoriaProblema.Length;
            var strElementos = String.Empty;

            try {

                if(cantElementos > 0) {
                    strElementos = " and mcp.CodCategoriaProblema IN (" + "'" + String.Join("','", listaCategoriaProblema) + "'" + ")";
                }

                lista = maquinaInoperativaBL.ReporteCategoriaProblemasListaJsonxFechas(fechaIni, fechaFin, strElementos);
                lista.Distinct();

                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("ReporteCategoriaProblemas");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "Nombre Categoria Problema";
                    workSheet.Cells[3, 3].Value = "Nombre Problema";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Maquina";
                    workSheet.Cells[3, 6].Value = "Modelo";
                    workSheet.Cells[3, 7].Value = "Linea";
                    workSheet.Cells[3, 8].Value = "Juego";
                    workSheet.Cells[3, 9].Value = "Numero Serie";
                    workSheet.Cells[3, 10].Value = "Propietario";
                    workSheet.Cells[3, 11].Value = "Ficha";
                    workSheet.Cells[3, 12].Value = "Marca";
                    workSheet.Cells[3, 13].Value = "Token";
                    workSheet.Cells[3, 14].Value = "Estado";
                    workSheet.Cells[3, 15].Value = "Fecha Reportado";
                    workSheet.Cells[3, 16].Value = "Fecha Resuelto";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.NombreCategoriaProblema.ToUpper();
                        workSheet.Cells[recordIndex, 3].Value = registro.NombreProblema.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreSala.ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.MaquinaLey.ToUpper();
                        workSheet.Cells[recordIndex, 6].Value = registro.MaquinaModelo.ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.MaquinaLinea.ToUpper();
                        workSheet.Cells[recordIndex, 8].Value = registro.MaquinaJuego.ToUpper();
                        workSheet.Cells[recordIndex, 9].Value = registro.MaquinaNumeroSerie.ToUpper();
                        workSheet.Cells[recordIndex, 10].Value = registro.MaquinaPropietario.ToUpper();
                        workSheet.Cells[recordIndex, 11].Value = registro.MaquinaFicha.ToUpper();
                        workSheet.Cells[recordIndex, 12].Value = registro.MaquinaMarca.ToUpper();
                        workSheet.Cells[recordIndex, 13].Value = registro.MaquinaToken.ToUpper();
                        workSheet.Cells[recordIndex, 14].Value = registro.CodEstadoProceso==1?"CREADO":registro.CodEstadoProceso==2?"OPERATIVA":"INOPERATIVA";
                        workSheet.Cells[recordIndex, 15].Value = registro.FechaCreado.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 16].Value = registro.CodEstadoProceso == 2 ? registro.CodEstadoReparacion==1?registro.FechaAtendidaInoperativaAprobado.ToString("dd-MM-yyyy hh:mm:ss tt"):registro.FechaAtendidaOperativa?.ToString("dd-MM-yyyy hh:mm:ss tt"):"No resuelto";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:P3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:P3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:P3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:P3"].Style.Font.Color.SetColor(Color.White);
                                        
                    workSheet.Cells["B3:P3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:P3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:P3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:P3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        
                    workSheet.Cells["B3:P3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:P3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:P3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:P3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:P" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:P" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    workSheet.Column(2).Width = 60;
                    workSheet.Column(3).Width = 60;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 60;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 60;
                    workSheet.Column(11).Width = 30;
                    workSheet.Column(12).Width = 60;
                    workSheet.Column(13).Width = 30;
                    workSheet.Column(14).Width = 30;
                    workSheet.Column(15).Width = 30;
                    workSheet.Column(16).Width = 30;
                    excelName = "ReporteCategoriaProblemas_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    respuesta = false;
                    mensaje = "No existe datos para generar el Excel.";
                }


            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult RevisarEstadoAlmacenes() {

            bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));

            return Json(new { data = estadoAlmacenes }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllComentariosxMaquina(int codMaquinaInoperativa) {

            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_ComentarioEntidad>();

            try {

                lista = comentarioBL.GetAllComentariosxMaquina(codMaquinaInoperativa);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }



            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AgregarComentario(int codMaquinaInoperativa, int estadoProceso, string texto, int[] listaEmpleados) {

            var errormensaje = "Insertado correctamente";
            bool respuesta = false;
            int correosEnviados = 0;

            try {

                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];

                if(listaEmpleados != null) {
                    if(listaEmpleados.Length > 0) {
                        correosEnviados = EnviarCorreoEmpleadosLink(listaEmpleados, codMaquinaInoperativa, estadoProceso);
                    }
                }


                MI_ComentarioEntidad comentario = new MI_ComentarioEntidad();

                comentario.Texto = texto;
                comentario.FechaRegistro = DateTime.Now;
                comentario.FechaModificacion = DateTime.Now;
                comentario.CodUsuario = usuario.UsuarioID;
                comentario.CodMaquinaInoperativa = codMaquinaInoperativa;
                comentario.EstadoProceso = estadoProceso;
                comentario.CorreoEnviado = correosEnviados;
                comentario.Estado = 1;

                int insertado = comentarioBL.ComentarioInsertarJson(comentario);
                respuesta = true;

                if(insertado == 0) {

                    respuesta = false;
                    errormensaje = "No se pudo insertar.";
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }



            return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarComentario(int codComentario) {

            var errormensaje = "Eliminado correctamente";
            bool respuesta = false;
            bool eliminado = false;

            try {

                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];

                MI_ComentarioEntidad comentario = comentarioBL.GetComentarioxCod(codComentario);

                if(usuario.UsuarioID == comentario.CodUsuario) {

                    eliminado = comentarioBL.ComentarioEliminarJson(codComentario);
                    respuesta = true;


                    if(!eliminado) {

                        respuesta = false;
                        errormensaje = "No se pudo eliminar.";
                    }

                } else {

                    respuesta = false;
                    errormensaje = "No puede eliminar mensajes de otros usuarios.";
                }



            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }



            return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        private int EnviarCorreoEmpleadosLink(int[] listaEmpleados, int codMaquinaInoperativa, int codEstadoProceso) {

            int respuesta = 0;
            List<string> listaCorreos = new List<string>();
            string asunto = string.Empty;
            string mensaje = string.Empty;

            string uriAplicacion = "http://" + Request.Url.Authority + Request.ApplicationPath +"/";

            try {
                int valueVista = codEstadoProceso;
                if (codEstadoProceso == 3)
                {
                    valueVista = 2;
                }
                
                if(codEstadoProceso == 2 )
                {
                    valueVista = 3;
                }

                asunto = "Maquinas Inoperativas - Nuevo Comentario";
                //string urlModelo = uriAplicacion + "MIMaquinaInoperativa/HistoricoMaquinaInoperativa/" + codMaquinaInoperativa + "/" + codEstadoProceso;
                string urlModelo = uriAplicacion + "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/" + codMaquinaInoperativa + "/" + valueVista;
                string titleModelo = "Nuevo Comentario";
                string headerTextModelo = "Nuevo comentario en el que ha sido etiquetado, revisarlo en el siguiente enlace:";
                mensaje = ModeloCorreoLink(urlModelo,titleModelo,headerTextModelo);

                List<SEG_EmpleadoEntidad> lista = empleadoBL.EmpleadoListarPorUsuariosJson();

                foreach(var item in listaEmpleados) {

                    SEG_EmpleadoEntidad empleado = lista.First(x => x.EmpleadoID == item);
                    if(empleado != null) {
                        listaCorreos.Add(empleado.MailJob);
                    }
                }

                listaCorreos = listaCorreos.Distinct().ToList();

                var listac = String.Join(",", listaCorreos);

                respuesta = 1;

                try {

                    if(listac.Trim() != "") {
                        correoBL.EnviarCorreo(listac, asunto, mensaje, true);
                        respuesta = 1;
                    }
                } catch(Exception exp) {

                    respuesta = 0;
                }


            } catch(Exception exp) {

                respuesta = 0;
            }

            return respuesta;
        }


        private string ModeloCorreoLink( string url = "",string title = "", string headerText = "", string footerText="", string logo = "https://i.pinimg.com/originals/ec/d9/c2/ecd9c2e8ed0dbbc96ac472a965e4afda.jpg") {

            string body = @"<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""center"" valign=""top"" style=""padding: 36px 24px;"">
              <a href=""#"" target=""_blank"" style=""display: inline-block;"">
                <img src="""+logo+@""" alt=""Logo"" border=""0"" width=""48"" style=""display: block; width: 48px; max-width: 48px; min-width: 48px;"">
              </a>
            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;"">
              <h1 style=""margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;"">"+title+@"</h1>
            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
              <p style=""margin: 0;"">"+headerText+@"</p>
            </td>
          </tr>
          <tr>
            <td align=""left"" bgcolor=""#ffffff"">
              <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr>
                  <td align=""center"" bgcolor=""#ffffff"" style=""padding: 12px;"">
                    <table border=""0"" cellpadding=""0"" cellspacing=""0"">
                      <tr>
                        <td align=""center"" bgcolor=""#1a82e2"" style=""border-radius: 6px;"">
                          <a href="""+url+ @""" target=""_blank"" style=""display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;"">Click Aqui</a>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
              <p style=""margin: 0;"">Si eso no funciona, copia y pega el siguiente enlace en tu navegador:</p>
              <p style=""margin: 0;""><a href=""" + url + @""" target=""_blank"">"+url+@"</a></p>
            </td>
          </tr>
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf"">
              <p style=""margin: 0;"">"+footerText+@"</p>
            </td>
          </tr>
        </table>
        <br>
        <br>
      </td>
    </tr>


  </table>";

            return body;

        }


        private string correoModeloInforme(MI_MaquinaInoperativaEntidad maquinaInoperativa, List<MI_MaquinaInoperativaProblemasEntidad> listaProblemas, List<MI_MaquinaInoperativaProblemasEntidad> listaProblemasAtencion) {

            string uriAplicacion = string.Empty;
            if (uriAplicacion == "")
            {
                uriAplicacion = "http://" + Request.Url.Authority + Request.ApplicationPath + "/";
            }
            string url = uriAplicacion + "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/" + maquinaInoperativa.CodMaquinaInoperativa;
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine(@"<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""margin:auto;font-family: 'Arial', sans-serif;color:#272727;width:700px"">");

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 20px; color: white;background-image: url(https://raw.githubusercontent.com/Derian95/images/master/Header-bg.png?token=GHSAT0AAAAAACOJUNXDBZK3L3G4QNEDB65MZOPQ6SQ);"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <h1 style=""margin: 0; font-size: 36px; font-weight: 700; letter-spacing: -1px; line-height: 60px;"">Maquinas inoperativas</h1>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 50px 20px; color: white;"" colspan=""3"">");
            htmlBuilder.AppendLine($@"            <h1 style=""margin: 0; font-size: 28px; font-weight: 700; letter-spacing: -1px; line-height: 60px;"">{maquinaInoperativa.NombreSala}</h1>");
            htmlBuilder.AppendLine($@"            <p style=""font-size: 14px; margin: 10px 0;"">Máquina: {maquinaInoperativa.MaquinaLey}</p>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 10px 20px; color: white;"" colspan=""3"">");
            htmlBuilder.AppendLine($@"            <p style=""font-size: 14px; margin: 10px 0;"">Se comunica que la máquina {maquinaInoperativa.MaquinaLey} será enviada a reparación con el técnico {maquinaInoperativa.TecnicoAtencion}, se adjuntan los datos de la atención con código de IST - {maquinaInoperativa.IST}</p>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"    <!-- Datos Generales -->");
            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 20px;"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <h2 style=""margin: 0; font-size: 24px; font-weight: 700;color:#4f5aba"">Datos Generales</h2>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            // Estado, Prioridad y Observaciones en una sola fila
            htmlBuilder.AppendLine(@"    <tr>");

            // Estado
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 20px; width: 33.33%;"" colspan=""1"" >");
            htmlBuilder.AppendLine(@"            <p style=""margin: 0; font-size: 20px; font-weight: 700;"">Estado</p>");
            if (maquinaInoperativa.CodEstadoInoperativa == 1)
            {
                htmlBuilder.AppendLine($@"            <p style=""margin: 10px 0; font-size: 18px;"">Op. con problemas</p>");
            }
            else
            {
                htmlBuilder.AppendLine($@"            <p style=""margin: 10px 0; font-size: 18px;"">Inoperativa</p>");
            }
            htmlBuilder.AppendLine(@"        </td>");

            // Prioridad
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 20px; width: 33.33%;"" colspan=""1"">");
            htmlBuilder.AppendLine(@"            <h3 style=""margin: 0; font-size: 20px; font-weight: 700;"">Prioridad</h3>");
            if (maquinaInoperativa.CodPrioridad == 1)
            {
                htmlBuilder.AppendLine($@"            <p style=""margin: 10px 0; font-size: 18px;"">Urgente</p>");
            }
            else
            {
                htmlBuilder.AppendLine($@"            <p style=""margin: 10px 0; font-size: 18px;"">Normal</p>");
            }
            htmlBuilder.AppendLine(@"        </td>");

            // Observaciones
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 20px; width: 33.33%;"" colspan=""1"" >");
            htmlBuilder.AppendLine(@"            <p style=""margin: 0; font-size: 20px; font-weight: 700;"">Observaciones</p>");
            htmlBuilder.AppendLine($@"            <p style=""margin: 10px 0; font-size: 18px;"">{maquinaInoperativa.ObservacionCreado}</p>");
            htmlBuilder.AppendLine(@"        </td>");

            htmlBuilder.AppendLine(@"    </tr>");
            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 0px; width: 33.33%;"" colspan=""3"" >");
            htmlBuilder.AppendLine(@"            <p style="" font-size: 20px; font-weight: 700;"">Lista de problemas</p>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");
            // Filas dinámicas para listaProblemas
            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""white"" style=""padding-bottom: 40px;padding-top:5px; text-align: center;"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <table border=""1"" cellpadding=""10"" cellspacing=""0"" width=""100%"" style=""border-color: rgb(243, 234, 255);"">");
            htmlBuilder.AppendLine(@"                <tr>");
            htmlBuilder.AppendLine(@"                    <td bgcolor=""#201f42"" style=""color:white"">Código</td>");
            htmlBuilder.AppendLine(@"                    <td bgcolor=""#201f42"" style=""color:white"">Nombre</td>");
            htmlBuilder.AppendLine(@"                    <td bgcolor=""#201f42"" style=""color:white"">Descripción</td>");
            htmlBuilder.AppendLine(@"                </tr>");

            foreach (var problema in listaProblemas)
            {
                htmlBuilder.AppendLine(@"                <tr>");
                htmlBuilder.AppendLine($@"                    <td>{problema.CodProblema}</td>");
                htmlBuilder.AppendLine($@"                    <td>{problema.NombreProblema}</td>");
                htmlBuilder.AppendLine($@"                    <td>{problema.DescripcionProblema}</td>");
                htmlBuilder.AppendLine(@"                </tr>");
            }

            htmlBuilder.AppendLine(@"            </table>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");


            htmlBuilder.AppendLine(@"    <!-- Datos Generales -->");
            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 20px;"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <h2 style=""margin: 0; font-size: 24px; font-weight: 700;color:#4f5aba"">Datos de Atención</h2>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 20px; width: 33.33%;"" colspan=""3"" >");
            htmlBuilder.AppendLine(@"            <p style=""margin: 0; font-size: 20px; font-weight: 700;"">Observaciones</p>");
            htmlBuilder.AppendLine($@"            <p style=""margin: 10px 0; font-size: 18px;"">{maquinaInoperativa.ObservacionAtencion}</p>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" style=""padding: 0px; width: 33.33%;"" colspan=""3"" >");
            htmlBuilder.AppendLine(@"            <p style="" font-size: 20px; font-weight: 700;"">Lista de problemas reales</p>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");
            // Filas dinámicas para listaProblemasAtencion
            htmlBuilder.AppendLine(@"    <!-- Filas dinámicas para listaProblemasAtencion -->");
            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""white"" style=""padding-bottom: 40px;padding-top:5px; text-align: center;"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <table border=""1"" cellpadding=""10"" cellspacing=""0"" width=""100%"" style=""border-color: rgb(243, 234, 255);"">");
            htmlBuilder.AppendLine(@"                <tr>");
            htmlBuilder.AppendLine(@"                    <td bgcolor=""#201f42"" style=""color:white"">Código</td>");
            htmlBuilder.AppendLine(@"                    <td bgcolor=""#201f42"" style=""color:white"">Nombre</td>");
            htmlBuilder.AppendLine(@"                    <td bgcolor=""#201f42"" style=""color:white"">Descripción</td>");
            htmlBuilder.AppendLine(@"                </tr>");


            foreach (var problemaAtencion in listaProblemasAtencion)
            {
                htmlBuilder.AppendLine(@"                <tr>");
                htmlBuilder.AppendLine($@"                    <td>{problemaAtencion.CodProblema}</td>");
                htmlBuilder.AppendLine($@"                    <td>{problemaAtencion.NombreProblema}</td>");
                htmlBuilder.AppendLine($@"                    <td>{problemaAtencion.DescripcionProblema}</td>");
                htmlBuilder.AppendLine(@"                </tr>");
            }

            htmlBuilder.AppendLine(@"            </table>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 20px 0; color: white;background-image: url(https://raw.githubusercontent.com/Derian95/images/master/Header-bg.png?token=GHSAT0AAAAAACOJUNXDBZK3L3G4QNEDB65MZOPQ6SQ);"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <h1 style=""margin: 0; font-size: 36px; font-weight: 700; letter-spacing: -1px; line-height: 60px;""></h1>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            if (maquinaInoperativa.OrdenCompra.Trim() != "")
            {

                htmlBuilder.AppendLine(@"    <tr>");
                htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 10px 20px; color: white;"" colspan=""3"">");
                htmlBuilder.AppendLine($@"            <p style=""font-size: 14px; margin: 10px 0;"">Se generó una orden de compra con el codigo:  {maquinaInoperativa.OrdenCompra}</p>");
                htmlBuilder.AppendLine(@"        </td>");
                htmlBuilder.AppendLine(@"    </tr>");
            }

            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 10px 20px; color: white;"" colspan=""3"">");
            htmlBuilder.AppendLine($@"            <p style=""font-size: 14px; margin: 10px 0;"">Para realizar un seguimiento sobre el estado, lo puede hacer en el siguiente link :  <a href ="" {url} "" target = ""_blank"" > {url} </a> </p>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");


            htmlBuilder.AppendLine(@"    <tr>");
            htmlBuilder.AppendLine(@"        <td align=""center"" bgcolor=""#201f42"" style=""padding: 20px; color: white;"" colspan=""3"">");
            htmlBuilder.AppendLine(@"            <h1 style=""margin: 0; font-size: 24px; font-weight: 700; letter-spacing: -1px; line-height: 60px;""></h1>");
            htmlBuilder.AppendLine(@"        </td>");
            htmlBuilder.AppendLine(@"    </tr>");

            htmlBuilder.AppendLine(@"</table>");

            // Ahora puedes usar htmlBuilder.ToString() en tu correo HTML
            string correoHTML = htmlBuilder.ToString();
            return correoHTML;
        }



        [HttpPost]
        public ActionResult AgregarOrdenCompra(MI_MaquinaInoperativaEntidad maquina){
            var errormensaje = "Error al agregar la orden de compra";
            var respuesta = false;

            try {

                respuesta = maquinaInoperativaBL.AgregarOrdenCompraMaquinaInoperativa(maquina);
                errormensaje = "Agregado correctamente";

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new {  respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


    }
}