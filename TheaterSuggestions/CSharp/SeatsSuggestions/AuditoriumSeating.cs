using System.Collections.Generic;
using System.Linq;

namespace SeatsSuggestions
{
    public struct AuditoriumSeating
    {
        public IReadOnlyDictionary<string, Row> Rows => _rows;

        private readonly Dictionary<string, Row> _rows;

        public AuditoriumSeating(Dictionary<string, Row> rows)
        {
            _rows = rows;
        }

        public SeatingOptionSuggested SuggestSeatingOptionFor(int partyRequested, PricingCategory pricingCategory)
        {
            foreach (var row in _rows.Values)
            {
                var seatOptionsSuggested = row.SuggestSeatingOption(partyRequested, pricingCategory);

                if (seatOptionsSuggested.MatchExpectation())
                {
                    return seatOptionsSuggested;
                }
            }

            return new SeatingOptionNotAvailable(partyRequested, pricingCategory);
        }



        public override bool Equals(object obj)
        {
            var secondAuditoriumSeating = (AuditoriumSeating)obj;

            return this.Rows.Values.ToList().TrueForAll(row => secondAuditoriumSeating.Rows.Values.Contains(row));
        }
    }
}