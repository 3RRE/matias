using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public enum tipo_campaña:int
    {
        Promocion=0,
        Sorteo=1,
    }
    public class tipos_campaña
    {
        public static int Promocion = 0;
        public static int Sorteo = 1;
    }

    public enum tipo_campaña_condicion : int
    {
        CoinIn = 0,
    }

}