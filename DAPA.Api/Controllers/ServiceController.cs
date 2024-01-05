using AutoMapper;
using DAPA.Database;
using DAPA.Models;
using DAPA.Models.Public;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public ServiceController(IServiceRepository serviceRepository, IDiscountRepository discountRepository,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _discountRepository = discountRepository;
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
        if (service == null)
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

        if (service == null)
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

        if (service == null)
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
        if (newService == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _serviceRepository.UpdateAsync(service);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(service);
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
        if (service == null)
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
}