namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Persistence;

    [AllowAnonymous]
    public class HealthCheckController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        public HealthCheckController(ConduitDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CheckDbHealth()
        {
            return Ok(await _context.Database.CanConnectAsync());
        }
    }
}