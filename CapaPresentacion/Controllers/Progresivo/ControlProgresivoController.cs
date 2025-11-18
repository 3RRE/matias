using CapaEntidad;
using CapaEntidad.Administrativo;
using CapaNegocio;
using CapaNegocio.Administrativo;
using CapaNegocio.Progresivo;
using CapaPresentacion.Context;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using ImageResizer.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebSockets;
using static QRCoder.PayloadGenerator;

namespace CapaPresentacion.Controllers.Progresivo
{
    [seguridad(false)]
    public class ControlProgresivoController : Controller
    {
        private readonly ADM_MaquinaBL _maquinaBL = new ADM_MaquinaBL();
        private readonly WS_DetalleContadoresGameWebBL _wsDetalleContadoresWebBL = new WS_DetalleContadoresGameWebBL();
        private readonly ADM_SalaProgresivoBL _salaProgresivoBL = new ADM_SalaProgresivoBL();
        private readonly ADM_DetalleSalaProgresivoBL _detalleSalaProgresivoBL = new ADM_DetalleSalaProgresivoBL();
        private readonly ADM_PozoHistoricoBL _pozoHistoricoBL= new ADM_PozoHistoricoBL();
        private readonly ADM_MaquinaSalaProgresivoBL _maquinaSalaProgresivoBL = new ADM_MaquinaSalaProgresivoBL();
        private readonly ADM_GanadorBL _ganadorBL = new ADM_GanadorBL();
        private readonly ADM_HistorialMaquinaBL _historialMaquinaBL = new ADM_HistorialMaquinaBL();
        private readonly ADM_DetalleContadoresGameBL _detalleContadoresGame = new ADM_DetalleContadoresGameBL();
        private readonly SeguridadPJContext _dbContext = new SeguridadPJContext();
        private readonly SalaBL _salaBL=new SalaBL();
        private readonly ProgresivoBL _progresivoBL=new ProgresivoBL();
        const int HORA_OPERATIVA_SALA = 7;//las salas empiezan a operar a las 7 am
		private readonly AlertaProgresivoBL _alertaProgresivoBL = new AlertaProgresivoBL();
		// GET: ControlProgresivo
		[HttpPost]
        public ActionResult RecepcionarDataMaquina(List<ADM_MaquinaEntidad> maquinas, List<WS_DetalleContadoresGameWebEntidad> contadoresGame, int CodSala, DateTime fechaOperacion)
        {
            try
            {
                if (maquinas == null)
                {
                    maquinas = new List<ADM_MaquinaEntidad>();
                }
                if (contadoresGame == null)
                {
                    contadoresGame = new List<WS_DetalleContadoresGameWebEntidad>();
                }
                var fechaHoy = DateTime.Now;
                var fechaAyer = DateTime.Now.AddDays(-1);
                //registrar maquinas
                var listaMaquinas = _maquinaBL.GetListadoADM_MaquinaPorSala(CodSala);
                foreach(var item in maquinas)
                {
                    int CodMaquina = 0;
                    var maquina = listaMaquinas.Where(x => x.CodAlterno.Trim() == item.CodAlterno.Trim()).FirstOrDefault();
                    if (maquina == null)
                    {
                        item.FechaRegistro = DateTime.Now;
                        item.FechaModificacion = DateTime.Now;
                        CodMaquina = _maquinaBL.GuardarADM_Maquina(item);
                    }
                    else
                    {
                        maquina.FechaModificacion = DateTime.Now;
                        maquina.CodSala = item.CodSala;
                        maquina.Estado = item.Estado;
                        maquina.CodEmpresa = item.CodEmpresa;
                        _maquinaBL.EditarADM_Maquina(maquina);
                        CodMaquina = maquina.CodMaquina;
                    }
                }
                //registrar contadoresboton
                _wsDetalleContadoresWebBL.EliminarWS_DetalleContadoresGameWebPorFecha(CodSala,fechaAyer);
                var contadoresHoy = _wsDetalleContadoresWebBL.GetListadoWS_DetalleContadoresGamePorFechaOperacion(CodSala, fechaAyer);
                if (contadoresHoy.Count == 0)
                {
                    foreach(var contador in contadoresGame)
                    {
                        _wsDetalleContadoresWebBL.GuardarWS_DetalleContadoresGameWeb(contador);
                    }
                }
                RegistrarHistorialMaquina(fechaOperacion, CodSala);
                SincronizarContadoresPorFecha(fechaOperacion, CodSala);
            }
            catch (Exception)
            {

                return Json(new { respuesta = false });
            }
            return Json(new { respuesta = true });
        }
        [HttpPost]
        public ActionResult RecepcionarDataProgresivos(List<ADM_SalaProgresivoEntidad> progresivos, int CodSala, DateTime fechaOperacion)
        {
            try
            {
                if (progresivos == null)
                {
                    progresivos = new List<ADM_SalaProgresivoEntidad>();
                }
                var listaProgresivosSala = _salaProgresivoBL.GetListadoADM_SalaProgresivoPorSala(CodSala);
                foreach(var itemSalaProgresivo in progresivos)
                {
                    int CodSalaProgresivo = 0;
                    var salaProgresivo = listaProgresivosSala.Where(x => x.CodProgresivoWO == itemSalaProgresivo.CodProgresivoWO).FirstOrDefault();
                    if (salaProgresivo == null)
                    {
                        itemSalaProgresivo.FechaRegistro = DateTime.Now;
                        itemSalaProgresivo.FechaModificacion = DateTime.Now;
                        CodSalaProgresivo = _salaProgresivoBL.GuardarADM_SalaProgresivo(itemSalaProgresivo);
                    }
                    else
                    {
                        itemSalaProgresivo.FechaModificacion = DateTime.Now;
                        itemSalaProgresivo.CodSalaProgresivo = salaProgresivo.CodSalaProgresivo;
                        CodSalaProgresivo = salaProgresivo.CodSalaProgresivo;
                        _salaProgresivoBL.EditarADM_SalaProgresivo(itemSalaProgresivo);
                    }
                    //agregar o editar detalleSalaProgresivo
                    if (itemSalaProgresivo.DetalleSalaProgresivo == null)
                    {
                        itemSalaProgresivo.DetalleSalaProgresivo = new List<ADM_DetalleSalaProgresivoEntidad>();
                    }
                    var listaDetalleSalaProgresivo = _detalleSalaProgresivoBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(CodSalaProgresivo);

                    foreach (var itemDetalleSalaProgresivo in itemSalaProgresivo.DetalleSalaProgresivo)
                    {
                        int CodDetalleSalaProgresivo = 0;
                        var detalleSalaProgresivo = listaDetalleSalaProgresivo
                            .Where(x => 
                                x.CodProgresivoExterno == itemDetalleSalaProgresivo.CodProgresivoExterno 
                                && 
                                x.NroPozo==itemDetalleSalaProgresivo.NroPozo)
                            .FirstOrDefault();
                        if (detalleSalaProgresivo == null)
                        {
                            itemDetalleSalaProgresivo.FechaRegistro = DateTime.Now;
                            itemDetalleSalaProgresivo.FechaModificacion = DateTime.Now;
                            itemDetalleSalaProgresivo.fechaFin= DateTime.Now.Date;
                            itemDetalleSalaProgresivo.fechaIni = fechaOperacion.Date;
                            itemDetalleSalaProgresivo.CodSalaProgresivo = CodSalaProgresivo;
                            CodDetalleSalaProgresivo = _detalleSalaProgresivoBL.GuardarADM_DetalleSalaProgresivo(itemDetalleSalaProgresivo);
                        }
                        else
                        {
                            //if(fechaOperacion > itemDetalleSalaProgresivo.fechaFin) {
                            //    itemDetalleSalaProgresivo.fechaFin=fechaOperacion.Date;
                            //} else {
                            //    itemDetalleSalaProgresivo.fechaFin = detalleSalaProgresivo.fechaFin;
                            //}
                            //itemDetalleSalaProgresivo.FechaModificacion = DateTime.Now;
                            //itemDetalleSalaProgresivo.CodDetalleSalaProgresivo = detalleSalaProgresivo.CodDetalleSalaProgresivo;
                            //itemDetalleSalaProgresivo.CodSalaProgresivo = CodSalaProgresivo;
                            //CodDetalleSalaProgresivo = detalleSalaProgresivo.CodDetalleSalaProgresivo;
                            //detalleSalaProgresivo.fechaFin = DateTime.Now;

                            detalleSalaProgresivo.fechaFin = fechaOperacion.Date;
                            detalleSalaProgresivo.FechaModificacion = DateTime.Now;
                            detalleSalaProgresivo.MontoBase = itemDetalleSalaProgresivo.MontoBase;
                            detalleSalaProgresivo.MontoIni = itemDetalleSalaProgresivo.MontoIni;
                            detalleSalaProgresivo.MontoFin = itemDetalleSalaProgresivo.MontoFin;
                            detalleSalaProgresivo.NroPozo = itemDetalleSalaProgresivo.NroPozo;
                            detalleSalaProgresivo.Dificultad = itemDetalleSalaProgresivo.Dificultad;
                            detalleSalaProgresivo.Incremento = itemDetalleSalaProgresivo.Incremento;
                            detalleSalaProgresivo.IncrementoPozoOculto = itemDetalleSalaProgresivo.IncrementoPozoOculto;
                            CodDetalleSalaProgresivo = detalleSalaProgresivo.CodDetalleSalaProgresivo;
                            _detalleSalaProgresivoBL.EditarADM_DetalleSalaProgresivo(detalleSalaProgresivo);
                        }
                        //Agregar o editar PozoHistorico
                        if (itemDetalleSalaProgresivo.PozoHistorico == null)
                        {
                            itemDetalleSalaProgresivo.PozoHistorico = new List<ADM_PozoHistoricoEntidad>();
                        }
                        //la base de un dia antes de la fecha de operacion
                        var listaPozoHistorico = _pozoHistoricoBL.GetListadoADM_PozoHistoricoPorCodDetalleSalaProgresivoYFecha(CodDetalleSalaProgresivo,fechaOperacion.AddDays(-1));
                        if (listaPozoHistorico.Count == 0)
                        {
                            foreach (var itemPozoHistorico in itemDetalleSalaProgresivo.PozoHistorico)
                            {
                                itemPozoHistorico.CodDetalleSalaProgresivo = CodDetalleSalaProgresivo;
                                itemPozoHistorico.FechaRegistro = DateTime.Now;
                                itemPozoHistorico.FechaOperacion=fechaOperacion.Date.AddDays(-1);
                                _pozoHistoricoBL.GuardarADM_PozoHistorico(itemPozoHistorico);
                            }
                        }
                       
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { respuesta = true });
            }
            return Json(new { respuesta = true });
        }
        [HttpPost]
        public ActionResult RecepcionarDataGanadoresyMaquinaSala(List<ADM_MaquinaSalaProgresivoEntidad> maquinas, List<ADM_GanadorEntidad> ganadores, int CodSala, DateTime fechaOperacion)
        {
            try
            {
                var listaMaquinasSala = _maquinaBL.GetListadoADM_MaquinaPorSala(CodSala);
                var listaSalaProgresivo = _salaProgresivoBL.GetListadoADM_SalaProgresivoPorSala(CodSala);
                //Agregar MaquinaSalaProgresivo
                foreach (var item in maquinas)
                {
                    var maquinaInsertar = listaMaquinasSala.Find(x => x.CodAlterno.Trim() == item.CodAlterno.Trim());
                    var salaProgresivoInsertar = listaSalaProgresivo.Find(x => x.CodProgresivoWO == item.CodProgresivoWO);
                    if (maquinaInsertar != null && salaProgresivoInsertar != null)
                    {
                        //BuscarPersona maquinasalaprogresivo 
                        var maquinaSalaProgresivoConsulta = _maquinaSalaProgresivoBL
                            .GetADM_MaquinaSalaProgresivoPorCodSalaProgresivoyCodMaquina(salaProgresivoInsertar.CodSalaProgresivo, maquinaInsertar.CodMaquina);
                        if (maquinaSalaProgresivoConsulta.CodMaquina == 0)
                        {
                            //Nuevo registro
                            item.FechaRegistro = DateTime.Now;
                            item.FechaEnlace = fechaOperacion.AddDays(-1);
                            item.FechaModificacion = DateTime.Now;
                            item.CodMaquina = maquinaInsertar.CodMaquina;
                            item.Activo = true;
                            item.Estado = 1;
                            item.CodSalaProgresivo = salaProgresivoInsertar.CodSalaProgresivo;
                            _maquinaSalaProgresivoBL.GuardarADM_MaquinaSalaProgresivo(item);
                        }
                    }
                }

                //verificar que haya ganadores del dia
                var ganadoresInsertar = ganadores;
                    //.Where(x => x.FechaOperacion.Date == fechaOperacion.Date).ToList();
                if (ganadoresInsertar.Count > 0)
                {
                   
                    //insertar ganadores del dia
                    foreach(var item in ganadoresInsertar) {
                      
                        var maquina = listaMaquinasSala.Where(x => x.CodAlterno == item.CodAlterno && x.CodSala == CodSala).FirstOrDefault();
                        if(maquina == null) {
                            continue;
                        }
                        var existe = _dbContext.ADM_Ganador.Where(x => x.CodMaquina == maquina.CodMaquina && x.FechaPremio == item.FechaPremio && x.MontoProgresivo == item.MontoProgresivo).FirstOrDefault();
                        if(existe != null) {
                            continue;
                        }
                        var salaProgresivo = listaSalaProgresivo.Where(x => x.CodProgresivoWO == item.CodProgresivoWO).FirstOrDefault();
                        if(salaProgresivo == null) {
                            continue;
                        }
                        var listaDetalleSalaProgresivo = _detalleSalaProgresivoBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(salaProgresivo.CodSalaProgresivo);
                        var detalleSalaProgresivo = listaDetalleSalaProgresivo.Where(x => x.NroPozo == item.TipoPozo).OrderByDescending(x => x.CodDetalleSalaProgresivo).FirstOrDefault();
                        if(detalleSalaProgresivo == null) {
                            continue;
                        }
                        item.FechaRegistro = DateTime.Now;
                        item.CodMaquina = maquina.CodMaquina;
                        item.CodDetalleSalaProgresivo = detalleSalaProgresivo.CodDetalleSalaProgresivo;
                        item.FechaRegistro = DateTime.Now;
                        item.FechaModificacion = DateTime.Now;
                        item.FechaOperacion = item.FechaPremio.Date;
                        item.Token = maquina.Token;
                        _ganadorBL.GuardarADM_Ganador(item);
                    }
                    
                    ////eliminar ganadores
                    //var eliminados = _ganadorBL.EliminarPremiosSalaFecha(CodSala, fechaOperacion);
                    //if (eliminados)
                    //{
                    //    //insertar ganadores del dia
                    //    foreach (var item in ganadoresInsertar)
                    //    {
                    //        var maquina= listaMaquinasSala.Where(x => x.CodMaquinaLey == item.CodMaquinaLey).FirstOrDefault();
                    //        if (maquina == null)
                    //        {
                    //            continue;
                    //        }
                    //        var salaProgresivo = listaSalaProgresivo.Where(x => x.CodProgresivoWO == item.CodProgresivoWO).FirstOrDefault();
                    //        if (salaProgresivo == null)
                    //        {
                    //            continue;
                    //        }
                    //        var listaDetalleSalaProgresivo = _detalleSalaProgresivoBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(salaProgresivo.CodSalaProgresivo);
                    //        var detalleSalaProgresivo = listaDetalleSalaProgresivo.Where(x => x.NroPozo == item.TipoPozo).OrderByDescending(x=>x.CodDetalleSalaProgresivo).FirstOrDefault();
                    //        if (detalleSalaProgresivo == null)
                    //        {
                    //            continue;
                    //        }
                    //        item.FechaRegistro = DateTime.Now;
                    //        item.CodMaquina = maquina.CodMaquina;
                    //        item.CodDetalleSalaProgresivo = detalleSalaProgresivo.CodDetalleSalaProgresivo;
                    //        item.FechaRegistro = DateTime.Now;
                    //        item.FechaModificacion = DateTime.Now;
                    //        item.Token = maquina.Token;
                    //        _ganadorBL.GuardarADM_Ganador(item);
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
                return Json(new { respuesta = false });
            }
            return Json(new { respuesta=true });
        }
        private void RegistrarHistorialMaquina(DateTime fechaOperacion, int CodSala)
        {
            try
            {
                var listaMaquinas = _maquinaBL.GetListadoADM_MaquinaPorSala(CodSala);
                var listaHistorialMaquinas = _historialMaquinaBL.GetListadoADM_HistorialMaquinaPorSala(CodSala);
                foreach(var vMaquina in listaMaquinas)
                {
                    var vHisto = listaHistorialMaquinas.Where(x => x.CodMaquina == vMaquina.CodMaquina)
                        .OrderByDescending(y => y.FechaOperacionFin)
                        .FirstOrDefault();
                    if (vHisto != null)
                    {
                        //Comparar cada una de las cosas para ver si ha cambiado
                        bool vCambio = false;
                        string vResumenCambios = string.Empty;
                        vCambio = VerificarCambios(vHisto, vMaquina, out vResumenCambios);
                        if (vCambio)
                        {
                            //Si cambio se inserta un nuevo Registro
                            ADM_HistorialMaquinaEntidad histo = new ADM_HistorialMaquinaEntidad();
                            histo.ApuestaMaxima = vMaquina.ApuestaMaxima;
                            histo.ApuestaMinima = vMaquina.ApuestaMinima;
                            histo.CodAlmacen = vMaquina.CodAlmacen;
                            histo.CodAlterno = vMaquina.CodAlterno;
                            histo.CodClasificacion = vMaquina.CodClasificacion;
                            histo.CodComparador = vMaquina.CodComparador;
                            histo.CodContrato = vMaquina.CodContrato;
                            histo.CodEmpresa = vMaquina.CodEmpresa;
                            histo.CodEstadoMaquina = vMaquina.CodEstadoMaquina;
                            histo.CodFicha = vMaquina.CodFicha;
                            histo.CodFormula = vMaquina.CodFormula;
                            histo.CodIsla = vMaquina.CodIsla;
                            histo.CodJuego = vMaquina.CodJuego;
                            histo.CodLinea = vMaquina.CodLinea;
                            histo.CodMaquina = vMaquina.CodMaquina;
                            histo.CodMaquinaLey = vMaquina.CodMaquinaLey;
                            histo.CodMedioJuego = vMaquina.CodMedioJuego;
                            histo.CodModeloBilletero = vMaquina.CodModeloBilletero;
                            histo.CodModeloHopper = vMaquina.CodModeloHopper;
                            histo.CodModeloMaquina = vMaquina.CodModeloMaquina;
                            histo.CodMoneda = vMaquina.CodMoneda;
                            histo.CodMueble = vMaquina.CodMueble;
                            histo.CodPantalla = vMaquina.CodPantalla;
                            histo.CodSala = vMaquina.CodSala;
                            histo.CodTipoFicha = vMaquina.CodTipoFicha;
                            histo.CodTipoMaquina = vMaquina.CodTipoMaquina;
                            histo.CodVolatilidad = vMaquina.CodVolatilidad;
                            histo.CodZona = vMaquina.CodZona;
                            histo.CordX = vMaquina.CordX;
                            histo.CordY = vMaquina.CordY;
                            histo.CreditoFicha = vMaquina.CreditoFicha;
                            histo.FechaFabricacion = vMaquina.FechaFabricacion;
                            histo.FechaOperacionFin = fechaOperacion;
                            histo.FechaOperacionIni = fechaOperacion.AddDays(-1);
                            histo.FechaReconstruccion = vMaquina.FechaReconstruccion;
                            histo.Hopper = vMaquina.Hopper;
                            histo.NroFabricacion = vMaquina.NroFabricacion;
                            histo.NroSerie = vMaquina.NroSerie;
                            histo.PorcentajeDevolucion = vMaquina.PorcentajeDevolucion;
                            histo.Posicion = vMaquina.Posicion;
                            histo.Segmento = vMaquina.Segmento;
                            histo.Token = vMaquina.Token;
                            histo.ValorComercial = vMaquina.ValorComercial;
                            histo.Estado = 1;
                            histo.ResumenCambios = vResumenCambios;
                            histo.FechaRegistro = DateTime.Now;
                            histo.FechaModificacion = DateTime.Now;
                            histo.Activo = true;
                            _historialMaquinaBL.GuardarADM_HistorialMaquina(histo);
                        }
                        else
                        {
                            //Si NO cambio se actualiza el nuevo Registro
                            if (vHisto.FechaOperacionFin < fechaOperacion)
                            {
                                vHisto.FechaOperacionFin = fechaOperacion;
                            }
                            if (vHisto.FechaOperacionIni > fechaOperacion)
                            {
                                vHisto.FechaOperacionIni = fechaOperacion;
                            }
                            _historialMaquinaBL.EditarADM_HistorialMaquina(vHisto);
                        }
                    }
                    else
                    {
                        //No existe la maquina hay que insertarla                                
                        ADM_HistorialMaquinaEntidad histo = new ADM_HistorialMaquinaEntidad();
                        histo.ApuestaMaxima = vMaquina.ApuestaMaxima;
                        histo.ApuestaMinima = vMaquina.ApuestaMinima;
                        histo.CodAlmacen = vMaquina.CodAlmacen;
                        histo.CodAlterno = vMaquina.CodAlterno;
                        histo.CodClasificacion = vMaquina.CodClasificacion;
                        histo.CodComparador = vMaquina.CodComparador;
                        histo.CodContrato = vMaquina.CodContrato;
                        histo.CodEmpresa = vMaquina.CodEmpresa;
                        histo.CodEstadoMaquina = vMaquina.CodEstadoMaquina;
                        histo.CodFicha = vMaquina.CodFicha;
                        histo.CodFormula = vMaquina.CodFormula;
                        histo.CodIsla = vMaquina.CodIsla;
                        histo.CodJuego = vMaquina.CodJuego;
                        histo.CodLinea = vMaquina.CodLinea;
                        histo.CodMaquina = vMaquina.CodMaquina;
                        histo.CodMaquinaLey = vMaquina.CodMaquinaLey;
                        histo.CodMedioJuego = vMaquina.CodMedioJuego;
                        histo.CodModeloBilletero = vMaquina.CodModeloBilletero;
                        histo.CodModeloHopper = vMaquina.CodModeloHopper;
                        histo.CodModeloMaquina = vMaquina.CodModeloMaquina;
                        histo.CodMoneda = vMaquina.CodMoneda;
                        histo.CodMueble = vMaquina.CodMueble;
                        histo.CodPantalla = vMaquina.CodPantalla;
                        histo.CodSala = vMaquina.CodSala;
                        histo.CodTipoFicha = vMaquina.CodTipoFicha;
                        histo.CodTipoMaquina = vMaquina.CodTipoMaquina;
                        histo.CodVolatilidad = vMaquina.CodVolatilidad;
                        histo.CodZona = vMaquina.CodZona;
                        histo.CordX = vMaquina.CordX;
                        histo.CordY = vMaquina.CordY;
                        histo.CreditoFicha = vMaquina.CreditoFicha;
                        histo.FechaFabricacion = vMaquina.FechaFabricacion;
                        histo.FechaOperacionFin = fechaOperacion;
                        histo.FechaOperacionIni = fechaOperacion.AddDays(-1);
                        histo.FechaReconstruccion = vMaquina.FechaReconstruccion;
                        histo.Hopper = vMaquina.Hopper;
                        histo.NroFabricacion = vMaquina.NroFabricacion;
                        histo.NroSerie = vMaquina.NroSerie;
                        histo.PorcentajeDevolucion = vMaquina.PorcentajeDevolucion;
                        histo.Posicion = vMaquina.Posicion;
                        histo.Segmento = vMaquina.Segmento;
                        histo.Token = vMaquina.Token;
                        histo.ValorComercial = vMaquina.ValorComercial;
                        histo.Estado = 1;
                        histo.ResumenCambios = "";
                        histo.FechaRegistro = DateTime.Now;
                        histo.FechaModificacion = DateTime.Now;
                        histo.Activo = true;
                        _historialMaquinaBL.GuardarADM_HistorialMaquina(histo);
                    }
               
                }
            }
            catch (Exception ex)
            {

            }
        }
        private bool VerificarCambios(ADM_HistorialMaquinaEntidad vHisto, ADM_MaquinaEntidad vMaquina, out string vResumenCambios)
        {
            try
            {
                vResumenCambios = "";
                bool vari = false;
                //if (vHisto.ApuestaMaxima != vMaquina.ApuestaMaxima)
                //{
                //    vResumenCambios = "APUESTA MAXIMA";
                //    vari = true;
                //}
                //if (vHisto.ApuestaMinima != vMaquina.ApuestaMinima)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "APUESTA MINIMA";
                //    vari = true;
                //}
                if (vHisto.CodAlmacen != vMaquina.CodAlmacen)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "ALMACEN";
                    vari = true;
                }
                if (vHisto.CodAlterno != vMaquina.CodAlterno)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "COD ALTERNO";
                    vari = true;
                }
                //if (vHisto.CodClasificacion != vMaquina.CodClasificacion)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "CLASIFICACION";
                //    vari = true;
                //}
                //if (vHisto.CodComparador != vMaquina.CodComparador)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "COMPARADOR";
                //    vari = true;
                //}
                if (vHisto.CodContrato != vMaquina.CodContrato)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "CONTRATO";
                    vari = true;
                }
                if (vHisto.CodEmpresa != vMaquina.CodEmpresa)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "EMPRESA";
                    vari = true;
                }
                //if (vHisto.CodEstadoMaquina != vMaquina.CodEstadoMaquina)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "ESTADO DE MAQUINA";
                //    vari = true;
                //}
                if (vHisto.CodFicha != vMaquina.CodFicha)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "FICHA";
                    vari = true;
                }
                if (vHisto.CodFormula != vMaquina.CodFormula)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "FORMULA";
                    vari = true;
                }
                //if (vHisto.CodIsla != vMaquina.CodIsla)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "ISLA";
                //    vari = true;
                //}
                if (vHisto.CodJuego != vMaquina.CodJuego)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "JUEGO";
                    vari = true;
                }
                //if (vHisto.CodLinea != vMaquina.CodLinea)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "LINEA";
                //    vari = true;
                //}
                if (vHisto.CodMaquinaLey != vMaquina.CodMaquinaLey)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "COD MAQUINA LEY";
                    vari = true;
                }
                //if (vHisto.CodMedioJuego != vMaquina.CodMedioJuego)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "MEDIO DE JUEGO";
                //    vari = true;
                //}
                //if (vHisto.CodModeloBilletero != vMaquina.CodModeloBilletero)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "MODELO BILLETERO";
                //    vari = true;
                //}
                //if (vHisto.CodModeloHopper != vMaquina.CodModeloHopper)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "MODELO HOPPER";
                //    vari = true;
                //}
                if (vHisto.CodModeloMaquina != vMaquina.CodModeloMaquina)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "MODELO MAQUINA";
                    vari = true;
                }
                if (vHisto.CodMoneda != vMaquina.CodMoneda)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "MONEDA";
                    vari = true;
                }
                //if (vHisto.CodMueble != vMaquina.CodMueble)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "MUEBLE";
                //    vari = true;
                //}
                //if (vHisto.CodPantalla != vMaquina.CodPantalla)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "PANTALLA";
                //    vari = true;
                //}
                if (vHisto.CodSala != vMaquina.CodSala)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "SALA";
                    vari = true;
                }
                //if (vHisto.CodTipoFicha != vMaquina.CodTipoFicha)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "TIPO FICHA";
                //    vari = true;
                //}
                //if (vHisto.CodTipoMaquina != vMaquina.CodTipoMaquina)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "TIPO MAQUINA";
                //    vari = true;
                //}
                //if (vHisto.CodVolatilidad != vMaquina.CodVolatilidad)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "VOLATILIDAD";
                //    vari = true;
                //}
                //if (vHisto.CodZona != vMaquina.CodZona)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "ZONA";
                //    vari = true;
                //}
                //if (vHisto.CordX != vMaquina.CordX)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "CORD X";
                //    vari = true;
                //}
                //if (vHisto.CordY != vMaquina.CordY)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "CORD Y";
                //    vari = true;
                //}
                //if (vHisto.CreditoFicha != vMaquina.CreditoFicha)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "CREDITO FICHA";
                //    vari = true;
                //}
                //if (vHisto.FechaFabricacion != vMaquina.FechaFabricacion)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "FECHA FABRICACION";
                //    vari = true;
                //}
                //if (vHisto.FechaReconstruccion != vMaquina.FechaReconstruccion)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "FECHA RECONSTRUCCION";
                //    vari = true;
                //}
                //if (vHisto.Hopper != vMaquina.Hopper)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "HOPPER";
                //    vari = true;
                //}
                //if (vHisto.NroFabricacion != vMaquina.NroFabricacion)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "NRO FABRICACION";
                //    vari = true;
                //}
                if (vHisto.NroSerie != vMaquina.NroSerie)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "NRO SERIE";
                    vari = true;
                }
                //if (vHisto.PorcentajeDevolucion != vMaquina.PorcentajeDevolucion)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "PORC DEVOLUCION";
                //    vari = true;
                //}
                //if (vHisto.Posicion != vMaquina.Posicion)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "POSICION";
                //    vari = true;
                //}
                //if (vHisto.Segmento != vMaquina.Segmento)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "SEGMENTO";
                //    vari = true;
                //}
                if (vHisto.Token != vMaquina.Token)
                {
                    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                    vResumenCambios = vResumenCambios + "TOKEN";
                    vari = true;
                }
                //if (vHisto.ValorComercial != vMaquina.ValorComercial)
                //{
                //    vResumenCambios = vResumenCambios.Length == 0 ? "" : vResumenCambios + ", ";
                //    vResumenCambios = vResumenCambios + "VALOR COMERCIAL";
                //    vari = true;
                //}
                return vari;
            }
            catch (Exception exp)
            {
                vResumenCambios = string.Empty;
                return false;
            }
            return false;
        }
        private void SincronizarContadoresPorFechaAnt(DateTime fechaOperacion, int CodSala)
        {
            try
            {
                var diaAnterior = fechaOperacion.AddDays(-1);
      
                var detalleContadoresGameHoy = _detalleContadoresGame.GetListado_DetalleContadoresGamePorFechaOperacion(CodSala, fechaOperacion);
                if (detalleContadoresGameHoy.Count == 0)
                {
                    var listaMaquinas = _maquinaBL.GetListadoADM_MaquinaPorSala(CodSala);
                    var detalleContadoresGameWebFin = _wsDetalleContadoresWebBL.GetListadoWS_DetalleContadoresGamePorFechaOperacion(CodSala, diaAnterior);
                    var detalleContadoreGameIni = _detalleContadoresGame.GetListado_DetalleContadoresGamePorFechaOperacion(CodSala, diaAnterior);
                    foreach(var contFin in detalleContadoresGameWebFin)
                    {
                        var maq = listaMaquinas
                            .Where(x => x.CodSala == CodSala && x.CodAlterno == contFin.CodMaquina)
                            .FirstOrDefault();
                        var detContGame = new ADM_DetalleContadoresGameEntidad();
                        detContGame.Activo = true;
                        detContGame.Estado = 1;
                        detContGame.CancelCreditFin = contFin.CancelCredit;
                        detContGame.CodMaquina = maq.CodMaquina;
                        detContGame.CodMoneda = maq.CodMoneda;
                        detContGame.CodEmpresa = contFin.CodEmpresa;
                        detContGame.CodSala = contFin.CodSala;
                        detContGame.CoinInFin = contFin.CoinIn;
                        detContGame.CoinOutFin = contFin.CoinOut;
                        var fReg = DateTime.Now;
                        detContGame.FechaModificacion = fReg;
                        detContGame.FechaOperacion = contFin.FechaOperacion;
                        detContGame.FechaRegistro = fReg;
                        detContGame.GamesPlayedFin = contFin.GamesPlayed;
                        detContGame.HandPayFin = contFin.HandPay;
                        detContGame.JackpotFin = contFin.Jackpot;
                        detContGame.ProduccionPorSlot1 = 0;
                        detContGame.ProduccionPorSlot2Reset = 0;
                        detContGame.ProduccionPorSlot3Rollover = 0;
                        detContGame.ProduccionPorSlot4Prueba = 0;
                        detContGame.ProduccionTotalPorSlot5Dia = 0;
                        //verificar esta parte
                        var objCGameIni = detalleContadoreGameIni.Where(x => x.CodMaquina == maq.CodMaquina).FirstOrDefault();
                        if (objCGameIni == null)
                        {
                            detContGame.CoinInIni = contFin.CoinIn;
                            detContGame.CoinOutIni = contFin.CoinOut;
                            detContGame.GamesPlayedIni = contFin.GamesPlayed;
                            detContGame.HandPayIni = contFin.HandPay;
                            detContGame.JackpotIni = contFin.Jackpot;
                            detContGame.CancelCreditIni = contFin.CancelCredit;
                        }
                        else
                        {

                            detContGame.CoinInIni = objCGameIni.CoinInFin;
                            detContGame.CoinOutIni = objCGameIni.CoinOutFin;
                            detContGame.GamesPlayedIni = objCGameIni.GamesPlayedFin;
                            detContGame.HandPayIni = objCGameIni.HandPayFin;
                            detContGame.JackpotIni = objCGameIni.JackpotFin;
                            detContGame.CancelCreditIni = objCGameIni.CancelCreditFin;
                        }
                       


                        detContGame.SaldoCoinIn = detContGame.CoinInFin - detContGame.CoinInIni;
                        detContGame.SaldoCoinOut = detContGame.CoinOutFin - detContGame.CoinOutIni;
                        detContGame.SaldoJackpot = detContGame.JackpotFin - detContGame.JackpotIni;
                        detContGame.SaldoGamesPlayed = detContGame.GamesPlayedFin - detContGame.GamesPlayedIni;
                        _detalleContadoresGame.Guardar_DetalleContadoresGame(detContGame);
                    }
                }
                
            }
            catch (Exception)
            {

            }
        }
        [HttpPost]
        public ActionResult getProgresivoControl(DateTime fechaIni, DateTime fechaFin, List<string> misteriosos)
        {
            try
            {

                var fechaIniOperativa= fechaIni.AddHours(HORA_OPERATIVA_SALA);
                var fechaFinOperativa= fechaFin.AddDays(1).AddHours(HORA_OPERATIVA_SALA);

                DateTime diaAnterior = fechaIni.AddDays(-1).Date;
                List<dynamic> listaMisteriosos = new List<dynamic>();
                List<int> listaSalas = new List<int>();
                foreach (var vItem in misteriosos)
                {
                    // misteriosos.Add(Int32.Parse(vItem.ToString()));
                    var progresivo = ((string)vItem).Split('-');
                    var sala = ((string)vItem).Split('-');
                    listaMisteriosos.Add(new
                    {
                        CodSalaProgresivo = int.Parse(progresivo[1]),
                        CodSala = int.Parse(sala[0])
                    });
                    listaSalas.Add(int.Parse(sala[0]));
                }

                //Actualizar Informacion en ADM_DetalleContadoresGame
                CorregirDataContadoresGame(fechaIni, fechaFin, listaSalas);
                //Actualizar Informacion en ADM_Ganador
                CorregirDataGanador(fechaIniOperativa, fechaFinOperativa, listaSalas);
                List<dynamic> listaPrincipal = new List<dynamic>();
                var detalleContadoresGameQuery = _dbContext.ADM_DetalleContadoresGame.Where(x => x.FechaOperacion >= fechaIni && x.FechaOperacion <= fechaFin);
                var lHistorial = _dbContext.ADM_HistorialMaquina.Where(x => ((x.FechaOperacionIni <= fechaIni || x.FechaOperacionFin >= fechaIni)
                                                                          &&
                                                                           (x.FechaOperacionFin >= fechaFin || x.FechaOperacionFin >= fechaIni)));
                var lMaquinaSalaProgresivo = _dbContext.ADM_MaquinaSalaProgresivo;
                var salaProgresivoQuery = _dbContext.ADM_SalaProgresivo;
                var ganadorQuery = _dbContext.ADM_Ganador.Where(x => x.FechaPremio >= fechaIniOperativa && x.FechaPremio<= fechaFinOperativa);
				//Obtener lista de AlertaProgresivos
				//En la tabla se encuentra con tipo 3 el string con este formato "El progresivo INDIAN DREAMING no responde.", INDIAN DREAMING es el nombre del progresivo 
				//que es el mismo en la tablas ADM_SalaProgresivo, solo se puede filtrar por ese string, ya que hay otras descripciones con el mismo tipo
				var listaAlertaProgresivos = _alertaProgresivoBL
                                                .ListarAlertasProgresivoSalaYTipo(listaSalas.ToArray(), fechaIniOperativa, fechaFinOperativa, 3)
                                                .Where(x => x.Descripcion.EndsWith("no responde."));

                foreach (var control in listaMisteriosos)
                {
                    int codSala = control.CodSala;
                    int codSalaProgresivo = control.CodSalaProgresivo;
                    //nombre de misterioso
                    var misteriosoData = salaProgresivoQuery.Where(x => x.CodSalaProgresivo == codSalaProgresivo && x.CodSala == codSala)
                        .Select(
                            x => new {
                                Progresivo=x.Nombre,
                                Sala=x.NombreSala,
                                Empresa=x.RazonSocial,
                            }
                        ).FirstOrDefault();

                    var listaCoinin = (from dg in (from dg in detalleContadoresGameQuery
                                                   join hm in lHistorial on dg.CodMaquina equals hm.CodMaquina
                                                   join mp in lMaquinaSalaProgresivo on dg.CodMaquina equals mp.CodMaquina
                                                   where dg.CodSala == codSala && mp.ADM_SalaProgresivo.CodSala == codSala
                                                   && mp.CodSalaProgresivo == codSalaProgresivo

                                                   select new
                                                   {
                                                       coinin = (decimal?)(dg.SaldoCoinIn * hm.Token),
                                                       extra = "extra",
                                                       dg.FechaOperacion
                                                   }
                                                   )
                                       group dg by new { dg.FechaOperacion }
                                    into g
                                       select new
                                       {
                                           coinin = g.Sum(p => p.coinin),
                                           g.Key.FechaOperacion
                                       }
                                    ).ToList();
                    var prelistaDetProgresivo = _dbContext.ADM_DetalleSalaProgresivo.Where(x => x.ADM_SalaProgresivo.CodSala == codSala && x.CodSalaProgresivo == codSalaProgresivo
                          && x.fechaIni <= fechaFin && x.fechaFin >= fechaIni
                    ).ToList();

                    var listaDetProgresivo = (from li in prelistaDetProgresivo
                                              select new
                                              {
                                                  li.CodSalaProgresivo,
                                                  li.Incremento,
                                                  li.MontoBase,
                                                  li.MontoFin,
                                                  li.MontoIni,
                                                  li.MontoOcultoBase,
                                                  li.MontoOcultoFin,
                                                  li.MontoOcultoIni,
                                                  li.NroPozo,
                                                  li.CodDetalleSalaProgresivo,
                                                  fechaMaxima = li.fechaFin > fechaFin ? fechaFin : li.fechaFin,
                                                  fechaMinima = li.fechaIni < fechaIni ? fechaIni : li.fechaIni,

                                                  // fechaMaxima = ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Count() == 0 ? fechaFin : ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Max(y => y.FechaOperacion).Date,
                                                  //fechaMinima = ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Count() == 0 ? fechaIni : ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Min(y => y.FechaOperacion).Date
                                              }).ToList();
                    var pozoHistoricoQuery = _dbContext.ADM_PozoHistorico.Include("ADM_DetalleSalaProgresivo");
                    //var pozoHistoricoQuery = repositoryPozoHistorico.GetQuery(new[] { "DetalleSalaProgresivo" });
                    var baseAntQuery = pozoHistoricoQuery.Where(x =>
                        x.FechaOperacion == diaAnterior && x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo);
                    var baseAnt = baseAntQuery.ToList();
                    var baseAndRepNewQuery = pozoHistoricoQuery.Where(x =>
                           x.FechaOperacion >= diaAnterior && x.FechaOperacion <= fechaFin &&
                           x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo);
                    var baseAndRepNew = baseAndRepNewQuery.ToList();
                    var repSalasQuery = pozoHistoricoQuery.Where(x => x.FechaOperacion == fechaFin && x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo);
                    var repSalas = repSalasQuery.ToList();
                    bool boolHayBase = false, boolHayRepSalas = false;

                    if (baseAnt.Count() > 0)
                    {
                        boolHayBase = true;
                    }
                    else
                    {
                        try
                        {
                            var fechaMax = pozoHistoricoQuery.Where(x =>
                                          x.FechaOperacion <= diaAnterior &&
                                          x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo).ToList().Max(y => y.FechaOperacion);
                            baseAnt = pozoHistoricoQuery.Where(x =>
                                          x.FechaOperacion == fechaMax &&
                                          x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo).ToList();
                            boolHayBase = true;

                        }
                        catch (Exception exp)
                        {


                        }
                        if (baseAnt.Count() > 0)
                        {
                            boolHayBase = true;
                        }

                    }
                    if (repSalas.Count() > 0)
                    {
                        boolHayRepSalas = true;
                    }

                    var resultQuery = (from dep in listaDetProgresivo
                                       join rephoy in repSalas on dep.CodDetalleSalaProgresivo  equals (int)rephoy.CodDetalleSalaProgresivo 
                                      into joinedvt
                                       from rephoy in joinedvt.DefaultIfEmpty()
                                       select new
                                       {
                                           dep.CodDetalleSalaProgresivo,
                                           dep.NroPozo,
                                           Pozo = (dep.NroPozo == 1 ? "Mega" : (dep.NroPozo == 2 ? "Maxi" : "Mini")),
                                           Incremento = dep.Incremento * 100,

                                           subido = dep.Incremento * (listaCoinin.Where(x => x.FechaOperacion <= dep.fechaMaxima && x.FechaOperacion >= dep.fechaMinima)).Sum(w => w.coinin),
                                           //basePozoHist =10,
                                           basePozoHist =
                                               (boolHayBase == false
                                                   ? 0
                                                   : (baseAndRepNew.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).FirstOrDefault() == null ? 0 : (baseAndRepNew.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).
                                                   OrderBy(y => y.FechaOperacion).FirstOrDefault().MontoActualSala))), //Viene del display
                                           Jackpot =
                                                ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).FirstOrDefault()
                                                     != null
                                                    ? ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                        .Sum(y => (y.MontoProgresivo - dep.MontoBase))
                                                    //: dep.MontoBase,
                                                    : 0,
                                           //TotalDisplay =11,
                                           TotalDisplay =
                                               (dep.Incremento * (listaCoinin.Where(x => x.FechaOperacion <= dep.fechaMaxima && x.FechaOperacion >= dep.fechaMinima)).Sum(w => w.coinin)) +
                                               (boolHayBase == false
                                                   ? 0
                                                   : ((baseAndRepNew.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).FirstOrDefault() == null ? 0 : baseAndRepNew.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).OrderBy(y => y.FechaOperacion).FirstOrDefault()
                                                       .MontoActualSala))) - (
                                                           ganadorQuery.Where(
                                                               z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                               .FirstOrDefault() != null
                                                               ? (ganadorQuery.Where(
                                                                   z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                                   .Sum(y => (y.MontoProgresivo - dep.MontoBase)))

                                                                  : 0
                                                           ),
                                           //RepSala =66,
                                           RepSala =
                                               (boolHayRepSalas == false
                                                   ? 0
                                                   : (baseAndRepNew.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).FirstOrDefault() == null ? 0 : (baseAndRepNew.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).OrderByDescending(y => y.FechaOperacion).FirstOrDefault().MontoActualSala))),

                                       });
                    var result = resultQuery.ToList().OrderBy(x => x.NroPozo);
                    var resultSec = (from gan in ganadorQuery
                                     where
                                         gan.FechaPremio >= fechaIniOperativa && gan.FechaPremio <= fechaFinOperativa &&
                                         gan.ADM_DetalleSalaProgresivo.ADM_SalaProgresivo.CodSala == codSala &&
                                         gan.ADM_DetalleSalaProgresivo.ADM_SalaProgresivo.CodSalaProgresivo == codSalaProgresivo
                                     select new
                                     {
                                         gan.CodDetalleSalaProgresivo,
                                         NPremio = gan.CodGanador,
                                         Fecha = gan.FechaPremio,
                                         FechaOperacion = gan.FechaPremio == null
                                             ? DbFunctions.TruncateTime(gan.FechaPremio)
                                             : gan.FechaPremio,
                                         NroMistery = (int?)gan.ADM_DetalleSalaProgresivo.NroPozo,
                                         Premio = (decimal?)gan.MontoProgresivo,
                                         baseGanador = (decimal?)gan.ADM_DetalleSalaProgresivo.MontoBase,
                                         pagar = (decimal?)(gan.MontoProgresivo - gan.ADM_DetalleSalaProgresivo.MontoBase),
                                         estado =
                                             gan.SubioPremio == 1 ? "Cobrado" : "Pendiente",

                                     })
                                      .AsEnumerable() // Traer resultados a memoria
                                        .Select(x => new
                                        {
                                            x.CodDetalleSalaProgresivo,
                                            x.NPremio,
                                            Fecha = Convert.ToDateTime(x.Fecha).ToString("yyyy-MM-dd HH:mm:ss"), // Convertir a cadena aquí
                                            x.FechaOperacion,
                                            x.NroMistery,
                                            x.Premio,
                                            x.baseGanador,
                                            x.pagar,
                                            x.estado
                                        })
                                     .OrderByDescending(x => x.FechaOperacion).ToList();
                    var errorResult = listaAlertaProgresivos
                        .Where(x => 
                            x.ProgresivoNombre.ToLower().Trim().Equals(misteriosoData.Progresivo.ToLower().Trim()) &&
                            x.SalaId == codSala
                            ).FirstOrDefault();
                    if (misteriosoData != null)
                    {
                        listaPrincipal.Add(new
                        {
                            Misterioso = misteriosoData.Progresivo,
                            CoinIn = listaCoinin.Sum(w => w.coinin) == null ? 0 : listaCoinin.Sum(w => w.coinin),
                            misteriosoData.Sala,
                            misteriosoData.Empresa,
                            data = result,
                            dataSec = resultSec,
                            dataError = errorResult
                        });
                    }
                }

                return Json(listaPrincipal);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpPost]
        public ActionResult ListarProgresivosSala(List<int> salas)
        {
            try
            {
                var result = _dbContext.ADM_SalaProgresivo
                    .Where(p => salas.Contains((int)p.CodSala))
                    .Select(p=>new {
                        CodSala=p.CodSala,
                        CodSalaProgresivo=p.CodSalaProgresivo,
                        Nombre=p.Nombre,
                        NombreSala=p.NombreSala,
                        ClaseProgresivo = p.ClaseProgresivo
                    })
                    .ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            //try
            //{
            //    IDictionary<string, DateTime> parametersFecha = new Dictionary<string, DateTime>();
            //    string querySalaProgresivo = $" where CodSala in ({string.Join(",", salas)})";
            //    var listaProgresivos = _salaProgresivoBL.GetListadoADM_SalaProgresivoPorQuery(querySalaProgresivo, parametersFecha);
            //    return Json(listaProgresivos);
            //}
            //catch (Exception)
            //{
            //    return Json(false);
            //}
        }
        [HttpPost]
        public ActionResult ExportToExcelReporteControlProgresivo(string jsonData)
        {
            string mensaje = "Descargando Archivo";
            bool respuesta = true;
            string base64String = string.Empty;
            string excelName = string.Empty;
            try
            {
                JObject data = JObject.Parse(@jsonData);
                var memoryStream = new MemoryStream();
                var bytes = ExportExcelReporteControlProgresivo(data);
                memoryStream.Write(bytes, 0, bytes.Length);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
                excelName = "reporte-control-progresivo.xlsx";
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                data = base64String,
                excelName,
                respuesta,
                mensaje
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }
        private static byte[] ExportExcelReporteControlProgresivo(JObject data)
        {
            byte[] result = null;
            var itemsprimero = data["Items"];

            var items2 = data["Items"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {


                foreach (var misterioso in itemsprimero)
                {
                    ExcelWorksheet worksheet = CreateSheet(package,
                        misterioso["Misterioso"].ToString() + " - " + misterioso["Sala"].ToString());
                    int startFromRow = 9;

                    var derechatabla = misterioso["dataSec"];
                    var items = misterioso["data"];
                    var itemsConsolidado = misterioso["dataConsolidado"];

                    if (items != null && items.Any())
                    {
                        var table = new DataTable();
                        var firstElement = (JObject)items.First();
                        var properties = firstElement.Properties();
                        foreach (var prop in properties)
                        {
                            var a = prop.Value.Type;
                            string tipo = a.ToString();
                            switch (tipo)
                            {
                                case "String":
                                    table.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                                    break;
                                case "Date":
                                    table.Columns.Add(new DataColumn(prop.Name, typeof(DateTime)));
                                    ;
                                    break;
                                case "decimal":
                                    table.Columns.Add(new DataColumn(prop.Name, typeof(decimal)));
                                    break;
                                case "Integer":
                                    if (prop.Name == "CodDetalleSalaProgresivo")
                                    {
                                        table.Columns.Add(new DataColumn(prop.Name, typeof(int)));
                                    }
                                    else
                                    {
                                        table.Columns.Add(new DataColumn(prop.Name, typeof(decimal)));
                                    }
                                    break;
                                default:
                                    table.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                                    break;
                            }
                        }

                        table.Columns.Add(new DataColumn("Diferencia", typeof(decimal)));
                        var columns = table.Columns;
                        int numeracion = 1;
                        foreach (var item in items)
                        {
                            decimal TotalDisplay = 0, RepSala = 0, Diferencia = 0;
                            DataRow row = table.NewRow();
                            int i = 0;

                            /* decimal? a = null;
                             decimal b = a.GetValueOrDefault(0m);*/
                            foreach (DataColumn col in columns)
                            {

                                if (col.DataType == typeof(System.DateTime))
                                {
                                    row[i] = ((DateTime)item[col.ToString()]).ToString("d");
                                }
                                else if (col.DataType == typeof(System.String))
                                {
                                    row[i] = (string)item[col.ToString()];
                                }

                                else if (col.DataType == typeof(System.Decimal))
                                {
                                    if (col.ToString() == "Diferencia")
                                    {
                                        var TotalDisplay2 = row["TotalDisplay"];
                                        var RepSala2 = row["RepSala"];
                                        row[i] = Decimal.Parse(TotalDisplay2.ToString()) -
                                                 Decimal.Parse(RepSala2.ToString());
                                    }
                                    else
                                    {

                                        row[i] = (string)item[col.ToString()];
                                    }

                                }
                                else if (col.ToString() == "NroPozo")
                                {
                                    row[i] = numeracion;
                                }
                                else
                                {
                                    if (col.ToString() == "CodDetalleSalaProgresivo")
                                    {
                                        row[i] = Int32.Parse((string)item[col.ToString()]);
                                    }
                                    else
                                    {
                                        row[i] = Double.Parse((string)item[col.ToString()]);
                                    }

                                }
                                i++;
                            }
                            table.Rows.Add(row);
                            numeracion++;
                        }

                        worksheet.Cells["A" + startFromRow].LoadFromDataTable(table, true);

                        int colIndex = 1;
                        int rowsLengthFin = table.Rows.Count + startFromRow;
                        foreach (DataColumn col in table.Columns)
                        {
                            ExcelRange cells =
                                worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex];
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Bottom.Style = ExcelBorderStyle.Thin;

                            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            var column = col.ColumnName;
                            if (col.DataType == typeof(System.DateTime))
                            {
                                cells.Style.Numberformat.Format = "yyyy-mm-dd";
                            }
                            else if (col.DataType == typeof(System.String))
                            {
                                // cells.Style.Numberformat.Format = "#,##0.00";
                            }
                            else
                            {
                                if (col.ToString() == "NroPozo" || col.ToString() == "CodDetalleSalaProgresivo")
                                {
                                }
                                else
                                {
                                    cells.Style.Numberformat.Format = "#,##0.00";
                                }
                            }
                            if (column == "MontoVale")
                            {
                                var cellAddress = new ExcelAddress(startFromRow + 1, colIndex + 1, rowsLengthFin + 1,
                                    colIndex + 1); //menores a 0color rojo
                                var cf = worksheet.ConditionalFormatting.AddLessThan(cellAddress);
                                cf.Formula = "0";
                                cf.Style.Font.Color.Color = Color.Red;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Formula = "SUM(" +
                                                                                       cells[startFromRow + 1, colIndex]
                                                                                           .Address + ":" +
                                                                                       cells[rowsLengthFin, colIndex]
                                                                                           .Address + ")";
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Font.Bold = true;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Numberformat.Format = "#,##0.00";
                            }
                            package.Workbook.Calculate();
                            worksheet.Column(colIndex).AutoFit();
                            colIndex++;
                        }

                        using (var range = worksheet.Cells[startFromRow, 1, startFromRow, table.Columns.Count + 1])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            range.Style.ShrinkToFit = false;
                        }
                    } //if items
                    else
                    {

                        worksheet.Cells[startFromRow + 2, 2].Value = "No hay registros";


                    }

                    if (derechatabla != null && derechatabla.Any())
                    {
                        var tablederecha = new DataTable();
                        var firstElementDerecha = (JObject)derechatabla.First();
                        var propertiesDerecha = firstElementDerecha.Properties();

                        foreach (var prop in propertiesDerecha)
                        {
                            if (prop.Name != "FechaOperacion")
                            {

                                var a = prop.Value.Type;
                                string tipo = a.ToString();
                                switch (tipo)
                                {
                                    case "String":
                                        tablederecha.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                                        break;
                                    case "Date":
                                        tablederecha.Columns.Add(new DataColumn(prop.Name, typeof(DateTime)));
                                        ;
                                        break;
                                    case "decimal":
                                        tablederecha.Columns.Add(new DataColumn(prop.Name, typeof(decimal)));
                                        break;
                                    case "Integer":
                                        if (prop.Name == "CodDetalleSalaProgresivo")
                                        {
                                            tablederecha.Columns.Add(new DataColumn(prop.Name, typeof(int)));
                                        }
                                        else
                                        {
                                            tablederecha.Columns.Add(new DataColumn(prop.Name, typeof(decimal)));
                                        }
                                        break;
                                    default:
                                        tablederecha.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                                        break;
                                }
                            }
                        }

                        var columnsComprimido = tablederecha.Columns;
                        int numeracion = 1;
                        foreach (var item in derechatabla)
                        {
                            DataRow row = tablederecha.NewRow();
                            int i = 0;
                            foreach (DataColumn col in columnsComprimido)
                            {
                                if (col.DataType == typeof(System.DateTime))
                                {
                                    row[i] = ((DateTime)item[col.ToString()]).ToString("d");
                                }
                                else if (col.DataType == typeof(System.String))
                                {
                                    row[i] = (string)item[col.ToString()];
                                }
                                else if (col.ToString() == "NPremio")
                                {
                                    row[i] = numeracion;
                                }
                                else
                                {
                                    if (col.ToString() == "CodDetalleSalaProgresivo")
                                    {
                                        row[i] = Int32.Parse((string)item[col.ToString()]);
                                    }
                                    else
                                    {
                                        row[i] = Double.Parse((string)item[col.ToString()]);
                                    }

                                }

                                i++;
                            }
                            tablederecha.Rows.Add(row);
                            numeracion++;
                        }

                        int filacomienzocomprimido = tablederecha.Rows.Count + startFromRow + 3;
                        //  worksheet.Cells["D" + filacomienzocomprimido.ToString()].LoadFromDataTable(tablederecha, true);
                        worksheet.Cells["l" + startFromRow].LoadFromDataTable(tablederecha, true);


                        //  int filacomienzocomprimido = tablederecha.Rows.Count + startFromRow + 3;
                        int colIndex = 12; // int rowsLengthFin = tablederecha.Rows.Count + filacomienzocomprimido;

                        int rowsLengthFin = tablederecha.Rows.Count + startFromRow;

                        foreach (DataColumn col in tablederecha.Columns)
                        {
                            // ExcelRange cells = worksheet.Cells[filacomienzocomprimido, colIndex, rowsLengthFin, colIndex];
                            ExcelRange cells =
                                worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex];

                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //format cell for number
                            var column = col.ColumnName;
                            if (col.DataType == typeof(System.DateTime))
                            {
                                cells.Style.Numberformat.Format = "yyyy-mm-dd";
                            }
                            else if (col.DataType == typeof(System.String))
                            {
                                // cells.Style.Numberformat.Format = "#,##0.00";
                            }
                            else
                            {
                                if (col.ToString() == "NPremio" || col.ToString() == "NroMistery" ||
                                    col.ToString() == "baseGanador" || col.ToString() == "CodDetalleSalaProgresivo")
                                {
                                }
                                else
                                {
                                    cells.Style.Numberformat.Format = "#,##0.00";
                                }

                            }
                            package.Workbook.Calculate();
                            worksheet.Column(colIndex).AutoFit();
                            colIndex++;
                        }


                        using (
                            var range = worksheet.Cells[startFromRow, 5, startFromRow, tablederecha.Columns.Count + 11])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            range.Style.ShrinkToFit = false;
                        }
                    } //if derecha tabla
                    else
                    {
                        worksheet.Cells[startFromRow + 2, 10].Value = "No hay registros";


                    }

                    if(itemsConsolidado != null && itemsConsolidado.Any()) {
                        var tableConsolidado = new DataTable();
                        var firstElement = (JObject)itemsConsolidado.First();
                        var propertiesConsolidado = firstElement.Properties();

                        foreach(var prop in propertiesConsolidado) {
                            var a = prop.Value.Type;
                            string tipo = a.ToString();
                            switch(tipo) {
                                case "String":
                                    tableConsolidado.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                                    break;
                                case "Date":
                                    tableConsolidado.Columns.Add(new DataColumn(prop.Name, typeof(DateTime)));
                                    ;
                                    break;
                                case "decimal":
                                    tableConsolidado.Columns.Add(new DataColumn(prop.Name, typeof(decimal)));
                                    break;
                                case "Integer":
                                    if(prop.Name == "CodDetalleSalaProgresivo") {
                                        tableConsolidado.Columns.Add(new DataColumn(prop.Name, typeof(int)));
                                    } else {
                                        tableConsolidado.Columns.Add(new DataColumn(prop.Name, typeof(decimal)));
                                    }
                                    break;
                                default:
                                    tableConsolidado.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                                    break;
                            }
                        }

                        var columnsComprimido = tableConsolidado.Columns;
                        int numeracion = 1;
                        foreach(var item in itemsConsolidado) {
                            DataRow row = tableConsolidado.NewRow();
                            int i = 0;
                            foreach(DataColumn col in columnsComprimido) {
                                if(col.DataType == typeof(System.DateTime)) {
                                    row[i] = ((DateTime)item[col.ToString()]).ToString("d");
                                } else if(col.DataType == typeof(System.String)) {
                                    row[i] = (string)item[col.ToString()];
                                } else if(col.ToString() == "NPremio") {
                                    row[i] = numeracion;
                                } else {
                                    if(col.ToString() == "CodDetalleSalaProgresivo") {
                                        row[i] = Int32.Parse((string)item[col.ToString()]);
                                    } else {
                                        row[i] = Double.Parse((string)item[col.ToString()]);
                                    }

                                }

                                i++;
                            }
                            tableConsolidado.Rows.Add(row);
                            numeracion++;
                        }

                        int filacomienzocomprimido = tableConsolidado.Rows.Count + startFromRow + 3;
                        //  worksheet.Cells["D" + filacomienzocomprimido.ToString()].LoadFromDataTable(tablederecha, true);
                        worksheet.Cells["u" + startFromRow].LoadFromDataTable(tableConsolidado, true);


                        //  int filacomienzocomprimido = tablederecha.Rows.Count + startFromRow + 3;
                        int colIndex = 21; // int rowsLengthFin = tablederecha.Rows.Count + filacomienzocomprimido;

                        int rowsLengthFin = tableConsolidado.Rows.Count + startFromRow;

                        foreach(DataColumn col in tableConsolidado.Columns) {
                            // ExcelRange cells = worksheet.Cells[filacomienzocomprimido, colIndex, rowsLengthFin, colIndex];
                            ExcelRange cells =
                                worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex];

                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style
                                .Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //format cell for number
                            if(col.ToString() == "NroPozo" || col.ToString() == "totalGanadores") {
                            } else {
                                cells.Style.Numberformat.Format = "#,##0.00";
                            }
                            package.Workbook.Calculate();
                            worksheet.Column(colIndex).AutoFit();
                            colIndex++;
                        }


                        using(
                            var range = worksheet.Cells[startFromRow, 5, startFromRow, tableConsolidado.Columns.Count + 21]) {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            range.Style.ShrinkToFit = false;
                        }
                    } //if derecha tabla
                  else {
                        worksheet.Cells[startFromRow + 2, 10].Value = "No hay registros";


                    }



                    if (items != null && items.Any())
                    {
                        worksheet.Cells[9, 1].Value = "CodPozo";
                        worksheet.Cells[9, 2].Value = "No";
                        worksheet.Cells[9, 3].Value = "Pozo";
                        worksheet.Cells[9, 4].Value = "% Inc.";
                        worksheet.Cells[9, 5].Value = "Subido";
                        worksheet.Cells[9, 6].Value = "Base:";
                        worksheet.Cells[9, 7].Value = "Jackpots";
                        worksheet.Cells[9, 8].Value = "Tot. Diplay";
                        worksheet.Cells[9, 9].Value = "Rep. Salas";
                        worksheet.Cells[9, 10].Value = "Diferencia";
                    }
                    if (derechatabla != null && derechatabla.Any())
                    {
                        worksheet.Cells[9, 12].Value = "CodPozo";
                        worksheet.Cells[9, 13].Value = "No Premio";
                        worksheet.Cells[9, 14].Value = "Fecha";
                        worksheet.Cells[9, 15].Value = "No Mistery";
                        worksheet.Cells[9, 16].Value = "Premio";
                        worksheet.Cells[9, 17].Value = "Base";
                        worksheet.Cells[9, 18].Value = "Pagar";
                        worksheet.Cells[9, 19].Value = "Estado";

                    }

                    if(itemsConsolidado != null && itemsConsolidado.Any()) {
                        worksheet.Cells[9, 21].Value = "Nro. Pozo";
                        worksheet.Cells[9, 22].Value = "Cant. Ganadores";
                        worksheet.Cells[9, 23].Value = "Total Premio";
                        worksheet.Cells[9, 24].Value = "Total Base";
                        worksheet.Cells[9, 25].Value = "Total Pagar";
                    }



                    //add header and an additional column (left) and row (top)
                    worksheet.Cells[2, 1].Value = "Reporte:";
                    worksheet.Cells[3, 1].Value = "Empresa:";
                    worksheet.Cells[4, 1].Value = "Sala:";
                    worksheet.Cells[5, 1].Value = "Misterioso:";
                    worksheet.Cells[6, 1].Value = "Periodo:";
                    worksheet.Cells[7, 1].Value = "Fecha:";
                    var rangeCells = worksheet.Cells[2, 1, 7, 1];
                    rangeCells.Style.Font.Bold = true;
                    worksheet.Cells[2, 2].Value = "REPORTE CONTROL PROGRESIVO";
                    worksheet.Cells[3, 2].Value = misterioso["Empresa"].ToString();
                    worksheet.Cells[4, 2].Value = misterioso["Sala"].ToString();
                    worksheet.Cells[5, 2].Value = misterioso["Misterioso"].ToString();
                    worksheet.Cells[6, 2].Value = data["Periodo"].ToString();
                    worksheet.Cells[7, 2].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Column(1).AutoFit();
                    worksheet.InsertColumn(1, 1);
                    worksheet.Column(1).Width = 5;
                    //set background color white
                    worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                } //FOREACH MISTERIOSO


                result = package.GetAsByteArray();
            } //FIN  EXCEL PACKAGE
            //}//FIN items !=null
            // 
            return result;
        }
        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName)
        {
            //p.Workbook.Worksheets.Add(sheetName);
            //ExcelWorksheet ws = p.Workbook.Worksheets[1];
            
            ExcelWorksheet ws = p.Workbook.Worksheets.Add(sheetName);
            ws.Name = sheetName; //Setting Sheet's name
            ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
            //ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
            //ws.Cells.Style.Font.Name = "Courier New";
            ws.Cells.Style.Font.Name = "Arial";
            return ws;
        }
        private void SincronizarContadoresPorFecha(DateTime fechaOperacion, int CodSala)
        {
            try
            {
                fechaOperacion = fechaOperacion.AddDays(-1);
                var diaAnterior = fechaOperacion.AddDays(-1);

                var wsDetalleContadoresIni = _wsDetalleContadoresWebBL.GetListadoWS_DetalleContadoresGamePorFechaOperacion(CodSala, diaAnterior);
                var wsDetalleContadoresFin = _wsDetalleContadoresWebBL.GetListadoWS_DetalleContadoresGamePorFechaOperacion(CodSala, fechaOperacion);
                var listaMaquinas = _maquinaBL.GetListadoADM_MaquinaPorSala(CodSala);

                _detalleContadoresGame.Eliminar_DetalleContadoresGamePorFecha(CodSala,fechaOperacion);
                foreach (var contFin in wsDetalleContadoresFin)
                {
                    var maq = listaMaquinas.Where(x => x.CodSala == CodSala && x.CodAlterno == contFin.CodMaquina)
                         .FirstOrDefault();
                    if (maq == null)
                    {
                        continue;
                    }
                    var contadorIni = wsDetalleContadoresIni.Where(x => x.CodMaquina == contFin.CodMaquina).FirstOrDefault();
                    var detContGame = new ADM_DetalleContadoresGameEntidad();
                    detContGame.Activo = true;
                    detContGame.Estado = 1;
                    detContGame.CancelCreditFin = contFin.CancelCredit;
                    detContGame.CodMaquina = maq.CodMaquina;
                    detContGame.CodMoneda = maq.CodMoneda;
                    detContGame.CodEmpresa = contFin.CodEmpresa;
                    detContGame.CodSala = contFin.CodSala;
                    detContGame.CoinInFin = contFin.CoinIn;
                    detContGame.CoinOutFin = contFin.CoinOut;
                    var fReg = DateTime.Now;
                    detContGame.FechaModificacion = fReg;
                    detContGame.FechaOperacion = contFin.FechaOperacion;
                    detContGame.FechaRegistro = fReg;
                    detContGame.GamesPlayedFin = contFin.GamesPlayed;
                    detContGame.HandPayFin = contFin.HandPay;
                    detContGame.JackpotFin = contFin.Jackpot;
                    detContGame.ProduccionPorSlot1 = 0;
                    detContGame.ProduccionPorSlot2Reset = 0;
                    detContGame.ProduccionPorSlot3Rollover = 0;
                    detContGame.ProduccionPorSlot4Prueba = 0;
                    detContGame.ProduccionTotalPorSlot5Dia = 0;
                    if (contadorIni==null)
                    {
                        detContGame.CoinInIni = contFin.CoinIn;
                        detContGame.CoinOutIni = contFin.CoinOut;
                        detContGame.GamesPlayedIni = contFin.GamesPlayed;
                        detContGame.HandPayIni = contFin.HandPay;
                        detContGame.JackpotIni = contFin.Jackpot;
                        detContGame.CancelCreditIni = contFin.CancelCredit;
                    }
                    else
                    {
                        detContGame.CoinInIni = contadorIni.CoinIn;
                        detContGame.CoinOutIni = contadorIni.CoinOut;
                        detContGame.GamesPlayedIni = contadorIni.GamesPlayed;
                        detContGame.HandPayIni = contadorIni.HandPay;
                        detContGame.JackpotIni = contadorIni.Jackpot;
                        detContGame.CancelCreditIni = contadorIni.CancelCredit;
                    }
                    detContGame.SaldoCoinIn = detContGame.CoinInFin - detContGame.CoinInIni;
                    detContGame.SaldoCoinOut = detContGame.CoinOutFin - detContGame.CoinOutIni;
                    detContGame.SaldoJackpot = detContGame.JackpotFin - detContGame.JackpotIni;
                    detContGame.SaldoGamesPlayed = detContGame.GamesPlayedFin - detContGame.GamesPlayedIni;
                    _detalleContadoresGame.Guardar_DetalleContadoresGame(detContGame);


                }

                //var detalleContadoresGameHoy = _detalleContadoresGame.GetListado_DetalleContadoresGamePorFechaOperacion(CodSala, fechaOperacion);
                //if (detalleContadoresGameHoy.Count == 0)
                //{
                //    var listaMaquinas = _maquinaBL.GetListadoADM_MaquinaPorSala(CodSala);
                //    var detalleContadoresGameWebFin = _wsDetalleContadoresWebBL.GetListadoWS_DetalleContadoresGamePorFechaOperacion(CodSala, diaAnterior);
                //    var detalleContadoreGameIni = _detalleContadoresGame.GetListado_DetalleContadoresGamePorFechaOperacion(CodSala, diaAnterior);
                //    foreach (var contFin in detalleContadoresGameWebFin)
                //    {
                //        var maq = listaMaquinas
                //            .Where(x => x.CodSala == CodSala && x.CodMaquinaLey == contFin.CodMaquina)
                //            .FirstOrDefault();
                //        var detContGame = new ADM_DetalleContadoresGameEntidad();
                //        detContGame.Activo = true;
                //        detContGame.Estado = 1;
                //        detContGame.CancelCreditFin = contFin.CancelCredit;
                //        detContGame.CodMaquina = maq.CodMaquina;
                //        detContGame.CodMoneda = maq.CodMoneda;
                //        detContGame.CodEmpresa = contFin.CodEmpresa;
                //        detContGame.CodSala = contFin.CodSala;
                //        detContGame.CoinInFin = contFin.CoinIn;
                //        detContGame.CoinOutFin = contFin.CoinOut;
                //        var fReg = DateTime.Now;
                //        detContGame.FechaModificacion = fReg;
                //        detContGame.FechaOperacion = contFin.FechaOperacion;
                //        detContGame.FechaRegistro = fReg;
                //        detContGame.GamesPlayedFin = contFin.GamesPlayed;
                //        detContGame.HandPayFin = contFin.HandPay;
                //        detContGame.JackpotFin = contFin.Jackpot;
                //        detContGame.ProduccionPorSlot1 = 0;
                //        detContGame.ProduccionPorSlot2Reset = 0;
                //        detContGame.ProduccionPorSlot3Rollover = 0;
                //        detContGame.ProduccionPorSlot4Prueba = 0;
                //        detContGame.ProduccionTotalPorSlot5Dia = 0;
                //        //verificar esta parte
                //        var objCGameIni = detalleContadoreGameIni.Where(x => x.CodMaquina == maq.CodMaquina).FirstOrDefault();
                //        if (objCGameIni == null)
                //        {
                //            detContGame.CoinInIni = contFin.CoinIn;
                //            detContGame.CoinOutIni = contFin.CoinOut;
                //            detContGame.GamesPlayedIni = contFin.GamesPlayed;
                //            detContGame.HandPayIni = contFin.HandPay;
                //            detContGame.JackpotIni = contFin.Jackpot;
                //            detContGame.CancelCreditIni = contFin.CancelCredit;
                //        }
                //        else
                //        {

                //            detContGame.CoinInIni = objCGameIni.CoinInFin;
                //            detContGame.CoinOutIni = objCGameIni.CoinOutFin;
                //            detContGame.GamesPlayedIni = objCGameIni.GamesPlayedFin;
                //            detContGame.HandPayIni = objCGameIni.HandPayFin;
                //            detContGame.JackpotIni = objCGameIni.JackpotFin;
                //            detContGame.CancelCreditIni = objCGameIni.CancelCreditFin;
                //        }



                //        detContGame.SaldoCoinIn = detContGame.CoinInFin - detContGame.CoinInIni;
                //        detContGame.SaldoCoinOut = detContGame.CoinOutFin - detContGame.CoinOutIni;
                //        detContGame.SaldoJackpot = detContGame.JackpotFin - detContGame.JackpotIni;
                //        detContGame.SaldoGamesPlayed = detContGame.GamesPlayedFin - detContGame.GamesPlayedIni;
                //        _detalleContadoresGame.Guardar_DetalleContadoresGame(detContGame);
                //    }
                //}

            }
            catch (Exception)
            {

            }
        }
        [HttpPost]
        public ActionResult SincronizarControlProgresivos(DateTime fechaOperacion, int CodSala)
        {
            Object objRespuesta = new object();
            try
            {
                var sala = _salaBL.SalaListaIdJson(CodSala);
                var urlProgresivo = sala.UrlProgresivo;
                //var result=ObtenerInformacionMaquinasYContadores(urlProgresivo,fechaOperacion);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
        private void CorregirDataContadoresGame(DateTime fechaInicio, DateTime fechaFin, List<int> listaSalas) {
            List<ADM_Maquina> listaMaquinas = new List<ADM_Maquina>();

            try {
                var listaContadoresAdministrativo = ObtenerContadoresGamesDesdeAdministrativo(fechaInicio,fechaFin,listaSalas);
                var listaContadoresLocal= _dbContext.ADM_DetalleContadoresGame.Where(x => x.FechaOperacion >= fechaInicio && x.FechaOperacion <= fechaFin && listaSalas.Contains((int)x.CodSala)).Include(p=>p.ADM_Maquina).ToList();
                var listaAlternaContadoresAdministrativo = listaContadoresAdministrativo;
                foreach(var contador in listaContadoresLocal) {
                    var fechaOperacion=contador.FechaOperacion.Value.Date;
                    var codMaquina = contador.ADM_Maquina.CodAlterno;
                    var existeContador=listaContadoresAdministrativo.Where(x=>x.FechaOperacion.Date == fechaOperacion && x.CodAlterno==codMaquina && x.CodSala==contador.CodSala).FirstOrDefault();
                    if(existeContador != null) {
                        //verificar valores
                        if(contador.SaldoCoinIn != existeContador.SaldoCoinIn) {
                            //Editar valores
                            ADM_DetalleContadoresGameEntidad itemEditar = new ADM_DetalleContadoresGameEntidad() {

                                CodDetalleContadoresGame = contador.CodDetalleContadoresGame,
                                SaldoCoinIn = existeContador.SaldoCoinIn,
                                CoinInIni = existeContador.CoinInIni,
                                CoinInFin = existeContador.CoinInFin,
                            };
                            _detalleContadoresGame.EditarDetalleContadoresGamePorMaquina(itemEditar);
                        }
                        listaAlternaContadoresAdministrativo.Remove(existeContador);
                    } 
                }
                //contadores que quedaron en el aire, existen en administrativo, pero no en local, hay que insertarlos
                if(listaAlternaContadoresAdministrativo.Count>0) {
                    listaMaquinas = _dbContext.ADM_Maquina.Where(x => listaSalas.Contains((int)x.CodSala)).ToList();
                }
                foreach(var item in listaAlternaContadoresAdministrativo) {
                    var maq = listaMaquinas.Where(x => x.CodAlterno == item.CodAlterno).FirstOrDefault();
                    if (maq==null)
                    {
                        continue;
                    }
                    var detContGame = new ADM_DetalleContadoresGameEntidad();
                    detContGame.Activo = item.Activo;
                    detContGame.Estado = item.Estado;
                    detContGame.CancelCreditFin = item.CancelCreditFin;
                    detContGame.CodMaquina = maq.CodMaquina;
                    detContGame.CodMoneda = (int)maq.CodMoneda;
                    detContGame.CodEmpresa = item.CodEmpresa;
                    detContGame.CodSala = item.CodSala;
                    detContGame.CoinInFin = item.CoinInFin;
                    detContGame.CoinOutFin = item.CoinOutFin;
                    detContGame.FechaModificacion = DateTime.Now;
                    detContGame.FechaOperacion = item.FechaOperacion.Date;
                    detContGame.FechaRegistro = DateTime.Now;
                    detContGame.GamesPlayedFin = item.GamesPlayedFin;
                    detContGame.HandPayFin = item.HandPayFin;
                    detContGame.JackpotFin = item.JackpotFin;
                    detContGame.ProduccionPorSlot1 = 0;
                    detContGame.ProduccionPorSlot2Reset = 0;
                    detContGame.ProduccionPorSlot3Rollover = 0;
                    detContGame.ProduccionPorSlot4Prueba = 0;
                    detContGame.ProduccionTotalPorSlot5Dia = 0;

                    detContGame.CoinInIni = item.CoinInIni;
                    detContGame.CoinOutIni = item.CoinOutIni;
                    detContGame.GamesPlayedIni = item.GamesPlayedIni;
                    detContGame.HandPayIni = item.HandPayIni;
                    detContGame.JackpotIni = item.JackpotIni;
                    detContGame.CancelCreditIni = item.CancelCreditIni;

                    detContGame.SaldoCoinIn = item.SaldoCoinIn;
                    detContGame.SaldoCoinOut = item.SaldoCoinOut;
                    detContGame.SaldoJackpot = item.SaldoJackpot;
                    detContGame.SaldoGamesPlayed = item.SaldoGamesPlayed;
                    _detalleContadoresGame.Guardar_DetalleContadoresGame(detContGame);
                }
            } catch(Exception) {

                throw;
            }
        }
        private List<DetalleContadoresGameResponse> ObtenerContadoresGamesDesdeAdministrativo(DateTime fechaInicio, DateTime fechaFin,List<int> listaSalas) {



            List<DetalleContadoresGameResponse> result = new List<DetalleContadoresGameResponse>();
            object oEnvio = new object();
            try {
                string UrlReclutamiento = Convert.ToString(ConfigurationManager.AppSettings["UriSistemaReclutamientoAdministrativo"]);
                UrlReclutamiento = UrlReclutamiento + "Administrativo/ListarDetalleContadoresPorFechaOperacion";
                oEnvio = new
                {
                    fechaInicio = fechaInicio,
                    fechaFin = fechaFin,
                    listaSalas = listaSalas
                };
                string inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(oEnvio); ;
                var client = new MyWebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.UploadString(UrlReclutamiento, "POST", inputJson);
                result=JsonConvert.DeserializeObject<List<DetalleContadoresGameResponse>>(response);
                //dynamic jsonObj = JsonConvert.DeserializeObject(response);
            } catch(Exception ex) {
                result = new List<DetalleContadoresGameResponse>();
            }
            return result;
        }
        private void CorregirDataGanador(DateTime fechaInicio,DateTime fechaFin, List<int> listaSalas) {
            string query = string.Empty;
            if(listaSalas.Count > 0) {
                query = $" and CodSala in ({String.Join(",", listaSalas)})";

            }
            var listaGanadoresDiario = _progresivoBL.ListarCabecerasPorFechaYSala(fechaInicio, fechaFin, query);
            var listaGanadores = _dbContext.ADM_Ganador
                .Where(x => x.FechaPremio >= fechaInicio && x.FechaPremio <= fechaFin && listaSalas.Contains((int)x.ADM_DetalleSalaProgresivo.ADM_SalaProgresivo.CodSala))
                .ToList();

            foreach(var item in listaGanadoresDiario) {
                //guardar si no existe
                var existe = listaGanadores.Where(x => x.FechaOperacion == item.Fecha && (double)x.MontoProgresivo == item.Monto && x.ADM_Maquina.CodAlterno == item.SlotID).FirstOrDefault();
                if(existe == null) {
                    var maquina = _dbContext.ADM_Maquina.Where(x => x.CodAlterno == item.SlotID && x.CodSala == item.CodSala).FirstOrDefault();
                    if(maquina == null) {
                        continue;
                    }
                    var existeGanador = _dbContext.ADM_Ganador.Where(x => x.CodMaquina == maquina.CodMaquina && x.FechaPremio == item.Fecha && x.MontoProgresivo == (decimal)item.Monto).FirstOrDefault();
                    if(existeGanador != null) {
                        continue;
                    }
                    var salaProgresivo = _dbContext.ADM_SalaProgresivo.Where(x => x.CodProgresivoWO == item.CodProgresivo && x.CodSala==item.CodSala).FirstOrDefault();
                    if(salaProgresivo == null) {
                        continue;
                    }
                    var listaDetalleSalaProgresivo = _detalleSalaProgresivoBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(salaProgresivo.CodSalaProgresivo);
                    var detalleSalaProgresivo = listaDetalleSalaProgresivo.Where(x => x.NroPozo == item.TipoPozo).OrderByDescending(x => x.CodDetalleSalaProgresivo).FirstOrDefault();
                    if(detalleSalaProgresivo == null) {
                        continue;
                    }
                    var itemInsertar = new ADM_GanadorEntidad() { 
                        FechaModificacion=DateTime.Now,
                        FechaRegistro=DateTime.Now,
                        CodMaquina=maquina.CodMaquina,
                        CodDetalleSalaProgresivo=detalleSalaProgresivo.CodDetalleSalaProgresivo,
                        Token=(decimal)maquina.Token,
                        MontoProgresivo=(decimal)item.Monto,
                        TipoPozo=item.TipoPozo,
                        FechaPremio=item.Fecha,
                        FechaSubida=item.Fecha,
                        CodProgresivoWO=item.CodProgresivo,
                        FechaOperacion=item.Fecha.Date,
                        SubioPremio=1
                    };
                    _ganadorBL.GuardarADM_Ganador(itemInsertar);
                }
            }
        }
        [HttpPost]
        public ActionResult getConsolidadoProgresivo(DateTime fechaInicial,DateTime fechaFinal, List<string> misteriosos ) {

            DateTime gfechaIni = fechaInicial;
            DateTime gfechaFin = fechaFinal;
            List<dynamic> listaMisteriosos = new List<dynamic>();
            List<int> listaSalas=new List<int>();
            foreach(var vItem in misteriosos) {
                // misteriosos.Add(Int32.Parse(vItem.ToString()));
                var progresivo = ((string)vItem).Split('-');
                var sala = ((string)vItem).Split('-');
                listaMisteriosos.Add(new
                {
                    CodSalaProgresivo = int.Parse(progresivo[1]),
                    CodSala = int.Parse(sala[0])
                });
                listaSalas.Add(int.Parse(sala[0]));

            }
            //Actualizar Informacion en ADM_DetalleContadoresGame
            CorregirDataContadoresGame(gfechaIni, gfechaFin, listaSalas);
            //Actualizar Informacion en ADM_Ganador
            var gfechaIniOperativa = gfechaIni.AddHours(HORA_OPERATIVA_SALA);
            var gfechaFinOperativa = gfechaFin.AddDays(1).AddHours(HORA_OPERATIVA_SALA);
            CorregirDataGanador(gfechaIniOperativa, gfechaFinOperativa, listaSalas);

            #region Diferencia por cada fecha
            var TimeDif = gfechaFin - gfechaIni;
            var DifDays = TimeDif.Days;
            List<dynamic> listaPrincipal = new List<dynamic>();
            for(var i = 0; i <= DifDays; i++) {
                DateTime fechaIni=gfechaIni.AddDays(i);
                DateTime fechaFin = gfechaIni.AddDays(i);
                DateTime diaAnterior = fechaIni.AddDays(-1).Date;

                var fechaIniOperativa = fechaIni.AddHours(HORA_OPERATIVA_SALA);
                var fechaFinOperativa = fechaFin.AddDays(1).AddHours(HORA_OPERATIVA_SALA);

                var detalleContadoresGameQuery = _dbContext.ADM_DetalleContadoresGame.Where(z => z.FechaOperacion >= fechaIni && z.FechaOperacion <= fechaFin);
                var lHistorial=_dbContext.ADM_HistorialMaquina.Where(x => ((x.FechaOperacionIni <= fechaIni || x.FechaOperacionFin >= fechaIni)
                                                       && (x.FechaOperacionFin >= fechaFin || x.FechaOperacionFin >= fechaIni)));
                var lMaquinaSalaProgresivo = _dbContext.ADM_MaquinaSalaProgresivo;
                var salaProgresivoQuery = _dbContext.ADM_SalaProgresivo;
                var ganadorQuery = _dbContext.ADM_Ganador.Where(x => x.FechaPremio >= fechaIniOperativa && x.FechaPremio <= fechaFinOperativa);


				//Obtener lista de AlertaProgresivos
				//En la tabla se encuentra con tipo 3 el string con este formato "El progresivo INDIAN DREAMING no responde.", INDIAN DREAMING es el nombre del progresivo 
				//que es el mismo en la tablas ADM_SalaProgresivo, solo se puede filtrar por ese string, ya que hay otras descripciones con el mismo tipo
				var listaAlertaProgresivos = _alertaProgresivoBL
												.ListarAlertasProgresivoSalaYTipo(listaSalas.ToArray(), fechaIniOperativa, fechaFinOperativa, 3)
												.Where(x => x.Descripcion.EndsWith("no responde."));

				foreach(var control in listaMisteriosos) {
                    int codSala = control.CodSala;
                    int codSalaProgresivo = control.CodSalaProgresivo;

                    //nombre de misterioso
                    var misteriosoData = salaProgresivoQuery.Where(x => x.CodSalaProgresivo == codSalaProgresivo && x.CodSala == codSala)
                        .Select(
                            x => new {
                                Progresivo = x.Nombre,
                                Sala = x.NombreSala,
                                Empresa = x.RazonSocial,
                            }
                        ).FirstOrDefault();
                    var listaCoinin = (from dg in (from dg in detalleContadoresGameQuery
                                                   join hm in lHistorial on dg.CodMaquina equals hm.CodMaquina
                                                   join mp in lMaquinaSalaProgresivo on dg.CodMaquina equals mp.CodMaquina
                                                   where dg.CodSala == codSala && mp.ADM_SalaProgresivo.CodSala == codSala
                                                   && mp.CodSalaProgresivo == codSalaProgresivo

                                                   select new
                                                   {
                                                       coinin = (decimal?)(dg.SaldoCoinIn * hm.Token),
                                                       extra = "extra",
                                                       dg.FechaOperacion
                                                   }
                                                )
                                       group dg by new { dg.FechaOperacion }
                                 into g
                                       select new
                                       {
                                           coinin = g.Sum(p => p.coinin),
                                           g.Key.FechaOperacion
                                       }
                                 ).ToList();
                    var prelistaDetProgresivo = _dbContext.ADM_DetalleSalaProgresivo.Where(x => x.ADM_SalaProgresivo.CodSala == codSala && x.CodSalaProgresivo == codSalaProgresivo
                          && x.fechaIni <= fechaFin && x.fechaFin >= fechaIni
                    ).ToList();

                    var listaDetProgresivo = (from li in prelistaDetProgresivo
                                              select new
                                              {
                                                  li.CodSalaProgresivo,
                                                  li.Incremento,
                                                  li.MontoBase,
                                                  li.MontoFin,
                                                  li.MontoIni,
                                                  li.MontoOcultoBase,
                                                  li.MontoOcultoFin,
                                                  li.MontoOcultoIni,
                                                  li.NroPozo,
                                                  li.CodDetalleSalaProgresivo,
                                                  fechaMaxima = li.fechaFin > fechaFin ? fechaFin : li.fechaFin,
                                                  fechaMinima = li.fechaIni < fechaIni ? fechaIni : li.fechaIni,

                                                  // fechaMaxima = ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Count() == 0 ? fechaFin : ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Max(y => y.FechaOperacion).Date,
                                                  //fechaMinima = ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Count() == 0 ? fechaIni : ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == li.CodDetalleSalaProgresivo).Min(y => y.FechaOperacion).Date
                                              }).ToList();
                    var pozoHistoricoQuery = _dbContext.ADM_PozoHistorico.Include("ADM_DetalleSalaProgresivo");
                    //var pozoHistoricoQuery = repositoryPozoHistorico.GetQuery(new[] { "DetalleSalaProgresivo" });
                    var baseAntQuery = pozoHistoricoQuery.Where(x =>
                        x.FechaOperacion == diaAnterior && x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo);
                    var baseAnt = baseAntQuery.ToList();
                    var baseAndRepNewQuery = pozoHistoricoQuery.Where(x =>
                           x.FechaOperacion >= diaAnterior && x.FechaOperacion <= fechaFin &&
                           x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo);
                    var baseAndRepNew = baseAndRepNewQuery.ToList();
                    var repSalasQuery = pozoHistoricoQuery.Where(x => x.FechaOperacion == fechaFin && x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo);
                    var repSalas = repSalasQuery.ToList();
                    bool boolHayBase = false, boolHayRepSalas = false;

                    if(baseAnt.Count() > 0) {
                        boolHayBase = true;
                    } else {
                        try {
                            var fechaMax = pozoHistoricoQuery.Where(x =>
                                          x.FechaOperacion <= diaAnterior &&
                                          x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo).ToList().Max(y => y.FechaOperacion);
                            baseAnt = pozoHistoricoQuery.Where(x =>
                                          x.FechaOperacion == fechaMax &&
                                          x.ADM_DetalleSalaProgresivo.CodSalaProgresivo == codSalaProgresivo).ToList();
                            boolHayBase = true;

                        } catch(Exception exp) {


                        }
                        if(baseAnt.Count() > 0) {
                            boolHayBase = true;
                        }

                    }
                    if(repSalas.Count() > 0) {
                        boolHayRepSalas = true;
                    }

                    var resultQuery = (from dep in listaDetProgresivo
                                       join rephoy in repSalas on dep.CodDetalleSalaProgresivo equals (int)rephoy.CodDetalleSalaProgresivo
                                      into joinedvt
                                       from rephoy in joinedvt.DefaultIfEmpty()
                                       select new
                                       {
                                           dep.CodDetalleSalaProgresivo,
                                           dep.NroPozo,
                                           FechaOperacion = fechaIni,
                                           Pozo = (dep.NroPozo == 1 ? "Mega" : (dep.NroPozo == 2 ? "Maxi" : "Mini")),
                                           Incremento = dep.Incremento * 100,
                                           subido = dep.Incremento * (listaCoinin.Where(x => x.FechaOperacion <= dep.fechaMaxima && x.FechaOperacion >= dep.fechaMinima)).Sum(w => w.coinin),

                                           basePozoHist =
                                             (boolHayBase == false
                                                 ? 0
                                                 : (baseAndRepNew
                                                    .Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                    .OrderBy(y => y.FechaOperacion)
                                                    .FirstOrDefault()
                                                        == null ? 0 :
                                                        (
                                                            baseAndRepNew
                                                            .Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                            .OrderBy(y => y.FechaOperacion)
                                                            .FirstOrDefault().MontoActualSala
                                                        )
                                                    )
                                            ),
                                           Jackpot =
                                              ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo).FirstOrDefault()
                                                   != null
                                                  ? ganadorQuery.Where(z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                      .Sum(y => (y.MontoProgresivo - dep.MontoBase))

                                                  : 0,
                                           TotalDisplay =
                                             (dep.Incremento * (listaCoinin.Where(x => x.FechaOperacion <= dep.fechaMaxima && x.FechaOperacion >= dep.fechaMinima)).Sum(w => w.coinin)) +
                                             (boolHayBase == false
                                                 ? 0
                                                 : (baseAnt.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                     .Sum(y => y.MontoActualSala))) - (
                                                         ganadorQuery.Where(
                                                             z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                             .FirstOrDefault() != null
                                                             ? (ganadorQuery.Where(
                                                                 z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                                 .Sum(y => (y.MontoProgresivo - dep.MontoBase)))

                                                                : 0
                                                         ),
                                           RepSala =
                                             (boolHayRepSalas == false
                                                 ? 0
                                                 : (repSalas.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                     .Sum(y => y.MontoActualSala))),
                                           Diferencia = ((dep.Incremento * (listaCoinin.Where(x => x.FechaOperacion <= dep.fechaMaxima && x.FechaOperacion >= dep.fechaMinima)).Sum(w => w.coinin)) +
                                             (boolHayBase == false
                                                 ? 0
                                                 : (baseAnt.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                     .Sum(y => y.MontoActualSala))) - (
                                                         ganadorQuery.Where(
                                                             z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                             .FirstOrDefault() != null
                                                             ? (ganadorQuery.Where(
                                                                 z => z.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                                 .Sum(y => (y.MontoProgresivo - dep.MontoBase)))

                                                                : 0
                                                         )) - (boolHayRepSalas == false
                                                 ? 0
                                                 : (repSalas.Where(x => x.CodDetalleSalaProgresivo == dep.CodDetalleSalaProgresivo)
                                                     .Sum(y => y.MontoActualSala))),

                                       });
                    var result = resultQuery.ToList().OrderBy(x => x.NroPozo);
                    var dataError = listaAlertaProgresivos
                        .Where(x => 
                            x.ProgresivoNombre.ToLower().Trim().Equals(misteriosoData.Progresivo.ToLower().Trim()) && 
                            x.FechaRegistro >= fechaIniOperativa &&
                            x.SalaId == codSala)
                        .FirstOrDefault();
                    if(misteriosoData != null) {
                        listaPrincipal.Add(new
                        {
                            Misterioso = misteriosoData.Progresivo,
                            CoinIn = listaCoinin.Sum(w => w.coinin) == null ? 0 : listaCoinin.Sum(w => w.coinin),
                            misteriosoData.Sala,
                            misteriosoData.Empresa,
                            data = result,
                            fecha=fechaIni,
                            ProgresivoCompleto=$"{misteriosoData.Sala} - {misteriosoData.Progresivo}",
                            dataError = dataError
                        });
                    }
                }
            }
            #endregion
            return Json(listaPrincipal);
        }
        [HttpPost]
        public ActionResult ExportToExcelConsolidadoContolProgresivo(string jsonData) {
            string mensaje = "Descargando Archivo";
            bool respuesta = true;
            string base64String = string.Empty;
            string excelName = string.Empty;
            try {
                JObject data = JObject.Parse(@jsonData);
                var memoryStream = new MemoryStream();
                var bytes = ExportExcelConsolidadoControlProgresivo(data);
                memoryStream.Write(bytes, 0, bytes.Length);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
                excelName = "consolidado-control-progresivo.xlsx";
            } catch(Exception ex) {
                respuesta = false;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                data = base64String,
                excelName,
                respuesta,
                mensaje
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }
        public static byte[] ExportExcelConsolidadoControlProgresivo(JObject data) {
            byte[] result = null;
            var itemsprimero = data["Items"];

            var Fecha1 = (JArray)data["FechaIni"];
            string fecha1str = String.Join("-", Fecha1);
            var Fecha2 = (JArray)data["FechaFin"];
            string fecha2str = String.Join("-", Fecha2);
            var periodo = data["Periodo"];
            var datos = data["Items"];
            var misteriosos = datos.Select(x => (string)x["Misterioso"]).Distinct().ToList();

            string tituloreporte = "Consolidado Progresivos";

            DateTime FechaIni = Convert.ToDateTime(fecha1str);// (DateTime)data["FechaIni"];
            DateTime FechaFin = Convert.ToDateTime(fecha2str);// (DateTime)data["FechaFin"];
            var TimeDif = FechaFin - FechaIni;
            var DifDias = TimeDif.Days;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using(var package = new ExcelPackage()) {

                ExcelWorksheet worksheet = CreateSheet(package, "Consolidado Progresivo");
                ExcelRange celda;
                int comienzoy = 2;
                worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                celda = worksheet.Cells[comienzoy, 1, comienzoy, 7];
                celda.Value = tituloreporte;
                celda.Merge = true;
                celda.Style.Font.Bold = true;
                celda.Style.Font.Size = 16;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                comienzoy++;
                comienzoy++;

                ///cabecera
                worksheet.Cells[comienzoy, 1].Value = "Periodo";
                var rangeCells = worksheet.Cells[comienzoy, 1, comienzoy, 1];
                rangeCells.Style.Font.Bold = true;
                worksheet.Cells[comienzoy, 2].Value = (string)periodo;
                worksheet.Cells[comienzoy, 2, comienzoy, 6].Merge = true;
                worksheet.Cells[comienzoy, 2, comienzoy, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment Left
                worksheet.Cells[comienzoy, 2, comienzoy, 6].Style.Font.Size = 11;
                comienzoy++;
                /////fin cabecera
                int comienzox = 1;
                comienzoy++;

                ////TABLAAAAAAAAA
                ///CABECERAS TABLA
                int segundafilacabecera = comienzoy;
                celda = worksheet.Cells[comienzoy, comienzox];
                celda.Value = "ITEM";
                GenerarBordesCelda(celda);
                celda.Style.Font.Bold = true;
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.ShrinkToFit = false;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                comienzox++;
                celda = worksheet.Cells[comienzoy, comienzox];
                celda.Value = "SALA";
                GenerarBordesCelda(celda);
                celda.Style.Font.Bold = true;
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.ShrinkToFit = false;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                comienzox++;
                celda = worksheet.Cells[comienzoy, comienzox];
                celda.Value = "PROGRESIVO";
                GenerarBordesCelda(celda);
                celda.Style.Font.Bold = true;
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.ShrinkToFit = false;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                comienzox++;
                celda = worksheet.Cells[comienzoy, comienzox];
                celda.Value = "POZO";
                GenerarBordesCelda(celda);
                celda.Style.Font.Bold = true;
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.ShrinkToFit = false;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                comienzox++;

                ///columns fechas
                for(var i = 0; i <= DifDias; i++) {
                    var fechaActual = FechaIni.AddDays(i);
                    celda = worksheet.Cells[comienzoy, comienzox];
                    celda.Value = fechaActual.ToShortDateString();
                    GenerarBordesCelda(celda);
                    celda.Style.Font.Bold = true;
                    celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    celda.Style.ShrinkToFit = false;
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    comienzox++;
                    //table.Columns.Add(new DataColumn(fechaActual.ToShortDateString(), typeof(decimal)));
                }

                celda = worksheet.Cells[comienzoy, comienzox];
                celda.Value = "TOTAL";
                GenerarBordesCelda(celda);
                celda.Style.Font.Bold = true;
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.ShrinkToFit = false;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ///FIN CABECERAS TALBA

                ///////DATOS TABLA
                comienzoy++;
                int contador = 1;
                comienzox = 1;
                foreach(var misterioso_fila in misteriosos) {
                    var misterioso_datos = datos.Where(x => (string)x["Misterioso"] == misterioso_fila).ToList();
                    var sala = misterioso_datos[0]["Sala"];
                    celda = worksheet.Cells[comienzoy, comienzox, comienzoy + 2, comienzox];
                    celda.Value = contador;
                    celda.Merge = true;

                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    GenerarBordesCelda(celda);

                    worksheet.Column(comienzox).AutoFit();

                    comienzox++;
                    celda = worksheet.Cells[comienzoy, comienzox, comienzoy + 2, comienzox];
                    celda.Value = (string)sala;
                    celda.Merge = true;
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    GenerarBordesCelda(celda);
                    worksheet.Column(comienzox).AutoFit();


                    comienzox++;
                    celda = worksheet.Cells[comienzoy, comienzox, comienzoy + 2, comienzox];
                    celda.Value = misterioso_fila;
                    celda.Merge = true;
                    GenerarBordesCelda(celda);
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Column(comienzox).AutoFit();

                    comienzox++;
                    celda = worksheet.Cells[comienzoy, comienzox];
                    celda.Value = "Superior";
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    GenerarBordesCelda(celda);

                    celda = worksheet.Cells[comienzoy + 1, comienzox];
                    celda.Value = "Medio";
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    GenerarBordesCelda(celda);

                    celda = worksheet.Cells[comienzoy + 2, comienzox];
                    celda.Value = "Inferior";
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    GenerarBordesCelda(celda);

                    worksheet.Column(comienzox).AutoFit();
                    comienzox++;

                    int columnasfechax = comienzox;
                    ////columnas fecha
                    for(var i = 0; i <= DifDias; i++) {
                        var fechaActual = FechaIni.AddDays(i);
                        string fechaAA = fechaActual.ToShortDateString();
                        var datos_misterioso = misterioso_datos.Where(x => x["data"].Count() > 0).ToList(); ;

                        decimal sumasup = 0;
                        decimal mediosuma = 0;
                        decimal inferiorsuma = 0;
                        if(datos_misterioso.Count() > 0) {
                            var progresivos_fecha = misterioso_datos.Where(x => Convert.ToDateTime(x["data"][0]["FechaOperacion"]).Date == fechaActual).ToList();
                            var data_fecha = progresivos_fecha[0]["data"].ToList();

                            var superiorsuma = data_fecha.Where(x => (int)x["NroPozo"] == 1).Select(x => x["Diferencia"]).ToList();
                            sumasup = superiorsuma.Sum(x => (decimal)x);

                            mediosuma = data_fecha.Where(x => (int)x["NroPozo"] == 2).Select(x => (decimal)x["Diferencia"]).ToList().Sum(x => x);
                            inferiorsuma = data_fecha.Where(x => (int)x["NroPozo"] == 3).Select(x => (decimal)x["Diferencia"]).ToList().Sum(x => x);
                        }

                        celda = worksheet.Cells[comienzoy, comienzox];
                        celda.Value = sumasup;
                        GenerarBordesCelda(celda);
                        celda.Style.Numberformat.Format = "#,##0.00";
                        var cf = worksheet.ConditionalFormatting.AddLessThan(celda);
                        cf.Formula = "0";
                        cf.Style.Font.Color.Color = Color.Red;

                        celda = worksheet.Cells[comienzoy + 1, comienzox];
                        celda.Value = mediosuma;
                        celda.Style.Numberformat.Format = "#,##0.00";
                        GenerarBordesCelda(celda);
                        cf = worksheet.ConditionalFormatting.AddLessThan(celda);
                        cf.Formula = "0";
                        cf.Style.Font.Color.Color = Color.Red;

                        celda = worksheet.Cells[comienzoy + 2, comienzox];
                        celda.Value = inferiorsuma;
                        celda.Style.Numberformat.Format = "#,##0.00";
                        GenerarBordesCelda(celda);
                        cf = worksheet.ConditionalFormatting.AddLessThan(celda);
                        cf.Formula = "0";
                        cf.Style.Font.Color.Color = Color.Red;

                        worksheet.Column(comienzox).AutoFit();

                        comienzox++;
                    }
                    celda = worksheet.Cells[comienzoy, comienzox];
                    celda.Value = "TOTAL";
                    GenerarBordesCelda(celda);



                    celda.Formula = "SUM(" + worksheet.Cells[comienzoy, columnasfechax].Address + ":" + worksheet.Cells[comienzoy, comienzox - 1].Address + ")";
                    celda.Style.Font.Bold = true;
                    celda.Style.Numberformat.Format = "#,##0.00";
                    GenerarBordesCelda(celda);

                    celda = worksheet.Cells[comienzoy + 1, comienzox];

                    celda.Formula = "SUM(" + worksheet.Cells[comienzoy + 1, columnasfechax].Address + ":" + worksheet.Cells[comienzoy + 1, comienzox - 1].Address + ")";
                    celda.Style.Font.Bold = true;
                    celda.Style.Numberformat.Format = "#,##0.00";
                    GenerarBordesCelda(celda);

                    celda = worksheet.Cells[comienzoy + 2, comienzox];
                    celda.Formula = "SUM(" + worksheet.Cells[comienzoy + 2, columnasfechax].Address + ":" + worksheet.Cells[comienzoy + 2, comienzox - 1].Address + ")";
                    celda.Style.Font.Bold = true;
                    celda.Style.Numberformat.Format = "#,##0.00";
                    GenerarBordesCelda(celda);


                    comienzoy = comienzoy + 3;
                    comienzox = 1;
                    contador++;
                }

                worksheet.InsertColumn(1, 1);

                result = package.GetAsByteArray();
            } //FIN  EXCEL PACKAGE
            return result;
        }
        public static void GenerarBordesCelda(ExcelRange celda) {
            celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            celda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
    }
}