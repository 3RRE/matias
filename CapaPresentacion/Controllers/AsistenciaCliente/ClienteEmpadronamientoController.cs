using CapaDatos.Utilitarios;
using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.AsistenciaCliente {
    [seguridad]
    public class ClienteEmpadronamientoController : Controller {
        private SalaBL salaBl = new SalaBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private AST_TipoDocumentoBL ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
        private AST_ClienteSalaBL ast_clienteSalaBL = new AST_ClienteSalaBL();
        private EMC_EmpadronamientoClienteBL empadronamientoBL = new EMC_EmpadronamientoClienteBL();
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        public ActionResult FormularioRegistro() {
            return View("~/Views/AsistenciaCliente/EmpadronamientoFormulario.cshtml");
        }


        [HttpPost]
        public ActionResult GuardarEmpadronamientoCliente(EMC_EmpadronamientoClienteEntidad empadronamiento) {
            string mensaje = "";
            bool respuesta = false;
            int idInsertado = 0;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    sala = listaSalas.FirstOrDefault();
                    DateTime hoy = DateTime.Now;
                    var registro = empadronamientoBL.GetEmpadronamientoCliente(hoy, empadronamiento.NroDoc);
                    if(registro.id == 0) {

                        empadronamiento.cod_sala = sala.CodSala;
                        empadronamiento.fecha = DateTime.Now;
                        empadronamiento.usuario_id = usuarioId;
                        respuesta = empadronamientoBL.GuardarEmpadronamientoCliente(empadronamiento);
                        if(respuesta) {
                            mensaje = "Registro Insertado";
                        } else {
                            mensaje = "No se pudo registrar";
                        }
                    } else {
                        int sala__ = registro.cod_sala;
                        var nombersala = "Sin Nombre";
                        if(sala__ > 0) {
                            var salaentidad = salaBl.SalaListaIdJson(sala__);
                            nombersala = salaentidad.Nombre;
                        }
                        mensaje = "El Nro de Documento ya se encuentra registrado , Con fecha : " + registro.fecha.ToString("dd/MM/yyyy hh:mm tt") + " , en la sala : " + nombersala;
                        respuesta = false;
                    }
                } else {
                    if(listaSalas.Count > 1) {

                        mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                    } else {

                        mensaje = "Usted debe tener asignado al menos una sala para poder realizar la acción";
                    }

                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, idInsertado });
        }

        [HttpPost]
        public ActionResult GetListadoEmpadronamientoSalaFiltros(DtParameters dtParameters, int salaid, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = "";
            List<EMC_EmpadronamientoClienteEntidad> lista = new List<EMC_EmpadronamientoClienteEntidad>();
            object oRespuesta = new object();

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            var count = 0;
            List<dynamic> registro = new List<dynamic>();
            try {
                lista = empadronamientoBL.GetListadoEmpadronamientoCliente(fechaIni, fechaFin, salaid);

                String busqueda = "";
                bool respuestaController = false;
                string accion = "EliminarEmpadronamiento";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + (int)Session["rol"] +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"

                                                                         );
                if(busqueda.Length < 3) {
                    respuestaController = false;
                } else {
                    respuestaController = true;
                }



                if(dtParameters.Order != null) {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                } else {
                    orderCriteria = "id";
                    orderAscendingDirection = false;
                }
                count = lista.Count();
                if(!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy))) {
                    lista = lista.Where(x => x.ApelMat.ToLower().Contains(searchBy.ToLower())
                                                  || x.ApelPat.ToLower().Contains(searchBy.ToLower())
                                                  || x.Nombre.ToLower().Contains(searchBy.ToLower())
                                                  || x.NombreCompleto.ToLower().Contains(searchBy.ToLower())
                                                    || x.NroDoc.ToLower().Contains(searchBy.ToLower())
                                                  || x.NombreSala.ToLower().Contains(searchBy.ToLower())
                                                  || x.fecha.ToString("dd/MM/yyyy").Contains(searchBy.ToLower())
                                                  || x.UsuarioNombre.ToLower().Contains(searchBy.ToLower())
                                                  ).ToList();
                }

                lista = orderAscendingDirection ? lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

                var filteredResultsCount = lista.Count();
                var totalResultsCount = count;

                registro.Add(new {
                    mensaje = "Listando Registros",
                    dataPermiso = respuestaController,
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = lista.Skip(dtParameters.Start)
                    .Take(dtParameters.Length).ToList()
                });

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(registro.FirstOrDefault()),
                ContentType = "application/json"
            };
            return result;
        }


        [HttpPost]
        public ActionResult ReporteCampaniasDescargarExcelJson(int salaid, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<EMC_EmpadronamientoClienteEntidad> lista = new List<EMC_EmpadronamientoClienteEntidad>();

            try {
                var salat = salaBl.SalaListaIdJson(salaid);

                lista = empadronamientoBL.GetListadoEmpadronamientoCliente(fechaIni, fechaFin, salaid);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Lista Empadronamiento");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALA : " + salat.Nombre;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Cliente";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Nro. Documento";
                    workSheet.Cells[3, 6].Value = "Fecha";

                    workSheet.Cells[3, 7].Value = "Entrego DNI";
                    workSheet.Cells[3, 8].Value = "Fuente";
                    workSheet.Cells[3, 9].Value = "Observacion";

                    workSheet.Cells[3, 10].Value = "Usuario";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.id;
                        workSheet.Cells[recordIndex, 3].Value = registro.NombreCompleto;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreSala;

                        workSheet.Cells[recordIndex, 5].Value = registro.NroDoc;
                        workSheet.Cells[recordIndex, 6].Value = registro.fecha.ToString("dd-MM-yyyy hh:mm:ss tt");

                        workSheet.Cells[recordIndex, 7].Value = registro.entrega_dni ? "SI" : "NO";
                        workSheet.Cells[recordIndex, 8].Value = registro.reniec ? "RENIEC" : "BD Local";
                        workSheet.Cells[recordIndex, 9].Value = registro.observacion;

                        workSheet.Cells[recordIndex, 10].Value = registro.UsuarioNombre;

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

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    workSheet.Cells["B2:j2"].Merge = true;
                    workSheet.Cells["B2:j2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;

                    workSheet.Cells["B" + filaFooter + ":j" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":j" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":j" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":j" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":j" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":j" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 10].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 28;
                    workSheet.Column(4).Width = 34;
                    workSheet.Column(5).Width = 38;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 15;
                    workSheet.Column(8).Width = 15;
                    workSheet.Column(9).Width = 25;
                    workSheet.Column(10).Width = 20;
                    excelName = "Empadronamiento_" + fechaIni.ToString("dd_MM_yyyy") + "_al_" + fechaFin.ToString("dd_MM_yyyy") + "_.xlsx";
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
        public ActionResult EliminarEmpadronamiento(int id) {
            string mensaje = "";
            bool respuesta = false;
            try {
                respuesta = empadronamientoBL.EliminarEmpadronamientoCliente(id);
                mensaje = "Registro Eliminado";
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> buscarPersonaJson(string nro_documento, Int32 usuario_id) {
            ApiReniec _apiReniec = new ApiReniec();
            string mensaje = "";
            bool respuesta = false;
            AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();

            EMP_respuestaAPICONSULTA data = new EMP_respuestaAPICONSULTA();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuario_id).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    clienteConsulta = ast_ClienteBL.GetListaClientesxNroDoc(nro_documento).FirstOrDefault();

                    if(clienteConsulta == null) {
                        var dataClienteAPI = await _apiReniec.Busqueda(nro_documento);
                        if(dataClienteAPI.Respuesta) {
                            data = new EMP_respuestaAPICONSULTA {
                                NombreCompleto = dataClienteAPI.NombreCompleto,
                                Nombre = dataClienteAPI.Nombre,
                                ApellidoPaterno = dataClienteAPI.ApellidoPaterno,
                                ApellidoMaterno = dataClienteAPI.ApellidoMaterno,
                                DNI = dataClienteAPI.DNI,
                                cliente_id = 0
                            };
                            respuesta = true;
                        } else {
                            data.cliente_id = -1;
                            respuesta = false;
                            mensaje = "Apis (Consulta PE o ApisPeru no respondieron, Ingrese Informacion Manualmente - web)";

                        }

                    } else {


                        data = new EMP_respuestaAPICONSULTA {
                            NombreCompleto = clienteConsulta.NombreCompleto,
                            Nombre = clienteConsulta.Nombre,
                            ApellidoPaterno = clienteConsulta.ApelPat,
                            ApellidoMaterno = clienteConsulta.ApelMat,
                            DNI = clienteConsulta.NroDoc,
                            cliente_id = clienteConsulta.Id
                        };
                        respuesta = true;

                        //DateTime hoy = DateTime.Now;
                        //EMC_EmpadronamientoClienteEntidad registro = empadronamientoBL.GetEmpadronamientoCliente(hoy, nro_documento);
                        //if (registro.id == 0){

                        //    data = new EMP_respuestaAPICONSULTA
                        //    {
                        //        NombreCompleto = clienteConsulta.NombreCompleto,
                        //        Nombre = clienteConsulta.Nombre,
                        //        ApellidoPaterno = clienteConsulta.ApelPat,
                        //        ApellidoMaterno = clienteConsulta.ApelMat,
                        //        DNI = clienteConsulta.NroDoc,
                        //        cliente_id = clienteConsulta.Id
                        //    };
                        //    respuesta = true;
                        //}
                        //else
                        //{
                        //    int sala__ = registro.cod_sala;
                        //    var nombersala = "Sin Nombre";
                        //    if (sala__ > 0)
                        //    {
                        //        var salaentidad = salaBl.SalaListaIdJson(sala__);
                        //        nombersala = salaentidad.Nombre;
                        //    }
                        //    mensaje = "El Nro de Documento ya se encuentra registrado ,\n Con fecha : " + registro.fecha.ToString("dd/MM/yyyy hh:mm tt") + " , en la sala : " + nombersala;
                        //    respuesta = false;
                        //}
                    }
                } else {
                    if(listaSalas.Count > 1) {

                        mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                    } else {

                        mensaje = "Usted debe tener asignado al menos una sala para poder realizar la acción";
                    }

                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data });
        }

        [seguridad(false)]
        public dynamic apireniec(string dni) {
            string vtoken = WebConfigurationManager.AppSettings["Token"];
            var rpta = false;

            //string json = "";
            dynamic item = new DynamicDictionary();
            List<dynamic> Lista = new List<dynamic>();
            try {
                #region apiperu

                string uri = "https://apiperu.dev/api/dni/" + dni;
                var clientApi = new RestClient(uri);
                var requestApi = new RestRequest(Method.GET);
                requestApi.AddHeader("Accept", "application/json");
                requestApi.AddHeader("Authorization", "Bearer " + "d2a43838fa0ba5f9f3c891d801b94cdf86839eded828bf85d6dbfdbf8b9cef19");

                try {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    IRestResponse responseApi = clientApi.Execute(requestApi);
                    dynamic oDataApiReal = JsonConvert.DeserializeObject(responseApi.Content);
                    dynamic oDataApi = oDataApiReal.data;
                    if(oDataApi != null) {
                        if(oDataApi.numero != null) {

                            item.NombreCompleto = oDataApi.nombre_completo;
                            item.DNI = oDataApi.numero;
                            item.Nombre = oDataApi.nombres;
                            item.ApellidoPaterno = oDataApi.apellido_paterno;
                            item.ApellidoMaterno = oDataApi.apellido_materno;
                            rpta = true;
                        }

                    }

                } catch(Exception op1) {
                    Console.WriteLine("apiperu" + op1.Message);

                }



                #endregion

                #region apiperu
                if(!rpta) {
                    uri = "https://apiperu.dev/api/dni/" + dni;
                    clientApi = new RestClient(uri);
                    requestApi = new RestRequest(Method.GET);
                    requestApi.AddHeader("Accept", "application/json");
                    requestApi.AddHeader("Authorization", "Bearer " + "71dd9c2a2d474859794cdb4bd251652951c817a2fa1ff48110c02e8a4e0db2f5");
                    try {
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        IRestResponse responseApi = clientApi.Execute(requestApi);
                        dynamic oDataApiReal = JsonConvert.DeserializeObject(responseApi.Content);
                        dynamic oDataApi = oDataApiReal.data;
                        if(oDataApi != null) {
                            if(oDataApi.numero != null) {


                                item.NombreCompleto = oDataApi.nombre_completo;
                                item.DNI = oDataApi.numero;
                                item.Nombre = oDataApi.nombres;
                                item.ApellidoPaterno = oDataApi.apellido_paterno;
                                item.ApellidoMaterno = oDataApi.apellido_materno;
                                rpta = true;
                            }

                        }

                    } catch(Exception op1) {
                        Console.WriteLine("apiperu" + op1.Message);

                    }
                }

                #endregion



                #region consultas.pe
                if(!rpta) {
                    string url = "https://consulta.pe/api/reniec/dni";
                    var client = new RestClient(url);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Authorization", "Bearer " + vtoken);
                    request.AddParameter("dni", dni);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    IRestResponse response = client.Execute(request);
                    dynamic oData = JsonConvert.DeserializeObject(response.Content);
                    if(oData != null) {
                        if(oData.dni != null) {

                            item.NombreCompleto = oData.nombres + " " + oData.apellido_paterno + " " + oData.apellido_materno;
                            item.DNI = dni;
                            item.Nombre = oData.nombres;
                            item.ApellidoPaterno = oData.apellido_paterno;
                            item.ApellidoMaterno = oData.apellido_materno;
                            rpta = true;
                        } else {
                            item.NombreCompleto = "Cliente No Encontrado";
                            item.DNI = "";
                            item.Nombre = "";
                            item.ApellidoPaterno = "";
                            item.ApellidoMaterno = "";
                        }
                    } else {
                        item.NombreCompleto = "Cliente No Encontrado";
                        item.DNI = "";
                        item.Nombre = "";
                        item.ApellidoPaterno = "";
                        item.ApellidoMaterno = "";
                    }
                }

                #endregion



            } catch(Exception e) {
                Console.WriteLine("apiperu" + e.Message);
                item.NombreCompleto = "Cliente No Encontrado";
                item.DNI = "";
                item.Nombre = "";
                item.ApellidoPaterno = "";
                item.ApellidoMaterno = "";
            }
            Lista.Add(item);
            return Lista;
            // return Json(new { data = item, mensaje = "" }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> ConsultaEmpadroanmientoSalida(string nro_documento, Int32 usuario_id) {


            // 0 : Nuevo cliente 
            // 1: Marcar entrada
            // 2: Marcar salida
            // 3: Ya marco salida y entrada el dia de hoy

            long empadronamientoId = 0;
            int salidaEntrada = 0;
            ApiReniec _apiReniec = new ApiReniec();
            string mensaje = "No se encontro el numero de documento, registrar nuevo cliente";
            bool respuesta = false;
            AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();

            EMP_respuestaAPICONSULTA data = new EMP_respuestaAPICONSULTA();

            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuario_id).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    clienteConsulta = ast_ClienteBL.GetListaClientesxNroDoc(nro_documento).FirstOrDefault();

                    if(clienteConsulta == null) {
                        var dataClienteAPI = await _apiReniec.Busqueda(nro_documento);
                        if(dataClienteAPI.Respuesta) {
                            data = new EMP_respuestaAPICONSULTA {
                                NombreCompleto = dataClienteAPI.NombreCompleto,
                                Nombre = dataClienteAPI.Nombre,
                                ApellidoPaterno = dataClienteAPI.ApellidoPaterno,
                                ApellidoMaterno = dataClienteAPI.ApellidoMaterno,
                                DNI = dataClienteAPI.DNI,
                                cliente_id = 0
                            };
                            respuesta = true;
                            mensaje = "Nuevo cliente, se encontro dni en Consulta ApisPeru";
                        } else {
                            data.cliente_id = -1;
                            respuesta = false;
                            mensaje = "Nuevo cliente, Apis (Consulta PE o ApisPeru no respondieron, Ingrese Informacion Manualmente - web)";
                        }

                    } else {
                        EMC_EmpadronamientoClienteEntidad emcCustomer = new EMC_EmpadronamientoClienteEntidad();
                        DateTime todayDate = DateTime.Now;

                        emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(clienteConsulta.Id, todayDate);

                        if(emcCustomer.id == 0) {
                            //Cliente encontrado y se mostrara la pantalla de marcar entrada
                            salidaEntrada = 1;
                            mensaje = "Se encontro empadronamiento el dia de hoy, ¿marcar entrada? ";
                        }
                        if(emcCustomer.id > 0) {



                            if(emcCustomer.Estado == 0) {
                                empadronamientoId = emcCustomer.id;
                                //Se encontro el registro (marco entrada)- muestra la pantalla de marcar salida
                                salidaEntrada = 2;
                                mensaje = $"El número de documento {emcCustomer.NroDoc} se encuentra registrado con fecha y hora {emcCustomer.fecha.ToString("dd/MM/yyyy hh:mm tt")} en {emcCustomer.NombreSala}";
                            } else {
                                //Ya marco entrada y salida el dia de hoy y no podra hacerla otra vez
                                salidaEntrada = 3;
                                mensaje = $"El número de documento {emcCustomer.NroDoc} ya tiene un empadronamiento para la fecha de hoy {emcCustomer.fecha.ToString("dd/MM/yyyy")} en {emcCustomer.NombreSala} ya no puede marcar empadronamiento el dia de hoy";
                            }
                        }

                        data = new EMP_respuestaAPICONSULTA {
                            NombreCompleto = clienteConsulta.NombreCompleto,
                            Nombre = clienteConsulta.Nombre,
                            ApellidoPaterno = clienteConsulta.ApelPat,
                            ApellidoMaterno = clienteConsulta.ApelMat,
                            DNI = clienteConsulta.NroDoc,
                            cliente_id = clienteConsulta.Id
                        };
                        respuesta = true;


                    }
                } else {
                    if(listaSalas.Count > 1) {

                        mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                    } else {

                        mensaje = "Usted debe tener asignado al menos una sala para poder realizar la acción";
                    }

                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data, salidaEntrada, empadronamientoId });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarEmpadronamientoMobiljson(EMC_EmpadronamientoMobil empadronamiento) {
            string mensaje = "";
            int idInsertado = 0;
            int response = 0;
            string message = "Los datos no se pudieron registrar";
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            EMC_EmpadronamientoClienteEntidad emcCustomer = new EMC_EmpadronamientoClienteEntidad();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(empadronamiento.usuario_id).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    sala = listaSalas.FirstOrDefault();


                    if(empadronamiento.cliente_id == 0) {
                        AST_ClienteEntidad astCliente = new AST_ClienteEntidad();

                        astCliente.TipoDocumentoId = 1;
                        astCliente.NroDoc = empadronamiento.nroDoc;
                        astCliente.Nombre = empadronamiento.nombre;
                        astCliente.ApelPat = empadronamiento.paterno;
                        astCliente.ApelMat = empadronamiento.materno;
                        astCliente.NombreCompleto = $"{empadronamiento.paterno} {empadronamiento.materno} {empadronamiento.nombre}";
                        astCliente.Estado = "P";
                        astCliente.SalaId = sala.CodSala;
                        astCliente.usuario_reg = empadronamiento.usuario_id;
                        astCliente.TipoRegistro = "EMPADRONAMIENTO";

                        empadronamiento.cliente_id = ast_ClienteBL.GuardarClienteEmpadronamientoV2(astCliente);
                    }

                    DateTime todayDate = DateTime.Now;
                    emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(empadronamiento.cliente_id, todayDate);

                    if(emcCustomer.id > 0) {
                        if(emcCustomer.Estado == 0) {
                            response = 2;
                            mensaje = $"El número de documento {emcCustomer.NroDoc} se encuentra registrado con fecha y hora {emcCustomer.fecha.ToString("dd/MM/yyyy hh:mm tt")} en {emcCustomer.NombreSala}";
                        } else {
                            response = 3;
                            mensaje = $"El número de documento {emcCustomer.NroDoc} ya tiene un empadronamiento para la fecha de hoy {emcCustomer.fecha.ToString("dd/MM/yyyy")} en {emcCustomer.NombreSala}";
                        }
                    } else {

                        EMC_EmpadronamientoClienteEntidad empadronamiento_ = new EMC_EmpadronamientoClienteEntidad();
                        empadronamiento_.cliente_id = empadronamiento.cliente_id;
                        empadronamiento_.cod_sala = sala.CodSala;
                        empadronamiento_.usuario_id = empadronamiento.usuario_id;
                        empadronamiento_.observacion = empadronamiento.observacion;
                        empadronamiento_.entrega_dni = empadronamiento.entrega_dni;
                        empadronamiento_.apuestaImportante = empadronamiento.apuesta;
                        empadronamiento_.reniec = empadronamiento.reniec;
                        empadronamiento_.fecha = DateTime.Now;
                        empadronamiento_.Estado = 0;
                        empadronamiento_.ZonaIdIn = empadronamiento.zona_id;
                        empadronamiento_.RegistroEntrada = 3;

                        long empadronamientoId = empadronamientoBL.GuardarEmpadronamientoClienteV2(empadronamiento_);

                        if(empadronamientoId > 0) {
                            emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(empadronamientoId);

                            response = 1;
                            mensaje = "Los datos se han registrado";
                        }
                    }



                } else {
                    if(listaSalas.Count > 1) {

                        mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                    } else {

                        mensaje = "Usted debe tener asignado al menos una sala para poder realizar la acción";
                    }

                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta = response, data = emcCustomer });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult RegistrarFechaHoraSalidaMobiljson(int empadronamientoId, int usuario_id) {
            bool response = false;
            string mensaje = "No se pudo registrar la fecha y hora de salida";

            EMC_EmpadronamientoClienteEntidad emcCustomer = new EMC_EmpadronamientoClienteEntidad();

            try {
                emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(empadronamientoId);

                TimeSpan clockTime = new TimeSpan(7, 0, 0);
                DateTime clockDate = DateTime.Now;
                DateTime currentDate = DateTime.Now;
                DateTime fechaSalida = DateTime.Now;

                if(emcCustomer.id > 0) {
                    clockDate = currentDate.Date.Add(clockTime);
                    fechaSalida = clockDate;

                    if(emcCustomer.fecha.Date.Equals(currentDate.Date) && emcCustomer.fecha >= clockDate) {
                        clockDate = currentDate.AddDays(1).Date.Add(clockTime);
                    }

                    if(currentDate < clockDate) {
                        fechaSalida = currentDate;
                    }
                }

                EMC_EmpadronamientoClienteEntidad empadronamiento = new EMC_EmpadronamientoClienteEntidad();
                empadronamiento.id = empadronamientoId;
                empadronamiento.FechaSalida = fechaSalida;
                empadronamiento.Estado = 1;
                empadronamiento.UsuarioIdOut = usuario_id;
                empadronamiento.RegistroSalida = 3;
                response = empadronamientoBL.RegistrarFechaHoraSalida(empadronamiento);
                if(response) {
                    mensaje = "Se registro la fecha y hora de salida";
                } else {
                    mensaje = "No se pudo registrar Empadronamiento";
                }
            } catch(Exception exception) {
                response = false;
                mensaje = exception.Message;
            }

            return Json(new {
                respuesta = response,
                mensaje,
            });
        }

        #region Empadronamimento Cliente V2

        // Empadronamiento Cliente V2
        public ActionResult RegistroEmpadronamiento() {
            return View("~/Views/AsistenciaCliente/RegistroEmpadronamiento.cshtml");
        }

        public ActionResult ReporteEmpadronamiento() {
            return View("~/Views/AsistenciaCliente/ReporteEmpadronamiento.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> BuscarEmpadronamientoCliente(string documentNumber) {
            int status = 0;
            string message = $"No se encontro registros con el número de documento {documentNumber}";
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            bool fromReniec = false;
            dynamic data = new ExpandoObject();

            if(usuarioId == 0) {
                return Json(new {
                    status = 3,
                    message = "Por favor, ingrese un identificador de usuario"
                });
            }

            List<SalaEntidad> listaSalasAsignadas = salaBl.ListadoSalaPorUsuario(usuarioId);

            if(listaSalasAsignadas.Count > 1) {
                return Json(new {
                    status = 3,
                    message = "Usted debe tener asignado solo una sala para poder realizar la acción"
                });
            }

            if(listaSalasAsignadas.Count == 0) {
                return Json(new {
                    status = 3,
                    message = "Usted debe tener asignado al menos una sala para poder realizar la acción"
                });
            }

            if(string.IsNullOrEmpty(documentNumber)) {
                return Json(new {
                    status = 3,
                    message = "Por favor, ingrese un número de documento"
                });
            }

            try {
                EMP_respuestaAPICONSULTA EMC_Customer = new EMP_respuestaAPICONSULTA();
                EMC_EmpadronamientoClienteEntidad EMC_Empadronamiento = new EMC_EmpadronamientoClienteEntidad();
                AST_ClienteEntidad cliente = new AST_ClienteEntidad();

                cliente = ast_ClienteBL.GetClientexNroDoc(documentNumber);

                if(cliente.Id > 0) {
                    DateTime todayDate = DateTime.Now;
                    EMC_EmpadronamientoClienteEntidad empadronamiento = empadronamientoBL.ObtenerEmpadronamientoCliente(cliente.Id, todayDate);

                    if(empadronamiento.id > 0) {
                        EMC_Empadronamiento = empadronamiento;

                        data = EMC_Empadronamiento;

                        if(empadronamiento.Estado == 0) {
                            status = 1;
                            message = $"El número de documento {documentNumber} se encuentra registrado con fecha y hora {empadronamiento.fecha.ToString("dd/MM/yyyy hh:mm tt")} en {empadronamiento.NombreSala}";
                        } else {
                            status = 3;
                            message = $"El número de documento {documentNumber} ya tiene un empadronamiento para la fecha de hoy {empadronamiento.fecha.ToString("dd/MM/yyyy")} en {empadronamiento.NombreSala}";
                        }
                    } else {
                        EMC_Customer = new EMP_respuestaAPICONSULTA {
                            NombreCompleto = cliente.NombreCompleto,
                            Nombre = cliente.Nombre,
                            ApellidoPaterno = cliente.ApelPat,
                            ApellidoMaterno = cliente.ApelMat,
                            DNI = cliente.NroDoc,
                            cliente_id = cliente.Id
                        };

                        data = EMC_Customer;

                        status = 2;
                        message = "Número de documento no se encuentra registrado en Empadronamiento";
                    }
                } else {
                    ApiReniec _apiReniec = new ApiReniec();

                    ApiReniecResponse apiReniecResponse = await _apiReniec.Busqueda(documentNumber);

                    if(apiReniecResponse.Respuesta) {
                        int salaId = 0;
                        List<SalaEntidad> listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();

                        if(listaSalas.Count > 0) {
                            SalaEntidad sala = listaSalas.FirstOrDefault();
                            salaId = sala.CodSala;
                        }

                        AST_ClienteEntidad astCliente = new AST_ClienteEntidad();

                        astCliente.TipoDocumentoId = 1;
                        astCliente.NroDoc = apiReniecResponse.DNI;
                        astCliente.Nombre = apiReniecResponse.Nombre;
                        astCliente.ApelPat = apiReniecResponse.ApellidoPaterno;
                        astCliente.ApelMat = apiReniecResponse.ApellidoMaterno;
                        astCliente.NombreCompleto = apiReniecResponse.NombreCompleto;
                        astCliente.Estado = "P";
                        astCliente.SalaId = salaId;
                        astCliente.usuario_reg = usuarioId;
                        astCliente.TipoRegistro = "EMPADRONAMIENTO";

                        int insertedId = ast_ClienteBL.GuardarClienteEmpadronamientoV2(astCliente);

                        if(insertedId > 0) {
                            cliente = ast_ClienteBL.ObtenerClienteById(insertedId);

                            if(cliente.Id > 0) {
                                EMC_Customer = new EMP_respuestaAPICONSULTA {
                                    NombreCompleto = cliente.NombreCompleto,
                                    Nombre = cliente.Nombre,
                                    ApellidoPaterno = cliente.ApelPat,
                                    ApellidoMaterno = cliente.ApelMat,
                                    DNI = cliente.NroDoc,
                                    cliente_id = cliente.Id
                                };

                                data = EMC_Customer;

                                status = 2;
                                message = "Número de documento no se encuentra registrado en Empadronamiento";
                                fromReniec = true;
                            }
                        }
                    } else {
                        status = 0;
                        message = "Data no encontrada, contacte al jefe de sala para el registro manual del cliente";
                    }
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data,
                fromReniec
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarEmpadronamientoClienteV2(EMC_EmpadronamientoClienteEntidad empadronamiento) {
            int status = 0;
            string message = "Los datos no se pudieron registrar";
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);

            EMC_EmpadronamientoClienteEntidad emcCustomer = new EMC_EmpadronamientoClienteEntidad();

            try {
                int clienteId = Convert.ToInt32(empadronamiento.cliente_id);

                DateTime todayDate = DateTime.Now;
                emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(clienteId, todayDate);

                if(emcCustomer.id > 0) {
                    status = 2;
                    message = $"El número de documento {emcCustomer.NroDoc} ya tiene un empadronamiento para la fecha de hoy {emcCustomer.fecha.ToString("dd/MM/yyyy")} en {emcCustomer.NombreSala}";
                } else {
                    empadronamiento.fecha = DateTime.Now;
                    empadronamiento.usuario_id = usuarioId;
                    empadronamiento.Estado = 0;
                    empadronamiento.RegistroEntrada = 1;

                    long empadronamientoId = empadronamientoBL.GuardarEmpadronamientoClienteV2(empadronamiento);

                    if(empadronamientoId > 0) {
                        emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(empadronamientoId);

                        status = 1;
                        message = "Los datos se han registrado";
                    }
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data = emcCustomer
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult RegistrarFechaHoraSalida(int empadronamientoId) {
            int status = 0;
            string message = "No se pudo registrar la fecha y hora de salida";
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);

            EMC_EmpadronamientoClienteEntidad emcCustomer = new EMC_EmpadronamientoClienteEntidad();

            try {
                emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(empadronamientoId);

                TimeSpan clockTime = new TimeSpan(7, 0, 0);
                DateTime clockDate = DateTime.Now;
                DateTime currentDate = DateTime.Now;
                DateTime fechaSalida = DateTime.Now;

                if(emcCustomer.id > 0) {
                    clockDate = currentDate.Date.Add(clockTime);
                    fechaSalida = clockDate;

                    if(emcCustomer.fecha.Date.Equals(currentDate.Date) && emcCustomer.fecha >= clockDate) {
                        clockDate = currentDate.AddDays(1).Date.Add(clockTime);
                    }

                    if(currentDate < clockDate) {
                        fechaSalida = currentDate;
                    }
                }

                EMC_EmpadronamientoClienteEntidad empadronamiento = new EMC_EmpadronamientoClienteEntidad();
                empadronamiento.id = empadronamientoId;
                empadronamiento.FechaSalida = fechaSalida;
                empadronamiento.Estado = 1;
                empadronamiento.UsuarioIdOut = usuarioId;
                empadronamiento.RegistroSalida = 1;

                if(empadronamientoBL.RegistrarFechaHoraSalida(empadronamiento)) {
                    emcCustomer = empadronamientoBL.ObtenerEmpadronamientoCliente(empadronamientoId);

                    status = 1;
                    message = "Se registro la fecha y hora de salida";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data = emcCustomer
            });
        }

        [HttpPost]
        public ActionResult ListarEmpadronamientoCliente(int roomId, string fromDate, string toDate) {
            int status = 0;
            string message = "No se encontraron registros";

            if(roomId == 0) {
                return Json(new {
                    status = 2,
                    message = "Por favor, seleccione una sala"
                });
            }

            if(string.IsNullOrEmpty(fromDate)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha inicio"
                });
            }

            if(string.IsNullOrEmpty(toDate)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha fin"
                });
            }

            List<EMC_EmpadronamientoClienteEntidad> listReport = new List<EMC_EmpadronamientoClienteEntidad>();

            try {
                listReport = empadronamientoBL.ListarEmpadronamientoCliente(roomId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));

                status = 1;
                message = "Datos obtenidos";
            } catch(Exception exception) {
                message = exception.Message;
            }

            var resultData = new {
                status,
                message,
                data = listReport,
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

        [HttpPost]
        public ActionResult ExcelEmpadronamientoCliente(int roomId, string fromDate, string toDate) {
            int status = 0;
            string message = "No se encontraron registros";

            if(roomId == 0) {
                return Json(new {
                    status = 2,
                    message = "Por favor, seleccione una sala"
                });
            }

            if(string.IsNullOrEmpty(fromDate)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha inicio"
                });
            }

            if(string.IsNullOrEmpty(toDate)) {
                return Json(new {
                    status = 2,
                    message = "Por favor, ingrese una fecha fin"
                });
            }

            SalaEntidad sala = salaBl.ObtenerSalaPorCodigo(roomId);

            if(sala.CodSala == 0) {
                return Json(new {
                    status = 2,
                    message = "Por favor, seleccione una sala"
                });
            }

            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;
            DateTime fromDateTime = Convert.ToDateTime(fromDate);
            DateTime toDateTime = Convert.ToDateTime(toDate);

            try {
                List<EMC_EmpadronamientoClienteEntidad> listEmpadronamiento = empadronamientoBL.ListarEmpadronamientoCliente(roomId, fromDateTime, toDateTime);

                // Data Excel
                dynamic arguments = new {
                    roomName = sala.Nombre,
                    fromDate,
                    toDate
                };

                MemoryStream memoryStream = ReportesHelper.ExcelEmpadronamientoCliente(listEmpadronamiento, arguments);
                fileName = $"Empadronamiento Cliente - {sala.Nombre} {fromDateTime.ToString("dd-MM-yyyy")} al {toDateTime.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

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
                data
            });
        }

        #endregion
    }
}
