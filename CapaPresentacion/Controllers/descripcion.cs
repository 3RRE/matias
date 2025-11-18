using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |
              AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor, Inherited = true, AllowMultiple = false)]
    public class descripcion: Attribute
    {
        public descripcion(string nombre)
        {
        }

    }
}