using BusinessObjects;
using BusinessObjects.Tools;
using BusinessObjects.Tools.Decorator;
using DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class BookingTest
    {
        //Create test objects
        Booking testBooking = new Booking();
        Booking testBooking2 = new Booking();
        Customer testCustomer = new Customer();
        Customer testCustomer2 = new Customer();
        Guest testGuest = new Guest();
        Guest testGuest2 = new Guest();
        //Fill them up with some data
        public Booking createTestBooking()
        {
            testBooking.ArrivalDate = DateTime.Today;
            testBooking.DepartureDate = DateTime.Today.AddDays(1);
            testBooking.ChaletID = 1;

            testCustomer.Name = "Richard Borbely";
            testCustomer.Id = Singleton.Instance.getNextKey_customerID();
            testCustomer.Address = "Edinburgh";

            testBooking.Customer = testCustomer;
            testBooking.ReferenceNo = Singleton.Instance.getNextKey_bookingRefNo();

            return testBooking;
        }
        public Booking createTestBooking2()
        {
            testBooking2.ArrivalDate = DateTime.Today;
            testBooking2.DepartureDate = DateTime.Today.AddDays(5);
            testBooking2.ChaletID = 2;

            testCustomer.Name = "Klaudia Borbely";
            testCustomer.Id = Singleton.Instance.getNextKey_customerID();
            testCustomer.Address = "Edinburgh";

            testBooking2.Customer = testCustomer;
            testBooking2.ReferenceNo = Singleton.Instance.getNextKey_bookingRefNo();

            return testBooking2;
        }
        public Customer createTestCustomer()
        {
            testCustomer.Name = "Richard Borbely";
            testCustomer.Address = "Edinburgh";
            testCustomer.Id = Singleton.Instance.getNextKey_customerID();

            return testCustomer;
        }
        public Customer createTestCustomer2()
        {
            testCustomer2.Name = "Klaudia Borbely";
            testCustomer2.Address = "Edinburgh";
            testCustomer2.Id = Singleton.Instance.getNextKey_customerID();

            return testCustomer2;
        }
        public Guest createTestGuest()
        {
            testGuest.Name = "Richard Borbely";
            testGuest.PassportNo = "BPHDR6CGS";
            testGuest.Age = 24;

            return testGuest;
        }
        public Guest createTestGuest2()
        {
            testGuest2.Name = "Klaudia Borbely";
            testGuest2.PassportNo = "BPYCNS87S";
            testGuest2.Age = 20;

            return testGuest2;
        }

        [TestMethod]
        public void canStoreAndRetrieveBooking()
        {
            //create test Booking
            createTestBooking();

            //expected
            Booking expectedStoredBooking = testBooking;

            //run
            Bookings.AddBooking(testBooking);

            //test
            Assert.AreSame(expectedStoredBooking, Bookings.getBooking(testBooking), "Booking is not the same, store and retrieve FAILED.");
        }
        [TestMethod]
        public void canRemoveBooking_getBooking()
        {
            //create test Booking
            createTestBooking();

            //expected
            Booking expectedStoredBooking = null;

            //run
            Bookings.AddBooking(testBooking);
            Bookings.RemoveBooking(testBooking.ReferenceNo);

            //test
            Assert.AreSame(expectedStoredBooking, Bookings.getBooking(testBooking.ReferenceNo), "Booking retrieved, remove FAILED.");
        }
        [TestMethod]
        public void canRemoveBooking_listCount()
        {
            //create test Booking
            createTestBooking();
            createTestBooking2();

            //expected
            int expectedListCount = 1;

            //run
            Bookings.setBookingsList(new List<Booking>()); //reset the list
            Bookings.AddBooking(testBooking);
            Bookings.AddBooking(testBooking2);
            Bookings.RemoveBooking(testBooking.ReferenceNo);

            //test
            Assert.AreEqual(expectedListCount, Bookings.getBookingsList().Count, "List Count incorrect, remove FAILED.");
        }
        [TestMethod]
        public void canSetAndRetrieveBookingsList()
        {
            //create test Booking and test list
            List<Booking> testList = new List<Booking>();
            createTestBooking();
            createTestBooking2();

            testList.Add(testBooking);
            testList.Add(testBooking2);

            //expected
            List<Booking> expectedList = testList;

            //run
            Bookings.setBookingsList(testList); //reset the list

            //test
            Assert.AreSame(expectedList, Bookings.getBookingsList(), "List not the same, Set and Retrieve FAILED.");
        }
        [TestMethod]
        public void isChaletTakenTrue()
        {
            //create test Bookings
            createTestBooking(); //from today to +1 day
            createTestBooking2(); //from today to +5 day

            testBooking2.ChaletID = 1; //set to same chalet ID

            Bookings.AddBooking(testBooking);
            Bookings.AddBooking(testBooking2);

            //expected
            //they overlap with same chalet ID, function is expected to throw an exception containing 'Chalet'


            //test
            try
            {
                Bookings.isChaletTaken(testBooking.ChaletID, testBooking.ArrivalDate, testBooking.DepartureDate);
                //Assert.Fail("Assertion failed, isChaletTaken function FAILED");
            }
            catch (Exception expectedException)
            {
                if (expectedException.Message.Contains("Chalet"))
                {
                    //PASSED
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("Assertion failed, isChaletTaken function FAILED");
                }
            }
        }
        [TestMethod]
        public void isChaletTakenFalse()
        {
            //create test Booking and test list
            createTestBooking(); //from today to +1 day
            createTestBooking2(); //from today to +5 day

            testBooking2.ChaletID = 1; //set to same chalet ID
            testBooking2.ArrivalDate = DateTime.Today.AddDays(2); // set arrival date so they dont overlap

            Bookings.AddBooking(testBooking);
            Bookings.AddBooking(testBooking2);

            //expected
            //chalet ids match but date dont overlap, function will not throw any error

            //test
            try
            {
                Bookings.isChaletTaken(testBooking.ChaletID, testBooking.ArrivalDate, testBooking.DepartureDate);
            }
            catch (Exception expectedException)
            {
                if (expectedException.Message.Contains("Chalet"))
                {
                    //PASSED
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("Assertion failed, isChaletTaken funtion FAILED");
                }
            }
        }
        [TestMethod]
        public void canStoreAndRetrieveCustomer()
        {
            //create test Customer
            createTestCustomer();

            //expected
            Customer expectedCustomer = testCustomer;

            //run
            Bookings.AddCustomer(testCustomer);

            //test
            Assert.AreSame(expectedCustomer, Bookings.getCustomer(testCustomer.Name, testCustomer.Address), "Customer is not the same, store and retrieve FAILED.");

        }
        [TestMethod]
        public void canRemoveCustomer_getCustomer()
        {
            //create test Customer
            createTestCustomer();

            //expected
            Customer expectedStoredCustomer = null;

            //run
            Bookings.AddCustomer(testCustomer);
            Bookings.RemoveCustomer(testCustomer.Id);

            //test
            Assert.AreSame(expectedStoredCustomer, Bookings.getCustomer(testCustomer.Id), "Customer retrieved, remove FAILED.");
        }
        [TestMethod]
        public void canRemoveCustomer_listCount()
        {
            //create test Customer
            createTestCustomer();
            createTestCustomer2();

            //expected
            int expectedListCount = 1;

            //run
            Bookings.setCustomersList(new List<Customer>()); //reset the list
            Bookings.AddCustomer(testCustomer);
            Bookings.AddCustomer(testCustomer2);
            Bookings.RemoveCustomer(testCustomer.Id);

            //test
            Assert.AreEqual(expectedListCount, Bookings.getCustomerList().Count, "List Count incorrect, remove FAILED.");
        }
        [TestMethod]
        public void canSetAndRetrieveCustomerList()
        {
            //create test Customer and test list
            List<Customer> testList = new List<Customer>();
            createTestCustomer();
            createTestCustomer2();

            testList.Add(testCustomer);
            testList.Add(testCustomer2);

            //expected
            List<Customer> expectedList = testList;

            //run
            Bookings.setCustomersList(testList); //reset the list

            //test
            Assert.AreSame(expectedList, Bookings.getCustomerList(), "List not the same, Set and Retrieve FAILED.");
        }
        [TestMethod]
        public void hasBookingTrue()
        {
            //reset lists
            Bookings.setBookingsList(new List<Booking>());
            Bookings.setCustomersList(new List<Customer>());

            //create test Booking
            createTestBooking();
            //store test booking
            Bookings.AddBooking(testBooking);

            //expected
            Booking expectedResult = testBooking;

            //test
            Assert.AreSame(Bookings.hasBooking(testCustomer), expectedResult, "hasBooking function FAILED.");

        }
        [TestMethod]
        public void hasBookingFalse()
        {
            //reset lists
            Bookings.setBookingsList(new List<Booking>());
            Bookings.setCustomersList(new List<Customer>());

            //create test Booking
            createTestBooking();
            //store test booking
            Bookings.AddBooking(testBooking);
            //create another customer and add to the list, check if this new customer has a booking
            createTestCustomer2();

            //expected
            Booking expectedResult = null;

            //test
            Assert.AreSame(Bookings.hasBooking(testCustomer2), expectedResult, "hasBooking function FAILED.");

        }
        [TestMethod]
        public void canStoreAndRetrieveGuest()
        {
            //create test booking and add it to the list
            createTestBooking();
            Bookings.AddBooking(testBooking);
            //create test Guest
            createTestGuest();

            //expected
            Guest expectedStoredGuest = testGuest;

            //run
            Bookings.AddGuest(testBooking, testGuest);

            //test
            Assert.AreSame(expectedStoredGuest, Bookings.getGuest(testBooking, testGuest.PassportNo), "Guest is not the same, store and retrieve FAILED.");
        }
        [TestMethod]
        public void canRemoveGuest_getGuest()
        {
            //create test booking and add it to the list
            createTestBooking();
            Bookings.AddBooking(testBooking);
            //create test Guest 
            createTestGuest();

            //expected
            Guest expectedStoredGuest = null;

            //run
            Bookings.AddGuest(testBooking, testGuest);
            Bookings.RemoveGuest(testBooking, testGuest);

            //test
            Assert.AreSame(expectedStoredGuest, Bookings.getGuest(testBooking, testGuest.PassportNo), "Guest retrieved, remove FAILED.");
        }
        [TestMethod]
        public void canRemoveGuest_listCount()
        {
            //create test booking and add it to the list
            createTestBooking();
            Bookings.AddBooking(testBooking);
            //create test Guest
            createTestGuest();
            createTestGuest2();

            //expected
            int expectedListCount = 1;

            //run
            Bookings.setGuestList(testBooking, new List<Guest>()); //reset the list
            Bookings.AddGuest(testBooking, testGuest);
            Bookings.AddGuest(testBooking, testGuest2);
            Bookings.RemoveGuest(testBooking, testGuest);

            //test
            Assert.AreEqual(expectedListCount, Bookings.getGuestList(testBooking).Count, "List Count incorrect, remove FAILED.");
        }
        [TestMethod]
        public void doesSingletonCounterWork()
        {
            //create variables to store booking reference and customer ID
            //get their next value and count from there
            int bookingRef = Singleton.Instance.getNextKey_bookingRefNo();
            int customerID = Singleton.Instance.getNextKey_customerID();

            //expected
            int expectedBookingRef = bookingRef + 15;
            int expectedCustomerID = customerID + 10;

            for (int i = 0; i < 15; i++)
            {
                bookingRef = Singleton.Instance.getNextKey_bookingRefNo();
            }
            Assert.AreEqual(expectedBookingRef, bookingRef, "Singleton Counter did not work properly.");

            for (int i = 0; i < 10; i++)
            {
                customerID = Singleton.Instance.getNextKey_customerID();
            }
            Assert.AreEqual(expectedCustomerID, customerID, "Singleton Counter did not work properly.");
        }
        [TestMethod]
        public void canCalculateBasePrice_1()
        {
            //create test Booking
            createTestBooking();

            //expected
            //60(chalet per night) * 1(number of nights)
            double expectedPrice = 60;

            //run
            double actualPrice = testBooking.CalculatePrice(testBooking.numberOfGuests(), testBooking.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_2()
        {
            //create test Booking
            createTestBooking2();

            //expected
            //60(chalet per night) * 5(number of nights)
            double expectedPrice = 300;

            //run
            double actualPrice = testBooking.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_3()
        {//calculate with people
            //create test Booking and test guests
            createTestBooking();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking.AddGuest(testGuest);
            testBooking.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 1(number of nights)) + (2(people) * 1(number of nights) * 25(night per person))
            double expectedPrice = 110;

            //run
            double actualPrice = testBooking.CalculatePrice(testBooking.numberOfGuests(), testBooking.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_4()
        {//calculate with people
            //create test Booking and test guests
            createTestBooking2();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking.AddGuest(testGuest);
            testBooking.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 5(number of nights)) + (2(people) * 5(number of nights) * 25(night per person))
            double expectedPrice = 300;

            //run
            double actualPrice = testBooking2.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_5()
        {//add breakfast
            //calculate with people
            //create test Booking and test guests
            createTestBooking();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking.AddGuest(testGuest);
            testBooking.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 5(number of nights)) + (2(people) * 5(number of nights) * 25(night per person)) 
            //+ 5(breakfast price) * 5(number of nights) * 2(people)
            double expectedPrice = 230;

            //run
            double actualPrice = testBooking.CalculatePrice(testBooking.numberOfGuests(), testBooking.numberOfNights(), 0); //baseprice

                //add extra
            testBooking.Breakfast = true;
            Breakfast_add breakfast = new Breakfast_add();
            breakfast.AddTo(testBooking);
            actualPrice += breakfast.CalculatePrice(testBooking.numberOfGuests(), testBooking.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_6()
        {//add breakfast for more days
            //calculate with people
            //create test Booking and test guests
            createTestBooking2();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking2.AddGuest(testGuest);
            testBooking2.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 5(number of nights)) + (2(people) * 5(number of nights) * 25(night per person)) 
            //+ 5(breakfast price) * 5(number of nights) * 2(people)
            double expectedPrice = 1150;

            //run
            double actualPrice = testBooking2.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //baseprice

            //add extra
            testBooking2.Breakfast = true;
            Breakfast_add breakfast = new Breakfast_add();
            breakfast.AddTo(testBooking);
            actualPrice += breakfast.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_7()
        {//add evening meal
            //calculate with people
            //create test Booking and test guests
            createTestBooking();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking.AddGuest(testGuest);
            testBooking.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 5(number of nights)) + (2(people) * 5(number of nights) * 25(night per person)) 
            //+ 10(evening meal price) * 5(number of nights) * 2(people)
            double expectedPrice = 240;

            //run
            double actualPrice = testBooking.CalculatePrice(testBooking.numberOfGuests(), testBooking.numberOfNights(), 0); //baseprice

            //add extra
            testBooking.EveningMeal = true;
            EveningMeal_add eveningMeal = new EveningMeal_add();
            eveningMeal.AddTo(testBooking);
            actualPrice += eveningMeal.CalculatePrice(testBooking.numberOfGuests(), testBooking.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_8()
        {//add evening meal for more days
            //calculate with people
            //create test Booking and test guests
            createTestBooking2();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking2.AddGuest(testGuest);
            testBooking2.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 5(number of nights)) + (2(people) * 5(number of nights) * 25(night per person)) 
            //+ 10(evening meal price) * 5(number of nights) * 2(people)
            double expectedPrice = 1200;

            //run
            double actualPrice = testBooking2.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //baseprice

            //add extra
            testBooking2.EveningMeal = true;
            EveningMeal_add eveningMeal = new EveningMeal_add();
            eveningMeal.AddTo(testBooking);
            actualPrice += eveningMeal.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
        [TestMethod]
        public void canCalculateBasePrice_9()
        {//add evening meal AND breakfast for more days
            //calculate with people
            //create test Booking and test guests
            createTestBooking2();
            createTestGuest();
            createTestGuest2();
            //add guests to the guest list of the booking
            testBooking2.AddGuest(testGuest);
            testBooking2.AddGuest(testGuest2);

            //expected
            //(60(chalet per night) * 5(number of nights)) + (2(people) * 5(number of nights) * 25(night per person)) 
            //+ 10(evening meal price) * 5(number of nights) * 2(people)
            //+ 5(breakfast price) * 5(number of nights) * 2(people)
            double expectedPrice = 1800;

            //run
            double actualPrice = testBooking2.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //baseprice

            //add extra evening meal
            testBooking2.EveningMeal = true;
            EveningMeal_add eveningMeal = new EveningMeal_add();
            eveningMeal.AddTo(testBooking);
            actualPrice += eveningMeal.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //add extra breakfast
            testBooking2.Breakfast = true;
            Breakfast_add breakfast = new Breakfast_add();
            breakfast.AddTo(testBooking);
            actualPrice += breakfast.CalculatePrice(testBooking2.numberOfGuests(), testBooking2.numberOfNights(), 0); //carhire days is 0, only calculating base price

            //test
            Assert.AreEqual(expectedPrice, actualPrice, "Booking price is not correct, price calculation FAILED.");
        }
    }
}
