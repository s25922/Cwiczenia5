using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Models.Dto
{
    public class GetTripsResponseDto
    {
        public int IdTrip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        // public IEnumerable<CountryResponseDto> Countries { get; set; }
        // public IEnumerable<ClientResponseDto> Clients { get; set; }
        public List<ClientResponseDto> Clients { get; set; }
        public List<CountryResponseDto> Countries { get; set; }
    }
}


// public int IdTrip { get; set; }
//
// public string Name { get; set; } = null!;
//
// public string Description { get; set; } = null!;
//
// public DateTime DateFrom { get; set; }
//
// public DateTime DateTo { get; set; }
//
// public int MaxPeople { get; set; }
//
// public List<ClientTripDTO> Clients { get; set; } = [];
//
// public List<CountryTripDTO> Countries { get; set; } = [];

