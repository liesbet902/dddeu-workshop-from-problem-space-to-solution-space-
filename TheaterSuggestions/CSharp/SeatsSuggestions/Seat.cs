namespace SeatsSuggestions
{
    public struct Seat
    {
        public Seat(string rowName, uint number, PricingCategory pricingCategory, SeatAvailability seatAvailability)
        {
            RowName = rowName;
            Number = number;
            PricingCategory = pricingCategory;
            SeatAvailability = seatAvailability;
        }

        public string RowName { get; }
        public uint Number { get; }
        public PricingCategory PricingCategory { get; }
        private SeatAvailability SeatAvailability { get; }

        public bool IsAvailable()
        {
            return SeatAvailability == SeatAvailability.Available;
        }

        public override string ToString()
        {
            return $"{RowName}{Number}";
        }

        public bool MatchCategory(PricingCategory pricingCategory)
        {
            if (pricingCategory == PricingCategory.Mixed)
            {
                return true;
            }

            return PricingCategory == pricingCategory;
        }

        public Seat Allocate()
        {
            if (SeatAvailability == SeatAvailability.Available)
            {
                return new Seat(this.RowName, this.Number, this.PricingCategory, SeatAvailability.Allocated);
                //SeatAvailability = SeatAvailability.Allocated;
            }

            return this;
        }

        //public override bool Equals(object obj)
        //{
        //    var secondSeat = (Seat)obj;
        //    return
        //        this.RowName.Equals(secondSeat.RowName) &&
        //        this.Number.Equals(secondSeat.Number) &&
        //        this.PricingCategory.Equals(secondSeat.PricingCategory);
        //}
    }
}