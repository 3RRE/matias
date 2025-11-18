using CapaDatos.Cortesias;
using CapaEntidad.Cortesias;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class CRT_MarcaBL {

        private readonly CRT_MarcaDAL marcaDAL;

        public CRT_MarcaBL() {
            marcaDAL = new CRT_MarcaDAL();
        }

        public List<CRT_Marca> ObtenerMarcas() {
            return marcaDAL.ObtenerMarcas();
        }

        public CRT_Marca ObtenerMarcaPorId(int id) {
            return marcaDAL.ObtenerMarcaPorId(id);
        }

        public bool InsertarMarca(CRT_Marca marca) {
            return marcaDAL.InsertarMarca(marca) != 0;
        }

        public bool ActualizarMarca(CRT_Marca marca) {
            return marcaDAL.ActualizarMarca(marca) != 0;
        }

        public bool EliminarMarca(int id) {
            return marcaDAL.EliminarMarca(id) != 0;
        }
    }
}
