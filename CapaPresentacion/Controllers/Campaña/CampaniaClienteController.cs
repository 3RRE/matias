using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaEntidad.Response;
using CapaEntidad.WhatsApp;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaNegocio.WhatsApp;
using CapaPresentacion.Utilitarios;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using S3k.Utilitario.Constants;
using S3k.Utilitario.Excel;
using S3k.Utilitario.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Campaña {
    [seguridad]
    public class CampaniaClienteController : Controller {
        private readonly CMP_ClienteBL clientebl;
        private readonly CMP_CampañaBL campaniabl;
        private readonly AST_ClienteBL ast_ClienteBL;
        private readonly AST_TipoDocumentoBL ast_TipoDocumentoBL;
        private readonly SEG_PermisoRolBL segPermisoRolBL;
        private readonly SalaBL salaBL;
        private readonly int CodigoSalaSomosCasino;
        private WSP_MensajeriaUltraMsgBL wspMensajeriUltraMsgBL;

        public CampaniaClienteController() {
            clientebl = new CMP_ClienteBL();
            campaniabl = new CMP_CampañaBL();
            ast_ClienteBL = new AST_ClienteBL();
            ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
            segPermisoRolBL = new SEG_PermisoRolBL();
            salaBL = new SalaBL();
            CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);
        }

        // GET: CampaniaCliente
        [HttpPost]
        public ActionResult ClientesCampaniaIDObtenerJson(Int64 id) {
            var errormensaje = "";

            var campania = new List<CMP_ClienteEntidad>();
            try {
                campania = clientebl.GetClientesCampaniaJson(id);
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = campania, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ReporteCampaniasDescargarExcelJson(int campania_id) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CMP_ClienteEntidad> lista = new List<CMP_ClienteEntidad>();
            var campania = new CMP_CampañaEntidad();
            try {
                campania = campaniabl.CampañaIdObtenerJson(campania_id);
                lista = clientebl.GetClientesCampaniaJson(campania_id);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Clientes Campaña");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "Campaña : " + campania.nombre;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Cliente";
                    workSheet.Cells[3, 4].Value = "Nro. Documento";
                    workSheet.Cells[3, 5].Value = "Fecha Nac.";
                    // workSheet.Cells[3, 6].Value = "Usuario Reg.";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        string registroticket = string.Empty;
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                        workSheet.Cells[recordIndex, 2].Value = registro.id;
                        workSheet.Cells[recordIndex, 3].Value = registro.NombreCompleto;
                        workSheet.Cells[recordIndex, 4].Value = registro.NroDoc;

                        workSheet.Cells[recordIndex, 5].Value = registro.FechaNacimiento.ToString("dd-MM-yyyy");
                        ;
                        //workSheet.Cells[recordIndex, 6].Value = "";

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:E3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:E3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:E3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:E3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:E3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:E3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:E3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:E3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:E3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:E3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:E3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:E3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:E" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;

                    workSheet.Cells["B4:E" + total].Style.WrapText = true;

                    int filaFooter_ = total + 1;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 3].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 34;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 20;

                    excelName = "Campañas_Clientes.xlsx";
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
        public ActionResult GuardarClienteCampaniaJson(CMP_ClienteEntidad cliente) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            var campania = new CMP_CampañaEntidad();
            try {
                campania = campaniabl.CampañaIdObtenerJson(cliente.campania_id);
                DateTime hoy = DateTime.Now.Date;
                bool es = false;
                if(hoy >= campania.fechaini.Date && hoy <= campania.fechafin.Date) {
                    es = true;
                }
                if(!es) {
                    errormensaje = "No se puede Agregar fuera de las fechas de Campaña";
                    respuesta = false;
                    return Json(new { respuesta, mensaje = errormensaje });
                }
                cliente.fecha_reg = DateTime.Now;
                respuestaConsulta = clientebl.GuardarClienteCampaniaJson(cliente);

                if(respuestaConsulta > 0) {
                    respuesta = true;
                    errormensaje = "Registro Guardado Correctamente";
                } else {
                    errormensaje = "error al crear , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }


        [HttpPost]
        public ActionResult QuitarClienteCampaniaJson(int id) {
            var errormensaje = "";
            bool respuesta = false;
            CMP_ClienteEntidad ticketentidad = new CMP_ClienteEntidad();
            try {
                respuesta = clientebl.eliminarCampaniaClienteJson(id);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó  Correctamente";
                } else {
                    errormensaje = "error al Quitar , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }


        [HttpPost]
        public ActionResult ClientesCampaniaBusquedaObtenerJson(string valor) {
            var errormensaje = "";

            var campania = new List<CMP_ClienteEntidad>();
            try {
                campania = clientebl.GetClientesCampaniaBuscar(valor);
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            object objRespuesta = new {
                errormensaje = errormensaje,
                respuesta = campania
            };
            var result = new ContentResult {
                Content = serializer.Serialize(objRespuesta),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { respuesta = campania, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult CampaniaGuardarClienteJson(AST_ClienteEntidad cliente) {
            var errormensaje = "";
            int respuestaConsulta = 0;
            bool respuesta = false;
            AST_ClienteEntidad clienteregistro = new AST_ClienteEntidad();
            try {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);



                var existe = ast_ClienteBL.GetListaClientesxNroDoc(cliente.NroDoc);
                if(existe.Count() > 0) {
                    errormensaje = "Cliente ya Registrado";
                    return Json(new { respuesta, mensaje = errormensaje });
                }
                cliente.NombreCompleto = cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat;
                cliente.usuario_reg = usuarioId;
                cliente.TipoRegistro = "CAMPAÑA";

                if(cliente.NroDoc != null) {
                    if(cliente.NroDoc.Trim().Length == 8) {
                        cliente.TipoDocumentoId = 1;
                    } else {
                        cliente.TipoDocumentoId = 3;
                    }
                } else {
                    cliente.TipoDocumentoId = 2;
                }

                respuestaConsulta = ast_ClienteBL.GuardarClienteCampania(cliente);
                clienteregistro = ast_ClienteBL.GetClienteID(respuestaConsulta);
                if(respuestaConsulta > 0) {
                    respuesta = true;
                    errormensaje = "Registro Guardado Correctamente";
                } else {
                    errormensaje = "error al crear , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje, idcliente = clienteregistro });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> GetClienteCoincidencia(string coincidencia) {
            string mensaje = "";
            bool respuesta = false;
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            ApiReniec _apiReniec = new ApiReniec();
            try {
                cliente = ast_ClienteBL.GetClientexNroDoc(coincidencia);
                if(cliente.Id > 0) {
                    respuesta = true;
                } else {

                    var dataClienteAPI = await _apiReniec.Busqueda(coincidencia);
                    if(dataClienteAPI.Respuesta) {
                        cliente.Nombre = dataClienteAPI.Nombre;
                        cliente.ApelPat = dataClienteAPI.ApellidoPaterno;
                        cliente.ApelMat = dataClienteAPI.ApellidoMaterno;
                        cliente.NroDoc = dataClienteAPI.DNI;

                    } else {
                        cliente.NroDoc = coincidencia;

                    }

                    mensaje = "Cliente no Registrado , Registrelo porfavor";
                    respuesta = false;
                }

            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
                return Json(new { mensaje, respuesta, data = cliente });
            }
            return Json(new { mensaje, respuesta, data = cliente });
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult ExisteCampaña(int codSala) {
            string displayMessage;
            bool campaniaExists;

            try {
                campaniaExists = ast_ClienteBL.VerificarSiExisteCampania(codSala);
                displayMessage = campaniaExists ? "Si existe campaña" : "No existe campaña";
            } catch(Exception ex) {
                campaniaExists = false;
                displayMessage = ex.Message;
            }

            return Json(new { campaniaExists, displayMessage });
        }

        #region Campaña WhatsApp
        [seguridad(false)]
        [HttpPost]
        public async Task<JsonResult> ExisteCliente(string documentNumber, int idDocumentType, string phoneNumber, int codSala) {
            string displayMessage;
            bool success;
            bool clientExists;
            object searchParams = new {
                codSala,
                idDocumentType,
                documentNumber,
                phoneNumber
            };
            ResponseEntidad<ClienteVerificacionResponse> response = new ResponseEntidad<ClienteVerificacionResponse>();
            try {
                if(Constants.UrlVerificationClients.ContainsKey(codSala)) {
                    response = await clientebl.VerificarClientePtkWigos(idDocumentType, documentNumber, phoneNumber, codSala);
                } else {
                    response = ast_ClienteBL.GetExistenciaDeClienteParaCampaniaWhatsApp(documentNumber, idDocumentType, phoneNumber, codSala);
                }
                displayMessage = response.displayMessage;
                clientExists = response.data.clientExist;
                success = response.success;
            } catch(Exception ex) {
                displayMessage = ex.Message;
                clientExists = true;
                success = false;
            }
            return Json(new { success, clientExists, searchParams, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<JsonResult> GenerarCodigoCliente(string documentNumber, int codSala, string countryCode, string phoneNumber, string provenance = "", bool recibeMensaje = true) {
            string promotionalCode = string.Empty;
            string displayMessage;
            bool success;
            bool promotionalCodeSent = false;
            object dataSent = new {
                documentNumber,
                countryCode,
                phoneNumber,
                codSala,
                provenance
            };
            try {
                List<CMP_CampañaEntidad> campaniasWhatsapp = campaniabl.ListarCampaniasEstadoTipo(codSala, 1, 2);
                if(campaniasWhatsapp.Count == 0) {
                    displayMessage = "No es posible generar código promocional debido a que no existe campañas de WhatsApp activas.";
                    success = false;
                    return Json(new { success, promotionalCode, promotionalCodeSent, displayMessage, dataSent });
                }

                AST_ClienteEntidad cliente = new AST_ClienteEntidad();
                cliente = ast_ClienteBL.GetClientexNroDoc(documentNumber);
                if(cliente.Id == 0) {
                    success = false;
                    displayMessage = $"No hay cliente registrado con número de documento '{documentNumber}'.";
                    return Json(new { success, promotionalCode, promotionalCodeSent, displayMessage, dataSent });
                }

                //if (cliente.SalaId != codSala) {
                //    success = false;
                //    displayMessage = $"La sala enviada no corresponde a la sala registrada por el cliente.";
                //    return Json(new { success, promotionalCode, promotionalCodeSent, displayMessage });
                //}

                CMP_CampañaEntidad campaniaActual = campaniasWhatsapp.First();

                CMP_ClienteEntidad clienteCampania = clientebl.BuscarClienteExistenteCmpCliente(cliente.Id, campaniaActual.id);
                if(clienteCampania.cliente_id > 0) {
                    success = false;
                    displayMessage = "Cliente ya existe en la campaña";
                    return Json(new { success, promotionalCode, promotionalCodeSent, displayMessage, dataSent });
                }

                int longitudCodigoPromocional = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("CANT_CARACT_COD_PROM", 6));
                promotionalCode = clientebl.GenerarCodigoPromocional(longitudCodigoPromocional);

                string urlApiLanding = Convert.ToString(ValidationsHelper.GetValueAppSettingDB("URL_API_LANDING_PAGES", "https://api.casinokeops.pe/"));
                //string urlApiLanding = Convert.ToString(ValidationsHelper.GetValueAppSettingDB("URL_API_LANDING_PAGES", "http://192.168.1.32:9998"));
                string procedenciaRegistro = await clientebl.ObtenerProcedenciaRegistro(provenance, urlApiLanding);

                DateTime ahora = DateTime.Now;
                //int diasValidezCodigo = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("DIAS_VALIDEZ_COD_PROM", 30));
                CMP_ClienteEntidad cmp_cliente = new CMP_ClienteEntidad {
                    cliente_id = cliente.Id,
                    campania_id = campaniaActual.id,
                    fecha_reg = ahora,
                    Codigo = promotionalCode,
                    FechaGeneracionCodigo = ahora,
                    FechaExpiracionCodigo = ahora.AddDays(campaniaActual.duracionCodigoDias).AddHours(campaniaActual.duracionCodigoHoras),
                    CodigoEnviado = false,
                    CodigoPais = countryCode,
                    NumeroCelular = phoneNumber,
                    ProcedenciaRegistro = procedenciaRegistro,
                    CodigoCanjeableEn = codSala.ToString(),
                    CodigoCanjeableMultiplesSalas = false,
                };

                int guardado = clientebl.GuardarClienteCampaniaJson(cmp_cliente);

                clienteCampania = clientebl.BuscarClienteExistenteCmpCliente(cliente.Id, campaniaActual.id);

                if(recibeMensaje) {
                    if(clienteCampania.EsPosibleEnviarMensajeWhatsApp() && !string.IsNullOrEmpty(campaniaActual.mensajeWhatsApp)) {
                        wspMensajeriUltraMsgBL = new WSP_MensajeriaUltraMsgBL(codSala);
                        string completeNumber = $"{clienteCampania.CodigoPais}{clienteCampania.NumeroCelular}";
                        clienteCampania.Codigo = promotionalCode;
                        string whatsAppMessage = clienteCampania.ObtenerMensajeFormateadoParaEnvio(campaniaActual.mensajeWhatsApp, campaniaActual, clienteCampania);
                        var envioCodigoWhatsApp = await wspMensajeriUltraMsgBL.SendMessage(completeNumber, whatsAppMessage);
                        promotionalCodeSent = envioCodigoWhatsApp.success;
                        if(promotionalCodeSent) {
                            clientebl.MarcarCodigoEnviado(guardado);
                        }
                    }
                }

                success = guardado > 0;
                displayMessage = success ? $"Código promocional generado para el cliente '{cliente.NombreCompleto}'" : "Ocurrió un problema al generar el código promocional del cliente.";

            } catch(Exception ex) {
                displayMessage = ex.Message;
                success = false;
            }
            return Json(new { success, promotionalCode, promotionalCodeSent, displayMessage, dataSent });
        }

        [HttpPost]
        public async Task<JsonResult> CanjearCodigoPromocional(string promotionalCode, string documentNumber, int codSala, int idDocumentType) {
            string displayMessage;
            bool success;
            object dataSent = new {
                idDocumentType,
                documentNumber,
                promotionalCode,
                codSala
            };
            try {
                List<int> salasUsuario = salaBL.ObtenerCodsSalasDeSesion(Session);
                salasUsuario = salasUsuario.Where(x => x >= 0 && x != CodigoSalaSomosCasino).ToList();
                if(salasUsuario.Count != 1) {
                    displayMessage = $"Solo puedes canjear códigos promocionales si tienes una sala asignada.";
                    success = false;
                    return Json(new { success, dataSent, displayMessage });
                }
                int codigoCanjeadoEn = salasUsuario.First();

                CMP_ClienteEntidad clienteCampania = clientebl.ObtenerClientePorCodigoPromocional(promotionalCode);

                if(clienteCampania.cliente_id == 0) {
                    displayMessage = $"No se encontró el código promocional '{promotionalCode}', intente nuevamente.";
                    success = false;
                    return Json(new { success, dataSent, displayMessage });
                }

                if(!clienteCampania.NroDoc.Equals(documentNumber) || !clienteCampania.TipoDocumentoId.Equals(idDocumentType)) {
                    AST_TipoDocumentoEntidad tipoDocumento = ast_TipoDocumentoBL.GetTipoDocumentoID(idDocumentType);
                    displayMessage = $"El código promocional '{promotionalCode}' no corresponde al {tipoDocumento.Nombre} número '{documentNumber}'.";
                    success = false;
                    return Json(new { success, dataSent, displayMessage });
                }

                if(clienteCampania.CodigoCanjeableMultiplesSalas) {
                    List<int> salasCanjeables = clienteCampania.CodigoCanjeableEn.ToCleanedIntList();
                    if(!salasCanjeables.Contains(codigoCanjeadoEn)) {
                        displayMessage = $"El código promocional '{promotionalCode}' no se puede canjear en esta sala.";
                        success = false;
                        return Json(new { success, dataSent, displayMessage });
                    }
                } else {
                    if(clienteCampania.SalaId != codSala) {
                        displayMessage = $"El código promocional '{promotionalCode}' corresponde a una campaña de la sala {clienteCampania.NombreSala}.";
                        success = false;
                        return Json(new { success, dataSent, displayMessage });
                    }
                }

                if(clienteCampania.CodigoEstaExpirado()) {
                    displayMessage = $"El código promocional '{promotionalCode}' ya expiró.";
                    success = false;
                    return Json(new { success, dataSent, displayMessage });
                }

                if(clienteCampania.CodigoCanjeado) {
                    displayMessage = $"El código promocional '{promotionalCode}' ya fue canjeado el {clienteCampania.FechaCanjeoCodigo.ToString("dd/MM/yyyy")} a las {clienteCampania.FechaCanjeoCodigo.ToString("hh:mm:ss tt")}";
                    success = false;
                    return Json(new { success, dataSent, displayMessage });
                }

                success = clientebl.CanjearCodigoPromocional(clienteCampania.id, codigoCanjeadoEn);
                displayMessage = success ?
                    clienteCampania.MontoRecargado > 0 ?
                        $"Código promocional canjeado correctamente para el cliente: {clienteCampania.NombreCompleto}. Monto de recarga: S/{clienteCampania.MontoRecargado:0.00}"
                        : $"Código promocional canjeado correctamente para el cliente: {clienteCampania.NombreCompleto}"
                    : $"Ocurrió un error al intentar canjear el código promocional '{promotionalCode}'";

                if(clienteCampania.EsPosibleEnviarMensajeWhatsApp()) {
                    wspMensajeriUltraMsgBL = new WSP_MensajeriaUltraMsgBL(clienteCampania.SalaId);
                    await wspMensajeriUltraMsgBL.SendMessage($"{clienteCampania.CodigoPais}{clienteCampania.NumeroCelular}", $"Acaba de canjear su código promocional *{promotionalCode}*, disfrute los beneficios.");
                }
            } catch(Exception ex) {
                displayMessage = ex.Message;
                success = false;
            }
            return Json(new { success, dataSent, displayMessage });
        }

        [HttpPost]
        public ActionResult ObtenerClientesDeCampaniaWhatsAppPorIdCampania(long idCampania) {
            bool success = false;
            List<CMP_ClienteEntidad> clientes = new List<CMP_ClienteEntidad>();
            string displayMessage;

            try {
                clientes = clientebl.ObtenerClientesDeCampaniaWhatsAppPorIdCampania(idCampania);
                success = clientes.Count > 0;
                displayMessage = success ? "Lista de clientes de la campaña." : "No hay clientes en la campaña.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            int sessionRolId = Convert.ToInt32(Session["rol"]);
            bool tienePermisoModificarNumeroCelularCliente = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaClienteController", "ActualizarNumeroCelularCliente");
            bool tienePermisoReenviarMensajeConCodigoPromocional = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaClienteController", "EnviarCodigoPromocionalWhatsApp");
            bool tienePermisoGenerarExcelClientesCampaniaWhatsApp = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaClienteController", "GenerarExcelClientesCampaniaWhatsApp");

            object permisos = new {
                tienePermisoModificarNumeroCelularCliente,
                tienePermisoReenviarMensajeConCodigoPromocional,
                tienePermisoGenerarExcelClientesCampaniaWhatsApp
            };

            return Json(new { success, data = clientes, displayMessage, permisos });
        }

        [HttpPost]
        public ActionResult GenerarExcelClientesCampaniaWhatsApp(long idCampania) {
            string displayMessage;
            bool success;

            CMP_CampañaEntidad campania = campaniabl.CampañaIdObtenerJson(idCampania);
            success = campania.id > 0;
            if(!success) {
                success = false;
                displayMessage = "No se encontro la camapaña que se dese generar el archivo excel";
                return Json(new { success, displayMessage });
            }

            List<CMP_ClienteEntidad> clientes = clientebl.ObtenerClientesDeCampaniaWhatsAppPorIdCampania(idCampania);
            success = clientes.Count > 0;
            if(!success) {
                displayMessage = $"Aún no hay clientes registrados en la campaña {campania.nombre}.";
                return Json(new { success, displayMessage });
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("Tipo de Documento", typeof(string));
            dataTable.Columns.Add("Número de Documento", typeof(string));
            dataTable.Columns.Add("Código de País", typeof(string));
            dataTable.Columns.Add("Número de Celular", typeof(string));
            dataTable.Columns.Add("Procedencia de Registro", typeof(string));
            dataTable.Columns.Add("Monto Recargado", typeof(string));
            dataTable.Columns.Add("Nacionalidad", typeof(string));
            dataTable.Columns.Add("Fecha de Nacimiento", typeof(DateTime));
            dataTable.Columns.Add("Edad", typeof(int));
            dataTable.Columns.Add("¿Cod. Prom. Enviado?", typeof(string));
            dataTable.Columns.Add("Código Promocional", typeof(string));
            dataTable.Columns.Add("¿Cod. Prom. Canjeado?", typeof(string));
            dataTable.Columns.Add("Fecha Generación Cod. Prom.", typeof(DateTime));
            dataTable.Columns.Add("Fecha Canjeo Cod. Prom.", typeof(object));
            dataTable.Columns.Add("Fecha Expiración Cod. Prom.", typeof(DateTime));
            dataTable.Columns.Add("¿Cod. Prom. Regenerado?", typeof(string));

            foreach(var cliente in clientes) {
                dataTable.Rows.Add(
                    cliente.NombreCompleto,
                    cliente.TipoDocumento,
                    cliente.NroDoc,
                    cliente.CodigoPais,
                    cliente.NumeroCelular,
                    cliente.ProcedenciaRegistro,
                    $"S/{cliente.MontoRecargadoStr}",
                    cliente.Nacionalidad,
                    cliente.FechaNacimiento.ToString("dd/MM/yyyy"),
                    cliente.Edad,
                    cliente.CodigoEnviado ? "Sí" : "No",
                    cliente.Codigo,
                    cliente.CodigoCanjeado ? "Sí" : "No",
                    cliente.FechaGeneracionCodigo.ToString("dd/MM/yyyy HH:mm:ss"),
                    cliente.CodigoCanjeado ? cliente.FechaCanjeoCodigo.ToString("dd/MM/yyyy HH:mm:ss") : "---",
                    cliente.FechaExpiracionCodigo.ToString("dd/MM/yyyy HH:mm"),
                    cliente.CodigoExpirado ? "Sí" : "No"
                );
            }

            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Clientes campaña {campania.nombre} - {campania.nombresala}",
                    SheetName = "clientes",
                    Data = dataTable,
                    Title = $"Clientes registrados en la campaña {campania.nombre}",
                    FirstColumNumber = false,
                };

                var excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

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


            return Json(new { success, data = clientes, displayMessage });
        }

        [HttpPost]
        public async Task<JsonResult> EnviarCodigoPromocionalWhatsApp(long idCampaniaCliente) {
            string displayMessage;
            bool success;
            bool promotionalCodeSent = false;
            try {
                CMP_ClienteEntidad cliente = clientebl.ObtenerClientePorIdCampaniaCliente(idCampaniaCliente);
                if(cliente.id == 0) {
                    success = false;
                    displayMessage = $"No se encontró el cliente que se desea enviar el código promocional.";
                    return Json(new { success, displayMessage, idCampaniaCliente });
                }

                if(cliente.CodigoCanjeado) {
                    success = false;
                    displayMessage = $"Código promocional canjeado, no se puede reenviar el codigo promocional al cliente '{cliente.NombreCompleto}'.";
                    return Json(new { success, displayMessage, idCampaniaCliente });
                }

                if(cliente.CodigoEstaExpirado()) {
                    success = false;
                    displayMessage = $"Código promocional expirado, no se puede reenviar el codigo promocional al cliente '{cliente.NombreCompleto}'.";
                    return Json(new { success, displayMessage, idCampaniaCliente });
                }

                CMP_CampañaEntidad campania = campaniabl.CampañaIdObtenerJson(cliente.campania_id);
                if(!campania.Existe()) {
                    success = false;
                    displayMessage = $"No se encontró la campaña.";
                    return Json(new { success, displayMessage, idCampaniaCliente });
                }

                if(cliente.EsPosibleEnviarMensajeWhatsApp() && !string.IsNullOrEmpty(campania.mensajeWhatsApp)) {
                    wspMensajeriUltraMsgBL = new WSP_MensajeriaUltraMsgBL(cliente.SalaId);
                    string phoneNumber = $"{cliente.CodigoPais}{cliente.NumeroCelular}";
                    string whatsAppMessage = cliente.ObtenerMensajeFormateadoParaEnvio(campania.mensajeWhatsApp, campania, cliente);
                    ResponseEntidad<WSP_UltraMsgResponse> envioCodigoWhatsApp = await wspMensajeriUltraMsgBL.SendMessage(phoneNumber, whatsAppMessage);
                    promotionalCodeSent = envioCodigoWhatsApp.success;
                    if(promotionalCodeSent) {
                        clientebl.MarcarCodigoEnviado(idCampaniaCliente);
                    } else {
                        success = false;
                        displayMessage = envioCodigoWhatsApp.displayMessage;
                        return Json(new { success, promotionalCodeSent, displayMessage });
                    }
                }

                success = promotionalCodeSent;
                displayMessage = success ? $"Código promocional reenviado por WhatsApp al cliente '{cliente.NombreCompleto}'" : "No es posible enviar mensaje debido a que la campaña no tiene mensaje de WhatsApp configurado o el número del cliente no es válido.";

            } catch(Exception ex) {
                displayMessage = ex.Message;
                success = false;
            }
            return Json(new { success, promotionalCodeSent, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarNumeroCelularCliente(CMP_ClienteEntidad cliente) {
            string displayMessage;
            bool success;
            object dataSent = new {
                cliente.id,
                cliente.CodigoPais,
                cliente.NumeroCelular
            };
            if(string.IsNullOrEmpty(cliente.NumeroCelular)) {
                success = false;
                displayMessage = "Ingrese Número de Celular.";
                return Json(new { success, displayMessage, dataSent });
            }

            if(string.IsNullOrEmpty(cliente.CodigoPais)) {
                success = false;
                displayMessage = "Ingrese Código de País.";
                return Json(new { success, displayMessage, dataSent });
            }

            if(cliente.NumeroCelular.Length < 9) {
                success = false;
                displayMessage = "El Número de Celular no puede tener menos de 9 dígitos.";
                return Json(new { success, displayMessage, dataSent });
            }

            CMP_ClienteEntidad campaniaCliente = clientebl.ObtenerClientePorIdCampaniaCliente(cliente.id);
            if(cliente.id == 0) {
                success = false;
                displayMessage = $"No se encontró el cliente que se desea editar el número de celular.";
                return Json(new { success, displayMessage, dataSent });
            }

            if(campaniaCliente.CodigoCanjeado) {
                success = false;
                displayMessage = $"Código promocional canjeado, no se puede modificar el número de celular al cliente '{campaniaCliente.NombreCompleto}'.";
                return Json(new { success, displayMessage, dataSent });
            }

            if(campaniaCliente.CodigoEstaExpirado()) {
                success = false;
                displayMessage = $"Código promocional expirado, no se puede modificar el número de celular al cliente '{campaniaCliente.NombreCompleto}'.";
                return Json(new { success, displayMessage, dataSent });
            }

            try {
                success = clientebl.ActualizarCelularCliente(cliente);
                displayMessage = success ? "Número de celular actualizado correctamente." : "No se pudo actualizar los datos del cliente.";
            } catch(Exception ex) {
                displayMessage = ex.Message;
                success = false;
            }
            return Json(new { success, displayMessage, dataSent });
        }
        #endregion
    }
}