using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// Console Line Interface
    /// </summary>
    public class ProjectCLI
    {
        //member variables
        private Dal _dal;

        //constructor, initiates with connection string
        public ProjectCLI(string connectionString)
        {
            _dal = new Dal(connectionString);
        }

        /// <summary>
        /// Converts the list of parks into a dictionary, uses parkid as the key
        /// </summary>
        /// <param name="parkList">ParkList</param>
        /// <returns>A dictionary of park objects</returns>
        public Dictionary<int, Park> ConvertToParkDictionary(List<Park> parkList)
        {
            Dictionary<int, Park> parkDictionary = new Dictionary<int, Park>();

            int id = 1;
            foreach (var park in parkList)
            {

                parkDictionary.Add(id, park);
                id++;
            }
            return parkDictionary;
        }

        /// <summary>
        /// Converts a list of campgrounds into a dictionary, uses campgorundid as the key
        /// </summary>
        /// <param name="campgroundList">CampgroundList</param>
        /// <returns>A dictionary of campground objects</returns>
        public Dictionary<int, Campground> ConvertToCampgroundDictionary(List<Campground> campgroundList)
        {
            Dictionary<int, Campground> campgroundDictionary = new Dictionary<int, Campground>();

            int id = 1;
            foreach (var park in campgroundList)
            {

                campgroundDictionary.Add(id, park);
                id++;
            }
            return campgroundDictionary;
        }

        /// <summary>
        /// Main menu shown on initial startup screen
        /// </summary>
        public void MainMenu()
        {

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("View Parks Interface");
                Console.WriteLine("Select a Park for Further Details");
                Console.WriteLine();

                List<Park> parkList = _dal.GetParks();
                Dictionary<int, Park> parkDictionary = ConvertToParkDictionary(parkList);

                foreach (var item in parkDictionary)
                {
                    Console.WriteLine($"({item.Value.Park_id}) {item.Value.Name}");

                }
                Console.WriteLine();
                Console.WriteLine("(Q) Quit");
                Console.WriteLine();
                Console.Write("Please make a choice: ");

                string choice = (Console.ReadLine().ToLower());

                Console.WriteLine();

                //if choice equals the key of the dictionary, then go to park value
                try
                {
                    if (choice == "q")
                    {
                        exit = true;
                    }

                    else if (choice != "q")
                    {
                        foreach (var item in parkDictionary)
                        {
                            if (int.Parse(choice) == item.Value.Park_id)
                            {
                                DisplayParkInfo(int.Parse(choice));

                            }
                        }
                    }
                }

                catch(Exception)
                {
                    Console.Write("Please choose one of the appropriate options.");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Display park info after users initial selection
        /// </summary>
        /// <param name="choice">The user selection, also indicates park dictionary key</param>
        public void DisplayParkInfo(int choice)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                List<Park> parkList = _dal.GetParks();
                Dictionary<int, Park> parks = ConvertToParkDictionary(parkList);

                Console.WriteLine("Park Information Screen");
                Console.WriteLine($"{parks[choice].DisplayName}");
                Console.WriteLine("Location:         " + $"{parks[choice].Location}");
                Console.WriteLine("Established:      " + $"{parks[choice].Establish_date.ToString("MM/dd/yyyy")}");
                Console.WriteLine($"Area:             " + $"{parks[choice].Area.ToString("N0")} sq km");
                Console.WriteLine($"Annual Visitors:  " + $"{parks[choice].Visitors.ToString("N0")}");
                Console.WriteLine();
                Console.WriteLine($"{parks[choice].Description}");
                Console.WriteLine();
                Console.WriteLine("Select Campgrounds:");
                Console.WriteLine(" 1)View Campgrounds");
                Console.WriteLine(" 2)Search for Reservation");
                Console.WriteLine(" 3)Return to Previous Screen");

                string selection = Console.ReadLine();

                try
                {
                    if (selection == "1")
                    {
                        DisplayCampgroundInfo(choice);
                    }
                    else if (selection == "2")
                    {
                        DisplayCampgroundReservations(choice);
                    }
                    else if (selection == "3")
                    {
                        exit = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Display screen once user selects to view campgrounds in the park
        /// </summary>
        /// <param name="parkId">ParkId</param>
        public void DisplayCampgroundInfo(int parkId)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                List<Park> parkList = _dal.GetParks();
                Dictionary<int, Park> parks = ConvertToParkDictionary(parkList);
                List<Campground> campgroundsList = _dal.GetCampgroundsByParkId(parkId);
                Dictionary<int, Campground> campgrounds = ConvertToCampgroundDictionary(campgroundsList);

                Console.WriteLine("Park Campgrounds");
                Console.WriteLine($"{parks[parkId].DisplayName} Campgrounds");
                Console.WriteLine();
                Console.WriteLine("   Name".PadRight(30) + "Open".PadRight(20) + "Close".PadRight(20) + "Daily Fee");

                foreach (var item in campgrounds)
                {
                    Console.WriteLine($"#{item.Key} {item.Value.Name}".PadRight(30) + $"{item.Value.DateConvertedFrom}".PadRight(20) + $"{item.Value.DateConvertedTo}".PadRight(20) + $"{item.Value.Daily_fee.ToString("C")}");
                }

                Console.WriteLine();
                Console.WriteLine("Select a Command");
                Console.WriteLine("1) Search for Available Reservation");
                Console.WriteLine("2) Return to Previous Screen");
                string choice = Console.ReadLine();

                try
                {
                    if (choice == "1")
                    {
                        DisplayCampgroundReservations(parkId);
                    }
                    else if (choice == "2")
                    {
                        exit = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Display screen once user selects to search for available reservations in campground
        /// </summary>
        /// <param name="parkId">ParkId</param>
        public void DisplayCampgroundReservations(int parkId)
        {
            bool exit = false;
            string campgroundInput = "";
            while (!exit)
            {
                List<Site> availableSites = new List<Site>();
                List<Campground> campgroundsList = _dal.GetCampgroundsByParkId(parkId);
                Dictionary<int, Campground> campgrounds = ConvertToCampgroundDictionary(campgroundsList);

                Console.Clear();
                Console.WriteLine("Search for Campground Reservation");

                foreach (var item in campgrounds)
                {
                    Console.WriteLine($"#{item.Key} {item.Value.Name}".PadRight(30) + $"{item.Value.DateConvertedFrom}".PadRight(20) + $"{item.Value.DateConvertedTo}".PadRight(20) + $"{item.Value.Daily_fee.ToString("C")}");
                }
                Console.WriteLine();
                Console.WriteLine("Which campground (enter 0 to cancel)?");
                campgroundInput = Console.ReadLine();
                try
                {
                    if (int.Parse(campgroundInput) != 0)
                    {
                        Console.WriteLine("What is the arrival date? (YYYY-MM-DD)");
                        string arrivalInput = Console.ReadLine();
                        DateTime arrivalDate = DateTime.Parse(arrivalInput);

                        //An option to handle one of the bonus, need a way to force them to reenter date
                        //if (arrivalDate.Month <= campgrounds[int.Parse(campgroundInput)].Open_from_mm)
                        //{
                        //    Console.WriteLine("The park is closed during the specified arrival date. Press any key to start over.");
                        //    Console.ReadKey();
                        //}

                        Console.WriteLine("What is the departure date? (YYYY-MM-DD)");
                        string departureInput = Console.ReadLine();
                        DateTime departureDate = DateTime.Parse(departureInput);

                        //An option to handle one of the bonus, need a way to force them to reenter date
                        //if (departureDate.Month >= campgrounds[int.Parse(campgroundInput)].Open_to_mm)
                        //{
                        //    Console.WriteLine("The park is closed during the specified departure date. Press any key to start over.");
                        //    Console.ReadKey();

                        //}

                        DisplayResults(parkId, campgroundInput, arrivalDate, departureDate);
                    }
                    else if (int.Parse(campgroundInput) == 0)
                    {
                        exit = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Display screen used to show up to five sites based upon the selected site number, arrival and departure dates
        /// </summary>
        /// <param name="parkId">ParkId</param>
        /// <param name="campgroundInput">Campground selected</param>
        /// <param name="arrivalDate">ArrivalDate</param>
        /// <param name="departureDate">DepartureDate</param>
        public void DisplayResults(int parkId, string campgroundInput, DateTime arrivalDate, DateTime departureDate)
        {
            bool exit = false;
            while (!exit)
            {
                TimeSpan totalDays = departureDate.Subtract(arrivalDate);

                List<Site> availableSites = new List<Site>();
                List<Campground> campgroundsList = _dal.GetCampgroundsByParkId(parkId);
                Dictionary<int, Campground> campgrounds = ConvertToCampgroundDictionary(campgroundsList);

                    if (campgrounds.ContainsKey(int.Parse(campgroundInput)))
                    {
                        availableSites = _dal.GetAvailableSites(parkId, int.Parse(campgroundInput), arrivalDate, departureDate);

                        Console.WriteLine();
                        Console.WriteLine("Results Matching Your Search Criteria");

                        Console.WriteLine("Site No.".PadRight(30) + "Max Occup.".PadRight(20) + "Accessible?".PadRight(20) + "Max RV Length".PadRight(20) + "Utility".PadRight(20) + "Cost");

                        foreach (var item in availableSites)
                        {
                            Console.WriteLine($"{item.Site_number}".PadRight(30) + $"{item.Max_occupancy}".PadRight(20) + $"{item.IsAccessible}".PadRight(20) +
                                              $"{item.RvLength}".PadRight(20) + $"{item.HasUtilities}".PadRight(20) + $"{(item.DailyFee * totalDays.Days).ToString("C")}");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Which site should be reserved (enter 0 to cancel)?");
                        string reservedSite = Console.ReadLine();
                        try
                        {
                            if (int.Parse(reservedSite) != 0)
                            {
                                Console.WriteLine("What name should the reservation be made under?");
                                string reservedName = Console.ReadLine();
                                Console.WriteLine();
                                Console.WriteLine($"The reservation has been made and the confirmation id is {_dal.BookReservation(int.Parse(campgroundInput), reservedName, arrivalDate, departureDate, DateTime.Now)}");
                                Console.WriteLine("Please enter any key to exit");
                                Console.ReadKey();
                                Environment.Exit(0);
                                //exit = true;
                            }
                            else if (int.Parse(reservedSite) == 0)
                            {
                                exit = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadKey();
                        }
                    }
            }
        }
    }
}
    



