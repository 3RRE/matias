using CapaDatos.Sunat;
using CapaEntidad;
using CapaEntidad.EventosSignificativos;
using CapaEntidad.Sunat;
using CapaNegocio;
using CapaNegocio.EventosSignificativos;
using CapaNegocio.Sunat;
using CapaPresentacion.Utilitarios;
using S3k.Utilitario.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Migracion
{
    [seguridad]
    public class EventosSignificativosController : Controller
    {
        private readonly EventosSignificativosBL _eventosSignificativoBl;
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();
        private readonly string DATE_FORMAT;
        private SalaBL _salaBl = new SalaBL();

        public EventosSignificativosController() {
            _eventosSignificativoBl = new EventosSignificativosBL();
            DATE_FORMAT = "dd/MM/yyyy HH:mm:ss";

        }

        // GET: EventosSignificativos
        public ActionResult Index()
        {
            return View("~/Views/EventosSignificativos/EventosSignificativos.cshtml");
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult GuardarEventosSignificativos(List<EventosSignificativosEntidad> eventosSignificativos) {
            bool success = _eventosSignificativoBl.GuardarEventosSignificativos(eventosSignificativos);
            string displayMessage = success ? $"{eventosSignificativos.Count} eventos sunat migrados correctamente." : "Error al migrar los eventos sunat.";
            JsonResult jsonResult = Json(new { success, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        public JsonResult ObtenerEventosSignificativos(int codSala, DateTime fechaInicio, DateTime fechaFin) { 
            bool success = false;
            List<EventosSignificativosEntidad> eventosSignificativos = new List<EventosSignificativosEntidad>();
            string displayMessage;
            try {
                eventosSignificativos = _eventosSignificativoBl.ListarEventosSignificativos(codSala, fechaInicio, fechaFin);
                success = eventosSignificativos.Count > 0;
                displayMessage = success ? $"Lista del {fechaInicio.ToShortDateString()} al {fechaFin.ToShortDateString()} eventos significativos." : "No se encontraron registros para los eventos significativos";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }
            JsonResult jsonResult = Json(new { success, data = eventosSignificativos, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GenerarExcelEventosSignificativos(int codSala, DateTime fechaInicio, DateTime fechaFin) {
            SalaEntidad sala = _salaBl.SalaListaIdJson(codSala);
            string displayMessage;
            bool success;
            //cantidadDias = cantidadDias > 0 ? cantidadDias : 10;

            List<EventosSignificativosEntidad> eventosSunat = _eventosSignificativoBl.ListarEventosSignificativos(codSala, fechaInicio, fechaFin);
            success = eventosSunat.Count > 0;
            if(!success) {
                displayMessage = $"No se encontraron registros para los eventos significativos";
                return Json(new { success, displayMessage });
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id EventoSignificativo", typeof(string));
            dataTable.Columns.Add("Código Evento OL", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Hora", typeof(DateTime));
            dataTable.Columns.Add("Código Tarjeta", typeof(int));
            dataTable.Columns.Add("Código Maquina", typeof(int));
            dataTable.Columns.Add("Código Evento", typeof(int));
            dataTable.Columns.Add("Nombre del Evento", typeof(string));

            foreach(var eventoSunat in eventosSunat) {
                dataTable.Rows.Add(
                    eventoSunat.IdEventoSignificativo,
                    eventoSunat.Cod_Even_OL,
                    eventoSunat.Fecha.ToString(DATE_FORMAT),
                    eventoSunat.Hora.ToString(DATE_FORMAT),
                    eventoSunat.CodTarjeta,
                    eventoSunat.CodMaquina ,
                    eventoSunat.Cod_Evento,
                    eventoSunat.NombreEvento
                );
            }

            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Eventos Significativos",
                    SheetName = "Eventos Significativos",
                    Data = dataTable,
                    Title = $"Eventos Significativos {sala.Nombre}",
                    FirstColumNumber = false,
                };

                var excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archivo excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                var json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al Administrador.";
            }


            return Json(new { success, data = eventosSunat, displayMessage });
        }
    }

   
}