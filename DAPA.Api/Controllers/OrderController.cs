using AutoMapper;
using DAPA.Database.Clients;
using DAPA.Database.Discounts;
using DAPA.Database.Orders;
using DAPA.Database.Reservations;
using DAPA.Database.Services;
using DAPA.Database.Staff;
using DAPA.Database.WorkingHours;
using DAPA.Models;
using DAPA.Models.Public.Orders;
using DAPA.Models.Public.Reservations;
using DAPA.Models.Public.Services;
using DAPA.Models.Public.WorkingHours;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public OrderController(IOrderRepository orderRepository, IClientRepository clientRepository,
        IStaffRepository staffRepository, IDiscountRepository discountRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
        _staffRepository = staffRepository;
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    [HttpGet("/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders([FromQuery] OrderFindRequest request)
    {
        try
        {
            var orders = await _orderRepository.GetAllAsync(request);
            return Ok(orders);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderCreateRequest request)
    {
        bool clientExists;
        bool staffExists;
        bool discountExists;

        try
        {
            clientExists = request.ClientId is null ||
                           await _clientRepository.ExistsByPropertyAsync(s => s.Id == request.ClientId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
            discountExists = request.DiscountId is null ||
                             await _discountRepository.ExistsByPropertyAsync(s => s.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!clientExists)
            return NotFound($"Could not find client with ID: {request.ClientId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID: {request.StaffId}");
        if (!discountExists)
            return NotFound($"Could not find discount with ID: {request.DiscountId}");

        var order = _mapper.Map<Order>(request);
        if (order is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _orderRepository.InsertAsync(order);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        Order? order;
        try
        {
            order = await _orderRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (order is null)
            return NotFound($"Could not find order with ID: {id}");

        return Ok(order);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Order>> UpdateOrder(int id, OrderUpdateRequest request)
    {
        bool clientExists;
        bool staffExists;
        bool discountExists;

        try
        {
            clientExists = request.ClientId is null ||
                           await _clientRepository.ExistsByPropertyAsync(s => s.Id == request.ClientId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
            discountExists = request.DiscountId is null ||
                             await _discountRepository.ExistsByPropertyAsync(s => s.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!clientExists)
            return NotFound($"Could not find client with ID: {request.ClientId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID: {request.StaffId}");
        if (!discountExists)
            return NotFound($"Could not find discount with ID: {request.DiscountId}");

        Order? order;
        try
        {
            order = await _orderRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (order is null)
            return NotFound($"Could not find order with ID: {id}");

        var updatedOrder = _mapper.Map(request, order);
        if (updatedOrder is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _orderRepository.UpdateAsync(updatedOrder);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(updatedOrder);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        bool orderExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {id}");

        var order = _mapper.Map<Order>(id);
        if (order is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _orderRepository.DeleteAsync(order);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpGet("total/{id}")]
    public async Task<ActionResult<List<WorkingHoursResponse>>> GetTotalByOrderId(int orderId)
    {
        return Ok();
        /*List<WorkingHoursDto> available = new();

        try
        {
            WorkingHoursFindRequest request = new();
            request.StaffId = staffId;
            var workingHours = await _workingHoursRepository.GetAllAsync(request);
            foreach (WorkingHour el in workingHours)
            {
                available.Add(new WorkingHoursDto { Start = el.StartTime, End = el.EndTime });
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        try
        {
            ReservationFindRequest request = new();
            request.StaffId = staffId;
            var reservations = await _reservationRepository.GetAllAsync(request);
            foreach (Models.Reservation el in reservations)
            {
                DateTime end;
                Console.WriteLine($"++++++++");
                Console.WriteLine($"{el.ServiceId}");
                Console.WriteLine($"{el.StaffId}");
                Console.WriteLine($"{el.DateTime}");

                try
                {
                    ServiceFindRequest request1 = new();
                    request1.Id = el.ServiceId;
                    var service = await _serviceRepository.GetAllAsync(request1);
                    Models.Service el2 = service.First();

                    Console.WriteLine($"********");
                    Console.WriteLine($"{el2.Duration}");
                    Console.WriteLine($"{el2.Id}");
                    end = el.DateTime.AddHours(el2.Duration);
                    Console.WriteLine($"{end}");
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                RemoveBusyHours(available, el.DateTime, end);
            }
            return Ok(available);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }*/
    }
}