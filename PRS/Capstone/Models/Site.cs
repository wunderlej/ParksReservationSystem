using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    //created to populate data properties from SQL site table
    public class Site
    {
        //properties
        public int Site_id { get; set; }
        public int Campground_id { get; set; }
        public int Site_number { get; set; }
        public int Max_occupancy { get; set; }
        public int Accessible { get; set; }
        public int Max_rv_length { get; set; }
        public int Utilities { get; set; }
        public decimal DailyFee { get; set; }

        //allows console to print yes or no for accessibility
        public string IsAccessible
        {
            get
            {
                string result;

                if(Accessible == 0)
                {
                    result = "No";
                }
                else 
                {   
                    result = "Yes";
                }
                return result;
            }
        }

        //allows console to print yes or no for utilities
        public string HasUtilities
        {
            get
            {
                string result;

                if (Utilities == 0)
                {
                    result = "N/A";
                }
                else
                {
                    result = "Yes";
                }
                return result;
            }

        }
        //allows console to print N/A for RV Length
        public string RvLength
        {
            get
            {
                 string result;

                if (Max_rv_length == 0)
                {
                    result = "N/A";
                }
                else
                {
                    result = Max_rv_length.ToString();
                }
                return result;
            }

        }
    }
}
