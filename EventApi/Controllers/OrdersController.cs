using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartItems(Guid cartId)
        {
            //Task<OrderInfo>
            var  cartItems = await _orderService.GetOrderAsync(cartId);
            return Ok(cartItems);
        }
        // POST orders/carts/{cartId}
        [HttpPost("carts/{cartId}")]
        public async Task<IActionResult> AddSeatToCart(Guid cartId, [FromBody] CartTicketData payload)
        {
            var cartItems = await _orderService.AddNewTicketToOrderAsync(cartId, payload);
            return Ok(cartItems);
        }
        // DELETE orders/carts/{cartId}/events/{eventId}/seats/{seatId}
        [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
        public async Task<IActionResult> DeleteSeatFromCart(Guid cartId, int eventId, int seatId)
        {
            var cartItems = await _orderService.DeletTicketAsync(cartId, eventId, seatId);
            return Ok(cartItems);
        }
        // PUT orders/carts/{cartId}/book
        [HttpPut("carts/{cartId}/book")]
        public async Task<IActionResult> BookCart(Guid cartId)
        {
            var paymentId = await _orderService.MooveTicketsInCartToBookedStatusAsync(cartId);

            return Ok(paymentId);
        }
    }
}
