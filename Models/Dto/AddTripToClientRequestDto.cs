using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Models.Dto
{
    public class AddTripToClientRequestDto
    {
        [NotNull]
        public string FirstName { get; set; }
        [NotNull]
        public string LastName { get; set; }
        [NotNull]
        public string Email { get; set; }
        [NotNull]
        public string Telephone { get; set; }
        [StringLength(11)]
        public string Pesel { get; set; }
        [NotNull]
        public int IdTrip { get; set; }
        [NotNull]
        public string TripName { get; set; }
        public string PaymentDate { get; set; } = string.Empty;
    }
}
