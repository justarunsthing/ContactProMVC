using ContactProMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactProMVC.Helpers
{
    public static class DataHelper
    {
        public static async Task ManageDataAsync(IServiceProvider serviceProvider)
        {
            // Get an instance of ApplicationDbContext
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Apply any pending migrations
            await dbContext.Database.MigrateAsync();
        }
    }
}