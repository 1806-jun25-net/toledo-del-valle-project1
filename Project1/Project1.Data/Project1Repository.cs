using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1.Data
{
    public class Project1Repository
    {
        private readonly Project1DBContext _db;

        public Project1Repository(Project1DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IEnumerable<Users> GetUsersWithLocationName()
        {
            List<Users> users = _db.Users.Include(m => m.Location).AsNoTracking().ToList();
            return users;
        }

        public IEnumerable<Locations> GetLocations()
        {
            List<Locations> locations = _db.Locations.AsNoTracking().ToList();
            return locations;
        }

        public int GetLocationId(string locationName)
        {
            var location = _db.Locations.FirstOrDefault(x => x.LocationName == locationName);
            if(location == null)
            {
                throw new ArgumentException("no such location with that name", nameof(locationName));
            }
            return location.Id;
        }

        public string GetUserLocation(string firstName, string lastName)
        {
            var user = _db.Users.Include(m => m.Location).FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (user == null)
            {
                string name = firstName + " " + lastName;
                throw new ArgumentException("no such user with that name", nameof(name));
            }
            return user.Location.LocationName;
        }

        public bool UserExists(string firstName, string lastName)
        {
            var user = _db.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public bool LocationExists(string locationName)
        {
            var location = _db.Locations.FirstOrDefault(x => x.LocationName == locationName);
            if (location == null)
            {
                return false;
            }
            return true;
        }
    }
}
