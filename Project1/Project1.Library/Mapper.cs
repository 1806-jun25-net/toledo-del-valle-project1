using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Mapper
    {
        public static User Map(Data.Users user, string userLocation) => new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            LocationName = userLocation
        };

        public static Data.Users Map(User user, int locationId) => new Data.Users
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            LocationId = locationId
        };

        //public static Location Map(Data.Locations location) => new Location
        //{
        //    Name = location.LocationName,

        //};
    }
}
