using FoodOrderSystemAPI.DTOs;
using FoodOrderSystemAPI.DTOs.Order;
using FoodOrderSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderSystemAPI.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }


        // GET: api/Orders
        [HttpGet, Authorize]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            return await _service.GetOrders();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            return await _service.GetOrder(id);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO orderDTO)
        {
            await _service.CreateOrder(orderDTO);
            return CreatedAtAction(nameof(GetOrder), new { id = orderDTO.OrderId }, orderDTO);
        }

        // PUT: api/Orders/status/5
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> PatchOrderStatus(int id, OrderStatusPatchDTO orderStatusPatchDTO)
        {
            return await _service.PatchOrderStatus(id, orderStatusPatchDTO.OrderStatusId);
        }

        // PUT: api/Orders/status/5
        [HttpPatch("menus/{orderId}")]
        public async Task<IActionResult> PatchOrderMenus(int orderId, List<MenuOrderDTO> menuOrderDTO)
        {
            return await _service.PatchOrderMenus(orderId, menuOrderDTO);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return await _service.DeleteOrder(id);
        }

    }
}
