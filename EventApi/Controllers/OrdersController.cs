using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET orders/carts/{cart_id}
        [HttpGet("{cart_id}")]
        public async Task<IActionResult> GetCartItems(Guid cart_id)
        {
            //Task<OrderInfo>
            var  cartItems = await _orderService.GetOrderAsync(cart_id);
            return Ok(cartItems);
        }

        // POST orders/carts/{cart_id}
        [HttpPost("carts/{cart_id}")]
        public IActionResult AddSeatToCart(Guid cart_id, [FromBody] CartTicketData payload)
        {
            var cartItems = _orderService.AddNewTicketToOrderAsync(cart_id, payload);
            return Ok(cartItems);

        }

        // DELETE orders/carts/{cart_id}/events/{event_id}/seats/{seat_id}
        [HttpDelete("carts/{cart_id}/events/{event_id}/seats/{seat_id}")]
        public IActionResult DeleteSeatFromCart(Guid cart_id, int event_id, int seat_id)
        {
            
            var cartItems = _orderService.DeletTicketAsync(cart_id, event_id, seat_id);
            return Ok(cartItems);
        }

        // PUT orders/carts/{cart_id}/book
        [HttpPut("carts/{cart_id}/book")]
        public IActionResult BookCart(Guid cart_id)
        {
            var paymentId = _orderService.MooveTicketsInCartToBookedStatusAsync(cart_id);

            return Ok(new { PaymentId = paymentId });
        }
    }
}
