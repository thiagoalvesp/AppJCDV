using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class Location
    {
        public Nullable<double> Accuracy { get; set; }
        public Nullable<double> Altitude { get; set; }
        public Nullable<double> Course { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Nullable<double> Speed { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }
    }
}
