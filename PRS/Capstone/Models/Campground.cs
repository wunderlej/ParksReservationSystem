using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    //created to populate data properties from SQL campground table
    public class Campground
    {
        //properties
        public int Campground_id { get; set; }
        public int Park_id { get; set; }
        public string Name { get; set; }
        public int Open_from_mm { get; set; }
        public int Open_to_mm { get; set; }
        public decimal Daily_fee { get; set; }

        //allows console to print string name of month instead of int number
        public string DateConvertedFrom
        {
            get
            {
                string result = "";
                if (Open_from_mm == 1)
                {
                    result = "January";
                }
                else if (Open_from_mm == 2)
                {
                    result = "February";
                }
                else if (Open_from_mm == 3)
                {
                    result = "March";
                }
                else if (Open_from_mm == 4)
                {
                    result = "April";
                }
                else if (Open_from_mm == 5)
                {
                    result = "May";
                }
                else if (Open_from_mm == 6)
                {
                    result = "June";
                }
                else if (Open_from_mm == 7)
                {
                    result = "July";
                }
                else if (Open_from_mm == 8)
                {
                    result = "August";
                }
                else if (Open_from_mm == 9)
                {
                    result = "September";
                }
                else if (Open_from_mm == 10)
                {
                    result = "October";
                }
                else if (Open_from_mm == 11)
                {
                    result = "November";
                }
                else if (Open_from_mm == 12)
                {
                    result = "December";
                }

                return result;
            }
        }

        //allows console to print string name of month instead of int number
        public string DateConvertedTo
        {
            get
            {
                string result = "";
                if (Open_to_mm == 1)
                {
                    result = "January";
                }
                else if (Open_to_mm == 2)
                {
                    result = "February";
                }
                else if (Open_to_mm == 3)
                {
                    result = "March";
                }
                else if (Open_to_mm == 4)
                {
                    result = "April";
                }
                else if (Open_to_mm == 5)
                {
                    result = "May";
                }
                else if (Open_to_mm == 6)
                {
                    result = "June";
                }
                else if (Open_to_mm == 7)
                {
                    result = "July";
                }
                else if (Open_to_mm == 8)
                {
                    result = "August";
                }
                else if (Open_to_mm == 9)
                {
                    result = "September";
                }
                else if (Open_to_mm == 10)
                {
                    result = "October";
                }
                else if (Open_to_mm == 11)
                {
                    result = "November";
                }
                else if (Open_to_mm == 12)
                {
                    result = "December";
                }
                return result;
            }
        }
    }
}
