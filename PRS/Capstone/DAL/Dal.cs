using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// Data access layer created to separate user input from SQL database
    /// </summary>
    public class Dal
    {
        //member variables
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString;

        //constructor, initiates with connection string
        public Dal(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Reads SQL park data from database and creates a list of parks with properties
        /// </summary>
        /// <returns>A list of parks</returns>
        public List<Park> GetParks()
        {
            List<Park> result = new List<Park>();

            string SqlParksList = $"SELECT * from park order by name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SqlParksList, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Park p = new Park();
                    p.Park_id = Convert.ToInt32(reader["park_id"]);
                    p.Name = Convert.ToString(reader["name"]);
                    p.Location = Convert.ToString(reader["location"]);
                    p.Establish_date = Convert.ToDateTime(reader["establish_date"]);
                    p.Area = Convert.ToInt32(reader["area"]);
                    p.Visitors = Convert.ToInt32(reader["visitors"]);
                    p.Description = Convert.ToString(reader["description"]);

                    result.Add(p);
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a list with a single park specified the the parkId passed in
        /// </summary>
        /// <param name="parkId">ParkId</param>
        /// <returns>List with a single park</returns>
        public List<Campground> GetCampgroundsByParkId(int parkId)
        {
            List<Campground> result = new List<Campground>();

            string SqlCampgroundList = $"SELECT * from campground Where park_id = @ParkId order by name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SqlCampgroundList, conn);
                cmd.Parameters.AddWithValue("@ParkId", parkId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Campground cG = new Campground();
                    cG.Name = Convert.ToString(reader["name"]);
                    cG.Open_from_mm = Convert.ToInt32(reader["open_from_mm"]);
                    cG.Open_to_mm = Convert.ToInt32(reader["open_to_mm"]);
                    cG.Daily_fee = Convert.ToDecimal(reader["daily_fee"]);

                    result.Add(cG);
                }
            }
            return result;
        }

        /// <summary>
        ///  Reads SQL site data from database and creates a list of sites with properties
        /// </summary>
        /// <param name="parkId">ParkId</param>
        /// <param name="campgroundId">CampgroundId</param>
        /// <returns>List of all sites</returns>
        public List<Site> GetAllSites(int parkId, int campgroundId)
        {
            List<Site> siteList = new List<Site>();
            List<Campground> campgroundList = GetCampgroundsByParkId(parkId);

            string SqlSiteList = $"SELECT * from site Where campground_id = @CampgroundId order by site_number;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SqlSiteList, conn);
                cmd.Parameters.AddWithValue("@CampgroundId", campgroundId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Site s = new Site();
                    s.Site_number = Convert.ToInt32(reader["site_number"]);
                    s.Max_occupancy = Convert.ToInt32(reader["max_occupancy"]);
                    s.Accessible = Convert.ToInt32(reader["accessible"]);
                    s.Max_rv_length = Convert.ToInt32(reader["max_rv_length"]);
                    s.Utilities = Convert.ToInt32(reader["utilities"]);

                    siteList.Add(s);
                }
            }
            return siteList;
        }

        //public bool IsParkOpen(int parkId, DateTime arrivalDate, DateTime departureDate)
        //{
        //    bool result = false;

        //    if()

        //    return result;
        //}

        /// <summary>
        ///  Reads SQL site data from database and creates a list of the top five sites that are available for reservation
        /// </summary>
        /// <param name="parkId">ParkId</param>
        /// <param name="campgroundId">CampgroundId</param>
        /// <param name="arrivalDate">ArrivaleDate</param>
        /// <param name="departureDate">DepartureDate</param>
        /// <returns></returns>
        public List<Site> GetAvailableSites(int parkId, int campgroundId, DateTime arrivalDate, DateTime departureDate)
        {

            List<Site> availableSites = new List<Site>();

            string SqlSiteList = $"select top 5 site.site_number, site.max_occupancy, site.accessible, " +
                                 $"site.max_rv_length, site.utilities, campground.daily_fee " +
                                 $"from site join campground on site.campground_id = campground.campground_id " +
                                 $"where site.site_id in (select site.site_number " +
                                 $"from site left join reservation on site.site_id = reservation.site_id " +
                                 $"join campground on site.campground_id = campground.campground_id " +
                                 $"where campground.park_id = @ParkId and campground.campground_id = @CampgroundId " +
                                 $"and (@DepartureDate < to_date or @ArrivalDate > from_date or reservation.from_date is null));";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SqlSiteList, conn);
                cmd.Parameters.AddWithValue("@CampgroundId", campgroundId);
                cmd.Parameters.AddWithValue("@ParkId", parkId);
                cmd.Parameters.AddWithValue("@ArrivalDate", arrivalDate);
                cmd.Parameters.AddWithValue("@DepartureDate", departureDate);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Site s = new Site();
                    s.Site_number = Convert.ToInt32(reader["site_number"]);
                    s.Max_occupancy = Convert.ToInt32(reader["max_occupancy"]);
                    s.Accessible = Convert.ToInt32(reader["accessible"]);
                    s.Max_rv_length = Convert.ToInt32(reader["max_rv_length"]);
                    s.Utilities = Convert.ToInt32(reader["utilities"]);
                    s.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                    availableSites.Add(s);
                }
            }
            return availableSites;
        }

        /// <summary>
        /// Writes to the SQL database and populates table with inputs provided by user
        /// </summary>
        /// <param name="siteId">SiteId</param>
        /// <param name="reservedName">Name for Reservation</param>
        /// <param name="arrivalDate">ArrivalDate</param>
        /// <param name="departureDate">DepartureDate</param>
        /// <param name="currentTime">Current day and time</param>
        /// <returns>ReservationId</returns>
        public int BookReservation(int siteId, string reservedName, DateTime arrivalDate, DateTime departureDate, DateTime currentTime)
        {
            int id = 0;

            // define my sql statement
            string SqlCreateDepartment = $"Insert Into reservation (site_id, name, from_date, to_date, create_date) " +
                                       $"Values (@SiteId, @Name, @FromDate, @ToDate, @CreateDate);" +
                                       _getLastIdSQL;

            // create my connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // open connection
                conn.Open();

                // create my command object
                SqlCommand cmd = new SqlCommand(SqlCreateDepartment, conn);
                cmd.Parameters.AddWithValue("@SiteId", siteId);
                cmd.Parameters.AddWithValue("@Name", reservedName);
                cmd.Parameters.AddWithValue("@FromDate", arrivalDate);
                cmd.Parameters.AddWithValue("@ToDate", departureDate);
                cmd.Parameters.AddWithValue("@CreateDate", currentTime);

                // execute command
                id = (int)cmd.ExecuteScalar();
            }
            return id;
        }
    }
}
