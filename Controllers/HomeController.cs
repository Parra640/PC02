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

         public IActionResult Registro(Menu x)
        {
                  if(ModelState.IsValid){
                 _context.Add(x);
                 _context.SaveChanges();
                 return RedirectToAction("Index");
             }

            return View();
        }



        public IActionResult AgregarMenu(){

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
