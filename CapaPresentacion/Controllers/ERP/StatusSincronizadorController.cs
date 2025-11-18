using CapaEntidad.Response;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using System.Linq;
using CapaEntidad.ERP;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class StatusSincronizadorController : Controller {

        private readonly SalaBL _salaBl = new SalaBL();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();

        public ActionResult StatusSincronizadorVista() {
            return View("~/Views/ERP/StatusSincronizadorVista.cshtml");
        }

        public async Task<ActionResult> GetStatusSincronizador(int salaId, DateTime fechaIni, DateTime fechaFin) {
            bool success = false;
            bool inVpn = false;
            string message = "No se ha encontrado registros";

            if(salaId <= 0) {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<StatusEnvio> data = new List<StatusEnvio>();

            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

                if(sala.CodSala == 0) {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                //sala.UrlProgresivo = "http://192.168.1.110:9895";

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/GetStatusSincronizador";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fechaIni = fechaIni.Date,
                        fechaFin = fechaFin.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                } else {


                    object arguments = new
                    {
                        fechaIni = fechaIni.Date,
                        fechaFin = fechaFin.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if(result.success) {
                    List<StatusEnvio> listaStatus = JsonConvert.DeserializeObject<List<StatusEnvio>>(result.data) ?? new List<StatusEnvio>();

                    if(listaStatus.Any()) {
                        data = listaStatus;

                        success = true;
                        message = "Registros obtenidos correctamente";
                    }
                } else {
                    success = false;
                    message = result.message;
                }
            } catch(Exception exception) {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        public async Task<ActionResult> UpdateStatusSincronizador(int salaId, DateTime fecha) {
            bool success = false;
            bool inVpn = false;
            string message = "No se ha encontrado registros";

            if(salaId <= 0) {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<StatusEnvio> data = new List<StatusEnvio>();

            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

                if(sala.CodSala == 0) {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                //sala.UrlProgresivo = "http://192.168.1.110:9895";

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/CierreCuadreTickets";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                } else {


                    object arguments = new
                    {
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if(result.success) {

                    success = true;
                    message = result.message;
                } else {
                    success = false;
                    message = result.message;
                }
            } catch(Exception exception) {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }


        public async Task<ActionResult> AbrirCierreStatusSincronizador(int salaId, DateTime fecha)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se ha encontrado registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<StatusEnvio> data = new List<StatusEnvio>();

            try
            {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

                if (sala.CodSala == 0)
                {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                //sala.UrlProgresivo = "http://192.168.1.110:9895";

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if (!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/AbrirCierreCuadreTickets";
                string content = string.Empty;
                string requestUri = string.Empty;

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                }
                else
                {


                    object arguments = new
                    {
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if (result.success)
                {

                    success = true;
                    message = result.message;
                }
                else
                {
                    success = false;
                    message = result.message;
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }


        public async Task<ActionResult> SendStatusSincronizador(int salaId, DateTime fecha) {
            bool success = false;
            bool inVpn = false;
            string message = "No se ha encontrado registros";

            if(salaId <= 0) {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<StatusEnvio> data = new List<StatusEnvio>();

            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

                if(sala.CodSala == 0) {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                //sala.UrlProgresivo = "http://192.168.1.110:9895";

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/CierreStatusSincronizador";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                } else {


                    object arguments = new
                    {
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if(result.success) {

                    success = true;
                    message = result.message;
                } else {
                    success = false;
                    message = result.message;
                }
            } catch(Exception exception) {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        public async Task<ActionResult> MatchStatusSincronizador(int salaId, DateTime fecha) {
            bool success = false;
            bool inVpn = false;
            string message = "No se ha encontrado registros";

            if(salaId <= 0) {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<StatusSincroTablaComparacion> data = new List<StatusSincroTablaComparacion>();

            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

                if(sala.CodSala == 0) {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                //sala.UrlProgresivo = "http://192.168.1.110:9895";

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/CompararCierreStatusSincronizador";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                } else {


                    object arguments = new
                    {
                        fecha = fecha.Date
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if(result.success) {
                    
                    List<StatusSincroTablaComparacion> listaMatchs = JsonConvert.DeserializeObject<List<StatusSincroTablaComparacion>>(result.data) ?? new List<StatusSincroTablaComparacion>();

                    if (listaMatchs.Any())
                    {
                        data = listaMatchs;

                        success = true;
                        message = "Registros obtenidos correctamente";
                    }
                    success = true;
                    message = result.message;
                } else {
                    success = false;
                    message = result.message;
                }
            } catch(Exception exception) {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        public ActionResult ListadoStatusSincronizadorExcelJson(List<StatusEnvio> lista,string nombresala) {


            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var salasSeleccionadas = String.Empty;
            try {

                List<StatusEnvio> data = new List<StatusEnvio>();
                data = lista;

                if(data.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add(nombresala);
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "FECHA OPERACION";
                    workSheet.Cells[3, 3].Value = "CONTADORES";
                    workSheet.Cells[3, 4].Value = "CUADRE TICKET";
                    workSheet.Cells[3, 5].Value = "CAJA";
                    workSheet.Cells[3, 6].Value = "ENVIADO";
                    workSheet.Cells[3, 7].Value = "FECHA CIERRE";

                    int recordIndex = 4;
                    int total = data.Count;
                    foreach(var registro in data) {
                        workSheet.Cells[recordIndex, 2].Value = registro.fechaoperacion.ToShortDateString();
                        workSheet.Cells[recordIndex, 3].Value = registro.contadores==1?"LISTO":"NO LISTO";
                        workSheet.Cells[recordIndex, 4].Value = registro.cuadreticket == 1 ? "LISTO" : "NO LISTO";
                        workSheet.Cells[recordIndex, 5].Value = registro.caja == 1 ? "LISTO" : "NO LISTO";
                        workSheet.Cells[recordIndex, 6].Value = registro.enviado == 1 ? "LISTO" : "NO LISTO";
                        workSheet.Cells[recordIndex, 7].Value = registro.fechacierre.ToLongDateString();
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:G3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:G3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:G3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:G3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:G3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:G" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:AH" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 7].AutoFilter = true;

                    workSheet.Column(2).Width = 30;
                    workSheet.Column(3).Width = 15;
                    workSheet.Column(4).Width = 15;
                    workSheet.Column(5).Width = 15;
                    workSheet.Column(6).Width = 15;
                    workSheet.Column(7).Width = 30;
                    excelName = "EstadoSincronizador_" + nombresala + "_" + fecha + ".xlsx";
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
        [seguridad(false)]
		public async Task<ActionResult> SendStatusSincronizadorTabla(int salaId, DateTime fecha, string tabla)
		{
			bool success = false;
			bool inVpn = false;
			string message = "No se ha encontrado registros";

			if(salaId <= 0)
			{
				return Json(new
				{
					success,
					message = "Por favor, seleccione una sala"
				});
			}

			List<StatusSincroTablaComparacion> data = new List<StatusSincroTablaComparacion>();

			try
			{
				SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

				if(sala.CodSala == 0)
				{
					return Json(new
					{
						success,
						message = "No se ha encontrado datos de la sala"
					});
				}

				//sala.UrlProgresivo = "http://192.168.1.110:9895";

				CheckPortHelper checkPortHelper = new CheckPortHelper();
				CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

				if(!tcpConnection.IsOpen)
				{
					return Json(new
					{
						success,
						message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
					});
				}

				string servicePath = "Servicio/UpdateDataTableAPIERPSlot";
				string content = string.Empty;
				string requestUri = string.Empty;

				if(tcpConnection.IsVpn)
				{
					inVpn = true;

					object arguments = new
					{
						ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
						fecha = fecha.Date,
						tabla
					};

					content = JsonConvert.SerializeObject(arguments);
					requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
				} else
				{


					object arguments = new
					{
						fecha = fecha.Date,
						tabla
					};

					content = JsonConvert.SerializeObject(arguments);
					requestUri = $"{sala.UrlProgresivo}/{servicePath}";
				}

				ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

				if(result.success)
				{
					List<StatusSincroTablaComparacion> listaMatchs = JsonConvert.DeserializeObject<List<StatusSincroTablaComparacion>>(result.data) ?? new List<StatusSincroTablaComparacion>();

					if(listaMatchs.Any())
					{
						data = listaMatchs;

						success = true;
						message = "Registros obtenidos correctamente";
					}
					success = true;
					message = result.message;
				} else
				{
					success = false;
					message = result.message;
				}
			} catch(Exception exception)
			{
				success = false;
				message = exception.Message;
			}

			return Json(new
			{
				success,
				message,
				data,
				inVpn
			});
		}


	}
}