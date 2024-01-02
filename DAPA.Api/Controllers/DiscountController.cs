using AutoMapper;
using DAPA.Database;
using DAPA.Models;
using DAPA.Models.Public;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public DiscountController(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("/discounts")]
    public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts(
        [FromQuery] DiscountFindRequest request
    )
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

    [HttpPost]
    public async Task<ActionResult<Discount>> CreateDiscount(DiscountCreateRequest request)
    {
        var discount = new Discount
        {
            Name = request.Name,
            Size = request.Size,
            Start_date = request.Start_date,
            End_date = request.End_date,
            Applicable_Category = request.Applicable_Category
        };

        try
        {
            await _discountRepository.InsertAsync(discount);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetById), new { discount.ID }, discount);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Discount>> GetById(int id)
    {
        Discount? discount;

        try
        {
            Console.WriteLine(id);
            discount = await _discountRepository.GetByPropertyAsync(d => d.ID == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (discount is null)
            return NotFound($"Could not find discount with ID: {id}");

        return Ok(discount);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Discount>> UpdateDiscount(
        int id,
        [FromBody] DiscountUpdateRequest request
    )
    {
        Discount? discount;

        try
        {
            discount = await _discountRepository.GetByPropertyAsync(d => d.ID == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (discount is null)
            return NotFound($"Could not find discount with ID: {id}");

        var newDiscount = _mapper.Map(request, discount);
        if (newDiscount is null) return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _discountRepository.UpdateAsync(discount);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(discount);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteDiscount(int id)
    {
        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(d => d.ID == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
            return NotFound($"Could not find discount with ID: {id}");

        var discount = _mapper.Map<Discount>(id);
        if (discount is null) return StatusCode(StatusCodes.Status500InternalServerError);

        // Perhaps also use AutoMapper here?
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
}