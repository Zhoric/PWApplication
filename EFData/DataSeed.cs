using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace EFData
{
    public class DataSeed
    {
        public static async Task SeedDataAsync(DataContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                                {
                                    new User
                                        {
                                            DisplayName = "TestUserFirst",
                                            Email = "testuserfirst@test.com"
                                        },

                                    new User
                                        {
                                            DisplayName = "TestUserSecond",
                                            Email = "testusersecond@test.com"
                                        }
                                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "password");
                }
            }
        }
    }
}