using EShop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ApiController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Produces("application/json")]
        [HttpPost("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var productName = _db.Products.Where(p => p.Name.Contains(term))
                                        .Select(p => p.Name).ToList();
                return Ok(productName);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

    }
}
