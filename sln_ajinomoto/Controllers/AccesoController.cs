using Microsoft.IdentityModel.Tokens;
using RetoTecnicoAjinomoto.Models;
using RetoTecnicoAjinomoto.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RetoTecnicoAjinomoto.Controllers
{
    public class AccesoController : Controller
    {

        private const string SecretKey = "HFmNLA8TSttPzq6lVGlG";

        public ActionResult Login(Login oLogin)
        {
            if (!string.IsNullOrWhiteSpace(oLogin.Usuario) && !string.IsNullOrWhiteSpace(oLogin.Contrasena))
            {
                using (TareasDbModels context = new TareasDbModels())
                {
                    //Desencriptar en tiempo de ejecución las contraseñas base64 que tenemos alojado en la bd.
                    oLogin.Contrasena = Base64Encode(oLogin.Contrasena);
                    //Buscamos el usuario logueado mediante el contexto en la tabla Usuarios.
                    var ListaUsuarioEntity = context.Usuarios.Where(x => x.Correo == oLogin.Usuario && x.Contrasena == oLogin.Contrasena).ToList(); 
                    if (ListaUsuarioEntity.Count == 0)
                    {
                        ViewData["Respuesta"] = "Usuario no encontrado";
                        return View();

                    }
                    else
                    {
                        var listadoTareas = context.Tareas.ToList();
                        //Realizamos un match para obtener la descripción estado de tareas para ser visualizado en pantalla (completas | incompletas).
                        foreach (var item in listadoTareas)
                        {
                            item.DescripcionEstado = context.EstadoTarea.Where(x => x.Id == item.IdEstadoTarea).FirstOrDefault().Nombre;
                        }
                        ViewBag.ItemsTarea = listadoTareas;


                        var estadosTarea = context.EstadoTarea.ToList();
                        
                        var modelo = new TareasViewModelRegister  //llenamos un objeto viewModel que nos servira para presentar el jwt y el listado de estados en la ventana principal de busqueda.
                        {
                            ListaEstadoTareas = estadosTarea
                        };

                        return View("~/Views/Tareas/Index.cshtml", modelo);
                    }
                }
            }
            else
            {
                return View();
            }
            
        }

        public static string Base64Encode(string plainText)
        {
            //Decodificación base64
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}