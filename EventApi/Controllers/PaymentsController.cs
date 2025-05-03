using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController:ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET payments/{payment_id}
        [HttpGet("{payment_id}")]
        public async Task<IActionResult> GetPaymentStatus(Guid payment_id)
        {
            var status = await _paymentService.GetPaymentStatus(payment_id);
            return Ok(status);
        }

        //POST payments/{payment_id}/complete Updates payment status and moves all the seats related to a payment to the sold state
        // POST payments/{payment_id}/complete
        [HttpGet("{payment_id}/complete")]
        public async Task<IActionResult> CompletePayment(Guid payment_id)
        {
            var status = await _paymentService.CompletePayment(payment_id);
            return Ok(status);
        }

        //POST payments/{payment_id}/failed Updates payment status and moves all the seats related to a payment to the available state.
        // POST payments/{payment_id}/failed
        [HttpGet("{payment_id}/failed")]
        public async Task<IActionResult> RollbackPayment(Guid payment_id)
        {
            var status = await _paymentService.RollBackPayment(payment_id);
            return Ok(status);
        }
    }
}
