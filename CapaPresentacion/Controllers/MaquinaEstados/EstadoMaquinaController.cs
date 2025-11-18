using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;
using CapaDatos.EntradaSalidaSala;
using CapaEntidad;
using CapaEntidad.EntradaSalidaSala;
using CapaEntidad.MaquinaEstado;
using CapaEntidad.Reclamaciones;
using CapaEntidad.Sala;
using CapaNegocio;
using CapaNegocio.EntradaSalidaSala;
using CapaNegocio.EstadoMaquinas;
using CapaNegocio.Reclamaciones;
using CapaNegocio.Sala;
using CapaPresentacion.Controllers;
using CapaPresentacion.Utilitarios;
using Microsoft.AspNet.Identity;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QRCoder;
using Rotativa;
using S3k.Utilitario.clases_especial;



namespace CapaPresentacion.Controllers.MaquinaEstados
{
    [seguridad]
    public class EstadoMaquinaController : Controller {
        // GET: MaquinaEstados

        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        TEC_EstadoMaquinaBL estadomaquinabl = new TEC_EstadoMaquinaBL();
        DestinatarioBL destinatariobl = new DestinatarioBL();
        ClaseError error = new ClaseError();


        public ActionResult MaquinaEstadosVista() {
            return View("~/Views/MaquinaEstados/EstadoMaquina.cshtml");
        }
        [seguridad(false)]
        public ActionResult RegistroMaquinaVista() {
            return View("~/Views/MaquinaEstados/RegistroMaquina.cshtml");
        }
      
        public ActionResult ReporteRegistroMaquinaVista() {
            return View("~/Views/MaquinaEstados/ReporteRegistroMaquina.cshtml");
        }



        [HttpPost]
        public ActionResult ListaMaquinaxSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<TEC_EstadoMaquinaEntidad> listamaquinaestado = new List<TEC_EstadoMaquinaEntidad>();
            TEC_estadomaquinaLista registros = new TEC_estadomaquinaLista();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            
            var strElementos = String.Empty; //codsala:[0]  codsala[0]=0
             try {
                
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                var ListadoUltimaFechaOperacionxSala = estadomaquinabl.ObtenerFechaOperacionUltimaxSala(salasBusqueda);
                var maquinaestadoTupla = estadomaquinabl.maquinaestadoListarxSalaFechaJson(salasBusqueda, fechaini, fechafin);
                registros.lista = maquinaestadoTupla.maquinaestadoLista;

                if(registros.lista.Count > 0 && ListadoUltimaFechaOperacionxSala.Count > 0) {
                    foreach(var item in registros.lista) {
                        var ultimaFechaOperacion = ListadoUltimaFechaOperacionxSala
                                            .FirstOrDefault(x => x.CodSala == item.sala_id);
                        if(ultimaFechaOperacion.FechaOperacion.Date == item.FechaOperacion.Date) {
                            item.Accion = 1; 
                        } else {
                            item.Accion = 0; 
                        }
                    }
                } 
                TEC_ConsolidadoMaquina sumando = new TEC_ConsolidadoMaquina();
                if(registros.lista.Count > 0) {
                    sumando.TotalConectadas = registros.lista.Sum(x => x.CantMaquinaConectada);
                    sumando.TotalDesconectadas = registros.lista.Sum(x => x.CantMaquinaNoConectada);
                    sumando.TotalMaquinaPLay = registros.lista.Sum(x => x.CantMaquinaPLay);
                    sumando.TotalRetiroTemporal = registros.lista.Sum(x => x.CantMaquinaRetiroTemporal);
                    sumando.TotalMaquinas = registros.lista.Sum(x => x.TotalMaquina);
                }
                
                registros.consolidado = sumando;


                if(error.Key.Equals(string.Empty)) {
                    mensaje = "Listando";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudieron listar";
                }

            } catch(Exception exp) {
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = registros, respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListaMaquinaxSalaxJson(int[] codsala) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<TEC_EstadoMaquinaEntidad> listamaquinaestado = new List<TEC_EstadoMaquinaEntidad>();
            TEC_estadomaquinaLista registros = new TEC_estadomaquinaLista();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            
            var strElementos = String.Empty; //codsala:[0]  codsala[0]=0
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }

                var maquinaestadoTupla = estadomaquinabl.maquinaestadoListarxSalaUltimaFechaOperacionJson(salasBusqueda);
                error = maquinaestadoTupla.error;
                registros.lista = maquinaestadoTupla.maquinaestadoLista;

                TEC_ConsolidadoMaquina sumando = new TEC_ConsolidadoMaquina();

                sumando.TotalConectadas = registros.lista.Sum(x => x.CantMaquinaConectada);
                sumando.TotalDesconectadas = registros.lista.Sum(x => x.CantMaquinaNoConectada);
                sumando.TotalMaquinaPLay = registros.lista.Sum(x => x.CantMaquinaPLay);
                sumando.TotalMaquinas = registros.lista.Sum(x => x.TotalMaquina);
                registros.consolidado = sumando;


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
            return Json(new { data = registros, respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerConsolidado(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<TEC_ConsolidadoMaquinaEntidad> consolidado = new List<TEC_ConsolidadoMaquinaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;

            try {
                if(cantElementos > 0) {
                    if(codsala[0] != 0) { 
                    strElementos = " sala_id in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
            }

        
                var consolidadoTupla = estadomaquinabl.ObtenerConsolidado(strElementos, fechaini, fechafin);
                var error = consolidadoTupla.error;
                consolidado = consolidadoTupla.consolidadoLista;

                if(error.Key.Equals(string.Empty)) {
                    mensaje = "Consolidado obtenido con éxito";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo obtener el consolidado";
                }
            } catch(Exception exp) {
                mensaje = exp.Message + ", contacte al administrador";
            }

            return Json(new { data = consolidado.ToList(), respuesta, mensaje, mensajeConsola });
        }

        //[HttpPost]
        //public ActionResult ReporteEstadoMaquinasDescargarExcelJson(int[] codsala, int[] ArrayEstadoMaquinasId, DateTime fechaini, DateTime fechafin) {
        //    int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
        //    string nombreUsuario = estadomaquinabl.NombreId(usuarioId);


        //    string mensaje = string.Empty;
        //    string mensajeConsola = string.Empty;
        //    bool respuesta = false;
        //    string base64String = "";
        //    string excelName = string.Empty;
        //    List<TEC_EstadoMaquinaEntidad> listaTEC_estadomaquinas = new List<TEC_EstadoMaquinaEntidad>();
        //    int cantElementos = (codsala == null) ? 0 : codsala.Length;
        //    //var strElementos = String.Empty;
        //    //var nombresala = new List<dynamic>();

        //    // var salasSeleccionadas = String.Empty;

        //    try {

        //        List<int> order = ArrayEstadoMaquinasId.Select(x => Convert.ToInt32(x)).ToList();

        //        string MaquinaEstados = String.Join(",", ArrayEstadoMaquinasId);
        //        string consulta = " id in (" + MaquinaEstados + ")";
        //        var TEC_EstadoMaquinaTupla = estadomaquinabl.TEC_EstadoMaquinaListarporIdsJson(consulta);
        //        //var TEC_EstadoMaquinaTupla = rec_reclamacionbl.REC_reclamacionListarxSalaFechaJson(strElementos, fechaini, fechafin);
        //        error = TEC_EstadoMaquinaTupla.error;
        //        listaTEC_estadomaquinas = TEC_EstadoMaquinaTupla.lista;

        //        Dictionary<long, TEC_EstadoMaquinaEntidad> d = listaTEC_estadomaquinas.ToDictionary(x => x.id);
        //        List<TEC_EstadoMaquinaEntidad> ordered = order.Select(i => d[i]).ToList();
        //        string salasSeleccionadas = String.Join(",", ordered.Select(x => x.sala).Distinct().ToList());

        //        if(error.Key.Equals(string.Empty)) {
        //            if(ordered.Count > 0) {

        //                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //                ExcelPackage excel = new ExcelPackage();

        //                var workSheet = excel.Workbook.Worksheets.Add("MaquinaEstado");
        //                workSheet.TabColor = System.Drawing.Color.Black;
        //                workSheet.DefaultRowHeight = 8;


        //                workSheet.Cells["B1:H1"].Merge = true;  
        //                workSheet.Cells["B1"].Value = "Reporte Estado Maquinas";
        //                workSheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                workSheet.Cells["B1"].Style.Font.Size = 14;
        //                workSheet.Cells["B1"].Style.Font.Bold = true;

        //                workSheet.Cells["G2"].Value = "Usuario: " + nombreUsuario;
        //                workSheet.Cells["G2"].Style.Font.Bold = true;

        //                workSheet.Cells["H2"].Value = "Fecha Reporte: " + DateTime.Now.ToString("dd-MM-yyyy");
        //                workSheet.Cells["H2"].Style.Font.Bold = true;



        //                //Header of table  
                        
        //                workSheet.Row(3).Height = 20;
        //                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                workSheet.Row(3).Style.Font.Bold = true;
        //                workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;
        //                workSheet.Cells[3, 2].Value = "ID";
        //                workSheet.Cells[3, 3].Value = "ID_Sala";
        //                workSheet.Cells[3, 4].Value = "Sala";
        //                workSheet.Cells[3, 5].Value = "Maquinas Conectadas";
        //                workSheet.Cells[3, 6].Value = "Maquinas No Conectadas";
        //                workSheet.Cells[3, 7].Value = "Maquinas En Juego";
        //                workSheet.Cells[3, 8].Value = "Total";
        //                workSheet.Cells[3, 9].Value = "Fecha de Registro";


        //                //Body of table  
                        
        //                int recordIndex = 4;
        //                int total = ordered.Count;
        //                foreach(var registro in ordered) {
        //                    workSheet.Cells[recordIndex, 2].Value = registro.id;
        //                    workSheet.Cells[recordIndex, 3].Value = registro.sala_id;
        //                    workSheet.Cells[recordIndex, 4].Value = registro.sala;
        //                    workSheet.Cells[recordIndex, 5].Value = registro.CantMaquinaConectada;
        //                    workSheet.Cells[recordIndex, 6].Value = registro.CantMaquinaNoConectada;
        //                    workSheet.Cells[recordIndex, 7].Value = registro.CantMaquinaPLay;
        //                    workSheet.Cells[recordIndex, 8].Value = registro.TotalMaquina;
        //                    workSheet.Cells[recordIndex, 9].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                    

        //                    recordIndex++;
        //                }
        //                Color colbackground = ColorTranslator.FromHtml("#003268");
        //                Color colborder = ColorTranslator.FromHtml("#074B88");

        //                workSheet.Cells["B3:I3"].Style.Font.Bold = true;
        //                workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(colbackground);
        //                workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

        //                workSheet.Cells["B3:I3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells["B3:I3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells["B3:I3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells["B3:I3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                workSheet.Cells["B3:I3"].Style.Border.Top.Color.SetColor(colborder);
        //                workSheet.Cells["B3:I3"].Style.Border.Left.Color.SetColor(colborder);
        //                workSheet.Cells["B3:I3"].Style.Border.Right.Color.SetColor(colborder);
        //                workSheet.Cells["B3:I3"].Style.Border.Bottom.Color.SetColor(colborder);

        //                //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
        //                int filasagregadas = 3;
        //                total = filasagregadas + total;

        //                workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //                workSheet.Cells["B2:F2"].Merge = true;
        //                workSheet.Cells["B2:F2"].Style.Font.Bold = true;

        //                int filaFooter_ = total + 1;

        //                workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Merge = true;
        //                workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Bold = true;
        //                workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
        //                workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Color.SetColor(Color.White);
        //                workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

        //                int filaultima = total;
        //                workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

        //                workSheet.Column(2).AutoFit();
        //                workSheet.Column(3).Width = 15;
        //                workSheet.Column(4).Width = 30;
        //                workSheet.Column(5).Width = 20;
        //                workSheet.Column(6).Width = 24;
        //                workSheet.Column(7).Width = 20;
        //                workSheet.Column(8).Width = 25;
        //                workSheet.Column(9).Width = 30;

        //                excelName = "MaquinaEstado_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
        //                var memoryStream = new MemoryStream();
        //                excel.SaveAs(memoryStream);
        //                base64String = Convert.ToBase64String(memoryStream.ToArray());

        //                mensaje = "Descargando Archivo";
        //                respuesta = true;
        //            } else {
        //                mensaje = "No se Pudo generar Archivo";
        //            }
        //        } else {
        //            mensajeConsola = error.Value;
        //            mensaje = "No se Pudo generar Archivo";
        //        }
        //    } catch(Exception exp) {
        //        respuesta = false;
        //        mensaje = exp.Message + ", Llame Administrador";
        //    }
        //    return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        //}


         

        //[seguridad(false)]
        //[HttpPost] 
        //public ActionResult GuardarMaquinaEstado(TEC_EstadoMaquinaEntidad maquinaestado) {
        //    bool status = false;

        //    string message = "No se pudo guardar los datos";

        //    try {

        //        maquinaestado.FechaRegistro = DateTime.Now;

        //        int insertedId = estadomaquinabl.GuardarMaquinaEstado(maquinaestado);

        //        if(insertedId > 0) {
        //            status = true;
        //            message = "Los datos se han guardado";
        //        }
        //    } catch(Exception exception) {
        //        message = exception.Message;
        //    }

        //    return Json(new {
        //        status,
        //        message
        //    });
        //}


        [seguridad(false)] 
        [HttpPost]
        public ActionResult EstadoMaquinaObtenerPorId(int id) {
            string mensaje = string.Empty;
            bool respuesta = false; 
            var data = new TEC_EstadoMaquinaEntidad();
            try {

                data = estadomaquinabl.GetEstadoMaquinaPorId(id);
                mensaje = "Obteniendo registro";
                respuesta = true;
               
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarRetiroTemporal(int IdEstadoMaquina) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<TEC_EstadoMaquinaDetalleEntidad>();
            try {

                data = estadomaquinabl.ListarRetiroTemporal(IdEstadoMaquina);
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

         
        [HttpPost]
        public ActionResult InsertarRetiroTemporal(TEC_EstadoMaquinaDetalleEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try { 
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                TEC_EstadoMaquinaEntidad registroestadomaquina = estadomaquinabl.ObtenerEstadoMaquinaporId(model.IdEstadoMaquina);
                string estadomaquina = estadomaquinabl.BuscarEstadoMaquinaxCodMaquina(model.CodMaquina, registroestadomaquina.FechaOperacion, registroestadomaquina.sala_id);

                if(estadomaquina != null) {

                    int idInsertado = estadomaquinabl.InsertarRetiroTemporal(model);
                    if(idInsertado > 0) {
                        bool actualizacionExitosa = estadomaquinabl.AgregarRetiroTemporal(model.IdEstadoMaquina, estadomaquina);

                        if(actualizacionExitosa) {
                            mensaje = "Registro insertado";
                            respuesta = true;
                        } else {
                            mensaje = "El registro se insertó, pero no se pudo actualizar la cantidad de máquinas.";
                        }
                    }
                } else {
                    mensaje = "La maquina no se pudo encontrar en nuestros registros.";
                    respuesta = false;
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        [seguridad(false)]
        [HttpPost]
        public bool ActualizarCantidadRetiroTemporal(int IdEstadoMaquina) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";
            try { 
                respuesta = estadomaquinabl.ActualizarCantidadRetiroTemporal(IdEstadoMaquina);

                if(respuesta) {
                    mensaje = "Los datos se han actualizado correctamente";
                }
            } catch(Exception exception) {
                mensaje = "Ha ocurrido un error, comunicarse con el administrador";
                Console.WriteLine("Error: " + exception.Message);  
            }
            return true;
        }
         
         
        [HttpPost]
        public ActionResult QuitarRetiroTemporal(int idEstadoMaquinaDetalle, int idEstadoMaquina, string codMaquina) {
            string mensaje = "No se pudo eliminar el registro";
            bool respuesta = false;
            try {
                TEC_EstadoMaquinaEntidad registroestadomaquina =  estadomaquinabl.ObtenerEstadoMaquinaporId(idEstadoMaquina);
                string estadomaquina = estadomaquinabl.BuscarEstadoMaquinaxCodMaquina(codMaquina, registroestadomaquina.FechaOperacion, registroestadomaquina.sala_id);

                if(estadomaquina != null) {
                    bool eliminado = estadomaquinabl.EliminarRetiroTemporal(idEstadoMaquinaDetalle);

                    if(eliminado) {
                        string UsuarioRegistro = (string)Session["UsuarioNombre"];
                        bool actualizacionExitosa = estadomaquinabl.QuitarRetiroTemporal(idEstadoMaquina, estadomaquina);

                        if(actualizacionExitosa) {
                            mensaje = "Registro eliminado";
                            respuesta = true;
                        } else {
                            mensaje = "El registro se quitó, pero no se pudo actualizar la cantidad de máquinas.";
                        }
                    }
                }

                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con el administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult InsertarRegistroMaquina(TEC_RegistroMaquinaEntidad registro) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                registro.FechaRegistro = DateTime.Now;
                registro.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = estadomaquinabl.InsertarRegistroMaquina(registro);
                if(idInsertado > 0) {
                        mensaje = "Registro insertado";
                        respuesta = true;
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        [seguridad(false)]
        [HttpPost]
            public ActionResult ObtenerTotalMaquina(int codSala) {
                string mensaje = "No se pudo obtener el total de máquinas";
                bool respuesta = false;
                int totalMaquina = 0;

                try {
                    totalMaquina = estadomaquinabl.ObtenerTotalMaquinaPorCodSala(codSala);

                    if(totalMaquina > 0) {
                        mensaje = "Total de máquinas obtenido correctamente";
                        respuesta = true;
                    } else {
                        mensaje = "No se encontraron máquinas para la sala seleccionada";
                    }

                    return Json(new { mensaje, respuesta, totalMaquina });
                } catch(Exception ex) {
                    mensaje = "Ha ocurrido un error, contacte con el administrador";
                    respuesta = false;
                    return Json(new { mensaje, respuesta, totalMaquina });
                }
            }
         
        [HttpPost]
        public ActionResult ListaReporteRegistroMaquinaxSalaJson(int[] codsala) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<TEC_RegistroMaquinaEntidad> listamaquinaestado = new List<TEC_RegistroMaquinaEntidad>();

            try {
                if(codsala != null && codsala.Length > 0) {
                    foreach(var sala in codsala) {
                        TEC_RegistroMaquinaEntidad registroMaquina = new TEC_RegistroMaquinaEntidad();
                        registroMaquina = estadomaquinabl.ReporteRegistroMaquinaxSalaJson(sala);

                        if(registroMaquina.IdRegistroMaquina > 0) 
                            {
                                listamaquinaestado.Add(registroMaquina);
                            }
                        else 
                            {
                                TEC_RegistroMaquinaEntidad nuevoRegistro = new TEC_RegistroMaquinaEntidad {
                                    CodSala = sala,
                                    CodMaquinaINDECI = "0",
                                    CodMaquinaRD = "0",
                                    FechaRegistro = DateTime.Now,
                                    UsuarioRegistro = (string)Session["UsuarioNombre"]
                                };
                                int id = estadomaquinabl.CrearSalaRegistroMaquina(nuevoRegistro);
                                TEC_RegistroMaquinaEntidad registroMaquina2 = new TEC_RegistroMaquinaEntidad();
                                registroMaquina2 = estadomaquinabl.ReporteRegistroMaquinaxSalaJson(nuevoRegistro.CodSala);
                                    if(id == 0) {
                                        mensaje = "No se ha podido insertar una sala";
                                    } else {
                                        listamaquinaestado.Add(registroMaquina2);
                                    }
                            }
                        }
                    } else {
                        mensaje = "No se proporcionó un código de sala válido.";
                    }
                } catch(Exception ex) {
                mensaje = ex.Message + ", contacte al administrador.";
                mensajeConsola = ex.ToString();
            }

            return Json(new {
                data = listamaquinaestado,
                respuesta,
                mensaje,
                mensajeConsola
            });
        }




         
        [HttpPost]
        public ActionResult ExcelReporteRegistroMaquinaxSalaJson(int[] codsala) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                string salasBusqueda = "";

                if(codsala.Contains(-1)) {
                    salasBusqueda = string.Join(",", salas.Select(x => x.CodSala));
                } else {
                    salasBusqueda = string.Join(",", codsala);
                }

                List<TEC_RegistroMaquinaEntidad> lista = new List<TEC_RegistroMaquinaEntidad>();

                var (maquinaestadoLista, error) = estadomaquinabl.ListaReporteRegistroMaquinaxSalaJson(salasBusqueda);

                if(maquinaestadoLista != null) {
                    lista = maquinaestadoLista.OrderByDescending(x => x.IdRegistroMaquina).ToList();
                }

                if(lista.Count > 0) {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using(ExcelPackage excel = new ExcelPackage()) {
                        var workSheet = excel.Workbook.Worksheets.Add("ReporteRegistroMaquina");
                        workSheet.TabColor = Color.Black;
                        workSheet.DefaultRowHeight = 7;

                        workSheet.Row(3).Height = 20;
                        workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Row(3).Style.Font.Bold = true;
                        workSheet.Cells[2, 2, 2, 7].Merge = true;
                        workSheet.Cells[2, 2].Value = "Reporte de Registro de Máquinas";
                        workSheet.Cells[2, 2].Style.Font.Bold = true;
                        workSheet.Cells[2, 2].Style.Font.Size = 13;
                        workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[3, 2].Value = "ID";
                        workSheet.Cells[3, 3].Value = "Sala";
                        workSheet.Cells[3, 4].Value = "Máquinas INDECI";
                        workSheet.Cells[3, 5].Value = "Máquinas RD";
                        workSheet.Cells[3, 6].Value = "Total de Máquinas";
                        workSheet.Cells[3, 7].Value = "Fecha Modificación";

                        int recordIndex = 4;
                        foreach(var item in lista) {
                            workSheet.Cells[recordIndex, 2].Value = item.IdRegistroMaquina;
                            workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                            workSheet.Cells[recordIndex, 4].Value = item.CodMaquinaINDECI;
                            workSheet.Cells[recordIndex, 5].Value = item.CodMaquinaRD;
                            workSheet.Cells[recordIndex, 6].Value = item.TotalMaquina;
                            workSheet.Cells[recordIndex, 7].Value = item.FechaModificacion.ToString("dd-MM-yyyy") == "01-01-1753" ? "" : item.FechaModificacion.ToString("dd-MM-yyyy hh:mm tt");
                            recordIndex++;
                        }

                        Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                        workSheet.Cells["B3:G3"].Style.Font.Bold = true;
                        workSheet.Cells["B3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells["B3:G3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                        workSheet.Cells["B3:G3"].Style.Font.Color.SetColor(Color.White);

                        for(int i = 2; i <= 8; i++) {
                            workSheet.Column(i).AutoFit();
                        }

                        excelName = $"ReporteRegistroMaquina_{DateTime.Now:dd_MM_yyyy}.xlsx";
                        using(var memoryStream = new MemoryStream()) {
                            excel.SaveAs(memoryStream);
                            base64String = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }

                    mensaje = "Archivo generado correctamente";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron datos para el reporte.";
                }
            } catch(Exception ex) {
                mensaje = $"Error al generar el archivo: {ex.Message}";
                respuesta = false;
            }

            return Json(new { data = base64String, excelName, respuesta, mensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarMaquinaEstado(TEC_EstadoMaquinaEntidad maquinaestado) {
            string mensaje = "No se pudo guardar los datos";
            bool respuesta = false;

            try {  
                int idEstadoMaquina = estadomaquinabl.ExisteRegistro(maquinaestado.FechaOperacion, maquinaestado.sala_id);
                if(idEstadoMaquina > 0) {
                    mensaje = "No se pudo insertar los datos, ya existe";
                    respuesta = false;
                } else {
                    maquinaestado.FechaRegistro = DateTime.Now;
                    int insertedId = estadomaquinabl.GuardarMaquinaEstado(maquinaestado);
                     if(insertedId > 0) {
                        mensaje = "Los datos se han guardado correctamente de Estado Maquina";
                        respuesta = true;
                        //foreach(var historialmaquina in maquinaestado.ListadoMaquinaEstado) {
                        //    int insertedIdHistorialMaquina = estadomaquinabl.GuardarHistorialMaquina(historialmaquina);
                        //    if(insertedIdHistorialMaquina > 0) {
                        //        mensaje = "Los datos se han guardado correctamente de Historial Maquina";
                        //        respuesta = true;
                        //    } else {
                        //        mensaje = "No se pudo guardar los datos de Historial Maquina";
                        //        respuesta = false;
                        //    }
                        //}
                        estadomaquinabl.GuardarHistorialMaquina(maquinaestado.ListadoMaquinaEstado);
                        mensaje = "Los datos se han guardado correctamente de Historial Maquina";
                        respuesta = true;



                        DateTime FechaOperacionAntigua = maquinaestado.FechaOperacion.AddDays(-30);
                        bool ExisteHistorialMaquinaAntiguas = estadomaquinabl.ExisteRegistroHistorialMaquinaxFechaOperacion(FechaOperacionAntigua, maquinaestado.sala_id);
                        if(ExisteHistorialMaquinaAntiguas) {
                            estadomaquinabl.EliminarRegistrosHistorialMaquinaAntiguos(FechaOperacionAntigua, maquinaestado.sala_id); 
                        }

                    } else {
                            mensaje = "No se pudo guardar los datos de Estado Maquina";
                            respuesta = false;
                        }
                }

            } catch(Exception exception) {
                mensaje = exception.Message;
                respuesta = false;
            }

            return Json(new { mensaje, respuesta });
        } 
        [HttpPost]
        public ActionResult ReporteEstadoMaquinasDescargarExcelJson(int[] codsala, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;


            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }

                List<TEC_EstadoMaquinaEntidad> lista = estadomaquinabl.ListadoEstadoMaquina(salasBusqueda, fechaIni, fechaFin);
                lista = lista.OrderByDescending(x => x.id).ToList();
                if(lista.Count > 0) {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    // Hoja Principal: Bienes Materiales
                    var workSheet = excel.Workbook.Worksheets.Add("EstadoMaquina");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 12;

                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 11].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Estado Maquina";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Cod Sala";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Maquina Conectada";
                    workSheet.Cells[3, 6].Value = "Maquina No Conectada";
                    workSheet.Cells[3, 7].Value = "Maquina Play";
                    workSheet.Cells[3, 8].Value = "Maquina Total";
                    workSheet.Cells[3, 9].Value = "Maquina Retiro Temporal";
                    workSheet.Cells[3, 10].Value = "Fecha Operacion";
                    workSheet.Cells[3, 11].Value = "Fecha Cierre";
                    workSheet.Cells[3, 12].Value = "Maquinas Retiro Temporal";

                    int recordIndex = 4;
                    int totalConectadas = 0;
                    int totalNoConectadas = 0;
                    int totalPlay = 0;
                    int totalTotal = 0;
                    int totalRetiroTemporal = 0;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.id;
                        workSheet.Cells[recordIndex, 3].Value = item.sala_id;
                        workSheet.Cells[recordIndex, 4].Value = item.sala;
                        workSheet.Cells[recordIndex, 5].Value = item.CantMaquinaConectada;
                        workSheet.Cells[recordIndex, 6].Value = item.CantMaquinaNoConectada;
                        workSheet.Cells[recordIndex, 7].Value = item.CantMaquinaPLay;
                        workSheet.Cells[recordIndex, 8].Value = item.TotalMaquina;
                        workSheet.Cells[recordIndex, 9].Value = item.CantMaquinaRetiroTemporal; 
                        workSheet.Cells[recordIndex, 10].Value = item.FechaOperacion.ToString("dd-MM-yyyy") == "01-01-1753" ? "" : item.FechaOperacion.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 11].Value = item.FechaCierre.ToString("dd-MM-yyyy") == "01-01-1753" ? "" : item.FechaCierre.ToString("dd-MM-yyyy hh:mm tt");

                        totalConectadas += item.CantMaquinaConectada;
                        totalNoConectadas += item.CantMaquinaNoConectada;
                        totalPlay += item.CantMaquinaPLay;
                        totalTotal += item.TotalMaquina;
                        totalRetiroTemporal += item.CantMaquinaRetiroTemporal;

                        if(item.Maquinas != null && item.Maquinas.Any()) {
                            var maquitnaSheet = excel.Workbook.Worksheets.Add($"Detalle_EstadoMaquina_{item.id}");

                            maquitnaSheet.Cells[1, 2, 1, 8].Merge = true;
                            maquitnaSheet.Cells[1, 2].Value = $"Detalle : Estado Maquina N°: {item.id}";
                            maquitnaSheet.Cells[1, 2].Style.Font.Bold = true;
                            maquitnaSheet.Cells[1, 2].Style.Font.Size = 13;
                            maquitnaSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            maquitnaSheet.Cells[3, 2].Value = "ID";
                            maquitnaSheet.Cells[3, 3].Value = "Código Maquina";
                            maquitnaSheet.Cells[3, 4].Value = "Fecha Registro"; 

                            Color headerBackground = ColorTranslator.FromHtml("#003268");
                            maquitnaSheet.Cells["B3:D3"].Style.Font.Bold = true;
                            maquitnaSheet.Cells["B3:D3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            maquitnaSheet.Cells["B3:D3"].Style.Fill.BackgroundColor.SetColor(headerBackground);
                            maquitnaSheet.Cells["B3:D3"].Style.Font.Color.SetColor(Color.White);
                            maquitnaSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            int MaquinaIndex = 4;
                            foreach(var maquina in item.Maquinas) {
                                maquitnaSheet.Cells[MaquinaIndex, 2].Value = maquina.IdEstadoMaquinaDetalle;
                                maquitnaSheet.Cells[MaquinaIndex, 3].Value = maquina.CodMaquina;
                                maquitnaSheet.Cells[MaquinaIndex, 4].Value = maquina.FechaRegistro.ToString("dd-MM-yyyy") == "01-01-1753" ? "" : maquina.FechaRegistro.ToString("dd-MM-yyyy hh:mm tt");  
                                MaquinaIndex++;
                            }

                            for(int i = 2; i <= 4; i++) {
                                maquitnaSheet.Column(i).AutoFit();
                            }

                            var hyperlink = new ExcelHyperLink($"#'Detalle_EstadoMaquina_{item.id}'!A1", "Ver Detalles");
                            workSheet.Cells[recordIndex, 12].Hyperlink = hyperlink;
                            workSheet.Cells[recordIndex, 12].Style.Font.UnderLine = true;
                            workSheet.Cells[recordIndex, 12].Style.Font.Color.SetColor(Color.Blue);
                        } else {
                            workSheet.Cells[recordIndex, 12].Value = "No hay maquinas";
                        }

                        recordIndex++;
                    }
                    int sumRow = recordIndex;
                    workSheet.Cells[sumRow, 4].Value = "Totales:";  // Etiqueta de totales
                    workSheet.Cells[sumRow, 5].Value = totalConectadas;
                    workSheet.Cells[sumRow, 6].Value = totalNoConectadas;
                    workSheet.Cells[sumRow, 7].Value = totalPlay;
                    workSheet.Cells[sumRow, 8].Value = totalTotal;
                    workSheet.Cells[sumRow, 9].Value = totalRetiroTemporal;

                    // Estilo para la fila de totales
                    workSheet.Row(sumRow).Style.Font.Bold = true;
                    workSheet.Row(sumRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"B{sumRow}:L{sumRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[$"B{sumRow}:L{sumRow}"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#003268"));
                    workSheet.Cells[$"B{sumRow}:L{sumRow}"].Style.Font.Color.SetColor(Color.White); 


                    for(int i = 2; i <= 12; i++) {
                        workSheet.Column(i).AutoFit();
                    }
                    
                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:L3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:L3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:L3"].Style.Font.Color.SetColor(Color.White);

                   

                    excelName = $"ReporteEstadoMaquina_{fechaIni:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";
                    using(var memoryStream = new MemoryStream()) {
                        excel.SaveAs(memoryStream);
                        base64String = Convert.ToBase64String(memoryStream.ToArray());
                    }

                    mensaje = "Archivo generado correctamente";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron datos para el reporte.";
                }
            } catch(Exception ex) {
                mensaje = $"Error al generar el archivo: {ex.Message}";
                respuesta = false;
            }

            return Json(new { data = base64String, excelName, respuesta, mensaje });
        }
         
        [HttpPost]
        public ActionResult ActualizarRegistroMaquina(TEC_RegistroMaquinaEntidad registroMaquina) {
            string mensaje = "No se pudo actualizar los datos";
            bool respuesta = false;

            try {
                registroMaquina.FechaModificacion = DateTime.Now;
                registroMaquina.UsuarioModificacion = (string)Session["UsuarioNombre"];
                bool actualizado = estadomaquinabl.ActualizarRegistroMaquina(registroMaquina);

                    if(actualizado) {
                        mensaje = "Los datos se han actualizado correctamente";
                        respuesta = true;
                    } else {
                        mensaje = "No se pudo actualizar los datos";
                        respuesta = false;
                    }

            } catch(Exception exception) {
                mensaje = exception.Message; 
                respuesta = false;
            }

            return Json(new { mensaje, respuesta });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult CrearSalaRegistroMaquina(int sala) {
            string mensaje = "No se pudieron insertar los datos";
            bool respuesta = false;

            try {
              
                    TEC_RegistroMaquinaEntidad registro = new TEC_RegistroMaquinaEntidad {
                        CodSala = sala,  
                        CodMaquinaINDECI = "0",  
                        CodMaquinaRD = "0",   
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = (string)Session["UsuarioNombre"]
                    };

                    int resultado = estadomaquinabl.CrearSalaRegistroMaquina(registro);

                    if(resultado <= 0) {
                        throw new Exception("No se pudo insertar uno de los registros.");
                    }
               

                mensaje = "Los registros fueron insertados correctamente.";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message; 
                respuesta = false;
            }

            return Json(new { mensaje, respuesta });


        }


    }
}

