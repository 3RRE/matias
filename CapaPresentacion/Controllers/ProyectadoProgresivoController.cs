using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class ProyectadoProgresivoController : Controller
    {
        // GET: ProyectadoProgresivo
        ProyectadoProgresivoBL proyectadobl = new ProyectadoProgresivoBL();

        public ActionResult ProyectadoProgresivoListarVista()
        {
            return View();
        }

        public ActionResult ProyectoProgresivoObtenerIdJson(int Id)
        {
            var progresivo = new ProyectadoProgresivoEntidad();
            var errormensaje =  "";
            try
            {
                progresivo = proyectadobl.ProyectadoProgresivoObtenerIdJson(Id);
            }catch(Exception ex)
            {
                errormensaje = ex.Message + ", Llame Administrador";
            }
            return Json(new { data = progresivo, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ProyectadoProgresivoListarJson()
        {
            var errormensaje = "";
            var lista = new List<ProyectadoProgresivoEntidad>();
            try
            {
                lista = proyectadobl.ProyectadoProgresivoListadoJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ProyectadoProgresivoInsertarJson(ProyectadoProgresivoInsertEntidad entidad)
        {
            var errormensaje = "";
            var response = false;
            var data = new ProyectadoProgresivoEntidad();
            try
            {
                data.NroMaquina = entidad.NroMaquina;
                data.Descripcion = entidad.Descripcion;
                data.TotalJugMes = decimal.Parse(entidad.TotalJugMes);
                data.TipoCambio = decimal.Parse(entidad.TipoCambio);
                data.Retencion = decimal.Parse(entidad.Retencion)/100;
                data.PremioBasePozoInferior = decimal.Parse(entidad.PremioBasePozoInferior);
                data.PremioBasePozoMedio = decimal.Parse(entidad.PremioBasePozoMedio);
                data.PremioBasePozoSuperior = decimal.Parse(entidad.PremioBasePozoSuperior);
                data.PremioMinimoPozoInferior = decimal.Parse(entidad.PremioMinimoPozoInferior);
                data.PremioMinimoPozoMedio = decimal.Parse(entidad.PremioMinimoPozoMedio);
                data.PremioMinimoPozoSuperior = decimal.Parse(entidad.PremioMinimoPozoSuperior);
                data.PremioMaximoPozoInferior = decimal.Parse(entidad.PremioMaximoPozoInferior);
                data.PremioMaximoPozoMedio = decimal.Parse(entidad.PremioMaximoPozoMedio);
                data.PremioMaximoPozoSuperior = decimal.Parse(entidad.PremioMaximoPozoSuperior);
                data.IncrementoPozoInferior = decimal.Parse(entidad.IncrementoPozoInferior) / 100;
                data.IncrementoPozoMedio = decimal.Parse(entidad.IncrementoPozoMedio) / 100;
                data.IncrementoPozoSuperior = decimal.Parse(entidad.IncrementoPozoSuperior) / 100;
                data.FechaRegistro = DateTime.Now;
                response = proyectadobl.ProyectadoProgresivoInsertarJson(data);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ProyectadoProgresivoEditarJson(ProyectadoProgresivoInsertEntidad entidad)
        {
            var errormensaje = "";
            var response = false;
            var data = new ProyectadoProgresivoEntidad();
            try
            {
                data.IdProyectadoProgresivo = entidad.IdProyectadoProgresivo;
                data.NroMaquina = entidad.NroMaquina;
                data.Descripcion = entidad.Descripcion;
                data.TotalJugMes = decimal.Parse(entidad.TotalJugMes);
                data.TipoCambio = decimal.Parse(entidad.TipoCambio);
                data.Retencion = decimal.Parse(entidad.Retencion) / 100;
                data.PremioBasePozoInferior = decimal.Parse(entidad.PremioBasePozoInferior);
                data.PremioBasePozoMedio = decimal.Parse(entidad.PremioBasePozoMedio);
                data.PremioBasePozoSuperior = decimal.Parse(entidad.PremioBasePozoSuperior);
                data.PremioMinimoPozoInferior = decimal.Parse(entidad.PremioMinimoPozoInferior);
                data.PremioMinimoPozoMedio = decimal.Parse(entidad.PremioMinimoPozoMedio);
                data.PremioMinimoPozoSuperior = decimal.Parse(entidad.PremioMinimoPozoSuperior);
                data.PremioMaximoPozoInferior = decimal.Parse(entidad.PremioMaximoPozoInferior);
                data.PremioMaximoPozoMedio = decimal.Parse(entidad.PremioMaximoPozoMedio);
                data.PremioMaximoPozoSuperior = decimal.Parse(entidad.PremioMaximoPozoSuperior);
                data.IncrementoPozoInferior = decimal.Parse(entidad.IncrementoPozoInferior) / 100;
                data.IncrementoPozoMedio = decimal.Parse(entidad.IncrementoPozoMedio) / 100;
                data.IncrementoPozoSuperior = decimal.Parse(entidad.IncrementoPozoSuperior) / 100;
                response = proyectadobl.ProyectadoProgresivoEditarJson(data);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }

    }
}