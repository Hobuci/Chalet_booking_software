using BusinessObjects;
using System;
using System.Windows;

namespace Presentation
{
    /* Richard Borbely
     * Presentation Layer, Interactions with the user, Car hire details input
     * Last modified: 04 December 2017
     * 
     */
    public partial class CarHireDetails : Window
    {
        public CarHireDetails(Booking selectedBookingFromMainWindow) //takes the selected booking object as argument
        {
            InitializeComponent();
            selectedBooking = selectedBookingFromMainWindow;
        }
        Booking selectedBooking;

        private void btn_saveCarHire_Click(object sender, RoutedEventArgs e)
        {//Save Carhire details in booking object and close window
            try
            {
                CarHire carhire = new CarHire(selectedBooking.ArrivalDate, selectedBooking.DepartureDate);
                carhire.DriverName = txtb_driverName.Text;
                carhire.HireStart = datepicker_carHireStart.SelectedDate.Value.Date;
                carhire.HireEnd = datepicker_carHireEnd.SelectedDate.Value.Date;

                selectedBooking.CarHire = carhire;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtb_driverName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_driverName.Text == "Driver Name")
            {
                txtb_driverName.Text = "";
            }
        }
        private void txtb_driverName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_driverName.Text == "")
            {
                txtb_driverName.Text = "Driver Name";
            }
        }
    }
}
