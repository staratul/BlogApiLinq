using BlogApiLinq.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApiLinq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LambdaExpressionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LambdaExpressionController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
