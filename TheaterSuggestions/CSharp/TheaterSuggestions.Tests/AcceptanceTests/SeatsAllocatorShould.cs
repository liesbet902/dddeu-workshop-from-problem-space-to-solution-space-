﻿using ExternalDependencies.AuditoriumLayoutRepository;
using ExternalDependencies.ReservationsProvider;
using NFluent;
using NUnit.Framework;

namespace SeatsSuggestions.Tests.AcceptanceTests
{
    [TestFixture]
    public class SeatsAllocatorShould
    {
        [Test]
        public void Return_SeatsNotAvailable_when_Auditorium_has_all_its_seats_already_reserved()
        {
            const string showId = "5";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);
            Check.That(suggestionsMade.PartyRequested).IsEqualTo(partyRequested);
            Check.That(suggestionsMade.ShowId).IsEqualTo(showId);

            Check.That(suggestionsMade).IsInstanceOf<SuggestionNotAvailable>();
        }

        [Test]
        public void Suggest_one_seat_when_Auditorium_contains_one_available_seat_only()
        {
            const string showId = "1";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).ContainsExactly("A3");
        }

        [Test]
        public void Offer_several_suggestions_ie_1_per_PricingCategory_and_other_one_without_category_affinity()
        {
            const string showId = "18";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).Contains("A4", "A5", "A6").Only();
            Check.That(suggestionsMade.SeatNames(PricingCategory.Second)).Contains("A1", "A2", "A9").Only();
            Check.That(suggestionsMade.SeatNames(PricingCategory.Third)).Contains("E4", "E5", "E6").Only();

            Check.That(suggestionsMade.SeatNames(PricingCategory.Mixed)).Contains("A4", "A5", "A6").Only();
        }

        [Test]
        public void Offer_adjacent_seats_nearer_the_middle_of_a_row()
        {
            // FIX ME
            const string showId = "9";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).ContainsExactly("A4", "A3", "B5");
        }

        [Test]
        public void Offer_adjacent_seats_nearer_the_middle_of_a_row_when_it_is_possible()
        {
            // FIX ME
            const string showId = "3";
            const int partyRequested = 4;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).IsEmpty();
            Check.That(suggestionsMade.SeatNames(PricingCategory.Second))
                .ContainsExactly("C5-C6-C7-C8", "C1-C2-C3-C4", "D5-D6-D7-D8");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Third))
                .ContainsExactly("E5-E6-E7-E8", "E1-E2-E3-E4", "F5-F6-F7-F8");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Mixed))
                .ContainsExactly("A6-A7-A8-A9", "B1-B2-B3-B4", "C5-C6-C7-C8");
        }
    }
}