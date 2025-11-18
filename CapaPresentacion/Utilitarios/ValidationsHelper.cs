using CapaEntidad.Settings;
using CapaNegocio.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace CapaPresentacion.Utilitarios
{
    public class ValidationsHelper
    {
        public static List<string> ValidEmails(List<string> emails)
        {
            List<string> listValidEmails = new List<string>();

            try
            {
                foreach (string email in emails)
                {
                    if (IsValidEmail(email))
                    {
                        listValidEmails.Add(email);
                    }
                }
            }
            catch(Exception)
            {
                listValidEmails = new List<string>();
            }

            return listValidEmails;
        }

        public static bool IsValidEmail(string email)
        {
            bool isValid = false;
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            try
            {
                if(Regex.IsMatch(email, pattern))
                {
                    isValid = true;
                }
            }
            catch(Exception)
            {
                isValid = false;
            }

            return isValid;
        }

        public static dynamic GetValueAppSettingsKey(string key, dynamic defaultValue)
        {
            dynamic value;

            try
            {
                dynamic valueKey = ConfigurationManager.AppSettings[key];

                value = valueKey ?? defaultValue;
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }

        public static dynamic GetValueAppSettingDB(string key, dynamic defaultValue)
        {
            dynamic value;

            try
            {
                App_SettingsEntidad appSetting = new App_SettingsBL().ObtenerAppSetting(key);

                value = appSetting.AS_Key != null && appSetting.AS_Key.Equals(key) ? appSetting.AS_Value : defaultValue;
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }
    }
}