using Ecpay.Models;
using Ecpay.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecpay.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index( )
        {
            return View();
        }

        [HttpGet( "Payment/Result" )]
        public IActionResult Result( )
        {
            return View();
        }

    }
}
