using FoodOrderSystemAPI.DTOs;
using FoodOrderSystemAPI.DTOs.Order;
using FoodOrderSystemAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystemAPI.Services
{
    public class OrderService
    {
        private readonly FoodOrderingSystemContext _context;

        public OrderService(FoodOrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDTO>> GetOrders()
        {
            var orders = await (from o in _context.Orders
                                join os in _context.OrderStatuses on o.OrderStatusId equals os.OrderStatusId
                                select new OrderDTO
                                {
                                    Username = o.Username,
                                    TableNo = o.TableNo,
                                    OrderNo = o.OrderNo,
                                    OrderStatusId = o.OrderStatusId,
                                    OrderStatus = os.Name,
                                    OrderId = o.OrderId,
                                    Date = o.Date
                                })
                               .AsNoTracking()
                               .ToListAsync();

            var ordersIds = orders.Select(Q => Q.OrderId).ToList();

            var menuOrders = await (from mo in _context.MenuOrders
                                    join m in _context.Menus on mo.MenuId equals m.MenuId
                                    where ordersIds.Contains(mo.OrderId)
                                    select new MenuOrderDTO
                                    {
                                        Menu = m.Name,
                                        MenuId = m.MenuId,
                                        OrderId = mo.OrderId,
                                        Qty = mo.Qty
                                    })
                   .AsNoTracking()
                   .ToListAsync();

            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].MenuOrders = menuOrders.Where(Q => Q.OrderId == orders[i].OrderId).ToList();   
            }

            return orders;
        }

        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await (from o in _context.Orders
                               join os in _context.OrderStatuses on o.OrderStatusId equals os.OrderStatusId
                               select new OrderDTO
                               {
                                   Username = o.Username,
                                   TableNo = o.TableNo,
                                   OrderNo = o.OrderNo,
                                   OrderStatusId = o.OrderStatusId,
                                   OrderStatus = os.Name,
                                   OrderId = o.OrderId,
                                   Date = o.Date
                               })
                               .AsNoTracking()
                               .FirstOrDefaultAsync();

            if (order == null)
            {
                return new NotFoundResult();
            }

            return order;
        }

        public async Task CreateOrder(OrderDTO orderDTO)
        {
            var newOrderNo = await CreateOrderNo();

            var order = new Order
            {
                Username = orderDTO.Username,
                OrderStatusId = orderDTO.OrderStatusId,
                Date = DateTime.Now,
                OrderNo = newOrderNo,
                TableNo = orderDTO.TableNo
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var menuOrders = new List<MenuOrder>();
            for (int i = 0; i < orderDTO.MenuOrders.Count; i++)
            {
                menuOrders.Add(new MenuOrder
                {
                    MenuId = orderDTO.MenuOrders[i].MenuId,
                    OrderId = order.OrderId,
                    Qty = orderDTO.MenuOrders[i].Qty,
                });
            }

            _context.MenuOrders.AddRange(menuOrders);
            await _context.SaveChangesAsync();
        }

        private async Task<string> CreateOrderNo()
        {
            var lastOrderNo = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(Q => Q.Date)
                .Select(Q => Q.OrderNo)
                .FirstOrDefaultAsync();

            var today = DateTime.Now;

            string day = today.Day.ToString();
            if (day.Length < 2)
            {
                day = "0" + day;
            }

            string month = today.Month.ToString();
            if (month.Length < 2)
            {
                month = "0" + month;
            }

            string year = today.Year.ToString();

            string newOrderNo = $"ABC{day}{month}{year}-";

            if (lastOrderNo == null)
            {
                newOrderNo += "001";
            }
            else
            {
                var lastNum = int.Parse(lastOrderNo.Substring(12, 3));
                lastNum++;

                if (lastNum < 10)
                {
                    newOrderNo += $"00{lastNum}";
                }
                else if (lastNum < 100)
                {
                    newOrderNo += $"0{lastNum}";
                }
            }

            return newOrderNo;
        }

        public async Task<ActionResult> DeleteOrder(int id)
        {
            var menuOrders = await _context.MenuOrders
                .AsNoTracking()
                .Where(Q => Q.OrderId == id)
                .ToListAsync();

            _context.MenuOrders.RemoveRange(menuOrders);
            
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return new NotFoundResult();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<ActionResult> PatchOrderStatus(int id, int orderStatusId)
        {
            var orderExists = await _context.Orders
                .AsTracking()
                .Where(Q => Q.OrderId == id)
                .FirstOrDefaultAsync();

            if (orderExists == null)
            {
                return new BadRequestResult();
            }

            orderExists.OrderStatusId = orderStatusId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<ActionResult> PatchOrderMenus(int id, List<MenuOrderDTO> menuOrderDTO)
        {
            var orderExists = await _context.Orders
                .AsTracking()
                .Where(Q => Q.OrderId == id)
                .FirstOrDefaultAsync();

            if (orderExists == null)
            {
                return new BadRequestResult();
            }

            // Remove older ones
            var oldMenuOrders = await _context.MenuOrders
                .AsNoTracking()
                .Where(Q => Q.OrderId == id)
                .ToListAsync();

            _context.MenuOrders.RemoveRange(oldMenuOrders);

            // Add new ones
            var newMenuOrders = new List<MenuOrder>();
            for (int i = 0; i < menuOrderDTO.Count; i++)
            {
                newMenuOrders.Add(new MenuOrder
                {
                    MenuId = menuOrderDTO[i].MenuId,
                    OrderId = id,
                    Qty = menuOrderDTO[i].Qty,
                });
            }

            _context.MenuOrders.AddRange(newMenuOrders);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

    }
}
