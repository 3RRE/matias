using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.IO;

namespace CapaPresentacion.Utilitarios
{
    public class ErrorCodeUtil
    {

        string[] wordsNumber = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        public List<string> setPageValues(HttpContextBase httpContext, string errorCode)
        {
            List<string> errorData = new List<string>();
            var errorCodes = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(httpContext.Server.MapPath("~/Content/Errors/errors.json")));
            errorData.Add(wordsNumber[int.Parse(errorCode[0].ToString())]);
            errorData.Add(wordsNumber[int.Parse(errorCode[1].ToString())]);
            errorData.Add(wordsNumber[int.Parse(errorCode[2].ToString())]);
            errorData.Add(errorCodes[errorCode]["title"].ToString());
            errorData.Add(errorCodes[errorCode]["description"].ToString());
            return errorData;
        }
    }
}