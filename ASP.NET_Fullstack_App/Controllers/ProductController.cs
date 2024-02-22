using ASP.NET_Fullstack_API.DAL;
using ASP.NET_Fullstack_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Fullstack_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public ProductController(MyAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var product = _context.Products.ToList();

                if (product.Count == 0)
                {
                    return NotFound("Products not available.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                var product = _context.Products.Find(id);

                if (product == null)
                {
                    return NotFound($"Product details not found with ID: {id}");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(Product model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return Ok("Product Created.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put(Product model)
        {
            if (model == null || model.ProductID == 0)
            {
                if(model == null)
                {
                    return BadRequest("Model data is invalid.");
                }
                else if(model.ProductID == 0)
                {
                    return BadRequest($"Product ID {model.ProductID} is invalid.");
                }
            }

            try
            {
                var product = _context.Products.Find(model.ProductID);

                if (product == null)
                {
                    return NotFound($"Product not found with ID: {model.ProductID}");
                }

                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Qty = model.Qty;
                _context.SaveChanges();
                return Ok("Product details updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _context.Products.Find(id);

                if (product == null)
                {
                    return NotFound($"Product not found with ID: {id}");
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok("Product details deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
