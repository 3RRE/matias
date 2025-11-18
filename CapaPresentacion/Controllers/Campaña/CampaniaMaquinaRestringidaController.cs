using CapaEntidad.Campañas;
using CapaNegocio.Campaña;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Campaña
{
    [seguridad]
    public class CampaniaMaquinaRestringidaController : Controller
    {
        private CMP_MaquinaRestringidaBL maquinaRestingidaBL = new CMP_MaquinaRestringidaBL();
        // GET: CampaniaMaquinaRestringida
        public ActionResult ListadoMaquinasRestringidas()
        {
            return View("~/Views/Campania/ListadoMaquinasRestringidas.cshtml");
        }
        [HttpPost]
        public ActionResult ListadoMaquinasRestringidasSalaJSON(string UrlProgresivoSala, int CodSala)
        {
            string mensaje = "";
            bool respuesta = false;
            List<CMP_MaquinaRestringidaEntidad> listaMaquinasOnline = new List<CMP_MaquinaRestringidaEntidad>();
            List<CMP_MaquinaRestringidaEntidad> listaMaquinasIAS = new List<CMP_MaquinaRestringidaEntidad>();
            List<CMP_MaquinaRestringidaEntidad> listaMaquinasEnviar = new List<CMP_MaquinaRestringidaEntidad>();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" }); ;
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                //Obtener Maquinas desde Online
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ListadoMaquinasBDtec";
                response = client.UploadString(url, "POST", "");
                if (!response.Equals("[]"))
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject(response);
                    foreach (var item in jsonObj)
                    {
                        CMP_MaquinaRestringidaEntidad maquina = new CMP_MaquinaRestringidaEntidad()
                        {
                            CodMaquina = item.CodMaquina,
                            CodSala = item.CodSala,
                            Juego = item.Juego,
                            Marca = item.Marca,
                            Modelo = item.Modelo,
                            EstadoOnline = item.Estado,
                            UsuarioId = usuarioId
                        };
                        listaMaquinasOnline.Add(maquina);
                    }
                }
                //Obtener Maquinas desde IAS
                listaMaquinasIAS = maquinaRestingidaBL.GetListadoMaquinaRestringidaSala(CodSala);
                //Generar lista a enviar
                if (listaMaquinasOnline.Count > 0) {
                    foreach (var maquinaOnline in listaMaquinasOnline)
                    {
                        var encontrado = listaMaquinasIAS.Where(x => x.CodMaquina.Trim().Equals(maquinaOnline.CodMaquina.Trim())).FirstOrDefault();
                        if (encontrado != null)
                        {
                            maquinaOnline.Restringido = encontrado.Restringido;
                            listaMaquinasIAS.Remove(encontrado);
                        }
                        listaMaquinasEnviar.Add(maquinaOnline);
                    }
                }
                //Por si aun quedan registros en lista del IAS
                if (listaMaquinasIAS.Count>0)
                {
                    foreach(var maquinaIAS in listaMaquinasIAS)
                    {
                        listaMaquinasEnviar.Add(maquinaIAS);
                    }
                }
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listaMaquinasEnviar });
        }
        [HttpPost]
        public ActionResult EditarEstadoRestriccionMaquina(CMP_MaquinaRestringidaEntidad maquina)
        {
            bool respuesta = false;
            string mensaje = "";
            CMP_MaquinaRestringidaEntidad maquinaConsulta = new CMP_MaquinaRestringidaEntidad();
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                maquina.UsuarioId = usuarioId;
                //obtener registro de BD
                maquinaConsulta = maquinaRestingidaBL.GetMaquinaRestringidaSalaPorSalaYMaquina(maquina.CodSala, maquina.CodMaquina);
                if (maquinaConsulta.CodMaquina==null)
                {
                    //insertar maquina
                    respuesta = maquinaRestingidaBL.GuardaMaquina(maquina);
                }
                else
                { 
                    //existe
                    respuesta = maquinaRestingidaBL.EditarEstadoRestriccionMaquina(maquina);
                }
                if (respuesta)
                {
                    mensaje = "Registro Editado";
                }
                else
                {
                    mensaje = "No se pudo editar el registro";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
    }
}