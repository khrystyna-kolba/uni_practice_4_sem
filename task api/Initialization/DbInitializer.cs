using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ContainersApiTask.Models;
using System;
using System.Linq;

namespace ContainersApiTask.Initialization
{
    public static class DbInitializer
    {
        public static async void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                var _userManager =
                         serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var _roleManager =
                         serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();




                if (!context.Users.Any(usr => usr.Email == "khrystyna-yaryna.kolba@lnu.edu.ua"))
                {
                    var user = new User()
                    {
                        UserName = "Khrystyna",
                        LastName = "Kolba",
                        Email = "khrystyna-yaryna.kolba@lnu.edu.ua",

                    };
                    var userResult = await _userManager.CreateAsync(user, "String-123");
                    context.SaveChanges();
                }

                if (!context.Users.Any(usr => usr.Email == "manager@gmail.com"))
                {
                    var user = new User()
                    {
                        UserName = "Manager",
                        LastName = "Manager",
                        Email = "manager@gmail.com",

                    };
                    var userResult = await _userManager.CreateAsync(user, "String-123");
                    context.SaveChanges();
                }
                //add roles
                //foreach (IdentityRole role in _roleManager.Roles)
                //{
                //    _roleManager.DeleteAsync(role).GetAwaiter().GetResult();
                //}
                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole("Manager")).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
                }

                //making Admin
                var adminUser = _userManager.FindByEmailAsync("khrystyna-yaryna.kolba@lnu.edu.ua").Result;
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                //making manager
                var manager = _userManager.FindByEmailAsync("manager@gmail.com").Result;
                await _userManager.AddToRoleAsync(manager, "Manager");
                //await _userManager.RemoveFromRolesAsync(adminUser, new string[] { "Manager" });
                //var r = _userManager.FindByEmailAsync("tetiana@gmail.com").Result;
                //await _userManager.AddToRoleAsync(r, "Customer");


                context.SaveChanges();
            }
        }
    }
}
