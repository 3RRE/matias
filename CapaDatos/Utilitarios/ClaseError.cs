using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaDatos.Utilitarios
{
    public class ClaseError
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public ClaseError()
        {
            Key = string.Empty;
            Value = string.Empty;
        }
    }
}