using System.Collections.Generic;

namespace SeatsSuggestions
{
    public struct Row
    {
        public string Name { get; }
        public List<Seat> Seats { get; }

        public Row(string name, List<Seat> seats)
        {
            Name = name;
            Seats = seats;
        }

        //public void AddSeat(Seat seat)
        //{
        //    Seats.Add(seat);
        //}

        public Row AddSeat(Seat seat)
        {
            var newSeats = new List<Seat>();// this.Seats.Add(seat);
            newSeats.AddRange(this.Seats);
            newSeats.Add(seat);
              
            return new Row(this.Name, newSeats);
        }

        public SeatingOptionSuggested SuggestSeatingOption(int partyRequested, PricingCategory pricingCategory)
        {
            foreach (var seat in Seats)
            {
                if (seat.IsAvailable() && seat.MatchCategory(pricingCategory))
                {
                    var seatAllocation = new SeatingOptionSuggested(partyRequested, pricingCategory);

                    seatAllocation.AddSeat(seat);

                    if (seatAllocation.MatchExpectation())
                    {
                        return seatAllocation;
                    }
                }
            }

            return new SeatingOptionNotAvailable(partyRequested, pricingCategory);
        }

        public override bool Equals(object obj)
        {
            var secondRow = (Row)obj;
            return this.Name.Equals(secondRow.Name) &&
                this.Seats.TrueForAll(seat => secondRow.Seats.Contains(seat));
        }
    }
}