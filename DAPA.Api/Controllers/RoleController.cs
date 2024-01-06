using AutoMapper;
using DAPA.Database.Roles;
using DAPA.Models;
using DAPA.Models.Public.Roles;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleController(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    [HttpGet("/roles")]
    public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles([FromQuery] RoleFindRequest request)
    {
        try
        {
            var roles = await _roleRepository.GetAllAsync(request);
            return Ok(roles);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Role>> CreateRole(RoleCreateRequest request)
    {
        try
        {
            var role = _mapper.Map<Role>(request);
            if (role is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _roleRepository.InsertAsync(role);

            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Role>> GetRoleById(int id)
    {
        Role? role;

        try
        {
            role = await _roleRepository.GetByPropertyAsync(r => r.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (role is null)
            return NotFound($"Could not find role with ID: {id}");

        return Ok(role);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Role>> UpdateRole(int id, RoleUpdateRequest request)
    {
        Role? role;

        try
        {
            role = await _roleRepository.GetByPropertyAsync(r => r.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (role is null)
            return NotFound($"Could not find role with ID: {id}");

        try
        {
            var updatedRole = _mapper.Map(request, role);
            if (updatedRole is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _roleRepository.UpdateAsync(updatedRole);

            return Ok(updatedRole);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        bool roleExists;
        try
        {
            roleExists = await _roleRepository.ExistsByPropertyAsync(r => r.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!roleExists)
            return NotFound($"Could not find role with ID: {id}");

        var role = _mapper.Map<Role>(id);
        if (role is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _roleRepository.DeleteAsync(role);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}