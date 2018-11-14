using System;

namespace BusinessObjects
{
    [Serializable]
    public class Guest
    {
        /* Richard Borbely
         * Business Layer, Object storing guest information
         * Last modified: 04 December 2017
         * 
         */
        private string name; //name of guest
        private string passportNo; //passport number of guest //max 10 length
        private int age; //age of guest //0-101

        public string Name { get => name; set => name = value; }
        public string PassportNo
        {
            get => passportNo;
            set
            {
                if (value.Length > 10)
                {
                    throw new ArgumentException("Passport number length invalid. (<10)");
                }
                passportNo = value;
            }
        }
        public int Age
        {
            get => age;
            set
            {
                if (value < 0 || value > 101)
                {
                    throw new ArgumentException("Age input invalid. (0-101)");
                }
                age = value;
            }
        }
    }
}
