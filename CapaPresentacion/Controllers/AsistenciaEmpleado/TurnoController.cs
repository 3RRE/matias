using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.AsistenciaEmpleado
{
    [seguridad(false)]
    public class TurnoController : Controller
    {
        // GET: Turno
        public ActionResult Index()
        {
            return View();
        }

        // GET: Turno/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Turno/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Turno/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Turno/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Turno/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Turno/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Turno/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
