using EBooKShopApi.Models;
using EBooKShopApi.Repositories;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EBooKShopApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        //https://localhost:port/api/orders/cart
        [HttpGet]
        [Authorize]
        [Route("cart")]
        public async Task<ActionResult> GetCartItems()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var res = await _orderRepository.GetCartItemsAsync(userId);
                    return Ok(new ApiResponse
                    {
                        Success = res != null ? true : false,
                        Message = "Get Cart",
                        Data = res
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Don't get Cart",
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/{id}
        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<ActionResult> GetDetailOrder(int id)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var res = await _orderRepository.GetDetailOrderAsync(id, userId);
                    /*var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    };
                    var json = JsonSerializer.Serialize(res, options);*/
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get detail order",
                        Data = res
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Don't get order",
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/addtocart
        [HttpPost]
        [Authorize]
        [Route("addtocart/{bookId:int}/{quantity:int}")]
        public async Task<ActionResult> AddToCart(int bookId, int quantity)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var res = await _orderRepository.AddToCartAsync(bookId, quantity, userId);
                    if (res)
                    {
                        return Ok(new ApiResponse
                        {
                            Success = res,
                            Message = "Add to cart successfully!!!",
                        });

                    }
                    else
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = res,
                            Message = "Order Item does not exist or the quantity exceeds the available stock."
                        });
                    }
                }
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "You are not logged in.",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/removecartitem/{orderItemId}
        [HttpDelete]
        [Authorize]
        [Route("removeitemtocart/{orderItemId:int}")]
        public async Task<ActionResult> RemoveCartItem(int orderItemId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var res = await _orderRepository.RemoveCartItemAsync(orderItemId, userId);
                    return Ok(new ApiResponse
                    {
                        Success = res,
                        Message = res ? "remove item successfully" : "Cannot remove item to cart ",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "You are not logged in.",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/changecartitem/{orderItemId}/{quantity}
        [HttpPut]
        [Authorize]
        [Route("changecartitem/{orderItemId:int}/{quantity:int}")]
        public async Task<ActionResult> ChangeCartItem(int orderItemId, int quantity)
        {
            try
            {
                
                var res = await _orderRepository.ChangeCartItemAsync(orderItemId, quantity);
                return Ok(new ApiResponse
                {
                    Success = res,
                    Message = res ? "change item successfully" : "Order Item does not exist or the quantity exceeds the available stock",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/checkout/{orderId:int}
        [HttpPut]
        [Authorize]
        [Route("checkout/{orderId:int}")]
        public async  Task<ActionResult> CheckOutCart(int orderId)
        {
            try
            {

                var res = await _orderRepository
                    .ChangeOrderStatusAsync(orderId, OrderStatus.InCart, OrderStatus.Pending);
                return Ok(new ApiResponse
                {
                    Success = res,
                    Message = res ? "Checkout to cart " : "Checkout fail",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/confirm/{orderId:int}
        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("confirm/{orderId:int}")]
        public async Task<ActionResult> ComfirmedByAdmin(int orderId)
        {
            try
            {

                var res = await _orderRepository
                    .ChangeOrderStatusAsync(orderId, OrderStatus.Pending, OrderStatus.Confirmed);
                return Ok(new ApiResponse
                {
                    Success = res,
                    Message = res ? "Comfirmed " : "Comfirmed fail",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/delivered/{orderId:int}
        [HttpPut]
        [Authorize]
        [Route("delivered/{orderId:int}")]
        public async Task<ActionResult> Delivered(int orderId)
        {
            try
            {

                var res = await _orderRepository
                    .ChangeOrderStatusAsync(orderId, OrderStatus.Confirmed, OrderStatus.Delivered);
                return Ok(new ApiResponse
                {
                    Success = res,
                    Message = res ? "Delivered " : "Delivered fail",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/cancel/{orderId:int} hủy đơn
        [HttpPut]
        [Authorize]
        [Route("cancel/{orderId:int}")]
        public async Task<ActionResult> Cancel(int orderId)
        {
            try
            {

                var res = await _orderRepository
                    .CancelOderAsync(orderId);
                return Ok(new ApiResponse
                {
                    Success = res,
                    Message = res ? "Cancel " : "Cancel fail",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/orders/status/{status}
        [HttpGet]
        [Authorize]
        [Route("status/{status:int}")]
        public async Task<ActionResult> GetOrderByOrderStatus(int status)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var res = await _orderRepository.GetOrderByOrderStatusAsync(status, userId);
                   
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get order",
                        Data = res
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Don't get order",
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}