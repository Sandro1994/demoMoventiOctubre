using RetoTecnicoAjinomoto.Models;
using RetoTecnicoAjinomoto.Service.Interface;
using RetoTecnicoAjinomoto.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetoTecnicoAjinomoto.Service.Implementation
{
    public class TareaService : ITareaService
    {
        public TareaResponse EditarTarea(Tareas objTareas)
        {
            TareaResponse tareaResponse = new TareaResponse();
            try
            {
                using (TareasDbModels context = new TareasDbModels())
                {
                    Tareas tarea = context.Tareas.Where(x=>x.Id== objTareas.Id).ToList().FirstOrDefault();
                    tarea.Titulo = objTareas.Titulo;
                    tarea.Descripcion = objTareas.Descripcion;
                    tarea.IdEstadoTarea = objTareas.IdEstadoTarea;
                    tareaResponse = new TareaResponse
                    {
                        Codigo = "OK",
                        Mensaje = "Tarea modificada correctamente."
                    };
                    context.Entry(tarea).State = System.Data.EntityState.Modified; //Aplicación de la entidad que viaje desde el viewModel para ser modificada
                    context.SaveChanges(); //guardamos los cambios para que surga efectos.
                 
                }
            }
            catch (Exception ex)
            {
                tareaResponse = new TareaResponse
                {
                    Codigo = "ERR",
                    Mensaje = $"Ocurrió un error: {ex.Message}"
                };
            }
            return tareaResponse;
        }

        public List<EstadoTarea> ObtenerEstadoTareas()
        {
            List<EstadoTarea> tareas = new List<EstadoTarea>();

            using (TareasDbModels context = new TareasDbModels())
            {
                tareas = context.EstadoTarea.ToList(); //Obtiene el catalogo de tareas del contexto dbModels
            }
            return tareas;
        }

        public TareaResponse RegistrarTarea(Tareas objTareas)
        {
            TareaResponse tareaResponse = new TareaResponse();
            try
            {
                using (TareasDbModels context = new TareasDbModels())
                {
                    tareaResponse = new TareaResponse {
                        Codigo = "OK",
                        Mensaje= "Tarea registrada correctamente."
                    };
                    context.Tareas.Add(objTareas);
                    context.SaveChanges();
                   
                }
                
            }
            catch(Exception ex)
            {
                tareaResponse = new TareaResponse
                {
                    Codigo = "ERR",
                    Mensaje = $"Ocurrió un error: {ex.Message}"
                };
            }
            

            return tareaResponse;
        }
    }
}