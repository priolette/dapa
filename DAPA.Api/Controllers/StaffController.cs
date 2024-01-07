using AutoMapper;
using DAPA.Database.Roles;
using DAPA.Database.Staff;
using DAPA.Models;
using DAPA.Models.Public.Staff;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffRepository _staffRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public StaffController(IStaffRepository staffRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _staffRepository = staffRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Staff>>> GetAllStaff([FromQuery] StaffFindRequest request)
    {
        try
        {
            var staff = await _staffRepository.GetAllAsync(request);
            return Ok(staff);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Staff>> CreateStaff(StaffCreateRequest request)
    {
        bool roleExists;
        try
        {
            roleExists = await _roleRepository.ExistsByPropertyAsync(s => s.Id == request.RoleId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!roleExists)
        {
            return NotFound($"Could not find role with ID: {request.RoleId}");
        }

        try
        {
            var staff = _mapper.Map<Staff>(request);
            if (staff is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _staffRepository.InsertAsync(staff);

            return CreatedAtAction(nameof(GetStaffById), new { id = staff.Id }, staff);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Staff>> GetStaffById(int id)
    {
        Staff? staff;

        try
        {
            staff = await _staffRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (staff is null)
            return NotFound($"Could not find staff with ID: {id}");

        return Ok(staff);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Staff>> UpdateStaff(int id, StaffUpdateRequest request)
    {
        bool roleExists;
        try
        {
            roleExists = await _roleRepository.ExistsByPropertyAsync(s => s.Id == request.RoleId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!roleExists)
        {
            return NotFound($"Could not find role with ID: {request.RoleId}");
        }

        Staff? staff;

        try
        {
            staff = await _staffRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (staff is null)
            return NotFound($"Could not find staff with ID: {id}");

        try
        {
            var mappedStaff = _mapper.Map(request, staff);
            if (mappedStaff is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _staffRepository.UpdateAsync(staff);

            return Ok(staff);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        bool staffExists;
        try
        {
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!staffExists)
            return NotFound($"Could not find staff with ID: {id}");

        var staff = _mapper.Map<Staff>(id);
        if (staff is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _staffRepository.DeleteAsync(staff);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}