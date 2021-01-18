using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserDashboard.Models
{
    public class User
    {
        public UserName Name { get; set; }
        public string FullName
        {
            get { return Name.First + " " + Name.Last; }
        }

        public string UserLocation
        {
            get { return $"{Location.Street.Number} {Location.Street.Name}, {Location.State}, {Location.Country}"; }
        }
        public UserLocation Location { get; set; }
        public UserPicture Picture { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public UserRegistered Registered { get; set; }
        public string Phone { get; set; }
        public UserDob Dob { get; set; } 
    }

    public class UserName
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class UserLocation
    {
        public Street Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
    }

    public class Street
    {
        public string Number { get; set; }
        public string Name { get; set; }
    }

    public class UserPicture
    {
        public string Large { get; set; }
        public string Medium { get; set; }
        public string Thumbnail { get; set; }
    }
    public class UserDob
    {
        public DateTime DateTime { get; set; }
        public int Age { get; set; }
    }

    public class UserRegistered
    {
        public DateTime DateTime { get; set; }
        public int Age { get; set; }
    }

}
