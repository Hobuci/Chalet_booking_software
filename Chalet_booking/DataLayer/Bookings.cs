using BusinessObjects;
using System;
using System.Collections.Generic;

namespace DataLayer
{
    [Serializable]
    public static class Bookings
    {
        /* Richard Borbely
         * Data Layer, Handles all data, Bookings and Customers are stored in lists
         * Multiple functions for accessing Bookings, Customers, Guests and their list
         * Last modified: 04 December 2017
         * Design pattern interaction: Decorator, Singleton
         */
        
        //List for Bookings and Customers
        private static List<Booking> Bookings_List = new List<Booking>();
        private static List<Customer> Customers_List = new List<Customer>();
        //Booking
        public static void AddBooking(Booking b)
        {//Add booking to the list
            Bookings_List.Add(b);
        }
        public static void RemoveBooking(int refNo)
        {//Remove booking from the list
            foreach (var booking in Bookings_List)
            {
                if (booking.ReferenceNo == refNo)
                {
                    Bookings_List.Remove(booking);
                    return;
                }
            }
        }
        public static Booking getBooking(int requested_refNo)
        {//Return Requested booking
            foreach (var booking in Bookings_List)
            {
                if (booking.ReferenceNo == requested_refNo)
                {
                    return booking;
                }
            }
            return null;
        }
        public static Booking getBooking(Booking b)
        {//Return Requested booking
            foreach (var booking in Bookings_List)
            {
                if (booking == b)
                {
                    return booking;
                }
            }
            return null;
        }
        public static List<Booking> getBookingsList()
        {//Return the whole list
            return Bookings_List;
        }
        public static void setBookingsList(List<Booking> bookings)
        {//Set Bookings list
            Bookings_List = bookings;
        }
        public static void isChaletTaken(int chaletID, DateTime arrival, DateTime departure)
        {//Iterates through the Bookings list looking for the given chalet, then checking if their dates overlap or not
            foreach (var booking in Bookings_List)
            {
                if (booking.ChaletID == chaletID)
                {//match chalet
                    if (arrival < booking.DepartureDate && booking.ArrivalDate < departure)
                    {//booking overlap
                        //throw error, displaying the date when the chalet is taken
                        throw new Exception(String.Format("Chalet {0} is taken from {1} until {2}, please choose another one.",
                            booking.ChaletID, booking.ArrivalDate.ToShortDateString(), booking.DepartureDate.ToShortDateString()));
                    }
                }
            }
        }
        //Customer
        public static void AddCustomer(Customer c)
        {//Add customer to the list
            Customers_List.Add(c);
        }
        public static void RemoveCustomer(int ID)
        {//Remove customer from the list
            foreach (var customer in Customers_List)
            {
                if (customer.Id == ID)
                {
                    Customers_List.Remove(customer);
                    return;
                }
            }
        }
        public static Customer getCustomer(string name, string address)
        {//Return existing customer object
            foreach (var customer in Customers_List)
            {
                if (customer.Name == name && customer.Address == address)
                {
                    return customer;
                }
            }
            return null;
        }
        public static Customer getCustomer(int ID)
        {//Return existing customer object
            foreach (var customer in Customers_List)
            {
                if (customer.Id == ID)
                {
                    return customer;
                }
            }
            return null;
        }
        public static Booking hasBooking(Customer c)
        {//returns true if the customer has a booking, false if not
            foreach (var booking in Bookings_List)
            {
                if (booking.Customer == c)
                {
                    return booking;
                }
            }
            return null;
        }
        public static List<Customer> getCustomerList()
        {//Return the whole list
            return Customers_List;
        }
        public static void setCustomersList(List<Customer> customers)
        {//Set Customers list
            Customers_List = customers;
        }
        //Guest
        public static void AddGuest(Booking b, Guest g)
        {//Add guest to the list
            getBooking(b).AddGuest(g);
        }
        public static void RemoveGuest(Booking b, Guest g)
        {//Remove guest from the list
            getBooking(b).RemoveGuest(g);
        }
        public static Guest getGuest(Booking b, string passportNo)
        {
            return getBooking(b).getGuest(passportNo);
        }
        public static List<Guest> getGuestList(Booking b)
        {//Return the guest list for the booking
            return getBooking(b).GuestList;
        }
        public static void setGuestList(Booking b, List<Guest> guestList)
        {
            getBooking(b).GuestList = guestList;
        }
    }
}
