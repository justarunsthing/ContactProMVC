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
            // await SeedDemoUserAsync(userManager, dbContext);
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

        public static async Task<AppUser> CreateDemoUserAsync(UserManager<AppUser> userManager, ApplicationDbContext context, string demoUserId)
        {
            var email = $"demouser-{demoUserId}@contactpro.com";
            var existing = await userManager.FindByEmailAsync(email);

            if (existing != null)
            {
                await DeleteDemoUserAsync(userManager, context, demoUserId);
            }

            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FirstName = "Demo",
                LastName = "User",
                EmailConfirmed = true,
                IsDemoUser = true
            };

            var result = await userManager.CreateAsync(user, "Password1!");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await SeedDemoCategoriesAsync(context, user);
            await SeedDemoContactsAsync(context, user);

            return user;
        }

        public static async Task DeleteDemoUserAsync(UserManager<AppUser> userManager, ApplicationDbContext context, string demoUserId)
        {
            var email = $"demouser-{demoUserId}@contactpro.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return;
            }

            var userId = user.Id;

            await context.Database.ExecuteSqlRawAsync($@"
                DELETE FROM ""CategoryContact""
                WHERE ""CategoriesId"" IN (
                    SELECT ""Id"" FROM ""Categories"" WHERE ""AppUserId"" = @p0
                )
                OR ""ContactsId"" IN (
                    SELECT ""Id"" FROM ""Contacts"" WHERE ""AppUserId"" = @p0
                );

                DELETE FROM ""Contacts"" WHERE ""AppUserId"" = @p0;
                DELETE FROM ""Categories"" WHERE ""AppUserId"" = @p0;
            ", userId);

            await userManager.DeleteAsync(user);
        }

        private static async Task SeedDemoCategoriesAsync(ApplicationDbContext context, AppUser demoUser)
        {
            try
            {
                IList<Category> categories =
                [
                    new() { Name = "Family",  AppUser = demoUser, AppUserId = demoUser.Id },
                    new() { Name = "Friends",  AppUser = demoUser, AppUserId = demoUser.Id },
                    new() { Name = "Work", AppUser = demoUser, AppUserId = demoUser.Id }
                ];

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

                IList<Contact> contacts =
                [
                    new()
                    {
                        Categories = [friendsCategory!],
                        FirstName = "Claudia",
                        LastName = "Black",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1993, 10, 11), DateTimeKind.Utc),
                        Address1 = "14 Crescent Road",
                        Address2 = "Flat 3",
                        City = "London",
                        PostCode = "NW3 5RT",
                        Email = "claudia.black@contactpro.com",
                        PhoneNumber = "07123456789",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "ClaudiaBlack.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [workCategory!, friendsCategory!],
                        FirstName = "Courtenay",
                        LastName = "Taylor",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1969, 7, 19), DateTimeKind.Utc),
                        Address1 = "88 Willowbank Terrace",
                        City = "Glasgow",
                        PostCode = "G3 7LH",
                        Email = "courtenay.taylor@contactpro.com",
                        PhoneNumber = "07988112233",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "CourtenayTaylor.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [familyCategory!],
                        FirstName = "Frank",
                        LastName = "Langella",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1984, 1, 1), DateTimeKind.Utc),
                        Address1 = "5 Kensington Court",
                        City = "London",
                        PostCode = "W8 5DL",
                        Email = "frank.langella@contactpro.com",
                        PhoneNumber = "07709887744",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "FrankLangella.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [friendsCategory!, workCategory!],
                        FirstName = "Gina",
                        LastName = "Torres",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1999, 4, 25), DateTimeKind.Utc),
                        Address1 = "22 Rosewood Close",
                        City = "Reading",
                        PostCode = "RG1 4JP",
                        Email = "gina.torres@contactpro.com",
                        PhoneNumber = "07855664422",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "GinaTorres.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [workCategory!],
                        FirstName = "Lance",
                        LastName = "Reddick",
                        BirthDate = DateTime.SpecifyKind(new DateTime(2002, 12, 31), DateTimeKind.Utc),
                        Address1 = "10 Kingfisher Way",
                        City = "Manchester",
                        PostCode = "M4 1FS",
                        Email = "lance.reddick@contactpro.com",
                        PhoneNumber = "07511993344",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "LanceReddick.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [friendsCategory!],
                        FirstName = "Moira",
                        LastName = "Quirk",
                        BirthDate = DateTime.SpecifyKind(new DateTime(2006, 10, 30), DateTimeKind.Utc),
                        Address1 = "31 Elmhurst Road",
                        Address2 = "Apartment 5B",
                        City = "Bristol",
                        PostCode = "BS8 2RJ",
                        Email = "moira.quirk@contactpro.com",
                        PhoneNumber = "07822554411",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "MoiraQuirk.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        FirstName = "Nathan",
                        LastName = "Fillion",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1991, 3, 27), DateTimeKind.Utc),
                        Address1 = "47 St Martin’s Lane",
                        City = "London",
                        PostCode = "WC2N 4HA",
                        Email = "nathan.fillion@contactpro.com",
                        PhoneNumber = "07234556677",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "NathanFillion.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [workCategory !],
                        FirstName = "Neil",
                        LastName = "Kaplan",
                        BirthDate = DateTime.SpecifyKind(new DateTime(2001, 3, 9), DateTimeKind.Utc),
                        Address1 = "6 Meadow View",
                        City = "York",
                        PostCode = "YO1 6HQ",
                        Email = "neil.kaplan@contactpro.com",
                        PhoneNumber = "07456778899",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "NeilKaplan.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        Categories = [friendsCategory !, workCategory !],
                        FirstName = "Nolan",
                        LastName = "North",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1996, 10, 31), DateTimeKind.Utc),
                        Address1 = "39 Highfield Road",
                        City = "Birmingham",
                        PostCode = "B15 3EB",
                        Email = "nolan.north@contactpro.com",
                        PhoneNumber = "07977441122",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "NolanNorth.png")),
                        ImageType = "image/png"
                    },
                    new()
                    {
                        FirstName = "Page",
                        LastName = "Leong",
                        BirthDate = DateTime.SpecifyKind(new DateTime(1980, 11, 30), DateTimeKind.Utc),
                        Address1 = "52 Cherry Blossom Way",
                        City = "Cambridge",
                        PostCode = "CB1 2HD",
                        Email = "page.leong@contactpro.com",
                        PhoneNumber = "07344228899",
                        Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        AppUser = demoUser,
                        AppUserId = demoUser.Id,
                        ImageData = LoadImageFile(Path.Combine("wwwroot", "img", "PageLeong.png")),
                        ImageType = "image/png"
                    }
                ];

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