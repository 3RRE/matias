using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Cortesias;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Cortesias;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Cortesias {
    public class CortesiasController : Controller {
        private readonly AST_ClienteBL clienteBL;
        private readonly CRT_ProductoBL productoBL;
        private readonly CRT_MarcaBL marcaBL;
        private readonly CRT_TipoBL tipoBL;
        private readonly CRT_SubTipoBL subTipoBL;
        private readonly SalaBL salaBL;
        private readonly ServiceCortesias serviceCortesias;

        public CortesiasController() {
            clienteBL = new AST_ClienteBL();
            productoBL = new CRT_ProductoBL();
            marcaBL = new CRT_MarcaBL();
            tipoBL = new CRT_TipoBL();
            subTipoBL = new CRT_SubTipoBL();
            salaBL = new SalaBL();
            serviceCortesias = new ServiceCortesias();
        }

        #region Views

        public ActionResult ProductoSala() {
            return View("~/Views/Cortesias/ProductoSala/ProductoSala.cshtml");
        }


        public async Task<ActionResult> ProductoSalaById(int codSala, int id) {
            CRT_ProductoSala item = await serviceCortesias.GetItemById<CRT_ProductoSala>("GetProductoById", codSala, id);
            item = item ?? new CRT_ProductoSala();
            item.Id = id;
            return View("~/Views/Cortesias/ProductoSala/ProductoSalaById.cshtml", item);
        }

        public ActionResult Configuracion() {

            return View("~/Views/Cortesias/Configuracion/Configuracion.cshtml");
        }
        #endregion

        #region Clientes

        [HttpPost]
        public ContentResult GetClientes() {
            bool success = false;
            string message = "No se pudo obtener";
            List<AST_ClienteCortesia> list = new List<AST_ClienteCortesia>();

            try {

                list = clienteBL.GetClientesCortesia();
                success = true;
                message = "Obtenido correctamente";

            } catch(Exception exp) {
                message = exp.Message + ",Llame Administrador";
            }

            var resultData = new { success, message, data = list };

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        #endregion

        #region Producto x Sala

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> GetProductosSala(int codSala) {
            bool success = false;
            string message;

            List<CRT_ProductoSala> data = new List<CRT_ProductoSala>();
            try {

                data = productoBL.ObtenerProductoSala();

                List<CRT_ProductoSala> dataSala = await serviceCortesias.GetList<CRT_ProductoSala>("GetProductos", codSala);

                foreach(var item in data) {
                    item.isChecked = dataSala.Select(x => x.Id).ToList().Contains(item.Id);
                    if(item.isChecked) {
                        item.Cantidad = dataSala.Where(x => x.Id == item.Id).First().Cantidad;
                        item.Precio = dataSala.Where(x => x.Id == item.Id).First().Precio;
                    }
                }

                success = true;
                message = success ? "Lista de prodctos por sala." : "No hay productos en sala.";

            } catch(Exception ex) {
                message = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, message, data });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> SaveProductoSala(int codSala, CRT_ProductoSala productoSala) {
            bool success = false;
            string message = "";

            try {
                CRT_Producto producto = productoBL.ObtenerProductoPorId(productoSala.Id);

                productoSala.IdMarca = producto.IdMarca;
                productoSala.IdSubTipo = producto.IdSubTipo;
                productoSala.Nombre = producto.Nombre;
                productoSala.ImagenUrl = producto.ImagenUrl;

                if(productoSala.Existe()) {

                    CRT_SubTipo subtipo = subTipoBL.ObtenerSubTipoPorId(productoSala.IdSubTipo);
                    CRT_Marca marca = marcaBL.ObtenerMarcaPorId(productoSala.IdMarca);
                    CRT_Tipo tipo = tipoBL.ObtenerTipoPorId(subtipo.IdTipo);
                    bool inserted = false;

                    inserted = await serviceCortesias.CreateOrUpdate<CRT_Tipo>("SaveTipo", codSala, tipo);
                    inserted = await serviceCortesias.CreateOrUpdate<CRT_SubTipo>("SaveSubTipo", codSala, subtipo);
                    inserted = await serviceCortesias.CreateOrUpdate<CRT_Marca>("SaveMarca", codSala, marca);
                    inserted = await serviceCortesias.CreateOrUpdate<CRT_ProductoSala>("SaveProducto", codSala, productoSala);

                    if(inserted) {
                        string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                        Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                        string filePathProducto = Path.Combine(PathArchivos, "Cortesias", "Productos", producto.ImagenUrl);
                        bool insertedImageProducto = await serviceCortesias.SendImage("SaveImageProducto", codSala, filePathProducto, producto.ImagenUrl);
                        if(insertedImageProducto) {
                            success = true;
                        }

                        string filePathTipo = Path.Combine(PathArchivos, "Cortesias", "Tipos", tipo.ImagenUrl);
                        bool insertedImageTipo = await serviceCortesias.SendImage("SaveImageTipo", codSala, filePathTipo, tipo.ImagenUrl);
                        if(insertedImageTipo) {
                            success = true;
                        }

                        string filePathSubTipo = Path.Combine(PathArchivos, "Cortesias", "SubTipos", subtipo.ImagenUrl);
                        bool insertedImageSubTipo = await serviceCortesias.SendImage("SaveImageSubTipo", codSala, filePathSubTipo, subtipo.ImagenUrl);
                        if(insertedImageSubTipo) {
                            success = true;
                        }
                    }

                    success = inserted;
                    message = success ? "Producto asignado correctamente" : "No se pudo asignar el pedido.";


                }




            } catch(Exception ex) {
                message = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, message });
        }


        #endregion

        #region Configuracion

        [HttpPost]
        public async Task<ActionResult> GetKeysConfiguracion(int codSala) {
            string message = "Operación falló";
            bool success = false;
            List<CRT_Configuracion> list = new List<CRT_Configuracion>();
            try {
                list = await serviceCortesias.GetList<CRT_Configuracion>("GetKeysConfiguracion", codSala);
                success = true;
                message = "Operación exitosa";

            } catch(Exception exp) {
                success = false;
                message = exp.Message + ", Llame Administrador";
            }
            return Json(new { success, message, data = list });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateKeyValueConfiguracion(int codSala, string key, int value) {
            string message = "Operación falló";
            bool success = false;
            CRT_Configuracion item = new CRT_Configuracion();
            try {
                success = await serviceCortesias.UpdateKeyValueConfiguracion("UpdateKeyValueConfiguracion", codSala,key, value);
                if(success)
                    message = "Operación exitosa";

            } catch(Exception exp) {
                success = false;
                message = exp.Message + ", Llame Administrador";
            }
            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateKeyStateConfiguracion(int codSala, string key, bool state) {
            string message = "Operación falló";
            bool success = false;
            CRT_Configuracion item = new CRT_Configuracion();
            try {
                success = await serviceCortesias.UpdateKeyStateConfiguracion("UpdateKeyStateConfiguracion", codSala, key, state);
                if(success)
                    message = "Operación exitosa";

            } catch(Exception exp) {
                success = false;
                message = exp.Message + ", Llame Administrador";
            }
            return Json(new { success, message });
        }

        #endregion

        

        #region Maquinas
        [HttpPost]
        public async Task<JsonResult> GetMaquinasByCodsSala(List<int> codsSala) {
            bool success = false;
            string displayMessage;
            List<CRT_Maquina> data = new List<CRT_Maquina>();

            try {
                foreach(int codSala in codsSala) {
                    SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                    List<CRT_Maquina> maquinas = await serviceCortesias.GetList<CRT_Maquina>("GetMaquinasZPI", codSala);
                    maquinas.ForEach(x => {
                        x.Id = $"{sala.CodSala}-{x.CodMaquina}";
                        x.CodSala = sala.CodSala;
                        x.Sala = sala.Nombre;
                    });
                    data.AddRange(maquinas);
                }
                success = data.Count > 0;
                displayMessage = success ? "Lista de máquina." : "No hay máquinas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }
        #endregion
    }
}