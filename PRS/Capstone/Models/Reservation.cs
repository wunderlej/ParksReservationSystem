using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    //created to populate data properties from SQL reservation table
    public class Reservation
    {
        //properties
        public int Reservation_id { get; set; }
        public int Site_id { get; set; }
        public string Name { get; set; }
        public DateTime From_date { get; set; }
        public DateTime To_date { get; set; }
        public DateTime Create_date { get; set; }
    }
}
