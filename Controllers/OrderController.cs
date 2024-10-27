using Microsoft.AspNetCore.Mvc;
using OrderService.Services;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly List<Order> Orders = new List<Order>();
        private readonly ProductServiceClient _productServiceClient;

        public OrderController(ProductServiceClient productServiceClient)
        {
            _productServiceClient = productServiceClient;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] List<OrderItem> items)
        {
            decimal totalPrice = 0;
            foreach (var item in items)
            {
                var product = await _productServiceClient.GetProductByIdAsync(item.ProductId);
                totalPrice += product.Price * item.Quantity;
            }

            var order = new Order { Id = Orders.Count + 1, Items = items, TotalPrice = totalPrice };
            Orders.Add(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            return order != null ? Ok(order) : NotFound();
        }
    }
}


