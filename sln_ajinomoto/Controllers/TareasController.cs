using RetoTecnicoAjinomoto.Models;
using RetoTecnicoAjinomoto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Search(int? id)
        {
            try
            {
                //if (oTareas.Tarea.Titulo != null && oTareas.Tarea.Descripcion != null && oTareas.Tarea.IdEstadoTarea != null)
                //{
                //    using (TareasDbModels context = new TareasDbModels())
                //    {
                //        context.Tareas.Add(oTareas.Tarea);
                //        context.SaveChanges();
                //        var listadoTareas = context.Tareas.ToList();
                //        foreach (var item in listadoTareas)
                //        {
                //            item.DescripcionEstado = context.EstadoTarea.Where(x => x.Id == item.IdEstadoTarea).FirstOrDefault().Nombre;
                //        }
                //        ViewBag.ItemsTarea = listadoTareas;
                //    }
                //}
                return View("~/Views/Tareas/Index.cshtml");

            }
            catch
            {
                return View();
            }
        }

        // GET: Tareas/Details/5
        public ActionResult Details(int id)
        {
            using(TareasDbModels context = new TareasDbModels())
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
        public ActionResult Create(TareasViewModelRegister oTareas)
        {
            try
            {
                if (oTareas.Tarea.Titulo != null && oTareas.Tarea.Descripcion != null && oTareas.Tarea.IdEstadoTarea != null)
                {
                    using (TareasDbModels context = new TareasDbModels())
                    {
                        context.Tareas.Add(oTareas.Tarea);
                        context.SaveChanges();
                        var listadoTareas = context.Tareas.ToList();
                        foreach (var item in listadoTareas)
                        {
                            item.DescripcionEstado = context.EstadoTarea.Where(x => x.Id == item.IdEstadoTarea).FirstOrDefault().Nombre;
                        }
                        ViewBag.ItemsTarea = listadoTareas;
                    }
                }
                return View("~/Views/Tareas/Index.cshtml");

            }
            catch
            {
                return View();
            }
        }

        // GET: Tareas/Edit/5
        public ActionResult Edit(int id)
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
        public ActionResult Edit(int id, TareasViewModelRegister oTareas)
        {
            try
            {
                using (TareasDbModels context = new TareasDbModels())
                {
                    oTareas.Tarea.Id = id;
                    context.Entry(oTareas.Tarea).State = System.Data.EntityState.Modified; //Aplicación de la entidad que viaje desde el viewModel para ser modificada
                    context.SaveChanges(); //guardamos los cambios para que surga efectos.
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
