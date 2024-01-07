using AutoMapper;
using DAPA.Database.Discounts;
using DAPA.Database.Products;
using DAPA.Models;
using DAPA.Models.Public.Products;
using Microsoft.AspNetCore.Mvc;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IDiscountRepository discountRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    [HttpGet("/products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts([FromQuery] ProductFindRequest request)
    {
        try
        {
            var products = await _productRepository.GetAllAsync(request);
            return Ok(products);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateRequest request)
    {
        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(d => d.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
            return NotFound($"Could not find discount with id {request.DiscountId}");

        try
        {
            var product = _mapper.Map<Product>(request);
            if (product is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _productRepository.InsertAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        try
        {
            var product = await _productRepository.GetByPropertyAsync(p => p.Id == id);
            if (product is null)
                return NotFound($"Could not find product with id {id}");

            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, ProductUpdateRequest request)
    {
        bool discountExists;
        try
        {
            discountExists = await _discountRepository.ExistsByPropertyAsync(d => d.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!discountExists)
            return NotFound($"Could not find discount with id {request.DiscountId}");

        Product? product;

        try
        {
            product = await _productRepository.GetByPropertyAsync(p => p.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var updatedProduct = _mapper.Map(request, product);
        if (updatedProduct is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _productRepository.UpdateAsync(updatedProduct);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        bool productExists;
        try
        {
            productExists = await _productRepository.ExistsByPropertyAsync(p => p.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!productExists)
            return NotFound($"Could not find product with id {id}");

        var product = _mapper.Map<Product>(id);
        if (product is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _productRepository.DeleteAsync(product);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}