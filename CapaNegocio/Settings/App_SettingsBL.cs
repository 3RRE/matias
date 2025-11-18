using CapaDatos.Settings;
using CapaEntidad.Settings;
using System.Collections.Generic;

namespace CapaNegocio.Settings
{
    public class App_SettingsBL
    {
        private readonly App_SettingsDAL _appSettingsDAL = new App_SettingsDAL();

        public List<App_SettingsEntidad> ListarAppSettings()
        {
            return _appSettingsDAL.ListarAppSettings();
        }

        public App_SettingsEntidad ObtenerAppSetting(string asKey)
        {
            return _appSettingsDAL.ObtenerAppSetting(asKey);
        }

        public string GuardarAppSetting(App_SettingsEntidad appSetting)
        {
            return _appSettingsDAL.GuardarAppSetting(appSetting);
        }

        public bool ActualizarAppSetting(App_SettingsEntidad appSetting)
        {
            return _appSettingsDAL.ActualizarAppSetting(appSetting);
        }

        public bool EliminarAppSetting(string asKey)
        {
            return _appSettingsDAL.EliminarAppSetting(asKey);
        }

        public bool ExisteAppSetting(string asKey)
        {
            return _appSettingsDAL.ExisteAppSetting(asKey);
        }
    }
}
