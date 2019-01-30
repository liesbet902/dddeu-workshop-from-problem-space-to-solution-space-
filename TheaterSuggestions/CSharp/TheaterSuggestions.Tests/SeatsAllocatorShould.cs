using ExternalDependencies.ReservationsProvider;
using ExternalDependencies.AuditoriumLayoutRepository;
using NUnit.Framework;
using NFluent;
using System.Linq;

namespace SeatsSuggestions.Tests
{
    [TestFixture]
    public class SeatsAllocatorShould
    {
        // TODO: add your first acceptance test here
        [Test]
        public void SeatsNotAvailable_WhenAlreadyReserved()
        {
            var showId_AllSeatsReserved = "5";
            var party = 1;

            var reservations = new ReservationsProvider();
            var auditoriumLayout = new AuditoriumLayoutRepository();
            var seatAllocator = new SeatAllocator(reservations, auditoriumLayout);
            SuggestionMade suggestionMade = seatAllocator.MakeSuggestions(showId_AllSeatsReserved, party);

            Check.That(suggestionMade.Seats).IsEmpty();
        }

        [Test]
        public void SeatsAvailable_PartyOfOne_ReturnFirstSeat()
        {
            var showId_AllSeatsAvailable = "8";
            var party = 1;
            var reservations = new ReservationsProvider();
            var auditoriumLayout = new AuditoriumLayoutRepository();

            var seatAllocator = new SeatAllocator(reservations, auditoriumLayout);
            SuggestionMade suggestionMade = seatAllocator.MakeSuggestions(showId_AllSeatsAvailable, party);

            Check.That(suggestionMade.Seats).Contains(new Seat("A1"));
        }
    }
}