using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ModeloMaquinaBL
    {
        private ModeloMaquinaDAL modelomaquindal = new ModeloMaquinaDAL();

        public List<ModeloMaquinaEntidad> ListaModeloMaquinaJson(int id)
        {
            return modelomaquindal.ListaModeloMaquinaJson(id);
        }
    }
}
