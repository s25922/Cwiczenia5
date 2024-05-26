using Cw5.Models;
using Cw5.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly S25922Context _dbContext;

        public TripController(S25922Context dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {
            var trips = await _dbContext.Trips
                .Select(e => new GetTripsResponseDto
                {
                    IdTrip = e.IdTrip,
                    Name = e.Name,
                    Description = e.Description,
                    DateFrom = e.DateFrom,
                    DateTo = e.DateTo,
                    MaxPeople = e.MaxPeople,
                    Clients = e.ClientTrips.Select(x => new ClientResponseDto
                    {
                        FirstName = x.IdClientNavigation.FirstName,
                        LastName = x.IdClientNavigation.LastName
                    }).ToList(),
                    Countries = e.IdCountries.Select(x => new CountryResponseDto
                    {
                        Name = x.Name
                    }).ToList()
                })
                .OrderBy(e => e.DateFrom)
                .ToListAsync();

            return Ok(trips);
        }


        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, AddTripToClientRequestDto addTripToClientRequestDto)
        {
            if (idTrip != addTripToClientRequestDto.IdTrip)
            {
                return BadRequest("IdTrip doesn't match query.");
            }

            var trip = await _dbContext.Trips
                .Include(t => t.ClientTrips)
                .Select(t => new
                {
                    IdTrip = t.IdTrip,
                    Name = t.Name,
                    ClientTrips = t.ClientTrips.Select(ct => new
                    {
                        IdClient = ct.IdClient
                    })
                })
                .FirstOrDefaultAsync(t => t.IdTrip == addTripToClientRequestDto.IdTrip && t.Name == addTripToClientRequestDto.TripName);

            if (trip == null)
            {
                return BadRequest("Trip doesn't exist.");
            }

            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == addTripToClientRequestDto.Pesel);

            if (client != null)
            {
                if (trip.ClientTrips.Any(ct => ct.IdClient == client.IdClient))
                {
                    return BadRequest("The client is already assigned to this tour.");
                }
            }
            else
            {
                var newIdClient = await _dbContext.Clients.MaxAsync(c => c.IdClient) + 1;

                client = new Client
                {
                    IdClient = newIdClient,
                    FirstName = addTripToClientRequestDto.FirstName,
                    LastName = addTripToClientRequestDto.LastName,
                    Pesel = addTripToClientRequestDto.Pesel,
                    Email = addTripToClientRequestDto.Email,
                    Telephone = addTripToClientRequestDto.Telephone
                };

                _dbContext.Clients.Add(client);
            }

            var newClientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                PaymentDate = DateTime.Parse(addTripToClientRequestDto.PaymentDate),
                RegisteredAt = DateTime.Now
            };

            _dbContext.ClientTrips.Add(newClientTrip);
            await _dbContext.SaveChangesAsync();

            return Ok("Client assigned to trip successfully.");
        }
    }
}
