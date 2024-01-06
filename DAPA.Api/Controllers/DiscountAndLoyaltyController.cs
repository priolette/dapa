using AutoMapper;
using DAPA.Database;
using DAPA.Database.Discounts;
using DAPA.Database.Loyalties;
using DAPA.Models;
using DAPA.Models.Public;
using DAPA.Models.Public.Discounts;
using DAPA.Models.Public.Loyalties;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
public class DiscountAndLoyaltyController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ILoyaltyRepository _loyaltyRepository;
    private readonly IMapper _mapper;

    public DiscountAndLoyaltyController(IDiscountRepository discountRepository,
        ILoyaltyRepository loyaltyRepository,
        IMapper mapper)
    {
        _discountRepository = discountRepository;
        _loyaltyRepository = loyaltyRepository;
        _mapper = mapper;
    }

    #region Discounts

    [HttpGet("/discounts")]
    public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts([FromQuery] DiscountFindRequest request)
    {
        try
        {
            var discounts = await _discountRepository.GetAllAsync(request);
            return Ok(discounts);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/discount")]
    public async Task<ActionResult<Discount>> CreateDiscount(DiscountCreateRequest request)
    {
        var discount = _mapper.Map<Discount>(request);
        if (discount is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _discountRepository.InsertAsync(discount);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetDiscountById),
            new
            {
                ID = discount.Id
            },
            discount);
    }

    [HttpGet("/discount/{id:int}")]
    public async Task<ActionResult<Discount>> GetDiscountById(int id)
    {
        Discount? discount;

        try
        {
            discount = await _discountRepository.GetByPropertyAsync(d => d.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (discount is null)
            return NotFound($"Could not find discount with ID: {id}");

        return Ok(discount);
    }

    [HttpPut("/discount/{id:int}")]
    public async Task<ActionResult<Discount>> UpdateDiscount(int id,
        [FromBody] DiscountUpdateRequest request)
    {
        Discount? discount;

        try
        {
            discount = await _discountRepository.GetByPropertyAsync(d => d.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (discount is null)
            return NotFound($"Could not find discount with ID: {id}");

        var newDiscount = _mapper.Map(request,
            discount);
        if (newDiscount is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _discountRepository.UpdateAsync(discount);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(newDiscount);
    }

    [HttpDelete("/discount/{id:int}")]
    public async Task<ActionResult> DeleteDiscount(int id)
    {
        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(d => d.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
            return NotFound($"Could not find discount with ID: {id}");

        var discount = _mapper.Map<Discount>(id);
        if (discount is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _discountRepository.DeleteAsync(discount);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    #endregion

    #region Loyalties

    [HttpGet("/loyalties")]
    public async Task<ActionResult<IEnumerable<Loyalty>>> GetAllLoyalties([FromQuery] LoyaltyFindRequest request)
    {
        try
        {
            var loyalties = await _loyaltyRepository.GetAllAsync(request);
            return Ok(loyalties);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/loyalty")]
    public async Task<ActionResult<Loyalty>> CreateLayout(LoyaltyCreateRequest request)
    {
        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(d => d.Id == request.Discount);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
            return NotFound($"Could not find discount with ID: {request.Discount}");

        var loyalty = _mapper.Map<Loyalty>(request);
        if (loyalty is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        Console.WriteLine(loyalty.DiscountId.ToString());

        try
        {
            await _loyaltyRepository.InsertAsync(loyalty);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetLoyaltyById),
            new
            {
                ID = loyalty.Id
            },
            loyalty);
    }

    [HttpGet("/loyalty/{id:int}")]
    public async Task<ActionResult<Loyalty>> GetLoyaltyById(int id)
    {
        Loyalty? loyalty;

        try
        {
            loyalty = await _loyaltyRepository.GetByPropertyAsync(l => l.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (loyalty is null)
            return NotFound($"Could not find loyalty with ID: {id}");

        return Ok(loyalty);
    }

    [HttpPut("/loyalty/{id:int}")]
    public async Task<ActionResult<Loyalty>> UpdateLoyalty(int id,
        [FromBody] LoyaltyUpdateRequest request)
    {
        Loyalty? loyalty;

        try
        {
            loyalty = await _loyaltyRepository.GetByPropertyAsync(d => d.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (loyalty is null)
            return NotFound($"Could not find loyalty with ID: {id}");

        var newLoyalty = _mapper.Map(request,
            loyalty);
        if (newLoyalty is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _loyaltyRepository.UpdateAsync(loyalty);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(loyalty);
    }

    [HttpDelete("/loyalty/{id:int}")]
    public async Task<ActionResult> DeleteLoyalty(int id)
    {
        bool loyaltyExists;
        try
        {
            loyaltyExists = await _loyaltyRepository.ExistsByPropertyAsync(l => l.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!loyaltyExists)
            return NotFound($"Could not find loyalty with ID: {id}");

        var loyalty = _mapper.Map<Loyalty>(id);
        if (loyalty is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _loyaltyRepository.DeleteAsync(loyalty);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    #endregion
}