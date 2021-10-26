using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentItAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RentItAPI
{
    public class DBSeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public DBSeeder(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingmigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingmigrations != null && pendingmigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Users.Any(u => u.Role.Name == "SuperAdmin"))
                {
                    var superAdmin = GetSuperAdmin();
                    _dbContext.Users.Add(superAdmin);
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
                },
                new Role()
                {
                    Name = "SuperAdmin"
                }
            };
            return roles;
        }

        private User GetSuperAdmin()
        {
            var superAdmin = new User
            {
                FirstName = "SuperAdmin",
                Email = _configuration.GetValue<string>("SuperAdminEmail"),
                RoleId = 3
            };
            return superAdmin;
        }
    }
}