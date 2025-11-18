using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaEntidad.ContadoresBonusIn;
using CapaNegocio.Administrativo;
using CapaNegocio.ContadoresBonusIn;
using CapaNegocio.Progresivo;
using CapaPresentacion.Models;

namespace CapaPresentacion.Controllers.ContadoresBonusIn {

    public class ContadoresBonusInController : Controller
    {

        private ContadoresBonusInBL contadoresBonusInBL = new ContadoresBonusInBL();
        private ProgresivoBL progresivoBL = new ProgresivoBL();
        private WS_DetalleContadoresGameWebBL wS_DetalleContadoresGameWebBL = new WS_DetalleContadoresGameWebBL();

        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarHoraUltimoContadorIAS(int sala) {
            var client = new System.Net.WebClient();
            var response = false;
            var jsonResponse = new ContadoresBonusInCompleto();
            var mensaje = "";
            try {

                response = true;
                mensaje = "Hora obtenida";

                string dateYesterday =  DateTime.Now.AddDays(-1).ToString();

                jsonResponse = contadoresBonusInBL.ObtenerUltimoContadorBonusIn(sala);

                if(jsonResponse.Hora.ToString() == "1/01/0001 00:00:00") {
                    return Json(new { respuesta = response, data = dateYesterday });
                }

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = jsonResponse.Hora.ToString() });
        }



        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInRecepcionIASJson(List<ContadoresBonusInCompleto> contadores, int sala) {
            var mensaje = "";
            var response = false;
            try {

                DateTime ultimaFecha = DateTime.Now.AddDays(-1);

                try {

                    ContadoresBonusInCompleto contadorUltimaFecha = contadoresBonusInBL.ObtenerUltimoContadorBonusIn(sala);
                    ultimaFecha = contadorUltimaFecha.Hora;

                }catch(Exception ex) {
                    ultimaFecha = DateTime.Now.AddDays(-1);
                }

                contadores = contadores.Where(x=>x.Hora>ultimaFecha).ToList();

                foreach(var item in contadores) {

                    item.CodSala = sala;

                    List<ContadoresBonusInCompleto> lista = new List<ContadoresBonusInCompleto>();

                    if(lista.Count == 0) {

                        var idInsertado = contadoresBonusInBL.GuardarContadoresBonusIn(item);

                        if(idInsertado <= 0) {
                            mensaje = "Error al guardar contador bonus in";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }

                    }

                }

                response = true;
                mensaje = "Guardado contadores bonus in";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }



        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInEnviarDetalleContadorBonusInFiltroFechasJson(int sala,string codMaq, DateTime fechaIni, DateTime fechaFin, int formula, int orden=1) {
            var mensaje = "";
            var response = false;

            List<DetalleContadorBonusInEntidad> listaDetalleEnvio = new List<DetalleContadorBonusInEntidad>();

            try {

				fechaIni = fechaIni.Date.AddDays(-1);
				fechaFin = fechaFin.Date;

				fechaIni = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala,codMaq, fechaIni, "ASC");
				fechaFin = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala,codMaq, fechaFin, "DESC");

				string querySala = "AND CodSala=" + sala;

                List<ContadoresBonusInCompleto> listaDetalleContadorBonusIn = new List<ContadoresBonusInCompleto>();
                List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonusInBL.ObtenerListadoDetalleContadorBonusInFiltroFechas(sala, codMaq, fechaIni.AddDays(-1), fechaFin);

				List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaHoy = listaDetalleContadorBonusInMaquina.Where(x => x.Hora >= fechaIni).OrderBy(x=>x.Hora).ToList();
				List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaAyer = listaDetalleContadorBonusInMaquina.Where(x => x.Hora < fechaIni).ToList();
				var listaMaquinas = listaDetalleContadorBonusInMaquinaHoy.Select(x => x.CodMaq).ToList();
				listaMaquinas = listaMaquinas.Distinct().ToList();

				ContadoresBonusInCompleto antDetalleContadorBonusIn = new ContadoresBonusInCompleto();
				try
				{
					antDetalleContadorBonusIn = listaDetalleContadorBonusInMaquinaAyer.Where(x => x.CodMaq == codMaq).OrderByDescending(x => x.Hora).First();

				} catch(Exception e)
				{

				}
								
				if(listaDetalleContadorBonusInMaquinaHoy.Any())
                {

                    if(orden == 1)
                    {
                        if(formula == 1)
                        {

                            if(listaDetalleContadorBonusInMaquinaHoy.First().EftIn != antDetalleContadorBonusIn.EftIn && listaDetalleContadorBonusInMaquinaHoy.First().TicketIn != antDetalleContadorBonusIn.TicketIn)
                            {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }

                        if(formula == 2)
                        {

                            if(listaDetalleContadorBonusInMaquinaHoy.First().tmpebw != antDetalleContadorBonusIn.tmpebw)
                            {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }
                    }

                    if(orden == 2)
                    {
                        if(listaDetalleContadorBonusInMaquinaHoy.First().tapebw != antDetalleContadorBonusIn.tapebw)
                        {
                            listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                        }
                    }

                    if(orden == 3)
                    {
                        if(listaDetalleContadorBonusInMaquinaHoy.First().tappw != antDetalleContadorBonusIn.tappw)
                        {
                            listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                        }
                    }


                    if(orden == 4)
                    {
                        if(listaDetalleContadorBonusInMaquinaHoy.First().tmppw != antDetalleContadorBonusIn.tmppw)
                        {
                            listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                        }
                    }
                }

                /**AQUI**/

				listaDetalleContadorBonusIn.AddRange(listaDetalleContadorBonusInMaquinaHoy);

                double EftInAnteriorAux = 0;
                double TicketInAnteriorAux = 0;
                double BonusInAnteriorAux = 0;

                bool primerContador = true;
                double diferenciaExistente = 0;

                if(orden == 1)
                {

					if(formula == 1)
					{

						foreach(var item in listaDetalleContadorBonusIn)
						{

							if(primerContador)
							{

								EftInAnteriorAux = item.EftIn;
								TicketInAnteriorAux = item.TicketIn;
								primerContador = false;

							} else
							{

								DetalleContadorBonusInEntidad detalle = new DetalleContadorBonusInEntidad();
								detalle.Fecha = item.Fecha;
								detalle.BonusInActual = item.EftIn - item.TicketIn;
								detalle.BonusInAnterior = EftInAnteriorAux - TicketInAnteriorAux;
								EftInAnteriorAux = item.EftIn;
								TicketInAnteriorAux = item.TicketIn;

								detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
								if(item.codevento == 251)
								{
									detalle.Desconexion = "Se detectó evento desconexion (251)";
								} else
								{
									detalle.Desconexion = "Sin observaciones";
								}

								if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente)
								{
									listaDetalleEnvio.Add(detalle);
								}

								diferenciaExistente = detalle.Diferencia;

							}

						}

					}

					if(formula == 2)
					{

						foreach(var item in listaDetalleContadorBonusIn)
						{

							if(primerContador)
							{

								BonusInAnteriorAux = item.tmpebw;
								primerContador = false;
							} else
							{

								DetalleContadorBonusInEntidad detalle = new DetalleContadorBonusInEntidad();
								detalle.Fecha = item.Fecha;
								detalle.BonusInActual = item.tmpebw;

								detalle.BonusInAnterior = BonusInAnteriorAux;
								BonusInAnteriorAux = detalle.BonusInActual;

								detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
								if(item.codevento == 251)
								{
									detalle.Desconexion = "Se detectó evento desconexion (251)";
								} else
								{
									detalle.Desconexion = "Sin observaciones";
								}
								if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente)
								{
									listaDetalleEnvio.Add(detalle);
								}
								diferenciaExistente = detalle.Diferencia;
							}

						}

					}
				}

                if(orden == 2)
                {
					foreach(var item in listaDetalleContadorBonusIn)
					{

						if(primerContador)
						{

							BonusInAnteriorAux = item.tapebw;
							primerContador = false;
						} else
						{

							DetalleContadorBonusInEntidad detalle = new DetalleContadorBonusInEntidad();
							detalle.Fecha = item.Fecha;
							detalle.BonusInActual = item.tapebw;

							detalle.BonusInAnterior = BonusInAnteriorAux;
							BonusInAnteriorAux = detalle.BonusInActual;

							detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
							if(item.codevento == 251)
							{
								detalle.Desconexion = "Se detectó evento desconexion (251)";
							} else
							{
								detalle.Desconexion = "Sin observaciones";
							}
							if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente)
							{
								listaDetalleEnvio.Add(detalle);
							}
							diferenciaExistente = detalle.Diferencia;
						}

					}
				}

				if(orden == 3)
				{
					foreach(var item in listaDetalleContadorBonusIn)
					{

						if(primerContador)
						{

							BonusInAnteriorAux = item.tappw;
							primerContador = false;
						} else
						{

							DetalleContadorBonusInEntidad detalle = new DetalleContadorBonusInEntidad();
							detalle.Fecha = item.Fecha;
							detalle.BonusInActual = item.tappw;

							detalle.BonusInAnterior = BonusInAnteriorAux;
							BonusInAnteriorAux = detalle.BonusInActual;

							detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
							if(item.codevento == 251)
							{
								detalle.Desconexion = "Se detectó evento desconexion (251)";
							} else
							{
								detalle.Desconexion = "Sin observaciones";
							}
							if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente)
							{
								listaDetalleEnvio.Add(detalle);
							}
							diferenciaExistente = detalle.Diferencia;
						}

					}
				}

				if(orden == 4)
				{
					foreach(var item in listaDetalleContadorBonusIn)
					{

						if(primerContador)
						{

							BonusInAnteriorAux = item.tmppw;
							primerContador = false;
						} else
						{

							DetalleContadorBonusInEntidad detalle = new DetalleContadorBonusInEntidad();
							detalle.Fecha = item.Fecha;
							detalle.BonusInActual = item.tmppw;

							detalle.BonusInAnterior = BonusInAnteriorAux;
							BonusInAnteriorAux = detalle.BonusInActual;

							detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
							if(item.codevento == 251)
							{
								detalle.Desconexion = "Se detectó evento desconexion (251)";
							} else
							{
								detalle.Desconexion = "Sin observaciones";
							}
							if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente)
							{
								listaDetalleEnvio.Add(detalle);
							}
							diferenciaExistente = detalle.Diferencia;
						}

					}
				}


				response = true;
                mensaje = "Obtenido Pagos IAS";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = listaDetalleEnvio, mensaje = mensaje.ToString() });
        }

        private class DetalleContadorBonusInEntidad {
            public DateTime Fecha { get; set; }
            public double BonusInActual { get; set; }
            public double BonusInAnterior { get; set; }
            public double Diferencia { get; set; }
            public string Desconexion { get; set; }
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInEnviarPagoIASFiltroFechasJson(int sala, DateTime fechaIni, DateTime fechaFin) {
            var mensaje = "";
            var response = false;

            List<PagoIASEntidad> listaPagosIAS = new List<PagoIASEntidad>();

            try {

				fechaIni = fechaIni.Date.AddDays(-1);
				fechaFin = fechaFin.Date;

				List<ContadoresOnlineBoton> fechaIniContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaIni);
				List<ContadoresOnlineBoton> fechaFinContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaFin);

				if(fechaIniContadoresMaquina.Any())
				{
					fechaIni = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Hora;
				} else
				{
					fechaIni = fechaIni.Date.AddDays(1);
				}
				if(fechaFinContadoresMaquina.Any())
				{
					fechaFin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Hora;
				} else
				{
					fechaFin = fechaFin.Date.AddDays(1);
				}

				string querySala = "AND CodSala=" + sala;

                List<CabeceraOfflineEntidad> listaCabeceras= progresivoBL.ListarCabecerasPorFechaYSala(fechaIni, fechaFin, querySala);
				List<CabeceraOfflineEntidad> listaRealCabeceras = new List<CabeceraOfflineEntidad>();

				//Create one object for machine
				foreach(var item in listaCabeceras) {


					DateTime fechaIniMaquina = fechaIni;
					DateTime fechaFinMaquina = fechaFin;

					var fechaIniContadorMaquina = fechaIniContadoresMaquina.FirstOrDefault(x => x.CodMaq == item.SlotID);
					if(fechaIniContadorMaquina != null)
					{
						fechaIniMaquina = fechaIniContadorMaquina.Hora;
					}
					var fechaFinContadorMaquina = fechaFinContadoresMaquina.FirstOrDefault(x => x.CodMaq == item.SlotID);
					if(fechaFinContadorMaquina != null)
					{
						fechaFinMaquina = fechaFinContadorMaquina.Hora;
					}

					List<CabeceraOfflineEntidad> listaAuxCabeceras = listaCabeceras.Where(x=> x.Fecha >= fechaIniMaquina && x.Fecha < fechaFinMaquina && x.SlotID==item.SlotID).ToList();

					if(listaAuxCabeceras.Any())
					{
						List<PagoIASEntidad> existe = listaPagosIAS.Where(x => x.CodMaq == item.SlotID).ToList();

						if(existe.Count == 0)
						{
							var objPagoIAS = new PagoIASEntidad();
							objPagoIAS.CodMaq = item.SlotID;
							objPagoIAS.PagoIAS = 0;
							listaPagosIAS.Add(objPagoIAS);
							listaRealCabeceras.AddRange(listaAuxCabeceras);
						}
					}

                }

				//Modify all objects with amount total
				foreach(var item in listaRealCabeceras) {
                    var index = listaPagosIAS.FindIndex(x => x.CodMaq == item.SlotID);
					listaPagosIAS[index].PagoIAS = listaPagosIAS[index].PagoIAS + item.Monto;

				}

                response = true;
                mensaje = "Obtenido Pagos IAS";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = listaPagosIAS, mensaje = mensaje.ToString() });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInEnviarPagoIASMultiSalaFiltroFechasJson(List<int> salas, DateTime fechaIni, DateTime fechaFin) {
            var mensaje = "";
            var response = false;

            List<PagoIASEntidad> listaPagosIAS = new List<PagoIASEntidad>();

			try {

				List<CabeceraOfflineEntidad> listaCabeceras = new List<CabeceraOfflineEntidad>();
				List<CabeceraOfflineEntidad> listaRealCabeceras = new List<CabeceraOfflineEntidad>();
				List<ContadoresOnlineBoton> fechaIniContadoresMaquina = new List<ContadoresOnlineBoton>();
				List<ContadoresOnlineBoton> fechaFinContadoresMaquina = new List<ContadoresOnlineBoton>();

				DateTime fechaIniReal = fechaIni.Date.AddDays(-1);
				DateTime fechaFinReal = fechaFin.Date;

				foreach(var sala in salas)
                {


					fechaIni = fechaIniReal;
					fechaFin = fechaFinReal;

					List<ContadoresOnlineBoton> fechaIniContadoresMaquinaAux = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaIni);
					List<ContadoresOnlineBoton> fechaFinContadoresMaquinaAux = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaFin);

					if(fechaIniContadoresMaquina.Any())
					{
						fechaIni = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Hora;
						fechaIniContadoresMaquina.AddRange(fechaIniContadoresMaquinaAux);
					} else
					{
						fechaIni = fechaIni.Date.AddDays(1);
					}
					if(fechaFinContadoresMaquina.Any())
					{
						fechaFin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Hora;
						fechaFinContadoresMaquina.AddRange(fechaFinContadoresMaquinaAux);
					} else
					{
						fechaFin = fechaFin.Date.AddDays(1);
					}

					string querySala = "AND CodSala=" + sala;

					List<CabeceraOfflineEntidad> listaCabecerasAux = progresivoBL.ListarCabecerasPorFechaYSala(fechaIni, fechaFin, querySala);
                    listaCabeceras.AddRange(listaCabecerasAux);

				}


                foreach(var item in listaCabeceras) {
					DateTime fechaIniMaquina = fechaIni;
					DateTime fechaFinMaquina = fechaFin;

					var fechaIniContadorMaquina = fechaIniContadoresMaquina.FirstOrDefault(x => x.CodMaq == item.SlotID);
					if(fechaIniContadorMaquina != null)
					{
						fechaIniMaquina = fechaIniContadorMaquina.Hora;
					}
					var fechaFinContadorMaquina = fechaFinContadoresMaquina.FirstOrDefault(x => x.CodMaq == item.SlotID);
					if(fechaFinContadorMaquina != null)
					{
						fechaFinMaquina = fechaFinContadorMaquina.Hora;
					}

					List<CabeceraOfflineEntidad> listaAuxCabeceras = listaCabeceras.Where(x => x.Fecha >= fechaIniMaquina && x.Fecha < fechaFinMaquina && x.SlotID == item.SlotID).ToList();

					if(listaAuxCabeceras.Any())
					{
						List<PagoIASEntidad> existe = listaPagosIAS.Where(x => x.CodMaq == item.SlotID).ToList();

						if(existe.Count == 0)
						{
							var objPagoIAS = new PagoIASEntidad();
							objPagoIAS.CodMaq = item.SlotID;
							objPagoIAS.PagoIAS = 0;
							listaPagosIAS.Add(objPagoIAS);
							listaRealCabeceras.AddRange(listaAuxCabeceras);
						}
					}
				}

                foreach(var item in listaRealCabeceras) {
                    var index = listaPagosIAS.FindIndex(x => x.CodMaq == item.SlotID);
                    listaPagosIAS[index].PagoIAS = listaPagosIAS[index].PagoIAS + item.Monto;

                }

                response = true;
                mensaje = "Obtenido Pagos IAS";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = listaPagosIAS, mensaje = mensaje.ToString() });
        }

        private class PagoIASEntidad {
            public string CodMaq { get; set; }
            public double PagoIAS { get; set; }
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInEnviarListadoFiltroFechasIASJson(int sala, DateTime fechaIni, DateTime fechaFin) {
            var mensaje = "";
            var response = false;

            List<ContadoresBonusInCompleto> listaContadores = new List<ContadoresBonusInCompleto>();

            try {
				fechaIni = fechaIni.Date.AddDays(-1);
				fechaFin = fechaFin.Date;
				
				List<ContadoresOnlineBoton> fechaIniContadoresMaquina= contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaIni);
				List<ContadoresOnlineBoton> fechaFinContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaFin);

				if(fechaIniContadoresMaquina.Any())
				{
					fechaIni = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Hora;
				} else
				{
					fechaIni = fechaIni.Date.AddDays(1);
				}
				if(fechaFinContadoresMaquina.Any())
				{
					fechaFin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Hora;
				} else
				{
					fechaFin = fechaFin.Date.AddDays(1);
				}

				List<ContadoresBonusInCompleto> listaCompletaContadores = contadoresBonusInBL.ObtenerListadoContadorBonusInFiltroFechas(sala, fechaIni.AddDays(-1), fechaFin);

				List<ContadoresBonusInCompleto> listaCompletaContadoresHoy = listaCompletaContadores.Where(x => x.Hora >= fechaIni).ToList();
				var listaMaquinas = listaCompletaContadoresHoy.Select(x => x.CodMaq).ToList();
				listaMaquinas = listaMaquinas.Distinct().ToList();

				foreach(var maquina in listaMaquinas)
				{

					DateTime fechaIniMaquina = fechaIni;
					DateTime fechaFinMaquina = fechaFin;

					var fechaIniContadorMaquina = fechaIniContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
					if(fechaIniContadorMaquina != null)
					{
						fechaIniMaquina = fechaIniContadorMaquina.Hora;
					}
					var fechaFinContadorMaquina = fechaFinContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
					if(fechaFinContadorMaquina != null)
					{
						fechaFinMaquina = fechaFinContadorMaquina.Hora;
					}


					List<ContadoresBonusInCompleto> listaCompletaContadoresMaquina = listaCompletaContadores.Where(x => x.CodMaq == maquina).ToList();

					List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaHoy = listaCompletaContadoresMaquina.Where(x => x.Hora >= fechaIniMaquina && x.Hora < fechaFinMaquina).ToList();
					List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaAyer = listaCompletaContadoresMaquina.Where(x => x.Hora < fechaIniMaquina).ToList();


					if(listaCompletaContadoresMaquinaHoy.Any())
					{

						var antDetalleContadorBonusIn = listaCompletaContadoresMaquinaAyer.OrderByDescending(x => x.Hora).FirstOrDefault();
						if(antDetalleContadorBonusIn != null)
						{
							listaCompletaContadoresMaquinaHoy.Add(antDetalleContadorBonusIn);
						} 

						ContadoresBonusInCompleto primeroLista = listaCompletaContadoresMaquinaHoy.OrderByDescending(x => x.Hora).First();
						listaContadores.Add(primeroLista);
						ContadoresBonusInCompleto ultimoLista = listaCompletaContadoresMaquinaHoy.OrderBy(x => x.Hora).First();
						listaContadores.Add(ultimoLista);
					}

				}

				response = true;
                mensaje = "Obtenido contadores bonus in";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }

            //var dataJson = new
            //{
            //    respuesta = response,
            //    data = listaContadores,
            //    mensaje = mensaje.ToString()
            //};

            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;

            //var result = new ContentResult {
            //    Content = serializer.Serialize(dataJson),
            //    ContentType = "application/json"
            //};
            //return result;

            return Json(new { respuesta = response,data=listaContadores, mensaje = mensaje.ToString() });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInEnviarListadoMultiSalaFiltroFechasIASJson(List<int> salas, DateTime fechaIni, DateTime fechaFin) {
            var mensaje = "";
            var response = false;

            List<ContadoresBonusInCompleto> listaContadores = new List<ContadoresBonusInCompleto>();

            try {


				DateTime fechaIniReal = fechaIni.Date.AddDays(-1);
				DateTime fechaFinReal = fechaFin.Date;

				foreach(var sala in salas) {

					List<ContadoresBonusInCompleto> listaContadoresAux = new List<ContadoresBonusInCompleto>();

					fechaIni = fechaIniReal;
					fechaFin = fechaFinReal;

					List<ContadoresOnlineBoton> fechaIniContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaIni);
					List<ContadoresOnlineBoton> fechaFinContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaFin);

					if(fechaIniContadoresMaquina.Any())
					{
						fechaIni = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Hora;
					} else
					{
						fechaIni = fechaIni.Date.AddDays(1);
					}
					if(fechaFinContadoresMaquina.Any())
					{
						fechaFin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Hora;
					} else
					{
						fechaFin = fechaFin.Date.AddDays(1);
					}

					List<ContadoresBonusInCompleto> listaCompletaContadores = contadoresBonusInBL.ObtenerListadoContadorBonusInFiltroFechas(sala, fechaIni.AddDays(-1), fechaFin);

					List<ContadoresBonusInCompleto> listaCompletaContadoresHoy = listaCompletaContadores.Where(x => x.Hora >= fechaIni).ToList();
					var listaMaquinas = listaCompletaContadoresHoy.Select(x => x.CodMaq).ToList();
					listaMaquinas = listaMaquinas.Distinct().ToList();

					foreach(var maquina in listaMaquinas)
					{

						DateTime fechaIniMaquina = fechaIni;
						DateTime fechaFinMaquina = fechaFin;

						var fechaIniContadorMaquina = fechaIniContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
						if(fechaIniContadorMaquina != null)
						{
							fechaIniMaquina = fechaIniContadorMaquina.Hora;
						}
						var fechaFinContadorMaquina = fechaFinContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
						if(fechaFinContadorMaquina != null)
						{
							fechaFinMaquina = fechaFinContadorMaquina.Hora;
						}


						List<ContadoresBonusInCompleto> listaCompletaContadoresMaquina = listaCompletaContadores.Where(x => x.CodMaq == maquina).ToList();

						List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaHoy = listaCompletaContadoresMaquina.Where(x => x.Hora >= fechaIniMaquina && x.Hora < fechaFinMaquina).ToList();
						List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaAyer = listaCompletaContadoresMaquina.Where(x => x.Hora < fechaIniMaquina).ToList();


						if(listaCompletaContadoresMaquinaHoy.Any())
						{

							var antDetalleContadorBonusIn = listaCompletaContadoresMaquinaAyer.OrderByDescending(x => x.Hora).FirstOrDefault();
							if(antDetalleContadorBonusIn != null)
							{
								listaCompletaContadoresMaquinaHoy.Add(antDetalleContadorBonusIn);
							}

							ContadoresBonusInCompleto primeroLista = listaCompletaContadoresMaquinaHoy.OrderByDescending(x => x.Hora).First();
							listaContadoresAux.Add(primeroLista);
							ContadoresBonusInCompleto ultimoLista = listaCompletaContadoresMaquinaHoy.OrderBy(x => x.Hora).First();
							listaContadoresAux.Add(ultimoLista);
						}

					}


					listaContadores.AddRange(listaContadoresAux);
                }


                response = true;
                mensaje = "Obtenido contadores bonus in";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }

            var dataJson = new
            {
                respuesta = response,
                data = listaContadores,
                mensaje = mensaje.ToString()
            };

			//var serializer = new JavaScriptSerializer();
			//serializer.MaxJsonLength = Int32.MaxValue;

			//var result = new ContentResult {
			//    Content = serializer.Serialize(dataJson),
			//    ContentType = "application/json"
			//};
			//return result;
			var result = Json(new { respuesta = response, data = listaContadores, mensaje = mensaje.ToString() });
            result.MaxJsonLength = int.MaxValue;
			return result;
			//return Json(new { respuesta = response,data=listaContadores, mensaje = mensaje.ToString() });
        }




        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadoresBonusInEnviarTotalBonusInJson(int sala, string codMaq, DateTime fecha,int formula) {
            var mensaje = "";
            var response = false;

            double totalBonusIn = 0;    

            try {

				DateTime fechaIni = fecha.Date.AddDays(-1);
				DateTime fechaFin = fecha.Date;

				fechaIni = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, codMaq, fechaIni, "ASC");
				fechaFin = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, codMaq, fechaFin, "DESC");

				string querySala = "AND CodSala=" + sala;

                List<ContadoresBonusInCompleto> listaDetalleContadorBonusIn = new List<ContadoresBonusInCompleto>();

                List<ContadoresBonusInCompleto>  listaDetalleContadorBonusInHoy = contadoresBonusInBL.ObtenerContadoresBonusInEnviarTotalBonusInJson(sala, codMaq, fechaIni,fechaFin);

                listaDetalleContadorBonusIn.AddRange(listaDetalleContadorBonusInHoy);

                bool primerBonusIn = true;

                if(formula == 1) {

                    double EftInAnteriorAux = 0;
                    double TicketInAnteriorAux = 0;

                    foreach(var item in listaDetalleContadorBonusIn) {

                        var EftInActualAux = item.EftIn * item.Token;
                        var TicketInActualAux = item.TicketIn * item.Token;

                        if(primerBonusIn) {

                            EftInAnteriorAux = EftInActualAux;
                            TicketInAnteriorAux = TicketInActualAux;
                            primerBonusIn = false;

                        } else {

                            totalBonusIn = totalBonusIn + ((EftInActualAux - EftInAnteriorAux) - (TicketInActualAux - TicketInAnteriorAux));
                            EftInAnteriorAux = EftInActualAux;
                            TicketInAnteriorAux = TicketInActualAux;

                        }
                    }

                }

                if(formula == 2) {

                    double BonusInAnteriorAux = 0;

                    foreach(var item in listaDetalleContadorBonusIn) {

                        var BonisInActualAux = item.tmpebw * item.Token;

                        if(primerBonusIn) {

                            BonusInAnteriorAux = BonisInActualAux;
                            primerBonusIn = false;

                        } else {

                            totalBonusIn = totalBonusIn + BonisInActualAux - BonusInAnteriorAux;
                            BonusInAnteriorAux = BonisInActualAux;
                        }
                    }

                } 

                response = true;
                mensaje = "Obtenido Pagos IAS";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = totalBonusIn, mensaje = mensaje.ToString() });
        }

		[seguridad(false)]
		[HttpPost]
		public ActionResult ConsultarHoraUltimoContadorBotonIAS(int sala)
		{
			var client = new System.Net.WebClient();
			var response = false;
			var jsonResponse = new ContadoresBonusInCompleto();
			var mensaje = "";
			try
			{

				response = true;
				mensaje = "Hora obtenida";

				string dateYesterday = DateTime.Now.AddDays(-1).ToString();

				jsonResponse = contadoresBonusInBL.ObtenerUltimoContadorBonusIn(sala);

				if(jsonResponse.Hora.ToString() == "1/01/0001 00:00:00")
				{
					return Json(new { respuesta = response, data = dateYesterday });
				}

			} catch(Exception ex)
			{
				response = false;
				return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
			}
			return Json(new { respuesta = response, data = jsonResponse.Hora.ToString() });
		}



		[seguridad(false)]
		[HttpPost]
		public ActionResult ContadoresBotonRecepcionIASJson(List<ContadoresOnlineBoton> contadores)
		{
			var mensaje = "";
			var response = false;
			try
			{
                if(contadores.Count > 0)
                {


					DateTime ultimaFecha = DateTime.Now.AddDays(-1);
                    int sala = contadores.First().CodSala;


					try
					{

						ultimaFecha = contadoresBonusInBL.ObtenerHoraUltimoContadorBoton(sala);

					} catch(Exception ex)
					{
						ultimaFecha = DateTime.Now.AddDays(-1);
					}

					contadores = contadores.Where(x => x.Hora > ultimaFecha).ToList();

					foreach(var item in contadores)
					{

						var idInsertado = contadoresBonusInBL.GuardarContadoresOnlineBoton(item);

						if(idInsertado <= 0)
						{
							mensaje = "Error al guardar contador boton";
							response = false;
							return Json(new { respuesta = response, mensaje = mensaje.ToString() });
						}

					}
				}

				response = true;
				mensaje = "Guardado contadores boton";

			} catch(Exception ex)
			{
				response = false;
				return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
			}
			return Json(new { respuesta = response, mensaje = mensaje.ToString() });
		}
		//[seguridad(false)]
		//[HttpPost]
		//public ActionResult ObtenerListadoContadoreBonusInPorRango(int sala, DateTime fechaIni, DateTime fechaFin) {
		//          var mensaje = "";
		//          var response = false;
		//	var data = new List<ContadoresBonusInCompleto>();
		//          try {
		//		data = contadoresBonusInBL.ObtenerListadoContadoreBonusInPorRango(sala, fechaIni, fechaFin);
		//              response = true;
		//              mensaje = "Guardado contadores boton";

		//          } catch(Exception ex) {
		//              response = false;
		//              return Json(new { respuesta = response, mensaje = mensaje.ToString() });
		//          }
		//          var result = Json(new { respuesta = response, data = data, mensaje = mensaje.ToString() });
		//          result.MaxJsonLength = int.MaxValue;
		//          return result;
		//          //return Json(new { respuesta = response, data = data, mensaje = mensaje.ToString() });
		//      }
		[seguridad(false)]
		[HttpPost]
		public ActionResult ObtenerListadoContadoreBonusInPorRangoV2(int sala, DateTime fechaIni, DateTime fechaFin) {
			var mensaje = "";
			var response = false;
			var data = new List<ContadoresBonusInCompleto>();
            List<ContadoresBonusInCompleto> listaContadores = new List<ContadoresBonusInCompleto>();

            try {
				
				var rangoFechas = ObtenerDiasEntreFechas(fechaIni,fechaFin);
				var fechaInicial = fechaIni.AddDays(-1);
				var fechaFinal = fechaFin;
				var fechaPrimerContadorBoton = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaInicial);
				var fechaUltimoContadorBoton = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fechaFinal);

				var contadoresBotonGeneral = contadoresBonusInBL.GetContadoresOnlineBotonPorRangoFechas(sala, fechaInicial, fechaFin);


                if(fechaPrimerContadorBoton.Any()) {
					fechaInicial = fechaPrimerContadorBoton.OrderBy(x => x.Hora).First().Hora;
				} else {
					fechaInicial = fechaInicial.AddDays(1);
				}
				if(fechaUltimoContadorBoton.Any()) {
					fechaFinal = fechaUltimoContadorBoton.OrderByDescending(x => x.Hora).First().Hora;
				} else {
					fechaFinal = fechaFinal.Date.AddDays(1);
				}

				var contadoresBonusInPorRangoGeneral = contadoresBonusInBL.ObtenerListadoContadorBonusInFiltroFechas(sala, fechaInicial.AddDays(-1), fechaFinal.AddDays(1));

                foreach(var fecha in rangoFechas) {
                    var fini = fecha.Date.AddDays(-1);
                    var ffin = fecha;
                    var foperacionini = fecha;
                    var foperacionfin = fecha;
					//List<ContadoresOnlineBoton> fechaIniContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fini);
					//List<ContadoresOnlineBoton> fechaFinContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, ffin);
					List<ContadoresOnlineBoton> fechaIniContadoresMaquina = contadoresBotonGeneral.Where(x => x.CodSala == sala && x.Fecha.Date == fini).ToList();
					List<ContadoresOnlineBoton> fechaFinContadoresMaquina = contadoresBotonGeneral.Where(x => x.CodSala == sala && x.Fecha.Date == ffin).ToList();
                    if(fechaIniContadoresMaquina.Any()) {
                        fini = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Hora;
                        foperacionini = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Fecha;
                    } else {
                        fini = fini.Date.AddDays(1);
                    }
                    if(fechaFinContadoresMaquina.Any()) {
                        ffin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Hora;
                        foperacionfin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Fecha;
                    } else {
                        ffin = ffin.Date.AddDays(1);
                    }

					List<ContadoresBonusInCompleto> listaCompletaContadores = contadoresBonusInPorRangoGeneral.Where(x => x.CodSala == sala && x.Hora >= fini.AddDays(-1) && x.Hora <= ffin).ToList();
                    //List<ContadoresBonusInCompleto> listaCompletaContadores = contadoresBonusInBL.ObtenerListadoContadorBonusInFiltroFechas(sala, fini.AddDays(-1), ffin);

                    List<ContadoresBonusInCompleto> listaCompletaContadoresHoy = listaCompletaContadores.Where(x => x.Hora >= fini).ToList();
                    var listaMaquinas = listaCompletaContadoresHoy.Select(x => x.CodMaq).Distinct().ToList();

                    foreach(var maquina in listaMaquinas) {

                        DateTime fechaIniMaquina = fini;
                        DateTime fechaFinMaquina = ffin;

                        var fechaIniContadorMaquina = fechaIniContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
                        if(fechaIniContadorMaquina != null) {
                            fechaIniMaquina = fechaIniContadorMaquina.Hora;
                        }
                        var fechaFinContadorMaquina = fechaFinContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
                        if(fechaFinContadorMaquina != null) {
                            fechaFinMaquina = fechaFinContadorMaquina.Hora;
                        }


                        List<ContadoresBonusInCompleto> listaCompletaContadoresMaquina = listaCompletaContadores.Where(x => x.CodMaq == maquina).ToList();

                        List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaHoy = listaCompletaContadoresMaquina.Where(x => x.Hora >= fechaIniMaquina && x.Hora < fechaFinMaquina).ToList();
                        List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaAyer = listaCompletaContadoresMaquina.Where(x => x.Hora < fechaIniMaquina).ToList();


                        if(listaCompletaContadoresMaquinaHoy.Any()) {

                            var antDetalleContadorBonusIn = listaCompletaContadoresMaquinaAyer.OrderByDescending(x => x.Hora).FirstOrDefault();
                            if(antDetalleContadorBonusIn != null) {
                                listaCompletaContadoresMaquinaHoy.Add(antDetalleContadorBonusIn);
                            }

                            ContadoresBonusInCompleto primeroLista = listaCompletaContadoresMaquinaHoy.OrderByDescending(x => x.Hora).First();
							primeroLista.fOperacion = primeroLista.Fecha.Date;
							if(!listaContadores.Any(x=>x.Cod_Cont_OL == primeroLista.Cod_Cont_OL)) {
                                listaContadores.Add(primeroLista);
                            }
                            ContadoresBonusInCompleto ultimoLista = listaCompletaContadoresMaquinaHoy.OrderBy(x => x.Hora).First();
							ultimoLista.fOperacion = ultimoLista.Fecha.Date;
							if(!listaContadores.Any(x=>x.Cod_Cont_OL == ultimoLista.Cod_Cont_OL)) {
                                listaContadores.Add(ultimoLista);
                            }
                        }
                    }
                }
                response = true;
            } catch(Exception) {
				response = false;
				return Json(new { respuesta = response, mensaje = mensaje.ToString() });
			}
            var result = Json(new { respuesta = response, data = listaContadores, mensaje = mensaje.ToString() });
			result.MaxJsonLength = int.MaxValue;
			return result;
		}
        private static List<DateTime> ObtenerDiasEntreFechas(DateTime fechaInicio, DateTime fechaFin) {
            if(fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio debe ser anterior o igual a la fecha de fin.");

            List<DateTime> dias = new List<DateTime>();
            DateTime actual = fechaInicio;

            while(actual <= fechaFin) {
                dias.Add(actual);
                actual = actual.AddDays(1);
            }

            return dias;
        }
		[seguridad(false)]
		[HttpPost]
		public ActionResult ContadoresBonusInEnviarDetalleContadorBonusInFiltroFechasJsonExcel(ExcelBonusIn data) {
            var mensaje = "";
            var response = false;

            List<DetalleContadorBonusIn> listaDetalleEnvio = new List<DetalleContadorBonusIn>();
            int orden = 0;
            int formula = 0;
            try {
                var fini = Convert.ToDateTime(data.FechaInicial);
                var ffin = Convert.ToDateTime(data.FechaFinal);
                fini = fini.Date.AddDays(-1);
                ffin = ffin.Date;
                var sala = data.CodSala;
                var contadoresBoton = contadoresBonusInBL.GetContadoresOnlineBotonPorRangoFechas(data.CodSala, fini.AddDays(-1), ffin);
                var contadoresBonuIn = contadoresBonusInBL.ObtenerListadoContadorBonusInFiltroFechas(data.CodSala, fini.AddDays(-1), ffin.AddDays(2));
                foreach(var dataMaquina in data.detalles) {
                    if(dataMaquina.BonusInDiferencia == 0) {
                        continue;
                    }
                    orden = 1;
                    var fechaIni = fini;
                    var fechaFin = ffin;
                    formula = dataMaquina.FormulaBonusIn;
                    var maq = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault();
                    if(maq == null) {
                        fechaIni = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaIni, "ASC");
                    } else {
                        fechaIni = maq.Hora;
                    }
                    var maqFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault();
                    if(maqFin == null) {
                        fechaFin = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaFin, "DESC");
                    } else {
                        fechaFin = maqFin.Hora;
                    }
                    //fechaIni = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault().Hora;
                    //fechaFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault().Hora;

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusIn = new List<ContadoresBonusInCompleto>();

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonuIn.Where(x => x.Hora >= fechaIni.AddDays(-1) && x.Hora <= fechaFin && x.CodMaq == dataMaquina.CodMaq).ToList();
                    //List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonusInBL.ObtenerListadoDetalleContadorBonusInFiltroFechas(sala, codMaq, fechaIni.AddDays(-1), fechaFin);

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaHoy = listaDetalleContadorBonusInMaquina.Where(x => x.Hora >= fechaIni).OrderBy(x => x.Hora).ToList();
                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaAyer = listaDetalleContadorBonusInMaquina.Where(x => x.Hora < fechaIni).ToList();

                    ContadoresBonusInCompleto antDetalleContadorBonusIn = new ContadoresBonusInCompleto();
                    try {
                        antDetalleContadorBonusIn = listaDetalleContadorBonusInMaquinaAyer.Where(x => x.CodMaq == dataMaquina.CodMaq).OrderByDescending(x => x.Hora).First();

                    } catch(Exception e) {

                    }

                    if(listaDetalleContadorBonusInMaquinaHoy.Any()) {

                        if(orden == 1) {
                            if(formula == 1) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().EftIn != antDetalleContadorBonusIn.EftIn && listaDetalleContadorBonusInMaquinaHoy.First().TicketIn != antDetalleContadorBonusIn.TicketIn) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }

                            if(formula == 2) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().tmpebw != antDetalleContadorBonusIn.tmpebw) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }
                        }

                        if(orden == 2) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tapebw != antDetalleContadorBonusIn.tapebw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }

                        if(orden == 3) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tappw != antDetalleContadorBonusIn.tappw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }


                        if(orden == 4) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tmppw != antDetalleContadorBonusIn.tmppw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }
                    }

                    /**AQUI**/

                    listaDetalleContadorBonusIn.AddRange(listaDetalleContadorBonusInMaquinaHoy);

                    double EftInAnteriorAux = 0;
                    double TicketInAnteriorAux = 0;
                    double BonusInAnteriorAux = 0;

                    bool primerContador = true;
                    double diferenciaExistente = 0;

                    if(orden == 1) {

                        if(formula == 1) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;
                                    primerContador = false;

                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.EftIn - item.TicketIn;
                                    detalle.BonusInAnterior = EftInAnteriorAux - TicketInAnteriorAux;
                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }

                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }

                                    diferenciaExistente = detalle.Diferencia;

                                }

                            }

                        }

                        if(formula == 2) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    BonusInAnteriorAux = item.tmpebw;
                                    primerContador = false;
                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.TipoBonusIn = orden;
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.tmpebw;

                                    detalle.BonusInAnterior = BonusInAnteriorAux;
                                    BonusInAnteriorAux = detalle.BonusInActual;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }
                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }
                                    diferenciaExistente = detalle.Diferencia;
                                }

                            }

                        }
                    }

                    if(orden == 2) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tapebw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.TipoBonusIn = orden;
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tapebw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 3) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tappw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tappw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 4) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tmppw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tmppw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }
                }
                foreach(var dataMaquina in data.detalles) {
                    if(dataMaquina.BonusInDiferencia2 == 0) {
                        continue;
                    }
                    var fechaIni = fini;
                    var fechaFin = ffin;
                    orden = 2;
                    formula = dataMaquina.FormulaBonusIn;
                    var maq = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault();
                    if(maq == null) {
                        fechaIni = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaIni, "ASC");
                    } else {
                        fechaIni = maq.Hora;
                    }
                    var maqFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault();
                    if(maqFin == null) {
                        fechaFin = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaFin, "DESC");
                    } else {
                        fechaFin = maqFin.Hora;
                    }
                    //fechaIni = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault().Hora;
                    //fechaFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault().Hora;

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusIn = new List<ContadoresBonusInCompleto>();

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonuIn.Where(x => x.Hora >= fechaIni.AddDays(-1) && x.Hora <= fechaFin && x.CodMaq == dataMaquina.CodMaq).ToList();
                    //List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonusInBL.ObtenerListadoDetalleContadorBonusInFiltroFechas(sala, codMaq, fechaIni.AddDays(-1), fechaFin);

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaHoy = listaDetalleContadorBonusInMaquina.Where(x => x.Hora >= fechaIni).OrderBy(x => x.Hora).ToList();
                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaAyer = listaDetalleContadorBonusInMaquina.Where(x => x.Hora < fechaIni).ToList();

                    ContadoresBonusInCompleto antDetalleContadorBonusIn = new ContadoresBonusInCompleto();
                    try {
                        antDetalleContadorBonusIn = listaDetalleContadorBonusInMaquinaAyer.Where(x => x.CodMaq == dataMaquina.CodMaq).OrderByDescending(x => x.Hora).First();

                    } catch(Exception e) {

                    }

                    if(listaDetalleContadorBonusInMaquinaHoy.Any()) {

                        if(orden == 1) {
                            if(formula == 1) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().EftIn != antDetalleContadorBonusIn.EftIn && listaDetalleContadorBonusInMaquinaHoy.First().TicketIn != antDetalleContadorBonusIn.TicketIn) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }

                            if(formula == 2) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().tmpebw != antDetalleContadorBonusIn.tmpebw) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }
                        }

                        if(orden == 2) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tapebw != antDetalleContadorBonusIn.tapebw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }

                        if(orden == 3) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tappw != antDetalleContadorBonusIn.tappw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }


                        if(orden == 4) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tmppw != antDetalleContadorBonusIn.tmppw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }
                    }

                    /**AQUI**/

                    listaDetalleContadorBonusIn.AddRange(listaDetalleContadorBonusInMaquinaHoy);

                    double EftInAnteriorAux = 0;
                    double TicketInAnteriorAux = 0;
                    double BonusInAnteriorAux = 0;

                    bool primerContador = true;
                    double diferenciaExistente = 0;

                    if(orden == 1) {

                        if(formula == 1) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;
                                    primerContador = false;

                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.EftIn - item.TicketIn;
                                    detalle.BonusInAnterior = EftInAnteriorAux - TicketInAnteriorAux;
                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }

                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }

                                    diferenciaExistente = detalle.Diferencia;

                                }

                            }

                        }

                        if(formula == 2) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    BonusInAnteriorAux = item.tmpebw;
                                    primerContador = false;
                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.tmpebw;

                                    detalle.BonusInAnterior = BonusInAnteriorAux;
                                    BonusInAnteriorAux = detalle.BonusInActual;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }
                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }
                                    diferenciaExistente = detalle.Diferencia;
                                }

                            }

                        }
                    }

                    if(orden == 2) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tapebw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tapebw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 3) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tappw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tappw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 4) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tmppw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tmppw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }
                }
                foreach(var dataMaquina in data.detalles) {
                    if(dataMaquina.BonusInDiferencia3 == 0) {
                        continue;
                    }
                    orden = 3;
                    var fechaIni = fini;
                    var fechaFin = ffin;
                    formula = dataMaquina.FormulaBonusIn;
                    var maq = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault();
                    if(maq == null) {
                        fechaIni = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaIni, "ASC");
                    } else {
                        fechaIni = maq.Hora;
                    }
                    var maqFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault();
                    if(maqFin == null) {
                        fechaFin = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaFin, "DESC");
                    } else {
                        fechaFin = maqFin.Hora;
                    }
                    //fechaIni = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault().Hora;
                    //fechaFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault().Hora;

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusIn = new List<ContadoresBonusInCompleto>();

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonuIn.Where(x => x.Hora >= fechaIni.AddDays(-1) && x.Hora <= fechaFin && x.CodMaq == dataMaquina.CodMaq).ToList();
                    //List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonusInBL.ObtenerListadoDetalleContadorBonusInFiltroFechas(sala, codMaq, fechaIni.AddDays(-1), fechaFin);

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaHoy = listaDetalleContadorBonusInMaquina.Where(x => x.Hora >= fechaIni).OrderBy(x => x.Hora).ToList();
                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaAyer = listaDetalleContadorBonusInMaquina.Where(x => x.Hora < fechaIni).ToList();

                    ContadoresBonusInCompleto antDetalleContadorBonusIn = new ContadoresBonusInCompleto();
                    try {
                        antDetalleContadorBonusIn = listaDetalleContadorBonusInMaquinaAyer.Where(x => x.CodMaq == dataMaquina.CodMaq).OrderByDescending(x => x.Hora).First();

                    } catch(Exception e) {

                    }

                    if(listaDetalleContadorBonusInMaquinaHoy.Any()) {

                        if(orden == 1) {
                            if(formula == 1) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().EftIn != antDetalleContadorBonusIn.EftIn && listaDetalleContadorBonusInMaquinaHoy.First().TicketIn != antDetalleContadorBonusIn.TicketIn) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }

                            if(formula == 2) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().tmpebw != antDetalleContadorBonusIn.tmpebw) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }
                        }

                        if(orden == 2) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tapebw != antDetalleContadorBonusIn.tapebw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }

                        if(orden == 3) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tappw != antDetalleContadorBonusIn.tappw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }


                        if(orden == 4) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tmppw != antDetalleContadorBonusIn.tmppw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }
                    }

                    /**AQUI**/

                    listaDetalleContadorBonusIn.AddRange(listaDetalleContadorBonusInMaquinaHoy);

                    double EftInAnteriorAux = 0;
                    double TicketInAnteriorAux = 0;
                    double BonusInAnteriorAux = 0;

                    bool primerContador = true;
                    double diferenciaExistente = 0;

                    if(orden == 1) {

                        if(formula == 1) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;
                                    primerContador = false;

                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.EftIn - item.TicketIn;
                                    detalle.BonusInAnterior = EftInAnteriorAux - TicketInAnteriorAux;
                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }

                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }

                                    diferenciaExistente = detalle.Diferencia;

                                }

                            }

                        }

                        if(formula == 2) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    BonusInAnteriorAux = item.tmpebw;
                                    primerContador = false;
                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.tmpebw;

                                    detalle.BonusInAnterior = BonusInAnteriorAux;
                                    BonusInAnteriorAux = detalle.BonusInActual;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }
                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }
                                    diferenciaExistente = detalle.Diferencia;
                                }

                            }

                        }
                    }

                    if(orden == 2) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tapebw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tapebw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 3) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tappw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tappw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 4) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tmppw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tmppw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }
                }
                foreach(var dataMaquina in data.detalles) {
                    if(dataMaquina.BonusInDiferencia4 == 0) {
                        continue;
                    }
                    //if(dataMaquina.CodMaq == "00042782") {
                    //    var a = 1;
                    //}
                    orden = 4;
                    var fechaIni = fini;
                    var fechaFin = ffin;
                    formula = dataMaquina.FormulaBonusIn;
                    var maq = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault();
                    if(maq == null) {
                        fechaIni = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaIni, "ASC");
                    } else {
                        fechaIni = maq.Hora;
                    }
                    var maqFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault();
                    if(maqFin == null) {
                        fechaFin = contadoresBonusInBL.GetHoraMaquinaContadoresOnlineBoton(sala, dataMaquina.CodMaq, fechaFin, "DESC");
                    } else {
                        fechaFin = maqFin.Hora;
                    }
                    //fechaIni = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaIni.Date).OrderBy(x => x.Hora).FirstOrDefault().Hora;
                    //fechaFin = contadoresBoton.Where(x => x.CodMaq == dataMaquina.CodMaq && x.Fecha.Date == fechaFin.Date).OrderBy(x => x.Hora).LastOrDefault().Hora;

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusIn = new List<ContadoresBonusInCompleto>();

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonuIn.Where(x => x.Hora >= fechaIni.AddDays(-1) && x.Hora <= fechaFin && x.CodMaq == dataMaquina.CodMaq).ToList();
                    //List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquina = contadoresBonusInBL.ObtenerListadoDetalleContadorBonusInFiltroFechas(sala, codMaq, fechaIni.AddDays(-1), fechaFin);

                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaHoy = listaDetalleContadorBonusInMaquina.Where(x => x.Hora >= fechaIni).OrderBy(x => x.Hora).ToList();
                    List<ContadoresBonusInCompleto> listaDetalleContadorBonusInMaquinaAyer = listaDetalleContadorBonusInMaquina.Where(x => x.Hora < fechaIni).ToList();

                    ContadoresBonusInCompleto antDetalleContadorBonusIn = new ContadoresBonusInCompleto();
                    try {
                        antDetalleContadorBonusIn = listaDetalleContadorBonusInMaquinaAyer.Where(x => x.CodMaq == dataMaquina.CodMaq).OrderByDescending(x => x.Hora).First();

                    } catch(Exception e) {

                    }

                    if(listaDetalleContadorBonusInMaquinaHoy.Any()) {

                        if(orden == 1) {
                            if(formula == 1) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().EftIn != antDetalleContadorBonusIn.EftIn && listaDetalleContadorBonusInMaquinaHoy.First().TicketIn != antDetalleContadorBonusIn.TicketIn) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }

                            if(formula == 2) {

                                if(listaDetalleContadorBonusInMaquinaHoy.First().tmpebw != antDetalleContadorBonusIn.tmpebw) {
                                    listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                                }
                            }
                        }

                        if(orden == 2) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tapebw != antDetalleContadorBonusIn.tapebw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }

                        if(orden == 3) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tappw != antDetalleContadorBonusIn.tappw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }


                        if(orden == 4) {
                            if(listaDetalleContadorBonusInMaquinaHoy.First().tmppw != antDetalleContadorBonusIn.tmppw) {
                                listaDetalleContadorBonusIn.Add(antDetalleContadorBonusIn);
                            }
                        }
                    }

                    /**AQUI**/

                    listaDetalleContadorBonusIn.AddRange(listaDetalleContadorBonusInMaquinaHoy);

                    double EftInAnteriorAux = 0;
                    double TicketInAnteriorAux = 0;
                    double BonusInAnteriorAux = 0;

                    bool primerContador = true;
                    double diferenciaExistente = 0;

                    if(orden == 1) {

                        if(formula == 1) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;
                                    primerContador = false;

                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.EftIn - item.TicketIn;
                                    detalle.BonusInAnterior = EftInAnteriorAux - TicketInAnteriorAux;
                                    EftInAnteriorAux = item.EftIn;
                                    TicketInAnteriorAux = item.TicketIn;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }

                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }

                                    diferenciaExistente = detalle.Diferencia;

                                }

                            }

                        }

                        if(formula == 2) {

                            foreach(var item in listaDetalleContadorBonusIn) {

                                if(primerContador) {

                                    BonusInAnteriorAux = item.tmpebw;
                                    primerContador = false;
                                } else {

                                    DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                    detalle.CodMaq = dataMaquina.CodMaq;
                                    detalle.TipoBonusIn = orden;
                                    detalle.Fecha = Convert.ToString(item.Fecha);
                                    detalle.BonusInActual = item.tmpebw;

                                    detalle.BonusInAnterior = BonusInAnteriorAux;
                                    BonusInAnteriorAux = detalle.BonusInActual;

                                    detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                    if(item.codevento == 251) {
                                        detalle.Desconexion = "Se detectó evento desconexion (251)";
                                    } else {
                                        detalle.Desconexion = "Sin observaciones";
                                    }
                                    if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                        listaDetalleEnvio.Add(detalle);
                                    }
                                    diferenciaExistente = detalle.Diferencia;
                                }

                            }

                        }
                    }

                    if(orden == 2) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tapebw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tapebw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 3) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tappw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tappw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }

                    if(orden == 4) {
                        foreach(var item in listaDetalleContadorBonusIn) {

                            if(primerContador) {

                                BonusInAnteriorAux = item.tmppw;
                                primerContador = false;
                            } else {

                                DetalleContadorBonusIn detalle = new DetalleContadorBonusIn();
                                detalle.CodMaq = dataMaquina.CodMaq;
                                detalle.TipoBonusIn = orden;
                                detalle.Fecha = Convert.ToString(item.Fecha);
                                detalle.BonusInActual = item.tmppw;

                                detalle.BonusInAnterior = BonusInAnteriorAux;
                                BonusInAnteriorAux = detalle.BonusInActual;

                                detalle.Diferencia = (detalle.BonusInActual - detalle.BonusInAnterior) * item.Token;
                                if(item.codevento == 251) {
                                    detalle.Desconexion = "Se detectó evento desconexion (251)";
                                } else {
                                    detalle.Desconexion = "Sin observaciones";
                                }
                                if(detalle.Diferencia != 0 && detalle.Diferencia != diferenciaExistente) {
                                    listaDetalleEnvio.Add(detalle);
                                }
                                diferenciaExistente = detalle.Diferencia;
                            }

                        }
                    }
                }

                response = true;
                mensaje = "Obtenido Detalles";

            } catch(Exception ex) {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = listaDetalleEnvio, mensaje = mensaje.ToString() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerListadoContadoreBonusInPorRango(int sala, DateTime fechaIni, DateTime fechaFin) {
            var mensaje = "";
            var response = false;
            var data = new List<ContadoresBonusInCompleto>();
            List<ContadoresBonusInCompleto> listaContadores = new List<ContadoresBonusInCompleto>();

            try {

                var rangoFechas = ObtenerDiasEntreFechas(fechaIni, fechaFin);

                foreach(var fechaOperacion in rangoFechas) {
                    var fini = fechaOperacion.Date.AddDays(-1);
                    var ffin = fechaOperacion.Date;

                    List<ContadoresOnlineBoton> fechaIniContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, fini);
                    List<ContadoresOnlineBoton> fechaFinContadoresMaquina = contadoresBonusInBL.GetHoraContadoresOnlineBotonAllMaquina(sala, ffin);

                    if(fechaIniContadoresMaquina.Any()) {
                        fini = fechaIniContadoresMaquina.OrderBy(x => x.Hora).First().Hora;
                    } else {
                        fini = fini.Date.AddDays(1);
                    }
                    if(fechaFinContadoresMaquina.Any()) {
                        ffin = fechaFinContadoresMaquina.OrderByDescending(x => x.Hora).First().Hora;
                    } else {
                        ffin = ffin.Date.AddDays(1);
                    }

                    List<ContadoresBonusInCompleto> listaCompletaContadores = contadoresBonusInBL.ObtenerListadoContadorBonusInFiltroFechas(sala, fini.AddDays(-1), ffin);

                    List<ContadoresBonusInCompleto> listaCompletaContadoresHoy = listaCompletaContadores.Where(x => x.Hora >= fini).ToList();
                    var listaMaquinas = listaCompletaContadoresHoy.Select(x => x.CodMaq).ToList();
                    listaMaquinas = listaMaquinas.Distinct().ToList();

                    foreach(var maquina in listaMaquinas) {

                        DateTime fechaIniMaquina = fini;
                        DateTime fechaFinMaquina = ffin;

                        var fechaIniContadorMaquina = fechaIniContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
                        if(fechaIniContadorMaquina != null) {
                            fechaIniMaquina = fechaIniContadorMaquina.Hora;
                        }
                        var fechaFinContadorMaquina = fechaFinContadoresMaquina.FirstOrDefault(x => x.CodMaq == maquina);
                        if(fechaFinContadorMaquina != null) {
                            fechaFinMaquina = fechaFinContadorMaquina.Hora;
                        }


                        List<ContadoresBonusInCompleto> listaCompletaContadoresMaquina = listaCompletaContadores.Where(x => x.CodMaq == maquina).ToList();

                        List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaHoy = listaCompletaContadoresMaquina.Where(x => x.Hora >= fechaIniMaquina && x.Hora < fechaFinMaquina).ToList();
                        List<ContadoresBonusInCompleto> listaCompletaContadoresMaquinaAyer = listaCompletaContadoresMaquina.Where(x => x.Hora < fechaIniMaquina).ToList();


                        if(listaCompletaContadoresMaquinaHoy.Any()) {

                            var antDetalleContadorBonusIn = listaCompletaContadoresMaquinaAyer.OrderByDescending(x => x.Hora).FirstOrDefault();
                            if(antDetalleContadorBonusIn != null) {
                                listaCompletaContadoresMaquinaHoy.Add(antDetalleContadorBonusIn);
                            }

                            ContadoresBonusInCompleto primeroLista = listaCompletaContadoresMaquinaHoy.OrderByDescending(x => x.Hora).First();
                            if(!listaContadores.Any(x => x.Cod_Cont_OL == primeroLista.Cod_Cont_OL)) {
                                primeroLista.fOperacion = fechaOperacion;
                                listaContadores.Add(primeroLista);
                            }
                            //listaContadores.Add(primeroLista);
                            ContadoresBonusInCompleto ultimoLista = listaCompletaContadoresMaquinaHoy.OrderBy(x => x.Hora).First();
                            if(!listaContadores.Any(x => x.Cod_Cont_OL == ultimoLista.Cod_Cont_OL)) {
                                ultimoLista.fOperacion = fechaOperacion;
                                listaContadores.Add(ultimoLista);
                            }
                            //listaContadores.Add(ultimoLista);
                        }

                    }
                }
                response = true;
            } catch(Exception) {
                response = false;
                return Json(new { respuesta = response, mensaje = mensaje.ToString() });
            }
            var result = Json(new { respuesta = response, data = listaContadores, mensaje = mensaje.ToString() });
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
    }
    public class ExcelBonusIn {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int CodSala { get; set; }
        public List<DetalleBonusIn> detalles { get; set; }
    }
    public class DetalleBonusIn {
        public string CodMaq { get; set; }
        public int FormulaBonusIn { get; set; }
        public double BonusInDiferencia { get; set; }
        public double BonusInDiferencia2 { get; set; }
        public double BonusInDiferencia3 { get; set; }
        public double BonusInDiferencia4 { get; set; }
    }
    class DetalleContadorBonusIn {
		public string CodMaq { get; set; }
        public string Fecha { get; set; }
        public double BonusInActual { get; set; }
        public double BonusInAnterior { get; set; }
        public double Diferencia { get; set; }
        public string Desconexion { get; set; }
        public int TipoBonusIn { get; set; }
    }
}