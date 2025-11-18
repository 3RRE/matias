using CapaEntidad;
using CapaEntidad.Excel;
using CapaEntidad.TransaccionTarjetaCliente.Dto;
using CapaEntidad.TransaccionTarjetaCliente.Entidad;
using CapaEntidad.TransaccionTarjetaCliente.Filtro;
using CapaNegocio;
using CapaNegocio.Excel;
using CapaNegocio.TransaccionTarjetaCliente;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.TransaccionTarjetaCliente {
    [seguridad(true)]
    public class TtcTransaccionesController : Controller {
        private readonly TTC_TransaccionBL transaccionBL;
        private readonly SalaMaestraBL salaMaestraBL;
        private readonly SalaBL salaBL;

        public TtcTransaccionesController() {
            transaccionBL = new TTC_TransaccionBL();
            salaMaestraBL = new SalaMaestraBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult TransaccionesClientesView() {
            return View("~/Views/TransaccionTarjetaCliente/Transacciones.cshtml");
        }
        #endregion

        #region Methods
        [seguridad(false)]
        [HttpPost]
        public async Task<JsonResult> ObtenerTransaccionesDeClientesPorSala(TTC_TransaccionFiltro filtros) {
            bool success = false;
            List<TTC_TransaccionDto> transaccionesCliente = new List<TTC_TransaccionDto>();
            string displayMessage = string.Empty;

            try {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(filtros.CodSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con código {filtros.CodSala}.";
                    return Json(new { success, displayMessage });
                }

                if(filtros.FechaInicio > filtros.FechaFin) {
                    success = false;
                    displayMessage = $"La fecha de inicio ({filtros.FechaInicio:dd/MM/yyyy}) no puede ser mayor a la fecha fin ({filtros.FechaFin:dd/MM/yyyy}).";
                    return Json(new { success, displayMessage });
                }

                transaccionesCliente = transaccionBL.ObtenerTransaccionesPorCodSala(filtros);
                success = transaccionesCliente.Count > 0;
                displayMessage = success ? $"Lista de transacciones de {filtros.FechaInicio:dd/MM/yyyy} al {filtros.FechaFin:dd/MM/yyyy}." : "No se encontraron transacciones de clientes con los filtros aplicados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }
            JsonResult jsonResult = Json(new { success, data = transaccionesCliente, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Methods Migration
        [HttpPost]
        [seguridad(false)]
        public JsonResult GuardarTransaccionesClientes(List<TTC_TransaccionEntidad> transacciones) {
            DateTime fechaMigracion = DateTime.Now.ToLocalTime();
            transacciones.ForEach(x => x.FechaMigracion = fechaMigracion);
            bool success = transaccionBL.GuardarTransaccionesMasivo(transacciones);
            string displayMessage = success ? $"{transacciones.Count} transacciones migradas." : "Error al migrar las transacciones con tarjeta de clientes.";
            JsonResult jsonResult = Json(new { success, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult ObtenerUltimoItemVoucherPorCodSala(int codSala) {
            int ultimoId = transaccionBL.ObtenerUltimoItemVoucherPorCodSala(codSala);
            bool success = ultimoId > 0;
            string displayMessage = success ? $"Valor del ultimo item voucher encontrado para la sala {codSala}: {ultimoId}." : $"Aún no hay registros de transacciones para la sala {codSala}.";
            JsonResult jsonResult = Json(new { success, data = ultimoId, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Methods Migration Moldat
        [HttpPost]
        [seguridad(false)]
        public JsonResult ObtenerTransaccionesParaMoldat(TTC_TransaccionMoldatFiltro filtro) {
            List<TTC_TransaccionEntidad> transacciones = new List<TTC_TransaccionEntidad>();
            List<int> ids = new List<int>();
            string displayMessage = string.Empty;
            bool success = false;
            try {
                transacciones = transaccionBL.ObtenerTransaccionesParaMoldat(filtro);
                success = transacciones.Count > 0;
                displayMessage = success ? $"{transacciones.Count} para migrar." : "No se encontraron registros de transacciones para migrar.";
            } catch(Exception ex) {
                displayMessage = $"Error al obtener las transacciones para migrar. {ex.Message}";
                success = false;
                transaccionBL.ActualizarEstadoMigracionesMoldat(ids, null);
            }

            JsonResult jsonResult = Json(new { success, displayMessage, data = transacciones });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult MarcarComoMigradas(List<int> ids, DateTime fechaMigracionMoldat) {
            string displayMessage = string.Empty;
            bool success = false;

            if(ids.Count == 0) {
                success = false;
                displayMessage = "Tiene que incluir al menos un id de transacción.";
                return Json(new { success, displayMessage });
            }

            try {
                transaccionBL.ActualizarEstadoMigracionesMoldat(ids, fechaMigracionMoldat);
                success = true;
                displayMessage = $"{ids.Count} transaccione(s) marcada(s) como migrado a moldat.";
            } catch(Exception ex) {
                displayMessage = $"Error al intentar revertir los estados de migración a moldat. {ex.Message}";
                success = false;
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult RevertirEstadoMigracion(List<int> ids) {
            string displayMessage = string.Empty;
            bool success = false;

            if(ids.Count == 0) {
                success = false;
                displayMessage = "Tiene que incluir al menos un id de transacción.";
                return Json(new { success, displayMessage });
            }

            try {
                transaccionBL.ActualizarEstadoMigracionesMoldat(ids, null);
                success = true;
                displayMessage = $"Estado de migración a moldat revertido correctamente.";
            } catch(Exception ex) {
                displayMessage = $"Error al intentar revertir los estados de migración a moldat. {ex.Message}";
                success = false;
            }

            return Json(new { success, displayMessage });
        }
        #endregion

        #region Excel
        public ActionResult GenerarExcelTransaccionesDeClientesPorSala(TTC_TransaccionFiltro filtros) {
            bool success = false;
            List<TTC_TransaccionDto> transaccionesCliente = new List<TTC_TransaccionDto>();
            string displayMessage = string.Empty;
            string fileName = string.Empty;
            string bytes = string.Empty;

            try {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(filtros.CodSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con código {filtros.CodSala}.";
                    return Json(new { success, displayMessage });
                }

                if(filtros.FechaInicio > filtros.FechaFin) {
                    success = false;
                    displayMessage = $"La fecha de inicio ({filtros.FechaInicio:dd/MM/yyyy}) no puede ser mayor a la fecha fin ({filtros.FechaFin:dd/MM/yyyy}).";
                    return Json(new { success, displayMessage });
                }
                transaccionesCliente = transaccionBL.ObtenerTransaccionesPorCodSala(filtros);
                success = transaccionesCliente.Count > 0;
                displayMessage = success ? $"Lista de transacciones de {filtros.FechaInicio:dd/MM/yyyy} al {filtros.FechaFin:dd/MM/yyyy}." : "No se encontraron transacciones de clientes con los filtros aplicados.";

                if(success) {
                    DateTime ahora = DateTime.Now.ToLocalTime();
                    SalaMaestraEntidad salaMaestra = salaMaestraBL.ObtenerSalaMaestraPorCodigoSala(filtros.CodSala);
                    DataTable dataTable = new DataTable();
                    ExcelHelper.AddColumnsTransaccionesClienteTarjeta(dataTable);
                    ExcelHelper.AddDataTransaccionesClienteTarjeta(dataTable, transaccionesCliente);
                    HojaExcelReporteAgrupado hojaTransacciones = new HojaExcelReporteAgrupado {
                        Data = dataTable,
                        Nombre = "Transacciones en Sala",
                        Color = "#28A745",
                        Titulo = "RESUMEN DE TRANSACCIONES EN SALA",
                        MetaData = new List<string> {
                            $"Sala: {salaMaestra.Nombre}",
                            $"Fecha: {ahora:dd/MM/yyyy}",
                            $"Hora: {ahora:HH:mm:ss}",
                            $"Cantidad de Registros: {transaccionesCliente.Count}",
                            $"Rango: {filtros.FechaInicio:dd/MM/yyyy} - {filtros.FechaFin:dd/MM/yyyy}"
                        }
                    };

                    List<HojaExcelReporteAgrupado> sheets = new List<HojaExcelReporteAgrupado> {
                        hojaTransacciones
                    };
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = ExcelHelper.GenerarExcel(sheets);

                    fileName = $"Transacciones de tarjetas de clientes de {salaMaestra.Nombre} desde {filtros.FechaInicio:dd-MM-yyyy} hasta {filtros.FechaFin:dd-MM-yyyy}.xlsx";
                    MemoryStream memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    bytes = Convert.ToBase64String(memoryStream.ToArray());
                    displayMessage = "Descargando archivo";
                }
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }

            object response = new {
                success,
                data = new {
                    fileName,
                    bytes
                },
                displayMessage
            };

            JsonResult jsonResult = Json(response);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    }
}