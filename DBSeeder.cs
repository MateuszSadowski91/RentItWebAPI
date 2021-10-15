using RentItAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RentItAPI
{
    public class DBSeeder
    {
        private AppDbContext _dbContext;

        public DBSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Customer"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
    }
}