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
                var workCategory = context.Categories.FirstOrDefault(c => c.Name == "Work" && c.AppUserId == demoUser.Id);
                var friendsCategory = context.Categories.FirstOrDefault(c => c.Name == "Friends" && c.AppUserId == demoUser.Id);

                IList<Contact> contacts = new List<Contact>()
                {
                    new()
                    {
                        Categories = new List<Category> { friendsCategory },
                        FirstName = "Claudia",
                        LastName = "Black",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1972, 10, 11), DateTimeKind.Utc),
                        Address1 = "14 Crescent Road",
                        Address2 = "Flat 3",
                        City = "London",
                        PostCode = "NW3 5RT",
                        Email = "claudia.black@contactpro.com",
                        PhoneNumber = "07123456789",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "ClaudiaBlack.png")),
                        ImageType = "image/png",
                        AppUser = demoUser,
                        AppUserId = demoUser.Id
                    },
                    new()
                    {
                        Categories = new List<Category> { workCategory },
                        FirstName = "Courtenay",
                        LastName = "Taylor",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1969, 07, 19), DateTimeKind.Utc),
                        Address1 = "88 Willowbank Terrace",
                        Address2 = "",
                        City = "Glasgow",
                        PostCode = "G3 7LH",
                        Email = "courtenay.taylor@contactpro.com",
                        PhoneNumber = "07988112233",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "CourtenayTaylor.png")),
                        ImageType = "image/png",
                        AppUser = demoUser,
                        AppUserId = demoUser.Id
                    },
                    new()
                    {
                        FirstName = "Frank",
                        LastName = "Langella",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1988, 01, 01), DateTimeKind.Utc),
                        Address1 = "5 Kensington Court",
                        Address2 = "",
                        City = "London",
                        PostCode = "W8 5FL",
                        Email = "frank.langella@contactpro.com",
                        PhoneNumber = "07709887744",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "FrankLangella.png")),
                        ImageType = "image/png",
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

        private static byte[] LoadImageFile(string path)
        {
            return File.Exists(path) ? File.ReadAllBytes(path) : Array.Empty<byte>();
        }
    }
}