using ExternalDependencies.AuditoriumLayoutRepository;
using ExternalDependencies.ReservationsProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeatsSuggestions.Tests
{
    public class SeatAllocator
    {
        public SeatAllocator(ReservationsProvider reservations, AuditoriumLayoutRepository auditoriumLayout)
        {
            Reservations = reservations;
            AuditoriumLayout = auditoriumLayout;
        }

        private ReservationsProvider Reservations { get; }

        private AuditoriumLayoutRepository AuditoriumLayout { get; }
   
        internal SuggestionMade MakeSuggestions(string showId, int party)
        {
            var layout = AuditoriumLayout.GetAuditoriumLayoutFor(showId);
            var seat = layout.Rows["A"][0];

            var reservations = Reservations.GetReservedSeats(showId);

            var suggestedSeats = new List<Seat>();
            if (!reservations.ReservedSeats.Contains(seat.Name))
                suggestedSeats.Add(new Seat(seat.Name));

            return new SuggestionMade()
            {
                Seats = suggestedSeats
            };
        }
    }

    internal class SuggestionMade
    {
        public List<Seat> Seats { get; set; }
    }

    public struct Seat
    {
        public string SeatLocation { get; set; }

        public Seat(string seatLocation)
        {
            SeatLocation = seatLocation;
        }
    }
}
