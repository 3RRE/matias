using CapaEntidad;
using CapaEntidad.GLPI;
using CapaEntidad.GLPI.DTO;
using CapaEntidad.GLPI.Enum;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Reporte;
using CapaEntidad.Queue;
using CapaNegocio;
using CapaNegocio.Campaña;
using CapaNegocio.GLPI;
using CapaNegocio.GLPI.Helper;
using CapaNegocio.GLPI.Mantenedores;
using CapaNegocio.Queue.Client;
using CapaPresentacion.Utilitarios;
using S3k.Utilitario.Excel;
using S3k.Utilitario.GLPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI {
    [seguridad(true)]
    public class GlpiTicketController : Controller {
        private readonly GLPI_TicketBL ticketBL;
        private readonly GLPI_AsignacionTicketBL asignacionTicketBL;
        private readonly GLPI_SeguimientoTicketBL seguimientoTicketBL;
        private readonly GLPI_CierreTicketBL cierreTicketBL;
        private readonly GLPI_CorreoBL correoBL;
        private readonly GLPI_FileUploadBL fileUploadBL;
        private readonly SEG_PermisoRolBL permisoRolBL;
        private readonly SalaBL salaBL;
        private readonly SEG_EmpleadoBL empleadoBL;
        private readonly CMP_impresora_usuarioBL impresorausuariobl;
        private readonly SEG_RolUsuarioBL web_RolUsuarioBL;
        private readonly SEG_UsuarioBL segUsuarioBl;
        private readonly Correo correo;
        private readonly RazorViewHelper razorViewHelper;
        private readonly AzureQueueClient queueClient;
        private readonly string PathArchivos;
        private readonly int QueueIdSistemaGlpiInfraestructura;
        private readonly string QueueEmail;
        private readonly string QueueWhatsApp;

        public GlpiTicketController() {
            ticketBL = new GLPI_TicketBL();
            asignacionTicketBL = new GLPI_AsignacionTicketBL();
            seguimientoTicketBL = new GLPI_SeguimientoTicketBL();
            cierreTicketBL = new GLPI_CierreTicketBL();
            correoBL = new GLPI_CorreoBL();
            fileUploadBL = new GLPI_FileUploadBL();
            permisoRolBL = new SEG_PermisoRolBL();
            salaBL = new SalaBL();
            empleadoBL = new SEG_EmpleadoBL();
            impresorausuariobl = new CMP_impresora_usuarioBL();
            web_RolUsuarioBL = new SEG_RolUsuarioBL();
            segUsuarioBl = new SEG_UsuarioBL();
            correo = new Correo();
            razorViewHelper = new RazorViewHelper();
            queueClient = AzureQueueClient.Instance;
            PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
            QueueIdSistemaGlpiInfraestructura = Convert.ToInt32(ConfigurationManager.AppSettings["AzureQueueIdSistemaGlpiInfraestructura"]);
            QueueEmail = ConfigurationManager.AppSettings["AzureQueueEmailName"];
            QueueWhatsApp = ConfigurationManager.AppSettings["AzureQueueWhatsAppName"];
        }

        #region Views
        public ActionResult CrearTicketsView() {
            return View("~/Views/GLPI/CrearTicket.cshtml");
        }
        public ActionResult AsignarTicketView() {
            return View("~/Views/GLPI/AsignarTicket.cshtml");
        }
        public ActionResult ReportesView() {
            return View("~/Views/GLPI/Reportes.cshtml");
        }
        #endregion

        #region Methods Ticket
        [HttpPost]
        public JsonResult GetTicketById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_TicketDto data = new GLPI_TicketDto();

            try {
                data = ticketBL.ObtenerTicketPorId(id);
                success = data.Existe();
                displayMessage = success ? "Ticket encontrado." : "No se encontró el ticket.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [seguridad(false)] //Debido a que se ejecuta al cargar una vista
        [HttpPost]
        public JsonResult GetTicketsPorIdUsuario() {
            bool success = false;
            string displayMessage;
            List<GLPI_TicketDto> ticketsCreados = new List<GLPI_TicketDto>();
            List<GLPI_TicketDto> ticketsAsignados = new List<GLPI_TicketDto>();
            List<GLPI_TicketDto> ticketsRecibidos = new List<GLPI_TicketDto>();

            try {
                int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
                int idRol = Convert.ToInt32(Session["rol"]);

                bool creaTickets = permisoRolBL.GetPermisoRolUsuario(idRol, nameof(CrearTicket)).Count > 0;
                if(creaTickets) {
                    ticketsCreados = ticketBL.ObtenerTicketsPorIdUsuarioSolicitante(idUsuario);
                }

                bool asignaTickets = permisoRolBL.GetPermisoRolUsuario(idRol, nameof(AsignarTicket)).Count > 0;
                if(asignaTickets) {
                    ticketsAsignados = ticketBL.ObtenerTicketsPorIdUsuarioAsigna(idUsuario);
                }

                bool recibeTickets = permisoRolBL.GetPermisoRolUsuario(idRol, nameof(RecibirTicket)).Count > 0;
                if(recibeTickets) {
                    ticketsRecibidos = ticketBL.ObtenerTicketsPorIdUsuarioAsignado(idUsuario);
                }

                string usuario = Session["UsuarioNombre"].ToString();
                success = ticketsAsignados.Count > 0 || ticketsRecibidos.Count > 0;
                displayMessage = success ? $"Tickets del usuario {usuario}." : $"Aún no hay ticket del/para el usuario {usuario}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            object data = new {
                ticketsCreados,
                ticketsAsignados,
                ticketsRecibidos
            };

            JsonResult jsonResult = Json(new { success, displayMessage, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [seguridad(false)] //Debido a que se ejecuta al cargar una vista
        [HttpPost]
        public JsonResult GetTicketsBySalasDeUsuarioYFase(GLPI_FaseTicket faseTicket) {
            bool success = false;
            string displayMessage;
            List<GLPI_TicketDto> data = new List<GLPI_TicketDto>();

            try {
                int idRol = Convert.ToInt32(Session["rol"]);
                bool puedeAsignar = permisoRolBL.GetPermisoRolUsuario(idRol, nameof(AsignarTicket)).Count > 0;
                if(!puedeAsignar) {
                    success = false;
                    displayMessage = $"El usuario no tiene el permiso para asignar tickets.";
                    return Json(new { success, data, displayMessage });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
                string usuario = Session["UsuarioNombre"].ToString();
                List<SalaEntidad> salas = salaBL.ListadoSalaPorUsuario(idUsuario);
                List<int> codsSala = salas.Select(x => x.CodSala).ToList();
                data = ticketBL.ObtenerTicketsPorCodsSalaYFase(codsSala, faseTicket);
                success = data.Count > 0;
                displayMessage = success ? $"Tickets registrados por el usuario {usuario}." : $"Aún no hay ticket registrados por {usuario}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> CrearTicket(GLPI_Ticket ticket, HttpPostedFileBase file) {
            bool success = false;
            string displayMessage;

            try {
                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                ticket.IdUsuarioSolicitante = idUsuario;

                // Solo se sube el archivo si existe y tiene contenido
                if(file != null && file.ContentLength > 0) {
                    List<string> subFolders = new List<string> { "GLPI", "Tickets" };
                    GLPI_FileUpload fileUpload = await fileUploadBL.UploadFileAsync(file, subFolders);

                    if(!fileUpload.Success) {
                        return Json(new {
                            success = false,
                            displayMessage = fileUpload.DisplayMessage
                        });
                    }

                    ticket.Adjunto = fileUpload.Path;
                }

                List<string> correos = GlpiCorreoHelper.FormarCorreos(ticket.Destinatarios);
                correoBL.InsertarCorreos(correos, idUsuario);

                int idTicket = ticketBL.InsertarTicket(ticket);
                success = idTicket > 0;

                if(success && correos.Count > 0) {
                    GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(idTicket);

                    EmailQueue emailQueue = new EmailQueue {
                        IdSistema = QueueIdSistemaGlpiInfraestructura,
                        Mensaje = new EmailQueueContentRequest {
                            Destinatarios = correos,
                            Asunto = $"Nuevo Ticket creado",
                            Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/CrearTicket.cshtml", ticketDto),
                            EsHtml = true
                        }
                    };

                    _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                }
                displayMessage = success ? "Ticket creado correctamente." : "No se pudo crear el ticket.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public async Task<JsonResult> EditarTicket(GLPI_Ticket ticket, HttpPostedFileBase file) {
            bool success = false;
            string displayMessage;

            try {
                GLPI_TicketDto ticketVerificacion = ticketBL.ObtenerTicketPorId(ticket.Id);
                bool puedeModificarTicket = ticketVerificacion.CodigoFaseTicket == GLPI_FaseTicket.Creado;

                if(!puedeModificarTicket) {
                    return Json(new {
                        success = false,
                        displayMessage = $"El ticket #{ticketVerificacion.Id} ya está asignado, por lo que no se puede editar."
                    });
                }

                ticket.Adjunto = ticketVerificacion.Adjunto; // Mantener adjunto anterior por defecto

                if(file != null && file.ContentLength > 0) {
                    List<string> subFolders = new List<string> { "GLPI", "Tickets" };

                    GLPI_FileUpload fileUpload = await fileUploadBL.UploadFileAsync(file, subFolders);

                    if(!fileUpload.Success) {
                        return Json(new {
                            success = false,
                            displayMessage = fileUpload.DisplayMessage
                        });
                    }

                    // Eliminar archivo anterior solo si existe
                    if(!string.IsNullOrEmpty(ticketVerificacion.Adjunto)) {
                        string filePath = Path.Combine(PathArchivos, ticketVerificacion.Adjunto);
                        fileUploadBL.DeleteFile(filePath);
                    }

                    ticket.Adjunto = fileUpload.Path;
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                List<string> correos = GlpiCorreoHelper.FormarCorreos(ticket.Destinatarios);
                correoBL.InsertarCorreos(correos, idUsuario);

                int idTicket = ticketBL.ActualizarTicket(ticket);
                success = idTicket > 0;

                if(success && correos.Count > 0) {
                    GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(idTicket);

                    EmailQueue emailQueue = new EmailQueue {
                        IdSistema = QueueIdSistemaGlpiInfraestructura,
                        Mensaje = new EmailQueueContentRequest {
                            Destinatarios = correos,
                            Asunto = $"Ticket #{ticket.Id} modificado",
                            Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/EditarTicket.cshtml", ticketDto),
                            EsHtml = true
                        }
                    };

                    _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                }

                displayMessage = success ? "Ticket editado correctamente." : "No se pudo editar el ticket.";
            
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarTicket(int id) {
            bool success = false;
            string displayMessage;

            try {
                if(ticketBL.TicketEstaEnFase(id, GLPI_FaseTicket.Asignado)) {
                    displayMessage = $"No se puede eliminar el ticket #{id} debido a que ya está asignado.";
                    return Json(new { success, displayMessage });
                }
                success = ticketBL.EliminarTicket(id);
                displayMessage = success ? "Ticket eliminado correctamente." : "No se pudo eliminar el ticket.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion

        #region Asignacion Tickets
        [HttpPost]
        public JsonResult AsignarTicket(GLPI_AsignacionTicket asignacionTicket) {
            bool success = false;
            string displayMessage;

            try {
                GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(asignacionTicket.IdTicket);
                if(!ticketDto.Existe()) {
                    displayMessage = $"No existe el ticket #{asignacionTicket.IdTicket}.";
                    return Json(new { success, displayMessage });
                }

                bool puedeAsignar = ticketDto.CodigoFaseTicket == GLPI_FaseTicket.Creado;
                if(!puedeAsignar) {
                    success = false;
                    displayMessage = $"El ticket #{asignacionTicket.IdTicket} ya se encuentra asignado, por lo que no se puede ser asignado nuevamente.";
                    return Json(new { success, displayMessage });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                asignacionTicket.Destinatarios = GlpiCorreoHelper.ValidarCorreos(asignacionTicket.Destinatarios);
                correoBL.InsertarCorreos(asignacionTicket.Destinatarios, idUsuario);
                asignacionTicket.IdUsuarioAsigna = idUsuario;
                success = asignacionTicketBL.InsertarAsignacionTicket(asignacionTicket);
                if(success) {
                    ticketBL.ActualizarFaseTicketPorIdTicket(asignacionTicket.IdTicket, GLPI_FaseTicket.Asignado);
                    ticketDto = ticketBL.ObtenerTicketPorId(ticketDto.Id);
                    if(ticketDto.Asignacion.UsuarioAsignado.TieneCorreoValido() || asignacionTicket.Destinatarios.Count > 0) {
                        List<string> destinatarios = ticketDto.Asignacion.UsuarioAsignado.ObtenerCorreosValidos();
                        destinatarios.AddRange(asignacionTicket.Destinatarios);
                        EmailQueue emailQueue = new EmailQueue {
                            IdSistema = QueueIdSistemaGlpiInfraestructura,
                            Mensaje = new EmailQueueContentRequest {
                                Destinatarios = destinatarios,
                                Asunto = $"Ticket #{ticketDto.Id} asignado",
                                Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/AsignarTicket.cshtml", ticketDto),
                                EsHtml = true
                            }
                        };
                        _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                    }
                }
                displayMessage = success ? "Asignación guardada correctamente." : "No se pudo guardar la asignación de ticket.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EditarAsignacionTicket(GLPI_AsignacionTicket asignacionTicket) {
            bool success = false;
            string displayMessage;

            try {
                GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(asignacionTicket.IdTicket);
                if(!ticketDto.Existe()) {
                    displayMessage = $"No existe el ticket #{asignacionTicket.IdTicket}.";
                    return Json(new { success, displayMessage });
                }

                bool puedeModificarAsignacion = ticketDto.CodigoFaseTicket == GLPI_FaseTicket.Asignado;
                if(!puedeModificarAsignacion) {
                    success = false;
                    displayMessage = $"El ticket #{asignacionTicket.IdTicket} ya está en proceso, por lo que no se puede editar la asignación.";
                    return Json(new { success, displayMessage });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                asignacionTicket.Destinatarios = GlpiCorreoHelper.ValidarCorreos(asignacionTicket.Destinatarios);
                correoBL.InsertarCorreos(asignacionTicket.Destinatarios, idUsuario);
                success = asignacionTicketBL.ActualizarAsignacionTicket(asignacionTicket);
                if(success) {
                    ticketDto = ticketBL.ObtenerTicketPorId(ticketDto.Id);
                    if(ticketDto.Asignacion.UsuarioAsignado.TieneCorreoValido() || asignacionTicket.Destinatarios.Count > 0) {
                        List<string> destinatarios = ticketDto.Asignacion.UsuarioAsignado.ObtenerCorreosValidos();
                        destinatarios.AddRange(asignacionTicket.Destinatarios);
                        EmailQueue emailQueue = new EmailQueue {
                            IdSistema = QueueIdSistemaGlpiInfraestructura,
                            Mensaje = new EmailQueueContentRequest {
                                Destinatarios = destinatarios,
                                Asunto = $"Modificación de la asignación del Ticket #{ticketDto.Id}",
                                Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/EditarAsignacionTicket.cshtml", ticketDto),
                                EsHtml = true
                            }
                        };
                        _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                    }
                }
                displayMessage = success ? "Asignación de ticket editada correctamente." : "No se pudo editar la asignación de ticket.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarAsignacionTicket(int id) {
            bool success = false;
            string displayMessage;

            try {
                bool tieneSeguimiento = ticketBL.TicketEstaEnFase(id, GLPI_FaseTicket.EnProceso);
                if(tieneSeguimiento) {
                    success = false;
                    displayMessage = $"El ticket con código {id} ya está en proceso, por lo que no se puede eliminar.";
                    return Json(new { success, displayMessage });
                }
                success = asignacionTicketBL.EliminarAsignacionTicket(id);
                if(success) {
                    ticketBL.ActualizarFaseTicketPorIdTicket(id, GLPI_FaseTicket.Creado);
                }
                displayMessage = success ? "Asignación de ticket eliminada correctamente." : "No se pudo eliminar la asignación de ticket.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion

        #region Recibir Ticket
        public JsonResult RecibirTicket() {
            return Json(new { success = true });
        }
        #endregion

        #region Seguimiento Ticket
        public JsonResult CrearSeguimientoTicket(GLPI_SeguimientoTicket seguimientoTicket) {
            bool success = false;
            string displayMessage;

            try {
                GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(seguimientoTicket.IdTicket);
                if(!ticketDto.Existe()) {
                    displayMessage = $"No existe el ticket #{seguimientoTicket.IdTicket}.";
                    return Json(new { success, displayMessage });
                }

                bool puedeDarSeguimiento = ticketDto.CodigoFaseTicket == GLPI_FaseTicket.EnProceso || ticketDto.CodigoFaseTicket == GLPI_FaseTicket.Asignado;
                if(!puedeDarSeguimiento) {
                    success = false;
                    displayMessage = $"El ticket #{seguimientoTicket.IdTicket} aún no está asignado, por lo que no se puede agregar seguimiento.";
                    return Json(new { success, displayMessage });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                seguimientoTicket.Destinatarios = GlpiCorreoHelper.ValidarCorreos(seguimientoTicket.Destinatarios);
                correoBL.InsertarCorreos(seguimientoTicket.Destinatarios, idUsuario);
                GLPI_SeguimientoTicket ultimoSeguimiento = seguimientoTicketBL.ObtenerUltimoSeguimientoDeTicket(seguimientoTicket.IdTicket);
                if(ultimoSeguimiento.Existe()) {
                    seguimientoTicket.IdEstadoTicketAnterior = ultimoSeguimiento.IdEstadoTicketActual;
                    seguimientoTicket.IdProcesoAnterior = ultimoSeguimiento.IdProcesoActual;
                }
                seguimientoTicket.IdUsuarioRegistra = idUsuario;
                success = seguimientoTicketBL.InsertarSeguimientoTicket(seguimientoTicket);
                if(success) {
                    ticketBL.ActualizarFaseTicketPorIdTicket(seguimientoTicket.IdTicket, GLPI_FaseTicket.EnProceso);
                    if(seguimientoTicket.Destinatarios.Count > 0) {
                        ticketDto = ticketBL.ObtenerTicketPorId(ticketDto.Id);
                        EmailQueue emailQueue = new EmailQueue {
                            IdSistema = QueueIdSistemaGlpiInfraestructura,
                            Mensaje = new EmailQueueContentRequest {
                                Destinatarios = seguimientoTicket.Destinatarios,
                                Asunto = $"Nuevo seguimiento de Ticket #{ticketDto.Id}",
                                Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/AgregarSeguimientoTicket.cshtml", ticketDto),
                                EsHtml = true
                            }
                        };
                        _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                    }
                }
                displayMessage = success ? $"Se guardo el seguimiento del ticket #{seguimientoTicket.IdTicket}." : $"No se pudo guardar el seguimiento del ticket #{seguimientoTicket.IdTicket}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }
        #endregion

        #region Cierre Ticket
        public JsonResult CerrarTicket(GLPI_CierreTicket cierreTicket) {
            bool success = false;
            string displayMessage;

            try {
                GLPI_TicketDto ticket = ticketBL.ObtenerTicketPorId(cierreTicket.IdTicket);
                if(!ticket.Existe()) {
                    displayMessage = $"No existe el ticket #{cierreTicket.IdTicket}.";
                    return Json(new { success, displayMessage });
                }

                bool puedeFinalizar = ticket.CodigoFaseTicket == GLPI_FaseTicket.EnProceso;
                if(!puedeFinalizar) {
                    success = false;
                    displayMessage = $"El ticket con código {cierreTicket.IdTicket} aún no tiene seguimiento, por lo que no se puede finalizar.";
                    return Json(new { success, displayMessage });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                GLPI_SeguimientoTicket ultimoSeguimiento = seguimientoTicketBL.ObtenerUltimoSeguimientoDeTicket(cierreTicket.IdTicket);
                if(ultimoSeguimiento.Existe()) {
                    cierreTicket.IdEstadoTicketAnterior = ultimoSeguimiento.IdEstadoTicketActual;
                }
                cierreTicket.IdUsuarioCierra = idUsuario;
                success = cierreTicketBL.InsertarCierreTicket(cierreTicket);
                if(success) {
                    ticketBL.ActualizarFaseTicketPorIdTicket(cierreTicket.IdTicket, GLPI_FaseTicket.Finalizado);
                    GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(cierreTicket.IdTicket);
                    if(ticketDto.UsuarioSolicitante.TieneCorreoValido()) {
                        EmailQueue emailQueue = new EmailQueue {
                            IdSistema = QueueIdSistemaGlpiInfraestructura,
                            Mensaje = new EmailQueueContentRequest {
                                Destinatarios = ticketDto.UsuarioSolicitante.ObtenerCorreosValidos(),
                                Asunto = $"Cierre de Ticket #{ticketDto.Id}",
                                Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/CierreTicket.cshtml", ticketDto),
                                EsHtml = true
                            }
                        };
                        _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                    }
                }
                displayMessage = success ? $"Se cerró correctamente el ticket #{cierreTicket.IdTicket}." : $"No se pudo cerrar el ticket #{cierreTicket.IdTicket}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        public JsonResult ConfirmarCierreTicket(int idTicket) {
            bool success = false;
            string displayMessage;

            try {
                GLPI_TicketDto ticket = ticketBL.ObtenerTicketPorId(idTicket);
                if(!ticket.Existe()) {
                    displayMessage = $"No existe el ticket #{idTicket}.";
                    return Json(new { success, displayMessage });
                }

                bool puedeCerrar = ticket.CodigoFaseTicket == GLPI_FaseTicket.Finalizado;
                if(!puedeCerrar) {
                    success = false;
                    displayMessage = $"El ticket #{idTicket} aún no está cerrado, por lo que no puede confirmar el cierre del ticket.";
                    return Json(new { success, displayMessage });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);

                success = cierreTicketBL.ConfirmarCierreTicket(idUsuario, idTicket);
                if(success) {
                    ticketBL.ActualizarFaseTicketPorIdTicket(idTicket, GLPI_FaseTicket.Cerrado);
                    GLPI_TicketDto ticketDto = ticketBL.ObtenerTicketPorId(idTicket);
                    if(ticketDto.UsuarioSolicitante.TieneCorreoValido()) {
                        EmailQueue emailQueue = new EmailQueue {
                            IdSistema = QueueIdSistemaGlpiInfraestructura,
                            Mensaje = new EmailQueueContentRequest {
                                Destinatarios = ticketDto.UsuarioSolicitante.ObtenerCorreosValidos(),
                                Asunto = $"Confirmación de Cierre de Ticket #{ticketDto.Id}",
                                Cuerpo = razorViewHelper.RenderViewToString(ControllerContext, "~/views/GLPI/Email/ConfirmaCierreTicket.cshtml", ticketDto),
                                EsHtml = true
                            }
                        };
                        _ = queueClient.SendMessageAsync(QueueEmail, emailQueue);
                    }
                }
                displayMessage = success ? $"Se confirmo el cierre del del ticket #{idTicket} correctamente." : $"No se pudo confirmar el cierre del ticket #{idTicket}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }
        #endregion

        #region Empleado
        [seguridad(false)]
        public JsonResult CrearEmpleado(SEG_EmpleadoEntidad empleado, SEG_UsuarioEntidad usuario, int rolId) {
            bool success = false;
            string displayMessage = "Error al registrar empleado";

            try {

                SEG_EmpleadoEntidad test = empleadoBL.EmpleadoListarJson().Where(x => x.DOI == empleado.DOI).FirstOrDefault();
                SEG_UsuarioEntidad usuarioValidar = segUsuarioBl.UsuarioListadoJson().Where(x => x.UsuarioNombre == usuario.UsuarioNombre).FirstOrDefault();
                if(test != null) {
                    displayMessage = "Numero de dni ya registrado";
                    success = false;
                    return Json(new { success, displayMessage });
                }
                if(usuarioValidar != null) {
                    displayMessage = "Nombre de usuario ya registrado";
                    success = false;
                    return Json(new { success, displayMessage });
                }

                empleado.EstadoEmpleado = 1;
                empleado.FechaAlta = DateTime.Now;
                Int32 empleadoRegistrado = empleadoBL.GuardarEmpleadoGlpi(empleado);

                if(empleadoRegistrado > 0) {
                    var usuarioPass = usuario.UsuarioContraseña;
                    usuario.FechaRegistro = DateTime.Now;
                    usuario.FailedAttempts = 0;
                    usuario.EstadoContrasena = 1;
                    usuario.Estado = 1;
                    usuario.EmpleadoID = empleadoRegistrado;
                    usuario.UsuarioContraseña = PasswordHashTool.PasswordHashManager.CreateHash(usuario.UsuarioContraseña);
                    Int32 usuarioRegistrado = segUsuarioBl.UsuarioGuardarGlpiJson(usuario);

                    if(usuarioRegistrado > 0) {
                        SEG_RolUsuarioEntidad rolUsuario = new SEG_RolUsuarioEntidad();

                        rolUsuario.WEB_RolID = rolId;
                        rolUsuario.UsuarioID = usuarioRegistrado;
                        web_RolUsuarioBL.EliminarRolUsuario(rolUsuario.UsuarioID);
                        rolUsuario.WEB_RUsuFechaRegistro = DateTime.Now;
                        bool respuestaConsulta = web_RolUsuarioBL.GuardarRolUsuario(rolUsuario);
                        if(respuestaConsulta) {
                            success = true;
                            displayMessage = "Empleado registrado correctamente";
                        }
                    }
                }

            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }
        #endregion

        #region Reporte
        [HttpPost]
        public JsonResult GetTicketsReportes(GLPI_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;
            List<GLPI_TicketDto> data = new List<GLPI_TicketDto>();

            try {
                data = ticketBL.ObtenerTicketsReportes(filtro);
                success = data.Count > 0;
                displayMessage = success ? "Lista de tickets." : "No hay tickets con los filtros ingresados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> GenerarExcelTicketsReportes(GLPI_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;

            #region Obtener los datos
            List<GLPI_TicketDto> data = ticketBL.ObtenerTicketsReportes(filtro);

            success = data.Count > 0;
            displayMessage = success ? "Lista de tickets." : "No se encontraron tickets con los filtros ingresados.";
            #endregion

            if(!success) {
                return Json(new { success, displayMessage });
            }

            #region Armar DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Cod. Sala", typeof(int));
            dataTable.Columns.Add("Sala", typeof(string));
            dataTable.Columns.Add("Número Documento", typeof(string));
            dataTable.Columns.Add("Solicitante", typeof(string));
            dataTable.Columns.Add("Cargo", typeof(string));
            dataTable.Columns.Add("Clasificación del Problema", typeof(string));
            dataTable.Columns.Add("Nivel de Atención", typeof(string));
            dataTable.Columns.Add("Estado Actual", typeof(string));
            dataTable.Columns.Add("Fase", typeof(string));
            dataTable.Columns.Add("Fecha Registro", typeof(string));

            foreach(GLPI_TicketDto item in data) {
                dataTable.Rows.Add(
                    item.Id,
                    item.Sala.CodSala,
                    item.Sala.Nombre,
                    item.UsuarioSolicitante.NumeroDocumento,
                    item.UsuarioSolicitante.ObtenerNombreCompleto(),
                    item.UsuarioSolicitante.Cargo,
                    item.ClasificacionProblema.Nombre,
                    item.NivelAtencion.Nombre,
                    item.EstadoActual.Nombre,
                    item.FaseTicket,
                    item.FechaRegistro.ToString("dd/MM/yyyy HH:MM:ss")
                );
            }
            #endregion

            #region CrearExcel
            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"GLPI Infraestructura - Tickets del {filtro.FechaInicio:dd/MM/yyyy} al {filtro.FechaFin:dd/MM/yyyy}",
                    SheetName = "Tickets",
                    Data = dataTable,
                    Title = $"Tickets",
                    FirstColumNumber = true,
                };

                byte[] excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                JsonResult json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al administrador.";
            }
            #endregion

            JsonResult jsonResult = Json(new { success, data, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    }
}