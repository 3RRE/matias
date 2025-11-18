using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CapaEntidad.Settings;
using CapaNegocio.Settings;
 
namespace CapaPresentacion.Controllers.Settings
{
    [seguridad]
    public class SettingsController : Controller
    {
        private readonly App_SettingsBL _appSettingsBL = new App_SettingsBL();

        [HttpGet]
        public ActionResult AppSettings()
        {
            return View("~/Views/Settings/AppSettings.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarAppSettings()
        {
            bool success = false;
            string message = "No hay configuraciones";

            List<App_SettingsEntidad> data = new List<App_SettingsEntidad>();

            try
            {
                List<App_SettingsEntidad> appSettings = _appSettingsBL.ListarAppSettings();

                if(appSettings.Any())
                {
                    data = appSettings;

                    success = true;
                    message = "Configuraciones obtenidos";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message,
                data
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerAppSetting(string asKey)
        {
            bool success = false;
            string message = $"No hay una configuración {asKey}";

            if (string.IsNullOrEmpty(asKey))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una configuración"
                });
            }

            App_SettingsEntidad data = new App_SettingsEntidad();

            try
            {
                App_SettingsEntidad appSetting = _appSettingsBL.ObtenerAppSetting(asKey);

                if (appSetting.AS_Key.Equals(asKey))
                {
                    data = appSetting;

                    success = true;
                    message = $"Configuración {asKey} obtenido";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message,
                data
            });
        }

        [HttpPost]
        public ActionResult GuardarAppSetting(App_SettingsEntidad appSetting)
        {
            bool success = false;
            string message = $"No se pudo guardar los datos de la configuración {appSetting.AS_Key}";
            int userId = Convert.ToInt32(Session["UsuarioID"]);

            if (string.IsNullOrEmpty(appSetting.AS_Key))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, ingrese una configuración"
                });
            }

            bool existsAppSetting = _appSettingsBL.ExisteAppSetting(appSetting.AS_Key);

            if(existsAppSetting)
            {
                return Json(new
                {
                    success,
                    message = $"Ya se encuentra registrado la configuración {appSetting.AS_Key}"
                });
            }

            try
            {
                appSetting.AS_CUserId = userId;

                string insertedKey = _appSettingsBL.GuardarAppSetting(appSetting);

                if (insertedKey.Equals(appSetting.AS_Key))
                {
                    success = true;
                    message = $"Los datos de la configuración {appSetting.AS_Key} se han guardado";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message
            });
        }

        [HttpPost]
        public ActionResult ActualizarAppSetting(App_SettingsEntidad appSetting)
        {
            bool success = false;
            string message = $"No se pudo actualizar los datos de la configuración {appSetting.AS_Key}";
            int userId = Convert.ToInt32(Session["UsuarioID"]);

            if (string.IsNullOrEmpty(appSetting.AS_Key))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una configuración"
                });
            }

            try
            {
                appSetting.AS_UUserId = userId;

                bool updated = _appSettingsBL.ActualizarAppSetting(appSetting);

                if (updated)
                {
                    success = true;
                    message = $"Los datos de la configuración {appSetting.AS_Key} se han actualizado";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message
            });
        }

        [HttpPost]
        public ActionResult EliminarAppSetting(string asKey)
        {
            bool success = false;
            string message = $"No se pudo eliminar la configuración {asKey}";

            if (string.IsNullOrEmpty(asKey))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una configuración"
                });
            }

            try
            {
                bool deleted = _appSettingsBL.EliminarAppSetting(asKey);

                if (deleted)
                {
                    success = true;
                    message = $"La configuración {asKey} se ha eliminado";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message
            });
        }
    }
}