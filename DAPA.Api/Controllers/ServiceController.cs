using AutoMapper;
using DAPA.Database;
using DAPA.Database.Discounts;
using DAPA.Database.Orders;
using DAPA.Database.Services;
using DAPA.Database.Staff;
using DAPA.Models;
using DAPA.Models.Public;
using DAPA.Models.Public.Services;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceCartRepository _serviceCartRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    public ServiceController(IServiceRepository serviceRepository, IServiceCartRepository serviceCartRepository,
        IDiscountRepository discountRepository, IOrderRepository orderRepository, IStaffRepository staffRepository,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _serviceCartRepository = serviceCartRepository;
        _discountRepository = discountRepository;
        _orderRepository = orderRepository;
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("/services")]
    public async Task<ActionResult<IEnumerable<Service>>> GetAllServices([FromQuery] ServiceFindRequest request)
    {
        try
        {
            var services = await _serviceRepository.GetAllAsync(request);
            return Ok(services);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Service>> CreateService(ServiceCreateRequest request)
    {
        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(s => s.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
        {
            return NotFound($"Could not find discount with ID: {request.DiscountId}");
        }

        var service = _mapper.Map<Service>(request);
        if (service is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceRepository.InsertAsync(service);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Service>> GetServiceById(int id)
    {
        Service? service;

        try
        {
            service = await _serviceRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (service is null)
            return NotFound($"Could not find service with ID: {id}");

        return Ok(service);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateService(int id, ServiceUpdateRequest request)
    {
        Service? service;

        try
        {
            service = await _serviceRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (service is null)
        {
            return NotFound($"Could not find service with ID: {id}");
        }

        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(s => s.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
        {
            return NotFound($"Could not find discount with ID: {request.DiscountId}");
        }

        var newService = _mapper.Map(request, service);
        if (newService is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceRepository.UpdateAsync(service);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(newService);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        bool serviceExists;
        try
        {
            serviceExists = await _serviceRepository.ExistsByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!serviceExists)
        {
            return NotFound($"Could not find service with ID: {id}");
        }

        var service = _mapper.Map<Service>(id);
        if (service is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceRepository.DeleteAsync(service);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpGet("/service_carts")]
    public async Task<ActionResult<IEnumerable<ServiceCart>>> GetAllServiceCarts(
        [FromQuery] ServiceCartFindRequest request)
    {
        try
        {
            var serviceCarts = await _serviceCartRepository.GetAllAsync(request);
            return Ok(serviceCarts);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/service_cart")]
    public async Task<ActionResult<ServiceCart>> CreateServiceCart(ServiceCartCreateRequest request)
    {
        bool serviceExists;
        bool orderExists;
        bool staffExists;
        try
        {
            serviceExists = await _serviceRepository.ExistsByPropertyAsync(s => s.Id == request.ServiceId);
            orderExists = await _orderRepository.ExistsByPropertyAsync(o => o.Id == request.OrderId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!serviceExists)
            return NotFound($"Could not find service with ID: {request.ServiceId}");
        if (!orderExists)
            return NotFound($"Could not find order with ID: {request.OrderId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID: {request.StaffId}");

        var serviceCart = _mapper.Map<ServiceCart>(request);
        if (serviceCart is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceCartRepository.InsertAsync(serviceCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetServiceCartById),
            new { orderId = request.OrderId, serviceId = request.ServiceId }, serviceCart);
    }

    [HttpGet("/service_cart/{orderId:int}/{serviceId:int}")]
    public async Task<ActionResult<ServiceCart>> GetServiceCartById(int orderId, int serviceId)
    {
        ServiceCart? serviceCart;

        try
        {
            serviceCart = await _serviceCartRepository.GetByPropertyAsync(s =>
                s.OrderId == orderId && s.ServiceId == serviceId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (serviceCart is null)
            return NotFound($"Could not find service cart with order ID: {orderId} and service ID: {serviceId}");

        return Ok(serviceCart);
    }

    [HttpPut("/service_cart/{orderId:int}/{serviceId:int}")]
    public async Task<IActionResult> UpdateServiceCart(int orderId, int serviceId, ServiceCartUpdateRequest request)
    {
        ServiceCart? serviceCart;

        try
        {
            serviceCart = await _serviceCartRepository.GetByPropertyAsync(s =>
                s.OrderId == orderId && s.ServiceId == serviceId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (serviceCart is null)
        {
            return NotFound($"Could not find service cart with order ID: {orderId} and service ID: {serviceId}");
        }

        bool serviceExists;
        bool orderExists;
        bool staffExists;
        try
        {
            serviceExists = await _serviceRepository.ExistsByPropertyAsync(s => s.Id == request.ServiceId);
            orderExists = await _orderRepository.ExistsByPropertyAsync(o => o.Id == request.OrderId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!serviceExists)
            return NotFound($"Could not find service with ID: {request.ServiceId}");
        if (!orderExists)
            return NotFound($"Could not find order with ID: {request.OrderId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID: {request.StaffId}");

        var newServiceCart = _mapper.Map(request, serviceCart);
        if (newServiceCart is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceCartRepository.UpdateAsync(serviceCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(newServiceCart);
    }

    [HttpDelete("/service_cart/{orderId:int}/{serviceId:int}")]
    public async Task<IActionResult> DeleteServiceCart(int orderId, int serviceId)
    {
        bool serviceCartExists;
        try
        {
            serviceCartExists = await _serviceCartRepository.ExistsByPropertyAsync(s =>
                s.OrderId == orderId && s.ServiceId == serviceId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!serviceCartExists)
        {
            return NotFound($"Could not find service cart with order ID: {orderId} and service ID: {serviceId}");
        }

        var serviceCart = _mapper.Map<ServiceCart>((orderId, serviceId));
        if (serviceCart is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceCartRepository.DeleteAsync(serviceCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}