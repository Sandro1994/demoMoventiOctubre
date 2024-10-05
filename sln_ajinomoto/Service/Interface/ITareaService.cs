using RetoTecnicoAjinomoto.Models;
using RetoTecnicoAjinomoto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTecnicoAjinomoto.Service.Interface
{
    public interface ITareaService
    {
        List<EstadoTarea> ObtenerEstadoTareas();
        TareaResponse RegistrarTarea(Tareas objTareas);
        TareaResponse EditarTarea(Tareas objTareas);

    }
}
