using AutoMapper;
using DAPA.Database.Clients;
using DAPA.Database.Loyalties;
using DAPA.Models;
using DAPA.Models.Public.Clients;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILoyaltyRepository _loyaltyRepository;
    private readonly IMapper _mapper;

    public ClientController(IClientRepository clientRepository, ILoyaltyRepository loyaltyRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _loyaltyRepository = loyaltyRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("/clients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients([FromQuery] ClientFindRequest request)
    {
        try
        {
            var clients = await _clientRepository.GetAllAsync(request);
            return Ok(clients);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(ClientCreateRequest request)
    {
        bool loyaltyExists;
        try
        {
            loyaltyExists = request.LoyaltyId is null ||
                            await _loyaltyRepository.ExistsByPropertyAsync(l => l.Id == request.LoyaltyId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!loyaltyExists)
        {
            return NotFound($"Could not find loyalty with ID: {request.LoyaltyId}");
        }

        var client = _mapper.Map<Client>(request);
        try
        {
            await _clientRepository.InsertAsync(client);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetClientById(int id)
    {
        try
        {
            var client = await _clientRepository.GetByPropertyAsync(c => c.Id == id);

            if (client == null)
                return NotFound($"Could not find client with ID: {id}");

            return Ok(client);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Client>> UpdateClient(int id, ClientUpdateRequest request)
    {
        bool loyaltyExists;
        try
        {
            loyaltyExists = request.LoyaltyId is null ||
                            await _loyaltyRepository.ExistsByPropertyAsync(l => l.Id == request.LoyaltyId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!loyaltyExists)
        {
            return NotFound($"Could not find loyalty with ID: {request.LoyaltyId}");
        }

        try
        {
            var clientExists = await _clientRepository.ExistsByPropertyAsync(c => c.Id == id);
            if (!clientExists)
                return NotFound($"Could not find client with ID: {id}");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var updatedClient = _mapper.Map<Client>(request);
        if (updatedClient == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _clientRepository.UpdateAsync(updatedClient);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(updatedClient);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Client>> DeleteClient(int id)
    {
        Client? client;
        try
        {
            client = await _clientRepository.GetByPropertyAsync(c => c.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (client == null)
            return NotFound($"Could not find client with ID: {id}");

        try
        {
            await _clientRepository.DeleteAsync(client);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}