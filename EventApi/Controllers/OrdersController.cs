using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMemoryCache _cache;
        private readonly ICacheHelper _cacheHelper;

        public OrdersController(IOrderService orderService, IMemoryCache memoryCache, ICacheHelper cacheHelper)
        {
            _cache = memoryCache;
            _orderService = orderService;
            _cacheHelper = cacheHelper;
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
            _cacheHelper.RemoveCacheData(_cache);
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
            _cacheHelper.RemoveCacheData(_cache);
            var paymentId = await _orderService.MooveTicketsInCartToBookedStatusAsync(cartId);

            return Ok(paymentId);
        }
    }
}
