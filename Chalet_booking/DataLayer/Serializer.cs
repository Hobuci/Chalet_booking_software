using DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BusinessObjects.Tools
{
    public static class Serializer
    {
        /* Richard Borbely
         * Data Layer, Persistance, Serializes bookings and customers list to a file, and de-serializes to load the data back to the program
         * Last modified: 04 December 2017
         * 
         */
        public static void SaveToFile()
        {//Serialize all Bookings and Customers
            //create file for bookings
            FileStream stream = File.Create("bookings.dat");
            BinaryFormatter formatter = new BinaryFormatter();
            //serialize bookings into the file
            formatter.Serialize(stream, Bookings.getBookingsList());
            //create file for customers
            stream = File.Create("customers.dat");
            //serialize customers into the file
            formatter.Serialize(stream, Bookings.getCustomerList());
            //close the filestream
            stream.Close();
        }

        public static void LoadFromFile()
        {//De-serialize all Bookings and Customers from files

            if (File.Exists("bookings.dat") && File.Exists("customers.dat"))
            {//check if the files exist
                FileStream stream = File.OpenRead("bookings.dat");
                BinaryFormatter formatter = new BinaryFormatter();

                List<Booking> bookings = (List<Booking>)formatter.Deserialize(stream);

                //load
                Bookings.setBookingsList(bookings);

                //set stream
                stream = File.OpenRead("customers.dat");
                List<Customer> customers = (List<Customer>)formatter.Deserialize(stream);

                //load
                Bookings.setCustomersList(customers);

                stream.Close();
            }
        }
    }
}
