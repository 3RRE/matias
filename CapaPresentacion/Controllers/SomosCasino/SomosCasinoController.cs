using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaEntidad.Queue;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaNegocio.Queue.Client;
using CapaPresentacion.Filters;
using CapaPresentacion.Utilitarios;
using CapaPresentacion.Validators.SomosCasino;
using FluentValidation.Results;
using S3k.Utilitario.Extensions;
using S3k.Utilitario.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.SomosCasino {
    public class SomosCasinoController : BaseController {
        private readonly CMP_ClienteBL clienteBL;
        private readonly CMP_CampañaBL campaniaBL;
        private readonly AST_ClienteBL astClienteBL;
        private readonly AST_TipoDocumentoBL astTipoDocumentoBL;
        private readonly AST_ClienteSalaBL astClienteSalaBL;
        private readonly SalaBL salaBL;
        private readonly AzureQueueClient queueClient;
        private readonly int QueueIdSistemaSomosCasino;
        private readonly string QueueWhatsApp;

        public SomosCasinoController() {
            clienteBL = new CMP_ClienteBL();
            campaniaBL = new CMP_CampañaBL();
            astClienteBL = new AST_ClienteBL();
            astTipoDocumentoBL = new AST_TipoDocumentoBL();
            astClienteSalaBL = new AST_ClienteSalaBL();
            salaBL = new SalaBL();
            queueClient = AzureQueueClient.Instance;
            QueueIdSistemaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["AzureQueueIdSistemaSomosCasino"]);
            QueueWhatsApp = ConfigurationManager.AppSettings["AzureQueueWhatsAppName"];
        }

        [SomoCasinoBearerAuthFilter]
        [HttpPost]
        public async Task<JsonResult> GenerarCodigo(ClienteSomosCasino clienteSomosCasino) {
            try {
                LimpiarValores(clienteSomosCasino);

                object erroresValidacion = ValidarCliente(clienteSomosCasino);
                if(erroresValidacion != null) {
                    return JsonResponse(new {
                        success = false,
                        displayMessage = "Errores de validación en los datos enviados.",
                        errors = erroresValidacion
                    }, HttpStatusCodes.UnprocessableEntity);
                }

                int idCliente = RegistrarOActualizarCliente(clienteSomosCasino);

                // Verificar si ya existe cliente en sala
                AST_ClienteSalaEntidad clienteSalaVerificacion = astClienteSalaBL.GetClienteSalaID(idCliente, clienteSomosCasino.CodSala);
                if(!clienteSalaVerificacion.Existe()) {
                    RegistrarClienteEnSala(clienteSomosCasino, idCliente);
                }

                // Buscar campaña activa
                CMP_CampañaEntidad campaniaActual = ObtenerCampaniaWhatsAppActiva(clienteSomosCasino.CodSala);
                if(campaniaActual == null) {
                    return JsonResponse(new {
                        success = false,
                        displayMessage = "No es posible generar código promocional debido a que no existen campañas de WhatsApp activas."
                    }, HttpStatusCodes.Conflict);
                }

                // Validar si cliente ya participa en la campania
                CMP_ClienteEntidad clienteCampania = clienteBL.BuscarClienteExistenteCmpCliente(idCliente, campaniaActual.id);
                if(clienteCampania.cliente_id > 0) {
                    return JsonResponse(new {
                        success = false,
                        displayMessage = "Cliente ya registrado en la campaña."
                    }, HttpStatusCodes.Conflict);
                }

                // Generar código promocional
                string promotionalCode = GenerarCodigoPromocional();
                int guardado = RegistrarClienteCampania(clienteSomosCasino, campaniaActual, idCliente, promotionalCode);

                // Enviar WhatsApp si corresponde
                await EnviarCodigoPorWhatsApp(clienteSomosCasino, campaniaActual, promotionalCode, guardado, idCliente);

                bool success = guardado > 0;
                string displayMessage = success
                    ? "Código promocional generado para el cliente."
                    : "Ocurrió un problema al generar el código promocional del cliente.";

                return JsonResponse(new {
                    success,
                    displayMessage,
                    promotionalCode
                }, HttpStatusCodes.Ok);
            } catch(Exception ex) {
                return JsonResponse(new {
                    success = false,
                    displayMessage = ex.Message
                }, HttpStatusCodes.InternalServerError);
            }
        }

        private void LimpiarValores(ClienteSomosCasino cliente) {
            cliente.Nombres = string.IsNullOrWhiteSpace(cliente.Nombres) ? string.Empty : cliente.Nombres.Trim().ToUpper();
            cliente.ApellidoPaterno = string.IsNullOrWhiteSpace(cliente.ApellidoPaterno) ? string.Empty : cliente.ApellidoPaterno.Trim().ToUpper();
            cliente.ApellidoMaterno = string.IsNullOrWhiteSpace(cliente.ApellidoMaterno) ? string.Empty : cliente.ApellidoMaterno.Trim().ToUpper();
            cliente.NumeroDocumento = string.IsNullOrWhiteSpace(cliente.NumeroDocumento) ? string.Empty : cliente.NumeroDocumento.Trim().ToUpper();
            cliente.FechaNacimiento = string.IsNullOrWhiteSpace(cliente.FechaNacimiento) ? string.Empty : cliente.FechaNacimiento.Trim().ToUpper();
            cliente.Genero = string.IsNullOrWhiteSpace(cliente.Genero) ? string.Empty : cliente.Genero.Trim().ToUpper();
            cliente.Celular = string.IsNullOrWhiteSpace(cliente.Celular) ? string.Empty : cliente.Celular.Trim().ToUpper();
            cliente.CodigoPais = string.IsNullOrWhiteSpace(cliente.CodigoPais) ? string.Empty : cliente.CodigoPais.Trim().ToUpper();
        }

        private object ValidarCliente(ClienteSomosCasino cliente) {
            ClienteSomosCasinoValidator validator = new ClienteSomosCasinoValidator();
            ValidationResult result = validator.Validate(cliente);

            if(!result.IsValid) {
                return result.Errors
                    .GroupBy(e => e.PropertyName)
                    .Select(g => new {
                        field = g.Key,
                        errorMessages = g.Select(e => e.ErrorMessage).ToList()
                    }).ToList();
            }
            return null;
        }

        private int RegistrarOActualizarCliente(ClienteSomosCasino cliente) {
            AST_ClienteEntidad verificacionCliente = astClienteBL.GetClientexNroyTipoDoc(cliente.IdTipoDocumento, cliente.NumeroDocumento);
            DateTime.TryParseExact(cliente.FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaNacimiento);

            if(verificacionCliente.Existe()) {
                ActualizarClienteExistente(verificacionCliente, cliente, fechaNacimiento);
                astClienteBL.EditarCliente(verificacionCliente);
                return verificacionCliente.Id;
            } else {
                return CrearNuevoCliente(cliente, fechaNacimiento);
            }
        }

        private void ActualizarClienteExistente(AST_ClienteEntidad existente, ClienteSomosCasino cliente, DateTime fechaNacimiento) {
            List<DateTime> fechasInvalidas = new List<DateTime> { new DateTime(1753, 1, 1), new DateTime(1, 1, 1) };
            existente.FechaNacimiento = existente.EsMayorDeEdad() && !fechasInvalidas.Contains(existente.FechaNacimiento.Date)
                ? existente.FechaNacimiento
                : fechaNacimiento;
            existente.Celular1 = cliente.Celular;
            existente.CodigoPais = cliente.CodigoPais;
            existente.Genero = string.IsNullOrEmpty(existente.Genero) ? cliente.Genero : existente.Genero;
            existente.TipoDocumentoId = existente.TipoDocumentoId == 0 ? cliente.IdTipoDocumento : existente.TipoDocumentoId;
            existente.Ciudadano = AST_ClienteEntidad.Ubigeo_Pais_Id.Equals(cliente.PaisId);
            existente.PaisId = cliente.PaisId;
        }

        private int CrearNuevoCliente(ClienteSomosCasino cliente, DateTime fechaNacimiento) {
            AST_ClienteEntidad clienteNuevo = new AST_ClienteEntidad {
                Nombre = cliente.Nombres,
                ApelPat = cliente.ApellidoPaterno,
                ApelMat = cliente.ApellidoMaterno,
                NombreCompleto = $"{cliente.Nombres} {cliente.ApellidoPaterno} {cliente.ApellidoMaterno}".Trim().ToUpper(),
                TipoDocumentoId = cliente.IdTipoDocumento,
                NroDoc = cliente.NumeroDocumento,
                FechaNacimiento = fechaNacimiento,
                Genero = cliente.Genero,
                UbigeoProcedenciaId = cliente.IdUbigeoProcedencia,
                PaisId = cliente.PaisId,
                Ciudadano = AST_ClienteEntidad.Ubigeo_Pais_Id.Equals(cliente.PaisId),
                Celular1 = cliente.Celular,
                CodigoPais = cliente.CodigoPais,
                Estado = "P",
                FechaRegistro = DateTime.Now,
                TipoRegistro = "CAMPAÑA WSP - SC",
                SalaId = cliente.CodSala
            };

            return astClienteBL.GuardarClienteCampaniaWhatsApp(clienteNuevo);
        }

        private void RegistrarClienteEnSala(ClienteSomosCasino cliente, int idCliente) {
            AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad {
                ClienteId = idCliente,
                SalaId = cliente.CodSala,
                TipoRegistro = "CAMPAÑA WSP - SC",
                EnviaNotificacionWhatsapp = cliente.EnviaNotificacionWhatsapp,
                EnviaNotificacionSms = cliente.EnviaNotificacionSms,
                EnviaNotificacionEmail = cliente.EnviaNotificacionEmail,
                LlamadaCelular = cliente.LlamadaCelular
            };

            astClienteSalaBL.GuardarClienteSala(clienteSala);
        }

        private CMP_CampañaEntidad ObtenerCampaniaWhatsAppActiva(int codSala) {
            List<CMP_CampañaEntidad> campaniasWhatsapp = campaniaBL.ListarCampaniasEstadoTipo(codSala, 1, 2);
            return campaniasWhatsapp.FirstOrDefault();
        }

        private string GenerarCodigoPromocional() {
            int longitudCodigoPromocional = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("CANT_CARACT_COD_PROM", 6));
            return $"SC-{clienteBL.GenerarCodigoPromocional(longitudCodigoPromocional)}";
        }

        private int RegistrarClienteCampania(ClienteSomosCasino cliente, CMP_CampañaEntidad campania, int idCliente, string codigo) {
            DateTime ahora = DateTime.Now;
            CMP_ClienteEntidad cmpCliente = new CMP_ClienteEntidad {
                cliente_id = idCliente,
                campania_id = campania.id,
                fecha_reg = ahora,
                Codigo = codigo,
                FechaGeneracionCodigo = ahora,
                FechaExpiracionCodigo = ahora.AddDays(campania.duracionCodigoDias).AddHours(campania.duracionCodigoHoras),
                CodigoEnviado = false,
                CodigoPais = cliente.CodigoPais,
                NumeroCelular = cliente.Celular,
                ProcedenciaRegistro = "LP Somos Casino",
                CodigoCanjeableEn = cliente.SalasAplicaCodigoPromocional.ToDelimitedString(),
                CodigoCanjeableMultiplesSalas = true,
                MontoRecargado = cliente.MontoRecargado
            };

            return clienteBL.GuardarClienteCampaniaJson(cmpCliente);
        }

        private async Task EnviarCodigoPorWhatsApp(ClienteSomosCasino cliente, CMP_CampañaEntidad campania, string codigo, int idClienteCampania, int idCliente) {
            CMP_ClienteEntidad clienteCampania = clienteBL.BuscarClienteExistenteCmpCliente(idCliente, campania.id);

            if(clienteCampania.EsPosibleEnviarMensajeWhatsApp() && !string.IsNullOrEmpty(campania.mensajeWhatsApp)) {
                List<SalaEntidad> salas = cliente.SalasAplicaCodigoPromocional
                    .Select(codSala => salaBL.ObtenerSalaPorCodigo(codSala))
                    .Where(s => s.Existe())
                    .ToList();

                clienteCampania.Codigo = codigo;
                clienteCampania.NombreSala = salas.Select(x => x.Nombre).ToList().JoinConjuncion();
                string whatsAppMessage = clienteCampania.ObtenerMensajeFormateadoParaEnvio(campania.mensajeWhatsApp, campania, clienteCampania);

                WhatsAppQueue whatsappQueue = new WhatsAppQueue {
                    IdSistema = QueueIdSistemaSomosCasino,
                    Mensaje = new WhatsAppQueueContentRequest {
                        CodigoPais = cliente.CodigoPais,
                        NumeroCelular = cliente.Celular,
                        Mensaje = whatsAppMessage
                    }
                };

                await queueClient.SendMessageAsync(QueueWhatsApp, whatsappQueue);
                clienteBL.MarcarCodigoEnviado(idClienteCampania);
            }
        }
    }
}