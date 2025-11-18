using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class AumentoCreditoMaquinaBL
    {
        public AumentoCreditoMaquinaDAL aumentoMaquinaDAL = new AumentoCreditoMaquinaDAL();
        public bool InsertarAumentoCreditoMaquina(AumentoCreditoMaquinaEntidad item)
        {
            return aumentoMaquinaDAL.InsertarAumentoCreditoMaquina(item);
        }
        public AumentoCreditoMaquinaEntidad ObtenerUltimoRegistro(string CodMaq,int CodSala)
        {
            return aumentoMaquinaDAL.ObtenerUltimoRegistro(CodMaq,CodSala);
        }
        public bool ActualizarCantidadEnAumentoCreditoMaquina(AumentoCreditoMaquinaEntidad item)
        {
            return aumentoMaquinaDAL.ActualizarCantidadEnAumentoCreditoMaquina(item);
        }
    }
}
