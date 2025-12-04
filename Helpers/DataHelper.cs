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
                    await SeedDemoCategoriesAsync(context, demoUser);
                    await SeedDemoContactsAsync(context, demoUser);
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

        private static async Task SeedDemoCategoriesAsync(ApplicationDbContext context, AppUser demoUser)
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

        private static async Task SeedDemoContactsAsync(ApplicationDbContext context, AppUser demoUser)
        {
            try
            {
                var familyCategory = context.Categories.FirstOrDefault(c => c.Name == "Family" && c.AppUserId == demoUser.Id);
                IList<Contact> contacts = new List<Contact>()
                {
                    new()
                    {
                        Categories = new List<Category> { familyCategory },
                        FirstName = "Claudia",
                        LastName = "Black",
                        BirthDate = new DateTime(1972, 10, 11),
                        Address1 = "14 Crescent Road",
                        Address2 = "Flat 3",
                        City = "London",
                        PostCode = "NW3 5RT",
                        Email = "claudia.black@contactpro.com",
                        PhoneNumber = "07123456789",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id
                    }
                };

                await context.Contacts.AddRangeAsync(contacts);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error seeding demo contacts");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**********************");

                throw;
            }
        }
    }
}