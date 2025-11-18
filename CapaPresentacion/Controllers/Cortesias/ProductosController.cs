using CapaEntidad.Cortesias;
using CapaNegocio;
using CapaNegocio.Cortesias;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cortesias {
    public class ProductosController : Controller {
        private readonly CRT_ProductoBL productoBL;
        private readonly CRT_SubTipoBL subTipoBL;
        private readonly CRT_TipoBL tipoBL;
        private readonly CRT_MarcaBL marcaBL;
        private readonly SalaBL salaBL;
        private readonly ServiceCortesias serviceCortesias;

        public ProductosController() {
            productoBL = new CRT_ProductoBL();
            subTipoBL = new CRT_SubTipoBL();
            tipoBL = new CRT_TipoBL();
            marcaBL = new CRT_MarcaBL();
            salaBL = new SalaBL();
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/Producto/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            CRT_Producto producto = id == 0 ? new CRT_Producto() : productoBL.ObtenerProductoPorId(id);
            return View("~/Views/Cortesias/Producto/AddEdit.cshtml", producto);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetProductos() {
            bool success = false;
            string displayMessage;
            List<CRT_Producto> data = new List<CRT_Producto>();

            try {
                data = productoBL.ObtenerProductos();
                success = data.Count > 0;

                foreach(var item in data) {

                    string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                    string filePath = Path.Combine(PathArchivos, "Cortesias", "Productos", item.ImagenUrl);

                    if(System.IO.File.Exists(filePath)) {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(imageBytes);
                        item.ImagenBase64 = $"data:image/png;base64,{base64String}";
                    } else {
                        item.ImagenBase64 = string.Empty;
                    }
                }

                displayMessage = success ? "Lista de productos." : "No hay productos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetProductosByIdsSubTipo(List<int> idsSubTipo) {
            bool success = false;
            string displayMessage;
            List<CRT_Producto> data = new List<CRT_Producto>();

            try {
                data = productoBL.ObtenerProductosPorIdsSubTipo(idsSubTipo);
                success = data.Count > 0;
                displayMessage = success ? "Lista de productos." : "No hay productos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetProductoById(int id) {
            bool success = false;
            string displayMessage;
            CRT_Producto data = new CRT_Producto();

            try {
                data = productoBL.ObtenerProductoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Producto encontrado." : "No se encontró el producto.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> SaveProducto(CRT_Producto producto, HttpPostedFileBase imageFile) {
            bool success = false;
            string displayMessage = "";
            bool isEdit = producto.Existe();

            try {


                if(imageFile != null && imageFile.ContentLength > 0) {
                    string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                    Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                    string filePath = Path.Combine(PathArchivos, "Cortesias", "Productos", fileName);

                    using(var stream = new FileStream(filePath, FileMode.Create)) {
                        await imageFile.InputStream.CopyToAsync(stream);
                        producto.ImagenUrl = fileName;
                    }

                }

                if(producto.ImagenUrl == null) {
                    producto.ImagenUrl = "default.png";
                }

                CRT_SubTipo subTipo = subTipoBL.ObtenerSubTipoPorId(producto.IdSubTipo);
                if(!subTipo.Existe()) {
                    success = false;
                    displayMessage = $"No existe el sub tipo con código '{producto.IdSubTipo}' al que intenta vincular al producto.";
                    return Json(new { success, displayMessage });
                }

                CRT_Marca marca2 = marcaBL.ObtenerMarcaPorId(producto.IdMarca);
                if(!marca2.Existe()) {
                    success = false;
                    displayMessage = $"No existe la marca con código '{producto.IdMarca}' al que intenta vincular al producto.";
                    return Json(new { success, displayMessage });
                }

                producto.IdUsuario = Convert.ToInt32(Session["UsuarioID"]);
                success = isEdit ? productoBL.ActualizarProducto(producto) : productoBL.InsertarProducto(producto);

                if(isEdit) {

                    var salas = salaBL.ListadoTodosSalaActivosOrderJson();

                    foreach(var sala in salas) {

                        List<CRT_ProductoSala> dataSala = await serviceCortesias.GetList<CRT_ProductoSala>("GetProductos", sala.CodSala);

                        var productoEncontrado = dataSala.FirstOrDefault(x => x.Id == producto.Id);


                        if(productoEncontrado != null) {

                            CRT_SubTipo subtipo = subTipoBL.ObtenerSubTipoPorId(productoEncontrado.IdSubTipo);
                            CRT_Marca marca = marcaBL.ObtenerMarcaPorId(productoEncontrado.IdMarca);
                            CRT_Tipo tipo = tipoBL.ObtenerTipoPorId(subtipo.IdTipo);
                            bool inserted = false;

                            inserted = await serviceCortesias.CreateOrUpdate<CRT_Tipo>("SaveTipo", sala.CodSala, tipo);
                            inserted = await serviceCortesias.CreateOrUpdate<CRT_SubTipo>("SaveSubTipo", sala.CodSala, subtipo);
                            inserted = await serviceCortesias.CreateOrUpdate<CRT_Marca>("SaveMarca", sala.CodSala, marca);
                            inserted = await serviceCortesias.CreateOrUpdate<CRT_Producto>("SaveProducto", sala.CodSala, producto);

                            if(inserted && producto.ImagenUrl != "default.png") {
                                string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                                Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                                string filePathProducto = Path.Combine(PathArchivos, "Cortesias", "Productos", producto.ImagenUrl);
                                bool insertedImageProducto = await serviceCortesias.SendImage("SaveImageProducto", sala.CodSala, filePathProducto, producto.ImagenUrl);
                                if(insertedImageProducto) {
                                    success = true;
                                }

                                string filePathTipo = Path.Combine(PathArchivos, "Cortesias", "Tipos", tipo.ImagenUrl);
                                bool insertedImageTipo = await serviceCortesias.SendImage("SaveImageTipo", sala.CodSala, filePathTipo, tipo.ImagenUrl);
                                if(insertedImageTipo) {
                                    success = true;
                                }

                                string filePathSubTipo = Path.Combine(PathArchivos, "Cortesias", "SubTipos", subtipo.ImagenUrl);
                                bool insertedImageSubTipo = await serviceCortesias.SendImage("SaveImageSubTipo", sala.CodSala, filePathSubTipo, subtipo.ImagenUrl);
                                if(insertedImageSubTipo) {
                                    success = true;
                                }
                            }
                        }
                    }
                }

                displayMessage = success ? "Productos guardado correctamente." : "No se pudo guardar el producto.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteProducto(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = productoBL.EliminarProducto(id);
                displayMessage = success ? "Producto eliminado correctamente." : "No se pudo eliminar el producto.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
