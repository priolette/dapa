using AutoMapper;
using DAPA.Database.Clients;
using DAPA.Database.Discounts;
using DAPA.Database.Orders;
using DAPA.Database.Payments;
using DAPA.Database.Products;
using DAPA.Database.Reservations;
using DAPA.Database.Services;
using DAPA.Database.Staff;
using DAPA.Database.WorkingHours;
using DAPA.Models;
using DAPA.Models.Public.Discounts;
using DAPA.Models.Public.Orders;
using DAPA.Models.Public.Payments;
using DAPA.Models.Public.Products;
using DAPA.Models.Public.Reservations;
using DAPA.Models.Public.Services;
using DAPA.Models.Public.WorkingHours;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DAPA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IServiceCartRepository _serviceCartRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IProductCartRepository _productCartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public OrderController(IOrderRepository orderRepository, IClientRepository clientRepository,
        IStaffRepository staffRepository, IDiscountRepository discountRepository, IMapper mapper, IServiceCartRepository serviceCartRepository,
        IServiceRepository serviceRepository, IProductCartRepository productCartRepository, IProductRepository productRepository,
        IPaymentRepository paymentRepository)
    {
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
        _staffRepository = staffRepository;
        _discountRepository = discountRepository;
        _serviceCartRepository = serviceCartRepository;
        _serviceRepository = serviceRepository;
        _productCartRepository = productCartRepository;
        _productRepository = productRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    [HttpGet("/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders([FromQuery] OrderFindRequest request)
    {
        try
        {
            var orders = await _orderRepository.GetAllAsync(request);
            return Ok(orders);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderCreateRequest request)
    {
        bool clientExists;
        bool staffExists;
        bool discountExists;

        try
        {
            clientExists = request.ClientId is null ||
                           await _clientRepository.ExistsByPropertyAsync(s => s.Id == request.ClientId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
            discountExists = request.DiscountId is null ||
                             await _discountRepository.ExistsByPropertyAsync(s => s.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!clientExists)
            return NotFound($"Could not find client with ID: {request.ClientId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID: {request.StaffId}");
        if (!discountExists)
            return NotFound($"Could not find discount with ID: {request.DiscountId}");

        var order = _mapper.Map<Order>(request);
        if (order is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _orderRepository.InsertAsync(order);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        Order? order;
        try
        {
            order = await _orderRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (order is null)
            return NotFound($"Could not find order with ID: {id}");

        return Ok(order);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Order>> UpdateOrder(int id, OrderUpdateRequest request)
    {
        bool clientExists;
        bool staffExists;
        bool discountExists;

        try
        {
            clientExists = request.ClientId is null ||
                           await _clientRepository.ExistsByPropertyAsync(s => s.Id == request.ClientId);
            staffExists = await _staffRepository.ExistsByPropertyAsync(s => s.Id == request.StaffId);
            discountExists = request.DiscountId is null ||
                             await _discountRepository.ExistsByPropertyAsync(s => s.Id == request.DiscountId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!clientExists)
            return NotFound($"Could not find client with ID: {request.ClientId}");
        if (!staffExists)
            return NotFound($"Could not find staff with ID: {request.StaffId}");
        if (!discountExists)
            return NotFound($"Could not find discount with ID: {request.DiscountId}");

        Order? order;
        try
        {
            order = await _orderRepository.GetByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (order is null)
            return NotFound($"Could not find order with ID: {id}");

        var updatedOrder = _mapper.Map(request, order);
        if (updatedOrder is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _orderRepository.UpdateAsync(updatedOrder);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(updatedOrder);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        bool orderExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {id}");

        var order = _mapper.Map<Order>(id);
        if (order is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        try
        {
            await _orderRepository.DeleteAsync(order);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpGet("total/{id}")]
    public async Task<ActionResult<OrderTotalPriceResponse>> GetTotalByOrderId(int id)
    {
        OrderTotalPriceResponse total = new();
        total.TotalAmount = 0;
        total.Paid = 0;
        int orderdiscount = 0;
        total.Discount = 0;

        bool orderExists;
        try
        {
            orderExists = await _orderRepository.ExistsByPropertyAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!orderExists)
            return NotFound($"Could not find order with ID: {id}");

        try
        {
            OrderFindRequest orderFindRequest = new();
            orderFindRequest.Id = id;
            var orders = await _orderRepository.GetAllAsync(orderFindRequest);
            Models.Order currentOrder = orders.First();
            total.TotalAmount += currentOrder.Tip;
            total.Tip = currentOrder.Tip;

            if(currentOrder.DiscountId != null)
            {
                DiscountFindRequest discountFindRequest = new();
                discountFindRequest.Id = currentOrder.DiscountId;
                var discounts = await _discountRepository.GetAllAsync(discountFindRequest);
                Models.Discount thisDiscount = discounts.First();
                orderdiscount = thisDiscount.Size;
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        try
        {
            ServiceCartFindRequest serviceCartFindRequest = new();
            serviceCartFindRequest.OrderId = id;
            var serviceCart = await _serviceCartRepository.GetAllAsync(serviceCartFindRequest);
            foreach (Models.ServiceCart el in serviceCart)
            {
                ServiceFindRequest serviceFindRequest = new();
                serviceFindRequest.Id = el.ServiceId;
                var service = await _serviceRepository.GetAllAsync(serviceFindRequest);
                Models.Service thisService = service.First();

                DiscountFindRequest discountFindRequest = new();
                discountFindRequest.Id = thisService.DiscountId;
                var discounts = await _discountRepository.GetAllAsync(discountFindRequest);
                Models.Discount thisDiscount = discounts.First();

                float dis = el.Quantity * (thisService.Price * (thisDiscount.Size) / 100);
                float sum = el.Quantity * thisService.Price;

                total.Discount += dis;
                total.TotalAmount += sum;
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        try
        {
            ProductCartFindRequest productCartFindRequest = new();
            productCartFindRequest.OrderId = id;
            var productCart = await _productCartRepository.GetAllAsync(productCartFindRequest);
            foreach (Models.ProductCart el in productCart)
            {
                ProductFindRequest productFindRequest = new();
                productFindRequest.Id = el.ProductId;
                var product = await _productRepository.GetAllAsync(productFindRequest);
                Models.Product thisProduct = product.First();

                DiscountFindRequest discountFindRequest = new();
                discountFindRequest.Id = thisProduct.DiscountId;
                var discounts = await _discountRepository.GetAllAsync(discountFindRequest);
                Models.Discount thisDiscount = discounts.First();

                float dis = el.Quantity * ( thisProduct.Price * (thisDiscount.Size) / 100 );
                float sum = el.Quantity * thisProduct.Price;

                total.Discount += dis;
                total.TotalAmount += sum;
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        total.Discount += total.TotalAmount * (orderdiscount) / 100;
        float realAmount = total.TotalAmount - total.Discount;
        
        total.Tax = realAmount / 100*21;

        try
        {
            PaymentFindRequest paymentFindRequest = new();
            paymentFindRequest.OrderId = id;
            var payments = await _paymentRepository.GetAllAsync(paymentFindRequest);
            foreach (Models.Payment el in payments)
            {
                if(el.Amount != null)
                {
                    total.Paid += el.Amount.Value;
                }   
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        total.ToPay = realAmount - total.Paid;
        return Ok(total);
    }
}