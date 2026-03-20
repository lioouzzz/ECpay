using Ecpay.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecpay.Controllers
{
    public class OrderController : Controller
    {

        [HttpGet( "/order/{id}" )]
        public IActionResult OrderId(int id)
        {
            var model = new OrderModel { Id = id };
            return View( model );
        }

    }
}
