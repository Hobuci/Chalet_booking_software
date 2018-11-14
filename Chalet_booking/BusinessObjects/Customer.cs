using System;

namespace BusinessObjects
{
    [Serializable]
    public class Customer
    {
        /* Richard Borbely
         * Business Layer, Object storing customer information
         * Last modified: 04 December 2017
         * Design pattern interaction: Singleton is generating it's id
         */
        private string name; //name of customer
        private string address; //address of customer
        private int id; //id number of customer //auto from 1

        public string Name
        {
            get => name;
            set
            {
                if (value == "" || value == "Customer Name")
                {
                    throw new ArgumentException("Customer Name must be given!");
                }
                name = value;
            }
        }
        public string Address
        {
            get => address;
            set
            {
                if (value == "" || value == "Customer Address")
                {
                    throw new ArgumentException("Customer Address must be given!");
                }
                address = value;
            }
        }
        public int Id { get => id; set => id = value; }
    }
}
