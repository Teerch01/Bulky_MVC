using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.DBInitializer;

public class DBInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db) : IDBInitializer
{
    public void Initialize()
    {
        //migrations if they are not applied
        try
        {
            if (db.Database.GetPendingMigrations().Count() > 0)
            {
                db.Database.Migrate();
            }
        }
        catch(Exception ex) { }

        //create roles if they are not created
        if (!roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

            //if roles are not created, then we will create admin user as well
            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com", Name = "Administrator", PhoneNumber = "1234567890", StreetAddress = "admin av", State = "OYO", PostalCode = "200005", City = "IB" };
            userManager.CreateAsync(user, "Admin1234!").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }

        return;
    }
}
