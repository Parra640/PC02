using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PC02.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PC02.Controllers
{
    public class HomeController : Controller
    {

        private dbContext _context;
        private SignInManager<Usuario> _sim;
        private UserManager<Usuario> _um;
        private RoleManager<IdentityRole> _rm;
        public HomeController(
            dbContext c,
            SignInManager<Usuario> s,
            UserManager<Usuario> um,
            RoleManager<IdentityRole> rm)
        {

            _context = c;
            _sim = s;
            _um = um;
            _rm = rm;
        }
        public IActionResult Index()
        {
            
            var lunes=_context.Menus.Where(x => x.Fecha<DateTime.Now && x.Fecha.AddDays(7) > DateTime.Now && x.Fecha.DayOfWeek==DayOfWeek.Monday).FirstOrDefault();
            var martes=_context.Menus.Where(x => x.Fecha<DateTime.Now && x.Fecha.AddDays(7) > DateTime.Now && x.Fecha.DayOfWeek==DayOfWeek.Tuesday).FirstOrDefault();
            var miercoles=_context.Menus.Where(x => x.Fecha<DateTime.Now && x.Fecha.AddDays(7) > DateTime.Now && x.Fecha.DayOfWeek==DayOfWeek.Wednesday).FirstOrDefault();
            var jueves=_context.Menus.Where(x => x.Fecha<DateTime.Now && x.Fecha.AddDays(7) > DateTime.Now && x.Fecha.DayOfWeek==DayOfWeek.Thursday).FirstOrDefault();
            var viernes=_context.Menus.Where(x => x.Fecha<DateTime.Now && x.Fecha.AddDays(7) > DateTime.Now && x.Fecha.DayOfWeek==DayOfWeek.Friday).FirstOrDefault();
            ViewBag.Lunes=lunes;
            ViewBag.Martes=martes;
            ViewBag.Miercoles=miercoles;
            ViewBag.Jueves=jueves;
            ViewBag.Viernes=viernes;
            
            //var menu=_context.Menus.Where(x => x.Fecha>DateTime.Now && x.fecha < DateTime.Now.AddDays(7)).ToList();
            //var menu=_context.Menus.Where(x => YEARWEEK(x.Fecha)==YEARWEEK(DateTime.Now));
            //var menu=_context.Menus.FromSqlRaw("select *from Menus WHERE YEARWEEK(UserPostDate) = YEARWEEK(NOW());").ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="admin")]
         public IActionResult Registro(Menu x)
        {
                  if(ModelState.IsValid){
                 _context.Add(x);
                 _context.SaveChanges();
                 return RedirectToAction("Index");
             }

            return View(x);
        }

        [Authorize(Roles="admin")]
        public IActionResult AsociarRol()
        {
            ViewBag.Usuarios = _um.Users.ToList();
            ViewBag.Roles = _rm.Roles.ToList();
            return View();
        }

        [Authorize(Roles="admin")]
        [HttpPost]
        public IActionResult AsociarRol(string usuario, string rol)
        {
            var user = _um.FindByIdAsync(usuario).Result;

            var resultado = _um.AddToRoleAsync(user, rol).Result;
            
            TempData["mensaje"]="Categoria asociada con éxito";
            TempData["tipoTexto"]="text-success";
            ViewBag.Usuarios = _um.Users.ToList();
            ViewBag.Roles = _rm.Roles.ToList();
            return View();
        }

        [Authorize(Roles="admin")]
        public IActionResult CrearRol()
        {
            return View();
        }

        
        [Authorize(Roles="admin")]
        [HttpPost]
        public IActionResult CrearRol(string nombre)
        {
            var rol = new IdentityRole();
            rol.Name = nombre;

            var resultado = _rm.CreateAsync(rol).Result;

            if(resultado.Succeeded){
                TempData["mensaje"]="Rol creado con éxito";
                TempData["tipoTexto"]="text-success";
            }else{
                TempData["mensaje"]="No se pudo crear el rol";
                TempData["tipoTexto"]="text-danger";
            }
            return View();
        }

        
        public IActionResult CrearCuenta()
        {
            return View();
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearCuenta(CrearCuentaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Guardar datos del modelo en la tabla usuarios
                var usuario = new Usuario();
                usuario.UserName = model.Correo;
                usuario.Correo = model.Correo;

                IdentityResult resultado = _um.CreateAsync(usuario, model.Password1).Result;
                var r = _um.AddToRoleAsync(usuario, "usuario").Result;

                if (resultado.Succeeded)
                {
                    TempData["mensaje"]="Cuenta creada con éxito";
                    TempData["tipoTexto"]="text-success";
                    return View();
                }
                else
                {
                    TempData["mensaje"]="Error. No se pudo crear a cuenta";
                    TempData["tipoTexto"]="text-danger";
                    foreach (var item in resultado.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {


                var resultado = _sim.PasswordSignInAsync(model.Correo, model.Password, true, false).Result;

                if (resultado.Succeeded)
                {

                    return RedirectToAction("index", "home");
                }
                else
                {
                    TempData["mensaje"]="Error. Datos incorrectos";
                    TempData["tipoTexto"]="text-danger";
                    ModelState.AddModelError("", "Datos incorrectos");
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            _sim.SignOutAsync();
            return RedirectToAction("index", "home");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
