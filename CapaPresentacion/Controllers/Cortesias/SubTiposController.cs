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
    public class SubTiposController : Controller {
        private readonly CRT_SubTipoBL subTipoBL;
        private readonly CRT_TipoBL tipoBL;
        private readonly SalaBL salaBL;
        private readonly ServiceCortesias serviceCortesias;

        public SubTiposController() {
            subTipoBL = new CRT_SubTipoBL();
            tipoBL = new CRT_TipoBL();
            salaBL = new SalaBL();
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/SubTipo/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            CRT_SubTipo subTipo = id == 0 ? new CRT_SubTipo() : subTipoBL.ObtenerSubTipoPorId(id);
            return View("~/Views/Cortesias/SubTipo/AddEdit.cshtml", subTipo);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetSubTipos() {
            bool success = false;
            string displayMessage;
            List<CRT_SubTipo> data = new List<CRT_SubTipo>();

            try {
                data = subTipoBL.ObtenerSubTipos();
                success = data.Count > 0;

                foreach(var item in data) {

                    string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                    string filePath = Path.Combine(PathArchivos, "Cortesias", "SubTipos", item.ImagenUrl);

                    if(System.IO.File.Exists(filePath)) {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(imageBytes);
                        item.ImagenBase64 = $"data:image/png;base64,{base64String}";
                    } else {
                        item.ImagenBase64 = string.Empty;
                    }
                }

                displayMessage = success ? "Lista de sub tipos." : "No hay sub tipos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetSubTiposByIdsTipo(List<int> idsTipo) {
            bool success = false;
            string displayMessage;
            List<CRT_SubTipo> data = new List<CRT_SubTipo>();

            try {
                data = subTipoBL.ObtenerSubTiposPorIdsTipo(idsTipo);
                success = data.Count > 0;
                displayMessage = success ? "Lista de sub tipos." : "No hay sub tipos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetSubTipoById(int id) {
            bool success = false;
            string displayMessage;
            CRT_SubTipo data = new CRT_SubTipo();

            try {
                data = subTipoBL.ObtenerSubTipoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Sub tipo encontrado." : "No se encontró el sub tipo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> SaveSubTipo(CRT_SubTipo subTipo, HttpPostedFileBase imageFile) {
            bool success = false;
            string displayMessage;
            bool isEdit = subTipo.Existe();
            try {



                if(imageFile != null && imageFile.ContentLength > 0) {
                    string fileName = subTipo.Nombre + Path.GetExtension(imageFile.FileName);
                    string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                    Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                    string filePath = Path.Combine(PathArchivos, "Cortesias", "SubTipos", fileName);

                    using(var stream = new FileStream(filePath, FileMode.Create)) {
                        await imageFile.InputStream.CopyToAsync(stream);
                        subTipo.ImagenUrl = fileName;
                    }

                }

                if(subTipo.ImagenUrl == null) {
                    subTipo.ImagenUrl = "default.png";
                }

                CRT_Tipo tipo2 = tipoBL.ObtenerTipoPorId(subTipo.IdTipo);
                if(!tipo2.Existe()) {
                    success = false;
                    displayMessage = $"No existe el tipo con código '{subTipo.IdTipo}' al que intenta vincular al sub tipo.";
                    return Json(new { success, displayMessage });
                }

                subTipo.IdUsuario = Convert.ToInt32(Session["UsuarioID"]);
                success = isEdit ? subTipoBL.ActualizarSubTipo(subTipo) : subTipoBL.InsertarSubTipo(subTipo);

                if(isEdit) {

                    var salas = salaBL.ListadoTodosSalaActivosOrderJson();

                    foreach(var sala in salas) {

                        List<CRT_ProductoSala> dataSala = await serviceCortesias.GetList<CRT_ProductoSala>("GetProductos", sala.CodSala);

                        var subTipoEncontrado = dataSala.FirstOrDefault(x => x.Id == subTipo.Id);

                        if(subTipoEncontrado != null) {

                            CRT_Tipo tipo = tipoBL.ObtenerTipoPorId(subTipo.IdTipo);
                            bool inserted = false;

                            inserted = await serviceCortesias.CreateOrUpdate<CRT_Tipo>("SaveTipo", sala.CodSala, tipo);
                            inserted = await serviceCortesias.CreateOrUpdate<CRT_SubTipo>("SaveSubTipo", sala.CodSala, subTipo);

                            if(inserted && subTipo.ImagenUrl != "default.png") {
                                string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                                Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                                string filePathTipo = Path.Combine(PathArchivos, "Cortesias", "Tipos", tipo.ImagenUrl);
                                bool insertedImageTipo = await serviceCortesias.SendImage("SaveImageTipo", sala.CodSala, filePathTipo, tipo.ImagenUrl);
                                if(insertedImageTipo) {
                                    success = true;
                                }

                                string filePathSubTipo = Path.Combine(PathArchivos, "Cortesias", "SubTipos", subTipo.ImagenUrl);
                                bool insertedImageSubTipo = await serviceCortesias.SendImage("SaveImageSubTipo", sala.CodSala, filePathSubTipo, subTipo.ImagenUrl);
                                if(insertedImageSubTipo) {
                                    success = true;
                                }
                            }
                        }



                    }

                }

                displayMessage = success ? "Sub tipo guardado correctamente." : "No se pudo guardar el sub tipo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteSubTipo(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = subTipoBL.EliminarSubTipo(id);
                displayMessage = success ? "Sub tipo eliminado correctamente." : "No se pudo eliminar el sub tipo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
