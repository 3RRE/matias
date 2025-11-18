using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaEntidad.Response;
using CapaEntidad.TITO;
using CapaEntidad.WhatsApp;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaNegocio.WhatsApp;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Campaña {
    [seguridad]
    public class CampaniaController : Controller {
        private SalaBL salaBl = new SalaBL();
        private CMP_SalalibreBL salalibreBl = new CMP_SalalibreBL();
        private CMP_SalasesionBL salasesionBl = new CMP_SalasesionBL();
        private CMP_CampañaBL campaniabl = new CMP_CampañaBL();
        private CMP_TicketBL ticketbl = new CMP_TicketBL();
        private bool MostrarWinCupones = Convert.ToBoolean(ConfigurationManager.AppSettings["MostrarWinCupones"]);
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private CMP_ClienteBL clientebl = new CMP_ClienteBL();
        private CMP_CuponesGeneradosBL cuponesBL = new CMP_CuponesGeneradosBL();
        private SEG_PermisoRolBL segPermisoRolBL = new SEG_PermisoRolBL();
        private WSP_MensajeriaUltraMsgBL wspMensajeriaUltraMsgBL;
        private readonly int CodigoSalaSomosCasino;

        public CampaniaController() {
            CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);
        }

        public ActionResult ListadoCampañas() {
            ViewBag.MostrarWinCupones = MostrarWinCupones;
            return View("~/Views/Campania/ListadoCampania.cshtml");
        }

        [HttpPost]
        public ActionResult CampaniaIDObtenerJson(int id) {
            var errormensaje = "";
            var campania = new CMP_CampañaEntidad();
            try {
                campania = campaniabl.CampañaIdObtenerJson(id);
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = campania, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CampaniaTicketsObtenerJson(Int64 campaniaid) {
            var errormensaje = "";
            var tickets = new List<CMP_TicketEntidad>();
            try {
                tickets = ticketbl.CMPTicketCampañaListadoJson(campaniaid);
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = tickets, mensaje = errormensaje });
        }


        [HttpPost]
        public ActionResult CampaniaGuardarJson(CMP_CampañaEntidad campania) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                campania.fechareg = DateTime.Now;
                campania.estado = 1;
                campania.usuario_id = Convert.ToInt32(Session["UsuarioID"]);

                if(campania.tipo == 2) {
                    var campaniasWhatsAppActivas = campaniabl.ListarCampaniasEstadoTipo(campania.sala_id, 1, 2);
                    if(campaniasWhatsAppActivas.Count > 0) {
                        respuesta = false;
                        errormensaje = $"Ya existe una campaña activa del tipo WhatsApp para la sala {campaniasWhatsAppActivas.First().nombresala}.";
                        return Json(new { respuesta, mensaje = errormensaje });
                    }

                    if(campania.duracionCodigoDias == 0 && campania.duracionCodigoHoras == 0) {
                        respuesta = false;
                        errormensaje = $"La duración del cupón promocional no puede ser cero.";
                        return Json(new { respuesta, mensaje = errormensaje });
                    }

                    if(campania.codigoSeReactiva && campania.duracionReactivacionCodigoDias == 0 && campania.duracionReactivacionCodigoHoras == 0) {
                        respuesta = false;
                        errormensaje = $"El tiempo de reactivación del cupón no puede ser cero.";
                        return Json(new { respuesta, mensaje = errormensaje });
                    }

                }

                respuestaConsulta = campaniabl.CampañaInsertarJson(campania);

                if(respuestaConsulta > 0) {
                    respuesta = true;
                    errormensaje = "Registro Campaña Guardado Correctamente";
                } else {
                    errormensaje = "error al crear la Campaña , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CampaniaActualizarJson(CMP_CampañaEntidad campania) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                //campania.fechareg = DateTime.Now;
                campania.usuario_id = Convert.ToInt32(Session["UsuarioID"]);

                if(campania.tipo == 2) {
                    bool actualEsTipoWhatsApp = campaniabl.CampañaIdObtenerJson(campania.id).tipo == 2;
                    var campaniasWhatsAppActivas = campaniabl.ListarCampaniasEstadoTipo(campania.sala_id, 1, 2).ToList();
                    campaniasWhatsAppActivas.RemoveAll(x => x.id == campania.id);

                    if(campaniasWhatsAppActivas.Count > 0 && !actualEsTipoWhatsApp) {
                        respuesta = false;
                        errormensaje = $"Ya existe una campaña activa del tipo WhatsApp para la sala {campaniasWhatsAppActivas.First().nombresala}.";
                        return Json(new { respuesta, mensaje = errormensaje });
                    }

                    if(campania.duracionCodigoDias == 0 && campania.duracionCodigoHoras == 0) {
                        respuesta = false;
                        errormensaje = $"La duración del cupón promocional no puede ser cero.";
                        return Json(new { respuesta, mensaje = errormensaje });
                    }

                    if(campania.codigoSeReactiva && campania.duracionReactivacionCodigoDias == 0 && campania.duracionReactivacionCodigoHoras == 0) {
                        respuesta = false;
                        errormensaje = $"El tiempo de reactivación del cupón no puede ser cero.";
                        return Json(new { respuesta, mensaje = errormensaje });
                    }
                }

                respuesta = campaniabl.CampañaEditarJson(campania);

                if(respuesta) {
                    errormensaje = "Registro de Campaña Actualizado Correctamente";
                } else {
                    errormensaje = "error al Actualizar Campaña , LLame Administrador";
                    respuesta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public JsonResult ListarCampañasxFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin, int estado) {
            var errormensaje = "";
            var lista = new List<CMP_CampañaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            try {
                switch(estado) {
                    case 0:
                        strElementos_ = " c.estado=0 and ";
                        break;
                    case 1:
                        strElementos_ = " c.estado=1 and ";
                        break;
                    case 2:
                        strElementos_ = " ";
                        break;
                }


                if(cantElementos > 0) {
                    strElementos = " c.[sala_id] in(" + "'" + String.Join("','", codsala) + "'" + ") and " + strElementos_;
                }
                lista = campaniabl.CampañaListadoJson(strElementos, fechaini, fechafin);

                //quitar las campañas wsp, para despues poder agregar todas las campañas wasap sin importar la fecha
                lista.RemoveAll(e => e.tipo == 2 && e.estado == 1);

                //agregar las campañas wsp de la sala
                List<CMP_CampañaEntidad> campaniasWhatsApp = new List<CMP_CampañaEntidad>();
                foreach(var idRoom in codsala) {
                    campaniasWhatsApp.AddRange(campaniabl.ListarCampaniasEstadoTipo(idRoom, 1, 2));
                }
                lista.AddRange(campaniasWhatsApp);

                if(lista.Count > 0) {
                    List<Int64> Id_Campañas = new List<Int64>();
                    DateTime fechaHoy = DateTime.Now;
                    foreach(var registro in lista) {
                        var fechat = registro.fechafin.ToString("dd/mm/yyyy");
                        DateTime fechatermino = DateTime.Parse(registro.fechafin.ToString("dd/MM/yyyy"));
                        if(fechatermino.Date < fechaHoy.Date) {
                            registro.estado = 0;
                            Id_Campañas.Add(registro.id);
                        }
                    }
                    if(Id_Campañas.Count > 0) {
                        var ids_listaedicion = " id in(" + "'" + String.Join("','", Id_Campañas) + "'" + ") ";
                        var actualizarEstado = campaniabl.EditarMultipleEstadoCampaña(ids_listaedicion, 0);
                    }
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            int sessionRolId = Convert.ToInt32(Session["rol"]);
            bool tienePermisoModificarMensajeWhatsAppCampania = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaController", "ActualizarMensajeWhatsAppCampania");
            bool tienePermisoCanjearCodigoPromocional = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaClienteController", "CanjearCodigoPromocional");
            bool tienePermisoVerClientesDeCampania = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaClienteController", "ObtenerClientesDeCampaniaWhatsAppPorIdCampania");

            object permisos = new {
                tienePermisoModificarMensajeWhatsAppCampania,
                tienePermisoCanjearCodigoPromocional,
                tienePermisoVerClientesDeCampania
            };

            return Json(new { data = lista.ToList(), mensaje = errormensaje, permisos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReporteCampaniasDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin, int estado) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CMP_CampañaEntidad> lista = new List<CMP_CampañaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            List<CMP_TicketEntidad> tickets = new List<CMP_TicketEntidad>();
            double totalmonto = 0;
            try {
                switch(estado) {
                    case 0:
                        strElementos_ = " c.estado=0 and ";
                        break;
                    case 1:
                        strElementos_ = " c.estado=1 and ";
                        break;
                    case 2:
                        strElementos_ = " ";
                        break;
                }

                if(cantElementos > 0) {
                    for(int i = 0; i < codsala.Length; i++) {
                        var salat = salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " c.[sala_id] in(" + "'" + String.Join("','", codsala) + "'" + ") and " + strElementos_;
                }

                lista = campaniabl.CampañaListadoJson(strElementos, fechaini, fechafin);

                //quitar las campañas wsp, para despues poder agregar todas las campañas wasap sin importar la fecha
                lista.RemoveAll(e => e.tipo == 2 && e.estado == 1);

                //agregar las campañas wsp de la sala
                List<CMP_CampañaEntidad> campaniasWhatsApp = new List<CMP_CampañaEntidad>();
                foreach(var idRoom in codsala) {
                    campaniasWhatsApp.AddRange(campaniabl.ListarCampaniasEstadoTipo(idRoom, 1, 2));
                }
                lista.AddRange(campaniasWhatsApp);

                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Campañas");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Nombre";
                    workSheet.Cells[3, 5].Value = "Descripcion";
                    workSheet.Cells[3, 6].Value = "Fecha Registro";
                    workSheet.Cells[3, 7].Value = "Fecha Inicio";
                    workSheet.Cells[3, 8].Value = "Fecha Fin";
                    workSheet.Cells[3, 9].Value = "Estado";
                    workSheet.Cells[3, 10].Value = "Usuario Reg.";
                    workSheet.Cells[3, 11].Value = "Tickets(Usu. Reg. - Nro. Ticket - Monto - Cliente - Nro Doc. - Fecha ticket)";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        string registroticket = string.Empty;
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        tickets = ticketbl.CMPTicketCampañaListadoJson(registro.id);
                        if(tickets.Count > 0) {
                            int cantid = tickets.Count;
                            int i = 0;
                            foreach(var regTicket in tickets) {
                                i++;
                                if(i == cantid) {
                                    registroticket += regTicket.nombre_usuario + " - " + regTicket.nroticket + " - S/. " + regTicket.monto.ToString("#,##0.00") + " - " + regTicket.Apeclie + " " + regTicket.NomClie + " - " + regTicket.Dni + " - " + regTicket.fecharegsala.ToString("dd-MM-yyyy hh:mm:ss tt");
                                } else {
                                    registroticket += regTicket.nombre_usuario + " - " + regTicket.nroticket + " - S/. " + regTicket.monto.ToString("#,##0.00") + " - " + regTicket.Apeclie + " " + regTicket.NomClie + " - " + regTicket.Dni + " - " + regTicket.fecharegsala.ToString("dd-MM-yyyy hh:mm:ss tt") + Environment.NewLine;
                                }

                            }
                            totalmonto += tickets.Select(x => x.monto).Sum();
                        }

                        workSheet.Cells[recordIndex, 2].Value = registro.id;
                        workSheet.Cells[recordIndex, 3].Value = registro.nombresala;
                        workSheet.Cells[recordIndex, 4].Value = registro.nombre;

                        workSheet.Cells[recordIndex, 5].Value = registro.descripcion;
                        workSheet.Cells[recordIndex, 6].Value = registro.fechareg.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 7].Value = registro.fechaini.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 8].Value = registro.fechafin.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 9].Value = registro.estado == 1 ? "Activa" : "Vencida";
                        workSheet.Cells[recordIndex, 10].Value = registro.usuarionombre;
                        workSheet.Cells[recordIndex, 11].Value = registroticket;

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:K3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:K3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:K3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:K3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:K3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                    workSheet.Cells["B2:K2"].Merge = true;
                    workSheet.Cells["B2:K2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 11].Value = totalmonto;
                    workSheet.Cells[filaFooter, 11].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:J" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 11].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 28;
                    workSheet.Column(4).Width = 34;
                    workSheet.Column(5).Width = 38;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 20;
                    workSheet.Column(8).Width = 20;
                    workSheet.Column(9).Width = 20;
                    workSheet.Column(10).Width = 24;
                    workSheet.Column(11).Width = 110;
                    excelName = "Campañas_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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
        public ActionResult ReporteTicketCampaniasDescargarExcelJson(int campaniaid) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CMP_TicketEntidad> lista = new List<CMP_TicketEntidad>();
            var nombresala = String.Empty;

            double totalmonto = 0;

            Dictionary<int, string> instanciasCode = new Dictionary<int, string>
            {
                { 1, "Procedimiento Manual" },
                { 2, "Instancia por Auditoria" }
            };

            try {
                var campaniadetalle = campaniabl.CampañaIdObtenerJson(campaniaid);
                var sala = salaBl.SalaListaIdJson(campaniadetalle.sala_id);
                nombresala = sala.Nombre;
                lista = ticketbl.CMPTicketCampañaListadoJson(campaniaid);
                if(lista.Count > 0) {
                    totalmonto = lista.Select(x => x.monto).Sum();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Campañas");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALA : " + nombresala + " - CAMPAÑA : " + campaniadetalle.nombre + " - Del " + campaniadetalle.fechaini.ToString("dd-MM-yyyy") + " al " + campaniadetalle.fechafin.ToString("dd-MM-yyyy");

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Fecha Ticket";
                    workSheet.Cells[3, 4].Value = "Nro. Ticket";
                    workSheet.Cells[3, 5].Value = "Monto";
                    workSheet.Cells[3, 6].Value = "Instancia";
                    workSheet.Cells[3, 7].Value = "Nombre Cliente";
                    workSheet.Cells[3, 8].Value = "Nro. Documento";
                    workSheet.Cells[3, 9].Value = "Telefono";
                    workSheet.Cells[3, 10].Value = "Correo";
                    workSheet.Cells[3, 11].Value = "Fecha Registro";
                    workSheet.Cells[3, 12].Value = "Usuario Reg.";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.id;
                        workSheet.Cells[recordIndex, 3].Value = registro.fecharegsala.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 4].Value = registro.nroticket;
                        workSheet.Cells[recordIndex, 5].Value = registro.monto;
                        workSheet.Cells[recordIndex, 6].Value = instanciasCode.ContainsKey(registro.estado) ? instanciasCode[registro.estado] : "";
                        ;
                        workSheet.Cells[recordIndex, 7].Value = registro.NombreCompleto;
                        workSheet.Cells[recordIndex, 8].Value = registro.NroDoc;
                        workSheet.Cells[recordIndex, 9].Value = registro.Celular1;
                        workSheet.Cells[recordIndex, 10].Value = registro.Correo;
                        workSheet.Cells[recordIndex, 11].Value = registro.fechareg.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 12].Value = registro.nombre_usuario;

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:L3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:L3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:L3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:L3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:L3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:L3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:L3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:L3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:L3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:L3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:L3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                    workSheet.Cells["B2:L2"].Merge = true;
                    workSheet.Cells["B2:L2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":D" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":L" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":L" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":L" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":L" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":L" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 5].Value = totalmonto;
                    workSheet.Cells[filaFooter, 5].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":L" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:K" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 11].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 22;
                    workSheet.Column(5).Width = 18;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 40;
                    workSheet.Column(8).Width = 23;
                    workSheet.Column(9).Width = 35;
                    workSheet.Column(10).Width = 35;
                    workSheet.Column(11).Width = 25;
                    workSheet.Column(12).Width = 24;
                    excelName = "Ticket_Campaña_" + DateTime.Now.ToString("dd_MM_yyyy") + "_.xlsx";
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
        public ActionResult BuscarTicketPromocional(int codsala, string nroticket) {
            SalaEntidad sala = new SalaEntidad();
            var errormensaje = "";
            var client = new System.Net.WebClient();
            var response = "";
            var rutaticketregistrado = "/servicio/ConsultaTicketRegistrado?nroTicket=" + nroticket;
            //var rutaticketregistrado = "/servicio/ConsultaTicketRegistradoCodCliente?nroTicket=" + nroticket;
            var jsonResponse = new List<Det0001TTO_00H>();
            bool respuesta = false;
            bool permiso = false;
            List<CMP_SalalibreEntidad> salalibre = new List<CMP_SalalibreEntidad>();
            try {
                salalibre = salalibreBl.CMPsalalibrexsala(codsala);
                if(salalibre.Count > 0) {
                    permiso = true;
                    errormensaje = "Sala con registro libre";
                    jsonResponse.Add(new Det0001TTO_00H());
                    return Json(new { respuesta, data = jsonResponse, permiso, mensaje = errormensaje });
                }

                sala = salaBl.SalaListaIdJson(codsala);

                if(string.IsNullOrEmpty(sala.UrlProgresivo)) {
                    return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
                    ;
                }
                rutaticketregistrado = sala.UrlProgresivo + rutaticketregistrado;
                client.Headers.Add("content-type", "application/json; charset=utf-8");
                response = client.UploadString(rutaticketregistrado, "POST");
                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

                // star
                if(jsonResponse.Count >= 2) {
                    List<Det0001TTO_00H> listD_00H = new List<Det0001TTO_00H>();

                    foreach(Det0001TTO_00H D_00H in jsonResponse) {
                        CMP_TicketEntidad ticketbusqueda = ticketbl.CMPTIcketmontonroObtenerJson(D_00H.Item, D_00H.Tito_NroTicket, D_00H.Tito_MontoTicket);

                        if(ticketbusqueda.id == 0) {
                            listD_00H.Add(D_00H);
                        }
                    }

                    jsonResponse = listD_00H.OrderBy(item => item.Estado).ThenBy(item => item.Tito_fechaini).Take(1).ToList();
                }
                // end

                if(jsonResponse.Count >= 1) {
                    if(jsonResponse.Count == 1) {
                        if(jsonResponse[0].tipo_ticket == "4" || jsonResponse[0].tipo_ticket == "5") {

                            if(jsonResponse[0].Estado == 1) {
                                errormensaje = "Ticket Encontrado";
                                respuesta = true;
                            } else {
                                if(jsonResponse[0].Estado == 2) {
                                    errormensaje = "Ticket Ya Cobrado";
                                    respuesta = false;
                                } else {
                                    errormensaje = "Ticket Con estado diferente a Cobrado y Pendiente";
                                    respuesta = false;
                                }
                            }

                        } else {
                            errormensaje = "Tipo de ticket erróneo (tiene que ser PROMOCIONAL)";
                            respuesta = false;
                        }
                    } else {
                        errormensaje = "Ticket Duplicado";
                        respuesta = false;
                    }
                } else {
                    errormensaje = "No se encontro El nro de Ticket";
                    respuesta = false;
                }


            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { respuesta, permiso, data = jsonResponse.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult CampaniaTicketGuardarJson(Det0001TTO_00H ticket, int id, int codSala) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            CMP_TicketEntidad ticketentidad = new CMP_TicketEntidad();
            CMP_CampañaEntidad campania = new CMP_CampañaEntidad();
            CMP_TicketEntidad ticketbusqueda = new CMP_TicketEntidad();
            try {
                campania = campaniabl.CampañaIdObtenerJson(id);
                if(campania.id == 0) {
                    errormensaje = "No se encontro Campaña";
                    return Json(new { respuesta, mensaje = errormensaje });
                }

                ticketbusqueda = ticketbl.CMPTIcketmontonroObtenerJson(ticket.Item, ticket.Tito_NroTicket, ticket.Tito_MontoTicket);
                if(ticketbusqueda.id > 0) {
                    errormensaje = "Ticket ya Registrado";
                    return Json(new { respuesta, mensaje = errormensaje });
                }

                #region verificar si es sala fisica
                List<int> salasVirtuales = new List<int>() { CodigoSalaSomosCasino };
                bool esSalaFisica = !salasVirtuales.Contains(codSala);
                #endregion

                #region artificio para obtener el codsala correcto, en caso sea de somos casino
                if(codSala == CodigoSalaSomosCasino) {
                    List<int> listaSalas = salaBl.ObtenerCodsSalasDeSesion(Session).Where(x => x != CodigoSalaSomosCasino).ToList();
                    if(listaSalas.Count > 0) {
                        codSala = listaSalas.First();
                    }
                }
                #endregion

                ticketentidad.fechareg = DateTime.Now;
                ticketentidad.estado = 1;
                ticketentidad.usuario_id = Convert.ToInt32(Session["UsuarioID"]);
                ticketentidad.campaña_id = id;
                ticketentidad.item = ticket.Item;
                ticketentidad.nroticket = ticket.Tito_NroTicket;
                ticketentidad.monto = ticket.Tito_MontoTicket;
                ticketentidad.fecharegsala = ticket.Tito_fechaini;
                ticketentidad.cliente_id = ticket.codclie;
                ticketentidad.Apeclie = ticket.Apeclie;
                ticketentidad.NomClie = ticket.NomClie;
                ticketentidad.Correo = ticket.Correo;
                ticketentidad.Dni = ticket.Dni;
                ticketentidad.FechaNacimiento = ticket.FechaNacimiento;
                ticketentidad.FechaApertura = ticket.Fecha_Apertura;
                ticketentidad.origen = ticket.Punto_venta_fin;
                ticketentidad.SalaOrigen = codSala;
                ticketentidad.SalaFisica = esSalaFisica;

                respuestaConsulta = ticketbl.TicketInsertarJson(ticketentidad);

                if(respuestaConsulta > 0) {
                    respuesta = true;
                    errormensaje = "Ticket Validado Correctamente";
                } else {
                    errormensaje = "error al Validar Ticket , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult QuitarTicketCampania(int id) {
            var errormensaje = "";
            bool respuesta = false;
            CMP_TicketEntidad ticketentidad = new CMP_TicketEntidad();
            try {
                respuesta = ticketbl.TicketEliminarJson(id);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó el Ticket Correctamente";
                } else {
                    errormensaje = "error al Quitar Ticket , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ValidacionListadoSalas() {
            var errormensaje = "";
            var lista = new List<CMP_SalalibreEntidad>();
            try {

                lista = salalibreBl.CMPsalalibretListadoCompletoJson();
            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult SalaVerificacionTicketPermisoJson(CMP_SalalibreEntidad sala) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                sala.fechareg = DateTime.Now;
                sala.estado = 1;
                if(sala.id != 0) {
                    respuesta = salalibreBl.salalibreEliminarJson(sala.id);
                    respuestaConsulta = 0;
                    if(respuesta) {
                        respuesta = true;
                        errormensaje = "Registro Sala Actualizado Correctamente";
                    } else {
                        errormensaje = "error al Actualizar Sala , LLame Administrador";
                        respuesta = false;
                    }
                } else {
                    respuestaConsulta = salalibreBl.salalibreInsertarJson(sala);
                    if(respuestaConsulta > 0) {
                        respuesta = true;
                        errormensaje = "Registro Sala Actualizado Correctamente";
                    } else {
                        errormensaje = "error al Actualizar Sala , LLame Administrador";
                        respuesta = false;
                    }
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, respuestaConsulta, mensaje = errormensaje });
        }


        [HttpPost]
        public ActionResult SalaCuponAutomaticoPermisoJson(CMP_SalasesionEntidad sala) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                sala.fechareg = DateTime.Now;
                sala.estado = 1;
                if(sala.id != 0) {
                    respuesta = salasesionBl.salasesionEliminarJson(sala.id);
                    respuestaConsulta = 0;
                    if(respuesta) {
                        respuesta = true;
                        errormensaje = "Registro Sala Actualizado Correctamente";
                    } else {
                        errormensaje = "error al Actualizar Sala , LLame Administrador";
                        respuesta = false;
                    }
                } else {
                    respuestaConsulta = salasesionBl.salasesionInsertarJson(sala);
                    if(respuestaConsulta > 0) {
                        respuesta = true;
                        errormensaje = "Registro Sala Actualizado Correctamente";
                    } else {
                        errormensaje = "error al Actualizar Sala , LLame Administrador";
                        respuesta = false;
                    }
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult SalaLibreActualizarJson(int id) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try {
                //campania.fechareg = DateTime.Now;
                respuestaConsulta = salalibreBl.salalibreEliminarJson(id);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Sala Actualizado Correctamente";
                } else {
                    errormensaje = "error al Actualizar Sala , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        [seguridad(false)]
        public ActionResult ClienteConsultaJuego() {
            return View("~/Views/Campania/ClienteConsultaJuego.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCampaniasxClienteJson(string nrodoc, DateTime fechaIni, DateTime fechaFin) {
            List<AST_ClienteEntidad> cliente = new List<AST_ClienteEntidad>();
            List<CMP_CuponesGeneradosEntidad> listacuponesGenerados = new List<CMP_CuponesGeneradosEntidad>();
            List<CMP_ClienteEntidad> listaCmpCliente = new List<CMP_ClienteEntidad>();
            string clienteIdQueryPromocion = string.Empty;
            string clienteIdQuerySorteo = string.Empty;
            string mensaje = string.Empty;
            bool respuesta = false;
            try {
                cliente = ast_ClienteBL.GetListaClientesxNroDoc(nrodoc);
                if(cliente.Count > 0) {
                    clienteIdQueryPromocion = " cli.cliente_id in (" + String.Join(",", cliente.Select(x => x.Id)) + ") ";
                    clienteIdQuerySorteo = " cli.ClienteId in (" + String.Join(",", cliente.Select(x => x.Id)) + ") ";
                    //Lista CMP_Cliente
                    listaCmpCliente = clientebl.GetClientesCampaniaxCliente(clienteIdQueryPromocion, fechaIni, fechaFin);
                    //Lista Cupones Generados
                    listacuponesGenerados = cuponesBL.GetListadoCuponesxCliente(clienteIdQuerySorteo, fechaIni, fechaFin);
                    foreach(var cmpcliente in listaCmpCliente) {
                        listacuponesGenerados.Add(new CMP_CuponesGeneradosEntidad {
                            CampaniaNombre = cmpcliente.CampaniaNombre,
                            NombreCompleto = cmpcliente.NombreCompleto,
                            NroDoc = cmpcliente.NroDoc,
                            ClienteId = cmpcliente.cliente_id,
                            CampaniaId = cmpcliente.campania_id,
                            TipoCampania = "Promocion",
                        });
                    }
                    respuesta = true;
                    mensaje = "Listando Registros";
                } else {
                    mensaje = "No se encontraron clientes con el nro. de documento " + nrodoc;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listacuponesGenerados });
        }

        // Auditoria TITO Promocionales
        public ActionResult AuditoriaTitoPromoVista() {
            return View("~/Views/Campania/AuditoriaTitoPromoVista.cshtml");
        }

        [HttpPost]
        public ActionResult ListarAuditoriaTITOPromo(ParametersTITOCaja args) {
            int status = 0;
            string message = "No se encontraron registros";
            int roomCode = Convert.ToInt32(args.RoomCode);
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            bool inVpn = false;

            SalaEntidad sala = salaBl.ObtenerSalaPorCodigo(roomCode);

            if(sala.CodSala == 0) {
                return Json(new {
                    status = 2,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if(string.IsNullOrEmpty(args.OpeningDateStart)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha inicio a buscar"
                });
            }

            if(string.IsNullOrEmpty(args.OpeningDateEnd)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha fin a buscar"
                });
            }

            List<SalaEntidad> listaSalas = salaBl.ListadoSalaPorUsuario(userId);
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            //Check port
            CheckPortHelper checkPortHelper = new CheckPortHelper();
            CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, urls);

            if(!tcpConnection.IsOpen) {
                return Json(new {
                    status = 2,
                    message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                });
            }

            // data
            List<Reporte_Detalle_TITO_Caja> listReport = new List<Reporte_Detalle_TITO_Caja>();
            IEnumerable<dynamic> listTitos = new List<dynamic>();

            try {
                var response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ListarDetalleTITOCajasCortesias";

                dynamic data = new {
                    OpeningDateStart = args.OpeningDateStart,
                    OpeningDateEnd = args.OpeningDateEnd
                };

                if(tcpConnection.IsVpn) {
                    inVpn = true;
                    uri = $"{tcpConnection.Url}/servicio/ListarDetalleTITOCajasCortesiasVpn";

                    data = new {
                        OpeningDateStart = args.OpeningDateStart,
                        OpeningDateEnd = args.OpeningDateEnd,
                        PrivateIp = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/servicio/ListarDetalleTITOCajasCortesias"
                    };
                }

                string json = JsonConvert.SerializeObject(data);

                using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                // Data
                listReport = JsonConvert.DeserializeObject<List<Reporte_Detalle_TITO_Caja>>(response, settings);
                listReport = listReport.Where(item => item.D00H_Item > 0).ToList();
                listReport = ReporteTITOCajaHelper.ChangeDataCustomers(listReport);

                // data Auditoria TITO Promocionales
                AuditoriaTITOPromoHelper auditoriaTito = new AuditoriaTITOPromoHelper(roomCode, listReport);
                listTitos = auditoriaTito.Titos();

                status = 1;
                message = "Datos obtenidos correctamente";
            } catch(Exception exception) {
                message = exception.Message;
            }

            var resultData = new {
                status,
                message,
                data = listTitos,
                inVpn
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer {
                MaxJsonLength = int.MaxValue
            };

            ContentResult result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetATPEnlazarTickets(int room_id) {
            int status = 0;
            bool withclient = false;
            int sessionRolId = Convert.ToInt32(Session["rol"]);

            List<CMP_CampañaEntidad> listCampanias = new List<CMP_CampañaEntidad>();

            try {
                withclient = segPermisoRolBL.AutorizedControllerAction(sessionRolId, "CampaniaController", "ATPPermisoCliente");
                listCampanias = campaniabl.ListarCampaniasEstadoTipo(room_id, 1, 0);

                status = 1;
            } catch(Exception exception) {
                return Json(new {
                    status,
                    message = exception.Message.ToString(),
                    campaigns = listCampanias,
                    withclient
                });
            }

            return Json(new {
                status,
                message = "Datos obtenidos correctamente",
                campaigns = listCampanias,
                withclient
            });
        }

        [HttpPost]
        public ActionResult ATPEnlazarTickets(int sala_id, int campania_id, int customer_id, List<Reporte_Detalle_TITO_Caja> tickets) {
            int status = 0;
            string message = "No se ha podido enlazar los Tickets";

            List<Reporte_Detalle_TITO_Caja> ticketsInsert = new List<Reporte_Detalle_TITO_Caja>();
            List<Reporte_Detalle_TITO_Caja> ticketsInsertados = new List<Reporte_Detalle_TITO_Caja>();
            List<Reporte_Detalle_TITO_Caja> ticketsNoInsertados = new List<Reporte_Detalle_TITO_Caja>();

            try {
                CMP_CampañaEntidad campania = campaniabl.CampañaIdObtenerJson(campania_id);

                if(campania.id == 0) {
                    return Json(new {
                        status = 2,
                        message = "No se encontro Campaña"
                    });
                }

                foreach(Reporte_Detalle_TITO_Caja ticket in tickets) {
                    CMP_TicketEntidad ticketExist = ticketbl.GetTicketItem(ticket.D00H_Item, ticket.Ticket, decimal.ToDouble(ticket.Monto_Dinero));

                    if(ticketExist.id == 0) {
                        ticketsInsert.Add(ticket);
                    }
                }

                if(tickets.Count > 0 && ticketsInsert.Count == 0) {
                    return Json(new {
                        status = 2,
                        message = "Los tickets seleccionados ya se encuentran enlazados"
                    });
                }

                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);

                ticketsInsertados = ticketbl.ATPGuardarTickets(usuarioId, sala_id, campania_id, customer_id, ticketsInsert);
                ticketsNoInsertados = ticketsInsert.Where(titoIn => !ticketsInsertados.Any(ticketIn => ticketIn.D00H_Item == titoIn.D00H_Item)).ToList();

                if(ticketsInsertados.Count > 0) {
                    status = 1;
                    message = $"{ticketsInsertados.Count}/{tickets.Count} Tickets enlazados a la Campaña {campania.nombre}";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                dataIn = ticketsInsertados,
                dataOut = ticketsNoInsertados
            });
        }

        [HttpPost]
        public ActionResult ExcelAuditoriaTITOPromo(ParametersTITOCaja parameters) {
            int status = 0;
            string message = "No se encontraron registros";
            int roomCode = Convert.ToInt32(parameters.RoomCode);
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            bool inVpn = false;

            SalaEntidad sala = salaBl.ObtenerSalaPorCodigo(roomCode);

            if(sala.CodSala == 0) {
                return Json(new {
                    status = 2,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if(string.IsNullOrEmpty(parameters.OpeningDateStart)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha inicio a buscar"
                });
            }

            if(string.IsNullOrEmpty(parameters.OpeningDateEnd)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha fin a buscar"
                });
            }

            List<SalaEntidad> listaSalas = salaBl.ListadoSalaPorUsuario(userId);
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            //Check port
            CheckPortHelper checkPortHelper = new CheckPortHelper();
            CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, urls);

            if(!tcpConnection.IsOpen) {
                return Json(new {
                    status = 2,
                    message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                });
            }

            // data
            List<Reporte_Detalle_TITO_Caja> listReport = new List<Reporte_Detalle_TITO_Caja>();
            IEnumerable<dynamic> listTitos = new List<dynamic>();

            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;
            DateTime fromDate = Convert.ToDateTime(parameters.OpeningDateStart);
            DateTime toDate = Convert.ToDateTime(parameters.OpeningDateEnd);

            try {
                var response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ListarDetalleTITOCajasCortesias";

                dynamic args = new {
                    OpeningDateStart = parameters.OpeningDateStart,
                    OpeningDateEnd = parameters.OpeningDateEnd
                };

                if(tcpConnection.IsVpn) {
                    inVpn = true;
                    uri = $"{tcpConnection.Url}/servicio/ListarDetalleTITOCajasCortesiasVpn";

                    args = new {
                        OpeningDateStart = parameters.OpeningDateStart,
                        OpeningDateEnd = parameters.OpeningDateEnd,
                        PrivateIp = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/servicio/ListarDetalleTITOCajasCortesias"
                    };
                }

                string json = JsonConvert.SerializeObject(args);

                using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                // Data
                listReport = JsonConvert.DeserializeObject<List<Reporte_Detalle_TITO_Caja>>(response, settings);
                listReport = listReport.Where(item => item.D00H_Item > 0).ToList();
                listReport = ReporteTITOCajaHelper.ChangeDataCustomers(listReport);

                // Data Auditoria TITO Promocionales
                AuditoriaTITOPromoHelper auditoriaTito = new AuditoriaTITOPromoHelper(roomCode, listReport);
                listTitos = auditoriaTito.Titos();

                // Data Excel
                dynamic arguments = new {
                    roomCode,
                    roomName = sala.Nombre,
                    fromDate = parameters.OpeningDateStart,
                    toDate = parameters.OpeningDateEnd
                };

                MemoryStream memoryStream = AuditoriaTITOPromoHelper.ExcelFisico(listTitos, arguments);
                fileName = $"Auditoria Titos Promocionales - {sala.Nombre} {fromDate.ToString("dd-MM-yyyy")} al {toDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

                status = 1;
                message = "Excel generado";
                data = Convert.ToBase64String(memoryStream.ToArray());
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                fileName,
                data,
                inVpn
            });
        }

        [HttpPost]
        public bool ATPPermisoCliente() {
            return true;
        }

        [HttpPost]
        public JsonResult ActualizarMensajeWhatsAppCampania(string mensajeWhatsApp, string mensajeWhatsAppReactivacion, long idCampania) {
            bool success = false;
            string displayMessage;
            try {
                CMP_CampañaEntidad campania = campaniabl.CampañaIdObtenerJson(idCampania);
                if(!campania.Existe()) {
                    return Json(new { success = false, displayMessage = $"No existe una campaña con código {idCampania}" });
                }

                if(campania.codigoSeReactiva && string.IsNullOrEmpty(mensajeWhatsAppReactivacion)) {
                    return Json(new { success = false, displayMessage = $"Tiene que ingresar un mensaje que se envía al momento de reactivar el código promocional." });
                }

                success = campaniabl.ActualizarMensajeWhatsAppCampania(mensajeWhatsApp, mensajeWhatsAppReactivacion, idCampania);
                displayMessage = success ? "Mensaje de WhatsApp actualizado correctamente." : "No se pudo actualizar el mensaje de WhatsApp, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public async Task<JsonResult> EnviarMensajeWhatsAppPrueba(int codSala, long idCampania, string message, string messageReactivacion, string phoneNumber) {
            ResponseEntidad<WSP_UltraMsgResponse> response = new ResponseEntidad<WSP_UltraMsgResponse>();
            wspMensajeriaUltraMsgBL = new WSP_MensajeriaUltraMsgBL(codSala);
            string userName = Convert.ToString(Session["UsuarioNombre"]);

            SalaEntidad sala = salaBl.SalaListaIdJson(codSala);
            if(sala.CodSala <= 0) {
                return Json(new { success = false, displayMessage = $"No existe una sala con código {codSala}" });
            }

            CMP_CampañaEntidad campania = campaniabl.CampañaIdObtenerJson(idCampania);
            if(!campania.Existe()) {
                return Json(new { success = false, displayMessage = $"No existe una campaña con código {idCampania}" });
            }

            DateTime fechaExpiracion = DateTime.Now.AddDays(campania.duracionCodigoDias).AddHours(campania.duracionCodigoHoras);

            CMP_ClienteEntidad campaniaCliente = new CMP_ClienteEntidad {
                Codigo = "A1B2C3",
                NombreSala = sala.Nombre,
                FechaExpiracionCodigo = fechaExpiracion,
                Nombre = "Juan",
                NombreCompleto = "Juan Perez",
                MontoRecargado = 123
            };

            if(!string.IsNullOrEmpty(message)) {
                try {
                    message = campaniaCliente.ObtenerMensajeFormateadoParaEnvio(message, campania, campaniaCliente);
                } catch(Exception ex) {
                    return Json(new { success = false, displayMessage = ex.Message });
                }
                message += $"\n--------------------------------------------------------------\nMensaje de prueba enviado desde el *IAS* por el usuario *'{userName}'*";
                response = await wspMensajeriaUltraMsgBL.SendMessage(phoneNumber, message);
            }

            if(!string.IsNullOrEmpty(messageReactivacion)) {
                try {
                    messageReactivacion = campaniaCliente.ObtenerMensajeFormateadoParaEnvio(messageReactivacion, campania, campaniaCliente);
                } catch(Exception ex) {
                    return Json(new { success = false, displayMessage = ex.Message });
                }
                messageReactivacion += $"\n--------------------------------------------------------------\nMensaje de prueba enviado desde el *IAS* por el usuario *'{userName}'*";
                response = await wspMensajeriaUltraMsgBL.SendMessage(phoneNumber, messageReactivacion);
            }

            return Json(response);
        }

        #region MetodosVPN
        [HttpPost]
        [seguridad(false)]
        public ActionResult BuscarTicketPromocionalVpn(int codsala, string nroticket, string urlPublica, string urlPrivada, string nroDoc, int idCampania) {
            SalaEntidad sala = new SalaEntidad();
            var errormensaje = "";
            var client = new System.Net.WebClient();
            var response = "";
            var rutaticketregistrado = "/servicio/ConsultaTicketRegistrado?nroTicket=" + nroticket;
            //var rutaticketregistrado = "/servicio/ConsultaTicketRegistradoCodCliente?nroTicket=" + nroticket;
            var jsonResponse = new List<Det0001TTO_00H>();
            bool respuesta = false;
            bool permiso = false;
            List<CMP_SalalibreEntidad> salalibre = new List<CMP_SalalibreEntidad>();
            try {
                salalibre = salalibreBl.CMPsalalibrexsala(codsala);
                if(salalibre.Count > 0) {
                    permiso = true;
                    respuesta = true;
                    errormensaje = "Sala con registro libre";
                    jsonResponse.Add(new Det0001TTO_00H());
                    return Json(new { respuesta, data = jsonResponse, permiso, mensaje = errormensaje });
                }

                #region artificio para obtener el codsala correcto, en caso sea de somos casino
                if(codsala == CodigoSalaSomosCasino) {
                    CMP_ClienteEntidad cmpCliente = clientebl.ObtenerClienteDeCampaniaWhatsAppPorIdCampaniaNumeroDocumento(idCampania, nroDoc);
                    if(cmpCliente.CodigoCanjeadoEn > 0) {
                        codsala = cmpCliente.CodigoCanjeadoEn;
                    } else {
                        List<int> listaSalas = salaBl.ObtenerCodsSalasDeSesion(Session).Where(x => x != CodigoSalaSomosCasino).ToList();
                        if(listaSalas.Count > 0) {
                            codsala = listaSalas.First();
                        }
                    }
                }
                #endregion

                sala = salaBl.SalaListaIdJson(codsala);

                if(string.IsNullOrEmpty(sala.IpPrivada)) {
                    return Json(new { respuesta = false, mensaje = "No se configuró la url privada de sala" });
                }

                //urlPublica = string.IsNullOrWhiteSpace(sala.UrlProgresivo) ? urlPublica : sala.UrlProgresivo;
                rutaticketregistrado = sala.IpPrivada + ":" + sala.PuertoServicioWebOnline + rutaticketregistrado;

                urlPublica = urlPublica + "/servicio/ConsultaTicketRegistradoVpn";
                client.Headers.Add("content-type", "application/json; charset=utf-8");
                object oEnvio = new {
                    ipPrivada = rutaticketregistrado
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                response = client.UploadString(urlPublica, "POST", inputJson);
                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

                // star
                if(jsonResponse.Count >= 2) {
                    List<Det0001TTO_00H> listD_00H = new List<Det0001TTO_00H>();

                    foreach(Det0001TTO_00H D_00H in jsonResponse) {
                        CMP_TicketEntidad ticketbusqueda = ticketbl.CMPTIcketmontonroObtenerJson(D_00H.Item, D_00H.Tito_NroTicket, D_00H.Tito_MontoTicket);

                        if(ticketbusqueda.id == 0) {
                            listD_00H.Add(D_00H);
                        }
                    }

                    jsonResponse = listD_00H.OrderBy(item => item.Estado).ThenBy(item => item.Tito_fechaini).Take(1).ToList();
                }
                // end

                if(jsonResponse.Count >= 1) {
                    if(jsonResponse.Count == 1) {
                        if(jsonResponse[0].tipo_ticket == "4" || jsonResponse[0].tipo_ticket == "5") {

                            if(jsonResponse[0].Estado == 1) {
                                errormensaje = "Ticket Encontrado";
                                respuesta = true;
                            } else {
                                if(jsonResponse[0].Estado == 2) {
                                    errormensaje = "Ticket Ya Cobrado";
                                    respuesta = false;
                                } else {
                                    errormensaje = "Ticket Con estado diferente a Cobrado y Pendiente";
                                    respuesta = false;
                                }
                            }

                        } else {
                            errormensaje = "Tipo de ticket erróneo (tiene que ser PROMOCIONAL)";
                            respuesta = false;
                        }
                    } else {
                        errormensaje = "Ticket Duplicado";
                        respuesta = false;
                    }
                } else {
                    errormensaje = "No se encontro El nro de Ticket";
                    respuesta = false;
                }


            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { respuesta, permiso, data = jsonResponse.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    }
}