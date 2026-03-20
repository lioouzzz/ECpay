using Ecpay.Models;
using Ecpay.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecpay.Controllers.Api
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;


        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet( "{id}" )]
        public IActionResult GetOrder(int id)
        {
            var order = _orderService.GetOrderById( id );

            if ( order == null ) return NotFound();

            return Ok( new { order.Id , order.OrderNo , order.TotalAmount , order.Status , order.ItemName } );
        }
    }
}
