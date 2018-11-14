using BusinessObjects.Tools.Decorator;
using System;
using System.Collections.Generic;

namespace BusinessObjects
{
    [Serializable]
    public class Booking : BookingCost
    {
        /* Richard Borbely
         * Business Layer, Object for storing booking information, with multiple function giving acces to its fields
         * Last modified: 04 December 2017
         * Design pattern interaction: Singleton is generating its reference number
         *                             Decorator is calculating its price
         */
        //base variables
        private DateTime arrivalDate; //booking arrival date
        private DateTime departureDate; //booking departure date
        private int referenceNo; //booking reference number //auto from 1
        private int chaletID; //booking chalet id //1-10
        private Customer customer; //customer object that is assigned to the booking
        private List<Guest> guestList = new List<Guest>(); //list for guest objects //max 6
        private double price; //price of booking
        //extras
        private bool hasEveningMeal; //stores booking evening meal extra
        private bool hasBreakfast; //stores booking breakfast extra
        private bool hasCarhire; //stores booking car hire extra
        private CarHire carHire; //object for car hire information


        public DateTime ArrivalDate
        {
            get => arrivalDate;
            set
            {
                if (value < DateTime.Today)
                {
                    throw new ArgumentException("Arrival date must be in the future.");
                }
                arrivalDate = value;
            }
        }
        public DateTime DepartureDate
        {
            get => departureDate;
            set
            {
                if (value < arrivalDate)
                {
                    throw new ArgumentException("Departure date must be after Arrival date.");
                }
                departureDate = value;
            }
        }
        public int ReferenceNo { get => referenceNo; set => referenceNo = value; }
        public int ChaletID
        {//Chalet IDs are only accepted in the range of 1-10
         //Also checking if the chalet is available at the given booking time
            get => chaletID;
            set
            {
                if (value < 1 || value > 10)
                {
                    throw new ArgumentException("Chalet ID is not within range. (1-10)");
                }
                chaletID = value;
            }
        }
        public Customer Customer { get => customer; set => customer = value; }
        public bool EveningMeal { get => hasEveningMeal; set => hasEveningMeal = value; }
        public bool Breakfast { get => hasBreakfast; set => hasBreakfast = value; }
        public bool Carhire { get => hasCarhire; set => hasCarhire = value; }
        public List<Guest> GuestList { get => guestList; set => guestList = value; }
        public double Price { get => price; set => price = value; }
        public CarHire CarHire { get => carHire; set => carHire = value; }

        public void AddGuest(Guest g)
        {//Add guest to the list
            if (GuestList.Count == 6)
            {
                throw new ArgumentException("Maximum number of guests (6) reached.");
            }
            GuestList.Add(g);
        }
        public void RemoveGuest(Guest g) { GuestList.Remove(g); }
        public Guest getGuest(string passportNo)
        {//Return requested guest from the list
            foreach (var guest in GuestList)
            {
                if (guest.PassportNo == passportNo)
                {
                    return guest;
                }
            }
            return null;
        }

        public double numberOfGuests()
        {//return number of guests
            return GuestList.Count;
        }
        public double numberOfNights()
        {//return number of booked nights
            return (departureDate - arrivalDate).TotalDays;
        }
        public double numberOfCarHireDays()
        {//return number of car hire days
            return (CarHire.HireEnd - CarHire.HireStart).TotalDays;
        }

        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Calculate the base price of a booking, any extras will be added to this
            double chaletPerNight = 60;
            double nightPerPerson = 25;
            bool hasExtras = (hasEveningMeal || hasBreakfast || hasCarhire);

            if (!hasExtras)
            {//if it has no extras, calculate and return base price only
             //if it has extras, the price that is returned already includes the applied extras
             //CalculateBasePrice() will force this by setting extras to false
                price = (nights * chaletPerNight)
                        + (people * nightPerPerson * nights);
            }

            return price;
        }
    }
}
