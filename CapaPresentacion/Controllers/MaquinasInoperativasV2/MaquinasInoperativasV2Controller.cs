using CapaEntidad.MaquinasInoperativas;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio.MaquinasInoperativas;
using CapaPresentacion.Utilitarios;
using System.Windows.Documents;
using System.Text;

namespace CapaPresentacion.Controllers
{
    public class MaquinasInoperativasV2Controller : Controller
    {
        MI_MaquinaInoperativaPiezasBL maquinaInoperativaPiezasBL = new MI_MaquinaInoperativaPiezasBL();
        MI_MaquinaInoperativaProblemasBL maquinaInoperativaProblemasBL = new MI_MaquinaInoperativaProblemasBL();
        MI_MaquinaInoperativaBL maquinaInoperativaBL = new MI_MaquinaInoperativaBL();
        MI_MaquinaInoperativaRepuestosBL maquinaInoperativaRepuestosBL = new MI_MaquinaInoperativaRepuestosBL();
        MI_CorreoBL maquinaInoperativaCorreosBL = new MI_CorreoBL();
        Correo correoBL = new Correo();


        public ActionResult MaquinaInoperativaV2() {
            return View("~/Views/MaquinasInoperativasV2/RegistrarMaquinaInoperativa.cshtml");
        }

        public ActionResult ListaMaquinasInoperativasCreadas()
        {
            return View("~/Views/MaquinasInoperativasV2/ListadoMaquinasInoperativasCreadas.cshtml");
        }
        public ActionResult ListaMaquinasInoperativasSinResolver() {
            return View("~/Views/MaquinasInoperativasV2/ListadoMaquinaInoperativaSinResolver.cshtml");
        }
        public ActionResult ListadoSalaCorreos() {
            return View("~/Views/MaquinasInoperativasV2/ConfigurarCorreosMaquinaInoperativa.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativa() {
            return View("~/Views/MaquinasInoperativasV2/ListadoMaquinaInoperativaHistorico.cshtml");
        }
        public ActionResult ListadoMaquinaInoperativaHistoricoKPI() {
            return View("~/Views/MaquinasInoperativasV2/ListadoMaquinaInoperativaHistoricoKPI.cshtml");
        }
        public ActionResult DetalleHistoricoMaquinaInoperativa(int id = 0, int id2 = 0) {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            try {
                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodHistoricoObtenerJson(id);
              

                    //Problemas Maquina Inoperativa
                    var listaProblemasTodo = new List<MI_MaquinaInoperativaProblemasEntidad>();
                var listaProblemas = new List<MI_MaquinaInoperativaProblemasEntidad>();
                var listaProblemasNuevo = new List<MI_MaquinaInoperativaProblemasEntidad>();
                var listaCorreos = new List<MI_CorreoEntidad>();
                listaProblemasTodo = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);
                listaCorreos = maquinaInoperativaCorreosBL.GetCorreosxMaquina(maquinaInoperativa.CodMaquinaInoperativa);
                listaProblemas = listaProblemasTodo.Where(x => x.Estado == 1).ToList();
                listaProblemasNuevo = listaProblemasTodo.Where(x => x.Estado == 2).ToList();

                var listaPiezas = new List<MI_MaquinaInoperativaPiezasEntidad>();
                var listaRepuestos = new List<MI_MaquinaInoperativaRepuestosEntidad>();
                var listaTraspasos = new List<MI_TraspasoRepuestoAlmacenEntidad>();
                var listaCompras = new List<MI_TraspasoRepuestoAlmacenEntidad>();

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaProblemasNuevo = listaProblemasNuevo;
                ViewBag.listaPiezas = listaPiezas;
                ViewBag.listaRepuestos = listaRepuestos;
                ViewBag.listaTraspasos = listaTraspasos;
                ViewBag.listaCompras = listaCompras;
                ViewBag.listaCorreos = listaCorreos;
                ViewBag.estadoActual = id2.ToString();

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa.cshtml");
        }
        public ActionResult AtenderMaquinaInoperativaCreada(int id = 0)
        {
            string mensaje = "";
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try
            {

                //Data Maquina Inoperativa
                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(id);

                //Problemas Maquina Inoperativa
                var listaPro = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaPro = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                int[] listaProblemas = new int[listaPro.Count];
                List<int> listaCategoriaProblemas = new List<int>();
                int i = 0;
                foreach (var item in listaPro)
                {

                    if (i == 0)
                    {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    }
                    else
                    {
                        bool add = true;
                        foreach (var cod in listaCategoriaProblemas)
                        {
                            if (cod == item.CodCategoriaProblema)
                            {
                                add = false;
                                break;
                            }
                        }
                        if (add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }

             

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return View("~/Views/MaquinasInoperativasV2/AtenderMaquinaInoperativaCreada.cshtml");
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
                foreach (var item in listaPro) {

                    if (i == 0) {
                        listaCategoriaProblemas.Add(item.CodCategoriaProblema);
                    } else {
                        bool add = true;
                        foreach (var cod in listaCategoriaProblemas) {
                            if (cod == item.CodCategoriaProblema) {
                                add = false;
                                break;
                            }
                        }
                        if (add) { listaCategoriaProblemas.Add(item.CodCategoriaProblema); };
                    }

                    listaProblemas[i] = item.CodProblema;
                    i++;
                }



                //Problemas Maquina Inoperativa
                var listaProblemasTodo = new List<MI_MaquinaInoperativaProblemasEntidad>();
                var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                var listaProblemasArrayNuevo = new List<MI_MaquinaInoperativaProblemasEntidad>();
                listaProblemasTodo = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(maquinaInoperativa.CodMaquinaInoperativa);

                listaProblemasArray = listaProblemasTodo.Where(x => x.Estado == 1).ToList();
                listaProblemasArrayNuevo = listaProblemasTodo.Where(x => x.Estado == 2).ToList();

                ViewBag.maquinaInoperativa = maquinaInoperativa;
                ViewBag.listaProblemas = listaProblemas;
                ViewBag.listaCategoriaProblemas = listaCategoriaProblemas;
                ViewBag.listaProblemasArray = listaProblemasArray;
                ViewBag.listaProblemasArrayNuevo = listaProblemasArrayNuevo; 

            } catch (Exception ex) {
                mensaje = ex.Message;
            }

            return View("~/Views/MaquinasInoperativasV2/AtenderMaquinaInoperativaSinResolver.cshtml");
        }





        [HttpPost]
        public ActionResult MaquinaInoperativaAtenderSolucionadoJson(int codMaquinaInoperativa,int tipoAtencion,string observacionAtencionNuevo,string ist, int[] listaProblemasNuevo, int[] listaCorreos)
        {

            var errormensaje = "";
            bool respuesta = true;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();


            try
            {
                if(tipoAtencion == 1)
                {
                    maquinaInoperativa.FechaAtendidaOperativa = DateTime.Now;
                    maquinaInoperativa.CodUsuarioAtendidaOperativa = Convert.ToInt32(usuario.UsuarioID);
                    maquinaInoperativa.CodEstadoProceso = 2;
                    maquinaInoperativa.CodMaquinaInoperativa = codMaquinaInoperativa;
                    maquinaInoperativa.ObservacionAtencion = observacionAtencionNuevo;
                    maquinaInoperativa.IST = ist;
                    maquinaInoperativa.TecnicoAtencion = usuario.UsuarioNombre;
                    maquinaInoperativaBL.AtenderMaquinaInoperativaOperativaResuelto(maquinaInoperativa);

                }
                else if (tipoAtencion == 2)
                {
                    maquinaInoperativa.FechaAtendidaInoperativa = DateTime.Now;
                    maquinaInoperativa.CodUsuarioAtendidaInoperativa = Convert.ToInt32(usuario.UsuarioID);
                    maquinaInoperativa.CodEstadoProceso = 3;
                    maquinaInoperativa.CodMaquinaInoperativa = codMaquinaInoperativa;
                    maquinaInoperativa.ObservacionAtencion = observacionAtencionNuevo;
                    maquinaInoperativa.IST = ist;
                    maquinaInoperativa.TecnicoAtencion = usuario.UsuarioNombre;
                    maquinaInoperativaBL.AtenderMaquinaInoperativaOperativaSinResolver(maquinaInoperativa);

                }

                //AGREGAR MAQUINA INOPERATIVA PROBLEMAS NUEVO
                if(listaProblemasNuevo != null) {
                    foreach(var item in listaProblemasNuevo) {

                        int respuestaConsultaProblema = 0;
                        MI_MaquinaInoperativaProblemasEntidad maquinaInoperativaProblemasEntidad = new MI_MaquinaInoperativaProblemasEntidad();

                        maquinaInoperativaProblemasEntidad.CodMaquinaInoperativa = codMaquinaInoperativa;
                        maquinaInoperativaProblemasEntidad.CodProblema = item;
                        maquinaInoperativaProblemasEntidad.FechaRegistro = DateTime.Now;
                        maquinaInoperativaProblemasEntidad.FechaModificacion = DateTime.Now;
                        maquinaInoperativaProblemasEntidad.CodUsuario = usuario.UsuarioNombre;
                        maquinaInoperativaProblemasEntidad.Estado = 2;
                        respuestaConsultaProblema = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasInsertarJson(maquinaInoperativaProblemasEntidad);

                        if(respuestaConsultaProblema == 0) {

                            errormensaje = "Registro de Maquina Inoperativa Guardado Con errores en los Problemas , LLame Administrador";
                            respuesta = true;
                            break;
                        }
                    }
                }


                errormensaje = "Maquina Inoperativa Atendida correctamente.";

                //AGREGAR MAQUINA INOPERATIVA CORREOS
                if (listaCorreos != null) {
                    int contCorreo = 0;
                    foreach(var item in listaCorreos) {

                        MI_CorreoEntidad correo = new MI_CorreoEntidad();

                        correo.CodMaquinaInoperativa = codMaquinaInoperativa;
                        correo.CodUsuario = listaCorreos[contCorreo];
                        correo.CodEstadoProceso = tipoAtencion + 1;
                        bool respuestaConsultaCorreo = maquinaInoperativaCorreosBL.AgregarCorreo(correo);
                        contCorreo++;

                        if(!respuestaConsultaCorreo) {

                            errormensaje = "Registro de Correos error , LLame Administrador";
                            respuesta = false;
                            break;
                        }

                        respuesta = true;

                    }
                    if(respuesta)
                    {
                        EnviarCorreoMaquinaInoperativa(codMaquinaInoperativa, tipoAtencion+1);
                    }
                }


            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult AtenderMaquinaInoperativa(int codMaquinaInoperativa,int AtencionNuevo, string ObservacionesAtencionNuevo, List<MI_MaquinaInoperativaProblemasEntidad> listaProblemaRepuestos, int [] listaCorreos)
        {

            var errormensaje = "";
            bool respuesta = true;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();


            try
            {
                

                maquinaInoperativa.FechaAtendidaInoperativaAprobado = DateTime.Now;
                maquinaInoperativa.CodUsuarioAtendidaInoperativaAprobado = Convert.ToInt32(usuario.UsuarioID);
                maquinaInoperativa.CodEstadoProceso = 2;
                maquinaInoperativa.CodEstadoReparacion = AtencionNuevo;
                maquinaInoperativa.CodMaquinaInoperativa = codMaquinaInoperativa;
                maquinaInoperativa.ObservacionAtencionNuevo = ObservacionesAtencionNuevo;
                maquinaInoperativaBL.AtenderMaquinaInoperativaReparacion(maquinaInoperativa);
                if(listaProblemaRepuestos != null) {
                    foreach(var item in listaProblemaRepuestos) {

                        bool response = false;
                        MI_MaquinaInoperativaProblemasEntidad MI_problemas = new MI_MaquinaInoperativaProblemasEntidad();
                      
                        MI_problemas.CodMaquinaInoperativaProblemas = item.CodMaquinaInoperativaProblemas;
                        MI_problemas.FechaModificacion = DateTime.Now;
                        MI_problemas.CodUsuario = usuario.UsuarioNombre;
                        MI_problemas.Estado = 2;
                        MI_problemas.NombreRepuesto = item.NombreRepuesto;

                        response = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasEditarJson(MI_problemas);
                        if(!response) {

                            errormensaje = "Registro de Maquina Inoperativa Actualizado Con errores en los repuestos , LLame Administrador";
                            respuesta = true;
                            break;
                        }
                    }
                }


                errormensaje = "Maquina Inoperativa Atendida correctamente.";


                //AGREGAR MAQUINA INOPERATIVA CORREOS
                if(listaCorreos != null) {
                    int contCorreo = 0;
                    foreach(var item in listaCorreos) {

                        MI_CorreoEntidad correo = new MI_CorreoEntidad();

                        correo.CodMaquinaInoperativa = codMaquinaInoperativa;
                        correo.CodUsuario = listaCorreos[contCorreo];
                        correo.CodEstadoProceso = 4;
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
                        EnviarCorreoMaquinaInoperativa(codMaquinaInoperativa, 4);
                    }
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }



        [seguridad(false)]
        [HttpPost]
        private ActionResult EnviarCorreoMaquinaInoperativa(int codMaquinaInoperativa, int codTipo, string uriAplicacion = "") {
            var errormensaje = "Correos enviados correctamente";
            var respuesta = false;
            var lista = new List<MI_CorreoEntidad>();
            var listaAll = new List<MI_CorreoEntidad>();
            var asunto = string.Empty;
            var mensaje = string.Empty;
            if(uriAplicacion == "") {
                uriAplicacion = "http://" + Request.Url.Authority + Request.ApplicationPath + "/";
            }

            string urlModelo = string.Empty;
            string titleModelo = string.Empty;
            string headerTextModelo = string.Empty;

            //Data Maquina Inoperativa
            MI_MaquinaInoperativaEntidad maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(codMaquinaInoperativa);

            try {
                switch(codTipo) {
                    case 1:
                        asunto = "Maquinas Inoperativas - Atender Maquina Inoperativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/AtenderMaquinaInoperativaCreada/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Atender Maquina Inoperativa";
                        headerTextModelo = "Se ha registrado la maquina "+maquinaInoperativa.MaquinaLey+" de la sala "+ maquinaInoperativa.NombreSala+ " como inoperativa, para más detalles revisarlo en el siguiente formulario:";
                        break;
                    case 2:
                        asunto = "Maquinas Inoperativas - Maquina Operativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Maquina Operativa";
                        headerTextModelo = "Se ha solucionado los problemas en la maquina "+maquinaInoperativa.MaquinaLey+ " de la sala " + maquinaInoperativa.NombreSala + " y ya está operativa, para más detalles revisarlo en el siguiente formulario:";
                        break;
                    case 3:
                        asunto = "Maquinas Inoperativas - Reparar Maquina Inoperativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/AtenderSolicitudMaquinaInoperativaVista/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Reparar Maquina Inoperativa";
                        headerTextModelo = "Se requiere reparar la maquina "+maquinaInoperativa.MaquinaLey+ " de la sala " + maquinaInoperativa.NombreSala + ", revisarlo en el siguiente formulario:";
                        break;
                    case 4:
                        asunto = "Maquinas Inoperativas - Maquina Reparada Operativa - " + maquinaInoperativa.NombreSala;
                        urlModelo = uriAplicacion + "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/" + maquinaInoperativa.CodMaquinaInoperativa;
                        titleModelo = "Reparacion Terminada";
                        headerTextModelo = "Se ha terminado de reparar la maquina "+maquinaInoperativa.MaquinaLey+ " de la sala " + maquinaInoperativa.NombreSala + " y ya está operativa, revisarlo en el siguiente formulario:";
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

                try {

                    if(listac.Trim() != "") {

                        if(codTipo == 3) {

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

                        } else {

                            correoBL.EnviarCorreo(listac, asunto, mensaje, true);

                        }

                        foreach(var item in lista) {
                            bool updated = maquinaInoperativaCorreosBL.ActulizarCantEnviosCorreo(item.CodCorreo);

                            if(!updated) {

                                respuesta = false;
                                errormensaje = "Error en el envio de correos" + ",Llame Administrador";
                                return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
                            }

                        }


                    }



                } catch(Exception exp) {

                    errormensaje = exp.Message + ",Llame Administrador";
                    respuesta = false;
                }

            } catch(Exception exp) {
                respuesta = false;
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        private string ModeloCorreoLink(string url = "", string title = "", string headerText = "", string footerText = "", string logo = "https://i.pinimg.com/originals/ec/d9/c2/ecd9c2e8ed0dbbc96ac472a965e4afda.jpg") {

            string body = @"<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""center"" valign=""top"" style=""padding: 36px 24px;"">
              <a href=""#"" target=""_blank"" style=""display: inline-block;"">
                <img src=""" + logo + @""" alt=""Logo"" border=""0"" width=""48"" style=""display: block; width: 48px; max-width: 48px; min-width: 48px;"">
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
              <h1 style=""margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;"">" + title + @"</h1>
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
              <p style=""margin: 0;"">" + headerText + @"</p>
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
                          <a href=""" + url + @""" target=""_blank"" style=""display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;"">Click Aqui</a>
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
              <p style=""margin: 0;""><a href=""" + url + @""" target=""_blank"">" + url + @"</a></p>
            </td>
          </tr>
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf"">
              <p style=""margin: 0;"">" + footerText + @"</p>
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
        public ActionResult ReenviarCorreos(int codMaquinaInoperativa, int[] listaCorreos) {

            var errormensaje = "Correos no reenviados";
            bool respuesta = false;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            MI_MaquinaInoperativaEntidad maquinaInoperativa = new MI_MaquinaInoperativaEntidad();
            List<MI_CorreoEntidad> listaCorreosAnt = new List<MI_CorreoEntidad>();
            List<MI_CorreoEntidad> listaCorreosNew = new List<MI_CorreoEntidad>();


            try {

                maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(codMaquinaInoperativa);
                listaCorreosAnt = maquinaInoperativaCorreosBL.GetCorreosxMaquina(codMaquinaInoperativa);

                int estadoProceso = 0;

                //AGREGAR MAQUINA INOPERATIVA CORREOS
                if(listaCorreos != null) {

                    if(maquinaInoperativa.CodEstadoProceso == 2 && maquinaInoperativa.CodEstadoReparacion == 1) {
                        estadoProceso = 4;
                    } else {
                        estadoProceso = maquinaInoperativa.CodEstadoProceso;
                    }

                    var listaIndexCorreosAnt = listaCorreosAnt.Where(x=>x.CodEstadoProceso==estadoProceso).Select(x => x.CodUsuario).ToList();

                    var listaIndexCorreos = listaCorreos.ToList();

                    listaIndexCorreos.AddRange(listaIndexCorreosAnt);

                    var listaCorreosFinal = listaIndexCorreos.Distinct().ToList();


                    int contCorreo = 0;
                    foreach(var item in listaCorreosFinal) {


                        var existe = listaIndexCorreosAnt.Where(x => x == item).ToList();

                        if(existe.Count > 0) {
                            contCorreo++;
                        } else {

                            MI_CorreoEntidad correo = new MI_CorreoEntidad();

                            correo.CodMaquinaInoperativa = codMaquinaInoperativa;
                            correo.CodUsuario = listaCorreos[contCorreo];
                            correo.CodEstadoProceso = estadoProceso;
                            bool respuestaConsultaCorreo = maquinaInoperativaCorreosBL.AgregarCorreo(correo);
                            contCorreo++;

                            if(!respuestaConsultaCorreo) {

                                errormensaje = "Registro de Correos error , LLame Administrador";
                                respuesta = false;
                                break;
                            }

                            respuesta = true;
                        }


                    }

                    if(listaCorreosFinal.Count>0) {
                        EnviarCorreoMaquinaInoperativa(codMaquinaInoperativa, estadoProceso);
                        listaCorreosNew = maquinaInoperativaCorreosBL.GetCorreosxMaquina(maquinaInoperativa.CodMaquinaInoperativa);
                        errormensaje = "Correos reenviados";
                        respuesta = true;
                    }

                }


            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje,data=listaCorreosNew });
        }

    }
}