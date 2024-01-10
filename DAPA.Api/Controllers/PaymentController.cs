using AutoMapper;
using DAPA.Database.Orders;
using DAPA.Database.Payments;
using DAPA.Models;
using DAPA.Models.Public.Payments;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public PaymentController(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    [HttpGet("/payments")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments([FromQuery] PaymentFindRequest request)
    {
        try
        {
            var payments = await _paymentRepository.GetAllAsync(request);
            return Ok(payments);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment([FromBody] PaymentCreateRequest request)
    {
        bool orderExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(x => x.Id == request.OrderId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {request.OrderId}");

        try
        {
            var payment = _mapper.Map<Payment>(request);
            if (payment is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _paymentRepository.InsertAsync(payment);

            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Payment>> GetPaymentById(int id)
    {
        try
        {
            var payment = await _paymentRepository.GetByPropertyAsync(x => x.Id == id);
            if (payment is null)
                return NotFound($"Could not find payment with ID: {id}");

            return Ok(payment);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Payment>> UpdatePayment(int id, [FromBody] PaymentUpdateRequest request)
    {
        bool orderExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(x => x.Id == request.OrderId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {request.OrderId}");

        try
        {
            var payment = await _paymentRepository.GetByPropertyAsync(x => x.Id == id);
            if (payment is null)
                return NotFound($"Could not find payment with ID: {id}");

            _mapper.Map(request, payment);
            await _paymentRepository.UpdateAsync(payment);

            return Ok(payment);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePayment(int id)
    {
        try
        {
            var payment = await _paymentRepository.GetByPropertyAsync(x => x.Id == id);
            if (payment is null)
                return NotFound($"Could not find payment with ID: {id}");

            await _paymentRepository.DeleteAsync(payment);

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}