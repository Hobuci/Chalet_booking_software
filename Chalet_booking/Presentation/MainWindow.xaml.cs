using BusinessObjects;
using BusinessObjects.Tools;
using BusinessObjects.Tools.Decorator;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Presentation
{
    /* Richard Borbely
     * Presentation Layer, Interactions with the user, Data input and output.
     * Last modified: 04 December 2017
     * Design pattern interaction: Decorator, Singleton
     */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Tools
        public enum Target { All, Booking, Customer, Guest, BookingsList, CustomersList, GuestList, Cost, CarHireDetails}
        private void mainwindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {//When user is closing the program
            //Call serializer to save data to file
            Serializer.SaveToFile();
        }
        private void mainwindow_Initialized(object sender, EventArgs e)
        {//When the main window loaded up
            //Call serializer to load data from the file
            Serializer.LoadFromFile();
            //refresh GUI
            refreshList(Target.BookingsList);
            refreshList(Target.CustomersList);
        }
        private int getSelectedKey_BookingRef()
        {//Returns the reference number from the bookings list SELECTED ITEM which is in a format of "Ref# [{0}],  {1} - {2}"
            string[] selected_refNo = listbox_bookingsList.SelectedItem.ToString().Split(new char[] { '[', ']' });
            return Int32.Parse(selected_refNo[1]);
        }
        private int getSelectedKey_CustomerID()
        {//Returns the ID from the vustomer list SELECTED ITEM which is in a format of "ID# [{0}],  {1}"
            string[] selected_ID = listbox_customerList.SelectedItem.ToString().Split(new char[] { '[', ']' });
            return Int32.Parse(selected_ID[1]);
        }
        private string getSelectedKey_GuestPassNo()
        {//Returns the passport number from the guest list SELECTED ITEM which is in a format of "{1} - [2]"
            string[] selected_ID = listbox_guestList.SelectedItem.ToString().Split(new char[] { '[', ']' });
            return selected_ID[1];
        }
        private void refreshList(Target target)
        {
            switch (target)
            {
                case Target.BookingsList:
                    //Refresh GUI Bookings list - wouldn't be very efficient with a massive list, but ensures that the GUI and the actual list are synchronised
                    //Clear GUI list
                    listbox_bookingsList.Items.Clear();
                    //Add each booking to the list
                    foreach (var booking in Bookings.getBookingsList())
                    {
                        listbox_bookingsList.Items.Add(String.Format("Ref# [{0}],  {1} - {2}", booking.ReferenceNo, booking.ArrivalDate.ToShortDateString(), booking.DepartureDate.ToShortDateString())); //add booking to GUI list
                    }
                    break;
                case Target.CustomersList:
                    //Refresh GUI Customer list
                    //Clear GUI list
                    listbox_customerList.Items.Clear();
                    //Add each customer to the list
                    foreach (var customer in Bookings.getCustomerList())
                    {
                        listbox_customerList.Items.Add(String.Format("ID# [{0}],  {1}", customer.Id, customer.Name)); //add customer to GUI list
                    }
                    break;
                case Target.GuestList:
                    //Refresh GUI Guest list
                    //Clear GUI list
                    listbox_guestList.Items.Clear();
                    Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
                    //Add each guest to the list
                    foreach (var guest in Bookings.getGuestList(selectedBooking))
                    {
                        listbox_guestList.Items.Add(String.Format("{0} - [{1}]", guest.Name, guest.PassportNo)); //add customer to GUI list
                    }
                    break;
            }
        }
        public void resetFields(Target target)
        {
            switch (target)
            {
                case Target.All:
                    datepicker_arrivalDate.SelectedDate = null;
                    datepicker_departureDate.SelectedDate = null;
                    txtb_chaletID.Text = "Chalet ID";
                    txtb_customerName.Text = "Customer Name";
                    txtb_customerAddress.Text = "Customer Address";
                    lbl_bookingRef.Content = "#Ref";
                    lbl_customerID.Content = "#ID";
                    txtb_guestName.Text = "Guest Name";
                    txtb_guestPassNo.Text = "Passport No.";
                    txtb_guestAge.Text = "Age";
                    btn_extra_eveningMeal.IsEnabled = false;
                    btn_extra_breakfast.IsEnabled = false;
                    btn_extra_carHire.IsEnabled = false;
                    btn_extra_eveningMeal_Remove.IsEnabled = false;
                    btn_extra_breakfast_Remove.IsEnabled = false;
                    btn_extra_carHire_Remove.IsEnabled = false;
                    lbl_eveningMeal_cost.Content = "Evening Meal = ";
                    lbl_breakfast_cost.Content = "Breakfast = ";
                    lbl_carHire_cost.Content = "Car Hire = ";
                    break;
                case Target.Guest:
                    txtb_guestName.Text = "Guest Name";
                    txtb_guestPassNo.Text = "Passport No.";
                    txtb_guestAge.Text = "Age";
                    break;
                case Target.Cost:
                    lbl_eveningMeal_cost.Content = "Evening Meal = ";
                    lbl_breakfast_cost.Content = "Breakfast = ";
                    lbl_carHire_cost.Content = "Car Hire = ";
                    txtb_cost.Text = "Holiday Chalet Booking Receipt" + System.Environment.NewLine + System.Environment.NewLine;
                    lbl_eveningMeal_cost.Visibility = Visibility.Hidden;
                    lbl_breakfast_cost.Visibility = Visibility.Hidden;
                    lbl_carHire_cost.Visibility = Visibility.Hidden;
                    break;
                case Target.CarHireDetails:
                    lbl_CarHire.Visibility = Visibility.Hidden;
                    txtb_CarHire.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void carHire_show(Booking selectedBooking)
        {//Display Car Hire details of a booking
            lbl_CarHire.Visibility = Visibility.Visible;
            txtb_CarHire.Visibility = Visibility.Visible;
            string carHireDetailsText = String.Format("Driver Name:  {0}/nStart Date:  {1}/nEnd Date:  {2}", selectedBooking.CarHire.DriverName, 
                selectedBooking.CarHire.HireStart.ToShortDateString(), selectedBooking.CarHire.HireEnd.ToShortDateString());
            carHireDetailsText = carHireDetailsText.Replace("/n", System.Environment.NewLine);
            txtb_CarHire.Text = carHireDetailsText;
        }
        //Amend mode
        private void Amend_Enter(Target target)
        {//Amend mode
            //Save Changes button will appear, overlapping the Add Button for both booking and customer, GUI changes so user can clearly see what is being amended
            switch (target)
            {
                case Target.Booking:
                    btn_amendBooking.Visibility = Visibility.Visible;
                    btn_deleteBooking.Visibility = Visibility.Visible;
                    btn_addCustomer.Visibility = Visibility.Hidden;
                    btn_deleteCustomer.Visibility = Visibility.Hidden;
                    btn_addGuest.Visibility = Visibility.Visible;
                    txtb_guestName.IsEnabled = true;
                    txtb_guestPassNo.IsEnabled = true;
                    txtb_guestAge.IsEnabled = true;
                    rect_booking.Stroke = new SolidColorBrush(Colors.Green);
                    rect_booking.StrokeThickness = 3;
                    rect_customer.Stroke = new SolidColorBrush(Colors.Green);
                    rect_customer.StrokeThickness = 3;
                    rect_guest.Stroke = new SolidColorBrush(Colors.Green);
                    rect_guest.StrokeThickness = 3;
                    break;
                case Target.Customer:
                    btn_amendCustomer.Visibility = Visibility.Visible;
                    btn_deleteCustomer.Visibility = Visibility.Visible;
                    btn_addBooking.Visibility = Visibility.Hidden;
                    btn_deleteBooking.Visibility = Visibility.Hidden;
                    rect_customer.Stroke = new SolidColorBrush(Colors.Green);
                    rect_customer.StrokeThickness = 3;
                    break;
                case Target.Guest:
                    btn_addBooking.Visibility = Visibility.Hidden;
                    btn_amendBooking.Visibility = Visibility.Hidden;
                    btn_deleteBooking.Visibility = Visibility.Hidden;
                    btn_amendGuest.Visibility = Visibility.Visible;
                    btn_deleteGuest.Visibility = Visibility.Visible;
                    rect_guest.Stroke = new SolidColorBrush(Colors.Green);
                    rect_guest.StrokeThickness = 3;
                    break;
            }
        }
        private void Amend_Exit(Target target)
        {//Amend mode
         //Exit this state, return to default GUI look
            switch (target)
            {
                case Target.Booking:
                    btn_amendBooking.Visibility = Visibility.Hidden;
                    btn_addBooking.Visibility = Visibility.Visible;
                    btn_addCustomer.Visibility = Visibility.Visible;
                    btn_deleteCustomer.Visibility = Visibility.Visible;
                    btn_addGuest.Visibility = Visibility.Hidden;
                    txtb_guestName.IsEnabled = false;
                    txtb_guestPassNo.IsEnabled = false;
                    txtb_guestAge.IsEnabled = false;
                    rect_booking.Stroke = new SolidColorBrush(Colors.Black);
                    rect_booking.StrokeThickness = 1;
                    rect_customer.Stroke = new SolidColorBrush(Colors.Black);
                    rect_customer.StrokeThickness = 1;
                    rect_guest.Stroke = new SolidColorBrush(Colors.Black);
                    rect_guest.StrokeThickness = 1;
                    listbox_guestList.Items.Clear(); //clear up guest list because there is no booking selected
                    txtb_price.Text = "TOTAL   £"; //display no price when there is no booking selected
                    txtb_cost.Text = "Holiday Chalet Booking Receipt" + System.Environment.NewLine + System.Environment.NewLine;
                    lbl_eveningMeal_cost.Visibility = Visibility.Hidden;
                    lbl_breakfast_cost.Visibility = Visibility.Hidden;
                    lbl_carHire_cost.Visibility = Visibility.Hidden;
                    lbl_CarHire.Visibility = Visibility.Hidden;
                    txtb_CarHire.Visibility = Visibility.Hidden;
                    break;
                case Target.Customer:
                    btn_amendCustomer.Visibility = Visibility.Hidden;
                    btn_deleteCustomer.Visibility = Visibility.Hidden;
                    btn_addBooking.Visibility = Visibility.Visible;
                    btn_deleteBooking.Visibility = Visibility.Visible;
                    rect_customer.Stroke = new SolidColorBrush(Colors.Black);
                    rect_customer.StrokeThickness = 1;
                    break;
                case Target.Guest:
                    btn_addBooking.Visibility = Visibility.Visible;
                    btn_amendBooking.Visibility = Visibility.Hidden;
                    btn_deleteBooking.Visibility = Visibility.Visible;
                    btn_amendGuest.Visibility = Visibility.Hidden;
                    btn_deleteGuest.Visibility = Visibility.Hidden;
                    rect_guest.Stroke = new SolidColorBrush(Colors.Black);
                    rect_guest.StrokeThickness = 1;
                    break;
            }
        }
        //Booking
        private void btn_addBooking_Click(object sender, RoutedEventArgs e)
        {//Add booking object and customer object to their corresponding list and GUI lists
            Booking booking = new Booking(); //create the booking object
            Customer customer = new Customer(); //create the customer object

            //Get input data for Booking and Customer if any of them are invalid display error message
            try
            {
                booking.ChaletID = Int32.Parse(txtb_chaletID.Text);
                booking.ArrivalDate = datepicker_arrivalDate.SelectedDate.Value.Date;
                booking.DepartureDate = datepicker_departureDate.SelectedDate.Value.Date;
                //check if chalet is taken
                Bookings.isChaletTaken(booking.ChaletID, booking.ArrivalDate, booking.DepartureDate);
                customer.Name = txtb_customerName.Text;
                customer.Address = txtb_customerAddress.Text;

                if (listbox_bookingsList.SelectedItem == null)
                {//if there is nothing selected in the GUI list then this is a new Booking
                    booking.ReferenceNo = Singleton.Instance.getNextKey_bookingRefNo(); //create new reference number
                }
                else
                {//means that the user is amending an existing booking
                    booking.ReferenceNo = getSelectedKey_BookingRef(); //use existing reference number
                }
                if (Bookings.getCustomer(txtb_customerName.Text, txtb_customerAddress.Text) == null)
                {//if the customer doesn't already exist, then create an ID for them
                    customer.Id = Singleton.Instance.getNextKey_customerID();
                    Bookings.AddCustomer(customer);
                    booking.Customer = customer;
                    refreshList(Target.CustomersList);
                }
                else
                {
                    booking.Customer = Bookings.getCustomer(txtb_customerName.Text, txtb_customerAddress.Text); //if the customer does exist, attach to booking
                }

                Bookings.AddBooking(booking); //add the booking to the bookings list
                refreshList(Target.BookingsList); //refresh GUI list
                resetFields(Target.All); //reset all fields for new input
            }
            catch (Exception ex)
            {
                if (ex is FormatException) //if its a user input error
                {
                    MessageBox.Show("Chalet ID input invalid, only Integers are accepted in the range of (1-10)");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btn_amendBooking_Click(object sender, RoutedEventArgs e)
        {//Amend booking object
            //Simulate Add button click to re-evaluate all the new input that the user might have amended
            //Save guest list, extras, car hire details and price of selected booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            //Save refNo as the listbox will lose it's selected item, but we need to re-select it again
            int selectedBookingRefNo = selectedBooking.ReferenceNo;
            List<Guest> guestList = Bookings.getGuestList(selectedBooking);
            CarHire carhire = selectedBooking.CarHire;
            bool eveningMeal = selectedBooking.EveningMeal;
            bool breakfast = selectedBooking.Breakfast;
            bool carHire = selectedBooking.Carhire;

            //Remove old booking and add the new booking
            Bookings.RemoveBooking(selectedBookingRefNo);
            btn_addBooking_Click(sender, e);

            //Select same booking from the listbox using the refNo
            selectedBooking = Bookings.getBooking(selectedBookingRefNo);
            //Readd the guest list, extras and price to the new booking
            Bookings.setGuestList(selectedBooking, guestList);
            selectedBooking.CarHire = carhire;
            selectedBooking.EveningMeal = eveningMeal;
            selectedBooking.Breakfast = breakfast;
            selectedBooking.Carhire = carHire;

            //Exit Amend mode
            Amend_Exit(Target.Booking);

            //set the selected item to null, (if there is one booking in the list it will stay selected, this ensured the user can amend it again)
            listbox_bookingsList.SelectedItem = null;
        }
        private void btn_deleteBooking_Click(object sender, RoutedEventArgs e)
        {//Delete Booking, refresh GUI list and exit Amend mode
            Bookings.RemoveBooking(getSelectedKey_BookingRef());
            refreshList(Target.BookingsList);
            resetFields(Target.All);
            Amend_Exit(Target.Booking);
        }
        private void listbox_bookingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//Get the selected booking object from the list and display it
            if (listbox_bookingsList.SelectedItem != null) //need to test for this, becase if the user stops amending then a null object will be selected
            {
                //Get booking object
                Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());

                //Display the booking object
                datepicker_arrivalDate.Text = selectedBooking.ArrivalDate.ToShortDateString();
                datepicker_departureDate.Text = selectedBooking.DepartureDate.ToShortDateString();
                lbl_bookingRef.Content = "#Ref " + selectedBooking.ReferenceNo.ToString();
                txtb_chaletID.Text = selectedBooking.ChaletID.ToString();
                //Display the customer for the booking
                lbl_customerID.Content = "#ID " + selectedBooking.Customer.Id.ToString();
                txtb_customerName.Text = selectedBooking.Customer.Name;
                txtb_customerAddress.Text = selectedBooking.Customer.Address;
                //Display the guests for the booking
                refreshList(Target.GuestList);
                //Calculate and display cost of the booking
                //Recalculation is important because the user might have changed the guest list or date, therefore the base price and 
                //each of the extras costs need to be changed accordingly.
                //Recalculate base price
                calculateBasePrice(selectedBooking);
                //Display basic costs
                resetFields(Target.Cost); //first reset listbox
                displayCost(selectedBooking);
                //Recalculate total price
                if (selectedBooking.EveningMeal)
                {
                    btn_extra_eveningMeal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //Button click simulation to re-add the extra
                }
                if (selectedBooking.Breakfast)
                {
                    btn_extra_breakfast.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                if (selectedBooking.Carhire)
                {
                    //if there is a carhire, display it
                    if (selectedBooking.CarHire != null)
                    {
                        carHire_show(selectedBooking);
                    }
                    btn_extra_carHire.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }

                //Display extras
                refreshExtrasButtons();
                //Enter Amend mode
                Amend_Enter(Target.Booking);
            }
        }
        private void calculateBasePrice(Booking selectedBooking)
        {//Calculates and displays the base price only (price without extras)
            //Forcing recalculation of base price by setting the extras to false, calculating, then setting them back
            //Save extras
            bool eveningMeal = selectedBooking.EveningMeal;
            bool breakfast = selectedBooking.Breakfast;
            bool carHire = selectedBooking.Carhire;
            selectedBooking.EveningMeal = false;
            selectedBooking.Breakfast = false;
            selectedBooking.Carhire = false;
            //Calculate
            selectedBooking.Price = selectedBooking.CalculatePrice(selectedBooking.numberOfGuests(), selectedBooking.numberOfNights(), 0);
            //Readd extras
            selectedBooking.EveningMeal = eveningMeal;
            selectedBooking.Breakfast = breakfast;
            selectedBooking.Carhire = carHire;
            //Display
            txtb_price.Text = "TOTAL   £" + selectedBooking.Price;
        }
        private void calculateTotalPrice(Booking selectedBooking, BookingCost extra)
        {//Calculates and displays the total price (base + extra)
            if (selectedBooking.CarHire != null)
            {//if there is a carhire, pass the number of days in as well
                selectedBooking.Price = extra.CalculatePrice(selectedBooking.numberOfGuests(), selectedBooking.numberOfNights(), selectedBooking.numberOfCarHireDays());
            }
            else
            {
                selectedBooking.Price = extra.CalculatePrice(selectedBooking.numberOfGuests(), selectedBooking.numberOfNights(), 0);
            }

            txtb_price.Text = "TOTAL   £" + selectedBooking.Price;
        }
        private void displayCost(Booking selectedBooking)
        {//Display costs on the receipt textbox
            string cost = String.Format("Chalet {0}/n{1} Guest(s) for/n{2} Night(s) = {3}", selectedBooking.ChaletID, selectedBooking.numberOfGuests().ToString(), selectedBooking.numberOfNights().ToString(), selectedBooking.Price.ToString());
            cost = cost.Replace("/n", System.Environment.NewLine);

            txtb_cost.Text += System.Environment.NewLine + System.Environment.NewLine + cost;
        }
        //Customer
        private void btn_addCustomer_Click(object sender, RoutedEventArgs e)
        {//Add customer object to Customer List and GUI list
            Customer customer = new Customer(); //create the customer object
            try
            {
                customer.Name = txtb_customerName.Text;
                customer.Address = txtb_customerAddress.Text;

                if (listbox_customerList.SelectedItem == null && Bookings.getCustomer(txtb_customerName.Text, txtb_customerAddress.Text) == null)
                {//if there is nothing selected in the GUI list and the customer is not in the customer list then this is a new Customer
                    customer.Id = Singleton.Instance.getNextKey_customerID(); //create new reference number
                    Bookings.AddCustomer(customer); //add new customer to the list
                }
                else if (Bookings.getCustomer(txtb_customerName.Text, txtb_customerAddress.Text) != null)
                {//throw an error if the customer is already in the list
                    throw new ArgumentException("Customer already exists.");
                }
                else
                {//means that the user is amending an existing customer
                    customer.Id = getSelectedKey_CustomerID(); //use existing ID
                    Bookings.AddCustomer(customer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            resetFields(Target.All);
            refreshList(Target.CustomersList);
        }
        private void btn_amendCustomer_Click(object sender, RoutedEventArgs e)
        {//Amend customer object
            //Remove old customer
            Bookings.RemoveCustomer(getSelectedKey_CustomerID());
            //Add new customer
            btn_addCustomer_Click(sender, e);

            //Exit Amend mode
            Amend_Exit(Target.Customer);

            //set the selected item to null, (if there is one customer in the list it will stay selected, this ensured the user can amend it again)
            listbox_customerList.SelectedItem = null;
        }
        private void btn_deleteCustomer_Click(object sender, RoutedEventArgs e)
        {//Delete Customer object IF there are no booking associated with them, refresh GUI list and exit Amend mode
            Booking theirBooking = Bookings.hasBooking(Bookings.getCustomer(getSelectedKey_CustomerID())); //get customer's booking
            if (theirBooking == null)
            {
                Bookings.RemoveCustomer(getSelectedKey_CustomerID());
                refreshList(Target.CustomersList);
                resetFields(Target.All);
                Amend_Exit(Target.Customer);
            }
            else
            {//Display error message plus reference of booking if the customer has a booking
                MessageBox.Show(String.Format("Customer can not be deleted, booking #Ref {0} is associated with them.", theirBooking.ReferenceNo.ToString()));
            }
        }
        private void listbox_customerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//Gets the selected customer object from the list and displays it
            if (listbox_customerList.SelectedItem != null)
            {
                //Get Customer object
                Customer selectedCustomer = Bookings.getCustomer(getSelectedKey_CustomerID());

                //Display the customer object
                lbl_customerID.Content = "#ID " + selectedCustomer.Id.ToString();
                txtb_customerName.Text = selectedCustomer.Name;
                txtb_customerAddress.Text = selectedCustomer.Address;

                //Enter Amend mode
                Amend_Enter(Target.Customer);
            }
        }
        //Guest
        private void btn_addGuest_Click(object sender, RoutedEventArgs e)
        {//Add guest object to Guest List and GUI list
            Guest guest = new Guest(); //create the guest object
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            try
            {
                guest.Name = txtb_guestName.Text;
                guest.PassportNo = txtb_guestPassNo.Text;
                guest.Age = Int32.Parse(txtb_guestAge.Text);

                if (listbox_guestList.SelectedItem == null && Bookings.getGuest(selectedBooking, guest.PassportNo) == null)
                {//if there is nothing selected in the GUI list and the guest is not in the guest list then this is a new Guest
                    Bookings.AddGuest(selectedBooking, guest); //add new guest to the list
                }
                else if (Bookings.getGuest(selectedBooking, guest.PassportNo) != null)
                {//throw an error if the guest is already in the list
                    throw new ArgumentException("Guest already exists.");
                }
                else
                {//means that the user is amending an existing guest
                    Bookings.RemoveGuest(selectedBooking, Bookings.getGuest(selectedBooking, guest.PassportNo)); //remove old guest
                    Bookings.AddGuest(selectedBooking, guest); //add amended guest to the list
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageBox.Show("Guest age must be an Integer in the range of (0-101).");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            resetFields(Target.Guest);
            refreshList(Target.GuestList);
        }
        private void btn_amendGuest_Click(object sender, RoutedEventArgs e)
        {//Amend guest object
            //Remove old guest
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            Bookings.RemoveGuest(selectedBooking, Bookings.getGuest(selectedBooking, getSelectedKey_GuestPassNo()));
            //Add new guest
            btn_addGuest_Click(sender, e);

            //Exit Amend mode
            Amend_Exit(Target.Guest);

            //set the selected item to null, (if there is one guest in the list it will stay selected, this ensured the user can amend it again)
            listbox_guestList.SelectedItem = null;
        }
        private void btn_deleteGuest_Click(object sender, RoutedEventArgs e)
        {//Delete guest object, refresh GUI list and exit Amend mode

            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            Bookings.RemoveGuest(selectedBooking, Bookings.getGuest(selectedBooking, getSelectedKey_GuestPassNo()));
            //removing extras
            selectedBooking.EveningMeal = false;
            selectedBooking.Breakfast = false;
            selectedBooking.Carhire = false;
            refreshExtrasButtons();

            resetFields(Target.Guest);
            refreshList(Target.GuestList);
            Amend_Exit(Target.Guest);

        }
        private void listbox_guestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//Gets the selected guest object from the list and displays it
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            if (listbox_guestList.SelectedItem != null)
            {
                //Get Guest object
                Guest selectedGuest = Bookings.getGuest(selectedBooking, getSelectedKey_GuestPassNo());

                //Display the guest object
                txtb_guestName.Text = selectedGuest.Name;
                txtb_guestPassNo.Text = selectedGuest.PassportNo;
                txtb_guestAge.Text = selectedGuest.Age.ToString();

                //Enter Amend mode
                Amend_Enter(Target.Guest);
            }
        }
        //Extras
        /*Extras are calculated dynamically, whenever one is added or removed.
         *When a booking is selected, click events will be triggered if the booking has that particular extra,
         *forcing recalculation of costs, since the user could have changed the guest list or dates, which affects the prices.
         */
        private void refreshExtrasButtons()
        {//Refresh button availability for extras
            //get the booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());

            //change extras button availability according to records
            if (!selectedBooking.EveningMeal) { btn_extra_eveningMeal.IsEnabled = true; btn_extra_eveningMeal_Remove.IsEnabled = false; }
            else
            {
                btn_extra_eveningMeal.IsEnabled = false;
                btn_extra_eveningMeal_Remove.IsEnabled = true;
            }

            if (!selectedBooking.Breakfast) { btn_extra_breakfast.IsEnabled = true; btn_extra_breakfast_Remove.IsEnabled = false; }
            else
            {
                btn_extra_breakfast.IsEnabled = false;
                btn_extra_breakfast_Remove.IsEnabled = true;
            }

            if (!selectedBooking.Carhire) { btn_extra_carHire.IsEnabled = true; btn_extra_carHire_Remove.IsEnabled = false; }
            else
            {
                btn_extra_carHire.IsEnabled = false;
                btn_extra_carHire_Remove.IsEnabled = true;
            }
        }
        private void btn_extra_eveningMeal_Click(object sender, RoutedEventArgs e)
        {//Add evening meal prices
            //get the booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            selectedBooking.EveningMeal = true; //store the extra

            //create new evening meal
            EveningMeal_add eveningMeal = new EveningMeal_add();
            eveningMeal.AddTo(selectedBooking);
            //calculate the new price
            calculateTotalPrice(selectedBooking, eveningMeal);
            //display cost
            lbl_eveningMeal_cost.Content += eveningMeal.getCost();
            lbl_eveningMeal_cost.Visibility = Visibility.Visible;
            //switch the buttons
            refreshExtrasButtons();
        }
        private void btn_extra_breakfast_Click(object sender, RoutedEventArgs e)
        {//Add breakfast prices
            //get booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            selectedBooking.Breakfast = true; //store the extra

            //create new breakfast
            Breakfast_add breakfast = new Breakfast_add();
            breakfast.AddTo(selectedBooking);
            //calculate new price
            calculateTotalPrice(selectedBooking, breakfast);
            //display cost
            lbl_breakfast_cost.Content += breakfast.getCost();
            lbl_breakfast_cost.Visibility = Visibility.Visible;
            //switch the buttons
            refreshExtrasButtons();
        }
        private void btn_extra_carHire_Click(object sender, RoutedEventArgs e)
        {//Add car hire price
            //get booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());
            //open window to enter car hire details
            if (selectedBooking.CarHire == null)
            {//get car hire detals if there is non
                CarHireDetails CarHireWindow = new CarHireDetails(selectedBooking);
                CarHireWindow.ShowDialog();
            }

            if (selectedBooking.CarHire != null)
            {// check if the user actually entered car hire details
                selectedBooking.Carhire = true; //store the extra

                //create new carhire
                CarHire_add car = new CarHire_add();
                car.AddTo(selectedBooking);
                //calculate new price
                calculateTotalPrice(selectedBooking, car);
                //display cost
                lbl_carHire_cost.Content += car.getCost();
                lbl_carHire_cost.Visibility = Visibility.Visible;

                //switch the buttons
                refreshExtrasButtons();

                //show car hire details
                carHire_show(selectedBooking);
            }
        }
        private void btn_extra_eveningMeal_Minus_Click(object sender, RoutedEventArgs e)
        {//Remove evening meal prices
            //get the booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());

            //create new evening meal
            EveningMeal_remove eveningMeal = new EveningMeal_remove();
            eveningMeal.AddTo(selectedBooking);
            //calculate the new price
            calculateTotalPrice(selectedBooking, eveningMeal);
            //remove display of cost and reset label
            lbl_eveningMeal_cost.Visibility = Visibility.Hidden;
            lbl_eveningMeal_cost.Content = "Evening Meal = ";

            selectedBooking.EveningMeal = false; //store the extra

            //switch the buttons
            refreshExtrasButtons();
        }
        private void btn_extra_breakfast_Minus_Click(object sender, RoutedEventArgs e)
        {//Remove breakfast prices
            //get booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());

            //create new breakfast
            Breakfast_remove breakfast = new Breakfast_remove();
            breakfast.AddTo(selectedBooking);
            //calculate new price
            calculateTotalPrice(selectedBooking, breakfast);
            //remove display of cost and reset label
            lbl_breakfast_cost.Visibility = Visibility.Hidden;
            lbl_breakfast_cost.Content = "Breakfast = ";

            selectedBooking.Breakfast = false; //store the extra

            //switch the buttons
            refreshExtrasButtons();
        }
        private void btn_extra_carHire_Minus_Click(object sender, RoutedEventArgs e)
        {//Remove carhire price
            //get booking
            Booking selectedBooking = Bookings.getBooking(getSelectedKey_BookingRef());

            //create new carhire
            CarHire_remove car = new CarHire_remove();
            car.AddTo(selectedBooking);
            //calculate new price
            calculateTotalPrice(selectedBooking, car);
            //remove display of cost and reset label
            lbl_carHire_cost.Visibility = Visibility.Hidden;
            lbl_carHire_cost.Content = "Car Hire = ";

            selectedBooking.Carhire = false; //store the extra

            //switch the buttons
            refreshExtrasButtons();

            //delete CarHire details
            selectedBooking.CarHire = null;

            //remove car hire display
            lbl_CarHire.Visibility = Visibility.Hidden;
            txtb_CarHire.Visibility = Visibility.Hidden;
        }

        #region TextBox Functionality
        //Textboxes have default labels in them to guide the user what the box is for
        //If the user clicks on the box, the contents will clear out so the user can put in their own data
        //If the user click outside the box, then the contents will change back to default
        private void txtb_chaletID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_chaletID.Text == "Chalet ID")
            {
                txtb_chaletID.Text = "";
            }
        }
        private void txtb_chaletID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_chaletID.Text == "")
            {
                txtb_chaletID.Text = "Chalet ID";
            }
        }
        private void txtb_customerName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_customerName.Text == "Customer Name")
            {
                txtb_customerName.Text = "";
            }
        }
        private void txtb_customerName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_customerName.Text == "")
            {
                txtb_customerName.Text = "Customer Name";
            }
        }
        private void txtb_customerAddress_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_customerAddress.Text == "Customer Address")
            {
                txtb_customerAddress.Text = "";
            }
        }
        private void txtb_customerAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_customerAddress.Text == "")
            {
                txtb_customerAddress.Text = "Customer Address";
            }
        }
        private void txtb_guestName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_guestName.Text == "Guest Name")
            {
                txtb_guestName.Text = "";
            }
        }
        private void txtb_guestName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_guestName.Text == "")
            {
                txtb_guestName.Text = "Guest Name";
            }
        }
        private void txtb_guestPassNo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_guestPassNo.Text == "Passport No.")
            {
                txtb_guestPassNo.Text = "";
            }
        }
        private void txtb_guestPassNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_guestPassNo.Text == "")
            {
                txtb_guestPassNo.Text = "Passport No.";
            }
        }
        private void txtb_guestAge_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_guestAge.Text == "Age")
            {
                txtb_guestAge.Text = "";
            }
        }
        private void txtb_guestAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_guestAge.Text == "")
            {
                txtb_guestAge.Text = "Age";
            }
        }
        #endregion
        #region Listbox functionality
        //When one of the listboxes gets the focus, the other one loses it, this way, only one of them will have focus at a time
        //They lose their selected items, and Amend modes exit
        private void listbox_bookingsList_GotFocus(object sender, RoutedEventArgs e)
        {
            listbox_customerList.SelectedItem = null;
            Amend_Exit(Target.Customer);
        }
        private void listbox_customerList_GotFocus(object sender, RoutedEventArgs e)
        {
            listbox_bookingsList.SelectedItem = null;
            Amend_Exit(Target.Booking);
        }
        private void listbox_guestList_GotFocus(object sender, RoutedEventArgs e)
        {//If a booking is selected and the user click on a guest, or simply the listbox, it means they mean to edit the guestlist
            listbox_customerList.SelectedItem = null;
            if (listbox_bookingsList.SelectedItem != null) //if a booking is selected in the GUI bookings list
            {
                Amend_Enter(Target.Guest);
            }
        }

        #endregion
    }
}
