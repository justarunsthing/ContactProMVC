using ContactProMVC.Data;
using ContactProMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactProMVC.Helpers
{
    public static class DataHelper
    {
        public static async Task ManageDataAsync(IServiceProvider serviceProvider)
        {
            // Get an instance of ApplicationDbContext
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Get an instance of UserManager
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Apply any pending migrations
            await dbContext.Database.MigrateAsync();

            // Seed demo user
            await SeedDemoUserAsync(userManager, dbContext);
        }

        public static async Task SeedDemoUserAsync(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            var demoUser = new AppUser
            {
                UserName = "demouser@contactpro.com",
                FirstName = "Demo",
                LastName = "User",
                Email = "demouser@contactpro.com",
                EmailConfirmed = true
            };

            try
            {
                var user = await userManager.FindByEmailAsync(demoUser.Email);

                if (user == null )
                {
                    await userManager.CreateAsync(demoUser, "Password1!");
                    await SeedDemoCategories(context, demoUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error seeding demo user");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**********************");

                throw;
            }
        }

        private static async Task SeedDemoCategories(ApplicationDbContext context, AppUser demoUser)
        {
            try
            {
                IList<Category> categories = new List<Category>()
                {
                    new() { Name = "Family",  AppUser = demoUser, AppUserId = demoUser.Id },
                    new() { Name = "Friends",  AppUser = demoUser, AppUserId = demoUser.Id },
                    new() { Name = "Work", AppUser = demoUser, AppUserId = demoUser.Id }
                };

                await context.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error seeding demo categories");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**********************");

                throw;
            }
        }
    }
}