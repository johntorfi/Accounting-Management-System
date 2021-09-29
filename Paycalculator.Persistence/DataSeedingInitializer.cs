using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Paycalculator.Persistence
{
    public static class DataSeedingInitializer
    {
        public static async Task UserAndRoleSeedAsync(UserManager<IdentityUser> userManager,
                                                RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Manager", "Staff" };
            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Create Admin User
            if (userManager.FindByEmailAsync("Admin@yahoo.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Admin@yahoo.com",
                    Email = "Admin@yahoo.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            //Create Manager User
            if (userManager.FindByEmailAsync("manager@yahoo.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "manager@yahoo.com",
                    Email = "manager@yahoo.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            //Create Staff User
            if (userManager.FindByEmailAsync("john@yahoo.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "john@yahoo.com",
                    Email = "john@yahoo.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Staff").Wait();
                }
            }

            //Create No Role User
            if (userManager.FindByEmailAsync("jane@yahoo.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "jane@yahoo.com",
                    Email = "jane@yahoo.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                //No Role assigned to jane
            }
        }
    }
}
