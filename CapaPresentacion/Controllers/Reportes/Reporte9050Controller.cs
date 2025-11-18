using CapaEntidad;
using CapaEntidad.Excel;
using CapaEntidad.Reportes._9050;
using CapaNegocio;
using CapaNegocio.Migracion;
using Newtonsoft.Json.Linq;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Reportes {
    [seguridad(true)]
    public class Reporte9050Controller : Controller {
        private readonly ContadoresOnlineBL contadoresOnlineBL;
        private readonly SalaBL salaBL;

        public Reporte9050Controller() {
            contadoresOnlineBL = new ContadoresOnlineBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult ConteoClientes() {
            return View("~/Views/Reportes/ERP/ConteoClientes.cshtml");
        }
        #endregion

        #region Methods
        [HttpPost]
        [Route("Reporte9050/ExportToExcelReport")]
        public JsonResult ExportToExcelReport(ExportToExcel exportToExcel) {
            if(exportToExcel == null) {
                return Json(new { success = false, displayMessage = "No se propocionó la información correcta para generar el excel." });
            }

            var data = JArray.Parse(exportToExcel.JsonContent);

            JArray ignoredColumns = new JArray();
            if(!string.IsNullOrEmpty(exportToExcel.IgnoredColumns)) {
                ignoredColumns = JArray.Parse(exportToExcel.IgnoredColumns);
            }

            if(data == null || data.Count == 0) {
                return Json(new { success = false, displayMessage = "No se propocionó la información correcta para generar el excel." });
            }

            #region Titulo y nombre del Excel
            SalaEntidad sala = salaBL.SalaListaIdJson(exportToExcel.CodSala);
            exportToExcel.Title = $"{exportToExcel.Title} - {sala.Nombre}";
            exportToExcel.FileName = $"{exportToExcel.FileName} - {sala.Nombre}";
            #endregion

            Dictionary<string, string> firstRow = ((IEnumerable<KeyValuePair<string, JToken>>)data[0]).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
            var randomString = new RandomString(8);
            Type type = DynamicTypeBuilder.CreateType(firstRow, randomString.GetUniqueString());

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

            foreach(var item in data) {
                list.Add(item.ToObject(type));
            }

            string[] columns = new string[ignoredColumns.Count];
            int i = 0;
            foreach(string column in ignoredColumns) {
                columns[i] = column;
                i++;
            }

            //var bytes = _service.ExportToExcel(dto.Title, list, type, columns);
            var bytes = ExportToExcel.ExportExcel(list, type, exportToExcel.Title, "Hoja 1", columns);
            string extension = ".xlsx";
            string mimeType = FileFormatMimeTypes.GetMimeType(extension);

            string fileName = string.IsNullOrWhiteSpace(exportToExcel.FileName)
                ? Path.ChangeExtension(Path.GetRandomFileName(), extension)
                : $"{exportToExcel.FileName}{extension}";

            var obj = new {
                success = true,
                fileName = fileName,
                fileContent = Convert.ToBase64String(bytes),
                fileMimeType = mimeType,
                displayMessage = "Archivo excel generado correctamente"
            };

            var response = Json(obj);
            response.MaxJsonLength = int.MaxValue;

            return response;
        }

        [HttpGet]
        [Route("Reporte9050/ObtenerContadores")]
        public JsonResult ObtenerContadores(DateTime fechaInicio, DateTime fechaFin, int codSala, double? CurrentC, int tipo) {
            var reporte = new ReporteGerenciaDto();

            var fecha_Inicio = fechaInicio.AddDays(-1).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            var fecha_fin = fechaFin.AddDays(2).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            var contadoresCabeceraList = new List<ReporteGerenciaCabeceraDto>();
            var contadoresCabecera = new ReporteGerenciaCabeceraDto();
            var contadoresdetalle = new ContadoresOnlineDto();
            var contadoresdetalleList = new List<ContadoresOnlineDto>();
            List<ContadoresOnlineDto> contadoresOnlineDto = contadoresOnlineBL.ObtenerContadoresOnlineParaReporte9050(fecha_Inicio, fecha_fin, codSala);

            //OBTENGO LAS MAQUINAS QUE TUVIERON CONTADORES Y RECORREMOS MAQUINA A MAQUINA POR  FECHA Y HORAS
            var maquinas = contadoresOnlineDto.Select(s => s.CodMaq).Distinct().ToList();
            var fechas = contadoresOnlineDto.Select(s => s.Fecha).Distinct().ToList();
            var fechaHoras = contadoresOnlineDto.Select(s => s.FechaHora).Distinct().ToList();
            if(tipo == 1) // INSTANTANEA  ULTIMOS  5 MIN 
            {
                foreach(var codMaq in maquinas) {
                    DateTime fecha = Convert.ToDateTime(fecha_Inicio).AddDays(1);
                    //RECORRO  FECHA A FECHA HASTA FECHA FIN POR MAQUINA
                    while(fecha <= fechaFin) {
                        var hora = 0;
                        //COMPRUEBO QUE EXISTA CONTADORES DE LA MAQUINA EN LA FECHA
                        var porFecha = contadoresOnlineDto.Where(x => x.CodMaq == codMaq && x.Fecha == fecha).ToList();
                        if(porFecha.Count() == 0) {
                            while(hora < 24) {
                                contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                    Fecha = fecha,
                                    Hora = hora,
                                    Maquinas = 0
                                };
                                contadoresCabeceraList.Add(contadoresCabecera);
                                hora++;
                            };
                        } else {
                            // SI HAY CONTADORES DE LA MAQUINA RECORRERLAS
                            foreach(var Fecha in porFecha) {
                                while(hora < 24) {
                                    var horaExacta = new ContadoresOnlineDto();
                                    //COMPRUEBO QUE EXISTE CONTADORES EN LA HORA  
                                    if(hora == 0) {
                                        var si1 = porFecha.Where(x => x.Hora == 23).Count();
                                        if(si1 != 0) {
                                            var HoraMaxAnt = porFecha.Where(x => x.Hora == 23).ToList().Max(x => x.CodContadoresOnline);
                                            horaExacta = contadoresOnlineDto.Where(x => x.CodContadoresOnline == HoraMaxAnt).FirstOrDefault();
                                            if(CurrentC == null || CurrentC == 0) {
                                                if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= 0.1) {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 1
                                                    };
                                                    contadoresCabeceraList.Add(contadoresCabecera);
                                                    contadoresdetalle = new ContadoresOnlineDto() {
                                                        CodMaq = codMaq,
                                                        NroSerie = horaExacta.NroSerie,
                                                        Sala = horaExacta.Sala,
                                                        Marca = horaExacta.Marca,
                                                        Modelo = horaExacta.Modelo,
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        CoinIn = Math.Round((horaExacta.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                        CoinOut = Math.Round((horaExacta.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                        Promedio = horaExacta.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn) / (horaExacta.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                        CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),

                                                    };
                                                    contadoresdetalleList.Add(contadoresdetalle);
                                                } else {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 0
                                                    };
                                                }
                                            } else {
                                                if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 1
                                                    };
                                                    contadoresCabeceraList.Add(contadoresCabecera);
                                                    contadoresdetalle = new ContadoresOnlineDto() {
                                                        CodMaq = codMaq,
                                                        NroSerie = horaExacta.NroSerie,
                                                        Sala = horaExacta.Sala,
                                                        Marca = horaExacta.Marca,
                                                        Modelo = horaExacta.Modelo,
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        CoinIn = Math.Round((horaExacta.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                        CoinOut = Math.Round((horaExacta.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                        Promedio = horaExacta.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn) / (horaExacta.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                        CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                    };
                                                    contadoresdetalleList.Add(contadoresdetalle);
                                                } else {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 0
                                                    };
                                                }
                                            }
                                        }
                                    } else if(hora == 8) {
                                        var si2 = contadoresOnlineDto.Where(x => x.CodMaq == codMaq && x.FechaReal == fecha && x.Hora == 7).Count();
                                        if(si2 > 0) {
                                            var HoraMaxAnt = contadoresOnlineDto.Where(x => x.CodMaq == codMaq && x.FechaReal == fecha && x.Hora == 7).ToList().Max(x => x.CodContadoresOnline);
                                            horaExacta = contadoresOnlineDto.Where(x => x.CodContadoresOnline == HoraMaxAnt).FirstOrDefault();
                                            if(CurrentC == null || CurrentC == 0) {
                                                if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= 0.1) {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 1
                                                    };
                                                    contadoresCabeceraList.Add(contadoresCabecera);
                                                    contadoresdetalle = new ContadoresOnlineDto() {
                                                        CodMaq = codMaq,
                                                        NroSerie = horaExacta.NroSerie,
                                                        Sala = horaExacta.Sala,
                                                        Marca = horaExacta.Marca,
                                                        Modelo = horaExacta.Modelo,
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        CoinIn = Math.Round((horaExacta.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                        CoinOut = Math.Round((horaExacta.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                        Promedio = horaExacta.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn) / (horaExacta.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                        CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                    };
                                                    contadoresdetalleList.Add(contadoresdetalle);
                                                } else {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 0
                                                    };
                                                }
                                            } else {
                                                if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 1
                                                    };
                                                    contadoresCabeceraList.Add(contadoresCabecera);
                                                    contadoresdetalle = new ContadoresOnlineDto() {
                                                        CodMaq = codMaq,
                                                        NroSerie = horaExacta.NroSerie,
                                                        Sala = horaExacta.Sala,
                                                        Marca = horaExacta.Marca,
                                                        Modelo = horaExacta.Modelo,
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        CoinIn = Math.Round((horaExacta.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                        CoinOut = Math.Round((horaExacta.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                        Promedio = horaExacta.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn) / (horaExacta.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                        CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                    };
                                                    contadoresdetalleList.Add(contadoresdetalle);
                                                } else {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 0
                                                    };
                                                }
                                            }

                                        }
                                    } else {
                                        var si3 = porFecha.Where(x => x.Hora == (hora - 1)).Count();
                                        if(si3 != 0) {
                                            var HoraMaxAnt = porFecha.Where(x => x.Hora == (hora - 1)).ToList().Max(x => x.CodContadoresOnline);
                                            horaExacta = porFecha.Where(x => x.CodContadoresOnline == HoraMaxAnt).FirstOrDefault();
                                            if(CurrentC == null || CurrentC == 0) {
                                                if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= 0.1) {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 1
                                                    };
                                                    contadoresCabeceraList.Add(contadoresCabecera);
                                                    contadoresdetalle = new ContadoresOnlineDto() {
                                                        CodMaq = codMaq,
                                                        NroSerie = horaExacta.NroSerie,
                                                        Sala = horaExacta.Sala,
                                                        Marca = horaExacta.Marca,
                                                        Modelo = horaExacta.Modelo,
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        CoinIn = Math.Round((horaExacta.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                        CoinOut = Math.Round((horaExacta.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                        Promedio = horaExacta.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn) / (horaExacta.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                        CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                    };
                                                    contadoresdetalleList.Add(contadoresdetalle);
                                                } else {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 0
                                                    };
                                                }
                                            } else {
                                                if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 1
                                                    };
                                                    contadoresCabeceraList.Add(contadoresCabecera);
                                                    contadoresdetalle = new ContadoresOnlineDto() {
                                                        CodMaq = codMaq,
                                                        NroSerie = horaExacta.NroSerie,
                                                        Sala = horaExacta.Sala,
                                                        Marca = horaExacta.Marca,
                                                        Modelo = horaExacta.Modelo,
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        CoinIn = Math.Round((horaExacta.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                        CoinOut = Math.Round((horaExacta.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                        Promedio = horaExacta.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn) / (horaExacta.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                        CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                    };
                                                    contadoresdetalleList.Add(contadoresdetalle);
                                                } else {
                                                    contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                        Fecha = fecha,
                                                        Hora = hora,
                                                        Maquinas = 0
                                                    };
                                                }
                                            }
                                        }
                                    }
                                    hora++;
                                }
                            }
                        }
                        fecha = fecha.AddDays(1);
                    }
                };
                var agrupados = (from i in (from rl in contadoresCabeceraList
                                            select new { rl.Fecha, rl.Hora, rl.Maquinas })
                                 group i by new { i.Fecha, i.Hora } into g
                                 select new { g.Key.Fecha, g.Key.Hora, Maquinas = g.Sum(rl => rl.Maquinas) }).OrderByDescending(x => x.Fecha).ToList();
                reporte.detalle = contadoresdetalleList;
                var cabecera = new List<ReporteGerenciaCabeceraDto>();
                var itemCabecera = new ReporteGerenciaCabeceraDto();
                foreach(var item in agrupados) {
                    itemCabecera = new ReporteGerenciaCabeceraDto() {
                        Fecha = item.Fecha,
                        Hora = item.Hora,
                        Maquinas = item.Maquinas
                    };
                    cabecera.Add(itemCabecera);
                }
                reporte.cabecera = cabecera;
            } else // POR +q1º1   1 HORA
              {
                foreach(var codMaq in maquinas) {
                    DateTime fecha = Convert.ToDateTime(fecha_Inicio).AddDays(1);
                    //RECORRO  FECHA A FECHA HASTA FECHA FIN POR MAQUINA
                    while(fecha <= fechaFin) {
                        var hora = 0;
                        //COMPRUEBO QUE EXISTA CONTADORES DE LA MAQUINA EN LA FECHA
                        var porFecha = contadoresOnlineDto.Where(x => x.CodMaq == codMaq && x.Fecha == fecha).ToList();
                        if(porFecha.Count() == 0) {
                            while(hora < 24) {
                                contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                    Fecha = fecha,
                                    Hora = hora,
                                    Maquinas = 0
                                };
                                contadoresCabeceraList.Add(contadoresCabecera);
                                hora++;
                            };
                        } else {
                            // SI HAY CONTADORES DE LA MAQUINA RECORRERLAS
                            foreach(var Fecha in porFecha) {
                                while(hora < 24) {
                                    var horaExacta = new ContadoresOnlineDto();
                                    //COMPRUEBO QUE EXISTE CONTADORES EN LA HORA
                                    var si = porFecha.Where(x => x.Hora == hora).Count();
                                    if(si > 0) {
                                        var HoraMax = porFecha.Where(x => x.Hora == hora).ToList().Max(x => x.CodContadoresOnline);
                                        var HoraMaxima = porFecha.Where(x => x.CodContadoresOnline == HoraMax).FirstOrDefault();
                                        horaExacta = HoraMaxima;
                                        var horaExactaAnt = new ContadoresOnlineDto();
                                        if(hora == 0) {
                                            var si3 = porFecha.Where(x => x.Hora == 23).Count();
                                            {
                                                if(si3 > 0) {
                                                    var HoraMaxAnt = porFecha.Where(x => x.Hora == 23).ToList().Max(x => x.CodContadoresOnline);
                                                    var HoraMaximaAnt = contadoresOnlineDto.Where(x => x.CodContadoresOnline == HoraMaxAnt).FirstOrDefault();
                                                    horaExactaAnt = HoraMaximaAnt;
                                                    if(CurrentC == null || CurrentC == 0) {
                                                        if(horaExacta.CoinIn - horaExactaAnt.CoinIn > 0) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = Math.Round((horaExacta.CoinIn - horaExactaAnt.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                                CoinOut = Math.Round((horaExacta.CoinOut - horaExactaAnt.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                                Promedio = (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed) == 0 ? 0 : (Math.Round(((horaExacta.CoinIn - horaExactaAnt.CoinIn) / (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);
                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    } else {
                                                        if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                            if(horaExacta.CoinIn - horaExactaAnt.CoinIn > 0) {
                                                                contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                    Fecha = fecha,
                                                                    Hora = hora,
                                                                    Maquinas = 1
                                                                };
                                                                contadoresCabeceraList.Add(contadoresCabecera);
                                                                contadoresdetalle = new ContadoresOnlineDto() {
                                                                    CodMaq = codMaq,
                                                                    NroSerie = horaExacta.NroSerie,
                                                                    Sala = horaExacta.Sala,
                                                                    Marca = horaExacta.Marca,
                                                                    Modelo = horaExacta.Modelo,
                                                                    Fecha = fecha,
                                                                    Hora = hora,
                                                                    CoinIn = Math.Round((horaExacta.CoinIn - horaExactaAnt.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                                    CoinOut = Math.Round((horaExacta.CoinOut - horaExactaAnt.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                                    Promedio = horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn - horaExactaAnt.CoinIn) / (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                                    CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                                };
                                                                contadoresdetalleList.Add(contadoresdetalle);
                                                            } else {
                                                                contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                    Fecha = fecha,
                                                                    Hora = hora,
                                                                    Maquinas = 0
                                                                };
                                                                contadoresCabeceraList.Add(contadoresCabecera);
                                                            }
                                                        }
                                                    }
                                                } else {
                                                    if(CurrentC == null || CurrentC == 0) {
                                                        if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= 0.1) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = 0,
                                                                CoinOut = 0,
                                                                Promedio = 0,
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);
                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    } else {
                                                        if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = 0,
                                                                CoinOut = 0,
                                                                Promedio = 0,
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);
                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    }
                                                }
                                            }
                                        } else if(hora == 8) {
                                            var si3 = contadoresOnlineDto.Where(x => x.CodMaq == codMaq && x.FechaReal == fecha && x.Hora == 7).Count();
                                            {
                                                if(si3 > 0) {
                                                    var HoraMaxAnt = contadoresOnlineDto.Where(x => x.CodMaq == codMaq && x.FechaReal == fecha && x.Hora == 7).ToList().Max(x => x.CodContadoresOnline);
                                                    var HoraMaximaAnt = contadoresOnlineDto.Where(x => x.CodContadoresOnline == HoraMaxAnt).FirstOrDefault();
                                                    horaExactaAnt = HoraMaximaAnt;
                                                    if(CurrentC == null || CurrentC == 0) {
                                                        if(horaExacta.CoinIn - horaExactaAnt.CoinIn > 0) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = Math.Round((horaExacta.CoinIn - horaExactaAnt.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                                CoinOut = Math.Round((horaExacta.CoinOut - horaExactaAnt.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                                Promedio = (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed) == 0 ? 0 : (Math.Round(((horaExacta.CoinIn - horaExactaAnt.CoinIn) / (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);

                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    } else {
                                                        if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                            if(horaExacta.CoinIn - horaExactaAnt.CoinIn > 0) {
                                                                contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                    Fecha = fecha,
                                                                    Hora = hora,
                                                                    Maquinas = 1
                                                                };
                                                                contadoresCabeceraList.Add(contadoresCabecera);
                                                                contadoresdetalle = new ContadoresOnlineDto() {
                                                                    CodMaq = codMaq,
                                                                    NroSerie = horaExacta.NroSerie,
                                                                    Sala = horaExacta.Sala,
                                                                    Marca = horaExacta.Marca,
                                                                    Modelo = horaExacta.Modelo,
                                                                    Fecha = fecha,
                                                                    Hora = hora,
                                                                    CoinIn = Math.Round((horaExacta.CoinIn - horaExactaAnt.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                                    CoinOut = Math.Round((horaExacta.CoinOut - horaExactaAnt.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                                    Promedio = horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn - horaExactaAnt.CoinIn) / (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                                    CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                                };
                                                                contadoresdetalleList.Add(contadoresdetalle);
                                                            } else {
                                                                contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                    Fecha = fecha,
                                                                    Hora = hora,
                                                                    Maquinas = 0
                                                                };
                                                                contadoresCabeceraList.Add(contadoresCabecera);
                                                            }
                                                        }
                                                    }
                                                } else {
                                                    if(CurrentC == null || CurrentC == 0) {
                                                        if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= 0.1) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = 0,
                                                                CoinOut = 0,
                                                                Promedio = 0,
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);
                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    } else {
                                                        if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = 0,
                                                                CoinOut = 0,
                                                                Promedio = 0,
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);
                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    }
                                                }
                                            }
                                        } else {
                                            var si2 = porFecha.Where(x => x.Hora == (hora - 1)).Count();
                                            if(si2 > 0) {
                                                var HoraMaxAnt = porFecha.Where(x => x.Hora == (hora - 1)).ToList().Max(x => x.CodContadoresOnline);
                                                var HoraMaximaAnt = porFecha.Where(x => x.CodContadoresOnline == HoraMaxAnt).FirstOrDefault();
                                                horaExactaAnt = HoraMaximaAnt;
                                                if(CurrentC == null || CurrentC == 0) {
                                                    if(horaExacta.CoinIn - horaExactaAnt.CoinIn > 0) {
                                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            Maquinas = 1
                                                        };
                                                        contadoresCabeceraList.Add(contadoresCabecera);
                                                        contadoresdetalle = new ContadoresOnlineDto() {
                                                            CodMaq = codMaq,
                                                            NroSerie = horaExacta.NroSerie,
                                                            Sala = horaExacta.Sala,
                                                            Marca = horaExacta.Marca,
                                                            Modelo = horaExacta.Modelo,
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            CoinIn = Math.Round((horaExacta.CoinIn - horaExactaAnt.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                            CoinOut = Math.Round((horaExacta.CoinOut - horaExactaAnt.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                            Promedio = (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed) == 0 ? 0 : (Math.Round(((horaExacta.CoinIn - horaExactaAnt.CoinIn) / (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                            CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                        };
                                                        contadoresdetalleList.Add(contadoresdetalle);

                                                    } else {
                                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            Maquinas = 0
                                                        };
                                                        contadoresCabeceraList.Add(contadoresCabecera);
                                                    }
                                                } else {
                                                    if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                        if(horaExacta.CoinIn - horaExactaAnt.CoinIn > 0) {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 1
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                            contadoresdetalle = new ContadoresOnlineDto() {
                                                                CodMaq = codMaq,
                                                                NroSerie = horaExacta.NroSerie,
                                                                Sala = horaExacta.Sala,
                                                                Marca = horaExacta.Marca,
                                                                Modelo = horaExacta.Modelo,
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                CoinIn = Math.Round((horaExacta.CoinIn - horaExactaAnt.CoinIn) * Convert.ToDouble(horaExacta.Token), 2),
                                                                CoinOut = Math.Round((horaExacta.CoinOut - horaExactaAnt.CoinOut) * Convert.ToDouble(horaExacta.Token), 2),
                                                                Promedio = horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed == 0 ? 0 : (Math.Round(((horaExacta.CoinIn - horaExactaAnt.CoinIn) / (horaExacta.GamesPlayed - horaExactaAnt.GamesPlayed)), 2)) * Convert.ToDouble(horaExacta.Token),
                                                                CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                            };
                                                            contadoresdetalleList.Add(contadoresdetalle);
                                                        } else {
                                                            contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                                Fecha = fecha,
                                                                Hora = hora,
                                                                Maquinas = 0
                                                            };
                                                            contadoresCabeceraList.Add(contadoresCabecera);
                                                        }
                                                    }
                                                }
                                            } else {
                                                if(CurrentC == null || CurrentC == 0) {
                                                    if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= 0.1) {
                                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            Maquinas = 1
                                                        };
                                                        contadoresCabeceraList.Add(contadoresCabecera);
                                                        contadoresdetalle = new ContadoresOnlineDto() {
                                                            CodMaq = codMaq,
                                                            NroSerie = horaExacta.NroSerie,
                                                            Sala = horaExacta.Sala,
                                                            Marca = horaExacta.Marca,
                                                            Modelo = horaExacta.Modelo,
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            CoinIn = 0,
                                                            CoinOut = 0,
                                                            Promedio = 0,
                                                            CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                        };
                                                        contadoresdetalleList.Add(contadoresdetalle);
                                                    } else {
                                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            Maquinas = 0
                                                        };
                                                        contadoresCabeceraList.Add(contadoresCabecera);
                                                    }
                                                } else {
                                                    if((horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token)) >= CurrentC) {
                                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            Maquinas = 1
                                                        };
                                                        contadoresCabeceraList.Add(contadoresCabecera);
                                                        contadoresdetalle = new ContadoresOnlineDto() {
                                                            CodMaq = codMaq,
                                                            NroSerie = horaExacta.NroSerie,
                                                            Sala = horaExacta.Sala,
                                                            Marca = horaExacta.Marca,
                                                            Modelo = horaExacta.Modelo,
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            CoinIn = 0,
                                                            CoinOut = 0,
                                                            Promedio = 0,
                                                            CurrentCredits = Math.Round(horaExacta.CurrentCredits * Convert.ToDouble(horaExacta.Token), 2),
                                                        };
                                                        contadoresdetalleList.Add(contadoresdetalle);
                                                    } else {
                                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                                            Fecha = fecha,
                                                            Hora = hora,
                                                            Maquinas = 0
                                                        };
                                                        contadoresCabeceraList.Add(contadoresCabecera);
                                                    }
                                                }
                                            }
                                        }
                                    } else {
                                        contadoresCabecera = new ReporteGerenciaCabeceraDto() {
                                            Fecha = fecha,
                                            Hora = hora,
                                            Maquinas = 0
                                        };
                                        contadoresCabeceraList.Add(contadoresCabecera);
                                    }
                                    hora++;
                                }
                            }
                        }
                        fecha = fecha.AddDays(1);
                    }
                };
                var agrupados = from i in (from rl in contadoresCabeceraList
                                           select new { rl.Fecha, rl.Hora, rl.Maquinas })
                                group i by new { i.Fecha, i.Hora } into g
                                select new { g.Key.Fecha, g.Key.Hora, Maquinas = g.Sum(rl => rl.Maquinas) };

                reporte.detalle = contadoresdetalleList;
                var cabecera = new List<ReporteGerenciaCabeceraDto>();
                var itemCabecera = new ReporteGerenciaCabeceraDto();
                foreach(var item in agrupados.OrderByDescending(x => x.Fecha)) {
                    itemCabecera = new ReporteGerenciaCabeceraDto() {
                        Fecha = item.Fecha,
                        Hora = item.Hora,
                        Maquinas = item.Maquinas
                    };
                    cabecera.Add(itemCabecera);
                }
                reporte.cabecera = cabecera;
            }

            var jsonResult = Json(reporte, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        #endregion
    }
}