using RetoTecnicoAjinomoto.Models;
using RetoTecnicoAjinomoto.Service.Implementation;
using RetoTecnicoAjinomoto.Service.Interface;
using RetoTecnicoAjinomoto.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace RetoTecnicoAjinomoto.Controllers
{
    //[Authorize]
    public class TareasController : Controller
    {
        // GET: Tareas
        public ActionResult Index()
        {
            using (TareasDbModels context = new TareasDbModels())
            {
                var listadoTareas = context.Tareas.ToList();
                foreach (var item in listadoTareas)
                {
                    //iteración de la descripción del estado por cada tarea.
                    item.DescripcionEstado = context.EstadoTarea.Where(x => x.Id == item.IdEstadoTarea).FirstOrDefault().Nombre;
                }
                ViewBag.ItemsTarea = listadoTareas;
                return View("~/Views/Tareas/Index.cshtml", listadoTareas);
            }
        }

        [HttpGet]
        public JsonResult ObtenerTareas()
        {
            ITareaService _tareaService = new TareaService();
            // Devuelve el array como un JSON
            return Json(_tareaService.ObtenerEstadoTareas(), JsonRequestBehavior.AllowGet);
        }


        // GET: Tareas/Details/5
        public ActionResult Details(int id)
        {
            using (TareasDbModels context = new TareasDbModels())
            {
                return View(context.Tareas.Where(x => x.Id == id)); //recupera el contexto para ser seteado en la vista detalle
            }
        }


        // GET: Tareas/Create
        public ActionResult Create()
        {

            using (TareasDbModels context = new TareasDbModels())
            {
                var estadosTarea = context.EstadoTarea.ToList();

                var modelo = new TareasViewModelRegister
                {
                    ListaEstadoTareas = estadosTarea
                };

                return View("~/Views/Tareas/Create.cshtml", modelo);
            }

        }

        // POST: Tareas/Create
        [HttpPost]
        public ActionResult Create(Tareas oTareas)
        {
            ITareaService _tareaService = new TareaService();
            return Json(_tareaService.RegistrarTarea(oTareas));

        }

        // GET: Tareas/Edit/5
        public ActionResult Edit(int? id)
        {
            using (TareasDbModels context = new TareasDbModels())
            {
                var estadosTarea = context.EstadoTarea.ToList();
                var modelo = new TareasViewModelRegister
                {
                    Tarea = context.Tareas.Where(x => x.Id == id).FirstOrDefault(),
                    ListaEstadoTareas = estadosTarea
                };

                return View("~/Views/Tareas/Edit.cshtml", modelo);

                //return View();
            }
        }

        // POST: Tareas/Edit/5
        [HttpPost]
        public ActionResult Edit(Tareas oTareas)
        {
            ITareaService _tareaService = new TareaService();
            return Json(_tareaService.EditarTarea(oTareas));

        }

        // GET: Tareas/Delete/5
        public ActionResult Delete(int id)
        {
            using (TareasDbModels context = new TareasDbModels())
            {
                return View(context.Tareas.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Tareas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (TareasDbModels context = new TareasDbModels())
                {
                    Tareas oTareas = context.Tareas.Where(x => x.Id == id).FirstOrDefault();
                    context.Tareas.Remove(oTareas); //remueve la tarea de la tabla haciendo referencia al contexto modelo.
                    context.SaveChanges();
                    var listadoTareas = context.Tareas.ToList();
                    foreach (var item in listadoTareas)
                    {
                        item.DescripcionEstado = context.EstadoTarea.Where(x => x.Id == item.IdEstadoTarea).FirstOrDefault().Nombre;
                    }
                    ViewBag.ItemsTarea = listadoTareas;
                }

                return View("~/Views/Tareas/Index.cshtml");
            }
            catch
            {
                return View();
            }
        }
    }
}
