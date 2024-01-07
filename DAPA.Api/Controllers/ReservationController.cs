using AutoMapper;
using DAPA.Database.Clients;
using DAPA.Database.Reservations;
using DAPA.Database.Services;
using DAPA.Database.Staff;
using DAPA.Models;
using DAPA.Models.Public.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    public ReservationController(IReservationRepository reservationRepository, IClientRepository clientRepository,
        IServiceRepository serviceRepository, IStaffRepository staffRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _clientRepository = clientRepository;
        _serviceRepository = serviceRepository;
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    [HttpGet("/reservations")]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations(
        [FromQuery] ReservationFindRequest request)
    {
        try
        {
            var reservations = await _reservationRepository.GetAllAsync(request);
            return Ok(reservations);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> CreateReservation([FromBody] ReservationCreateRequest request)
    {
        bool clientExists;
        bool serviceExists;
        bool staffExists;
        try
        {
            clientExists = await _clientRepository.ExistsByPropertyAsync(c => c.Id == request.ClientId);
            serviceExists = await _serviceRepository.ExistsByPropertyAsync(s => s.Id == request.ServiceId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!clientExists)
            return NotFound($"Could not find client with ID {request.ClientId}");
        if (!serviceExists)
            return NotFound($"Could not find service with ID {request.ServiceId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID {request.StaffId}");

        try
        {
            var reservation = _mapper.Map<Reservation>(request);
            if (reservation is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _reservationRepository.InsertAsync(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reservation>> GetReservationById(int id)
    {
        try
        {
            var reservation = await _reservationRepository.GetByPropertyAsync(r => r.Id == id);
            if (reservation is null)
                return NotFound($"Could not find reservation with ID {id}");

            return Ok(reservation);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Reservation>> UpdateReservation(int id, ReservationUpdateRequest request)
    {
        bool clientExists;
        bool serviceExists;
        bool staffExists;
        try
        {
            clientExists = await _clientRepository.ExistsByPropertyAsync(c => c.Id == request.ClientId);
            serviceExists = await _serviceRepository.ExistsByPropertyAsync(s => s.Id == request.ServiceId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!clientExists)
            return NotFound($"Could not find client with ID {request.ClientId}");
        if (!serviceExists)
            return NotFound($"Could not find service with ID {request.ServiceId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID {request.StaffId}");

        try
        {
            var reservation = await _reservationRepository.GetByPropertyAsync(r => r.Id == id);
            if (reservation is null)
                return NotFound($"Could not find reservation with ID {id}");

            var mappedReservation = _mapper.Map(request, reservation);
            if (mappedReservation is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _reservationRepository.UpdateAsync(mappedReservation);

            return Ok(mappedReservation);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Reservation>> DeleteReservation(int id)
    {
        try
        {
            var reservationExists = await _reservationRepository.ExistsByPropertyAsync(r => r.Id == id);
            if (!reservationExists)
                return NotFound($"Could not find reservation with ID {id}");

            var reservation = _mapper.Map<Reservation>(id);
            if (reservation is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _reservationRepository.DeleteAsync(reservation);

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}