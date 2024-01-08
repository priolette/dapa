using AutoMapper;
using DAPA.Database.Discounts;
using DAPA.Database.Orders;
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
    private readonly IProductCartRepository _productCartRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IProductCartRepository productCartRepository,
        IDiscountRepository discountRepository, IOrderRepository orderRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _productCartRepository = productCartRepository;
        _discountRepository = discountRepository;
        _orderRepository = orderRepository;
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
            return NotFound($"Could not find discount with ID: {request.DiscountId}");

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
                return NotFound($"Could not find product with ID: {id}");

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
            return NotFound($"Could not find discount with ID: {request.DiscountId}");

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

    [HttpGet("/product_carts")]
    public async Task<ActionResult<IEnumerable<ProductCart>>> GetAllProductCarts(
        [FromQuery] ProductCartFindRequest request)
    {
        try
        {
            var productCarts = await _productCartRepository.GetAllAsync(request);
            return Ok(productCarts);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/product_cart")]
    public async Task<ActionResult<ProductCart>> CreateProductCart([FromBody] ProductCartCreateRequest request)
    {
        bool orderExists;
        bool productExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(o => o.Id == request.OrderId);
            productExists = await _productRepository.ExistsByPropertyAsync(p => p.Id == request.ProductId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {request.OrderId}");
        if (!productExists)
            return NotFound($"Could not find product with ID: {request.ProductId}");

        try
        {
            var productCart = _mapper.Map<ProductCart>(request);
            if (productCart is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _productCartRepository.InsertAsync(productCart);

            return CreatedAtAction(nameof(GetProductCartById),
                new { orderId = request.OrderId, productId = request.ProductId }, productCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("/product_cart/{orderId:int}/{productId:int}")]
    public async Task<ActionResult<ProductCart>> GetProductCartById(int orderId, int productId)
    {
        try
        {
            var productCart =
                await _productCartRepository.GetByPropertyAsync(
                    pc => pc.OrderId == orderId && pc.ProductId == productId);
            if (productCart is null)
                return NotFound($"Could not find product cart with order ID: {orderId} and product ID: {productId}");

            return Ok(productCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("/product_cart/{orderId:int}/{productId:int}")]
    public async Task<ActionResult<ProductCart>> UpdateProductCart(int orderId, int productId,
        ProductCartUpdateRequest request)
    {
        bool orderExists;
        bool productExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(o => o.Id == orderId);
            productExists = await _productRepository.ExistsByPropertyAsync(p => p.Id == productId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {orderId}");
        if (!productExists)
            return NotFound($"Could not find product with ID: {productId}");

        ProductCart? productCart;

        try
        {
            productCart = await _productCartRepository.GetByPropertyAsync(
                pc => pc.OrderId == orderId && pc.ProductId == productId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var updatedProductCart = _mapper.Map(request, productCart);
        if (updatedProductCart is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _productCartRepository.UpdateAsync(updatedProductCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(updatedProductCart);
    }

    [HttpDelete("/product_cart/{orderId:int}/{productId:int}")]
    public async Task<IActionResult> DeleteProductCart(int orderId, int productId)
    {
        bool productCartExists;
        try
        {
            productCartExists = await _productCartRepository.ExistsByPropertyAsync(
                pc => pc.OrderId == orderId && pc.ProductId == productId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!productCartExists)
            return NotFound($"Could not find product cart with order ID: {orderId} and product ID: {productId}");

        var productCart = _mapper.Map<ProductCart>((orderId, productId));
        if (productCart is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _productCartRepository.DeleteAsync(productCart);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}