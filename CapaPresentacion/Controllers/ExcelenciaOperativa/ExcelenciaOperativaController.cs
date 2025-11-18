using CapaDatos.Utilitarios;
using CapaEntidad;
using CapaEntidad.ExcelenciaOperativa;
using CapaNegocio;
using CapaNegocio.ExcelenciaOperativa;
using CapaPresentacion.Utilitarios;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ExcelenciaOperativa {
    [seguridad]
    public class ExcelenciaOperativaController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        ClaseError error = new ClaseError();
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        protected EO_FichaExcelenciaOperativaBL fichaExcelenciaOperativaBL = new EO_FichaExcelenciaOperativaBL();

        public ActionResult FichaVistaJOP() {
            ViewBag.isUpdate = 0;
            ViewBag.FichaId = 0;
            ViewBag.fecha = DateTime.Now.ToString("dd/MM/yyyy");

            return View("~/Views/ExcelenciaOperativa/FichaVistaJOP.cshtml");
        }

        public ActionResult FichaVistaGU() {
            ViewBag.isUpdate = 0;
            ViewBag.FichaId = 0;
            ViewBag.fecha = DateTime.Now.ToString("dd/MM/yyyy");

            return View("~/Views/ExcelenciaOperativa/FichaVistaGU.cshtml");
        }

        public ActionResult EditarFichaVistaJOP(long FichaId = 0) {
            EO_FichaExcelenciaOperativaEntidad ficha = fichaExcelenciaOperativaBL.GetOnlyFicha(FichaId);
            List<SalaEntidad> salas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"]));
            bool isAssignedRoom = salas.Any(item => item.CodSala == ficha.SalaId);

            if(!isAssignedRoom) {
                return View("~/Views/ExcelenciaOperativa/FichaMensaje.cshtml");
            }

            ViewBag.isUpdate = 1;
            ViewBag.FichaId = FichaId;
            ViewBag.fecha = DateTime.Now.ToString("dd/MM/yyyy");

            if(ficha.Tipo == 1) {
                return View("~/Views/ExcelenciaOperativa/FichaVistaJOP.cshtml");
            }

            return View("~/Views/ExcelenciaOperativa/FichaMensajeNoFound.cshtml");
        }

        public ActionResult EditarFichaVistaGU(long FichaId = 0) {
            EO_FichaExcelenciaOperativaEntidad ficha = fichaExcelenciaOperativaBL.GetOnlyFicha(FichaId);
            List<SalaEntidad> salas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"]));
            bool isAssignedRoom = salas.Any(item => item.CodSala == ficha.SalaId);

            if(!isAssignedRoom) {
                return View("~/Views/ExcelenciaOperativa/FichaMensaje.cshtml");
            }

            ViewBag.isUpdate = 1;
            ViewBag.FichaId = FichaId;
            ViewBag.fecha = DateTime.Now.ToString("dd/MM/yyyy");

            if(ficha.Tipo == 2) {
                return View("~/Views/ExcelenciaOperativa/FichaVistaGU.cshtml");
            }

            return View("~/Views/ExcelenciaOperativa/FichaMensajeNoFound.cshtml");
        }

        public ActionResult FichaReporteVista() {
            return View();
        }

        public ActionResult VistaReporteNominal() {
            return View("~/Views/ExcelenciaOperativa/VistaReporteNominal.cshtml");
        }

        public ActionResult VistaReporteNominalPuntuacion() {
            return View("~/Views/ExcelenciaOperativa/VistaReporteNominalPuntuacion.cshtml");
        }

        [HttpPost]
        public ActionResult FichaNuevoJson(EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa) {
            string mensaje = "No se ha podido registrar";
            string mensajeConsola = "";
            bool respuesta = false;

            try {
                fichaExcelenciaOperativa.UsuarioId = Convert.ToInt32(Session["UsuarioID"]);
                //fichaExcelenciaOperativa.Fecha = DateTime.Now;

                if(fichaExcelenciaOperativaBL.GuardarFichaExcelenciaOperativa(fichaExcelenciaOperativa)) {
                    mensaje = "Registrado Correctamente";
                    respuesta = true;
                }
            } catch(Exception exp) {
                mensaje = exp.Message + ", Llame Administrador";
                respuesta = false;
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult FichaEditarJson(EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa) {
            string mensaje = "No se ha podido modificar esta ficha";
            string mensajeConsola = "";
            bool respuesta = false;

            try {
                if(fichaExcelenciaOperativaBL.UpdateFichaExcelenciaOperativa(fichaExcelenciaOperativa)) {
                    mensaje = "Actualizado Correctamente";
                    respuesta = true;
                }
            } catch(Exception exp) {
                mensaje = exp.Message + ", Llame Administrador";
                respuesta = false;
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult FichaEOListarxTipoFechaJson(int[] tipo, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;

            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            listaSalas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"])).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
            var strElementosSala = String.Empty;
            var codSalaListatado = new List<int>();

            List<EO_FichaExcelenciaOperativaEntidad> listaFichas = new List<EO_FichaExcelenciaOperativaEntidad>();
            int cantElementos = (tipo == null) ? 0 : tipo.Length;
            var strElementos = String.Empty;

            string elementsIn = String.Empty;

            try {
                if(listaSalas.Count > 0) {
                    codSalaListatado = listaSalas.Select(x => x.CodSala).ToList();
                    strElementosSala = " ficha.SalaId in(" + "'" + String.Join("','", codSalaListatado) + "'" + ") and ";
                }

                if(cantElementos > 0) {
                    strElementos = " ficha.Tipo in(" + "'" + String.Join("','", tipo) + "'" + ") and ";
                }

                elementsIn = strElementos + strElementosSala;

                var fichaExperienciaOperativaTupla = fichaExcelenciaOperativaBL.FichaEOFiltroListarxTipoFechaJson(elementsIn, fechaini, fechafin);
                error = fichaExperienciaOperativaTupla.error;
                listaFichas = fichaExperienciaOperativaTupla.fichaExcelenciasOperativasLista;
                if(error.Key.Equals(string.Empty)) {
                    mensaje = "Listando";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar";
                }

            } catch(Exception exp) {
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaFichas.ToList(), respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult FichaEOIdObtenerJson(long id) {
            string mensaje = "No se pudo procesar datos";
            string mensajeConsola = "";
            bool respuesta = false;

            EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa = new EO_FichaExcelenciaOperativaEntidad();

            EO_FichaCategoriaBL fichaCategoriaBL = new EO_FichaCategoriaBL();
            EO_FichaItemBL fichaItemBL = new EO_FichaItemBL();

            List<EO_FichaCategoriaEntidad> listaCategoria = new List<EO_FichaCategoriaEntidad>();

            try {

                var fichaSintomatologicaTupla = fichaExcelenciaOperativaBL.FichaEOIdObtenerJson(id);
                error = fichaSintomatologicaTupla.error;

                fichaExcelenciaOperativa = fichaSintomatologicaTupla.fichaExcelenciaOperativa;

                foreach(EO_FichaCategoriaEntidad categoria in fichaCategoriaBL.FichaCategoriaIdFichaObtenerJson(fichaExcelenciaOperativa.FichaId)) {
                    List<EO_FichaItemEntidad> listaItem = new List<EO_FichaItemEntidad>();

                    foreach(EO_FichaItemEntidad item in fichaItemBL.FichaItemIdCategoriaObtenerJson(categoria.CategoriaId)) {
                        listaItem.Add(item);
                    }

                    categoria.Items = listaItem;

                    listaCategoria.Add(categoria);
                }

                fichaExcelenciaOperativa.Categorias = listaCategoria;

                if(error.Key.Equals(string.Empty)) {
                    mensaje = "Obteniendo Información de Registro Seleccionado";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = fichaExcelenciaOperativa, respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult ListarReporteNominal(int roomCode, int typeCode, DateTime startDate, DateTime endDate) {
            bool status = false;
            string message = "No se encontraron registros";

            List<EO_FichaExcelenciaOperativaEntidad> data = new List<EO_FichaExcelenciaOperativaEntidad>();

            try {
                string inFilters = string.Empty;
                string equalsRoom = string.Empty;
                string equalsType = string.Empty;

                if(roomCode > 0) {
                    equalsRoom = $" ficha.SalaId = {roomCode} AND ";
                }

                if(typeCode > 0) {
                    equalsType = $" ficha.Tipo = {typeCode} AND ";
                }

                inFilters = $"{equalsRoom} {equalsType}";

                List<EO_FichaExcelenciaOperativaEntidad> ficha = fichaExcelenciaOperativaBL.ListaFichaEO(inFilters, startDate, endDate, "ASC");

                if(ficha.Any()) {
                    status = true;
                    message = "Datos obtenidos";
                    data = ficha;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data
            });
        }

        [HttpPost]
        public ActionResult ListarReporteNominalPuntuacion(int roomCode, int typeCode, DateTime startDate, DateTime endDate) {
            bool status = false;
            string message = "No se encontraron registros";

            List<EO_FichaExcelenciaOperativaEntidad> data = new List<EO_FichaExcelenciaOperativaEntidad>();

            try {
                string inFilters = string.Empty;
                string equalsRoom = string.Empty;
                string equalsType = string.Empty;

                if(roomCode > 0) {
                    equalsRoom = $" ficha.SalaId = {roomCode} AND ";
                }

                if(typeCode > 0) {
                    equalsType = $" ficha.Tipo = {typeCode} AND ";
                }

                inFilters = $"{equalsRoom} {equalsType}";

                List<EO_FichaExcelenciaOperativaEntidad> ficha = fichaExcelenciaOperativaBL.ListaFichaEO(inFilters, startDate, endDate, "ASC");

                if(ficha.Any()) {
                    status = true;
                    message = "Datos obtenidos";
                    data = ficha;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data
            });
        }

        [HttpPost]
        public ActionResult ReporteFichaEODescargarExcelJson(int[] types, DateTime startDate, DateTime endDate) {
            bool status = false;
            string message = "No se encontraron registros";
            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;

            try {
                List<SalaEntidad> listaSalas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"]));

                string inFilters = string.Empty;
                string inSalas = string.Empty;
                string inTypes = string.Empty;

                if(listaSalas.Any()) {
                    List<int> roomCodes = listaSalas.Select(sala => sala.CodSala).ToList();
                    inSalas = $" ficha.SalaId IN ({string.Join(",", roomCodes)}) AND ";
                }

                if(types.Any()) {
                    inTypes = $" ficha.Tipo IN ({string.Join(",", types)}) AND ";
                }

                inFilters = $"{inSalas} {inTypes}";

                List<EO_FichaExcelenciaOperativaEntidad> ficha = fichaExcelenciaOperativaBL.ListaFichaEO(inFilters, startDate, endDate);

                if(ficha.Any()) {
                    dynamic parameters = new {
                        startDate,
                        endDate
                    };

                    MemoryStream memoryStream = ExcelenciaOPHelper.ExcelFisico(ficha, parameters);
                    fileName = $"Fichas Excelencia OP {startDate.ToString("dd-MM-yyyy")} al {endDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

                    status = true;
                    message = "Excel generado";
                    data = Convert.ToBase64String(memoryStream.ToArray());
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                fileName,
                data
            });
        }

        [HttpPost]
        public ActionResult DescargarExcelNominal(int roomCode, int typeCode, DateTime startDate, DateTime endDate) {
            bool status = false;
            string message = "No se encontraron registros";
            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;

            try {
                SalaEntidad sala = _salaBl.SalaListaIdJson(Convert.ToInt32(roomCode));

                if(sala == null) {
                    return Json(new {
                        status,
                        message = "No se encontro sala, por favor ingrese datos correctos"
                    });
                }

                string inFilters = string.Empty;
                string equalsRoom = string.Empty;
                string equalsType = string.Empty;

                if(roomCode > 0) {
                    equalsRoom = $" ficha.SalaId = {roomCode} AND ";
                }

                if(typeCode > 0) {
                    equalsType = $" ficha.Tipo = {typeCode} AND ";
                }

                inFilters = $"{equalsRoom} {equalsType}";

                List<EO_FichaExcelenciaOperativaEntidad> ficha = fichaExcelenciaOperativaBL.ListaFichaEO(inFilters, startDate, endDate, "ASC");

                if(ficha.Any()) {
                    string typeName = "";

                    if(typeCode == 1) {
                        typeName = "JOP";
                    }

                    if(typeCode == 2) {
                        typeName = "GU";
                    }

                    dynamic parameters = new {
                        roomCode,
                        roomName = sala.Nombre,
                        typeCode,
                        startDate,
                        endDate
                    };

                    MemoryStream memoryStream = ExcelenciaOPHelper.ExcelNominal(ficha, parameters);
                    fileName = $"Excelencia OP {typeName} Nominal - {sala.Nombre} {startDate.ToString("dd-MM-yyyy")} al {endDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

                    status = true;
                    message = "Excel generado";
                    data = Convert.ToBase64String(memoryStream.ToArray());
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                fileName,
                data
            });
        }

        [HttpPost]
        public ActionResult DescargarExcelNominalPuntuacion(int roomCode, int typeCode, DateTime startDate, DateTime endDate) {
            bool status = false;
            string message = "No se encontraron registros";
            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;

            try {
                SalaEntidad sala = _salaBl.SalaListaIdJson(Convert.ToInt32(roomCode));

                if(sala == null) {
                    return Json(new {
                        status,
                        message = "No se encontro sala, por favor ingrese datos correctos"
                    });
                }

                string inFilters = string.Empty;
                string equalsRoom = string.Empty;
                string equalsType = string.Empty;

                if(roomCode > 0) {
                    equalsRoom = $" ficha.SalaId = {roomCode} AND ";
                }

                if(typeCode > 0) {
                    equalsType = $" ficha.Tipo = {typeCode} AND ";
                }

                inFilters = $"{equalsRoom} {equalsType}";

                List<EO_FichaExcelenciaOperativaEntidad> ficha = fichaExcelenciaOperativaBL.ListaFichaEO(inFilters, startDate, endDate, "ASC");

                if(ficha.Any()) {
                    string typeName = "";

                    if(typeCode == 1) {
                        typeName = "JOP";
                    }

                    if(typeCode == 2) {
                        typeName = "GU";
                    }

                    dynamic arguments = new {
                        roomCode,
                        roomName = sala.Nombre,
                        typeCode,
                        startDate,
                        endDate
                    };

                    MemoryStream memoryStream = ExcelenciaOPHelper.ExcelNominalPuntuacion(ficha, arguments);
                    fileName = $"Excelencia OP {typeName} Nominal Puntuacion - {sala.Nombre} {startDate.ToString("dd-MM-yyyy")} al {endDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

                    status = true;
                    message = "Excel generado";
                    data = Convert.ToBase64String(memoryStream.ToArray());
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                fileName,
                data
            });
        }

        public ActionResult FichaEORespuestaPDFDescarga(string doc, bool todo = false) {
            var filename = doc + ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("GenerateRespuestaPDF_", new { doc = doc, todo = todo }) {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore --page-offset 0 --footer-center [page]/[toPage] --footer-font-size 8",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

            return actionPDF;
        }


        [seguridad(false)]
        public ActionResult GenerateRespuestaPDF_(string doc, bool todo) {
            EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa = new EO_FichaExcelenciaOperativaEntidad();

            EO_FichaCategoriaBL fichaCategoriaBL = new EO_FichaCategoriaBL();
            EO_FichaItemBL fichaItemBL = new EO_FichaItemBL();

            List<EO_FichaCategoriaEntidad> listaCategoria = new List<EO_FichaCategoriaEntidad>();

            try {
                var fichaExperienciaOperativaTupla = fichaExcelenciaOperativaBL.FichaEOIdObtenerJson(Convert.ToInt64(doc));
                error = fichaExperienciaOperativaTupla.error;

                fichaExcelenciaOperativa = fichaExperienciaOperativaTupla.fichaExcelenciaOperativa;

                foreach(EO_FichaCategoriaEntidad categoria in fichaCategoriaBL.FichaCategoriaIdFichaObtenerJson(fichaExcelenciaOperativa.FichaId)) {
                    List<EO_FichaItemEntidad> listaItem = new List<EO_FichaItemEntidad>();

                    foreach(EO_FichaItemEntidad item in fichaItemBL.FichaItemIdCategoriaObtenerJson(categoria.CategoriaId)) {
                        listaItem.Add(item);
                    }

                    categoria.Items = listaItem;

                    listaCategoria.Add(categoria);
                }

                fichaExcelenciaOperativa.Categorias = listaCategoria;

                if(error.Key.Equals(string.Empty)) {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.ficha = fichaExcelenciaOperativa;
                } else {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";

                    return View("~/Views/Home/Error.cshtml");
                }
            } catch(Exception exp) {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }

            return View("~/Views/ExcelenciaOperativa/FichaPDFVista.cshtml");
        }

        [HttpPost]
        public ActionResult GenerarFisicoFichaEOPdfJson(int[] tipo, DateTime fechaini, DateTime fechafin) {
            bool respuesta = false;
            ActionAsPdf actionPDF;
            DateTime fechaActual = DateTime.Now;
            string filename = fechaActual.Day + "_" + fechaActual.Month + "_" + fechaActual.Year + "_Fisico_Ficha_ExperienciaOperativa" + ".pdf";

            int cantElementos = (tipo == null) ? 0 : tipo.Length;
            var strElementos = String.Empty;

            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            listaSalas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"])).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
            var strElementosSala = String.Empty;
            var codSalaListatado = new List<int>();

            string elementsIn = String.Empty;

            if(listaSalas.Count > 0) {
                codSalaListatado = listaSalas.Select(x => x.CodSala).ToList();
                strElementosSala = " ficha.SalaId in(" + "'" + String.Join("','", codSalaListatado) + "'" + ") and ";
            }

            if(cantElementos > 0) {
                strElementos = " ficha.Tipo in(" + "'" + String.Join("','", tipo) + "'" + ") and ";
            }

            elementsIn = strElementos + strElementosSala;

            actionPDF = new ActionAsPdf("PdfFicha_EOMultiple", new { elementsIn, fechaini, fechafin }) {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore --page-offset 0 --footer-center [page]/[toPage] --footer-font-size 8",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);
            string file = Convert.ToBase64String(applicationPDFData);
            respuesta = true;

            return Json(new { data = file, filename, respuesta });
        }

        [seguridad(false)]
        public ActionResult PdfFicha_EOMultiple(string elementsIn, DateTime fechaini, DateTime fechafin) {
            List<EO_FichaExcelenciaOperativaEntidad> listaFichas = new List<EO_FichaExcelenciaOperativaEntidad>();

            try {
                listaFichas = fichaExcelenciaOperativaBL.ListaFichaEO(elementsIn, fechaini, fechafin);

                ViewBag.respuesta = true;
                ViewBag.data = listaFichas;
            } catch(Exception ex) {
                ViewBag.respuesta = false;
                ViewBag.Exception = ex.Message;
                ViewBag.data = listaFichas;
            }

            return View("~/Views/ExcelenciaOperativa/FichaPDFVistaMultiple.cshtml");
        }

        [HttpPost]
        public ActionResult EliminarExcelenciaOperativa(long id) {
            string mensaje = string.Empty;
            bool respuesta = false;

            try {
                if(fichaExcelenciaOperativaBL.EliminarExcelenciaOperativa(id)) {
                    respuesta = true;
                    mensaje = "Registro Eliminado";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new {
                mensaje,
                respuesta
            });
        }

        [HttpPost]
        public ActionResult DescargarExcel(string fichaId) {
            bool status = false;
            string message = string.Empty;
            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;

            if(string.IsNullOrEmpty(fichaId)) {
                return Json(new {
                    status,
                    message = "Ingrese identificador de la ficha",
                    fileName,
                    data
                });
            }

            MemoryStream memoryStream = new MemoryStream();

            try {

                EO_FichaExcelenciaOperativaEntidad ficha = fichaExcelenciaOperativaBL.FichaEOId(Convert.ToInt64(fichaId));

                List<SalaEntidad> salas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"]));
                bool isAssignedRoom = salas.Any(item => item.CodSala == ficha.SalaId);

                if(!isAssignedRoom) {
                    message = "La sala asignada en la ficha no corresponde con las salas asignadas al usuario";
                }

                if(ficha != null) {
                    if(ficha.Tipo == 1) {
                        memoryStream = ExcelenciaOPHelper.ExcelTypeJOP(ficha);
                        fileName = $"Excelencia OP JOP {ficha.Fecha.ToString("dd-MM-yyyy")}-{currentDate.ToString("HHmmss")}.{fileExtension}";
                    }

                    if(ficha.Tipo == 2) {
                        memoryStream = ExcelenciaOPHelper.ExcelTypeGU(ficha);
                        fileName = $"Excelencia OP GU {ficha.Fecha.ToString("dd-MM-yyyy")}-{currentDate.ToString("HHmmss")}.{fileExtension}";
                    }

                    status = true;
                    message = "Excel generado";
                    data = Convert.ToBase64String(memoryStream.ToArray());
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                fileName,
                data
            });
        }
    }
}