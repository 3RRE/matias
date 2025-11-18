using CapaEntidad;
using CapaEntidad.Cortesias;
using CapaEntidad.Cortesias.Reporte;
using CapaNegocio;
using CapaNegocio.Cortesias;
using CapaPresentacion.Utilitarios;
using S3k.Utilitario.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CapaPresentacion.Controllers.Cortesias {
    public class ReportesCortesiaController : Controller {
        private readonly SalaBL salaBL;
        private readonly CRT_ProductoBL productoBL;
        private readonly ServiceCortesias serviceCortesias;

        public ReportesCortesiaController() {
            salaBL = new SalaBL();
            productoBL = new CRT_ProductoBL();
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Pedidos() {
            return View("~/Views/Cortesias/Reportes/Pedidos.cshtml");
        }

        public ActionResult Productos() {
            return View("~/Views/Cortesias/Reportes/Productos.cshtml");
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<JsonResult> ObtenerReportePorProducto(CRT_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;
            CRT_ReporteProductoChart chart = new CRT_ReporteProductoChart();
            List<CRT_ReporteProducto> table = new List<CRT_ReporteProducto>();

            List<CRT_Producto> productos = productoBL.ObtenerProductosFiltrados(filtro);
            chart.Productos = productos.Select(x => x.Nombre).ToList();
            try {
                foreach(int codSala in filtro.CodsSala) {
                    SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                    List<CRT_ReporteProducto> productosReporte = productos.Select(p => new CRT_ReporteProducto() {
                        CodSala = codSala,
                        Sala = sala.Nombre,
                        NombreMarca = p.NombreMarca,
                        NombreProducto = p.Nombre,
                        NombreSubTipo = p.NombreSubTipo,
                        NombreTipo = p.NombreTipo,
                        IdProducto = p.Id,
                        IdSubTipo = p.IdSubTipo,
                        IdMarca = p.IdMarca,
                        IdTipo = p.IdTipo,
                        Cantidad = 0
                    }).ToList();

                    List<CRT_ReporteProducto> productosSala = await serviceCortesias.GetList<CRT_ReporteProducto, CRT_ReporteFiltro>("ObtenerReportePorProducto", codSala, filtro);
                    if(productosSala.Count > 0) {
                        productosReporte.ForEach(x => {
                            CRT_ReporteProducto productoReporte = productosSala.FirstOrDefault(y => y.IdProducto == x.IdProducto) ?? new CRT_ReporteProducto();
                            if(productoReporte.Existe()) {
                                x.Cantidad = productoReporte.Cantidad;
                            }
                        });
                    }
                    CRT_ReporteProductoDataSet dataSet = new CRT_ReporteProductoDataSet() {
                        Sala = sala.Nombre,
                        Cantidades = productosReporte.Select(x => x.Cantidad).ToList(),
                    };

                    chart.DataSets.Add(dataSet);
                    table.AddRange(productosReporte);
                }
                success = productos.Count > 0;
                displayMessage = success ? "Lista de productos." : "No hay registros.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            object data = new {
                table,
                chart
            };

            JsonResult jsonResult = Json(new { success, displayMessage, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public async Task<JsonResult> ObtenerReportePedidos(CRT_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;
            List<CRT_ReportePedido> data = new List<CRT_ReportePedido>();

            int codSalaFiltro = 0;
            string codMaquinaFiltro = string.Empty;

            if(filtro.TieneSalaMaquina()) {
                List<string> filtrosMaquina = filtro.IdSalaMaquina.Split('-').ToList();
                int.TryParse(filtrosMaquina.ElementAt(0), out codSalaFiltro);
                codMaquinaFiltro = filtrosMaquina.ElementAt(1);
            }

            try {
                foreach(int codSala in filtro.CodsSala) {
                    filtro.CodMaquina = codSala == codSalaFiltro ? codMaquinaFiltro : string.Empty;
                    List<CRT_ReportePedido> pedidoSala = await serviceCortesias.GetList<CRT_ReportePedido, CRT_ReporteFiltro>("ObtenerReportePedidos", codSala, filtro);
                    if(pedidoSala.Count > 0) {
                        SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                        pedidoSala.ForEach(x => {
                            x.CodSala = sala.CodSala;
                            x.Sala = sala.Nombre;
                        });
                    }
                    data.AddRange(pedidoSala);
                }
                success = data.Count > 0;
                displayMessage = success ? "Lista de pedidos." : "No hay pedidos con los filtros ingresados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            JsonResult jsonResult = Json(new { success, displayMessage, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Excel
        [HttpPost]
        public async Task<JsonResult> GenerarExcelReportePorProducto(CRT_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;

            #region Obtener los datos
            List<CRT_ReporteProducto> data = new List<CRT_ReporteProducto>();
            List<CRT_Producto> productos = productoBL.ObtenerProductosFiltrados(filtro);

            foreach(int codSala in filtro.CodsSala) {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                List<CRT_ReporteProducto> productosReporte = productos.Select(p => new CRT_ReporteProducto() {
                    CodSala = codSala,
                    Sala = sala.Nombre,
                    NombreMarca = p.NombreMarca,
                    NombreProducto = p.Nombre,
                    NombreSubTipo = p.NombreSubTipo,
                    NombreTipo = p.NombreTipo,
                    IdProducto = p.Id,
                    IdSubTipo = p.IdSubTipo,
                    IdMarca = p.IdMarca,
                    IdTipo = p.IdTipo,
                    Cantidad = 0
                }).ToList();

                List<CRT_ReporteProducto> productosSala = await serviceCortesias.GetList<CRT_ReporteProducto, CRT_ReporteFiltro>("ObtenerReportePorProducto", codSala, filtro);
                if(productosSala.Count > 0) {
                    productosReporte.ForEach(x => {
                        CRT_ReporteProducto productoReporte = productosSala.FirstOrDefault(y => y.IdProducto == x.IdProducto) ?? new CRT_ReporteProducto();
                        if(productoReporte.Existe()) {
                            x.Cantidad = productoReporte.Cantidad;
                        }
                    });
                }

                data.AddRange(productosReporte);
            }

            success = data.Count > 0;
            displayMessage = success ? "Lista de productos." : "No se encontraron registros de productos con los filtros ingresados.";
            #endregion

            if(!success) {
                return Json(new { success, displayMessage });
            }

            #region Armar DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Cod. Sala", typeof(int));
            dataTable.Columns.Add("Sala", typeof(string));
            dataTable.Columns.Add("Tipo", typeof(string));
            dataTable.Columns.Add("Sub Tipo", typeof(string));
            dataTable.Columns.Add("Marca", typeof(string));
            dataTable.Columns.Add("Producto", typeof(string));
            dataTable.Columns.Add("Cantidad", typeof(int));

            foreach(CRT_ReporteProducto item in data) {
                dataTable.Rows.Add(
                    item.CodSala,
                    item.Sala,
                    item.NombreTipo,
                    item.NombreSubTipo,
                    item.NombreMarca,
                    item.NombreProducto,
                    item.Cantidad
                );
            }
            #endregion

            #region CrearExcel
            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Productos consumidos por Sala",
                    SheetName = "Productos consumidos por Sala",
                    Data = dataTable,
                    Title = $"Productos consumidos por Sala",
                    FirstColumNumber = true,
                };

                byte[] excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                JsonResult json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al administrador.";
            }
            #endregion

            JsonResult jsonResult = Json(new { success, data, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public async Task<JsonResult> GenerarExcelReportePedidos(CRT_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;

            #region Obtener los datos
            List<CRT_ReportePedido> data = new List<CRT_ReportePedido>();

            int codSalaFiltro = 0;
            string codMaquinaFiltro = string.Empty;

            if(filtro.TieneSalaMaquina()) {
                List<string> filtrosMaquina = filtro.IdSalaMaquina.Split('-').ToList();
                int.TryParse(filtrosMaquina.ElementAt(0), out codSalaFiltro);
                codMaquinaFiltro = filtrosMaquina.ElementAt(1);
            }

            foreach(int codSala in filtro.CodsSala) {
                filtro.CodMaquina = codSala == codSalaFiltro ? codMaquinaFiltro : string.Empty;
                List<CRT_ReportePedido> pedidoSala = await serviceCortesias.GetList<CRT_ReportePedido, CRT_ReporteFiltro>("ObtenerReportePedidos", codSala, filtro);
                if(pedidoSala.Count > 0) {
                    SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                    pedidoSala.ForEach(x => {
                        x.CodSala = sala.CodSala;
                        x.Sala = sala.Nombre;
                    });
                }
                data.AddRange(pedidoSala);
            }

            success = data.Count > 0;
            displayMessage = success ? "Lista de pedidos." : "No hay pedidos con los filtros ingresados.";
            #endregion

            if(!success) {
                return Json(new { success, displayMessage });
            }

            #region Armar DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Cod. Sala", typeof(int));
            dataTable.Columns.Add("Sala", typeof(string));
            dataTable.Columns.Add("Id Pedido", typeof(int));
            dataTable.Columns.Add("Productos", typeof(string));
            dataTable.Columns.Add("Cod. Máquina", typeof(string));
            dataTable.Columns.Add("Zona", typeof(string));
            dataTable.Columns.Add("Posición", typeof(int));
            dataTable.Columns.Add("Isla", typeof(string));
            dataTable.Columns.Add("Anfitriona", typeof(string));
            dataTable.Columns.Add("Nro. Doc. Cliente", typeof(string));
            dataTable.Columns.Add("Cliente", typeof(string));

            foreach(CRT_ReportePedido item in data) {
                dataTable.Rows.Add(
                    item.CodSala,
                    item.Sala,
                    item.IdPedido,
                    item.Productos,
                    item.CodMaquina,
                    item.NombreZona,
                    item.Posicion,
                    item.NombreIsla,
                    item.Anfitriona,
                    item.NumeroDocumentoCliente,
                    item.NombreCliente
                );
            }
            #endregion

            #region CrearExcel
            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Pedidos por Sala",
                    SheetName = "Pedidos por Sala",
                    Data = dataTable,
                    Title = $"Pedidos por Sala",
                    FirstColumNumber = true,
                };

                byte[] excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                JsonResult json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al administrador.";
            }
            #endregion

            JsonResult jsonResult = Json(new { success, data, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
    #endregion
}