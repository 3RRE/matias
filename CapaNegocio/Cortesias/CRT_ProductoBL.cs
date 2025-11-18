using CapaDatos.Cortesias;
using CapaEntidad.Cortesias;
using CapaEntidad.Cortesias.Reporte;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class CRT_ProductoBL {

        private readonly CRT_ProductoDAL productoDAL;

        public CRT_ProductoBL() {
            productoDAL = new CRT_ProductoDAL();
        }

        public List<CRT_Producto> ObtenerProductos() {
            return productoDAL.ObtenerProductos();
        }

        public List<CRT_Producto> ObtenerProductosFiltrados(CRT_ReporteFiltro filtro) {
            string whereFilter = string.Empty;
            if(filtro.TieneTipos() || filtro.TieneSubTipos() || filtro.TieneProductos()) {
                whereFilter += "WHERE ";
            }
            if(filtro.TieneTipos()) {
                string ids = string.Join(",", filtro.IdsTipo);
                whereFilter += $"t.Id IN ({ids}) ";
            }
            if(filtro.TieneSubTipos()) {
                string ids = string.Join(",", filtro.IdsSubTipo);
                whereFilter += $"AND st.Id IN ({ids}) ";
            }
            if(filtro.TieneProductos()) {
                string ids = string.Join(",", filtro.IdsProducto);
                whereFilter += $"AND p.Id IN ({ids})";
            }
            return productoDAL.ObtenerProductosFiltrados(whereFilter);
        }

        public List<CRT_ProductoSala> ObtenerProductoSala() {
            return productoDAL.ObtenerProductosSala();
        }

        public List<CRT_Producto> ObtenerProductosPorIdsSubTipo(List<int> idsSubTipo) {
            string idsSubTipoStr = string.Join(",", idsSubTipo);
            return productoDAL.ObtenerProductosPorIdsSubTipo(idsSubTipoStr);
        }

        public CRT_Producto ObtenerProductoPorId(int id) {
            return productoDAL.ObtenerProductoPorId(id);
        }

        public bool InsertarProducto(CRT_Producto producto) {
            return productoDAL.InsertarProducto(producto) != 0;
        }

        public bool ActualizarProducto(CRT_Producto producto) {
            return productoDAL.ActualizarProducto(producto) != 0;
        }

        public bool EliminarProducto(int id) {
            return productoDAL.EliminarProducto(id) != 0;
        }
    }
}
