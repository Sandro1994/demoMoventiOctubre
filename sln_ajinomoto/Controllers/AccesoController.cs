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
                        var token = GenerateJwtToken(); //si el usuario logueado existe se genera el JWT para ser utilizado como seguridad en todo el proyecto.
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
                            JWT = token,
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

        private string GenerateJwtToken()
        {
            //Generación JWT mediante un apiKey global que se encuentra en el startUp.
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "usuario")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}