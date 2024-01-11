using AutoMapper;
using DAPA.Database.Migrations;
using DAPA.Database.Reservations;
using DAPA.Database.Services;
using DAPA.Database.Staff;
using DAPA.Database.WorkingHours;
using DAPA.Models;
using DAPA.Models.Public.Reservations;
using DAPA.Models.Public.WorkingHours;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WorkingHoursController : ControllerBase
{
    private readonly IWorkingHoursRepository _workingHoursRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public WorkingHoursController(IWorkingHoursRepository workingHoursRepository, 
        IReservationRepository reservationRepository, IServiceRepository serviceRepository, IMapper mapper)
    {
        _reservationRepository  = reservationRepository;
        _serviceRepository      = serviceRepository;
        _workingHoursRepository = workingHoursRepository;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkingHour>>> GetAllWorkingHours([FromQuery] WorkingHoursFindRequest request)
    {
        WorkingHoursFindRequest request1 = new();
        request1.Id = 1;
        try
        {
            var workingHours = await _workingHoursRepository.GetAllAsync(request);
            return Ok(workingHours);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/workinghour")]
    public async Task<ActionResult<WorkingHour>> CreateWorkingHours(WorkingHoursCreateRequest request)
    {
        if (request is null)
        {
            return BadRequest("Invalid request");
        }

        bool staffExists;
        try
        {
            staffExists = await _workingHoursRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!staffExists)
            return NotFound($"Could not find staff with ID {request.StaffId}");

        try
        {
            var workingHour = _mapper.Map<WorkingHour>(request);
            if (workingHour is null)
                return StatusCode(StatusCodes.Status500InternalServerError);
                
            await _workingHoursRepository.InsertAsync(workingHour);

            return CreatedAtAction(nameof(GetWorkingHoursById), new { id = workingHour.Id }, workingHour);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkingHour>> GetWorkingHoursById(int id)
    {
        WorkingHour? workingHour;

        try
        {
            workingHour = await _workingHoursRepository.GetByPropertyAsync(r => r.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (workingHour is null)
            return NotFound($"Could not find working hours for with ID: {id}");

        return Ok(workingHour);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkingHour>> UpdateWorkingHours(int id, WorkingHoursUpdateRequest request)
    {
        WorkingHour? workingHour;

        try
        {
            workingHour = await _workingHoursRepository.GetByPropertyAsync(r => r.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (workingHour is null)
            return NotFound($"Could not find working hours for with ID: {id}");

        try
        {
            var updatedWorkingHours = _mapper.Map(request, workingHour);
            if (updatedWorkingHours is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _workingHoursRepository.UpdateAsync(updatedWorkingHours);

            return Ok(updatedWorkingHours);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkingHours(int id)
    {
        bool workingHourExists;
        try
        {
            workingHourExists = await _workingHoursRepository.ExistsByPropertyAsync(r => r.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!workingHourExists)
            return NotFound($"Could not find working hours for with ID: {id}");

        var workingHour = _mapper.Map<WorkingHour>(id);
        if (workingHour is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _workingHoursRepository.DeleteAsync(workingHour);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpGet("available/{StaffId:int}")]
    public async Task<ActionResult<IEnumerable<WorkingHour>>> GetAvailableWorkingHoursByStaffId(int staffId)
    {
        try
        {
            var workingHours = await _workingHoursRepository.GetAllAsync();
            return Ok(workingHours);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}