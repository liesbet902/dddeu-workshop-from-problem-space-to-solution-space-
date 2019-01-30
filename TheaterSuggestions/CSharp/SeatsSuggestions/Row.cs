using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace SeatsSuggestions
{
    public class Row : ValueType<Row>
    {
        public string Name { get; }
        public List<Seat> Seats { get; }

        public Row(string name, List<Seat> seats)
        {
            Name = name;
            Seats = SetPreferencesToSeats(seats);
        }

        private List<Seat> SetPreferencesToSeats(List<Seat> seats)
        {
            var length = seats.Count();
            var middle = (length + 1) / 2;
            var seatsByPreference = seats
                .Select(seat => seat.SetPreference(Math.Abs(middle - seat.Number)));
            return seatsByPreference.ToList();
        }

        public Row AddSeat(Seat seat)
        {
            var updatedList = Seats.Select(s => s == seat ? seat : s).ToList();

            return new Row(Name, SetPreferencesToSeats(updatedList));
        }

        public SeatingOptionSuggested SuggestSeatingOption(SuggestionRequest suggestionRequest)
        {
            var seatingOptionSuggested = new SeatingOptionSuggested(suggestionRequest);

            var availableSeats = SelectAvailableSeatsCompliantWith(suggestionRequest.PricingCategory);
            var orderedSeats = availableSeats.OrderBy(seat => seat.PreferenceScore);
            foreach (var seat in orderedSeats)
            {
                //if IsneighborAtLeastOne(seatingOptionSuggested)
                seatingOptionSuggested.AddSeat(seat);

                if (seatingOptionSuggested.MatchExpectation())
                {
                    return seatingOptionSuggested;
                }
            }

            return new SeatingOptionNotAvailable(suggestionRequest);
        }

        private IEnumerable<Seat> SelectAvailableSeatsCompliantWith(PricingCategory pricingCategory)
        {
            return Seats
                .Where(s => s.IsAvailable() && s.MatchCategory(pricingCategory));
        }

        public Row Allocate(Seat seat)
        {
            var newVersionOfSeats = new List<Seat>();

            foreach (var currentSeat in Seats)
            {
                if (currentSeat.SameSeatLocation(seat))
                {
                    newVersionOfSeats.Add(new Seat(seat.RowName, seat.Number, seat.PricingCategory,
                        SeatAvailability.Allocated));
                }
                else
                {
                    newVersionOfSeats.Add(currentSeat);
                }
            }

            return new Row(seat.RowName, newVersionOfSeats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {Name, new ListByValue<Seat>(Seats)};
        }
    }
}