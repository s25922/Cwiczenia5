using Cw5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly S25922Context _dbContext;

        public ClientController(S25922Context dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            if (idClient <= 0)
            {
                return BadRequest("Invalid client ID.");
            }

            var client = await _dbContext.Clients.FindAsync(idClient);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            var clientTripsCount = await _dbContext.ClientTrips
                .CountAsync(ct => ct.IdClient == idClient);

            if (clientTripsCount > 0)
            {
                return BadRequest("Cannot remove client from database because they have associated trips.");
            }

            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();

            return Ok("Client deleted successfully.");
        }
    }
}