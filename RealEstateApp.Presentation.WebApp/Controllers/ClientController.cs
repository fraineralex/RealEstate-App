using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Enums;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Client")]                   //Hay que arreglarlo
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
