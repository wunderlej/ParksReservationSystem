using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    //created to populate data properties from SQL park table
    public class Park
    {
        //properties
        public int Park_id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Establish_date { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

        //allows console to print full park name when necessary
        public string DisplayName
        {
            get
            {
                string result = "";
                if (Name == "Acadia")
                {
                    result = "Acadia National Park";
                }
                else if (Name == "Arches")
                {
                    result = "Arches National Park";
                }
                else if (Name == "Cuyahoga Valley")
                {
                    result = "Cuyahoga National Valley Park";
                }
                else
                {
                    result = Name;
                }

                return result;
            }
        }
    }
}
