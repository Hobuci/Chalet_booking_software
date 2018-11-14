namespace BusinessObjects.Tools
{
    public class Singleton
    {
        /* Richard Borbely
         * Business Layer, Singleton design pattern, handling generation of Booking reference numbers and Customer ID's
         * Last modified: 04 December 2017
         *               
         */
        private static Singleton instance;
        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }

        }

        private int booking_referenceNo = 0;
        private int customerNo = 0;

        public int getNextKey_bookingRefNo()
        {
            booking_referenceNo++;
            return booking_referenceNo;
        }

        public int getNextKey_customerID()
        {
            customerNo++;
            return customerNo;
        }
    }
}
